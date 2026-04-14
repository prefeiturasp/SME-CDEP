using FluentAssertions;
using SME.CDEP.Dominio.Extensions;

namespace SME.CDEP.TesteUnitario.Domain.Extensions
{
    public class StringExtensionTestes
    {
        [Theory]
        [InlineData(".jpg", true)]
        [InlineData(".jpeg", true)]
        [InlineData(".png", true)]
        [InlineData(".tiff", true)]
        [InlineData(".tif", true)]
        [InlineData(".pdf", false)]
        [InlineData(".txt", false)]
        public void DadoExtensao_QuandoEhExtensaoImagemGerarMiniatura_EntaoRetornaResultadoEsperado(string extensao, bool esperado)
        {
            // Arrange & Act
            var resultado = extensao.EhExtensaoImagemGerarMiniatura();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("foto.jpg", true)]
        [InlineData("documento.pdf", false)]
        public void DadoNomeArquivo_QuandoEhArquivoImagemParaOtimizar_EntaoRetornaResultadoEsperado(string nomeArquivo, bool esperado)
        {
            // Arrange & Act
            var resultado = nomeArquivo.EhArquivoImagemParaOtimizar();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("DOCUMENTO.FT", ".FT", true)]
        [InlineData("DOCUMENTO.AG", ".AG", true)]
        [InlineData("DOCUMENTO.PDF", ".FT", false)]
        public void DadoString_QuandoContemSigla_EntaoRetornaResultadoEsperado(string valor, string sigla, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.ContemSigla(sigla);

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("ABCD", "A")]
        [InlineData("ABC", "")]
        [InlineData("AB", "")]
        public void DadoString_QuandoRemoverSufixo_EntaoRetornaStringSemOsTresUltimosCaracteres(string valor, string esperado)
        {
            // Arrange & Act
            var resultado = valor.RemoverSufixo();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("Texto", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void DadoString_QuandoEstaPreenchido_EntaoRetornaResultadoEsperado(string? valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor!.EstaPreenchido();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("Texto", false)]
        [InlineData("", true)]
        [InlineData(null, true)]
        public void DadoString_QuandoNaoEstaPreenchido_EntaoRetornaResultadoEsperado(string? valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor!.NaoEstaPreenchido();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("12345", 3, "123")]
        [InlineData("12", 5, "12")]
        [InlineData("123", 3, "123")]
        public void DadoString_QuandoLimite_EntaoRetornaStringLimitadaAoTamanho(string valor, int limite, string esperado)
        {
            // Arrange & Act
            var resultado = valor.Limite(limite);

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("<p>Texto</p>", "Texto")]
        [InlineData("Texto<br>Teste", "Texto Teste")]
        [InlineData("<li>Item</li>", "Item")]
        [InlineData("Texto&nbsp;com&nbsp;espaco", "Texto com espaco")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void DadoStringHtml_QuandoRemoverTagsHtml_EntaoRetornaTextoLimpo(string? valor, string esperado)
        {
            // Arrange & Act
            var resultado = valor!.RemoverTagsHtml();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", true)]
        [InlineData("application/pdf", false)]
        public void DadoContentType_QuandoEhArquivoXlsx_EntaoRetornaResultadoEsperado(string valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.EhArquivoXlsx();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", false)]
        [InlineData("application/pdf", true)]
        public void DadoContentType_QuandoNaoEhArquivoXlsx_EntaoRetornaResultadoEsperado(string valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.NaoEhArquivoXlsx();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Fact]
        public void DadoTextoComPipes_QuandoSplitPipe_EntaoRetornaArrayDeStringsTratadas()
        {
            // Arrange
            var texto = " Autor A | Autor B | Autor C ";

            // Act
            var resultado = texto.SplitPipe();

            // Assert
            resultado.Should().HaveCount(3);
            resultado[0].Should().Be("Autor A");
            resultado[1].Should().Be("Autor B");
            resultado[2].Should().Be("Autor C");
        }

        [Fact]
        public void DadoTextoComPipes_QuandoFormatarTextoEmArray_EntaoRetornaArrayDistinto()
        {
            // Arrange
            var texto = "Autor A|Autor B|Autor A";

            // Act
            var resultado = texto.FormatarTextoEmArray().ToList();

            // Assert
            resultado.Should().HaveCount(2);
            resultado.Should().Contain("Autor A");
            resultado.Should().Contain("Autor B");
        }

        [Theory]
        [InlineData("abc", 5, true)]
        [InlineData("abc", 3, true)]
        [InlineData("abcde", 3, false)]
        [InlineData("abc", 0, true)]
        public void DadoString_QuandoValidarLimiteDeCaracteres_EntaoRetornaResultadoEsperado(string valor, int limite, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.ValidarLimiteDeCaracteres(limite);

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("string", true)]
        [InlineData("int", false)]
        public void DadoFormato_QuandoEhFormatoString_EntaoRetornaResultadoEsperado(string valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.EhFormatoString();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("sim", true)]
        [InlineData("SIM", true)]
        [InlineData("não", false)]
        public void DadoValor_QuandoEhOpcaoSim_EntaoRetornaResultadoEsperado(string valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.EhOpcaoSim();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("não", true)]
        [InlineData("NÃO", true)]
        [InlineData("sim", false)]
        public void DadoValor_QuandoEhOpcaoNao_EntaoRetornaResultadoEsperado(string valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.EhOpcaoNao();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("Teste", "teste", true)]
        [InlineData("Teste", "Testes", false)]
        public void DadoValores_QuandoSaoIguais_EntaoRetornaComparacaoIgnorandoCase(string valor1, string valor2, bool esperado)
        {
            // Arrange & Act
            var resultado = valor1.SaoIguais(valor2);

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("10", 10)]
        [InlineData("-5", -5)]
        public void DadoStringNumerica_QuandoConverterParaInteiro_EntaoRetornaInteiro(string valor, int esperado)
        {
            // Arrange & Act
            var resultado = valor.ConverterParaInteiro();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData(10.5)]
        [InlineData(10.0)]
        public void DadoStringNumerica_QuandoConverterParaDouble_EntaoRetornaDouble(double esperado)
        {
            // Arrange
            var valorStr = esperado.ToString(); // Garantindo o formato da Culture atual da thread

            // Act
            var resultado = valorStr.ConverterParaDouble();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("image/tiff", true)]
        [InlineData("image/jpeg", false)]
        public void DadoNomeArquivo_QuandoEhImagemTiff_EntaoRetornaResultadoEsperado(string valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.EhImagemTiff();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("10", "10")]
        [InlineData("", "0")]
        [InlineData(null, "0")]
        public void DadoString_QuandoObterValorOuZero_EntaoRetornaValorOuZeroSeVazio(string? valor, string esperado)
        {
            // Arrange & Act
            var resultado = valor!.ObterValorOuZero();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("Atenção", "Atencao")]
        [InlineData("Árvore", "Arvore")]
        [InlineData("João", "Joao")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void DadoStringComAcentos_QuandoRemoverAcentuacao_EntaoRetornaStringLimpa(string? valor, string? esperado)
        {
            // Arrange & Act
            var resultado = valor!.RemoverAcentuacao();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("Atenção", "atencao")]
        [InlineData("Árvore", "arvore")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void DadoStringComAcentos_QuandoRemoverAcentuacaoFormatarMinusculo_EntaoRetornaStringLimpaEMinuscula(string? valor, string? esperado)
        {
            // Arrange & Act
            var resultado = valor!.RemoverAcentuacaoFormatarMinusculo();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("arquivo.pdf", ".pdf")]
        [InlineData("foto.jpg", ".jpg")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void DadoNomeArquivo_QuandoObterExtensao_EntaoRetornaAExtensao(string? valor, string? esperado)
        {
            // Arrange & Act
            var resultado = valor!.ObterExtensao();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("12,34", false)] // É numérico com casas (falha na negativa = false)
        [InlineData("12.34", true)]  // Formato inválido pelo regex estrito
        [InlineData("abc", true)]    // Formato inválido
        [InlineData("", true)]
        public void DadoValor_QuandoNaoEhNumericoComCasasDecimais_EntaoValidaRegex(string valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.NaoEhNumericoComCasasDecimais();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("12.34", "12,34")]
        [InlineData("12,34", "12,34")]
        [InlineData("abc", "abc")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void DadoValor_QuandoTratarLiteralComoDecimalComCasasDecimais_EntaoRetornaFormatado(string? valor, string? esperado)
        {
            // Arrange & Act
            var resultado = valor!.TratarLiteralComoDecimalComCasasDecimais();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("[199?]", 1990)]
        [InlineData("[19--]", 1900)]
        [InlineData("2026", 2026)]
        [InlineData("", 0)]
        [InlineData(null, 0)]
        public void DadoAnoComMascara_QuandoObterAnoNumerico_EntaoRetornaInteiroConvertido(string? valor, int esperado)
        {
            // Arrange & Act
            var resultado = valor!.ObterAnoNumerico();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("[19--]", true)]
        [InlineData("1990", false)]
        public void DadoAno_QuandoContemDecadaOuSeculoCertoOuPossivel_EntaoRetornaResultado(string valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.ContemDecadaOuSeculoCertoOuPossivel();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("[19--]", true)]
        [InlineData("[199-?]", true)]
        [InlineData("[1999]", true)]
        [InlineData("1999", true)]
        [InlineData("abcd", false)]
        public void DadoAno_QuandoEhAnoConformeFormatoABNT_EntaoValidaPeloRegex(string valor, bool esperado)
        {
            // Arrange & Act
            var resultado = valor.EhAnoConformeFormatoABNT();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("111.222.333-44", "11122233344")]
        [InlineData("123456", "123456")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void DadoCpfComMascara_QuandoRemoverMascaraCPF_EntaoRetornaSomenteNumeros(string? valor, string? esperado)
        {
            // Arrange & Act
            var resultado = valor!.RemoverMascaraCPF();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData("  Texto  ", "Texto")]
        [InlineData("Texto", "Texto")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void DadoString_QuandoRemoverEspacos_EntaoRetornaTrimmed(string? valor, string? esperado)
        {
            // Arrange & Act
            var resultado = valor!.RemoverEspacos();

            // Assert
            resultado.Should().Be(esperado);
        }
    }
}