namespace SME.CDEP.Dominio;

public abstract class EntidadeBaseSemAuditoria
{
    public long Id { get; set; }
    public bool Excluido { get; set; }
}
