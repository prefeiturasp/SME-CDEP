namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoDTO : BaseAuditavelDTO
{
    public string Titulo { get; set; }
    public long TipoAcervoId { get; set; }
    public string Codigo { get; set; }
    public long CreditoAutorId { get; set; }
}