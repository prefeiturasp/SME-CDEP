using Dapper.FluentMap.Dommel.Mapping;
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcervoDocumentalAcessoDocumentoMap : DommelEntityMap<AcervoDocumentalAcessoDocumento>
    {
        public AcervoDocumentalAcessoDocumentoMap()
        {
            ToTable("acervo_documental_acesso_documento");
            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.AcessoDocumentalId).ToColumn("acervo_documental_id");
            Map(c => c.AcessoDocumentoId).ToColumn("acesso_documento_id");
        }
    }
}