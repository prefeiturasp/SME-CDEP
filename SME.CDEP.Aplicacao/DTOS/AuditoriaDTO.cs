using System.Runtime.CompilerServices;
using SME.CDEP.Dominio;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AuditoriaDTO
{
    public DateTime? AlteradoEm { get; set; }
    public string? AlteradoPor { get; set; }
    public string? AlteradoLogin { get; set; }
    public DateTime CriadoEm { get; set; }
    public string CriadoPor { get; set; }
    public string CriadoLogin { get; set; }
    
    public static explicit operator AuditoriaDTO(EntidadeBaseAuditavel entidade)
        => entidade.EhNulo() ? null :
            new AuditoriaDTO()
            {
                CriadoEm = entidade.CriadoEm,
                CriadoPor = entidade.CriadoPor,
                CriadoLogin = entidade.CriadoLogin,
                AlteradoEm = entidade.AlteradoEm, 
                AlteradoPor = entidade.AlteradoPor,
                AlteradoLogin = entidade.AlteradoLogin
            };
}