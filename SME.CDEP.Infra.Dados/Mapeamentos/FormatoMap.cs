using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class FormatoMap : BaseSemAuditoriaMap<Formato>
    {
        public FormatoMap()
        {
            ToTable("formato");
            Map(c => c.Nome).ToColumn("nome");
        }
    }
}