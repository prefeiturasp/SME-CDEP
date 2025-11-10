using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.UseCase.Interface
{
    public interface IRelatorioControleDownloadAcervoUseCase
    {
        Task<Stream?> ExecutarAsync(RelatorioControleDownloadAcervoRequest filtros);
    }
}
