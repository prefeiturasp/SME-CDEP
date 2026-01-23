using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoProcessamentoSituacaoSolicitacao(
            IRepositorioAcervoSolicitacao repositorioSolicitacao,
            IRepositorioAcervoSolicitacaoItem repositorioItem) : IServicoProcessamentoSituacaoSolicitacao
    {
        public async Task AtualizarSituacaoGeralSolicitacaoAsync(AcervoSolicitacao acervoSolicitacao, bool todosItensEstaoCancelados = false)
        {
            var itens = await repositorioItem
                .ObterItensVigentesPorSolicitacaoIdAsync(acervoSolicitacao.Id);

            SituacaoSolicitacao novaSituacao;

            if (todosItensEstaoCancelados)
                novaSituacao = SituacaoSolicitacao.CANCELADO;
            else if (itens.All(a => a.Situacao == SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE))
                novaSituacao = SituacaoSolicitacao.FINALIZADO_ATENDIMENTO;
            else if (itens.Any(a => a.Situacao == SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO))
                novaSituacao = SituacaoSolicitacao.ATENDIDO_PARCIALMENTE;
            else if (itens.Any(a => a.Situacao == SituacaoSolicitacaoItem.AGUARDANDO_VISITA))
                novaSituacao = SituacaoSolicitacao.AGUARDANDO_VISITA;
            else if (itens.Any(a => a.Situacao == SituacaoSolicitacaoItem.PRESENCIAL_ABERTO))
                novaSituacao = SituacaoSolicitacao.PRESENCIAL_ABERTO;
            else
                throw new NegocioException(MensagemNegocio.SITUACAO_NAO_MAPEADA);

            acervoSolicitacao.Situacao = novaSituacao;

            await repositorioSolicitacao.Atualizar(acervoSolicitacao);
        }
    }
}
