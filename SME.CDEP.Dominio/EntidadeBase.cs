namespace SME.CDEP.Dominio;

public class EntidadeBase<TChave> where TChave : struct
{
    public TChave Id { get; set; }
}
