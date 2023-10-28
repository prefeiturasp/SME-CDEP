using Dapper.FluentMap.Dommel.Mapping;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcervoTridimensionalArquivoMap : DommelEntityMap<AcervoTridimensionalArquivo>
    {
        public AcervoTridimensionalArquivoMap()
        {
            ToTable("acervo_tridimensional_arquivo");
            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.AcervoTridimensionalId).ToColumn("acervo_tridimensional_id");
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
        }
    }
}