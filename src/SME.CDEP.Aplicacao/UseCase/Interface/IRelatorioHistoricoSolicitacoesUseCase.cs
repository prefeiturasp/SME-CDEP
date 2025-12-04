using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.UseCase.Interface
{
    public interface IRelatorioHistoricoSolicitacoesUseCase
    {
        Task<Stream?> ExecutarAsync(RelatorioHistoricoSolicitacoesRequest filtros);
    }
}