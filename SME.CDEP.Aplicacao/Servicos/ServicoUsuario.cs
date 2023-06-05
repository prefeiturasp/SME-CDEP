using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Dominios;
using SME.CDEP.Infra.Dados.Dtos;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoUsuario : IServicoUsuario
    {
        private readonly IRepositorioUsuario repositorioUsuario;
        public ServicoUsuario(IRepositorioUsuario repositorioUsuario) 
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
        }

        public async Task<long> Inserir(UsuarioDto usuarioDto)
        {
            var usuario = new Usuario()
            {
                CriadoEm = DateTime.Now,
                CriadoPor = "Teste",
                CriadoRF = "Teste",
                Login = usuarioDto.Login,
                Nome = usuarioDto.Nome,
            }; 

            return await repositorioUsuario.Inserir(usuario);
        }

        public async Task<IList<Usuario>> ObterTodos()
        {
            return await repositorioUsuario.ObterTodos();
        }

        public async Task<Usuario> Alterar(UsuarioDto usuarioDto)
        {
            var usuario = new Usuario()
            {
                CriadoEm = DateTime.Now,
                CriadoPor = "Teste",
                CriadoRF = "Teste",
                Login = usuarioDto.Login,
                Nome = usuarioDto.Nome,
                AlteradoEm = DateTime.Now,
                AlteradoPor = "Teste Alterado",
                AlteradoRF = "Teste Alterado",
                Id = usuarioDto.Id,
            };

            return await repositorioUsuario.Atualizar(usuario);
        }

        public async Task<Usuario> ObterPorId(long usuarioId)
        {
            return await repositorioUsuario.ObterPorId(usuarioId);
        }
    }
}
