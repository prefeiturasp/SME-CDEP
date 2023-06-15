using SME.CDEP.Aplicacao.DTOS;
using SSME.CDEP.Aplicacao.Integracoes.Interfaces;

namespace SME.CDEP.TesteIntegracao.ServicosFakes;

public class ServicoAcessosFake: IServicoAcessos
{
    public async Task<UsuarioAutenticacaoRetornoDTO> Autenticar(string login, string senha)
    {
        return new UsuarioAutenticacaoRetornoDTO()
        {
            Email = "seu.email@cdep.gov.br",
            Login = "login_10",
            Nome = "Nome do usuário de login 10",
        };
    }

    public Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UsuarioCadastradoCoreSSO(string login)
    {
        return login.Equals("usuario_coresso");
    }

    public async Task<bool> CadastrarUsuarioCoreSSO(string login, string nome, string email, string senha)
    {
        return true;
    }
}