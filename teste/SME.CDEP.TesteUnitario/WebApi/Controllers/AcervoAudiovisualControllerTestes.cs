using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Enumerados;
using SME.CDEP.Webapi.Controllers;

namespace SME.CDEP.TesteUnitario.Webapi.Controllers
{
    public class AcervoAudiovisualControllerTestes
    {
        private readonly Mock<IServicoAcervoAudiovisual> servicoAcervoAudiovisualMock;
        private readonly AcervoAudiovisualController sut;

        public AcervoAudiovisualControllerTestes()
        {
            var mocker = new AutoMocker();

            servicoAcervoAudiovisualMock = mocker.GetMock<IServicoAcervoAudiovisual>();

            sut = mocker.CreateInstance<AcervoAudiovisualController>();
        }

        [Fact]
        public async Task DadoAcervoAudiovisualCadastroValido_QuandoInserir_EntaoRetornaOkComId()
        {
            // Arrange
            var dto = GerarAcervoAudiovisualCadastroDTO();
            var idGerado = new Faker().Random.Long(1, 1000);

            servicoAcervoAudiovisualMock
                .Setup(s => s.Inserir(dto))
                .ReturnsAsync(idGerado);

            // Act
            var resultado = await sut.Inserir(dto, servicoAcervoAudiovisualMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(idGerado);
            servicoAcervoAudiovisualMock.Verify(s => s.Inserir(dto), Times.Once);
        }

        [Fact]
        public async Task DadoAcervoAudiovisualAlteracaoValida_QuandoAlterar_EntaoRetornaOkComAcervoAlterado()
        {
            // Arrange
            var dto = GerarAcervoAudiovisualAlteracaoDTO();
            var dtoRetorno = GerarAcervoAudiovisualDTO();

            servicoAcervoAudiovisualMock
                .Setup(s => s.Alterar(dto))
                .ReturnsAsync(dtoRetorno);

            // Act
            var resultado = await sut.Alterar(dto, servicoAcervoAudiovisualMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoRetorno);
            servicoAcervoAudiovisualMock.Verify(s => s.Alterar(dto), Times.Once);
        }

        [Fact]
        public async Task DadoAcervoIdValido_QuandoObterPorId_EntaoRetornaOkComAcervoEncontrado()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);
            var dtoRetorno = GerarAcervoAudiovisualDTO();

            servicoAcervoAudiovisualMock
                .Setup(s => s.ObterPorId(id))
                .ReturnsAsync(dtoRetorno);

            // Act
            var resultado = await sut.ObterPorId(id, servicoAcervoAudiovisualMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoRetorno);
            servicoAcervoAudiovisualMock.Verify(s => s.ObterPorId(id), Times.Once);
        }

        [Fact]
        public async Task DadoAcervoIdValido_QuandoExclusaoLogica_EntaoRetornaOkComResultadoBooleano()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);
            var exclusaoSucesso = true;

            servicoAcervoAudiovisualMock
                .Setup(s => s.Excluir(id))
                .ReturnsAsync(exclusaoSucesso);

            // Act
            var resultado = await sut.ExclusaoLogica(id, servicoAcervoAudiovisualMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(exclusaoSucesso);
            servicoAcervoAudiovisualMock.Verify(s => s.Excluir(id), Times.Once);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static AcervoAudiovisualCadastroDTO GerarAcervoAudiovisualCadastroDTO() => new Faker<AcervoAudiovisualCadastroDTO>("pt_BR")
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.Descricao, f => f.Lorem.Paragraph())
            .RuleFor(x => x.Codigo, f => f.Random.AlphaNumeric(10))
            .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
            .RuleFor(x => x.SituacaoAcervo, f => f.PickRandom<SituacaoAcervo>())
            .RuleFor(x => x.Localizacao, f => f.Address.FullAddress())
            .RuleFor(x => x.Procedencia, f => f.Lorem.Sentence())
            .RuleFor(x => x.Copia, f => f.Lorem.Word())
            .RuleFor(x => x.PermiteUsoImagem, f => f.Random.Bool())
            .RuleFor(x => x.ConservacaoId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.SuporteId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.Duracao, f => $"{f.Random.Int(0, 2):D2}:{f.Random.Int(0, 59):D2}:{f.Random.Int(0, 59):D2}")
            .RuleFor(x => x.CromiaId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.TamanhoArquivo, f => $"{f.Random.Int(1, 500)} MB")
            .RuleFor(x => x.Acessibilidade, f => f.Lorem.Word())
            .RuleFor(x => x.Disponibilizacao, f => f.Lorem.Sentence())
            .Generate();

        private static AcervoAudiovisualAlteracaoDTO GerarAcervoAudiovisualAlteracaoDTO() => new Faker<AcervoAudiovisualAlteracaoDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.AcervoId, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
            .RuleFor(x => x.SuporteId, f => f.Random.Long(1, 100))
            .Generate();

        private static AcervoAudiovisualDTO GerarAcervoAudiovisualDTO() => new Faker<AcervoAudiovisualDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.AcervoId, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.Codigo, f => f.Random.AlphaNumeric(10))
            .RuleFor(x => x.Descricao, f => f.Lorem.Paragraph())
            .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
            .RuleFor(x => x.Duracao, f => $"{f.Random.Int(0, 2):D2}:{f.Random.Int(0, 59):D2}:{f.Random.Int(0, 59):D2}")
            .Generate();
    }
}