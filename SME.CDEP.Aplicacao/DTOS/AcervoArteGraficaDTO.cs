using System.Runtime.CompilerServices;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoArteGraficaDTO
{
    public long Id { get; set; }
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public long TipoAcervoId { get; set; }
    public string Codigo { get; set; }
    public string Localizacao { get; set; }
    public string Procedencia { get; set; }
    public bool? CopiaDigital { get; set; }
    public bool? PermiteUsoImagem { get; set; }
    public long? ConservacaoId { get; set; }
    public long? CromiaId { get; set; }
    public string Largura { get; set; }
    public string Altura { get; set; }
    public string Diametro { get; set; }
    public string Tecnica { get; set; }
    public long? SuporteId { get; set; }
    public long? Quantidade { get; set; }
    public string Descricao { get; set; }
    public ArquivoResumidoDTO[]? Arquivos { get; set; }
    public AuditoriaDTO Auditoria { get; set; }
    public long[] CreditosAutoresIds { get; set; }
    public string? Ano { get; set; }
}