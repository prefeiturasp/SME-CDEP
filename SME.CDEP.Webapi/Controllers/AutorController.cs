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
public class AutorController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(IdNomeExcluidoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.AUT_I, Policy = "Bearer")]
    [Permissao(Permissao.AUT_A, Policy = "Bearer")]
    public async Task<IActionResult> CadastrarAlterar([FromBody] IdNomeExcluidoAuditavelDTO autor, [FromServices] IServicoAutor servicoAutor)
    {
        return autor.Id > 0 ? Ok(await servicoAutor.Alterar(autor)) : Ok(await servicoAutor.Inserir(autor));
    }

    [HttpGet]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeExcluidoDTO), 200)]  
    [Permissao(Permissao.AUT_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterTodos([FromServices]IServicoAutor servicoAutor)
    {
        return Ok(await servicoAutor.ObterTodos());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeExcluidoDTO), 200)]  
    [Permissao(Permissao.AUT_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterPorId([FromRoute] long id,[FromServices]IServicoAutor servicoAutor)
    {
        return Ok(await servicoAutor.ObterPorId(id));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Permissao(Permissao.AUT_E, Policy = "Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long id, [FromServices] IServicoAutor servicoAutor)
    {
        return Ok(await servicoAutor.Excluir(id));
    }
    
    [HttpGet("pesquisar/{nome}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeExcluidoAuditavelDTO), 200)]  
    [Permissao(Permissao.AUT_C, Policy = "Bearer")]
    public async Task<IActionResult> PesquisarPorNome([FromRoute] string nome, [FromServices] IServicoAutor servicoAutor)
    {
        return Ok(await servicoAutor.PesquisarPorNome(nome));
    }
}