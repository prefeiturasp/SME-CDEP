using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class EventoController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(long), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao., Policy = "Bearer")]
    public async Task<IActionResult> Inserir([FromBody] AcervoSolicitacaoItemCadastroDTO[] acervosSolicitacaoItensCadastroDTO, [FromServices] IServicoAcervoSolicitacao servicoAcervoSolicitacao)
    {
        return Ok(await servicoAcervoSolicitacao.Inserir(acervosSolicitacaoItensCadastroDTO));
    }
}