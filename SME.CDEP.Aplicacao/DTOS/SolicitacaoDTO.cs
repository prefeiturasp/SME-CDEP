
namespace SME.CDEP.Aplicacao.DTOS;

public class SolicitacaoDTO : MinhaSolicitacaoDTO  
{
    public string Solicitante { get; set; }
    public string Responsavel { get; set; }
    public string TipoAcervo { get; set; }
    public string SituacaoEmprestimo { get; set; }
}