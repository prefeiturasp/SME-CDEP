using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_pesquisar_acervo_solicitacao : TesteBase
    {
        public Ao_pesquisar_acervo_solicitacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
       
       
        [Fact(DisplayName = "Acervo Solicitação - Pesquisar por AcervoSolicitacaoId")]
        public async Task Obter_pesquisar_por_acervo_solicitacao_id()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var filtro = new FiltroSolicitacaoDTO() { AcervoSolicitacaoId = 1 };
            var retorno = await servicoAcervoSolicitacao.ObterSolicitacoesPorFiltro(filtro);
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Pesquisar por Item da Situação ")]
        public async Task Obter_pesquisar_por_situacao_item()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var filtro = new FiltroSolicitacaoDTO() { SituacaoItem = SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE };
            var retorno = await servicoAcervoSolicitacao.ObterSolicitacoesPorFiltro(filtro);
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Pesquisar por Tipo de Acervo")]
        public async Task Obter_pesquisar_por_tipo_de_acervo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var filtro = new FiltroSolicitacaoDTO() { TipoAcervo = TipoAcervo.Tridimensional };
            var retorno = await servicoAcervoSolicitacao.ObterSolicitacoesPorFiltro(filtro);
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Pesquisar por Data de solicitação")]
        public async Task Obter_pesquisar_por_data_solicitacao()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var filtro = new FiltroSolicitacaoDTO() { DataSolicitacaoInicio = DateTimeExtension.HorarioBrasilia() };
            var retorno = await servicoAcervoSolicitacao.ObterSolicitacoesPorFiltro(filtro);
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Pesquisar por Data de visita válida")]
        public async Task Obter_pesquisar_por_data_de_visita_valida()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var filtro = new FiltroSolicitacaoDTO()
            {
                DataVisitaInicio = DateTimeExtension.HorarioBrasilia(),
                DataVisitaFim = DateTimeExtension.HorarioBrasilia().AddDays(20)
            };
            var retorno = await servicoAcervoSolicitacao.ObterSolicitacoesPorFiltro(filtro);
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Pesquisar por Data de visita inválida")]
        public async Task Obter_pesquisar_por_data_de_visita_invalida()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var filtro = new FiltroSolicitacaoDTO()
            {
                DataVisitaInicio = DateTimeExtension.HorarioBrasilia(),
                DataVisitaFim = DateTimeExtension.HorarioBrasilia()
            };
            var retorno = await servicoAcervoSolicitacao.ObterSolicitacoesPorFiltro(filtro);
            retorno.Items.Count().ShouldBe(0);
        }
    }
}