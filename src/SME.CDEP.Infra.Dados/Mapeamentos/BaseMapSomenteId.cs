using Dapper.FluentMap.Dommel.Mapping;
using SME.CDEP.Dominio;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class BaseMapSomenteId<T> : DommelEntityMap<T> where T : EntidadeBaseSomenteId
    {
        public BaseMapSomenteId()
        {
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
        }
    }
}
