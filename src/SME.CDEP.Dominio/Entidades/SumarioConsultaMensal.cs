namespace SME.CDEP.Dominio.Entidades
{
    public class SumarioConsultaMensal
    {
        public DateOnly MesReferencia { get; set; }
        public long TotalConsultas { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
    }
}