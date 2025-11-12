using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Controllers.Filtros;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ValidaDto]
public class EventoController : BaseController
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
    public async Task<IActionResult> ObterEventosTagPorData([FromQuery] DiaMesDTO diaMesDto, [FromServices] IServicoEvento servicoEvento)
    {
        return Ok(await servicoEvento.ObterEventosTagPorData(diaMesDto));
    }

    [HttpDelete("{eventoId}")]
    [ProducesResponseType(typeof(long), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.GestaoDeVisitaCalendario_E, Policy = "Bearer")]
    public async Task<IActionResult> ExcluirLogicamente([FromRoute] long eventoId, [FromServices] IServicoEvento servicoEvento)
    {
        return Ok(await servicoEvento.ExcluirLogicamente(eventoId));
    }

    [HttpGet("{eventoId}")]
    [ProducesResponseType(typeof(IEnumerable<EventoTagDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.GestaoDeVisitaCalendario_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterEventoPorId([FromRoute] int eventoId, [FromServices] IServicoEvento servicoEvento)
    {
        return Ok(await servicoEvento.ObterEventoPorId(eventoId));
    }

    [HttpGet("calendario/{mes}")]
    [ProducesResponseType(typeof(CalendarioEventoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.GestaoDeVisitaCalendario_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterCalendarioDeEventosPorMes([FromRoute] int mes, [FromServices] IServicoEvento servicoEvento)
    {
        return Ok(await servicoEvento.ObterCalendarioDeEventosPorMes(mes, DateTimeExtension.HorarioBrasilia().Year));
    }

    [HttpGet("detalhes-dia")]
    [ProducesResponseType(typeof(IEnumerable<EventoDetalheDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.GestaoDeVisitaCalendario_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterDetalhesDoDiaPorDiaMes([FromQuery] DiaMesDTO diaMesDto, [FromServices] IServicoEvento servicoEvento)
    {
        return Ok(await servicoEvento.ObterDetalhesDoDiaPorDiaMes(diaMesDto));
    }
}