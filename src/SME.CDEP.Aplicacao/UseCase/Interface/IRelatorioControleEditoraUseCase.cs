using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.UseCase.Interface
{
    public interface IRelatorioControleEditoraUseCase
    {
        Task<Stream> Executar(RelatorioControleEditoraRequest filtros);
    }
}
