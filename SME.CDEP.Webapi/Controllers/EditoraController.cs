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
    [ProducesResponseType(typeof(IdNomeExcluidoAuditavelDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroEditora_I, Policy = "Bearer")]
    public async Task<IActionResult> Inserir([FromBody] NomeDTO editora, [FromServices] IServicoEditora servicoEditora)
    {
        return Ok(await servicoEditora.Inserir(new IdNomeExcluidoAuditavelDTO() { Nome = editora.Nome}));
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(IdNomeExcluidoAuditavelDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroEditora_A, Policy = "Bearer")]
    public async Task<IActionResult> Alterar([FromBody] IdNomeDTO editora, [FromServices] IServicoEditora servicoEditora)
    {
        return Ok(await servicoEditora.Alterar(new IdNomeExcluidoAuditavelDTO() {Id = editora.Id, Nome = editora.Nome}));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginacaoResultadoDTO<IdNomeExcluidoAuditavelDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)] 
    [Permissao(Permissao.CadastroEditora_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterTodosOuPorNome([FromQuery] string? nome,[FromServices]IServicoEditora servicoEditora)
    {
        return Ok(await servicoEditora.ObterPaginado(nome));
    }
    
    [HttpGet("resumido")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeDTO), 200)]  
    [Permissao(Permissao.CadastroEditora_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterTodos([FromServices]IServicoEditora servicoEditora)
    {
        return Ok((await servicoEditora.ObterTodos()).Select(s=> new IdNomeDTO() { Id = s.Id, Nome = s.Nome}));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IdNomeExcluidoAuditavelDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)] 
    [Permissao(Permissao.CadastroEditora_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterPorId([FromRoute] long id,[FromServices]IServicoEditora servicoEditora)
    {
        return Ok(await servicoEditora.ObterPorId(id));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroEditora_E, Policy = "Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long id, [FromServices] IServicoEditora servicoEditora)
    {
        return Ok(await servicoEditora.Excluir(id));
    }
}