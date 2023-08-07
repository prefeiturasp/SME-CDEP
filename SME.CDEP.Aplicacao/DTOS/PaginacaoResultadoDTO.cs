using System.Collections.Generic;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class PaginacaoResultadoDTO<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalRegistros { get; set; }
    }
}