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
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes();

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 3);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

            var acervosEmprestados = ObterTodos<AcervoEmprestimo>();
            acervosEmprestados.Count().ShouldBe(3);
            
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
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes();

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 3);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                TipoAcervo = TipoAcervo.Bibliografico

            });
                
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(7).Date,
                TipoAcervo = TipoAcervo.Bibliografico
                
            });
            
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
            solicitacaoAlterada.Excluido.ShouldBeFalse();
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(2);
            
            var itemAguardandoVisita = itensAlterados.FirstOrDefault(w => w.Id == 1);
            itemAguardandoVisita.DataVisita.Value.Date.ShouldBe(DataHelper.ProximaDataUtil(DateTime.Now));
            itemAguardandoVisita.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemAguardandoVisita.Excluido.ShouldBeFalse();
            itemAguardandoVisita.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itemFinalizadoManualmente = itensAlterados.FirstOrDefault(w => w.Id == 2);
            itemFinalizadoManualmente.DataVisita.Value.Date.ShouldBe(DataHelper.ProximaDataUtil(DateTime.Now));
            itemFinalizadoManualmente.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemFinalizadoManualmente.Excluido.ShouldBeFalse();
            itemFinalizadoManualmente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(2);
            eventos.All(a=> a.Data.Date == DataHelper.ProximaDataUtil(DateTime.Now)).ShouldBeTrue();
            
            var acervoEmprestimos = ObterTodos<AcervoEmprestimo>();
            acervoEmprestimos.Count().ShouldBe(5);
            
            acervoEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();
            acervoEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(8).Date).ShouldBeTrue();
            
            acervoEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();
            acervoEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(7).Date).ShouldBeTrue();
            
            acervoEmprestimos.All(a=> a.Situacao.EstaEmprestado()).ShouldBeTrue();

            var acervosBibliograficos = ObterTodos<AcervoBibliografico>();
            acervosBibliograficos.FirstOrDefault(f=> f.AcervoId == 1).SituacaoSaldo.ShouldBe(SituacaoSaldo.EMPRESTADO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Deve registrar empréstimo em alteração de data de visita")]
        public async Task Deve_confirmar_atendimento_mas_nao_deve_registrar_emprestimo_em_alteracao_de_data_de_visita()
        {
            //Arrange
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervosBibliograficos();
        
            await InserirAcervosSolicitacoes();
        
            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
           
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                TipoAtendimento = TipoAtendimento.Presencial,
                TipoAcervo = TipoAcervo.Bibliografico
            });
             
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                TipoAtendimento = TipoAtendimento.Presencial,
                TipoAcervo = TipoAcervo.Bibliografico
            });
            
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.AGUARDANDO_VISITA);
            solicitacaoAlterada.Excluido.ShouldBeFalse();
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(2);
            
            var itemAguardandoVisita = itensAlterados.FirstOrDefault(w => w.Id == 1);
            itemAguardandoVisita.DataVisita.Value.Date.ShouldBe(DataHelper.ProximaDataUtil(DateTime.Now));
            itemAguardandoVisita.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemAguardandoVisita.Excluido.ShouldBeFalse();
            itemAguardandoVisita.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            
            var itemFinalizadoManualmente = itensAlterados.FirstOrDefault(w => w.Id == 2);
            itemFinalizadoManualmente.DataVisita.Value.Date.ShouldBe(DataHelper.ProximaDataUtil(DateTime.Now));
            itemFinalizadoManualmente.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemFinalizadoManualmente.Excluido.ShouldBeFalse();
            itemFinalizadoManualmente.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(2);
            eventos.All(a=> a.Data.Date == DataHelper.ProximaDataUtil(DateTime.Now)).ShouldBeTrue();
            
            var acervoEmprestimos = ObterTodos<AcervoEmprestimo>();
            acervoEmprestimos.Count().ShouldBe(0);
            
            var acervosBibliograficos = ObterTodos<AcervoBibliografico>();
            acervosBibliograficos.FirstOrDefault(f=> f.AcervoId == 1).SituacaoSaldo.ShouldBe(SituacaoSaldo.RESERVADO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Não deve confirmar empréstimo com acervos diferentes de bibliográficos")]
        public async Task Nao_deve_confirmar_atendimento_em_acervos_diferentes_de_bibliograficos()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervoTridimensional();
        
            await InserirAcervosSolicitacoes();
        
            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
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
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(7).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Não deve confirmar empréstimo com uma das datas de empréstimo e devolução faltando")]
        public async Task Nao_deve_confirmar_atendimento_com_uma_das_datas_de_emprestimo_e_devolucao_faltando()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervosBibliograficos();
        
            await InserirAcervosSolicitacoes();
        
            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 3);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);
        
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                TipoAtendimento = TipoAtendimento.Presencial,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                TipoAcervo = TipoAcervo.Bibliografico
            }).ShouldThrowAsync<NegocioException>();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                TipoAcervo = TipoAcervo.Bibliografico
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Não deve confirmar empréstimo com data de empréstimo futura")]
        public async Task Nao_deve_confirmar_atendimento_com_datas_de_emprestimo_futura()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervosBibliograficos();
        
            await InserirAcervosSolicitacoes();
        
            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 3);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);
        
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(7).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Não deve confirmar empréstimo com data de empréstimo menor que data de visita")]
        public async Task Nao_deve_confirmar_atendimento_com_datas_de_emprestimo_menor_que_data_de_visita()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervosBibliograficos();
        
            await InserirAcervosSolicitacoes();
        
            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 3);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);
        
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(-5).Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(-5).Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(7).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Não deve confirmar empréstimo com data de devolução menor que data do empréstimo")]
        public async Task Nao_deve_confirmar_atendimento_com_datas_de_devolucao_menor_que_data_do_emprestimo()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervosBibliograficos();
        
            await InserirAcervosSolicitacoes();
        
            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 3);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);
        
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-8).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
                
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-7).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação empréstimo - Deve prorrogar empréstimo")]
        public async Task Deve_prorrogar_emprestimo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

            var servicoAcervoEmprestimo = GetServicoAcervoEmprestimo();
            
            var retorno = await servicoAcervoEmprestimo.ProrrogarEmprestimo(new AcervoEmprestimoProrrogacaoDTO()
            {
                AcervoSolicitacaoItemId = 1,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10).Date
            });
            retorno.ShouldBeTrue();
            
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
            solicitacaoAlterada.Excluido.ShouldBeFalse();
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(2);
            
            var itemAguardandoVisita = itensAlterados.FirstOrDefault(w => w.Id == 1);
            itemAguardandoVisita.DataVisita.Value.Date.ShouldBe(DataHelper.ProximaDataUtil(DateTime.Now));
            itemAguardandoVisita.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemAguardandoVisita.Excluido.ShouldBeFalse();
            itemAguardandoVisita.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itemFinalizadoManualmente = itensAlterados.FirstOrDefault(w => w.Id == 2);
            itemFinalizadoManualmente.DataVisita.Value.Date.ShouldBe(DataHelper.ProximaDataUtil(DateTime.Now));
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
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

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
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

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
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

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
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

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
            
            var acervosBibliograficos = ObterTodos<AcervoBibliografico>();
            acervosBibliograficos.FirstOrDefault(f=> f.AcervoId == 1).SituacaoSaldo.ShouldBe(SituacaoSaldo.DISPONIVEL);
        }
        
        [Fact(DisplayName = "Acervo Solicitação empréstimo - Não deve permitir devolver item emprestado com item inválido")]
        public async Task Nao_deve_permitir_devolver_item_emprestado_com_item_invalido()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1, SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

            var servicoAcervoEmprestimo = GetServicoAcervoEmprestimo();
            
            await servicoAcervoEmprestimo.DevolverItemEmprestado(1000).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Confirmar empréstimo após inserção manual")]
        public async Task Deve_permitir_fazer_acervo_manual_com_emprestimo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

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
                        DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                        TipoAcervo = TipoAcervo.Bibliografico
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                        TipoAcervo = TipoAcervo.Bibliografico
                    }
                }
            });

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
            itemAguardandoVisita.DataVisita.Value.Date.ShouldBe(DataHelper.ProximaDataUtil(DateTime.Now));
            itemAguardandoVisita.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemAguardandoVisita.Excluido.ShouldBeFalse();
            itemAguardandoVisita.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);

            var itemFinalizadoManualmente = itensAlterados.FirstOrDefault(w => w.AcervoId == 3);
            itemFinalizadoManualmente.DataVisita.Value.Date.ShouldBe(DataHelper.ProximaDataUtil(DateTime.Now));
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
            
            var acervosBibliograficos = ObterTodos<AcervoBibliografico>();
            acervosBibliograficos.FirstOrDefault(f=> f.AcervoId == 2).SituacaoSaldo.ShouldBe(SituacaoSaldo.EMPRESTADO);
            acervosBibliograficos.FirstOrDefault(f=> f.AcervoId == 3).SituacaoSaldo.ShouldBe(SituacaoSaldo.EMPRESTADO);
        }

        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Confirmar empréstimo após alteração manual")]
        public async Task Deve_permitir_fazer_acervo_manual_com_emprestimo_apos_alteracao()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

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
                        DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                        TipoAcervo = TipoAcervo.Bibliografico
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
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
                        DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10).Date,
                        TipoAcervo = TipoAcervo.Bibliografico
                    },
                    new ()
                    {
                        Id = 2,
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(15).Date,
                        TipoAcervo = TipoAcervo.Bibliografico
                    },
                    new ()
                    {
                        Id = 3,
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(18).Date,
                        TipoAcervo = TipoAcervo.Bibliografico
                    }
                }
            });

            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().LastOrDefault();
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
            solicitacaoAlterada.Excluido.ShouldBeFalse();

            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>();
            itensAlterados.Count().ShouldBe(3);

            var itemEnviadoViaEmail = itensAlterados.FirstOrDefault(f=> f.AcervoId == 1);
            itemEnviadoViaEmail.DataVisita.Value.Date.ShouldBe(DataHelper.ProximaDataUtil(DateTime.Now));
            itemEnviadoViaEmail.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemEnviadoViaEmail.Excluido.ShouldBeFalse();
            itemEnviadoViaEmail.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);

            var itemAguardandoVisita = itensAlterados.FirstOrDefault(f=> f.AcervoId == 2);
            itemAguardandoVisita.DataVisita.Value.Date.ShouldBe(DataHelper.ProximaDataUtil(DateTime.Now));
            itemAguardandoVisita.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemAguardandoVisita.Excluido.ShouldBeFalse();
            itemAguardandoVisita.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);

            var itemFinalizadoManualmente = itensAlterados.FirstOrDefault(w => w.AcervoId == 3);
            itemFinalizadoManualmente.DataVisita.Value.Date.ShouldBe(DataHelper.ProximaDataUtil(DateTime.Now));
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
            
            var acervosBibliograficos = ObterTodos<AcervoBibliografico>();
            acervosBibliograficos.FirstOrDefault(f=> f.AcervoId == 1).SituacaoSaldo.ShouldBe(SituacaoSaldo.EMPRESTADO);
            acervosBibliograficos.FirstOrDefault(f=> f.AcervoId == 2).SituacaoSaldo.ShouldBe(SituacaoSaldo.EMPRESTADO);
            acervosBibliograficos.FirstOrDefault(f=> f.AcervoId == 3).SituacaoSaldo.ShouldBe(SituacaoSaldo.EMPRESTADO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação empréstimo - Não deve permitir confirmar atendimento com data de empréstimo e devolução em data de visita futura")]
        public async Task Nao_deve_permitir_confirmar_atendimento_com_data_de_emprestimo_e_devolucao_em_visita_futura()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervosSolicitacoes();

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 3);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1, 
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now.AddDays(1)),
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(2).Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now.AddDays(4)),
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(3).Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(11).Date,
                TipoAcervo = TipoAcervo.Fotografico
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação empréstimo - Não deve permitir confirmar atendimento alterando data de empréstimo e devolução")]
        public async Task Nao_deve_permitir_confirmar_atendimento_alterando_data_de_emprestimo_e_devolucao()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervosSolicitacoes();

            var atendimentoPresencial = TipoAtendimento.Presencial;
            var dataVisita = DataHelper.ProximaDataUtil(DateTime.Now);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,1,2);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,2,4);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,3,3);
            await InserirAcervoSolicitacaoItem(atendimentoPresencial, dataVisita,4,1);
            
            var itensDaSolicitacao = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            
            foreach (var item in itensDaSolicitacao)
                await InserirAcervoEmprestimo(item.Id, dataVisita);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1, 
                DataVisita = DataHelper.ProximaDataUtil(DateTime.Now),
                TipoAtendimento = TipoAtendimento.Presencial,
                DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10).Date,
                TipoAcervo = TipoAcervo.Bibliografico
            }).ShouldThrowAsync<NegocioException>();
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