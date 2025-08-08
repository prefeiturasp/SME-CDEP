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
        public async Task<IActionResult> GerarSync([FromBody] RelatorioControleLivroEmprestadosRequest filtros,
           [FromServices] IRelatorioControleLivrosEmprestadosUseCase relatorioControleLivrosEmprestadosUseCase)
        {
            return Ok(await relatorioControleLivrosEmprestadosUseCase.Executar(filtros));
        }
    }
}
