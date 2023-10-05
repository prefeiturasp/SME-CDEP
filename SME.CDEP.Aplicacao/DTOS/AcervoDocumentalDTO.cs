using System.Runtime.CompilerServices;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoDocumentalDTO
{
    public long? MaterialId { get; set; }
    public long IdiomaId { get; set; }
    public string? Ano { get; set; }
    public string NumeroPagina { get; set; }
    public string? Volume { get; set; }
    public string Descricao { get; set; }
    public string? TipoAnexo { get; set; }
    public double? Largura { get; set; }
    public double? Altura { get; set; }
    public string? TamanhoArquivo { get; set; }
    public string? Localizacao { get; set; }
    public bool Digitalizado { get; set; }
    public long? ConservacaoId { get; set; }
    public ArquivoResumidoDTO[]? Arquivos { get; set; }
    public AcessoDocumentoResumido[]? AcessoDocumentos { get; set; }
    public AuditoriaDTO Auditoria { get; set; }
    public long[] CreditosAutoresIds { get; set; }
}