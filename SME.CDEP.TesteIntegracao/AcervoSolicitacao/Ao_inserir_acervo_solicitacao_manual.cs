using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_inserir_acervo_solicitacao_manual : TesteBase
    {
        public Ao_inserir_acervo_solicitacao_manual(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo Solicitação - Ao fazer solicitação de acervo manual com itens presenciais e via e-mail")]
        public async Task Ao_fazer_solicitacao_manual_com_itens_presenciais_e_via_email()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional(false);
            
            await InserirNaBase(new Evento()
            {
                Data = DateTimeExtension.HorarioBrasilia().AddDays(40).Date,
                Tipo = TipoEvento.VISITA,
                Descricao = "Visita",
                CriadoPor = "Sistema", CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema"
            });

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

            var solicitacaoCadastrada = ObterTodos<AcervoSolicitacao>().LastOrDefault();
            solicitacaoCadastrada.UsuarioId.ShouldBe(acervoSolicitacaoManual.UsuarioId);
            solicitacaoCadastrada.DataSolicitacao.ShouldBe(acervoSolicitacaoManual.DataSolicitacao);
            solicitacaoCadastrada.Origem.ShouldBe(Origem.Manual);
            solicitacaoCadastrada.Situacao.ShouldBe(SituacaoSolicitacao.AGUARDANDO_VISITA);
            solicitacaoCadastrada.Excluido.ShouldBeFalse();
            
            var itensCadastrados = ObterTodos<AcervoSolicitacaoItem>();

            var itemEmail = itensCadastrados.FirstOrDefault(f => f.AcervoId == 1);
            itemEmail.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            itemEmail.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            itemEmail.DataVisita.ShouldBeNull();
            itemEmail.Excluido.ShouldBeFalse();
            itemEmail.ResponsavelId.ShouldNotBeNull();
            
            var primeiroPresencial = itensCadastrados.FirstOrDefault(f => f.AcervoId == 2);
            primeiroPresencial.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            primeiroPresencial.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            primeiroPresencial.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().AddDays(4).Date);
            primeiroPresencial.Excluido.ShouldBeFalse();
            primeiroPresencial.ResponsavelId.ShouldNotBeNull();
            
            var segundoPresencial = itensCadastrados.FirstOrDefault(f => f.AcervoId == 3);
            segundoPresencial.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            segundoPresencial.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            segundoPresencial.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().AddDays(40).Date);
            segundoPresencial.Excluido.ShouldBeFalse();
            segundoPresencial.ResponsavelId.ShouldNotBeNull();

            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(3);
            eventos.Any(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(40).Date).ShouldBeTrue();
            
            eventos.Count(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(4).Date).ShouldBe(1);
            eventos.Any(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(4).Date && a.Tipo.EhVisita()).ShouldBeTrue();
            
            eventos.Count(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(40).Date).ShouldBe(2);
            eventos.Any(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(40).Date && a.Tipo.EhVisita()).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Ao fazer solicitação de acervo manual com todos os itens via e-mail")]
        public async Task Ao_fazer_solicitacao_manual_com_todos_os_itens_via_email()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional(false);
            
            await InserirNaBase(new Evento()
            {
                Data = DateTimeExtension.HorarioBrasilia().AddDays(40).Date,
                Tipo = TipoEvento.VISITA,
                Descricao = "Visita",
                CriadoPor = "Sistema", CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema"
            });

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
                        TipoAtendimento = TipoAtendimento.Email
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Email
                    }
                }
            };
            
            var retorno = await servicoAcervoSolicitacao.Inserir(acervoSolicitacaoManual);
            retorno.ShouldBeGreaterThan(0);

            var solicitacaoCadastrada = ObterTodos<AcervoSolicitacao>().LastOrDefault();
            solicitacaoCadastrada.UsuarioId.ShouldBe(acervoSolicitacaoManual.UsuarioId);
            solicitacaoCadastrada.DataSolicitacao.ShouldBe(acervoSolicitacaoManual.DataSolicitacao);
            solicitacaoCadastrada.Origem.ShouldBe(Origem.Manual);
            solicitacaoCadastrada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
            solicitacaoCadastrada.Excluido.ShouldBeFalse();
            
            var itensCadastrados = ObterTodos<AcervoSolicitacaoItem>();

            var primeiroAcervoViaEmail = itensCadastrados.FirstOrDefault(f => f.AcervoId == 1);
            primeiroAcervoViaEmail.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            primeiroAcervoViaEmail.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            primeiroAcervoViaEmail.DataVisita.ShouldBeNull();
            primeiroAcervoViaEmail.Excluido.ShouldBeFalse();
            primeiroAcervoViaEmail.ResponsavelId.ShouldNotBeNull();
            
            var segundoAcevoViaEmail = itensCadastrados.FirstOrDefault(f => f.AcervoId == 1);
            segundoAcevoViaEmail.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            segundoAcevoViaEmail.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            segundoAcevoViaEmail.DataVisita.ShouldBeNull();
            segundoAcevoViaEmail.Excluido.ShouldBeFalse();
            segundoAcevoViaEmail.ResponsavelId.ShouldNotBeNull();
            
            var terceiroAcervoViaEmail = itensCadastrados.FirstOrDefault(f => f.AcervoId == 3);
            terceiroAcervoViaEmail.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            terceiroAcervoViaEmail.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            terceiroAcervoViaEmail.DataVisita.ShouldBeNull();
            terceiroAcervoViaEmail.Excluido.ShouldBeFalse();
            terceiroAcervoViaEmail.ResponsavelId.ShouldNotBeNull();
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(1);
            eventos.Any(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(40).Date).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Ao fazer solicitação de acervo manual com todos os itens de acervo com tipo de atendimento presenciais")]
        public async Task Ao_fazer_solicitacao_manual_com_todos_os_itens_de_acervo_com_tipo_de_atendimento_presenciais()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional(false);
            
            await InserirNaBase(new Evento()
            {
                Data = DateTimeExtension.HorarioBrasilia().AddDays(40).Date,
                Tipo = TipoEvento.VISITA,
                Descricao = "Visita",
                CriadoPor = "Sistema", CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema"
            });

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
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia()
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

            var solicitacaoCadastrada = ObterTodos<AcervoSolicitacao>().LastOrDefault();
            solicitacaoCadastrada.UsuarioId.ShouldBe(acervoSolicitacaoManual.UsuarioId);
            solicitacaoCadastrada.DataSolicitacao.ShouldBe(acervoSolicitacaoManual.DataSolicitacao);
            solicitacaoCadastrada.Origem.ShouldBe(Origem.Manual);
            solicitacaoCadastrada.Situacao.ShouldBe(SituacaoSolicitacao.AGUARDANDO_VISITA);
            solicitacaoCadastrada.Excluido.ShouldBeFalse();
            
            var itensCadastrados = ObterTodos<AcervoSolicitacaoItem>();

            var primeiroAcervoPresencial = itensCadastrados.FirstOrDefault(f => f.AcervoId == 1);
            primeiroAcervoPresencial.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            primeiroAcervoPresencial.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            primeiroAcervoPresencial.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            primeiroAcervoPresencial.Excluido.ShouldBeFalse();
            primeiroAcervoPresencial.ResponsavelId.ShouldNotBeNull();
            
            var segundoAcervoPresencial = itensCadastrados.FirstOrDefault(f => f.AcervoId == 2);
            segundoAcervoPresencial.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            segundoAcervoPresencial.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            segundoAcervoPresencial.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().AddDays(4).Date);
            segundoAcervoPresencial.Excluido.ShouldBeFalse();
            segundoAcervoPresencial.ResponsavelId.ShouldNotBeNull();
            
            var terceiroAcervoPresencial = itensCadastrados.FirstOrDefault(f => f.AcervoId == 3);
            terceiroAcervoPresencial.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            terceiroAcervoPresencial.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            terceiroAcervoPresencial.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().AddDays(40).Date);
            terceiroAcervoPresencial.Excluido.ShouldBeFalse();
            terceiroAcervoPresencial.ResponsavelId.ShouldNotBeNull();
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(4);
            eventos.Count(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(40).Date && a.Tipo.EhVisita()).ShouldBe(2);
            
            eventos.Count(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBe(1);
            eventos.Any(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().Date && a.Tipo.EhVisita()).ShouldBeTrue();
            
            eventos.Count(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(4).Date).ShouldBe(1);
            eventos.Any(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(4).Date && a.Tipo.EhVisita()).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve fazer solicitação de acervo manual com data de visita passada em acervo presencial")]
        public async Task Nao_deve_fazer_solicitacao_manual_com_data_de_visita_passada_em_acervo_presencial()
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
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(-8)
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
            
            await servicoAcervoSolicitacao.Inserir(acervoSolicitacaoManual).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve fazer solicitação de acervo manual com data de visita em tipo de atendimento e-mail")]
        public async Task Nao_deve_fazer_solicitacao_manual_com_data_de_visita_em_tipo_de_atendimento_via_email()
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
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Email,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(4)
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
            
            await servicoAcervoSolicitacao.Inserir(acervoSolicitacaoManual).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve fazer solicitação de acervo manual sem data de visita em tipo de atendimento presencial")]
        public async Task Nao_deve_fazer_solicitacao_manual_sem_de_data_de_visita_em_tipo_de_atendimento_presencial()
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
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(4)
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(40)
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial
                    }
                }
            };
            
            await servicoAcervoSolicitacao.Inserir(acervoSolicitacaoManual).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve fazer solicitação de acervo manual com data de visita em dia de feriado")]
        public async Task Nao_deve_fazer_solicitacao_manual_com_data_de_visita_em_dia_de_feriado()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional(false);
            
            await InserirNaBase(new Evento()
            {
                Data = DateTimeExtension.HorarioBrasilia().AddDays(40).Date,
                Tipo = TipoEvento.FERIADO,
                Descricao = "Feriado",
                CriadoPor = "Sistema", CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema"
            });

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var acervoSolicitacaoManual = new AcervoSolicitacaoManualDTO()
            {
                UsuarioId = 1,
                DataSolicitacao = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                Itens = new List<AcervoSolicitacaoItemManualDTO>()
                {
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
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(60)
                    }
                }
            };
            
            await servicoAcervoSolicitacao.Inserir(acervoSolicitacaoManual).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve fazer solicitação de acervo manual com data de visita em dia de suspensão")]
        public async Task Nao_deve_fazer_solicitacao_manual_com_data_de_visita_em_dia_de_suspensao()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional(false);
            
            await InserirNaBase(new Evento()
            {
                Data = DateTimeExtension.HorarioBrasilia().AddDays(40).Date,
                Tipo = TipoEvento.SUSPENSAO,
                Descricao = "Suspensão",
                Justificativa = "Justificativa da Suspensão",
                CriadoPor = "Sistema", CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema"
            });

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var acervoSolicitacaoManual = new AcervoSolicitacaoManualDTO()
            {
                UsuarioId = 1,
                DataSolicitacao = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5),
                Itens = new List<AcervoSolicitacaoItemManualDTO>()
                {
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
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(60)
                    }
                }
            };
            
            await servicoAcervoSolicitacao.Inserir(acervoSolicitacaoManual).ShouldThrowAsync<NegocioException>();
        }
    }
}