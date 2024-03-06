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
}