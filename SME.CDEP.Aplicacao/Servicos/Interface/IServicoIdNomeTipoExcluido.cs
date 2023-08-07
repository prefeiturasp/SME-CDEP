using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoIdNomeTipoExcluido
    {
        Task<long> Inserir(IdNomeExcluidoTipoDto idNomeExcluidoTipoDto);
        Task<IList<IdNomeExcluidoTipoDto>> ObterTodos();
        Task<IdNomeExcluidoTipoDto> Alterar(IdNomeExcluidoTipoDto idNomeExcluidoTipoDto);
        Task<IdNomeExcluidoTipoDto> ObterPorId(long Id);
        Task<bool> Excluir(long Id);
    }
}
