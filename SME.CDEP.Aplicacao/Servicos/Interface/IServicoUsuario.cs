using SME.CDEP.Aplicacao.DTO;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoUsuario : IServicoAplicacao
    {
        Task<long> Inserir(UsuarioIdNomeLoginDTO usuarioIdNomeLoginDto);
        Task<IList<UsuarioDTO>> ObterTodos();
        Task<UsuarioDTO> Alterar(UsuarioDTO usuarioDTO);
        Task<UsuarioDTO> ObterPorId(long usuarioId);
        Task<UsuarioAutenticacaoRetornoDTO> Autenticar(string login, string senha);
        Task<UsuarioDTO> ObterPorLogin(string login);
        Task<bool> CadastrarUsuarioExterno(UsuarioExternoDTO usuarioExternoDto);
        Task<DadosUsuarioDTO> ObterMeusDados(string login);
    }
}
