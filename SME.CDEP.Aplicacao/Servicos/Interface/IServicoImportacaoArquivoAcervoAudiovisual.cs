using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoImportacaoArquivoAcervoAudiovisual
    {
        Task<ImportacaoArquivoRetornoDTO<AcervoAudiovisualLinhaRetornoDTO>> ImportarArquivo(IFormFile file);
        Task PersistenciaAcervo(IEnumerable<AcervoAudiovisualLinhaDTO> acervosAudiovisualLinhas);
        void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoAudiovisualLinhaDTO> linhas);
        Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoAudiovisualLinhaDTO> linhas);
        void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores);
        Task<ImportacaoArquivoRetornoDTO<AcervoAudiovisualLinhaRetornoDTO>> ObterImportacaoPendente();
        Task<bool> RemoverLinhaDoArquivo(long id, int linhaDoArquivo);
    }
}
