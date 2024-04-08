using System.ComponentModel.DataAnnotations;
using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Extensions;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcervoSolicitacao : IServicoAcervoSolicitacao
    {
        private readonly IRepositorioAcervoSolicitacao repositorioAcervoSolicitacao;
        private readonly IRepositorioAcervoSolicitacaoItem repositorioAcervoSolicitacaoItem;
        private readonly IMapper mapper;
        private readonly ITransacao transacao;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IRepositorioAcervo repositorioAcervo;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IContextoAplicacao contextoAplicacao;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IServicoEvento servicoEvento;
        private readonly IServicoMensageria servicoMensageria;
        private readonly IRepositorioAcervoEmprestimo repositorioAcervoEmprestimo;
        private readonly IRepositorioParametroSistema repositorioParametroSistema;
        private readonly IServicoAcervo servicoAcervo;
        private readonly IServicoAcervoBibliografico servicoAcervoBibliografico;
        
        public ServicoAcervoSolicitacao(IRepositorioAcervoSolicitacao repositorioAcervoSolicitacao, 
            IMapper mapper,ITransacao transacao,IRepositorioAcervoSolicitacaoItem repositorioAcervoSolicitacaoItem,
            IRepositorioUsuario repositorioUsuario,IRepositorioAcervo repositorioAcervo,
            IServicoUsuario servicoUsuario,IContextoAplicacao contextoAplicacao,
            IRepositorioEvento repositorioEvento, IServicoEvento servicoEvento,
            IServicoMensageria servicoMensageria,IRepositorioAcervoEmprestimo repositorioAcervoEmprestimo,
            IRepositorioParametroSistema repositorioParametroSistema,
            IServicoAcervo servicoAcervo, IServicoAcervoBibliografico servicoAcervoBibliografico) 
        {
            this.repositorioAcervoSolicitacao = repositorioAcervoSolicitacao ?? throw new ArgumentNullException(nameof(repositorioAcervoSolicitacao));
            this.repositorioAcervoSolicitacaoItem = repositorioAcervoSolicitacaoItem ?? throw new ArgumentNullException(nameof(repositorioAcervoSolicitacaoItem));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.transacao = transacao ?? throw new ArgumentNullException(nameof(transacao));
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.repositorioAcervo = repositorioAcervo ?? throw new ArgumentNullException(nameof(repositorioAcervo));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.servicoEvento = servicoEvento ?? throw new ArgumentNullException(nameof(servicoEvento));
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
            this.repositorioAcervoEmprestimo = repositorioAcervoEmprestimo ?? throw new ArgumentNullException(nameof(repositorioAcervoEmprestimo));
            this.repositorioParametroSistema = repositorioParametroSistema ?? throw new ArgumentNullException(nameof(repositorioParametroSistema));
            this.servicoAcervo = servicoAcervo ?? throw new ArgumentNullException(nameof(servicoAcervo));
            this.servicoAcervoBibliografico = servicoAcervoBibliografico ?? throw new ArgumentNullException(nameof(servicoAcervoBibliografico));
        }

        public async Task<long> Inserir(AcervoSolicitacaoItemCadastroDTO[] acervosSolicitacaoItensCadastroDTO)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            
            var usuarioSolicitante = await repositorioUsuario.ObterPorId(usuarioLogado.Id);
            if (usuarioSolicitante.EhNulo())
                throw new NegocioException(MensagemNegocio.USUARIO_NAO_ENCONTRADO);

            var arquivosEncontrados = await repositorioAcervo.ObterArquivosPorAcervoId(acervosSolicitacaoItensCadastroDTO.Select(s=> s.AcervoId).ToArray());
            
            var tran = transacao.Iniciar();
            try
            {
                var acervoSolicitacao = new AcervoSolicitacao()
                {
                    UsuarioId = usuarioLogado.Id,
                    DataSolicitacao = DateTimeExtension.HorarioBrasilia().Date,
                    Situacao = acervosSolicitacaoItensCadastroDTO
                        .Select(s => s.AcervoId)
                        .Except(arquivosEncontrados.Select(s => s.AcervoId))
                        .Any() ? SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO : SituacaoSolicitacao.FINALIZADO_ATENDIMENTO
                };
                
                acervoSolicitacao.Id =  await repositorioAcervoSolicitacao.Inserir(acervoSolicitacao);

                foreach (var item in acervosSolicitacaoItensCadastroDTO)
                {
                    var acervoSolicitacaoItem = mapper.Map<AcervoSolicitacaoItem>(item);
                    
                    acervoSolicitacaoItem.AcervoSolicitacaoId = acervoSolicitacao.Id;
                    
                    acervoSolicitacaoItem.Situacao = arquivosEncontrados.Any(a => a.AcervoId == item.AcervoId)
                        ? SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE
                        : SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO;
                    
                    await repositorioAcervoSolicitacaoItem.Inserir(acervoSolicitacaoItem);
                    
                    await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.RESERVADO,item.AcervoId);
                }
                tran.Commit();

                return acervoSolicitacao.Id;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                tran.Dispose();
            }
        }

        public async Task<bool> Remover(long acervoSolicitacaoId)
        {
            await repositorioAcervoSolicitacao.Remover(acervoSolicitacaoId);
            return true;
        }

        public async Task<IEnumerable<AcervoTipoTituloAcervoIdCreditosAutoresDTO>> ObterItensAcervoPorAcervosIds(long[] acervosIds)
        {
            var acervos = await repositorioAcervoSolicitacao.ObterItensDoAcervoPorAcervosIds(acervosIds);

            if (acervos.EhNulo())
                throw new NegocioException(MensagemNegocio.ACERVO_NAO_ENCONTRADO);
            
            return mapper.Map<IEnumerable<AcervoTipoTituloAcervoIdCreditosAutoresDTO>>(acervos);
        }
        
        public async Task<AcervoSolicitacaoRetornoCadastroDTO> ObterPorId(long acervoSolicitacaoId)
        {
            var tiposAcervosPermitidos = servicoAcervo.ObterTiposAcervosPermitidosDoPerfilLogado();

            return await ObterAcervosSolicitacoesPorIdTiposPermitidos(acervoSolicitacaoId, tiposAcervosPermitidos);
        }

        private async Task<AcervoSolicitacaoRetornoCadastroDTO> ObterAcervosSolicitacoesPorIdTiposPermitidos(long acervoSolicitacaoId, long[] tiposAcervosPermitidos)
        {
            var acervosItensCompletos = await repositorioAcervo.ObterAcervosSolicitacoesItensCompletoPorId(acervoSolicitacaoId, tiposAcervosPermitidos);

            var acervoItensRetorno = mapper.Map<IEnumerable<AcervoSolicitacaoItemRetornoCadastroDTO>>(acervosItensCompletos);

            var arquivosDoAcervo = await repositorioAcervo.ObterArquivosPorAcervoId(acervosItensCompletos.Select(s => s.AcervoId).ToArray());

            foreach (var retorno in acervoItensRetorno)
                retorno.Arquivos = mapper.Map<IEnumerable<ArquivoCodigoNomeDTO>>(arquivosDoAcervo.Where(w => w.AcervoId == retorno.AcervoId).Select(s=> s));

            return new AcervoSolicitacaoRetornoCadastroDTO()
            {
                PodeCancelarSolicitacao = acervosItensCompletos.Any(a=> a.SituacaoItem.PodeCancelarAtendimento()),
                Itens = acervoItensRetorno
            };
        }

        public async Task<AcervoSolicitacaoRetornoCadastroDTO> ObterMinhaSolicitacaoPorId(long acervoSolicitacaoId)
        {
            var tiposAcervosPermitidos = Enum.GetValues(typeof(TipoAcervo))
                .Cast<TipoAcervo>()
                .Select(v => (long)v).ToArray();
                
            return await ObterAcervosSolicitacoesPorIdTiposPermitidos(acervoSolicitacaoId, tiposAcervosPermitidos);
        }
        
        public async Task<bool> Excluir(long acervoSolicitacaoId)
        {
            await repositorioAcervoSolicitacao.Excluir(acervoSolicitacaoId);
            return true;
        }

        public async Task<PaginacaoResultadoDTO<MinhaSolicitacaoDTO>> ObterMinhasSolicitacoes()
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();

            var acervoSolicitacaoItemsDTOs = mapper.Map<IEnumerable<MinhaSolicitacaoDTO>>(await repositorioAcervoSolicitacaoItem.ObterMinhasSolicitacoes(usuario.Id));
            
            var totalRegistros = acervoSolicitacaoItemsDTOs.Count();
            var paginacao = Paginacao;
            
            return new PaginacaoResultadoDTO<MinhaSolicitacaoDTO>()
            {
                Items = acervoSolicitacaoItemsDTOs.Skip(paginacao.QuantidadeRegistrosIgnorados).Take(paginacao.QuantidadeRegistros),
                TotalRegistros = totalRegistros,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacao.QuantidadeRegistros)
            };
        }

        public Paginacao Paginacao
        {
            get
            {
                var numeroPaginaQueryString = contextoAplicacao.ObterVariavel<string>("NumeroPagina");
                var numeroRegistrosQueryString = contextoAplicacao.ObterVariavel<string>("NumeroRegistros");
                var ordenacaoQueryString = contextoAplicacao.ObterVariavel<string>("Ordenacao");

                if (numeroPaginaQueryString.NaoEstaPreenchido() || numeroRegistrosQueryString.NaoEstaPreenchido()|| ordenacaoQueryString.NaoEstaPreenchido())
                    return new Paginacao(0, 0,0);

                var numeroPagina = numeroPaginaQueryString.ConverterParaInteiro();
                var numeroRegistros = numeroRegistrosQueryString.ConverterParaInteiro();
                var ordenacao = ordenacaoQueryString.ConverterParaInteiro();

                return new Paginacao(numeroPagina, numeroRegistros == 0 ? 10 : numeroRegistros,ordenacao);
            }
        }

        public Task<IEnumerable<SituacaoItemDTO>> ObterSituacoesAtendimentosItem()
        {
            var lista = Enum.GetValues(typeof(SituacaoSolicitacaoItem))
                .Cast<SituacaoSolicitacaoItem>()
                .OrderBy(O=> O)
                .Select(v => new SituacaoItemDTO
                {
                    Id = (short)v,
                    Nome = v.Descricao()
                });

            return Task.FromResult(lista);
        }

        public async Task<PaginacaoResultadoDTO<SolicitacaoDTO>> ObterAtendimentoSolicitacoesPorFiltro(FiltroSolicitacaoDTO filtroSolicitacaoDto)
        {
            var tiposAcervosPermitidos = servicoAcervo.ObterTiposAcervosPermitidosDoPerfilLogado();
            
            var solicitacoes = mapper.Map<IEnumerable<SolicitacaoDTO>>(await repositorioAcervoSolicitacaoItem
                .ObterSolicitacoesPorFiltro(filtroSolicitacaoDto.AcervoSolicitacaoId, filtroSolicitacaoDto.TipoAcervo, 
                    filtroSolicitacaoDto.DataSolicitacaoInicio, filtroSolicitacaoDto.DataSolicitacaoFim,filtroSolicitacaoDto.Responsavel, filtroSolicitacaoDto.SituacaoItem, 
                    filtroSolicitacaoDto.DataVisitaInicio, filtroSolicitacaoDto.DataVisitaFim,filtroSolicitacaoDto.SolicitanteRf,filtroSolicitacaoDto.SituacaoEmprestimo,tiposAcervosPermitidos));

            var totalRegistros = solicitacoes.Count();
            var paginacao = Paginacao;
            
            return new PaginacaoResultadoDTO<SolicitacaoDTO>()
            {
                Items = solicitacoes.Skip(paginacao.QuantidadeRegistrosIgnorados).Take(paginacao.QuantidadeRegistros),
                TotalRegistros = totalRegistros,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacao.QuantidadeRegistros)
            };
        }

        public async Task<AcervoSolicitacaoDetalheDTO> ObterDetalhesParaAtendimentoSolicitadoesPorId(long acervoSolicitacaoId)
        {
            var perfilLogado = new Guid(contextoAplicacao.PerfilUsuario);
            
            var tiposAcervosPermitidos = servicoAcervo.ObterTiposAcervosPermitidosDoPerfilLogado();
            
            var acervoSolicitacao = mapper.Map<AcervoSolicitacaoDetalheDTO>(await repositorioAcervoSolicitacao.ObterDetalhesPorIdTiposPermitidos(acervoSolicitacaoId, tiposAcervosPermitidos));

            if (acervoSolicitacao.EhNulo())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_ENCONTRADA);

            acervoSolicitacao.DadosSolicitante = mapper.Map<DadosSolicitanteDTO>(await servicoUsuario.ObterDadosSolicitantePorUsuarioId(acervoSolicitacao.UsuarioId));
            acervoSolicitacao.PodeFinalizar = perfilLogado.EhPerfilAdminGeral() && acervoSolicitacao.SituacaoId.NaoEstaFinalizadoAtendimentoOuCancelado()
                                              && !acervoSolicitacao.Itens.Any(a =>
                                                  a.SituacaoId.EstaAguardandoAtendimento()
                                                  || (a.SituacaoId.EstaAguardandoVisita() && a.DataVisita.HasValue && a.DataVisita.EhDataFutura()));
            
            acervoSolicitacao.PodeCancelar = perfilLogado.EhPerfilAdminGeral() && acervoSolicitacao.SituacaoId.NaoEstaFinalizadoAtendimentoOuCancelado()
                                             && !acervoSolicitacao.Itens.Any(a=> 
                                                 a.SituacaoId == SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE 
                                                 || a.SituacaoId == SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE);

            if (acervoSolicitacao.Itens.Any(a=> a.TipoAcervoId.EhAcervoBibliografico()))
            {
                var limiteDiasEmprestimoAcervo = await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.LimiteDiasEmprestimoAcervo,DateTimeExtension.HorarioBrasilia().Year);
                acervoSolicitacao.LimiteDiasEmprestimoAcervo = int.Parse(limiteDiasEmprestimoAcervo.Valor);
            }

            return acervoSolicitacao;
        }

        public IEnumerable<IdNomeDTO> ObterTiposDeAtendimentos()
        {
            return Enum.GetValues(typeof(TipoAtendimento))
                    .Cast<TipoAtendimento>()
                    .Select(v => new IdNomeDTO
                    {
                        Id = (int)v,
                        Nome = v.ObterAtributo<DisplayAttribute>().Description,
                    });
        }
        
        public async Task<bool> ConfirmarAtendimento(AcervoSolicitacaoConfirmarDTO acervoSolicitacaoConfirmar)
        {
            ValidacoesParaConfirmarAtendimentoPortal(acervoSolicitacaoConfirmar);

            var acervoSolicitacao = await repositorioAcervoSolicitacao.ObterPorId(acervoSolicitacaoConfirmar.Id);
            
            if (acervoSolicitacao.EhNulo())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_ENCONTRADA);
            
            var itensDaSolicitacaoAtual = await repositorioAcervoSolicitacaoItem
                .ObterItensEmSituacaoAguardandoAtendimentoOuVisitaOuFinalizadoManualmentePorSolicitacaoId(acervoSolicitacaoConfirmar.Id);

            if (acervoSolicitacaoConfirmar.TipoAtendimento.EhAtendimentoPresencial() )
                await ValidarConflitosEventos(new List<DateTime>() { acervoSolicitacaoConfirmar.DataVisita.Value });
            
            var itemDaSolicitacaoAtual = itensDaSolicitacaoAtual.FirstOrDefault(w => w.Id == acervoSolicitacaoConfirmar.ItemId);
            
            var tran = transacao.Iniciar();
            try
            {
                var eraPresencial = itemDaSolicitacaoAtual.TipoAtendimento.EhAtendimentoPresencial();
                
                var possuiInformacoesEmprestimo = acervoSolicitacaoConfirmar.DataEmprestimo.HasValue && acervoSolicitacaoConfirmar.DataDevolucao.HasValue;

                itemDaSolicitacaoAtual.TipoAtendimento = acervoSolicitacaoConfirmar.TipoAtendimento;
                
                if (acervoSolicitacaoConfirmar.DataVisita.HasValue && acervoSolicitacaoConfirmar.TipoAtendimento.EhAtendimentoPresencial())
                {
                    itemDaSolicitacaoAtual.DataVisita = acervoSolicitacaoConfirmar.DataVisita;
                    itemDaSolicitacaoAtual.Situacao =   possuiInformacoesEmprestimo ? SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE : SituacaoSolicitacaoItem.AGUARDANDO_VISITA;
                }
                else
                {
                    itemDaSolicitacaoAtual.Situacao = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE;
                    itemDaSolicitacaoAtual.DataVisita = null;
                }
                
                itemDaSolicitacaoAtual.ResponsavelId = (await servicoUsuario.ObterUsuarioLogado()).Id;
                
                await repositorioAcervoSolicitacaoItem.Atualizar(itemDaSolicitacaoAtual);

                if (eraPresencial && itemDaSolicitacaoAtual.TipoAtendimento.EhAtendimentoViaEmail())
                {
                    await servicoEvento.ExcluirEventoPorAcervoSolicitacaoItem(itemDaSolicitacaoAtual.Id);
                    
                    if (acervoSolicitacaoConfirmar.TipoAcervo.EhAcervoBibliografico())
                        await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.DISPONIVEL,itemDaSolicitacaoAtual.AcervoId);
                }

                var temInformacaoEmprestimo = acervoSolicitacaoConfirmar.DataEmprestimo.HasValue 
                                               && acervoSolicitacaoConfirmar.DataDevolucao.HasValue;

                if (itemDaSolicitacaoAtual.TipoAtendimento.EhAtendimentoPresencial())
                {
                    await servicoEvento.AtualizarEventoVisita(itemDaSolicitacaoAtual.DataVisita.Value, itemDaSolicitacaoAtual.Id);
                
                    if (acervoSolicitacaoConfirmar.TipoAcervo.EhAcervoBibliografico())
                    {
                        var itemEmprestado = await repositorioAcervoEmprestimo.ObterUltimoEmprestimoPorAcervoSolicitacaoItemId(acervoSolicitacaoConfirmar.ItemId);

                        if (itemEmprestado.NaoEhNulo())
                            throw new NegocioException(MensagemNegocio.VOCE_NAO_PODE_ALTERAR_EMPRESTIMOS_ACERVOS);
                        
                        if (temInformacaoEmprestimo)
                        {
                            await InserirAcervoEmprestimo(itemDaSolicitacaoAtual.Id, acervoSolicitacaoConfirmar.DataEmprestimo.Value, acervoSolicitacaoConfirmar.DataDevolucao.Value, SituacaoEmprestimo.EMPRESTADO);
                            await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.EMPRESTADO,itemDaSolicitacaoAtual.AcervoId);
                        }
                        else
                            await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.RESERVADO,itemDaSolicitacaoAtual.AcervoId);
                    }
                }
                
                tran.Commit();
                
                await AtualizarSituacaoAtendimento(acervoSolicitacao);

                if (itemDaSolicitacaoAtual.TipoAtendimento.EhAtendimentoPresencial())
                {
                    var confirmarAtendimento = new ConfirmarAtendimentoDTO()
                    {
                        Id = acervoSolicitacao.Id,
                        ItemId = itemDaSolicitacaoAtual.Id
                    };
                    await servicoMensageria.Publicar(RotasRabbit.NotificarViaEmailConfirmacaoAtendimentoPresencial, confirmarAtendimento, null);
                }
                
                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                tran.Dispose();
            }
        }

        private static void ValidacoesParaConfirmarAtendimentoPortal(AcervoSolicitacaoConfirmarDTO acervoSolicitacaoConfirmar)
        {
            if (acervoSolicitacaoConfirmar.DataEmprestimo.HasValue && acervoSolicitacaoConfirmar.DataDevolucao.HasValue && acervoSolicitacaoConfirmar.TipoAcervo.NaoEhAcervoBibliografico())
                throw new NegocioException(MensagemNegocio.DATA_DO_EMPRESTIMO_E_DEVOLUCAO_EXCLUSIVO_PARA_ACERVOS_BIBLIOGRAFICOS);

            if (acervoSolicitacaoConfirmar.ItemId.EhMenorIgualQueZero())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_ITEM_NAO_ENCONTRADA);

            var ehAcervoBibliografico = acervoSolicitacaoConfirmar.TipoAcervo.EhAcervoBibliografico();
            
            if (ehAcervoBibliografico)
            {
                if ((acervoSolicitacaoConfirmar.DataEmprestimo.HasValue && !acervoSolicitacaoConfirmar.DataDevolucao.HasValue) 
                    || (!acervoSolicitacaoConfirmar.DataEmprestimo.HasValue && acervoSolicitacaoConfirmar.DataDevolucao.HasValue))
                    throw new NegocioException(MensagemNegocio.DATA_DO_EMPRESTIMO_E_OU_DA_DEVOLUCAO_INVALIDOS);
                
                if (acervoSolicitacaoConfirmar.DataEmprestimo.EhDataFutura())
                    throw new NegocioException(MensagemNegocio.DATA_DO_EMPRESTIMO_NAO_PODE_SER_FUTURA);
                
                if (acervoSolicitacaoConfirmar.DataEmprestimo.EhMenorQue(acervoSolicitacaoConfirmar.DataVisita))
                    throw new NegocioException(MensagemNegocio.DATA_DO_EMPRESTIMO_MENOR_QUE_DATA_VISITA);
                
                if (acervoSolicitacaoConfirmar.DataDevolucao.EhMenorIgualQue(acervoSolicitacaoConfirmar.DataEmprestimo))
                    throw new NegocioException(MensagemNegocio.DATA_DA_DEVOLUCAO_MENOR_DATA_DO_EMPRESTIMO);
                
                if ((acervoSolicitacaoConfirmar.DataEmprestimo.HasValue || acervoSolicitacaoConfirmar.DataDevolucao.HasValue) 
                    && acervoSolicitacaoConfirmar.DataVisita.EhDataFutura())
                    throw new NegocioException(MensagemNegocio.DATA_DA_DEVOLUCAO_E_DATA_FUTURA_EM_VISITA_FUTURA);
            }

            if (acervoSolicitacaoConfirmar.TipoAtendimento.EhAtendimentoPresencial() && !acervoSolicitacaoConfirmar.DataVisita.HasValue)
                throw new NegocioException(MensagemNegocio.ITENS_ACERVOS_PRESENCIAL_DEVEM_TER_DATA_ACERVO);
            
            if (acervoSolicitacaoConfirmar.TipoAtendimento.EhAtendimentoViaEmail() && acervoSolicitacaoConfirmar.DataVisita.HasValue )
                throw new NegocioException(MensagemNegocio.ITENS_ACERVOS_EMAIL_NAO_DEVEM_TER_DATA_ACERVO);
        }

        private async Task InserirAcervoEmprestimo(long acervoSolicitacaoItemId, DateTime dataEmprestimo, DateTime dataDevolucao, SituacaoEmprestimo situacaoEmprestimo)
        {
            var acervoEmprestimo = new AcervoEmprestimo()
            {
                AcervoSolicitacaoItemId = acervoSolicitacaoItemId,
                DataEmprestimo = dataEmprestimo,
                DataDevolucao = dataDevolucao,
                Situacao = situacaoEmprestimo
            };
            await repositorioAcervoEmprestimo.Inserir(acervoEmprestimo);
        }

        private async Task AtualizarSituacaoAtendimento(AcervoSolicitacao acervoSolicitacao, bool todosItensEstaoCancelados = false)
        {
            acervoSolicitacao.Situacao = todosItensEstaoCancelados 
                ? SituacaoSolicitacao.CANCELADO 
                : await DeterminarSituacaoAtendimento(acervoSolicitacao.Id);
            
            acervoSolicitacao.DataSolicitacao = DateTimeExtension.HorarioBrasilia();
            
            await repositorioAcervoSolicitacao.Atualizar(acervoSolicitacao);
        }

        private async Task<SituacaoSolicitacao> DeterminarSituacaoAtendimento(long id)
        {
            var itensDaSolicitacao = await repositorioAcervoSolicitacaoItem.ObterItensEmSituacaoAguardandoAtendimentoOuVisitaOuFinalizadoManualmentePorSolicitacaoId(id);
            
            var temItensAguardandoVisita = itensDaSolicitacao.Any(a => a.Situacao.EstaAguardandoVisita());
            var temItensAguardandoAtendimento = itensDaSolicitacao.Any(a => a.Situacao.EstaAguardandoAtendimento());
            var todosItensFinalizadosManualmente = itensDaSolicitacao.All(a => a.Situacao.EstaFinalizadoManualmente());
            var todosItensAguardandoVisita = itensDaSolicitacao.All(a => a.Situacao.EstaAguardandoVisita());
            
            if (todosItensFinalizadosManualmente)
                return SituacaoSolicitacao.FINALIZADO_ATENDIMENTO;

            if (temItensAguardandoAtendimento)
                return SituacaoSolicitacao.ATENDIDO_PARCIALMENTE;
            
            if (todosItensAguardandoVisita || temItensAguardandoVisita)
                return SituacaoSolicitacao.AGUARDANDO_VISITA;

            throw new NegocioException(MensagemNegocio.SITUACAO_NAO_MAPEADA);
        }

        private async Task ValidarConflitosEventos(IEnumerable<DateTime> datasDasVisitas)
        {
            var eventosConflitantes = datasDasVisitas.PossuiElementos()
                ? await repositorioEvento.ObterEventosDeFeriadoESuspensaoPorDatas(datasDasVisitas.ToArray())
                : Enumerable.Empty<DateTime>();

            if (eventosConflitantes.Any())
                throw new NegocioException(string.Format(MensagemNegocio.DATAS_DE_VISITAS_CONFLITANTES,string.Join(',',eventosConflitantes.Select(s=> s.ToString("dd/MM")))));
        }

        public async Task<bool> FinalizarAtendimento(long acervoSolicitacaoId)
        {
            var acervoSolicitacao = await repositorioAcervoSolicitacao.ObterPorId(acervoSolicitacaoId);
            
            if (acervoSolicitacao.EhNulo())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_ENCONTRADA);
            
            if (acervoSolicitacao.Situacao.FoiAtendidaParcialmente())
                throw new NegocioException(MensagemNegocio.CANCELAR_SOLICITACAO_NAO_PERMITIDO_QUANDO_ITENS_ATENDIDOS_PARCIALMENTE);

            if (await repositorioAcervoSolicitacaoItem.PossuiItensEmSituacaoAguardandoAtendimentoOuAguardandoVisitaComDataFutura(acervoSolicitacaoId))
                throw new NegocioException(MensagemNegocio.NÃO_PODE_FINALIZAR_QUANDO_AGUARDANDO_VISITA_DATA_FUTURA_OU_AGUARDANDO_ATENDIMENTO);

            var itens = await repositorioAcervoSolicitacaoItem.ObterItensEmSituacaoAguardandoVisitaPorSolicitacaoId(acervoSolicitacaoId);
            
            var tran = transacao.Iniciar();
            try
            {
                acervoSolicitacao.Situacao = SituacaoSolicitacao.FINALIZADO_ATENDIMENTO;
                await repositorioAcervoSolicitacao.Atualizar(acervoSolicitacao);

                foreach (var item in itens)
                {
                    item.Situacao = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE;
                    await repositorioAcervoSolicitacaoItem.Atualizar(item);
                }
                
                tran.Commit();
                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                tran.Dispose();
            }
        }
        
        public async Task<bool> FinalizarAtendimentoItem(long acervoSolicitacaoItemId)
        {
            var acervoSolicitacaoItem = await repositorioAcervoSolicitacaoItem.ObterPorId(acervoSolicitacaoItemId);
            
            if (acervoSolicitacaoItem.EhNulo())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_ITEM_NAO_ENCONTRADA);

            var acervo = await repositorioAcervo.ObterPorId(acervoSolicitacaoItem.AcervoId);
            
            if (acervo.EhNulo())
                throw new NegocioException(MensagemNegocio.ACERVO_NAO_ENCONTRADO);
            
            var podeFinalizarItem = acervoSolicitacaoItem.TipoAtendimento.EhAtendimentoPresencial()
                                    && acervoSolicitacaoItem.DataVisita.HasValue
                                    && acervo.TipoAcervoId.NaoEhAcervoBibliografico()
                                    && acervoSolicitacaoItem.DataVisita.NaoEhDataFutura();
            
            if (!podeFinalizarItem)
                throw new NegocioException(MensagemNegocio.PERMITIDO_FINALIZAR_ATENDIMENTO_AGUARDANDO_VISITA_ATE_O_DIA_DE_HOJE);
            
            acervoSolicitacaoItem.Situacao = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE;
            await repositorioAcervoSolicitacaoItem.Atualizar(acervoSolicitacaoItem);

           var itens = await repositorioAcervoSolicitacaoItem.ObterItensPorSolicitacaoId(acervoSolicitacaoItem.AcervoSolicitacaoId);
           
           if (!itens.Any(a=> a.Situacao.EstaEmSituacaoAguardandoVisitaEAguardandoAtendimento()))
           {
               var acervoSolicitacao = await repositorioAcervoSolicitacao.ObterPorId(acervoSolicitacaoItem.AcervoSolicitacaoId);
               acervoSolicitacao.Situacao = SituacaoSolicitacao.FINALIZADO_ATENDIMENTO;
               await repositorioAcervoSolicitacao.Atualizar(acervoSolicitacao);
           }
           
           return true;
        }

        public async Task<bool> CancelarAtendimento(long acervoSolicitacaoId)
        {
            var acervoSolicitacao = await repositorioAcervoSolicitacao.ObterPorId(acervoSolicitacaoId);
            
            if (acervoSolicitacao.EhNulo())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_ENCONTRADA);
            
            if (await repositorioAcervoSolicitacaoItem.PossuiItensFinalizadosAutomaticamente(acervoSolicitacaoId))
                throw new NegocioException(MensagemNegocio.NAO_PODE_CANCELAR_ATENDIMENTO_COM_ITEM_FINALIZADO_AUTOMATICAMENTE_MANUALMENTE);
            
            var itens = await repositorioAcervoSolicitacaoItem.ObterItensPorSolicitacaoId(acervoSolicitacaoId);

            var acervos = await repositorioAcervo.ObterAcervosPorIds(itens.Select(s => s.AcervoId).ToArray());
                
            var tran = transacao.Iniciar();
            try
            {
                acervoSolicitacao.Situacao = SituacaoSolicitacao.CANCELADO;
                await repositorioAcervoSolicitacao.Atualizar(acervoSolicitacao);

                foreach (var item in itens)
                {
                    item.Situacao = SituacaoSolicitacaoItem.CANCELADO;
                    await repositorioAcervoSolicitacaoItem.Atualizar(item);

                    if (item.TipoAtendimento.NaoEhNulo() && item.TipoAtendimento.EhAtendimentoPresencial())
                    {
                        await servicoEvento.ExcluirEventoPorAcervoSolicitacaoItem(item.Id);

                        var ehAcervoBibliografico = acervos.Any(f => f.Id == item.AcervoId && f.TipoAcervoId.EhAcervoBibliografico());
                        
                        if (ehAcervoBibliografico)
                            await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.DISPONIVEL,item.AcervoId);  
                    }
                }
                
                tran.Commit();
                await servicoMensageria.Publicar(RotasRabbit.NotificarViaEmailCancelamentoAtendimento, acervoSolicitacaoId, Guid.NewGuid(), null);
                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                tran.Dispose();
            }
        }

        public async Task<bool> CancelarItemAtendimento(long acervoSolicitacaoItemId)
        {
            var acervoSolicitacaoItem = await repositorioAcervoSolicitacaoItem.ObterPorId(acervoSolicitacaoItemId);
            
            if (acervoSolicitacaoItem.EhNulo())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_ITEM_NAO_ENCONTRADA);
            
            var itens = await repositorioAcervoSolicitacaoItem.ObterItensPorSolicitacaoId(acervoSolicitacaoItem.AcervoSolicitacaoId);

            if (itens.Any(a=> a.Situacao.EstaEmSituacaoFinalizadoAutomaticamenteOuCancelado() && a.Id == acervoSolicitacaoItemId)) 
                throw new NegocioException(MensagemNegocio.NAO_PODE_CANCELAR_ATENDIMENTO_COM_ITEM_FINALIZADO_AUTOMATICAMENTE_MANUALMENTE);
            
            var acervoSolicitacao = await repositorioAcervoSolicitacao.ObterPorId(acervoSolicitacaoItem.AcervoSolicitacaoId);
            var todosItensEstaoCancelados = itens.Where(w => w.Id != acervoSolicitacaoItemId).All(a => a.Situacao.EstaCancelado());
            
            var acervos = await repositorioAcervo.ObterAcervosPorIds(new []{ acervoSolicitacaoItem.AcervoId });
            
            var tran = transacao.Iniciar();
            try
            {
                acervoSolicitacaoItem.Situacao = SituacaoSolicitacaoItem.CANCELADO;
                await repositorioAcervoSolicitacaoItem.Atualizar(acervoSolicitacaoItem);

                if (acervoSolicitacaoItem.TipoAtendimento.EhAtendimentoPresencial())
                    await servicoEvento.ExcluirEventoPorAcervoSolicitacaoItem(acervoSolicitacaoItem.Id);
                
                if (acervos.Any(a=> a.TipoAcervoId.EhAcervoBibliografico()))
                    await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.DISPONIVEL,acervoSolicitacaoItem.AcervoId);
                
                await AtualizarSituacaoAtendimento(acervoSolicitacao,todosItensEstaoCancelados);
                
                await servicoMensageria.Publicar(RotasRabbit.NotificarViaEmailCancelamentoAtendimentoItem, acervoSolicitacaoItemId, Guid.NewGuid(), null);
                
                tran.Commit();

                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                tran.Dispose();
            }
        }

        public async Task<bool> AlterarDataVisitaDoItemAtendimento(AlterarDataVisitaAcervoSolicitacaoItemDTO alterarDataVisitaAcervoSolicitacaoItemDto)
        {
            if (alterarDataVisitaAcervoSolicitacaoItemDto.DataVisita < DateTimeExtension.HorarioBrasilia().Date)
                throw new NegocioException(MensagemNegocio.ITENS_ACERVOS_PRESENCIAL_NAO_DEVEM_TER_DATA_ACERVO_PASSADAS);
            
            var acervoSolicitacaoItem = await repositorioAcervoSolicitacaoItem.ObterPorId(alterarDataVisitaAcervoSolicitacaoItemDto.Id);
            
            if (acervoSolicitacaoItem.EhNulo())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_ITEM_NAO_ENCONTRADA);
            
            if (await repositorioAcervoSolicitacaoItem.AtendimentoPossuiSituacaoAguardandoVisitaEItemSituacaoFinalizadoAutomaticamenteOuCancelado(alterarDataVisitaAcervoSolicitacaoItemDto.Id))
                throw new NegocioException(MensagemNegocio.ATENDIMENTO_NAO_ESTA_AGUARDANDO_VISITA);

            acervoSolicitacaoItem.DataVisita = alterarDataVisitaAcervoSolicitacaoItemDto.DataVisita;
            await repositorioAcervoSolicitacaoItem.Atualizar(acervoSolicitacaoItem);
            
            return true;
        }
        public async Task<long> Inserir(AcervoSolicitacaoManualDTO acervoSolicitacaoManualDto)
        {
            var usuario = await servicoUsuario.ObterPorId(acervoSolicitacaoManualDto.UsuarioId);
            
            if (usuario.EhNulo())
                throw new NegocioException(MensagemNegocio.USUARIO_NAO_ENCONTRADO);
            
            ValidarCamposDataVisitaEmprestimoDevolucao(acervoSolicitacaoManualDto);
            
            var datasDasVisitas = acervoSolicitacaoManualDto.Itens
                .Where(w => w.TipoAtendimento.EhAtendimentoPresencial())
                .Select(s => s.DataVisita.Value);

            await ValidarConflitosEventos(datasDasVisitas);
            
            var acervoSolicitacao = mapper.Map<AcervoSolicitacao>(acervoSolicitacaoManualDto);
            
            acervoSolicitacao.Origem = Origem.Manual;

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            
            acervoSolicitacao.Situacao = acervoSolicitacaoManualDto.Itens.Any(a=> a.TipoAtendimento.EhAtendimentoPresencial() && !a.DataEmprestimo.HasValue && !a.DataDevolucao.HasValue) 
                ? SituacaoSolicitacao.AGUARDANDO_VISITA 
                : SituacaoSolicitacao.FINALIZADO_ATENDIMENTO;
            
            var tran = transacao.Iniciar();
            try
            {
                acervoSolicitacao.Id =  await repositorioAcervoSolicitacao.Inserir(acervoSolicitacao);

                foreach (var item in acervoSolicitacao.Itens)
                {
                    item.AcervoSolicitacaoId = acervoSolicitacao.Id;
                    item.ResponsavelId = usuarioLogado.Id;
                    
                    var acervoSolicitacaoItemManualDto = acervoSolicitacaoManualDto.Itens.FirstOrDefault(f => f.AcervoId == item.AcervoId);

                    var possuiInformacoesEmprestimo = acervoSolicitacaoItemManualDto.DataEmprestimo.HasValue && acervoSolicitacaoItemManualDto.DataDevolucao.HasValue;

                    item.Situacao = item.TipoAtendimento.EhAtendimentoViaEmail() || possuiInformacoesEmprestimo
                        ? SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                        : SituacaoSolicitacaoItem.AGUARDANDO_VISITA;
                    
                    await repositorioAcervoSolicitacaoItem.Inserir(item);

                    if (item.TipoAtendimento.EhAtendimentoPresencial())
                    {
                        await servicoEvento.InserirEventoVisita(item.DataVisita.Value, item.Id);

                        if (acervoSolicitacaoItemManualDto.TipoAcervo.EhAcervoBibliografico())
                        {
                            if (possuiInformacoesEmprestimo)
                            {
                                await InserirAcervoEmprestimo(item.Id, acervoSolicitacaoItemManualDto.DataEmprestimo.Value, acervoSolicitacaoItemManualDto.DataDevolucao.Value, SituacaoEmprestimo.EMPRESTADO);
                                await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.EMPRESTADO,item.AcervoId);
                            }
                            else
                                await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.RESERVADO,item.AcervoId);
                        }
                    }
                }
                tran.Commit();

                return acervoSolicitacao.Id;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                tran.Dispose();
            }
        }

        public async Task<long> Alterar(AcervoSolicitacaoManualDTO acervoSolicitacaoManualDto)
        {
           var usuario = await servicoUsuario.ObterPorId(acervoSolicitacaoManualDto.UsuarioId);
            
            if (usuario.EhNulo())
                throw new NegocioException(MensagemNegocio.USUARIO_NAO_ENCONTRADO);

            var acervoSolicitacao = await repositorioAcervoSolicitacao.ObterPorId(acervoSolicitacaoManualDto.Id);
            
            if (acervoSolicitacao.EhNulo())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_ENCONTRADA);
            
            ValidarCamposDataVisitaEmprestimoDevolucao(acervoSolicitacaoManualDto);

            var datasDasVisitas = acervoSolicitacaoManualDto.Itens
                .Where(w => w.TipoAtendimento.EhAtendimentoPresencial())
                .Select(s => s.DataVisita.Value);

            await ValidarConflitosEventos(datasDasVisitas);
            
            acervoSolicitacao.Origem = Origem.Manual;
            
            acervoSolicitacao.DataSolicitacao = acervoSolicitacaoManualDto.DataSolicitacao;

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            
            acervoSolicitacao.Situacao = acervoSolicitacaoManualDto.Itens.Any(a=> a.TipoAtendimento.EhAtendimentoPresencial() && !a.DataEmprestimo.HasValue && !a.DataDevolucao.HasValue) 
                ? SituacaoSolicitacao.AGUARDANDO_VISITA 
                : SituacaoSolicitacao.FINALIZADO_ATENDIMENTO;
            
            var itensAtuais = await repositorioAcervoSolicitacaoItem.ObterItensPorSolicitacaoId(acervoSolicitacao.Id);
            
            var itensEmprestados = await repositorioAcervoEmprestimo.ObterUltimoEmprestimoPorAcervoSolicitacaoItemIds(acervoSolicitacaoManualDto.Itens.Where(w=> w.Id.HasValue).Select(s => s.Id.Value).ToArray());

            var tran = transacao.Iniciar();
            try
            {
                await repositorioAcervoSolicitacao.Atualizar(acervoSolicitacao);
                
                foreach (var itemProposto in acervoSolicitacaoManualDto.Itens)
                {
                    var item = mapper.Map<AcervoSolicitacaoItem>(itemProposto);
                    
                    var possuiInformacoesEmprestimo = itemProposto.DataEmprestimo.HasValue && itemProposto.DataDevolucao.HasValue;
                    
                    item.Situacao =  item.TipoAtendimento.EhAtendimentoViaEmail()|| possuiInformacoesEmprestimo
                                     ? SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                                     : SituacaoSolicitacaoItem.AGUARDANDO_VISITA;

                    item.ResponsavelId = usuarioLogado.Id;
                    
                    if (item.Id.EhMaiorQueZero())
                    {
                        var itemAtual = itensAtuais.FirstOrDefault(f=> f.Id == item.Id);

                        var eraPresencial = itemAtual.TipoAtendimento.EhAtendimentoPresencial();
                        
                        itemAtual.TipoAtendimento = item.TipoAtendimento;
                        
                        itemAtual.DataVisita = item.DataVisita;
                        
                        itemAtual.Situacao = item.Situacao;
                        
                        await repositorioAcervoSolicitacaoItem.Atualizar(itemAtual);

                        var itemEmprestado = itensEmprestados.FirstOrDefault(f => f.AcervoSolicitacaoItemId == itemAtual.Id);
                        var naoTemEmprestimo = itemEmprestado.EhNulo();
                        
                        if (eraPresencial && item.TipoAtendimento.EhAtendimentoViaEmail())
                        {
                            await servicoEvento.ExcluirEventoPorAcervoSolicitacaoItem(item.Id);
                            await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.DISPONIVEL,item.AcervoId);
                        }

                        if (item.TipoAtendimento.EhAtendimentoPresencial())
                        {
                            await servicoEvento.AtualizarEventoVisita(item.DataVisita.Value, item.Id);
                            
                            if (itemProposto.TipoAcervo.EhAcervoBibliografico())
                            {
                                if (naoTemEmprestimo)
                                {
                                    if (possuiInformacoesEmprestimo)
                                    {
                                        await InserirAcervoEmprestimo(item.Id, itemProposto.DataEmprestimo.Value, itemProposto.DataDevolucao.Value, SituacaoEmprestimo.EMPRESTADO);
                                        await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.EMPRESTADO,item.AcervoId);
                                    }
                                    else
                                        await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.RESERVADO,item.AcervoId);
                                }
                                else
                                {
                                    itemEmprestado.DataEmprestimo = itemProposto.DataEmprestimo.Value;
                                    itemEmprestado.DataDevolucao = itemProposto.DataDevolucao.Value;
                                    itemEmprestado.Situacao = SituacaoEmprestimo.EMPRESTADO;
                                    await repositorioAcervoEmprestimo.Atualizar(itemEmprestado);
                                }
                            }
                        }
                    }
                    else
                    {
                        item.AcervoSolicitacaoId = acervoSolicitacao.Id;
                    
                        await repositorioAcervoSolicitacaoItem.Inserir(item);

                        var temInformacoesDeEmprestimo = itemProposto.DataEmprestimo.HasValue &&
                                                         itemProposto.DataDevolucao.HasValue &&
                                                         itemProposto.TipoAcervo.EhAcervoBibliografico();

                        if (item.TipoAtendimento.EhAtendimentoPresencial())
                        {
                            await servicoEvento.InserirEventoVisita(item.DataVisita.Value, item.Id);

                            if (itemProposto.TipoAcervo.EhAcervoBibliografico())
                            {
                                if (temInformacoesDeEmprestimo)
                                {
                                    await InserirAcervoEmprestimo(item.Id, itemProposto.DataEmprestimo.Value, itemProposto.DataDevolucao.Value, SituacaoEmprestimo.EMPRESTADO);
                                    await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.EMPRESTADO,item.AcervoId);
                                }
                                else
                                    await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.RESERVADO,item.AcervoId);
                            }
                        }
                    }
                }
                tran.Commit();

                return acervoSolicitacao.Id;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                tran.Dispose();
            }
        }

        private static void ValidarCamposDataVisitaEmprestimoDevolucao(AcervoSolicitacaoManualDTO acervoSolicitacaoManualDto)
        {
            if (acervoSolicitacaoManualDto.Itens.Any(a=> a.TipoAtendimento.EhAtendimentoPresencial() && !a.DataVisita.HasValue))
                throw new NegocioException(MensagemNegocio.ITENS_ACERVOS_PRESENCIAL_DEVEM_TER_DATA_ACERVO);

            if (acervoSolicitacaoManualDto.Itens.Any(a=> a.TipoAtendimento.EhInvalido()))
                throw new NegocioException(MensagemNegocio.TIPO_ATENDIMENTO_INVALIDO);
            
            if (acervoSolicitacaoManualDto.Itens.Any(a=> a.TipoAtendimento.EhAtendimentoViaEmail() && a.DataVisita.HasValue))
                throw new NegocioException(MensagemNegocio.ITENS_ACERVOS_EMAIL_NAO_DEVEM_TER_DATA_ACERVO);
            
            if (acervoSolicitacaoManualDto.Itens.Any(a=> a.TipoAcervo.EhAcervoBibliografico()))
            {
                if (acervoSolicitacaoManualDto.Itens.Any(a=> a.DataEmprestimo.HasValue && a.DataEmprestimo.Value.Date > DateTimeExtension.HorarioBrasilia().Date))
                    throw new NegocioException(MensagemNegocio.DATA_DO_EMPRESTIMO_NAO_PODE_SER_FUTURA);
                
                if (acervoSolicitacaoManualDto.Itens.Any(a=> a.DataEmprestimo.HasValue && a.DataVisita.HasValue && a.DataEmprestimo.Value.Date < a.DataVisita.Value.Date))
                    throw new NegocioException(MensagemNegocio.DATA_DO_EMPRESTIMO_MENOR_QUE_DATA_VISITA);
                
                if (acervoSolicitacaoManualDto.Itens.Any(a=> a.DataDevolucao.HasValue && a.DataEmprestimo.HasValue && a.DataDevolucao.Value.Date < a.DataEmprestimo.Value.Date))
                    throw new NegocioException(MensagemNegocio.DATA_DA_DEVOLUCAO_MENOR_DATA_DO_EMPRESTIMO);
                
            }
            else
            {
                if (acervoSolicitacaoManualDto.Itens.Any(a=> a.DataEmprestimo.HasValue || a.DataDevolucao.HasValue))
                    throw new NegocioException(MensagemNegocio.DATA_DO_EMPRESTIMO_E_DEVOLUCAO_EXCLUSIVO_PARA_ACERVOS_BIBLIOGRAFICOS);
            }
        }
    }
}
