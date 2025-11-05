namespace SME.CDEP.Aplicacao.DTOS
{
    public class RelatorioControleAcervoDTO : RelatorioControleAcervoRequest
    {
        public string Usuario { get; set; }
        public string UsuarioRF { get; set; }
        public long[] TiposAcervosPermitidos { get; set; }
    }
}
