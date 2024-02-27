
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Aplicacao
{
    public class NotificarViaEmailCancelamentoAtendimento : INotificarViaEmailCancelamentoAtendimento
    {
        private IRepositorioAcervoSolicitacaoItem repositorioAcervoSolicitacaoItem;
        private IServicoNotificacaoEmail servicoNotificacaoEmail;
        
        public NotificarViaEmailCancelamentoAtendimento(IRepositorioAcervoSolicitacaoItem repositorioAcervoSolicitacaoItem,IServicoNotificacaoEmail servicoNotificacaoEmail)
        {
            this.repositorioAcervoSolicitacaoItem = repositorioAcervoSolicitacaoItem ?? throw new ArgumentNullException(nameof(repositorioAcervoSolicitacaoItem));
            this.servicoNotificacaoEmail = servicoNotificacaoEmail ?? throw new ArgumentNullException(nameof(servicoNotificacaoEmail));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            if (param.Mensagem.EhNulo())
                throw new NegocioException(MensagemNegocio.PARAMETROS_INVALIDOS);

            long acervoSolicitacaoId = 0;
            
            if (long.TryParse(param.Mensagem.ToString(),out acervoSolicitacaoId))
                throw new NegocioException(MensagemNegocio.PARAMETROS_INVALIDOS);

            var detalhesAcervo = await repositorioAcervoSolicitacaoItem.ObterDetalhementoDosItensPorSolicitacaoOuItem(acervoSolicitacaoId, null);

            await servicoNotificacaoEmail.NotificarCancelamentoAtendimento(detalhesAcervo);
            
            return true;
        }
    }
}