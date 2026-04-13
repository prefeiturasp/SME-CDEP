using FluentAssertions;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;

namespace SME.CDEP.TesteUnitario.Domain.Extensions
{
    public class DoubleExtensionTestes
    {
        [Theory]
        [InlineData(10.5)]
        [InlineData(100)]
        [InlineData(0)]
        [InlineData(-5.5)]
        public void DadoStringComValorNumerico_QuandoObterDoubleOuNuloPorValorDoCampo_EntaoRetornaValorConvertido(double valorEsperado)
        {
            // Arrange
            var valorFormatadoParaParse = valorEsperado.ToString();

            // Act
            var resultado = valorFormatadoParaParse.ObterDoubleOuNuloPorValorDoCampo();

            // Assert
            resultado.Should().Be(valorEsperado);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void DadoStringVaziaOuNula_QuandoObterDoubleOuNuloPorValorDoCampo_EntaoRetornaNulo(string? valorOriginal)
        {
            // Arrange & Act
            var resultado = valorOriginal!.ObterDoubleOuNuloPorValorDoCampo();

            // Assert
            resultado.Should().BeNull();
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("True", true)]
        [InlineData("False", false)]
        public void DadoStringComValorBooleano_QuandoObterBooleanoPorValorDoCampo_EntaoRetornaValorConvertido(string valorOriginal, bool valorEsperado)
        {
            // Arrange & Act
            var resultado = valorOriginal.ObterBooleanoPorValorDoCampo();

            // Assert
            resultado.Should().Be(valorEsperado);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void DadoStringVaziaOuNula_QuandoObterBooleanoPorValorDoCampo_EntaoRetornaFalso(string? valorOriginal)
        {
            // Arrange & Act
            var resultado = valorOriginal!.ObterBooleanoPorValorDoCampo();

            // Assert
            resultado.Should().BeFalse();
        }

        [Theory]
        [InlineData(10.5)]
        [InlineData(100)]
        public void DadoStringComValorNumerico_QuandoObterDoublePorValorDoCampo_EntaoRetornaValorConvertido(double valorEsperado)
        {
            // Arrange
            var valorFormatadoParaParse = valorEsperado.ToString();

            // Act
            var resultado = valorFormatadoParaParse.ObterDoublePorValorDoCampo();

            // Assert
            resultado.Should().Be(valorEsperado);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void DadoStringVaziaOuNula_QuandoObterDoublePorValorDoCampo_EntaoLancaNegocioException(string? valorOriginal)
        {
            // Arrange
            Action acao = () => valorOriginal!.ObterDoublePorValorDoCampo();

            // Act & Assert
            acao.Should().Throw<NegocioException>();
        }

        [Theory]
        [InlineData(10.5, 10.5, true)]
        [InlineData(10.5, 10.6, false)]
        [InlineData(0, 0, true)]
        [InlineData(100, -100, false)]
        public void DadoDoisValoresDouble_QuandoSaoIguais_EntaoRetornaVerdadeiroSeForemIguais(double valor1, double valor2, bool esperado)
        {
            // Arrange & Act
            var resultado = valor1.SaoIguais(valor2);

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData(10.5, 10.5, true)]
        [InlineData(10.5, 10.6, false)]
        [InlineData(null, null, true)]
        [InlineData(10.5, null, false)]
        [InlineData(null, 10.5, false)]
        public void DadoDoisValoresDoubleNulaveis_QuandoSaoIguais_EntaoRetornaVerdadeiroSeForemIguaisOuAmbosNulos(double? valor1, double? valor2, bool esperado)
        {
            // Arrange & Act
            var resultado = valor1.SaoIguais(valor2);

            // Assert
            resultado.Should().Be(esperado);
        }

        [Fact]
        public void DadoValorNulo_QuandoFormatarParaDoubleComCasasDecimais_EntaoRetornaNulo()
        {
            // Arrange
            double? valor = null;

            // Act
            var resultado = valor.FormatarParaDoubleComCasasDecimais();

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public void DadoValorComVirgulaOuPonto_QuandoFormatarParaDoubleComCasasDecimaisNulavel_EntaoRetornaMesmoValor()
        {
            // Arrange
            double? valor = 10.5;

            // Act
            var resultado = valor.FormatarParaDoubleComCasasDecimais();

            // Assert
            resultado.Should().Be(valor);
        }

        [Fact]
        public void DadoValorInteiroSemCasas_QuandoFormatarParaDoubleComCasasDecimaisNulavel_EntaoRetornaValorDivididoPorCem()
        {
            // Arrange
            double? valor = 1000; // Representando 10,00 sem vírgula

            // Act
            var resultado = valor.FormatarParaDoubleComCasasDecimais();

            // Assert
            resultado.Should().Be(10); // 1000 / 100 = 10
        }

        [Fact]
        public void DadoValorComVirgulaOuPonto_QuandoFormatarDoubleComCasasDecimais_EntaoRetornaMesmoValor()
        {
            // Arrange
            double valor = 15.5;

            // Act
            var resultado = valor.FormatarDoubleComCasasDecimais();

            // Assert
            resultado.Should().Be(valor);
        }

        [Fact]
        public void DadoValorInteiroSemCasas_QuandoFormatarDoubleComCasasDecimais_EntaoRetornaValorDivididoPorCem()
        {
            // Arrange
            double valor = 2500; // Representando 25,00 sem vírgula

            // Act
            var resultado = valor.FormatarDoubleComCasasDecimais();

            // Assert
            resultado.Should().Be(25); // 2500 / 100 = 25
        }
    }
}