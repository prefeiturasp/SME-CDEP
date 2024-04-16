using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoImportacaoArquivoAcervoFotografico
    {
        Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoFotograficoDTO,AcervoFotograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ImportarArquivo(IFormFile file);
        Task PersistenciaAcervo(IEnumerable<AcervoFotograficoLinhaDTO> acervosFotograficosLinhas);
        void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoFotograficoLinhaDTO> linhas);
        Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoFotograficoDTO,AcervoFotograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPendente();
        Task<bool> RemoverLinhaDoArquivo(long id, LinhaDTO linhaDoArquivo);
        Task<bool> AtualizarLinhaParaSucesso(long id, LinhaDTO linhaDoArquivo);
        Task<long> AtualizarImportacao(long id, string conteudo, ImportacaoStatus? status = null);
        Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoFotograficoDTO,AcervoFotograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPorId(long id);
        Task CarregarDominiosFotograficos();
    }
}
