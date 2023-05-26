using Microsoft.AspNetCore.Mvc;

namespace SME.CDEP.Webapi.Controllers;

public class TesteController : BaseController
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok();
    }
}
