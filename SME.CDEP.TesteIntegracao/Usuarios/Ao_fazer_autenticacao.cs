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
    public class Ao_fazer_autenticacao : TesteBase
    {
        public Ao_fazer_autenticacao(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        [Fact(DisplayName = "Usuário - Ao autenticar um usuário novo, deve cadastrá-lo")]
        public async Task AutenticarUsuarioNovo()
        {
            var usuario = await GetServicoUsuario().Autenticar("login_10","teste");
            usuario.ShouldNotBeNull();
            
            var usuarios = ObterTodos<Dominio.Dominios.Usuario>();
            usuarios.FirstOrDefault(f => f.Login.Equals("login_10"));
        }
        
        [Fact(DisplayName = "Usuário - Ao autenticar um usuário existente, deve atualizar a data de login")]
        public async Task AutenticarUsuarioExistente()
        {
            await InserirNaBase(new Dominio.Dominios.Usuario()
            {
                Login = "login_1",
                Nome = "Usuário do Login_1",
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5)
            });
            
            var usuario = await GetServicoUsuario().Autenticar("login_1","teste");
            usuario.ShouldNotBeNull();
            
            var usuarios = ObterTodos<Dominio.Dominios.Usuario>();
            usuarios.FirstOrDefault(f => f.Login.Equals("login_1"));
            usuarios.FirstOrDefault(f => f.UltimoLogin.Date == DateTimeExtension.HorarioBrasilia().Date);
        }

        private IServicoUsuario GetServicoUsuario()
        {
            return ServiceProvider.GetService<IServicoUsuario>();
        }
    }
}