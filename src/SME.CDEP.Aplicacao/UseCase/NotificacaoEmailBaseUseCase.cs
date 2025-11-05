using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao
{
    public class NotificacaoEmailBaseUseCase : INotificacaoBaseUseCase
    {
        private readonly IRepositorioParametroSistema repositorioParametroSistema;
        private readonly IServicoNotificacaoEmail servicoNotificacaoEmail;
        private string EnderecoContatoCDEP;
        private string EnderecoSedeCDEPVisita;
        private string HorarioFuncionamento;
        private int AnoAtual;
        
        public NotificacaoEmailBaseUseCase(IRepositorioParametroSistema repositorioParametroSistema,IServicoNotificacaoEmail servicoNotificacaoEmail)
        {
            this.repositorioParametroSistema = repositorioParametroSistema ?? throw new ArgumentNullException(nameof(repositorioParametroSistema));
            this.servicoNotificacaoEmail = servicoNotificacaoEmail ?? throw new ArgumentNullException(nameof(servicoNotificacaoEmail));
        }

        public async Task CarregarParametros()
        {
            AnoAtual = DateTimeExtension.HorarioBrasilia().Year;

            EnderecoContatoCDEP = (await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita, AnoAtual)).Valor;
            
            EnderecoSedeCDEPVisita = (await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoSedeCDEPVisita, AnoAtual)).Valor;
            
            HorarioFuncionamento = (await repositorioParametroSistema.ObterParametroPorTipoEAno(TipoParametroSistema.HorarioFuncionamentoSedeCDEPVisita, AnoAtual)).Valor;
        }

        public async Task<string> MontarDadosNoTemplateEmail(string nomeDestinatario, string conteudoInterno, TipoParametroSistema parametroTemplateEmail)
        {
            var modeloEmail = (await repositorioParametroSistema.ObterParametroPorTipoEAno(parametroTemplateEmail, AnoAtual)).Valor;
            
            return modeloEmail
                .Replace("#NOME", nomeDestinatario)
                .Replace("#CONTEUDO_TABELA", conteudoInterno)
                .Replace("#LINK_FORMULARIO_CDEP", EnderecoContatoCDEP)
                .Replace("#ENDERECO_SEDE_CDEP_VISITA", EnderecoSedeCDEPVisita)
                .Replace("#HORARIO_FUNCIONAMENTO_SEDE_CDEP", HorarioFuncionamento);
        }

        public async Task EnviarEmail(string nomeDestinatario, string emailDestinatario, string assunto, string conteudoEmail)
        {
            await servicoNotificacaoEmail.Enviar(nomeDestinatario, emailDestinatario, assunto, conteudoEmail);
        }
    }
}