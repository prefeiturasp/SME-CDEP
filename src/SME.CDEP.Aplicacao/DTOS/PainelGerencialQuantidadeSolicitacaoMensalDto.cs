namespace SME.CDEP.Aplicacao.DTOS
{
    public class PainelGerencialQuantidadeSolicitacaoMensalDto : ItemGraficoChaveValorDto<int>
    {
        public int TotalAutomatica { get; set; }
        public int TotalManual { get; set; }
    }
}