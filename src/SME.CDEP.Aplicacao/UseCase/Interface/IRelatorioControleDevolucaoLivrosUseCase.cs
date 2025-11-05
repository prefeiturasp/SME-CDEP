using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.UseCase.Interface
{
    public interface IRelatorioControleDevolucaoLivrosUseCase
    {
        Task<Stream> Executar(RelatorioControleDevolucaoLivrosRequest filtros);
    }
}
