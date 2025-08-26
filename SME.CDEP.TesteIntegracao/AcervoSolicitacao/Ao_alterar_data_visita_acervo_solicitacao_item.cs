using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_alterar_data_visita_acervo_solicitacao_item : TesteBase
    {
        public Ao_alterar_data_visita_acervo_solicitacao_item(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
       
        [Fact(DisplayName = "Acervo Solicitação Item - Alterar data de visita ")]
        public async Task Deve_alterar_data_visita_do_item()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            acervoSolicitacao.Situacao = SituacaoSolicitacao.AGUARDANDO_VISITA;
            
           await InserirNaBase(acervoSolicitacao);
           
           foreach (var item in acervoSolicitacao.Itens)
           {
                item.AcervoSolicitacaoId = 1;
                item.DataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
                item.Situacao = SituacaoSolicitacaoItem.AGUARDANDO_VISITA;
                await InserirNaBase(item);
           }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var retorno = await servicoAcervoSolicitacao.AlterarDataVisitaDoItemAtendimento(new AlterarDataVisitaAcervoSolicitacaoItemDTO()
            {
                Id = 1,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now.AddDays(10))
            });
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacaoItem>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.DataVisita.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.AddDays(10));
        }
        
        [Fact(DisplayName = "Acervo Solicitação Item - Não deve alterar data de visita quando item não for encontrado ")]
        public async Task Nao_deve_alterar_data_visita_quando_item_nao_for_encontrado()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
            await InserirNaBase(acervoSolicitacao);
           
            foreach (var item in acervoSolicitacao.Itens)
            {
                item.AcervoSolicitacaoId = 1;
                item.DataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
                await InserirNaBase(item);
            }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.AlterarDataVisitaDoItemAtendimento(new AlterarDataVisitaAcervoSolicitacaoItemDTO()
            {
                Id = 2024,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now.AddDays(10))
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação Item - Não deve alterar data de visita quando a data for no passado")]
        public async Task Nao_deve_alterar_data_visita_quando_data_eh_no_passado()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            acervoSolicitacao.Situacao = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO;
            
            await InserirNaBase(acervoSolicitacao);

            foreach (var item in acervoSolicitacao.Itens)
            {
                item.AcervoSolicitacaoId = 1;
                item.DataVisita = DateTimeExtension.HorarioBrasilia().Date;
                item.Situacao = SituacaoSolicitacaoItem.CANCELADO;
                await InserirNaBase(item);
            }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.AlterarDataVisitaDoItemAtendimento(new AlterarDataVisitaAcervoSolicitacaoItemDTO()
            {
                Id = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date.AddDays(-10)
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação Item - Não deve alterar data de visita quando a situação do item for cancelado")]
        public async Task Nao_deve_alterar_data_visita_quando_situacao_item_for_cancelado()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            acervoSolicitacao.Situacao = SituacaoSolicitacao.AGUARDANDO_VISITA;
            
            await InserirNaBase(acervoSolicitacao);

            foreach (var item in acervoSolicitacao.Itens)
            {
                item.AcervoSolicitacaoId = 1;
                item.DataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
                item.Situacao = SituacaoSolicitacaoItem.CANCELADO;
                await InserirNaBase(item);
            }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.AlterarDataVisitaDoItemAtendimento(new AlterarDataVisitaAcervoSolicitacaoItemDTO()
            {
                Id = 1,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now.AddDays(10))
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação Item - Não deve alterar data de visita quando a situação do item da solicitação for finalizado automaticamente")]
        public async Task Nao_deve_alterar_data_visita_quando_situacao_do_item_da_solicitacao_for_finalizado_automaticamente()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            acervoSolicitacao.Situacao = SituacaoSolicitacao.AGUARDANDO_VISITA;
            
            await InserirNaBase(acervoSolicitacao);

            foreach (var item in acervoSolicitacao.Itens)
            {
                item.AcervoSolicitacaoId = 1;
                item.DataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
                item.Situacao = SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE;
                await InserirNaBase(item);
            }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.AlterarDataVisitaDoItemAtendimento(new AlterarDataVisitaAcervoSolicitacaoItemDTO()
            {
                Id = 1,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now.AddDays(10))
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação Item - Não deve alterar data de visita quando a situação da solicitação for cancelado")]
        public async Task Nao_deve_alterar_data_visita_quando_situacao_da_solicitacao_for_cancelado()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            acervoSolicitacao.Situacao = SituacaoSolicitacao.CANCELADO;
            
            await InserirNaBase(acervoSolicitacao);

            foreach (var item in acervoSolicitacao.Itens)
            {
                item.AcervoSolicitacaoId = 1;
                item.DataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
                item.Situacao = SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE;
                await InserirNaBase(item);
            }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.AlterarDataVisitaDoItemAtendimento(new AlterarDataVisitaAcervoSolicitacaoItemDTO()
            {
                Id = 1,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now.AddDays(10))
            }).ShouldThrowAsync<NegocioException>();
        }
    }
}