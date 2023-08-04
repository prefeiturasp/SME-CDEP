using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
[Authorize("Bearer")]
public class CreditoController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(IdNomeExcluidoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CRD_I, Policy = "Bearer")]
    [Permissao(Permissao.CRD_A, Policy = "Bearer")]
    public async Task<IActionResult> CadastrarAlterar([FromBody] IdNomeExcluidoAuditavelDTO creditoDTO, [FromServices] IServicoCredito servicoCredito)
    {
        return creditoDTO.Id > 0 ? Ok(await servicoCredito.Alterar(creditoDTO)) : Ok(await servicoCredito.Inserir(creditoDTO));
    }

    [HttpGet]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeExcluidoDTO), 200)]  
    [Permissao(Permissao.CRD_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterTodos([FromServices]IServicoCredito servicoCredito)
    {
        return Ok(await servicoCredito.ObterTodos());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeExcluidoDTO), 200)]  
    [Permissao(Permissao.CRD_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterPorId([FromRoute] long id,[FromServices]IServicoCredito servicoCredito)
    {
        return Ok(await servicoCredito.ObterPorId(id));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Permissao(Permissao.CRD_E, Policy = "Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long id, [FromServices] IServicoCredito servicoCredito)
    {
        return Ok(await servicoCredito.Excluir(id));
    }
    
    [HttpGet("pesquisar/{nome}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeExcluidoAuditavelDTO), 200)]  
    [Permissao(Permissao.CRD_C, Policy = "Bearer")]
    public async Task<IActionResult> PesquisarPorNome([FromRoute] string nome, [FromServices]IServicoCredito servicoCredito)
    {
        return Ok(await servicoCredito.PesquisarPorNome(nome));
    }
}