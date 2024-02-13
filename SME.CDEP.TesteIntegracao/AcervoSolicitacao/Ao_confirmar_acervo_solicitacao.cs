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
            
            var itensAlterados = ObterTodos<AcervoSolicitacaoItem>().Where(w=> w.AcervoSolicitacaoId == 1);
            itensAlterados.Count().ShouldBe(3);
            
            var itensAlteradosPresenciais = itensAlterados.Where(w => w.TipoAtendimento.HasValue && w.TipoAtendimento.Value.EhAtendimentoPresencial());
            itensAlteradosPresenciais.FirstOrDefault().DataVisita.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            itensAlteradosPresenciais.FirstOrDefault().TipoAtendimento.ShouldBe(TipoAtendimento.Presencial);

            var itensAlteradosEmail = itensAlterados.Where(w => w.TipoAtendimento.HasValue && w.TipoAtendimento.Value.EhAtendimentoViaEmail());
            itensAlteradosEmail.FirstOrDefault().DataVisita.ShouldBeNull();
            itensAlteradosEmail.FirstOrDefault().TipoAtendimento.ShouldBe(TipoAtendimento.Email);
            
            var itensImutavel = itensAlterados.Where(w => w.TipoAtendimento.EhNulo());
            itensImutavel.LastOrDefault().DataVisita.ShouldBeNull();
            itensImutavel.LastOrDefault().TipoAtendimento.ShouldBeNull();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve confirmar o atendimento sem itens")]
        public async Task Noa_deve_confirmar_atendimento_sem_itens()
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
        public async Task Noa_deve_confirmar_atendimento_em_itens_sem_data_de_visita_tipo_presencial()
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
        public async Task Noa_deve_confirmar_atendimento_em_itens_com_data_de_visita_passada_nos_itens_tipo_presenciais()
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
        public async Task Noa_deve_confirmar_atendimento_em_itens_com_data_de_visita_valida_nos_itens_tipo_email()
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
        public async Task Noa_deve_confirmar_atendimento_quando_nao_for_localizada()
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
    }
}