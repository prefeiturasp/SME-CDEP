using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoSolicitacaoItemRetornoCadastroDTO  
{
    public long Id { get; set; }
    public string TipoAcervo { get; set; }
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public string[] AutoresCreditos { get; set; }
    public string Situacao { get; set; }
    public int SituacaoId { get; set; }
    public string TipoAtendimento { get; set; }
    public DateTime? DataVisita { get; set; }
    public IEnumerable<ArquivoCodigoNomeDTO> Arquivos { get; set; }
    public bool AlteraDataVisita { get; set; }
    public SituacaoEmprestimo? SituacaoEmprestimo { get; set; }
    public SituacaoSaldo SituacaoSaldo { get; set; }
    public long acervoSolicitacaoId { get; set; }
    public bool TemControleDisponibilidade { get; set; }
    public string SituacaoDisponibilidade { get; set; }
    public bool EstaDisponivel { get; set; }
}