using Shouldly;
using SME.CDEP.Infra.Dominio.Enumerados;
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
            var respostaPrimeiroItem = resposta[0];
            respostaPrimeiroItem.Id.ShouldBe(TipoAcervo.Fotografico);
            respostaPrimeiroItem.Nome.ShouldBe(TipoAcervo.Fotografico.Descricao());
            respostaPrimeiroItem.Valor.ShouldBe(10);

            var respostaSegundoItem = resposta[1];
            respostaSegundoItem.Id.ShouldBe(TipoAcervo.DocumentacaoTextual);
            respostaSegundoItem.Nome.ShouldBe(TipoAcervo.DocumentacaoTextual.Descricao());
            respostaSegundoItem.Valor.ShouldBe(10);

            var respostaTerceiroItem = resposta[2];
            respostaTerceiroItem.Id.ShouldBe(TipoAcervo.Tridimensional);
            respostaTerceiroItem.Nome.ShouldBe(TipoAcervo.Tridimensional.Descricao());
            respostaTerceiroItem.Valor.ShouldBe(45);

            var respostaQuartoItem = resposta[3];
            respostaQuartoItem.Id.ShouldBe(TipoAcervo.Bibliografico);
            respostaQuartoItem.Nome.ShouldBe(TipoAcervo.Bibliografico.Descricao());
            respostaQuartoItem.Valor.ShouldBe(20);

            var respostaQuintoItem = resposta[4];
            respostaQuintoItem.Id.ShouldBe(TipoAcervo.Audiovisual);
            respostaQuintoItem.Nome.ShouldBe(TipoAcervo.Audiovisual.Descricao());
            respostaQuintoItem.Valor.ShouldBe(10);

            var respostaSextoItem = resposta[5];
            respostaSextoItem.Id.ShouldBe(TipoAcervo.ArtesGraficas);
            respostaSextoItem.Nome.ShouldBe(TipoAcervo.ArtesGraficas.Descricao());
            respostaSextoItem.Valor.ShouldBe(10);
        }
    }
}
