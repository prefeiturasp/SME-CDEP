using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Dominios;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoPerfilUsuario
    {
        Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login);
    }
}
