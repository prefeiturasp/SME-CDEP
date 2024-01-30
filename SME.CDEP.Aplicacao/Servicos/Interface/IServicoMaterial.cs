using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoMaterial : IServicoIdNomeTipoExcluido
    {
        Task<long> ObterPorNomeETipo(string nome, TipoMaterial tipo);
    }
}
