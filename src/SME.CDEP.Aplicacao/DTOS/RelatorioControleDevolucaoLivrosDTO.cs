namespace SME.CDEP.Aplicacao.DTOS
{
    public class RelatorioControleDevolucaoLivrosDTO : RelatorioControleDevolucaoLivrosRequest
    {
        public string Usuario { get; set; }
        public string UsuarioRF { get; set; }
        public long[] TiposAcervosPermitidos { get; set; }
    }
}
