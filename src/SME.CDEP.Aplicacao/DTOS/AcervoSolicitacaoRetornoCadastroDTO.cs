namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoSolicitacaoRetornoCadastroDTO  
{
    public bool PodeCancelarSolicitacao { get; set; }
    public IEnumerable<AcervoSolicitacaoItemRetornoCadastroDTO> Itens { get; set; }
}