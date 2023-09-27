namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoTridimensional : EntidadeBase
    {
        public Acervo Acervo { get; set; }
        public long AcervoId { get; set; }
        public string Procedencia { get; set; }
        public string DataAcervo { get; set; }
        public long ConservacaoId { get; set; }
        public long Quantidade { get; set; }
        public string Descricao { get; set; }
        public float? Largura { get; set; }
        public float? Altura { get; set; }
        public float? Profundidade { get; set; }
        public float? Diametro { get; set; }
    }
}
