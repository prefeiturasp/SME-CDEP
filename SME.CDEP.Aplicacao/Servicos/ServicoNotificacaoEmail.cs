using System.Text;
using MailKit.Net.Smtp;
using MimeKit;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoNotificacaoEmail : IServicoNotificacaoEmail
    {
        private readonly string nomeDestinatario;
        private readonly string emailDestinatario;
        private readonly string assunto;
        private readonly string mensagem;
        private readonly IRepositorioParametroSistema repositorioParametroSistema;

        public ServicoNotificacaoEmail(IRepositorioParametroSistema repositorioParametroSistema)
        {
            this.repositorioParametroSistema = repositorioParametroSistema ?? throw new ArgumentNullException(nameof(repositorioParametroSistema));
        }

        public async Task<bool> Enviar(string nomeDestinatario, string emailDestinatario, string assunto, string mensagem)
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            
            var emailRemetente = await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.EmailRemetente, anoAtual);
            var nomeRemetente = await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.NomeRemetenteEmail, anoAtual);
            var enderecoSMTP = await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoSMTP, anoAtual);
            var usuarioRemtenteEmail = await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.UsuarioRemetenteEmail, anoAtual);
            var senhaRemtenteEmail = await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.SenhaRemetenteEmail, anoAtual);
            var usarTLSEmail = await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.UsarTLSEmail, anoAtual);
            var portaEnvioEmail = await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.PortaEnvioEmail, anoAtual);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(nomeRemetente.Valor, emailRemetente.Valor));
            message.To.Add(new MailboxAddress(nomeDestinatario, emailDestinatario));
            message.Subject = assunto;

            message.Body = new TextPart("html")
            {
                Text = mensagem
            };

            using (var client = new SmtpClient())
            {
                client.Connect(enderecoSMTP.Valor, int.Parse(portaEnvioEmail.Valor), bool.Parse(usarTLSEmail.Valor));

                client.Authenticate(usuarioRemtenteEmail.Valor, senhaRemtenteEmail.Valor);

                client.Send(message);
                client.Disconnect(true);
            }

            return true;
        }

        public async Task NotificarCancelamentoAtendimento(IEnumerable<AcervoSolicitacaoItemDetalhe> detalhesAcervo)
        {
            var destinatario = detalhesAcervo.FirstOrDefault().Solicitante;
            
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;

            var modeloEmail = (await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.ModeloEmailCancelamentoSolicitacao, anoAtual)).Valor;

            var enderecoContatoCDEP = (await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita, anoAtual)).Valor;
            
            var textoEmail = modeloEmail
                .Replace("#NOME", destinatario)
                .Replace("#CONTEUDO_TABELA", GerarConteudoTabela(detalhesAcervo))
                .Replace("#ENDERECO_CDEP", enderecoContatoCDEP);

            await Enviar(destinatario, detalhesAcervo.FirstOrDefault().Email, "CDEP - Atendimento cancelado", textoEmail);
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
                var codigoTombo = ObterCodigoTombo(detalhe);
                
                var dataVisita = ObterDataVisita(detalhe);
                
                conteudo.AppendLine($@"
                <tr>
                  <td>{detalhe.AcervoSolicitacaoId}</td>
                  <td>{detalhe.Id}</td>
                  <td>{codigoTombo}</td>
                  <td>{detalhe.Titulo}</td>
                  <td>{detalhe.TipoAcervo.Descricao()}</td>
                  <td>{dataVisita}</td>
                </tr>");
            }

            conteudo.AppendLine(@"
            </tbody>
            </table>");
            
            return conteudo.ToString();
        }

        private static string ObterDataVisita(AcervoSolicitacaoItemDetalhe detalhe)
        {
            return detalhe.DataVisita.HasValue ? detalhe.DataVisita.Value.ToString("dd/MM/yyyy") : "-";
        }

        private static string ObterCodigoTombo(AcervoSolicitacaoItemDetalhe detalhe)
        {
            return detalhe.Codigo.EstaPreenchido() && detalhe.codigoNovo.EstaPreenchido()
                ? $"{detalhe.Codigo}/{detalhe.codigoNovo}"
                : detalhe.Codigo.EstaPreenchido()
                    ? detalhe.Codigo
                    : detalhe.codigoNovo;
        }
    }
}
