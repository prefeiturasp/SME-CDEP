using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoLinhaRetornoDtoTeste
    {
        [Fact]
        public void DadoStatus_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var status = ImportacaoStatus.Sucesso;
            var dto = new AcervoLinhaRetornoDTO();

            dto.Status = status;

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact]
        public void DadoNumeroLinha_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var numeroLinha = 42;
            var dto = new AcervoLinhaRetornoDTO();

            dto.NumeroLinha = numeroLinha;

            dto.NumeroLinha.Should().Be(numeroLinha);
        }

        [Fact]
        public void DadoMensagem_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var mensagem = "Acervo importado com sucesso";
            var dto = new AcervoLinhaRetornoDTO();

            dto.Mensagem = mensagem;

            dto.Mensagem.Should().Be(mensagem);
        }

        [Fact]
        public void DadoErrosCampos_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var erros = new[] { "Título inválido", "Ano inválido" };
            var dto = new AcervoLinhaRetornoDTO();

            dto.ErrosCampos = erros;

            dto.ErrosCampos.Should().BeEquivalentTo(erros);
            dto.ErrosCampos.Should().HaveCount(2);
        }

        [Fact]
        public void DadoAcervoLinhaRetornoDTO_QuandoInstanciar_EntaoPropriedadesAssunemValorPadrao()
        {
            var dto = new AcervoLinhaRetornoDTO();

            dto.Status.Should().Be((ImportacaoStatus)0);
            dto.NumeroLinha.Should().Be(0);
            dto.Mensagem.Should().BeNull();
            dto.ErrosCampos.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoLinhaRetornoDTO_QuandoAtribuirTodosOsPropriedades_EntaoRetornaTodosOsValoresAssignados()
        {
            var status = ImportacaoStatus.Erros;
            var numeroLinha = 15;
            var mensagem = "Falha na importação";
            var erros = new[] { "Campo1", "Campo2", "Campo3" };
            var dto = new AcervoLinhaRetornoDTO();

            dto.Status = status;
            dto.NumeroLinha = numeroLinha;
            dto.Mensagem = mensagem;
            dto.ErrosCampos = erros;

            dto.Status.Should().Be(status);
            dto.NumeroLinha.Should().Be(numeroLinha);
            dto.Mensagem.Should().Be(mensagem);
            dto.ErrosCampos.Should().BeEquivalentTo(erros);
        }

        [Fact]
        public void DadoAcervoLinhaRetornoDTO_QuandoAtribuirNullAoMensagem_EntaoRetornaNulo()
        {
            var dto = new AcervoLinhaRetornoDTO { Mensagem = "Mensagem anterior" };

            dto.Mensagem = null!;

            dto.Mensagem.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoLinhaRetornoDTO_QuandoAtribuirNullAoErrosCampos_EntaoRetornaNulo()
        {
            var dto = new AcervoLinhaRetornoDTO { ErrosCampos = new[] { "Erro1" } };

            dto.ErrosCampos = null!;

            dto.ErrosCampos.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoLinhaRetornoDTO_QuandoAtribuirValoresMultiplosAoStatus_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoLinhaRetornoDTO();

            dto.Status = ImportacaoStatus.Pendente;
            dto.Status = ImportacaoStatus.Erros;
            dto.Status = ImportacaoStatus.Sucesso;

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact]
        public void DadoAcervoLinhaRetornoDTO_QuandoAtribuirValoresMultiplosAoNumeroLinha_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoLinhaRetornoDTO();

            dto.NumeroLinha = 10;
            dto.NumeroLinha = 20;
            dto.NumeroLinha = 30;

            dto.NumeroLinha.Should().Be(30);
        }

        [Theory]
        [InlineData(ImportacaoStatus.Pendente)]
        [InlineData(ImportacaoStatus.Erros)]
        [InlineData(ImportacaoStatus.Sucesso)]
        public void DadoDiferentesStatus_QuandoAssignar_EntaoRetornaValoresCorretos(ImportacaoStatus status)
        {
            var dto = new AcervoLinhaRetornoDTO();

            dto.Status = status;

            dto.Status.Should().Be(status);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(999)]
        [InlineData(int.MaxValue)]
        public void DadoDiferentesNumerosLinhas_QuandoAssignar_EntaoRetornaValoresCorretos(int numeroLinha)
        {
            var dto = new AcervoLinhaRetornoDTO();

            dto.NumeroLinha = numeroLinha;

            dto.NumeroLinha.Should().Be(numeroLinha);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Importação realizada com sucesso")]
        [InlineData("Mensagem com caracteres especiais !@#$%")]
        [InlineData("Mensagem muito longa com muitos caracteres para testar a capacidade de armazenamento de um campo de mensagem genérica")]
        public void DadoDiferentesMensagens_QuandoAssignar_EntaoRetornaValoresCorretos(string mensagem)
        {
            var dto = new AcervoLinhaRetornoDTO();

            dto.Mensagem = mensagem;

            dto.Mensagem.Should().Be(mensagem);
        }

        [Fact]
        public void DadoErrosCamposVazio_QuandoAssignar_EntaoRetornaArrayVazio()
        {
            var erros = Array.Empty<string>();
            var dto = new AcervoLinhaRetornoDTO();

            dto.ErrosCampos = erros;

            dto.ErrosCampos.Should().BeEmpty();
            dto.ErrosCampos.Should().HaveCount(0);
        }

        [Fact]
        public void DadoErrosCamposComMultiplosItens_QuandoAssignar_EntaoRetornaArrayCompleto()
        {
            var erros = new[] { "Erro1", "Erro2", "Erro3", "Erro4", "Erro5" };
            var dto = new AcervoLinhaRetornoDTO();

            dto.ErrosCampos = erros;

            dto.ErrosCampos.Should().HaveCount(5);
            dto.ErrosCampos[0].Should().Be("Erro1");
            dto.ErrosCampos[4].Should().Be("Erro5");
        }

        [Fact]
        public void DadoAcervoLinhaRetornoDTO_QuandoUsarComStatusPendente_EntaoArmazenaValorCorretamente()
        {
            var dto = new AcervoLinhaRetornoDTO
            {
                Status = ImportacaoStatus.Pendente,
                NumeroLinha = 1,
                Mensagem = "Aguardando processamento",
                ErrosCampos = null!
            };

            dto.Status.Should().Be(ImportacaoStatus.Pendente);
            dto.NumeroLinha.Should().Be(1);
            dto.Mensagem.Should().Be("Aguardando processamento");
            dto.ErrosCampos.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoLinhaRetornoDTO_QuandoUsarComStatusErros_EntaoArmazenaValorComErros()
        {
            var errosEsperados = new[] { "Título obrigatório", "Ano fora do intervalo válido" };
            var dto = new AcervoLinhaRetornoDTO
            {
                Status = ImportacaoStatus.Erros,
                NumeroLinha = 5,
                Mensagem = "Falha na validação",
                ErrosCampos = errosEsperados
            };

            dto.Status.Should().Be(ImportacaoStatus.Erros);
            dto.NumeroLinha.Should().Be(5);
            dto.Mensagem.Should().Be("Falha na validação");
            dto.ErrosCampos.Should().BeEquivalentTo(errosEsperados);
            dto.ErrosCampos.Should().HaveCount(2);
        }

        [Fact]
        public void DadoAcervoLinhaRetornoDTO_QuandoUsarComStatusSucesso_EntaoArmazenaSemErros()
        {
            var dto = new AcervoLinhaRetornoDTO
            {
                Status = ImportacaoStatus.Sucesso,
                NumeroLinha = 10,
                Mensagem = "Acervo importado com sucesso",
                ErrosCampos = Array.Empty<string>()
            };

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.NumeroLinha.Should().Be(10);
            dto.Mensagem.Should().Be("Acervo importado com sucesso");
            dto.ErrosCampos.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoLinhaRetornoDTO_QuandoModificarPropriedadesSequencialmente_EntaoMantémCoerencia()
        {
            var dto = new AcervoLinhaRetornoDTO();

            dto.Status = ImportacaoStatus.Pendente;
            dto.NumeroLinha = 1;
            dto.Status.Should().Be(ImportacaoStatus.Pendente);

            dto.Mensagem = "Processando";
            dto.ErrosCampos = new[] { "Erro1" };
            dto.Mensagem.Should().Be("Processando");
            dto.ErrosCampos.Should().Contain("Erro1");

            dto.Status = ImportacaoStatus.Sucesso;
            dto.Mensagem = "Sucesso";
            dto.ErrosCampos = Array.Empty<string>();
            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.Mensagem.Should().Be("Sucesso");
            dto.ErrosCampos.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoLinhaRetornoDTO_QuandoCriarMultiplasInstancias_EntaoSãoIndependentes()
        {
            var dto1 = new AcervoLinhaRetornoDTO { Status = ImportacaoStatus.Sucesso, NumeroLinha = 1 };
            var dto2 = new AcervoLinhaRetornoDTO { Status = ImportacaoStatus.Erros, NumeroLinha = 2 };
            var dto3 = new AcervoLinhaRetornoDTO { Status = ImportacaoStatus.Pendente, NumeroLinha = 3 };

            dto1.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto1.NumeroLinha.Should().Be(1);

            dto2.Status.Should().Be(ImportacaoStatus.Erros);
            dto2.NumeroLinha.Should().Be(2);

            dto3.Status.Should().Be(ImportacaoStatus.Pendente);
            dto3.NumeroLinha.Should().Be(3);

            dto1.Status = ImportacaoStatus.Erros;
            dto2.Status.Should().Be(ImportacaoStatus.Erros);
            dto3.Status.Should().Be(ImportacaoStatus.Pendente);
        }

        [Fact]
        public void DadoMensagemVazia_QuandoAssignar_EntaoArmazenaString()
        {
            var dto = new AcervoLinhaRetornoDTO();

            dto.Mensagem = string.Empty;

            dto.Mensagem.Should().Be(string.Empty);
            dto.Mensagem.Should().NotBeNull();
        }
    }
}
