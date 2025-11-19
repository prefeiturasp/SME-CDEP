using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Controllers.Filtros;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class AcervoFotograficoController : BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(AcervoFotograficoCadastroDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_I, Policy = "Bearer")]
    public async Task<IActionResult> Inserir([FromBody] AcervoFotograficoCadastroDTO acervoFotografico, [FromServices] IServicoAcervoFotografico servicoAcervoFotografico)
    {
        return Ok(await servicoAcervoFotografico.Inserir(acervoFotografico));
    }

    [HttpPut]
    [ProducesResponseType(typeof(AcervoFotograficoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_A, Policy = "Bearer")]
    public async Task<IActionResult> Alterar([FromBody] AcervoFotograficoAlteracaoDTO acervoFotografico, [FromServices] IServicoAcervoFotografico servicoAcervoFotografico)
    {
        return Ok(await servicoAcervoFotografico.Alterar(acervoFotografico));
    }

    [HttpGet("{acervoId}")]
    [ProducesResponseType(typeof(AcervoFotograficoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterPorId([FromRoute] long acervoId, [FromServices] IServicoAcervoFotografico servicoAcervoFotografico)
    {
        return Ok(await servicoAcervoFotografico.ObterPorId(acervoId));
    }

    [HttpDelete("{acervoId}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_E, Policy = "Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long acervoId, [FromServices] IServicoAcervoFotografico servicoAcervoFotografico)
    {
        return Ok(await servicoAcervoFotografico.Excluir(acervoId));
    }
}