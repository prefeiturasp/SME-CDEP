using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoFormato : IServicoFormato
    {
        private readonly IRepositorioFormato repositorioFormato;
        private readonly IMapper mapper;
        
        public ServicoFormato(IRepositorioFormato repositorioFormato, IMapper mapper) 
        {
            this.repositorioFormato = repositorioFormato ?? throw new ArgumentNullException(nameof(repositorioFormato));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<long> Inserir(FormatoDTO formatoDto)
        {
            var formato = mapper.Map<Formato>(formatoDto);
            return repositorioFormato.Inserir(formato);
        }

        public async Task<IList<FormatoDTO>> ObterTodos()
        {
            return (await repositorioFormato.ObterTodos()).Where(w=> !w.Excluido).Select(s=> mapper.Map<FormatoDTO>(s)).ToList();
        }

        public async Task<FormatoDTO> Alterar(FormatoDTO formatoDTO)
        {
            var formato = mapper.Map<Formato>(formatoDTO);
            return mapper.Map<FormatoDTO>(await repositorioFormato.Atualizar(formato));
        }

        public async Task<FormatoDTO> ObterPorId(long formatoId)
        {
            return mapper.Map<FormatoDTO>(await repositorioFormato.ObterPorId(formatoId));
        }

        public async Task<bool> Excluir(long formatoId)
        {
            var formato = await ObterPorId(formatoId);
            formato.Excluido = true;
            await Alterar(formato);
            return true;
        }
    }
}
