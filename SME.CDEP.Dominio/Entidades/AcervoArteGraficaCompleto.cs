namespace SME.CDEP.Dominio.Entidades;

public class AcervoArteGraficaCompleto: EntidadeBaseAuditavel
{
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public long TipoAcervoId { get; set; }
    public string Codigo { get; set; }
    public int Ano { get; set; }
    public long CreditoAutorId { get; set; }
    public string CreditoAutorNome { get; set; }
    public string Localizacao { get; set; }
    public string Procedencia { get; set; }
    public string DataAcervo { get; set; }
    public bool CopiaDigital { get; set; }
    public bool PermiteUsoImagem { get; set; }
    public long ConservacaoId { get; set; }
    public long CromiaId { get; set; }
    public string CromiaNome { get; set; }
    public double? Largura { get; set; }
    public double? Altura { get; set; }
    public double? Diametro { get; set; }
    public string Tecnica { get; set; }
    public long SuporteId { get; set; }
    public long Quantidade { get; set; }
    public string Descricao { get; set; }
    public ArquivoResumido[] Arquivos  { get; set; }
    public long ArquivoId { get; set; }
    public string ArquivoNome { get; set; }
    public Guid ArquivoCodigo { get; set; }
    public long[] CreditosAutoresIds { get; set; }
    public string CreditosAutores { get; set; }
    public string ConservacaoNome { get; set; }
    public string SuporteNome { get; set; }
    public string ArquivoNomeMiniatura { get; set; }
    public Guid ArquivoCodigoMiniatura { get; set; }
    public IEnumerable<ImagemDetalhe>? Imagens { get; set; }
}