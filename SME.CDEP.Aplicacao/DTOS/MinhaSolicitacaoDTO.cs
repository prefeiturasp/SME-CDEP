
namespace SME.CDEP.Aplicacao.DTOS;

public class MinhaSolicitacaoDTO  
{
    public string TipoAcervo { get; set; }
    public string Titulo { get; set; }
    public long AcervoSolicitacaoId { get; set; }
    public string DataCriacao { get; set; }
    public string DataVisita { get; set; }
    public string Situacao { get; set; }
}