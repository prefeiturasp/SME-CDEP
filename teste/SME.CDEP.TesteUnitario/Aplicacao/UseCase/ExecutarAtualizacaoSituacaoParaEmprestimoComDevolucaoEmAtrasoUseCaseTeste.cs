using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase
{
    public class ExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtrasoUseCaseTeste
    {
        private readonly Mock<IRepositorioAcervoEmprestimo> _repositorioAcervoEmprestimoMock;
        private readonly ExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtrasoUseCase _useCase;

        public ExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtrasoUseCaseTeste()
        {
            var mocker = new AutoMocker();
            _repositorioAcervoEmprestimoMock = mocker.GetMock<IRepositorioAcervoEmprestimo>();
            _useCase = mocker.CreateInstance<ExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtrasoUseCase>();
        }

        #region Testes do Construtor

        [Fact]
        public void DadoDependenciasValidas_QuandoInstanciarUseCase_EntaoRetornaInstanciaComSucesso()
        {
            Action acao = () => new ExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtrasoUseCase(
                _repositorioAcervoEmprestimoMock.Object);

            acao.Should().NotThrow();
        }

        [Fact]
        public void DadoRepositorioNulo_QuandoInstanciarUseCase_EntaoLancaArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtrasoUseCase(null!));
        }

        #endregion

        #region Testes do Método Executar - Cenários Sem Itens Atrasados

        [Fact]
        public async Task DadoNenhumItemEmprestadoAtrasado_QuandoExecutar_EntaoRetornaTrueEInserirNuncaEhChamado()
        {
            _repositorioAcervoEmprestimoMock
                .Setup(r => r.ObterItensEmprestadosAtrasados())
                .ReturnsAsync(new List<AcervoEmprestimo>());

            var mensagemRabbit = new MensagemRabbit();

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            _repositorioAcervoEmprestimoMock.Verify(r => r.Inserir(It.IsAny<AcervoEmprestimo>()), Times.Never);
            _repositorioAcervoEmprestimoMock.Verify(r => r.ObterItensEmprestadosAtrasados(), Times.Once);
        }

        [Fact]
        public async Task DadoItensEmprestadosAtrazdosNull_QuandoExecutar_EntaoLancaArgumentNullException()
        {
            _repositorioAcervoEmprestimoMock
                .Setup(r => r.ObterItensEmprestadosAtrasados())
                .ReturnsAsync((List<AcervoEmprestimo>)null!);

            var mensagemRabbit = new MensagemRabbit();

            await Assert.ThrowsAsync<ArgumentNullException>(() => _useCase.Executar(mensagemRabbit));
        }

        #endregion

        #region Testes do Método Executar - Cenários Com Itens Atrasados

        [Fact]
        public async Task DadoUmItemEmprestadoAtrasado_QuandoExecutar_EntaoAtualizaDevolucaoEmAtrasoEInserirUmaVez()
        {
            var itemAtrasado = CriarAcervoEmprestimoAtrasado();
            var itensAtrasados = new List<AcervoEmprestimo> { itemAtrasado };

            _repositorioAcervoEmprestimoMock
                .Setup(r => r.ObterItensEmprestadosAtrasados())
                .ReturnsAsync(itensAtrasados);

            _repositorioAcervoEmprestimoMock
                .Setup(r => r.Inserir(It.IsAny<AcervoEmprestimo>()))
                .ReturnsAsync(1L);

            var mensagemRabbit = new MensagemRabbit();

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            _repositorioAcervoEmprestimoMock.Verify(r => r.Inserir(It.IsAny<AcervoEmprestimo>()), Times.Once);
            _repositorioAcervoEmprestimoMock.Verify(r => r.ObterItensEmprestadosAtrasados(), Times.Once);
        }

        [Fact]
        public async Task DadoMultiplosItensEmprestadosAtrasados_QuandoExecutar_EntaoAtualizaTodosEInsereParaCadaUm()
        {
            var item1 = CriarAcervoEmprestimoAtrasado(1);
            var item2 = CriarAcervoEmprestimoAtrasado(2);
            var item3 = CriarAcervoEmprestimoAtrasado(3);

            var itensAtrasados = new List<AcervoEmprestimo> { item1, item2, item3 };

            _repositorioAcervoEmprestimoMock
                .Setup(r => r.ObterItensEmprestadosAtrasados())
                .ReturnsAsync(itensAtrasados);

            _repositorioAcervoEmprestimoMock
                .Setup(r => r.Inserir(It.IsAny<AcervoEmprestimo>()))
                .ReturnsAsync(1L);

            var mensagemRabbit = new MensagemRabbit();

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            _repositorioAcervoEmprestimoMock.Verify(r => r.Inserir(It.IsAny<AcervoEmprestimo>()), Times.Exactly(3));
            _repositorioAcervoEmprestimoMock.Verify(r => r.ObterItensEmprestadosAtrasados(), Times.Once);
        }

        [Fact]
        public async Task DadoItemEmprestadoAtrasado_QuandoExecutar_EntaoMetodoDefinirDevolucaoEmAtrasoEhChamado()
        {
            var itemAtrasado = CriarAcervoEmprestimoAtrasado();
            var itensAtrasados = new List<AcervoEmprestimo> { itemAtrasado };

            _repositorioAcervoEmprestimoMock
                .Setup(r => r.ObterItensEmprestadosAtrasados())
                .ReturnsAsync(itensAtrasados);

            _repositorioAcervoEmprestimoMock
                .Setup(r => r.Inserir(It.IsAny<AcervoEmprestimo>()))
                .ReturnsAsync(1L);

            var mensagemRabbit = new MensagemRabbit();

            await _useCase.Executar(mensagemRabbit);

            _repositorioAcervoEmprestimoMock.Verify(
                r => r.Inserir(It.Is<AcervoEmprestimo>(a => a.Id == itemAtrasado.Id)),
                Times.Once);
        }

        [Fact]
        public async Task DadoParametroMensagemRabbitValido_QuandoExecutar_EntaoProcessaComSucesso()
        {
            var itensAtrasados = new List<AcervoEmprestimo> 
            { 
                CriarAcervoEmprestimoAtrasado(10), 
                CriarAcervoEmprestimoAtrasado(20) 
            };

            _repositorioAcervoEmprestimoMock
                .Setup(r => r.ObterItensEmprestadosAtrasados())
                .ReturnsAsync(itensAtrasados);

            _repositorioAcervoEmprestimoMock
                .Setup(r => r.Inserir(It.IsAny<AcervoEmprestimo>()))
                .ReturnsAsync(1L);

            var mensagemRabbit = new MensagemRabbit { Mensagem = "Teste" };

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            _repositorioAcervoEmprestimoMock.Verify(
                r => r.Inserir(It.IsAny<AcervoEmprestimo>()),
                Times.Exactly(2));
        }

        #endregion

        #region Testes de Exceção

        [Fact]
        public async Task DadoRepositorioLancaExcecao_QuandoExecutar_EntaoExcecaoEhPropagada()
        {
            var mensagemErro = "Erro ao obter itens emprestados atrasados";

            _repositorioAcervoEmprestimoMock
                .Setup(r => r.ObterItensEmprestadosAtrasados())
                .ThrowsAsync(new Exception(mensagemErro));

            var mensagemRabbit = new MensagemRabbit();

            await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(mensagemRabbit));
        }

        [Fact]
        public async Task DadoErroNaInsercao_QuandoExecutar_EntaoExcecaoEhPropagada()
        {
            var itemAtrasado = CriarAcervoEmprestimoAtrasado();
            var itensAtrasados = new List<AcervoEmprestimo> { itemAtrasado };

            var mensagemErro = "Erro ao inserir item";

            _repositorioAcervoEmprestimoMock
                .Setup(r => r.ObterItensEmprestadosAtrasados())
                .ReturnsAsync(itensAtrasados);

            _repositorioAcervoEmprestimoMock
                .Setup(r => r.Inserir(It.IsAny<AcervoEmprestimo>()))
                .ThrowsAsync(new Exception(mensagemErro));

            var mensagemRabbit = new MensagemRabbit();

            await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(mensagemRabbit));
        }

        #endregion

        #region Métodos Auxiliares

        private AcervoEmprestimo CriarAcervoEmprestimoAtrasado(long? id = 1)
        {
            return new AcervoEmprestimo
            {
                Id = id ?? 1,
                AcervoSolicitacaoItemId = 100,
                DataEmprestimo = DateTime.Now.AddDays(-30),
                DataDevolucao = DateTime.Now.AddDays(-5)
            };
        }

        #endregion
    }
}
