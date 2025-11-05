using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Extensions;
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
    public IActionResult ObterTiposDeAcervos([FromServices] IServicoAcervo servicoAcervo)
    {
        return Ok(servicoAcervo.ObterTodosTipos());
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PaginacaoResultadoDTO<AcervoTableRowDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterTodosOuPorTipoTituloCreditoAutoriaTomboECodigo([FromQuery] FiltroTipoTituloCreditoAutoriaCodigoAcervoDTO filtro,[FromServices]IServicoAcervo servicoAcervo)
    {
        return Ok(await servicoAcervo.ObterPorFiltro(filtro.TipoAcervo, filtro.Titulo, filtro.CreditoAutorId, filtro.Codigo, filtro.IdEditora));
    }
    
    [HttpGet("pesquisar-acervos")]
    [ProducesResponseType(typeof(PaginacaoResultadoDTO<PesquisaAcervoDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 204)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [AllowAnonymous]
    public async Task<IActionResult> ObterPorTextoLivreETipoAcervo([FromQuery] FiltroTextoLivreTipoAcervoDTO filtroTextoLivreTipoAcervo,[FromServices]IServicoAcervo servicoAcervo)
    {
        return Ok(await servicoAcervo.ObterPorTextoLivreETipoAcervo(filtroTextoLivreTipoAcervo));
    }
    
    [HttpGet("detalhar-acervo")]
    [ProducesResponseType(typeof(AcervoDetalheDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [AllowAnonymous]
    public async Task<IActionResult> ObterDetalhamentoAcervo([FromQuery] FiltroDetalharAcervoDTO filtroDetalharAcervoDto,[FromServices]IServicoAcervo servicoAcervo)
    {
        return Ok(await servicoAcervo.ObterDetalhamentoPorTipoAcervoECodigo(filtroDetalharAcervoDto));
    }
    
    [HttpGet("termo-compromisso")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterTermoDeCompromisso([FromServices] IServicoAcervo servicoAcervo)
    {
        return Ok(await servicoAcervo.ObterTermoDeCompromisso());
    }
    
    [HttpGet("pesquisar")]
    [ProducesResponseType(typeof(IdNomeCodigoTipoParaEmprestimoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_C, Policy = "Bearer")]
    public async Task<IActionResult> PesquisarAcervoPorCodigoTombo([FromQuery] FiltroCodigoTomboDTO filtroCodigoTomboDto,[FromServices] IServicoAcervo servicoAcervo)
    {
        return Ok(await servicoAcervo.PesquisarAcervoPorCodigoTombo(filtroCodigoTomboDto));
    }
}
