using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class AcervoDocumentalController : BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(AcervoDocumentalCadastroDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_I, Policy = "Bearer")]
    public async Task<IActionResult> Inserir([FromBody] AcervoDocumentalCadastroDTO acervoDocumental, [FromServices] IServicoAcervoDocumental servicoArteDocumental)
    {
        return Ok(await servicoArteDocumental.Inserir(acervoDocumental));
    }

    [HttpPut]
    [ProducesResponseType(typeof(AcervoDocumentalDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_A, Policy = "Bearer")]
    public async Task<IActionResult> Alterar([FromBody] AcervoDocumentalAlteracaoDTO acervoDocumental, [FromServices] IServicoAcervoDocumental servicoArteDocumental)
    {
        return Ok(await servicoArteDocumental.Alterar(acervoDocumental));
    }

    [HttpGet("{acervoId}")]
    [ProducesResponseType(typeof(AcervoDocumentalDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterPorId([FromRoute] long acervoId, [FromServices] IServicoAcervoDocumental servicoArteDocumental)
    {
        return Ok(await servicoArteDocumental.ObterPorId(acervoId));
    }

    [HttpDelete("{acervoId}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_E, Policy = "Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long acervoId, [FromServices] IServicoAcervoDocumental servicoArteDocumental)
    {
        return Ok(await servicoArteDocumental.Excluir(acervoId));
    }
}