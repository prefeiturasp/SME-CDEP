using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_fazer_manutencao_acervo_solicitacao : TesteBase
    {
        public Ao_fazer_manutencao_acervo_solicitacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo Solicitação - Obter por Id")]
        public async Task Obter_por_id()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var retorno = await servicoAcervoSolicitacao.ObterPorId(1);
            retorno.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Ao enviar a solicitação para análise - offline - sem arquivos")]
        public async Task Ao_enviar_solicitacao_para_analise_sem_arquivos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional(false);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var acervoSolicitacaoInserir = ObterItens();
            
            var retorno = await servicoAcervoSolicitacao.Inserir(acervoSolicitacaoInserir.ToArray());
            retorno.ShouldBeGreaterThan(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Ao enviar a solicitação para finalizado - online - com arquivos")]
        public async Task Ao_enviar_solicitacao_para_finalizado_com_arquivos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var acervoSolicitacaoInserir = ObterItens();
            
            var retorno = await servicoAcervoSolicitacao.Inserir(acervoSolicitacaoInserir.ToArray());
            retorno.ShouldBeGreaterThan(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Remover")]
        public async Task Ao_remover()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao(10);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var retornoAcervoSolicitacaoAlterado = await servicoAcervoSolicitacao.Remover(1);
            retornoAcervoSolicitacaoAlterado.ShouldBeTrue();
            
            var acervoSolicitacaoAlterado = await servicoAcervoSolicitacao.ObterPorId(1);
            acervoSolicitacaoAlterado.Itens.PossuiElementos().ShouldBeFalse();
        }
        
        private List<AcervoSolicitacaoItemCadastroDTO> ObterItens()
        {
            return new List<AcervoSolicitacaoItemCadastroDTO>()
            {
                new() { AcervoId = 1 },
                new() { AcervoId = 2 },
                new() { AcervoId = 3 },
            };
        }
    }
}