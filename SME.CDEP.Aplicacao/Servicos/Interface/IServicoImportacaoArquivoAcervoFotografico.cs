using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoImportacaoArquivoAcervoFotografico
    {
        Task<ImportacaoArquivoRetornoDTO<AcervoFotograficoLinhaRetornoDTO>> ImportarArquivo(IFormFile file);
        Task PersistenciaAcervo(IEnumerable<AcervoFotograficoLinhaDTO> acervosFotograficosLinhas);
        void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoFotograficoLinhaDTO> linhas);
        Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoFotograficoLinhaDTO> linhas);
        void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores);
        Task<ImportacaoArquivoRetornoDTO<AcervoFotograficoLinhaRetornoDTO>> ObterImportacaoPendente();
    }
}
