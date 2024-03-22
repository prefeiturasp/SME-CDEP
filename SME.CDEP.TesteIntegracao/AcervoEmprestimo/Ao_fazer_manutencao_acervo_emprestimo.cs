using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_fazer_manutencao_acervo_emprestimo : TesteBase
    {
        public Ao_fazer_manutencao_acervo_emprestimo(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
       
        [Fact(DisplayName = "Acervo empréstimo - Deve inserir empréstimo de acervo - persistência")]
        public async Task Deve_inserir_emprestimo_de_acervo()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes();

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DateTimeExtension.HorarioBrasilia().Date;
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 3);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

            var acervosEmprestados = ObterTodos<AcervoEmprestimo>();
            acervosEmprestados.Count().ShouldBe(3);
            
            //Assert
            foreach (var acervoEmprestado in acervosEmprestados)
            {
                acervoEmprestado.DataEmprestimo.Date.ShouldBe(dataVisita.Date);
                acervoEmprestado.DataDevolucao.Date.ShouldBe(dataVisita.AddDays(7).Date);
                acervoEmprestado.Excluido.ShouldBeFalse();
                acervoEmprestado.Situacao.ShouldBe(SituacaoEmprestimo.EMPRESTADO);
                acervoEmprestado.AcervoSolicitacaoItemId.ShouldBeGreaterThan(0);
            }
        }

        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Confirmar atendimento/empréstimo")]
        public async Task Deve_confirmar_atendimento_emprestimo()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes();

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DateTimeExtension.HorarioBrasilia().Date;
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 3);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

            //Act
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                TipoAcervo = TipoAcervo.Bibliografico

            });
                
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(7).Date,
                TipoAcervo = TipoAcervo.Bibliografico
                
            });
            
            //Assert
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
            solicitacaoAlterada.Excluido.ShouldBeFalse();
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(2);
            
            var itemAguardandoVisita = itensAlterados.FirstOrDefault(w => w.Id == 1);
            itemAguardandoVisita.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            itemAguardandoVisita.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemAguardandoVisita.Excluido.ShouldBeFalse();
            itemAguardandoVisita.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itemFinalizadoManualmente = itensAlterados.FirstOrDefault(w => w.Id == 2);
            itemFinalizadoManualmente.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            itemFinalizadoManualmente.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemFinalizadoManualmente.Excluido.ShouldBeFalse();
            itemFinalizadoManualmente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(2);
            eventos.All(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();
            
            var acervoEmprestimos = ObterTodos<AcervoEmprestimo>();
            acervoEmprestimos.Count().ShouldBe(5);
            
            acervoEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();
            acervoEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(8).Date).ShouldBeTrue();
            
            acervoEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();
            acervoEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(7).Date).ShouldBeTrue();
            
            acervoEmprestimos.All(a=> a.Situacao.EstaEmprestado()).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Não deve registrar empréstimo em alteração de data de visita")]
        public async Task Deve_confirmar_atendimento_mas_nao_deve_registrar_emprestimo_em_alteracao_de_data_de_visita()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervosBibliograficos();
        
            await InserirAcervosSolicitacoes();
        
            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DateTimeExtension.HorarioBrasilia().Date;
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
           
            //Act
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                TipoAcervo = TipoAcervo.Bibliografico
            });
             
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                TipoAcervo = TipoAcervo.Bibliografico
            });
            
            //Assert
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.AGUARDANDO_VISITA);
            solicitacaoAlterada.Excluido.ShouldBeFalse();
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(2);
            
            var itemAguardandoVisita = itensAlterados.FirstOrDefault(w => w.Id == 1);
            itemAguardandoVisita.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            itemAguardandoVisita.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemAguardandoVisita.Excluido.ShouldBeFalse();
            itemAguardandoVisita.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            
            var itemFinalizadoManualmente = itensAlterados.FirstOrDefault(w => w.Id == 2);
            itemFinalizadoManualmente.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            itemFinalizadoManualmente.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemFinalizadoManualmente.Excluido.ShouldBeFalse();
            itemFinalizadoManualmente.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(2);
            eventos.All(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();
            
            var acervoEmprestimos = ObterTodos<AcervoEmprestimo>();
            acervoEmprestimos.Count().ShouldBe(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Não deve confirmar empréstimo com acervos diferentes de bibliográficos")]
        public async Task Nao_deve_confirmar_atendimento_em_acervos_diferentes_de_bibliograficos()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervoTridimensional();
        
            await InserirAcervosSolicitacoes();
        
            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DateTimeExtension.HorarioBrasilia().Date;
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 3);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);
        
            //Act
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(7).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Não deve confirmar empréstimo com uma das datas de empréstimo e devolução faltando")]
        public async Task Nao_deve_confirmar_atendimento_com_uma_das_datas_de_emprestimo_e_devolucao_faltando()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervosBibliograficos();
        
            await InserirAcervosSolicitacoes();
        
            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DateTimeExtension.HorarioBrasilia().Date;
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 3);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);
        
            //Act
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                TipoAcervo = TipoAcervo.Bibliografico
            }).ShouldThrowAsync<NegocioException>();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                TipoAcervo = TipoAcervo.Bibliografico
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Não deve confirmar empréstimo com data de empréstimo futura")]
        public async Task Nao_deve_confirmar_atendimento_com_datas_de_emprestimo_futura()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervosBibliograficos();
        
            await InserirAcervosSolicitacoes();
        
            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DateTimeExtension.HorarioBrasilia().Date;
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 3);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);
        
            //Act
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(7).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Não deve confirmar empréstimo com data de empréstimo menor que data de visita")]
        public async Task Nao_deve_confirmar_atendimento_com_datas_de_emprestimo_menor_que_data_de_visita()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervosBibliograficos();
        
            await InserirAcervosSolicitacoes();
        
            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DateTimeExtension.HorarioBrasilia().Date;
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 3);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);
        
            //Act
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(-5).Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(-5).Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(7).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Não deve confirmar empréstimo com data de devolução menor que data do empréstimo")]
        public async Task Nao_deve_confirmar_atendimento_com_datas_de_devolucao_menor_que_data_do_emprestimo()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervosBibliograficos();
        
            await InserirAcervosSolicitacoes();
        
            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DateTimeExtension.HorarioBrasilia().Date;
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 3);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);
        
            //Act
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-8).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
                
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-7).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação empréstimo - Deve prorrogar empréstimo")]
        public async Task Deve_prorrogar_emprestimo()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DateTimeExtension.HorarioBrasilia().Date;
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

            //Act
            var servicoAcervoEmprestimo = GetServicoAcervoEmprestimo();
            
            var retorno = await servicoAcervoEmprestimo.ProrrogarEmprestimo(new AcervoEmprestimoProrrogacaoDTO()
            {
                AcervoSolicitacaoItemId = 1,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10).Date
            });
            retorno.ShouldBeTrue();
            
            //Assert
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
            solicitacaoAlterada.Excluido.ShouldBeFalse();
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(2);
            
            var itemAguardandoVisita = itensAlterados.FirstOrDefault(w => w.Id == 1);
            itemAguardandoVisita.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            itemAguardandoVisita.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemAguardandoVisita.Excluido.ShouldBeFalse();
            itemAguardandoVisita.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itemFinalizadoManualmente = itensAlterados.FirstOrDefault(w => w.Id == 2);
            itemFinalizadoManualmente.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            itemFinalizadoManualmente.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemFinalizadoManualmente.Excluido.ShouldBeFalse();
            itemFinalizadoManualmente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var acervoEmprestimos = ObterTodos<AcervoEmprestimo>();
            acervoEmprestimos.Count().ShouldBe(3);
            
            acervoEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();
            acervoEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(7).Date).ShouldBeTrue();
            
            acervoEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();
            acervoEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(7).Date).ShouldBeTrue();
            
            acervoEmprestimos.Count(a=> a.Situacao.EstaEmprestado()).ShouldBe(2);
            acervoEmprestimos.Count(a=> a.Situacao.EmprestadoComProrrogacao()).ShouldBe(1);
            acervoEmprestimos.Any(a=> a.Situacao.EmprestadoComProrrogacao() && a.DataDevolucao == DateTimeExtension.HorarioBrasilia().AddDays(10).Date).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação empréstimo - Não deve prorrogar empréstimo quando o item da solicitação não for encontrado")]
        public async Task Nao_deve_prorrogar_emprestimo_quando_item_da_solicitacao_nao_for_encontrado()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DateTimeExtension.HorarioBrasilia().Date;
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

            //Act
            var servicoAcervoEmprestimo = GetServicoAcervoEmprestimo();
            
            await servicoAcervoEmprestimo.ProrrogarEmprestimo(new AcervoEmprestimoProrrogacaoDTO()
            {
                AcervoSolicitacaoItemId = 1000,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10).Date
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação empréstimo - Não deve prorrogar empréstimo quando a data de devolução for no passado")]
        public async Task Nao_deve_prorrogar_emprestimo_quando_a_data_da_devolucao_for_no_passado()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DateTimeExtension.HorarioBrasilia().Date;
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

            //Act
            var servicoAcervoEmprestimo = GetServicoAcervoEmprestimo();
            
            await servicoAcervoEmprestimo.ProrrogarEmprestimo(new AcervoEmprestimoProrrogacaoDTO()
            {
                AcervoSolicitacaoItemId = 1000,
                DataDevolucao = DateTime.MinValue
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação empréstimo - Não deve prorrogar empréstimo quando a data de devolução for menor que a atual")]
        public async Task Nao_deve_prorrogar_emprestimo_quando_a_data_da_devolucao_for_menor_que_a_atual()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DateTimeExtension.HorarioBrasilia().Date;
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

            //Act
            var servicoAcervoEmprestimo = GetServicoAcervoEmprestimo();
            
            await servicoAcervoEmprestimo.ProrrogarEmprestimo(new AcervoEmprestimoProrrogacaoDTO()
            {
                AcervoSolicitacaoItemId = 1000,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(6)
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação empréstimo - Deve permitir devolver item emprestado")]
        public async Task Deve_permitir_devolver_item_emprestado()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DateTimeExtension.HorarioBrasilia().Date;
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

            //Act
            var servicoAcervoEmprestimo = GetServicoAcervoEmprestimo();
            
            var retorno = await servicoAcervoEmprestimo.DevolverItemEmprestado(1);
            retorno.ShouldBeTrue();
            
            var acervoEmprestimos = ObterTodos<AcervoEmprestimo>();
            acervoEmprestimos.Count().ShouldBe(3);
            
            acervoEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();
            acervoEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(7).Date).ShouldBeTrue();
            
            acervoEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();
            acervoEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(7).Date).ShouldBeTrue();
            
            acervoEmprestimos.Count(a=> a.Situacao.EstaEmprestado()).ShouldBe(2);
            acervoEmprestimos.Count(a=> a.Situacao.EstaDevolvido()).ShouldBe(1);
            acervoEmprestimos.Any(a=> a.Situacao.EstaDevolvido() && a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação empréstimo - Não deve permitir devolver item emprestado com item inválido")]
        public async Task Nao_deve_permitir_devolver_item_emprestado_com_item_invalido()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DateTimeExtension.HorarioBrasilia().Date;
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

            //Act
            var servicoAcervoEmprestimo = GetServicoAcervoEmprestimo();
            
            await servicoAcervoEmprestimo.DevolverItemEmprestado(1000).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Confirmar empréstimo após inserção manual")]
        public async Task Deve_permitir_fazer_acervo_manual_com_emprestimo()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            //Act
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var retorno = await servicoAcervoSolicitacao.Inserir(new AcervoSolicitacaoManualDTO()
            {
                UsuarioId = 1,
                DataSolicitacao = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                Itens = new List<AcervoSolicitacaoItemManualDTO>()
                {
                    new ()
                    {
                        AcervoId = 1,
                        TipoAtendimento = TipoAtendimento.Email
                    },
                    new ()
                    {
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                        TipoAcervo = TipoAcervo.Bibliografico
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                        TipoAcervo = TipoAcervo.Bibliografico
                    }
                }
            });

            //Assert
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().LastOrDefault();
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
            solicitacaoAlterada.Excluido.ShouldBeFalse();

            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>();
            itensAlterados.Count().ShouldBe(3);

            var itemEnviadoViaEmail = itensAlterados.FirstOrDefault(f=> f.AcervoId == 1);
            itemEnviadoViaEmail.DataVisita.ShouldBeNull();
            itemEnviadoViaEmail.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            itemEnviadoViaEmail.Excluido.ShouldBeFalse();
            itemEnviadoViaEmail.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);

            var itemAguardandoVisita = itensAlterados.FirstOrDefault(f=> f.AcervoId == 2);
            itemAguardandoVisita.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            itemAguardandoVisita.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemAguardandoVisita.Excluido.ShouldBeFalse();
            itemAguardandoVisita.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);

            var itemFinalizadoManualmente = itensAlterados.FirstOrDefault(w => w.AcervoId == 3);
            itemFinalizadoManualmente.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            itemFinalizadoManualmente.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemFinalizadoManualmente.Excluido.ShouldBeFalse();
            itemFinalizadoManualmente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);

            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(2);
            eventos.All(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();

            var acervoEmprestimos = ObterTodos<AcervoEmprestimo>();
            acervoEmprestimos.Count().ShouldBe(2);

            acervoEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();
            acervoEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(5).Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();

            acervoEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();
            acervoEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(8).Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();
        }

        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Confirmar empréstimo após alteração manual")]
        public async Task Deve_permitir_fazer_acervo_manual_com_emprestimo_apos_alteracao()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            //Act
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var retorno = await servicoAcervoSolicitacao.Inserir(new AcervoSolicitacaoManualDTO()
            {
                UsuarioId = 1,
                DataSolicitacao = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                Itens = new List<AcervoSolicitacaoItemManualDTO>()
                {
                    new ()
                    {
                        AcervoId = 1,
                        TipoAtendimento = TipoAtendimento.Email
                    },
                    new ()
                    {
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                        TipoAcervo = TipoAcervo.Bibliografico
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                        TipoAcervo = TipoAcervo.Bibliografico
                    }
                }
            });

            retorno = await servicoAcervoSolicitacao.Alterar(new AcervoSolicitacaoManualDTO()
            {
                Id = 1,
                UsuarioId = 1,
                DataSolicitacao = DateTimeExtension.HorarioBrasilia().Date.AddDays(-10),
                Itens = new List<AcervoSolicitacaoItemManualDTO>()
                {
                    new ()
                    {
                        Id = 1,
                        AcervoId = 1,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10).Date,
                        TipoAcervo = TipoAcervo.Bibliografico
                    },
                    new ()
                    {
                        Id = 2,
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(15).Date,
                        TipoAcervo = TipoAcervo.Bibliografico
                    },
                    new ()
                    {
                        Id = 3,
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(18).Date,
                        TipoAcervo = TipoAcervo.Bibliografico
                    }
                }
            });

            //Assert
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().LastOrDefault();
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
            solicitacaoAlterada.Excluido.ShouldBeFalse();

            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>();
            itensAlterados.Count().ShouldBe(3);

            var itemEnviadoViaEmail = itensAlterados.FirstOrDefault(f=> f.AcervoId == 1);
            itemEnviadoViaEmail.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            itemEnviadoViaEmail.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemEnviadoViaEmail.Excluido.ShouldBeFalse();
            itemEnviadoViaEmail.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);

            var itemAguardandoVisita = itensAlterados.FirstOrDefault(f=> f.AcervoId == 2);
            itemAguardandoVisita.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            itemAguardandoVisita.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemAguardandoVisita.Excluido.ShouldBeFalse();
            itemAguardandoVisita.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);

            var itemFinalizadoManualmente = itensAlterados.FirstOrDefault(w => w.AcervoId == 3);
            itemFinalizadoManualmente.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            itemFinalizadoManualmente.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemFinalizadoManualmente.Excluido.ShouldBeFalse();
            itemFinalizadoManualmente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);

            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(3);
            eventos.All(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();

            var acervoEmprestimos = ObterTodos<AcervoEmprestimo>();
            acervoEmprestimos.Count().ShouldBe(3);

            acervoEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();
            acervoEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(10).Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();

            acervoEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();
            acervoEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(15).Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();

            acervoEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();
            acervoEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(18).Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação empréstimo - Não deve permitir confirmar atendimento com data de empréstimo e devolução em data de visita futura")]
        public async Task Nao_deve_permitir_confirmar_atendimento_com_data_de_emprestimo_e_devolucao_em_visita_futura()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervosSolicitacoes();

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DateTimeExtension.HorarioBrasilia().Date;
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 3);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

            //Act
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            // await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            // {
            //     Id = 1,
            //     Itens = new List<AcervoSolicitacaoItemConfirmarDTO>()
            //     {
            //         new()
            //         {
            //             Id = 1,
            //             DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(1).Date,
            //             TipoAtendimento = TipoAtendimento.Presencial,
            //             DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(2).Date,
            //             DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10).Date,
            //             TipoAcervo = TipoAcervo.Fotografico
            //         },
            //         new()
            //         {
            //             Id = 2,
            //             DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(4).Date,
            //             TipoAtendimento = TipoAtendimento.Presencial,
            //             DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(3).Date,
            //             DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(11).Date,
            //             TipoAcervo = TipoAcervo.Fotografico
            //         }
            //     }
            // }).ShouldThrowAsync<NegocioException>();
        }
        
         [Fact(DisplayName = "Acervo Solicitação com empréstimo - Deve cancelar item emprestado quando remover as informações de empréstimo em acervo já emprestado")]
        public async Task Deve_cancelar_item_emprestado_quando_remover_as_informacoes_de_emprestimo_em_acervo_ja_emprestado()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes();

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DateTimeExtension.HorarioBrasilia().Date;
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

            //Act
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            // var retorno = await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            // {
            //     Id = 1,
            //     Itens = new List<AcervoSolicitacaoItemConfirmarDTO>()
            //     {
            //         new()
            //         {
            //             Id = 1,
            //             DataVisita = DateTimeExtension.HorarioBrasilia().Date,
            //             TipoAtendimento = TipoAtendimento.Presencial,
            //             TipoAcervo = TipoAcervo.Bibliografico
            //         },
            //         new()
            //         {
            //             Id = 2,
            //             DataVisita = DateTimeExtension.HorarioBrasilia().Date,
            //             TipoAtendimento = TipoAtendimento.Presencial,
            //             DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
            //             DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(7).Date,
            //             TipoAcervo = TipoAcervo.Bibliografico
            //         }
            //     }
            // });
            //
            // //Assert
            // var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            // solicitacaoAlterada.Id.ShouldBe(1);
            // solicitacaoAlterada.UsuarioId.ShouldBe(1);
            // solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.AGUARDANDO_VISITA);
            // solicitacaoAlterada.Excluido.ShouldBeFalse();
            //
            // var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            // itensAlterados.Count().ShouldBe(2);
            //
            // var itemAguardandoVisita = itensAlterados.FirstOrDefault(w => w.Id == 1);
            // itemAguardandoVisita.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            // itemAguardandoVisita.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            // itemAguardandoVisita.Excluido.ShouldBeFalse();
            // itemAguardandoVisita.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            //
            // var itemFinalizadoManualmente = itensAlterados.FirstOrDefault(w => w.Id == 2);
            // itemFinalizadoManualmente.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            // itemFinalizadoManualmente.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            // itemFinalizadoManualmente.Excluido.ShouldBeFalse();
            // itemFinalizadoManualmente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            //
            // var eventos = ObterTodos<Evento>();
            // eventos.Count(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().Date && !a.Excluido).ShouldBe(2);
            //
            // var acervoEmprestimos = ObterTodos<AcervoEmprestimo>();
            // acervoEmprestimos.Count().ShouldBe(2);
            //
            // acervoEmprestimos.Count(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBe(2);
            // acervoEmprestimos.Count(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(7).Date).ShouldBe(2);
            //
            // acervoEmprestimos.Count(a=> a.Situacao.EstaEmprestado()).ShouldBe(1);
            // acervoEmprestimos.Count(a=> a.Situacao.EstaCancelado()).ShouldBe(1);
        }

        private async Task InserirAcervoSolicitacaoItem(TipoAtendimento atendimentoPresencial, DateTime dataVisita, long acervoSolicitacaoId, int qtdeItens, SituacaoSolicitacaoItem situacaoSolicitacaoItem = SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO)
        {
            await InserirVariosNaBase(AcervoSolicitacaoItemMock.Instance.Gerar(acervoSolicitacaoId,atendimentoPresencial, dataVisita, situacaoSolicitacaoItem).Generate(qtdeItens));
        }

        private async Task InserirAcervosSolicitacoes(SituacaoSolicitacao situacaoSolicitacao = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO)
        {
            var inserindoSolicitacoes = AcervoSolicitacaoMock.Instance.Gerar(situacaoSolicitacao).Generate(4);
            await InserirVariosNaBase(inserindoSolicitacoes);
        }

        private async Task InserirAcervoEmprestimo(long id, DateTime dataVisita)
        {
            var inserirAcervoEmprestimo = AcervoEmprestimoMock.Instance.Gerar(id, dataVisita).Generate();
            await InserirNaBase(inserirAcervoEmprestimo);
        }
    }
}