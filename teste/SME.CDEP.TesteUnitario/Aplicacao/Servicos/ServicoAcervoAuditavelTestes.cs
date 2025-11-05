using AutoMapper;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Minio.DataModel;
using Moq;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Dtos;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento;
using SME.CDEP.Infra.Servicos.ServicoArmazenamento.Interface;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoAcervoAuditavelTestes
    {
        private readonly Mock<IRepositorioAcervo> _repositorioAcervoMock;
        private readonly Mock<IRepositorioAcervoCreditoAutor> _repositorioAcervoCreditoAutorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IContextoAplicacao> _contextoAplicacaoMock;
        private readonly Mock<IRepositorioArquivo> _repositorioArquivoMock;
        private readonly Mock<IRepositorioAcervoBibliografico> _repositorioAcervoBibliograficoMock;
        private readonly Mock<IRepositorioParametroSistema> _repositorioParametroSistemaMock;
        private readonly Mock<IServicoArmazenamento> _servicoArmazenamentoMock;
        private readonly Mock<IRepositorioAcervoDocumental> _repositorioAcervoDocumentalMock;
        private readonly Mock<IOptions<ConfiguracaoArmazenamentoOptions>> _optionsMock;

        private readonly ServicoAcervoAuditavel _servico;

        public ServicoAcervoAuditavelTestes()
        {
            _repositorioAcervoMock = new Mock<IRepositorioAcervo>();
            _repositorioAcervoCreditoAutorMock = new Mock<IRepositorioAcervoCreditoAutor>();
            _mapperMock = new Mock<IMapper>();
            _contextoAplicacaoMock = new Mock<IContextoAplicacao>();
            _repositorioArquivoMock = new Mock<IRepositorioArquivo>();
            _repositorioAcervoBibliograficoMock = new Mock<IRepositorioAcervoBibliografico>();
            _repositorioParametroSistemaMock = new Mock<IRepositorioParametroSistema>();
            _servicoArmazenamentoMock = new Mock<IServicoArmazenamento>();
            _repositorioAcervoDocumentalMock = new Mock<IRepositorioAcervoDocumental>();

            var configOptions = new ConfiguracaoArmazenamentoOptions { EndPoint = "localhost/arquivos", TipoRequisicao = "http" };
            _optionsMock = new Mock<IOptions<ConfiguracaoArmazenamentoOptions>>();
            _optionsMock.Setup(o => o.Value).Returns(configOptions);

            _contextoAplicacaoMock.Setup(c => c.UsuarioLogado).Returns("usuario.teste");
            _contextoAplicacaoMock.Setup(c => c.NomeUsuario).Returns("Usuário de Teste");

            _servico = new ServicoAcervoAuditavel(
                _repositorioAcervoMock.Object,
                _mapperMock.Object,
                _contextoAplicacaoMock.Object,
                _repositorioAcervoCreditoAutorMock.Object,
                _repositorioArquivoMock.Object,
                _repositorioAcervoBibliograficoMock.Object,
                _repositorioAcervoDocumentalMock.Object,
                Mock.Of<IRepositorioAcervoArteGrafica>(),
                Mock.Of<IRepositorioAcervoAudiovisual>(),
                Mock.Of<IRepositorioAcervoFotografico>(),
                Mock.Of<IRepositorioAcervoTridimensional>(),
                _repositorioParametroSistemaMock.Object,
                _optionsMock.Object,
                _servicoArmazenamentoMock.Object
            );
        }

        [Fact]
        public async Task Inserir_Com_Acervo_Valido_Deve_Salvar_Repositorio_E_Autores()
        {
            var acervo = new Acervo
            {
                Codigo = "TMB-001",
                Ano = "2024",
                TipoAcervoId = (long)TipoAcervo.Bibliografico,
                CreditosAutoresIds = new long[] { 1, 2 },
                CoAutores = new List<CoAutor> { new CoAutor { CreditoAutorId = 3, TipoAutoria = "Ilustrador" } }
            };

            _repositorioAcervoMock.Setup(r => r.ExisteCodigo(acervo.Codigo, 0, It.IsAny<TipoAcervo>())).ReturnsAsync(false);
            _repositorioAcervoMock.Setup(r => r.Inserir(acervo)).ReturnsAsync(99);

            var resultadoId = await _servico.Inserir(acervo);

            resultadoId.Should().Be(99);
            _repositorioAcervoMock.Verify(r => r.Inserir(It.Is<Acervo>(a => a.CriadoLogin == "usuario.teste")), Times.Once);
            _repositorioAcervoCreditoAutorMock.Verify(r => r.Inserir(It.IsAny<AcervoCreditoAutor>()), Times.Exactly(3));
        }

        [Fact]
        public async Task Inserir_Com_Codigo_Duplicado_Deve_Lancar_Negocio_Exception()
        {
            var acervo = new Acervo { Codigo = "TMB-001", Ano = "2024" };
            _repositorioAcervoMock.Setup(r => r.ExisteCodigo(acervo.Codigo, 0, It.IsAny<TipoAcervo>())).ReturnsAsync(true);

            Func<Task> acao = async () => await _servico.Inserir(acervo);

            await acao.Should().ThrowAsync<NegocioException>().WithMessage("Registro 'Tombo' duplicado");
        }

        [Fact]
        public async Task Inserir_Com_Ano_Futuro_Deve_Lancar_Negocio_Exception()
        {
            var acervo = new Acervo { Codigo = "TMB-001", Ano = (DateTime.Now.Year + 1).ToString() };

            Func<Task> acao = async () => await _servico.Inserir(acervo);

            await acao.Should().ThrowAsync<NegocioException>().WithMessage("O campo ano não admite anos futuros. Apenas anos atuais e anteriores são permitidos.");
        }

        [Fact]
        public async Task Alterar_Com_Dados_Validos_Deve_Atualizar_Acervo_E_Autores()
        {
            var acervo = new Acervo
            {
                Id = 1,
                Codigo = "TMB-001",
                Ano = "2023",
                TipoAcervoId = (long)TipoAcervo.Bibliografico,
                CreditosAutoresIds = new long[] { 2, 3 }, 
                CoAutores = new List<CoAutor>()
            };
            var acervoDto = new AcervoDTO
            {
                Id = acervo.Id,
                Codigo = acervo.Codigo,
                Ano = acervo.Ano,
                TipoAcervoId = acervo.TipoAcervoId,
                CreditosAutoresIds = acervo.CreditosAutoresIds
            };
            var autoresAtuais = new List<AcervoCreditoAutor>
            {
                new AcervoCreditoAutor { AcervoId = 1, CreditoAutorId = 1 },
                new AcervoCreditoAutor { AcervoId = 1, CreditoAutorId = 2 }
            };

            _repositorioAcervoMock.Setup(r => r.ObterPorId(It.IsAny<long>())).ReturnsAsync(acervo);
            _repositorioAcervoMock.Setup(r => r.ExisteCodigo(acervo.Codigo, acervo.Id, It.IsAny<TipoAcervo>())).ReturnsAsync(false);
            _repositorioAcervoCreditoAutorMock.Setup(r => r.ObterPorAcervoId(acervo.Id, false)).ReturnsAsync(autoresAtuais);
            _repositorioAcervoCreditoAutorMock.Setup(r => r.ObterPorAcervoId(acervo.Id, true)).ReturnsAsync(new List<AcervoCreditoAutor>());
            _repositorioAcervoMock.Setup(r => r.Atualizar(acervo)).ReturnsAsync(acervo);
            _mapperMock.Setup(m => m.Map<Acervo>(It.IsAny<AcervoDTO>())).Returns(acervo);
            _mapperMock.Setup(m => m.Map<AcervoDTO>(It.IsAny<Acervo>())).Returns(acervoDto);

            await _servico.Alterar(acervoDto);

            _repositorioAcervoMock.Verify(r => r.Atualizar(It.Is<Acervo>(a => a.AlteradoLogin == "usuario.teste")), Times.Once);
            _repositorioAcervoCreditoAutorMock.Verify(r => r.Inserir(It.Is<AcervoCreditoAutor>(a => a.CreditoAutorId == 3)), Times.Once);
            _repositorioAcervoCreditoAutorMock.Verify(r => r.Excluir(It.Is<long[]>(ids => ids.Contains(1L)), acervo.Id), Times.Once);
        }

        [Fact]
        public async Task Obter_Detalhamento_Para_Tipo_Bibliografico_Deve_Chamar_Repositorio_Correto()
        {
            var filtro = new FiltroDetalharAcervoDTO { Codigo = "COD-BIB", Tipo = TipoAcervo.Bibliografico };
            var detalhe = new AcervoBibliograficoDetalhe { Titulo = "Livro Teste" };
            var detalheDTO = new AcervoBibliograficoDetalheDTO
            {
                Titulo = detalhe.Titulo,
            };

            _repositorioAcervoBibliograficoMock.Setup(r => r.ObterDetalhamentoPorCodigo(It.IsAny<string>())).ReturnsAsync(detalhe);
            _repositorioArquivoMock.Setup(r => r.ObterArquivoPorNomeTipoArquivo(It.IsAny<string>(), TipoArquivo.Sistema))
                                      .ReturnsAsync(new Arquivo { Codigo = Guid.NewGuid(), Nome = "padrao.jpg" });
            _mapperMock.Setup(m => m.Map<AcervoBibliograficoDetalheDTO>(It.IsAny<AcervoBibliograficoDetalhe>())).Returns(detalheDTO);

            var resultado = await _servico.ObterDetalhamentoPorTipoAcervoECodigo(filtro);

            resultado.Should().BeOfType<AcervoBibliograficoDetalheDTO>();
            resultado.Should().BeEquivalentTo(detalheDTO);
            _repositorioAcervoBibliograficoMock.Verify(r => r.ObterDetalhamentoPorCodigo(filtro.Codigo), Times.Once);
        }

        [Fact]
        public async Task Obter_Detalhamento_Quando_Acervo_Nao_Encontrado_Deve_Lancar_Negocio_Exception()
        {
            var filtro = new FiltroDetalharAcervoDTO { Codigo = "COD-INEXISTENTE", Tipo = TipoAcervo.Bibliografico };
            _repositorioAcervoBibliograficoMock.Setup(r => r.ObterDetalhamentoPorCodigo(filtro.Codigo)).ReturnsAsync((AcervoBibliograficoDetalhe)null);

            Func<Task> acao = async () => await _servico.ObterDetalhamentoPorTipoAcervoECodigo(filtro);

            await acao.Should().ThrowAsync<NegocioException>().WithMessage("Acervo não encontrado.");
        }

        [Fact]
        public void Obter_Tipos_Permitidos_Para_Perfil_Admin_Geral_Deve_Retornar_Todos_Os_Tipos()
        {
            _contextoAplicacaoMock.Setup(c => c.PerfilUsuario).Returns("D3766FB4-D753-4398-BFB0-C357724BB0A2");
            var totalTipos = Enum.GetValues(typeof(TipoAcervo)).Length;

            var resultado = _servico.ObterTiposAcervosPermitidosDoPerfilLogado();

            resultado.Should().HaveCount(totalTipos);
        }

        [Fact]
        public void Obter_Tipos_Permitidos_Para_Perfil_Admin_Biblioteca_Deve_Retornar_Apenas_Bibliografico()
        {
            _contextoAplicacaoMock.Setup(c => c.PerfilUsuario).Returns("B82673B9-52B9-4E01-9157-E19339B7211A");

            var resultado = _servico.ObterTiposAcervosPermitidosDoPerfilLogado();

            resultado.Should().HaveCount(1);
            resultado.First().Should().Be((long)TipoAcervo.Bibliografico);
        }

        [Fact]
        public async Task Obter_Por_Filtro_Quando_Nao_Encontra_Registros_Deve_Retornar_Paginacao_Vazia()
        {
            var filtro = new AcervoFiltroDto(null, "", null, "", 0);
            _repositorioAcervoMock.Setup(r => r.ContarPorFiltro(It.IsAny<AcervoFiltroDto>()))
                                  .ReturnsAsync(0);

            var resultado = await _servico.ObterPorFiltro(filtro.TipoAcervo, filtro.Titulo, filtro.CreditoAutorId, filtro.Codigo, filtro.IdEditora);

            resultado.Should().NotBeNull();
            resultado.TotalRegistros.Should().Be(0);
            resultado.TotalPaginas.Should().Be(0);
            resultado.Items.Should().NotBeNull().And.BeEmpty();

            _repositorioAcervoMock.Verify(r => r.PesquisarPorFiltroPaginado(It.IsAny<AcervoFiltroDto>(), It.IsAny<PaginacaoDto>()), Times.Never);
        }

        [Fact]
        public async Task Obter_Por_Filtro_Quando_Encontra_Registros_Deve_Retornar_Paginacao_Com_Itens_Formatados()
        {
            var totalRegistros = 2;
            var paginacao = new Paginacao(1, 10, 0);
            var paginacaoDto = new PaginacaoDto { Pagina = 1, QuantidadeRegistros = 10 };

            var autores = new Faker<CreditoAutor>("pt_BR").Generate(2);

            var acervosDoRepo = new List<Acervo>
            {
                new Faker<Acervo>("pt_BR")
                    .RuleFor(a => a.CreditosAutores, f => autores)
                    .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.Bibliografico)
                    .RuleFor(a => a.Id, f => f.Random.Int())
                    .Generate(),
                new Faker<Acervo>("pt_BR")
                    .RuleFor(a => a.CreditosAutores, f => null) 
                    .RuleFor(a => a.TipoAcervoId, (long)TipoAcervo.Fotografico)
                    .RuleFor(a => a.Id, f => f.Random.Int())
                    .Generate(),
            };

            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroPagina")).Returns(paginacao.Pagina.ToString());
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroRegistros")).Returns(paginacao.QuantidadeRegistros.ToString());
            _mapperMock.Setup(m => m.Map<PaginacaoDto>(It.IsAny<Paginacao>())).Returns(paginacaoDto);

            _repositorioAcervoMock.Setup(r => r.ContarPorFiltro(It.IsAny<AcervoFiltroDto>()))
                                  .ReturnsAsync(totalRegistros);
            _repositorioAcervoMock.Setup(r => r.PesquisarPorFiltroPaginado(It.IsAny<AcervoFiltroDto>(), It.IsAny<PaginacaoDto>()))
                                  .ReturnsAsync(acervosDoRepo);

            var resultado = await _servico.ObterPorFiltro(null, "", null, "", 0);

            resultado.Should().NotBeNull();
            resultado.TotalRegistros.Should().Be(totalRegistros);
            resultado.Items.Should().HaveCount((int)totalRegistros);

            var itemComAutor = resultado.Items.First(i => i.AcervoId == acervosDoRepo[0].Id);
            var autoresEsperados = string.Join(", ", autores.Select(a => a.Nome));
            itemComAutor.CreditoAutoria.Should().Be(autoresEsperados);

            var itemSemAutor = resultado.Items.First(i => i.AcervoId == acervosDoRepo[1].Id);
            itemSemAutor.CreditoAutoria.Should().BeEmpty();
        }

        [Theory(DisplayName = "Deve mapear corretamente a descrição do TipoAcervo")]
        [InlineData(TipoAcervo.Bibliografico, "Bibliográfico")]
        [InlineData(TipoAcervo.DocumentacaoTextual, "Documentação textual")]
        [InlineData(TipoAcervo.ArtesGraficas, "Artes gráficas")]
        [InlineData(TipoAcervo.Audiovisual, "Audiovisual")]
        [InlineData(TipoAcervo.Fotografico, "Fotográfico")]
        [InlineData(TipoAcervo.Tridimensional, "Tridimensional")]
        public async Task ObterPorFiltro_DeveMapearCorretamenteDescricaoDoTipoAcervo(TipoAcervo tipoAcervo, string descricaoEsperada)
        {
            var acervoDoRepo = new List<Acervo>
            {
                new Faker<Acervo>().RuleFor(a => a.TipoAcervoId, (long)tipoAcervo).Generate()
            };

            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>(It.IsAny<string>())).Returns("1");
            _mapperMock.Setup(m => m.Map<PaginacaoDto>(It.IsAny<Paginacao>())).Returns(new PaginacaoDto());
            _repositorioAcervoMock.Setup(r => r.ContarPorFiltro(It.IsAny<AcervoFiltroDto>())).ReturnsAsync(1);
            _repositorioAcervoMock.Setup(r => r.PesquisarPorFiltroPaginado(It.IsAny<AcervoFiltroDto>(), It.IsAny<PaginacaoDto>()))
                                  .ReturnsAsync(acervoDoRepo);

            var resultado = await _servico.ObterPorFiltro(null, "", null, "", 0);

            resultado.Items.Should().HaveCount(1);
            var item = resultado.Items.First();
            item.TipoAcervoId.Should().Be(tipoAcervo);
            item.TipoAcervo.Should().Be(descricaoEsperada);
        }

        [Theory(DisplayName = "Deve calcular corretamente o total de páginas")]
        [InlineData(19, 10, 2)] // 19 itens, 10 por página -> 2 páginas
        [InlineData(20, 10, 2)] // 20 itens, 10 por página -> 2 páginas
        [InlineData(21, 10, 3)] // 21 itens, 10 por página -> 3 páginas
        [InlineData(5, 10, 1)]  // 5 itens, 10 por página -> 1 página
        [InlineData(0, 10, 0)]   // 0 itens, 10 por página -> 0 páginas
        public async Task Obter_Por_Filtro_Deve_Calcular_Corretamente_Total_De_Paginas(int totalRegistros, int registrosPorPagina, int paginasEsperadas)
        {
            // Arrange
            var acervosDoRepo = new List<Acervo> {new Acervo
            {
                Id = 1,
                TipoAcervoId = (long)TipoAcervo.Bibliografico,
                CreditosAutores = new List<CreditoAutor> { new CreditoAutor { Nome = "Autor Teste" } },
                Titulo = "Acervo Teste",
            } };

            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroPagina")).Returns("1");
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroRegistros")).Returns(registrosPorPagina.ToString());
            _mapperMock.Setup(m => m.Map<PaginacaoDto>(It.IsAny<Paginacao>()))
                       .Returns(new PaginacaoDto { QuantidadeRegistros = registrosPorPagina });

            _repositorioAcervoMock.Setup(r => r.ContarPorFiltro(It.IsAny<AcervoFiltroDto>()))
                                  .ReturnsAsync(totalRegistros);
            _repositorioAcervoMock.Setup(r => r.PesquisarPorFiltroPaginado(It.IsAny<AcervoFiltroDto>(), It.IsAny<PaginacaoDto>()))
                                  .ReturnsAsync(acervosDoRepo);

            var resultado = await _servico.ObterPorFiltro(null, "", null, "", 0);

            resultado.TotalPaginas.Should().Be(paginasEsperadas);
        }

        [Fact]
        public async Task ObterPorFiltro_QuandoDocumentacaoTextualECapaDocumentoVazia_DeveRetornarCapaDocumentoVazia()
        {
            // Arrange
            var filtro = new AcervoFiltroDto(null, "", null, "", null);
            var acervo = new Acervo
            {
                Id = 1,
                TipoAcervoId = (long)TipoAcervo.DocumentacaoTextual,
                CreditosAutores = new List<CreditoAutor> { new CreditoAutor { Nome = "Autor Teste" } },
                Titulo = "Acervo Teste",
                CapaDocumento = null // Capa documento vazia
            };
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroPagina")).Returns("1");
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroRegistros")).Returns("10");
            _mapperMock.Setup(m => m.Map<PaginacaoDto>(It.IsAny<Paginacao>()))
                       .Returns(new PaginacaoDto { QuantidadeRegistros = 10 });
            _repositorioAcervoMock.Setup(r => r.ContarPorFiltro(It.IsAny<AcervoFiltroDto>()))
                                  .ReturnsAsync(1);
            _repositorioAcervoMock.Setup(r => r.PesquisarPorFiltroPaginado(It.IsAny<AcervoFiltroDto>(), It.IsAny<PaginacaoDto>()))
                                  .ReturnsAsync(new List<Acervo> { acervo });
            // Act
            var resultado = await _servico.ObterPorFiltro(null, "", null, "", null);
            // Assert
            resultado.Items.Should().HaveCount(1);
            var item = resultado.Items.First();
            item.CapaDocumento.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task ObterImagemBase64_QuandoArquivoExiste_DeveRetornarStringBase64Formatada()
        {
            // Arrange
            const string nomeArquivo = "imagem.png";
            const string contentType = "image/png";
            var bytesArquivo = System.Text.Encoding.UTF8.GetBytes("conteudo-simulado-da-imagem");
            var streamArquivo = new MemoryStream(bytesArquivo);
            var metadados = ObterMetadados(nomeArquivo, contentType, 10);
            var base64Esperada = Convert.ToBase64String(bytesArquivo);
            var resultadoEsperado = $"data:{contentType};base64,{base64Esperada}";

            _servicoArmazenamentoMock.Setup(s => s.ObterMetadadosObjeto(It.IsAny<string>(), It.IsAny<string>()))
                                     .ReturnsAsync(metadados);
            _servicoArmazenamentoMock.Setup(s => s.ObterStream(It.IsAny<string>(), It.IsAny<string>()))
                                     .ReturnsAsync(streamArquivo);

            // Act
            var resultado = await _servico.ObterImagemBase64(nomeArquivo);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().Be(resultadoEsperado);
            _servicoArmazenamentoMock.Verify(s => s.ObterMetadadosObjeto(nomeArquivo, null), Times.Once);
            _servicoArmazenamentoMock.Verify(s => s.ObterStream(nomeArquivo, null), Times.Once);
        }

        [Fact]
        public async Task ObterImagemBase64_QuandoMetadadosNaoEncontrados_DeveRetornarNull()
        {
            // Arrange
            const string nomeArquivo = "arquivo-inexistente.txt";

            // Act
            var resultado = await _servico.ObterImagemBase64(nomeArquivo);

            // Assert
            resultado.Should().BeNull();
            _servicoArmazenamentoMock.Verify(s => s.ObterStream("", ""), Times.Never);
        }

        [Theory(DisplayName = "Deve retornar nulo quando stream do arquivo é nulo ou vazio")]
        [InlineData(true)]  // Testa com stream nulo
        [InlineData(false)] // Testa com stream vazio
        public async Task ObterImagemBase64_QuandoStreamNuloOuVazio_DeveRetornarNull(bool streamNulo)
        {
            // Arrange
            const string nomeArquivo = "arquivo-com-problema.dat";

            var metadados = ObterMetadados(nomeArquivo, "application/octet-stream", 10);

            Stream? stream = streamNulo ? null : new MemoryStream();

            _servicoArmazenamentoMock.Setup(s => s.ObterMetadadosObjeto(It.IsAny<string>(), It.IsAny<string>()))
                                     .ReturnsAsync(metadados);
            _servicoArmazenamentoMock.Setup(s => s.ObterStream(It.IsAny<string>(), It.IsAny<string>()))
                                     .ReturnsAsync(stream);

            // Act
            var resultado = await _servico.ObterImagemBase64(nomeArquivo);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task ObterImagemBase64_QuandoServicoArmazenamentoLancaExcecao_DeveRetornarNull()
        {
            // Arrange
            const string nomeArquivo = "arquivo-com-erro.zip";
            _servicoArmazenamentoMock.Setup(s => s.ObterMetadadosObjeto(It.IsAny<string>(), It.IsAny<string>()))
                                     .ThrowsAsync(new Exception("Erro de conexão simulado."));

            // Act
            var resultado = await _servico.ObterImagemBase64(nomeArquivo);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task ObterDetalhamento_ParaTipoDocumentalComImagens_DeveRetornarDetalhesCorretos()
        {
            // Arrange
            var filtro = new FiltroDetalharAcervoDTO { Codigo = "COD-DOC-01", Tipo = TipoAcervo.DocumentacaoTextual };
            var enderecoBase = _optionsMock.Object.Value.EnderecoCompletoBucketArquivos();

            var nomeOriginalArquivo = "documento_historico.jpg";
            var codigoOriginalGuid = Guid.NewGuid();
            var codigoThumbnailGuid = Guid.NewGuid();

            var imagemDominio = new ImagemDetalhe
            {
                NomeOriginal = nomeOriginalArquivo,
                CodigoOriginal = codigoOriginalGuid,
                CodigoThumbnail = codigoThumbnailGuid
            };

            var detalheDominio = new AcervoDocumentalDetalhe
            {
                Id = 1,
                Titulo = "Documento Histórico",
                Imagens = new List<ImagemDetalhe> { imagemDominio }
            };

            var imagemDto = new ImagemDTO
            {
                Original = imagemDominio.Original,
                Thumbnail = imagemDominio.Thumbnail
            };

            var detalheDto = new AcervoDocumentalDetalheDTO
            {
                Titulo = detalheDominio.Titulo,
                Imagens = new[] { imagemDto }
            };

            _repositorioAcervoDocumentalMock.Setup(r => r.ObterDetalhamentoPorCodigo(filtro.Codigo)).ReturnsAsync(detalheDominio);
            _mapperMock.Setup(m => m.Map<AcervoDocumentalDetalheDTO>(detalheDominio)).Returns(detalheDto);

            // Act
            var resultado = await _servico.ObterDetalhamentoPorTipoAcervoECodigo(filtro) as AcervoDocumentalDetalheDTO;

            // Assert
            resultado.Should().NotBeNull();
            resultado.Titulo.Should().Be(detalheDominio.Titulo);
            resultado.TipoAcervoId.Should().Be((int)TipoAcervo.DocumentacaoTextual);
            resultado.EnderecoImagemPadrao.Should().BeEmpty();

            var urlOriginalEsperada = $"{enderecoBase}/{imagemDominio.Original}";
            var urlThumbnailEsperada = $"{enderecoBase}/{imagemDominio.Thumbnail}";

            resultado.Imagens.First().Original.Should().Be(urlOriginalEsperada);
            resultado.Imagens.First().Thumbnail.Should().Be(urlThumbnailEsperada);

            _repositorioAcervoDocumentalMock.Verify(r => r.ObterDetalhamentoPorCodigo(filtro.Codigo), Times.Once);
        }
        [Fact]
        public async Task ObterDetalhamento_ParaTipoDocumentalSemImagens_DeveRetornarComImagemPadrao()
        {
            // Arrange
            var filtro = new FiltroDetalharAcervoDTO { Codigo = "COD-DOC-02", Tipo = TipoAcervo.DocumentacaoTextual };
            var enderecoBase = _optionsMock.Object.Value.EnderecoCompletoBucketArquivos();
            var codigo = Guid.NewGuid();
            var nomeImagemPadraoFisico = $"{codigo}.png";

            var detalheDominio = new AcervoDocumentalDetalhe { Id = 2, Titulo = "Documento Sem Imagem", Imagens = new List<ImagemDetalhe>() };
            var detalheDto = new AcervoDocumentalDetalheDTO { Titulo = detalheDominio.Titulo, Imagens = Array.Empty<ImagemDTO>() };

            _repositorioAcervoDocumentalMock.Setup(r => r.ObterDetalhamentoPorCodigo(filtro.Codigo)).ReturnsAsync(detalheDominio);
            _mapperMock.Setup(m => m.Map<AcervoDocumentalDetalheDTO>(detalheDominio)).Returns(detalheDto);

            var arquivoPadrao = new Arquivo
            {
                Codigo = codigo,
                Nome = nomeImagemPadraoFisico
            };

            _repositorioArquivoMock.Setup(r => r.ObterArquivoPorNomeTipoArquivo(It.IsAny<string>(), TipoArquivo.Sistema))
                                   .ReturnsAsync(arquivoPadrao);

            // Act
            var resultado = await _servico.ObterDetalhamentoPorTipoAcervoECodigo(filtro) as AcervoDocumentalDetalheDTO;

            // Assert
            resultado.Should().NotBeNull();
            resultado.Imagens.Should().BeEmpty();

            // Valida que, na ausência de imagens, o endereço da imagem padrão foi preenchido com o nome físico correto.
            resultado.EnderecoImagemPadrao.Should().Be($"{enderecoBase}{nomeImagemPadraoFisico}");

            _repositorioAcervoDocumentalMock.Verify(r => r.ObterDetalhamentoPorCodigo(filtro.Codigo), Times.Once);
            _repositorioArquivoMock.Verify(r => r.ObterArquivoPorNomeTipoArquivo(It.IsAny<string>(), TipoArquivo.Sistema), Times.Once);
        }

        [Fact]
        public async Task ObterDetalhamento_ParaTipoDocumentalNaoEncontrado_DeveLancarNegocioException()
        {
            // Arrange
            var filtro = new FiltroDetalharAcervoDTO { Codigo = "COD-INEXISTENTE", Tipo = TipoAcervo.DocumentacaoTextual };

            _repositorioAcervoDocumentalMock.Setup(r => r.ObterDetalhamentoPorCodigo(filtro.Codigo))
                                            .ReturnsAsync((AcervoDocumentalDetalhe)null);

            // Act
            Func<Task> acao = async () => await _servico.ObterDetalhamentoPorTipoAcervoECodigo(filtro);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>().WithMessage("Acervo não encontrado.");
            _repositorioAcervoDocumentalMock.Verify(r => r.ObterDetalhamentoPorCodigo(filtro.Codigo), Times.Once);
        }

        private static ObjectStat ObterMetadados(string nomeArquivo, string contentType, long tamanho)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Content-Type", contentType },
                { "Content-Length", tamanho.ToString() },
                { "Last-Modified", DateTime.Now.ToString("R") }, // Formato RFC1123
                { "ETag", "etag-teste" }
            };
            return ObjectStat.FromResponseHeaders(nomeArquivo, headers);
        }
    }
}
