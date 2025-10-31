namespace SME.CDEP.Aplicacao.DTOS
{
    public class RelatorioControleAcervoAutorDTO : RelatorioControleAcervoAutorRequest
    {
        public string Usuario { get; set; }
        public string UsuarioRF { get; set; }
        public long[] TiposAcervosPermitidos { get; set; }
    }
}
