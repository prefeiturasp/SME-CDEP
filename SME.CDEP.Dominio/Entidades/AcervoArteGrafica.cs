namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoArteGrafica : EntidadeBase
    {
        public Acervo Acervo { get; set; }
        public long AcervoId { get; set; }
        public string Localizacao { get; set; }
        public string Procedencia { get; set; }
        public string DataAcervo { get; set; }
        public bool CopiaDigital { get; set; }
        public bool PermiteUsoImagem { get; set; }
        public long ConservacaoId { get; set; }
        public long CromiaId { get; set; }
        public float? Largura { get; set; }
        public float? Altura { get; set; }
        public float? Diametro { get; set; }
        public string Tecnica { get; set; }
        public long SuporteId { get; set; }
        public long Quantidade { get; set; }
    }
}
