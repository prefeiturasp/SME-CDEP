using Shouldly;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_finalizar_acervo_solicitacao : TesteBase
    {
        private readonly IServicoAcervoSolicitacao servicoAcervoSolicitacao;

        public Ao_finalizar_acervo_solicitacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
        }
       
        [Fact(DisplayName = "Acervo Solicitação - Deve finalizar com todos os itens cancelados")]
        public async Task Deve_finalizar_atendimento_com_itens_cancelados()
        {
            CriarClaimUsuario();
            
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
            
            var retorno = await servicoAcervoSolicitacao.FinalizarAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve finalizar com itens em situação aguardando atendimento")]
        public async Task Nao_deve_finalizar_atendimento_com_itens_aguardando_atendimento()
        {
            CriarClaimUsuario();
            
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
            
            await servicoAcervoSolicitacao.FinalizarAtendimento(1).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve finalizar atendimento com itens em situação aguardando visita que já passou")]
        public async Task Deve_finalizar_atendimento_com_itens_em_situacao_aguardando_visita_que_ja_passou()
        {
            CriarClaimUsuario();
            
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
                item.DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(-2);
                await InserirNaBase(item);
            }
            
            var retorno = await servicoAcervoSolicitacao.FinalizarAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve finalizar atendimento com itens em situação finalizada manualmente")]
        public async Task Deve_finalizar_atendimento_com_itens_em_situacao_finalizada_manualmente()
        {
            CriarClaimUsuario();
            
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
            
            var retorno = await servicoAcervoSolicitacao.FinalizarAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve finalizar atendimento com itens em situação finalizada automaticamente")]
        public async Task Deve_finalizar_atendimento_com_itens_em_situacao_finalizada_automaticamente()
        {
            CriarClaimUsuario();
            
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
            
            var retorno = await servicoAcervoSolicitacao.FinalizarAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve finalizar atendimento com itens em situação aguardando visita com data de visita futura")]
        public async Task Nao_deve_finalizar_atendimento_com_itens_em_situacao_aguardando_visita_com_data_de_visita_futura()
        {
            CriarClaimUsuario();
            
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
                item.DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(2);
                await InserirNaBase(item);
            }
            
            await servicoAcervoSolicitacao.FinalizarAtendimento(1).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve finalizar atendimento que foi parcialmente atendido")]
        public async Task Nao_deve_finalizar_atendimento_que_foi_parcialmente_atendido()
        {
            CriarClaimUsuario();
            
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
                item.DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(2);
                await InserirNaBase(item);
            }
            
            await servicoAcervoSolicitacao.FinalizarAtendimento(1).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve finalizar somente os itens do perfil e não finalizar o atendimento quando perfil não Admin Geral")]
        public async Task Deve_finalizar_somente_os_itens_do_perfil_e_nao_finalizar_o_atendimento()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_MEMORIAL_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficosEmMassa(1, 2);
            await InserirAcervosArteGraficasEmMassa(3, 1);

            await InserirNaBase(AcervoSolicitacaoMock.Instance.Gerar(SituacaoSolicitacao.ATENDIDO_PARCIALMENTE).Generate());

            await InserirNaBase(ObterAcervoSolicitacaoItem(1,1));
            await InserirNaBase(ObterAcervoSolicitacaoItem(1,2));
            await InserirNaBase(ObterAcervoSolicitacaoItem(1,3, TipoAtendimento.Presencial, DateTime.Now, SituacaoSolicitacaoItem.AGUARDANDO_VISITA));
            
            var retorno = await servicoAcervoSolicitacao.FinalizarAtendimento(1);

            var acervosSolicitacoes = ObterTodos<AcervoSolicitacao>();
            acervosSolicitacoes.FirstOrDefault().Situacao.ShouldBe(SituacaoSolicitacao.ATENDIDO_PARCIALMENTE);
            
            var acervoSolicitacaoItens = ObterTodos<AcervoSolicitacaoItem>();
            acervoSolicitacaoItens.FirstOrDefault(f=> f.AcervoSolicitacaoId == 1 && f.AcervoId == 1).Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO);
            acervoSolicitacaoItens.FirstOrDefault(f=> f.AcervoSolicitacaoId == 1 && f.AcervoId == 2).Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO);
            acervoSolicitacaoItens.FirstOrDefault(f=> f.AcervoSolicitacaoId == 1 && f.AcervoId == 3).Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve finalizar todos os itens e atendimento quando perfil Admin Geral")]
        public async Task Deve_finalizar_todos_os_itens_quando_perfil_admin_geral()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficosEmMassa(1, 2);
            await InserirAcervosArteGraficasEmMassa(3, 1);

            await InserirNaBase(AcervoSolicitacaoMock.Instance.Gerar(SituacaoSolicitacao.ATENDIDO_PARCIALMENTE).Generate());

            await InserirNaBase(ObterAcervoSolicitacaoItem(1,1,situacao: SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE));
            await InserirNaBase(ObterAcervoSolicitacaoItem(1,2, TipoAtendimento.Email, situacao: SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE));
            await InserirNaBase(ObterAcervoSolicitacaoItem(1,3, TipoAtendimento.Presencial, DateTime.Now, SituacaoSolicitacaoItem.AGUARDANDO_VISITA));
            
            var retorno = await servicoAcervoSolicitacao.FinalizarAtendimento(1);

            var acervosSolicitacoes = ObterTodos<AcervoSolicitacao>();
            acervosSolicitacoes.FirstOrDefault().Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
            
            var acervoSolicitacaoItens = ObterTodos<AcervoSolicitacaoItem>();
            acervoSolicitacaoItens.FirstOrDefault(f=> f.AcervoSolicitacaoId == 1 && f.AcervoId == 1).Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE);
            acervoSolicitacaoItens.FirstOrDefault(f=> f.AcervoSolicitacaoId == 1 && f.AcervoId == 2).Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            acervoSolicitacaoItens.FirstOrDefault(f=> f.AcervoSolicitacaoId == 1 && f.AcervoId == 3).Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve finalizar os itens do perfil e atendimento quando os outros itens já foram atendidos em perfil não Admin Geral")]
        public async Task Deve_finalizar_os_itens_do_perfil_quando_os_outros_itens_foram_atendidos_em_perfil_nao_admin_geral()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_MEMORIAL_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficosEmMassa(1, 2);
            await InserirAcervosArteGraficasEmMassa(3, 1);

            await InserirNaBase(AcervoSolicitacaoMock.Instance.Gerar(SituacaoSolicitacao.ATENDIDO_PARCIALMENTE).Generate());

            await InserirNaBase(ObterAcervoSolicitacaoItem(1,1,situacao: SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE));
            await InserirNaBase(ObterAcervoSolicitacaoItem(1,2, TipoAtendimento.Email, situacao: SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE));
            await InserirNaBase(ObterAcervoSolicitacaoItem(1,3, TipoAtendimento.Presencial, DateTime.Now, SituacaoSolicitacaoItem.AGUARDANDO_VISITA));
            
            var retorno = await servicoAcervoSolicitacao.FinalizarAtendimento(1);

            var acervosSolicitacoes = ObterTodos<AcervoSolicitacao>();
            acervosSolicitacoes.FirstOrDefault().Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
            
            var acervoSolicitacaoItens = ObterTodos<AcervoSolicitacaoItem>();
            acervoSolicitacaoItens.FirstOrDefault(f=> f.AcervoSolicitacaoId == 1 && f.AcervoId == 1).Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE);
            acervoSolicitacaoItens.FirstOrDefault(f=> f.AcervoSolicitacaoId == 1 && f.AcervoId == 2).Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            acervoSolicitacaoItens.FirstOrDefault(f=> f.AcervoSolicitacaoId == 1 && f.AcervoId == 3).Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
        }
    }
}