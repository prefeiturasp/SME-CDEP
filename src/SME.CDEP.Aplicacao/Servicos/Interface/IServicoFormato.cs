using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoFormato : IServicoIdNomeTipoExcluido
    {
        Task<long> ObterPorNomeETipo(string nome, int tipoFormato);
    }
}
