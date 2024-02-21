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
            
            if (await repositorioEvento.ExisteFeriadoOuSuspensaoNoDia(eventoCadastroDto.Data, eventoCadastroDto.Id))
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
            if (!eventoCadastroDto.Id.HasValue)
                throw new NegocioException(MensagemNegocio.EVENTO_NAO_ENCONTRADO);
            
            var eventoAtual = await repositorioEvento.ObterPorId(eventoCadastroDto.Id.Value);
            
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

        public async Task<bool> ExcluirLogicamente(long eventoId)
        {
            var evento = await repositorioEvento.ObterPorId(eventoId);

            if (evento.EhNulo())
                throw new NegocioException(MensagemNegocio.EVENTO_NAO_ENCONTRADO);

            evento.Excluido = true;
            await repositorioEvento.Remover(evento);

            return true;
        }

        public async Task<EventoDTO> ObterEventoPorId(int eventoId)
        {
            var evento = await repositorioEvento.ObterPorId(eventoId);

            if (evento.EhNulo())
                throw new NegocioException(MensagemNegocio.EVENTO_NAO_ENCONTRADO);

            return mapper.Map<EventoDTO>(evento);
        }

        public async Task<CalendarioEventoDTO> ObterCalendarioDeEventosPorMes(int mes, int ano)
        {
            var calendarioEvento = new CalendarioEventoDTO();
            
            var primeiroDiaMes = new DateTime(ano, mes, 1);
            var ultimoDiaMes = primeiroDiaMes.AddMonths(1).AddDays(-1);

            var eventosTag = await repositorioEvento.ObterEventosTagPorMesAno(mes, ano);
            
            var percorrerSemanasAte = ultimoDiaMes.ObterSabado();
            
            var primeiroDiaSemana = primeiroDiaMes.ObterDomingoRetroativo();
            var ultimoDiaSemana = primeiroDiaMes.ObterSabado();

            var podePercorrerSemana = true;
            var contadorSemanas = 1;
            
            while (podePercorrerSemana)
            {
                var dias = new List<DiaDTO>();
                
                while (primeiroDiaSemana <= ultimoDiaSemana)
                {
                    var desabilitado = primeiroDiaSemana.Month != primeiroDiaMes.Month;
                    
                    var data = new DateTime(ano, primeiroDiaSemana.Month, primeiroDiaSemana.Day);
                    
                    dias.Add(new DiaDTO()
                    {
                        Dia = primeiroDiaSemana.Day,
                        Desabilitado = desabilitado,
                        DayOfWeek =  (int)primeiroDiaSemana.DayOfWeek,
                        EventosTag = desabilitado 
                                     ? Enumerable.Empty<EventoTagDTO>() 
                                     : ObterEventosTag(eventosTag, data) 
                    });
                    primeiroDiaSemana = primeiroDiaSemana.AddDays(1);
                    
                }

                calendarioEvento.Semanas.Add(new SemanaDTO()
                {
                    Numero = contadorSemanas,
                    Dias = dias
                });
                contadorSemanas++;
                
                primeiroDiaSemana = primeiroDiaSemana.ObterDomingoRetroativo();
                ultimoDiaSemana = primeiroDiaSemana.ObterSabado();

                if (primeiroDiaSemana > percorrerSemanasAte)
                    podePercorrerSemana = false;
            }

            return calendarioEvento;
        }

        private IEnumerable<EventoTagDTO> ObterEventosTag(IEnumerable<Evento> eventosTag, DateTime data)
        {
            if (eventosTag.NaoPossuiElementos())
                return default;
            
            return mapper.Map<IEnumerable<EventoTagDTO>>(eventosTag.Where(w=> w.Data == data));
        }

        public Task<IEnumerable<EventoDetalheDTO>> ObterDetalhesDoDiaPorDiaMes(DiaMesDTO diaMesDto)
        {
            throw new NotImplementedException();
        }
    }
}
