using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoEvento : IServicoEvento
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IMapper mapper;
        
        public ServicoEvento(IRepositorioEvento repositorioEvento,IMapper mapper) 
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<long> Inserir(EventoDTO eventoDto)
        {
            var usuario = mapper.Map<Evento>(eventoDto);
            return await repositorioEvento.Inserir(usuario);
        }

        public async Task<IEnumerable<EventoDTO>> ObterTodos()
        {
            return (await repositorioEvento.ObterTodos()).ToList().Select(s=> mapper.Map<EventoDTO>(s));
        }

        public async Task<EventoDTO> Alterar(EventoDTO eventoDto)
        {
            var evento = mapper.Map<Evento>(eventoDto);
            return mapper.Map<EventoDTO>(await repositorioEvento.Atualizar(evento));
        }

        public async Task<EventoDTO> ObterPorId(long eventoId)
        {
            return mapper.Map<EventoDTO>(await repositorioEvento.ObterPorId(eventoId));
        }
    }
}
