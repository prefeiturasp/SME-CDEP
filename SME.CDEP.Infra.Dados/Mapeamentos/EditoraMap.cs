using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class EditoraMap : BaseMapAuditavel<Editora>
    {
        public EditoraMap()
        {
            ToTable("editora");
            Map(c => c.Nome).ToColumn("nome");
        }
    }
}