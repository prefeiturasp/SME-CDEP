using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class FiltroTipoTituloCreditoAutoriaCodigoAcervoDTO
{
    public int? TipoAcervo { get; set; }
    public string? Titulo { get; set; }
    public long? CreditoAutorId { get; set; }
    public string? Codigo { get; set; }
}
