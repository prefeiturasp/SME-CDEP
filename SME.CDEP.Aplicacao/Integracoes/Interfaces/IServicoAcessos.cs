using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Integracoes.Interfaces;

public interface IServicoAcessos
{
    Task<UsuarioAutenticacaoRetornoDTO> Autenticar(string login, string senha);
    Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login);
    Task<bool> UsuarioCadastradoCoreSSO(string login);
    Task<bool> CadastrarUsuarioCoreSSO(string login, string nome, string email, string senha);
    Task<bool> VincularPerfilExternoCoreSSO(string login, Guid perfilId);
}