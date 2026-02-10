using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos.Fachadas
{
    public record ConfirmacaoAtendimentoRecursos(
            IRepositorioAcervoSolicitacao RepositorioSolicitacao,
            IRepositorioAcervoSolicitacaoItem RepositorioItem,
            IServicoUsuario ServicoUsuario,
            ITransacao Transacao,
            IServicoMensageria ServicoMensageria
        );
}
