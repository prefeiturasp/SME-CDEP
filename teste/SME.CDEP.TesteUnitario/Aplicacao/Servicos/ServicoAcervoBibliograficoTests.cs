using AutoMapper;
using Bogus;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Enumerados;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using System.Data;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoAcervoBibliograficoTestes
    {
        private readonly Mock<IRepositorioAcervoBibliograficoAssunto> repositorioAcervoBibliograficoAssuntoMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<ITransacao> transacaoMock;
        private readonly Mock<IRepositorioAcervoBibliografico> repositorioAcervoBibliograficoMock;
        private readonly Mock<IRepositorioAssunto> repositorioAssuntoMock;
        private readonly Mock<IServicoAcervo> servicoAcervoMock;
        private readonly Mock<IRepositorioAcervoEmprestimo> repositorioAcervoEmprestimoMock;
        private readonly Mock<IDbTransaction> dbTransactionMock;
        private readonly ServicoAcervoBibliografico sut;

        public ServicoAcervoBibliograficoTestes()
        {
            var mocker = new AutoMocker();

            repositorioAcervoBibliograficoAssuntoMock = mocker.GetMock<IRepositorioAcervoBibliograficoAssunto>();
            mapperMock = mocker.GetMock<IMapper>();
            transacaoMock = mocker.GetMock<ITransacao>();
            repositorioAcervoBibliograficoMock = mocker.GetMock<IRepositorioAcervoBibliografico>();
            repositorioAssuntoMock = mocker.GetMock<IRepositorioAssunto>();
            servicoAcervoMock = mocker.GetMock<IServicoAcervo>();
            repositorioAcervoEmprestimoMock = mocker.GetMock<IRepositorioAcervoEmprestimo>();
            dbTransactionMock = new Mock<IDbTransaction>();

            transacaoMock.Setup(t => t.Iniciar()).Returns(dbTransactionMock.Object);

            sut = mocker.CreateInstance<ServicoAcervoBibliografico>();
        }

        [Fact]
        public async Task DadoDtoValido_QuandoInserir_EntaoInsereAcervoAcervoBibliograficoEAssuntosComitaTransacaoERetornaId()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDTO();
            var acervo = new Acervo();
            var acervoBibliografico = new AcervoBibliografico();
            var assuntos = new List<Assunto> { new() { Id = 1 }, new() { Id = 2 } };
            var idGerado = 99L;

            repositorioAssuntoMock.Setup(r => r.ObterPorIds(dto.AssuntosIds)).ReturnsAsync(assuntos);
            mapperMock.Setup(m => m.Map<Acervo>(dto)).Returns(acervo);
            mapperMock.Setup(m => m.Map<AcervoBibliografico>(dto)).Returns(acervoBibliografico);
            servicoAcervoMock.Setup(s => s.Inserir(acervo)).ReturnsAsync(idGerado);
            repositorioAcervoBibliograficoMock.Setup(r => r.Inserir(acervoBibliografico)).ReturnsAsync(idGerado);

            // Act
            var resultado = await sut.Inserir(dto);

            // Assert
            resultado.Should().Be(idGerado);
            acervo.Situacao.Should().Be(SituacaoAcervo.Ativo);
            acervo.TipoAcervoId.Should().Be((int)TipoAcervo.Bibliografico);
            acervoBibliografico.AcervoId.Should().Be(idGerado);

            servicoAcervoMock.Verify(s => s.Inserir(acervo), Times.Once);
            repositorioAcervoBibliograficoMock.Verify(r => r.Inserir(acervoBibliografico), Times.Once);
            repositorioAcervoBibliograficoAssuntoMock.Verify(r => r.Inserir(It.IsAny<AcervoBibliograficoAssunto>()), Times.Exactly(assuntos.Count));
            dbTransactionMock.Verify(t => t.Commit(), Times.Once);
        }

        [Fact]
        public async Task DadoLarguraComFormatoInvalido_QuandoInserir_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDTO();
            dto.Largura = "10.5";

            // Act
            Func<Task> acao = async () => await sut.Inserir(dto);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.LARGURA));
        }

        [Fact]
        public async Task DadoAlturaComFormatoInvalido_QuandoInserir_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDTO();
            dto.Altura = "10.5";

            // Act
            Func<Task> acao = async () => await sut.Inserir(dto);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.ALTURA));
        }

        [Fact]
        public async Task DadoExcecaoLancadaPorQualquerDependencia_QuandoInserir_EntaoRealizaRollbackNaTransacaoERelancaExcecao()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDTO();
            var assuntos = new List<Assunto> { new() { Id = 1 }, new() { Id = 2 } };
            repositorioAssuntoMock.Setup(r => r.ObterPorIds(dto.AssuntosIds)).ReturnsAsync(assuntos);
            mapperMock.Setup(m => m.Map<Acervo>(dto)).Returns(new Acervo());
            servicoAcervoMock.Setup(r => r.Inserir(It.IsAny<Acervo>())).ThrowsAsync(new Exception());

            // Act
            Func<Task> acao = async () => await sut.Inserir(dto);

            // Assert
            await acao.Should().ThrowAsync<Exception>();
            dbTransactionMock.Verify(t => t.Rollback(), Times.Once);
        }

        [Fact]
        public async Task DadoAcervosCadastrados_QuandoObterTodos_EntaoRetornaListaDeAcervosBibliograficosDto()
        {
            // Arrange
            var entidades = new List<AcervoBibliografico> { new(), new() };
            var dtos = new List<AcervoBibliograficoDTO> { new(), new() };

            repositorioAcervoBibliograficoMock.Setup(r => r.ObterTodos()).ReturnsAsync(entidades);
            mapperMock.Setup(m => m.Map<AcervoBibliograficoDTO>(It.IsAny<AcervoBibliografico>())).Returns(dtos[0]);

            // Act
            var resultado = await sut.ObterTodos();

            // Assert
            resultado.Should().HaveCount(2);
            repositorioAcervoBibliograficoMock.Verify(r => r.ObterTodos(), Times.Once);
        }

        [Fact]
        public async Task DadoDtoValidoComNovosAssuntosEAssuntosAExcluir_QuandoAlterar_EntaoAtualizaAcervosInsereEExcluiAssuntosComitaTransacaoERetornaDtoAtualizado()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoAlteracaoDTO();
            dto.AssuntosIds = [1L, 3L];
            var assuntosExistentesNoBanco = new List<AcervoBibliograficoAssunto>
            {
                new() { AssuntoId = 1L },
                new() { AssuntoId = 2L }
            };

            var acervoBibliografico = new AcervoBibliografico { Id = dto.Id };
            var acervoDtoMap = new AcervoDto();
            var dtoRetornoFinal = new AcervoBibliograficoDTO { Id = dto.Id };

            mapperMock.Setup(m => m.Map<AcervoBibliografico>(dto)).Returns(acervoBibliografico);
            mapperMock.Setup(m => m.Map<AcervoDto>(dto)).Returns(acervoDtoMap);

            repositorioAcervoBibliograficoAssuntoMock
                .Setup(r => r.ObterPorAcervoBibliograficoId(dto.Id))
                .ReturnsAsync(assuntosExistentesNoBanco);

            var acervoBibliograficoCompleto = new AcervoBibliograficoCompleto();
            repositorioAcervoBibliograficoMock.Setup(r => r.ObterAcervoBibliograficoCompletoPorId(dto.AcervoId)).ReturnsAsync(acervoBibliograficoCompleto);
            mapperMock.Setup(m => m.Map<AcervoBibliograficoDTO>(acervoBibliograficoCompleto)).Returns(dtoRetornoFinal);

            // Act
            var resultado = await sut.Alterar(dto);

            // Assert
            resultado.Should().NotBeNull();
            servicoAcervoMock.Verify(s => s.Alterar(acervoDtoMap), Times.Once);
            repositorioAcervoBibliograficoMock.Verify(r => r.Atualizar(acervoBibliografico), Times.Once);
            repositorioAcervoBibliograficoAssuntoMock.Verify(r => r.Inserir(It.Is<AcervoBibliograficoAssunto>(a => a.AssuntoId == 3L)), Times.Once);
            repositorioAcervoBibliograficoAssuntoMock.Verify(r => r.Excluir(It.Is<long[]>(a => a.Contains(2L)), acervoBibliografico.Id), Times.Once);
            dbTransactionMock.Verify(t => t.Commit(), Times.Once);
        }

        [Fact]
        public async Task DadoLarguraComFormatoInvalido_QuandoAlterar_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoAlteracaoDTO();
            dto.Largura = "10.5";

            // Act
            Func<Task> acao = async () => await sut.Alterar(dto);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.LARGURA));
        }

        [Fact]
        public async Task DadoAlturaComFormatoInvalido_QuandoAlterar_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoAlteracaoDTO();
            dto.Altura = "10.5";

            // Act
            Func<Task> acao = async () => await sut.Alterar(dto);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(string.Format(MensagemNegocio.CAMPO_X_ESPERADO_NUMERICO_E_COM_CASAS_DECIMAIS, Constantes.ALTURA));
        }

        [Fact]
        public async Task DadoExcecaoLancadaPorQualquerDependencia_QuandoAlterar_EntaoRealizaRollbackNaTransacaoERelancaExcecao()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoAlteracaoDTO();
            var assuntosExistentesNoBanco = new List<AcervoBibliograficoAssunto>
            {
                new() { AssuntoId = 1L },
                new() { AssuntoId = 2L }
            };

            repositorioAcervoBibliograficoAssuntoMock
                .Setup(r => r.ObterPorAcervoBibliograficoId(dto.Id))
                .ReturnsAsync(assuntosExistentesNoBanco);
            servicoAcervoMock.Setup(r => r.Alterar(It.IsAny<AcervoDto>())).ThrowsAsync(new Exception());

            // Act
            Func<Task> acao = async () => await sut.Alterar(dto);

            // Assert
            await acao.Should().ThrowAsync<Exception>();
            dbTransactionMock.Verify(t => t.Rollback(), Times.Once);
        }

        [Fact]
        public async Task DadoIdExistente_QuandoObterPorId_EntaoRetornaAcervoBibliograficoDtoMapeadoComAuditoria()
        {
            // Arrange
            var id = 1L;
            var entidadeCompleta = new AcervoBibliograficoCompleto();
            var dto = new AcervoBibliograficoDTO();
            var auditoriaDto = new AuditoriaDTO();

            repositorioAcervoBibliograficoMock.Setup(r => r.ObterAcervoBibliograficoCompletoPorId(id)).ReturnsAsync(entidadeCompleta);
            mapperMock.Setup(m => m.Map<AcervoBibliograficoDTO>(entidadeCompleta)).Returns(dto);
            mapperMock.Setup(m => m.Map<AuditoriaDTO>(entidadeCompleta)).Returns(auditoriaDto);

            // Act
            var resultado = await sut.ObterPorId(id);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().Be(dto);
            resultado.Auditoria.Should().Be(auditoriaDto);
        }

        [Fact]
        public async Task DadoIdInexistente_QuandoObterPorId_EntaoRetornaNulo()
        {
            // Arrange
            var id = 1L;
            repositorioAcervoBibliograficoMock.Setup(r => r.ObterAcervoBibliograficoCompletoPorId(id)).ReturnsAsync((AcervoBibliograficoCompleto)null!);

            // Act
            var resultado = await sut.ObterPorId(id);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoIdValido_QuandoExcluir_EntaoChamaExclusaoNoServicoAcervoERetornaResultadoBooleano()
        {
            // Arrange
            var id = 1L;
            servicoAcervoMock.Setup(s => s.Excluir(id)).ReturnsAsync(true);

            // Act
            var resultado = await sut.Excluir(id);

            // Assert
            resultado.Should().BeTrue();
            servicoAcervoMock.Verify(s => s.Excluir(id), Times.Once);
        }

        [Fact]
        public async Task DadoIdExistente_QuandoAlterarSituacaoSaldo_EntaoAtualizaSituacaoNoRepositorioERetornaVerdadeiro()
        {
            // Arrange
            var id = 1L;
            var situacao = SituacaoSaldo.RESERVADO;
            var acervo = new AcervoBibliografico { SituacaoSaldo = SituacaoSaldo.DISPONIVEL };

            repositorioAcervoBibliograficoMock.Setup(r => r.ObterPorAcervoId(id)).ReturnsAsync(acervo);

            // Act
            var resultado = await sut.AlterarSituacaoSaldo(situacao, id);

            // Assert
            resultado.Should().BeTrue();
            acervo.SituacaoSaldo.Should().Be(situacao);
            repositorioAcervoBibliograficoMock.Verify(r => r.Atualizar(acervo), Times.Once);
        }

        [Fact]
        public async Task DadoIdInexistente_QuandoAlterarSituacaoSaldo_EntaoNaoAtualizaERetornaFalso()
        {
            // Arrange
            var id = 1L;
            var situacao = SituacaoSaldo.RESERVADO;

            repositorioAcervoBibliograficoMock.Setup(r => r.ObterPorAcervoId(id)).ReturnsAsync((AcervoBibliografico)null!);

            // Act
            var resultado = await sut.AlterarSituacaoSaldo(situacao, id);

            // Assert
            resultado.Should().BeFalse();
            repositorioAcervoBibliograficoMock.Verify(r => r.Atualizar(It.IsAny<AcervoBibliografico>()), Times.Never);
        }

        [Fact]
        public async Task DadoItemJaPossuiEmprestimo_QuandoGerenciarEmprestimoAsync_EntaoLancaNegocioExceptionDeAlteracaoNaoPermitida()
        {
            // Arrange
            var itemId = 1L;
            repositorioAcervoEmprestimoMock.Setup(r => r.ObterUltimoEmprestimoPorAcervoSolicitacaoItemId(itemId)).ReturnsAsync(new AcervoEmprestimo());

            // Act
            Func<Task> acao = async () => await sut.GerenciarEmprestimoAsync(itemId, 2L, DateTime.Now, DateTime.Now);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>().WithMessage(MensagemNegocio.VOCE_NAO_PODE_ALTERAR_EMPRESTIMOS_ACERVOS);
        }

        [Fact]
        public async Task DadoItemSemEmprestimoEComDatasPreenchidas_QuandoGerenciarEmprestimoAsync_EntaoInsereNovoEmprestimoEAlteraSaldoParaEmprestado()
        {
            // Arrange
            var itemId = 1L;
            var acervoId = 2L;
            var acervo = new AcervoBibliografico();

            repositorioAcervoEmprestimoMock.Setup(r => r.ObterUltimoEmprestimoPorAcervoSolicitacaoItemId(itemId)).ReturnsAsync((AcervoEmprestimo)null!);
            repositorioAcervoBibliograficoMock.Setup(r => r.ObterPorAcervoId(acervoId)).ReturnsAsync(acervo);

            // Act
            await sut.GerenciarEmprestimoAsync(itemId, acervoId, DateTime.Now, DateTime.Now);

            // Assert
            repositorioAcervoEmprestimoMock.Verify(r => r.Inserir(It.Is<AcervoEmprestimo>(a => a.Situacao == SituacaoEmprestimo.EMPRESTADO)), Times.Once);
            repositorioAcervoBibliograficoMock.Verify(r => r.Atualizar(It.Is<AcervoBibliografico>(a => a.SituacaoSaldo == SituacaoSaldo.EMPRESTADO)), Times.Once);
        }

        [Fact]
        public async Task DadoItemSemEmprestimoESemDatasPreenchidas_QuandoGerenciarEmprestimoAsync_EntaoNaoInsereEmprestimoEAlteraSaldoParaReservado()
        {
            // Arrange
            var itemId = 1L;
            var acervoId = 2L;
            var acervo = new AcervoBibliografico();

            repositorioAcervoEmprestimoMock.Setup(r => r.ObterUltimoEmprestimoPorAcervoSolicitacaoItemId(itemId)).ReturnsAsync((AcervoEmprestimo)null!);
            repositorioAcervoBibliograficoMock.Setup(r => r.ObterPorAcervoId(acervoId)).ReturnsAsync(acervo);

            // Act
            await sut.GerenciarEmprestimoAsync(itemId, acervoId, null, null);

            // Assert
            repositorioAcervoEmprestimoMock.Verify(r => r.Inserir(It.IsAny<AcervoEmprestimo>()), Times.Never);
            repositorioAcervoBibliograficoMock.Verify(r => r.Atualizar(It.Is<AcervoBibliografico>(a => a.SituacaoSaldo == SituacaoSaldo.RESERVADO)), Times.Once);
        }

        [Fact]
        public async Task DadoItemComEmprestimoExistente_QuandoAtualizarOuCriarEmprestimoAsync_EntaoAtualizaDatasESituacaoParaEmprestadoEAlteraSaldoParaEmprestado()
        {
            // Arrange
            var itemId = 1L;
            var acervoId = 2L;
            var dataNova = DateTime.Now.AddDays(1);
            var emprestimo = new AcervoEmprestimo { Situacao = SituacaoEmprestimo.DEVOLUCAO_EM_ATRASO };
            var acervo = new AcervoBibliografico();

            repositorioAcervoEmprestimoMock.Setup(r => r.ObterUltimoEmprestimoPorAcervoSolicitacaoItemId(itemId)).ReturnsAsync(emprestimo);
            repositorioAcervoBibliograficoMock.Setup(r => r.ObterPorAcervoId(acervoId)).ReturnsAsync(acervo);

            // Act
            await sut.AtualizarOuCriarEmprestimoAsync(itemId, acervoId, dataNova, dataNova);

            // Assert
            emprestimo.DataEmprestimo.Should().Be(dataNova);
            emprestimo.DataDevolucao.Should().Be(dataNova);
            emprestimo.Situacao.Should().Be(SituacaoEmprestimo.EMPRESTADO);

            repositorioAcervoEmprestimoMock.Verify(r => r.Atualizar(emprestimo), Times.Once);
            repositorioAcervoEmprestimoMock.Verify(r => r.Inserir(It.IsAny<AcervoEmprestimo>()), Times.Never);
            repositorioAcervoBibliograficoMock.Verify(r => r.Atualizar(It.Is<AcervoBibliografico>(a => a.SituacaoSaldo == SituacaoSaldo.EMPRESTADO)), Times.Once);
        }

        [Fact]
        public async Task DadoItemSemEmprestimoExistente_QuandoAtualizarOuCriarEmprestimoAsync_EntaoInsereNovoEmprestimoEAlteraSaldoParaEmprestado()
        {
            // Arrange
            var itemId = 1L;
            var acervoId = 2L;
            var dataNova = DateTime.Now;
            var acervo = new AcervoBibliografico();

            repositorioAcervoEmprestimoMock.Setup(r => r.ObterUltimoEmprestimoPorAcervoSolicitacaoItemId(itemId)).ReturnsAsync((AcervoEmprestimo)null!);
            repositorioAcervoBibliograficoMock.Setup(r => r.ObterPorAcervoId(acervoId)).ReturnsAsync(acervo);

            // Act
            await sut.AtualizarOuCriarEmprestimoAsync(itemId, acervoId, dataNova, dataNova);

            // Assert
            repositorioAcervoEmprestimoMock.Verify(r => r.Atualizar(It.IsAny<AcervoEmprestimo>()), Times.Never);
            repositorioAcervoEmprestimoMock.Verify(r => r.Inserir(It.Is<AcervoEmprestimo>(a => a.Situacao == SituacaoEmprestimo.EMPRESTADO)), Times.Once);
            repositorioAcervoBibliograficoMock.Verify(r => r.Atualizar(It.Is<AcervoBibliografico>(a => a.SituacaoSaldo == SituacaoSaldo.EMPRESTADO)), Times.Once);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static AcervoBibliograficoCadastroDTO GerarAcervoBibliograficoCadastroDTO() => new Faker<AcervoBibliograficoCadastroDTO>("pt_BR")
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
            .RuleFor(x => x.MaterialId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.IdiomaId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.LocalizacaoCDD, f => f.Random.AlphaNumeric(10))
            .RuleFor(x => x.AssuntosIds, f => new[] { f.Random.Long(1, 10) })
            .RuleFor(x => x.Largura, f => "10,50")
            .RuleFor(x => x.Altura, f => "20,00")
            .Generate();

        private static AcervoBibliograficoAlteracaoDTO GerarAcervoBibliograficoAlteracaoDTO() => new Faker<AcervoBibliograficoAlteracaoDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.AcervoId, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
            .RuleFor(x => x.MaterialId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.IdiomaId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.LocalizacaoCDD, f => f.Random.AlphaNumeric(10))
            .RuleFor(x => x.AssuntosIds, f => new[] { f.Random.Long(1, 10) })
            .RuleFor(x => x.Largura, f => "10,50")
            .RuleFor(x => x.Altura, f => "20,00")
            .Generate();
    }
}