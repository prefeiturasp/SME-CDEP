using SME.CDEP.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Dtos
{
    public class PaginacaoDto
    {
        public int Pagina { get; set; }
        public int QuantidadeRegistros { get; set; }
        public TipoOrdenacaoDto OrdenacaoDto { get; set; }
        public DirecaoOrdenacaoDto DirecaoOrdenacaoDto { get; set; }
    }
}
