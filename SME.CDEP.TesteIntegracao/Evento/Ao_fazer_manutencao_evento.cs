using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.Infra.Dominio.Enumerados;
using Xunit;

namespace SME.CDEP.TesteIntegracao.Eventos
{
    public class Ao_fazer_manutencao_evento : TesteBase
    {
        public Ao_fazer_manutencao_evento(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Evento - Inserir")]
        public async Task Inserir()
        {
            var servicoEvento = GetServicoEvento();
            var eventoDto = EventoCadastroDTO.GerarEventoDTO(TipoEvento.VISITA).Generate(); 

            var eventoId = await servicoEvento.Inserir(eventoDto);
            eventoId.ShouldBeGreaterThan(0);
            
            var retorno = ObterTodos<Dominio.Entidades.Evento>().LastOrDefault();
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(eventoId);
            retorno.Data.ShouldBe(eventoDto.Data);
            retorno.Descricao.ShouldBe(eventoDto.Descricao);
            retorno.Tipo.ShouldBe(eventoDto.Tipo);
            retorno.Justificativa.ShouldBe(eventoDto.Justificativa);
        }

        [Fact(DisplayName = "Evento - Obter todos")]
        public async Task ObterTodosEventos()
        {
            IServicoEvento servicoEvento = await CadastrarVarios(TipoEvento.VISITA);

            var eventoDtos = await servicoEvento.ObterTodos();
            eventoDtos.ShouldNotBeNull();
            eventoDtos.Count().ShouldBe(10);
        }

        [Fact(DisplayName = "Evento - Obter por id")]
        public async Task ObterEventoPorId()
        {
            IServicoEvento servicoEvento = await CadastrarVarios(TipoEvento.SUSPENSAO);

            var usuario = await servicoEvento.ObterPorId(1);
            usuario.ShouldNotBeNull();
            usuario.Id.ShouldBe(1);
        }

        [Fact(DisplayName = "Evento - Atualizar")]
        public async Task Atualizar()
        {
            CriarClaimUsuario();
            IServicoEvento servicoEvento = await CadastrarVarios(TipoEvento.FERIADO);

            var evento = await servicoEvento.ObterPorId(1);

            var data = DateTimeExtension.HorarioBrasilia().AddDays(35);
            
            var eventoAlteracao = new Aplicacao.DTOS.EventoCadastroDTO()
            {
                Id = evento.Id,
                Dia = data.Day,
                Mes = data.Month,
                Descricao = evento.Descricao.Limite(50) + "_alterado",
                Tipo = evento.Tipo,
                Justificativa = evento.Justificativa.Limite(50) + "_alterado"
            };
            
            var retorno = await servicoEvento.Alterar(eventoAlteracao);
            retorno.ShouldNotBeNull();
            eventoAlteracao.Id.ShouldNotBeNull();
            retorno.Id.ShouldBe(eventoAlteracao.Id.Value);
            retorno.Data.ShouldBe(eventoAlteracao.Data);
            retorno.Descricao.ShouldBe(eventoAlteracao.Descricao);
            retorno.Tipo.ShouldBe(eventoAlteracao.Tipo);
            retorno.Justificativa.ShouldBe(eventoAlteracao.Justificativa);

            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(10);
        }
        
        [Fact(DisplayName = "Evento - Obter Eventos tag")]
        public async Task Obter_eventos_tag()
        {
            CriarClaimUsuario();
            IServicoEvento servicoEvento = await CadastrarVarios(TipoEvento.FERIADO);

            var eventos = ObterTodos<Dominio.Entidades.Evento>();

            var diaMes = new DiaMesDTO()
            {
                Dia = eventos.FirstOrDefault().Data.Day,
                Mes = eventos.FirstOrDefault().Data.Month
            };

            var eventosTag = await servicoEvento.ObterEventosTagPorData(diaMes);
            eventosTag.ShouldNotBeNull();
            eventosTag.All(a=> a.TipoId.EstaPreenchido()).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Evento - Excluir logicamente")]
        public async Task Excluir()
        {
            CriarClaimUsuario();
            IServicoEvento servicoEvento = await CadastrarVarios(TipoEvento.FERIADO);

            var retorno = await servicoEvento.ExcluirLogicamente(1);
            retorno.ShouldBeTrue();

            var eventos = ObterTodos<Evento>();
            
            var primeiroEvento = eventos.FirstOrDefault(f => f.Id == 1);
            primeiroEvento.Excluido.ShouldBeTrue();
            
            var segundoEvento = eventos.FirstOrDefault(f => f.Id == 2);
            segundoEvento.Excluido.ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Evento - Obter calendário de eventos do mês de janeiro de 2024")]
        public async Task Calendario_de_eventos_do_mes_janeiro_2024()
        {
            var servicoEvento = GetServicoEvento();

            var retorno = await servicoEvento.ObterCalendarioDeEventosPorMes(1, 2024);
            retorno.ShouldNotBeNull();
            
            var diasPrimeiraSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 1);
            diasPrimeiraSemana.Dias.FirstOrDefault().Dia.ShouldBe(31);
            diasPrimeiraSemana.Dias.LastOrDefault().Dia.ShouldBe(6);
            
            var diasSegundaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 2);
            diasSegundaSemana.Dias.FirstOrDefault().Dia.ShouldBe(7);
            diasSegundaSemana.Dias.LastOrDefault().Dia.ShouldBe(13);
            
            var diasTerceiraSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 3);
            diasTerceiraSemana.Dias.FirstOrDefault().Dia.ShouldBe(14);
            diasTerceiraSemana.Dias.LastOrDefault().Dia.ShouldBe(20);
            
            var diasQuartaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 4);
            diasQuartaSemana.Dias.FirstOrDefault().Dia.ShouldBe(21);
            diasQuartaSemana.Dias.LastOrDefault().Dia.ShouldBe(27);
            
