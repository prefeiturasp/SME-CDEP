using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcervoTridimensionalMap : BaseMapSomenteId<AcervoTridimensional>
    {
        public AcervoTridimensionalMap()
        {
            ToTable("acervo_tridimensional");
            Map(c => c.AcervoId).ToColumn("acervo_id");
            Map(c => c.Procedencia).ToColumn("procedencia");
            Map(c => c.DataAcervo).ToColumn("data_acervo");
            Map(c => c.ConservacaoId).ToColumn("conservacao_id");
            Map(c => c.Quantidade).ToColumn("quantidade");
            Map(c => c.Largura).ToColumn("largura");
            Map(c => c.Altura).ToColumn("altura");
            Map(c => c.Profundidade).ToColumn("profundidade");
            Map(c => c.Diametro).ToColumn("diametro");
            Map(c => c.Excluido).Ignore();
        }
    }
}