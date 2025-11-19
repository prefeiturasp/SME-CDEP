using Bogus;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Webapi.Controllers;

namespace SME.CDEP.TesteUnitario.WebApi.Controllers
{
    public class PainelGerencialControllerTestes
    {
        private readonly Mock<IServicoPainelGerencial> _servicoPainelGerencialMock;
        private readonly PainelGerencialController _painelGerencialController;
        private readonly Faker _faker;

        public PainelGerencialControllerTestes()
        {
            var mocker = new AutoMocker();
            _servicoPainelGerencialMock = mocker.GetMock<IServicoPainelGerencial>();
            _painelGerencialController = mocker.CreateInstance<PainelGerencialController>();
            _faker = new();
        }
    }
}
