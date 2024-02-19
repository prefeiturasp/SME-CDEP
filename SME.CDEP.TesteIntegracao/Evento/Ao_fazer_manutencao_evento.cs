using Shouldly;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.Infra.Dominio.Enumerados;
using Xunit;

namespace SME.CDEP.TesteIntegracao.Usuario
{
    public class Ao_fazer_manutencao_evento : TesteBase
    {
        public Ao_fazer_manutencao_evento(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Evento - Inserir")]
        public async Task Inserir()
        {
            var servicoEvento = GetServicoEvento();
            var eventoDto = EventoDTOMock.GerarEventoDTO(TipoEvento.VISITA).Generate(); 

            var eventoId = await servicoEvento.Inserir(eventoDto);
            eventoId.ShouldBeGreaterThan(0);
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
            evento.Data = DateTimeExtension.HorarioBrasilia().Date;
            
            evento = await servicoEvento.Alterar(evento);
            evento.ShouldNotBeNull();
            evento.Data.ShouldBe(evento.Data);
        }

        private async Task<IServicoEvento> CadastrarVarios(TipoEvento tipoEvento)
        {
            var servicoEvento = GetServicoEvento();

            var eventoDtos = EventoDTOMock.GerarEventoDTO(tipoEvento).Generate(10);

            var contador = 1;

            foreach (var eventoDto in eventoDtos)
            {
                var eventoId = await servicoEvento.Inserir(eventoDto);
                eventoId.ShouldBeGreaterThan(0);
            }

            return servicoEvento;
        }
    }
}