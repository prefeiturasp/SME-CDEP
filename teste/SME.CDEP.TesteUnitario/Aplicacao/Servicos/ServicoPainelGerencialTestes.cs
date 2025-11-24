using AutoMapper;
using Bogus;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos;

public class ServicoPainelGerencialTestes
{
    private readonly Mock<IRepositorioPainelGerencial> _repositorioPainelGerencialMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ServicoPainelGerencial _servicoPainelGerencial;
    private readonly Faker _faker;

    public ServicoPainelGerencialTestes()
    {
        var mocker = new AutoMocker();
        _repositorioPainelGerencialMock = mocker.GetMock<IRepositorioPainelGerencial>();
        _mapperMock = mocker.GetMock<IMapper>();
        _servicoPainelGerencial = mocker.CreateInstance<ServicoPainelGerencial>();
        _faker = new();
    }

    [Fact]
    public async Task DadoQueExistemAcervosNoBanco_QuandoObterAcervosCadastrados_EntaoDeveRetornarListaDeDtos()
    {
        // Arrange
        var acervosCadastrados = new List<PainelGerencialAcervosCadastrados>
        {
            new() { TipoAcervo = TipoAcervo.Bibliografico, Quantidade = 100 },
            new() { TipoAcervo = TipoAcervo.Fotografico, Quantidade = 50 }
        };
        var acervosCadastradosDto = new List<PainelGerencialAcervosCadastradosDto>
        {
            new() { Id = TipoAcervo.Bibliografico, Nome = TipoAcervo.Bibliografico.Descricao(), Valor = 100 },
            new() { Id = TipoAcervo.Fotografico, Nome = TipoAcervo.Fotografico.Descricao(), Valor = 50 }
        };
        _repositorioPainelGerencialMock
            .Setup(r => r.ObterAcervosCadastrados())
            .ReturnsAsync(acervosCadastrados);
        _mapperMock
            .Setup(m => m.Map<List<PainelGerencialAcervosCadastradosDto>>(acervosCadastrados))
            .Returns(acervosCadastradosDto);

        // Act
        var resultado = await _servicoPainelGerencial.ObterAcervosCadastradosAsync();

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(2);
        resultado[0].Id.Should().Be(TipoAcervo.Bibliografico);
        resultado[0].Nome.Should().Be(TipoAcervo.Bibliografico.Descricao());
        resultado[0].Valor.Should().Be(100);
        resultado[1].Id.Should().Be(TipoAcervo.Fotografico);
        resultado[1].Nome.Should().Be(TipoAcervo.Fotografico.Descricao());
        resultado[1].Valor.Should().Be(50);
    }
}