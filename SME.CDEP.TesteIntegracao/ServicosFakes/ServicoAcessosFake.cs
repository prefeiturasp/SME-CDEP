using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;

namespace SME.CDEP.TesteIntegracao.ServicosFakes;

public class ServicoAcessosFake: IServicoAcessos
{
    public Task<UsuarioAutenticacaoRetornoDTO> Autenticar(string login, string senha)
    {
        return Task.FromResult(new UsuarioAutenticacaoRetornoDTO()
        {
            Email = "seu.email@cdep.gov.br",
            Login = "99999999999",
            Nome = "Nome do usuário de login 10",
        });
    }

    public Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UsuarioCadastradoCoreSSO(string login)
    {
        return Task.FromResult(login.Equals("99999999998"));
    }

    public Task<bool> CadastrarUsuarioCoreSSO(string login, string nome, string email, string senha)
    {
        return Task.FromResult(true);
    }

    public Task<bool> VincularPerfilExternoCoreSSO(string login, Guid perfilId)
    {
        return Task.FromResult(true);
    }

    public Task<bool> AlterarSenha(string login, string senhaAtual, string senhaNova)
    {
        return Task.FromResult(true);
    }
}