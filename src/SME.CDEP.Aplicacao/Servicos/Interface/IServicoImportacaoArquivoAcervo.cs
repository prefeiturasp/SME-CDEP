namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoImportacaoArquivoAcervo
    {
        Task<bool> Excluir(long id);
    }
}
