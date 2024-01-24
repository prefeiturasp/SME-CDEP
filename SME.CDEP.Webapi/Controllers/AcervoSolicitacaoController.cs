using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class AcervoSolicitacaoController: BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AcervoTipoTituloAcervoIdCreditosAutoresDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterItensDoAcervoPorFiltros([FromQuery] long[] acervosIds, [FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(await servicoAcervoSolicitacao.ObterItensDoAcervoPorFiltros(acervosIds));
    }

    [HttpPost]
    [ProducesResponseType(typeof(long), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_I, Policy = "Bearer")]
    public async Task<IActionResult> Inserir([FromBody] AcervoSolicitacaoItemCadastroDTO[] acervosSolicitacaoItensCadastroDTO, [FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(await servicoAcervoSolicitacao.Inserir(acervosSolicitacaoItensCadastroDTO));
    }
    
    [HttpGet("{acervoSolicitacaoId}")]
    [ProducesResponseType(typeof(IEnumerable<AcervoSolicitacaoItemRetornoCadastroDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterPorId([FromRoute] long acervoSolicitacaoId,[FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(await servicoAcervoSolicitacao.ObterPorId(acervoSolicitacaoId));
    }
    
    [HttpDelete("{acervoSolicitacaoId}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_E, Policy = "Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long acervoSolicitacaoId, [FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitac)
    {
        return Ok(await servicoAcervoSolicitac.Excluir(acervoSolicitacaoId));
    }
    
    [HttpGet("minhas-solicitacoes")]
    [ProducesResponseType(typeof(PaginacaoResultadoDTO<MinhaSolicitacaoDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterMinhasSolicitacoes([FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(await servicoAcervoSolicitacao.ObterMinhasSolicitacoes());
    }
    
    [HttpGet("situacoes")]
    [ProducesResponseType(typeof(IEnumerable<SituacaoItemDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterSituacoesAtendimentos([FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(await servicoAcervoSolicitacao.ObterSituacoesAtendimentosItem());
    }
    
    [HttpGet("atendimento-solicitacoes")]
    [ProducesResponseType(typeof(PaginacaoResultadoDTO<SolicitacaoDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterSolicitacoesPorFiltro([FromQuery] FiltroSolicitacaoDTO filtroSolicitacaoDto, [FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(await servicoAcervoSolicitacao.ObterSolicitacoesPorFiltro(filtroSolicitacaoDto));
    }
}