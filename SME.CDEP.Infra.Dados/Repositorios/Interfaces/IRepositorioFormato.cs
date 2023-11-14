using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioFormato : IRepositorioBase<Formato>
    {
        Task<long> ObterPorNomeETipo(string nome, int tipoFormato);
    }
}