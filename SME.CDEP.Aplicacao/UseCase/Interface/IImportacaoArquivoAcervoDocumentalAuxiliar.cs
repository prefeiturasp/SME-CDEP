
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra;

namespace SME.CDEP.Aplicacao
{
    public interface IImportacaoArquivoAcervoDocumentalAuxiliar
    {
        Task CarregarDominiosDocumentais();
        Task PersistenciaAcervo(IEnumerable<AcervoDocumentalLinhaDTO> acervosDocumentalLinhas);
        void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoDocumentalLinhaDTO> linhas);
    }
}