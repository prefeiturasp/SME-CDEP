namespace SME.CDEP.Aplicacao.DTOS
{
    public class RelatorioControleDevolucaoLivrosRequest
    {
        public List<int>? Solicitante { get; set; }
        public bool SomenteEmAtraso { get; set; }
    }
}
