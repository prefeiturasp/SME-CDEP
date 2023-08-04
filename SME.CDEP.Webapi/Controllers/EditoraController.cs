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
public class EditoraController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(IdNomeExcluidoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.EDT_I, Policy = "Bearer")]
    [Permissao(Permissao.EDT_A, Policy = "Bearer")]
    public async Task<IActionResult> CadastrarAlterar([FromBody] IdNomeExcluidoAuditavelDTO editora, [FromServices] IServicoEditora servicoEditora)
    {
        return editora.Id > 0 ? Ok(await servicoEditora.Alterar(editora)) : Ok(await servicoEditora.Inserir(editora));
    }

    [HttpGet]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeExcluidoDTO), 200)]  
    [Permissao(Permissao.EDT_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterTodos([FromServices]IServicoEditora servicoEditora)
    {
        return Ok(await servicoEditora.ObterTodos());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeExcluidoDTO), 200)]  
    [Permissao(Permissao.EDT_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterPorId([FromRoute] long id,[FromServices]IServicoEditora servicoEditora)
    {
        return Ok(await servicoEditora.ObterPorId(id));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Permissao(Permissao.EDT_E, Policy = "Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long id, [FromServices] IServicoEditora servicoEditora)
    {
        return Ok(await servicoEditora.Excluir(id));
    }
    
    [HttpGet("pesquisar/{nome}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeExcluidoAuditavelDTO), 200)]  
    [Permissao(Permissao.EDT_C, Policy = "Bearer")]
    public async Task<IActionResult> PesquisarPorNome([FromRoute] string nome, [FromServices] IServicoEditora servicoEditora)
    {
        return Ok(await servicoEditora.PesquisarPorNome(nome));
    }
}