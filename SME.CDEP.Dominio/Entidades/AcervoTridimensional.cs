using SME.CDEP.Dominio.Enumerados;
using System.ComponentModel.DataAnnotations.Schema;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoTridimensional : EntidadeBase
    {
        public Acervo Acervo { get; set; }
        public long AcervoId { get; set; }
        public string Procedencia { get; set; }
        public long ConservacaoId { get; set; }
        public int Quantidade { get; set; }
        public string? Largura { get; set; }
        public string? Altura { get; set; }
        public string? Profundidade { get; set; }
        public string? Diametro { get; set; }

        [Column("situacao_acervo")]
        public SituacaoAcervo SituacaoAcervo { get; set; }
    }
}
