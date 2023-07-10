using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class MaterialDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public bool Excluido { get; set; }
        public int TipoMaterial { get; set; }
    }
}
