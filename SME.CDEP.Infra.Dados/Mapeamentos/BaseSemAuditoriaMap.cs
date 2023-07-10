using Dapper.FluentMap.Dommel.Mapping;
using SME.CDEP.Dominio;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class BaseSemAuditoriaMap<T> : DommelEntityMap<T> where T : EntidadeBase
    {
        public BaseSemAuditoriaMap()
        {
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
