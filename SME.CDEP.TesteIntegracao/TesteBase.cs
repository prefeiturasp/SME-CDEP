using AutoMapper;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Constantes;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.Webapi.Contexto;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace SME.CDEP.TesteIntegracao
{
    [Collection("TesteIntegradoCDEP")]
    public class TesteBase : BaseMock, IClassFixture<TestFixture>
    {
        protected readonly CollectionFixture _collectionFixture;
        protected readonly Faker faker;

        public ServiceProvider ServiceProvider => _collectionFixture.ServiceProvider;

        public TesteBase(CollectionFixture collectionFixture)
        {
            _collectionFixture = collectionFixture;
            _collectionFixture.Database.LimparBase();
            _collectionFixture.IniciarServicos();

            RegistrarFakes(_collectionFixture.Services);
            _collectionFixture.BuildServiceProvider();
            
            faker = new Faker("pt_BR");
        }

        protected virtual void RegistrarFakes(IServiceCollection services)
        {
            RegistrarCommandFakes(services);
            RegistrarQueryFakes(services);        
        }

        protected virtual void RegistrarCommandFakes(IServiceCollection services)
        {
            //services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>),typeof(PublicarFilaSgpCommandHandlerFake), ServiceLifetime.Scoped));
        }

        protected virtual void RegistrarQueryFakes(IServiceCollection services)
        {
            //services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery, TurmaParaSyncInstitucionalDto>),
            //    typeof(ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQueryHandlerFake), ServiceLifetime.Scoped));
        }

        public Task InserirNaBase<T>(IEnumerable<T> objetos) where T : class, new()
        {
            _collectionFixture.Database.Inserir(objetos);
            return Task.CompletedTask;
        }

        public Task InserirNaBase<T>(T objeto) where T : class, new()
        {
            _collectionFixture.Database.Inserir(objeto);
            return Task.CompletedTask;
        }
        
        public Task AtualizarNaBase<T>(T objeto) where T : class, new()
        {
            _collectionFixture.Database.Atualizar(objeto);
            return Task.CompletedTask;
        }

        public Task InserirNaBase(string nomeTabela, params string[] campos)
        {
            _collectionFixture.Database.Inserir(nomeTabela, campos);
            return Task.CompletedTask;
        }
        
        public Task InserirNaBase(string nomeTabela, string[] campos, string[] valores)
        {
            _collectionFixture.Database.Inserir(nomeTabela, campos, valores);
            return Task.CompletedTask;
        }

        public IEnumerable<T> ObterTodos<T>() where T : class, new()
        {
            return _collectionFixture.Database.ObterTodos<T>();
        }

        public T ObterPorId<T, K>(K id)
            where T : class, new()
            where K : struct
        {
            return _collectionFixture.Database.ObterPorId<T, K>(id);
        }

        protected IServicoUsuario GetServicoUsuario()
        {
            return ObterServicoAplicacao<IServicoUsuario>();
        }
        
        protected IServicoAcessoDocumento GetServicoAcessoDocumento()
        {
            return ObterServicoAplicacao<IServicoAcessoDocumento>();
        }
        
        protected IServicoConservacao GetServicoConservacao()
        {
            return ObterServicoAplicacao<IServicoConservacao>();
        }
        
        protected IServicoCromia GetServicoCromia()
        {
            return ObterServicoAplicacao<IServicoCromia>();
        }
        
        protected IServicoFormato GetServicoFormato()
        {
            return ObterServicoAplicacao<IServicoFormato>();
        }
        
        protected IServicoIdioma GetServicoIdioma()
        {
            return ObterServicoAplicacao<IServicoIdioma>();
        }
        
        protected IServicoMaterial GetServicoMaterial()
        {
            return ObterServicoAplicacao<IServicoMaterial>();
        }
        
        protected IServicoSuporte GetServicoSuporte()
        {
            return ObterServicoAplicacao<IServicoSuporte>();
        }
        
        protected IServicoCreditoAutor GetServicoCreditoAutor()
        {
            return ObterServicoAplicacao<IServicoCreditoAutor>();
        }
        
        protected IServicoAcervo GetServicoAcervo()
        {
            return ObterServicoAplicacao<IServicoAcervo>();
        }
        
        protected IServicoEditora GetServicoEditora()
        {
            return ObterServicoAplicacao<IServicoEditora>();
        }
        
        protected IServicoAssunto GetServicoAssunto()
        {
            return ObterServicoAplicacao<IServicoAssunto>();
        }
        
        protected IServicoSerieColecao GetServicoSerieColecao()
        {
            return ObterServicoAplicacao<IServicoSerieColecao>();
        }
        
        protected IServicoAcervoFotografico GetServicoAcervoFotografico()
        {
            return ObterServicoAplicacao<IServicoAcervoFotografico>();
        }
        
        protected IServicoAcervoArteGrafica GetServicoAcervoArteGrafica()
        {
            return ObterServicoAplicacao<IServicoAcervoArteGrafica>();
        }
        
        protected IServicoAcervoTridimensional GetServicoAcervoTridimensional()
        {
            return ObterServicoAplicacao<IServicoAcervoTridimensional>();
        }
        
        protected IServicoAcervoSolicitacao GetServicoAcervoSolicitacao()
        {
            return ObterServicoAplicacao<IServicoAcervoSolicitacao>();
        }
        
        protected IServicoAcervoAudiovisual GetServicoAcervoAudiovisual()
        {
            return ObterServicoAplicacao<IServicoAcervoAudiovisual>();
        }
        
        protected IServicoAcervoDocumental GetServicoAcervoDocumental()
        {
            return ObterServicoAplicacao<IServicoAcervoDocumental>();
        }
        
        protected IServicoAcervoBibliografico GetServicoAcervoBibliografico()
        {
            return ObterServicoAplicacao<IServicoAcervoBibliografico>();
        }
        
        protected IMapper GetServicoMapper()
        {
            return ServiceProvider.GetService<IMapper>();
        }
        
        protected IServicoExcluirArquivo GetServicoExcluirArquivo()
        {
            return ServiceProvider.GetService<IServicoExcluirArquivo>();
        }
        
        protected IServicoMoverArquivoTemporario GetServicoMoverArquivoTemporario()
        {
            return ServiceProvider.GetService<IServicoMoverArquivoTemporario>();
        }
        
        protected IServicoUploadArquivo GetServicoUploadArquivo()
        {
            return ServiceProvider.GetService<IServicoUploadArquivo>();
        }
        
        protected IServicoImportacaoArquivoAcervoDocumental GetServicoImportacaoArquivoAcervoDocumental()
        {
            return ServiceProvider.GetService<IServicoImportacaoArquivoAcervoDocumental>();
        }
        
        protected IServicoImportacaoArquivoAcervoBibliografico GetServicoImportacaoArquivoAcervoBibliografico()
        {
            return ServiceProvider.GetService<IServicoImportacaoArquivoAcervoBibliografico>();
        }
        
        protected IServicoImportacaoArquivoAcervoArteGrafica GetServicoImportacaoArquivoAcervoArteGrafica()
        {
            return ServiceProvider.GetService<IServicoImportacaoArquivoAcervoArteGrafica>();
        }
        
        protected IServicoImportacaoArquivoAcervoAudiovisual GetServicoImportacaoArquivoAcervoAudiovisual()
        {
            return ServiceProvider.GetService<IServicoImportacaoArquivoAcervoAudiovisual>();
        }
        
        protected IServicoImportacaoArquivoAcervoFotografico GetServicoImportacaoArquivoAcervoFotografico()
        {
            return ServiceProvider.GetService<IServicoImportacaoArquivoAcervoFotografico>();
        }
        
        protected IServicoImportacaoArquivoAcervoTridimensional GetServicoImportacaoArquivoAcervoTridimensional()
        {
            return ServiceProvider.GetService<IServicoImportacaoArquivoAcervoTridimensional>();
        }
        
        protected IServicoImportacaoArquivoAcervo GetServicoImportacaoArquivoAcervo()
        {
            return ServiceProvider.GetService<IServicoImportacaoArquivoAcervo>();
        }
        
        public T ObterServicoAplicacao<T>()
            where T : IServicoAplicacao
        {
            return ServiceProvider.GetService<T>() ?? throw new Exception($"Serviço {typeof(T).Name} não registrado!");
        }
        
        protected void CriarClaimUsuario()
        {
            var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();
            
            contextoAplicacao.AdicionarVariaveis(ObterVariaveisPorPerfil());
        }

        private Dictionary<string, object> ObterVariaveisPorPerfil()
        {
            var rfLoginPerfil = ConstantesTestes.LOGIN_123456789;
            
            return new Dictionary<string, object>
            {
                { ConstantesTestes.USUARIO_CHAVE, ConstantesTestes.SISTEMA },
                { ConstantesTestes.USUARIO_LOGADO_CHAVE, ConstantesTestes.LOGIN_123456789 },
                {
                    ConstantesTestes.USUARIO_CLAIMS_CHAVE,
                    new Tuple<string, string>(rfLoginPerfil, ConstantesTestes.USUARIO_CLAIM_TIPO_RF)
                }
            };
        }

        protected async Task InserirDadosBasicosAleatorios()
        {
            var random = new Random();
            
            for (int i = 1; i <= 5; i++)
            {
                await InserirNaBase(new CreditoAutor()
                {
                    Nome = faker.Lorem.Sentence().Limite(200),
                    CriadoPor = ConstantesTestes.SISTEMA, 
                    CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                    CriadoLogin = ConstantesTestes.LOGIN_123456789 
                });
                
                await InserirNaBase(new Suporte()
                {
                    Nome = faker.Lorem.Sentence().Limite(500),
                    Tipo = (TipoSuporte)random.Next(1,2),
                });
                
                await InserirNaBase(new Formato()
                {
                    Nome = faker.Lorem.Sentence().Limite(500),
                    Tipo = (TipoFormato)random.Next(1,2),
                });
                
                await InserirNaBase(new Cromia() { Nome = faker.Lorem.Sentence().Limite(500)});
                
                await InserirNaBase(new Conservacao() { Nome = faker.Lorem.Sentence().Limite(500)});
                
                await InserirNaBase(new Idioma() { Nome = faker.Lorem.Sentence().Limite(500)});
                await InserirNaBase(new Material() { Nome = faker.Lorem.Sentence().Limite(500)});
                await InserirNaBase(new AcessoDocumento() { Nome = faker.Lorem.Sentence().Limite(500)});
                await InserirNaBase(new Editora()
                { 
                    Nome = faker.Lorem.Sentence().Limite(200), 
                    CriadoPor = ConstantesTestes.SISTEMA, 
                    CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                    CriadoLogin = ConstantesTestes.LOGIN_123456789 
                });
                await InserirNaBase(new SerieColecao()
                { 
                    Nome = faker.Lorem.Sentence().Limite(200), 
                    CriadoPor = ConstantesTestes.SISTEMA, 
                    CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                    CriadoLogin = ConstantesTestes.LOGIN_123456789 
                });
                await InserirNaBase(new Assunto()
                { 
                    Nome = faker.Lorem.Sentence().Limite(200), 
                    CriadoPor = ConstantesTestes.SISTEMA, 
                    CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                    CriadoLogin = ConstantesTestes.LOGIN_123456789 
                });
                
                await InserirNaBase(new Dominio.Entidades.Usuario()
                { 
                    Login = $"login_{i}", 
                    Nome = faker.Lorem.Sentence().Limite(200), 
                    CriadoPor = ConstantesTestes.SISTEMA, 
                    CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                    CriadoLogin = ConstantesTestes.LOGIN_123456789 
                });
            }
            await InserirNaBase(new Dominio.Entidades.Usuario()
            { 
                Login = "Sistema", 
                Nome = faker.Lorem.Sentence().Limite(200), 
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
        }
        
        protected async Task InserirDadosBasicos()
        {
            await InserirSuportesBase();
            await InserirConservacoesBase();
            await InserirCromiasBase();
            await InserirAcessoDocumentos();
            await InserirCreditosAutorias();
            await InserirFormatos();
            await InserirMateriais();
            await InserirIdiomas();
            await InserirSeriesColecoes();
            await InserirEditoras();
        }
        
        private async Task InserirEditoras()
        {
            var editoras = new List<Editora>
            {
                new() { Nome = ConstantesTestes.EDITORA_A },
                new() { Nome = ConstantesTestes.EDITORA_B },
                new() { Nome = ConstantesTestes.EDITORA_C},
                new() { Nome = ConstantesTestes.EDITORA_D },
            };

            foreach (var editora in editoras)
                await InserirNaBase(new Editora() 
                { 
                    Nome = editora.Nome, 
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), 
                    CriadoLogin = ConstantesTestes.LOGIN_123456789 
                });
        }
        
        private async Task InserirSeriesColecoes()
        {
            var serieColecoes = new List<SerieColecao>
            {
                new() { Nome = ConstantesTestes.SERIE_COLECAO_A },
                new() { Nome = ConstantesTestes.SERIE_COLECAO_B },
                new() { Nome = ConstantesTestes.SERIE_COLECAO_C},
                new() { Nome = ConstantesTestes.SERIE_COLECAO_D },
            };

            foreach (var serieColecao in serieColecoes)
                await InserirNaBase(new SerieColecao() 
                { 
                    Nome = serieColecao.Nome, 
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), 
                    CriadoLogin = ConstantesTestes.LOGIN_123456789 
                });
        }
        
        private async Task InserirIdiomas()
        {
            var idiomas = new List<Idioma>
            {
                new() { Nome = ConstantesTestes.PORTUGUES },
                new() { Nome = ConstantesTestes.INGLES },
                new() { Nome = ConstantesTestes.ESPANHOL},
                new() { Nome = ConstantesTestes.FRANCES },
            };

            foreach (var idioma in idiomas)
                await InserirNaBase(new Idioma() { Nome = idioma.Nome });
        }
        
        private async Task InserirMateriais()
        {
            var materiais = new List<Material>
            {
                new() { Nome = ConstantesTestes.APOSTILA, Tipo = TipoMaterial.DOCUMENTAL },
                new() { Nome = ConstantesTestes.LIVRO, Tipo = TipoMaterial.DOCUMENTAL },
                new() { Nome = ConstantesTestes.PERIODICO, Tipo = TipoMaterial.DOCUMENTAL },
                
                new() { Nome = ConstantesTestes.LIVRO, Tipo = TipoMaterial.BIBLIOGRAFICO },
                new() { Nome = ConstantesTestes.TESE, Tipo = TipoMaterial.BIBLIOGRAFICO },
                new() { Nome = ConstantesTestes.PERIODICO, Tipo = TipoMaterial.BIBLIOGRAFICO },
            };

            foreach (var material in materiais)
            {
                await InserirNaBase(new Material()
                {
                    Nome = material.Nome,
                    Tipo = material.Tipo
                });
            }
        }
        
        private async Task InserirFormatos()
        {
            var formatos = new List<Formato>
            {
                new() { Nome = ConstantesTestes.JPEG, Tipo = TipoFormato.ACERVO_FOTOS },
                new() { Nome = ConstantesTestes.TIFF, Tipo = TipoFormato.ACERVO_FOTOS },
            };

            foreach (var formato in formatos)
            {
                await InserirNaBase(new Formato()
                {
                    Nome = formato.Nome,
                    Tipo = formato.Tipo
                });
            }
        }
        
        private async Task InserirSuportesBase()
        {
            var suportes = new List<Suporte>
            {
                new() { Nome = ConstantesTestes.PAPEL, Tipo = TipoSuporte.IMAGEM },
                new() { Nome = ConstantesTestes.DIGITAL, Tipo = TipoSuporte.IMAGEM },
                new() { Nome = ConstantesTestes.NEGATIVO, Tipo = TipoSuporte.IMAGEM },
                new() { Nome = ConstantesTestes.VHS, Tipo = TipoSuporte.VIDEO },
                new() { Nome = ConstantesTestes.DVD, Tipo = TipoSuporte.VIDEO },
            };

            foreach (var suporte in suportes)
            {
                await InserirNaBase(new Suporte()
                {
                    Nome = suporte.Nome,
                    Tipo = suporte.Tipo
                });
            }
        }
        
        private async Task InserirConservacoesBase()
        {
            var conservacaos = new List<Conservacao>
            {
                new() { Nome = ConstantesTestes.OTIMO },
                new() { Nome = ConstantesTestes.BOM },
                new() { Nome = ConstantesTestes.REGULAR },
                new() { Nome = ConstantesTestes.RUIM },
            };

            foreach (var conservacao in conservacaos)
            {
                await InserirNaBase(new Conservacao()
                {
                    Nome = conservacao.Nome,
                });
            }
        }
        
        private async Task InserirCromiasBase()
        {
            var cromias = new List<Cromia>
            {
                new() { Nome = ConstantesTestes.COLOR },
                new() { Nome = ConstantesTestes.PB },
            };

            foreach (var cromia in cromias)
            {
                await InserirNaBase(new Cromia()
                {
                    Nome = cromia.Nome,
                });
            }
        }

        private async Task InserirAcessoDocumentos()
        {
            var acessosDocumentos = new List<AcessoDocumento>
            {
                new() { Nome = ConstantesTestes.DIGITAL },
                new() { Nome = ConstantesTestes.FISICO },
                new() { Nome = ConstantesTestes.ONLINE },
            };

            foreach (var acessoDocumento in acessosDocumentos)
            {
                await InserirNaBase(new AcessoDocumento()
                {
                    Nome = acessoDocumento.Nome,
                });
            }
        }
        
        private async Task InserirCreditosAutorias()
        {
            for (int i = 0; i < 10; i++)
            {
                await InserirNaBase(new CreditoAutor()
                {
                    Nome = $"{faker.Person.UserName.Limite(200)}-{i}", 
                    Tipo = TipoCreditoAutoria.Autoria, 
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), 
                    CriadoLogin = ConstantesTestes.LOGIN_123456789
                });
                
                await InserirNaBase(new CreditoAutor()
                {
                    Nome = $"{faker.Person.LastName.Limite(200)}-{i}", 
                    Tipo = TipoCreditoAutoria.Credito, 
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), 
                    CriadoLogin = ConstantesTestes.LOGIN_123456789
                });
            }
        }

        protected async Task InserirCreditosAutorias(IEnumerable<string> creditos, TipoCreditoAutoria tipoCreditoAutoria = TipoCreditoAutoria.Credito)
        {
            foreach (var credito in creditos)
                await InserirNaBase(new CreditoAutor()
                {
                    Nome = credito, Tipo = tipoCreditoAutoria, CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = ConstantesTestes.LOGIN_123456789
                });
        }

        protected async Task InserirFormatos(IEnumerable<string> formatos, TipoFormato tipoFormato)
        {
            foreach (var formato in formatos)
                await InserirNaBase(new Formato() { Nome = formato, Tipo = tipoFormato });
        }

        protected async Task InserirSuportes(IEnumerable<string> suportes, TipoSuporte tipo = TipoSuporte.IMAGEM)
        {
            foreach (var suporte in suportes)
                await InserirNaBase(new Suporte() { Nome = suporte, Tipo = tipo });
        }

        protected async Task InserirCromias(IEnumerable<string> cromias)
        {
            foreach (var cromia in cromias)
                await InserirNaBase(new Cromia() { Nome = cromia });
        }

        protected async Task InserirConservacoes(IEnumerable<string> conservacoes)
        {
            foreach (var conservacao in conservacoes)
                await InserirNaBase(new Conservacao() { Nome = conservacao });
        }

        protected async Task InserirIdiomas(IEnumerable<string> idiomas)
        {
            foreach (var idioma in idiomas)
                await InserirNaBase(new Idioma() { Nome = idioma });
        }

        protected async Task InserirSeriesColecoes(IEnumerable<string> seriesColecoes)
        {
            foreach (var serieColecao in seriesColecoes)
                await InserirNaBase(new SerieColecao() { Nome = serieColecao, CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = ConstantesTestes.LOGIN_123456789 });
        }

        protected async Task InserirAssuntos(IEnumerable<string> assuntos)
        {
            foreach (var assunto in assuntos)
                await InserirNaBase(new Assunto() { Nome = assunto, CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoLogin = ConstantesTestes.LOGIN_123456789 });
        }

        protected async Task InserirEditoras(IEnumerable<string> editoras)
        {
            foreach (var editora in editoras)
                await InserirNaBase(new Suporte() { Nome = editora });
        }

        protected async Task InserirMateriais(IEnumerable<string> materiais, TipoMaterial tipoMaterial)
        {
            foreach (var material in materiais)
                await InserirNaBase(new Material() { Nome = material, Tipo = tipoMaterial });
        }
        
        protected async Task InserirAcervoTridimensional()
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
                    Largura = "50,45",
                    Altura = "10,20",
                    Diametro = "15,40",	
                    Profundidade = "18,01",	
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
