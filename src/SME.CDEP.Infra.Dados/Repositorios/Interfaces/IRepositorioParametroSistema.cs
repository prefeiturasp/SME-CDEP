using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioParametroSistema : IRepositorioBaseAuditavel<ParametroSistema>
    {
        Task<ParametroSistema> ObterParametroPorTipoEAno(TipoParametroSistema tipoParametroSistema, int ano = 0);
        Task<ParametroSistema?> ObterParametroPorTipoAsync(TipoParametroSistema tipoParametroSistema);
    }
}