            var diasQuintaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 5);
            diasQuintaSemana.Dias.FirstOrDefault().Dia.ShouldBe(28);
            diasQuintaSemana.Dias.LastOrDefault().Dia.ShouldBe(03);
        }
        
        [Fact(DisplayName = "Evento - Obter calendário de eventos do mês de fevereiro de 2024")]
        public async Task Calendario_de_eventos_do_mes_fevereiro_2024()
        {
            var servicoEvento = GetServicoEvento();

            var retorno = await servicoEvento.ObterCalendarioDeEventosPorMes(2, 2024);
            retorno.ShouldNotBeNull();
            
            var diasPrimeiraSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 1);
            diasPrimeiraSemana.Dias.FirstOrDefault().Dia.ShouldBe(28);
            diasPrimeiraSemana.Dias.LastOrDefault().Dia.ShouldBe(3);
            
            var diasSegundaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 2);
            diasSegundaSemana.Dias.FirstOrDefault().Dia.ShouldBe(4);
            diasSegundaSemana.Dias.LastOrDefault().Dia.ShouldBe(10);
            
            var diasTerceiraSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 3);
            diasTerceiraSemana.Dias.FirstOrDefault().Dia.ShouldBe(11);
            diasTerceiraSemana.Dias.LastOrDefault().Dia.ShouldBe(17);
            
            var diasQuartaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 4);
            diasQuartaSemana.Dias.FirstOrDefault().Dia.ShouldBe(18);
            diasQuartaSemana.Dias.LastOrDefault().Dia.ShouldBe(24);
            
            var diasQuintaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 5);
            diasQuintaSemana.Dias.FirstOrDefault().Dia.ShouldBe(25);
            diasQuintaSemana.Dias.LastOrDefault().Dia.ShouldBe(2);
        }
        
        [Fact(DisplayName = "Evento - Obter calendário de eventos do mês de setembro de 2024")]
        public async Task Calendario_de_eventos_do_mes_setembro_2024()
        {
            var servicoEvento = GetServicoEvento();

            var retorno = await servicoEvento.ObterCalendarioDeEventosPorMes(9, 2024);
            retorno.ShouldNotBeNull();
            
            var diasPrimeiraSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 1);
            diasPrimeiraSemana.Dias.FirstOrDefault().Dia.ShouldBe(1);
            diasPrimeiraSemana.Dias.LastOrDefault().Dia.ShouldBe(7);
            
            var diasSegundaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 2);
            diasSegundaSemana.Dias.FirstOrDefault().Dia.ShouldBe(8);
            diasSegundaSemana.Dias.LastOrDefault().Dia.ShouldBe(14);
            
            var diasTerceiraSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 3);
            diasTerceiraSemana.Dias.FirstOrDefault().Dia.ShouldBe(15);
            diasTerceiraSemana.Dias.LastOrDefault().Dia.ShouldBe(21);
            
            var diasQuartaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 4);
            diasQuartaSemana.Dias.FirstOrDefault().Dia.ShouldBe(22);
            diasQuartaSemana.Dias.LastOrDefault().Dia.ShouldBe(28);
            
            var diasQuintaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 5);
            diasQuintaSemana.Dias.FirstOrDefault().Dia.ShouldBe(29);
            diasQuintaSemana.Dias.LastOrDefault().Dia.ShouldBe(5);
        }
        
        [Fact(DisplayName = "Evento - Obter calendário de eventos do mês de agosto de 2025")]
        public async Task Calendario_de_eventos_do_mes_agosto_2025()
        {
            var servicoEvento = GetServicoEvento();

            var retorno = await servicoEvento.ObterCalendarioDeEventosPorMes(8, 2025);
            retorno.ShouldNotBeNull();
            
            var diasPrimeiraSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 1);
            diasPrimeiraSemana.Dias.FirstOrDefault().Dia.ShouldBe(27);
            diasPrimeiraSemana.Dias.LastOrDefault().Dia.ShouldBe(2);
            
            var diasSegundaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 2);
            diasSegundaSemana.Dias.FirstOrDefault().Dia.ShouldBe(3);
            diasSegundaSemana.Dias.LastOrDefault().Dia.ShouldBe(9);
            
            var diasTerceiraSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 3);
            diasTerceiraSemana.Dias.FirstOrDefault().Dia.ShouldBe(10);
            diasTerceiraSemana.Dias.LastOrDefault().Dia.ShouldBe(16);
            
            var diasQuartaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 4);
            diasQuartaSemana.Dias.FirstOrDefault().Dia.ShouldBe(17);
            diasQuartaSemana.Dias.LastOrDefault().Dia.ShouldBe(23);
            
            var diasQuintaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 5);
            diasQuintaSemana.Dias.FirstOrDefault().Dia.ShouldBe(24);
            diasQuintaSemana.Dias.LastOrDefault().Dia.ShouldBe(30);
            
            var diasSextaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 6);
            diasSextaSemana.Dias.FirstOrDefault().Dia.ShouldBe(31);
            diasSextaSemana.Dias.LastOrDefault().Dia.ShouldBe(6);
        }
        
        [Fact(DisplayName = "Evento - Obter calendário de eventos do mês de fevereiro de 2025")]
        public async Task Calendario_de_eventos_do_mes_fevereiro_2025()
        {
            var servicoEvento = GetServicoEvento();

            var retorno = await servicoEvento.ObterCalendarioDeEventosPorMes(2, 2025);
            retorno.ShouldNotBeNull();
            
            var diasPrimeiraSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 1);
            diasPrimeiraSemana.Dias.FirstOrDefault().Dia.ShouldBe(26);
            diasPrimeiraSemana.Dias.LastOrDefault().Dia.ShouldBe(1);
            
            var diasSegundaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 2);
            diasSegundaSemana.Dias.FirstOrDefault().Dia.ShouldBe(2);
            diasSegundaSemana.Dias.LastOrDefault().Dia.ShouldBe(8);
            
            var diasTerceiraSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 3);
            diasTerceiraSemana.Dias.FirstOrDefault().Dia.ShouldBe(9);
            diasTerceiraSemana.Dias.LastOrDefault().Dia.ShouldBe(15);
            
            var diasQuartaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 4);
            diasQuartaSemana.Dias.FirstOrDefault().Dia.ShouldBe(16);
            diasQuartaSemana.Dias.LastOrDefault().Dia.ShouldBe(22);
            
            var diasQuintaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 5);
            diasQuintaSemana.Dias.FirstOrDefault().Dia.ShouldBe(23);
            diasQuintaSemana.Dias.LastOrDefault().Dia.ShouldBe(1);
        }
        
        [Fact(DisplayName = "Evento - Obter calendário de eventos do mês de junho de 2025")]
        public async Task Calendario_de_eventos_do_mes_junho_2025()
        {
            var servicoEvento = GetServicoEvento();

            var retorno = await servicoEvento.ObterCalendarioDeEventosPorMes(6, 2025);
            retorno.ShouldNotBeNull();
            
            var diasPrimeiraSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 1);
            diasPrimeiraSemana.Dias.FirstOrDefault().Dia.ShouldBe(1);
            diasPrimeiraSemana.Dias.LastOrDefault().Dia.ShouldBe(7);
            
            var diasSegundaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 2);
            diasSegundaSemana.Dias.FirstOrDefault().Dia.ShouldBe(8);
            diasSegundaSemana.Dias.LastOrDefault().Dia.ShouldBe(14);
            
            var diasTerceiraSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 3);
            diasTerceiraSemana.Dias.FirstOrDefault().Dia.ShouldBe(15);
            diasTerceiraSemana.Dias.LastOrDefault().Dia.ShouldBe(21);
            
            var diasQuartaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 4);
            diasQuartaSemana.Dias.FirstOrDefault().Dia.ShouldBe(22);
            diasQuartaSemana.Dias.LastOrDefault().Dia.ShouldBe(28);
            
            var diasQuintaSemana = retorno.Semanas.FirstOrDefault(a => a.Numero == 5);
            diasQuintaSemana.Dias.FirstOrDefault().Dia.ShouldBe(29);
            diasQuintaSemana.Dias.LastOrDefault().Dia.ShouldBe(5);
        }

        private async Task<IServicoEvento> CadastrarVarios(TipoEvento tipoEvento)
        {
            var servicoEvento = GetServicoEvento();

            var eventos = EventoMock.Instance.GerarEvento(tipoEvento).Generate(10);

            var contador = 1;

            foreach (var eventoDto in eventos)
            {
                eventoDto.Data.AddDays(contador);
                await InserirNaBase(eventoDto);
            }

            return servicoEvento;
        }
    }
}