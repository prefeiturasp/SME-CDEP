﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.TesteIntegracao.ServicosFakes;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
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
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = "999.999.999-99", Senha = "senha_teste", ConfirmarSenha = "teste_senha"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode ser menor que 8 caracteres")]
        public async Task ValidarSenhasMenores8Caracteres()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = "999.999.999-99", Senha = "Cdep@1", ConfirmarSenha = "Cdep@1"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode ser maior que 12 caracteres")]
        public async Task ValidarSenhasMaiores12Caracteres()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = "999.999.999-99", Senha = "Cdep@12345678910", ConfirmarSenha = "Cdep@12345678910"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode ter espaços em branco")]
        public async Task ValidarSenhasComEspacosEmBranco()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = "999.999.999-99", Senha = "Cdep @12", ConfirmarSenha = "Cdep @12"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha deve conter pelo menos 1 letra maiúscula, 1 minúscula, 1 número e/ou 1 caractere especial e não pode conter acentuação")]
        public async Task ValidarSenhasConformeCriterios()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = "999.999.999-99", Senha = "senha_teste", ConfirmarSenha = "senha_teste"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode conter acentos em letras minusculas")]
        public async Task ValidarSenhasSemAcentosMinusculas()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = "999.999.999-99", Senha = "Cdép@1234", ConfirmarSenha = "Cdép@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode conter acentos em letras minusculas - várias")]
        public async Task ValidarSenhasSemAcentosMinusculasVarias()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = "999.999.999-99", Senha = "Cdépáêíú@1234", ConfirmarSenha = "Cdépáêíú@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode conter acentos em letras maiúsculas")]
        public async Task ValidarSenhasSemAcentosMaiusculas()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = "999.999.999-99", Senha = "CDÉPe@1234", ConfirmarSenha = "CDÉPe@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha não pode conter acentos em letras maiúsculas - várias")]
        public async Task ValidarSenhasSemAcentosMaiusculasVarias()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = "999.999.999-99", Senha = "CDÉPÁÊ@1234", ConfirmarSenha = "CDÉPÁÊ@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha deve conter letra maiúscula")]
        public async Task ValidarSenhasSemCaracterMaiusculo()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = "999.999.999-99", Senha = "cdep@1234", ConfirmarSenha = "cdep@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha deve conter letra minúscula")]
        public async Task ValidarSenhasSemCaracterMinusculo()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = "999.999.999-99", Senha = "CDEP@1234", ConfirmarSenha = "CEDEP@1234"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha deve conter números")]
        public async Task ValidarSenhasSemNumeros()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = "999.999.999-99", Senha = "CDEP@&&&&", ConfirmarSenha = "CDEP@&&&&"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - A senha deve conter números ou caracteres especiais")]
        public async Task ValidarSenhasSemNumerosECaracteresEspeciais()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO(){Cpf = "999.999.999-99", Senha = "CDEPacdep", ConfirmarSenha = "CDEPacdep"}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - O usuário já existe no Acervo")]
        public async Task ValidarUsuarioExistenteAcervo()
        {
            CriarClaimUsuario();
            await InserirNaBase(new Dominio.Dominios.Usuario()
            {
                Login = "99999999999",
                Nome = "Usuário do Login_1",
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoLogin = SISTEMA, CriadoPor = SISTEMA, CriadoEm = DateTimeExtension.HorarioBrasilia().Date
            });
            
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO()
            {
                Senha = "Cdep@1234", ConfirmarSenha = "Cdep@1234", Cpf = "999.999.999-99"
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - Cadastrando usuário externo")]
        public async Task ValidarCadastroUsuarioExterno()
        {
            var usuarioExterno = new UsuarioExternoDTO()
            {
                Cpf = "99999999999", Nome = "Nome login_1", Senha = "Cdep@1234", ConfirmarSenha = "Cdep@1234",
                Cep = "88058-000", Cidade = "Florianópolis", Estado = "SC", Complemento = "Casa 01", Numero = 10,
                Email = "login_1@email.com.br", Endereco = "Rua do login_1", Telefone = "99 99999 9999",
                TipoUsuario = TipoUsuario.PROFESSOR
            };
            var usuario = await GetServicoUsuario().CadastrarUsuarioExterno(usuarioExterno);
            usuario.ShouldBeTrue();
            
            var usuarios = ObterTodos<Dominio.Dominios.Usuario>();
            usuarios.FirstOrDefault(f => f.Login.Equals("99999999999"));
            usuarios.FirstOrDefault(f => f.UltimoLogin.Date == DateTimeExtension.HorarioBrasilia().Date);
            usuarios.FirstOrDefault(f => f.TipoUsuario == TipoUsuario.PROFESSOR);
        }
        
        [Fact(DisplayName = "Usuário - O usuário já existe no CoreSSO")]
        public async Task ValidarUsuarioExistenteCoreSSO()
        {
            await GetServicoUsuario().CadastrarUsuarioExterno(new UsuarioExternoDTO()
            {
                Senha = "Cdep@1234", ConfirmarSenha = "Cdep@1234", Cpf = "999.999.999-98",
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Usuário - Ao autenticar um usuário existente, deve atualizar a data de login")]
        public async Task AutenticarUsuarioExistente()
        {
            CriarClaimUsuario();
            await InserirNaBase(new Dominio.Dominios.Usuario()
            {
                Login = "99999999999",
                Nome = "Usuário do Login_1",
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoLogin = SISTEMA, CriadoPor = SISTEMA, CriadoEm = DateTimeExtension.HorarioBrasilia().Date
            });
            
            var usuario = await GetServicoUsuario().Autenticar("99999999999","teste");
            usuario.ShouldNotBeNull();
            
            var usuarios = ObterTodos<Dominio.Dominios.Usuario>();
            usuarios.FirstOrDefault(f => f.Login.Equals("99999999999"));
            usuarios.FirstOrDefault(f => f.UltimoLogin.Date == DateTimeExtension.HorarioBrasilia().Date);
        }

        private IServicoUsuario GetServicoUsuario()
        {
            return ServiceProvider.GetService<IServicoUsuario>();
        }
    }
}