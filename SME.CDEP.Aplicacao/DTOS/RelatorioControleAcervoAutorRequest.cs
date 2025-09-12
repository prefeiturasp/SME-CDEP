using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class RelatorioControleAcervoAutorRequest
    {
        public List<int>? Autores { get; set; }
        public TipoAcervo TipoAcervo { get; set; }
    }
}
