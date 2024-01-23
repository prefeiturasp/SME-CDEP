using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
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
        
        public ServicoAcervoSolicitacao(IRepositorioAcervoSolicitacao repositorioAcervoSolicitacao, 
            IMapper mapper,ITransacao transacao,IRepositorioAcervoSolicitacaoItem repositorioAcervoSolicitacaoItem,
            IRepositorioAcervoCreditoAutor repositorioAcervoCreditoAutor,IRepositorioUsuario repositorioUsuario,IRepositorioAcervo repositorioAcervo) 
        {
            this.repositorioAcervoSolicitacao = repositorioAcervoSolicitacao ?? throw new ArgumentNullException(nameof(repositorioAcervoSolicitacao));
            this.repositorioAcervoSolicitacaoItem = repositorioAcervoSolicitacaoItem ?? throw new ArgumentNullException(nameof(repositorioAcervoSolicitacaoItem));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.transacao = transacao ?? throw new ArgumentNullException(nameof(transacao));
            this.repositorioAcervoCreditoAutor = repositorioAcervoCreditoAutor ?? throw new ArgumentNullException(nameof(repositorioAcervoCreditoAutor));
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.repositorioAcervo = repositorioAcervo ?? throw new ArgumentNullException(nameof(repositorioAcervo));
        }

        public async Task<IEnumerable<AcervoSolicitacaoItemRetornoCadastroDTO>> Inserir(AcervoSolicitacaoCadastroDTO acervoSolicitacaoCadastroDTO)
        {
            var usuarioSolicitante = await repositorioUsuario.ObterPorId(acervoSolicitacaoCadastroDTO.UsuarioId);
            if (usuarioSolicitante.EhNulo())
                throw new NegocioException(MensagemNegocio.USUARIO_NAO_ENCONTRADO);

            var arquivosEncontrados = await repositorioAcervo.ObterArquivosPorAcervoId(acervoSolicitacaoCadastroDTO.Itens.Select(s=> s.AcervoId).ToArray());
            
            var acervoSolicitacao = mapper.Map<AcervoSolicitacao>(acervoSolicitacaoCadastroDTO);
            
            var tran = transacao.Iniciar();
            try
            {
                acervoSolicitacao.Situacao = acervoSolicitacao.Itens
                    .Select(s => s.AcervoId)
                    .Except(arquivosEncontrados.Select(s => s.AcervoId))
                    .Any() ? SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO : SituacaoSolicitacao.FINALIZADO_ATENDIMENTO;
                
                acervoSolicitacao.Id =  await repositorioAcervoSolicitacao.Inserir(acervoSolicitacao);

                foreach (var item in acervoSolicitacaoCadastroDTO.Itens)
                {
                    var acervoSolicitacaoItem = mapper.Map<AcervoSolicitacaoItem>(item);
                    
                    acervoSolicitacaoItem.AcervoSolicitacaoId = acervoSolicitacao.Id;
                    
                    acervoSolicitacaoItem.Situacao = arquivosEncontrados.Any(a => a.AcervoId == item.AcervoId)
                        ? SituacaoSolicitacaoItem.FINALIZADO
                        : SituacaoSolicitacaoItem.EM_ANALISE;
                    
                    await repositorioAcervoSolicitacaoItem.Inserir(acervoSolicitacaoItem);
                }
                tran.Commit();

                var retorno = await MapearRetornoDosItens(acervoSolicitacaoCadastroDTO,arquivosEncontrados);
                return retorno;
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

        public async Task<AcervoSolicitacaoDTO> ObterPorId(long acervoSolicitacaoId)
        {
            return mapper.Map<AcervoSolicitacaoDTO>(await repositorioAcervoSolicitacao.ObterAcervoSolicitacaoCompletoPorId(acervoSolicitacaoId));
        }

        public async Task<IEnumerable<AcervoSolicitacaoDTO>> ObterTodosPorUsuario(int usuarioId)
        {
            return mapper.Map<IEnumerable<AcervoSolicitacaoDTO>>(await repositorioAcervoSolicitacao.ObterTodosCompletosPorUsuario(usuarioId));
        }

        public async Task<AcervoSolicitacaoDTO> Alterar(AcervoSolicitacaoDTO acervoSolicitacaoDto)
        {
            var usuario = mapper.Map<AcervoSolicitacao>(acervoSolicitacaoDto);
            return mapper.Map<AcervoSolicitacaoDTO>(await repositorioAcervoSolicitacao.Atualizar(usuario));
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
            
            var itensSolicitacaoItemRetornoDTO = mapper.Map<IEnumerable<AcervoTipoTituloAcervoIdCreditosAutoresDTO>>(acervos);

            return itensSolicitacaoItemRetornoDTO;
        }
        
        private async Task<IEnumerable<AcervoSolicitacaoItemRetornoCadastroDTO>> MapearRetornoDosItens(AcervoSolicitacaoCadastroDTO acervoSolicitacaoCadastroDTO, IEnumerable<ArquivoCodigoNomeAcervoId> arquivos)
        {
            var acervosItensCompletos = await repositorioAcervo
                .ObterAcervosSolicitacoesItensCompletoPorAcervosIds(acervoSolicitacaoCadastroDTO.Itens.Select(s=> s.AcervoId).ToArray());

            var retornos = mapper.Map<IEnumerable<AcervoSolicitacaoItemRetornoCadastroDTO>>(acervosItensCompletos);

            foreach (var retorno in retornos)
                retorno.Arquivos = mapper.Map<IEnumerable<ArquivoCodigoNomeDTO>>(arquivos.Where(w => w.AcervoId == retorno.AcervoId).Select(s=> s));

            return retornos;
        }
    }
}
