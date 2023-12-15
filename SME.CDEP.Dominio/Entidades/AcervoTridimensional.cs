namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoTridimensional : EntidadeBase
    {
        public Acervo Acervo { get; set; }
        public long AcervoId { get; set; }
        public string Procedencia { get; set; }
        public long ConservacaoId { get; set; }
        public long Quantidade { get; set; }
        public double? Largura { get; set; }
        public double? Altura { get; set; }
        public double? Profundidade { get; set; }
        public double? Diametro { get; set; }
    }
}
