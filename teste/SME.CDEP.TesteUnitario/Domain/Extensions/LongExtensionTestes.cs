using FluentAssertions;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Domain.Extensions
{
    public class LongExtensionTestes
    {
        [Theory]
        [InlineData((long)TipoAcervo.DocumentacaoTextual, true)]
        [InlineData((long)TipoAcervo.Bibliografico, false)]
        public void DadoValor_QuandoEhAcervoDocumental_EntaoRetornaResultadoEsperado(long valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.EhAcervoDocumental();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData((long)TipoAcervo.Tridimensional, true)]
        [InlineData((long)TipoAcervo.DocumentacaoTextual, false)]
        public void DadoValor_QuandoEhAcervoTridimensional_EntaoRetornaResultadoEsperado(long valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.EhAcervoTridimensional();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData((long)TipoAcervo.Bibliografico, true)]
        [InlineData((long)TipoAcervo.Audiovisual, false)]
        public void DadoValor_QuandoEhAcervoBibliografico_EntaoRetornaResultadoEsperado(long valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.EhAcervoBibliografico();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData((long)TipoAcervo.Audiovisual, true)]
        [InlineData((long)TipoAcervo.Bibliografico, false)]
        public void DadoValor_QuandoNaoEhAcervoBibliografico_EntaoRetornaResultadoEsperado(long valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.NaoEhAcervoBibliografico();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData((long)TipoAcervo.ArtesGraficas, true)]
        [InlineData((long)TipoAcervo.Fotografico, false)]
        public void DadoValor_QuandoEhAcervoArteGrafica_EntaoRetornaResultadoEsperado(long valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.EhAcervoArteGrafica();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData((long)TipoAcervo.Fotografico, true)]
        [InlineData((long)TipoAcervo.DocumentacaoTextual, false)]
        public void DadoValor_QuandoEhAcervoFotografico_EntaoRetornaResultadoEsperado(long valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.EhAcervoFotografico();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData((long)TipoAcervo.Audiovisual, true)]
        [InlineData((long)TipoAcervo.Tridimensional, false)]
        public void DadoValor_QuandoEhAcervoAudiovisual_EntaoRetornaResultadoEsperado(long valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.EhAcervoAudiovisual();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData((long)TipoAcervo.Bibliografico, true)]
        [InlineData((long)TipoAcervo.DocumentacaoTextual, false)]
        public void DadoValor_QuandoNaoEhAcervoDocumental_EntaoRetornaResultadoEsperado(long valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.NaoEhAcervoDocumental();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(100, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public void DadoValor_QuandoEhMaiorQueZero_EntaoRetornaResultadoEsperado(long valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.EhMaiorQueZero();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(-5, true)]
        [InlineData(1, false)]
        [InlineData(50, false)]
        public void DadoValor_QuandoEhMenorIgualQueZero_EntaoRetornaResultadoEsperado(long valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.EhMenorIgualQueZero();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("10", 10)]
        [InlineData("99999999", 99999999)]
        [InlineData("-50", -50)]
        public void DadoStringComValorNumerico_QuandoObterLongoOuNuloPorValorDoCampo_EntaoRetornaValorConvertido(string valorOriginal, long valorEsperado)
        {
            // Arrange & Act
            var resultado = valorOriginal.ObterLongoOuNuloPorValorDoCampo();

            // Assert
            resultado.Should().Be(valorEsperado);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void DadoStringVaziaOuNula_QuandoObterLongoOuNuloPorValorDoCampo_EntaoRetornaNulo(string? valorOriginal)
        {
            // Arrange & Act
            var resultado = valorOriginal!.ObterLongoOuNuloPorValorDoCampo();

            // Assert
            resultado.Should().BeNull();
        }

        [Theory]
        [InlineData("100", 100)]
        [InlineData("-50", -50)]
        public void DadoStringComValorNumerico_QuandoObterLongoPorValorDoCampo_EntaoRetornaValorConvertido(string valorOriginal, long valorEsperado)
        {
            // Arrange & Act
            var resultado = valorOriginal.ObterLongoPorValorDoCampo();

            // Assert
            resultado.Should().Be(valorEsperado);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("abc")]
        [InlineData("10.5")]
        public void DadoStringInvalida_QuandoObterLongoPorValorDoCampo_EntaoLancaNegocioException(string? valorOriginal)
        {
            // Arrange
            Action acao = () => valorOriginal!.ObterLongoPorValorDoCampo();

            // Act & Assert
            acao.Should().Throw<NegocioException>();
        }

        [Theory]
        [InlineData(10, 10, true)]
        [InlineData(10, 15, false)]
        [InlineData(0, 0, true)]
        [InlineData(-5, 5, false)]
        public void DadoValoresLongos_QuandoSaoIguais_EntaoRetornaResultadoEsperado(long valor1, long valor2, bool esperado)
        {
            // Arrange & Act
            var resultado = valor1.SaoIguais(valor2);

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData(10L, 10L, true)]
        [InlineData(10L, 15L, false)]
        [InlineData(null, null, true)]
        [InlineData(10L, null, false)]
        [InlineData(null, 10L, false)]
        public void DadoValoresLongosNulaveis_QuandoSaoIguais_EntaoRetornaResultadoEsperado(long? valor1, long? valor2, bool esperado)
        {
            // Arrange & Act
            var resultado = valor1.SaoIguais(valor2);

            // Assert
            resultado.Should().Be(esperado);
        }
    }
}