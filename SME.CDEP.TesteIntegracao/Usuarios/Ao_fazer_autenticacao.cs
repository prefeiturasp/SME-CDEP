using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.TesteIntegracao.ServicosFakes;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Constantes;
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
            CriarClaimUsuario();
            
            var usuario = await GetServicoUsuario().Autenticar(ConstantesTestes.LOGIN_99999999999,string.Empty);
            usuario.ShouldNotBeNull();
            
            var usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            usuarios.Any(f => f.Login.Equals(ConstantesTestes.LOGIN_99999999999)).ShouldBeTrue();
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
            usuarios.Any(f => f.Login.Equals(ConstantesTestes.LOGIN_99999999999)).ShouldBeTrue();
            usuarios.Any(f => f.UltimoLogin.Value.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();
        }
    }
}