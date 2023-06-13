using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[Route("api/v1/autenticacao")]
[ValidaDto]
public class AutenticacaoController: ControllerBase
{
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(UsuarioAutenticacaoRetornoDTO), 200)]
    [AllowAnonymous]
    public async Task<IActionResult> Autenticar(AutenticacaoDTO autenticacaoDto, [FromServices] IServicoUsuario servicoUsuario)
    {
        var retornoAutenticacao = await servicoUsuario.Autenticar(autenticacaoDto.Login, autenticacaoDto.Senha);

        if (string.IsNullOrEmpty(retornoAutenticacao.Login))
            return StatusCode(401);

        return Ok(retornoAutenticacao);
    }
    
    [HttpGet("usuarios/{login}/perfis")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(RetornoPerfilUsuarioDTO), 500)]        
    public async Task<IActionResult> ListarPerfisUsuario(string login, [FromServices]IServicoPerfilUsuario servicoPerfilUsuario)
    {
        var retorno = await servicoPerfilUsuario.ObterPerfisUsuario(login);

        return Ok(retorno);
    }
}