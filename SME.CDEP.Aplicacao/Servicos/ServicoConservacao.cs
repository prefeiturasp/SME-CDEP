using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoConservacao : IServicoConservacao
    {
        private readonly IRepositorioConservacao repositorioConservacao;
        private readonly IMapper mapper;
        
        public ServicoConservacao(IRepositorioConservacao repositorioConservacao, IMapper mapper) 
        {
            this.repositorioConservacao = repositorioConservacao ?? throw new ArgumentNullException(nameof(repositorioConservacao));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<long> Inserir(ConservacaoDTO conservacaoDTO)
        {
            var conservacao = mapper.Map<Conservacao>(conservacaoDTO);
            return await repositorioConservacao.Inserir(conservacao);
        }

        public async Task<IList<ConservacaoDTO>> ObterTodos()
        {
            return (await repositorioConservacao.ObterTodos()).ToList().Where(w=> !w.Excluido).Select(s=> mapper.Map<ConservacaoDTO>(s)).ToList();
        }

        public async Task<ConservacaoDTO> Alterar(ConservacaoDTO conservacaoDTO)
        {
            var conservacao = mapper.Map<Conservacao>(conservacaoDTO);
            return mapper.Map<ConservacaoDTO>(await repositorioConservacao.Atualizar(conservacao));
        }

        public async Task<ConservacaoDTO> ObterPorId(long conservacaoId)
        {
            return mapper.Map<ConservacaoDTO>(await repositorioConservacao.ObterPorId(conservacaoId));
        }

        public async Task<bool> Excluir(long conservacaoId)
        {
            var conservacao = await ObterPorId(conservacaoId);
            conservacao.Excluido = true;
            await Alterar(conservacao);
            return true;
        }
    }
}
