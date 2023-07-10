using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcessoDocumentoMap : BaseSemAuditoriaMap<AcessoDocumento>
    {
        public AcessoDocumentoMap()
        {
            ToTable("acesso_documento");
            Map(c => c.Nome).ToColumn("nome");
        }
    }
}