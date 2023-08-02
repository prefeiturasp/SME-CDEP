using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AssuntoMap : BaseMapAuditavel<Assunto>
    {
        public AssuntoMap()
        {
            ToTable("assunto");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}