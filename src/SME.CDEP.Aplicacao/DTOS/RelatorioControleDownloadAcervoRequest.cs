using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class RelatorioControleDownloadAcervoRequest
    {
        public string? Titulo { get; set; }
        public TipoAcervo? TipoAcervo { get; set; }
    }
}
