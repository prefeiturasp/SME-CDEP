namespace SME.CDEP.Aplicacao.DTOS
{
    public class PaginacaoAcervoAuditavel
    {
        public PaginacaoAcervoAuditavel(int pagina, int quantidadeRegistros, int ordenacao, string? direcaoOrdenacao = "ASC")
        {
            pagina = pagina < 1 ? 1 : pagina;
            quantidadeRegistros = quantidadeRegistros < 1 ? 0 : quantidadeRegistros;

            Pagina = pagina;
            QuantidadeRegistros = quantidadeRegistros;
            QuantidadeRegistrosIgnorados = (pagina - 1) * quantidadeRegistros;
            Ordenacao = ordenacao;
            DirecaoOrdenacao = direcaoOrdenacao?.ToUpper() ?? "ASC";
        }

        public int Pagina { get; set; }
        public int QuantidadeRegistros { get; private set; }
        public int QuantidadeRegistrosIgnorados { get; private set; }
        public int Ordenacao { get; private set; }
        public string DirecaoOrdenacao { get; private set; }
    }
}