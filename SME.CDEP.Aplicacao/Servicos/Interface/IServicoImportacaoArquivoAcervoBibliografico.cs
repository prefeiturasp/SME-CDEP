using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoImportacaoArquivoAcervoBibliografico
    {
        Task<ImportacaoArquivoRetornoDTO<AcervoBibliograficoLinhaRetornoDTO>> ImportarArquivo(IFormFile file);
        Task PersistenciaAcervo(IEnumerable<AcervoBibliograficoLinhaDTO> acervosBibliograficosLinhas);
        CoAutorDTO[] ObterCoAutoresTipoAutoria(string coautores, string tiposAutoria);
        void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoBibliograficoLinhaDTO> linhas);
        Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoBibliograficoLinhaDTO> linhas);
        void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores);
        Task<ImportacaoArquivoRetornoDTO<AcervoBibliograficoLinhaRetornoDTO>> ObterImportacaoPendente();
        Task<bool> RemoverLinhaDoArquivo(long id, int linhaDoArquivo);
    }
}
