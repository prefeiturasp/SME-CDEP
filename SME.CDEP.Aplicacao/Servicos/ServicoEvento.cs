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

        public async Task<long> Inserir(EventoCadastroDTO eventoCadastroDto)
        {
            var evento = mapper.Map<Evento>(eventoCadastroDto);
            
            await Validar(eventoCadastroDto);
            
            return await repositorioEvento.Inserir(evento);
        }
        
        private async Task Validar(EventoCadastroDTO eventoCadastroDto)
        {
            if (eventoCadastroDto.Justificativa.NaoEstaPreenchido() && eventoCadastroDto.Tipo.EhSuspensao())
                throw new NegocioException(MensagemNegocio.JUSTIFICATIVA_NAO_INFORMADA);  
            
            if (await repositorioEvento.ExisteFeriadoOuSuspensaoNoDia(eventoCadastroDto.Data))
                throw new NegocioException(MensagemNegocio.EXISTE_SUSPENSAO_OU_FERIADO_NESSE_DIA);
        }

        public async Task<IEnumerable<EventoDTO>> ObterTodos()
        {
            var eventos = await repositorioEvento.ObterTodos();

            if (eventos.NaoPossuiElementos())
                return default;
            
            return eventos.Select(s=> mapper.Map<EventoDTO>(s));
        }

        public async Task<EventoDTO> Alterar(EventoCadastroDTO eventoCadastroDto)
        {
            var eventoAtual = await repositorioEvento.ObterPorId(eventoCadastroDto.Id);
            
            var evento = mapper.Map<Evento>(eventoCadastroDto);
            evento.CriadoEm = eventoAtual.CriadoEm;
            evento.CriadoPor = eventoAtual.CriadoPor;
            evento.CriadoLogin = eventoAtual.CriadoLogin;
            
            await Validar(eventoCadastroDto);
            
            return mapper.Map<EventoDTO>(await repositorioEvento.Atualizar(evento));
        }

        public async Task<EventoDTO> ObterPorId(long eventoId)
        {
            var evento = await repositorioEvento.ObterPorId(eventoId);

            if (evento.EhNulo())
                return default;
            
            return mapper.Map<EventoDTO>(evento);
        }

        public async Task<IEnumerable<EventoTagDTO>> ObterEventosTagPorData(DiaMesDTO diaMesDto)
        {
            var eventosTag = await repositorioEvento.ObterEventosTagPorData(diaMesDto.Data);

            if (eventosTag.EhNulo())
                return default;
         
            return mapper.Map<IEnumerable<EventoTagDTO>>(eventosTag);
        }
    }
}
