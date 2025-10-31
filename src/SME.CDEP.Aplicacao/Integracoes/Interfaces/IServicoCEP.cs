using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Integracoes.Interfaces;

public interface IServicoCEP
{
    Task<CEPDTO> BuscarCEP(string cep);
}