using Dapper.FluentMap.Dommel.Mapping;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcervoBibliograficoAssuntoMap : DommelEntityMap<AcervoBibliograficoAssunto>
    {
        public AcervoBibliograficoAssuntoMap()
        {
            ToTable("acervo_bibliografico_assunto");
            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.AcervoBibliograficoId).ToColumn("acervo_bibliografico_id");
            Map(c => c.AssuntolId).ToColumn("assunto_id");
        }
    }
}