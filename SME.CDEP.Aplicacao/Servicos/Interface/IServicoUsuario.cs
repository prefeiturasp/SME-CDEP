using SME.CDEP.Aplicacao.Dtos;
using SME.CDEP.Dominio.Dominios;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoUsuario
    {
        Task<long> Inserir(UsuarioDto usuarioDto);
        Task<IList<Usuario>> ObterTodos();
        Task<Usuario> Alterar(UsuarioDto usuarioDto);
        Task<Usuario> ObterPorId(long usuarioId);
        Task<UsuarioAutenticacaoRetornoDto> Autenticar(string login, string senha);
        Task<Usuario> ObterPorLogin(string login);
    }
}
