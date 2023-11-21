using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoArteGraficaLinhaRetornoDTO : AcervoLinhaRetornoDTO
{
    public LinhaConteudoAjustarRetornoDTO Titulo { get; set; }
    public LinhaConteudoAjustarRetornoDTO Codigo { get; set; }
    public LinhaConteudoAjustarRetornoDTO Credito { get; set; }
    public LinhaConteudoAjustarRetornoDTO Localizacao { get; set; }
    public LinhaConteudoAjustarRetornoDTO Procedencia { get; set; }
    public LinhaConteudoAjustarRetornoDTO Data { get; set; }
    public LinhaConteudoAjustarRetornoDTO CopiaDigital { get; set; }
    public LinhaConteudoAjustarRetornoDTO PermiteUsoImagem { get; set; }
    public LinhaConteudoAjustarRetornoDTO EstadoConservacao { get; set; }
    public LinhaConteudoAjustarRetornoDTO Cromia { get; set; }
    public LinhaConteudoAjustarRetornoDTO Largura { get; set; }
    public LinhaConteudoAjustarRetornoDTO Altura { get; set; }
    public LinhaConteudoAjustarRetornoDTO Diametro { get; set; }
    public LinhaConteudoAjustarRetornoDTO Tecnica { get; set; }
    public LinhaConteudoAjustarRetornoDTO Suporte { get; set; }
    public LinhaConteudoAjustarRetornoDTO Quantidade { get; set; }
    public LinhaConteudoAjustarRetornoDTO Descricao { get; set; }
}