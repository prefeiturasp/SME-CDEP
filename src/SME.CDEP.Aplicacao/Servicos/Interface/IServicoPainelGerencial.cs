using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface;

public interface IServicoPainelGerencial : IServicoAplicacao
{
    Task<List<PainelGerencialAcervosCadastradosDto>> ObterAcervosCadastradosAsync();
    Task<List<PainelGerencialQuantidadePesquisasMensaisDto>> ObterQuantidadePesquisasMensaisDoAnoAtualAsync();
    Task<List<PainelGerencialQuantidadeSolicitacaoMensalDto>> ObterQuantidadeSolicitacoesMensaisDoAnoAtualAsync();
    Task<List<PainelGerencialQuantidadeSolicitacaoPorTipoDeAcervoDto>> ObterQuantidadeDeSolicitacoesPorTipoAcervoAsync();
}