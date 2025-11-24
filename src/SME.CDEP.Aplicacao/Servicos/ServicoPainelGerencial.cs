using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos;

public class ServicoPainelGerencial(
    IMapper mapper, 
    IRepositorioPainelGerencial repositorioPainelGerencial,
    TimeProvider timeProvider) : IServicoPainelGerencial
{
    public async Task<List<PainelGerencialAcervosCadastradosDto>> ObterAcervosCadastradosAsync()
    {
        var acervos = await repositorioPainelGerencial.ObterAcervosCadastradosAsync();
        return mapper.Map<List<PainelGerencialAcervosCadastradosDto>>(acervos);
    }

    public async Task<List<PainelGerencialQuantidadePesquisasMensaisDto>> ObterQuantidadePesquisasMensaisDoAnoAtualAsync()
    {
        var ano = timeProvider.GetLocalNow().Year;
        var pesquisas = await repositorioPainelGerencial.ObterSumarioConsultasMensalAsync(ano);
        return mapper.Map<List<PainelGerencialQuantidadePesquisasMensaisDto>>(pesquisas);
    }
}