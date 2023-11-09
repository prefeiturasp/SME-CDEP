using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[ApiController]
[ValidaDto]
public class ImportacaoArquivoAcervoTridimensionalController: BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ImportacaoArquivoRetornoDTO<AcervoTridimensionalLinhaRetornoDTO>),200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Authorize("Bearer")] 
    public async Task<IActionResult> ImportarArquivo(IFormFile file,[FromServices] IServicoImportacaoArquivoAcervoTridimensional servicoImportacaoArquivoAcervoTridimensional)
    {
        return Ok(await servicoImportacaoArquivoAcervoTridimensional.ImportarArquivo(file));
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(ImportacaoArquivoRetornoDTO<AcervoTridimensionalLinhaRetornoDTO>),200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Authorize("Bearer")] 
    public async Task<IActionResult> ObterImportacaoPendente([FromServices] IServicoImportacaoArquivoAcervoTridimensional servicoImportacaoArquivoAcervoTridimensional)
    {
        return Ok(await servicoImportacaoArquivoAcervoTridimensional.ObterImportacaoPendente());
    }
}