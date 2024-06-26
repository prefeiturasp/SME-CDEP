﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class UsuarioController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioExternoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    public async Task<IActionResult> Inserir(UsuarioExternoDTO usuarioExternoDto, [FromServices] IServicoUsuario servicoUsuario)
    {
        return Ok(await servicoUsuario.InserirUsuarioExterno(usuarioExternoDto));
    }
    
    [HttpPost("{login}/solicitar-recuperacao-senha")] 
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [AllowAnonymous]
    public async Task<IActionResult> SolicitarRecuperacaoSenha([FromRoute] string login, [FromServices] IServicoUsuario servicoUsuario)
    {
        return Ok(await servicoUsuario.SolicitarRecuperacaoSenha(login));
    }
    
    [HttpGet("valida-token-recuperacao-senha/{token}")] 
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [AllowAnonymous]
    public async Task<IActionResult> TokenRecuperacaoSenhaEstaValidoAsync([FromRoute] Guid token, [FromServices] IServicoUsuario servicoUsuario)
    {
        return Ok(await servicoUsuario.TokenRecuperacaoSenhaEstaValido(token));
    }
    
    [HttpPut("recuperar-senha")] 
    [ProducesResponseType(typeof(RetornoPerfilUsuarioDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [AllowAnonymous]
    public async Task<IActionResult> RecuperarSenha([FromBody]RecuperacaoSenhaDto recuperacaoSenhaDto, [FromServices] IServicoUsuario servicoUsuario)
    {
        return Ok(await servicoUsuario.AlterarSenhaComTokenRecuperacao(recuperacaoSenhaDto));
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
    
    [HttpPut("{login}/tipo-usuario")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Authorize("Bearer")]
    public async Task<IActionResult> AlterarTipoUsuario([FromRoute] string login, [FromBody] TipoUsuarioExternoDTO tipoUsuario, [FromServices] IServicoUsuario servicoUsuario)
    {
        var retorno = await servicoUsuario.AlterarTipoUsuario(login, tipoUsuario);
       
        return Ok(retorno);
    }
    
    [HttpGet("{cpf}/existe")] 
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [AllowAnonymous]
    public async Task<IActionResult> ValidarCpfExistente([FromRoute] string cpf, [FromServices] IServicoUsuario servicoUsuario)
    {
        return Ok(await servicoUsuario.ValidarCpfExistente(cpf));
    }
    
    [HttpGet("dados-solicitante")] 
    [ProducesResponseType(typeof(DadosSolicitanteDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Permissao(Permissao.OperacoesSolicitacoes_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterDadosSolicitante([FromServices] IServicoUsuario servicoUsuario)
    {
        return Ok(await servicoUsuario.ObterDadosSolicitante());
    }
    
    [HttpGet("perfis/responsaveis")] 
    [ProducesResponseType(typeof(IEnumerable<ResponsavelDTO>), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Permissao(Permissao.OperacoesSolicitacoes_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterUsuariosComPerfisResponsavel([FromServices] IServicoUsuario servicoUsuario)
    {
        return Ok(await servicoUsuario.ObterUsuariosComPerfisResponsavel());
    }
    
    [HttpGet("{rfCpf}/dados-solicitante")] 
    [ProducesResponseType(typeof(DadosSolicitanteDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Permissao(Permissao.OperacoesSolicitacoes_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterDadosSolicitantePorRfOuCpf([FromRoute] string rfCpf, [FromServices] IServicoUsuario servicoUsuario)
    {
        return Ok(await servicoUsuario.ObterDadosSolicitantePorRfOuCpf(rfCpf));
    }
}
