using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
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
        private readonly IRepositorioAcervoCreditoAutor repositorioAcervoCreditoAutor;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IRepositorioAcervo repositorioAcervo;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IContextoAplicacao contextoAplicacao;
        
        public ServicoAcervoSolicitacao(IRepositorioAcervoSolicitacao repositorioAcervoSolicitacao, 
            IMapper mapper,ITransacao transacao,IRepositorioAcervoSolicitacaoItem repositorioAcervoSolicitacaoItem,
            IRepositorioAcervoCreditoAutor repositorioAcervoCreditoAutor,IRepositorioUsuario repositorioUsuario,IRepositorioAcervo repositorioAcervo,
            IServicoUsuario servicoUsuario,IContextoAplicacao contextoAplicacao) 
        {
            this.repositorioAcervoSolicitacao = repositorioAcervoSolicitacao ?? throw new ArgumentNullException(nameof(repositorioAcervoSolicitacao));
            this.repositorioAcervoSolicitacaoItem = repositorioAcervoSolicitacaoItem ?? throw new ArgumentNullException(nameof(repositorioAcervoSolicitacaoItem));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.transacao = transacao ?? throw new ArgumentNullException(nameof(transacao));
            this.repositorioAcervoCreditoAutor = repositorioAcervoCreditoAutor ?? throw new ArgumentNullException(nameof(repositorioAcervoCreditoAutor));
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.repositorioAcervo = repositorioAcervo ?? throw new ArgumentNullException(nameof(repositorioAcervo));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
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

        public async Task<IEnumerable<AcervoTipoTituloAcervoIdCreditosAutoresDTO>> ObterItensDoAcervoPorFiltros(long[] acervosIds)
        {
            var acervos = await repositorioAcervoSolicitacao.ObterItensDoAcervoPorAcervosIds(acervosIds);

            if (acervos.EhNulo())
                throw new NegocioException(MensagemNegocio.ACERVO_NAO_ENCONTRADO);
            
            return mapper.Map<IEnumerable<AcervoTipoTituloAcervoIdCreditosAutoresDTO>>(acervos);
        }
        
        public async Task<IEnumerable<AcervoSolicitacaoItemRetornoCadastroDTO>> ObterPorId(long acervoSolicitacaoId)
        {
            var acervosItensCompletos = await repositorioAcervo.ObterAcervosSolicitacoesItensCompletoPorId(acervoSolicitacaoId);

            var acervoItensRetorno = mapper.Map<IEnumerable<AcervoSolicitacaoItemRetornoCadastroDTO>>(acervosItensCompletos);

            var arquivosDoAcervo = await repositorioAcervo.ObterArquivosPorAcervoId(acervosItensCompletos.Select(s => s.AcervoId).ToArray());

            foreach (var retorno in acervoItensRetorno)
                retorno.Arquivos = mapper.Map<IEnumerable<ArquivoCodigoNomeDTO>>(arquivosDoAcervo.Where(w => w.AcervoId == retorno.AcervoId).Select(s=> s));

            return acervoItensRetorno;
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

        public async Task<PaginacaoResultadoDTO<SolicitacaoDTO>> ObterSolicitacoesPorFiltro(FiltroSolicitacaoDTO filtroSolicitacaoDto)
        {
            var solicitacoes = mapper.Map<IEnumerable<SolicitacaoDTO>>(await repositorioAcervoSolicitacaoItem
                .ObterSolicitacoesPorFiltro(filtroSolicitacaoDto.AcervoSolicitacaoId, filtroSolicitacaoDto.TipoAcervo, 
                    filtroSolicitacaoDto.DataSolicitacao, filtroSolicitacaoDto.Responsavel, filtroSolicitacaoDto.SituacaoItem, 
                    filtroSolicitacaoDto.DataVisitaInicio, filtroSolicitacaoDto.DataVisitaFim));
            
            var totalRegistros = solicitacoes.Count();
            var paginacao = Paginacao;
            
            return new PaginacaoResultadoDTO<SolicitacaoDTO>()
            {
                Items = solicitacoes.Skip(paginacao.QuantidadeRegistrosIgnorados).Take(paginacao.QuantidadeRegistros),
                TotalRegistros = totalRegistros,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacao.QuantidadeRegistros)
            };
        }
    }
}
