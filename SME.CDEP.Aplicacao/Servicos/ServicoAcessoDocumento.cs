using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcessoDocumento : IServicoAcessoDocumento
    {
        private readonly IRepositorioAcessoDocumento repositorioAcessoDocumento;
        private readonly IMapper mapper;
        
        public ServicoAcessoDocumento(IRepositorioAcessoDocumento repositorioAcessoDocumento, IMapper mapper) 
        {
            this.repositorioAcessoDocumento = repositorioAcessoDocumento ?? throw new ArgumentNullException(nameof(repositorioAcessoDocumento));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<long> Inserir(AcessoDocumentoDTO acessoDocumentoDto)
        {
            var acessoDocumento = mapper.Map<AcessoDocumento>(acessoDocumentoDto);
            return await repositorioAcessoDocumento.Inserir(acessoDocumento);
        }

        public async Task<IList<AcessoDocumentoDTO>> ObterTodos()
        {
            return (await repositorioAcessoDocumento.ObterTodos()).ToList().Where(w=> !w.Excluido).Select(s=> mapper.Map<AcessoDocumentoDTO>(s)).ToList();
        }

        public async Task<AcessoDocumentoDTO> Alterar(AcessoDocumentoDTO acessoDocumentoDto)
        {
            var acessoDocumento = mapper.Map<AcessoDocumento>(acessoDocumentoDto);
            return mapper.Map<AcessoDocumentoDTO>(await repositorioAcessoDocumento.Atualizar(acessoDocumento));
        }

        public async Task<AcessoDocumentoDTO> ObterPorId(long acessoDocumentoId)
        {
            return mapper.Map<AcessoDocumentoDTO>(await repositorioAcessoDocumento.ObterPorId(acessoDocumentoId));
        }

        public async Task<bool> Excluir(long acessoDocumentoId)
        {
            await repositorioAcessoDocumento.Remover(acessoDocumentoId);
            return true;
        }
    }
}
