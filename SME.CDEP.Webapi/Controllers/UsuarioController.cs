﻿using Microsoft.AspNetCore.Authorization;
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
    
    [HttpPost("solicitar-recuperacao-senha")] 
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [AllowAnonymous]
    public async Task<IActionResult> SolicitarRecuperacaoSenha(string login, [FromServices] IServicoUsuario servicoUsuario)
    {
        return Ok(await servicoUsuario.SolicitarRecuperacaoSenha(login));
    }
    
    [HttpGet("valida-token-recuperacao-senha/{token}")] 
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [AllowAnonymous]
    public async Task<IActionResult> TokenRecuperacaoSenhaEstaValidoAsync(Guid token, [FromServices] IServicoUsuario servicoUsuario)
    {
        return Ok(await servicoUsuario.TokenRecuperacaoSenhaEstaValido(token));
    }
    
    [HttpPost("recuperar-senha")] 
    [ProducesResponseType(typeof(UsuarioAutenticacaoRetornoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [AllowAnonymous]
    public async Task<IActionResult> RecuperarSenha([FromForm]RecuperacaoSenhaDto recuperacaoSenhaDto, [FromServices] IServicoUsuario servicoUsuario)
    {
        return Ok(await servicoUsuario.AlterarSenhaComTokenRecuperacao(recuperacaoSenhaDto));
    }
}