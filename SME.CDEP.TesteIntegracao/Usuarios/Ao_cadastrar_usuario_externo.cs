using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Excecoes;
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
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = ConstantesTestes.CPF_999_999_999_99, Senha = ConstantesTestes.SENHA_TESTE, ConfirmarSenha = ConstantesTestes.SENHA_TESTE}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode ser menor que 8 caracteres")]
        public async Task ValidarSenhasMenores8Caracteres()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = ConstantesTestes.CPF_999_999_999_99, Senha = "Cdep@1", ConfirmarSenha = "Cdep@1"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode ser maior que 12 caracteres")]
        public async Task ValidarSenhasMaiores12Caracteres()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = ConstantesTestes.CPF_999_999_999_99, Senha = "Cdep@12345678910", ConfirmarSenha = "Cdep@12345678910"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode ter espaços em branco")]
        public async Task ValidarSenhasComEspacosEmBranco()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = ConstantesTestes.CPF_999_999_999_99, Senha = "Cdep @12", ConfirmarSenha = "Cdep @12"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha deve conter pelo menos 1 letra maiúscula, 1 minúscula, 1 número e/ou 1 caractere especial e não pode conter acentuação")]
        public async Task ValidarSenhasConformeCriterios()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = ConstantesTestes.CPF_999_999_999_99, Senha = ConstantesTestes.SENHA_TESTE, ConfirmarSenha = ConstantesTestes.SENHA_TESTE}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode conter acentos em letras minusculas")]
        public async Task ValidarSenhasSemAcentosMinusculas()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = ConstantesTestes.CPF_999_999_999_99, Senha = "Cdép@1234", ConfirmarSenha = "Cdép@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode conter acentos em letras minusculas - várias")]
        public async Task ValidarSenhasSemAcentosMinusculasVarias()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = ConstantesTestes.CPF_999_999_999_99, Senha = "Cdépáêíú@1234", ConfirmarSenha = "Cdépáêíú@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode conter acentos em letras maiúsculas")]
        public async Task ValidarSenhasSemAcentosMaiusculas()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = ConstantesTestes.CPF_999_999_999_99, Senha = "CDÉPe@1234", ConfirmarSenha = "CDÉPe@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode conter acentos em letras maiúsculas - várias")]
        public async Task ValidarSenhasSemAcentosMaiusculasVarias()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = ConstantesTestes.CPF_999_999_999_99, Senha = "CDÉPÁÊ@1234", ConfirmarSenha = "CDÉPÁÊ@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha deve conter letra maiúscula")]
        public async Task ValidarSenhasSemCaracterMaiusculo()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = ConstantesTestes.CPF_999_999_999_99, Senha = "cdep@1234", ConfirmarSenha = "cdep@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha deve conter letra minúscula")]
        public async Task ValidarSenhasSemCaracterMinusculo()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = ConstantesTestes.CPF_999_999_999_99, Senha = "CDEP@1234", ConfirmarSenha = "CEDEP@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha deve conter números")]
        public async Task ValidarSenhasSemNumeros()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = ConstantesTestes.CPF_999_999_999_99, Senha = "CDEP@&&&&", ConfirmarSenha = "CDEP@&&&&"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha deve conter números ou caracteres especiais")]
        public async Task ValidarSenhasSemNumerosECaracteresEspeciais()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = ConstantesTestes.CPF_999_999_999_99, Senha = "CDEPacdep", ConfirmarSenha = "CDEPacdep"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - O usuário já existe no Acervo")]
        public async Task ValidarUsuarioExistenteAcervo()
        {
            CriarClaimUsuario();
            await InserirNaBase(new Dominio.Entidades.Usuario()
            {
                Login = ConstantesTestes.LOGIN_99999999999,
                Nome = ConstantesTestes.USUARIO_INTERNO_99999999999,
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoLogin = ConstantesTestes.SISTEMA, CriadoPor = ConstantesTestes.SISTEMA, CriadoEm = DateTimeExtension.HorarioBrasilia().Date
            });
            
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO()
            {
                Senha = "Cdep@1234", ConfirmarSenha = "Cdep@1234", Cpf = ConstantesTestes.CPF_999_999_999_99
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
                Cep = ConstantesTestes.CEP_88058999, Cidade = ConstantesTestes.CIDADE_99999999999, Estado = ConstantesTestes.ESTADO_SC, 
                Complemento = ConstantesTestes.COMPLEMENTO_CASA_99, Numero = int.Parse(ConstantesTestes.NUMERO_99),
                Email = ConstantesTestes.EMAIL_INTERNO, 
                Endereco = ConstantesTestes.RUA_99999999999, Telefone = ConstantesTestes.TELEFONE_99_99999_9999,
                TipoUsuario = TipoUsuario.PROFESSOR
            };
            var usuario = await GetServicoUsuario().CadastrarUsuarioExterno(usuarioExterno);
            usuario.ShouldBeTrue();
            
            var usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            usuarios.FirstOrDefault(f => f.Login.Equals(ConstantesTestes.LOGIN_99999999999));
            usuarios.FirstOrDefault(f => f.UltimoLogin.Date == DateTimeExtension.HorarioBrasilia().Date);
            usuarios.FirstOrDefault(f => f.TipoUsuario == TipoUsuario.PROFESSOR);
        }
        
        [Fact(DisplayName = "Usuário - O usuário já existe no CoreSSO")]
        public async Task ValidarUsuarioExistenteCoreSSO()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO()
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
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoLogin = ConstantesTestes.SISTEMA, CriadoPor = ConstantesTestes.SISTEMA, CriadoEm = DateTimeExtension.HorarioBrasilia().Date
            });
            
            var usuario = await GetServicoUsuario().Autenticar(ConstantesTestes.LOGIN_99999999999,string.Empty);
            usuario.ShouldNotBeNull();
            
            var usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            usuarios.FirstOrDefault(f => f.Login.Equals(ConstantesTestes.LOGIN_99999999999));
            usuarios.FirstOrDefault(f => f.UltimoLogin.Date == DateTimeExtension.HorarioBrasilia().Date);
        }
        
        [Fact(DisplayName = "Usuário - Alterar endereço e telefone de usuário externo")]
        public async Task AlterarEnderecoTelefone()
        {
            CriarClaimUsuario();
            await InserirNaBase(new Dominio.Entidades.Usuario()
            {
                Login = ConstantesTestes.LOGIN_99999999999,
                Nome = ConstantesTestes.USUARIO_INTERNO_99999999999,
                Endereco = ConstantesTestes.RUA_99999999999,
                Numero = int.Parse(ConstantesTestes.NUMERO_99),
                Complemento = ConstantesTestes.COMPLEMENTO_CASA_99,
                Bairro = ConstantesTestes.BAIRRO_99999999999,
                Cidade = ConstantesTestes.CIDADE_99999999999,
                Estado = ConstantesTestes.ESTADO_SC,
                Cep = ConstantesTestes.CEP_88058999,
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                TipoUsuario = TipoUsuario.PROFESSOR,
                CriadoLogin = ConstantesTestes.SISTEMA, CriadoPor = ConstantesTestes.SISTEMA, CriadoEm = DateTimeExtension.HorarioBrasilia().Date
            });

            var usuatioAlterado = new EnderecoTelefoneUsuarioExternoDTO()
            {
                Login = ConstantesTestes.LOGIN_99999999999,
                Endereco = ConstantesTestes.RUA_99999999998,
                Numero = int.Parse(ConstantesTestes.NUMERO_98),
                Complemento = ConstantesTestes.COMPLEMENTO_CASA_98,
                Bairro = ConstantesTestes.BAIRRO_99999999998,
                Cidade = ConstantesTestes.CIDADE_99999999998,
                Cep = ConstantesTestes.CEP_88058998,
            };
            
            var usuario = await GetServicoUsuario().AlterarEnderecoTelefone(usuatioAlterado);
            usuario.ShouldBeTrue();
            
            var usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            usuarios.FirstOrDefault(f => f.Login.Equals(ConstantesTestes.LOGIN_99999999999));
            usuarios.FirstOrDefault(f => f.Endereco.Equals(ConstantesTestes.RUA_99999999998));
            usuarios.FirstOrDefault(f => f.Numero.Equals(ConstantesTestes.NUMERO_98));
            usuarios.FirstOrDefault(f => f.Complemento.Equals(ConstantesTestes.COMPLEMENTO_CASA_98));
            usuarios.FirstOrDefault(f => f.Bairro.Equals(ConstantesTestes.BAIRRO_99999999998));
            usuarios.FirstOrDefault(f => f.Cidade.Equals(ConstantesTestes.CIDADE_99999999998));
            usuarios.FirstOrDefault(f => f.Cep.Equals(ConstantesTestes.CEP_88058998));
            usuarios.FirstOrDefault(f => f.TipoUsuario == TipoUsuario.PROFESSOR);
        }
        
        [Fact(DisplayName = "Usuário - Não deve permitir alterar endereço e telefone de usuário que não seja externo")]
        public async Task NaoDeveAlterarEnderecoTelefone()
        {
            CriarClaimUsuario();
            await InserirNaBase(new Dominio.Entidades.Usuario()
            {
                Login = ConstantesTestes.LOGIN_99999999999,
                Nome = ConstantesTestes.USUARIO_INTERNO_99999999999,
                Endereco = ConstantesTestes.RUA_99999999999,
                Numero = int.Parse(ConstantesTestes.NUMERO_99),
                Complemento = ConstantesTestes.COMPLEMENTO_CASA_99,
                Bairro = ConstantesTestes.BAIRRO_99999999999,
                Cidade = ConstantesTestes.CIDADE_99999999999,
                Estado = ConstantesTestes.ESTADO_SC,
                Cep = ConstantesTestes.CEP_88058999,
                TipoUsuario = TipoUsuario.CORESSO,
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoLogin = ConstantesTestes.SISTEMA, CriadoPor = ConstantesTestes.SISTEMA, CriadoEm = DateTimeExtension.HorarioBrasilia().Date
            });

            var usuatioAlterado = new EnderecoTelefoneUsuarioExternoDTO()
            {
                Login = ConstantesTestes.LOGIN_99999999999,
                Endereco = ConstantesTestes.RUA_99999999998,
                Numero = int.Parse(ConstantesTestes.NUMERO_98),
                Complemento = ConstantesTestes.COMPLEMENTO_CASA_98,
                Bairro = ConstantesTestes.BAIRRO_99999999998,
                Cidade = ConstantesTestes.CIDADE_99999999998,
                Cep = ConstantesTestes.CEP_88058998,
            };
            
            await GetServicoUsuario().AlterarEnderecoTelefone(usuatioAlterado).ShouldThrowAsync<NegocioException>();
            
            var usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            usuarios.FirstOrDefault(f => f.Login.Equals(ConstantesTestes.LOGIN_99999999999));
            usuarios.FirstOrDefault(f => f.Endereco.Equals(ConstantesTestes.RUA_99999999999));
            usuarios.FirstOrDefault(f => f.Numero.Equals(ConstantesTestes.NUMERO_99));
            usuarios.FirstOrDefault(f => f.Complemento.Equals(ConstantesTestes.COMPLEMENTO_CASA_99));
            usuarios.FirstOrDefault(f => f.Bairro.Equals(ConstantesTestes.BAIRRO_99999999999));
            usuarios.FirstOrDefault(f => f.Cidade.Equals(ConstantesTestes.CIDADE_99999999999));
            usuarios.FirstOrDefault(f => f.Cep.Equals(ConstantesTestes.CEP_88058999));
            usuarios.FirstOrDefault(f => f.TipoUsuario == TipoUsuario.CORESSO);
        }
    }
}