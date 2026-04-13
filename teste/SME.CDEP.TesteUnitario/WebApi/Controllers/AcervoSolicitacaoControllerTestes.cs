using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Controllers;

namespace SME.CDEP.TesteUnitario.Webapi.Controllers
{
    public class AcervoSolicitacaoControllerTestes
    {
        private readonly Mock<IServicoAcervoSolicitacao> servicoAcervoSolicitacaoMock;
        private readonly AcervoSolicitacaoController sut;

        public AcervoSolicitacaoControllerTestes()
        {
            var mocker = new AutoMocker();

            servicoAcervoSolicitacaoMock = mocker.GetMock<IServicoAcervoSolicitacao>();

            sut = mocker.CreateInstance<AcervoSolicitacaoController>();
        }

        [Fact]
        public async Task DadoAcervosIdsValidos_QuandoObterItensAcervoPorAcervosIdsViaConsultaAcervoPortal_EntaoRetornaOkComDados()
        {
            // Arrange
            var faker = new Faker();
            var acervosIds = new long[] { faker.Random.Long(1, 100), faker.Random.Long(1, 100), faker.Random.Long(1, 100) };
            var retornoMock = GerarAcervoTipoTituloAcervoIdCreditosAutoresDTO(3);

            servicoAcervoSolicitacaoMock
                .Setup(s => s.ObterItensAcervoPorAcervosIds(acervosIds))
                .ReturnsAsync(retornoMock);

            // Act
            var resultado = await sut.ObterItensAcervoPorAcervosIdsViaConsultaAcervoPortal(acervosIds, servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(retornoMock);
            servicoAcervoSolicitacaoMock.Verify(s => s.ObterItensAcervoPorAcervosIds(acervosIds), Times.Once);
        }

        [Fact]
        public async Task DadoDtoDeCadastroValido_QuandoCadastrarAcervoSolicitacaoViaPortal_EntaoRetornaOkComId()
        {
            // Arrange
            var dto = GerarAcervoSolicitacaoItemCadastroDTO(2);
            var idGerado = new Faker().Random.Long(1, 1000);

            servicoAcervoSolicitacaoMock
                .Setup(s => s.Inserir(dto))
                .ReturnsAsync(idGerado);

            // Act
            var resultado = await sut.CadastrarAcervoSolicitacaoViaPortal(dto, servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(idGerado);
            servicoAcervoSolicitacaoMock.Verify(s => s.Inserir(dto), Times.Once);
        }

        [Fact]
        public async Task DadoIdExistente_QuandoObterSolicitacaoPorId_EntaoRetornaOkComDados()
        {
            // Arrange
            var acervoSolicitacaoId = new Faker().Random.Long(1, 100);
            var retornoMock = new AcervoSolicitacaoRetornoCadastroDTO { PodeCancelarSolicitacao = true, Itens = new List<AcervoSolicitacaoItemRetornoCadastroDTO>() };

            servicoAcervoSolicitacaoMock
                .Setup(s => s.ObterPorId(acervoSolicitacaoId))
                .ReturnsAsync(retornoMock);

            // Act
            var resultado = await sut.ObterSolicitacaoPorId(acervoSolicitacaoId, servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(retornoMock);
            servicoAcervoSolicitacaoMock.Verify(s => s.ObterPorId(acervoSolicitacaoId), Times.Once);
        }

        [Fact]
        public async Task DadoIdExistente_QuandoObterMinhaSolicitacaoPorId_EntaoRetornaOkComDados()
        {
            // Arrange
            var acervoSolicitacaoId = new Faker().Random.Long(1, 100);
            var retornoMock = new AcervoSolicitacaoRetornoCadastroDTO { PodeCancelarSolicitacao = false, Itens = new List<AcervoSolicitacaoItemRetornoCadastroDTO>() };

            servicoAcervoSolicitacaoMock
                .Setup(s => s.ObterMinhaSolicitacaoPorId(acervoSolicitacaoId))
                .ReturnsAsync(retornoMock);

            // Act
            var resultado = await sut.ObterMinhaSolicitacaoPorId(acervoSolicitacaoId, servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(retornoMock);
            servicoAcervoSolicitacaoMock.Verify(s => s.ObterMinhaSolicitacaoPorId(acervoSolicitacaoId), Times.Once);
        }

        [Fact]
        public async Task DadoIdExistente_QuandoExcluirAtendimentoLogicamente_EntaoRetornaOkComTrue()
        {
            // Arrange
            var acervoSolicitacaoId = new Faker().Random.Long(1, 100);

            servicoAcervoSolicitacaoMock
                .Setup(s => s.Excluir(acervoSolicitacaoId))
                .ReturnsAsync(true);

            // Act
            var resultado = await sut.ExcluirAtendimentoLogicamente(acervoSolicitacaoId, servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);
            servicoAcervoSolicitacaoMock.Verify(s => s.Excluir(acervoSolicitacaoId), Times.Once);
        }

        [Fact]
        public async Task DadoUsuarioAutenticado_QuandoObterMinhasSolicitacoes_EntaoRetornaOkComListaPaginada()
        {
            // Arrange
            var retornoMock = new PaginacaoResultadoDTO<MinhaSolicitacaoDTO>
            {
                TotalPaginas = 1,
                TotalRegistros = 10,
                Items = []
            };

            servicoAcervoSolicitacaoMock
                .Setup(s => s.ObterMinhasSolicitacoes())
                .ReturnsAsync(retornoMock);

            // Act
            var resultado = await sut.ObterMinhasSolicitacoes(servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(retornoMock);
            servicoAcervoSolicitacaoMock.Verify(s => s.ObterMinhasSolicitacoes(), Times.Once);
        }

        [Fact]
        public async Task DadoRequisicaoValida_QuandoObterSituacoesAtendimentosItem_EntaoRetornaOkComLista()
        {
            // Arrange
            var retornoMock = new List<SituacaoItemDTO> { new SituacaoItemDTO { Id = 1, Nome = "Pendente" } };

            servicoAcervoSolicitacaoMock
                .Setup(s => s.ObterSituacoesAtendimentosItem())
                .ReturnsAsync(retornoMock);

            // Act
            var resultado = await sut.ObterSituacoesAtendimentosItem(servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(retornoMock);
            servicoAcervoSolicitacaoMock.Verify(s => s.ObterSituacoesAtendimentosItem(), Times.Once);
        }

        [Fact]
        public async Task DadoFiltroPreenchido_QuandoObterAtendimentoSolicitacoesPorFiltro_EntaoRetornaOkComResultado()
        {
            // Arrange
            var filtro = GerarFiltroSolicitacaoDTO();
            var retornoMock = new PaginacaoResultadoDTO<SolicitacaoDTO> { TotalRegistros = 5, Items = new List<SolicitacaoDTO>() };

            servicoAcervoSolicitacaoMock
                .Setup(s => s.ObterAtendimentoSolicitacoesPorFiltro(filtro))
                .ReturnsAsync(retornoMock);

            // Act
            var resultado = await sut.ObterAtendimentoSolicitacoesPorFiltro(filtro, servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(retornoMock);
            servicoAcervoSolicitacaoMock.Verify(s => s.ObterAtendimentoSolicitacoesPorFiltro(filtro), Times.Once);
        }

        [Fact]
        public async Task DadoIdSolicitacaoExistente_QuandoObterDetalhesParaAtendimentoSolicitadoesPorId_EntaoRetornaOkComDados()
        {
            // Arrange
            var acervoSolicitacaoId = new Faker().Random.Long(1, 100);
            var retornoMock = new AcervoSolicitacaoDetalheDTO { Id = acervoSolicitacaoId, PodeCancelar = true };

            servicoAcervoSolicitacaoMock
                .Setup(s => s.ObterDetalhesParaAtendimentoSolicitadoesPorId(acervoSolicitacaoId))
                .ReturnsAsync(retornoMock);

            // Act
            var resultado = await sut.ObterDetalhesParaAtendimentoSolicitadoesPorId(acervoSolicitacaoId, servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(retornoMock);
            servicoAcervoSolicitacaoMock.Verify(s => s.ObterDetalhesParaAtendimentoSolicitadoesPorId(acervoSolicitacaoId), Times.Once);
        }

        [Fact]
        public void DadoRequisicaoValida_QuandoObterTiposDeAtendimentos_EntaoRetornaOkComLista()
        {
            // Arrange
            var retornoMock = new List<IdNomeDTO> { new IdNomeDTO { Id = 1, Nome = "Presencial" } };

            servicoAcervoSolicitacaoMock
                .Setup(s => s.ObterTiposDeAtendimentos())
                .Returns(retornoMock);

            // Act
            var resultado = sut.ObterTiposDeAtendimentos(servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(retornoMock);
            servicoAcervoSolicitacaoMock.Verify(s => s.ObterTiposDeAtendimentos(), Times.Once);
        }

        [Fact]
        public async Task DadoDtoConfirmacaoValido_QuandoConfirmarAtendimento_EntaoRetornaOkComTrue()
        {
            // Arrange
            var dto = GerarAcervoSolicitacaoConfirmarDto();

            servicoAcervoSolicitacaoMock
                .Setup(s => s.ConfirmarAtendimento(dto))
                .ReturnsAsync(true);

            // Act
            var resultado = await sut.ConfirmarAtendimento(dto, servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);
            servicoAcervoSolicitacaoMock.Verify(s => s.ConfirmarAtendimento(dto), Times.Once);
        }

        [Fact]
        public async Task DadoIdSolicitacaoExistente_QuandoFinalizarAtendimento_EntaoRetornaOkComTrue()
        {
            // Arrange
            var acervoSolicitacaoId = new Faker().Random.Long(1, 100);

            servicoAcervoSolicitacaoMock
                .Setup(s => s.FinalizarAtendimento(acervoSolicitacaoId))
                .ReturnsAsync(true);

            // Act
            var resultado = await sut.FinalizarAtendimento(acervoSolicitacaoId, servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);
            servicoAcervoSolicitacaoMock.Verify(s => s.FinalizarAtendimento(acervoSolicitacaoId), Times.Once);
        }

        [Fact]
        public async Task DadoIdItemExistente_QuandoFinalizarAtendimentoItem_EntaoRetornaOkComTrue()
        {
            // Arrange
            var acervoSolicitacaoItemId = new Faker().Random.Long(1, 100);

            servicoAcervoSolicitacaoMock
                .Setup(s => s.FinalizarAtendimentoItem(acervoSolicitacaoItemId))
                .ReturnsAsync(true);

            // Act
            var resultado = await sut.FinalizarAtendimentoItem(acervoSolicitacaoItemId, servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);
            servicoAcervoSolicitacaoMock.Verify(s => s.FinalizarAtendimentoItem(acervoSolicitacaoItemId), Times.Once);
        }

        [Fact]
        public async Task DadoIdSolicitacaoExistente_QuandoCancelarAtendimento_EntaoRetornaOkComTrue()
        {
            // Arrange
            var acervoSolicitacaoId = new Faker().Random.Long(1, 100);

            servicoAcervoSolicitacaoMock
                .Setup(s => s.CancelarAtendimento(acervoSolicitacaoId))
                .ReturnsAsync(true);

            // Act
            var resultado = await sut.CancelarAtendimento(acervoSolicitacaoId, servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);
            servicoAcervoSolicitacaoMock.Verify(s => s.CancelarAtendimento(acervoSolicitacaoId), Times.Once);
        }

        [Fact]
        public async Task DadoIdItemExistente_QuandoCancelarItemAtendimento_EntaoRetornaOkComTrue()
        {
            // Arrange
            var acervoSolicitacaoItemId = new Faker().Random.Long(1, 100);

            servicoAcervoSolicitacaoMock
                .Setup(s => s.CancelarItemAtendimento(acervoSolicitacaoItemId))
                .ReturnsAsync(true);

            // Act
            var resultado = await sut.CancelarItemAtendimento(acervoSolicitacaoItemId, servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);
            servicoAcervoSolicitacaoMock.Verify(s => s.CancelarItemAtendimento(acervoSolicitacaoItemId), Times.Once);
        }

        [Fact]
        public async Task DadoDtoAlteracaoDataValido_QuandoAlterarDataVisitaDoItemAtendimento_EntaoRetornaOkComTrue()
        {
            // Arrange
            var dto = GerarAlterarDataVisitaAcervoSolicitacaoItemDTO();

            servicoAcervoSolicitacaoMock
                .Setup(s => s.AlterarDataVisitaDoItemAtendimento(dto))
                .ReturnsAsync(true);

            // Act
            var resultado = await sut.AlterarDataVisitaDoItemAtendimento(dto, servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);
            servicoAcervoSolicitacaoMock.Verify(s => s.AlterarDataVisitaDoItemAtendimento(dto), Times.Once);
        }

        [Fact]
        public async Task DadoDtoInsercaoManualValido_QuandoCadastrarAcervoSolicitacaoManual_EntaoRetornaOkComId()
        {
            // Arrange
            var dto = GerarAcervoSolicitacaoManualDTO();
            var idGerado = new Faker().Random.Long(1, 1000);

            servicoAcervoSolicitacaoMock
                .Setup(s => s.Inserir(dto))
                .ReturnsAsync(idGerado);

            // Act
            var resultado = await sut.CadastrarAcervoSolicitacaoManual(dto, servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(idGerado);
            servicoAcervoSolicitacaoMock.Verify(s => s.Inserir(dto), Times.Once);
        }

        [Fact]
        public async Task DadoDtoAlteracaoManualValido_QuandoAlterarAcervoSolicitacaoManual_EntaoRetornaOkComId()
        {
            // Arrange
            var dto = GerarAcervoSolicitacaoManualDTO();
            var idAtualizado = new Faker().Random.Long(1, 1000);

            servicoAcervoSolicitacaoMock
                .Setup(s => s.Alterar(dto))
                .ReturnsAsync(idAtualizado);

            // Act
            var resultado = await sut.AlterarAcervoSolicitacaoManual(dto, servicoAcervoSolicitacaoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(idAtualizado);
            servicoAcervoSolicitacaoMock.Verify(s => s.Alterar(dto), Times.Once);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static AcervoTipoTituloAcervoIdCreditosAutoresDTO[] GerarAcervoTipoTituloAcervoIdCreditosAutoresDTO(int quantidade) =>
            [.. new Faker<AcervoTipoTituloAcervoIdCreditosAutoresDTO>("pt_BR")
                .RuleFor(x => x.TipoAcervo, f => f.Commerce.Department())
                .RuleFor(x => x.AcervoId, f => f.Random.Long(1, 100))
                .RuleFor(x => x.Titulo, f => f.Commerce.ProductName())
                .RuleFor(x => x.SituacaoDisponibilidade, f => f.PickRandom("Disponível", "Indisponível"))
                .RuleFor(x => x.EstaDisponivel, f => f.Random.Bool())
                .RuleFor(x => x.TemControleDisponibilidade, f => f.Random.Bool())
                .RuleFor(x => x.AutoresCreditos, f => f.Make(2, () => f.Name.FullName()).ToArray())
                .RuleFor(x => x.TipoAcervoId, f => f.PickRandom<TipoAcervo>())
                .Generate(quantidade)];

        private static AcervoSolicitacaoItemCadastroDTO[] GerarAcervoSolicitacaoItemCadastroDTO(int quantidade) =>
            [.. new Faker<AcervoSolicitacaoItemCadastroDTO>("pt_BR")
                .RuleFor(x => x.AcervoId, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.DataVisita, f => f.Date.Future())
                .Generate(quantidade)];

        private static FiltroSolicitacaoDTO GerarFiltroSolicitacaoDTO() => new Faker<FiltroSolicitacaoDTO>("pt_BR")
                .RuleFor(x => x.AcervoSolicitacaoId, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.TipoAcervo, f => f.PickRandom<TipoAcervo>())
                .RuleFor(x => x.DataSolicitacaoInicio, f => f.Date.Past())
                .RuleFor(x => x.DataSolicitacaoFim, f => f.Date.Recent())
                .RuleFor(x => x.DataVisitaInicio, f => f.Date.Soon())
                .RuleFor(x => x.DataVisitaFim, f => f.Date.Future())
                .RuleFor(x => x.Responsavel, f => f.Name.FullName())
                .RuleFor(x => x.SituacaoItem, f => f.PickRandom<SituacaoSolicitacaoItem>())
                .RuleFor(x => x.SolicitanteRf, f => f.Random.Replace("#######"))
                .RuleFor(x => x.SituacaoEmprestimo, f => f.PickRandom<SituacaoEmprestimo>())
                .Generate();

        private static AcervoSolicitacaoConfirmarDto GerarAcervoSolicitacaoConfirmarDto() =>
            new Faker<AcervoSolicitacaoConfirmarDto>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.ItemId, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.DataVisita, f => f.Date.Future())
                .RuleFor(x => x.DataEmprestimo, f => f.Date.Future())
                .RuleFor(x => x.DataDevolucao, f => f.Date.Future())
                .RuleFor(x => x.TipoAcervo, f => f.PickRandom<TipoAcervo>())
                .RuleFor(x => x.TipoAtendimento, f => f.PickRandom<TipoAtendimento>())
                .Generate();

        private static AlterarDataVisitaAcervoSolicitacaoItemDTO GerarAlterarDataVisitaAcervoSolicitacaoItemDTO() =>
            new Faker<AlterarDataVisitaAcervoSolicitacaoItemDTO>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.DataVisita, f => f.Date.Future())
                .Generate();

        private static AcervoSolicitacaoManualDTO GerarAcervoSolicitacaoManualDTO() => new Faker<AcervoSolicitacaoManualDTO>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long(1, 100))
                .RuleFor(x => x.UsuarioId, f => f.Random.Long(1, 500))
                .RuleFor(x => x.DataSolicitacao, f => f.Date.Recent())
                .Generate();
    }
}