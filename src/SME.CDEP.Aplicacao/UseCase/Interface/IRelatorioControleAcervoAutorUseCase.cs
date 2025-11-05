using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.UseCase.Interface
{
    public interface IRelatorioControleAcervoAutorUseCase
    {
        Task<Stream> Executar(RelatorioControleAcervoAutorRequest filtros);
    }
}
