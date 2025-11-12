using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Controllers.Filtros;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ValidaDto]
public class MaterialController : BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(IdNomeTipoExcluidoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_I, Policy = "Bearer")]
    public async Task<IActionResult> Inserir([FromBody] NomeTipoDTO material, [FromServices] IServicoMaterial servicoMaterial)
    {
        return Ok(await servicoMaterial.Inserir(new IdNomeTipoExcluidoDTO() { Nome = material.Nome, Tipo = material.Tipo }));
    }

    [HttpPut]
    [ProducesResponseType(typeof(IdNomeTipoExcluidoDTO), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_A, Policy = "Bearer")]
    public async Task<IActionResult> Alterar([FromBody] IdNomeTipoDTO material, [FromServices] IServicoMaterial servicoMaterial)
    {
        return Ok(await servicoMaterial.Alterar(new IdNomeTipoExcluidoDTO() { Id = material.Id, Nome = material.Nome, Tipo = material.Tipo }));
    }

    [HttpGet]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeTipoExcluidoDTO), 200)]
    [Permissao(Permissao.CadastroAcervo_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterTodos(TipoMaterial tipoMaterial, [FromServices] IServicoMaterial servicoMaterial)
    {
        var materiais = await servicoMaterial.ObterTodos();
        return Ok(tipoMaterial == TipoMaterial.NAO_DEFINIDO ? materiais : materiais.Where(w => w.Tipo == (int)tipoMaterial));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(IdNomeTipoExcluidoDTO), 200)]
    [Permissao(Permissao.CadastroAcervo_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterTodos([FromRoute] long id, [FromServices] IServicoMaterial servicoMaterial)
    {
        return Ok(await servicoMaterial.ObterPorId(id));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [ProducesResponseType(typeof(bool), 200)]
    [Permissao(Permissao.CadastroAcervo_E, Policy = "Bearer")]
    public async Task<IActionResult> ExclusaoLogica([FromRoute] long id, [FromServices] IServicoMaterial servicoMaterial)
    {
        return Ok(await servicoMaterial.Excluir(id));
    }
}