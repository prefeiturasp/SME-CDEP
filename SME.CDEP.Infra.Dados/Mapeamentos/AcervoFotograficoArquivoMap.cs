using Dapper.FluentMap.Dommel.Mapping;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcervoFotograficoArquivoMap : DommelEntityMap<AcervoFotograficoArquivo>
    {
        public AcervoFotograficoArquivoMap()
        {
            ToTable("acervo_fotografico_arquivo");
            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.AcervoFotograficoId).ToColumn("acervo_fotografico_id");
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
            Map(c => c.ArquivoMiniaturaId).ToColumn("arquivo_miniatura_id");
        }
    }
}