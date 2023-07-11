using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class IdiomaMap : BaseMap<Idioma>
    {
        public IdiomaMap()
        {
            ToTable("idioma");
            Map(c => c.Nome).ToColumn("nome");
        }
    }
}