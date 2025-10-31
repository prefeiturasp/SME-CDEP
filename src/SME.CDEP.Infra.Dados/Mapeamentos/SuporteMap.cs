using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class SuporteMap : BaseMap<Suporte>
    {
        public SuporteMap()
        {
            ToTable("suporte");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Tipo).ToColumn("tipo");
        }
    }
}