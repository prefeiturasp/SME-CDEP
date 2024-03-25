using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Constantes;
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
        
        [Fact(DisplayName = "Acervo Solicitação Manual - Deve alterar solicitação de acervo com itens presenciais e via e-mail, com um item novo")]
        public async Task Deve_alterar_solicitacao_manual_com_itens_presenciais_e_via_email_com_um_item_novo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

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
                DataSolicitacao = DateTimeExtension.HorarioBrasilia().Date.AddDays(-10),
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
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(20),
                        TipoAcervo = TipoAcervo.Bibliografico
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
            solicitacaoCadastrada.UsuarioId.ShouldBe(alteracaoAcervoSolicitacaoManual.UsuarioId);
            solicitacaoCadastrada.DataSolicitacao.ShouldBe(alteracaoAcervoSolicitacaoManual.DataSolicitacao);
            solicitacaoCadastrada.Origem.ShouldBe(Origem.Manual);
            solicitacaoCadastrada.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
            solicitacaoCadastrada.Excluido.ShouldBeFalse();
            
            var itensCadastrados = ObterTodos<AcervoSolicitacaoItem>();

            var primeiroItemEmail = itensCadastrados.FirstOrDefault(f => f.AcervoId == 1);
            primeiroItemEmail.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            primeiroItemEmail.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            primeiroItemEmail.DataVisita.ShouldBeNull();
            primeiroItemEmail.Excluido.ShouldBeFalse();
            primeiroItemEmail.ResponsavelId.ShouldNotBeNull();
            
            var segundoItemEmail = itensCadastrados.FirstOrDefault(f => f.AcervoId == 2);
            segundoItemEmail.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            segundoItemEmail.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            segundoItemEmail.Excluido.ShouldBeFalse();
            segundoItemEmail.ResponsavelId.ShouldNotBeNull();
            
            var terceiroItemPresencial = itensCadastrados.FirstOrDefault(f => f.AcervoId == 3);
            terceiroItemPresencial.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            terceiroItemPresencial.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            terceiroItemPresencial.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            terceiroItemPresencial.Excluido.ShouldBeFalse();
            terceiroItemPresencial.ResponsavelId.ShouldNotBeNull();
            
            var quartoItemEmail = itensCadastrados.FirstOrDefault(f => f.AcervoId == 4);
            quartoItemEmail.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            quartoItemEmail.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            quartoItemEmail.Excluido.ShouldBeFalse();
            quartoItemEmail.ResponsavelId.ShouldNotBeNull();
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(2);
            eventos.Count(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().Date).ShouldBe(1);
            eventos.Count(a=> a.Excluido).ShouldBe(1);
            eventos.Count(a=> !a.Excluido).ShouldBe(1);
            
            var acervoEmprestimos = ObterTodos<AcervoEmprestimo>();
            acervoEmprestimos.Count().ShouldBe(1);

            acervoEmprestimos.Count(a=> a.DataEmprestimo.Date == DateTimeExtension.HorarioBrasilia().Date && a.Situacao.EstaEmprestado()).ShouldBe(1);
            acervoEmprestimos.Count(a=> a.DataDevolucao.Date == DateTimeExtension.HorarioBrasilia().AddDays(20).Date && a.Situacao.EstaEmprestado()).ShouldBe(1);;
        }
        
        [Fact(DisplayName = "Acervo Solicitação Manual - Deve alterar solicitação de acervo com itens presenciais e via e-mail, com todos os itens novos")]
        public async Task Deve_alterar_solicitacao_manual_com_itens_presenciais_e_via_email_com_todos_os_itens_novos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();
            await InserirAcervosTridimensionais(11);
            
            await InserirNaBase(new Evento()
            {
                Data = DateTimeExtension.HorarioBrasilia().AddDays(50).Date,
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
                        AcervoId = 11,
                        TipoAtendimento = TipoAtendimento.Email,
                        TipoAcervo = TipoAcervo.Bibliografico
                    },
                    new ()
                    {
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(4),
                        TipoAcervo = TipoAcervo.Bibliografico
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(40),
                        TipoAcervo = TipoAcervo.Bibliografico
                    }
                }
            };
            
            var acervoSolicitacaoInserida = await servicoAcervoSolicitacao.Inserir(acervoSolicitacaoManual);
            acervoSolicitacaoInserida.ShouldBeGreaterThan(0);

            var itensInseridos = ObterTodos<AcervoSolicitacaoItem>().Where(w => w.AcervoSolicitacaoId == acervoSolicitacaoInserida);
            
            //Cancelando itens
            foreach (var item in itensInseridos)
                (await servicoAcervoSolicitacao.CancelarItemAtendimento(item.Id)).ShouldBeTrue();
            
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
                        AcervoId = 12,
                        TipoAtendimento = TipoAtendimento.Email,
                        TipoAcervo = TipoAcervo.Tridimensional
                    },
                    new ()
                    {
                        AcervoId = 13,
                        TipoAtendimento = TipoAtendimento.Email,
                        TipoAcervo = TipoAcervo.Tridimensional
                    },
                    new ()
                    {
                        AcervoId = 6,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(50),
                        TipoAcervo = TipoAcervo.Bibliografico
                    }
                }
            };
            
            acervoSolicitacaoInserida = await servicoAcervoSolicitacao.Alterar(alteracaoAcervoSolicitacaoManual);
            acervoSolicitacaoInserida.ShouldBeGreaterThan(0);

            var solicitacaoCadastrada = ObterTodos<AcervoSolicitacao>().LastOrDefault();
            solicitacaoCadastrada.UsuarioId.ShouldBe(acervoSolicitacaoManual.UsuarioId);
            solicitacaoCadastrada.DataSolicitacao.ShouldBe(acervoSolicitacaoManual.DataSolicitacao);
            solicitacaoCadastrada.Origem.ShouldBe(Origem.Manual);
            solicitacaoCadastrada.Situacao.ShouldBe(SituacaoSolicitacao.AGUARDANDO_VISITA);
            solicitacaoCadastrada.Excluido.ShouldBeFalse();
            
            var itensCadastrados = ObterTodos<AcervoSolicitacaoItem>();

            var primeiroItemEmail = itensCadastrados.FirstOrDefault(f => f.AcervoId == 4);
            primeiroItemEmail.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            primeiroItemEmail.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            primeiroItemEmail.DataVisita.ShouldBeNull();
            primeiroItemEmail.Excluido.ShouldBeFalse();
            primeiroItemEmail.ResponsavelId.ShouldNotBeNull();
            
            var segundoItemEmail = itensCadastrados.FirstOrDefault(f => f.AcervoId == 5);
            segundoItemEmail.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            segundoItemEmail.TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            segundoItemEmail.Excluido.ShouldBeFalse();
            segundoItemEmail.ResponsavelId.ShouldNotBeNull();
            
            var terceiroItemPresencial = itensCadastrados.FirstOrDefault(f => f.AcervoId == 6);
            terceiroItemPresencial.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_VISITA);
            terceiroItemPresencial.TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);
            terceiroItemPresencial.DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().AddDays(50).Date);
            terceiroItemPresencial.Excluido.ShouldBeFalse();
            terceiroItemPresencial.ResponsavelId.ShouldNotBeNull();
            
            var eventos = ObterTodos<Evento>();
            eventos.Count().ShouldBe(4);
            eventos.Count(a=> a.Excluido).ShouldBe(2);
            eventos.Count(a=> !a.Excluido).ShouldBe(2);
            eventos.Any(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(50).Date).ShouldBeTrue();
            eventos.Count(a=> a.Data.Date == DateTimeExtension.HorarioBrasilia().AddDays(50).Date).ShouldBe(2);
            
            var acervoEmprestimos = ObterTodos<AcervoEmprestimo>();
            acervoEmprestimos.Count().ShouldBe(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação Manual - Não deve alterar solicitação de acervo manual em dia de feriado")]
        public async Task Nao_deve_alterar_solicitacao_manual_em_dia_de_feriado()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional(true);
            
            await InserirNaBase(new Evento()
            {
                Data = DateTimeExtension.HorarioBrasilia().AddDays(20).Date,
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
                DataSolicitacao = DateTimeExtension.HorarioBrasilia().Date.AddDays(-10),
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
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(20)
                    },
                    new ()
                    {
                        AcervoId = 4,
                        TipoAtendimento = TipoAtendimento.Email
                    }
                }
            };
            
            // act
            var excecao = await Should.ThrowAsync<NegocioException>(() => servicoAcervoSolicitacao.Alterar(alteracaoAcervoSolicitacaoManual));

            // assert
            excecao.Message.Contains(MensagemNegocio.DATAS_DE_VISITAS_CONFLITANTES.Substring(0,75)).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação Manual - Não deve alterar solicitação de acervo em dia de suspensão")]
        public async Task Nao_deve_alterar_solicitacao_manual_em_dia_de_suspensao()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional(true);
            
            await InserirNaBase(new Evento()
            {
                Data = DateTimeExtension.HorarioBrasilia().AddDays(20).Date,
                Tipo = TipoEvento.SUSPENSAO,
                Descricao = "Suspensão",
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
            
            //Atualizando
            var alteracaoAcervoSolicitacaoManual = new AcervoSolicitacaoManualDTO()
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
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(20)
                    },
                    new ()
                    {
                        AcervoId = 4,
                        TipoAtendimento = TipoAtendimento.Email
                    }
                }
            };
            
            // act
            var excecao = await Should.ThrowAsync<NegocioException>(() => servicoAcervoSolicitacao.Alterar(alteracaoAcervoSolicitacaoManual));

            // assert
            excecao.Message.Contains(MensagemNegocio.DATAS_DE_VISITAS_CONFLITANTES.Substring(0,75)).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação Manual - Não deve inserir com data de empréstimo futura")]
        public async Task Nao_deve_inserir_solicitacao_manual_com_data_de_emprestimo_futura()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();
            
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
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(4),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(4),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(40),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(4),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    }
                }
            };
            
            // act
            var excecao = await Should.ThrowAsync<NegocioException>(() => servicoAcervoSolicitacao.Inserir(acervoSolicitacaoManual));

            // assert
            excecao.Message.Contains(MensagemNegocio.DATA_DO_EMPRESTIMO_NAO_PODE_SER_FUTURA).ShouldBeTrue(); 
        }
        
        [Fact(DisplayName = "Acervo Solicitação Manual - Não deve inserir com data de empréstimo menor que data de visita")]
        public async Task Nao_deve_inserir_solicitacao_manual_com_data_de_emprestimo_menor_que_visita()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();
            
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
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(-4),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(-4),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(40),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(-4),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    }
                }
            };
            
            // act
            var excecao = await Should.ThrowAsync<NegocioException>(() => servicoAcervoSolicitacao.Inserir(acervoSolicitacaoManual));

            // assert
            excecao.Message.Contains(MensagemNegocio.DATA_DO_EMPRESTIMO_MENOR_QUE_DATA_VISITA).ShouldBeTrue(); 
        }
        
        [Fact(DisplayName = "Acervo Solicitação Manual - Não deve inserir com data de devolução menor que data de empréstimo")]
        public async Task Nao_deve_inserir_solicitacao_manual_com_data_de_devolucao_menor_que_data_emprestimo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();
            
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
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-1),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-5),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    }
                }
            };
            
            // act
            var excecao = await Should.ThrowAsync<NegocioException>(() => servicoAcervoSolicitacao.Inserir(acervoSolicitacaoManual));

            // assert
            excecao.Message.Contains(MensagemNegocio.DATA_DA_DEVOLUCAO_MENOR_DATA_DO_EMPRESTIMO).ShouldBeTrue(); 
        }
        
        [Fact(DisplayName = "Acervo Solicitação Manual - Não deve inserir com data de empréstimo e devolução em acervos diferentes de bibliográficos")]
        public async Task Nao_deve_inserir_solicitacao_manual_com_data_de_emprestimo_devolucao_em_acervos_diferentes_de_bibliograficos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();
            
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
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-1),
                        TipoAcervo = TipoAcervo.Tridimensional,
                    },
                    new ()
                    {
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-5),
                        TipoAcervo = TipoAcervo.Tridimensional,
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                        TipoAcervo = TipoAcervo.Tridimensional,
                    }
                }
            };
            
            // act
            var excecao = await Should.ThrowAsync<NegocioException>(() => servicoAcervoSolicitacao.Inserir(acervoSolicitacaoManual));

            // assert
            excecao.Message.Contains(MensagemNegocio.DATA_DO_EMPRESTIMO_E_DEVOLUCAO_EXCLUSIVO_PARA_ACERVOS_BIBLIOGRAFICOS).ShouldBeTrue(); 
        }
        
        [Fact(DisplayName = "Acervo Solicitação Manual - Não deve alterar com data de empréstimo futura")]
        public async Task Nao_deve_alterar_solicitacao_manual_com_data_de_emprestimo_futura()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

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
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
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
                DataSolicitacao = DateTimeExtension.HorarioBrasilia().Date.AddDays(-10),
                Itens = new List<AcervoSolicitacaoItemManualDTO>()
                {
                    new ()
                    {
                        Id = 1,
                        AcervoId = 1,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(4),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        Id = 2,
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(4),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        Id = 3,
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(40),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(4),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    }
                }
            };
            
            // act
            var excecao = await Should.ThrowAsync<NegocioException>(() => servicoAcervoSolicitacao.Alterar(alteracaoAcervoSolicitacaoManual));

            // assert
            excecao.Message.Contains(MensagemNegocio.DATA_DO_EMPRESTIMO_NAO_PODE_SER_FUTURA).ShouldBeTrue(); 
        }
        
        [Fact(DisplayName = "Acervo Solicitação Manual - Não deve alterar com data de empréstimo menor que data de visita")]
        public async Task Nao_deve_alterar_solicitacao_manual_com_data_de_emprestimo_menor_que_visita()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

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
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
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
                DataSolicitacao = DateTimeExtension.HorarioBrasilia().Date.AddDays(-10),
                Itens = new List<AcervoSolicitacaoItemManualDTO>()
                {
                    new ()
                    {
                        Id = 1,
                        AcervoId = 1,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(-4),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        Id = 2,
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(-4),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        Id = 3,
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia().AddDays(40),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(-4),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    }
                }
            };
            
            // act
            var excecao = await Should.ThrowAsync<NegocioException>(() => servicoAcervoSolicitacao.Alterar(alteracaoAcervoSolicitacaoManual));

            // assert
            excecao.Message.Contains(MensagemNegocio.DATA_DO_EMPRESTIMO_MENOR_QUE_DATA_VISITA).ShouldBeTrue(); 
        }
        
        [Fact(DisplayName = "Acervo Solicitação Manual - Não deve alterar com data de devolução menor que data de empréstimo")]
        public async Task Nao_deve_alterar_solicitacao_manual_com_data_de_devolucao_menor_que_data_emprestimo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

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
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
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
                DataSolicitacao = DateTimeExtension.HorarioBrasilia().Date.AddDays(-10),
                Itens = new List<AcervoSolicitacaoItemManualDTO>()
                {
                    new ()
                    {
                        Id = 1,
                        AcervoId = 1,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-1),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        Id = 2,
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-5),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        Id = 3,
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    }
                }
            };
            
            // act
            var excecao = await Should.ThrowAsync<NegocioException>(() => servicoAcervoSolicitacao.Alterar(alteracaoAcervoSolicitacaoManual));

            // assert
            excecao.Message.Contains(MensagemNegocio.DATA_DA_DEVOLUCAO_MENOR_DATA_DO_EMPRESTIMO).ShouldBeTrue(); 
        }
        
        [Fact(DisplayName = "Acervo Solicitação Manual - Não deve alterar com data de empréstimo e devolução em acervos diferentes de bibliográficos")]
        public async Task Nao_deve_alterar_solicitacao_manual_com_data_de_emprestimo_devolucao_em_acervos_diferentes_de_bibliograficos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

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
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
                    },
                    new ()
                    {
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(10),
                        TipoAcervo = TipoAcervo.Bibliografico,
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
                DataSolicitacao = DateTimeExtension.HorarioBrasilia().Date.AddDays(-10),
                Itens = new List<AcervoSolicitacaoItemManualDTO>()
                {
                    new ()
                    {
                        Id = 1,
                        AcervoId = 1,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-1),
                        TipoAcervo = TipoAcervo.Tridimensional,
                    },
                    new ()
                    {
                        Id = 2,
                        AcervoId = 2,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-5),
                        TipoAcervo = TipoAcervo.Tridimensional,
                    },
                    new ()
                    {
                        Id = 3,
                        AcervoId = 3,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataVisita = DateTimeExtension.HorarioBrasilia(),
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia(),
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                        TipoAcervo = TipoAcervo.Tridimensional,
                    }
                }
            };
            
            // act
            var excecao = await Should.ThrowAsync<NegocioException>(() => servicoAcervoSolicitacao.Alterar(alteracaoAcervoSolicitacaoManual));

            // assert
            excecao.Message.Contains(MensagemNegocio.DATA_DO_EMPRESTIMO_E_DEVOLUCAO_EXCLUSIVO_PARA_ACERVOS_BIBLIOGRAFICOS).ShouldBeTrue(); 
        }
    }
}