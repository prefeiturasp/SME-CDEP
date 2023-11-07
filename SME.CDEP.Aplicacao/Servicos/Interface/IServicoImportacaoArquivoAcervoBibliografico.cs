using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoImportacaoArquivoAcervoBibliografico
    {
        Task<ImportacaoArquivoRetornoDTO<AcervoBibliograficoLinhaRetornoDTO>> ImportarArquivo(IFormFile file);
        Task PersistenciaAcervoBibliografico(IEnumerable<AcervoBibliograficoLinhaDTO> acervosBibliograficosLinhas, long importacaoArquivoId);
        CoAutorDTO[] ObterCoAutoresTipoAutoria(string coautores, string tiposAutoria);
        void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoBibliograficoLinhaDTO> linhas);
        Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoBibliograficoLinhaDTO> linhas);
        void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores);
    }
}
