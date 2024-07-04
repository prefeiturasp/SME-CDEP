using Newtonsoft.Json;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades;

public class ImportacaoArquivoCompleto: EntidadeBaseAuditavel
{
    public string Nome { get; set; }
    public TipoAcervo TipoAcervo { get; set; }
    public ImportacaoStatus StatusArquivo { get; set; }
    public string Conteudo { get; set; }
}