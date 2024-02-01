using System.Runtime.CompilerServices;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoDocumentalDTO
{
    public long Id { get; set; }
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public long TipoAcervoId { get; set; }
    public string Codigo { get; set; }
    public string CodigoNovo { get; set; }
    public long? MaterialId { get; set; }
    public long? IdiomaId { get; set; }
    public string? Ano { get; set; }
    public string NumeroPagina { get; set; }
    public string? Volume { get; set; }
    public string Descricao { get; set; }
    public string? TipoAnexo { get; set; }
    public string? Largura { get; set; }
    public string? Altura { get; set; }
    public string? TamanhoArquivo { get; set; }
    public string? Localizacao { get; set; }
    public bool? CopiaDigital { get; set; }
    public long? ConservacaoId { get; set; }
    public ArquivoResumidoDTO[]? Arquivos { get; set; }
    public long[] AcessoDocumentosIds { get; set; }
    public AuditoriaDTO Auditoria { get; set; }
    public long[] CreditosAutoresIds { get; set; }
}