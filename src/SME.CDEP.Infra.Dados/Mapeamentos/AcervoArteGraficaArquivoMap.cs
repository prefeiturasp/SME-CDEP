using Dapper.FluentMap.Dommel.Mapping;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcervoArteGraficaArquivoMap : DommelEntityMap<AcervoArteGraficaArquivo>
    {
        public AcervoArteGraficaArquivoMap()
        {
            ToTable("acervo_arte_grafica_arquivo");
            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.AcervoArteGraficaId).ToColumn("acervo_arte_grafica_id");
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
            Map(c => c.ArquivoMiniaturaId).ToColumn("arquivo_miniatura_id");
        }
    }
}