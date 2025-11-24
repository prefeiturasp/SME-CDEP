using AutoMapper;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos;

public class ServicoPainelGerencial(IMapper mapper, IRepositorioPainelGerencial repositorioPainelGerencial) : IServicoPainelGerencial
{
    public async Task<List<PainelGerencialAcervosCadastradosDto>> ObterAcervosCadastradosAsync()
    {
        var acervos = await repositorioPainelGerencial.ObterAcervosCadastrados();
        return mapper.Map<List<PainelGerencialAcervosCadastradosDto>>(acervos);
    }
}