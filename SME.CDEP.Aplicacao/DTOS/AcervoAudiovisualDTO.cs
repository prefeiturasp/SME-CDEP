using System.Runtime.CompilerServices;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoAudiovisualDTO
{
    public long Id { get; set; }
    public long AcervoId { get; set; }
    public string Titulo { get; set; }
    public long TipoAcervoId { get; set; }
    public string Codigo { get; set; }
    public string? Localizacao { get; set; }
    public string? Procedencia { get; set; }
    public string DataAcervo { get; set; }
    public string? Copia { get; set; }
    public bool? PermiteUsoImagem { get; set; }
    public long? ConservacaoId { get; set; }
    public string Descricao { get; set; }
    public long? SuporteId { get; set; }
    public string Duracao { get; set; }
    public long? CromiaId { get; set; }
    public string? TamanhoArquivo { get; set; }
    public string? Acessibilidade { get; set; }
    public string? Disponibilizacao { get; set; }
    public AuditoriaDTO Auditoria { get; set; }
    public long[] CreditosAutoresIds { get; set; }
    public int? Ano { get; set; }
}