using SME.CDEP.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoDTO : BaseAuditavelDTO
{
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public long TipoAcervoId { get; set; }
    public string Codigo { get; set; }
    public long[] CreditosAutoresIds { get; set; }
    public CreditoAutorDTO CreditoAutor { get; set; }
    public string CodigoNovo { get; set; }
    public string SubTitulo { get; set; }
    public IEnumerable<CoAutorDTO> CoAutores { get; set; }
    public string DataAcervo { get; set; }
    public string Ano { get; set; }
    public SituacaoAcervo? SituacaoAcervo { get; set; }
}