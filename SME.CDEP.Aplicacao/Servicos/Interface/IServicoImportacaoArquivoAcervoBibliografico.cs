using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoImportacaoArquivoAcervoBibliografico
    {
        Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoBibliograficoDTO,AcervoBibliograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ImportarArquivo(IFormFile file);
        Task PersistenciaAcervo(IEnumerable<AcervoBibliograficoLinhaDTO> acervosBibliograficosLinhas);
        CoAutorDTO[] ObterCoAutoresTipoAutoria(string coautores, string tiposAutoria);
        void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoBibliograficoLinhaDTO> linhas);
        Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoBibliograficoLinhaDTO> linhas);
        void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores);
        Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoBibliograficoDTO,AcervoBibliograficoLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPendente();
        Task<bool> RemoverLinhaDoArquivo(long id, int linhaDoArquivo);
        Task<bool> AtualizarLinhaParaSucesso(long id, int linhaDoArquivo);
    }
}
