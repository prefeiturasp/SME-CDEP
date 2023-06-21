using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.TesteIntegracao.ServicosFakes;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using Xunit;

namespace SME.CDEP.TesteIntegracao.Usuario
{
    public class Ao_fazer_manutencao_usuario : TesteBase
    {
        public Ao_fazer_manutencao_usuario(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Usuário - Cadastrar usuario")]
        public async Task Cadastrar_usuario()
        {
            var servicoUsuario = GetServicoUsuario();
            var usuarioDto = ObterUsuarioDto();

            var usuarioId = await servicoUsuario.Inserir(usuarioDto);
            usuarioId.ShouldBeGreaterThan(0);
        }

        [Fact(DisplayName = "Usuário - Obter todos os usuarios")]
        public async Task ObterTodosUsuarios()
        {
            IServicoUsuario servicoUsuario = await CadastrarVariosUsuarios();

            var usuarios = await servicoUsuario.ObterTodos();
            usuarios.ShouldNotBeNull();
            usuarios.Count.ShouldBe(10);
        }

        [Fact(DisplayName = "Usuário - Obter por id")]
        public async Task ObterUsuarioPorId()
        {
            IServicoUsuario servicoUsuario = await CadastrarVariosUsuarios();

            var usuario = await servicoUsuario.ObterPorId(1);
            usuario.ShouldNotBeNull();
            usuario.Id.ShouldBe(1);
        }

        [Fact(DisplayName = "Usuário - Atualizar")]
        public async Task Atualizar()
        {
            CriarClaimUsuario();
            IServicoUsuario servicoUsuario = await CadastrarVariosUsuarios();

            var usuarioAlterado = await servicoUsuario.ObterPorId(1);
            usuarioAlterado.Login = "login alterado";
            usuarioAlterado.Nome = "Nome alterado";
            var usuario = await servicoUsuario.Alterar(usuarioAlterado);
            usuario.ShouldNotBeNull();
            usuario.Login.ShouldBe("login alterado");
            usuario.AlteradoLogin.ShouldBe(LOGIN_123456789);
            usuario.AlteradoPor.ShouldBe(SISTEMA);
            usuario.AlteradoEm.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
        }
        
        [Fact(DisplayName = "Usuário - Obter por login")]
        public async Task ObterPorLogin()
        {
            IServicoUsuario servicoUsuario = await CadastrarVariosUsuarios();

            var usuario = await servicoUsuario.ObterPorLogin("login_8");
            usuario.ShouldNotBeNull();
            usuario.Login.ShouldBe("login_8");
        }
        
        [Fact(DisplayName = "Usuário - Obter por login - falhar")]
        public async Task ObterPorLoginComFalha()
        {
            IServicoUsuario servicoUsuario = await CadastrarVariosUsuarios();

            var usuario = await servicoUsuario.ObterPorLogin("login_10");
            usuario.ShouldNotBeNull();
            usuario.Login.ShouldBe("login_10");
        }

        private async Task<IServicoUsuario> CadastrarVariosUsuarios()
        {
            var servicoUsuario = GetServicoUsuario();
            var usuarioDto = ObterUsuarioDto();

            for (int i = 1; i <= 10; i++)
            {
                usuarioDto.Login = $"login_{i}";
                usuarioDto.Nome = $"Nome '{i}' do teste de integração";

                var usuarioId = await servicoUsuario.Inserir(usuarioDto);
                usuarioId.ShouldBeGreaterThan(0);
            }

            return servicoUsuario;
        }

        private UsuarioIdNomeLoginDTO ObterUsuarioDto()
        {
            return new UsuarioIdNomeLoginDTO
            {
                Login = "login do teste de integração",
                Nome = "Nome do teste de integração"
            };
        }
    }
}