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
    public async Task<IActionResult> CadastrarAcervoSolicitacaoViaPortal([FromBody] AcervoSolicitacaoItemCadastroDTO[] acervosSolicitacaoItensCadastroDTO, [FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(await servicoAcervoSolicitacao.Inserir(acervosSolicitacaoItensCadastroDTO));
    }
    
    [HttpGet("{acervoSolicitacaoId}")]
    [ProducesResponseType(typeof(AcervoSolicitacaoRetornoCadastroDTO), 200)]
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
    
    [HttpGet("detalhes/{acervoSolicitacaoId}")]
    [ProducesResponseType(typeof(AcervoSolicitacaoDetalheDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterDetalhesPorId([FromRoute] long acervoSolicitacaoId,[FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(await servicoAcervoSolicitacao.ObterDetalhesPorId(acervoSolicitacaoId));
    }
    
    [HttpGet("tipo-atendimento")]
    [ProducesResponseType(typeof(IEnumerable<IdNomeDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_C, Policy = "Bearer")]
    public IActionResult ObterTiposDeAtendimentos([FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(servicoAcervoSolicitacao.ObterTiposDeAtendimentos());
    }
    
    [HttpPut("confirmar-atendimento")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_A, Policy = "Bearer")]
    public async Task<IActionResult> ConfirmarAtendimento([FromBody] AcervoSolicitacaoConfirmarDTO acervoSolicitacaoConfirmar, [FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(await servicoAcervoSolicitacao.ConfirmarAtendimento(acervoSolicitacaoConfirmar));
    }
    
    [HttpPut("{acervoSolicitacaoId}/finalizar-atendimento")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_A, Policy = "Bearer")]
    public async Task<IActionResult> FinalizarAtendimento([FromRoute] long acervoSolicitacaoId, [FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(await servicoAcervoSolicitacao.FinalizarAtendimento(acervoSolicitacaoId));
    }
    
    [HttpPut("{acervoSolicitacaoId}/cancelar-atendimento")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_A, Policy = "Bearer")]
    public async Task<IActionResult> CancelarAtendimento([FromRoute] long acervoSolicitacaoId, [FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(await servicoAcervoSolicitacao.CancelarAtendimento(acervoSolicitacaoId));
    }
    
    [HttpPut("{acervoSolicitacaoItemId}/cancelar-item-atendimento")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_A, Policy = "Bearer")]
    public async Task<IActionResult> CancelarItemAtendimento([FromRoute] long acervoSolicitacaoItemId, [FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(await servicoAcervoSolicitacao.CancelarItemAtendimento(acervoSolicitacaoItemId));
    }
    
    [HttpPut("alterar-data-visita")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_A, Policy = "Bearer")]
    public async Task<IActionResult> AlterarDataVisitaDoItemAtendimento([FromBody] AlterarDataVisitaAcervoSolicitacaoItemDTO alterarDataVisitaAcervoSolicitacaoItemDto, [FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(await servicoAcervoSolicitacao.AlterarDataVisitaDoItemAtendimento(alterarDataVisitaAcervoSolicitacaoItemDto));
    }
    
    [HttpPut("cancelar-itens")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_E, Policy = "Bearer")]
    public async Task<IActionResult> CancelarItensAcervoSolicitacao([FromBody] long[] acervoSolicitacaoItemIds, [FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(await servicoAcervoSolicitacao.CancelarItensAcervoSolicitacao(acervoSolicitacaoItemIds));
    }
    
    [HttpGet("situacao-atendimento")]
    [ProducesResponseType(typeof(IEnumerable<IdNomeDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_C, Policy = "Bearer")]
    public IActionResult ObterSituacaoAtendimentos([FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(servicoAcervoSolicitacao.ObterSituacoesDeAtendimentos());
    }
    
    [HttpPost("manual")]
    [ProducesResponseType(typeof(long), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_I, Policy = "Bearer")]
    public async Task<IActionResult> CadastrarAcervoSolicitacaoManual([FromBody] AcervoSolicitacaoManualCadastroDTO acervoSolicitacaoManualCadastroDTO, [FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(await servicoAcervoSolicitacao.Inserir(acervoSolicitacaoManualCadastroDTO));
    }
}