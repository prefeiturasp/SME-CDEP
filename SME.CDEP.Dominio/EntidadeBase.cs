namespace SME.CDEP.Dominio;

public abstract class EntidadeBase
{
    public long Id { get; set; }
    public DateTime? AlteradoEm { get; set; }
    public string AlteradoPor { get; set; }
    public string AlteradoLogin { get; set; }
    public DateTime CriadoEm { get; set; }
    public string CriadoPor { get; set; }
    public string CriadoLogin { get; set; }
    public bool Excluido { get; set; }
}
