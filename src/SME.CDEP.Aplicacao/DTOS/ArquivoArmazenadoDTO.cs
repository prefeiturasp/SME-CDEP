using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class ArquivoArmazenadoDTO
{
    public ArquivoArmazenadoDTO(long id, Guid codigo, string path)
    {
        Id = id;
        Codigo = codigo;
        Path = path;
    }
    
    public ArquivoArmazenadoDTO(string path, Guid codigo, string nome, string contentType, TipoArquivo tipoArquivo)
    {
        Codigo = codigo;
        Path = path;
        Nome = nome;
        ContentType = contentType;
        TipoArquivo = tipoArquivo;
    }

    public long Id { get; set; }
    public Guid Codigo { get; set; }
    public string Path { get; set; }
    public string Nome { get; set; }
    public string ContentType { get; set; }
    public TipoArquivo TipoArquivo { get; set; }
}