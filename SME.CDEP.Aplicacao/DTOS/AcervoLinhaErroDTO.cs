using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoLinhaErroDTO<T,U>
{
    public int NumeroLinha { get; set; }
    public string Titulo { get; set; }
    public string Tombo { get; set; }
    public T RetornoObjeto { get; set; }
    public U RetornoErro { get; set; }
}