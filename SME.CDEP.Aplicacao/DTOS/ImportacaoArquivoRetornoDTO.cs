using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class ImportacaoArquivoRetornoDTO<T,U>
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public TipoAcervo TipoAcervo { get; set; }
    public DateTime? DataImportacao { get; set; }
    public IEnumerable<T> Erros { get; set; } 
    public IEnumerable<U> Sucesso { get; set; }
    public ImportacaoStatus Status { get; set; }
}