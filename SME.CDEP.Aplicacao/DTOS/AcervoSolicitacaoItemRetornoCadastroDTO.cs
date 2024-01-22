using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoSolicitacaoItemRetornoCadastroDTO : AcervoSolicitacaoItemRetornoDTO 
{
    public SituacaoSolicitacaoItem Situacao { get; set; }
    public IEnumerable<ArquivoCodigoNomeAcervoIdDTO> Arquivos { get; set; }
}