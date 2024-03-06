namespace SME.CDEP.Aplicacao.DTOS;

public class ConfirmarAtendimentoDTO
{
    public long Id { get; set; }
    public IEnumerable<long> Itens { get; set; } = Enumerable.Empty<long>();
}