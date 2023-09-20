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
    public long ConservacaoId { get; set; }
    public long Quantidade { get; set; }
    public string Descricao { get; set; }
    public float Largura { get; set; }
    public float Altura { get; set; }
    public float Profundidade { get; set; }
    public float Diametro { get; set; }
    public ArquivoResumidoDTO[]? Arquivos { get; set; }
    public AuditoriaDTO Auditoria { get; set; }
    public long[] CreditosAutoresIds { get; set; }
}