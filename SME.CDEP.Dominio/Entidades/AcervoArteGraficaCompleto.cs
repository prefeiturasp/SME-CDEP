﻿namespace SME.CDEP.Dominio.Entidades;

public class AcervoArteGraficaCompleto: EntidadeBase
{
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
    public long CromiaId { get; set; }
    public float Largura { get; set; }
    public float Altura { get; set; }
    public float Diametro { get; set; }
    public string Tecnica { get; set; }
    public long SuporteId { get; set; }
    public long Quantidade { get; set; }
    public string Descricao { get; set; }
    public DateTime? AlteradoEm { get; set; }
    public string? AlteradoPor { get; set; }
    public string? AlteradoLogin { get; set; }
    public DateTime CriadoEm { get; set; }
    public string CriadoPor { get; set; }
    public string CriadoLogin { get; set; }
    public ArquivoResumido[] Arquivos  { get; set; }
    public long ArquivoId { get; set; }
    public string ArquivoNome { get; set; }
    public Guid ArquivoCodigo { get; set; }
    public long[] CreditosAutoresIds { get; set; }
}