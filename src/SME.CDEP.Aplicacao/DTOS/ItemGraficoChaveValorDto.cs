namespace SME.CDEP.Aplicacao.DTOS
{
    public class ItemGraficoChaveValorDto<TKey>
    {
        public required TKey Id { get; set; }
        public required string Nome { get; set; }
        public required long Valor { get; set; }
    }
}