using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class AcervoController: BaseController
{
    [HttpGet("tipos")]
    [ProducesResponseType(typeof(IEnumerable<IdNomeDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ACR_C, Policy = "Bearer")]
    public IActionResult ObterTiposDeAcervos([FromServices]IServicoAcervo servicoAcervo)
    {
        return Ok(servicoAcervo.ObterTodosTipos());
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PaginacaoResultadoDTO<IdTipoTituloCreditoAutoriaCodigoAcervoDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ACR_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterTodosOuPorTipoTituloCreditoAutoriaTomboCodigoe([FromQuery] FiltroTipoTituloCreditoAutoriaCodigoAcervoDTO filtro,[FromServices]IServicoAcervo servicoAcervo)
    {
        return Ok(await servicoAcervo.ObterPorFiltro(filtro.TipoAcervo, filtro.Titulo, filtro.CreditoAutorId, filtro.Codigo));
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PaginacaoResultadoDTO<PesquisaAcervoDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [AllowAnonymous]
    public async Task<IActionResult> ObterPorTextoLivreETipoAcervo([FromQuery] FiltroTextoLivreTipoAcervoDTO filtroTextoLivreTipoAcervo,[FromServices]IServicoAcervo servicoAcervo)
    {
        return Ok(await servicoAcervo.ObterPorTextoLivreETipoAcervo(filtroTextoLivreTipoAcervo));
    }
}