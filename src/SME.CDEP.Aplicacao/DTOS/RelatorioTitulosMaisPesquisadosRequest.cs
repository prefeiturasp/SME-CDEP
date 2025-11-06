using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class RelatorioTitulosMaisPesquisadosRequest
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public List<TipoAcervo>? TipoAcervos { get; set; }
    }
}
