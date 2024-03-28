using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoTipoTituloAcervoIdCreditosAutoresDTO 
{
    public string TipoAcervo { get; set; }
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public string SituacaoDisponibilidade { get; set; }
    public bool  EstaDisponivel { get; set; }
    public bool TemControleDisponibilidade { get; set; }
    public string[] AutoresCreditos { get; set; }
    public TipoAcervo TipoAcervoId { get; set; }
}