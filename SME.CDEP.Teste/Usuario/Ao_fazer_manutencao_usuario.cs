using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dados.Dtos;
using SME.CDEP.Teste;
using SME.CDEP.Teste.Setup;
using Xunit;

namespace SME.CDE.Teste.Usuario
{
    public class Ao_fazer_manutencao_usuario : TesteBase
    {
        public Ao_fazer_manutencao_usuario(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

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
            IServicoUsuario servicoUsuario = await CadastrarVariosUsuarios();

            var usuario = await servicoUsuario.Alterar(new UsuarioDto() { Id = 1, Login = "login alterado", Nome = "Nome alterado" });
            usuario.ShouldNotBeNull();
            usuario.Login.ShouldBe("login alterado");
        }

        private async Task<IServicoUsuario> CadastrarVariosUsuarios()
        {
            var servicoUsuario = GetServicoUsuario();
            var usuarioDto = ObterUsuarioDto();

            for (int i = 0; i < 10; i++)
            {
                usuarioDto.Login = $"login_{i}";
                usuarioDto.Nome = $"Nome '{i}' do teste de integração";

                var usuarioId = await servicoUsuario.Inserir(usuarioDto);
                usuarioId.ShouldBeGreaterThan(0);
            }

            return servicoUsuario;
        }

        private UsuarioDto ObterUsuarioDto()
        {
            return new UsuarioDto
            {
                Login = "login do teste de integração",
                Nome = "Nome do teste de integração"
            };
        }

        private IServicoUsuario GetServicoUsuario()
        {
            return ServiceProvider.GetService<IServicoUsuario>();
        }
    }
}