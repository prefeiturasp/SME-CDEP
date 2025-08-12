using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.UseCase.Interface
{
    public interface IRelatorioControleAcervoUseCase
    {
        Task<Stream> Executar(RelatorioControleAcervoRequest filtros);
    }
}
