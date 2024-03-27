using Shouldly;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_finalizar_acervo_solicitacao_item : TesteBase
    {
        private readonly IServicoAcervoSolicitacao servicoAcervoSolicitacao;

        public Ao_finalizar_acervo_solicitacao_item(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
        }
       
        [Fact(DisplayName = "Acervo Solicitação Item - Deve finalizar item e o atendimento com perfil Admin Geral")]
        public async Task Deve_finalizar_item_e_o_atendimento_com_perfil_admin_geral()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_MEMORIAL_GUID);

            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficosEmMassa(1, 2);
            await InserirAcervosArteGraficasEmMassa(3, 1);

            await InserirNaBase(AcervoSolicitacaoMock.Instance.Gerar(SituacaoSolicitacao.ATENDIDO_PARCIALMENTE).Generate());

            await InserirNaBase(ObterAcervoSolicitacaoItem(1,1,situacao: SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE));
            await InserirNaBase(ObterAcervoSolicitacaoItem(1,2, TipoAtendimento.Email, situacao: SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE));
            await InserirNaBase(ObterAcervoSolicitacaoItem(1,3, TipoAtendimento.Presencial, DateTime.Now, SituacaoSolicitacaoItem.AGUARDANDO_VISITA));

            var retorno = await servicoAcervoSolicitacao.FinalizarAtendimentoItem(3);

            var acervosSolicitacoes = ObterTodos<AcervoSolicitacao>();
            acervosSolicitacoes.FirstOrDefault().Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);

            var acervoSolicitacaoItens = ObterTodos<AcervoSolicitacaoItem>();
            acervoSolicitacaoItens.FirstOrDefault(f=> f.AcervoSolicitacaoId == 1 && f.AcervoId == 1).Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE);
            acervoSolicitacaoItens.FirstOrDefault(f=> f.AcervoSolicitacaoId == 1 && f.AcervoId == 2).Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            acervoSolicitacaoItens.FirstOrDefault(f=> f.AcervoSolicitacaoId == 1 && f.AcervoId == 3).Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
        }
        
        [Fact(DisplayName = "Acervo Solicitação Item - Deve finalizar somente o item com perfil Admin Geral")]
        public async Task Deve_finalizar_somente_o_item_com_perfil_admin_geral()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_MEMORIAL_GUID);

            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficosEmMassa(1, 2);
            await InserirAcervosArteGraficasEmMassa(3, 1);

            await InserirNaBase(AcervoSolicitacaoMock.Instance.Gerar(SituacaoSolicitacao.ATENDIDO_PARCIALMENTE).Generate());

            await InserirNaBase(ObterAcervoSolicitacaoItem(1,1,situacao: SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO));
            await InserirNaBase(ObterAcervoSolicitacaoItem(1,2, TipoAtendimento.Email, situacao: SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE));
            await InserirNaBase(ObterAcervoSolicitacaoItem(1,3, TipoAtendimento.Presencial, DateTime.Now, SituacaoSolicitacaoItem.AGUARDANDO_VISITA));

            var retorno = await servicoAcervoSolicitacao.FinalizarAtendimentoItem(3);

            var acervosSolicitacoes = ObterTodos<AcervoSolicitacao>();
            acervosSolicitacoes.FirstOrDefault().Situacao.ShouldBe(SituacaoSolicitacao.ATENDIDO_PARCIALMENTE);

            var acervoSolicitacaoItens = ObterTodos<AcervoSolicitacaoItem>();
            acervoSolicitacaoItens.FirstOrDefault(f=> f.AcervoSolicitacaoId == 1 && f.AcervoId == 1).Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO);
            acervoSolicitacaoItens.FirstOrDefault(f=> f.AcervoSolicitacaoId == 1 && f.AcervoId == 2).Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            acervoSolicitacaoItens.FirstOrDefault(f=> f.AcervoSolicitacaoId == 1 && f.AcervoId == 3).Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
        }
    }
}