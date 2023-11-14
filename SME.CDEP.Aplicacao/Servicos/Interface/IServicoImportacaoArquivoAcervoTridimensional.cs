using Microsoft.AspNetCore.Http;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoImportacaoArquivoAcervoTridimensional
    {
        Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ImportarArquivo(IFormFile file);
        Task PersistenciaAcervo(IEnumerable<AcervoTridimensionalLinhaDTO> acervosTridimensionalsLinhas);
        void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoTridimensionalLinhaDTO> linhas);
        Task ValidacaoObterOuInserirDominios(IEnumerable<AcervoTridimensionalLinhaDTO> linhas);
        void DefinirCreditosAutores(List<IdNomeTipoDTO> creditosAutores);
        Task<ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoTridimensionalDTO,AcervoTridimensionalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>> ObterImportacaoPendente();
        Task<bool> RemoverLinhaDoArquivo(long id, int linhaDoArquivo);
        Task<bool> AtualizarLinhaParaSucesso(long id, int linhaDoArquivo);
        Task<long> AtualizarImportacao(long id, string conteudo, ImportacaoStatus? status = null);
    }
}
