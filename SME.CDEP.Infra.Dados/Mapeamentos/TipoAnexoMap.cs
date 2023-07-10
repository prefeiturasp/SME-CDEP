using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class TipoAnexoMap : BaseSemAuditoriaMap<TipoAnexo>
    {
        public TipoAnexoMap()
        {
            ToTable("tipo_anexo");
            Map(c => c.Nome).ToColumn("nome");
        }
    }
}