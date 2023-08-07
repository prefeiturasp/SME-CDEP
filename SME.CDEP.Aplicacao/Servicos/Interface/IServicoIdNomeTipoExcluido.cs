using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoIdNomeTipoExcluido
    {
        Task<long> Inserir(BaseComNomeTipoDto baseComNomeTipoDto);
        Task<IList<BaseComNomeTipoDto>> ObterTodos();
        Task<BaseComNomeTipoDto> Alterar(BaseComNomeTipoDto baseComNomeTipoDto);
        Task<BaseComNomeTipoDto> ObterPorId(long Id);
        Task<bool> Excluir(long Id);
    }
}
