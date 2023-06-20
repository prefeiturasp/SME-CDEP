using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;

namespace SME.CDEP.TesteIntegracao.ServicosFakes;

public class ServicoAcessosFake: IServicoAcessos
{
    public async Task<UsuarioAutenticacaoRetornoDTO> Autenticar(string login, string senha)
    {
        return new UsuarioAutenticacaoRetornoDTO()
        {
            Email = "seu.email@cdep.gov.br",
            Login = "99999999999",
            Nome = "Nome do usuário de login 10",
        };
    }

    public Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UsuarioCadastradoCoreSSO(string login)
    {
        return login.Equals("99999999998");
    }

    public async Task<bool> CadastrarUsuarioCoreSSO(string login, string nome, string email, string senha)
    {
        return true;
    }

    public async Task<bool> VincularPerfilExternoCoreSSO(string login, Guid perfilId)
    {
        return true;
    }
}