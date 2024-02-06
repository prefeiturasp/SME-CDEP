namespace SME.CDEP.Aplicacao.DTOS
{
    public class AcervoSolicitacaoItemDetalheResumidoDTO
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public string TipoAcervo { get; set; }
        public string Titulo { get; set; }
        public string Situacao { get; set; }
        public DateTime? DataVisita { get; set; }
    }
}
