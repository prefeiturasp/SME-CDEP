using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Controllers.Filtros;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers;

[Route("api/v1/acervo/bibliografico/importacao/planilha")]
[ValidaDto]
public class ImportacaoArquivoAcervoBibliograficoController : BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoBibliograficoDTO, AcervoBibliograficoLinhaRetornoDTO>, AcervoLinhaRetornoSucessoDTO>), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Permissao(Permissao.CadastroAcervo_I, Policy = "Bearer")]
    public async Task<IActionResult> ImportarArquivo(IFormFile file, [FromServices] IServicoImportacaoArquivoAcervoBibliografico servicoImportacaoArquivoAcervoBibliografico)
    {
        return Ok(await servicoImportacaoArquivoAcervoBibliografico.ImportarArquivo(file));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoBibliograficoDTO, AcervoBibliograficoLinhaRetornoDTO>, AcervoLinhaRetornoSucessoDTO>), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Permissao(Permissao.CadastroAcervo_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterImportacaoPendente([FromServices] IServicoImportacaoArquivoAcervoBibliografico servicoImportacaoArquivoAcervoBibliografico)
    {
        return Ok(await servicoImportacaoArquivoAcervoBibliografico.ObterImportacaoPendente());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ImportacaoArquivoRetornoDTO<AcervoLinhaErroDTO<AcervoBibliograficoDTO, AcervoBibliograficoLinhaRetornoDTO>, AcervoLinhaRetornoSucessoDTO>), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
    [Permissao(Permissao.CadastroAcervo_C, Policy = "Bearer")]
    public async Task<IActionResult> ObterImportacaoPorId([FromRoute] long id, [FromServices] IServicoImportacaoArquivoAcervoBibliografico servicoImportacaoArquivoAcervoBibliografico)
    {
        return Ok(await servicoImportacaoArquivoAcervoBibliografico.ObterImportacaoPorId(id));
    }

    [HttpPatch("{Id}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_E, Policy = "Bearer")]
    public async Task<IActionResult> RemoverLinhaDoArquivo([FromRoute] long id, [FromBody] LinhaDTO linha, [FromServices] IServicoImportacaoArquivoAcervoBibliografico servicoImportacaoArquivoAcervoBibliografico)
    {
        return Ok(await servicoImportacaoArquivoAcervoBibliografico.RemoverLinhaDoArquivo(id, linha));
    }

    [HttpPatch("atualizar-linha/{id}/sucesso")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 403)]
    [ProducesResponseType(typeof(RetornoBaseDTO), 601)]
    [Permissao(Permissao.CadastroAcervo_A, Policy = "Bearer")]
    public async Task<IActionResult> AtualizarLinhaParaSucesso([FromRoute] long id, [FromBody] LinhaDTO linha, [FromServices] IServicoImportacaoArquivoAcervoBibliografico servicoImportacaoArquivoAcervoBibliografico)
    {
        return Ok(await servicoImportacaoArquivoAcervoBibliografico.AtualizarLinhaParaSucesso(id, linha));
    }
}