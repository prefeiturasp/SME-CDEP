using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Dominios;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoUsuario
    {
        Task<long> Inserir(UsuarioDTO usuarioDto);
        Task<IList<RetornoUsuarioDTO>> ObterTodos();
        Task<RetornoUsuarioDTO> Alterar(UsuarioDTO usuarioDto);
        Task<RetornoUsuarioDTO> ObterPorId(long usuarioId);
        Task<UsuarioAutenticacaoRetornoDTO> Autenticar(string login, string senha);
        Task<RetornoUsuarioDTO> ObterPorLogin(string login);
        Task<bool> CadastrarUsuarioExterno(UsuarioExternoDTO usuarioExternoDto);
    }
}
