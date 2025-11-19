using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.UseCase.Interface
{
    public interface IRelatorioControleLivrosEmprestadosUseCase
    {
        Task<Stream?> ExecutarAsync(RelatorioControleLivroEmprestadosRequest filtros);
    }
}
