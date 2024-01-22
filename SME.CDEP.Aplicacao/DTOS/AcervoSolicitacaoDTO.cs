using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class AcervoSolicitacaoDTO
    {
        public long Id { get; set; }
        public long UsuarioId { get; set; }
        public SituacaoSolicitacao Situacao { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoLogin { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoLogin { get; set; }
        public bool Excluido { get; set; }
        public IEnumerable<AcervoSolicitacaoItemDTO> Itens { get; set; }
    }
}
