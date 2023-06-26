using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[Route("api/v1/usuarios")]
[ValidaDto]
public class UsuarioController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(UsuarioExternoDTO), 200)]
    public async Task<IActionResult> CadastrarUsuarioExterno(UsuarioExternoDTO usuarioExternoDto, [FromServices] IServicoUsuario servicoUsuario)
    {
        return Ok(await servicoUsuario.CadastrarUsuarioExterno(usuarioExternoDto));
    }
    
    [HttpPost("solicitar-recuperacao-senha")] //Esses métodos no SGP são no AutenticarController
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [AllowAnonymous]
    public async Task<IActionResult> SolicitarRecuperacaoSenha(string login, [FromServices] IServicoRecuperacaoSenha servicoRecuperacaoSenha)
    {
        return Ok(await servicoRecuperacaoSenha.SolicitarRecuperacaoSenha(login));
    }
    
    [HttpGet("valida-token-recuperacao-senha/{token}")] //Esses métodos no SGP são no AutenticarController
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [AllowAnonymous]
    public async Task<IActionResult> TokenRecuperacaoSenhaEstaValidoAsync(Guid token, [FromServices] IServicoRecuperacaoSenha servicoRecuperacaoSenha)
    {
        return Ok(await servicoRecuperacaoSenha.TokenRecuperacaoSenhaEstaValido(token));
    }
    
    [HttpPost("recuperar-senha")] //Esses métodos no SGP são no AutenticarController
    [ProducesResponseType(typeof(UsuarioAutenticacaoRetornoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [AllowAnonymous]
    public async Task<IActionResult> RecuperarSenha([FromForm]RecuperacaoSenhaDto recuperacaoSenhaDto, [FromServices] IServicoRecuperacaoSenha servicoRecuperacaoSenha)
    {
        return Ok(await servicoRecuperacaoSenha.AlterarSenhaComTokenRecuperacao(recuperacaoSenhaDto));
    }
}