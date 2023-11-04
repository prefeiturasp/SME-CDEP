using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioMaterial : IRepositorioBase<Material>
    {
        Task<long> ObterPorNomeTipo(string material, TipoMaterial tipoMaterial);
    }
}