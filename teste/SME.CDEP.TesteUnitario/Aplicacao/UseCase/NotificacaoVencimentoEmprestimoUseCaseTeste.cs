using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;
using Xunit;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase
{
    public class NotificacaoVencimentoEmprestimoUseCaseTeste
    {
        private readonly Mock<IServicoAcervoEmprestimo> servicoAcervoEmprestimoMock;
        private readonly NotificacaoVencimentoEmprestimoUseCase sut;

        public NotificacaoVencimentoEmprestimoUseCaseTeste()
        {
            var mocker = new AutoMocker();
            servicoAcervoEmprestimoMock = mocker.GetMock<IServicoAcervoEmprestimo>();
            sut = mocker.CreateInstance<NotificacaoVencimentoEmprestimoUseCase>();
        }

        [Fact]
        public void DadoDependenciasValidas_QuandoInstanciarUseCase_EntaoRetornaInstanciaComSucesso()
        {
            var instancia = new NotificacaoVencimentoEmprestimoUseCase(
                servicoAcervoEmprestimoMock.Object);

            instancia.Should().NotBeNull();
            sut.Should().NotBeNull();
        }

        [Fact]
        public void DadoServicoAcervoEmprestimoNulo_QuandoInstanciarUseCase_EntaoLancaArgumentNullException()
        {
            // A instância criada é usada na ação, não precisa ser atribuída a uma variável.
            Action acao = () => _ = new NotificacaoVencimentoEmprestimoUseCase(null!);

            acao.Should().Throw<ArgumentNullException>()
                .WithParameterName("servicoEventoAcervoEmprestimo");
        }

        [Fact]
        public async Task DadoMensagemRabbitValida_QuandoExecutar_EntaoChamaServicoNotificarVencimentoEmprestimoComSucesso()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "mensagem de teste" };

            servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarVencimentoEmprestimo())
                .Returns(Task.CompletedTask);

            var resultado = await sut.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            servicoAcervoEmprestimoMock.Verify(
                s => s.NotificarVencimentoEmprestimo(),
                Times.Once);
        }

        [Fact]
        public async Task DadoMensagemRabbitComMensagemVazia_QuandoExecutar_EntaoChamaServicoERetornaVerdadeiro()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = string.Empty };

            servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarVencimentoEmprestimo())
                .Returns(Task.CompletedTask);

            var resultado = await sut.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            servicoAcervoEmprestimoMock.Verify(
                s => s.NotificarVencimentoEmprestimo(),
                Times.Once);
        }

        [Fact]
        public async Task DadoMensagemRabbitComMensagemNula_QuandoExecutar_EntaoChamaServicoERetornaVerdadeiro()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = null! };

            servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarVencimentoEmprestimo())
                .Returns(Task.CompletedTask);

            var resultado = await sut.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            servicoAcervoEmprestimoMock.Verify(
                s => s.NotificarVencimentoEmprestimo(),
                Times.Once);
        }

        [Fact]
        public async Task DadoServicoAcervoEmprestimoThrowException_QuandoExecutar_EntaoRelancaExcecao()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "mensagem de teste" };
            var excecaoEsperada = new NegocioException("Erro ao notificar vencimento");

            servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarVencimentoEmprestimo())
                .ThrowsAsync(excecaoEsperada);

            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage("Erro ao notificar vencimento");
        }

        [Fact]
        public async Task DadoMensagemRabbitComCaracteresEspeciais_QuandoExecutar_EntaoChamaServicoERetornaVerdadeiro()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "mensagem <especial> & \"teste\"" };

            servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarVencimentoEmprestimo())
                .Returns(Task.CompletedTask);

            var resultado = await sut.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            servicoAcervoEmprestimoMock.Verify(
                s => s.NotificarVencimentoEmprestimo(),
                Times.Once);
        }

        [Fact]
        public async Task DadoMensagemRabbitComTextoGrande_QuandoExecutar_EntaoChamaServicoERetornaVerdadeiro()
        {
            var textoGrande = new string('a', 10000);
            var mensagemRabbit = new MensagemRabbit { Mensagem = textoGrande };

            servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarVencimentoEmprestimo())
                .Returns(Task.CompletedTask);

            var resultado = await sut.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            servicoAcervoEmprestimoMock.Verify(
                s => s.NotificarVencimentoEmprestimo(),
                Times.Once);
        }

        [Fact]
        public async Task DadoMensagemRabbitValida_QuandoExecutarMultiplasChamadas_EntaoChamaServicoCorretoNumeroDeTimes()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "mensagem de teste" };

            servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarVencimentoEmprestimo())
                .Returns(Task.CompletedTask);

            var resultado1 = await sut.Executar(mensagemRabbit);
            var resultado2 = await sut.Executar(mensagemRabbit);
            var resultado3 = await sut.Executar(mensagemRabbit);

            resultado1.Should().BeTrue();
            resultado2.Should().BeTrue();
            resultado3.Should().BeTrue();
            servicoAcervoEmprestimoMock.Verify(
                s => s.NotificarVencimentoEmprestimo(),
                Times.Exactly(3));
        }

        [Fact]
        public async Task DadoRetornoTrue_QuandoExecutarComSucesso_EntaoRetornaTrueComSucesso()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "teste" };

            servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarVencimentoEmprestimo())
                .Returns(Task.CompletedTask);

            var resultado = await sut.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task DadoExcecaoGenerica_QuandoExecutar_EntaoRelancaExcecao()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "mensagem de teste" };
            var excecaoEsperada = new Exception("Erro genérico");

            servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarVencimentoEmprestimo())
                .ThrowsAsync(excecaoEsperada);

            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            await acao.Should().ThrowAsync<Exception>()
                .WithMessage("Erro genérico");
        }

        [Fact]
        public async Task DadoServicoAcervoEmprestimoCompleta_QuandoExecutar_EntaoMantemasDependenciasIntactas()
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = "mensagem" };

            servicoAcervoEmprestimoMock
                .Setup(s => s.NotificarVencimentoEmprestimo())
                .Returns(Task.CompletedTask);

            await sut.Executar(mensagemRabbit);

            servicoAcervoEmprestimoMock.Verify(
                s => s.NotificarVencimentoEmprestimo(),
                Times.Once);
        }
    }
}
