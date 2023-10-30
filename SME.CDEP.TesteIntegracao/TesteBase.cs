using AutoMapper;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
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

        // protected UsuarioDTO ObterUsuarioDto(TipoUsuario tipoUsuario, string numero)
        // {
        //     var retorno = new UsuarioDTO()
        //     {
        //         Login = $"{faker.Person.FirstName}_{numero}",
        //         Nome = faker.Person.FullName,
        //         Endereco = faker.Address.FullAddress(),
        //         Numero = int.Parse(faker.Address.BuildingNumber()),
        //         Complemento = faker.Address.StreetSuffix(),
        //         Cep = faker.Address.ZipCode(),
        //         Cidade = faker.Address.City(),
        //         Estado = faker.Address.StateAbbr(),
        //         Telefone = faker.Phone.PhoneNumber("(##) #####-####"),
        //         Bairro = faker.Address.County(),
        //         TipoUsuario = (int)tipoUsuario
        //     };
        //     return retorno;
        // }

        protected async Task InserirDadosBasicos()
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
            }
        }
        
        protected static Faker<Acervo> GerarAcervo(TipoAcervo tipoAcervo)
        {
            var random = new Random();
            var faker = new Faker<Acervo>("pt_BR");
            faker.RuleFor(x => x.Titulo, f => f.Lorem.Text().Limite(500));
            faker.RuleFor(x => x.Descricao, f => f.Lorem.Text());
            faker.RuleFor(x => x.Codigo, f => random.Next(1,499).ToString());
            
            if (((long)tipoAcervo).EhAcervoDocumental())
                faker.RuleFor(x => x.CodigoNovo, f => random.Next(500,999).ToString());
            
            faker.RuleFor(x => x.SubTitulo, f => f.Lorem.Text().Limite(500));
            faker.RuleFor(x => x.TipoAcervoId, f => (int)tipoAcervo);
            AuditoriaFaker(faker);
            return faker;
        }
        
        protected Faker<AcervoArteGrafica> GerarAcervoArteGrafica()
        {
            var random = new Random();
            var faker = new Faker<AcervoArteGrafica>("pt_BR");
            
            faker.RuleFor(x => x.Localizacao, f => f.Lorem.Text().Limite(100));
            faker.RuleFor(x => x.Procedencia, f => f.Lorem.Text().Limite(200));
            faker.RuleFor(x => x.DataAcervo, f => f.Date.Recent().Year.ToString());
            faker.RuleFor(x => x.CopiaDigital, f => true);
            faker.RuleFor(x => x.PermiteUsoImagem, f => true);
            faker.RuleFor(x => x.Largura, f => random.Next(15,55));
            faker.RuleFor(x => x.Altura, f => random.Next(15,55));
            faker.RuleFor(x => x.ConservacaoId, f => random.Next(1,5));
            faker.RuleFor(x => x.CromiaId, f => random.Next(1,5));
            faker.RuleFor(x => x.Diametro, f => random.Next(15,55));
            faker.RuleFor(x => x.Tecnica, f => f.Lorem.Sentence().Limite(100));
            faker.RuleFor(x => x.SuporteId, f => random.Next(1,5));
            faker.RuleFor(x => x.Quantidade, f => random.Next(15,55));
            faker.RuleFor(x => x.Acervo, f => GerarAcervo(TipoAcervo.ArtesGraficas).Generate());
            return faker;
        }
        
        protected Faker<AcervoArteGraficaCadastroDTO> GerarAcervoArteGraficaCadastroDTO()
        {
            var random = new Random();
            var faker = new Faker<AcervoArteGraficaCadastroDTO>("pt_BR");
            
            faker.RuleFor(x => x.Codigo, f => random.Next(1,499).ToString());
            faker.RuleFor(x => x.Titulo, f => f.Lorem.Text().Limite(500));
            faker.RuleFor(x => x.Descricao, f => f.Lorem.Text());
            faker.RuleFor(x => x.SubTitulo, f => f.Lorem.Text().Limite(500));
            faker.RuleFor(x => x.Localizacao, f => f.Lorem.Text().Limite(100));
            faker.RuleFor(x => x.Procedencia, f => f.Lorem.Text().Limite(200));
            faker.RuleFor(x => x.DataAcervo, f => f.Date.Recent().Year.ToString());
            faker.RuleFor(x => x.CopiaDigital, f => true);
            faker.RuleFor(x => x.PermiteUsoImagem, f => true);
            faker.RuleFor(x => x.Largura, f => random.Next(15,55));
            faker.RuleFor(x => x.Altura, f => random.Next(15,55));
            faker.RuleFor(x => x.ConservacaoId, f => random.Next(1,5));
            faker.RuleFor(x => x.CromiaId, f => random.Next(1,5));
            faker.RuleFor(x => x.Diametro, f => random.Next(15,55));
            faker.RuleFor(x => x.Tecnica, f => f.Lorem.Sentence().Limite(100));
            faker.RuleFor(x => x.SuporteId, f => random.Next(1,5));
            faker.RuleFor(x => x.Quantidade, f => random.Next(15,55));
            faker.RuleFor(x => x.CreditosAutoresIds, f => new long[]{1,2,3,4,5});
            faker.RuleFor(x => x.Arquivos, f => new long[]{random.Next(1,10),random.Next(1,10),random.Next(1,10),random.Next(1,10),random.Next(1,10)});
            return faker;
        }
        
        protected Faker<Arquivo> GerarArquivo(TipoArquivo tipoArquivo)
        {
            var faker = new Faker<Arquivo>("pt_BR");
            
            faker.RuleFor(x => x.Codigo, f => Guid.NewGuid());
            faker.RuleFor(x => x.Nome, f => f.Lorem.Text().Limite(20));
            faker.RuleFor(x => x.Tipo, f => tipoArquivo);
            faker.RuleFor(x => x.TipoConteudo, f => "image/jpeg");
            AuditoriaFaker(faker);
            return faker;
        }
        
        protected Faker<UsuarioDTO> GerarUsuarioDTO(TipoUsuario tipoUsuario)
        {
            var faker = new Faker<UsuarioDTO>("pt_BR");
            
            faker.RuleFor(x => x.Login, f => f.Person.FirstName.Limite(45));
            faker.RuleFor(x => x.Nome, f => f.Lorem.Text().Limite(100));
            faker.RuleFor(x => x.Endereco, f => f.Address.FullAddress().Limite(200));
            faker.RuleFor(x => x.Numero, f => int.Parse(f.Address.BuildingNumber().Limite(4)));
            faker.RuleFor(x => x.Complemento, f => f.Address.StreetSuffix().Limite(20));
            faker.RuleFor(x => x.Cep, f => f.Address.ZipCode());
            faker.RuleFor(x => x.Cidade, f => f.Address.City());
            faker.RuleFor(x => x.Estado, f => f.Address.StateAbbr());
            faker.RuleFor(x => x.Telefone, f => f.Phone.PhoneNumber("(##) #####-####"));
            faker.RuleFor(x => x.Bairro, f => f.Address.County().Limite(200));
            faker.RuleFor(x => x.TipoUsuario, f => (int)tipoUsuario);
            return faker;
        }
    }
}
