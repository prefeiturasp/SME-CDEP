using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class FiltroDetalharAcervoDTO
{
    public string Codigo { get; set; }
    public TipoAcervo Tipo { get; set; }
}
