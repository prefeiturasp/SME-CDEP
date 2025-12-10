using AutoMapper;
using Bogus;
using Elasticsearch.Net;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
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
    private readonly FakeTimeProvider _fakeTimeProvider;
    private readonly ServicoPainelGerencial _servicoPainelGerencial;
    private readonly Faker _faker;

    public ServicoPainelGerencialTestes()
    {
        var mocker = new AutoMocker();
        _repositorioPainelGerencialMock = mocker.GetMock<IRepositorioPainelGerencial>();
        _mapperMock = mocker.GetMock<IMapper>();
        _fakeTimeProvider = new();
        _fakeTimeProvider.SetLocalTimeZone(TimeZoneInfo.Local);
        mocker.Use<TimeProvider>(_fakeTimeProvider);
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
            .Setup(r => r.ObterAcervosCadastradosAsync())
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

    [Fact]
    public async Task DadoQueExistemPesquisasNoBanco_QuandoObterQuantidadePesquisasMensais_EntaoDeveRetornarListaDeDtos()
    {
        // Arrange
        var ano = _faker.Random.Int(2000, 2024);
        var dataSimulada = new DateTimeOffset(ano, 6, 15, 12, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(dataSimulada);
        var sumarioConsultasMensal = new List<SumarioConsultaMensal>
        {
            new() { MesReferencia = DateOnly.FromDateTime(new DateTime(ano, 1, 1)), TotalConsultas = 150 },
            new() { MesReferencia = DateOnly.FromDateTime(new DateTime(ano, 2, 1)), TotalConsultas = 200 }
        };
        var quantidadePesquisasMensaisDto = new List<PainelGerencialQuantidadePesquisasMensaisDto>
        {
            new() { Id = 1, Nome = "Janeiro", Valor = 150 },
            new() { Id = 2, Nome = "Fevereiro", Valor = 200 }
        };
        _repositorioPainelGerencialMock
            .Setup(r => r.ObterSumarioConsultasMensalAsync(ano))
            .ReturnsAsync(sumarioConsultasMensal);
        _mapperMock
            .Setup(m => m.Map<List<PainelGerencialQuantidadePesquisasMensaisDto>>(sumarioConsultasMensal))
            .Returns(quantidadePesquisasMensaisDto);

        // Act
        var resultado = await _servicoPainelGerencial.ObterQuantidadePesquisasMensaisDoAnoAtualAsync();
        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(2);
        resultado[0].Id.Should().Be(1);
        resultado[0].Valor.Should().Be(150);
        resultado[0].Nome.Should().Be("Janeiro");
        resultado[1].Id.Should().Be(2);
        resultado[1].Valor.Should().Be(200);
        resultado[1].Nome.Should().Be("Fevereiro");
    }

    [Fact]
    public async Task DadoQueExistemSolicitacoesNoBanco_QuandoObterQuantidadeSolicitacoesMensais_EntaoDeveRetornarListaDeDtos()
    {
        // Arrange
        var ano = _faker.Random.Int(2000, 2024);
        var dataSimulada = new DateTimeOffset(ano, 8, 10, 10, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(dataSimulada);
        var quantidadeSolicitacoesMensais = new List<PainelGerencialQuantidadeSolicitacaoMensal>
        {
            new() { MesReferencia = DateOnly.FromDateTime(new DateTime(ano, 3, 1)), TotalSolicitacoes = 80 },
            new() { MesReferencia = DateOnly.FromDateTime(new DateTime(ano, 4, 1)), TotalSolicitacoes = 120 }
        };
        var quantidadeSolicitacoesMensaisDto = new List<PainelGerencialQuantidadeSolicitacaoMensalDto>
        {
            new() { Id = 3, Nome = "Março", Valor = 80 },
            new() { Id = 4, Nome = "Abril", Valor = 120 }
        };
        _repositorioPainelGerencialMock
            .Setup(r => r.ObterQuantidadeSolicitacoesMensaisAsync(ano))
            .ReturnsAsync(quantidadeSolicitacoesMensais);
        _mapperMock
            .Setup(m => m.Map<List<PainelGerencialQuantidadeSolicitacaoMensalDto>>(quantidadeSolicitacoesMensais))
            .Returns(quantidadeSolicitacoesMensaisDto);
        // Act
        var resultado = await _servicoPainelGerencial.ObterQuantidadeSolicitacoesMensaisDoAnoAtualAsync();
        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(2);
        resultado[0].Id.Should().Be(3);
        resultado[0].Nome.Should().Be("Março");
        resultado[0].Valor.Should().Be(80);
        resultado[1].Id.Should().Be(4);
        resultado[1].Nome.Should().Be("Abril");
        resultado[1].Valor.Should().Be(120);
    }

    [Fact]
    public async Task DadoQueExistemSolicitacoesNoBanco_QuandoObterQuantidadeSolicitacoesPorTipoAcervo_EntaoDeveRetornarListaDeDtos()
    {
        // Arrange
        var quantidadeSolicitacoesPorTipoAcervo = new List<PainelGerencialQuantidadeDeSolicitacoesPorTipoAcervo>
        {
            new() { TipoAcervo = TipoAcervo.Bibliografico, Quantidade = 300 },
            new() { TipoAcervo = TipoAcervo.Fotografico, Quantidade = 150 }
        };
        var quantidadeSolicitacoesPorTipoAcervoDto = new List<PainelGerencialQuantidadeSolicitacaoPorTipoDeAcervoDto>
        {
            new() { Id = (int)TipoAcervo.Bibliografico, Nome = TipoAcervo.Bibliografico.Descricao(), Valor = 300 },
            new() { Id = (int)TipoAcervo.Fotografico, Nome = TipoAcervo.Fotografico.Descricao(), Valor = 150 }
        };
        _repositorioPainelGerencialMock
            .Setup(r => r.ObterQuantidadeDeSolicitacoesPorTipoAcervoAsync())
            .ReturnsAsync(quantidadeSolicitacoesPorTipoAcervo);
        _mapperMock
            .Setup(m => m.Map<List<PainelGerencialQuantidadeSolicitacaoPorTipoDeAcervoDto>>(quantidadeSolicitacoesPorTipoAcervo))
            .Returns(quantidadeSolicitacoesPorTipoAcervoDto);
        // Act
        var resultado = await _servicoPainelGerencial.ObterQuantidadeDeSolicitacoesPorTipoAcervoAsync();
        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(2);
        resultado[0].Id.Should().Be((int)TipoAcervo.Bibliografico);
        resultado[0].Nome.Should().Be(TipoAcervo.Bibliografico.Descricao());
        resultado[0].Valor.Should().Be(300);
        resultado[1].Id.Should().Be((int)TipoAcervo.Fotografico);
        resultado[1].Nome.Should().Be(TipoAcervo.Fotografico.Descricao());
        resultado[1].Valor.Should().Be(150);
    }

    [Fact]
    public async Task DadoQueExistemEmprestimosNoBanco_QuandoObterQuantidadeAcervoEmprestadoPorSituacao_EntaoDeveRetornarListaDeDtos()
    {
        // Arrange
        var quantidadeAcervoEmprestadoPorSituacao = new List<PainelGerencialQuantidadeAcervoEmprestadoPorSituacao>
        {
            new() { Situacao = SituacaoEmprestimo.EMPRESTADO, Quantidade = 400 },
            new() { Situacao = SituacaoEmprestimo.DEVOLUCAO_EM_ATRASO, Quantidade = 75 }
        };
        var quantidadeAcervoEmprestadoPorSituacaoDto = new List<PainelGerencialQuantidadeAcervoEmprestadoPorSituacaoDto>
        {
            new() { Id = (int)SituacaoEmprestimo.EMPRESTADO, Nome = SituacaoEmprestimo.EMPRESTADO.Descricao(), Valor = 400 },
            new() { Id = (int)SituacaoEmprestimo.DEVOLUCAO_EM_ATRASO, Nome = SituacaoEmprestimo.DEVOLUCAO_EM_ATRASO.Descricao(), Valor = 75 }
        };
        _repositorioPainelGerencialMock
            .Setup(r => r.ObterQuantidadeAcervoEmprestadoPorSituacaoAsync())
            .ReturnsAsync(quantidadeAcervoEmprestadoPorSituacao);
        _mapperMock
            .Setup(m => m.Map<List<PainelGerencialQuantidadeAcervoEmprestadoPorSituacaoDto>>(quantidadeAcervoEmprestadoPorSituacao))
            .Returns(quantidadeAcervoEmprestadoPorSituacaoDto);
        // Act
        var resultado = await _servicoPainelGerencial.ObterQuantidadeAcervoEmprestadoPorSituacaoAsync();
        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(2);
        resultado[0].Id.Should().Be((int)SituacaoEmprestimo.EMPRESTADO);
        resultado[0].Nome.Should().Be(SituacaoEmprestimo.EMPRESTADO.Descricao());
        resultado[0].Valor.Should().Be(400);
        resultado[1].Id.Should().Be((int)SituacaoEmprestimo.DEVOLUCAO_EM_ATRASO);
        resultado[1].Nome.Should().Be(SituacaoEmprestimo.DEVOLUCAO_EM_ATRASO.Descricao());
        resultado[1].Valor.Should().Be(75);
    }
}