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
        [Permissao(Permissao.PainelGerencial_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterAcervosCadastrados()
        {
            var acervosCadastrados =  await servicoPainelGerencial.ObterAcervosCadastradosAsync();
            return Ok(acervosCadastrados);
        }

        [HttpGet("quantidade-pesquisas-mensais")]
        [ProducesResponseType(typeof(List<PainelGerencialQuantidadePesquisasMensaisDto>), 200)]
        [Produces("application/json")]
        [Permissao(Permissao.PainelGerencial_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuantidadePesquisasMensais()
        {
            var quantidadePesquisasMensais = await servicoPainelGerencial.ObterQuantidadePesquisasMensaisDoAnoAtualAsync();
            return Ok(quantidadePesquisasMensais);
        }

        [HttpGet("quantidade-solicitacoes-mensais")]
        [ProducesResponseType(typeof(List<PainelGerencialQuantidadeSolicitacaoMensalDto>), 200)]
        [Produces("application/json")]
        [Permissao(Permissao.PainelGerencial_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuantidadeSolicitacoesMensais()
        {
            var quantidadeSolicitacoesMensais = await servicoPainelGerencial.ObterQuantidadeSolicitacoesMensaisDoAnoAtualAsync();
            return Ok(quantidadeSolicitacoesMensais);
        }

        [HttpGet("solicitacoes-tipo-acervo")]
        [ProducesResponseType(typeof(List<PainelGerencialQuantidadeSolicitacaoPorTipoDeAcervoDto>), 200)]
        [Produces("application/json")]
        [Permissao(Permissao.PainelGerencial_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuantidadeSolicitacoesPorTipoAcervo()
        {
            var quantidadeSolicitacoesPorTipoAcervo = await servicoPainelGerencial.ObterQuantidadeDeSolicitacoesPorTipoAcervoAsync();
            return Ok(quantidadeSolicitacoesPorTipoAcervo);
        }
    }
}