using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_cancelar_acervo_solicitacao_item : TesteBase
    {
        public Ao_cancelar_acervo_solicitacao_item(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
       
        [Fact(DisplayName = "Acervo Solicitação Item - Cancelar com situação aguardando visita")]
        public async Task Deve_cancelar_atendimento_com_situacao_aguardando_visita()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
           await InserirNaBase(acervoSolicitacao);

           var acervoSolicitadoId = (ObterTodos<AcervoSolicitacao>()).LastOrDefault().Id;
           
           foreach (var item in acervoSolicitacao.Itens)
           {
                item.AcervoSolicitacaoId = acervoSolicitadoId;
                item.Situacao = SituacaoSolicitacaoItem.AGUARDANDO_VISITA;
                await InserirNaBase(item);
           }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var retorno = await servicoAcervoSolicitacao.CancelarItemAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoItemAlterada = ObterTodos<AcervoSolicitacaoItem>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoItemAlterada.Id.ShouldBe(1);
            solicitacaoItemAlterada.Situacao.ShouldBe(SituacaoSolicitacaoItem.CANCELADO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação Item - Cancelar com situação aguardando atendimento")]
        public async Task Deve_cancelar_atendimento_com_situacao_aguardando_atendimento()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
            await InserirNaBase(acervoSolicitacao);

            var acervoSolicitadoId = (ObterTodos<AcervoSolicitacao>()).LastOrDefault().Id;
           
            foreach (var item in acervoSolicitacao.Itens)
            {
                item.AcervoSolicitacaoId = acervoSolicitadoId;
                item.Situacao = SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO;
                await InserirNaBase(item);
            }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var retorno = await servicoAcervoSolicitacao.CancelarItemAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoItemAlterada = ObterTodos<AcervoSolicitacaoItem>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoItemAlterada.Id.ShouldBe(1);
            solicitacaoItemAlterada.Situacao.ShouldBe(SituacaoSolicitacaoItem.CANCELADO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação Item - Não deve cancelar com situação finalizado automaticamente")]
        public async Task Nao_deve_cancelar_atendimento_com_situacao_finalizado_automaticamente()
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
           
            await servicoAcervoSolicitacao.CancelarItemAtendimento(1).ShouldThrowAsync<NegocioException>();
        }
    }
}