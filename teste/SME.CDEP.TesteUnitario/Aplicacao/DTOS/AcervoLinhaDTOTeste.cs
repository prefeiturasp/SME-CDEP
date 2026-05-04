using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoLinhaDTOTeste
    {
        #region Status

        [Fact]
        public void DadoStatusVazio_QuandoCriarDTO_EntaoStatusDeveSerValorPadrao()
        {
            var dto = new AcervoLinhaDTO();

            dto.Status.Should().Be(default(ImportacaoStatus));
        }

        [Fact]
        public void DadoStatus_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var status = ImportacaoStatus.Sucesso;
            var dto = new AcervoLinhaDTO { Status = status };

            dto.Status.Should().Be(status);
        }

        [Fact]
        public void DadoStatusErros_QuandoAtribuir_EntaoDeveArmazenarErros()
        {
            var dto = new AcervoLinhaDTO { Status = ImportacaoStatus.Erros };

            dto.Status.Should().Be(ImportacaoStatus.Erros);
        }

        [Fact]
        public void DadoStatusPendente_QuandoAtribuir_EntaoDeveArmazenarPendente()
        {
            var dto = new AcervoLinhaDTO { Status = ImportacaoStatus.Pendente };

            dto.Status.Should().Be(ImportacaoStatus.Pendente);
        }

        #endregion

        #region Mensagem

        [Fact]
        public void DadoMensagemVazia_QuandoCriarDTO_EntaoMensagemDeveSerNull()
        {
            var dto = new AcervoLinhaDTO();

            dto.Mensagem.Should().BeNull();
        }

        [Fact]
        public void DadoMensagem_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var mensagem = new Faker().Lorem.Sentence();
            var dto = new AcervoLinhaDTO { Mensagem = mensagem };

            dto.Mensagem.Should().Be(mensagem);
        }

        [Fact]
        public void DadoMensagemErro_QuandoAtribuirTextoDeErro_EntaoDeveArmazenarCompleto()
        {
            var mensagem = "Erro: Campo obrigatório não preenchido";
            var dto = new AcervoLinhaDTO { Mensagem = mensagem };

            dto.Mensagem.Should().Be(mensagem);
            dto.Mensagem.Should().Contain("Erro");
        }

        [Fact]
        public void DadoMensagemVazia_QuandoAtribuirString_EntaoDeveArmazenarVazia()
        {
            var dto = new AcervoLinhaDTO { Mensagem = string.Empty };

            dto.Mensagem.Should().Be(string.Empty);
        }

        #endregion

        #region NumeroLinha

        [Fact]
        public void DadoNumeroLinhaVazio_QuandoCriarDTO_EntaoNumeroLinhaDeveSerZero()
        {
            var dto = new AcervoLinhaDTO();

            dto.NumeroLinha.Should().Be(0);
        }

        [Fact]
        public void DadoNumeroLinha_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var numeroLinha = new Faker().Random.Int(1, 1000);
            var dto = new AcervoLinhaDTO { NumeroLinha = numeroLinha };

            dto.NumeroLinha.Should().Be(numeroLinha);
        }

        [Fact]
        public void DadoNumeroLinhaMaximo_QuandoAtribuirIntMaxValue_EntaoDeveArmazenar()
        {
            var dto = new AcervoLinhaDTO { NumeroLinha = int.MaxValue };

            dto.NumeroLinha.Should().Be(int.MaxValue);
        }

        [Fact]
        public void DadoNumeroLinhaUm_QuandoAtribuir1_EntaoDeveArmazenarUm()
        {
            var dto = new AcervoLinhaDTO { NumeroLinha = 1 };

            dto.NumeroLinha.Should().Be(1);
        }

        #endregion

        #region PossuiErros

        [Fact]
        public void DadoPossuiErrosVazio_QuandoCriarDTO_EntaoPossuiErrosDeveSerFalse()
        {
            var dto = new AcervoLinhaDTO();

            dto.PossuiErros.Should().BeFalse();
        }

        [Fact]
        public void DadoPossuiErrosTrue_QuandoAtribuir_EntaoDeveArmazenarTrue()
        {
            var dto = new AcervoLinhaDTO { PossuiErros = true };

            dto.PossuiErros.Should().BeTrue();
        }

        [Fact]
        public void DadoPossuiErrosFalse_QuandoAtribuir_EntaoDeveArmazenarFalse()
        {
            var dto = new AcervoLinhaDTO { PossuiErros = false };

            dto.PossuiErros.Should().BeFalse();
        }

        #endregion

        #region Método: DefinirLinhaComoErro

        [Fact]
        public void DadoDTOSemErros_QuandoChamarDefinirLinhaComoErro_EntaoDeveAdicionarErro()
        {
            var dto = new AcervoLinhaDTO
            {
                PossuiErros = false,
                Status = ImportacaoStatus.Sucesso
            };

            var mensagem = "Erro na validação";
            dto.DefinirLinhaComoErro(mensagem);

            dto.PossuiErros.Should().BeTrue();
            dto.Status.Should().Be(ImportacaoStatus.Erros);
            dto.Mensagem.Should().Be(mensagem);
        }

        [Fact]
        public void DadoDTOComMensagemExistente_QuandoChamarDefinirLinhaComoErro_EntaoDeveSubstituirMensagem()
        {
            var dto = new AcervoLinhaDTO
            {
                PossuiErros = false,
                Mensagem = "Mensagem anterior",
                Status = ImportacaoStatus.Sucesso
            };

            var novaMensagem = "Nova mensagem de erro";
            dto.DefinirLinhaComoErro(novaMensagem);

            dto.Mensagem.Should().Be(novaMensagem);
            dto.PossuiErros.Should().BeTrue();
        }

        [Fact]
        public void DadoMensagemVazia_QuandoChamarDefinirLinhaComoErro_EntaoDeveArmazenarMensagemVazia()
        {
            var dto = new AcervoLinhaDTO();

            dto.DefinirLinhaComoErro(string.Empty);

            dto.PossuiErros.Should().BeTrue();
            dto.Status.Should().Be(ImportacaoStatus.Erros);
            dto.Mensagem.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoMensagemComCaracteresEspeciais_QuandoChamarDefinirLinhaComoErro_EntaoDeveArmazenarCompleto()
        {
            var dto = new AcervoLinhaDTO();
            var mensagem = "Erro: Campo 'Título' com valor inválido @#$%";

            dto.DefinirLinhaComoErro(mensagem);

            dto.Mensagem.Should().Be(mensagem);
            dto.PossuiErros.Should().BeTrue();
        }

        [Fact]
        public void DadoMensagemComQuebrasDeLinha_QuandoChamarDefinirLinhaComoErro_EntaoDeveArmazenarCompleto()
        {
            var dto = new AcervoLinhaDTO();
            var mensagem = "Erro 1\nErro 2\nErro 3";

            dto.DefinirLinhaComoErro(mensagem);

            dto.Mensagem.Should().Be(mensagem);
            dto.PossuiErros.Should().BeTrue();
        }

        [Fact]
        public void DadoMensagemMuitoLonga_QuandoChamarDefinirLinhaComoErro_EntaoDeveArmazenarCompleto()
        {
            var dto = new AcervoLinhaDTO();
            var mensagem = new string('A', 5000);

            dto.DefinirLinhaComoErro(mensagem);

            dto.Mensagem.Should().Be(mensagem);
            dto.Mensagem.Length.Should().Be(5000);
        }

        [Fact]
        public void DadoNumeroLinhaComErro_QuandoChamarDefinirLinhaComoErro_EntaoNaoDeveAlterarNumeroLinha()
        {
            var numeroLinha = 42;
            var dto = new AcervoLinhaDTO { NumeroLinha = numeroLinha };

            dto.DefinirLinhaComoErro("Erro detectado");

            dto.NumeroLinha.Should().Be(numeroLinha);
        }

        [Fact]
        public void DadoStatusPendenteAntes_QuandoChamarDefinirLinhaComoErro_EntaoMudaParaErros()
        {
            var dto = new AcervoLinhaDTO { Status = ImportacaoStatus.Pendente };

            dto.DefinirLinhaComoErro("Erro na processamento");

            dto.Status.Should().Be(ImportacaoStatus.Erros);
        }

        [Fact]
        public void DadoDTOJaComErro_QuandoChamarDefinirLinhaComoErroNovamente_EntaoDeveAtualizarMensagem()
        {
            var dto = new AcervoLinhaDTO();
            var mensagemPrimeira = "Primeiro erro";
            var mensagemSegunda = "Segundo erro";

            dto.DefinirLinhaComoErro(mensagemPrimeira);
            dto.PossuiErros.Should().BeTrue();

            dto.DefinirLinhaComoErro(mensagemSegunda);

            dto.Mensagem.Should().Be(mensagemSegunda);
            dto.PossuiErros.Should().BeTrue();
            dto.Status.Should().Be(ImportacaoStatus.Erros);
        }

        #endregion

        #region Testes de Integração - Múltiplas Propriedades

        [Fact]
        public void DadoDTOCompleto_QuandoInstanciarComTodosOsParametros_EntaoDeveArmazenarTodosCorretamente()
        {
            var faker = new Faker("pt_BR");
            var numeroLinha = faker.Random.Int(1, 100);
            var status = ImportacaoStatus.Sucesso;
            var mensagem = faker.Lorem.Sentence();

            var dto = new AcervoLinhaDTO
            {
                NumeroLinha = numeroLinha,
                Status = status,
                Mensagem = mensagem,
                PossuiErros = false
            };

            dto.NumeroLinha.Should().Be(numeroLinha);
            dto.Status.Should().Be(status);
            dto.Mensagem.Should().Be(mensagem);
            dto.PossuiErros.Should().BeFalse();
        }

        [Fact]
        public void DadoDTOVazio_QuandoInstanciarSemParametros_EntaoDeveSerValido()
        {
            var dto = new AcervoLinhaDTO();

            dto.Should().NotBeNull();
            dto.NumeroLinha.Should().Be(0);
            dto.Status.Should().Be(default(ImportacaoStatus));
            dto.Mensagem.Should().BeNull();
            dto.PossuiErros.Should().BeFalse();
        }

        [Fact]
        public void DadoDTOComValoresNulos_QuandoAtribuirExplicitamente_EntaoDeveArmazenarNull()
        {
            var dto = new AcervoLinhaDTO
            {
                Mensagem = null,
                Status = ImportacaoStatus.Sucesso,
                NumeroLinha = 0,
                PossuiErros = false
            };

            dto.Mensagem.Should().BeNull();
            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.NumeroLinha.Should().Be(0);
            dto.PossuiErros.Should().BeFalse();
        }

        [Fact]
        public void DadoDuasInstancias_QuandoComMesmosValores_EntaoSaoInstanciasDistintas()
        {
            var dto1 = new AcervoLinhaDTO
            {
                NumeroLinha = 1,
                Status = ImportacaoStatus.Sucesso,
                Mensagem = "Teste",
                PossuiErros = false
            };

            var dto2 = new AcervoLinhaDTO
            {
                NumeroLinha = 1,
                Status = ImportacaoStatus.Sucesso,
                Mensagem = "Teste",
                PossuiErros = false
            };

            dto1.Should().NotBeSameAs(dto2);
            dto1.NumeroLinha.Should().Be(dto2.NumeroLinha);
            dto1.Status.Should().Be(dto2.Status);
            dto1.Mensagem.Should().Be(dto2.Mensagem);
        }

        [Fact]
        public void DadoDTOComAcesso_QuandoAcessarMultiplasVezes_EntaoValoresPermanecem()
        {
            var dto = new AcervoLinhaDTO 
            { 
                NumeroLinha = 5,
                Status = ImportacaoStatus.Pendente,
                Mensagem = "Aguardando processamento",
                PossuiErros = false
            };

            var valor1 = dto.NumeroLinha;
            var valor2 = dto.NumeroLinha;
            var status1 = dto.Status;
            var status2 = dto.Status;

            valor1.Should().Be(valor2);
            status1.Should().Be(status2);
        }

        [Fact]
        public void DadoDTOComDiversosStatus_QuandoAlternarStatus_EntaoAlternaCorretamente()
        {
            var dto = new AcervoLinhaDTO();

            dto.Status = ImportacaoStatus.Pendente;
            dto.Status.Should().Be(ImportacaoStatus.Pendente);

            dto.Status = ImportacaoStatus.Erros;
            dto.Status.Should().Be(ImportacaoStatus.Erros);

            dto.Status = ImportacaoStatus.Sucesso;
            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact]
        public void DadoDTOComAtualizacoes_QuandoAlterarMultiplosValores_EntaoTodosDevemSerAtualizados()
        {
            var dto = new AcervoLinhaDTO();

            dto.NumeroLinha = 10;
            dto.Status = ImportacaoStatus.Erros;
            dto.Mensagem = "Erro na processamento";
            dto.PossuiErros = true;

            dto.NumeroLinha.Should().Be(10);
            dto.Status.Should().Be(ImportacaoStatus.Erros);
            dto.Mensagem.Should().Be("Erro na processamento");
            dto.PossuiErros.Should().BeTrue();
        }

        [Fact]
        public void DadoDTOComFluxoCompl4to_QuandoProcessarFluxoDeErro_EntaoDeveReflectirCorretamente()
        {
            var dto = new AcervoLinhaDTO { NumeroLinha = 15 };

            dto.Status.Should().Be(default(ImportacaoStatus));
            dto.PossuiErros.Should().BeFalse();

            dto.DefinirLinhaComoErro("Erro na validação");

            dto.PossuiErros.Should().BeTrue();
            dto.Status.Should().Be(ImportacaoStatus.Erros);
            dto.Mensagem.Should().Be("Erro na validação");
            dto.NumeroLinha.Should().Be(15);
        }

        [Fact]
        public void DadoDTOComMultiplosErros_QuandoProcessarSequenciaDeErros_EntaoUltimoMensagemPrevalecer()
        {
            var dto = new AcervoLinhaDTO { NumeroLinha = 1 };

            dto.DefinirLinhaComoErro("Erro 1");
            dto.Mensagem.Should().Be("Erro 1");

            dto.DefinirLinhaComoErro("Erro 2");
            dto.Mensagem.Should().Be("Erro 2");

            dto.DefinirLinhaComoErro("Erro 3");
            dto.Mensagem.Should().Be("Erro 3");

            dto.PossuiErros.Should().BeTrue();
            dto.Status.Should().Be(ImportacaoStatus.Erros);
        }

        #endregion
    }
}
