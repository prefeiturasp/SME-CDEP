namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoFotografico : EntidadeBase
    {
        public Acervo Acervo { get; set; }
        public long AcervoId { get; set; }
        public string Localizacao { get; set; }
        public string Procedencia { get; set; }
        public bool? CopiaDigital { get; set; }
        public bool? PermiteUsoImagem { get; set; }
        public long ConservacaoId { get; set; }
        public long Quantidade { get; set; }
        public double? Largura { get; set; }
        public double? Altura { get; set; }
        public long SuporteId { get; set; }
        public long FormatoId { get; set; }
        public long CromiaId { get; set; }
        public string Resolucao { get; set; }
        public string TamanhoArquivo { get; set; }
    }
}
