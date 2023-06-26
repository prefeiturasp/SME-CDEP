using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
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
    
    [HttpGet("{login}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(DadosUsuarioDTO), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> MeusDados(string login, [FromServices]IServicoUsuario servicoUsuario)
    {
        var retorno = await servicoUsuario.ObterMeusDados(login);

        return Ok(retorno);
    }
    
    [HttpPut("senha")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Authorize("Bearer")]
    public async Task<IActionResult> AlterarSenha(string login, string senhaAtual, string senhaNova, string confirmarSenha, [FromServices] IServicoUsuario servicoUsuario)
    {
        var retorno = await servicoUsuario.AlterarSenha(login, senhaAtual, senhaNova, confirmarSenha);
       
        return Ok(retorno);
    }
    
    [HttpPut("{login}/email")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Authorize("Bearer")]
    public async Task<IActionResult> AlterarEmail(string login, string email, [FromServices] IServicoUsuario servicoUsuario)
    {
        var retorno = await servicoUsuario.AlterarEmail(login, email);
       
        return Ok(retorno);
    }
    
    [HttpPut("endereco")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Authorize("Bearer")]
    public async Task<IActionResult> AlterarEnderecoAcervo(EnderecoTelefoneUsuarioExternoDTO enderecoTelefoneUsuarioExternoDto, [FromServices] IServicoUsuario servicoUsuario)
    {
        var retorno = await servicoUsuario.AlterarEndereco(enderecoTelefoneUsuarioExternoDto);
       
        return Ok(retorno);
    }
    
    [HttpPut("telefone")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Authorize("Bearer")]
    public async Task<IActionResult> AlterarTelefoneAcervo(EnderecoTelefoneUsuarioExternoDTO TelefoneUsuarioExternoDto, [FromServices] IServicoUsuario servicoUsuario)
    {
        var retorno = await servicoUsuario.AlterarTelefone(TelefoneUsuarioExternoDto);
       
        return Ok(retorno);
    }
}