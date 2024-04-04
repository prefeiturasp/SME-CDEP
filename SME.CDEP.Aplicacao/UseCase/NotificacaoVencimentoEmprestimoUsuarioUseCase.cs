
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
    public class NotificacaoVencimentoEmprestimoUsuarioUseCase : INotificacaoVencimentoEmprestimoUsuarioUseCase
    {
        private IRepositorioParametroSistema repositorioParametroSistema;
        private IServicoNotificacaoEmail servicoNotificacaoEmail;
        
        public NotificacaoVencimentoEmprestimoUsuarioUseCase(IRepositorioParametroSistema repositorioParametroSistema,IServicoNotificacaoEmail servicoNotificacaoEmail)
        {
            this.repositorioParametroSistema = repositorioParametroSistema ?? throw new ArgumentNullException(nameof(repositorioParametroSistema));
            this.servicoNotificacaoEmail = servicoNotificacaoEmail ?? throw new ArgumentNullException(nameof(servicoNotificacaoEmail));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var acervoEmprestimoAntesVencimentoDevolucao = param.ObterObjetoMensagem<AcervoEmprestimoAntesVencimentoDevolucao>();
            
            if (acervoEmprestimoAntesVencimentoDevolucao.EhNulo())
                throw new NegocioException(MensagemNegocio.PARAMETROS_INVALIDOS);
            
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;

            var modeloEmail = (await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.ModeloEmailAvisoDevolucaoEmprestimo, anoAtual)).Valor;

            var enderecoContatoCDEP = (await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita, anoAtual)).Valor;
            
            var enderecoSedeCDEPVisita = (await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoSedeCDEPVisita, anoAtual)).Valor;
            
            var horarioFuncionamento = (await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.HorarioFuncionamentoSedeCDEPVisita, anoAtual)).Valor;
            
            var textoEmail = modeloEmail
                .Replace("#NOME", acervoEmprestimoAntesVencimentoDevolucao.Solicitante)
                .Replace("#CONTEUDO_TABELA", GerarConteudoTabela(acervoEmprestimoAntesVencimentoDevolucao))
                .Replace("#LINK_FORMULARIO_CDEP", enderecoContatoCDEP)
                .Replace("#ENDERECO_SEDE_CDEP_VISITA", enderecoSedeCDEPVisita)
                .Replace("#HORARIO_FUNCIONAMENTO_SEDE_CDEP", horarioFuncionamento);
            
            await servicoNotificacaoEmail.Enviar(acervoEmprestimoAntesVencimentoDevolucao.Solicitante, acervoEmprestimoAntesVencimentoDevolucao.Email, "CDEP - Aviso de vencimento do empréstimo", textoEmail);
            
            return true;
        }
        
        private string GerarConteudoTabela(AcervoEmprestimoAntesVencimentoDevolucao acervoEmprestimoAntesVencimentoDevolucao)
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
                  <td>{acervoEmprestimoAntesVencimentoDevolucao.AcervoSolicitacaoId}</td>
                  <td>{acervoEmprestimoAntesVencimentoDevolucao.AcervoSolicitacaoItemId}</td>
                  <td>{acervoEmprestimoAntesVencimentoDevolucao.Codigo}</td>
                  <td>{acervoEmprestimoAntesVencimentoDevolucao.Titulo}</td>
                  <td>{acervoEmprestimoAntesVencimentoDevolucao.DataEmprestimo.ToString("dd/MM HH:mm")}</td>
                  <td>{acervoEmprestimoAntesVencimentoDevolucao.DataDevolucao.ToString("dd/MM")}</td>
                </tr>
            </tbody>
            </table>");
            
            return conteudo.ToString();
        }
    }
}