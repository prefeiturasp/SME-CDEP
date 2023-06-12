using SME.CDEP.Aplicacao.Dtos;
using SSME.CDEP.Aplicacao.Integracoes.Interfaces;

namespace SME.CDEP.TesteIntegracao.ServicosFakes;

public class ServicoAcessosFake: IServicoAcessos
{
    public async Task<UsuarioAutenticacaoRetornoDto> Autenticar(string login, string senha)
    {
        return new UsuarioAutenticacaoRetornoDto()
        {
            Autenticado = true,
            Email = "seu.email@cdep.gov.br",
            Token = "token",
            UsuarioLogin = "login_10",
            UsuarioNome = "Nome do usuário de login 10",
            DataHoraExpiracao = DateTime.Now.AddMinutes(120)
        };
    }
}