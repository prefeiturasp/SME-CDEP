using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class ImportacaoArquivoRetornoDTO<T> //where T : LinhaDTO
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public TipoAcervo TipoAcervo { get; set; }
    public DateTime DataImportacao { get; set; }
    public IEnumerable<T> Erros { get; set; } 
    public IEnumerable<T> Sucesso { get; set; } 
}