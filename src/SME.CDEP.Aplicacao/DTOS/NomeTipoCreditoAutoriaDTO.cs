using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class NomeTipoCreditoAutoriaDTO
{
    public string? Nome { get; set; }
    public TipoCreditoAutoria Tipo { get; set; }
}