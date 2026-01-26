using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Fachadas;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoManutencaoSolicitacaoAcervo(
        ContextoDadosAcervoSolicitacao dados,
        ContextoInfraAcervoSolicitacao infra,
        ContextoRegrasAcervoSolicitacao regras
    ) : IServicoManutencaoSolicitacaoAcervo
    {
        public async Task<long> Inserir(AcervoSolicitacaoManualDTO dto)
        {
            await ValidarSolicitacao(dto);

            var acervoSolicitacao = infra.Mapper.Map<AcervoSolicitacao>(dto);
            acervoSolicitacao.Origem = Origem.Manual;
            acervoSolicitacao.Situacao = CalcularSituacaoGeral(dto.Itens);

            var usuarioLogado = await infra.ServicoUsuario.ObterUsuarioLogado();

            using var tran = infra.Transacao.Iniciar();
            try
            {
                acervoSolicitacao.Id = await dados.RepositorioSolicitacao.Inserir(acervoSolicitacao);

                foreach (var item in acervoSolicitacao.Itens)
                {
                    item.AcervoSolicitacaoId = acervoSolicitacao.Id;
                    item.ResponsavelId = usuarioLogado.Id;

                    var itemDto = dto.Itens.First(f => f.AcervoId == item.AcervoId);

                    await PersistirItem(item, itemDto);
                }

                tran.Commit();
                return acervoSolicitacao.Id;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public async Task<long> Alterar(AcervoSolicitacaoManualDTO dto)
        {
            await ValidarSolicitacao(dto);

            var acervoSolicitacao = await dados.RepositorioSolicitacao.ObterPorId(dto.Id);
            if (acervoSolicitacao.EhNulo())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_ENCONTRADA);

            // Prepara objetos para atualização
            acervoSolicitacao.Origem = Origem.Manual;
            acervoSolicitacao.DataSolicitacao = dto.DataSolicitacao;
            acervoSolicitacao.Situacao = CalcularSituacaoGeral(dto.Itens);

            var itensAtuais = await dados.RepositorioItem.ObterItensPorSolicitacaoId(acervoSolicitacao.Id);
            var usuarioLogado = await infra.ServicoUsuario.ObterUsuarioLogado();

            using var tran = infra.Transacao.Iniciar();
            try
            {
                await dados.RepositorioSolicitacao.Atualizar(acervoSolicitacao);

                foreach (var itemDto in dto.Itens)
                {
                    var itemEntidade = infra.Mapper.Map<AcervoSolicitacaoItem>(itemDto);
                    itemEntidade.AcervoSolicitacaoId = acervoSolicitacao.Id;
                    itemEntidade.ResponsavelId = usuarioLogado.Id;

                    // Verifica se é atualização de item existente
                    var itemExistente = itemDto.Id.HasValue && itemDto.Id.Value > 0
                        ? itensAtuais.FirstOrDefault(f => f.Id == itemDto.Id.Value)
                        : null;

                    await PersistirItem(itemEntidade, itemDto, itemExistente);
                }

                tran.Commit();
                return acervoSolicitacao.Id;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        private async Task PersistirItem(AcervoSolicitacaoItem item, AcervoSolicitacaoItemManualDTO itemDto, AcervoSolicitacaoItem? itemExistente = null)
        {
            var temEmprestimo = itemDto.DataEmprestimo.HasValue && itemDto.DataDevolucao.HasValue;
            var temDataVisita = item.DataVisita.HasValue;

            // Definição de Estado
            item.Situacao = CalcularSituacaoItem(item.TipoAtendimento, temEmprestimo, temDataVisita);

            // 1. Persistência do Registro Base
            await SalvarOuAtualizarItemBase(item, itemExistente);

            // 2. Regras de Negócio (Limpeza de estado anterior)
            await ProcessarLimpezaDeEstadoAnterior(item, itemExistente);

            // 3. Regras de Negócio (Aplicação de novo estado)
            if (item.TipoAtendimento == TipoAtendimento.Presencial)
            {
                await GerenciarAgendaVisita(item, itemExistente);

                if (itemDto.TipoAcervo == TipoAcervo.Bibliografico)
                {
                    await GerenciarEstoqueEEmprestimo(item, itemDto, temEmprestimo);
                }
            }
        }

        private async Task SalvarOuAtualizarItemBase(AcervoSolicitacaoItem item, AcervoSolicitacaoItem? itemExistente)
        {
            if (itemExistente != null)
            {
                // Copia dados para a entidade rastreada
                itemExistente.TipoAtendimento = item.TipoAtendimento;
                itemExistente.DataVisita = item.DataVisita;
                itemExistente.Situacao = item.Situacao;
                itemExistente.ResponsavelId = item.ResponsavelId;

                await dados.RepositorioItem.Atualizar(itemExistente);
                item.Id = itemExistente.Id; // Garante ID atualizado no objeto de escopo
            }
            else
            {
                await dados.RepositorioItem.Inserir(item);
            }
        }

        private async Task ProcessarLimpezaDeEstadoAnterior(AcervoSolicitacaoItem item, AcervoSolicitacaoItem? itemExistente)
        {
            if (itemExistente == null) return;

            var eraPresencial = itemExistente.TipoAtendimento == TipoAtendimento.Presencial;
            var virouEmail = item.TipoAtendimento == TipoAtendimento.Email;
            var removeuDataVisita = itemExistente.DataVisita.HasValue && !item.DataVisita.HasValue;

            // Se mudou de Presencial para Email, limpa resquícios de agendamento e reserva
            if (eraPresencial && virouEmail)
            {
                await regras.ServicoEvento.ExcluirEventoPorAcervoSolicitacaoItem(item.Id);
                await regras.ServicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.DISPONIVEL, item.AcervoId);
            }
            // Se removeu a data da visita, exclui o evento agendado
            else if (eraPresencial && removeuDataVisita)
            {
                await regras.ServicoEvento.ExcluirEventoPorAcervoSolicitacaoItem(item.Id);
            }
        }

        private async Task GerenciarAgendaVisita(AcervoSolicitacaoItem item, AcervoSolicitacaoItem? itemExistente)
        {
            if (!item.DataVisita.HasValue) return;

            // Lógica: Se é novo ou não tinha data antes -> Inserir. Senão -> Atualizar.
            var deveInserir = itemExistente == null || !itemExistente.DataVisita.HasValue;

            if (deveInserir)
            {
                await regras.ServicoEvento.InserirEventoVisita(item.DataVisita.Value, item.Id);
            }
            else
            {
                await regras.ServicoEvento.AtualizarEventoVisita(item.DataVisita.Value, item.Id);
            }
        }

        private async Task GerenciarEstoqueEEmprestimo(AcervoSolicitacaoItem item, AcervoSolicitacaoItemManualDTO itemDto, bool temEmprestimo)
        {
            if (temEmprestimo)
            {
                await regras.ServicoAcervoBibliografico.AtualizarOuCriarEmprestimoAsync(
                    item.Id,
                    item.AcervoId,
                    itemDto.DataEmprestimo!.Value,
                    itemDto.DataDevolucao!.Value
                );
            }
            else
            {
                // Se removeu as datas, volta para RESERVADO (fluxo padrão sem empréstimo efetivado)
                await regras.ServicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.RESERVADO, item.AcervoId);
            }
        }

        // Helpers de Lógica Pura (Domain Logic)

        private static SituacaoSolicitacao CalcularSituacaoGeral(IEnumerable<AcervoSolicitacaoItemManualDTO> itens)
        {
            var pendenteVisita = itens.Any(a => a.TipoAtendimento.EhAtendimentoPresencial()
                                                && (!a.DataEmprestimo.HasValue || !a.DataDevolucao.HasValue));

            return pendenteVisita ? SituacaoSolicitacao.AGUARDANDO_VISITA : SituacaoSolicitacao.FINALIZADO_ATENDIMENTO;
        }

        private static SituacaoSolicitacaoItem CalcularSituacaoItem(TipoAtendimento? tipo, bool temEmprestimo, bool temDataVisita)
        {
            if (tipo == TipoAtendimento.Presencial)
            {
                if (temDataVisita)
                {
                    return temEmprestimo
                        ? SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                        : SituacaoSolicitacaoItem.AGUARDANDO_VISITA;
                }
                return SituacaoSolicitacaoItem.PRESENCIAL_ABERTO;
            }

            return SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE;
        }

        private async Task ValidarSolicitacao(AcervoSolicitacaoManualDTO dto)
        {
            var usuario = await infra.ServicoUsuario.ObterPorId(dto.UsuarioId);
            if (usuario.EhNulo())
                throw new NegocioException(MensagemNegocio.USUARIO_NAO_ENCONTRADO);

            ValidarRegrasDeDatas(dto);

            var datasDasVisitas = dto.Itens
                .Where(w => w.TipoAtendimento == TipoAtendimento.Presencial && w.DataVisita.HasValue)
                .Select(s => s.DataVisita!.Value);

            await regras.ServicoEvento.ValidarConflitosAsync(datasDasVisitas);
        }

        private static void ValidarRegrasDeDatas(AcervoSolicitacaoManualDTO dto)
        {
            if (dto.Itens.Any(a => a.TipoAtendimento.EhInvalido()))
                throw new NegocioException(MensagemNegocio.TIPO_ATENDIMENTO_INVALIDO);

            if (dto.Itens.Any(a => a.TipoAtendimento.EhAtendimentoViaEmail() && a.DataVisita.HasValue))
                throw new NegocioException(MensagemNegocio.ITENS_ACERVOS_EMAIL_NAO_DEVEM_TER_DATA_ACERVO);

            // Validações Bibliográficas
            var itensBiblio = dto.Itens.Where(a => a.TipoAcervo.EhAcervoBibliografico());
            var hoje = DateTimeExtension.HorarioBrasilia().Date;

            if (itensBiblio.Any(a => a.DataEmprestimo.HasValue && a.DataEmprestimo.Value.Date > hoje))
                throw new NegocioException(MensagemNegocio.DATA_DO_EMPRESTIMO_NAO_PODE_SER_FUTURA);

            if (itensBiblio.Any(a => a.DataEmprestimo.HasValue && a.DataVisita.HasValue && a.DataEmprestimo.Value.Date < a.DataVisita.Value.Date))
                throw new NegocioException(MensagemNegocio.DATA_DO_EMPRESTIMO_MENOR_QUE_DATA_VISITA);

            if (itensBiblio.Any(a => a.DataDevolucao.HasValue && a.DataEmprestimo.HasValue && a.DataDevolucao.Value.Date < a.DataEmprestimo.Value.Date))
                throw new NegocioException(MensagemNegocio.DATA_DA_DEVOLUCAO_MENOR_DATA_DO_EMPRESTIMO);

            // Validações Não Bibliográficas
            if (dto.Itens.Any(a => !a.TipoAcervo.EhAcervoBibliografico() && (a.DataEmprestimo.HasValue || a.DataDevolucao.HasValue)))
                throw new NegocioException(MensagemNegocio.DATA_DO_EMPRESTIMO_E_DEVOLUCAO_EXCLUSIVO_PARA_ACERVOS_BIBLIOGRAFICOS);
        }
    }
}
