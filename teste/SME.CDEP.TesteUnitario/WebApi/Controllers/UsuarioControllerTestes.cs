using Bogus;
using Bogus.Extensions.Brazil;
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
    public class UsuarioControllerTestes
    {
        private readonly Mock<IServicoUsuario> servicoUsuarioMock;
        private readonly UsuarioController sut;
        private readonly Faker _faker;

        public UsuarioControllerTestes()
        {
            var mocker = new AutoMocker();

            servicoUsuarioMock = mocker.GetMock<IServicoUsuario>();

            sut = mocker.CreateInstance<UsuarioController>();
            _faker = new();
        }

        [Fact]
        public async Task DadoUsuarioExternoValido_QuandoInserir_EntaoRetornaOkComVerdadeiro()
        {
            // Arrange
            var dto = GerarUsuarioExternoDTO();

            servicoUsuarioMock
                .Setup(s => s.InserirUsuarioExterno(dto))
                .ReturnsAsync(true);

            // Act
            var resultado = await sut.Inserir(dto, servicoUsuarioMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);
            servicoUsuarioMock.Verify(s => s.InserirUsuarioExterno(dto), Times.Once);
        }

        [Fact]
        public async Task DadoLoginValido_QuandoSolicitarRecuperacaoSenha_EntaoRetornaOkComMensagem()
        {
            // Arrange
            var login = _faker.Internet.UserName();
            var mensagemRetorno = "Email de recuperação enviado com sucesso.";

            servicoUsuarioMock
                .Setup(s => s.SolicitarRecuperacaoSenha(login))
                .ReturnsAsync(mensagemRetorno);

            // Act
            var resultado = await sut.SolicitarRecuperacaoSenha(login, servicoUsuarioMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(mensagemRetorno);
            servicoUsuarioMock.Verify(s => s.SolicitarRecuperacaoSenha(login), Times.Once);
        }

        [Fact]
        public async Task DadoTokenValido_QuandoTokenRecuperacaoSenhaEstaValidoAsync_EntaoRetornaOkComVerdadeiro()
        {
            // Arrange
            var token = Guid.NewGuid();

            servicoUsuarioMock
                .Setup(s => s.TokenRecuperacaoSenhaEstaValido(token))
                .ReturnsAsync(true);

            // Act
            var resultado = await sut.TokenRecuperacaoSenhaEstaValidoAsync(token, servicoUsuarioMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);
            servicoUsuarioMock.Verify(s => s.TokenRecuperacaoSenhaEstaValido(token), Times.Once);
        }

        [Fact]
        public async Task DadoDtoRecuperacaoSenhaValido_QuandoRecuperarSenha_EntaoRetornaOkComPerfilDeUsuario()
        {
            // Arrange
            var dto = GerarRecuperacaoSenhaDto();
            var retornoEsperado = GerarRetornoPerfilUsuarioDTO();

            servicoUsuarioMock
                .Setup(s => s.AlterarSenhaComTokenRecuperacao(dto))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await sut.RecuperarSenha(dto, servicoUsuarioMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(retornoEsperado);
            servicoUsuarioMock.Verify(s => s.AlterarSenhaComTokenRecuperacao(dto), Times.Once);
        }

        [Fact]
        public async Task DadoLoginValido_QuandoMeusDados_EntaoRetornaOkComDadosDoUsuario()
        {
            // Arrange
            var login = _faker.Internet.UserName();
            var dadosEsperados = GerarDadosUsuarioDTO();

            servicoUsuarioMock
                .Setup(s => s.ObterMeusDados(login))
                .ReturnsAsync(dadosEsperados);

            // Act
            var resultado = await sut.MeusDados(login, servicoUsuarioMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dadosEsperados);
            servicoUsuarioMock.Verify(s => s.ObterMeusDados(login), Times.Once);
        }

        [Fact]
        public async Task DadoSenhaAlteracaoValida_QuandoAlterarSenha_EntaoRetornaOkComVerdadeiro()
        {
            // Arrange
            var login = _faker.Internet.UserName();
            var dto = GerarAlterarSenhaUsuarioDTO();

            servicoUsuarioMock
                .Setup(s => s.AlterarSenha(login, dto))
                .ReturnsAsync(true);

            // Act
            var resultado = await sut.AlterarSenha(login, dto, servicoUsuarioMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);
            servicoUsuarioMock.Verify(s => s.AlterarSenha(login, dto), Times.Once);
        }

        [Fact]
        public async Task DadoEmailValido_QuandoAlterarEmail_EntaoRetornaOkComVerdadeiro()
        {
            // Arrange
            var login = _faker.Internet.UserName();
            var dto = new EmailUsuarioDTO { Email = _faker.Internet.Email() };

            servicoUsuarioMock
                .Setup(s => s.AlterarEmail(login, dto.Email))
                .ReturnsAsync(true);

            // Act
            var resultado = await sut.AlterarEmail(login, dto, servicoUsuarioMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);
            servicoUsuarioMock.Verify(s => s.AlterarEmail(login, dto.Email), Times.Once);
        }

        [Fact]
        public async Task DadoEnderecoValido_QuandoAlterarEnderecoAcervo_EntaoRetornaOkComVerdadeiro()
        {
            // Arrange
            var login = _faker.Internet.UserName();
            var dto = GerarEnderecoUsuarioExternoDTO();

            servicoUsuarioMock
                .Setup(s => s.AlterarEndereco(login, dto))
                .ReturnsAsync(true);

            // Act
            var resultado = await sut.AlterarEnderecoAcervo(login, dto, servicoUsuarioMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);
            servicoUsuarioMock.Verify(s => s.AlterarEndereco(login, dto), Times.Once);
        }

        [Fact]
        public async Task DadoTelefoneValido_QuandoAlterarTelefoneAcervo_EntaoRetornaOkComVerdadeiro()
        {
            // Arrange
            var login = _faker.Internet.UserName();
            var dto = new TelefoneUsuarioExternoDTO { Telefone = _faker.Phone.PhoneNumber() };

            servicoUsuarioMock
                .Setup(s => s.AlterarTelefone(login, dto.Telefone))
                .ReturnsAsync(true);

            // Act
            var resultado = await sut.AlterarTelefoneAcervo(login, dto, servicoUsuarioMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);
            servicoUsuarioMock.Verify(s => s.AlterarTelefone(login, dto.Telefone), Times.Once);
        }

        [Fact]
        public async Task DadoTipoUsuarioValido_QuandoAlterarTipoUsuario_EntaoRetornaOkComVerdadeiro()
        {
            // Arrange
            var login = _faker.Internet.UserName();
            var dto = new TipoUsuarioExternoDTO { Tipo = (int)TipoUsuario.ESTUDANTE };

            servicoUsuarioMock
                .Setup(s => s.AlterarTipoUsuario(login, dto))
                .ReturnsAsync(true);

            // Act
            var resultado = await sut.AlterarTipoUsuario(login, dto, servicoUsuarioMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);
            servicoUsuarioMock.Verify(s => s.AlterarTipoUsuario(login, dto), Times.Once);
        }

        [Fact]
        public async Task DadoCpfValido_QuandoValidarCpfExistente_EntaoRetornaOkComResultado()
        {
            // Arrange
            var cpf = _faker.Person.Cpf();

            servicoUsuarioMock
                .Setup(s => s.ValidarCpfExistente(cpf))
                .ReturnsAsync(true);

            // Act
            var resultado = await sut.ValidarCpfExistente(cpf, servicoUsuarioMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(true);
            servicoUsuarioMock.Verify(s => s.ValidarCpfExistente(cpf), Times.Once);
        }

        [Fact]
        public async Task DadoRequisicaoValida_QuandoObterDadosSolicitante_EntaoRetornaOkComDados()
        {
            // Arrange
            var dadosEsperados = GerarDadosSolicitanteDto();

            servicoUsuarioMock
                .Setup(s => s.ObterDadosSolicitante())
                .ReturnsAsync(dadosEsperados);

            // Act
            var resultado = await sut.ObterDadosSolicitante(servicoUsuarioMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dadosEsperados);
            servicoUsuarioMock.Verify(s => s.ObterDadosSolicitante(), Times.Once);
        }

        [Fact]
        public async Task DadoRequisicaoValida_QuandoObterUsuariosComPerfisResponsavel_EntaoRetornaOkComListaDeResponsaveis()
        {
            // Arrange
            var listaEsperada = new List<ResponsavelDTO>
            {
                new() { Login = _faker.Internet.UserName(), Nome = _faker.Name.FullName() },
                new() { Login = _faker.Internet.UserName(), Nome = _faker.Name.FullName() }
            };

            servicoUsuarioMock
                .Setup(s => s.ObterUsuariosComPerfisResponsavel())
                .ReturnsAsync(listaEsperada);

            // Act
            var resultado = await sut.ObterUsuariosComPerfisResponsavel(servicoUsuarioMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(listaEsperada);
            servicoUsuarioMock.Verify(s => s.ObterUsuariosComPerfisResponsavel(), Times.Once);
        }

        [Fact]
        public async Task DadoRfOuCpfValido_QuandoObterDadosSolicitantePorRfOuCpf_EntaoRetornaOkComDados()
        {
            // Arrange
            var rfCpf = _faker.Random.Replace("#######");
            var dadosEsperados = GerarDadosSolicitanteDto();

            servicoUsuarioMock
                .Setup(s => s.ObterDadosSolicitantePorRfOuCpf(rfCpf))
                .ReturnsAsync(dadosEsperados);

            // Act
            var resultado = await sut.ObterDadosSolicitantePorRfOuCpf(rfCpf, servicoUsuarioMock.Object);

            // Assert
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(dadosEsperados);
            servicoUsuarioMock.Verify(s => s.ObterDadosSolicitantePorRfOuCpf(rfCpf), Times.Once);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static UsuarioExternoDTO GerarUsuarioExternoDTO() => new Faker<UsuarioExternoDTO>("pt_BR")
            .RuleFor(x => x.Cpf, f => f.Person.Cpf(false))
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Nome, f => f.Name.FullName())
            .RuleFor(x => x.Telefone, f => f.Phone.PhoneNumber())
            .RuleFor(x => x.Endereco, f => f.Address.StreetName())
            .RuleFor(x => x.Complemento, f => f.Address.SecondaryAddress())
            .RuleFor(x => x.Numero, f => f.Address.BuildingNumber())
            .RuleFor(x => x.Cidade, f => f.Address.City())
            .RuleFor(x => x.Estado, f => f.Address.StateAbbr())
            .RuleFor(x => x.Cep, f => f.Address.ZipCode())
            .RuleFor(x => x.Senha, f => "Senha@123")
            .RuleFor(x => x.ConfirmarSenha, f => "Senha@123")
            .RuleFor(x => x.Tipo, f => f.PickRandom<TipoUsuario>())
            .RuleFor(x => x.Bairro, f => f.Address.County())
            .RuleFor(x => x.Instituicao, f => f.Company.CompanyName())
            .Generate();

        private static RecuperacaoSenhaDto GerarRecuperacaoSenhaDto() => new Faker<RecuperacaoSenhaDto>("pt_BR")
            .RuleFor(x => x.NovaSenha, f => f.Internet.Password())
            .RuleFor(x => x.Token, f => Guid.NewGuid())
            .Generate();

        private static DadosUsuarioDTO GerarDadosUsuarioDTO() => new Faker<DadosUsuarioDTO>("pt_BR")
            .RuleFor(x => x.Nome, f => f.Name.FullName())
            .RuleFor(x => x.Cpf, f => f.Person.Cpf(false))
            .RuleFor(x => x.Login, f => f.Internet.UserName())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Telefone, f => f.Phone.PhoneNumber())
            .RuleFor(x => x.Endereco, f => f.Address.StreetName())
            .RuleFor(x => x.Numero, f => f.Address.BuildingNumber())
            .RuleFor(x => x.Complemento, f => f.Address.SecondaryAddress())
            .RuleFor(x => x.Bairro, f => f.Address.County())
            .RuleFor(x => x.Cep, f => f.Address.ZipCode())
            .RuleFor(x => x.Cidade, f => f.Address.City())
            .RuleFor(x => x.Estado, f => f.Address.StateAbbr())
            .RuleFor(x => x.Tipo, f => f.Random.Int(1, 5))
            .RuleFor(x => x.Instituicao, f => f.Company.CompanyName())
            .Generate();

        private static AlterarSenhaUsuarioDTO GerarAlterarSenhaUsuarioDTO() => new Faker<AlterarSenhaUsuarioDTO>("pt_BR")
            .RuleFor(x => x.SenhaAtual, f => "Antiga@123")
            .RuleFor(x => x.SenhaNova, f => "Nova@1234")
            .RuleFor(x => x.ConfirmarSenha, f => "Nova@1234")
            .Generate();

        private static EnderecoUsuarioExternoDTO GerarEnderecoUsuarioExternoDTO() => new Faker<EnderecoUsuarioExternoDTO>("pt_BR")
            .RuleFor(x => x.Endereco, f => f.Address.StreetName())
            .RuleFor(x => x.Complemento, f => f.Address.SecondaryAddress())
            .RuleFor(x => x.Numero, f => f.Address.BuildingNumber())
            .RuleFor(x => x.Cidade, f => f.Address.City())
            .RuleFor(x => x.Estado, f => f.Address.StateAbbr())
            .RuleFor(x => x.Cep, f => f.Address.ZipCode())
            .RuleFor(x => x.Bairro, f => f.Address.County())
            .Generate();

        private static DadosSolicitanteDto GerarDadosSolicitanteDto() => new Faker<DadosSolicitanteDto>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 100))
            .RuleFor(x => x.Nome, f => f.Name.FullName())
            .RuleFor(x => x.Login, f => f.Internet.UserName())
            .RuleFor(x => x.Telefone, f => f.Phone.PhoneNumber())
            .RuleFor(x => x.Endereco, f => f.Address.FullAddress())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Tipo, f => f.Name.JobTitle())
            .RuleFor(x => x.TipoId, f => f.PickRandom<TipoUsuario>())
            .Generate();

        private static RetornoPerfilUsuarioDTO GerarRetornoPerfilUsuarioDTO() => new Faker<RetornoPerfilUsuarioDTO>("pt_BR")
            .RuleFor(x => x.UsuarioNome, f => f.Name.FullName())
            .RuleFor(x => x.UsuarioLogin, f => f.Internet.UserName())
            .RuleFor(x => x.DataHoraExpiracao, f => f.Date.Future())
            .RuleFor(x => x.Token, f => f.Random.Hash())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Autenticado, f => true)
            .RuleFor(x => x.PerfilUsuario, f => new List<PerfilUsuarioDTO> { new(Guid.NewGuid(), "Admin") })
            .Generate();
    }
}