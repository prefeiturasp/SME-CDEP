using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoImportacaoArquivoAcervoDocumental
    {
        Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ImportarArquivo(IFormFile file);
        Task PersistenciaAcervo(IEnumerable<AcervoDocumentalLinhaDTO> acervosDocumentalLinhas);
        void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoDocumentalLinhaDTO> linhas);
        Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPendente();
        Task<bool> RemoverLinhaDoArquivo(long id, LinhaDTO linhaDoArquivo);
        Task<bool> AtualizarLinhaParaSucesso(long id, LinhaDTO linhaDoArquivo);
        Task<long> AtualizarImportacao(long id, string conteudo, ImportacaoStatus? status = null);
        Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoDocumentalDTO, AcervoDocumentalLinhaRetornoDTO>, AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPorId(long id);
        Task CarregarDominios();
    }
}
