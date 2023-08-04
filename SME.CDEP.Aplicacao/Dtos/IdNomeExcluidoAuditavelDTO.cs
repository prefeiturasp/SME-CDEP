using SME.CDEP.Dominio.Constantes;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class IdNomeExcluidoAuditavelDTO : BaseDTO
    {
        public string Nome { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public string? AlteradoPor { get; set; }
        public string? AlteradoLogin { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoLogin { get; set; }
    }
}
