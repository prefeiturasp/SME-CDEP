using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Dominios;
using SME.CDEP.Dominio.Excecoes;
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

        public async Task<long> Inserir(UsuarioDTO usuarioDto)
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

        public async Task<Usuario> Alterar(UsuarioDTO usuarioDto)
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

        public async Task<bool> CadastrarUsuarioExterno(UsuarioExternoDTO usuarioExternoDto)
        {
            var usuarioAcervo = await ObterPorLogin(usuarioExternoDto.Login);
            if (usuarioAcervo != null)
                throw new NegocioException(MensagemNegocio.VOCE_JA_POSSUI_LOGIN_ACERVO);
            
            var usuarioCoreSSO = await servicoAcessos.UsuarioCadastradoCoreSSO(usuarioExternoDto.Login);
            if (usuarioCoreSSO != null)
                throw new NegocioException(MensagemNegocio.VOCE_JA_POSSUI_LOGIN_CORESSO);

            var retornoCoreSSO = await servicoAcessos.CadastrarUsuarioCoreSSO(usuarioExternoDto.Login, usuarioExternoDto.Nome, usuarioExternoDto.Email, usuarioExternoDto.Senha);

            if (!retornoCoreSSO)
                throw new NegocioException(MensagemNegocio.NAO_FOI_POSSIVEL_CADASTRAR_USUARIO_EXTERNO_NO_CORESSO);
                
            var retorno = await repositorioUsuario.Inserir(new Usuario()
            {
                CriadoEm = DateTime.Now, CriadoPor = usuarioExternoDto.Nome, CriadoRF = usuarioExternoDto.Login,
                Login = usuarioExternoDto.Login, Nome = usuarioExternoDto.Nome, UltimoLogin = DateTime.Now,
            });

            return retorno != 0;
        }

        public async Task<UsuarioAutenticacaoRetornoDTO> Autenticar(string login, string senha)
        {
            var retorno = await servicoAcessos.Autenticar(login, senha);

            await ManutencaoUsuario(login, retorno);
            
            return retorno;
        }

        private async Task ManutencaoUsuario(string login, UsuarioAutenticacaoRetornoDTO retorno)
        {
            var usuario = await repositorioUsuario.ObterPorLogin(login);
            if (usuario != null)
            {
                usuario.UltimoLogin = DateTime.Now;
                usuario.Nome = retorno.Nome;
                await repositorioUsuario.Atualizar(usuario);
            }
            else
            {
                await repositorioUsuario.Inserir(new Usuario()
                {
                    CriadoEm = DateTime.Now, CriadoPor = retorno.Nome, CriadoRF = retorno.Login,
                    Login = retorno.Login, Nome = retorno.Nome, UltimoLogin = DateTime.Now,
                });
            }
        }
    }
}
