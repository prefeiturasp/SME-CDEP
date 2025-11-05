using Shouldly;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_finalizar_acervo_solicitacao : TesteBase
    {
        public Ao_finalizar_acervo_solicitacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
       
       
        [Fact(DisplayName = "Acervo Solicitação - Deve finalizar com todos os itens cancelados")]
        public async Task Deve_finalizar_atendimento_com_itens_cancelados()
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
                item.TipoAtendimento = TipoAtendimento.Email;
                await InserirNaBase(item);
           }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var retorno = await servicoAcervoSolicitacao.FinalizarAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve finalizar com itens em situação aguardando atendimento")]
        public async Task Nao_deve_finalizar_atendimento_com_itens_aguardando_atendimento()
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
            
            await servicoAcervoSolicitacao.FinalizarAtendimento(1).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve finalizar atendimento com itens em situação aguardando visita que já passou")]
        public async Task Deve_finalizar_atendimento_com_itens_em_situacao_aguardando_visita_que_ja_passou()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
            await InserirNaBase(acervoSolicitacao);

            var acervoSolicitadoId = (ObterTodos<AcervoSolicitacao>()).LastOrDefault().Id;
           
            foreach (var item in acervoSolicitacao.Itens)
            {
                item.AcervoSolicitacaoId = acervoSolicitadoId;
                item.TipoAtendimento = TipoAtendimento.Presencial;
                item.Situacao = SituacaoSolicitacaoItem.AGUARDANDO_VISITA;
                item.DataVisita = DataHelper.ProximaDataUtil(DateTime.Now.AddDays(-2));
                await InserirNaBase(item);
            }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var retorno = await servicoAcervoSolicitacao.FinalizarAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve finalizar atendimento com itens em situação finalizada manualmente")]
        public async Task Deve_finalizar_atendimento_com_itens_em_situacao_finalizada_manualmente()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
            await InserirNaBase(acervoSolicitacao);

            var acervoSolicitadoId = (ObterTodos<AcervoSolicitacao>()).LastOrDefault().Id;
           
            foreach (var item in acervoSolicitacao.Itens)
            {
                item.AcervoSolicitacaoId = acervoSolicitadoId;
                item.TipoAtendimento = TipoAtendimento.Email;
                item.Situacao = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE;
                await InserirNaBase(item);
            }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var retorno = await servicoAcervoSolicitacao.FinalizarAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve finalizar atendimento com itens em situação finalizada automaticamente")]
        public async Task Deve_finalizar_atendimento_com_itens_em_situacao_finalizada_automaticamente()
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
            
            var retorno = await servicoAcervoSolicitacao.FinalizarAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve finalizar atendimento com itens em situação aguardando visita com data de visita futura")]
        public async Task Nao_deve_finalizar_atendimento_com_itens_em_situacao_aguardando_visita_com_data_de_visita_futura()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
            await InserirNaBase(acervoSolicitacao);

            var acervoSolicitadoId = (ObterTodos<AcervoSolicitacao>()).LastOrDefault().Id;
           
            foreach (var item in acervoSolicitacao.Itens)
            {
                item.AcervoSolicitacaoId = acervoSolicitadoId;
                item.TipoAtendimento = TipoAtendimento.Presencial;
                item.Situacao = SituacaoSolicitacaoItem.AGUARDANDO_VISITA;
                item.DataVisita = DataHelper.ProximaDataUtil(DateTime.Now.AddDays(2));
                await InserirNaBase(item);
            }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.FinalizarAtendimento(1).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve finalizar atendimento que foi parcialmente atendido")]
        public async Task Nao_deve_finalizar_atendimento_que_foi_parcialmente_atendido()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            acervoSolicitacao.Situacao = SituacaoSolicitacao.ATENDIDO_PARCIALMENTE;
            await InserirNaBase(acervoSolicitacao);

            var acervoSolicitadoId = (ObterTodos<AcervoSolicitacao>()).LastOrDefault().Id;

            var contador = 1;
            foreach (var item in acervoSolicitacao.Itens)
            {
                item.AcervoSolicitacaoId = acervoSolicitadoId;
                item.TipoAtendimento = TipoAtendimento.Presencial;
                item.Situacao = contador > 1 ? SituacaoSolicitacaoItem.AGUARDANDO_VISITA : SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO;
                item.DataVisita = DataHelper.ProximaDataUtil(DateTime.Now.AddDays(2));
                await InserirNaBase(item);
            }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.FinalizarAtendimento(1).ShouldThrowAsync<NegocioException>();
        }
    }
}