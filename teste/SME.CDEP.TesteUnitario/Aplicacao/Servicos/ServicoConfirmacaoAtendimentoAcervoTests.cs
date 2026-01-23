using Bogus;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Fachadas;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using System.Data;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoConfirmacaoAtendimentoAcervoTests
    {
        private readonly Mock<IServicoEvento> _servicoEventoMock;
        private readonly Mock<IServicoAcervoBibliografico> _servicoAcervoBibliograficoMock;
        private readonly Mock<IServicoProcessamentoSituacaoSolicitacao> _servicoProcessamentoSituacaoMock;
        private readonly Mock<ITransacao> _transacaoMock;
        private readonly Mock<IRepositorioAcervoSolicitacao> _repositorioSolicitacaoMock;
        private readonly Mock<IRepositorioAcervoSolicitacaoItem> _repositorioItemMock;
        private readonly Mock<IServicoUsuario> _servicoUsuarioMock;
        private readonly Mock<IServicoMensageria> _servicoMensageriaMock;
        private readonly ServicoConfirmacaoAtendimentoAcervo _servicoConfirmacaoAtendimentoAcervo;
        private readonly ConfirmacaoAtendimentoRecursos _recursos;
        private readonly Faker _faker;

        public ServicoConfirmacaoAtendimentoAcervoTests()
        {
            var mocker = new AutoMocker();
            _servicoEventoMock = mocker.GetMock<IServicoEvento>();
            _servicoAcervoBibliograficoMock = mocker.GetMock<IServicoAcervoBibliografico>();
            _servicoProcessamentoSituacaoMock = mocker.GetMock<IServicoProcessamentoSituacaoSolicitacao>();
            _transacaoMock = mocker.GetMock<ITransacao>();
            _repositorioSolicitacaoMock = mocker.GetMock<IRepositorioAcervoSolicitacao>();
            _repositorioItemMock = mocker.GetMock<IRepositorioAcervoSolicitacaoItem>();
            _servicoUsuarioMock = mocker.GetMock<IServicoUsuario>();
            _servicoMensageriaMock = mocker.GetMock<IServicoMensageria>();
            _recursos = mocker.CreateInstance<ConfirmacaoAtendimentoRecursos>();
            mocker.Use(_recursos);
            _servicoConfirmacaoAtendimentoAcervo = mocker.CreateInstance<ServicoConfirmacaoAtendimentoAcervo>();
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task DadoUmAcervoSolicitacaoConfirmacaoComItemIdInvalido_QuandoExecutar_EntaoDeveLancarExcecao()
        {
            // Arrange
            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = _faker.Random.Long(1),
                ItemId = _faker.Random.Long(max: 0)
            };
            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servicoConfirmacaoAtendimentoAcervo.Executar(dto));
        }

        [Fact]
        public async Task DadoAtendimentoPorEmailComDataDeVisita_QuandoExecutar_EntaoDeveLancarExcecao()
        {
            // Arrange
            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = _faker.Random.Long(1),
                ItemId = _faker.Random.Long(1),
                TipoAtendimento = TipoAtendimento.Email,
                DataVisita = _faker.Date.Future()
            };
            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servicoConfirmacaoAtendimentoAcervo.Executar(dto));
        }

        [Fact]
        public async Task DadoTipoAcervoDiferenteDeBibliograficoComDataDeEmprestimoEDataDevolucao_QuandoExecutar_EntaoDeveLancarExcecao()
        {
            // Arrange
            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = _faker.Random.Long(1),
                ItemId = _faker.Random.Long(1),
                TipoAcervo = TipoAcervo.DocumentacaoTextual,
                DataEmprestimo = _faker.Date.Past(),
                DataDevolucao = _faker.Date.Future()
            };
            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servicoConfirmacaoAtendimentoAcervo.Executar(dto));
        }

        [Fact]
        public async Task DadoAcervoBibliograficoComDataEmprestimoSemDataDevolucao_QuandoExecutar_EntaoDeveLancarExcecao()
        {
            // Arrange
            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = _faker.Random.Long(1),
                ItemId = _faker.Random.Long(1),
                TipoAcervo = TipoAcervo.Bibliografico,
                DataEmprestimo = _faker.Date.Past(),
                DataDevolucao = null
            };
            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servicoConfirmacaoAtendimentoAcervo.Executar(dto));
        }

        [Fact]
        public async Task DadoAcervoBibliograficoComDataDevolucaoSemDataEmprestimo_QuandoExecutar_EntaoDeveLancarExcecao()
        {
            // Arrange
            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = _faker.Random.Long(1),
                ItemId = _faker.Random.Long(1),
                TipoAcervo = TipoAcervo.Bibliografico,
                DataEmprestimo = null,
                DataDevolucao = _faker.Date.Future()
            };
            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servicoConfirmacaoAtendimentoAcervo.Executar(dto));
        }

        [Fact]
        public async Task DadoAcervoBibliograficoComDataEmprestimoFutura_QuandoExecutar_EntaoDeveLancarExcecao()
        {
            // Arrange
            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = _faker.Random.Long(1),
                ItemId = _faker.Random.Long(1),
                TipoAcervo = TipoAcervo.Bibliografico,
                DataEmprestimo = _faker.Date.Future(),
                DataDevolucao = _faker.Date.Future()
            };
            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servicoConfirmacaoAtendimentoAcervo.Executar(dto));
        }

        [Fact]
        public async Task DadoAcervoBibliograficoComDataEmprestimoMenorQueDataVisita_QuandoExecutar_EntaoDeveLancarExcecao()
        {
            // Arrange
            var dataVisita = _faker.Date.Recent();
            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = _faker.Random.Long(1),
                ItemId = _faker.Random.Long(1),
                TipoAcervo = TipoAcervo.Bibliografico,
                DataVisita = dataVisita,
                DataEmprestimo = dataVisita.AddDays(-1),
                DataDevolucao = dataVisita.AddDays(5)
            };
            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servicoConfirmacaoAtendimentoAcervo.Executar(dto));
        }

        [Fact]
        public async Task DadoAcervoBibliograficoComDataDevolucaoMenorQueDataEmprestimo_QuandoExecutar_EntaoDeveLancarExcecao()
        {
            // Arrange
            var dataEmprestimo = _faker.Date.Past();
            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = _faker.Random.Long(1),
                ItemId = _faker.Random.Long(1),
                TipoAcervo = TipoAcervo.Bibliografico,
                DataEmprestimo = dataEmprestimo,
                DataDevolucao = dataEmprestimo.AddDays(-1),
                DataVisita = dataEmprestimo.AddDays(-1)
            };
            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servicoConfirmacaoAtendimentoAcervo.Executar(dto));
        }

        [Fact]
        public async Task DadoAcervoBibliograficoComDataVisitaFutura_QuandoExecutar_EntaoDeveLancarExcecao()
        {
            // Arrange
            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = _faker.Random.Long(1),
                ItemId = _faker.Random.Long(1),
                TipoAcervo = TipoAcervo.Bibliografico,
                DataVisita = _faker.Date.Future(),
                DataEmprestimo = _faker.Date.Past(),
                DataDevolucao = _faker.Date.Future()
            };
            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servicoConfirmacaoAtendimentoAcervo.Executar(dto));
        }

        [Fact]
        public async Task DadoUmaSolicitacaoInexistente_QuandoExecutar_EntaoDeveLancarExcecao()
        {
            // Arrange
            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = _faker.Random.Long(1),
                ItemId = _faker.Random.Long(1),
                TipoAcervo = TipoAcervo.Bibliografico,
                DataVisita = _faker.Date.Past(),
                DataEmprestimo = _faker.Date.Recent(),
                DataDevolucao = _faker.Date.Future()
            };
            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servicoConfirmacaoAtendimentoAcervo.Executar(dto));
        }

        [Fact]
        public async Task DadoUmItemSolicitacaoInexistente_QuandoExecutar_EntaoDeveLancarExcecao()
        {
            // Arrange
            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = _faker.Random.Long(1),
                ItemId = _faker.Random.Long(1),
                TipoAcervo = TipoAcervo.DocumentacaoTextual,
                TipoAtendimento = TipoAtendimento.Presencial
            };

            _repositorioSolicitacaoMock
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(new AcervoSolicitacao());

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servicoConfirmacaoAtendimentoAcervo.Executar(dto));
        }

        [Fact]
        public async Task DadoUmaSolicitacaoPresencial_QuandoExecutar_DeveAtualizarRegistrosCorretamente()
        {
            // Arrange
            var idSolicitacao = _faker.Random.Long(1);
            var idItem = _faker.Random.Long(1);
            var dataVisita = _faker.Date.Future();
            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = idSolicitacao,
                ItemId = idItem,
                TipoAcervo = TipoAcervo.Fotografico,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataVisita = dataVisita
            };
            _repositorioSolicitacaoMock
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(new AcervoSolicitacao() { Id = idSolicitacao });
            _repositorioItemMock
                .Setup(r => r.ObterItensVigentesPorSolicitacaoIdAsync(It.IsAny<long>()))
                .ReturnsAsync(
                [
                    new ()
                    {
                        Id = idItem,
                        AcervoSolicitacaoId = idSolicitacao,
                        TipoAtendimento = TipoAtendimento.Presencial
                    },
                    new ()
                    {
                        Id = idItem + 1,
                        AcervoSolicitacaoId = idSolicitacao
                    }
                ]);
            _servicoUsuarioMock
                .Setup(s => s.ObterUsuarioLogado())
                .ReturnsAsync(new UsuarioDTO { Id = 1 });

            var mockTransaction = new Mock<IDbTransaction>();

            _transacaoMock.Setup(t => t.Iniciar()).Returns(mockTransaction.Object);

            // Act
            await _servicoConfirmacaoAtendimentoAcervo.Executar(dto);

            // Assert
            _servicoEventoMock.Verify(s => s.ValidarConflitosAsync(It.IsAny<IEnumerable<DateTime>>()), Times.Once);
            _repositorioItemMock.Verify(r => r.Atualizar(It.Is<AcervoSolicitacaoItem>(item =>
                item.Id == idItem &&
                item.AcervoSolicitacaoId == idSolicitacao &&
                item.TipoAtendimento == TipoAtendimento.Presencial &&
                item.DataVisita == dto.DataVisita &&
                item.Situacao == SituacaoSolicitacaoItem.AGUARDANDO_VISITA &&
                item.ResponsavelId == 1
                )), Times.Once);
            _servicoEventoMock.Verify(s => s.AtualizarEventoVisita(dataVisita, idItem), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            _servicoProcessamentoSituacaoMock.Verify(s => s.AtualizarSituacaoGeralSolicitacaoAsync(It.Is<AcervoSolicitacao>(sol => sol.Id == idSolicitacao)), Times.Once);
            _servicoMensageriaMock.Verify(s => s.Publicar(RotasRabbit.NotificarViaEmailConfirmacaoAtendimentoPresencial,
                It.Is<ConfirmarAtendimentoDTO>(c => c.Id == idSolicitacao && c.ItemId == idItem), null), Times.Once);
        }

        [Fact]
        public async Task DadoPresencialSemDataVisita_QuandoExecutar_EntaoDeveAtualizarComSituacaoPresencialEmAberto()
        {
            // Arrange
            var idSolicitacao = _faker.Random.Long(1);
            var idItem = _faker.Random.Long(1);
            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = idSolicitacao,
                ItemId = idItem,
                TipoAcervo = TipoAcervo.Fotografico,
                TipoAtendimento = TipoAtendimento.Presencial
            };
            _repositorioSolicitacaoMock
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(new AcervoSolicitacao() { Id = idSolicitacao });
            _repositorioItemMock
                .Setup(r => r.ObterItensVigentesPorSolicitacaoIdAsync(It.IsAny<long>()))
                .ReturnsAsync(
                [
                    new ()
                    {
                        Id = idItem,
                        AcervoSolicitacaoId = idSolicitacao,
                        TipoAtendimento = TipoAtendimento.Presencial
                    },
                    new ()
                    {
                        Id = idItem + 1,
                        AcervoSolicitacaoId = idSolicitacao
                    }
                ]);
            _servicoUsuarioMock
                .Setup(s => s.ObterUsuarioLogado())
                .ReturnsAsync(new UsuarioDTO { Id = 1 });

            var mockTransaction = new Mock<IDbTransaction>();

            _transacaoMock.Setup(t => t.Iniciar()).Returns(mockTransaction.Object);

            // Act
            await _servicoConfirmacaoAtendimentoAcervo.Executar(dto);

            // Assert
            _servicoEventoMock.Verify(s => s.ValidarConflitosAsync(It.IsAny<IEnumerable<DateTime>>()), Times.Never);
            _repositorioItemMock.Verify(r => r.Atualizar(It.Is<AcervoSolicitacaoItem>(item =>
                item.Id == idItem &&
                item.AcervoSolicitacaoId == idSolicitacao &&
                item.TipoAtendimento == TipoAtendimento.Presencial &&
                !item.DataVisita.HasValue &&
                item.Situacao == SituacaoSolicitacaoItem.PRESENCIAL_ABERTO &&
                item.ResponsavelId == 1
                )), Times.Once);
            _servicoEventoMock.Verify(s => s.AtualizarEventoVisita(It.IsAny<DateTime>(), It.IsAny<long>()), Times.Never);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            _servicoProcessamentoSituacaoMock
                .Verify(s => s.AtualizarSituacaoGeralSolicitacaoAsync(It.Is<AcervoSolicitacao>(sol => sol.Id == idSolicitacao))
                , Times.Once);
            _servicoMensageriaMock.Verify(s => s.Publicar(RotasRabbit.NotificarViaEmailConfirmacaoAtendimentoPresencial,
                It.Is<ConfirmarAtendimentoDTO>(c => c.Id == idSolicitacao && c.ItemId == idItem), null), Times.Once);
        }

        [Fact]
        public async Task DadoMudancaDeAtendimentoParaEmail_QuandoExecutar_EntaoDeveExcluirEvento()
        {
            // Arrange
            var idSolicitacao = _faker.Random.Long(1);
            var idItem = _faker.Random.Long(1);
            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = idSolicitacao,
                ItemId = idItem,
                TipoAcervo = TipoAcervo.Fotografico,
                TipoAtendimento = TipoAtendimento.Email
            };
            _repositorioSolicitacaoMock
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(new AcervoSolicitacao() { Id = idSolicitacao });
            _repositorioItemMock
                .Setup(r => r.ObterItensVigentesPorSolicitacaoIdAsync(It.IsAny<long>()))
                .ReturnsAsync(
                [
                    new ()
                    {
                        Id = idItem,
                        AcervoSolicitacaoId = idSolicitacao,
                        TipoAtendimento = TipoAtendimento.Presencial
                    }
                ]);
            _servicoUsuarioMock
                .Setup(s => s.ObterUsuarioLogado())
                .ReturnsAsync(new UsuarioDTO { Id = 1 });

            var mockTransaction = new Mock<IDbTransaction>();

            _transacaoMock.Setup(t => t.Iniciar()).Returns(mockTransaction.Object);

            // Act
            await _servicoConfirmacaoAtendimentoAcervo.Executar(dto);

            // Assert
            _servicoEventoMock.Verify(s => s.ExcluirEventoPorAcervoSolicitacaoItem(idItem), Times.Once);
        }

        [Fact]
        public async Task DadoBibliograficoComDataVisitaRemovida_QuandoExecutar_DeveAlterarSaldoDoAcervoParaDisponivel()
        {
            // Arrange
            var idSolicitacao = _faker.Random.Long(1);
            var idItem = _faker.Random.Long(1);
            var idAcervo = _faker.Random.Long(1);
            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = idSolicitacao,
                ItemId = idItem,
                TipoAcervo = TipoAcervo.Bibliografico,
                TipoAtendimento = TipoAtendimento.Presencial
            };
            _repositorioSolicitacaoMock
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(new AcervoSolicitacao() { Id = idSolicitacao });
            _repositorioItemMock
                .Setup(r => r.ObterItensVigentesPorSolicitacaoIdAsync(It.IsAny<long>()))
                .ReturnsAsync(
                [
                    new ()
                    {
                        Id = idItem,
                        AcervoSolicitacaoId = idSolicitacao,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = _faker.Date.Future(),
                        AcervoId = idAcervo
                    }
                ]);
            _servicoUsuarioMock
                .Setup(s => s.ObterUsuarioLogado())
                .ReturnsAsync(new UsuarioDTO { Id = 1 });

            var mockTransaction = new Mock<IDbTransaction>();

            _transacaoMock.Setup(t => t.Iniciar()).Returns(mockTransaction.Object);

            // Act
            await _servicoConfirmacaoAtendimentoAcervo.Executar(dto);

            // Assert
            _servicoEventoMock.Verify(s => s.ExcluirEventoPorAcervoSolicitacaoItem(idItem), Times.Once);
            _servicoEventoMock.Verify(s => s.AtualizarEventoVisita(It.IsAny<DateTime>(), It.IsAny<long>()), Times.Never);
            _servicoAcervoBibliograficoMock.Verify(s => s.AlterarSituacaoSaldo(SituacaoSaldo.DISPONIVEL, idAcervo), Times.Once);
            _servicoAcervoBibliograficoMock.Verify(s => s.GerenciarEmprestimoAsync(idItem, idAcervo, null, null), Times.Once);
        }

        [Fact]
        public async Task DadoQuePossuiInformacoesDeEmprestimo_QuandoExecutar_EntaoSituacaoDoItemDeveSerFinalizadoManualmente()
        {
            // Arrange
            var idSolicitacao = _faker.Random.Long(1);
            var idItem = _faker.Random.Long(1);
            var idAcervo = _faker.Random.Long(1);
            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = idSolicitacao,
                ItemId = idItem,
                TipoAcervo = TipoAcervo.Bibliografico,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataVisita = _faker.Date.Past(),
                DataEmprestimo = _faker.Date.Recent(),
                DataDevolucao = _faker.Date.Future()
            };
            _repositorioSolicitacaoMock
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(new AcervoSolicitacao() { Id = idSolicitacao });
            _repositorioItemMock
                .Setup(r => r.ObterItensVigentesPorSolicitacaoIdAsync(It.IsAny<long>()))
                .ReturnsAsync(
                [
                    new ()
                    {
                        Id = idItem,
                        AcervoSolicitacaoId = idSolicitacao,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = _faker.Date.Future(),
                        AcervoId = idAcervo
                    }
                ]);
            _servicoUsuarioMock
                .Setup(s => s.ObterUsuarioLogado())
                .ReturnsAsync(new UsuarioDTO { Id = 1 });

            var mockTransaction = new Mock<IDbTransaction>();

            _transacaoMock.Setup(t => t.Iniciar()).Returns(mockTransaction.Object);

            // Act
            await _servicoConfirmacaoAtendimentoAcervo.Executar(dto);

            // Assert
            _repositorioItemMock.Verify(r => r.Atualizar(It.Is<AcervoSolicitacaoItem>(item =>
                item.Id == idItem &&
                item.AcervoSolicitacaoId == idSolicitacao &&
                item.Situacao == SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE
                )), Times.Once);
        }
        [Fact]
        public async Task DadoPresencialComDataVisitaRemovida_QuandoExecutar_DeveExcluirEventoAnterior()
        {
            // Arrange
            var idSolicitacao = _faker.Random.Long(1);
            var idItem = _faker.Random.Long(1);
            var idAcervo = _faker.Random.Long(1);

            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = idSolicitacao,
                ItemId = idItem,
                TipoAcervo = TipoAcervo.Bibliografico,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataVisita = null
            };

            ConfigurarMocksPadrao(idSolicitacao, idItem, idAcervo, eraPresencial: true, dataVisitaAntiga: DateTime.Now);

            // Act
            await _servicoConfirmacaoAtendimentoAcervo.Executar(dto);

            // Assert
            _servicoEventoMock.Verify(s => s.ExcluirEventoPorAcervoSolicitacaoItem(idItem), Times.Once);
            _servicoAcervoBibliograficoMock.Verify(s => s.AlterarSituacaoSaldo(SituacaoSaldo.DISPONIVEL, idAcervo), Times.Once);
        }

        [Fact]
        public async Task DadoBibliograficoComDatasEmprestimoMasVisitaFutura_QuandoExecutar_EntaoDeveLancarExcecao()
        {
            // Arrangem
            var dataVisitaFutura = DateTime.Now.AddDays(30);

            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = _faker.Random.Long(1),
                ItemId = _faker.Random.Long(1),
                TipoAcervo = TipoAcervo.Bibliografico,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataVisita = dataVisitaFutura,
                DataEmprestimo = DateTime.Now,
                DataDevolucao = DateTime.Now.AddDays(7)
            };

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servicoConfirmacaoAtendimentoAcervo.Executar(dto));
        }

        [Fact]
        public async Task DadoErroNoProcessamento_QuandoExecutar_DeveRealizarRollbackDaTransacao()
        {
            // Arrange
            var idSolicitacao = _faker.Random.Long(1);
            var dto = new AcervoSolicitacaoConfirmarDto { Id = idSolicitacao, ItemId = 1, TipoAtendimento = TipoAtendimento.Email };

            _repositorioSolicitacaoMock.Setup(r => r.ObterPorId(It.IsAny<long>())).ReturnsAsync(new AcervoSolicitacao());
            _servicoUsuarioMock.Setup(s => s.ObterUsuarioLogado()).ReturnsAsync(new UsuarioDTO());

            _repositorioItemMock.Setup(r => r.ObterItensVigentesPorSolicitacaoIdAsync(It.IsAny<long>()))
                .ReturnsAsync([new AcervoSolicitacaoItem { Id = 1, TipoAtendimento = TipoAtendimento.Presencial }]);

            _servicoEventoMock.Setup(s => s.ExcluirEventoPorAcervoSolicitacaoItem(It.IsAny<long>()))
                .ThrowsAsync(new Exception("Erro crítico no serviço de eventos"));

            var mockTransaction = new Mock<IDbTransaction>();
            _transacaoMock.Setup(t => t.Iniciar()).Returns(mockTransaction.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _servicoConfirmacaoAtendimentoAcervo.Executar(dto));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }
        private void ConfigurarMocksPadrao(long idSolicitacao, long idItem, long idAcervo, bool eraPresencial, DateTime? dataVisitaAntiga = null)
        {
            _repositorioSolicitacaoMock
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(new AcervoSolicitacao { Id = idSolicitacao });

            _repositorioItemMock
                .Setup(r => r.ObterItensVigentesPorSolicitacaoIdAsync(It.IsAny<long>()))
                .ReturnsAsync([
                    new AcervoSolicitacaoItem {
                        Id = idItem,
                        AcervoSolicitacaoId = idSolicitacao,
                        TipoAtendimento = eraPresencial ? TipoAtendimento.Presencial : TipoAtendimento.Email,
                        DataVisita = dataVisitaAntiga,
                        AcervoId = idAcervo
                    }
                ]);

            _servicoUsuarioMock.Setup(s => s.ObterUsuarioLogado()).ReturnsAsync(new UsuarioDTO { Id = 1 });

            var mockTransaction = new Mock<IDbTransaction>();
            _transacaoMock.Setup(t => t.Iniciar()).Returns(mockTransaction.Object);
        }
    }
}