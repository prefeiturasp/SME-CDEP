using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoTridimensional : IRepositorioBase<AcervoTridimensional>
    {
        Task<AcervoTridimensionalCompleto> ObterPorId(long id);
    }
}