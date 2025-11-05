
using SME.CDEP.Dominio.Entidades;

namespace SME.CDEP.Aplicacao
{
    public interface INotificacaoBaseUseCase
    {
        Task CarregarParametros();
        Task<string> MontarDadosNoTemplateEmail(string nomeDestinatario, string conteudoInterno, TipoParametroSistema parametroTemplateEmail);
        Task EnviarEmail(string nomeDestinatario, string emailDestinatario, string assunto, string conteudoEmail);
    }
}