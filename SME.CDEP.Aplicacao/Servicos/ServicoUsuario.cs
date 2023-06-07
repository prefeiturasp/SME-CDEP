using SME.CDEP.Aplicacao.Dtos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Dominios;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SSME.CDEP.Aplicacao.Integracoes.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoUsuario : IServicoUsuario
    {
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IServicoAcessos servicoAcessos;
        
        public ServicoUsuario(IRepositorioUsuario repositorioUsuario,IServicoAcessos servicoAcessos) 
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.servicoAcessos = servicoAcessos ?? throw new ArgumentNullException(nameof(servicoAcessos));
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
        
        public async Task<Usuario> ObterPorLogin(string login)
        {
            return await repositorioUsuario.ObterPorLogin(login);
        }

        public async Task<UsuarioAutenticacaoRetornoDto> Autenticar(string login, string senha)
        {
            var retorno = await servicoAcessos.Autenticar(login, senha);

            await ManutencaoUsuario(login, retorno);
            
            return retorno;
        }

        private async Task ManutencaoUsuario(string login, UsuarioAutenticacaoRetornoDto retorno)
        {
            var usuario = await repositorioUsuario.ObterPorLogin(login);
            if (usuario != null)
            {
                usuario.UltimoLogin = DateTime.Now;
                usuario.Nome = retorno.UsuarioNome;
                await repositorioUsuario.Atualizar(usuario);
            }
            else
            {
                await repositorioUsuario.Inserir(new Usuario()
                {
                    CriadoEm = DateTime.Now, CriadoPor = retorno.UsuarioNome, CriadoRF = retorno.UsuarioLogin,
                    Login = retorno.UsuarioLogin, Nome = retorno.UsuarioNome, UltimoLogin = DateTime.Now,
                    CodigoRf = retorno.UsuarioLogin, PerfilAtual = retorno.Perfil
                });
            }
        }
    }
}
