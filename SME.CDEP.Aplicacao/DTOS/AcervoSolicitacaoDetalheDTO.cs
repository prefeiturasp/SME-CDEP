using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class AcervoSolicitacaoDetalheDTO
    {
        public DadosSolicitanteDTO DadosSolicitante { get; set; }
        
        public long Id { get; set; }
        public long UsuarioId { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public string Situacao { get; set; }
        public SituacaoSolicitacao SituacaoId { get; set; }
        public IEnumerable<AcervoSolicitacaoItemDetalheResumidoDTO> Itens { get; set; } = Enumerable.Empty<AcervoSolicitacaoItemDetalheResumidoDTO>();
    }
}
