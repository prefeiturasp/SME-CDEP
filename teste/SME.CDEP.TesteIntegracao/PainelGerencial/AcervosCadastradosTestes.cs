using Shouldly;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao.PainelGerencial
{
    public class AcervosCadastradosTestes(CollectionFixture collectionFixture) : TesteBase(collectionFixture)
    {
        [Fact(DisplayName = "Obter Acervos Cadastrados")]
        public async Task AoObterAcervosCadastradosDeveRetornarDadosCorretamente()
        {
            // Arrange
            await InserirDadosBasicosAleatorios();
            await InserirAcervoTridimensional();
            await InserirAcervosBibliograficos();
            await InserirAcervos();
            var servicoPainelGerencial = GetServicoPainelGerencial();

            // Act
            var resposta = await servicoPainelGerencial.ObterAcervosCadastradosAsync();

            // Assert
            resposta.Count.ShouldBe(6);
        }
    }
}