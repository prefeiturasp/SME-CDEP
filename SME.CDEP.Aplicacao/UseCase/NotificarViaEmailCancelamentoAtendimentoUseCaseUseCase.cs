
using System.Text;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Aplicacao
{
    public class NotificarViaEmailCancelamentoAtendimentoUseCaseUseCase : INotificarViaEmailCancelamentoAtendimentoUseCase
    {
        private IRepositorioAcervoSolicitacaoItem repositorioAcervoSolicitacaoItem;
        private IServicoNotificacaoEmail servicoNotificacaoEmail;
        private IRepositorioParametroSistema repositorioParametroSistema;
        
        public NotificarViaEmailCancelamentoAtendimentoUseCaseUseCase(IRepositorioAcervoSolicitacaoItem repositorioAcervoSolicitacaoItem,
            IServicoNotificacaoEmail servicoNotificacaoEmail,IRepositorioParametroSistema repositorioParametroSistema)
        {
            this.repositorioAcervoSolicitacaoItem = repositorioAcervoSolicitacaoItem ?? throw new ArgumentNullException(nameof(repositorioAcervoSolicitacaoItem));
            this.servicoNotificacaoEmail = servicoNotificacaoEmail ?? throw new ArgumentNullException(nameof(servicoNotificacaoEmail));
            this.repositorioParametroSistema = repositorioParametroSistema ?? throw new ArgumentNullException(nameof(repositorioParametroSistema));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            if (param.Mensagem.EhNulo())
                throw new NegocioException(MensagemNegocio.PARAMETROS_INVALIDOS);

            long acervoSolicitacaoId = 0;
            
            if (!long.TryParse(param.Mensagem.ToString(),out acervoSolicitacaoId))
                throw new NegocioException(MensagemNegocio.PARAMETROS_INVALIDOS);

            var detalhesAcervo = await repositorioAcervoSolicitacaoItem.ObterDetalhamentoDosItensPorSolicitacaoOuItem(acervoSolicitacaoId, null);

            if (detalhesAcervo.Any(a=> a.Email.NaoEstaPreenchido()))
                throw new NegocioException(MensagemNegocio.SOLICITANTE_NAO_POSSUI_EMAIL);
            
            if (detalhesAcervo.NaoPossuiElementos())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_CONTEM_ACERVOS);
            
            var destinatario = detalhesAcervo.FirstOrDefault().Solicitante;
            
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;

            var modeloEmail = (await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.ModeloEmailCancelamentoSolicitacao, anoAtual)).Valor;

            var enderecoContatoCDEP = (await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita, anoAtual)).Valor;
            
            var textoEmail = modeloEmail
                .Replace("#NOME", destinatario)
                .Replace("#CONTEUDO_TABELA", GerarConteudoTabela(detalhesAcervo))
                .Replace("#LINK_FORMULARIO_CDEP", enderecoContatoCDEP);

            await servicoNotificacaoEmail.Enviar(destinatario, detalhesAcervo.FirstOrDefault().Email, "CDEP - Atendimento cancelado", textoEmail);
            
            return true;
        }
        
        private string GerarConteudoTabela(IEnumerable<AcervoSolicitacaoItemDetalhe> detalhesAcervo)
        {
            var conteudo = new StringBuilder();

            conteudo.AppendLine(@"
            <table>
            <thead>
                <tr>
                    <th>Solicitação</th>
                    <th>Item</th>
                    <th>Código/Tombo</th>
                    <th>Acervo</th>
                    <th>Tipo</th>
                    <th>Data da visita</th>                    
                </tr>
            </thead>
            <tbody>");
            
            foreach (var detalhe in detalhesAcervo)
            {
                conteudo.AppendLine($@"
                <tr>
                  <td>{detalhe.AcervoSolicitacaoId}</td>
                  <td>{detalhe.Id}</td>
                  <td>{detalhe.ObterCodigoTombo}</td>
                  <td>{detalhe.Titulo}</td>
                  <td>{detalhe.TipoAcervo.Descricao()}</td>
                  <td>{detalhe.ObterDataVisitaOuTraco}</td>
                </tr>");
            }

            conteudo.AppendLine(@"
            </tbody>
            </table>");
            
            return conteudo.ToString();
        }
    }
}