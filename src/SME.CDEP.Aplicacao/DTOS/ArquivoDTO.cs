using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class ArquivoDTO : BaseAuditavelDTO
{
    public string Nome { get; set; }
    public Guid Codigo { get; set; }
    public string TipoConteudo { get; set; }
    public TipoArquivo Tipo { get; set; }
}