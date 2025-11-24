using Microsoft.AspNetCore.Mvc;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Webapi.Filtros;

namespace SME.CDEP.Webapi.Controllers
{
    public class PainelGerencialController (IServicoPainelGerencial servicoPainelGerencial) : BaseController
    {
        [HttpGet("acervos-cadastrados")]
        [ProducesResponseType(typeof(List<PainelGerencialAcervosCadastradosDto>), 200)]
        [Produces("application/json")]
        [Permissao(Permissao.CadastroAcervo_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterAcervosCadastrados()
        {
            var acervosCadastrados =  await servicoPainelGerencial.ObterAcervosCadastradosAsync();
            return Ok(acervosCadastrados);
        }
    }
}