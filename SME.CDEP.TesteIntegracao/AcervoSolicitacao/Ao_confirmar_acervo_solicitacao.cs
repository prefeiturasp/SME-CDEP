using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_confirmar_acervo_solicitacao : TesteBase
    {
        public Ao_confirmar_acervo_solicitacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
       
       
        [Fact(DisplayName = "Acervo Solicitação - Confirmar acervos tridimensionais")]
        public async Task Deve_confirmar_atendimento_tridimensional()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervoTridimensional();
        
            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                TipoAcervo = TipoAcervo.Tridimensional
            });
                
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                TipoAtendimento = TipoAtendimento.Email,
                TipoAcervo = TipoAcervo.Tridimensional
            });
            
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.AGUARDANDO_VISITA);
            solicitacaoAlterada.Excluido.ShouldBeFalse();
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(3);
            
            var itemAguardandoVisita = itensAlterados.FirstOrDefault(w => w.Id == 1);
            itemAguardandoVisita.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            itemAguardandoVisita.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemAguardandoVisita.Excluido.ShouldBeFalse();
            itemAguardandoVisita.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            
            var itemFinalizadoManualmente = itensAlterados.FirstOrDefault(w => w.Id == 2);
            itemFinalizadoManualmente.DataVisita.ShouldBeNull();
            itemFinalizadoManualmente.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            itemFinalizadoManualmente.Excluido.ShouldBeFalse();
            itemFinalizadoManualmente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itemFinalizadoAutomaticamente = itensAlterados.FirstOrDefault(w => w.Id == 3);
            itemFinalizadoAutomaticamente.DataVisita.ShouldBeNull();
            itemFinalizadoAutomaticamente.TipoAtendimento.ShouldBeNull();
            itemFinalizadoAutomaticamente.Excluido.ShouldBeFalse();
            itemFinalizadoAutomaticamente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE);
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(1);
            eventos.Any(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();
            
            var acervosBibliograficos = ObterTodos<AcervoBibliografico>();
            acervosBibliograficos.Count().ShouldBe(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Confirmar acervos bibliograficos")]
        public async Task Deve_confirmar_atendimento_acervos_bibliograficos()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervosBibliograficos();
        
            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                TipoAcervo = TipoAcervo.Bibliografico
            });
            
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.ATENDIDO_PARCIALMENTE);
            solicitacaoAlterada.Excluido.ShouldBeFalse();
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(3);
            
            var itemAguardandoVisita = itensAlterados.FirstOrDefault(w => w.Id == 1);
            itemAguardandoVisita.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            itemAguardandoVisita.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemAguardandoVisita.Excluido.ShouldBeFalse();
            itemAguardandoVisita.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            
            var itemFinalizadoManualmente = itensAlterados.FirstOrDefault(w => w.Id == 2);
            itemFinalizadoManualmente.DataVisita.ShouldBeNull();
            itemFinalizadoManualmente.TipoAtendimento.ShouldBeNull();
            itemFinalizadoManualmente.Excluido.ShouldBeFalse();
            itemFinalizadoManualmente.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO);
            
            var itemFinalizadoAutomaticamente = itensAlterados.FirstOrDefault(w => w.Id == 3);
            itemFinalizadoAutomaticamente.DataVisita.ShouldBeNull();
            itemFinalizadoAutomaticamente.TipoAtendimento.ShouldBeNull();
            itemFinalizadoAutomaticamente.Excluido.ShouldBeFalse();
            itemFinalizadoAutomaticamente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE);
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(1);
            eventos.Any(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();
            
            var acervosBibliograficos = ObterTodos<AcervoBibliografico>();
            acervosBibliograficos.FirstOrDefault(f=> f.AcervoId == 1).SituacaoSaldo.ShouldBe(SituacaoSaldo.RESERVADO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Confirmar o atendimento como finalizado apenas quando todos os itens forem finalizados manualmente")]
        public async Task Deve_confirmar_e_alterar_a_situação_do_atendimento_para_finalizado_quando_todos_os_itens_estiverem_finalizados_manualmente()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervoTridimensional();
        
            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                TipoAtendimento = TipoAtendimento.Email
            });
                
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                TipoAtendimento = TipoAtendimento.Email
            });
            
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO); 
            solicitacaoAlterada.Excluido.ShouldBeFalse();
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(3);
            
            var primeiroItemFinalizadoManualmente = itensAlterados.FirstOrDefault(w => w.Id == 1);
            primeiroItemFinalizadoManualmente.DataVisita.ShouldBeNull();
            primeiroItemFinalizadoManualmente.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            primeiroItemFinalizadoManualmente.Excluido.ShouldBeFalse();
            primeiroItemFinalizadoManualmente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var ultimoItemFinalizadoManualmente = itensAlterados.LastOrDefault(w => w.Id == 2);
            ultimoItemFinalizadoManualmente.DataVisita.ShouldBeNull();
            ultimoItemFinalizadoManualmente.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            ultimoItemFinalizadoManualmente.Excluido.ShouldBeFalse();
            ultimoItemFinalizadoManualmente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itemFinalizadoAutomaticamente = itensAlterados.FirstOrDefault(w => w.Id == 3);
            itemFinalizadoAutomaticamente.DataVisita.ShouldBeNull();
            itemFinalizadoAutomaticamente.TipoAtendimento.ShouldBeNull();
            itemFinalizadoAutomaticamente.Excluido.ShouldBeFalse();
            itemFinalizadoAutomaticamente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE);
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Confirmar o atendimento como finalizado apenas quando todos os itens forem aguardando visita")]
        public async Task Deve_confirmar_e_alterar_a_situação_do_atendimento_para_finalizado_quando_todos_os_itens_estiverem_aguardando_visita()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervoTridimensional();
        
            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date.AddDays(4).Date
            });
                
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date.AddDays(8).Date
            });
            
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.AGUARDANDO_VISITA); 
            solicitacaoAlterada.Excluido.ShouldBeFalse();
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(3);
            
            var primeiroItemAguardandoVisita = itensAlterados.FirstOrDefault(w => w.Id == 1);
            primeiroItemAguardandoVisita.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            primeiroItemAguardandoVisita.DataVisita.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.AddDays(4).Date);
            primeiroItemAguardandoVisita.Excluido.ShouldBeFalse();
            primeiroItemAguardandoVisita.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            
            var ultimoItemAguardandoVisita = itensAlterados.FirstOrDefault(w => w.Id == 2);
            ultimoItemAguardandoVisita.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            ultimoItemAguardandoVisita.DataVisita.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.AddDays(8).Date);
            ultimoItemAguardandoVisita.Excluido.ShouldBeFalse();
            ultimoItemAguardandoVisita.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            
            var itemFinalizadoManualmente = itensAlterados.FirstOrDefault(w => w.Id == 3);
            itemFinalizadoManualmente.DataVisita.ShouldBeNull();
            itemFinalizadoManualmente.TipoAtendimento.ShouldBeNull();
            itemFinalizadoManualmente.Excluido.ShouldBeFalse();
            itemFinalizadoManualmente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE);
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(2);
        }
        
         [Fact(DisplayName = "Acervo Solicitação - Confirmar o atendimento como aguardando visita quando itens forem aguardando visita e finalizados manualmente")]
        public async Task Deve_confirmar_e_alterar_a_situação_do_atendimento_para_aguardando_visita_quando_itens_forem_aguardando_visita_e_finalizados_manualmente()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervoTridimensional();
        
            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date.AddDays(4).Date
            });
                
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                TipoAtendimento = TipoAtendimento.Email
            });
            
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.AGUARDANDO_VISITA); 
            solicitacaoAlterada.Excluido.ShouldBeFalse();
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(3);
            
            var itemAguardandoVisita = itensAlterados.FirstOrDefault(w => w.Id == 1);
            itemAguardandoVisita.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemAguardandoVisita.DataVisita.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.AddDays(4).Date);
            itemAguardandoVisita.Excluido.ShouldBeFalse();
            itemAguardandoVisita.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            
            var itemFinalizadoManualmente = itensAlterados.FirstOrDefault(w => w.Id == 2);
            itemFinalizadoManualmente.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            itemFinalizadoManualmente.DataVisita.ShouldBeNull();
            itemFinalizadoManualmente.Excluido.ShouldBeFalse();
            itemFinalizadoManualmente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itemFinalizadoAutomaticamente = itensAlterados.FirstOrDefault(w => w.Id == 3);
            itemFinalizadoAutomaticamente.DataVisita.ShouldBeNull();
            itemFinalizadoAutomaticamente.TipoAtendimento.ShouldBeNull();
            itemFinalizadoAutomaticamente.Excluido.ShouldBeFalse();
            itemFinalizadoAutomaticamente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE);
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(1);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Confirmar o atendimento como atendido parcialmente quando itens forem aguardando visita e aguardando atendimento")]
        public async Task Deve_confirmar_e_alterar_a_situação_do_atendimento_para_atendido_parcialmente_quando_itens_forem_aguardando_visita_e_aguardando_atendimento()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervoTridimensional();
        
            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var retorno = await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date.AddDays(4).Date
            });
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.ATENDIDO_PARCIALMENTE); 
            solicitacaoAlterada.Excluido.ShouldBeFalse();
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(3);
            
            var itemAguardandoVisita = itensAlterados.FirstOrDefault(w => w.Id == 1);
            itemAguardandoVisita.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itemAguardandoVisita.DataVisita.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.AddDays(4).Date);
            itemAguardandoVisita.Excluido.ShouldBeFalse();
            itemAguardandoVisita.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            
            var itemAguardandoAtendimento = itensAlterados.FirstOrDefault(w => w.Id == 2);
            itemAguardandoAtendimento.TipoAtendimento.ShouldBeNull();
            itemAguardandoAtendimento.DataVisita.ShouldBeNull();
            itemAguardandoAtendimento.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO);
            itemAguardandoAtendimento.Excluido.ShouldBeFalse();
            
            var itemFinalizadoAutomaticamente = itensAlterados.FirstOrDefault(w => w.Id == 3);
            itemFinalizadoAutomaticamente.DataVisita.ShouldBeNull();
            itemFinalizadoAutomaticamente.TipoAtendimento.ShouldBeNull();
            itemFinalizadoAutomaticamente.Excluido.ShouldBeFalse();
            itemFinalizadoAutomaticamente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE);
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(1);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Confirmar o atendimento como atendido parcialmente quando itens forem finalizados manualmente e aguardando atendimento")]
        public async Task Deve_confirmar_e_alterar_a_situação_do_atendimento_para_atendido_parcialmente_quando_itens_forem_finalizados_manualmente_e_aguardando_atendimento()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervoTridimensional();
        
            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var retorno = await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                TipoAtendimento = TipoAtendimento.Email
            });
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.ATENDIDO_PARCIALMENTE); 
            solicitacaoAlterada.Excluido.ShouldBeFalse();
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(3);
            
            var itemFinalizadoManualmente = itensAlterados.FirstOrDefault(w => w.Id == 1);
            itemFinalizadoManualmente.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            itemFinalizadoManualmente.DataVisita.ShouldBeNull();
            itemFinalizadoManualmente.Excluido.ShouldBeFalse();
            itemFinalizadoManualmente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            
            var itemAguardandoAtendimento = itensAlterados.FirstOrDefault(w => w.Id == 2);
            itemAguardandoAtendimento.TipoAtendimento.ShouldBeNull();
            itemAguardandoAtendimento.DataVisita.ShouldBeNull();
            itemAguardandoAtendimento.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO);
            itemAguardandoAtendimento.Excluido.ShouldBeFalse();
            
            var itemFinalizadoAutomaticamente = itensAlterados.FirstOrDefault(w => w.Id == 3);
            itemFinalizadoAutomaticamente.DataVisita.ShouldBeNull();
            itemFinalizadoAutomaticamente.TipoAtendimento.ShouldBeNull();
            itemFinalizadoAutomaticamente.Excluido.ShouldBeFalse();
            itemFinalizadoAutomaticamente.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE);
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Confirmar acervos tridimensionais editando tipo de atendimento e data de visita")]
        public async Task Deve_confirmar_acervos_tridimensionais_atendimento_editando_confirmacao()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervoTridimensional();
        
            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                TipoAcervo = TipoAcervo.Tridimensional
            });
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                TipoAtendimento = TipoAtendimento.Email,
                TipoAcervo = TipoAcervo.Tridimensional
            });
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                TipoAtendimento = TipoAtendimento.Email,
                TipoAcervo = TipoAcervo.Tridimensional
            });
                
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(10).Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                TipoAcervo = TipoAcervo.Tridimensional
            });
            
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.AGUARDANDO_VISITA);
            solicitacaoAlterada.Excluido.ShouldBeFalse();
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(3);
            
            var itensAlteradosPresenciais = itensAlterados.Where(w => w.TipoAtendimento.HasValue && w.TipoAtendimento.Value.EhAtendimentoPresencial());
            itensAlteradosPresenciais.FirstOrDefault().DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().AddDays(10).Date);
            itensAlteradosPresenciais.FirstOrDefault().TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itensAlteradosPresenciais.FirstOrDefault().Excluido.ShouldBeFalse();
            
            var itensAlteradosEmail = itensAlterados.Where(w => w.TipoAtendimento.HasValue && w.TipoAtendimento.Value.EhAtendimentoViaEmail());
            itensAlteradosEmail.FirstOrDefault().DataVisita.ShouldBeNull();
            itensAlteradosEmail.FirstOrDefault().TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            itensAlteradosEmail.FirstOrDefault().Excluido.ShouldBeFalse();
            
            var itensImutavel = itensAlterados.Where(w => w.TipoAtendimento.EhNulo());
            itensImutavel.LastOrDefault().DataVisita.ShouldBeNull();
            itensImutavel.LastOrDefault().TipoAtendimento.ShouldBeNull();
            itensImutavel.FirstOrDefault().Excluido.ShouldBeFalse();
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(2);
            eventos.Count(a=> a.Excluido).ShouldBe(1);
            eventos.Count(a=> !a.Excluido).ShouldBe(1);
            
            var acervosBibliograficos = ObterTodos<AcervoBibliografico>();
            acervosBibliograficos.Count().ShouldBe(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve confirmar o atendimento sem itens")]
        public async Task Nao_deve_confirmar_atendimento_sem_itens()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervoTridimensional();
        
            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve confirmar atendimento sem data no item que é tipo presencial")]
        public async Task Nao_deve_confirmar_atendimento_em_item_sem_data_de_visita_tipo_presencial()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervoTridimensional();
        
            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
        
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                TipoAtendimento = TipoAtendimento.Presencial,
                TipoAcervo = TipoAcervo.Tridimensional
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve confirmar atendimento com data de visita passada nos itens que são do tipo presenciais")]
        public async Task Deve_confirmar_atendimento_em_itens_com_data_de_visita_passada_nos_itens_tipo_presenciais()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervoTridimensional();
        
            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
        
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(-1),
                TipoAtendimento = TipoAtendimento.Presencial,
                TipoAcervo = TipoAcervo.Tridimensional
            });
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                TipoAtendimento = TipoAtendimento.Email,
                TipoAcervo = TipoAcervo.Tridimensional
            });
            
            
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.AGUARDANDO_VISITA);
            solicitacaoAlterada.Excluido.ShouldBeFalse();
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(3);
            
            var itensAlteradosPresenciais = itensAlterados.Where(w => w.TipoAtendimento.HasValue && w.TipoAtendimento.Value.EhAtendimentoPresencial());
            itensAlteradosPresenciais.FirstOrDefault().DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().AddDays(-1).Date);
            itensAlteradosPresenciais.FirstOrDefault().TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            itensAlteradosPresenciais.FirstOrDefault().Excluido.ShouldBeFalse();
            
            var itensAlteradosEmail = itensAlterados.Where(w => w.TipoAtendimento.HasValue && w.TipoAtendimento.Value.EhAtendimentoViaEmail());
            itensAlteradosEmail.FirstOrDefault().DataVisita.ShouldBeNull();
            itensAlteradosEmail.FirstOrDefault().TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            itensAlteradosEmail.FirstOrDefault().Excluido.ShouldBeFalse();
            
            var itensImutavel = itensAlterados.Where(w => w.TipoAtendimento.EhNulo());
            itensImutavel.LastOrDefault().DataVisita.ShouldBeNull();
            itensImutavel.LastOrDefault().TipoAtendimento.ShouldBeNull();
            itensImutavel.FirstOrDefault().Excluido.ShouldBeFalse();
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(1);
            eventos.Count(a=> !a.Excluido).ShouldBe(1);
            eventos.Any(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(-1).Date).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve confirmar atendimento quando não for localizada")]
        public async Task Nao_deve_confirmar_atendimento_quando_nao_for_localizada()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervoTridimensional();
        
            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
        
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 101515,
                ItemId = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                TipoAcervo = TipoAcervo.Tridimensional
            }).ShouldThrowAsync<NegocioException>();
            

            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 101515,
                ItemId = 2,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Email,
                TipoAcervo = TipoAcervo.Tridimensional
            }).ShouldThrowAsync<NegocioException>();
                
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 101515,
                ItemId = 3,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Email,
                TipoAcervo = TipoAcervo.Tridimensional
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve confirmar atendimento quando a data da visita for em dia de feriado")]
        public async Task Nao_deve_confirmar_atendimento_quando_data_visita_for_em_dia_de_feriado()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervoTridimensional();
        
            await InserirAcervoSolicitacao(10);
        
            await InserirNaBase(new Evento()
            {
                Data = DateTimeExtension.HorarioBrasilia().Date,
                Tipo = TipoEvento.FERIADO,
                Descricao = "Feriado",
                CriadoPor = "Sistema", CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema"
            });
            
            await InserirNaBase(new Evento()
            {
                Data = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                Tipo = TipoEvento.VISITA,
                Descricao = "Visita",
                CriadoPor = "Sistema", CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema"
            });
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
        
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                TipoAcervo = TipoAcervo.Tridimensional
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve confirmar atendimento quando a data da visita for em dia de suspensão")]
        public async Task Nao_deve_confirmar_atendimento_quando_data_visita_for_em_dia_de_suspensao()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervoTridimensional();
        
            await InserirAcervoSolicitacao(10);
        
            await InserirNaBase(new Evento()
            {
                Data = DateTimeExtension.HorarioBrasilia().Date,
                Tipo = TipoEvento.SUSPENSAO,
                Descricao = "Suspensão",
                Justificativa = "Justificativa da suspensão",
                CriadoPor = "Sistema", CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema"
            });
            
            await InserirNaBase(new Evento()
            {
                Data = DateTimeExtension.HorarioBrasilia().AddDays(10).Date,
                Tipo = TipoEvento.VISITA,
                Descricao = "Visita",
                CriadoPor = "Sistema", CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema"
            });
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
        
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                TipoAcervo = TipoAcervo.Tridimensional
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve confirmar atendimento sem suspensão e feriado")]
        public async Task Deve_confirmar_atendimento_sem_suspensao_e_feriado()
        {
            await InserirDadosBasicosAleatorios();
        
            await InserirAcervoTridimensional();
        
            await InserirAcervoSolicitacao(10);
            
            await InserirNaBase(new Evento()
            {
                Data = DateTimeExtension.HorarioBrasilia().AddDays(10).Date,
                Tipo = TipoEvento.VISITA,
                Descricao = "Visita",
                CriadoPor = "Sistema", CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema"
            });
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                TipoAcervo = TipoAcervo.Tridimensional
            });
                
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ItemId = 2,
                DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(10).Date,
                TipoAtendimento = TipoAtendimento.Presencial,
                TipoAcervo = TipoAcervo.Tridimensional
            });
            
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.AGUARDANDO_VISITA);
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(3);
            
            var primeiroItemPresencial = itensAlterados.FirstOrDefault(f=> f.Id == 1);
            primeiroItemPresencial.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().AddDays(5).Date);
            primeiroItemPresencial.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            primeiroItemPresencial.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            
            var segundoItemPresencial = itensAlterados.FirstOrDefault(f=> f.Id == 2);
            segundoItemPresencial.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().AddDays(10).Date);
            segundoItemPresencial.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            segundoItemPresencial.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            
            var terceiroItemPresencial = itensAlterados.FirstOrDefault(f=> f.Id == 3);
            terceiroItemPresencial.TipoAtendimento.ShouldBeNull();
            terceiroItemPresencial.DataVisita.ShouldBeNull();
            terceiroItemPresencial.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE);
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(3);
            eventos.Any(f=> f.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(10).Date).ShouldBeTrue();
            eventos.Count(f=> f.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(10).Date).ShouldBe(2);
            eventos.Count(f=> f.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(5).Date).ShouldBe(1);
        }
    }
}