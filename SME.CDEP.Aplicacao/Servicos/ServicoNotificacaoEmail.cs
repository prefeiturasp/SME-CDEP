using System.Net.Mail;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

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
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            
            var emailRemetente = await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.EmailRemetente, anoAtual);
            var nomeRemetente = await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.NomeRemetenteEmail, anoAtual);
            var enderecoSMTP = await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoSMTP, anoAtual);
            var usuarioRemtenteEmail = await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.UsuarioRemetenteEmail, anoAtual);

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
