using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoTridimensionalLinhaRetornoDTO : AcervoLinhaDTO
{
    public LinhaConteudoAjustarRetornoDTO Titulo { get; set; }
    public LinhaConteudoAjustarRetornoDTO Tombo { get; set; }
    public LinhaConteudoAjustarRetornoDTO Procedencia { get; set; }
    public LinhaConteudoAjustarRetornoDTO Data { get; set; }
    public LinhaConteudoAjustarRetornoDTO EstadoConservacao { get; set; }
    public LinhaConteudoAjustarRetornoDTO Quantidade { get; set; }
    public LinhaConteudoAjustarRetornoDTO Descricao { get; set; }
    public LinhaConteudoAjustarRetornoDTO Largura { get; set; }
    public LinhaConteudoAjustarRetornoDTO Altura { get; set; }
    public LinhaConteudoAjustarRetornoDTO Profundidade { get; set; }
    public LinhaConteudoAjustarRetornoDTO Diametro { get; set; }
}