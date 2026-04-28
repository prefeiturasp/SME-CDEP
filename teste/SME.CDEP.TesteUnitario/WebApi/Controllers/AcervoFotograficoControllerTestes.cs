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
    public class AcervoFotograficoControllerTestes
    {
        private readonly Mock<IServicoAcervoFotografico> servicoAcervoFotograficoMock;
        private readonly AcervoFotograficoController sut;

        public AcervoFotograficoControllerTestes()
        {
            var mocker = new AutoMocker();

            servicoAcervoFotograficoMock = mocker.GetMock<IServicoAcervoFotografico>();

            sut = mocker.CreateInstance<AcervoFotograficoController>();
        }

        [Fact]
        public async Task DadoAcervoFotograficoCadastroValido_QuandoInserir_EntaoRetornaOkComId()
        {
            // Arrange
            var dto = GerarAcervoFotograficoCadastroDTO();
            var idGerado = new Faker().Random.Long(1, 1000);

            servicoAcervoFotograficoMock
                .Setup(s => s.Inserir(dto))
                .ReturnsAsync(idGerado);

            // Act
            var resultado = await sut.Inserir(dto, servicoAcervoFotograficoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(idGerado);
            servicoAcervoFotograficoMock.Verify(s => s.Inserir(dto), Times.Once);
        }

        [Fact]
        public async Task DadoAcervoFotograficoAlteracaoValida_QuandoAlterar_EntaoRetornaOkComAcervoAlterado()
        {
            // Arrange
            var dto = GerarAcervoFotograficoAlteracaoDTO();
            var dtoRetorno = GerarAcervoFotograficoDTO();

            servicoAcervoFotograficoMock
                .Setup(s => s.Alterar(dto))
                .ReturnsAsync(dtoRetorno);

            // Act
            var resultado = await sut.Alterar(dto, servicoAcervoFotograficoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoRetorno);
            servicoAcervoFotograficoMock.Verify(s => s.Alterar(dto), Times.Once);
        }

        [Fact]
        public async Task DadoAcervoIdValido_QuandoObterPorId_EntaoRetornaOkComAcervoEncontrado()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);
            var dtoRetorno = GerarAcervoFotograficoDTO();

            servicoAcervoFotograficoMock
                .Setup(s => s.ObterPorId(id))
                .ReturnsAsync(dtoRetorno);

            // Act
            var resultado = await sut.ObterPorId(id, servicoAcervoFotograficoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoRetorno);
            servicoAcervoFotograficoMock.Verify(s => s.ObterPorId(id), Times.Once);
        }

        [Fact]
        public async Task DadoAcervoIdValido_QuandoExclusaoLogica_EntaoRetornaOkComResultadoBooleano()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);
            var exclusaoSucesso = true;

            servicoAcervoFotograficoMock
                .Setup(s => s.Excluir(id))
                .ReturnsAsync(exclusaoSucesso);

            // Act
            var resultado = await sut.ExclusaoLogica(id, servicoAcervoFotograficoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(exclusaoSucesso);
            servicoAcervoFotograficoMock.Verify(s => s.Excluir(id), Times.Once);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static AcervoFotograficoCadastroDTO GerarAcervoFotograficoCadastroDTO() => new Faker<AcervoFotograficoCadastroDTO>("pt_BR")
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.Descricao, f => f.Lorem.Paragraph())
            .RuleFor(x => x.Codigo, f => f.Random.AlphaNumeric(10))
            .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
            .RuleFor(x => x.SituacaoAcervo, f => f.PickRandom<SituacaoAcervo>())
            .RuleFor(x => x.Localizacao, f => f.Address.FullAddress())
            .RuleFor(x => x.Procedencia, f => f.Lorem.Sentence())
            .RuleFor(x => x.CopiaDigital, f => f.Random.Bool())
            .RuleFor(x => x.PermiteUsoImagem, f => f.Random.Bool())
            .RuleFor(x => x.ConservacaoId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.Quantidade, f => f.Random.Int(1, 50))
            .RuleFor(x => x.Largura, f => f.Random.Double(10, 50).ToString("N2"))
            .RuleFor(x => x.Altura, f => f.Random.Double(10, 50).ToString("N2"))
            .RuleFor(x => x.SuporteId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.FormatoId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.TamanhoArquivo, f => $"{f.Random.Int(1, 500)} MB")
            .RuleFor(x => x.CromiaId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.Resolucao, f => $"{f.Random.Int(72, 300)} DPI")
            .Generate();

        private static AcervoFotograficoAlteracaoDTO GerarAcervoFotograficoAlteracaoDTO() => new Faker<AcervoFotograficoAlteracaoDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.AcervoId, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
            .RuleFor(x => x.Procedencia, f => f.Lorem.Sentence())
            .RuleFor(x => x.ConservacaoId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.Quantidade, f => f.Random.Int(1, 50))
            .RuleFor(x => x.SuporteId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.FormatoId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.TamanhoArquivo, f => $"{f.Random.Int(1, 500)} MB")
            .RuleFor(x => x.CromiaId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.Resolucao, f => $"{f.Random.Int(72, 300)} DPI")
            .Generate();

        private static AcervoFotograficoDTO GerarAcervoFotograficoDTO() => new Faker<AcervoFotograficoDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.AcervoId, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.Codigo, f => f.Random.AlphaNumeric(10))
            .RuleFor(x => x.Localizacao, f => f.Address.FullAddress())
            .RuleFor(x => x.Procedencia, f => f.Lorem.Sentence())
            .RuleFor(x => x.Descricao, f => f.Lorem.Paragraph())
            .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
            .RuleFor(x => x.TamanhoArquivo, f => $"{f.Random.Int(1, 500)} MB")
            .RuleFor(x => x.Resolucao, f => $"{f.Random.Int(72, 300)} DPI")
            .Generate();
    }
}