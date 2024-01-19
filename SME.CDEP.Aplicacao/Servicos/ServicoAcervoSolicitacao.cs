using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcervoSolicitacao : IServicoAcervoSolicitacao
    {
        private readonly IRepositorioAcervoSolicitacao repositorioAcervoSolicitacao;
        private readonly IRepositorioAcervoSolicitacaoItem repositorioAcervoSolicitacaoItem;
        private readonly IMapper mapper;
        private readonly ITransacao transacao;
        
        public ServicoAcervoSolicitacao(IRepositorioAcervoSolicitacao repositorioAcervoSolicitacao, 
            IMapper mapper,ITransacao transacao,IRepositorioAcervoSolicitacaoItem repositorioAcervoSolicitacaoItem) 
        {
            this.repositorioAcervoSolicitacao = repositorioAcervoSolicitacao ?? throw new ArgumentNullException(nameof(repositorioAcervoSolicitacao));
            this.repositorioAcervoSolicitacaoItem = repositorioAcervoSolicitacaoItem ?? throw new ArgumentNullException(nameof(repositorioAcervoSolicitacaoItem));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.transacao = transacao ?? throw new ArgumentNullException(nameof(transacao));
        }

        public async Task<long> Inserir(AcervoSolicitacaoDTO acervoSolicitacaoDto)
        {
            var tran = transacao.Iniciar();
            try
            {
                var acervoSolicitacao = mapper.Map<AcervoSolicitacao>(acervoSolicitacaoDto);
                acervoSolicitacao.Id =  await repositorioAcervoSolicitacao.Inserir(acervoSolicitacao);

                foreach (var item in acervoSolicitacao.Itens)
                {
                    var acervoSolicitacaoItem = mapper.Map<AcervoSolicitacaoItem>(item);
                    acervoSolicitacaoItem.AcervoSolicitacaoId = acervoSolicitacao.Id;
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
    }
}
