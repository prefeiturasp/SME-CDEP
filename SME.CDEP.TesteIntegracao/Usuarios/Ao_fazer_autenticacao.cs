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
        public async Task Ao_autenticar_usuario_novo_deve_cadastra_lo()
        {
            CriarClaimUsuario();
            
            var usuario = await GetServicoUsuario().Autenticar(ConstantesTestes.LOGIN_99999999999,string.Empty);
            usuario.ShouldNotBeNull();
            
            var usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            usuarios.Any(f => f.Login.Equals(ConstantesTestes.LOGIN_99999999999)).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Usuário - Ao autenticar um usuário existente, deve atualizar a data de login")]
        public async Task Ao_autenticar_usuario_existente_deve_atualizar_data_de_login()
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
        
        [Fact(DisplayName = "Usuário - Revalidar token")]
        public async Task Ao_revalidar_token()
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
            
            var retorno = await GetServicoUsuario().RevalidarToken(usuario.Token);
            retorno.ShouldNotBeNull();
            retorno.Autenticado.ShouldBeTrue();
            retorno.DataHoraExpiracao.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            retorno.Email.ShouldNotBeNull();
            retorno.Token.ShouldNotBeNull();
            retorno.UsuarioLogin.ShouldNotBeNull();
            retorno.UsuarioNome.ShouldNotBeNull();
            retorno.PerfilUsuario.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Usuário - Atualizar perfil")]
        public async Task Ao_atualizar_perfil()
        {
            CriarClaimUsuario();
            
            await InserirNaBase(new Dominio.Entidades.Usuario()
            {
                Login = ConstantesTestes.LOGIN_123456789,
                Nome = ConstantesTestes.NOME_123456789,
                UltimoLogin = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                CriadoLogin = ConstantesTestes.SISTEMA, CriadoPor = ConstantesTestes.SISTEMA, CriadoEm = DateTimeExtension.HorarioBrasilia().Date
            });
           
            var usuario = await GetServicoUsuario().Autenticar(ConstantesTestes.LOGIN_123456789,string.Empty);
            usuario.ShouldNotBeNull();
            
            var retorno = await GetServicoUsuario().AtualizarPerfil(usuario.PerfilUsuario.FirstOrDefault().Perfil);
            retorno.ShouldNotBeNull();
            
            var usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            usuarios.Any(f => f.Login.Equals(ConstantesTestes.LOGIN_123456789)).ShouldBeTrue();
            usuarios.Any(f => f.UltimoLogin.Value.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();
        }
    }
}