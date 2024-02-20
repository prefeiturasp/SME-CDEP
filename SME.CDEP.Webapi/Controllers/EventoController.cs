using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class EventoController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(long), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.GestaoDeVisitaCalendario_I, Policy = "Bearer")]
    public async Task<IActionResult> Inserir([FromBody] EventoCadastroDTO eventoCadastroDto, [FromServices] IServicoEvento servicoEvento)
    {
        return Ok(await servicoEvento.Inserir(eventoCadastroDto));
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(long), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.GestaoDeVisitaCalendario_A, Policy = "Bearer")]
    public async Task<IActionResult> Alterar([FromBody] EventoCadastroDTO eventoCadastroDto, [FromServices] IServicoEvento servicoEvento)
    {
        return Ok(await servicoEvento.Alterar(eventoCadastroDto));
    }
    
    [HttpGet("eventos-tag")]
    [ProducesResponseType(typeof(IEnumerable<EventoTagDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.GestaoDeVisitaCalendario_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterEventosTagPorData([FromBody] DiaMesDTO diaMesDto, [FromServices] IServicoEvento servicoEvento)
    {
        return Ok(await servicoEvento.ObterEventosTagPorData(diaMesDto));
    }
}