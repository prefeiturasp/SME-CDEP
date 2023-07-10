using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcessoDocumentoMap : BaseMap<AcessoDocumento>
    {
        public AcessoDocumentoMap()
        {
            ToTable("acesso_documento");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}