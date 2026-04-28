using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Enumerados;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Controllers;

namespace SME.CDEP.TesteUnitario.Webapi.Controllers
{
    public class AcervoBibliograficoControllerTestes
    {
        private readonly Mock<IServicoAcervoBibliografico> servicoAcervoBibliograficoMock;
        private readonly AcervoBibliograficoController sut;

        public AcervoBibliograficoControllerTestes()
        {
            var mocker = new AutoMocker();

            servicoAcervoBibliograficoMock = mocker.GetMock<IServicoAcervoBibliografico>();

            sut = mocker.CreateInstance<AcervoBibliograficoController>();
        }

        [Fact]
        public async Task DadoAcervoBibliograficoCadastroValido_QuandoInserir_EntaoRetornaOkComId()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDTO();
            var idGerado = new Faker().Random.Long(1, 1000);

            servicoAcervoBibliograficoMock
                .Setup(s => s.Inserir(dto))
                .ReturnsAsync(idGerado);

            // Act
            var resultado = await sut.Inserir(dto, servicoAcervoBibliograficoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(idGerado);
            servicoAcervoBibliograficoMock.Verify(s => s.Inserir(dto), Times.Once);
        }

        [Fact]
        public async Task DadoAcervoBibliograficoAlteracaoValida_QuandoAlterar_EntaoRetornaOkComAcervoAlterado()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoAlteracaoDTO();
            var dtoRetorno = GerarAcervoBibliograficoDTO();

            servicoAcervoBibliograficoMock
                .Setup(s => s.Alterar(dto))
                .ReturnsAsync(dtoRetorno);

            // Act
            var resultado = await sut.Alterar(dto, servicoAcervoBibliograficoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoRetorno);
            servicoAcervoBibliograficoMock.Verify(s => s.Alterar(dto), Times.Once);
        }

