using SME.CDEP.Aplicacao.UseCase.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Aplicacao.UseCase
{
    public class AtualizarSituacaoDasSolicitacoesDeAcervoVencidasUseCase(IRepositorioAcervoSolicitacaoItem repositorioAcervo, IRepositorioParametroSistema repositorioParametroSistema) : IAtualizarSituacaoDasSolicitacoesDeAcervoVencidasUseCase
    {
        public async Task<bool> Executar(MensagemRabbit param)
        {
            List<SituacaoSolicitacaoItem> situacaoParaIgnorar = [
                SituacaoSolicitacaoItem.CANCELADO,
                SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE,
                SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE,
                SituacaoSolicitacaoItem.SEM_RESPOSTA_SOLICITANTE
            ];

            var prazoEmDiasParaAtualizacao = await repositorioParametroSistema.ObterParametroPorTipoAsync(TipoParametroSistema.PrazoEncerramentoAutomaticoSolicitacao)
                                          ?? throw new NegocioException(string.Format(MensagemNegocio.PARAMETRO_NAO_ENCONTRADO_TIPO_X, TipoParametroSistema.PrazoEncerramentoAutomaticoSolicitacao));

            if (!int.TryParse(prazoEmDiasParaAtualizacao.Valor, out var prazoEmDias))
                throw new NegocioException(string.Format(MensagemNegocio.PARAMETRO_TIPO_X_INVALIDO, TipoParametroSistema.PrazoEncerramentoAutomaticoSolicitacao));

            var solicitacoesVencidas = await repositorioAcervo.ObterSolicitacoesDeAcervoVencidasAsync(situacaoParaIgnorar, prazoEmDias);

            if (solicitacoesVencidas is null || !solicitacoesVencidas.Any())
                return true;

            foreach (var solicitacao in solicitacoesVencidas)
                await repositorioAcervo.AtualizarSituacaoSolicitacaoItemAsync(solicitacao, SituacaoSolicitacaoItem.SEM_RESPOSTA_SOLICITANTE);

            return true;
        }
    }
}
