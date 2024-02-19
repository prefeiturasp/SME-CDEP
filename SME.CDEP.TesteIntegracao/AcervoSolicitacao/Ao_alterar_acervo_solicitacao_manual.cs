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
    public class Ao_fazer_acervo_solicitacao_manual : TesteBase
    {
        public Ao_fazer_acervo_solicitacao_manual(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo Solicitação - Ao alterar solicitação de acervo manual com itens presenciais e via e-mail, com item novo")]
        public async Task Ao_alterar_solicitacao_manual_com_itens_presenciais_e_via_email_com_item_novo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional(true);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var acervoSolicitacaoManual = new AcervoSolicitacaoManualDTO()
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
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(4)
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(40)
                    }
                }
            };
            
            var retorno = await servicoAcervoSolicitacao.Inserir(acervoSolicitacaoManual);
            retorno.ShouldBeGreaterThan(0);
            
            //Atualizando
            var alteracaoAcervoSolicitacaoManual = new AcervoSolicitacaoManualDTO()
            {
                Id = 1,
                UsuarioId = 1,
                DataSolicitacao = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                Itens = new List<AcervoSolicitacaoItemManualDTO>()
                {
                    new ()
                    {
                        Id = 1,
                        AcervoId = 1,
                        TipoAtendimento = TipoAtendimento.Email
                    },
                    new ()
                    {
                        Id = 2,
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Email
                    },
                    new ()
                    {
                        Id = 3,
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(40)
                    },
                    new ()
                    {
                        AcervoId = 4,
                        TipoAtendimento = TipoAtendimento.Email
                    }
                }
            };
            
            retorno = await servicoAcervoSolicitacao.Alterar(alteracaoAcervoSolicitacaoManual);
            retorno.ShouldBeGreaterThan(0);

            var solicitacaoCadastrada = ObterTodos<AcervoSolicitacao>().LastOrDefault();
            solicitacaoCadastrada.UsuarioId.ShouldBe(acervoSolicitacaoManual.UsuarioId);
            solicitacaoCadastrada.ResponsavelId.ShouldNotBeNull();
            solicitacaoCadastrada.DataSolicitacao.ShouldBe(acervoSolicitacaoManual.DataSolicitacao);
            solicitacaoCadastrada.Origem.ShouldBe(Origem.Manual);
            solicitacaoCadastrada.Situacao.ShouldBe(SituacaoSolicitacao.AGUARDANDO_VISITA);
            
            var itensCadastrados = ObterTodos<AcervoSolicitacaoItem>();

            var primeiroItemEmail = itensCadastrados.FirstOrDefault(f => f.AcervoId == 1);
            primeiroItemEmail.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            primeiroItemEmail.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            primeiroItemEmail.DataVisita.ShouldBeNull();
            
            var segundoItemEmail = itensCadastrados.FirstOrDefault(f => f.AcervoId == 2);
            segundoItemEmail.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            segundoItemEmail.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            
            var terceiroItemPresencial = itensCadastrados.FirstOrDefault(f => f.AcervoId == 3);
            terceiroItemPresencial.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            terceiroItemPresencial.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            terceiroItemPresencial.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().AddDays(40).Date);
            
            var quartoItemEmail = itensCadastrados.FirstOrDefault(f => f.AcervoId == 4);
            quartoItemEmail.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            quartoItemEmail.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Ao alterar solicitação de acervo manual com itens presenciais e via e-mail, com todos os itens novos")]
        public async Task Ao_alterar_solicitacao_manual_com_itens_presenciais_e_via_email_com_todos_os_itens_novos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional(true);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var acervoSolicitacaoManual = new AcervoSolicitacaoManualDTO()
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
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(4)
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(40)
                    }
                }
            };
            
            var retorno = await servicoAcervoSolicitacao.Inserir(acervoSolicitacaoManual);
            retorno.ShouldBeGreaterThan(0);
            
            //Atualizando
            var alteracaoAcervoSolicitacaoManual = new AcervoSolicitacaoManualDTO()
            {
                Id = 1,
                UsuarioId = 1,
                DataSolicitacao = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                Itens = new List<AcervoSolicitacaoItemManualDTO>()
                {
                    new ()
                    {
                        AcervoId = 4,
                        TipoAtendimento = TipoAtendimento.Email
                    },
                    new ()
                    {
                        AcervoId = 5,
                        TipoAtendimento = TipoAtendimento.Email
                    },
                    new ()
                    {
                        AcervoId = 6,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(40)
                    }
                }
            };
            
            retorno = await servicoAcervoSolicitacao.Alterar(alteracaoAcervoSolicitacaoManual);
            retorno.ShouldBeGreaterThan(0);

            var solicitacaoCadastrada = ObterTodos<AcervoSolicitacao>().LastOrDefault();
            solicitacaoCadastrada.UsuarioId.ShouldBe(acervoSolicitacaoManual.UsuarioId);
            solicitacaoCadastrada.ResponsavelId.ShouldNotBeNull();
            solicitacaoCadastrada.DataSolicitacao.ShouldBe(acervoSolicitacaoManual.DataSolicitacao);
            solicitacaoCadastrada.Origem.ShouldBe(Origem.Manual);
            solicitacaoCadastrada.Situacao.ShouldBe(SituacaoSolicitacao.AGUARDANDO_VISITA);
            
            var itensCadastrados = ObterTodos<AcervoSolicitacaoItem>();

            var primeiroItemEmail = itensCadastrados.FirstOrDefault(f => f.AcervoId == 4);
            primeiroItemEmail.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            primeiroItemEmail.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            primeiroItemEmail.DataVisita.ShouldBeNull();
            
            var segundoItemEmail = itensCadastrados.FirstOrDefault(f => f.AcervoId == 5);
            segundoItemEmail.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            segundoItemEmail.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            
            var terceiroItemPresencial = itensCadastrados.FirstOrDefault(f => f.AcervoId == 6);
            terceiroItemPresencial.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            terceiroItemPresencial.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            terceiroItemPresencial.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().AddDays(40).Date);
            
        }
    }
}