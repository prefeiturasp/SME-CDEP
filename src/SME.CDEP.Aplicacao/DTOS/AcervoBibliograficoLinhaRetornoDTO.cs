using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoBibliograficoLinhaRetornoDTO : AcervoLinhaRetornoDTO
{
    public LinhaConteudoAjustarRetornoDTO Titulo { get; set; }
    public LinhaConteudoAjustarRetornoDTO SubTitulo { get; set; }
    public LinhaConteudoAjustarRetornoDTO MaterialId { get; set; }
    public LinhaConteudoAjustarRetornoDTO CreditosAutoresIds { get; set; }
    public LinhaConteudoAjustarRetornoDTO CoAutores { get; set; }
    public LinhaConteudoAjustarRetornoDTO TipoAutoria { get; set; }
    public LinhaConteudoAjustarRetornoDTO EditoraId { get; set; }
    public LinhaConteudoAjustarRetornoDTO AssuntosIds { get; set; }
    public LinhaConteudoAjustarRetornoDTO Ano { get; set; }
    public LinhaConteudoAjustarRetornoDTO Edicao { get; set; }
    public LinhaConteudoAjustarRetornoDTO NumeroPagina { get; set; }
    public LinhaConteudoAjustarRetornoDTO Altura { get; set; }
    public LinhaConteudoAjustarRetornoDTO Largura { get; set; }
    public LinhaConteudoAjustarRetornoDTO SerieColecaoId { get; set; }
    public LinhaConteudoAjustarRetornoDTO Volume { get; set; }
    public LinhaConteudoAjustarRetornoDTO IdiomaId { get; set; }
    public LinhaConteudoAjustarRetornoDTO LocalizacaoCDD { get; set; }
    public LinhaConteudoAjustarRetornoDTO LocalizacaoPHA { get; set; }
    public LinhaConteudoAjustarRetornoDTO NotasGerais { get; set; }
    public LinhaConteudoAjustarRetornoDTO Isbn { get; set; }
    public LinhaConteudoAjustarRetornoDTO Codigo { get; set; }
}