using AutoMapper;
using Bogus;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

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
}
