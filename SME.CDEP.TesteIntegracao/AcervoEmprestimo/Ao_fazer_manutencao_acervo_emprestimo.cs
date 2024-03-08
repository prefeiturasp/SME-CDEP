using Shouldly;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_fazer_manutencao_acervo_emprestimo : TesteBase
    {
        public Ao_fazer_manutencao_acervo_emprestimo(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
       
        [Fact(DisplayName = "Acervo empréstimo - Deve inserir empréstimo de acervo")]
        public async Task Deve_inserir_emprestimo_de_acervo()
        {
            await InserirDadosBasicosAleatorios();

            var acervoId = 1;
            var inserindoAcervoBibliografico = AcervoBibliograficoMock.Instance.Gerar().Generate(10);
            foreach (var acervoBibliografico in inserindoAcervoBibliografico)
            {
                await InserirNaBase(acervoBibliografico.Acervo);
                acervoBibliografico.AcervoId = acervoId;
                await InserirNaBase(acervoBibliografico);
                acervoId++;
            }
            
            var inserindoSolicitacoes = AcervoSolicitacaoMock.Instance.Gerar().Generate(4);
            await InserirVariosNaBase(inserindoSolicitacoes);

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DateTimeExtension.HorarioBrasilia().Date;
            
            var inserindoItensSolicitacao1 = AcervoSolicitacaoItemMock.Instance.Gerar(1,atendimentoPresencial, dataVisita).Generate(2);
            await InserirVariosNaBase(inserindoItensSolicitacao1);
            
            var inserindoItensSolicitacao2 = AcervoSolicitacaoItemMock.Instance.Gerar(2,atendimentoPresencial, dataVisita).Generate(4);
            await InserirVariosNaBase(inserindoItensSolicitacao2);
            
            var inserindoItensSolicitacao3 = AcervoSolicitacaoItemMock.Instance.Gerar(3,atendimentoPresencial, dataVisita).Generate(3);
            await InserirVariosNaBase(inserindoItensSolicitacao3);
            
            var inserindoItensSolicitacao4 = AcervoSolicitacaoItemMock.Instance.Gerar(4,atendimentoPresencial, dataVisita).Generate();
            await InserirNaBase(inserindoItensSolicitacao4);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 3);
           
            foreach (var item in itensDaSolicitacao)
            {
                var inserirAcervoEmprestimo = AcervoEmprestimoMock.Instance.Gerar(item.Id, dataVisita).Generate();
                await InserirNaBase(inserirAcervoEmprestimo);
            }

            var acervosEmprestados = ObterTodos<AcervoEmprestimo>();
            acervosEmprestados.Count().ShouldBe(3);
            
            foreach (var acervoEmprestado in acervosEmprestados)
            {
                acervoEmprestado.DataEmprestimo.Date.ShouldBe(dataVisita.Date);
                acervoEmprestado.DataDevolucao.Date.ShouldBe(dataVisita.AddDays(7).Date);
                acervoEmprestado.Excluido.ShouldBeFalse();
                acervoEmprestado.Situacao.ShouldBe(SituacaoEmprestimo.EMPRESTADO);
                acervoEmprestado.AcervoSolicitacaoItemId.ShouldBeGreaterThan(0);
            }
        }
    }
}