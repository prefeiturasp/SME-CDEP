using Shouldly;
using SME.CDEP.Aplicacao;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_atualizar_situacao_para_emprestimo_com_devolucao_em_atraso : TesteBase
    {
        public Ao_atualizar_situacao_para_emprestimo_com_devolucao_em_atraso(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo Emprestimo - Deve incluir situação em atraso")]
        public async Task Deve_incluir_situacao_em_atraso()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();
            
            var acervosSolicitacoes = ObterAcervosSolicitacoes();

            var contadorSolicitacoes = 1;
            var contadorSolicitacoesItens = 1;
            foreach (var acervoSolicitacao in acervosSolicitacoes)
            {
                await InserirNaBase(acervoSolicitacao);
                
                var acervosSolicitacoesItens = ObterAcervoSolicitacaoItem(contadorSolicitacoes);
                
                foreach (var acervoSolicitacaoItem in acervosSolicitacoesItens)
                {
                    await InserirNaBase(acervoSolicitacaoItem);
                    
                    var acervoEmprestimo = AcervoEmprestimoMock.Instance.Gerar(contadorSolicitacoesItens, DateTimeExtension.HorarioBrasilia().AddDays(-15)).Generate();
                    await InserirNaBase(acervoEmprestimo);
                    contadorSolicitacoesItens++;
                }
                contadorSolicitacoes++;
            }
            
            var casoDeUso = ObterCasoDeUso<IExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtrasoUseCase>();
            
            await casoDeUso.Executar(new MensagemRabbit(string.Empty));

            var acervosEmprestimos = ObterTodos<AcervoEmprestimo>();
            
            acervosEmprestimos.FirstOrDefault(a => a.AcervoSolicitacaoItemId == 1).Situacao.EstaEmprestado().ShouldBeTrue();
            acervosEmprestimos.Count(a => a.Situacao.EstaEmprestado()).ShouldBe(100);
            
            acervosEmprestimos.LastOrDefault(a => a.AcervoSolicitacaoItemId == 1).Situacao.DevolucaoAtrasada().ShouldBeTrue();
            acervosEmprestimos.Count(a => a.Situacao.DevolucaoAtrasada()).ShouldBe(100);
            
            acervosEmprestimos.Count().ShouldBe(200);
        }
        
        [Fact(DisplayName = "Acervo Emprestimo - Não deve incluir situação em atraso")]
        public async Task Nao_deve_incluir_situacao_em_atraso()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();
            
            var acervosSolicitacoes = ObterAcervosSolicitacoes();

            var contadorSolicitacoes = 1;
            var contadorSolicitacoesItens = 1;
            foreach (var acervoSolicitacao in acervosSolicitacoes)
            {
                await InserirNaBase(acervoSolicitacao);
                
                var acervosSolicitacoesItens = ObterAcervoSolicitacaoItem(contadorSolicitacoes);
                
                foreach (var acervoSolicitacaoItem in acervosSolicitacoesItens)
                {
                    await InserirNaBase(acervoSolicitacaoItem);
                    
                    var acervoEmprestimo = AcervoEmprestimoMock.Instance.Gerar(contadorSolicitacoesItens, DateTimeExtension.HorarioBrasilia()).Generate();
                    await InserirNaBase(acervoEmprestimo);
                    contadorSolicitacoesItens++;
                }
                contadorSolicitacoes++;
            }
            
            var casoDeUso = ObterCasoDeUso<IExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtrasoUseCase>();
            
            await casoDeUso.Executar(new MensagemRabbit(string.Empty));

            var acervosEmprestimos = ObterTodos<AcervoEmprestimo>();
            acervosEmprestimos.All(a => a.Situacao.EstaEmprestado()).ShouldBeTrue();
            acervosEmprestimos.Count(a => a.Situacao.EstaEmprestado()).ShouldBe(100);
            acervosEmprestimos.Count().ShouldBe(100);
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