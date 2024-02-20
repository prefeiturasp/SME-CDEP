using Shouldly;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_obter_detalhes_acervo_solicitacao : TesteBase
    {
        public Ao_obter_detalhes_acervo_solicitacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo Solicitação - Obter todos por usuário logado")]
        public async Task Obter_todos_por_usuario_logado()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao(10);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var retorno = await servicoAcervoSolicitacao.ObterMinhasSolicitacoes();
            retorno.ShouldNotBeNull();
        }
        
       
        [Fact(DisplayName = "Acervo Solicitação - Obter Detalhes por Id")]
        public async Task Obter_detalhes_por_id()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var retorno = await servicoAcervoSolicitacao.ObterDetalhesParaAtendimentoSolicitadoesPorId(1);
            retorno.ShouldNotBeNull();
            retorno.DadosSolicitante.ShouldNotBeNull();
            retorno.DadosSolicitante.Nome.ShouldNotBeEmpty();
            retorno.DadosSolicitante.Tipo.ShouldNotBeEmpty();
            retorno.Id.ShouldBeGreaterThan(0);
            retorno.Situacao.ShouldNotBeEmpty();
            retorno.UsuarioId.ShouldBeGreaterThan(0);
            retorno.Itens.Any().ShouldBeTrue();
        }
    }
}