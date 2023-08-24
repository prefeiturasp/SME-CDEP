using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class AcervoController: BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(IList<IdNomeDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    public IActionResult ObterTiposDeAcervos([FromServices]IServicoAcervo servicoAcervo)
    {
        return Ok(servicoAcervo.ObterTodos());
    }
}