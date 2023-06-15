using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Dominios;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoUsuario
    {
        Task<long> Inserir(UsuarioDTO usuarioDto);
        Task<IList<Usuario>> ObterTodos();
        Task<Usuario> Alterar(UsuarioDTO usuarioDto);
        Task<Usuario> ObterPorId(long usuarioId);
        Task<UsuarioAutenticacaoRetornoDTO> Autenticar(string login, string senha);
        Task<Usuario> ObterPorLogin(string login);
        Task<bool> CadastrarUsuarioExterno(UsuarioExternoDTO usuarioExternoDto);
    }
}
