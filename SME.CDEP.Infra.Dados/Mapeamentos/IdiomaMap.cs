using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class IdiomaMap : BaseSemAuditoriaMap<Idioma>
    {
        public IdiomaMap()
        {
            ToTable("idioma");
            Map(c => c.Nome).ToColumn("nome");
        }
    }
}