using Newtonsoft.Json;
using Shouldly;
using SME.CDEP.Aplicacao;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_notificar_vencimento_emprestimo : TesteBase
    {
        public Ao_notificar_vencimento_emprestimo(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo Emprestimo - Notificar vencimento de empréstimo")]
        public async Task Deve_notificar_vencimento_de_emprestimo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();
            
            var notificarQtdeDiasAntesDoVencimentoEmprestimo = ParametroSistemaMock.Instance.GerarParametroSistema(TipoParametroSistema.QtdeDiasAntesDoVencimentoEmprestimo, "1");
            await InserirNaBase(notificarQtdeDiasAntesDoVencimentoEmprestimo);
            
            var acervosSolicitacoes = ObterAcervosSolicitacoes();

            var contadorSolicitacoes = 1;
            var contadorSolicitacoesItens = 1;
            var dataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(int.Parse(notificarQtdeDiasAntesDoVencimentoEmprestimo.Valor));
            foreach (var acervoSolicitacao in acervosSolicitacoes)
            {
                await InserirNaBase(acervoSolicitacao);
                
                var acervosSolicitacoesItens = ObterAcervoSolicitacaoItem(contadorSolicitacoes);
                
                foreach (var acervoSolicitacaoItem in acervosSolicitacoesItens)
                {
                    await InserirNaBase(acervoSolicitacaoItem);
                    
                    var acervoEmprestimo = AcervoEmprestimoMock.Instance.Gerar(contadorSolicitacoesItens, DateTimeExtension.HorarioBrasilia().AddDays(-15)).Generate();
                    acervoEmprestimo.DataDevolucao = dataDevolucao;
                    await InserirNaBase(acervoEmprestimo);
                    contadorSolicitacoesItens++;
                }
                contadorSolicitacoes++;
            }
            
            var casoDeUso = ObterCasoDeUso<INotificacaoVencimentoEmprestimoUseCase>();
            
            await casoDeUso.Executar(new MensagemRabbit());
        }
        
        [Fact(DisplayName = "Acervo Emprestimo - Notificar usuarios sobre o vencimento de empréstimo")]
        public async Task Deve_notificar_usuario_vencimento_de_emprestimo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();
            
            var notificarQtdeDiasAntesDoVencimentoEmprestimo = ParametroSistemaMock.Instance.GerarParametroSistema(TipoParametroSistema.QtdeDiasAntesDoVencimentoEmprestimo, "1");
            await InserirNaBase(notificarQtdeDiasAntesDoVencimentoEmprestimo);
            
            await InserirNaBase(ParametroSistemaMock.Instance.GerarParametroSistema(TipoParametroSistema.ModeloEmailAvisoDevolucaoEmprestimo, ""));
            await InserirNaBase(ParametroSistemaMock.Instance.GerarParametroSistema(TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita, ""));
            await InserirNaBase(ParametroSistemaMock.Instance.GerarParametroSistema(TipoParametroSistema.EnderecoSedeCDEPVisita, ""));
            await InserirNaBase(ParametroSistemaMock.Instance.GerarParametroSistema(TipoParametroSistema.HorarioFuncionamentoSedeCDEPVisita, ""));
            
            var acervosSolicitacoes = ObterAcervosSolicitacoes();

            var contadorSolicitacoes = 1;
            var contadorSolicitacoesItens = 1;
            var dataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(int.Parse(notificarQtdeDiasAntesDoVencimentoEmprestimo.Valor));
            foreach (var acervoSolicitacao in acervosSolicitacoes)
            {
                await InserirNaBase(acervoSolicitacao);
                
                var acervosSolicitacoesItens = ObterAcervoSolicitacaoItem(contadorSolicitacoes);
                
                foreach (var acervoSolicitacaoItem in acervosSolicitacoesItens)
                {
                    await InserirNaBase(acervoSolicitacaoItem);
                    
                    var acervoEmprestimo = AcervoEmprestimoMock.Instance.Gerar(contadorSolicitacoesItens, DateTimeExtension.HorarioBrasilia().AddDays(-15)).Generate();
                    acervoEmprestimo.DataDevolucao = dataDevolucao;
                    await InserirNaBase(acervoEmprestimo);
                    contadorSolicitacoesItens++;
                }
                contadorSolicitacoes++;
            }
            
            var casoDeUso = ObterCasoDeUso<INotificacaoVencimentoEmprestimoUsuarioUseCase>();

            var acervoBase = (ObterTodos<Acervo>()).FirstOrDefault();
            var acervoEmprestimoBase = (ObterTodos<AcervoEmprestimo>()).FirstOrDefault();

            var acervoEmprestimoAntesVencimentoDevolucao = new AcervoEmprestimoDevolucao()
            {
                DataDevolucao = dataDevolucao,
                Codigo = acervoBase.Codigo,
                Titulo = acervoBase.Titulo,
                DataEmprestimo = acervoEmprestimoBase.DataEmprestimo,
                AcervoSolicitacaoId = 1,
                AcervoSolicitacaoItemId = 1,
                Email = "email@email.com",
                Solicitante = "Nome"
            };
            await casoDeUso.Executar(new MensagemRabbit(JsonConvert.SerializeObject(acervoEmprestimoAntesVencimentoDevolucao)));
        }
        
        private static List<AcervoSolicitacaoItem> ObterAcervoSolicitacaoItem(int contador)
        {
            return AcervoSolicitacaoItemMock.Instance
                .Gerar(contador,TipoAtendimento.Presencial, 
                DateTimeExtension.HorarioBrasilia().AddDays(-15),
                SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE)
                .Generate(10);
        }

        private static List<AcervoSolicitacao> ObterAcervosSolicitacoes()
        {
            return AcervoSolicitacaoMock.Instance.Gerar(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO).Generate(10);
        }
    }
}