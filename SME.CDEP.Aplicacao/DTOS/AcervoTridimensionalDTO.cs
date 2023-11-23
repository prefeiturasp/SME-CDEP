using System.Runtime.CompilerServices;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoTridimensionalDTO
{
    public long Id { get; set; }
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public long TipoAcervoId { get; set; }
    public string Codigo { get; set; }
    public string Procedencia { get; set; }
    public string DataAcervo { get; set; }
    public long? ConservacaoId { get; set; }
    public long Quantidade { get; set; }
    public string Descricao { get; set; }
    public double? Largura { get; set; }
    public double? Altura { get; set; }
    public double? Profundidade { get; set; }
    public double? Diametro { get; set; }
    public ArquivoResumidoDTO[]? Arquivos { get; set; }
    public AuditoriaDTO Auditoria { get; set; }
}