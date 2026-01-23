using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoEvento(IRepositorioEvento repositorioEvento, IMapper mapper, IRepositorioEventoFixo repositorioEventoFixo,
        IRepositorioAcervoSolicitacaoItem repositorioAcervoSolicitacaoItem, IServicoMensageria servicoMensageria,
        IServicoAcervo servicoAcervo) : IServicoEvento
    {
        public async Task<long> Inserir(EventoCadastroDTO eventoCadastroDto)
        {
            var evento = mapper.Map<Evento>(eventoCadastroDto);

            ValidarVisitaEmFinaisDeSemana(eventoCadastroDto.Tipo.EhVisita(), evento.Data.FimDeSemana());

            evento.Descricao = eventoCadastroDto.Tipo.EhVisita() 
                ? await ObterDetalhesDoAcervo(evento) 
                : eventoCadastroDto.Tipo.EhFeriado() ? eventoCadastroDto.Descricao : "Suspensão";

            await Validar(evento);
            
            return await repositorioEvento.Inserir(evento);
        }

        private void ValidarVisitaEmFinaisDeSemana(bool ehVisita, bool ehFinalDeSemana)
        {
            if (ehVisita && ehFinalDeSemana)
                throw new NegocioException(MensagemNegocio.NAO_EH_PERMITIDO_AGENDAR_VISITA_NO_FINAL_DE_SEMANA);
        }

        private async Task<string> ObterDetalhesDoAcervo(Evento evento)
        {
            if (evento.AcervoSolicitacaoItemId.HasValue)
            {
                var acervo = await repositorioAcervoSolicitacaoItem.ObterAcervoPorAcervoSolicitacaoItemId(evento.AcervoSolicitacaoItemId.Value);

                if (acervo is not null)
                {
                    var codigoTombo = acervo.Codigo.EstaPreenchido() && acervo.CodigoNovo.EstaPreenchido() ? $"{acervo.Codigo}/{acervo.CodigoNovo}"
                        : acervo.Codigo.EstaPreenchido() ? acervo.Codigo : acervo.CodigoNovo;

                    return $"Visita agendada ao acervo de tombo/código: {codigoTombo}";
                }
            }

            return "Visita agendada";
        }

        private async Task Validar(Evento evento)
        {
            if (evento.Justificativa.NaoEstaPreenchido() && evento.Tipo.EhSuspensao())
                throw new NegocioException(MensagemNegocio.JUSTIFICATIVA_NAO_INFORMADA);  
            
            if (await repositorioEvento.ExisteFeriadoOuSuspensaoNoDia(evento.Data, evento.Id))
                throw new NegocioException(MensagemNegocio.EXISTE_SUSPENSAO_OU_FERIADO_NESSE_DIA);
        }

        public async Task<IEnumerable<EventoDTO>> ObterTodos()
        {
            var eventos = await repositorioEvento.ObterTodos();

            if (eventos.NaoPossuiElementos())
                return default!;
            
            return eventos.Select(s=> mapper.Map<EventoDTO>(s));
        }

        public async Task<EventoDTO> Alterar(EventoCadastroDTO eventoCadastroDto)
        {
            var eventoAtual = await repositorioEvento.ObterPorId(eventoCadastroDto.Id.Value);
           
            if (eventoAtual.EhNulo())
                throw new NegocioException(MensagemNegocio.EVENTO_NAO_ENCONTRADO);
            
            ValidarVisitaEmFinaisDeSemana(eventoCadastroDto.Tipo.EhVisita(), eventoAtual.Data.FimDeSemana());
            
            var evento = mapper.Map<Evento>(eventoCadastroDto);
            evento.CriadoEm = eventoAtual.CriadoEm;
            evento.CriadoPor = eventoAtual.CriadoPor;
            evento.CriadoLogin = eventoAtual.CriadoLogin;
            
            return await ValidarEAtualizar(evento);
        }

        private async Task<EventoDTO> ValidarEAtualizar(Evento evento)
        {
            await Validar(evento);

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
                return default!;
         
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

            var primeiroDiaMes = new DateTime(ano, mes, 1, 0, 0, 0, DateTimeKind.Local);
            var ultimoDiaMes = primeiroDiaMes.AddMonths(1).AddDays(-1);

            var tiposAcervosPermitidos = servicoAcervo.ObterTiposAcervosPermitidosDoPerfilLogado();
            
            var eventosTag = await repositorioEvento.ObterEventosTagPorMesAno(mes, ano,tiposAcervosPermitidos);
            
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

                    var data = new DateTime(ano, primeiroDiaSemana.Month, primeiroDiaSemana.Day, 0, 0, 0, DateTimeKind.Local);

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
                return default!;
            
            return mapper.Map<IEnumerable<EventoTagDTO>>(eventosTag.Where(w=> w.Data.Date == data.Date));
        }

        public async Task<IEnumerable<EventoDetalheDTO>> ObterDetalhesDoDiaPorDiaMes(DiaMesDTO diaMesDto)
        {
            var tiposAcervosPermitidos = servicoAcervo.ObterTiposAcervosPermitidosDoPerfilLogado();
            return mapper.Map<IEnumerable<EventoDetalheDTO>>(await repositorioEvento.ObterDetalhesDoDiaPorData(diaMesDto.Data, tiposAcervosPermitidos));
        }

        public async Task InserirEventoVisita(DateTime dataVisita, long atendimentoItemId)
        {
            await Inserir(new EventoCadastroDTO(dataVisita, TipoEvento.VISITA, TipoEvento.VISITA.Descricao(),atendimentoItemId));
        }

        public async Task AtualizarEventoVisita(DateTime dataVisita, long atendimentoItemId)
        {
            var evento = await repositorioEvento.ObterPorAtendimentoItemId(atendimentoItemId);

            if (evento.EhNulo())
                await InserirEventoVisita(dataVisita, atendimentoItemId);
            else
            {
                evento.Data = dataVisita;
                
                await ValidarEAtualizar(evento);
            }
        }

        public async Task ExcluirEventoPorAcervoSolicitacaoItem(long atendimentoItemId)
        {
            var evento = await repositorioEvento.ObterPorAtendimentoItemId(atendimentoItemId);

            if (evento.EhNulo())
                return;

            await repositorioEvento.Remover(evento.Id);
        }

        public async Task GerarEventosFixos()
        {
            var eventosFixos = await repositorioEventoFixo.ObterTodos();

            foreach (var eventoFixo in eventosFixos)
                await servicoMensageria.Publicar(RotasRabbit.ExecutarCriacaoDeEventosTipoFeriadoAnoAtualPorData, new EventoCadastroDTO(eventoFixo.Data, eventoFixo.Tipo, eventoFixo.Descricao), Guid.NewGuid(), null);
        }

        public async Task GerarEventosMoveis()
        {
            var pascoa = CalcularPascoa(DateTimeExtension.HorarioBrasilia().Year);
            var carnaval = pascoa.AddDays(-47);
            var sextaFeiraSanta = pascoa.AddDays(-2);
            var corpusChristi = pascoa.AddDays(60);

            var eventosMoveis = new List<EventoCadastroDTO>()
            {
                new (pascoa, TipoEvento.FERIADO, "Páscoa"),
                new (carnaval, TipoEvento.FERIADO, "Carnaval"),
                new (sextaFeiraSanta, TipoEvento.FERIADO, "Sexta-feira Santa"),
                new (corpusChristi, TipoEvento.FERIADO, "Corpus Christi")
            };
            
            foreach (var eventoMovel in eventosMoveis)
                await servicoMensageria.Publicar(RotasRabbit.ExecutarCriacaoDeEventosTipoFeriadoAnoAtualPorData, eventoMovel, Guid.NewGuid(), null);
        }
        public async Task ValidarConflitosAsync(IEnumerable<DateTime> datasDasVisitas)
        {
            if (datasDasVisitas is null || !datasDasVisitas.Any()) return;

            var eventosConflitantes = await repositorioEvento.ObterEventosDeFeriadoESuspensaoPorDatas([.. datasDasVisitas]);

            if (eventosConflitantes.Any())
            {
                var datasFormatadas = string.Join(',', eventosConflitantes.Select(s => s.ToString("dd/MM")));
                throw new NegocioException(string.Format(MensagemNegocio.DATAS_DE_VISITAS_CONFLITANTES, datasFormatadas));
            }
        }

        private static DateTime CalcularPascoa(int ano)
        {
            int r1 = ano % 19;
            int r2 = ano % 4;
            int r3 = ano % 7;
            int r4 = (19 * r1 + 24) % 30;
            int r5 = (6 * r4 + 4 * r3 + 2 * r2 + 5) % 7;
            var dataPascoa = new DateTime(ano, 3, 22, 0, 0, 0, DateTimeKind.Local).AddDays(r4 + r5);
            int dia = dataPascoa.Day;
            switch (dia)
            {
                case 26:
                    dataPascoa = new DateTime(ano, 4, 19, 0, 0, 0, DateTimeKind.Local);
                    break;

                case 25:
                    if (r1 > 10)
                        dataPascoa = new DateTime(ano, 4, 18, 0, 0, 0, DateTimeKind.Local);
                    break;
            }
            return dataPascoa.Date;
        }
    }
}
