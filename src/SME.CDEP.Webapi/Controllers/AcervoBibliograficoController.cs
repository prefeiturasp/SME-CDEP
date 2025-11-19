using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Controllers.Filtros;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class AcervoBibliograficoController : BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(AcervoBibliograficoCadastroDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_I, Policy = "Bearer")]
    public async Task<IActionResult> Inserir([FromBody] AcervoBibliograficoCadastroDTO acervoBibliografico, [FromServices] IServicoAcervoBibliografico servicoAcervoBibliografico)
    {
        return Ok(await servicoAcervoBibliografico.Inserir(acervoBibliografico));
    }

    [HttpPut]
    [ProducesResponseType(typeof(AcervoBibliograficoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_A, Policy = "Bearer")]
    public async Task<IActionResult> Alterar([FromBody] AcervoBibliograficoAlteracaoDTO acervoBibliografico, [FromServices] IServicoAcervoBibliografico servicoAcervoBibliografico)
    {
        return Ok(await servicoAcervoBibliografico.Alterar(acervoBibliografico));
    }

    [HttpGet("{acervoId}")]
    [ProducesResponseType(typeof(AcervoBibliograficoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterPorId([FromRoute] long acervoId, [FromServices] IServicoAcervoBibliografico servicoAcervoBibliografico)
    {
        return Ok(await servicoAcervoBibliografico.ObterPorId(acervoId));
    }

    [HttpDelete("{acervoId}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_E, Policy = "Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long acervoId, [FromServices] IServicoAcervoBibliografico servicoAcervoBibliografico)
    {
        return Ok(await servicoAcervoBibliografico.Excluir(acervoId));
    }
}