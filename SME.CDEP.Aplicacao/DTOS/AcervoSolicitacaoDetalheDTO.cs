namespace SME.CDEP.Aplicacao.DTOS
{
    public class AcervoSolicitacaoDetalheDTO
    {
        public DadosSolicitanteDTO DadosSolicitante { get; set; }
        
        public long Id { get; set; }
        public long UsuarioId { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public string ResponsavelRf { get; set; }
        public string Situacao { get; set; }
        public IEnumerable<AcervoSolicitacaoItemDetalheResumidoDTO> Itens { get; set; } = Enumerable.Empty<AcervoSolicitacaoItemDetalheResumidoDTO>();
    }
}
