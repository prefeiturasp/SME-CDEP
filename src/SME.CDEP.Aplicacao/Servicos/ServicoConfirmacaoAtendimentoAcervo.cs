using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Fachadas;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoConfirmacaoAtendimentoAcervo(
        ConfirmacaoAtendimentoRecursos recursos,
        IServicoEvento servicoEvento,
        IServicoAcervoBibliografico servicoAcervoBibliografico,
        IServicoProcessamentoSituacaoSolicitacao servicoProcessamentoSituacao) :
        IServicoConfirmacaoAtendimentoAcervo
    {
        public async Task<bool> Executar(AcervoSolicitacaoConfirmarDto dto)
        {
            ValidarRegrasIniciais(dto);

            var acervoSolicitacao = await recursos.RepositorioSolicitacao.ObterPorId(dto.Id) ??
                                    throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_ENCONTRADA);
            if (dto.TipoAtendimento == TipoAtendimento.Presencial && dto.DataVisita is not null)
                await servicoEvento.ValidarConflitosAsync([dto.DataVisita.Value]);

            var itemSolicitacao = await ObterItemSolicitacao(dto);
            var usuarioLogado = await recursos.ServicoUsuario.ObterUsuarioLogado();

            using var tran = recursos.Transacao.Iniciar();
            try
            {
                var eraPresencial = itemSolicitacao.TipoAtendimento == TipoAtendimento.Presencial;
                var tinhaDataVisita = itemSolicitacao.DataVisita.HasValue;

                AtualizarDadosItem(itemSolicitacao, dto, usuarioLogado.Id);
                await recursos.RepositorioItem.Atualizar(itemSolicitacao);

                await ProcessarLogicaDeEventosEEstoque(itemSolicitacao, dto, eraPresencial, tinhaDataVisita);

                tran.Commit();

                await servicoProcessamentoSituacao.AtualizarSituacaoGeralSolicitacaoAsync(acervoSolicitacao);
                await NotificarSeNecessario(acervoSolicitacao.Id, itemSolicitacao);

                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        private async Task<AcervoSolicitacaoItem> ObterItemSolicitacao(AcervoSolicitacaoConfirmarDto dto)
        {
            var itens = await recursos.RepositorioItem
                .ObterItensVigentesPorSolicitacaoIdAsync(dto.Id);

            return itens.FirstOrDefault(w => w.Id == dto.ItemId)
                   ?? throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_ITEM_NAO_ENCONTRADA);
        }

        private static void AtualizarDadosItem(AcervoSolicitacaoItem item, AcervoSolicitacaoConfirmarDto dto, long usuarioId)
        {
            var possuiInformacoesEmprestimo = dto.DataEmprestimo.HasValue && dto.DataDevolucao.HasValue;

            item.TipoAtendimento = dto.TipoAtendimento;
            item.ResponsavelId = usuarioId;

            if (dto.DataVisita.HasValue && dto.TipoAtendimento == TipoAtendimento.Presencial)
            {
                item.DataVisita = dto.DataVisita;
                item.Situacao = possuiInformacoesEmprestimo
                    ? SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                    : SituacaoSolicitacaoItem.AGUARDANDO_VISITA;
            }
            else if (dto.TipoAtendimento == TipoAtendimento.Presencial)
            {
                item.DataVisita = null;
                item.Situacao = SituacaoSolicitacaoItem.PRESENCIAL_ABERTO;
            }
            else
            {
                item.Situacao = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE;
                item.DataVisita = null;
            }
        }

        private async Task ProcessarLogicaDeEventosEEstoque(AcervoSolicitacaoItem item, AcervoSolicitacaoConfirmarDto dto, bool eraPresencial, bool tinhaDataVisita)
        {
            var virouEmail = item.TipoAtendimento == TipoAtendimento.Email;
            var removeuDataVisita = tinhaDataVisita && item.DataVisita is null;
            if (eraPresencial && (virouEmail || removeuDataVisita))
            {
                await servicoEvento.ExcluirEventoPorAcervoSolicitacaoItem(item.Id);

                if (dto.TipoAcervo == TipoAcervo.Bibliografico)
                    await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.DISPONIVEL, item.AcervoId);
            }

            if (item.TipoAtendimento == TipoAtendimento.Presencial)
            {
                if (item.DataVisita is not null)
                    await servicoEvento.AtualizarEventoVisita(item.DataVisita!.Value, item.Id);

                if (dto.TipoAcervo == TipoAcervo.Bibliografico)
                    await servicoAcervoBibliografico.GerenciarEmprestimoAsync(
                            item.Id,
                            item.AcervoId,
                            dto.DataEmprestimo,
                            dto.DataDevolucao
                        );
            }
        }

        private async Task NotificarSeNecessario(long solicitacaoId, AcervoSolicitacaoItem item)
        {
            if (item.TipoAtendimento == TipoAtendimento.Presencial)
            {
                var confirmarAtendimento = new ConfirmarAtendimentoDTO
                {
                    Id = solicitacaoId,
                    ItemId = item.Id
                };
                await recursos.ServicoMensageria.Publicar(RotasRabbit.NotificarViaEmailConfirmacaoAtendimentoPresencial, confirmarAtendimento, null);
            }
        }

        private static void ValidarRegrasIniciais(AcervoSolicitacaoConfirmarDto dto)
        {
            if (dto.ItemId <= 0)
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_ITEM_NAO_ENCONTRADA);

            if (dto.TipoAtendimento == TipoAtendimento.Email && dto.DataVisita.HasValue)
                throw new NegocioException(MensagemNegocio.ITENS_ACERVOS_EMAIL_NAO_DEVEM_TER_DATA_ACERVO);

            ValidarRegrasDeEmprestimo(dto);
        }

        private static void ValidarRegrasDeEmprestimo(AcervoSolicitacaoConfirmarDto dto)
        {
            var temEmprestimo = dto.DataEmprestimo.HasValue;
            var temDevolucao = dto.DataDevolucao.HasValue;

            if (dto.TipoAcervo != TipoAcervo.Bibliografico)
            {
                if (temEmprestimo && temDevolucao)
                    throw new NegocioException(MensagemNegocio.DATA_DO_EMPRESTIMO_E_DEVOLUCAO_EXCLUSIVO_PARA_ACERVOS_BIBLIOGRAFICOS);
                return;
            }

            // Lógica para Acervo Bibliográfico
            if (temEmprestimo != temDevolucao)
                throw new NegocioException(MensagemNegocio.DATA_DO_EMPRESTIMO_E_OU_DA_DEVOLUCAO_INVALIDOS);

            if (!temEmprestimo)
                return;

            if (dto.DataEmprestimo.EhDataFutura())
                throw new NegocioException(MensagemNegocio.DATA_DO_EMPRESTIMO_NAO_PODE_SER_FUTURA);

            if (dto.DataEmprestimo.EhMenorQue(dto.DataVisita))
                throw new NegocioException(MensagemNegocio.DATA_DO_EMPRESTIMO_MENOR_QUE_DATA_VISITA);

            if (dto.DataDevolucao.EhMenorIgualQue(dto.DataEmprestimo))
                throw new NegocioException(MensagemNegocio.DATA_DA_DEVOLUCAO_MENOR_DATA_DO_EMPRESTIMO);

            if (dto.DataVisita.EhDataFutura())
                throw new NegocioException(MensagemNegocio.DATA_DA_DEVOLUCAO_E_DATA_FUTURA_EM_VISITA_FUTURA);
        }
    }
}