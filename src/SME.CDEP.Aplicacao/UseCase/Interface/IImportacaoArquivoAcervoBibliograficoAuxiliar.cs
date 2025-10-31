
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra;

namespace SME.CDEP.Aplicacao
{
    public interface IImportacaoArquivoAcervoBibliograficoAuxiliar
    {
        Task CarregarDominiosBibliograficos();
        Task PersistenciaAcervo(IEnumerable<AcervoBibliograficoLinhaDTO> acervosBibliograficosLinhas);
        void ValidarPreenchimentoValorFormatoQtdeCaracteres(IEnumerable<AcervoBibliograficoLinhaDTO> linhas);
    }
}