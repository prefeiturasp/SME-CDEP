using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces;

public interface IRepositorioPainelGerencial 
{ 
    Task<List<PainelGerencialAcervosCadastrados>> ObterAcervosCadastradosAsync();
    Task<List<SumarioConsultaMensal>> ObterSumarioConsultasMensalAsync(int ano);
    Task<List<PainelGerencialQuantidadeSolicitacaoMensal>> ObterQuantidadeSolicitacoesMensaisAsync(int ano);
    Task<List<PainelGerencialQuantidadeDeSolicitacoesPorTipoAcervo>> ObterQuantidadeDeSolicitacoesPorTipoAcervoAsync();
    Task<List<PainelGerencialQuantidadeAcervoEmprestadoPorSituacao>> ObterQuantidadeAcervoEmprestadoPorSituacaoAsync();
}