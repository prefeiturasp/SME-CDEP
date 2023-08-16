using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AutorMap : BaseMapAuditavel<Autor>
    {
        public AutorMap()
        {
            ToTable("autor");
            Map(c => c.Nome).ToColumn("nome");
        }
    }
}