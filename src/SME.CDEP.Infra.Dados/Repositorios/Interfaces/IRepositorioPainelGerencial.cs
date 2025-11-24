using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces;

public interface IRepositorioPainelGerencial 
{ 
    Task<List<PainelGerencialAcervosCadastrados>> ObterAcervosCadastrados();
}