using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_obter_itens_por_acervo_ids : TesteBase
    {
        private readonly IServicoAcervoSolicitacao servicoAcervoSolicitacao;
        
        public Ao_obter_itens_por_acervo_ids(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
        }

        [Fact(DisplayName = "Acervo - Obter acervos disponíveis por ids")]
        public async Task Obter_acervos_disponiveis_por_ids()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await GerarArquivosSistema();

            var acervosInseridos = ObterTodos<Acervo>();
            
            var obterItensAcervoPorAcervosIds = await servicoAcervoSolicitacao.ObterItensAcervoPorAcervosIds(acervosInseridos.Select(s=> s.Id).ToArray());
            obterItensAcervoPorAcervosIds.ShouldNotBeNull();
            obterItensAcervoPorAcervosIds.Any(a=> a.EstaDisponivel).ShouldBeTrue();
        }
        
        [Theory(DisplayName = "Acervo - Obter acervos indisponíveis por ids")]
        [InlineData(SituacaoSaldo.RESERVADO)]
        [InlineData(SituacaoSaldo.EMPRESTADO)]
        public async Task Obter_acervos_indisponiveis_por_ids(SituacaoSaldo situacaoSaldo)
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos(situacaoSaldo);

            await GerarArquivosSistema();

            var acervosInseridos = ObterTodos<Acervo>();
            
            var obterItensAcervoPorAcervosIds = await servicoAcervoSolicitacao.ObterItensAcervoPorAcervosIds(acervosInseridos.Select(s=> s.Id).ToArray());
            obterItensAcervoPorAcervosIds.ShouldNotBeNull();
            obterItensAcervoPorAcervosIds.Any(a=> a.EstaDisponivel).ShouldBeFalse();
        }
    }
}