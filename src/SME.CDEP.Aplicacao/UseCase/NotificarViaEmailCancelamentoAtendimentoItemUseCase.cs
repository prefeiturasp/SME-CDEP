
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
    public class NotificarViaEmailCancelamentoAtendimentoItemUseCase(IRepositorioAcervoSolicitacaoItem repositorioAcervoSolicitacaoItem,
        IServicoNotificacaoEmail servicoNotificacaoEmail, IRepositorioParametroSistema repositorioParametroSistema) : INotificarViaEmailCancelamentoAtendimentoItemUseCase
    {
        public async Task<bool> Executar(MensagemRabbit param)
        {
            if (param.Mensagem.EhNulo())
                throw new NegocioException(MensagemNegocio.PARAMETROS_INVALIDOS);

            long acervoSolicitacaoItemId = 0;
            
            if (!long.TryParse(param.Mensagem.ToString(),out acervoSolicitacaoItemId))
                throw new NegocioException(MensagemNegocio.PARAMETROS_INVALIDOS);

            var detalhesAcervo = await repositorioAcervoSolicitacaoItem.ObterDetalhamentoDosItensPorSolicitacaoOuItem(null,acervoSolicitacaoItemId);

            if (detalhesAcervo.Any(a=> string.IsNullOrWhiteSpace(a.Email)))
                throw new NegocioException(MensagemNegocio.SOLICITANTE_NAO_POSSUI_EMAIL);
            
            if (detalhesAcervo is null || !detalhesAcervo.Any())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_CONTEM_ACERVOS);
            
            var destinatario = detalhesAcervo.First().Solicitante!;
            
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;

            var modeloEmail = (await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.ModeloEmailCancelamentoSolicitacaoItem, anoAtual)).Valor;

            var enderecoContatoCDEP = (await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita, anoAtual)).Valor;
            
            var textoEmail = modeloEmail
                .Replace("#NOME", destinatario)
                .Replace("#CONTEUDO_TABELA", GerarConteudoTabela(detalhesAcervo.First()))
                .Replace("#LINK_FORMULARIO_CDEP", enderecoContatoCDEP);
            
            await servicoNotificacaoEmail.Enviar(destinatario, detalhesAcervo.First().Email!, "CDEP - Atendimento item cancelado", textoEmail);
            
            return true;
        }
        
        private static string GerarConteudoTabela(AcervoSolicitacaoItemDetalhe detalhe)
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
            
            conteudo.AppendLine($@"
            <tr>
              <td>{detalhe.AcervoSolicitacaoId}</td>
              <td>{detalhe.Id}</td>
              <td>{detalhe.ObterCodigoTombo}</td>
              <td>{detalhe.Titulo}</td>
              <td>{detalhe.TipoAcervo.Descricao()}</td>
              <td>{detalhe.ObterDataVisitaOuTraco}</td>
            </tr>");

            conteudo.AppendLine(@"
            </tbody>
            </table>");
            
            return conteudo.ToString();
        }
    }
}