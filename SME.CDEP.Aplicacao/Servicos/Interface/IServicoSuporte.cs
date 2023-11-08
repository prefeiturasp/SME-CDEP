using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoSuporte : IServicoAplicacao,IServicoIdNomeTipoExcluido
    {
        Task<long> ObterPorNomeETipo(string nome, int tipoSuporte);
    }
}
