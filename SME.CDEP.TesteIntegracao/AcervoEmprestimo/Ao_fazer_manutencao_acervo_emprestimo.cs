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
       
        [Fact(DisplayName = "Acervo empréstimo - Deve inserir empréstimo de acervo")]
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

        [Fact(DisplayName = "Acervo Solicitação com empréstimo - Confirmar empréstimo")]
        public async Task Deve_confirmar_atendimento()
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
            
            var retorno = await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                Itens = new List<AcervoSolicitacaoItemConfirmarDto>()
                {
                    new()
                    {
                        Id = 1,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                        TipoAcervo = TipoAcervo.Bibliografico
                    },
                    new()
                    {
                        Id = 2,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(7).Date,
                        TipoAcervo = TipoAcervo.Bibliografico
                    }
                }
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
            
            var retorno = await servicoAcervoSolicitacao.ConfirmarAtendimento(new AcervoSolicitacaoConfirmarDTO()
            {
                Id = 1,
                Itens = new List<AcervoSolicitacaoItemConfirmarDto>()
                {
                    new()
                    {
                        Id = 1,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        TipoAcervo = TipoAcervo.Bibliografico
                    },
                    new()
                    {
                        Id = 2,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        TipoAcervo = TipoAcervo.Bibliografico
                    }
                }
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
                Itens = new List<AcervoSolicitacaoItemConfirmarDto>()
                {
                    new()
                    {
                        Id = 1,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                        TipoAcervo = TipoAcervo.Fotografico
                    },
                    new()
                    {
                        Id = 2,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(7).Date,
                        TipoAcervo = TipoAcervo.Fotografico
                    }
                }
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
                Itens = new List<AcervoSolicitacaoItemConfirmarDto>()
                {
                    new()
                    {
                        Id = 1,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                        TipoAcervo = TipoAcervo.Bibliografico
                    },
                    new()
                    {
                        Id = 2,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAcervo = TipoAcervo.Bibliografico
                    }
                }
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
                Itens = new List<AcervoSolicitacaoItemConfirmarDto>()
                {
                    new()
                    {
                        Id = 1,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                        TipoAcervo = TipoAcervo.Fotografico
                    },
                    new()
                    {
                        Id = 2,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(7).Date,
                        TipoAcervo = TipoAcervo.Fotografico
                    }
                }
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
                Itens = new List<AcervoSolicitacaoItemConfirmarDto>()
                {
                    new()
                    {
                        Id = 1,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(-5).Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(8).Date,
                        TipoAcervo = TipoAcervo.Fotografico
                    },
                    new()
                    {
                        Id = 2,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(-5).Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(7).Date,
                        TipoAcervo = TipoAcervo.Fotografico
                    }
                }
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
                Itens = new List<AcervoSolicitacaoItemConfirmarDto>()
                {
                    new()
                    {
                        Id = 1,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-8).Date,
                        TipoAcervo = TipoAcervo.Fotografico
                    },
                    new()
                    {
                        Id = 2,
                        DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                        TipoAtendimento = TipoAtendimento.Presencial,
                        DataEmprestimo = DateTimeExtension.HorarioBrasilia().AddDays(5).Date,
                        DataDevolucao = DateTimeExtension.HorarioBrasilia().AddDays(-7).Date,
                        TipoAcervo = TipoAcervo.Fotografico
                    }
                }
            }).ShouldThrowAsync<NegocioException>();
        }

        private async Task InserirAcervoSolicitacaoItem(TipoAtendimento atendimentoPresencial, DateTime dataVisita, long acervoSolicitacaoId, int qtdeItens)
        {
            await InserirVariosNaBase(AcervoSolicitacaoItemMock.Instance.Gerar(acervoSolicitacaoId,atendimentoPresencial, dataVisita).Generate(qtdeItens));
        }

        private async Task InserirAcervosSolicitacoes()
        {
            var inserindoSolicitacoes = AcervoSolicitacaoMock.Instance.Gerar().Generate(4);
            await InserirVariosNaBase(inserindoSolicitacoes);
        }

        private async Task InserirAcervosBibliograficos()
        {
            var acervoId = 1;
            var inserindoAcervoBibliografico = AcervoBibliograficoMock.Instance.Gerar().Generate(10);
            foreach (var acervoBibliografico in inserindoAcervoBibliografico)
            {
                await InserirNaBase(acervoBibliografico.Acervo);
                acervoBibliografico.AcervoId = acervoId;
                await InserirNaBase(acervoBibliografico);
                acervoId++;
            }
        }

        private async Task InserirAcervoEmprestimo(long id, DateTime dataVisita)
        {
            var inserirAcervoEmprestimo = AcervoEmprestimoMock.Instance.Gerar(id, dataVisita).Generate();
            await InserirNaBase(inserirAcervoEmprestimo);
        }
    }
}