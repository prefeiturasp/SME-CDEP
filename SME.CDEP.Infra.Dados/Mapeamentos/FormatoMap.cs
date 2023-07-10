using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class FormatoMap : BaseMap<Formato>
    {
        public FormatoMap()
        {
            ToTable("formato");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}