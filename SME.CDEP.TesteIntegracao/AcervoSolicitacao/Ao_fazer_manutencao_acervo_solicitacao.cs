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
        
        [Fact(DisplayName = "Acervo Solicitação - Obter todos por usuário logado")]
        public async Task Obter_todos_por_usuario_logado()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var acervoSolicitacao = ObterAcervoSolicitacao();
            
            await InserirAcervoSolicitacao(acervoSolicitacao,10);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var retorno = await servicoAcervoSolicitacao.ObterMinhasSolicitacoes();
            retorno.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Ao enviar a solicitação para análise - offline - sem arquivos")]
        public async Task Ao_enviar_solicitacao_para_analise_sem_arquivos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional(false);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var acervoSolicitacaoInserir = ObterAcervoSolicitacaoDTO();
            
            var retorno = await servicoAcervoSolicitacao.Inserir(acervoSolicitacaoInserir);
            retorno.ShouldNotBeNull();
            retorno.Any(a=> a.Arquivos.NaoPossuiElementos()).ShouldBeTrue();
            retorno.All(a=> a.Situacao.ToString().Equals("AGUARDANDO_ATENDIMENTO")).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Ao enviar a solicitação para finalizado - online - com arquivos")]
        public async Task Ao_enviar_solicitacao_para_finalizado_com_arquivos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var acervoSolicitacaoInserir = ObterAcervoSolicitacaoDTO();
            
            var retorno = await servicoAcervoSolicitacao.Inserir(acervoSolicitacaoInserir);
            retorno.ShouldNotBeNull();
            retorno.Any(a=> a.Arquivos.PossuiElementos()).ShouldBeTrue();
            retorno.All(a=> a.Situacao.ToString().Equals("FINALIZADO_AUTOMATICAMENTE")).ShouldBeTrue();
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
            acervoSolicitacaoAlterado.PossuiElementos().ShouldBeFalse();
        }

        private AcervoSolicitacaoCadastroDTO ObterAcervoSolicitacaoDTO()
        {
            var itens = new List<AcervoSolicitacaoItemCadastroDTO>()
            {
                new() { AcervoId = 1 },
                new() { AcervoId = 2 },
                new() { AcervoId = 3 },
            };

            return new AcervoSolicitacaoCadastroDTO()
            {
                UsuarioId = 1,
                Itens = itens.ToArray()
            };
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
                        Situacao = SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO,
                        AcervoId = 1,
                        CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema", CriadoPor = "Sistema",
                    },
                    new ()
                    {
                        Situacao = SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE,
                        AcervoId = 2,
                        CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = "Sistema", CriadoPor = "Sistema",
                    },
                    new ()
                    {
                        Situacao = SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE,
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
        
       private async Task InserirAcervoTridimensional(bool incluirArquivos = true)
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

                if (incluirArquivos)
                {
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
}