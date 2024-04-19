
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra;

namespace SME.CDEP.Aplicacao
{
    public interface IImportacaoArquivoAcervoAudiovisualAuxiliar
    {
        Task CarregarDominiosAudiovisuais();
        Task PersistenciaAcervo(IEnumerable<AcervoAudiovisualLinhaDTO> acervosAudiovisualLinhas);
        void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoAudiovisualLinhaDTO> linhas);
    }
}