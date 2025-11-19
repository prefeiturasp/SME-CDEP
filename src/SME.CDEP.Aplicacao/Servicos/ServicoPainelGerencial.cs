using AutoMapper;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos;

public class ServicoPainelGerencial(IMapper mapper, IRepositorioPainelGerencial repositorioPainelGerencial) : IServicoPainelGerencial
{
}
