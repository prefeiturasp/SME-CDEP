using Bogus.Extensions.Brazil;
using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;

namespace SME.CDEP.TesteIntegracao.Usuario
{
    public class Ao_cadastrar_usuario_externo : TesteBase
    {
        public Ao_cadastrar_usuario_externo(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        [Fact(DisplayName = "Usuário - A senha e a confirmação da senha devem ser iguais")]
        public async Task ValidarSenhasDiferentes()
        {
            await GetServicoUsuario().InserirUsuarioExterno(new UsuarioExternoDTO(){Cpf = faker.Person.Cpf(), Senha = ConstantesTestes.SENHA_TESTE, ConfirmarSenha = ConstantesTestes.SENHA_TESTE}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode ser menor que 8 caracteres")]
        public async Task ValidarSenhasMenores8Caracteres()
        {
            await GetServicoUsuario().InserirUsuarioExterno(new UsuarioExternoDTO(){Cpf = faker.Person.Cpf(), Senha = "Cdep@1", ConfirmarSenha = "Cdep@1"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode ser maior que 12 caracteres")]
        public async Task ValidarSenhasMaiores12Caracteres()
        {
            await GetServicoUsuario().InserirUsuarioExterno(new UsuarioExternoDTO(){Cpf = faker.Person.Cpf(), Senha = "Cdep@12345678910", ConfirmarSenha = "Cdep@12345678910"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode ter espaços em branco")]
        public async Task ValidarSenhasComEspacosEmBranco()
        {
            await GetServicoUsuario().InserirUsuarioExterno(new UsuarioExternoDTO(){Cpf = faker.Person.Cpf(), Senha = "Cdep @12", ConfirmarSenha = "Cdep @12"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha deve conter pelo menos 1 letra maiúscula, 1 minúscula, 1 número e/ou 1 caractere especial e não pode conter acentuação")]
        public async Task ValidarSenhasConformeCriterios()
        {
            await GetServicoUsuario().InserirUsuarioExterno(new UsuarioExternoDTO(){Cpf = faker.Person.Cpf(), Senha = ConstantesTestes.SENHA_TESTE, ConfirmarSenha = ConstantesTestes.SENHA_TESTE}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode conter acentos em letras minusculas")]
        public async Task ValidarSenhasSemAcentosMinusculas()
        {
            await GetServicoUsuario().InserirUsuarioExterno(new UsuarioExternoDTO(){Cpf = faker.Person.Cpf(), Senha = "Cdép@1234", ConfirmarSenha = "Cdép@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode conter acentos em letras minusculas - várias")]
        public async Task ValidarSenhasSemAcentosMinusculasVarias()
        {
            await GetServicoUsuario().InserirUsuarioExterno(new UsuarioExternoDTO(){Cpf = faker.Person.Cpf(), Senha = "Cdépáêíú@1234", ConfirmarSenha = "Cdépáêíú@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode conter acentos em letras maiúsculas")]
        public async Task ValidarSenhasSemAcentosMaiusculas()
        {
            await GetServicoUsuario().InserirUsuarioExterno(new UsuarioExternoDTO(){Cpf = faker.Person.Cpf(), Senha = "CDÉPe@1234", ConfirmarSenha = "CDÉPe@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode conter acentos em letras maiúsculas - várias")]
        public async Task ValidarSenhasSemAcentosMaiusculasVarias()
        {
            await GetServicoUsuario().InserirUsuarioExterno(new UsuarioExternoDTO(){Cpf = faker.Person.Cpf(), Senha = "CDÉPÁÊ@1234", ConfirmarSenha = "CDÉPÁÊ@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha deve conter letra maiúscula")]
        public async Task ValidarSenhasSemCaracterMaiusculo()
        {
            await GetServicoUsuario().InserirUsuarioExterno(new UsuarioExternoDTO(){Cpf = faker.Person.Cpf(), Senha = "cdep@1234", ConfirmarSenha = "cdep@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha deve conter letra minúscula")]
        public async Task ValidarSenhasSemCaracterMinusculo()
        {
            await GetServicoUsuario().InserirUsuarioExterno(new UsuarioExternoDTO(){Cpf = faker.Person.Cpf(), Senha = "CDEP@1234", ConfirmarSenha = "CEDEP@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha deve conter números")]
        public async Task ValidarSenhasSemNumeros()
        {
            await GetServicoUsuario().InserirUsuarioExterno(new UsuarioExternoDTO(){Cpf = faker.Person.Cpf(), Senha = "CDEP@&&&&", ConfirmarSenha = "CDEP@&&&&"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha deve conter números ou caracteres especiais")]
        public async Task ValidarSenhasSemNumerosECaracteresEspeciais()
        {
            await GetServicoUsuario().InserirUsuarioExterno(new UsuarioExternoDTO(){Cpf = faker.Person.Cpf(), Senha = "CDEPacdep", ConfirmarSenha = "CDEPacdep"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - O usuário já existe no Acervo")]
        public async Task ValidarUsuarioExistenteAcervo()
        {
            CriarClaimUsuario();
            await InserirNaBase(new Dominio.Entidades.Usuario()
            {
                Login = faker.Person.FirstName,
                Nome = faker.Person.FullName,
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoLogin = ConstantesTestes.SISTEMA, CriadoPor = ConstantesTestes.SISTEMA, CriadoEm = DateTimeExtension.HorarioBrasilia().Date
            });
            
            await GetServicoUsuario().InserirUsuarioExterno(new UsuarioExternoDTO()
            {
                Senha = "Cdep@1234", ConfirmarSenha = "Cdep@1234", Cpf = ConstantesTestes.LOGIN_99999999998
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - Cadastrando usuário externo")]
        public async Task ValidarCadastroUsuarioExterno()
        {
            var usuarioExterno = new UsuarioExternoDTO()
            {
                Cpf = ConstantesTestes.LOGIN_99999999999, 
                Nome = ConstantesTestes.USUARIO_INTERNO_99999999999, 
                Senha = "Cdep@1234", ConfirmarSenha = "Cdep@1234",
                Endereco = faker.Address.FullAddress(),
                Numero = faker.Address.BuildingNumber(),
                Complemento = faker.Address.StreetSuffix(),
                Cep = faker.Address.ZipCode(),
                Cidade = faker.Address.City(),
                Estado = faker.Address.StateAbbr(),
                Telefone = faker.Phone.PhoneNumber("(##) #####-####"),
                Bairro = faker.Address.County(),
                Tipo = TipoUsuario.PROFESSOR,
                Instituicao = faker.Company.CompanyName()
            };
            var usuario = await GetServicoUsuario().InserirUsuarioExterno(usuarioExterno);
            usuario.ShouldBeTrue();
            
            var usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            usuarios.Any(f => f.Login.Equals(ConstantesTestes.LOGIN_99999999999)).ShouldBeTrue();
            usuarios.Any(f => f.UltimoLogin.HasValue).ShouldBeFalse();
            usuarios.Any(f => f.TipoUsuario == TipoUsuario.PROFESSOR).ShouldBeTrue();
            usuarios.Any(f => f.Instituicao == usuarioExterno.Instituicao).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Usuário - O usuário já existe no CoreSSO")]
        public async Task ValidarUsuarioExistenteCoreSSO()
        {
            await GetServicoUsuario().InserirUsuarioExterno(new UsuarioExternoDTO()
            {
                Senha = "Cdep@1234", ConfirmarSenha = "Cdep@1234", Cpf = ConstantesTestes.LOGIN_99999999998,
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - Ao autenticar um usuário existente, deve atualizar a data de login")]
        public async Task AutenticarUsuarioExistente()
        {
            CriarClaimUsuario();
            await InserirNaBase(new Dominio.Entidades.Usuario()
            {
                Login = ConstantesTestes.LOGIN_99999999999,
                Nome = ConstantesTestes.USUARIO_INTERNO_99999999999,
                UltimoLogin = DateTimeExtension.HorarioBrasilia(),
                CriadoLogin = ConstantesTestes.SISTEMA, CriadoPor = ConstantesTestes.SISTEMA, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            var usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            
            var usuario = await GetServicoUsuario().Autenticar(ConstantesTestes.LOGIN_99999999999,string.Empty);
            usuario.ShouldNotBeNull();
            
            usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            usuarios.Any(f => f.Login.Equals(ConstantesTestes.LOGIN_99999999999)).ShouldBeTrue();
            usuarios.Any(f => f.UltimoLogin.Value.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Usuário - Alterar endereço  de usuário externo")]
        public async Task AlterarEndereco()
        {
            CriarClaimUsuario();
            var login = faker.Person.FirstName;
            
            var usuarioInserir = new Dominio.Entidades.Usuario()
            {
                Login = login,
                Nome = faker.Person.FullName,
                Endereco = faker.Address.FullAddress(),
                Numero = faker.Address.BuildingNumber(),
                Complemento = faker.Address.StreetSuffix(),
                Cep = faker.Address.ZipCode(),
                Cidade = faker.Address.City(),
                Estado = faker.Address.StateAbbr(),
                Telefone = faker.Phone.PhoneNumber("(##) #####-####"),
                Bairro = faker.Address.County(),
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                TipoUsuario = TipoUsuario.PROFESSOR,
                CriadoLogin = ConstantesTestes.SISTEMA, CriadoPor = ConstantesTestes.SISTEMA,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                Instituicao = faker.Company.CompanyName()
            };
            
            await InserirNaBase(usuarioInserir);

            var usuarioAlterado = new EnderecoUsuarioExternoDTO()
            {
                Endereco = faker.Address.FullAddress(),
                Numero = faker.Address.BuildingNumber(),
                Complemento = faker.Address.StreetSuffix(),
                Cep = faker.Address.ZipCode(),
                Cidade = faker.Address.City(),
                Estado = faker.Address.StateAbbr(),
                Bairro = faker.Address.County(),
            };
            
            var usuario = await GetServicoUsuario().AlterarEndereco(login,usuarioAlterado);
            usuario.ShouldBeTrue();
            
            var usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            usuarios.Any(f => f.Login.Equals(login)).ShouldBeTrue();
            usuarios.Any(f => f.Endereco.Equals(usuarioAlterado.Endereco)).ShouldBeTrue();
            usuarios.Any(f => f.Numero == usuarioAlterado.Numero).ShouldBeTrue();
            usuarios.Any(f => f.Complemento.Equals(usuarioAlterado.Complemento)).ShouldBeTrue();
            usuarios.Any(f => f.Bairro.Equals(usuarioAlterado.Bairro)).ShouldBeTrue();
            usuarios.Any(f => f.Cidade.Equals(usuarioAlterado.Cidade)).ShouldBeTrue();
            usuarios.Any(f => f.Cep.Equals(usuarioAlterado.Cep)).ShouldBeTrue();
            usuarios.Any(f => f.TipoUsuario == TipoUsuario.PROFESSOR).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Usuário - Alterar telefone de usuário externo")]
        public async Task AlterarTelefone()
        {
            CriarClaimUsuario();
            var login = faker.Person.FirstName;
            
            await InserirNaBase(new Dominio.Entidades.Usuario()
            {
                Login = login,
                Nome = faker.Person.FullName,
                Telefone = faker.Phone.PhoneNumber("(##) #####-####"),
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                TipoUsuario = TipoUsuario.PROFESSOR,
                CriadoLogin = ConstantesTestes.SISTEMA, CriadoPor = ConstantesTestes.SISTEMA, CriadoEm = DateTimeExtension.HorarioBrasilia().Date
            });

            var telefoneAlterado = faker.Phone.PhoneNumber("(##) #####-####");
            var usuario = await GetServicoUsuario().AlterarTelefone(login,telefoneAlterado);
            usuario.ShouldBeTrue();
            
            var usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            usuarios.Any(f => f.Login.Equals(login)).ShouldBeTrue();
            usuarios.Any(f => f.Telefone.Equals(telefoneAlterado)).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Usuário - Não deve permitir alterar endereço de usuário que não seja externo")]
        public async Task NaoDeveAlterarEndereco()
        {
            CriarClaimUsuario();
            var login = faker.Person.FirstName;
            var inserido = new Dominio.Entidades.Usuario()
            {
                Login = login,
                Nome = faker.Person.FullName,
                Endereco = faker.Address.FullAddress(),
                Numero = faker.Address.BuildingNumber(),
                Complemento = faker.Address.StreetSuffix(),
                Cep = faker.Address.ZipCode(),
                Cidade = faker.Address.City(),
                Estado = faker.Address.StateAbbr(),
                Telefone = faker.Phone.PhoneNumber("(##) #####-####"),
                Bairro = faker.Address.County(),
                TipoUsuario = TipoUsuario.CORESSO,
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoLogin = ConstantesTestes.SISTEMA, CriadoPor = ConstantesTestes.SISTEMA,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date
            };
            
            await InserirNaBase(inserido);

            var usuarioAlterado = new EnderecoUsuarioExternoDTO()
            {
                Endereco = faker.Address.FullAddress(),
                Numero = faker.Address.BuildingNumber(),
                Complemento = faker.Address.StreetSuffix(),
                Cep = faker.Address.ZipCode(),
                Cidade = faker.Address.City(),
                Estado = faker.Address.StateAbbr(),
                Bairro = faker.Address.County(),
            };
            
            await GetServicoUsuario().AlterarEndereco(login,usuarioAlterado).ShouldThrowAsync<NegocioException>();
            
            var usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            usuarios.Any(f => f.Login.Equals(login)).ShouldBeTrue();
            usuarios.Any(f => f.Endereco.Equals(inserido.Endereco)).ShouldBeTrue();
            usuarios.Any(f => f.Numero == inserido.Numero).ShouldBeTrue();
            usuarios.Any(f => f.Complemento.Equals(inserido.Complemento)).ShouldBeTrue();
            usuarios.Any(f => f.Bairro.Equals(inserido.Bairro)).ShouldBeTrue();
            usuarios.Any(f => f.Cidade.Equals(inserido.Cidade)).ShouldBeTrue();
            usuarios.Any(f => f.Cep.Equals(inserido.Cep)).ShouldBeTrue();
            usuarios.Any(f => f.TipoUsuario == TipoUsuario.CORESSO).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Usuário - Não deve permitir alterar telefone de usuário que não seja externo")]
        public async Task NaoDeveAlterarTelefone()
        {
            CriarClaimUsuario();
            var login = faker.Person.FirstName;
            var telefoneInserido = faker.Phone.PhoneNumber("(##) #####-####");
            
            await InserirNaBase(new Dominio.Entidades.Usuario()
            {
                Login = login,
                Nome = faker.Person.FullName,
                Telefone = telefoneInserido,
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoLogin = ConstantesTestes.SISTEMA, CriadoPor = ConstantesTestes.SISTEMA, CriadoEm = DateTimeExtension.HorarioBrasilia().Date
            });

            var usuarios = ObterTodos<Dominio.Entidades.Usuario>();

            var telefone = faker.Phone.PhoneNumber("(##) #####-####");
            await GetServicoUsuario().AlterarTelefone(login,telefone).ShouldThrowAsync<NegocioException>();
            
            usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            usuarios.Any(f => f.Login.Equals(login)).ShouldBeTrue();
            usuarios.Any(f => f.Telefone.Equals(telefoneInserido)).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Usuário - Alterar tipo usuário para usuário externo")]
        public async Task AlterarTipoUsuario()
        {
            CriarClaimUsuario();
            await InserirNaBase(new Dominio.Entidades.Usuario()
            {
                Login = ConstantesTestes.LOGIN_99999999999,
                Nome = ConstantesTestes.USUARIO_INTERNO_99999999999,
                Telefone = faker.Phone.PhoneNumber("(##) #####-####"),
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                TipoUsuario = TipoUsuario.PROFESSOR,
                CriadoLogin = ConstantesTestes.SISTEMA, CriadoPor = ConstantesTestes.SISTEMA, CriadoEm = DateTimeExtension.HorarioBrasilia().Date
            });

            var usuario = await GetServicoUsuario().AlterarTipoUsuario(ConstantesTestes.LOGIN_99999999999,new TipoUsuarioExternoDTO() { Tipo = (int)TipoUsuario.POPULACAO_GERAL});
            usuario.ShouldBeTrue();
            
            var usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            usuarios.Any(f => f.Login.Equals(ConstantesTestes.LOGIN_99999999999)).ShouldBeTrue();
            usuarios.Any(f => f.TipoUsuario == TipoUsuario.POPULACAO_GERAL).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Usuário - Não deve permitir alterar tipo de usuário para quem não seja externo")]
        public async Task NaoDeveAlterarTipoUsuario()
        {
            CriarClaimUsuario();
            var telefone = faker.Phone.PhoneNumber("(##) #####-####");
            
            await InserirNaBase(new Dominio.Entidades.Usuario()
            {
                Login = ConstantesTestes.LOGIN_99999999999,
                Nome = ConstantesTestes.USUARIO_INTERNO_99999999999,
                Telefone = telefone,
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoLogin = ConstantesTestes.SISTEMA, CriadoPor = ConstantesTestes.SISTEMA, CriadoEm = DateTimeExtension.HorarioBrasilia().Date
            });

            var usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            
            await GetServicoUsuario().AlterarTipoUsuario(ConstantesTestes.LOGIN_99999999999,new TipoUsuarioExternoDTO() { Tipo = (int)TipoUsuario.POPULACAO_GERAL}).ShouldThrowAsync<NegocioException>();
            
            usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            usuarios.Any(f => f.Login.Equals(ConstantesTestes.LOGIN_99999999999)).ShouldBeTrue();
            usuarios.Any(f => f.Telefone.Equals(telefone)).ShouldBeTrue();
            usuarios.Any(f => f.TipoUsuario == TipoUsuario.CORESSO).ShouldBeTrue();
        }
    }
}