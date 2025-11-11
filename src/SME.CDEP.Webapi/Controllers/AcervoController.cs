using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ValidaDto]
public class AcervoController (IServicoAcervo servicoAcervo) : BaseController
{
    [HttpGet("tipos")]
    [ProducesResponseType(typeof(IEnumerable<IdNomeDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    public IActionResult ObterTiposDeAcervos()
    {
        return Ok(servicoAcervo.ObterTodosTipos());
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginacaoResultadoDTO<AcervoTableRowDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterTodos([FromQuery] FiltroTipoTituloCreditoAutoriaCodigoAcervoDTO filtro)
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
    public async Task<IActionResult> ObterPorTextoLivreETipoAcervo([FromQuery] FiltroTextoLivreTipoAcervoDTO filtroTextoLivreTipoAcervo)
    {
        return Ok(await servicoAcervo.ObterPorTextoLivreETipoAcervo(filtroTextoLivreTipoAcervo));
    }

    [HttpGet("detalhar-acervo")]
    [ProducesResponseType(typeof(AcervoDetalheDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [AllowAnonymous]
    public async Task<IActionResult> ObterDetalhamentoAcervo([FromQuery] FiltroDetalharAcervoDTO filtroDetalharAcervoDto)
    {
        return Ok(await servicoAcervo.ObterDetalhamentoPorTipoAcervoECodigo(filtroDetalharAcervoDto));
    }

    [HttpGet("termo-compromisso")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterTermoDeCompromisso()
    {
        return Ok(await servicoAcervo.ObterTermoDeCompromisso());
    }

    [HttpGet("pesquisar")]
    [ProducesResponseType(typeof(IdNomeCodigoTipoParaEmprestimoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_C, Policy = "Bearer")]
    public async Task<IActionResult> PesquisarAcervoPorCodigoTombo([FromQuery] FiltroCodigoTomboDTO filtroCodigoTomboDto)
    {
        return Ok(await servicoAcervo.PesquisarAcervoPorCodigoTombo(filtroCodigoTomboDto));
    }

    [HttpGet("autocompletar-titulo")]
    [ProducesResponseType(typeof(IEnumerable<string>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterAutocompletacaoTituloAcervosBaixados([FromQuery] string termoPesquisado)
    {
        return Ok(await servicoAcervo.ObterAutocompletacaoTituloAcervosBaixadosAsync(termoPesquisado));
    }
}
