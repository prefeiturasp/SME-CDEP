using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoFotograficoDTO
{
    public long Id { get; set; }
    public AcervoDTO Acervo { get; set; }
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public long TipoAcervoId { get; set; }
    public string Codigo { get; set; }
    public long CreditoAutorId { get; set; }
    public string CreditoAutorNome { get; set; }
    public string Localizacao { get; set; }
    public string Procedencia { get; set; }
    public string DataAcervo { get; set; }
    public bool CopiaDigital { get; set; }
    public bool PermiteUsoImagem { get; set; }
    public long ConservacaoId { get; set; }
    public string Descricao { get; set; }
    public long Quantidade { get; set; }
    public float Largura { get; set; }
    public float Altura { get; set; }
    public long SuporteId { get; set; }
    public long FormatoId { get; set; }
    public long CromiaId { get; set; }
    public string Resolucao { get; set; }
    public string TamanhoArquivo { get; set; }
}