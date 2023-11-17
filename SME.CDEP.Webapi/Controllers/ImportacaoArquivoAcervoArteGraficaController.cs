﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[Route("api/v1/acervo/artegrafica/importacao/planilha")]
[ValidaDto]
public class ImportacaoArquivoAcervoArteGraficaController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoArteGraficaDTO,AcervoArteGraficaLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>),200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Permissao(Permissao.ACR_I, Policy = "Bearer")]
    public async Task<IActionResult> ImportarArquivo(IFormFile file,[FromServices] IServicoImportacaoArquivoAcervoArteGrafica servicoImportacaoArquivoAcervoArteGrafica)
    {
        return Ok(await servicoImportacaoArquivoAcervoArteGrafica.ImportarArquivo(file));
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoArteGraficaDTO,AcervoArteGraficaLinhaRetornoDTO>,AcervoLinhaRetornoSucessoDTO>),200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Permissao(Permissao.ACR_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterImportacaoPendente([FromServices] IServicoImportacaoArquivoAcervoArteGrafica servicoImportacaoArquivoAcervoArteGrafica)
    {
        return Ok(await servicoImportacaoArquivoAcervoArteGrafica.ObterImportacaoPendente());
    }
    
    [HttpPatch("{Id}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ACR_E, Policy = "Bearer")]
    public async Task<IActionResult> RemoverLinhaDoArquivo([FromRoute] long id, [FromBody] LinhaDTO linha, [FromServices] IServicoImportacaoArquivoAcervoArteGrafica servicoImportacaoArquivoAcervoArteGrafica)
    {
        return Ok(await servicoImportacaoArquivoAcervoArteGrafica.RemoverLinhaDoArquivo(id, linha.NumeroLinha));
    }
    
    [HttpPatch("atualizar-linha/{id}/sucesso")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.ACR_A, Policy = "Bearer")]
    public async Task<IActionResult> AtualizarLinhaParaSucesso([FromRoute] long id, [FromBody] LinhaDTO linha, [FromServices] IServicoImportacaoArquivoAcervoArteGrafica servicoImportacaoArquivoAcervoArteGrafica)
    {
        return Ok(await servicoImportacaoArquivoAcervoArteGrafica.AtualizarLinhaParaSucesso(id,linha.NumeroLinha));
    }
}