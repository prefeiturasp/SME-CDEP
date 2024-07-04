using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[Route("api/v1/autenticacao")]
[ValidaDto]
public class AutenticacaoController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(UsuarioAutenticacaoRetornoDTO), 200)]
    [AllowAnonymous]
    public async Task<IActionResult> Autenticar(AutenticacaoDTO autenticacaoDto, [FromServices] IServicoUsuario servicoUsuario)
    {
        return Ok(await servicoUsuario.Autenticar(autenticacaoDto.Login, autenticacaoDto.Senha));
    }
    
    [HttpPost("revalidar")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoPerfilUsuarioDTO), 200)]
    [AllowAnonymous]
    public async Task<IActionResult> Revalidar([FromBody] AutenticacaoRevalidarDTO autenticacaoRevalidarDTO, [FromServices] IServicoUsuario servicoUsuario)
    {
        return Ok(await servicoUsuario.RevalidarToken(autenticacaoRevalidarDTO.Token));
    }

    [HttpPut("perfis/{perfilUsuarioId}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoPerfilUsuarioDTO), 200)]
    [Authorize("Bearer")]
    public async Task<IActionResult> AtualizarPerfil(Guid perfilUsuarioId,[FromServices] IServicoUsuario servicoUsuario)
    {
        return Ok(await servicoUsuario.AtualizarPerfil(perfilUsuarioId));
    }
}