using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoImportacaoArquivoAcervoDocumental
    {
        Task<ImportacaoArquivoRetornoDTO<AcervoDocumentalLinhaRetornoDTO>> ImportarArquivo(IFormFile file);
        Task PersistenciaAcervo(IEnumerable<AcervoDocumentalLinhaDTO> acervosDocumentalLinhas);
        void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoDocumentalLinhaDTO> linhas);
        Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoDocumentalLinhaDTO> linhas);
        void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores);
        Task<ImportacaoArquivoRetornoDTO<AcervoDocumentalLinhaRetornoDTO>> ObterImportacaoPendente();
    }
}
