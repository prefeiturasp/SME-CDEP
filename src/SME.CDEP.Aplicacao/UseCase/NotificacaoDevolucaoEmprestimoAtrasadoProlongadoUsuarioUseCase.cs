
using System.Text;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.Aplicacao
{
    public class NotificacaoDevolucaoEmprestimoAtrasadoProlongadoUsuarioUseCase(IRepositorioParametroSistema repositorioParametroSistema,
        IServicoNotificacaoEmail servicoNotificacaoEmail) : NotificacaoEmailBaseUseCase(repositorioParametroSistema, servicoNotificacaoEmail), INotificacaoDevolucaoEmprestimoAtrasadoProlongadoUsuarioUseCase
    {
        public async Task<bool> Executar(MensagemRabbit param)
        {
            await CarregarParametros();
            
            var acervoEmAtrasoDevolucaoEmprestimo = param.ObterObjetoMensagem<AcervoEmprestimoDevolucao>();
            
            if (acervoEmAtrasoDevolucaoEmprestimo.EhNulo())
                throw new NegocioException(MensagemNegocio.PARAMETROS_INVALIDOS);

            var conteudoEmail = await MontarDadosNoTemplateEmail(acervoEmAtrasoDevolucaoEmprestimo.Solicitante,
                GerarConteudoTabela(acervoEmAtrasoDevolucaoEmprestimo), TipoParametroSistema.ModeloEmailAvisoAtrasoProlongadoDevolucaoEmprestimo);

            conteudoEmail = conteudoEmail.Replace("#DATA_DEVOLUCAO_PROGRAMADA", acervoEmAtrasoDevolucaoEmprestimo.DataDevolucao.ToString("dd/MM"));

            await EnviarEmail(acervoEmAtrasoDevolucaoEmprestimo.Solicitante,
                acervoEmAtrasoDevolucaoEmprestimo.Email,
                "CDEP - Aviso de devolução de empréstimo em atraso prolongado", conteudoEmail);
            
            return true;
        }
        
        private string GerarConteudoTabela(AcervoEmprestimoDevolucao acervoEmprestimoDevolucao)
        {
            var conteudo = new StringBuilder();

            conteudo.AppendLine($@"
            <table>
            <thead>
                <tr>
                    <th>Solicitação</th>
                    <th>Item</th>
                    <th>Código</th>
                    <th>Acervo</th>
                    <th>Data do empréstimo</th>
                    <th>Data da devolução</th>                    
                </tr>
            </thead>
            <tbody>
                <tr>
                  <td>{acervoEmprestimoDevolucao.AcervoSolicitacaoId}</td>
                  <td>{acervoEmprestimoDevolucao.AcervoSolicitacaoItemId}</td>
                  <td>{acervoEmprestimoDevolucao.Codigo}</td>
                  <td>{acervoEmprestimoDevolucao.Titulo}</td>
                  <td>{acervoEmprestimoDevolucao.DataEmprestimo.ToString("dd/MM HH:mm")}</td>
                  <td>{acervoEmprestimoDevolucao.DataDevolucao.ToString("dd/MM")}</td>
                </tr>
            </tbody>
            </table>");
            
            return conteudo.ToString();
        }
    }
} 