using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Constantes;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.Webapi.Contexto;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace SME.CDEP.TesteIntegracao
{
    [Collection("TesteIntegradoCDEP")]
    public class TesteBase : IClassFixture<TestFixture>
    {
        protected readonly CollectionFixture _collectionFixture;

        public ServiceProvider ServiceProvider => _collectionFixture.ServiceProvider;

        public TesteBase(CollectionFixture collectionFixture)
        {
            _collectionFixture = collectionFixture;
            _collectionFixture.Database.LimparBase();
            _collectionFixture.IniciarServicos();

            RegistrarFakes(_collectionFixture.Services);
            _collectionFixture.BuildServiceProvider();
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
        
        protected IServicoAcervoDocumental GetServicoAcervoDocumental()
        {
            return ObterServicoAplicacao<IServicoAcervoDocumental>();
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

        protected static UsuarioDTO ObterUsuarioDto(TipoUsuario tipoUsuario, string numero)
        {
            var retorno = new UsuarioDTO()
            {
                Login = $"9999999999{numero}",
                Nome = $"Usuário 9999999999{numero}'",
                Endereco = $"Rua 9999999999{numero}'",
                Numero = int.Parse($"9{numero}"),
                Complemento = $"Casa 9{numero}'",
                Cep = $"8805899{numero}'",
                Cidade = $"Cidade 9999999999{numero}'",
                Estado = ConstantesTestes.ESTADO_SC,
                Telefone = $"99_99999_999{numero}'",
                Bairro = $"Bairro 9999999999{numero}'",
                TipoUsuario = (int)tipoUsuario
            };
            return retorno;
        }

        protected async Task InserirDadosBasicos()
        {
            var random = new Random();
            
            for (int i = 1; i <= 5; i++)
            {
                await InserirNaBase(new CreditoAutor()
                {
                    Nome = string.Format(ConstantesTestes.CREDITO_AUTOR_X,i),
                    CriadoPor = ConstantesTestes.SISTEMA, 
                    CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                    CriadoLogin = ConstantesTestes.LOGIN_123456789 
                });
                
                await InserirNaBase(new Suporte()
                {
                    Nome = string.Format(ConstantesTestes.SUPORTE_X,i),
                    Tipo = (TipoSuporte)random.Next(1,2),
                });
                
                await InserirNaBase(new Formato()
                {
                    Nome = string.Format(ConstantesTestes.FORMATO_X,i),
                    Tipo = (TipoFormato)random.Next(1,2),
                });
                
                await InserirNaBase(new Cromia() { Nome = string.Format(ConstantesTestes.CROMIA_X,i)});
                
                await InserirNaBase(new Conservacao() { Nome = string.Format(ConstantesTestes.CONSERVACAO_X,i)});
            }
        }
    }
}
