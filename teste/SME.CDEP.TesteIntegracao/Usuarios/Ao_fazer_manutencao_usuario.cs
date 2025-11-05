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
            var usuarioDto = UsuarioDTOMock.GerarUsuarioDTO(TipoUsuario.SERVIDOR_PUBLICO).Generate(); 

            var usuarioId = await servicoUsuario.Inserir(usuarioDto);
            usuarioId.ShouldBeGreaterThan(0);
        }

        [Fact(DisplayName = "Usuário - Obter todos os usuarios")]
        public async Task ObterTodosUsuarios()
        {
            IServicoUsuario servicoUsuario = await CadastrarVariosUsuarios(TipoUsuario.SERVIDOR_PUBLICO);

            var usuarios = await servicoUsuario.ObterTodos();
            usuarios.ShouldNotBeNull();
            usuarios.Count().ShouldBe(10);
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
            usuarioAlterado.Login = faker.Person.FirstName;
            usuarioAlterado.Nome = faker.Person.FullName;
            var usuario = await servicoUsuario.Alterar(usuarioAlterado);
            usuario.ShouldNotBeNull();
            usuario.Login.ShouldBe(usuarioAlterado.Login);
            usuario.AlteradoLogin.ShouldNotBeEmpty();
            usuario.AlteradoPor.ShouldNotBeEmpty();
            usuario.AlteradoEm.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
        }
        
        [Fact(DisplayName = "Usuário - Obter por login")]
        public async Task ObterPorLogin()
        {
            IServicoUsuario servicoUsuario = await CadastrarVariosUsuarios(TipoUsuario.PROFESSOR);
            var usuarios = ObterTodos<Dominio.Entidades.Usuario>();

            var usuario = await servicoUsuario.ObterPorLogin(usuarios.FirstOrDefault().Login);
            usuario.ShouldNotBeNull();
            usuario.Login.ShouldBe(usuarios.FirstOrDefault().Login);
        }
        
        [Fact(DisplayName = "Usuário - Obter por login - falhar")]
        public async Task ObterPorLoginComFalha()
        {
            IServicoUsuario servicoUsuario = await CadastrarVariosUsuarios(TipoUsuario.POPULACAO_GERAL);
            var usuarios = ObterTodos<Dominio.Entidades.Usuario>();
            
            var usuario = await servicoUsuario.ObterPorLogin(usuarios.FirstOrDefault().Login);
            usuario.ShouldNotBeNull();
            usuario.Login.ShouldBe(usuarios.FirstOrDefault().Login);
        }

        private async Task<IServicoUsuario> CadastrarVariosUsuarios(TipoUsuario tipoUsuario)
        {
            var servicoUsuario = GetServicoUsuario();

            var usuariosDTOs = UsuarioDTOMock.GerarUsuarioDTO(tipoUsuario).Generate(10);

            var contador = 1;

            foreach (var usuarioDto in usuariosDTOs)
            {
                usuarioDto.Login = $"{usuarioDto.Login}_{contador}";
                var usuarioId = await servicoUsuario.Inserir(usuarioDto);
                usuarioId.ShouldBeGreaterThan(0);
                contador++;
            }

            return servicoUsuario;
        }
    }
}