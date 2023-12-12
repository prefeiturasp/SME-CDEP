using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class IdTipoTituloCreditoAutoriaCodigoAcervoDTO
{
    public long AcervoId { get; set; }
    public string TipoAcervo { get; set; }
    public TipoAcervo TipoAcervoId { get; set; }
    public string Titulo { get; set; }
    public string CreditoAutoria { get; set; }
    public string Codigo { get; set; }
    public string Data { get; set; }
}