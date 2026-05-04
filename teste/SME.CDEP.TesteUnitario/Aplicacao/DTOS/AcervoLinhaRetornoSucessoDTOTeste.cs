using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoLinhaRetornoSucessoDTOTeste
    {
        [Fact]
        public void DadoNumeroLinha_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var numeroLinha = 42;
            var dto = new AcervoLinhaRetornoSucessoDTO();

            dto.NumeroLinha = numeroLinha;

            dto.NumeroLinha.Should().Be(numeroLinha);
        }

        [Fact]
        public void DadoTitulo_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var titulo = "Título do Acervo";
            var dto = new AcervoLinhaRetornoSucessoDTO();

            dto.Titulo = titulo;

            dto.Titulo.Should().Be(titulo);
        }

        [Fact]
        public void DadoTombo_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var tombo = "2024.001.0001";
            var dto = new AcervoLinhaRetornoSucessoDTO();

            dto.Tombo = tombo;

            dto.Tombo.Should().Be(tombo);
        }

        [Fact]
        public void DadoAcervoLinhaRetornoSucessoDTO_QuandoInstanciar_EntaoPropriedadesAssunemValorPadrao()
        {
            var dto = new AcervoLinhaRetornoSucessoDTO();

            dto.NumeroLinha.Should().Be(0);
            dto.Titulo.Should().BeNull();
            dto.Tombo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoLinhaRetornoSucessoDTO_QuandoAtribuirTodosOsPropriedades_EntaoRetornaTodosOsValoresAssignados()
        {
            var numeroLinha = 15;
            var titulo = "Livro de Referência";
            var tombo = "2024.002.0002";
            var dto = new AcervoLinhaRetornoSucessoDTO();

            dto.NumeroLinha = numeroLinha;
            dto.Titulo = titulo;
            dto.Tombo = tombo;

            dto.NumeroLinha.Should().Be(numeroLinha);
            dto.Titulo.Should().Be(titulo);
            dto.Tombo.Should().Be(tombo);
        }

        [Fact]
        public void DadoAcervoLinhaRetornoSucessoDTO_QuandoAtribuirNullAoTitulo_EntaoRetornaNulo()
        {
            var dto = new AcervoLinhaRetornoSucessoDTO { Titulo = "Qualquer valor" };

            dto.Titulo = null;

            dto.Titulo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoLinhaRetornoSucessoDTO_QuandoAtribuirNullAoTombo_EntaoRetornaNulo()
        {
            var dto = new AcervoLinhaRetornoSucessoDTO { Tombo = "2024.001.0001" };

            dto.Tombo = null;

            dto.Tombo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoLinhaRetornoSucessoDTO_QuandoAtribuirValoresMultiplosAoNumeroLinha_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoLinhaRetornoSucessoDTO();

            dto.NumeroLinha = 10;
            dto.NumeroLinha = 20;
            dto.NumeroLinha = 30;

            dto.NumeroLinha.Should().Be(30);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(999)]
        [InlineData(int.MaxValue)]
        public void DadoDiferentesNumerosLinhas_QuandoAssignar_EntaoRetornaValoresCorretos(int numeroLinha)
        {
            var dto = new AcervoLinhaRetornoSucessoDTO();

            dto.NumeroLinha = numeroLinha;

            dto.NumeroLinha.Should().Be(numeroLinha);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Título com caracteres especiais !@#$%")]
        [InlineData("Título muito longo com muitos caracteres para testar a capacidade de armazenamento")]
        public void DadoDiferentesTitulos_QuandoAssignar_EntaoRetornaValoresCorretos(string titulo)
        {
            var dto = new AcervoLinhaRetornoSucessoDTO();

            dto.Titulo = titulo;

            dto.Titulo.Should().Be(titulo);
        }

        [Theory]
        [InlineData("")]
        [InlineData("TOMB-2024-001")]
        [InlineData("001.002.003.004")]
        public void DadoDiferentesTombos_QuandoAssignar_EntaoRetornaValoresCorretos(string tombo)
        {
            var dto = new AcervoLinhaRetornoSucessoDTO();

            dto.Tombo = tombo;

            dto.Tombo.Should().Be(tombo);
        }

        [Fact]
        public void DadoAcervoLinhaRetornoSucessoDTO_QuandoAtribuirValoresMultiplosAoTitulo_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoLinhaRetornoSucessoDTO();

            dto.Titulo = "Título 1";
            dto.Titulo = "Título 2";
            dto.Titulo = "Título 3";

            dto.Titulo.Should().Be("Título 3");
        }

        [Fact]
        public void DadoAcervoLinhaRetornoSucessoDTO_QuandoAtribuirValoresMultiplosAoTombo_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoLinhaRetornoSucessoDTO();

            dto.Tombo = "001";
            dto.Tombo = "002";
            dto.Tombo = "003";

            dto.Tombo.Should().Be("003");
        }

        [Fact]
        public void DadoAcervoLinhaRetornoSucessoDTO_QuandoCriarMultiplasInstancias_EntaoSãoIndependentes()
        {
            var dto1 = new AcervoLinhaRetornoSucessoDTO { NumeroLinha = 1, Titulo = "Título 1", Tombo = "001" };
            var dto2 = new AcervoLinhaRetornoSucessoDTO { NumeroLinha = 2, Titulo = "Título 2", Tombo = "002" };
            var dto3 = new AcervoLinhaRetornoSucessoDTO { NumeroLinha = 3, Titulo = "Título 3", Tombo = "003" };

            dto1.NumeroLinha.Should().Be(1);
            dto1.Titulo.Should().Be("Título 1");
            dto1.Tombo.Should().Be("001");

            dto2.NumeroLinha.Should().Be(2);
            dto2.Titulo.Should().Be("Título 2");
            dto2.Tombo.Should().Be("002");

            dto3.NumeroLinha.Should().Be(3);
            dto3.Titulo.Should().Be("Título 3");
            dto3.Tombo.Should().Be("003");

            dto1.NumeroLinha = 10;
            dto2.NumeroLinha.Should().Be(2);
            dto3.NumeroLinha.Should().Be(3);
        }

        [Fact]
        public void DadoTituloVazio_QuandoAssignar_EntaoArmazenaString()
        {
            var dto = new AcervoLinhaRetornoSucessoDTO();

            dto.Titulo = string.Empty;

            dto.Titulo.Should().Be(string.Empty);
            dto.Titulo.Should().NotBeNull();
        }

        [Fact]
        public void DadoTomboVazio_QuandoAssignar_EntaoArmazenaString()
        {
            var dto = new AcervoLinhaRetornoSucessoDTO();

            dto.Tombo = string.Empty;

            dto.Tombo.Should().Be(string.Empty);
            dto.Tombo.Should().NotBeNull();
        }

        [Fact]
        public void DadoAcervoLinhaRetornoSucessoDTO_QuandoModificarPropriedadesSequencialmente_EntaoMantémCoerencia()
        {
            var dto = new AcervoLinhaRetornoSucessoDTO();

            dto.NumeroLinha = 1;
            dto.Titulo = "Título Inicial";
            dto.NumeroLinha.Should().Be(1);
            dto.Titulo.Should().Be("Título Inicial");

            dto.Tombo = "2024.001.0001";
            dto.Titulo = "Título Modificado";
            dto.Tombo.Should().Be("2024.001.0001");
            dto.Titulo.Should().Be("Título Modificado");

            dto.NumeroLinha = 5;
            dto.Titulo = "Título Final";
            dto.Tombo = "2024.005.0005";
            dto.NumeroLinha.Should().Be(5);
            dto.Titulo.Should().Be("Título Final");
            dto.Tombo.Should().Be("2024.005.0005");
        }

        [Fact]
        public void DadoAcervoLinhaRetornoSucessoDTO_QuandoInstanciarComConstrutorPadrao_EntaoTodosOsPropriedadesEstaemAcessiveis()
        {
            var dto = new AcervoLinhaRetornoSucessoDTO();

            dto.Should().NotBeNull();
            dto.NumeroLinha.Should().Be(0);
            dto.Titulo.Should().BeNull();
            dto.Tombo.Should().BeNull();
        }
    }
}
