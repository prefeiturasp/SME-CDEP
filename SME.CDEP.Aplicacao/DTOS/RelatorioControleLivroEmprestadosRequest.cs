using SME.CDEP.Aplicacao.Enumerados;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class RelatorioControleLivroEmprestadosRequest
    {
        public string? Solicitante { get; set; }
        public string? Tombo { get; set; }
        public SituacaoSolicitacaoItem SituacaoSolicitacaoItem { get; set; }
        public SituacaoEmprestimo SituacaoEmprestimo { get; set; }
        public ModeloRelatorio Modelo { get; set; }
        public bool SomenteDevolvidos { get; set; }
       
    }
}
