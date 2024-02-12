using Shouldly;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_cancelar_acervo_solicitacao : TesteBase
    {
        public Ao_cancelar_acervo_solicitacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
       
        [Fact(DisplayName = "Acervo Solicitação - Deve cancelar atendimento quando todos os itens estiverem cancelados")]
        public async Task Deve_cancelar_atendimento_quando_todos_os_itens_estiverem_cancelados()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
           await InserirNaBase(acervoSolicitacao);

           var acervoSolicitadoId = (ObterTodos<AcervoSolicitacao>()).LastOrDefault().Id;
           
           foreach (var item in acervoSolicitacao.Itens)
           {
                item.AcervoSolicitacaoId = acervoSolicitadoId;
                item.Situacao = SituacaoSolicitacaoItem.CANCELADO;
                await InserirNaBase(item);
           }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var retorno = await servicoAcervoSolicitacao.CancelarAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.CANCELADO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve cancelar atendimento com itens em situação finalizado automaticamente")]
        public async Task Nao_deve_cancelar_atendimento_com_itens_em_situacao_finalizado_automaticamente()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
            await InserirNaBase(acervoSolicitacao);

            var acervoSolicitadoId = (ObterTodos<AcervoSolicitacao>()).LastOrDefault().Id;
           
            foreach (var item in acervoSolicitacao.Itens)
            {
                item.AcervoSolicitacaoId = acervoSolicitadoId;
                item.Situacao = SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE;
                await InserirNaBase(item);
            }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.CancelarAtendimento(1).ShouldThrowAsync<NegocioException>();
        }
    }
}