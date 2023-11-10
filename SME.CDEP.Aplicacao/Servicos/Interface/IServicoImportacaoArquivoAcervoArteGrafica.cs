using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoImportacaoArquivoAcervoArteGrafica
    {
        Task<ImportacaoArquivoRetornoDTO<AcervoArteGraficaLinhaRetornoDTO>> ImportarArquivo(IFormFile file);
        Task PersistenciaAcervo(IEnumerable<AcervoArteGraficaLinhaDTO> acervosArteGraficaLinhas);
        void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoArteGraficaLinhaDTO> linhas);
        Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoArteGraficaLinhaDTO> linhas);
        void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores);
        Task<ImportacaoArquivoRetornoDTO<AcervoArteGraficaLinhaRetornoDTO>> ObterImportacaoPendente();
        Task<bool> RemoverLinhaDoArquivo(long id, int linhaDoArquivo);
        Task<bool> AtualizarLinhaParaSucesso(long id, int linhaDoArquivo);
    }
}
