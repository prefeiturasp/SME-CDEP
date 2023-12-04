using Dapper.FluentMap.Dommel.Mapping;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcervoCreditoAutorMap : DommelEntityMap<AcervoCreditoAutor>
    {
        public AcervoCreditoAutorMap()
        {
            ToTable("acervo_credito_autor");
            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.AcervoId).ToColumn("acervo_id");
            Map(c => c.CreditoAutorId).ToColumn("credito_autor_id");
            Map(c => c.TipoAutoria).ToColumn("tipo_autoria");
            Map(c => c.EhCoAutor).ToColumn("ehcoautor");
        }
    }
}