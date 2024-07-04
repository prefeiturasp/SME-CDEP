namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoArteGraficaArquivo : EntidadeBaseSomenteId
    {
        public long AcervoArteGraficaId { get; set; }
        public long ArquivoId { get; set; }
        public long? ArquivoMiniaturaId { get; set; }
    }
}