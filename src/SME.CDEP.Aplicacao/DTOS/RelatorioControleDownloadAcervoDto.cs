namespace SME.CDEP.Aplicacao.DTOS
{
    public class RelatorioControleDownloadAcervoDto : RelatorioControleDownloadAcervoRequest
    {
        public string Usuario { get; set; } = null!;
        public string UsuarioRF { get; set; } = null!;
    }
}
