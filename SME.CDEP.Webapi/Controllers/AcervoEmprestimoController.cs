using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
[ValidaDto]
public class AcervoEmprestimoController: BaseController
{
    [HttpPut("prorrogar")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_A, Policy = "Bearer")]
    public async Task<IActionResult> ProrrogarEmprestimo([FromBody] AcervoEmprestimoProrrogacaoDTO acervoEmprestimoProrrogacaoDTO, [FromServices] IServicoAcervoEmprestimo servicoAcervoEmprestimo)
    {
        return Ok(await servicoAcervoEmprestimo.ProrrogarEmprestimo(acervoEmprestimoProrrogacaoDTO));
    }
    
    [HttpGet("situacoes")]
    [ProducesResponseType(typeof(IEnumerable<SituacaoItemDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterSituacoesEmprestimo([FromServices] IServicoAcervoEmprestimo servicoAcervoEmprestimo)
    {
        return Ok(await servicoAcervoEmprestimo.ObterSituacoesEmprestimo());
    }
    
    [HttpPut("{acervoSolicitacaoItemId}/devolver")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.OperacoesSolicitacoes_A, Policy = "Bearer")]
    public async Task<IActionResult> DevolverItemEmprestado([FromRoute] long acervoSolicitacaoItemId, [FromServices] IServicoAcervoEmprestimo servicoAcervoEmprestimo)
    {
        return Ok(await servicoAcervoEmprestimo.DevolverItemEmprestado(acervoSolicitacaoItemId));
    }
}