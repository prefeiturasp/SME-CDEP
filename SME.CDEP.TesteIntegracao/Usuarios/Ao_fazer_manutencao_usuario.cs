using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Constantes;
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
            var usuarioDto = ObterUsuarioDto(TipoUsuario.SERVIDOR_PUBLICO, ConstantesTestes.NUMERO_1);

            var usuarioId = await servicoUsuario.Inserir(usuarioDto);
            usuarioId.ShouldBeGreaterThan(0);
        }

        [Fact(DisplayName = "Usuário - Obter todos os usuarios")]
        public async Task ObterTodosUsuarios()
        {
            IServicoUsuario servicoUsuario = await CadastrarVariosUsuarios(TipoUsuario.SERVIDOR_PUBLICO);

            var usuarios = await servicoUsuario.ObterTodos();
            usuarios.ShouldNotBeNull();
            usuarios.Count.ShouldBe(11);
        }

        [Fact(DisplayName = "Usuário - Obter por id")]
        public async Task ObterUsuarioPorId()
        {
            IServicoUsuario servicoUsuario = await CadastrarVariosUsuarios(TipoUsuario.POPULACAO_GERAL);

            var usuario = await servicoUsuario.ObterPorId(1);
            usuario.ShouldNotBeNull();
            usuario.Id.ShouldBe(1);
        }

        [Fact(DisplayName = "Usuário - Atualizar")]
        public async Task Atualizar()
        {
            CriarClaimUsuario();
            IServicoUsuario servicoUsuario = await CadastrarVariosUsuarios(TipoUsuario.ESTUDANTE);

            var usuarioAlterado = await servicoUsuario.ObterPorId(1);
            usuarioAlterado.Login = ConstantesTestes.LOGIN_123456789;
            usuarioAlterado.Nome = ConstantesTestes.NOME_123456789;
            var usuario = await servicoUsuario.Alterar(usuarioAlterado);
            usuario.ShouldNotBeNull();
            usuario.Login.ShouldBe(ConstantesTestes.LOGIN_123456789);
            usuario.AlteradoLogin.ShouldBe(ConstantesTestes.LOGIN_123456789);
            usuario.AlteradoPor.ShouldBe(ConstantesTestes.SISTEMA);
            usuario.AlteradoEm.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
        }
        
        [Fact(DisplayName = "Usuário - Obter por login")]
        public async Task ObterPorLogin()
        {
            IServicoUsuario servicoUsuario = await CadastrarVariosUsuarios(TipoUsuario.PROFESSOR);

            var usuario = await servicoUsuario.ObterPorLogin(ConstantesTestes.LOGIN_99999999998);
            usuario.ShouldNotBeNull();
            usuario.Login.ShouldBe(ConstantesTestes.LOGIN_99999999998);
        }
        
        [Fact(DisplayName = "Usuário - Obter por login - falhar")]
        public async Task ObterPorLoginComFalha()
        {
            IServicoUsuario servicoUsuario = await CadastrarVariosUsuarios(TipoUsuario.POPULACAO_GERAL);

            var usuario = await servicoUsuario.ObterPorLogin(ConstantesTestes.LOGIN_99999999999);
            usuario.ShouldNotBeNull();
            usuario.Login.ShouldBe(ConstantesTestes.LOGIN_99999999999);
        }

        private async Task<IServicoUsuario> CadastrarVariosUsuarios(TipoUsuario tipoUsuario)
        {
            var servicoUsuario = GetServicoUsuario();

            for (int i = 0; i <= 10; i++)
            {
                var usuarioDto = ObterUsuarioDto(tipoUsuario, i.ToString());

                var usuarioId = await servicoUsuario.Inserir(usuarioDto);
                usuarioId.ShouldBeGreaterThan(0);
            }

            return servicoUsuario;
        }
    }
}