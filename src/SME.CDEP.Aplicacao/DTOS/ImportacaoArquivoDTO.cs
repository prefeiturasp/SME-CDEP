using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class ImportacaoArquivoDTO : BaseAuditavelDTO 
{
    public string Nome { get; set; }
    public TipoAcervo TipoAcervo { get; set; }
    public ImportacaoStatus Status { get; set; }
    public string Conteudo { get; set; }
}