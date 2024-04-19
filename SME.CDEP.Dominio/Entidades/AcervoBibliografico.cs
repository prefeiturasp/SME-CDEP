using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoBibliografico : EntidadeBase
    {
        public Acervo Acervo { get; set; }
        public long AcervoId { get; set; }
        public long MaterialId { get; set; }
        public long? EditoraId { get; set; }
        public string? Edicao { get; set; }
        public int? NumeroPagina { get; set; }
        public string? Largura { get; set; }
        public string? Altura { get; set; }
        public long? SerieColecaoId { get; set; }
        public string? Volume { get; set; }
        public long IdiomaId { get; set; }
        public string LocalizacaoCDD { get; set; }
        public string? LocalizacaoPHA { get; set; }
        public string? NotasGerais { get; set; }
        public string? Isbn { get; set; }
        public SituacaoSaldo SituacaoSaldo { get; set; }

        public void DefinirSituacaoSaldo(SituacaoSaldo situacaoSaldo)
        {
            SituacaoSaldo = situacaoSaldo;
        }
    }
}
