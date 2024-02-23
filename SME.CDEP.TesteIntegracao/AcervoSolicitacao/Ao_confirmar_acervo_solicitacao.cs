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
       
       
        [Fact(DisplayName = "Acervo Solicitação - Confirmar")]
        public async Task Deve_confirmar_atendimento()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var retorno = await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ResponsavelRf = "login_1",
                Itens = new List<AcervoSolicitacaoItemConfirmarDTO>()
                {
                    new()
                    {
                        Id = 1,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial
                    },
                    new()
                    {
                        Id = 2,
                        TipoAtendimento = TipoAtendimento.Email
                    }
                }
            });
            
            retorno.ShouldBeTrue();
            var solicitacaoAlterada = ObterTodos<AcervoSolicitacao>().FirstOrDefault(f=> f.Id == 1);
            solicitacaoAlterada.Id.ShouldBe(1);
            solicitacaoAlterada.UsuarioId.ShouldBe(1);
            solicitacaoAlterada.Situacao.ShouldBe(SituacaoSolicitacao.AGUARDANDO_VISITA);
            solicitacaoAlterada.Excluido.ShouldBeFalse();
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(3);
            
            var itensAlteradosPresenciais = itensAlterados.Where(w => w.TipoAtendimento.HasValue && w.TipoAtendimento.Value.EhAtendimentoPresencial());
            itensAlteradosPresenciais.FirstOrDefault().DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
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
            eventos.Any(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Confirmar editando tipo de atendimento e data de visita")]
        public async Task Deve_confirmar_atendimento_editando_confirmacao()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ResponsavelRf = "login_1",
                Itens = new List<AcervoSolicitacaoItemConfirmarDTO>()
                {
                    new()
                    {
                        Id = 1,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial
                    },
                    new()
                    {
                        Id = 2,
                        TipoAtendimento = TipoAtendimento.Email
                    }
                }
            });
            
            var retorno = await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ResponsavelRf = "login_1",
                Itens = new List<AcervoSolicitacaoItemConfirmarDTO>()
                {
                    new()
                    {
                        Id = 1,
                        TipoAtendimento = TipoAtendimento.Email
                    },
                    new()
                    {
                        Id = 2,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(10).Date,
                        TipoAtendimento = TipoAtendimento.Presencial
                    }
                }
            });
            
            retorno.ShouldBeTrue();
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
            eventos.Any(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(10).Date).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve confirmar o atendimento sem itens")]
        public async Task Nao_deve_confirmar_atendimento_sem_itens()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var acervoArteGraficaCadastroDto = new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1
            };
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(acervoArteGraficaCadastroDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve confirmar atendimento sem data nos itens que são tipo presenciais")]
        public async Task Nao_deve_confirmar_atendimento_em_itens_sem_data_de_visita_tipo_presencial()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var acervoArteGraficaCadastroDto = new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                Itens = new List<AcervoSolicitacaoItemConfirmarDTO>()
                {
                    new()
                    {
                        Id = 1,
                        TipoAtendimento = TipoAtendimento.Presencial
                    },
                    new()
                    {
                        Id = 2,
                        TipoAtendimento = TipoAtendimento.Email
                    },
                    new()
                    {
                        Id = 3,
                        TipoAtendimento = TipoAtendimento.Email
                    }
                }
            };
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(acervoArteGraficaCadastroDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve confirmar atendimento com data de visita passada nos itens que são do tipo presenciais")]
        public async Task Nao_deve_confirmar_atendimento_em_itens_com_data_de_visita_passada_nos_itens_tipo_presenciais()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var acervoArteGraficaCadastroDto = new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                Itens = new List<AcervoSolicitacaoItemConfirmarDTO>()
                {
                    new()
                    {
                        Id = 1,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(-1),
                        TipoAtendimento = TipoAtendimento.Presencial
                    },
                    new()
                    {
                        Id = 2,
                        TipoAtendimento = TipoAtendimento.Email
                    },
                    new()
                    {
                        Id = 3,
                        TipoAtendimento = TipoAtendimento.Email
                    }
                }
            };
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(acervoArteGraficaCadastroDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve confirmar atendimento com data de visita válida nos itens que são do tipo e-mail")]
        public async Task Nao_deve_confirmar_atendimento_em_itens_com_data_de_visita_valida_nos_itens_tipo_email()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var acervoArteGraficaCadastroDto = new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                Itens = new List<AcervoSolicitacaoItemConfirmarDTO>()
                {
                    new()
                    {
                        Id = 1,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial
                    },
                    new()
                    {
                        Id = 2,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Email
                    },
                    new()
                    {
                        Id = 3,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Email
                    }
                }
            };
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(acervoArteGraficaCadastroDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve confirmar atendimento quando não for localizada")]
        public async Task Nao_deve_confirmar_atendimento_quando_nao_for_localizada()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao(10);
            
            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var acervoArteGraficaCadastroDto = new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 101515,
                Itens = new List<AcervoSolicitacaoItemConfirmarDTO>()
                {
                    new()
                    {
                        Id = 1,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial
                    },
                    new()
                    {
                        Id = 2,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Email
                    },
                    new()
                    {
                        Id = 3,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Email
                    }
                }
            };
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(acervoArteGraficaCadastroDto).ShouldThrowAsync<NegocioException>();
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

            var acervoArteGraficaCadastroDto = new AcervoSolicitacaoConfirmarDTO()
            {
                Itens = new List<AcervoSolicitacaoItemConfirmarDTO>()
                {
                    new()
                    {
                        Id = 1,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial
                    },
                    new()
                    {
                        Id = 2,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                        TipoAtendimento = TipoAtendimento.Presencial
                    },
                    new()
                    {
                        Id = 3,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(10).Date,
                        TipoAtendimento = TipoAtendimento.Presencial
                    }
                }
            };
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(acervoArteGraficaCadastroDto).ShouldThrowAsync<NegocioException>();
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

            var acervoArteGraficaCadastroDto = new AcervoSolicitacaoConfirmarDTO()
            {
                Itens = new List<AcervoSolicitacaoItemConfirmarDTO>()
                {
                    new()
                    {
                        Id = 1,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial
                    },
                    new()
                    {
                        Id = 2,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                        TipoAtendimento = TipoAtendimento.Presencial
                    },
                    new()
                    {
                        Id = 3,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(10).Date,
                        TipoAtendimento = TipoAtendimento.Presencial
                    }
                }
            };
            
            await servicoAcervoSolicitacao.ConfirmarAtendimento(acervoArteGraficaCadastroDto).ShouldThrowAsync<NegocioException>();
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
            
            var retorno = await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                ResponsavelRf = "login_1",
                Itens = new List<AcervoSolicitacaoItemConfirmarDTO>()
                {
                    new()
                    {
                        Id = 1,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                        TipoAtendimento = TipoAtendimento.Presencial
                    },
                    new()
                    {
                        Id = 2,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(10).Date,
                        TipoAtendimento = TipoAtendimento.Presencial
                    }
                }
            });
            
            retorno.ShouldBeTrue();
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