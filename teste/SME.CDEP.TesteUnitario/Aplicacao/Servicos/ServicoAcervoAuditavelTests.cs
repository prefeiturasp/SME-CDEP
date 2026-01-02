using AutoMapper;
using Bogus;
using Microsoft.Extensions.Options;
using Minio.DataModel;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Dtos;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;
using System.Reflection;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoAcervoAuditavelTests
    {
        private readonly AutoMocker _mocker;
        private readonly ServicoAcervoAuditavel _servico;
        private readonly Faker _faker;

        public ServicoAcervoAuditavelTests()
        {
            _mocker = new AutoMocker();
            _faker = new Faker("pt_BR");

            // Configuração padrão de Options
            var options = Options.Create(new ConfiguracaoArmazenamentoOptions
            {
                BucketArquivos = "bucket-teste",
                TipoRequisicao = "http://localhost",
                Port = 9000,
            });
            _mocker.Use(options);

            // Mock padrão de contexto para evitar NullReference em logs de auditoria
            var mockContexto = _mocker.GetMock<IContextoAplicacao>();
            mockContexto.Setup(c => c.NomeUsuario).Returns("UsuarioTeste");
            mockContexto.Setup(c => c.UsuarioLogado).Returns("login.teste");
            // Guid aleatório para simular um perfil
            mockContexto.Setup(c => c.PerfilUsuario).Returns(Guid.NewGuid().ToString());

            _servico = _mocker.CreateInstance<ServicoAcervoAuditavel>();
        }

        [Fact]
        public async Task DadoAcervoValido_QuandoInserir_EntaoDeveRetornarId()
        {
            // Arrange
            var acervo = new Faker<Acervo>()
                .RuleFor(a => a.Codigo, f => f.Random.AlphaNumeric(10))
                .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.Bibliografico)
                .RuleFor(a => a.Ano, "[2023]") // Formato ABNT válido
                .RuleFor(a => a.CreditosAutoresIds, f => new long[] { 1, 2 })
                .RuleFor(a => a.CoAutores, f => new List<CoAutor> { new CoAutor { CreditoAutorId = 3, TipoAutoria = "Colaborador" } })
                .Generate();

            _mocker.GetMock<IRepositorioAcervo>()
                .Setup(r => r.ExisteCodigo(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<TipoAcervo>()))
                .ReturnsAsync(false);

            _mocker.GetMock<IRepositorioAcervo>()
                .Setup(r => r.Inserir(It.IsAny<Acervo>()))
                .ReturnsAsync(100);

            // Act
            var resultado = await _servico.Inserir(acervo);

            // Assert
            Assert.Equal(100, resultado);

            _mocker.GetMock<IRepositorioAcervo>().Verify(r => r.Inserir(It.IsAny<Acervo>()), Times.Once);

            // Verifica inserção de autores e co-autores
            _mocker.GetMock<IRepositorioAcervoCreditoAutor>()
                .Verify(r => r.Inserir(It.IsAny<AcervoCreditoAutor>()), Times.AtLeast(3)); // 2 Autores + 1 CoAutor
        }

        [Fact]
        public async Task DadoAcervoComAnoFuturo_QuandoInserir_EntaoDeveLancarNegocioException()
        {
            // Arrange
            var anoFuturo = DateTime.Now.Year + 1;
            var acervo = new Faker<Acervo>()
                .RuleFor(a => a.Codigo, f => f.Random.AlphaNumeric(10))
                .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.Bibliografico)
                .RuleFor(a => a.Ano, $"[{anoFuturo}]")
                .Generate();

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _servico.Inserir(acervo));
            Assert.Equal(MensagemNegocio.NAO_PERMITIDO_ANO_FUTURO, excecao.Message);
        }

        [Fact]
        public async Task DadoAcervoComCodigoDuplicado_QuandoInserir_EntaoDeveLancarNegocioException()
        {
            // Arrange
            var acervo = new Faker<Acervo>()
                .RuleFor(a => a.Codigo, "COD123")
                .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.Bibliografico)
                .RuleFor(a => a.Ano, "[2020]")
                .Generate();

            _mocker.GetMock<IRepositorioAcervo>()
                .Setup(r => r.ExisteCodigo("COD123", It.IsAny<long>(), It.IsAny<TipoAcervo>()))
                .ReturnsAsync(true);

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _servico.Inserir(acervo));
            Assert.Contains("duplicado", excecao.Message);
        }

        [Fact]
        public async Task DadoAcervoNaoDocumentalComCodigoNovo_QuandoInserir_EntaoDeveLancarNegocioException()
        {
            // Arrange
            var acervo = new Faker<Acervo>()
                .RuleFor(a => a.Codigo, "CODANTIGO")
                .RuleFor(a => a.CodigoNovo, "CODNOVO") // Preenchido
                .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.Bibliografico) // Não é Documental
                .RuleFor(a => a.Ano, "[2020]")
                .Generate();

            _mocker.GetMock<IRepositorioAcervo>()
                .Setup(r => r.ExisteCodigo(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<TipoAcervo>()))
                .ReturnsAsync(false);

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _servico.Inserir(acervo));
            Assert.Equal(MensagemNegocio.SOMENTE_ACERVO_DOCUMENTAL_POSSUI_CODIGO_NOVO, excecao.Message);
        }

        [Fact]
        public async Task DadoCodigosNaoPreenchidos_QuandoInserir_EntaoDeveLancarNegocioException()
        {
            // Arrange
            var acervo = new Faker<Acervo>()
                .RuleFor(a => a.Codigo, "") // Vazio
                .RuleFor(a => a.CodigoNovo, "") // Vazio
                .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.Bibliografico)
                .RuleFor(a => a.Ano, "[2020]")
                .Generate();

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.Inserir(acervo));
        }

        [Fact]
        public async Task DadoCodigoIgualCodigoNovo_QuandoInserir_EntaoDeveLancarNegocioException()
        {
            // Arrange
            var acervo = new Faker<Acervo>()
                .RuleFor(a => a.Codigo, "CODIGO123")
                .RuleFor(a => a.CodigoNovo, "CODIGO123") // Igual ao Código
                .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.DocumentacaoTextual)
                .RuleFor(a => a.Ano, "[2020]")
                .Generate();
            _mocker.GetMock<IRepositorioAcervo>()
                .Setup(r => r.ExisteCodigo(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<TipoAcervo>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.Inserir(acervo));
        }

        [Fact]
        public async Task DadoUmAnoForaDoFormatoAbnt_QuandoInserir_EntaoDeveLancarNegocioException()
        {
            // Arrange
            var acervo = new Faker<Acervo>()
                .RuleFor(a => a.Codigo, f => f.Random.AlphaNumeric(10))
                .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.Bibliografico)
                .RuleFor(a => a.Ano, "202A") // Formato inválido
                .Generate();
            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _servico.Inserir(acervo));
            Assert.Equal(MensagemNegocio.O_ANO_NAO_ESTA_SEGUINDO_FORMATO_ABNT, excecao.Message);
        }

        [Fact]
        public async Task DadoAcervosMisturadosEntreAtivosEExcluidos_QuandoObterTodos_EntaoDeveRetornarApenasAtivos()
        {
            // Arrange
            var listaAcervos = new List<Acervo>
            {
                new() { Id = 1, Codigo = "AC001", Excluido = false },
                new() { Id = 2, Codigo = "AC002", Excluido = true },
                new() { Id = 3, Codigo = "AC003", Excluido = false },
            };

            var listaAcervosDto = new List<AcervoDTO>
            {
                new() { Id = 1, Codigo = "AC001" },
                new() { Id = 3, Codigo = "AC003" },
            };

            _mocker.GetMock<IRepositorioAcervo>()
                .Setup(r => r.ObterTodos())
                .ReturnsAsync(listaAcervos);

            var mapper = _mocker.GetMock<IMapper>();
            mapper.Setup(m => m.Map<IEnumerable<AcervoDTO>>(It.IsAny<IEnumerable<Acervo>>()))
                  .Returns(listaAcervosDto);

            // Act
            var resultado = await _servico.ObterTodos();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.DoesNotContain(resultado, a => a.Codigo == "AC002");
        }

        [Fact]
        public async Task DadoUmAnoForaDoPadraoAbnt_QuandoAlterarCreditoAutor_EntaoDeveLancarNegocioException()
        {
            // Arrange
            var acervo = new Faker<Acervo>()
                .RuleFor(a => a.Id, 1)
                .RuleFor(a => a.Codigo, f => f.Random.AlphaNumeric(10))
                .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.Bibliografico)
                .RuleFor(a => a.Ano, "20X0") // Formato inválido
                .Generate();
            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _servico.AlterarCreditoAutor(acervo));
            Assert.Equal(MensagemNegocio.O_ANO_NAO_ESTA_SEGUINDO_FORMATO_ABNT, excecao.Message);
        }

        [Fact]
        public async Task DadoUmCodigoNovoParaAcervoNaoDocumental_QuandoAlterarCreditoAutor_EntaoDeveLancarNegocioException()
        {
            // Arrange
            var acervo = new Faker<Acervo>()
                .RuleFor(a => a.Id, 1)
                .RuleFor(a => a.Codigo, f => f.Random.AlphaNumeric(10))
                .RuleFor(a => a.CodigoNovo, f => f.Random.AlphaNumeric(10)) // Preenchido
                .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.Bibliografico) // Não é Documental
                .RuleFor(a => a.Ano, "[2020]")
                .Generate();
            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _servico.AlterarCreditoAutor(acervo));
            Assert.Equal(MensagemNegocio.SOMENTE_ACERVO_DOCUMENTAL_POSSUI_CODIGO_NOVO, excecao.Message);
        }

        [Fact]
        public async Task DadoUmAcervoValido_QuandoAlterarCreditoAutor_EntaoDeveAlterarERetornarVerdadeiro()
        {
            // Arrange
            var acervo = new Faker<Acervo>()
                .RuleFor(a => a.Id, 1)
                .RuleFor(a => a.Codigo, f => f.Random.AlphaNumeric(10))
                .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.Bibliografico)
                .RuleFor(a => a.Ano, "[2020]")
                .RuleFor(a => a.CreditosAutoresIds, f => [1, 2])
                .RuleFor(a => a.CoAutores, f => [new() { CreditoAutorId = 3, TipoAutoria = "Colaborador" }])
                .Generate();
            _mocker.GetMock<IRepositorioAcervo>()
                .Setup(r => r.ExisteCodigo(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<TipoAcervo>()))
                .ReturnsAsync(false);
            _mocker.GetMock<IRepositorioAcervoCreditoAutor>()
                .Setup(r => r.ObterPorAcervoId(It.IsAny<long>()))
                .ReturnsAsync([]);
            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<AcervoDTO>(It.IsAny<Acervo>()))
                .Returns(new AcervoDTO());

            // Act
            var resultado = await _servico.AlterarCreditoAutor(acervo);

            // Assert
            _mocker.GetMock<IRepositorioAcervoCreditoAutor>()
                .Verify(r => r.Inserir(It.IsAny<AcervoCreditoAutor>()), Times.AtLeast(2));
            _mocker.GetMock<IRepositorioAcervoCreditoAutor>()
                .Verify(r => r.Excluir(It.IsAny<long[]>(), It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async Task DadoFiltroTextoLivre_QuandoObterPorTextoLivreETipoAcervo_EntaoDeveRetornarPaginacaoComItens()
        {
            // Arrange
            var filtro = new FiltroTextoLivreTipoAcervoDTO { TextoLivre = "História", TipoAcervo = TipoAcervo.Bibliografico };
            var codigoAcervo = Guid.NewGuid();

            // Mock da Paginação (Contexto)
            var mockContexto = _mocker.GetMock<IContextoAplicacao>();
            mockContexto.Setup(c => c.ObterVariavel<string>("NumeroPagina")).Returns("1");
            mockContexto.Setup(c => c.ObterVariavel<string>("NumeroRegistros")).Returns("10");

            var listaAcervos = new Faker<PesquisaAcervo>()
                .RuleFor(p => p.AcervoId, f => f.IndexFaker + 1)
                .RuleFor(p => p.Titulo, f => f.Lorem.Sentence())
                .RuleFor(p => p.Tipo, TipoAcervo.Bibliografico)
                .RuleFor(p => p.SituacaoSaldo, SituacaoSaldo.DISPONIVEL)
                .Generate(2);

            _mocker.GetMock<IRepositorioAcervo>()
                .Setup(r => r.ObterPorTextoLivreETipoAcervo(It.IsAny<string>(), It.IsAny<TipoAcervo?>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(listaAcervos);

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterAcervoCodigoNomeArquivoPorAcervoId(It.IsAny<long[]>()))
                .ReturnsAsync(new List<AcervoArquivoCodigoNomeResumido>()); // Sem imagens específicas

            _mocker.GetMock<IRepositorioArquivo>()
                .Setup(r => r.ObterArquivoPorNomeTipoArquivo(It.IsAny<string>(), TipoArquivo.Sistema))
                .ReturnsAsync(new Arquivo { Nome = "padrao.jpg", Codigo = codigoAcervo });

            // Act
            var resultado = await _servico.ObterPorTextoLivreETipoAcervo(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.TotalRegistros);
            Assert.NotEmpty(resultado.Items);

            _mocker.GetMock<IServicoHistoricoConsultaAcervo>()
                .Verify(s => s.InserirAsync(It.IsAny<HistoricoConsultaAcervoDto>()), Times.Once);
        }

        [Fact]
        public async Task DadoIdExistente_QuandoExcluir_EntaoDeveRetornarVerdadeiro()
        {
            // Arrange
            long idParaExcluir = 50;

            // Act
            var resultado = await _servico.Excluir(idParaExcluir);

            // Assert
            Assert.True(resultado);
            _mocker.GetMock<IRepositorioAcervo>().Verify(r => r.Remover(idParaExcluir), Times.Once);
        }

        [Fact]
        public async Task DadoCodigoInexistente_QuandoObterDetalhamento_EntaoDeveLancarNegocioException()
        {
            // Arrange
            var filtro = new FiltroDetalharAcervoDTO { Codigo = "INEXISTENTE", Tipo = TipoAcervo.Bibliografico };

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _servico.ObterDetalhamentoPorTipoAcervoECodigo(filtro));
            Assert.Equal(MensagemNegocio.ACERVO_NAO_ENCONTRADO, excecao.Message);
        }

        [Fact]
        public async Task DadoUmCodigoNovoParaAcervoNaoDocumental_QuandoAlterar_EntaoDeveLancarNegocioException()
        {
            // Arrange
            var acervo = new Faker<Acervo>()
                .RuleFor(a => a.Id, 1)
                .RuleFor(a => a.Codigo, f => f.Random.AlphaNumeric(10))
                .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.Bibliografico) // Não é Documental
                .RuleFor(a => a.Ano, "[2020]")
                .Generate();
            _mocker.GetMock<IRepositorioAcervo>()
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(acervo);
            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _servico.Alterar(new() { Id = 1, CodigoNovo = _faker.Random.AlphaNumeric(10) }));
            Assert.Equal(MensagemNegocio.SOMENTE_ACERVO_DOCUMENTAL_POSSUI_CODIGO_NOVO, excecao.Message);
        }

        [Fact]
        public async Task DadoAcervoValido_QuandoAlterar_EntaoDeveAlterarERetornarVerdadeiro()
        {
            // Arrange
            var acervo = new Faker<Acervo>()
                .RuleFor(a => a.Id, 1)
                .RuleFor(a => a.Codigo, f => f.Random.AlphaNumeric(10))
                .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.Bibliografico)
                .RuleFor(a => a.Ano, "[2020]")
                .RuleFor(a => a.CreditosAutoresIds, f => [1, 2])
                .RuleFor(a => a.CoAutores, f => [new() { CreditoAutorId = 3, TipoAutoria = "Colaborador" }])
                .Generate();
            _mocker.GetMock<IRepositorioAcervo>()
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(acervo);
            _mocker.GetMock<IRepositorioAcervo>()
                .Setup(r => r.ExisteCodigo(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<TipoAcervo>()))
                .ReturnsAsync(false);
            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<Acervo>(It.IsAny<AcervoDTO>()))
                .Returns(acervo);
            _mocker.GetMock<IRepositorioAcervo>()
                .Setup(r => r.ExisteCodigo(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<TipoAcervo>()))
                .ReturnsAsync(false);
            _mocker.GetMock<IRepositorioAcervoCreditoAutor>()
                .Setup(r => r.ObterPorAcervoId(It.IsAny<long>()))
                .ReturnsAsync([]);
            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<AcervoDTO>(It.IsAny<Acervo>()))
                .Returns(new AcervoDTO());
            // Act
            var resultado = await _servico.Alterar(new()
            {
                Id = 1,
                Codigo = acervo.Codigo,
                TipoAcervoId = acervo.TipoAcervoId,
                Ano = acervo.Ano
            });

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task DadoIdExistente_QuandoObterPorId_EntaoDeveRetornarAcervoDTO()
        {
            // Arrange
            var acervo = new Faker<Acervo>()
                .RuleFor(a => a.Id, 1)
                .RuleFor(a => a.Codigo, f => f.Random.AlphaNumeric(10))
                .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.Bibliografico)
                .RuleFor(a => a.Ano, "[2020]")
                .Generate();
            _mocker.GetMock<IRepositorioAcervo>()
                .Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(acervo);
            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<AcervoDTO>(It.IsAny<Acervo>()))
                .Returns(new AcervoDTO { Id = acervo.Id, Codigo = acervo.Codigo });
            // Act
            var resultado = await _servico.ObterPorId(acervo.Id);
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(acervo.Id, resultado.Id);
            Assert.Equal(acervo.Codigo, resultado.Codigo);
        }

        [Fact]
        public async Task DadoFiltroSemResultados_QuandoObterPorFiltro_EntaoDeverRetornarListaVaziaSemConsultarBanco()
        {
            // Arrange
            var mockRepositorio = _mocker.GetMock<IRepositorioAcervo>();

            // Act
            var resultado = await _servico.ObterPorFiltro(It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<string>(), It.IsAny<int?>());

            // Assert
            Assert.NotNull(resultado);
            mockRepositorio.Verify(r => r.PesquisarPorFiltroPaginado(It.IsAny<AcervoFiltroDto>(), It.IsAny<PaginacaoDto>()), Times.Never);
        }

        [Fact]
        public async Task DadoAcervoDocumentalComCapa_QuandoObterPorFiltro_EntaoDeveRetornarImagemBase64()
        {
            // Arrange
            var codigoAcervo = Guid.NewGuid();
            var listaAcervos = new List<Acervo>
            {
                new Faker<Acervo>()
                    .RuleFor(a => a.Id, 1)
                    .RuleFor(a => a.Codigo, f => f.Random.AlphaNumeric(10))
                    .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.DocumentacaoTextual)
                    .RuleFor(a => a.CapaDocumento, f => f.System.FileName())
                    .RuleFor(a => a.Ano, "[2020]")
                    .Generate()
            }; 
            var objectStatSimulado = CriarObjectStatSimulado("image/jpeg");
            var streamSimulado = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("conteudo_imagem"));
            var repositorioAcervoMock = _mocker.GetMock<IRepositorioAcervo>();
            repositorioAcervoMock
                .Setup(r => r.ContarPorFiltro(It.IsAny<AcervoFiltroDto>()))
                .ReturnsAsync(1);
            repositorioAcervoMock
                .Setup(r => r.PesquisarPorFiltroPaginado(It.IsAny<AcervoFiltroDto>(), It.IsAny<PaginacaoDto>()))
                .ReturnsAsync(listaAcervos);
            _mocker.GetMock<IServicoArmazenamento>()
                .Setup(r => r.ObterMetadadosObjeto(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(objectStatSimulado);
            _mocker.GetMock<IServicoArmazenamento>()
                .Setup(r => r.ObterStream(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(streamSimulado);

            // Act
            var resultado = await _servico.ObterPorFiltro(It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<string>(), It.IsAny<int?>());

            // Assert
            var itemRetornado = resultado.Items.First();
            Assert.NotNull(itemRetornado.CapaDocumento);
            Assert.StartsWith("data:image/jpeg;base64,", itemRetornado.CapaDocumento);
        }

        private ObjectStat CriarObjectStatSimulado(string contentType)
        {
            // 1. Instancia o objeto sem passar pelo construtor (bypassa a proteção private)
            var obj = (ObjectStat)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(ObjectStat));

            // 2. Tenta definir a propriedade via Reflection (mesmo que o set seja private)
            var propertyInfo = typeof(ObjectStat).GetProperty(nameof(ObjectStat.ContentType),
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic);

            if (propertyInfo != null)
            {
                propertyInfo.SetValue(obj, contentType);
            }
            else
            {
                // Caso a propriedade não tenha 'set', tentamos preencher o Backing Field (variável privada interna)
                // O padrão do compilador C# para auto-properties é <NomePropriedade>k__BackingField
                var fieldInfo = typeof(ObjectStat).GetField($"<{nameof(ObjectStat.ContentType)}>k__BackingField",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic);

                fieldInfo?.SetValue(obj, contentType);
            }

            return obj;
        }
    }
}