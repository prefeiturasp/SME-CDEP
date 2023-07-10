using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class TipoAnexoMap : BaseMap<TipoAnexo>
    {
        public TipoAnexoMap()
        {
            ToTable("tipo_anexo");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}