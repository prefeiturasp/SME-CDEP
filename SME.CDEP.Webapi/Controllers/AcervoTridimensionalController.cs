using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class AcervoTridimensionalController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(AcervoTridimensionalCadastroDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    // [Permissao(Permissao.ACR_I, Policy = "Bearer")]
    public async Task<IActionResult> Inserir([FromBody] AcervoTridimensionalCadastroDTO acervoTridimensional, [FromServices] IServicoAcervoTridimensional servicoAcervoTridimensional)
    {
        return Ok(await servicoAcervoTridimensional.Inserir(acervoTridimensional));
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(AcervoTridimensionalDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    // [Permissao(Permissao.ACR_A, Policy = "Bearer")]
    public async Task<IActionResult> Alterar([FromBody] AcervoTridimensionalAlteracaoDTO acervoTridimensional, [FromServices] IServicoAcervoTridimensional servicoAcervoTridimensional)
    {
        return Ok(await servicoAcervoTridimensional.Alterar(acervoTridimensional));
    }
    
    [HttpGet("{acervoId}")]
    [ProducesResponseType(typeof(AcervoArteGraficaDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ACR_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterPorId([FromRoute] long acervoId,[FromServices] IServicoAcervoTridimensional servicoAcervoTridimensional)
    {
        return Ok(await servicoAcervoTridimensional.ObterPorId(acervoId));
    }
    
    [HttpDelete("{acervoId}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ACR_E, Policy = "Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long acervoId, [FromServices] IServicoAcervoTridimensional servicoAcervoTridimensional)
    {
        return Ok(await servicoAcervoTridimensional.Excluir(acervoId));
    }
}