        [Fact]
        public async Task DadoAcervoIdValido_QuandoObterPorId_EntaoRetornaOkComAcervoEncontrado()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);
            var dtoRetorno = GerarAcervoBibliograficoDTO();

            servicoAcervoBibliograficoMock
                .Setup(s => s.ObterPorId(id))
                .ReturnsAsync(dtoRetorno);

            // Act
            var resultado = await sut.ObterPorId(id, servicoAcervoBibliograficoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dtoRetorno);
            servicoAcervoBibliograficoMock.Verify(s => s.ObterPorId(id), Times.Once);
        }

        [Fact]
        public async Task DadoAcervoIdValido_QuandoExclusaoLogica_EntaoRetornaOkComResultadoBooleano()
        {
            // Arrange
            var id = new Faker().Random.Long(1, 1000);
            var exclusaoSucesso = true;

            servicoAcervoBibliograficoMock
                .Setup(s => s.Excluir(id))
                .ReturnsAsync(exclusaoSucesso);

            // Act
            var resultado = await sut.ExclusaoLogica(id, servicoAcervoBibliograficoMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(exclusaoSucesso);
            servicoAcervoBibliograficoMock.Verify(s => s.Excluir(id), Times.Once);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static AcervoBibliograficoCadastroDTO GerarAcervoBibliograficoCadastroDTO() => new Faker<AcervoBibliograficoCadastroDTO>("pt_BR")
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.Descricao, f => f.Lorem.Paragraph())
            .RuleFor(x => x.Codigo, f => f.Random.AlphaNumeric(10))
            .RuleFor(x => x.CodigoNovo, f => f.Random.AlphaNumeric(10))
            .RuleFor(x => x.SubTitulo, f => f.Lorem.Sentence(2))
            .RuleFor(x => x.DataAcervo, f => f.Date.Past().ToString("dd/MM/yyyy"))
            .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
            .RuleFor(x => x.SituacaoAcervo, f => f.PickRandom<SituacaoAcervo>())
            .RuleFor(x => x.MaterialId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.EditoraId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.AssuntosIds, f => new[] { f.Random.Long(1, 10), f.Random.Long(11, 20) })
            .RuleFor(x => x.Edicao, f => f.Random.Int(1, 10).ToString())
            .RuleFor(x => x.NumeroPagina, f => f.Random.Int(50, 1000))
            .RuleFor(x => x.Largura, f => f.Random.Double(10, 50).ToString("N2"))
            .RuleFor(x => x.Altura, f => f.Random.Double(10, 50).ToString("N2"))
            .RuleFor(x => x.SerieColecaoId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.Volume, f => f.Random.Int(1, 5).ToString())
            .RuleFor(x => x.IdiomaId, f => f.Random.Long(1, 10))
            .RuleFor(x => x.LocalizacaoCDD, f => f.Random.AlphaNumeric(20))
            .RuleFor(x => x.LocalizacaoPHA, f => f.Random.AlphaNumeric(20))
            .RuleFor(x => x.NotasGerais, f => f.Lorem.Paragraph())
            .RuleFor(x => x.Isbn, f => f.Commerce.Ean13())
            .RuleFor(x => x.SituacaoSaldo, f => f.PickRandom<SituacaoSaldo>())
            .RuleFor(x => x.CreditosAutoresIds, f => new[] { f.Random.Long(1, 10) })
            .RuleFor(x => x.CoAutores, f => GerarListaCoAutorDTO().ToArray())
            .Generate();

        private static AcervoBibliograficoAlteracaoDTO GerarAcervoBibliograficoAlteracaoDTO() => new Faker<AcervoBibliograficoAlteracaoDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.AcervoId, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
            .RuleFor(x => x.MaterialId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.IdiomaId, f => f.Random.Long(1, 10))
            .RuleFor(x => x.LocalizacaoCDD, f => f.Random.AlphaNumeric(20))
            .Generate();

        private static AcervoBibliograficoDTO GerarAcervoBibliograficoDTO() => new Faker<AcervoBibliograficoDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.AcervoId, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(x => x.SubTitulo, f => f.Lorem.Sentence(2))
            .RuleFor(x => x.Codigo, f => f.Random.AlphaNumeric(10))
            .RuleFor(x => x.MaterialId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.EditoraId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.AssuntosIds, f => new[] { f.Random.Long(1, 10) })
            .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
            .RuleFor(x => x.Edicao, f => f.Random.Int(1, 10).ToString())
            .RuleFor(x => x.NumeroPagina, f => f.Random.Int(50, 1000))
            .RuleFor(x => x.Largura, f => f.Random.Double(10, 50).ToString("N2"))
            .RuleFor(x => x.Altura, f => f.Random.Double(10, 50).ToString("N2"))
            .RuleFor(x => x.Volume, f => f.Random.Int(1, 5).ToString())
            .RuleFor(x => x.IdiomaId, f => f.Random.Long(1, 10))
            .RuleFor(x => x.LocalizacaoCDD, f => f.Random.AlphaNumeric(20))
            .RuleFor(x => x.Isbn, f => f.Commerce.Ean13())
            .RuleFor(x => x.CreditosAutoresIds, f => new[] { f.Random.Long(1, 10) })
            .RuleFor(x => x.CoAutores, f => GerarListaCoAutorDTO().ToArray())
            .RuleFor(x => x.SituacaoSaldo, f => f.PickRandom<SituacaoSaldo>())
            .RuleFor(x => x.SituacaoAcervo, f => f.PickRandom<SituacaoAcervo>())
            .Generate();

        private static System.Collections.Generic.List<CoAutorDTO> GerarListaCoAutorDTO() => new Faker<CoAutorDTO>("pt_BR")
            .RuleFor(x => x.CreditoAutorId, f => f.Random.Long(1, 100))
            .RuleFor(x => x.TipoAutoria, f => f.Lorem.Word())
            .RuleFor(x => x.CreditoAutorNome, f => f.Name.FullName())
            .Generate(2)
            .ToList();
    }
}