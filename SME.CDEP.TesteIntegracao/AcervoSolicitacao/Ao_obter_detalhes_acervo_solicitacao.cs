using Shouldly;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_obter_detalhes_acervo_solicitacao : TesteBase
    {
        private readonly IServicoAcervoSolicitacao servicoAcervoSolicitacao;
        public Ao_obter_detalhes_acervo_solicitacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Obter todos por usuário logado")]
        public async Task Obter_todos_por_usuario_logado()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao(10);

            var retorno = await servicoAcervoSolicitacao.ObterMinhasSolicitacoes();
            retorno.ShouldNotBeNull();
        }
       
        [Fact(DisplayName = "Acervo Solicitação - Obter Detalhes por Id com perfil Admin Geral")]
        public async Task Obter_detalhes_por_id_com_perfil_admin_geral()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);

            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var retorno = await servicoAcervoSolicitacao.ObterDetalhesParaAtendimentoSolicitadoesPorId(1);
            retorno.ShouldNotBeNull();
            retorno.DadosSolicitante.ShouldNotBeNull();
            retorno.DadosSolicitante.Nome.ShouldNotBeEmpty();
            retorno.DadosSolicitante.Tipo.ShouldNotBeEmpty();
            retorno.Id.ShouldBeGreaterThan(0);
            retorno.Situacao.ShouldNotBeEmpty();
            retorno.UsuarioId.ShouldBeGreaterThan(0);
            retorno.PodeFinalizar.ShouldBeFalse();
            retorno.PodeCancelar.ShouldBeFalse();
            retorno.Itens.Any().ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Obter Detalhes por Id com Admin Memorial")]
        public async Task Obter_detalhes_por_id_com_admin_memorial()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_MEMORIAL_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficosEmMassa(1, 2);
            await InserirAcervosArteGraficasEmMassa(3, 1);

            await InserirNaBase(AcervoSolicitacaoMock.Instance.Gerar(SituacaoSolicitacao.ATENDIDO_PARCIALMENTE).Generate());

            await InserirNaBase(ObterAcervoSolicitacaoItem(1,1));
            await InserirNaBase(ObterAcervoSolicitacaoItem(1,2));
            await InserirNaBase(ObterAcervoSolicitacaoItem(1,3, TipoAtendimento.Presencial, DateTime.Now, SituacaoSolicitacaoItem.AGUARDANDO_VISITA));

            var retorno = await servicoAcervoSolicitacao.ObterDetalhesParaAtendimentoSolicitadoesPorId(1);
            retorno.ShouldNotBeNull();
            retorno.DadosSolicitante.ShouldNotBeNull();
            retorno.DadosSolicitante.Nome.ShouldNotBeEmpty();
            retorno.DadosSolicitante.Tipo.ShouldNotBeEmpty();
            retorno.Id.ShouldBeGreaterThan(0);
            retorno.Situacao.ShouldNotBeEmpty();
            retorno.UsuarioId.ShouldBeGreaterThan(0);
            retorno.PodeFinalizar.ShouldBeTrue();
            retorno.PodeCancelar.ShouldBeTrue();
            retorno.Itens.Any().ShouldBeTrue();
        }
    }
}