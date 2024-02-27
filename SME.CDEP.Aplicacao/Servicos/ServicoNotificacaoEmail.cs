using MailKit.Net.Smtp;
using MimeKit;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
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

        public Task NotificarCancelamentoAtendimento(IEnumerable<AcervoSolicitacaoItemDetalhe> detalhesAcervo)
        {
            var caminho = $"{Directory.GetCurrentDirectory()}/wwwroot/ModelosEmail/ValidacaoEmail_Conecta.txt";
            
            var textoArquivo = File.ReadAllText(caminho);
            
            var textoEmail = textoArquivo
                .Replace("#NOME", nomeUsuario)
                .Replace("#SISTEMA", nomeSistema)
                .Replace("#LINK", string.Format(endereco,token));
        }
    }
}
