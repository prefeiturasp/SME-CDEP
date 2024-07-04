using Dapper.FluentMap.Dommel.Mapping;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcervoDocumentalArquivoMap : DommelEntityMap<AcervoDocumentalArquivo>
    {
        public AcervoDocumentalArquivoMap()
        {
            ToTable("acervo_documental_arquivo");
            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.AcervoDocumentalId).ToColumn("acervo_documental_id");
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
        }
    }
}