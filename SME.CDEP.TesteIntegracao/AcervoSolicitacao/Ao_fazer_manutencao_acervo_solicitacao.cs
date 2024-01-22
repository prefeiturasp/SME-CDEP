using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_fazer_manutencao_acervo_solicitacao : TesteBase
    {
        public Ao_fazer_manutencao_acervo_solicitacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo Solicitação - Obter por Id")]
        public async Task Obter_por_id()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
            await InserirAcervoSolicitacao(acervoSolicitacao);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var retorno = await servicoAcervoSolicitacao.ObterPorId(1);
            retorno.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
            await InserirAcervoSolicitacao(acervoSolicitacao,10);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var retorno = await servicoAcervoSolicitacao.ObterTodosPorUsuario(1);
            retorno.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Inserir")]
        public async Task Ao_inserir()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
            await InserirAcervoSolicitacao(acervoSolicitacao,10);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var acervoSolicitacaoInserir = ObterAcervoSolicitacaoDTO();
            acervoSolicitacaoInserir.Situacao = SituacaoSolicitacao.FINALIZADO_ATENDIMENTO;
            foreach (var item in acervoSolicitacaoInserir.Itens)
                item.Situacao = SituacaoSolicitacaoItem.FINALIZADO;
            
            // var acervoSolicitacaoId = await servicoAcervoSolicitacao.Inserir(acervoSolicitacaoInserir);
            // acervoSolicitacaoId.ShouldBeGreaterThan(0);
            //
            // var acervoSolicitacaoInserido = await servicoAcervoSolicitacao.ObterPorId(acervoSolicitacaoId);
            // acervoSolicitacaoInserido.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Atualizar")]
        public async Task Ao_atualizar()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
            await InserirAcervoSolicitacao(acervoSolicitacao,10);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var acervoSolicitacaoAlterar = await servicoAcervoSolicitacao.ObterPorId(10);
            acervoSolicitacaoAlterar.Situacao = SituacaoSolicitacao.FINALIZADO_ATENDIMENTO;
            foreach (var item in acervoSolicitacaoAlterar.Itens)
                item.Situacao = SituacaoSolicitacaoItem.FINALIZADO;
            
            var acervoSolicitacaoAlterado = await servicoAcervoSolicitacao.Alterar(acervoSolicitacaoAlterar);
            acervoSolicitacaoAlterado.ShouldNotBeNull();
            acervoSolicitacaoAlterado.Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
            acervoSolicitacaoAlterado.Id.ShouldBe(10);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Remover")]
        public async Task Ao_remover()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
            await InserirAcervoSolicitacao(acervoSolicitacao,10);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var retornoAcervoSolicitacaoAlterado = await servicoAcervoSolicitacao.Remover(1);
            retornoAcervoSolicitacaoAlterado.ShouldBeTrue();
            
            var acervoSolicitacaoAlterado = await servicoAcervoSolicitacao.ObterPorId(1);
            acervoSolicitacaoAlterado.ShouldBeNull();
        }

        private AcervoSolicitacaoDTO ObterAcervoSolicitacaoDTO()
        {
            var acervoSolicitacao = new AcervoSolicitacaoDTO()
            {
                UsuarioId = 1,
                Situacao = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema", CriadoPor = "Sistema",
                Itens = new List<AcervoSolicitacaoItemDTO>()
                {
                    new ()
                    {
                        Situacao = SituacaoSolicitacaoItem.EM_ANALISE,
                        AcervoId = 1,
                        CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema", CriadoPor = "Sistema",
                    },
                    new ()
                    {
                        Situacao = SituacaoSolicitacaoItem.FINALIZADO,
                        AcervoId = 2,
                        CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema", CriadoPor = "Sistema",
                    },
                    new ()
                    {
                        Situacao = SituacaoSolicitacaoItem.FINALIZADO,
                        AcervoId = 3,
                        CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema", CriadoPor = "Sistema",
                    }
                }
            };
            return acervoSolicitacao;
        }
        
        private AcervoSolicitacao ObterAcervoSolicitacao()
        {
            var acervoSolicitacao = new AcervoSolicitacao()
            {
                UsuarioId = 1,
                Situacao = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema", CriadoPor = "Sistema",
                Itens = new List<AcervoSolicitacaoItem>()
                {
                    new ()
                    {
                        Situacao = SituacaoSolicitacaoItem.EM_ANALISE,
                        AcervoId = 1,
                        CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema", CriadoPor = "Sistema",
                    },
                    new ()
                    {
                        Situacao = SituacaoSolicitacaoItem.FINALIZADO,
                        AcervoId = 2,
                        CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema", CriadoPor = "Sistema",
                    },
                    new ()
                    {
                        Situacao = SituacaoSolicitacaoItem.FINALIZADO,
                        AcervoId = 3,
                        CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema", CriadoPor = "Sistema",
                    }
                }
            };
            return acervoSolicitacao;
        }

        private async Task InserirAcervoSolicitacao(AcervoSolicitacao acervoSolicitacao, int quantidade = 1)
        {
            for (int i = 1; i <= quantidade; i++)
            {
                await InserirNaBase(acervoSolicitacao);

                var acervoSolicitadoId = (ObterTodos<AcervoSolicitacao>()).LastOrDefault().Id;
                foreach (var item in acervoSolicitacao.Itens)
                {
                    item.AcervoSolicitacaoId = acervoSolicitadoId;
                    await InserirNaBase(item);
                }
            }
        }
        
       private async Task InserirAcervoTridimensional()
        {
            var random = new Random();
            
            var arquivoId = 1;

            for (int j = 1; j <= 35; j++)
            {
                await InserirNaBase(new Acervo()
                {
                    Codigo = $"{j.ToString()}.TD",
                    Titulo = faker.Lorem.Text().Limite(500),
                    Descricao = faker.Lorem.Text(),
                    TipoAcervoId = (int)TipoAcervo.Tridimensional,
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia().AddMinutes(-15),
                    CriadoLogin = ConstantesTestes.LOGIN_123456789,
                    Ano = faker.Date.Past().Year,
                    DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                });

                await InserirNaBase(new AcervoTridimensional()
                {
                    AcervoId = j,
                    Procedencia = faker.Lorem.Text().Limite(200),
                    ConservacaoId = random.Next(1,5),
                    Quantidade = random.Next(15,55),
                    Largura = double.Parse("50,45"),
                    Altura = double.Parse("10.20"),
                    Diametro = double.Parse("1540"),	
                    Profundidade = double.Parse("1801"),	
                });
                
                await InserirNaBase(new Arquivo()
                {
                    Nome = faker.Lorem.Text(),
                    Codigo = Guid.NewGuid(),
                    Tipo = TipoArquivo.AcervoTridimensional,
                    TipoConteudo = ConstantesTestes.MIME_TYPE_JPG,
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia().AddMinutes(-15),
                    CriadoLogin = ConstantesTestes.LOGIN_123456789,
                });
                
                arquivoId++;
                
                await InserirNaBase(new Arquivo()
                {
                    Nome = $"{faker.Lorem.Text()}_{arquivoId}.jpeg",
                    Codigo = Guid.NewGuid(),
                    Tipo = TipoArquivo.AcervoArteGrafica,
                    TipoConteudo = ConstantesTestes.MIME_TYPE_JPG,
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia().AddMinutes(-15),
                    CriadoLogin = ConstantesTestes.LOGIN_123456789,
                });
                
                await InserirNaBase(new AcervoTridimensionalArquivo()
                {
                    ArquivoId = arquivoId-1,
                    AcervoTridimensionalId = j,
                    ArquivoMiniaturaId = arquivoId
                });
                
                arquivoId++;
            }
        }
    }
}