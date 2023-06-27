using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoUsuario : IServicoAplicacao
    {
        Task<long> Inserir(UsuarioDTO usuarioDto);
        Task<IList<UsuarioDTO>> ObterTodos();
        Task<UsuarioDTO> Alterar(UsuarioDTO usuarioDTO);
        Task<UsuarioDTO> ObterPorId(long usuarioId);
        Task<UsuarioAutenticacaoRetornoDTO> Autenticar(string login, string senha);
        Task<UsuarioDTO> ObterPorLogin(string login);
        Task<bool> CadastrarUsuarioExterno(UsuarioExternoDTO usuarioExternoDto);
        Task<DadosUsuarioDTO> ObterMeusDados(string login);
        Task<bool> AlterarSenha(string login, string senhaAtual, string senhaNova, string confirmarSenha);
        Task<bool> AlterarEmail(string login, string email);
        Task<bool> AlterarEndereco(EnderecoTelefoneUsuarioExternoDTO enderecoTelefoneUsuarioExternoDto);
        Task<bool> AlterarTelefone(EnderecoTelefoneUsuarioExternoDTO enderecoTelefoneUsuarioExternoDto);
    }
}
