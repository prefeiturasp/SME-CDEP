using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.UseCase.Interface
{
    public interface IRelatorioTitulosMaisPesquisadosUseCase
    {
        Task<Stream?> ExecutarAsync(RelatorioTitulosMaisPesquisadosRequest filtros);
    }
}
