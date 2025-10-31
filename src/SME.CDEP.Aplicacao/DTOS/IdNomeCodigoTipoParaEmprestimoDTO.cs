namespace SME.CDEP.Aplicacao.DTOS;

public class IdNomeCodigoTipoParaEmprestimoDTO
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Codigo { get; set; }
    public int Tipo { get; set; }
    public string SituacaoDisponibilidade { get; set; }
    public bool EstaDisponivel { get; set; }
    public bool TemControleDisponibilidade { get; set; }
}