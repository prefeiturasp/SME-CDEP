using FluentAssertions;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Dominio.Extensions;

namespace SME.CDEP.TesteUnitario.Domain.Extensions
{
    public class EnumExtensionTestes
    {
        [Theory]
        [InlineData(TipoAcervo.DocumentacaoTextual, false)]
        [InlineData(TipoAcervo.Bibliografico, true)]
        [InlineData(TipoAcervo.Audiovisual, true)]
        public void DadoTipoAcervo_QuandoNaoEhAcervoDocumental_EntaoRetornaResultadoEsperado(TipoAcervo tipoAcervo, bool esperado)
        {
            // Arrange & Act
            var resultado = tipoAcervo.NaoEhAcervoDocumental();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData(TipoAcervo.Bibliografico, true)]
        [InlineData(TipoAcervo.DocumentacaoTextual, false)]
        [InlineData(TipoAcervo.Fotografico, false)]
        public void DadoTipoAcervo_QuandoEhAcervoBibliografico_EntaoRetornaResultadoEsperado(TipoAcervo tipoAcervo, bool esperado)
        {
            // Arrange & Act
            var resultado = tipoAcervo.EhAcervoBibliografico();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData(TipoAcervo.Bibliografico, false)]
        [InlineData(TipoAcervo.DocumentacaoTextual, true)]
        [InlineData(TipoAcervo.Tridimensional, true)]
        public void DadoTipoAcervo_QuandoNaoEhAcervoBibliografico_EntaoRetornaResultadoEsperado(TipoAcervo tipoAcervo, bool esperado)
        {
            // Arrange & Act
            var resultado = tipoAcervo.NaoEhAcervoBibliografico();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData(TipoAcervo.Bibliografico, Constantes.PLANILHA_ACERVO_BIBLIOGRAFICO)]
        [InlineData(TipoAcervo.DocumentacaoTextual, Constantes.PLANILHA_ACERVO_DOCUMENTAL)]
        [InlineData(TipoAcervo.ArtesGraficas, Constantes.PLANILHA_ACERVO_ARTE_GRAFICA)]
        [InlineData(TipoAcervo.Audiovisual, Constantes.PLANILHA_ACERVO_AUDIOVISUAL)]
        [InlineData(TipoAcervo.Fotografico, Constantes.PLANILHA_ACERVO_FOTOGRAFICO)]
        [InlineData(TipoAcervo.Tridimensional, Constantes.PLANILHA_ACERVO_TRIDIMENSIONAL)]
        public void DadoTipoAcervoValido_QuandoObterPlanilhaModelo_EntaoRetornaNomeDaPlanilhaConstante(TipoAcervo tipoAcervo, string planilhaEsperada)
        {
            // Arrange & Act
            var resultado = tipoAcervo.ObterPlanilhaModelo();

            // Assert
            resultado.Should().Be(planilhaEsperada);
        }

        [Fact]
        public void DadoTipoAcervoInvalido_QuandoObterPlanilhaModelo_EntaoLancaArgumentOutOfRangeException()
        {
            // Arrange
            var tipoAcervoInvalido = (TipoAcervo)999;

            // Act
            Action acao = () => tipoAcervoInvalido.ObterPlanilhaModelo();

            // Assert
            acao.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("tipoAcervo");
        }
    }
}
