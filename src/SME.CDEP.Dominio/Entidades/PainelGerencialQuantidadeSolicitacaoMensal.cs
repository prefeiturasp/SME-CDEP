namespace SME.CDEP.Dominio.Entidades
{
    public class PainelGerencialQuantidadeSolicitacaoMensal
    {
        public DateOnly MesReferencia { get; set; }
        public int TotalSolicitacoes { get; set; }
        public int TotalAutomatica { get; set; }
        public int TotalManual { get; set; }
    }
}