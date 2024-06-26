﻿using System.Runtime.CompilerServices;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades;

public class AcervoFotograficoCompleto: EntidadeBaseAuditavel
{
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public long TipoAcervoId { get; set; }
    public string Codigo { get; set; }
    public string Localizacao { get; set; }
    public string Procedencia { get; set; }
    public string Ano { get; set; }
    public string DataAcervo { get; set; }
    public bool? CopiaDigital { get; set; }
    public bool? PermiteUsoImagem { get; set; }
    public long ConservacaoId { get; set; }
    public string Descricao { get; set; }
    public int Quantidade { get; set; }
    public string? Largura { get; set; }
    public string? Altura { get; set; }
    public long SuporteId { get; set; }
    public long FormatoId { get; set; }
    public long CromiaId { get; set; }
    public string Resolucao { get; set; }
    public string TamanhoArquivo { get; set; }
    public ArquivoResumido[] Arquivos  { get; set; }
    public long[] CreditosAutoresIds { get; set; }
}