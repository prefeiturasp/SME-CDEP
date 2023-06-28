using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[Route("api/v1/autenticacao")]
[ValidaDto]
public class AutenticacaoController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(UsuarioAutenticacaoRetornoDTO), 200)]
    [AllowAnonymous]
    public async Task<IActionResult> Autenticar(AutenticacaoDTO autenticacaoDto, [FromServices] IServicoUsuario servicoUsuario)
    {
        var retornoAutenticacao = await servicoUsuario.Autenticar(autenticacaoDto.Login, autenticacaoDto.Senha);

        if (string.IsNullOrEmpty(retornoAutenticacao.Login))
            throw new NegocioException(MensagemNegocio.USUARIO_OU_SENHA_INVALIDOS);

        return Ok(retornoAutenticacao);
    }
    
    [HttpGet("usuarios/{login}/perfis")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(RetornoPerfilUsuarioDTO), 200)]        
    public async Task<IActionResult> ListarPerfisUsuario(string login, [FromServices]IServicoPerfilUsuario servicoPerfilUsuario)
    {
        var retorno = await servicoPerfilUsuario.ObterPerfisUsuario(login);

        return Ok(retorno);
    }
}