using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoBibliograficoLinhaRetornoDTO : LinhaDTO
{
    public LinhaConteudoAjustarRetornoDTO Titulo { get; set; }
    public LinhaConteudoAjustarRetornoDTO SubTitulo { get; set; }
    public LinhaConteudoAjustarRetornoDTO Material { get; set; }
    public LinhaConteudoAjustarRetornoDTO Autor { get; set; }
    public LinhaConteudoAjustarRetornoDTO CoAutor { get; set; }
    public LinhaConteudoAjustarRetornoDTO TipoAutoria { get; set; }
    public LinhaConteudoAjustarRetornoDTO Editora { get; set; }
    public LinhaConteudoAjustarRetornoDTO Assunto { get; set; }
    public LinhaConteudoAjustarRetornoDTO Ano { get; set; }
    public LinhaConteudoAjustarRetornoDTO Edicao { get; set; }
    public LinhaConteudoAjustarRetornoDTO NumeroPaginas { get; set; }
    public LinhaConteudoAjustarRetornoDTO Altura { get; set; }
    public LinhaConteudoAjustarRetornoDTO Largura { get; set; }
    public LinhaConteudoAjustarRetornoDTO SerieColecao { get; set; }
    public LinhaConteudoAjustarRetornoDTO Volume { get; set; }
    public LinhaConteudoAjustarRetornoDTO Idioma { get; set; }
    public LinhaConteudoAjustarRetornoDTO LocalizacaoCDD { get; set; }
    public LinhaConteudoAjustarRetornoDTO LocalizacaoPHA { get; set; }
    public LinhaConteudoAjustarRetornoDTO NotasGerais { get; set; }
    public LinhaConteudoAjustarRetornoDTO Isbn { get; set; }
    public LinhaConteudoAjustarRetornoDTO Tombo { get; set; }
    public ImportacaoStatus Status { get; set; }
    public int NumeroLinha { get; set; } = 1;
}