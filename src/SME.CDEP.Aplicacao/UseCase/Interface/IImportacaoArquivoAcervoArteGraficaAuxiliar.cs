
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra;

namespace SME.CDEP.Aplicacao
{
    public interface IImportacaoArquivoAcervoArteGraficaAuxiliar
    {
        Task CarregarDominiosArteGrafica();
        Task PersistenciaAcervo(IEnumerable<AcervoArteGraficaLinhaDTO> acervosArtesGraficasLinhas);
        void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoArteGraficaLinhaDTO> linhas);
    }
}