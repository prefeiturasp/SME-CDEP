using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoSolicitacaoItemDetalhe
    {
        public long Id { get; set; }
        public long AcervoSolicitacaoId { get; set; }
        public TipoAcervo TipoAcervo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataVisita { get; set; }
        public string? Solicitante { get; set; }
        public string? Responsavel { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Codigo { get; set; }
        public string? CodigoNovo { get; set; }
        public string? Email { get; set; }
        public SituacaoSolicitacaoItem Situacao { get; set; }
        public SituacaoEmprestimo? SituacaoEmprestimo { get; set; }
        public TipoAtendimento? TipoAtendimento { get; set; }
        public string ObterDataVisitaOuTraco
        {
            get
            {
                if (!DataVisita.HasValue)
                    return "-";

                return DataVisita.Value.ToString("dd/MM HH:mm");
            }
        }
        
        public string ObterCodigoTombo
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Codigo) && !string.IsNullOrWhiteSpace(CodigoNovo)
                    ? $"{Codigo}/{CodigoNovo}"
                    : !string.IsNullOrWhiteSpace(Codigo)
                        ? Codigo
                        : CodigoNovo ?? string.Empty;
            }
        }
    }
}
