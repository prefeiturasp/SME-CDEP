using System.Runtime.CompilerServices;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoBibliograficoDTO
{
    public long Id { get; set; }
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public string SubTitulo { get; set; }
    public long TipoAcervoId { get; set; }
    public string Codigo { get; set; }
    public long? MaterialId { get; set; }
    public long? EditoraId { get; set; }
    public long[] AssuntosIds { get; set; }
    public string? Ano { get; set; }
    public string? Edicao { get; set; }
    public string? NumeroPagina { get; set; }
    public double? Largura { get; set; }
    public double? Altura { get; set; }
    public long? SerieColecaoId { get; set; }
    public string? Volume { get; set; }
    public long IdiomaId { get; set; }
    public string LocalizacaoCDD { get; set; }
    public string LocalizacaoPHA { get; set; }
    public string? NotasGerais { get; set; }
    public string? Isbn { get; set; }
    public AuditoriaDTO Auditoria { get; set; }
    public long[] CreditosAutoresIds { get; set; }
    public CoAutorDTO[]? CoAutores { get; set; }
}