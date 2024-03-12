namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoDetalheDTO
{
    public string Titulo { get; set; }
    public string Codigo { get; set; }
    public string Ano { get; set; }
    public long AcervoId { get; set; }
    public string EnderecoImagemPadrao { get; set; }
    public string SituacaoDisponibilidade { get; set; }
    public bool EstaDisponivel { get; set; }
    public bool TemControleDisponibilidade { get; set; }
}