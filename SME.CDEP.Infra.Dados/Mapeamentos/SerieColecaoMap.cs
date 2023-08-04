using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class SerieColecaoMap : BaseMapAuditavel<SerieColecao>
    {
        public SerieColecaoMap()
        {
            ToTable("serie_colecao");
            Map(c => c.Nome).ToColumn("nome");
        }
    }
}