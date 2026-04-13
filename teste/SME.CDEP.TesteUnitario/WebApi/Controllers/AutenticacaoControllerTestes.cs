using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Webapi.Controllers;

namespace SME.CDEP.TesteUnitario.Webapi.Controllers
{
    public class AutenticacaoControllerTestes
    {
        private readonly Mock<IServicoUsuario> servicoUsuarioMock;
        private readonly AutenticacaoController sut;

        public AutenticacaoControllerTestes()
        {
            var mocker = new AutoMocker();

            servicoUsuarioMock = mocker.GetMock<IServicoUsuario>();

            sut = mocker.CreateInstance<AutenticacaoController>();
        }

        [Fact]
        public async Task DadoAutenticacaoDtoValido_QuandoAutenticar_EntaoRetornaOkComRetornoPerfilUsuarioDto()
        {
            // Arrange
            var dto = GerarAutenticacaoDTO();
            var retornoEsperado = GerarRetornoPerfilUsuarioDTO();

            servicoUsuarioMock
                .Setup(s => s.Autenticar(dto.Login, dto.Senha))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await sut.Autenticar(dto, servicoUsuarioMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(retornoEsperado);
            servicoUsuarioMock.Verify(s => s.Autenticar(dto.Login, dto.Senha), Times.Once);
        }

        [Fact]
        public async Task DadoTokenValido_QuandoRevalidar_EntaoRetornaOkComRetornoPerfilUsuarioDto()
        {
            // Arrange
            var dto = GerarAutenticacaoRevalidarDTO();
            var retornoEsperado = GerarRetornoPerfilUsuarioDTO();

            servicoUsuarioMock
                .Setup(s => s.RevalidarToken(dto.Token))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await sut.Revalidar(dto, servicoUsuarioMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(retornoEsperado);
            servicoUsuarioMock.Verify(s => s.RevalidarToken(dto.Token), Times.Once);
        }

        [Fact]
        public async Task DadoPerfilUsuarioIdValido_QuandoAtualizarPerfil_EntaoRetornaOkComRetornoPerfilUsuarioDto()
        {
            // Arrange
            var perfilUsuarioId = Guid.NewGuid();
            var retornoEsperado = GerarRetornoPerfilUsuarioDTO();

            servicoUsuarioMock
                .Setup(s => s.AtualizarPerfil(perfilUsuarioId))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await sut.AtualizarPerfil(perfilUsuarioId, servicoUsuarioMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(retornoEsperado);
            servicoUsuarioMock.Verify(s => s.AtualizarPerfil(perfilUsuarioId), Times.Once);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static AutenticacaoDTO GerarAutenticacaoDTO() => new Faker<AutenticacaoDTO>("pt_BR")
            .RuleFor(x => x.Login, f => f.Internet.UserName())
            .RuleFor(x => x.Senha, f => f.Internet.Password(8))
            .Generate();

        private static AutenticacaoRevalidarDTO GerarAutenticacaoRevalidarDTO() => new Faker<AutenticacaoRevalidarDTO>("pt_BR")
            .RuleFor(x => x.Token, f => f.Random.AlphaNumeric(100))
            .Generate();

        private static RetornoPerfilUsuarioDTO GerarRetornoPerfilUsuarioDTO() => new Faker<RetornoPerfilUsuarioDTO>("pt_BR")
            .RuleFor(x => x.UsuarioNome, f => f.Name.FullName())
            .RuleFor(x => x.UsuarioLogin, f => f.Internet.UserName())
            .RuleFor(x => x.DataHoraExpiracao, f => f.Date.Future())
            .RuleFor(x => x.Token, f => f.Random.AlphaNumeric(100))
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Autenticado, true)
            .RuleFor(x => x.PerfilUsuario, f => new List<PerfilUsuarioDTO>
            {
                new PerfilUsuarioDTO(Guid.NewGuid(), f.Name.JobTitle())
            })
            .Generate();
    }
}