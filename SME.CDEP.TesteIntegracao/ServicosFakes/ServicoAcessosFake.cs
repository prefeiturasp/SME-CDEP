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
            Perfil = Guid.NewGuid(),
            Token = "token",
            PerfilNome = "Perfil do usuário",
            UsuarioLogin = "login do usuario",
            UsuarioNome = "Nome do usuário",
            DataHoraExpiracao = DateTime.Now.AddMinutes(120)
        };
    }
}