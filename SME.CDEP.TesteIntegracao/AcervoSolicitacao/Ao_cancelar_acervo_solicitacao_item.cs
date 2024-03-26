using Shouldly;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_cancelar_acervo_solicitacao_item : TesteBase
    {
        private readonly IServicoAcervoBibliografico servicoAcervoBibliografico; 
        private readonly IServicoAcervoSolicitacao servicoAcervoSolicitacao;
           
        public Ao_cancelar_acervo_solicitacao_item(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            servicoAcervoBibliografico = GetServicoAcervoBibliografico();
            servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
        }
       
        [Fact(DisplayName = "Acervo Solicitação Item - Deve cancelar um item tridimensional em situação presencial e com empréstimo")]
        public async Task Deve_cancelar_um_item_tridimensional_em_situacao_presencial_e_com_emprestimo()
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
                item.DataVisita = DateTimeExtension.HorarioBrasilia().Date.AddDays(2);
                await InserirNaBase(item);
                
                await InserirNaBase(new Evento()
                {
                    Data = DateTimeExtension.HorarioBrasilia().AddDays(2).Date,
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
            
            var retorno = await servicoAcervoSolicitacao.CancelarItemAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoItemAlterada = ObterTodos<AcervoSolicitacaoItem>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoItemAlterada.Id.ShouldBe(1);
            solicitacaoItemAlterada.Situacao.ShouldBe(SituacaoSolicitacaoItem.CANCELADO);
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(3);
            eventos.Count(a=> !a.Excluido).ShouldBe(2);
            eventos.Count(a=> a.Excluido).ShouldBe(1);
            
            var acervosEmprestimos = ObterTodos<AcervoEmprestimo>();
            acervosEmprestimos.Count().ShouldBe(4);
            acervosEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().AddDays(2).Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();
            acervosEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(7).Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();
            acervosEmprestimos.Count(a=> a.Situacao.EstaEmprestado()).ShouldBe(3);
            acervosEmprestimos.Count(a=> a.Situacao.EstaCancelado()).ShouldBe(1);
        }
        
         [Fact(DisplayName = "Acervo Solicitação Item - Deve cancelar um item bibliográfico presencial com empréstimo")]
        public async Task Deve_cancelar_um_item_bibliografico_em_situacao_presencial_com_emprestimo()
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
                item.DataVisita = DateTimeExtension.HorarioBrasilia().Date.AddDays(2);
                await InserirNaBase(item);
                
                await InserirNaBase(new Evento()
                {
                    Data = DateTimeExtension.HorarioBrasilia().AddDays(2).Date,
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
           
           var retorno = await servicoAcervoSolicitacao.CancelarItemAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoItemAlterada = ObterTodos<AcervoSolicitacaoItem>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoItemAlterada.Id.ShouldBe(1);
            solicitacaoItemAlterada.Situacao.ShouldBe(SituacaoSolicitacaoItem.CANCELADO);
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(3);
            eventos.Count(a=> !a.Excluido).ShouldBe(2);
            eventos.Count(a=> a.Excluido).ShouldBe(1);
            
            var acervosEmprestimos = ObterTodos<AcervoEmprestimo>();
            acervosEmprestimos.Count().ShouldBe(4);
            acervosEmprestimos.Any(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().AddDays(2).Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();
            acervosEmprestimos.Any(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(7).Date && a.Situacao.EstaEmprestado()).ShouldBeTrue();
            acervosEmprestimos.Count(a=> a.Situacao.EstaEmprestado()).ShouldBe(3);
            acervosEmprestimos.Count(a=> a.Situacao.EstaCancelado()).ShouldBe(1);

            var acervos = ObterTodos<AcervoBibliografico>();
            acervos.FirstOrDefault(a=> a.Id == 1).SituacaoSaldo.ShouldBe(SituacaoSaldo.DISPONIVEL);
        }
        
        [Fact(DisplayName = "Acervo Solicitação Item - Deve cancelar um item tridimensional em situação finalizado manualmente")]
        public async Task Deve_cancelar_um_item_tridimensional_em_situacao_finalizado_manualmente()
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
                item.DataVisita = null;
                await InserirNaBase(item);
            }
            
            var retorno = await servicoAcervoSolicitacao.CancelarItemAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoItemAlterada = ObterTodos<AcervoSolicitacaoItem>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoItemAlterada.Id.ShouldBe(1);
            solicitacaoItemAlterada.Situacao.ShouldBe(SituacaoSolicitacaoItem.CANCELADO);
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação Item - Deve cancelar um item bibliografico em situação finalizado manualmente")]
        public async Task Deve_cancelar_um_item_bibliografico_em_situacao_finalizado_manualmente()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
            await InserirNaBase(acervoSolicitacao);

            var acervoSolicitadoId = (ObterTodos<AcervoSolicitacao>()).LastOrDefault().Id;
           
            foreach (var item in acervoSolicitacao.Itens)
            {
                item.AcervoSolicitacaoId = acervoSolicitadoId;
                item.Situacao = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE;
                item.DataVisita = null;
                await InserirNaBase(item);
            }

            var retorno = await servicoAcervoSolicitacao.CancelarItemAtendimento(1);
            retorno.ShouldBeTrue();
            
            var solicitacaoItemAlterada = ObterTodos<AcervoSolicitacaoItem>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoItemAlterada.Id.ShouldBe(1);
            solicitacaoItemAlterada.Situacao.ShouldBe(SituacaoSolicitacaoItem.CANCELADO);
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação Item - Deve cancelar um item tridimensional em situação aguardando atendimento")]
        public async Task Deve_cancelar_um_item_tridimensional_em_situacao_aguardando_atendimento()
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
                await InserirNaBase(item);
            }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var retorno = await servicoAcervoSolicitacao.CancelarItemAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoItemAlterada = ObterTodos<AcervoSolicitacaoItem>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoItemAlterada.Id.ShouldBe(1);
            solicitacaoItemAlterada.Situacao.ShouldBe(SituacaoSolicitacaoItem.CANCELADO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação Item - Deve cancelar um item bibliografico em situação aguardando atendimento")]
        public async Task Deve_cancelar_um_item_bibliografico_em_situacao_aguardando_atendimento()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
            await InserirNaBase(acervoSolicitacao);

            var acervoSolicitadoId = (ObterTodos<AcervoSolicitacao>()).LastOrDefault().Id;
           
            foreach (var item in acervoSolicitacao.Itens)
            {
                item.AcervoSolicitacaoId = acervoSolicitadoId;
                item.Situacao = SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO;
                await InserirNaBase(item);
            }
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var retorno = await servicoAcervoSolicitacao.CancelarItemAtendimento(1);
            
            retorno.ShouldBeTrue();
            var solicitacaoItemAlterada = ObterTodos<AcervoSolicitacaoItem>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoItemAlterada.Id.ShouldBe(1);
            solicitacaoItemAlterada.Situacao.ShouldBe(SituacaoSolicitacaoItem.CANCELADO);
        }
        
        [Fact(DisplayName = "Acervo Solicitação Item - Não deve cancelar um item em situação finalizado automaticamente")]
        public async Task Nao_deve_cancelar_um_item_em_situacao_finalizado_automaticamente()
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
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
           
            await servicoAcervoSolicitacao.CancelarItemAtendimento(1).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação Item - Não deve cancelar um item com situação já cancelado")]
        public async Task Nao_deve_cancelar_um_item_em_situacao_ja_cancelado()
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
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
           
            await servicoAcervoSolicitacao.CancelarItemAtendimento(1).ShouldThrowAsync<NegocioException>();
        }
    }
}