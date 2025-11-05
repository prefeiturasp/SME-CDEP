using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioSuporte : IRepositorioBase<Suporte>
    {
        Task<long> ObterPorNomeTipo(string nome, int tipoSuporte);
    }
}