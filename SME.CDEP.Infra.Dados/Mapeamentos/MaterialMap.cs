using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class MaterialMap : BaseSemAuditoriaMap<Material>
    {
        public MaterialMap()
        {
            ToTable("material");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.TipoMaterial).ToColumn("tipo_material");
        }
    }
}