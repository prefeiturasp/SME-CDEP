using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoImportacaoArquivoAcervoArteGrafica
    {
        Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoArteGraficaDTO,AcervoArteGraficaLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ImportarArquivo(IFormFile file);
        Task PersistenciaAcervo(IEnumerable<AcervoArteGraficaLinhaDTO> acervosArteGraficaLinhas);
        void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoArteGraficaLinhaDTO> linhas);
        Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoArteGraficaLinhaDTO> linhasComsucesso);
        void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores);
        Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoArteGraficaDTO,AcervoArteGraficaLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPendente();
        Task<bool> RemoverLinhaDoArquivo(long id, LinhaDTO linha);
        Task<bool> AtualizarLinhaParaSucesso(long id, LinhaDTO linha);
        Task<long> AtualizarImportacao(long id, string conteudo, ImportacaoStatus? status = null);
    }
}
