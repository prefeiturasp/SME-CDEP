using Shouldly;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_cancelar_acervo_solicitacao : TesteBase
    {
        private readonly IServicoAcervoBibliografico servicoAcervoBibliografico; 
        private readonly IServicoAcervoSolicitacao servicoAcervoSolicitacao;

        public Ao_cancelar_acervo_solicitacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            servicoAcervoBibliografico = GetServicoAcervoBibliografico();
            servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
        }
       
        [Fact(DisplayName = "Acervo Solicitação - Deve cancelar atendimento quando itens tridimensionais aguardando visita")]
        public async Task Deve_cancelar_atendimento_quando_itens_tridimensionais_aguardando_visita()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
           await InserirNaBase(acervoSolicitacao);

           var acervoSolicitadoId = (ObterTodos<AcervoSolicitacao>()).LastOrDefault().Id;

           var itemCount = 1;
           
           foreach (var item in acervoSolicitacao.Itens)
           {
                item.AcervoSolicitacaoId = acervoSolicitadoId;
                item.Situacao = SituacaoSolicitacaoItem.AGUARDANDO_VISITA;
                item.TipoAtendimento = TipoAtendimento.Presencial;
                item.DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(15);
                await InserirNaBase(item);
                
                await InserirNaBase(new Evento()
                {
                    Data = DateTimeExtension.HorarioBrasilia().AddDays(15),
                    Tipo = TipoEvento.VISITA,
                    AcervoSolicitacaoItemId = itemCount,
                    Descricao = "Visita",
                    CriadoPor = "Sistema", CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema"
                });
                
                await InserirNaBase(new AcervoEmprestimo()
                {
                    AcervoSolicitacaoItemId = itemCount,
                    DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(2).Date,
                    DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(7).Date,
                    Situacao = SituacaoEmprestimo.EMPRESTADO,
                    CriadoPor = "Sistema", CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema"
                });

                itemCount++;
           }
           
            var retorno = await servicoAcervoSolicitacao.CancelarAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.CANCELADO);

            var itens = ObterTodos<AcervoSolicitacaoItem>();
            itens.All(a=> a.Situacao.EstaCancelado()).ShouldBeTrue();
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(3);
            eventos.Count(a=> !a.Excluido).ShouldBe(0);
            eventos.Count(a=> a.Excluido).ShouldBe(3);
            eventos.Any(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(15).Date).ShouldBeTrue();
            
            var acervosEmprestimos = ObterTodos<AcervoEmprestimo>();
            acervosEmprestimos.Count().ShouldBe(6);
            acervosEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().AddDays(2).Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();
            acervosEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(7).Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();
            acervosEmprestimos.Count(a=> a.Situacao.EstaEmprestado()).ShouldBe(3);
            acervosEmprestimos.Count(a=> a.Situacao.EstaCancelado()).ShouldBe(3);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve cancelar atendimento quando itens bibliograficos aguardando visita")]
        public async Task Deve_cancelar_atendimento_quando_itens_bibliograficos_aguardando_visita()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos(); 

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
           await InserirNaBase(acervoSolicitacao);

           var acervoSolicitadoId = (ObterTodos<AcervoSolicitacao>()).LastOrDefault().Id;

           var itemCount = 1;
           
           foreach (var item in acervoSolicitacao.Itens)
           {
                item.AcervoSolicitacaoId = acervoSolicitadoId;
                item.Situacao = SituacaoSolicitacaoItem.AGUARDANDO_VISITA;
                item.TipoAtendimento = TipoAtendimento.Presencial;
                item.DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(15);
                await InserirNaBase(item);
                
                await InserirNaBase(new Evento()
                {
                    Data = DateTimeExtension.HorarioBrasilia().AddDays(15),
                    Tipo = TipoEvento.VISITA,
                    AcervoSolicitacaoItemId = itemCount,
                    Descricao = "Visita",
                    CriadoPor = "Sistema", CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema"
                });
                
                await InserirNaBase(new AcervoEmprestimo()
                {
                    AcervoSolicitacaoItemId = itemCount,
                    DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(2).Date,
                    DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(7).Date,
                    Situacao = SituacaoEmprestimo.EMPRESTADO,
                    CriadoPor = "Sistema", CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema"
                });

                itemCount++;
           }
           
            await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.EMPRESTADO, 1);
            await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.EMPRESTADO, 2);
            await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.EMPRESTADO, 3);
            
            var retorno = await servicoAcervoSolicitacao.CancelarAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.CANCELADO);

            var itens = ObterTodos<AcervoSolicitacaoItem>();
            itens.All(a=> a.Situacao.EstaCancelado()).ShouldBeTrue();
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(3);
            eventos.Count(a=> !a.Excluido).ShouldBe(0);
            eventos.Count(a=> a.Excluido).ShouldBe(3);
            eventos.Any(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(15).Date).ShouldBeTrue();
            
            var acervosEmprestimos = ObterTodos<AcervoEmprestimo>();
            acervosEmprestimos.Count().ShouldBe(6);
            acervosEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().AddDays(2).Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();
            acervosEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(7).Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();
            acervosEmprestimos.Count(a=> a.Situacao.EstaEmprestado()).ShouldBe(3);
            acervosEmprestimos.Count(a=> a.Situacao.EstaCancelado()).ShouldBe(3);
            
            var acervosBibliograficos = ObterTodos<AcervoBibliografico>();
            acervosBibliograficos.FirstOrDefault(f=> f.Id == 1).SituacaoSaldo.ShouldBe(SituacaoSaldo.DISPONIVEL);
            acervosBibliograficos.FirstOrDefault(f=> f.Id == 2).SituacaoSaldo.ShouldBe(SituacaoSaldo.DISPONIVEL);
            acervosBibliograficos.FirstOrDefault(f=> f.Id == 3).SituacaoSaldo.ShouldBe(SituacaoSaldo.DISPONIVEL);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve cancelar atendimento quando itens tridimensionais cancelados")]
        public async Task Deve_cancelar_atendimento_quando_itens_tridimensionais_cancelados()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
            await InserirNaBase(acervoSolicitacao);

            var acervoSolicitadoId = (ObterTodos<AcervoSolicitacao>()).LastOrDefault().Id;
           
            foreach (var item in acervoSolicitacao.Itens)
            {
                item.AcervoSolicitacaoId = acervoSolicitadoId;
                item.Situacao = SituacaoSolicitacaoItem.CANCELADO;
                await InserirNaBase(item);
            }
            
            var retorno = await servicoAcervoSolicitacao.CancelarAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.CANCELADO);
            
            var itens = ObterTodos<AcervoSolicitacaoItem>();
            itens.All(a=> a.Situacao.EstaCancelado()).ShouldBeTrue();
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve cancelar atendimento quando itens tridimensionais aguardando atendimento")]
        public async Task Deve_cancelar_atendimento_quando_itens_tridimensionais_aguardando_atendimento()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
            await InserirNaBase(acervoSolicitacao);

            var acervoSolicitadoId = (ObterTodos<AcervoSolicitacao>()).LastOrDefault().Id;
           
            foreach (var item in acervoSolicitacao.Itens)
            {
                item.AcervoSolicitacaoId = acervoSolicitadoId;
                item.Situacao = SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO;
                item.DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(5);
                await InserirNaBase(item);
            }
            
            var retorno = await servicoAcervoSolicitacao.CancelarAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.CANCELADO);
            
            var itens = ObterTodos<AcervoSolicitacaoItem>();
            itens.All(a=> a.Situacao.EstaCancelado()).ShouldBeTrue();
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve cancelar atendimento quando itens finalizados manualmente")]
        public async Task Nao_deve_cancelar_atendimento_quando_itens_finalizado_manualmente()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
            await InserirNaBase(acervoSolicitacao);

            var acervoSolicitadoId = (ObterTodos<AcervoSolicitacao>()).LastOrDefault().Id;
           
            foreach (var item in acervoSolicitacao.Itens)
            {
                item.AcervoSolicitacaoId = acervoSolicitadoId;
                item.Situacao = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE;
                await InserirNaBase(item);
            }
            
            await servicoAcervoSolicitacao.CancelarAtendimento(1).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve cancelar atendimento com itens em situação finalizado automaticamente")]
        public async Task Nao_deve_cancelar_atendimento_com_itens_em_situacao_finalizado_automaticamente()
        {
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
            
            await servicoAcervoSolicitacao.CancelarAtendimento(1).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve cancelar atendimento com itens atendidos parcialmente com visita")]
        public async Task Deve_cancelar_atendimento_com_itens_em_atendidos_parcialmente_com_visita()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            acervoSolicitacao.Situacao = SituacaoSolicitacao.ATENDIDO_PARCIALMENTE;
            await InserirNaBase(acervoSolicitacao);

            var acervoSolicitadoId = (ObterTodos<AcervoSolicitacao>()).LastOrDefault().Id;

            var contador = 1;
            foreach (var item in acervoSolicitacao.Itens)
            {
                item.AcervoSolicitacaoId = acervoSolicitadoId;
                item.Situacao = contador >= 2 ? SituacaoSolicitacaoItem.AGUARDANDO_VISITA : SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO;
                item.TipoAtendimento = contador >= 2 ? TipoAtendimento.Presencial : null; 
                item.DataVisita = contador >= 2 ? DateTimeExtension.HorarioBrasilia() : null; 
                await InserirNaBase(item);

                if (contador >= 2)
                {
                    await InserirNaBase(new AcervoEmprestimo()
                    {
                        AcervoSolicitacaoItemId = contador,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(2).Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(7).Date,
                        Situacao = SituacaoEmprestimo.EMPRESTADO,
                        CriadoPor = "Sistema", CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema"
                    });
                    
                    await InserirNaBase(new Evento()
                    {
                        AcervoSolicitacaoItemId = contador,
                        Data = DateTimeExtension.HorarioBrasilia(),
                        Tipo = TipoEvento.VISITA,
                        Descricao = TipoEvento.VISITA.Descricao(),
                        CriadoPor = "Sistema", CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema"
                    });
                }
                
                contador++;
            }
            
            var retorno = await servicoAcervoSolicitacao.CancelarAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.CANCELADO);
            
            var itens = ObterTodos<AcervoSolicitacaoItem>();
            itens.All(a=> a.Situacao.EstaCancelado()).ShouldBeTrue();
            
            var eventos = ObterTodos<Evento>();
            eventos.Count(s=> !s.Excluido).ShouldBe(0);
            eventos.Count(s=> s.Excluido).ShouldBe(2);
            
            var acervosEmprestimos = ObterTodos<AcervoEmprestimo>();
            acervosEmprestimos.Count().ShouldBe(4);
            acervosEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().AddDays(2).Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();
            acervosEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(7).Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();
            acervosEmprestimos.Count(a=> a.Situacao.EstaCancelado()).ShouldBe(2);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve cancelar atendimento com itens atendidos parcialmente aguardando atendimento")]
        public async Task Nao_deve_cancelar_atendimento_com_itens_em_atendidos_parcialmente_finalizado_manualmente_aguardando_atendimento()
        {
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
                item.Situacao = contador >= 2 ? SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE : SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO;
                await InserirNaBase(item);
                contador++;
            }
            
            await servicoAcervoSolicitacao.CancelarAtendimento(1).ShouldThrowAsync<NegocioException>();
        }
    }
}