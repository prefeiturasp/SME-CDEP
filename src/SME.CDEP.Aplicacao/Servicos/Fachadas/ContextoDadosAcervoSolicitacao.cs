using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos.Fachadas
{
    public record ContextoDadosAcervoSolicitacao(
            IRepositorioAcervoSolicitacao RepositorioSolicitacao,
            IRepositorioAcervoSolicitacaoItem RepositorioItem,
            IRepositorioAcervoEmprestimo RepositorioAcervoEmprestimo,
            IRepositorioAcervo RepositorioAcervo);
}
