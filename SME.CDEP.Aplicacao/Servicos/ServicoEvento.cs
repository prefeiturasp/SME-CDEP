using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

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
            if (await repositorioEvento.ExisteFeriadoOuSuspensaoNoDia(eventoDto.Data))
                throw new NegocioException(MensagemNegocio.EXISTE_SUSPENSAO_OU_FERIADO_NESSE_DIA);
            
            var evento = mapper.Map<Evento>(eventoDto);
            
            evento.Validar();
            
            return await repositorioEvento.Inserir(evento);
        }

        public async Task<IEnumerable<EventoDTO>> ObterTodos()
        {
            return (await repositorioEvento.ObterTodos()).ToList().Select(s=> mapper.Map<EventoDTO>(s));
        }

        public async Task<EventoDTO> Alterar(EventoDTO eventoDto)
        {
            var eventoAtual = await repositorioEvento.ObterPorId(eventoDto.Id);
            
            var evento = mapper.Map<Evento>(eventoDto);
            evento.CriadoEm = eventoAtual.CriadoEm;
            evento.CriadoPor = eventoAtual.CriadoPor;
            evento.CriadoLogin = eventoAtual.CriadoLogin;
            
            evento.Validar();
            
            return mapper.Map<EventoDTO>(await repositorioEvento.Atualizar(evento));
        }

        public async Task<EventoDTO> ObterPorId(long eventoId)
        {
            return mapper.Map<EventoDTO>(await repositorioEvento.ObterPorId(eventoId));
        }
    }
}
