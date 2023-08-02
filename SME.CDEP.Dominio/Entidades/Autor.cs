namespace SME.CDEP.Dominio.Entidades
{
    public class Autor : EntidadeBaseAuditavel
    {
        public string Nome { get; set; }
        public bool Excluido { get; set; }
    }
}
