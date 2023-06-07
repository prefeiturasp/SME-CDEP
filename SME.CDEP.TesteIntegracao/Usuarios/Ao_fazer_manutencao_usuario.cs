using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.CDEP.Aplicacao.Dtos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.TesteIntegracao.ServicosFakes;
using SME.CDEP.TesteIntegracao.Setup;
using SSME.CDEP.Aplicacao.Integracoes.Interfaces;
using Xunit;

namespace SME.CDEP.TesteIntegracao.Usuario
{
    public class Ao_fazer_manutencao_usuario : TesteBase
    {
        public Ao_fazer_manutencao_usuario(CollectionFixture collectionFixture) : base(collectionFixture)
        { }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IServicoAcessos), typeof(ServicoAcessosFake), ServiceLifetime.Scoped));
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
            var usuarios = ObterTodos<Dominio.Dominios.Usuario>();
            var usuario = await servicoUsuario.Alterar(new UsuarioDto() { Id = 1, Login = "login alterado", Nome = "Nome alterado" });
            usuario.ShouldNotBeNull();
            usuario.Login.ShouldBe("login alterado");
            usuarios = ObterTodos<Dominio.Dominios.Usuario>();
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
            usuario.ShouldBeNull();
        }
        
        [Fact(DisplayName = "Usuário - Ao autenticar um usuário novo, deve cadastrá-lo")]
        public async Task AutenticarUsuarioNovo()
        {
            // IServicoUsuario servicoUsuario = 
            //
            // var usuario = await servicoUsuario.ObterPorLogin("login_10");
            // usuario.ShouldBeNull();
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