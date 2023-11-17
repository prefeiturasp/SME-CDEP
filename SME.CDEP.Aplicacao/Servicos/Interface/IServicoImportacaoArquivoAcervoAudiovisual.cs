using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoImportacaoArquivoAcervoAudiovisual
    {
        Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoAudiovisualDTO,AcervoAudiovisualLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ImportarArquivo(IFormFile file);
        Task PersistenciaAcervo(IEnumerable<AcervoAudiovisualLinhaDTO> acervosAudiovisualLinhas);
        void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoAudiovisualLinhaDTO> linhas);
        Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoAudiovisualLinhaDTO> linhas);
        void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores);
        Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoAudiovisualDTO,AcervoAudiovisualLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPendente();
        Task<bool> RemoverLinhaDoArquivo(long id, LinhaDTO linhaDoArquivo);
        Task<bool> AtualizarLinhaParaSucesso(long id, LinhaDTO linhaDoArquivo);
        Task<long> AtualizarImportacao(long id, string conteudo, ImportacaoStatus? status = null);
    }
}
