namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoDocumental : EntidadeBase
    {
        public Acervo Acervo { get; set; }
        public long AcervoId { get; set; }
        public long? MaterialId { get; set; }
        public long IdiomaId { get; set; }
        public string Ano { get; set; }
        public string NumeroPagina { get; set; }
        public string Volume { get; set; }
        public string TipoAnexo { get; set; }
        public double? Largura { get; set; }
        public double? Altura { get; set; }
        public string TamanhoArquivo { get; set; }
        public string Localizacao { get; set; }
        public bool CopiaDigital { get; set; }
        public long? ConservacaoId { get; set; }
    }
}
