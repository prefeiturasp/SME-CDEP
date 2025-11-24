using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface;

public interface IServicoPainelGerencial : IServicoAplicacao
{
    Task<List<PainelGerencialAcervosCadastradosDto>> ObterAcervosCadastradosAsync();
}