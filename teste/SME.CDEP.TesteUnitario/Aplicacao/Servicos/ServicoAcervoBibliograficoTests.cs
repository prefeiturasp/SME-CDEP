using Bogus;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoAcervoBibliograficoTests
    {
        private readonly AutoMocker _mocker;
        private readonly ServicoAcervoBibliografico _servico;
        private readonly Faker _faker;

        public ServicoAcervoBibliograficoTests()
        {
            _mocker = new AutoMocker();
            _servico = _mocker.CreateInstance<ServicoAcervoBibliografico>();
            _faker = new Faker("pt_BR");
        }

        #region AlterarSituacaoSaldo

        [Fact(DisplayName = "Dado um acervo existente, Quando AlterarSituacaoSaldo, Então deve atualizar o saldo no banco")]
        public async Task AlterarSituacaoSaldo_AcervoExistente_DeveAtualizar()
        {
            // Arrange
            var acervoId = _faker.Random.Long(1);
            var situacaoEsperada = SituacaoSaldo.EMPRESTADO;
            var acervoBibliografico = new AcervoBibliografico { AcervoId = acervoId, SituacaoSaldo = SituacaoSaldo.DISPONIVEL };

            _mocker.GetMock<IRepositorioAcervoBibliografico>()
                .Setup(r => r.ObterPorAcervoId(acervoId))
                .ReturnsAsync(acervoBibliografico);

            // Act
            var resultado = await _servico.AlterarSituacaoSaldo(situacaoEsperada, acervoId);

            // Assert
            resultado.Should().BeTrue();
            acervoBibliografico.SituacaoSaldo.Should().Be(situacaoEsperada); // Verifica alteração na memória

            _mocker.GetMock<IRepositorioAcervoBibliografico>()
                .Verify(r => r.Atualizar(acervoBibliografico), Times.Once);
        }

        [Fact(DisplayName = "Dado um acervo inexistente, Quando AlterarSituacaoSaldo, Então deve retornar falso e não atualizar")]
        public async Task AlterarSituacaoSaldo_AcervoInexistente_DeveRetornarFalso()
        {
            // Act
            var resultado = await _servico.AlterarSituacaoSaldo(SituacaoSaldo.RESERVADO, 1);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<IRepositorioAcervoBibliografico>()
                .Verify(r => r.Atualizar(It.IsAny<AcervoBibliografico>()), Times.Never);
        }

        #endregion

        #region GerenciarEmprestimoAsync (Fluxo Confirmação)

        [Fact(DisplayName = "Dado que já existe empréstimo, Quando GerenciarEmprestimo, Então deve lançar exceção")]
        public async Task GerenciarEmprestimo_ItemJaEmprestado_DeveLancarExcecao()
        {
            // Arrange
            var itemId = 1L;
            _mocker.GetMock<IRepositorioAcervoEmprestimo>()
                .Setup(r => r.ObterUltimoEmprestimoPorAcervoSolicitacaoItemId(itemId))
                .ReturnsAsync(new AcervoEmprestimo()); // Retorna algo, simulando existência

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() =>
                _servico.GerenciarEmprestimoAsync(itemId, 1, DateTime.Now, DateTime.Now));
        }

        [Fact(DisplayName = "Dado datas válidas, Quando GerenciarEmprestimo, Então deve inserir empréstimo e marcar saldo como EMPRESTADO")]
        public async Task GerenciarEmprestimo_ComDatas_DeveInserirEAtualizarSaldo()
        {
            // Arrange
            var itemId = _faker.Random.Long(1);
            var acervoId = _faker.Random.Long(1);
            var dataEmp = DateTime.Now;
            var dataDev = DateTime.Now.AddDays(7);

            // Mock necessário para o AlterarSituacaoSaldo funcionar internamente
            _mocker.GetMock<IRepositorioAcervoBibliografico>()
                .Setup(r => r.ObterPorAcervoId(acervoId))
                .ReturnsAsync(new AcervoBibliografico());

            // Act
            await _servico.GerenciarEmprestimoAsync(itemId, acervoId, dataEmp, dataDev);

            // Assert
            _mocker.GetMock<IRepositorioAcervoEmprestimo>()
                .Verify(r => r.Inserir(It.Is<AcervoEmprestimo>(e =>
                    e.AcervoSolicitacaoItemId == itemId &&
                    e.Situacao == SituacaoEmprestimo.EMPRESTADO &&
                    e.DataEmprestimo == dataEmp &&
                    e.DataDevolucao == dataDev
                )), Times.Once);

            _mocker.GetMock<IRepositorioAcervoBibliografico>()
                .Verify(r => r.Atualizar(It.Is<AcervoBibliografico>(a => a.SituacaoSaldo == SituacaoSaldo.EMPRESTADO)), Times.Once);
        }

        [Fact(DisplayName = "Dado sem datas de empréstimo, Quando GerenciarEmprestimo, Então deve apenas reservar saldo")]
        public async Task GerenciarEmprestimo_SemDatas_DeveApenasReservar()
        {
            // Arrange
            var itemId = 1L;
            var acervoId = 2L;

            _mocker.GetMock<IRepositorioAcervoBibliografico>()
                .Setup(r => r.ObterPorAcervoId(acervoId))
                .ReturnsAsync(new AcervoBibliografico());

            // Act (Passando null nas datas)
            await _servico.GerenciarEmprestimoAsync(itemId, acervoId, null, null);

            // Assert
            // Não deve inserir empréstimo
            _mocker.GetMock<IRepositorioAcervoEmprestimo>().Verify(r => r.Inserir(It.IsAny<AcervoEmprestimo>()), Times.Never);

            // Deve atualizar saldo para RESERVADO
            _mocker.GetMock<IRepositorioAcervoBibliografico>()
                .Verify(r => r.Atualizar(It.Is<AcervoBibliografico>(a => a.SituacaoSaldo == SituacaoSaldo.RESERVADO)), Times.Once);
        }

        #endregion

        #region AtualizarOuCriarEmprestimoAsync (Fluxo Manutenção)

        [Fact(DisplayName = "Dado empréstimo inexistente, Quando AtualizarOuCriar, Então deve inserir novo")]
        public async Task AtualizarOuCriarEmprestimo_Inexistente_DeveInserir()
        {
            // Arrange
            var itemId = _faker.Random.Long(1);
            var acervoId = _faker.Random.Long(1);

            _mocker.GetMock<IRepositorioAcervoBibliografico>()
                .Setup(r => r.ObterPorAcervoId(acervoId))
                .ReturnsAsync(new AcervoBibliografico());

            // Act
            await _servico.AtualizarOuCriarEmprestimoAsync(itemId, acervoId, DateTime.Now, DateTime.Now.AddDays(7));

            // Assert
            _mocker.GetMock<IRepositorioAcervoEmprestimo>()
                .Verify(r => r.Inserir(It.IsAny<AcervoEmprestimo>()), Times.Once);

            _mocker.GetMock<IRepositorioAcervoEmprestimo>()
                .Verify(r => r.Atualizar(It.IsAny<AcervoEmprestimo>()), Times.Never);
        }

        [Fact(DisplayName = "Dado empréstimo existente, Quando AtualizarOuCriar, Então deve atualizar dados")]
        public async Task AtualizarOuCriarEmprestimo_Existente_DeveAtualizar()
        {
            // Arrange
            var itemId = _faker.Random.Long(1);
            var emprestimoExistente = new AcervoEmprestimo { Id = 10, AcervoSolicitacaoItemId = itemId };
            var novaDataDevolucao = DateTime.Now.AddDays(15);

            _mocker.GetMock<IRepositorioAcervoEmprestimo>()
                .Setup(r => r.ObterUltimoEmprestimoPorAcervoSolicitacaoItemId(itemId))
                .ReturnsAsync(emprestimoExistente);

            _mocker.GetMock<IRepositorioAcervoBibliografico>()
                .Setup(r => r.ObterPorAcervoId(It.IsAny<long>()))
                .ReturnsAsync(new AcervoBibliografico());

            // Act
            await _servico.AtualizarOuCriarEmprestimoAsync(itemId, 1, DateTime.Now, novaDataDevolucao);

            // Assert
            _mocker.GetMock<IRepositorioAcervoEmprestimo>()
                .Verify(r => r.Atualizar(It.Is<AcervoEmprestimo>(e =>
                    e.Id == 10 &&
                    e.DataDevolucao == novaDataDevolucao &&
                    e.Situacao == SituacaoEmprestimo.EMPRESTADO
                )), Times.Once);

            _mocker.GetMock<IRepositorioAcervoEmprestimo>()
                .Verify(r => r.Inserir(It.IsAny<AcervoEmprestimo>()), Times.Never);
        }

        #endregion
    }
}
