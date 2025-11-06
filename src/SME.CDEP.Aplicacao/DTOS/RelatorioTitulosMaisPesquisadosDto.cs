namespace SME.CDEP.Aplicacao.DTOS
{
    public class RelatorioTitulosMaisPesquisadosDto : RelatorioTitulosMaisPesquisadosRequest
    {
        public string Usuario { get; set; } = null!;
        public string UsuarioRF { get; set; } = null!;
    }
}
