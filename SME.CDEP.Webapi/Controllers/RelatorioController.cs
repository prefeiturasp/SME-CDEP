using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.UseCase.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios")]
    public class RelatorioController : ControllerBase
    {
        [HttpPost("controle-livros-emprestados")]
        [Permissao(Permissao.OperacoesSolicitacoes_C, Policy = "Bearer")]
        public async Task<IActionResult> RelatorioControleLivrosEmprestados([FromBody] RelatorioControleLivroEmprestadosRequest filtros,
           [FromServices] IRelatorioControleLivrosEmprestadosUseCase relatorioControleLivrosEmprestadosUseCase)
        {
            var file = await relatorioControleLivrosEmprestadosUseCase.Executar(filtros);
            if (file == null)
                return NotFound();

            return File(file, "application/vnd.ms-excel", "relatorio.xls", enableRangeProcessing: true);
        }

        [HttpPost("controle-acervo")]
        [Permissao(Permissao.OperacoesSolicitacoes_C, Policy = "Bearer")]
        public async Task<IActionResult> RelatorioControleAcervo([FromBody] RelatorioControleAcervoRequest filtros,
          [FromServices] IRelatorioControleAcervoUseCase relatorioControleAcervoUseCase)
        {
            var file = await relatorioControleAcervoUseCase.Executar(filtros);
            if (file == null)
                return NotFound();

            return File(file, "application/vnd.ms-excel", "relatorio.xls", enableRangeProcessing: true);
        }
    }
}
