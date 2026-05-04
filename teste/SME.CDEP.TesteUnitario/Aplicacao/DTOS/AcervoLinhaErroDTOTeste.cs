using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoLinhaErroDTOTeste
    {
        [Fact]
        public void DadoNumeroLinha_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var numeroLinha = 42;
            var dto = new AcervoLinhaErroDTO<string, string>();

            dto.NumeroLinha = numeroLinha;

            dto.NumeroLinha.Should().Be(numeroLinha);
        }

        [Fact]
        public void DadoTitulo_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var titulo = "Título do Acervo";
            var dto = new AcervoLinhaErroDTO<string, string>();

            dto.Titulo = titulo;

            dto.Titulo.Should().Be(titulo);
        }

        [Fact]
        public void DadoTombo_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var tombo = "2024.001.0001";
            var dto = new AcervoLinhaErroDTO<string, string>();

            dto.Tombo = tombo;

            dto.Tombo.Should().Be(tombo);
        }

        [Fact]
        public void DadoRetornoObjeto_QuandoAssignarComTipoPrimitivo_EntaoRetornaValorAssignado()
        {
            var objeto = "Sucesso";
            var dto = new AcervoLinhaErroDTO<string, string>();

            dto.RetornoObjeto = objeto;

            dto.RetornoObjeto.Should().Be(objeto);
        }

        [Fact]
        public void DadoRetornoObjeto_QuandoAssignarComTipoComplexo_EntaoRetornaValorAssignado()
        {
            var objeto = new TestObject { Id = 1, Nome = "Teste" };
            var dto = new AcervoLinhaErroDTO<TestObject, string>();

            dto.RetornoObjeto = objeto;

            dto.RetornoObjeto.Should().NotBeNull();
            dto.RetornoObjeto.Id.Should().Be(1);
            dto.RetornoObjeto.Nome.Should().Be("Teste");
        }

        [Fact]
        public void DadoRetornoErro_QuandoAssignarComTipoPrimitivo_EntaoRetornaValorAssignado()
        {
            var erro = "Erro na linha";
            var dto = new AcervoLinhaErroDTO<string, string>();

            dto.RetornoErro = erro;

            dto.RetornoErro.Should().Be(erro);
        }

        [Fact]
        public void DadoRetornoErro_QuandoAssignarComTipoComplexo_EntaoRetornaValorAssignado()
        {
            var erro = new TestError { Codigo = 500, Mensagem = "Erro interno" };
            var dto = new AcervoLinhaErroDTO<string, TestError>();

            dto.RetornoErro = erro;

            dto.RetornoErro.Should().NotBeNull();
            dto.RetornoErro.Codigo.Should().Be(500);
            dto.RetornoErro.Mensagem.Should().Be("Erro interno");
        }

        [Fact]
        public void DadoAcervoLinhaErroDTO_QuandoAtribuirTodosOsPropriedades_EntaoRetornaTodosOsValoresAssignados()
        {
            var numeroLinha = 15;
            var titulo = "Livro de Referência";
            var tombo = "2024.002.0002";
            var objeto = new TestObject { Id = 2, Nome = "Objeto de Retorno" };
            var erro = new TestError { Codigo = 400, Mensagem = "Dados inválidos" };
            var dto = new AcervoLinhaErroDTO<TestObject, TestError>();

            dto.NumeroLinha = numeroLinha;
            dto.Titulo = titulo;
            dto.Tombo = tombo;
            dto.RetornoObjeto = objeto;
            dto.RetornoErro = erro;

            dto.NumeroLinha.Should().Be(numeroLinha);
            dto.Titulo.Should().Be(titulo);
            dto.Tombo.Should().Be(tombo);
            dto.RetornoObjeto.Should().BeEquivalentTo(objeto);
            dto.RetornoErro.Should().BeEquivalentTo(erro);
        }

        [Fact]
        public void DadoAcervoLinhaErroDTO_QuandoInstanciarComValoresPadrao_EntaoPropriedadesAssunemValorPadrao()
        {
            var dto = new AcervoLinhaErroDTO<int, string>();

            dto.NumeroLinha.Should().Be(0);
            dto.Titulo.Should().BeNull();
            dto.Tombo.Should().BeNull();
            dto.RetornoObjeto.Should().Be(0);
            dto.RetornoErro.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoLinhaErroDTO_QuandoAtribuirNullAosTitulo_EntaoRetornaNulo()
        {
            var dto = new AcervoLinhaErroDTO<string, string> { Titulo = "Qualquer valor" };

            dto.Titulo = null;

            dto.Titulo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoLinhaErroDTO_QuandoAtribuirNullAoTombo_EntaoRetornaNulo()
        {
            var dto = new AcervoLinhaErroDTO<string, string> { Tombo = "2024.001.0001" };

            dto.Tombo = null;

            dto.Tombo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoLinhaErroDTO_QuandoAtribuirNullAoRetornoObjeto_EntaoRetornaNulo()
        {
            var dto = new AcervoLinhaErroDTO<TestObject, string> { RetornoObjeto = new TestObject() };

            dto.RetornoObjeto = null;

            dto.RetornoObjeto.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoLinhaErroDTO_QuandoAtribuirNullAoRetornoErro_EntaoRetornaNulo()
        {
            var dto = new AcervoLinhaErroDTO<string, TestError> { RetornoErro = new TestError() };

            dto.RetornoErro = null;

            dto.RetornoErro.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoLinhaErroDTO_QuandoAtribuirValoresMultiplosAoNumeroLinha_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoLinhaErroDTO<string, string>();

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
            var dto = new AcervoLinhaErroDTO<string, string>();

            dto.NumeroLinha = numeroLinha;

            dto.NumeroLinha.Should().Be(numeroLinha);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Título com caracteres especiais !@#$%")]
        [InlineData("Título muito longo com muitos caracteres para testar a capacidade de armazenamento")]
        public void DadoDiferentesTitulos_QuandoAssignar_EntaoRetornaValoresCorretos(string titulo)
        {
            var dto = new AcervoLinhaErroDTO<string, string>();

            dto.Titulo = titulo;

            dto.Titulo.Should().Be(titulo);
        }

        [Theory]
        [InlineData("")]
        [InlineData("TOMB-2024-001")]
        [InlineData("001.002.003.004")]
        public void DadoDiferentesTombos_QuandoAssignar_EntaoRetornaValoresCorretos(string tombo)
        {
            var dto = new AcervoLinhaErroDTO<string, string>();

            dto.Tombo = tombo;

            dto.Tombo.Should().Be(tombo);
        }

        [Fact]
        public void DadoAcervoLinhaErroDTO_QuandoUsarComDiferentesGenericos_EntaoFuncionaCorretamente()
        {
            var dtoStringString = new AcervoLinhaErroDTO<string, string>
            {
                NumeroLinha = 1,
                Titulo = "Teste 1",
                Tombo = "001",
                RetornoObjeto = "Objeto String",
                RetornoErro = "Erro String"
            };

            var dtoIntInt = new AcervoLinhaErroDTO<int, int>
            {
                NumeroLinha = 2,
                Titulo = "Teste 2",
                Tombo = "002",
                RetornoObjeto = 100,
                RetornoErro = 200
            };

            var dtoObjectObject = new AcervoLinhaErroDTO<object, object>
            {
                NumeroLinha = 3,
                Titulo = "Teste 3",
                Tombo = "003",
                RetornoObjeto = new object(),
                RetornoErro = new object()
            };

            dtoStringString.RetornoObjeto.Should().Be("Objeto String");
            dtoIntInt.RetornoObjeto.Should().Be(100);
            dtoObjectObject.RetornoObjeto.Should().NotBeNull();
        }

        private class TestObject
        {
            public int Id { get; set; }
            public string Nome { get; set; }
        }

        private class TestError
        {
            public int Codigo { get; set; }
            public string Mensagem { get; set; }
        }
    }
}
