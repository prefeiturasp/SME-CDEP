using SME.CDEP.Dominio.Enumerados;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class RelatorioControleAcervoRequest
    {
        public SituacaoAcervo SituacaoAcervo { get; set; }
        public TipoAcervo TipoAcervo { get; set; }
    }
}
