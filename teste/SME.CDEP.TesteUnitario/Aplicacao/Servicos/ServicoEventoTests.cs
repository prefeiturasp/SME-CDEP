using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoEventoTests
    {
        private readonly Mock<IRepositorioEvento> _repositorioEventoMock;
        private readonly ServicoEvento servicoEvento;

        public ServicoEventoTests()
        {
            var mocker = new AutoMocker();
            _repositorioEventoMock = mocker.GetMock<IRepositorioEvento>();
            servicoEvento = mocker.CreateInstance<ServicoEvento>();
        }

        [Fact]
        public async Task DadoConflitoDeEventos_QuandoValidarConflitosAsync_EntaoDeveLancarExcecaoDeNegocio()
        {
            // Arrange
            var datasComConflito = new List<DateTime>
            {
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(2)
            };
            _repositorioEventoMock
                .Setup(r => r.ObterEventosDeFeriadoESuspensaoPorDatas(It.IsAny<DateTime[]>()))
                .ReturnsAsync([datasComConflito[0]]);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() =>
                servicoEvento.ValidarConflitosAsync(datasComConflito));
        }

        [Fact]
        public async Task DadoSemConflitoDeEventos_QuandoValidarConflitosAsync_EntaoNaoDeveLancarExcecao()
        {
            // Arrange
            var datasSemConflito = new List<DateTime>
            {
                DateTime.Now.AddDays(3),
                DateTime.Now.AddDays(4)
            };
            _repositorioEventoMock
                .Setup(r => r.ObterEventosDeFeriadoESuspensaoPorDatas(It.IsAny<DateTime[]>()))
                .ReturnsAsync([]);

            // Act
            await servicoEvento.ValidarConflitosAsync(datasSemConflito);

            // Assert
            _repositorioEventoMock.Verify(r => r.ObterEventosDeFeriadoESuspensaoPorDatas(It.IsAny<DateTime[]>()), Times.Once);
        }

        [Fact]
        public async Task DadoListaVazia_QuandoValidarConflitosAsync_EntaoNaoDeveLancarExcecao()
        {
            // Arrange
            var datasVazia = new List<DateTime>();
            // Act
            await servicoEvento.ValidarConflitosAsync(datasVazia);
            // Assert
            _repositorioEventoMock.Verify(r => r.ObterEventosDeFeriadoESuspensaoPorDatas(It.IsAny<DateTime[]>()), Times.Never);
        }

        [Fact]
        public async Task DadoListaNula_QuandoValidarConflitosAsync_EntaoNaoDeveLancarExcecao()
        {
            // Arrange
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            List<DateTime> datasNula = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            // Act
#pragma warning disable CS8604 // Possible null reference argument.
            await servicoEvento.ValidarConflitosAsync(datasNula);
#pragma warning restore CS8604 // Possible null reference argument.
            // Assert
            _repositorioEventoMock.Verify(r => r.ObterEventosDeFeriadoESuspensaoPorDatas(It.IsAny<DateTime[]>()), Times.Never);
        }
    }
}
