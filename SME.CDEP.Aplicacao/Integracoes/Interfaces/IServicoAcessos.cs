using SME.CDEP.Aplicacao.Dtos;

namespace SSME.CDEP.Aplicacao.Integracoes.Interfaces;

public interface IServicoAcessos
{
    Task<UsuarioAutenticacaoRetornoDto> Autenticar(string login, string senha);
}