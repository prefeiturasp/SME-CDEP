using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.Dtos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[Route("api/v1/autenticacao")]
[ValidaDto]
[Authorize("Bearer")]
public class AutenticacaoController: ControllerBase
{
    private readonly IServicoUsuario servicoUsuario;
    public AutenticacaoController(IServicoUsuario servicoUsuario)
    {
        this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
    }
    
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDto), 500)]
    [ProducesResponseType(typeof(UsuarioAutenticacaoRetornoDto), 200)]
    [AllowAnonymous]
    public async Task<IActionResult> Autenticar(AutenticacaoDto autenticacaoDto)
    {
        var retornoAutenticacao = await servicoUsuario.Autenticar(autenticacaoDto.Login, autenticacaoDto.Senha);

        if (!retornoAutenticacao.Autenticado)
            return StatusCode(401);

        return Ok(retornoAutenticacao);
    }
}