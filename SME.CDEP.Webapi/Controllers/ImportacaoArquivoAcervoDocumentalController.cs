﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[Route("api/v1/acervo/documental/importacao/planilha")]
[ValidaDto]
public class ImportacaoArquivoAcervoDocumentalController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>),200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Permissao(Permissao.CadastroAcervo_I, Policy = "Bearer")]
    public async Task<IActionResult> ImportarArquivo(IFormFile file,[FromServices] IServicoImportacaoArquivoAcervoDocumental servicoImportacaoArquivoAcervoDocumental)
    {
        return Ok(await servicoImportacaoArquivoAcervoDocumental.ImportarArquivo(file));
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>),200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Permissao(Permissao.CadastroAcervo_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterImportacaoPendente([FromServices] IServicoImportacaoArquivoAcervoDocumental servicoImportacaoArquivoAcervoDocumental)
    {
        return Ok(await servicoImportacaoArquivoAcervoDocumental.ObterImportacaoPendente());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoDocumentalDTO,AcervoDocumentalLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>),200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Permissao(Permissao.CadastroAcervo_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterImportacaoPorId([FromRoute] long id, [FromServices] IServicoImportacaoArquivoAcervoDocumental servicoImportacaoArquivoAcervoDocumental)
    {
        return Ok(await servicoImportacaoArquivoAcervoDocumental.ObterImportacaoPorId(id));
    }
    
    [HttpPatch("{Id}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_E, Policy = "Bearer")]
    public async Task<IActionResult> RemoverLinhaDoArquivo([FromRoute] long id, [FromBody] LinhaDTO linha, [FromServices] IServicoImportacaoArquivoAcervoDocumental servicoImportacaoArquivoAcervoDocumental)
    {
        return Ok(await servicoImportacaoArquivoAcervoDocumental.RemoverLinhaDoArquivo(id, linha));
    }
    
    [HttpPatch("atualizar-linha/{id}/sucesso")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_A, Policy = "Bearer")]
    public async Task<IActionResult> AtualizarLinhaParaSucesso([FromRoute] long id, [FromBody] LinhaDTO linha, [FromServices] IServicoImportacaoArquivoAcervoDocumental servicoImportacaoArquivoAcervoDocumental)
    {
        return Ok(await servicoImportacaoArquivoAcervoDocumental.AtualizarLinhaParaSucesso(id,linha));
    }
}