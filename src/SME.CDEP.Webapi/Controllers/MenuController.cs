using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;

namespace SME.CDEP.Webapi.Controllers
{
    [ApiController]
    [Authorize("Bearer")]
    public class MenuController : BaseController
    {
        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [ProducesResponseType(typeof(IEnumerable<RetornoBaseDTO>), 200)]
        public async Task<IActionResult> Get([FromServices]IServicoMenu servicoMenu)
        {
            return Ok(await servicoMenu.ObterMenu());
        }
    }
}