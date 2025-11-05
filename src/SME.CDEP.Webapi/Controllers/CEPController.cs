using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Integracoes.Interfaces;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class CEPController: BaseController
{
    [HttpGet("{cep}")] 
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [AllowAnonymous]
    public async Task<IActionResult> BuscarCep([FromRoute] string cep, [FromServices] IServicoCEP servicoCEP)
    {
        return Ok(await servicoCEP.BuscarCEP(cep));
    }
}