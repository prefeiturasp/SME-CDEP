using Bogus;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoProcessamentoSituacaoSolicitacaoTests
    {
        private readonly Mock<IRepositorioAcervoSolicitacao> _repositorioSolicitacaoMock;
        private readonly Mock<IRepositorioAcervoSolicitacaoItem> _repositorioItemMock;
        private readonly ServicoProcessamentoSituacaoSolicitacao servicoProcessamentoSituacaoSolicitacao;
        private readonly Faker _faker;

        public ServicoProcessamentoSituacaoSolicitacaoTests()
        {
            var mocker = new AutoMocker();
            _repositorioSolicitacaoMock = mocker.GetMock<IRepositorioAcervoSolicitacao>();
            _repositorioItemMock = mocker.GetMock<IRepositorioAcervoSolicitacaoItem>();
            servicoProcessamentoSituacaoSolicitacao = mocker.CreateInstance<ServicoProcessamentoSituacaoSolicitacao>();
            _faker = new();
        }

        [Fact]
        public async Task DadoTodosOsItensCancelados_QuandoAtualizarSituacaoGeralSolicitacao_EntaoSituacaoDoAcervoDeveSerCancelado()
        {
            // Arrange
            var acervoSolicitacao = new AcervoSolicitacao
            {
                Id = _faker.Random.Long(1),
                Situacao = SituacaoSolicitacao.ATENDIDO_PARCIALMENTE
            };

            // Act
            await servicoProcessamentoSituacaoSolicitacao.AtualizarSituacaoGeralSolicitacaoAsync(acervoSolicitacao, true);

            // Assert
            _repositorioSolicitacaoMock
                .Verify(r => r.Atualizar(It.Is<AcervoSolicitacao>(a =>
                    a.Situacao == SituacaoSolicitacao.CANCELADO &&
                    a.Id == acervoSolicitacao.Id))
                , Times.Once);
        }

        [Fact]
        public async Task DadoTodosOsItensFinalizadosManualmente_QuandoAtualizarSituacaoGeralSolicitacao_EntaoSituacaoDoAcervoDeveSerFinalizadoAtendimento()
        {
            // Arrange
            var acervoSolicitacao = new AcervoSolicitacao
            {
                Id = _faker.Random.Long(1),
                Situacao = SituacaoSolicitacao.AGUARDANDO_VISITA
            };
            var itens = new List<AcervoSolicitacaoItem>
            {
                new() { Situacao = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE },
                new() { Situacao = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE }
            };
            _repositorioItemMock
                .Setup(r => r.ObterItensVigentesPorSolicitacaoIdAsync(acervoSolicitacao.Id))
                .ReturnsAsync(itens);
            // Act
            await servicoProcessamentoSituacaoSolicitacao.AtualizarSituacaoGeralSolicitacaoAsync(acervoSolicitacao);
            // Assert
            _repositorioSolicitacaoMock
                .Verify(r => r.Atualizar(It.Is<AcervoSolicitacao>(a =>
                    a.Situacao == SituacaoSolicitacao.FINALIZADO_ATENDIMENTO &&
                    a.Id == acervoSolicitacao.Id))
                , Times.Once);
        }

        [Fact]
        public async Task DadoItensComSituacaoAguardandoAtendimento_QuandoAtualizarSituacaoGeralSolicitacao_EntaoSituacaoDoAcervoDeveSerAtendidoParcialmente()
        {
            // Arrange
            var acervoSolicitacao = new AcervoSolicitacao
            {
                Id = _faker.Random.Long(1),
                Situacao = SituacaoSolicitacao.AGUARDANDO_VISITA
            };
            var itens = new List<AcervoSolicitacaoItem>
            {
                new() { Situacao = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE },
                new() { Situacao = SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO },
                new() { Situacao = SituacaoSolicitacaoItem.AGUARDANDO_VISITA },
                new() { Situacao = SituacaoSolicitacaoItem.PRESENCIAL_ABERTO }
            };
            _repositorioItemMock
                .Setup(r => r.ObterItensVigentesPorSolicitacaoIdAsync(acervoSolicitacao.Id))
                .ReturnsAsync(itens);
            // Act
            await servicoProcessamentoSituacaoSolicitacao.AtualizarSituacaoGeralSolicitacaoAsync(acervoSolicitacao);
            // Assert
            _repositorioSolicitacaoMock
                .Verify(r => r.Atualizar(It.Is<AcervoSolicitacao>(a =>
                    a.Situacao == SituacaoSolicitacao.ATENDIDO_PARCIALMENTE &&
                    a.Id == acervoSolicitacao.Id))
                , Times.Once);
        }

        [Fact]
        public async Task DadoItensComSituacaoAguardandoVisita_QuandoAtualizarSituacaoGeralSolicitacao_EntaoSituacaoDoAcervoDeveSerAguardandoVisita()
        {
            // Arrange
            var acervoSolicitacao = new AcervoSolicitacao
            {
                Id = _faker.Random.Long(1),
                Situacao = SituacaoSolicitacao.PRESENCIAL_ABERTO
            };
            var itens = new List<AcervoSolicitacaoItem>
            {
                new() { Situacao = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE },
                new() { Situacao = SituacaoSolicitacaoItem.AGUARDANDO_VISITA },
                new() { Situacao = SituacaoSolicitacaoItem.PRESENCIAL_ABERTO }
            };
            _repositorioItemMock
                .Setup(r => r.ObterItensVigentesPorSolicitacaoIdAsync(acervoSolicitacao.Id))
                .ReturnsAsync(itens);
            // Act
            await servicoProcessamentoSituacaoSolicitacao.AtualizarSituacaoGeralSolicitacaoAsync(acervoSolicitacao);
            // Assert
            _repositorioSolicitacaoMock
                .Verify(r => r.Atualizar(It.Is<AcervoSolicitacao>(a =>
                    a.Situacao == SituacaoSolicitacao.AGUARDANDO_VISITA &&
                    a.Id == acervoSolicitacao.Id))
                , Times.Once);
        }

        [Fact]
        public async Task DadoItensComSituacaoPresencialAberto_QuandoAtualizarSituacaoGeralSolicitacao_EntaoSituacaoDoAcervoDeveSerPresencialAberto()
        {
            // Arrange
            var acervoSolicitacao = new AcervoSolicitacao
            {
                Id = _faker.Random.Long(1),
                Situacao = SituacaoSolicitacao.AGUARDANDO_VISITA
            };
            var itens = new List<AcervoSolicitacaoItem>
            {
                new() { Situacao = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE },
                new() { Situacao = SituacaoSolicitacaoItem.PRESENCIAL_ABERTO }
            };
            _repositorioItemMock
                .Setup(r => r.ObterItensVigentesPorSolicitacaoIdAsync(acervoSolicitacao.Id))
                .ReturnsAsync(itens);
            // Act
            await servicoProcessamentoSituacaoSolicitacao.AtualizarSituacaoGeralSolicitacaoAsync(acervoSolicitacao);
            // Assert
            _repositorioSolicitacaoMock
                .Verify(r => r.Atualizar(It.Is<AcervoSolicitacao>(a =>
                    a.Situacao == SituacaoSolicitacao.PRESENCIAL_ABERTO &&
                    a.Id == acervoSolicitacao.Id))
                , Times.Once);
        }

        [Fact]
        public async Task DadoItensSemSituacaoMapeada_QuandoAtualizarSituacaoGeralSolicitacao_EntaoDeveLancarNegocioException()
        {
            // Arrange
            var acervoSolicitacao = new AcervoSolicitacao
            {
                Id = _faker.Random.Long(1),
                Situacao = SituacaoSolicitacao.AGUARDANDO_VISITA
            };
            var itens = new List<AcervoSolicitacaoItem>
            {
                new() { Situacao = (SituacaoSolicitacaoItem)999 }
            };
            _repositorioItemMock
                .Setup(r => r.ObterItensVigentesPorSolicitacaoIdAsync(acervoSolicitacao.Id))
                .ReturnsAsync(itens);
            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(async () =>
                await servicoProcessamentoSituacaoSolicitacao.AtualizarSituacaoGeralSolicitacaoAsync(acervoSolicitacao));
        }
    }
}
