using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoSuporte : IServicoSuporte
    {
        private readonly IRepositorioSuporte repositorioSuporte;
        private readonly IMapper mapper;
        
        public ServicoSuporte(IRepositorioSuporte repositorioSuporte, IMapper mapper) 
        {
            this.repositorioSuporte = repositorioSuporte ?? throw new ArgumentNullException(nameof(repositorioSuporte));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<long> Inserir(SuporteDTO suporteDTO)
        {
            var suporte = mapper.Map<Suporte>(suporteDTO);
            return await repositorioSuporte.Inserir(suporte);
        }

        public async Task<IList<SuporteDTO>> ObterTodos()
        {
            return (await repositorioSuporte.ObterTodos()).ToList().Where(w=> !w.Excluido).Select(s=> mapper.Map<SuporteDTO>(s)).ToList();
        }

        public async Task<SuporteDTO> Alterar(SuporteDTO suporteDTO)
        {
            var suporte = mapper.Map<Suporte>(suporteDTO);
            return mapper.Map<SuporteDTO>(await repositorioSuporte.Atualizar(suporte));
        }

        public async Task<SuporteDTO> ObterPorId(long suporteId)
        {
            return mapper.Map<SuporteDTO>(await repositorioSuporte.ObterPorId(suporteId));
        }

        public async Task<bool> Excluir(long suporteId)
        {
            var suporteDto = await ObterPorId(suporteId);
            suporteDto.Excluido = true;
            await Alterar(suporteDto);
            return true;
        }
    }
}
