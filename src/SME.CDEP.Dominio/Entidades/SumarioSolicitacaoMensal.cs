namespace SME.CDEP.Dominio.Entidades
{
    public class SumarioSolicitacaoMensal
    {
        public DateOnly MesReferencia { get; set; }
        public int TotalSolicitacoes { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
    }
}