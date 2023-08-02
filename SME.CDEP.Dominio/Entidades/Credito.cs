namespace SME.CDEP.Dominio.Entidades
{
    public class Credito : EntidadeBaseAuditavel
    {
        public string Nome { get; set; }
        public bool Excluido { get; set; }
    }
}
