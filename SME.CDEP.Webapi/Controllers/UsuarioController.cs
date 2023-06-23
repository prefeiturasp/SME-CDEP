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
        var retorno = await servicoUsuario.CadastrarUsuarioExterno(usuarioExternoDto);
       
        return Ok(retorno);
    }
    
    [HttpPut("alterar-senha")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(UsuarioExternoDTO), 200)]
    // [Authorize("Bearer")]
    public async Task<IActionResult> AlterarSenha(string login, string senhaAtual, string senhaNova, string confirmarSenha, [FromServices] IServicoUsuario servicoUsuario)
    {
        var retorno = await servicoUsuario.AlterarSenha(login, senhaAtual, senhaNova, confirmarSenha);
       
        return Ok(retorno);
    }
}