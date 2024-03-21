using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoUsuario : IServicoAplicacao
    {
        Task<long> Inserir(UsuarioDTO usuarioDto);
        Task<IEnumerable<UsuarioDTO>> ObterTodos();
        Task<UsuarioDTO> Alterar(UsuarioDTO usuarioDTO);
        Task<UsuarioDTO> ObterPorId(long usuarioId);
        Task<RetornoPerfilUsuarioDTO> Autenticar(string login, string senha);
        Task<UsuarioDTO> ObterPorLogin(string login);
        Task<bool> InserirUsuarioExterno(UsuarioExternoDTO usuarioExternoDto);
        Task<DadosUsuarioDTO> ObterMeusDados(string login);
        Task<bool> AlterarSenha(string login, AlterarSenhaUsuarioDTO alterarSenhaUsuarioDto);
        Task<bool> AlterarEmail(string login, string email);
        Task<bool> AlterarEndereco(string login, EnderecoUsuarioExternoDTO enderecoUsuarioExternoDto);
        Task<bool> AlterarTelefone(string login, string telefone);
        Task<string> SolicitarRecuperacaoSenha(string login);
        Task<bool> TokenRecuperacaoSenhaEstaValido(Guid token);
        Task<RetornoPerfilUsuarioDTO?> AlterarSenhaComTokenRecuperacao(RecuperacaoSenhaDto recuperacaoSenhaDto);
        Task<bool> ValidarCpfExistente(string cpf);
        Task<bool> AlterarTipoUsuario(string login, TipoUsuarioExternoDTO tipoUsuario);
        IEnumerable<Permissao> ObterPermissoes();
        Task<RetornoPerfilUsuarioDTO> RevalidarToken(string token);
        Task<RetornoPerfilUsuarioDTO> AtualizarPerfil(Guid perfilUsuarioId);
        Task<DadosSolicitanteDTO> ObterDadosSolicitante();
        Task<UsuarioDTO> ObterUsuarioLogado();
        Task<IEnumerable<ResponsavelDTO>> ObterUsuariosComPerfisResponsavel();
        Task<DadosSolicitanteDTO> ObterDadosSolicitantePorUsuarioId(long usuarioId);
        Task<DadosSolicitanteDTO> ObterDadosSolicitantePorRfOuCpf(string rfOuCpf);
        Guid ObterPerfilUsuarioLogado();
    }
}
