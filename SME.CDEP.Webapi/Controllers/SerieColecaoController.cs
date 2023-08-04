using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class SerieColecaoController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(IdNomeExcluidoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Authorize("Bearer")]
    public async Task<IActionResult> CadastrarAlterar([FromBody] IdNomeExcluidoAuditavelDTO serieColecao, [FromServices] IServicoSerieColecao servicoSerieColecao)
    {
        return serieColecao.Id > 0 ? Ok(await servicoSerieColecao.Alterar(serieColecao)) : Ok(await servicoSerieColecao.Inserir(serieColecao));
    }

    [HttpGet]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeExcluidoDTO), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> ObterTodos([FromServices]IServicoSerieColecao servicoSerieColecao)
    {
        return Ok(await servicoSerieColecao.ObterTodos());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeExcluidoDTO), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> ObterTodos([FromRoute] long id,[FromServices]IServicoSerieColecao servicoSerieColecao)
    {
        return Ok(await servicoSerieColecao.ObterPorId(id));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Authorize("Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long id, [FromServices] IServicoSerieColecao servicoSerieColecao)
    {
        return Ok(await servicoSerieColecao.Excluir(id));
    }
    
    [HttpGet("pesquisar/{nome}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeExcluidoAuditavelDTO), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> PesquisarPorNome([FromRoute] string nome, [FromServices] IServicoSerieColecao servicoSerieColecao)
    {
        return Ok(await servicoSerieColecao.PesquisarPorNome(nome));
    }
}