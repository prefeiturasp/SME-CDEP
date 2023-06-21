using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoPerfilUsuario : IServicoAplicacao
    {
        Task<RetornoPerfilUsuarioDTO> ObterPerfisUsuario(string login);
        Task<bool> VincularPerfilExternoCoreSSO(string login, Guid perfilId);
    }
}
