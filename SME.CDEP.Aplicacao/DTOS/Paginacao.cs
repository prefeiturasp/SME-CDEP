using SME.CDEP.Aplicacao.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class Paginacao
    {
        public Paginacao(int pagina, int registros, int ordenacao)
        {
            pagina = pagina < 1 ? 1 : pagina;
            registros = registros < 1 ? 0 : registros;

            QuantidadeRegistros = registros;
            QuantidadeRegistrosIgnorados = (pagina - 1) * registros;
            Ordenacao = (TipoOrdenacao)ordenacao;
        }

        public int QuantidadeRegistros { get; private set; }
        public int QuantidadeRegistrosIgnorados { get; private set; }
        public TipoOrdenacao Ordenacao { get; private set; }
    }
}