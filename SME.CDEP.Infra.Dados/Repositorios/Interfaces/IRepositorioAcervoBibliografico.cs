using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoBibliografico : IRepositorioBase<AcervoBibliografico>
    {
        Task<AcervoBibliograficoCompleto> ObterPorId(long id);
    }
}