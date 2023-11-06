using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoMaterial : IServicoAplicacao,IServicoIdNomeTipoExcluido
    {
        Task<long> ObterPorNomeTipo(string nome, TipoMaterial tipo);
    }
}
