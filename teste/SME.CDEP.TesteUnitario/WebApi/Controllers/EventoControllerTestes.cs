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
    public class EventoControllerTestes
    {
        private readonly Mock<IServicoEvento> servicoEventoMock;
        private readonly EventoController sut;

        public EventoControllerTestes()
        {
            var mocker = new AutoMocker();

            servicoEventoMock = mocker.GetMock<IServicoEvento>();

            sut = mocker.CreateInstance<EventoController>();
        }

        [Fact]
        public async Task DadoEventoCadastroValido_QuandoInserir_EntaoRetornaOkComIdGerado()
        {
            // Arrange
            var dto = GerarEventoCadastroDTO();
            var idGerado = new Faker().Random.Long(1, 1000);

            servicoEventoMock
                .Setup(s => s.Inserir(dto))
                .ReturnsAsync(idGerado);

            // Act
            var resultado = await sut.Inserir(dto, servicoEventoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(idGerado);
            servicoEventoMock.Verify(s => s.Inserir(dto), Times.Once);
        }

        [Fact]
        public async Task DadoEventoCadastroValido_QuandoAlterar_EntaoRetornaOkComEventoAlterado()
        {
            // Arrange
            var dto = GerarEventoCadastroDTO();
            var eventoAlterado = GerarEventoDTO();

            servicoEventoMock
                .Setup(s => s.Alterar(dto))
                .ReturnsAsync(eventoAlterado);

            // Act
            var resultado = await sut.Alterar(dto, servicoEventoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(eventoAlterado);
            servicoEventoMock.Verify(s => s.Alterar(dto), Times.Once);
        }

        [Fact]
        public async Task DadoDiaMesValido_QuandoObterEventosTagPorData_EntaoRetornaOkComListaDeTags()
        {
            // Arrange
            var dto = GerarDiaMesDTO();
            var listaTags = GerarListaEventoTagDTO(3);

            servicoEventoMock
                .Setup(s => s.ObterEventosTagPorData(dto))
                .ReturnsAsync(listaTags);

            // Act
            var resultado = await sut.ObterEventosTagPorData(dto, servicoEventoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(listaTags);
            servicoEventoMock.Verify(s => s.ObterEventosTagPorData(dto), Times.Once);
        }

        [Fact]
        public async Task DadoEventoIdValido_QuandoExcluirLogicamente_EntaoRetornaOkComBooleano()
        {
            // Arrange
            var eventoId = new Faker().Random.Long(1, 1000);
            var exclusaoComSucesso = true;

            servicoEventoMock
                .Setup(s => s.ExcluirLogicamente(eventoId))
                .ReturnsAsync(exclusaoComSucesso);

            // Act
            var resultado = await sut.ExcluirLogicamente(eventoId, servicoEventoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(exclusaoComSucesso);
            servicoEventoMock.Verify(s => s.ExcluirLogicamente(eventoId), Times.Once);
        }

        [Fact]
        public async Task DadoEventoIdValido_QuandoObterEventoPorId_EntaoRetornaOkComDadosDoEvento()
        {
            // Arrange
            var eventoId = new Faker().Random.Int(1, 1000);
            var eventoDto = GerarEventoDTO();

            servicoEventoMock
                .Setup(s => s.ObterEventoPorId(eventoId))
                .ReturnsAsync(eventoDto);

            // Act
            var resultado = await sut.ObterEventoPorId(eventoId, servicoEventoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(eventoDto);
            servicoEventoMock.Verify(s => s.ObterEventoPorId(eventoId), Times.Once);
        }

        [Fact]
        public async Task DadoMesValido_QuandoObterCalendarioDeEventosPorMes_EntaoRetornaOkComCalendario()
        {
            // Arrange
            var mes = new Faker().Random.Int(1, 12);
            var calendarioEsperado = GerarCalendarioEventoDTO();

            servicoEventoMock
                .Setup(s => s.ObterCalendarioDeEventosPorMes(mes, It.IsAny<int>()))
                .ReturnsAsync(calendarioEsperado);

            // Act
            var resultado = await sut.ObterCalendarioDeEventosPorMes(mes, servicoEventoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(calendarioEsperado);
            servicoEventoMock.Verify(s => s.ObterCalendarioDeEventosPorMes(mes, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DadoDiaMesValido_QuandoObterDetalhesDoDiaPorDiaMes_EntaoRetornaOkComListaDeDetalhes()
        {
            // Arrange
            var dto = GerarDiaMesDTO();
            var detalhesEsperados = GerarListaEventoDetalheDTO(2);

            servicoEventoMock
                .Setup(s => s.ObterDetalhesDoDiaPorDiaMes(dto))
                .ReturnsAsync(detalhesEsperados);

            // Act
            var resultado = await sut.ObterDetalhesDoDiaPorDiaMes(dto, servicoEventoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(detalhesEsperados);
            servicoEventoMock.Verify(s => s.ObterDetalhesDoDiaPorDiaMes(dto), Times.Once);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static EventoCadastroDTO GerarEventoCadastroDTO() => new Faker<EventoCadastroDTO>("pt_BR")
            .RuleFor(x => x.Dia, f => f.Random.Int(1, 28)) // Fixado até 28 para evitar exception em meses como Fevereiro
            .RuleFor(x => x.Mes, f => f.Random.Int(1, 12))
            .RuleFor(x => x.Ano, f => f.Date.Future().Year)
            .RuleFor(x => x.Hora, f => f.Random.Int(0, 23))
            .RuleFor(x => x.Minuto, f => f.Random.Int(0, 59))
            .RuleFor(x => x.Id, f => f.Random.Long(1, 100))
            .RuleFor(x => x.Tipo, f => f.PickRandom<TipoEvento>())
            .RuleFor(x => x.Descricao, f => f.Lorem.Sentence())
            .RuleFor(x => x.Justificativa, f => f.Lorem.Paragraph())
            .RuleFor(x => x.AcervoSolicitacaoItemId, f => f.Random.Long(1, 1000))
            .Generate();

        private static EventoDTO GerarEventoDTO() => new Faker<EventoDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Tipo, f => f.PickRandom<TipoEvento>())
            .RuleFor(x => x.Descricao, f => f.Lorem.Sentence())
            .RuleFor(x => x.Justificativa, f => f.Lorem.Paragraph())
            .RuleFor(x => x.AcervoSolicitacaoItemId, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Data, f => f.Date.Future())
            .Generate();

        private static DiaMesDTO GerarDiaMesDTO() => new Faker<DiaMesDTO>("pt_BR")
            .RuleFor(x => x.Dia, f => f.Random.Int(1, 28))
            .RuleFor(x => x.Mes, f => f.Random.Int(1, 12))
            .RuleFor(x => x.Ano, f => f.Date.Future().Year)
            .RuleFor(x => x.Hora, f => f.Random.Int(0, 23))
            .RuleFor(x => x.Minuto, f => f.Random.Int(0, 59))
            .Generate();

        private static List<EventoTagDTO> GerarListaEventoTagDTO(int quantidade) => new Faker<EventoTagDTO>("pt_BR")
            .RuleFor(x => x.TipoId, f => f.PickRandom<TipoEvento>())
            .RuleFor(x => x.Tipo, f => f.Lorem.Word())
            .Generate(quantidade);

        private static CalendarioEventoDTO GerarCalendarioEventoDTO()
        {
            var dias = new Faker<DiaDTO>("pt_BR")
                .RuleFor(x => x.Dia, f => f.Random.Int(1, 28))
                .RuleFor(x => x.DayOfWeek, f => f.Random.Int(0, 6))
                .RuleFor(x => x.Desabilitado, f => f.Random.Bool())
                .RuleFor(x => x.EventosTag, f => GerarListaEventoTagDTO(2))
                .Generate(7);

            var semanas = new Faker<SemanaDTO>("pt_BR")
                .RuleFor(x => x.Numero, f => f.Random.Int(1, 5))
                .RuleFor(x => x.Dias, dias)
                .Generate(4);

            return new CalendarioEventoDTO { Semanas = semanas };
        }

        private static List<EventoDetalheDTO> GerarListaEventoDetalheDTO(int quantidade) => new Faker<EventoDetalheDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.TipoId, f => f.PickRandom<TipoEvento>())
            .RuleFor(x => x.Tipo, f => f.Lorem.Word())
            .RuleFor(x => x.Solicitante, f => f.Name.FullName())
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence())
            .RuleFor(x => x.CodigoTombo, f => f.Random.Replace("TB-####"))
            .RuleFor(x => x.AcervoSolicitacaoId, f => f.Random.Long(1, 500))
            .RuleFor(x => x.Descricao, f => f.Lorem.Sentence())
            .RuleFor(x => x.Justificativa, f => f.Lorem.Paragraph())
            .RuleFor(x => x.SituacaoSolicitacaoItemId, f => f.PickRandom<SituacaoSolicitacaoItem>())
            .RuleFor(x => x.SituacaoSolicitacaoItemDescricao, f => f.Lorem.Word())
            .RuleFor(x => x.Horario, f => $"{f.Random.Int(0, 23):D2}:{f.Random.Int(0, 59):D2}")
            .Generate(quantidade);
    }
}