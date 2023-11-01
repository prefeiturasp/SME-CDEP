using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class ImportacaoArquivo : EntidadeBaseAuditavel
    {
        public string Nome { get; set; }
        public TipoAcervo TipoAcervo { get; set; }
        public ImportacaoStatus Status { get; set; }
        public string Conteudo { get; set; }
    }
}
