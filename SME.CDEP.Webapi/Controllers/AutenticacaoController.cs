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
public class AutenticacaoController: ControllerBase
{
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(UsuarioAutenticacaoRetornoDTO), 200)]
    [AllowAnonymous]
    public async Task<IActionResult> Autenticar(AutenticacaoDTO autenticacaoDto, [FromServices] IServicoUsuario servicoUsuario)
    {
        var retornoAutenticacao = await servicoUsuario.Autenticar(autenticacaoDto.Login, autenticacaoDto.Senha);

        if (retornoAutenticacao == null)
            throw new NegocioException(MensagemNegocio.USUARIO_OU_SENHA_INVALIDOS);
        
        if (string.IsNullOrEmpty(retornoAutenticacao.Login))
            throw new NegocioException(MensagemNegocio.USUARIO_OU_SENHA_INVALIDOS);

        return Ok(retornoAutenticacao);
    }
    
    [HttpGet("usuarios/{login}/perfis")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoPerfilUsuarioDTO), 500)]        
    public async Task<IActionResult> ListarPerfisUsuario(string login, [FromServices]IServicoPerfilUsuario servicoPerfilUsuario)
    {
        var retorno = await servicoPerfilUsuario.ObterPerfisUsuario(login);

        if (retorno == null)
            throw new NegocioException(MensagemNegocio.PERFIS_DO_USUARIO_NAO_LOCALIZADOS_VERIFIQUE_O_LOGIN);

        if (retorno.PerfilUsuario == null)
        {
            await servicoPerfilUsuario.VincularPerfilExternoCoreSSO(login,new Guid(Constantes.PERFIL_EXTERNO_GUID));
            retorno = await servicoPerfilUsuario.ObterPerfisUsuario(login);
        }
        return Ok(retorno);
    }
}