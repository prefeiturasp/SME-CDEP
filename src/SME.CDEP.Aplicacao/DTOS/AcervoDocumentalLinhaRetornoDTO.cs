using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoDocumentalLinhaRetornoDTO : AcervoLinhaRetornoDTO
{
    public LinhaConteudoAjustarRetornoDTO Titulo { get; set; }
    public LinhaConteudoAjustarRetornoDTO Codigo { get; set; }
    public LinhaConteudoAjustarRetornoDTO CodigoNovo { get; set; }
    public LinhaConteudoAjustarRetornoDTO MaterialId { get; set; }
    public LinhaConteudoAjustarRetornoDTO IdiomaId { get; set; }
    public LinhaConteudoAjustarRetornoDTO CreditosAutoresIds { get; set; }
    public LinhaConteudoAjustarRetornoDTO Ano { get; set; }
    public LinhaConteudoAjustarRetornoDTO NumeroPagina { get; set; }
    public LinhaConteudoAjustarRetornoDTO Volume { get; set; }
    public LinhaConteudoAjustarRetornoDTO Descricao { get; set; }
    public LinhaConteudoAjustarRetornoDTO TipoAnexo { get; set; }
    public LinhaConteudoAjustarRetornoDTO Altura { get; set; }
    public LinhaConteudoAjustarRetornoDTO Largura { get; set; }
    public LinhaConteudoAjustarRetornoDTO TamanhoArquivo { get; set; }
    public LinhaConteudoAjustarRetornoDTO AcessoDocumentosIds { get; set; }
    public LinhaConteudoAjustarRetornoDTO Localizacao { get; set; }
    public LinhaConteudoAjustarRetornoDTO CopiaDigital { get; set; }
    public LinhaConteudoAjustarRetornoDTO ConservacaoId { get; set; }
}