namespace SME.CDEP.Aplicacao.DTOS;

public class MinhaSolicitacaoDTO  
{
    public long Id { get; set; }
    public long AcervoSolicitacaoId { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataVisita { get; set; }
    public string Situacao { get; set; }
}