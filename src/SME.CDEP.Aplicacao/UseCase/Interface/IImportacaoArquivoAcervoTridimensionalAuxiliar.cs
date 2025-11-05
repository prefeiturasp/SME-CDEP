
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra;

namespace SME.CDEP.Aplicacao
{
    public interface IImportacaoArquivoAcervoTridimensionalAuxiliar
    {
        Task CarregarDominiosTridimensionais();
        Task PersistenciaAcervo(IEnumerable<AcervoTridimensionalLinhaDTO> acervosTridimensionalsLinhas);
        void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoTridimensionalLinhaDTO> linhas);
    }
}