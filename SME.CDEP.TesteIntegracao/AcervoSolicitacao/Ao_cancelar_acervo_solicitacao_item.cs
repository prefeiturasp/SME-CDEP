using Shouldly;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_cancelar_acervo_solicitacao_item : TesteBase
    {
        public Ao_cancelar_acervo_solicitacao_item(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
       
        [Fact(DisplayName = "Acervo Solicitação Item - Deve cancelar um item em situação aguardando visita")]
        public async Task Deve_cancelar_um_item_em_situacao_aguardando_visita()
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
                item.DataVisita = DateTimeExtension.HorarioBrasilia().Date.AddDays(2);
                await InserirNaBase(item);
           }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var retorno = await servicoAcervoSolicitacao.CancelarItemAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoItemAlterada = ObterTodos<AcervoSolicitacaoItem>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoItemAlterada.Id.ShouldBe(1);
            solicitacaoItemAlterada.Situacao.ShouldBe(SituacaoSolicitacaoItem.CANCELADO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação Item - Deve cancelar um item em situação aguardando atendimento")]
        public async Task Deve_cancelar_um_item_em_situacao_aguardando_atendimento()
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
        
        [Fact(DisplayName = "Acervo Solicitação Item - Não deve cancelar um item em situação finalizado automaticamente")]
        public async Task Nao_deve_cancelar_um_item_em_situacao_finalizado_automaticamente()
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
        
        [Fact(DisplayName = "Acervo Solicitação Item - Não deve cancelar um item com situação já cancelado")]
        public async Task Nao_deve_cancelar_um_item_em_situacao_ja_cancelado()
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
           
            await servicoAcervoSolicitacao.CancelarItemAtendimento(1).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação Item - Deve cancelar o único item e cancelar a solicitação")]
        public async Task Deve_cancelar_o_unico_item_e_cancelar_solicitacao()
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
            
            retorno = await servicoAcervoSolicitacao.CancelarItemAtendimento(2);
            retorno.ShouldBeTrue();
            solicitacaoItemAlterada = ObterTodos<AcervoSolicitacaoItem>().FirstOrDefault(f=> f.Id == 2);
            solicitacaoItemAlterada.Id.ShouldBe(2);
            solicitacaoItemAlterada.Situacao.ShouldBe(SituacaoSolicitacaoItem.CANCELADO);
            
            retorno = await servicoAcervoSolicitacao.CancelarItemAtendimento(3);
            retorno.ShouldBeTrue();
            solicitacaoItemAlterada = ObterTodos<AcervoSolicitacaoItem>().FirstOrDefault(f=> f.Id == 3);
            solicitacaoItemAlterada.Id.ShouldBe(3);
            solicitacaoItemAlterada.Situacao.ShouldBe(SituacaoSolicitacaoItem.CANCELADO);
            
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.CANCELADO);
        }
    }
}