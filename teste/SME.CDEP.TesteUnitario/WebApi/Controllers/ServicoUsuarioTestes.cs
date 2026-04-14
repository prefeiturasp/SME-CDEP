using AutoMapper;
using Bogus;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoUsuarioTestes
    {
        private readonly Mock<IRepositorioUsuario> repositorioUsuarioMock;
        private readonly Mock<IServicoAcessos> servicoAcessosMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IServicoPerfilUsuario> servicoPerfilUsuarioMock;
        private readonly Mock<IContextoAplicacao> contextoAplicacaoMock;
        private readonly ServicoUsuario sut;

        public ServicoUsuarioTestes()
        {
            var mocker = new AutoMocker();

            repositorioUsuarioMock = mocker.GetMock<IRepositorioUsuario>();
            servicoAcessosMock = mocker.GetMock<IServicoAcessos>();
            mapperMock = mocker.GetMock<IMapper>();
            servicoPerfilUsuarioMock = mocker.GetMock<IServicoPerfilUsuario>();
            contextoAplicacaoMock = mocker.GetMock<IContextoAplicacao>();

            sut = mocker.CreateInstance<ServicoUsuario>();
        }

        [Fact]
        public async Task DadoUsuarioDtoValido_QuandoInserir_EntaoRetornaIdUsuarioInserido()
        {
            // Arrange
            var dto = GerarUsuarioDTO();
            var entidade = new Usuario();
            var idGerado = 10L;

            mapperMock.Setup(m => m.Map<Usuario>(dto)).Returns(entidade);
            repositorioUsuarioMock.Setup(r => r.Inserir(entidade)).ReturnsAsync(idGerado);

            // Act
            var resultado = await sut.Inserir(dto);

            // Assert
            resultado.Should().Be(idGerado);
            repositorioUsuarioMock.Verify(r => r.Inserir(entidade), Times.Once);
        }

        [Fact]
        public async Task DadoExistemUsuarios_QuandoObterTodos_EntaoRetornaListaDeUsuarios()
        {
            // Arrange
            var entidades = new List<Usuario> { new(), new() };
            var dtos = new List<UsuarioDTO> { new(), new() };

            repositorioUsuarioMock.Setup(r => r.ObterTodos()).ReturnsAsync(entidades);
            mapperMock.Setup(m => m.Map<UsuarioDTO>(It.IsAny<Usuario>())).Returns(dtos[0]);

            // Act
            var resultado = await sut.ObterTodos();

            // Assert
            resultado.Should().HaveCount(2);
            repositorioUsuarioMock.Verify(r => r.ObterTodos(), Times.Once);
        }

        [Fact]
        public async Task DadoUsuarioExternoValido_QuandoInserirUsuarioExterno_EntaoRetornaVerdadeiro()
        {
            // Arrange
            var dto = GerarUsuarioExternoDTOValido();
            var entidade = new Usuario();

            repositorioUsuarioMock.Setup(r => r.ObterPorLogin(It.IsAny<string>())).ReturnsAsync((Usuario)null!);
            servicoAcessosMock.Setup(s => s.UsuarioCadastradoCoreSSO(It.IsAny<string>())).ReturnsAsync(false);
            servicoAcessosMock.Setup(s => s.CadastrarUsuarioCoreSSO(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            mapperMock.Setup(m => m.Map<Usuario>(dto)).Returns(entidade);
            repositorioUsuarioMock.Setup(r => r.Inserir(entidade)).ReturnsAsync(1L);

            // Act
            var resultado = await sut.InserirUsuarioExterno(dto);

            // Assert
            resultado.Should().BeTrue();
            repositorioUsuarioMock.Verify(r => r.Inserir(entidade), Times.Once);
        }

        [Fact]
        public async Task DadoUsuarioExternoComEmailInvalido_QuandoInserirUsuarioExterno_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = GerarUsuarioExternoDTOValido();
            dto.Email = "email_invalido";

            repositorioUsuarioMock.Setup(r => r.ObterPorLogin(It.IsAny<string>())).ReturnsAsync((Usuario)null!);
            servicoAcessosMock.Setup(s => s.UsuarioCadastradoCoreSSO(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            Func<Task> acao = async () => await sut.InserirUsuarioExterno(dto);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>().WithMessage(MensagemNegocio.EMAIL_INVALIDO);
        }

        [Fact]
        public async Task DadoSenhasDiferentes_QuandoAlterarSenha_EntaoLancaNegocioException()
        {
            // Arrange
            var login = "login123";
            var dto = new AlterarSenhaUsuarioDTO
            {
                SenhaAtual = "Senha@123",
                SenhaNova = "Nova@1234",
                ConfirmarSenha = "Nova@Diferente"
            };

            // Act
            Func<Task> acao = async () => await sut.AlterarSenha(login, dto);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .Where(e => (e.Mensagens).Contains(MensagemNegocio.CONFIRMACAO_SENHA_DEVE_SER_IGUAL_A_SENHA));
        }

        [Fact]
        public async Task DadoCredenciaisValidas_QuandoAutenticar_EntaoRetornaPerfilUsuario()
        {
            // Arrange
            var login = "teste123";
            var senha = "Senha@123";
            var autenticacaoRetorno = new UsuarioAutenticacaoRetornoDTO { Login = login, Nome = "Teste", Email = "teste@teste.com" };
            var perfilRetorno = new RetornoPerfilUsuarioDTO { Autenticado = true, UsuarioLogin = login };

            servicoAcessosMock.Setup(s => s.Autenticar(login, senha)).ReturnsAsync(autenticacaoRetorno);
            repositorioUsuarioMock.Setup(r => r.ObterPorLogin(login)).ReturnsAsync(new Usuario());
            servicoPerfilUsuarioMock.Setup(s => s.ObterPerfisUsuario(login)).ReturnsAsync(perfilRetorno);

            // Act
            var resultado = await sut.Autenticar(login, senha);

            // Assert
            resultado.Should().BeEquivalentTo(perfilRetorno);
            repositorioUsuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public async Task DadoCredenciaisInvalidas_QuandoAutenticar_EntaoLancaNegocioException()
        {
            // Arrange
            var login = "teste123";
            var senha = "SenhaInvalida";
            var autenticacaoRetorno = new UsuarioAutenticacaoRetornoDTO { Login = string.Empty };

            servicoAcessosMock.Setup(s => s.Autenticar(login, senha)).ReturnsAsync(autenticacaoRetorno);

            // Act
            Func<Task> acao = async () => await sut.Autenticar(login, senha);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>().WithMessage(MensagemNegocio.USUARIO_OU_SENHA_INVALIDOS);
        }

        [Fact]
        public async Task DadoLoginValido_QuandoSolicitarRecuperacaoSenha_EntaoRetornaMensagemComEmailMascarado()
        {
            // Arrange
            var login = "teste123";
            var emailOriginal = "usuario.teste@gmail.com";

            servicoAcessosMock.Setup(s => s.SolicitarRecuperacaoSenha(login)).ReturnsAsync(emailOriginal);

            // Act
            var resultado = await sut.SolicitarRecuperacaoSenha(login);

            // Assert
            resultado.Should().Contain("usu**********@gmail.com");
            servicoAcessosMock.Verify(s => s.SolicitarRecuperacaoSenha(login), Times.Once);
        }

        [Fact]
        public void DadoContextoComClaims_QuandoObterPermissoes_EntaoRetornaListaDePermissoes()
        {
            // Arrange
            var claims = new List<Tuple<string, string>>
            {
                new(Constantes.CLAIM_PERMISSAO, Permissao.CadastroAcervo_C.ToString()),
                new(Constantes.CLAIM_PERMISSAO, Permissao.CadastroAssunto_A.ToString())
            };

            contextoAplicacaoMock.Setup(c => c.ObterVariavel(Constantes.CLAIMS)).Returns(claims);

            // Act
            var resultado = sut.ObterPermissoes().ToList();

            // Assert
            resultado.Should().HaveCount(2);
            resultado.Should().Contain(Permissao.CadastroAcervo_C);
            resultado.Should().Contain(Permissao.CadastroAssunto_A);
        }

        [Fact]
        public async Task DadoUsuarioExternoExistente_QuandoAlterarTelefone_EntaoRetornaVerdadeiroEAvaliaAtualizacao()
        {
            // Arrange
            var login = "12345678900";
            var telefoneNovo = "11999999999";
            var usuario = new Usuario { Login = login, TipoUsuario = TipoUsuario.POPULACAO_GERAL };

            repositorioUsuarioMock.Setup(r => r.ObterPorLogin(login)).ReturnsAsync(usuario);

            // Act
            var resultado = await sut.AlterarTelefone(login, telefoneNovo);

            // Assert
            resultado.Should().BeTrue();
            usuario.Telefone.Should().Be(telefoneNovo);
            repositorioUsuarioMock.Verify(r => r.Atualizar(usuario), Times.Once);
        }

        [Fact]
        public async Task DadoUsuarioInterno_QuandoAlterarTelefone_EntaoLancaNegocioException()
        {
            // Arrange
            var login = "admin";
            var usuario = new Usuario { Login = login, TipoUsuario = TipoUsuario.CORESSO };

            repositorioUsuarioMock.Setup(r => r.ObterPorLogin(login)).ReturnsAsync(usuario);

            // Act
            Func<Task> acao = async () => await sut.AlterarTelefone(login, "11999999999");

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.SO_EH_PERMITIDO_ALTERAR_ENDERECO_TELEFONE_DE_USUARIOS_EXTERNOS);
        }

        // ================= HELPER BOGUS GENERATORS ================= //

        private static UsuarioDTO GerarUsuarioDTO() => new Faker<UsuarioDTO>("pt_BR")
            .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
            .RuleFor(x => x.Login, f => f.Internet.UserName())
            .RuleFor(x => x.Nome, f => f.Name.FullName())
            .Generate();

        private static UsuarioExternoDTO GerarUsuarioExternoDTOValido() => new Faker<UsuarioExternoDTO>("pt_BR")
            .RuleFor(x => x.Cpf, f => f.Random.Replace("###########"))
            .RuleFor(x => x.Nome, f => f.Name.FullName())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Senha, "Senha@123") // Hardcoded para passar no regex complexo de segurança
            .RuleFor(x => x.ConfirmarSenha, "Senha@123")
            .RuleFor(x => x.Telefone, f => f.Phone.PhoneNumber())
            .RuleFor(x => x.Endereco, f => f.Address.StreetName())
            .RuleFor(x => x.Numero, f => f.Address.BuildingNumber())
            .RuleFor(x => x.Cidade, f => f.Address.City())
            .RuleFor(x => x.Estado, f => f.Address.StateAbbr())
            .RuleFor(x => x.Cep, f => f.Address.ZipCode())
            .RuleFor(x => x.Bairro, f => f.Address.County())
            .RuleFor(x => x.Tipo, f => TipoUsuario.POPULACAO_GERAL)
            .Generate();
    }
}