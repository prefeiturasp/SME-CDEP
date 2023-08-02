namespace SME.CDEP.Dominio.Entidades
{
    public class Editora : EntidadeBaseAuditavel
    {
        public string Nome { get; set; }
        public bool Excluido { get; set; }
    }
}
