using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoImportacaoArquivo  : IServicoAplicacao, IServicoImportacaoArquivoManutencao
    {
        Task<long> Inserir(ImportacaoArquivo importacaoArquivo);
        Task<ImportacaoArquivoDTO> Alterar(ImportacaoArquivo importacaoArquivo);
        Task<bool> Excluir(long importacaoArquivoId);
        Task<ImportacaoArquivoCompleto> ObterUltimaImportacao();
    }
}
