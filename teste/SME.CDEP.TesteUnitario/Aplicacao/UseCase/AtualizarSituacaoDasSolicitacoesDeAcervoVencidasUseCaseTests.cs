using Bogus;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.UseCase;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase
{
    public class AtualizarSituacaoDasSolicitacoesDeAcervoVencidasUseCaseTests
    {
        private readonly Mock<IRepositorioAcervoSolicitacaoItem> _repositorioAcervoMock;
        private readonly Mock<IRepositorioParametroSistema> _repositorioParametroSistemaMock;
        private readonly AtualizarSituacaoDasSolicitacoesDeAcervoVencidasUseCase _useCase;
        private readonly Faker _faker;

        public AtualizarSituacaoDasSolicitacoesDeAcervoVencidasUseCaseTests()
        {
            var mocker = new AutoMocker();
            _repositorioAcervoMock = mocker.GetMock<IRepositorioAcervoSolicitacaoItem>();
            _repositorioParametroSistemaMock = mocker.GetMock<IRepositorioParametroSistema>();
            _useCase = mocker.CreateInstance<AtualizarSituacaoDasSolicitacoesDeAcervoVencidasUseCase>();
            _faker = new("pt_BR");
        }

        [Fact]
        public async Task DadoParametroInexistente_QuandoExecutar_EntaoDeveLancarExcecao() => await Assert.ThrowsAsync<NegocioException>(async () => await _useCase.Executar(new()));

        [Fact]
        public async Task DadoParametroInvalido_QuandoExecutar_EntaoDeveLancarExcecao()
        {
            var parametroInvalido = new ParametroSistema
            {
                Tipo = TipoParametroSistema.PrazoEncerramentoAutomaticoSolicitacao,
                Valor = _faker.Lorem.Word()
            };
            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoAsync(TipoParametroSistema.PrazoEncerramentoAutomaticoSolicitacao))
                .ReturnsAsync(parametroInvalido);
            await Assert.ThrowsAsync<NegocioException>(async () => await _useCase.Executar(new()));
        }

        [Fact]
        public async Task DadoNenhumaSolicitacaoVencida_QuandoExecutar_EntaoNaoDeveAtualizarNenhumaSolicitacao()
        {
            // Arrange
            var parametroValido = new ParametroSistema
            {
                Tipo = TipoParametroSistema.PrazoEncerramentoAutomaticoSolicitacao,
                Valor = _faker.Random.Int(1).ToString()
            };
            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoAsync(TipoParametroSistema.PrazoEncerramentoAutomaticoSolicitacao))
                .ReturnsAsync(parametroValido);
            _repositorioAcervoMock
                .Setup(r => r.ObterSolicitacoesDeAcervoVencidasAsync(It.IsAny<List<SituacaoSolicitacaoItem>>(), It.IsAny<int>()))
                .ReturnsAsync([]);

            // Act
            await _useCase.Executar(new());

            // Assert
            _repositorioAcervoMock.Verify(r => r.AtualizarSituacaoSolicitacaoItemAsync(It.IsAny<long>(), It.IsAny<SituacaoSolicitacaoItem>()), Times.Never);
        }

        [Fact]
        public async Task DadoSolicitacoesVencidasNull_QuandoExecutar_EntaoNaoDeveAtualizarNenhumaSolicitacao()
        {
            // Arrange
            var parametroValido = new ParametroSistema
            {
                Tipo = TipoParametroSistema.PrazoEncerramentoAutomaticoSolicitacao,
                Valor = _faker.Random.Int(1).ToString()
            };
            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoAsync(TipoParametroSistema.PrazoEncerramentoAutomaticoSolicitacao))
                .ReturnsAsync(parametroValido);

            // Act
            await _useCase.Executar(new ());

            // Assert
            _repositorioAcervoMock.Verify(r => r.AtualizarSituacaoSolicitacaoItemAsync(It.IsAny<long>(), It.IsAny<SituacaoSolicitacaoItem>()), Times.Never);
        }

        [Fact]
        public async Task DadoSolicitacoesVencidas_QuandoExecutar_EntaoDeveAtualizarSituacaoDasSolicitacoes()
        {
            // Arrange
            var parametroValido = new ParametroSistema
            {
                Tipo = TipoParametroSistema.PrazoEncerramentoAutomaticoSolicitacao,
                Valor = _faker.Random.Int(1).ToString()
            };
            var solicitacoesVencidas = new List<long?> { 1, 2, 3 };

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoAsync(TipoParametroSistema.PrazoEncerramentoAutomaticoSolicitacao))
                .ReturnsAsync(parametroValido);
            _repositorioAcervoMock
                .Setup(r => r.ObterSolicitacoesDeAcervoVencidasAsync(It.IsAny<List<SituacaoSolicitacaoItem>>(), It.IsAny<int>()))
                .ReturnsAsync(solicitacoesVencidas);

            // Act
            await _useCase.Executar(new());

            // Assert
            _repositorioAcervoMock
                .Verify(r => r.AtualizarSituacaoSolicitacaoItemAsync(It.IsAny<long?>(), It.IsAny<SituacaoSolicitacaoItem>())
                , Times.Exactly(solicitacoesVencidas.Count));
            _repositorioAcervoMock.Verify(r => r.AtualizarSituacaoSolicitacaoItemAsync(1, SituacaoSolicitacaoItem.SEM_RESPOSTA_SOLICITANTE), Times.Once);
            _repositorioAcervoMock.Verify(r => r.AtualizarSituacaoSolicitacaoItemAsync(2, SituacaoSolicitacaoItem.SEM_RESPOSTA_SOLICITANTE), Times.Once);
            _repositorioAcervoMock.Verify(r => r.AtualizarSituacaoSolicitacaoItemAsync(3, SituacaoSolicitacaoItem.SEM_RESPOSTA_SOLICITANTE), Times.Once);
        }
    }
}