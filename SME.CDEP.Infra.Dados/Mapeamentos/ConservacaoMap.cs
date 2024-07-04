using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class ConservacaoMap : BaseMap<Conservacao>
    {
        public ConservacaoMap()
        {
            ToTable("conservacao");
            Map(c => c.Nome).ToColumn("nome");
        }
    }
}