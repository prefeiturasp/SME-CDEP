namespace SME.CDEP.Dominio.Entidades
{
    public class Assunto : EntidadeBaseAuditavel
    {
        public string Nome { get; set; }
        public bool Excluido { get; set; }
    }
}
