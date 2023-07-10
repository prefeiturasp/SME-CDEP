using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class CromiaMap : BaseSemAuditoriaMap<Cromia>
    {
        public CromiaMap()
        {
            ToTable("cromia");
            Map(c => c.Nome).ToColumn("nome");
        }
    }
}