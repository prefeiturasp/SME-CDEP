using Dapper.FluentMap.Dommel.Mapping;
using SME.CDEP.Dominio.Entidades;
using System.Diagnostics.CodeAnalysis;

namespace SME.CDEP.Infra.Dados.Mapeamentos
{
    [ExcludeFromCodeCoverage]
    public class HistoricoConsultaAcervoMap : DommelEntityMap<HistoricoConsultaAcervo>
    {
        public HistoricoConsultaAcervoMap()
        {
            ToTable("historico_consultas_acervos");
            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.TermoPesquisado).ToColumn("termo_pesquisado");
            Map(c => c.AnoInicial).ToColumn("ano_inicial");
            Map(c => c.AnoFinal).ToColumn("ano_final");
            Map(c => c.TipoAcervo).ToColumn("tipo_acervo");
            Map(c => c.DataConsulta).ToColumn("data_consulta");
            Map(c => c.QuantidadeResultados).ToColumn("quantidade_resultados");
        }
    }
}