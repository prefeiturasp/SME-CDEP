using System.Net.Mail;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoNotificacaoEmail : ServicoNotificacao, IServicoNotificacaoEmail
    {
        private readonly string nomeDestinatario;
        private readonly string emailDestinatario;
        private readonly string assunto;
        private readonly string mensagem;
        private readonly IRepositorioParametroSistema repositorioParametroSistema;

        public ServicoNotificacaoEmail(string nomeDestinatario, string emailDestinatario, string assunto, string mensagem,
            IRepositorioParametroSistema repositorioParametroSistema)
        {
            this.nomeDestinatario = nomeDestinatario;
            this.emailDestinatario = emailDestinatario;
            this.assunto = assunto;
            this.mensagem = mensagem;
            this.repositorioParametroSistema = repositorioParametroSistema;
        }

        public override async Task<bool> Enviar()
        {
            var emailRemetente = 

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(configuracaoEmail.Nome, configuracaoEmail.Email));
            message.To.Add(new MailboxAddress(nomeDestinatario, emailDestinatario));
            message.Subject = assunto;

            message.Body = new TextPart("html")
            {
                Text = mensagemHtml
            };

            using (var client = new SmtpClient())
            {
                client.Connect(configuracaoEmail.Smtp, configuracaoEmail.Porta, configuracaoEmail.TLS);

                client.Authenticate(configuracaoEmail.Usuario, configuracaoEmail.Senha);

                client.Send(message);
                client.Disconnect(true);
            }

            return true;
        }
    }
}
