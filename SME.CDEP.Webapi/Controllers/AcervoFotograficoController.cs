using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class AcervoFotograficoController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(AcervoFotograficoCadastroDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ACR_I, Policy = "Bearer")]
    public async Task<IActionResult> Inserir([FromBody] AcervoFotograficoCadastroDTO acervoFotografico, [FromServices] IServicoAcervoFotografico servicoAssuntoAcervoFotografico)
    {
        return Ok(await servicoAssuntoAcervoFotografico.Inserir(acervoFotografico));
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(AcervoFotograficoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 422)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ACR_A, Policy = "Bearer")]
    public async Task<IActionResult> Alterar([FromBody] AcervoFotograficoAlteracaoDTO acervoFotografico, [FromServices] IServicoAcervoFotografico servicoAssuntoAcervoFotografico)
    {
        return Ok(await servicoAssuntoAcervoFotografico.Alterar(acervoFotografico));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AcervoFotograficoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ACR_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterPorId([FromRoute] long id,[FromServices] IServicoAcervoFotografico servicoAssuntoAcervoFotografico)
    {
        return Ok(await servicoAssuntoAcervoFotografico.ObterPorId(id));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ACR_E, Policy = "Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long id, [FromServices] IServicoAcervoFotografico servicoAssuntoAcervoFotografico)
    {
        return Ok(await servicoAssuntoAcervoFotografico.Excluir(id));
    }
}