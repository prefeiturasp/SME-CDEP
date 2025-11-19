using Shouldly;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.Infra.Dominio.Enumerados;
using Xunit;

namespace SME.CDEP.TesteIntegracao.Eventos
{
    public class Ao_gerar_evento_fixos_moveis : TesteBase
    {
        public Ao_gerar_evento_fixos_moveis(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Evento Fixos - Inserir")]
        public async Task Inserir_eventos_fixos()
        {
            var servicoEvento = GetServicoEvento();

            await InserirNaBase(new EventoFixo()
            {
                Data = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                Descricao = "Confraternização universal",
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema", CriadoPor = "Sistema"
            });

            await InserirNaBase(new EventoFixo()
            {
                Data = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 25),
                Descricao = "Aniversário de São Paulo",
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema", CriadoPor = "Sistema"
            });
            
            await InserirNaBase(new EventoFixo()
            {
                Data = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 21),
                Descricao = "Tiradentes",
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema", CriadoPor = "Sistema"
            });
            
            await servicoEvento.GerarEventosFixos();

            var eventosFixos = ObterTodos<EventoFixo>();
            eventosFixos.Count().ShouldBe(3);
            eventosFixos.Any(a=> a.Data == new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01)).ShouldBeTrue();
            eventosFixos.Any(a=> a.Data == new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 25)).ShouldBeTrue();
            eventosFixos.Any(a=> a.Data == new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 21)).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Evento Móveis - Inserir")]
        public async Task Inserir_eventos_moveis()
        {
            var servicoEvento = GetServicoEvento();

            await servicoEvento.GerarEventosMoveis();
        }
    }
}