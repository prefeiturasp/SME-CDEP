using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoTipoAnexo : IServicoTipoAnexo
    {
        private readonly IRepositorioTipoAnexo repositorioTipoAnexo;
        private readonly IMapper mapper;
        
        public ServicoTipoAnexo(IRepositorioTipoAnexo repositorioTipoAnexo, IMapper mapper) 
        {
            this.repositorioTipoAnexo = repositorioTipoAnexo ?? throw new ArgumentNullException(nameof(repositorioTipoAnexo));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<long> Inserir(TipoAnexoDTO tipoAnexoDTO)
        {
            var tipoAnexo = mapper.Map<TipoAnexo>(tipoAnexoDTO);
            return repositorioTipoAnexo.Inserir(tipoAnexo);
        }

        public async Task<IList<TipoAnexoDTO>> ObterTodos()
        {
            return (await repositorioTipoAnexo.ObterTodos()).Where(w=> !w.Excluido).Select(s=> mapper.Map<TipoAnexoDTO>(s)).ToList();
        }

        public async Task<TipoAnexoDTO> Alterar(TipoAnexoDTO tipoAnexoDTO)
        {
            var tipoAnexo = mapper.Map<TipoAnexo>(tipoAnexoDTO);
            return mapper.Map<TipoAnexoDTO>(await repositorioTipoAnexo.Atualizar(tipoAnexo));
        }

        public async Task<TipoAnexoDTO> ObterPorId(long tipoAnexoId)
        {
            return mapper.Map<TipoAnexoDTO>(await repositorioTipoAnexo.ObterPorId(tipoAnexoId));
        }

        public async Task<bool> Excluir(long tipoAnexoId)
        {
            var tipoAnexo = await ObterPorId(tipoAnexoId);
            tipoAnexo.Excluido = true;
            await Alterar(tipoAnexo);
            return true;
        }
    }
}
