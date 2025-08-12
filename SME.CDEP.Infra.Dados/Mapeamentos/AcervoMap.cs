using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    public class AcervoMap : BaseMapAuditavel<Acervo>
    {
        public AcervoMap()
        {
            ToTable("acervo");
            Map(c => c.Titulo).ToColumn("titulo");
            Map(c => c.TipoAcervoId).ToColumn("tipo");
            Map(c => c.Codigo).ToColumn("codigo");
            Map(c => c.CodigoNovo).ToColumn("codigo_novo");
            Map(c => c.SubTitulo).ToColumn("subtitulo");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.DataAcervo).ToColumn("data_acervo");
            Map(c => c.Ano).ToColumn("ano");
            Map(c => c.AnoInicio).ToColumn("ano_inicio");
            Map(c => c.AnoFim).ToColumn("ano_fim");
            Map(c => c.CapaDocumento).Ignore();
        }
    }
}