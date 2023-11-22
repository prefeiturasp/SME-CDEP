using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoTridimensionalLinhaRetornoDTO : AcervoLinhaRetornoDTO
{
    public LinhaConteudoAjustarRetornoDTO Titulo { get; set; }
    public LinhaConteudoAjustarRetornoDTO Codigo { get; set; }
    public LinhaConteudoAjustarRetornoDTO Procedencia { get; set; }
    public LinhaConteudoAjustarRetornoDTO DataAcervo { get; set; }
    public LinhaConteudoAjustarRetornoDTO ConservacaoId { get; set; }
    public LinhaConteudoAjustarRetornoDTO Quantidade { get; set; }
    public LinhaConteudoAjustarRetornoDTO Descricao { get; set; }
    public LinhaConteudoAjustarRetornoDTO Largura { get; set; }
    public LinhaConteudoAjustarRetornoDTO Altura { get; set; }
    public LinhaConteudoAjustarRetornoDTO Profundidade { get; set; }
    public LinhaConteudoAjustarRetornoDTO Diametro { get; set; }
}