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
        var retorno = await servicoUsuario.CadastrarUsuarioExterno(usuarioExternoDto);
       
        return Ok(retorno);
    }
    
    [HttpGet("{login}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(DadosUsuarioDTO), 200)]  
    [Authorize("Bearer")]
    public async Task<IActionResult> MeusDados([FromRoute] string login, [FromServices]IServicoUsuario servicoUsuario)
    {
        var retorno = await servicoUsuario.ObterMeusDados(login);

        return Ok(retorno);
    }
    
    [HttpPut("{login}/senha")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Authorize("Bearer")]
    public async Task<IActionResult> AlterarSenha([FromRoute] string login, [FromBody] AlterarSenhaUsuarioDTO alterarSenhaUsuarioDto, [FromServices] IServicoUsuario servicoUsuario)
    {
        var retorno = await servicoUsuario.AlterarSenha(login, alterarSenhaUsuarioDto);
       
        return Ok(retorno);
    }
    
    [HttpPut("{login}/email")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Authorize("Bearer")]
    public async Task<IActionResult> AlterarEmail([FromRoute] string login, [FromBody] EmailUsuarioDTO emailUsuarioDto, [FromServices] IServicoUsuario servicoUsuario)
    {
        var retorno = await servicoUsuario.AlterarEmail(login, emailUsuarioDto.Email);
       
        return Ok(retorno);
    }
    
    [HttpPut("{login}/endereco")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Authorize("Bearer")]
    public async Task<IActionResult> AlterarEnderecoAcervo([FromRoute] string login, [FromBody] EnderecoUsuarioExternoDTO enderecoTelefoneUsuarioExternoDto, [FromServices] IServicoUsuario servicoUsuario)
    {
        var retorno = await servicoUsuario.AlterarEndereco(login, enderecoTelefoneUsuarioExternoDto);
       
        return Ok(retorno);
    }
    
    [HttpPut("{login}/telefone")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Authorize("Bearer")]
    public async Task<IActionResult> AlterarTelefoneAcervo([FromRoute] string login, [FromBody] TelefoneUsuarioExternoDTO telefoneUsuarioExternoDto, [FromServices] IServicoUsuario servicoUsuario)
    {
        var retorno = await servicoUsuario.AlterarTelefone(login,telefoneUsuarioExternoDto.Telefone);
       
        return Ok(retorno);
    }
}