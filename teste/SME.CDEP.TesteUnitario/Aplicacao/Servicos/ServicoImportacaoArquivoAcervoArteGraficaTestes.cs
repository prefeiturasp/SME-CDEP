using AutoMapper;
using ClosedXML.Excel;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.AutoMock;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoImportacaoArquivoAcervoArteGraficaTestes
    {
        private readonly Mock<IRepositorioImportacaoArquivo> repositorioImportacaoArquivoMock;
        private readonly Mock<IServicoMaterial> servicoMaterialMock;
        private readonly Mock<IServicoEditora> servicoEditoraMock;
        private readonly Mock<IServicoSerieColecao> servicoSerieColecaoMock;
        private readonly Mock<IServicoIdioma> servicoIdiomaMock;
        private readonly Mock<IServicoAssunto> servicoAssuntoMock;
        private readonly Mock<IServicoCreditoAutor> servicoCreditoAutorMock;
        private readonly Mock<IServicoConservacao> servicoConservacaoMock;
        private readonly Mock<IServicoAcessoDocumento> servicoAcessoDocumentoMock;
        private readonly Mock<IServicoCromia> servicoCromiaMock;
        private readonly Mock<IServicoSuporte> servicoSuporteMock;
        private readonly Mock<IServicoFormato> servicoFormatoMock;
        private readonly Mock<IServicoMensageria> servicoMensageriaMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IRepositorioParametroSistema> repositorioParametroSistemaMock;
        private readonly ServicoImportacaoArquivoAcervoArteGrafica sut;

        public ServicoImportacaoArquivoAcervoArteGraficaTestes()
        {
            var mocker = new AutoMocker();

            repositorioImportacaoArquivoMock = mocker.GetMock<IRepositorioImportacaoArquivo>();
            servicoMaterialMock = mocker.GetMock<IServicoMaterial>();
            servicoEditoraMock = mocker.GetMock<IServicoEditora>();
            servicoSerieColecaoMock = mocker.GetMock<IServicoSerieColecao>();
            servicoIdiomaMock = mocker.GetMock<IServicoIdioma>();
            servicoAssuntoMock = mocker.GetMock<IServicoAssunto>();
            servicoCreditoAutorMock = mocker.GetMock<IServicoCreditoAutor>();
            servicoConservacaoMock = mocker.GetMock<IServicoConservacao>();
            servicoAcessoDocumentoMock = mocker.GetMock<IServicoAcessoDocumento>();
            servicoCromiaMock = mocker.GetMock<IServicoCromia>();
            servicoSuporteMock = mocker.GetMock<IServicoSuporte>();
            servicoFormatoMock = mocker.GetMock<IServicoFormato>();
            servicoMensageriaMock = mocker.GetMock<IServicoMensageria>();
            mapperMock = mocker.GetMock<IMapper>();
            repositorioParametroSistemaMock = mocker.GetMock<IRepositorioParametroSistema>();

            sut = mocker.CreateInstance<ServicoImportacaoArquivoAcervoArteGrafica>();
        }

        [Fact]
        public void DadoDependenciasValidas_QuandoInstanciarServico_EntaoRetornaInstanciaComSucesso()
        {
            // Arrange
            Action acao = () => new ServicoImportacaoArquivoAcervoArteGrafica(
                repositorioImportacaoArquivoMock.Object,
                servicoMaterialMock.Object,
                servicoEditoraMock.Object,
                servicoSerieColecaoMock.Object,
                servicoIdiomaMock.Object,
                servicoAssuntoMock.Object,
                servicoCreditoAutorMock.Object,
                servicoConservacaoMock.Object,
                servicoAcessoDocumentoMock.Object,
                servicoCromiaMock.Object,
                servicoSuporteMock.Object,
                servicoFormatoMock.Object,
                servicoMensageriaMock.Object,
                mapperMock.Object,
                repositorioParametroSistemaMock.Object);

            // Act & Assert
            acao.Should().NotThrow();
            sut.Should().NotBeNull();
        }

        [Fact]
        public void DadoServicoMensageriaNulo_QuandoInstanciarServico_EntaoLancaArgumentNullException()
        {
            // Arrange
            IServicoMensageria servicoMensageriaNulo = null!;

            // Act
            Action acao = () => new ServicoImportacaoArquivoAcervoArteGrafica(
                repositorioImportacaoArquivoMock.Object,
                servicoMaterialMock.Object,
                servicoEditoraMock.Object,
                servicoSerieColecaoMock.Object,
                servicoIdiomaMock.Object,
                servicoAssuntoMock.Object,
                servicoCreditoAutorMock.Object,
                servicoConservacaoMock.Object,
                servicoAcessoDocumentoMock.Object,
                servicoCromiaMock.Object,
                servicoSuporteMock.Object,
                servicoFormatoMock.Object,
                servicoMensageriaNulo,
                mapperMock.Object,
                repositorioParametroSistemaMock.Object);

            // Assert
            acao.Should().Throw<ArgumentNullException>()
                .WithParameterName("servicoMensageria");
        }

        [Fact]
        public async Task DadoNenhumaImportacaoPendente_QuandoObterImportacaoPendente_EntaoRetornaNulo()
        {
            // Arrange
            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterUltimaImportacao(TipoAcervo.ArtesGraficas))
                .ReturnsAsync((ImportacaoArquivo)null!);

            // Act
            var resultado = await sut.ObterImportacaoPendente();

            // Assert
            resultado.Should().BeNull();

            repositorioImportacaoArquivoMock.Verify(r => r.ObterUltimaImportacao(TipoAcervo.ArtesGraficas), Times.Once);
        }

        [Fact]
        public async Task DadoImportacaoPendenteExistente_QuandoObterImportacaoPendente_EntaoRetornaDtoComSucessoEErros()
        {
            // Arrange
            var linhasPlanilhaMock = new List<AcervoArteGraficaLinhaDTO>
            {
                new() {
                    NumeroLinha = 1,
                    PossuiErros = false,
                    Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Obra Arte 1" },
                    Codigo = new LinhaConteudoAjustarDTO { Conteudo = "TOMBO-001" }
                },
                new() {
                    NumeroLinha = 2,
                    PossuiErros = true,
                    Mensagem = "Erro na validação da linha",
                    Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Obra Arte 2", PossuiErro = true, Mensagem = "Título obrigatório" },
                    Codigo = new LinhaConteudoAjustarDTO { Conteudo = "TOMBO-002" },
                    Credito = new LinhaConteudoAjustarDTO(),
                    Localizacao = new LinhaConteudoAjustarDTO(),
                    Procedencia = new LinhaConteudoAjustarDTO(),
                    Ano = new LinhaConteudoAjustarDTO(),
                    CopiaDigital = new LinhaConteudoAjustarDTO(),
                    PermiteUsoImagem = new LinhaConteudoAjustarDTO(),
                    EstadoConservacao = new LinhaConteudoAjustarDTO(),
                    Cromia = new LinhaConteudoAjustarDTO(),
                    Largura = new LinhaConteudoAjustarDTO(),
                    Altura = new LinhaConteudoAjustarDTO(),
                    Diametro = new LinhaConteudoAjustarDTO(),
                    Tecnica = new LinhaConteudoAjustarDTO(),
                    Suporte = new LinhaConteudoAjustarDTO(),
                    Quantidade = new LinhaConteudoAjustarDTO(),
                    Descricao = new LinhaConteudoAjustarDTO()
                }
            };

            var arquivoImportadoMock = new ImportacaoArquivo
            {
                Id = 99,
                Nome = "planilha_acervo_arte_grafica.xlsx",
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasPlanilhaMock),
                CriadoEm = DateTime.Now
            };

            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterUltimaImportacao(TipoAcervo.ArtesGraficas))
                .ReturnsAsync(arquivoImportadoMock);

            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "100" });

            servicoSuporteMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoCromiaMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoConservacaoMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoAcessoDocumentoMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoCreditoAutorMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoMaterialMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoEditoraMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoSerieColecaoMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoIdiomaMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoAssuntoMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoFormatoMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);

            // Act
            var resultado = await sut.ObterImportacaoPendente();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(arquivoImportadoMock.Id);
            resultado.TipoAcervo.Should().Be(TipoAcervo.ArtesGraficas);

            resultado.Sucesso.Should().HaveCount(1);
            resultado.Sucesso.First().NumeroLinha.Should().Be(1);

            resultado.Erros.Should().HaveCount(1);
            resultado.Erros.First().NumeroLinha.Should().Be(2);
            resultado.Erros.First().RetornoErro.ErrosCampos.Should().Contain("Título obrigatório");

            repositorioImportacaoArquivoMock.Verify(r => r.ObterUltimaImportacao(TipoAcervo.ArtesGraficas), Times.Once);
            repositorioParametroSistemaMock.Verify(p => p.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DadoIdInexistente_QuandoObterImportacaoPorId_EntaoRetornaNulo()
        {
            // Arrange
            var idInexistente = 999L;

            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterImportacaoPorId(idInexistente))
                .ReturnsAsync((ImportacaoArquivo)null!);

            // Act
            var resultado = await sut.ObterImportacaoPorId(idInexistente);

            // Assert
            resultado.Should().BeNull();

            repositorioImportacaoArquivoMock.Verify(r => r.ObterImportacaoPorId(idInexistente), Times.Once);
        }

        [Fact]
        public async Task DadoIdExistente_QuandoObterImportacaoPorId_EntaoRetornaDtoComSucessoEErros()
        {
            // Arrange
            var idExistente = 10L;

            var linhasPlanilhaMock = new List<AcervoArteGraficaLinhaDTO>
            {
                new() {
                    NumeroLinha = 1,
                    PossuiErros = false,
                    Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Obra Arte 1" },
                    Codigo = new LinhaConteudoAjustarDTO { Conteudo = "TOMBO-001" }
                },
                new() {
                    NumeroLinha = 2,
                    PossuiErros = true,
                    Mensagem = "Erro na validação da linha",
                    Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Obra Arte 2", PossuiErro = true, Mensagem = "Título obrigatório" },
                    Codigo = new LinhaConteudoAjustarDTO { Conteudo = "TOMBO-002" },
                    Credito = new LinhaConteudoAjustarDTO(),
                    Localizacao = new LinhaConteudoAjustarDTO(),
                    Procedencia = new LinhaConteudoAjustarDTO(),
                    Ano = new LinhaConteudoAjustarDTO(),
                    CopiaDigital = new LinhaConteudoAjustarDTO(),
                    PermiteUsoImagem = new LinhaConteudoAjustarDTO(),
                    EstadoConservacao = new LinhaConteudoAjustarDTO(),
                    Cromia = new LinhaConteudoAjustarDTO(),
                    Largura = new LinhaConteudoAjustarDTO(),
                    Altura = new LinhaConteudoAjustarDTO(),
                    Diametro = new LinhaConteudoAjustarDTO(),
                    Tecnica = new LinhaConteudoAjustarDTO(),
                    Suporte = new LinhaConteudoAjustarDTO(),
                    Quantidade = new LinhaConteudoAjustarDTO(),
                    Descricao = new LinhaConteudoAjustarDTO()
                }
            };

            var arquivoImportadoMock = new ImportacaoArquivo
            {
                Id = idExistente,
                Nome = "planilha_acervo_arte_grafica_2.xlsx",
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasPlanilhaMock),
                CriadoEm = DateTime.Now
            };

            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterImportacaoPorId(idExistente))
                .ReturnsAsync(arquivoImportadoMock);

            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "100" });

            servicoSuporteMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoCromiaMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoConservacaoMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoAcessoDocumentoMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoCreditoAutorMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoMaterialMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoEditoraMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoSerieColecaoMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoIdiomaMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoAssuntoMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoFormatoMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);

            // Act
            var resultado = await sut.ObterImportacaoPorId(idExistente);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(idExistente);
            resultado.TipoAcervo.Should().Be(TipoAcervo.ArtesGraficas);

            resultado.Sucesso.Should().HaveCount(1);
            resultado.Sucesso.First().NumeroLinha.Should().Be(1);

            resultado.Erros.Should().HaveCount(1);
            resultado.Erros.First().NumeroLinha.Should().Be(2);
            resultado.Erros.First().RetornoErro.ErrosCampos.Should().Contain("Título obrigatório");

            repositorioImportacaoArquivoMock.Verify(r => r.ObterImportacaoPorId(idExistente), Times.Once);
            repositorioParametroSistemaMock.Verify(p => p.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()), Times.Once);
        }
        [Fact]
        public async Task DadoLinhaValidaEArquivoComVariasLinhas_QuandoRemoverLinhaDoArquivo_EntaoRetornaTrueEAtualizaArquivo()
        {
            // Arrange
            var idImportacao = 1L;
            var linhaParaRemover = new LinhaDTO { NumeroLinha = 1 };

            var linhasPlanilhaMock = new List<AcervoArteGraficaLinhaDTO>
            {
                new() { NumeroLinha = 1, Titulo = new LinhaConteudoAjustarDTO() },
                new() { NumeroLinha = 2, Titulo = new LinhaConteudoAjustarDTO() }
            };

            var arquivoImportadoMock = new ImportacaoArquivo
            {
                Id = idImportacao,
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Conteudo = JsonConvert.SerializeObject(linhasPlanilhaMock)
            };

            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterPorId(idImportacao))
                .ReturnsAsync(arquivoImportadoMock);

            repositorioImportacaoArquivoMock
                .Setup(r => r.Salvar(It.IsAny<ImportacaoArquivo>()))
                .ReturnsAsync(idImportacao);

            // Act
            var resultado = await sut.RemoverLinhaDoArquivo(idImportacao, linhaParaRemover);

            // Assert
            resultado.Should().BeTrue();

            repositorioImportacaoArquivoMock.Verify(r => r.ObterPorId(idImportacao), Times.Exactly(2));
            repositorioImportacaoArquivoMock.Verify(r => r.Salvar(It.Is<ImportacaoArquivo>(a =>
                !a.Conteudo.Contains("\"NumeroLinha\":1") &&
                a.Conteudo.Contains("\"NumeroLinha\":2")
            )), Times.Once);
        }

        [Fact]
        public async Task DadoLinhaValidaESemOutrosErros_QuandoAtualizarLinhaParaSucesso_EntaoRetornaTrueEAtualizaStatusParaSucesso()
        {
            // Arrange
            var idImportacao = 1L;
            var linhaParaAtualizar = new LinhaDTO { NumeroLinha = 1 };

            var linhaCompletaComErro = new AcervoArteGraficaLinhaDTO
            {
                NumeroLinha = 1,
                PossuiErros = true,
                Titulo = new LinhaConteudoAjustarDTO(),
                Codigo = new LinhaConteudoAjustarDTO(),
                Credito = new LinhaConteudoAjustarDTO(),
                Localizacao = new LinhaConteudoAjustarDTO(),
                Procedencia = new LinhaConteudoAjustarDTO(),
                CopiaDigital = new LinhaConteudoAjustarDTO(),
                PermiteUsoImagem = new LinhaConteudoAjustarDTO(),
                EstadoConservacao = new LinhaConteudoAjustarDTO(),
                Cromia = new LinhaConteudoAjustarDTO(),
                Largura = new LinhaConteudoAjustarDTO(),
                Altura = new LinhaConteudoAjustarDTO(),
                Diametro = new LinhaConteudoAjustarDTO(),
                Tecnica = new LinhaConteudoAjustarDTO(),
                Suporte = new LinhaConteudoAjustarDTO(),
                Quantidade = new LinhaConteudoAjustarDTO(),
                Descricao = new LinhaConteudoAjustarDTO()
            };

            var arquivoImportadoMock = new ImportacaoArquivo
            {
                Id = idImportacao,
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Status = ImportacaoStatus.Erros,
                Conteudo = JsonConvert.SerializeObject(new List<AcervoArteGraficaLinhaDTO> { linhaCompletaComErro })
            };

            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterPorId(idImportacao))
                .ReturnsAsync(arquivoImportadoMock);

            repositorioImportacaoArquivoMock
                .Setup(r => r.Salvar(It.IsAny<ImportacaoArquivo>()))
                .ReturnsAsync(idImportacao);

            // Act
            var resultado = await sut.AtualizarLinhaParaSucesso(idImportacao, linhaParaAtualizar);

            // Assert
            resultado.Should().BeTrue();

            repositorioImportacaoArquivoMock.Verify(r => r.Salvar(It.Is<ImportacaoArquivo>(a =>
                a.Status == ImportacaoStatus.Sucesso
            )), Times.Once);
        }

        [Fact]
        public async Task DadoLinhaValidaEComOutrosErrosRestantes_QuandoAtualizarLinhaParaSucesso_EntaoRetornaTrueEMantemStatusErros()
        {
            // Arrange
            var idImportacao = 1L;
            var linhaParaAtualizar = new LinhaDTO { NumeroLinha = 1 };

            var linhaCompletaComErro = new AcervoArteGraficaLinhaDTO
            {
                NumeroLinha = 1,
                PossuiErros = true,
                Titulo = new LinhaConteudoAjustarDTO(),
                Codigo = new LinhaConteudoAjustarDTO(),
                Credito = new LinhaConteudoAjustarDTO(),
                Localizacao = new LinhaConteudoAjustarDTO(),
                Procedencia = new LinhaConteudoAjustarDTO(),
                CopiaDigital = new LinhaConteudoAjustarDTO(),
                PermiteUsoImagem = new LinhaConteudoAjustarDTO(),
                EstadoConservacao = new LinhaConteudoAjustarDTO(),
                Cromia = new LinhaConteudoAjustarDTO(),
                Largura = new LinhaConteudoAjustarDTO(),
                Altura = new LinhaConteudoAjustarDTO(),
                Diametro = new LinhaConteudoAjustarDTO(),
                Tecnica = new LinhaConteudoAjustarDTO(),
                Suporte = new LinhaConteudoAjustarDTO(),
                Quantidade = new LinhaConteudoAjustarDTO(),
                Descricao = new LinhaConteudoAjustarDTO()
            };

            var outraLinhaComErro = new AcervoArteGraficaLinhaDTO
            {
                NumeroLinha = 2,
                PossuiErros = true,
                Titulo = new LinhaConteudoAjustarDTO()
            };

            var arquivoImportadoMock = new ImportacaoArquivo
            {
                Id = idImportacao,
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Status = ImportacaoStatus.Erros,
                Conteudo = JsonConvert.SerializeObject(new List<AcervoArteGraficaLinhaDTO> { linhaCompletaComErro, outraLinhaComErro })
            };

            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterPorId(idImportacao))
                .ReturnsAsync(arquivoImportadoMock);

            repositorioImportacaoArquivoMock
                .Setup(r => r.Salvar(It.IsAny<ImportacaoArquivo>()))
                .ReturnsAsync(idImportacao);

            // Act
            var resultado = await sut.AtualizarLinhaParaSucesso(idImportacao, linhaParaAtualizar);

            // Assert
            resultado.Should().BeTrue();

            repositorioImportacaoArquivoMock.Verify(r => r.Salvar(It.Is<ImportacaoArquivo>(a =>
                a.Status == ImportacaoStatus.Erros
            )), Times.Once);
        }
        [Fact]
        public async Task DadoArquivoNuloOuVazio_QuandoImportarArquivo_EntaoLancaNegocioException()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(0);

            // Act
            Func<Task> acao = async () => await sut.ImportarArquivo(fileMock.Object);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.ARQUIVO_VAZIO);
        }

        [Fact]
        public async Task DadoArquivoComFormatoInvalido_QuandoImportarArquivo_EntaoLancaNegocioException()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(100);
            fileMock.Setup(f => f.ContentType).Returns("text/csv");

            // Act
            Func<Task> acao = async () => await sut.ImportarArquivo(fileMock.Object);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.SOMENTE_ARQUIVO_XLSX_SUPORTADO);
        }

        [Fact]
        public async Task DadoPlanilhaVazia_QuandoImportarArquivo_EntaoLancaNegocioException()
        {
            // Arrange
            var stream = CriarStreamExcelPlanilhaMock(p => { /* Planilha sem dados */ });

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(100);
            fileMock.Setup(f => f.ContentType).Returns(Constantes.CONTENT_TYPE_EXCEL);
            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);

            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.LimiteAcervosImportadosViaPanilha, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "100" });

            // Act
            Func<Task> acao = async () => await sut.ImportarArquivo(fileMock.Object);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.PLANILHA_VAZIA);
        }

        [Fact]
        public async Task DadoPlanilhaComMaisLinhasQueOLimite_QuandoImportarArquivo_EntaoLancaNegocioException()
        {
            // Arrange
            var stream = CriarStreamExcelPlanilhaMock(p =>
            {
                GerarCabecalhoPlanilha(p);
                p.Cell(2, 1).Value = "Linha de Dado 1";
                p.Cell(3, 1).Value = "Linha de Dado 2";
            });

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(100);
            fileMock.Setup(f => f.ContentType).Returns(Constantes.CONTENT_TYPE_EXCEL);
            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);

            var limiteParametrizado = 1;
            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.LimiteAcervosImportadosViaPanilha, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = limiteParametrizado.ToString() });

            // Act
            Func<Task> acao = async () => await sut.ImportarArquivo(fileMock.Object);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(string.Format(MensagemNegocio.LIMITE_ACERVOS_IMPORTADOS_VIA_PLANILHA, limiteParametrizado));
        }

        [Fact]
        public async Task DadoPlanilhaComColunasForaDeOrdemOuNomesIncorretos_QuandoImportarArquivo_EntaoLancaNegocioException()
        {
            // Arrange
            var stream = CriarStreamExcelPlanilhaMock(p =>
            {
                GerarCabecalhoPlanilha(p);
                p.Cell(1, 1).Value = "COLUNA_ERRADA";
                p.Cell(2, 1).Value = "Dado de Teste";
            });

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(100);
            fileMock.Setup(f => f.ContentType).Returns(Constantes.CONTENT_TYPE_EXCEL);
            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);

            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.LimiteAcervosImportadosViaPanilha, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "100" });

            // Act
            Func<Task> acao = async () => await sut.ImportarArquivo(fileMock.Object);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(string.Format(Constantes.A_PLANLHA_DE_ACERVO_X_NAO_TEM_O_NOME_DA_COLUNA_Y_NA_COLUNA_Z,
                    Constantes.ARTE_GRAFICA, Constantes.NOME_DA_COLUNA_TITULO, Constantes.ACERVO_ARTE_GRAFICA_CAMPO_TITULO));
        }

        [Fact]
        public async Task DadoArquivoEPlanilhaValidos_QuandoImportarArquivo_EntaoPersisteArquivoEPublicaNoRabbitMQERetornaDtoPendente()
        {
            // Arrange
            var fileName = "arte_grafica_importacao.xlsx";
            var stream = CriarStreamExcelPlanilhaMock(p =>
            {
                GerarCabecalhoPlanilha(p);
                p.Cell(2, 1).Value = "Obra Arte 1"; // Titulo
                p.Cell(2, 2).Value = "TMB-123";     // Tombo
            });

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(100);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.ContentType).Returns(Constantes.CONTENT_TYPE_EXCEL);
            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);

            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.LimiteAcervosImportadosViaPanilha, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "100" });

            var idImportacaoGerado = 77L;
            repositorioImportacaoArquivoMock
                .Setup(r => r.Salvar(It.IsAny<ImportacaoArquivo>()))
                .ReturnsAsync(idImportacaoGerado);

            // Act
            var resultado = await sut.ImportarArquivo(fileMock.Object);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(idImportacaoGerado);
            resultado.Nome.Should().Be(fileName);
            resultado.TipoAcervo.Should().Be(TipoAcervo.ArtesGraficas);
            resultado.Status.Should().Be(ImportacaoStatus.Pendente);

            repositorioImportacaoArquivoMock.Verify(r => r.Salvar(It.Is<ImportacaoArquivo>(a =>
                a.TipoAcervo == TipoAcervo.ArtesGraficas &&
                a.Status == ImportacaoStatus.Pendente &&
                a.Conteudo.Contains("Obra Arte 1")
            )), Times.Once);

            servicoMensageriaMock.Verify(s => s.Publicar(
                RotasRabbit.ExecutarImportacaoArquivoAcervoArteGrafica,
                idImportacaoGerado,
                It.IsAny<Guid>()
            ), Times.Once);
        }

        [Fact]
        public async Task DadoProcessamentoDeArquivo_QuandoObterImportacao_EntaoDeveCarregarDominiosDaBase()
        {
            // Arrange
            var idImportacao = 1L;
            var arquivoImportadoMock = new ImportacaoArquivo
            {
                Id = idImportacao,
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Conteudo = JsonConvert.SerializeObject(new List<AcervoArteGraficaLinhaDTO>())
            };

            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterImportacaoPorId(idImportacao))
                .ReturnsAsync(arquivoImportadoMock);

            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.LimiteAcervosImportadosViaPanilha, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "100" });

            servicoSuporteMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoCromiaMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoConservacaoMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoAcessoDocumentoMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoCreditoAutorMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoMaterialMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoEditoraMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoSerieColecaoMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoIdiomaMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoAssuntoMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);
            servicoFormatoMock.Setup(s => s.ObterTodos()).ReturnsAsync([]);

            // Act
            await sut.ObterImportacaoPorId(idImportacao);

            // Assert
            servicoSuporteMock.Verify(s => s.ObterTodos(), Times.Once);
            servicoCromiaMock.Verify(s => s.ObterTodos(), Times.Once);
            servicoConservacaoMock.Verify(s => s.ObterTodos(), Times.Once);
            servicoCreditoAutorMock.Verify(s => s.ObterTodos(), Times.Once);
            servicoMaterialMock.Verify(s => s.ObterTodos(), Times.Once);
        }

        [Fact]
        public async Task DadoLinhaComSucesso_QuandoProcessarImportacao_EntaoRetornaSucessoComSufixoAdicionadoAoTombo()
        {
            // Arrange
            var idImportacao = 1L;
            var linhaSucesso = new AcervoArteGraficaLinhaDTO
            {
                NumeroLinha = 1,
                PossuiErros = false,
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Mona Lisa" },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = "TMB-001" } // Sem sufixo
            };

            var arquivoImportadoMock = new ImportacaoArquivo
            {
                Id = idImportacao,
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Conteudo = JsonConvert.SerializeObject(new List<AcervoArteGraficaLinhaDTO> { linhaSucesso })
            };

            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterImportacaoPorId(idImportacao))
                .ReturnsAsync(arquivoImportadoMock);

            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.LimiteAcervosImportadosViaPanilha, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "100" });

            // Act
            var resultado = await sut.ObterImportacaoPorId(idImportacao);

            // Assert
            resultado.Sucesso.Should().HaveCount(1);
            var sucesso = resultado.Sucesso.First();
            sucesso.Titulo.Should().Be("Mona Lisa");
            sucesso.Tombo.Should().Be($"TMB-001{Constantes.SIGLA_ACERVO_ARTE_GRAFICA}");
        }

        [Fact]
        public async Task DadoLinhaComErrosDeValidacao_QuandoProcessarImportacao_EntaoMapeiaListaDeErrosDaLinha()
        {
            // Arrange
            var idImportacao = 1L;
            var linhaErro = new AcervoArteGraficaLinhaDTO
            {
                NumeroLinha = 2,
                PossuiErros = true,
                Mensagem = "Existem erros na linha",
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = string.Empty, PossuiErro = true, Mensagem = "Título é obrigatório" },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = "TMB-002", PossuiErro = false },
                Largura = new LinhaConteudoAjustarDTO { Conteudo = "ABC", PossuiErro = true, Mensagem = "Largura inválida" },
                Altura = new LinhaConteudoAjustarDTO { Conteudo = "10,50", PossuiErro = false },
                Credito = new LinhaConteudoAjustarDTO(),
                Localizacao = new LinhaConteudoAjustarDTO(),
                Procedencia = new LinhaConteudoAjustarDTO(),
                Ano = new LinhaConteudoAjustarDTO(),
                CopiaDigital = new LinhaConteudoAjustarDTO(),
                PermiteUsoImagem = new LinhaConteudoAjustarDTO(),
                EstadoConservacao = new LinhaConteudoAjustarDTO(),
                Cromia = new LinhaConteudoAjustarDTO(),
                Diametro = new LinhaConteudoAjustarDTO(),
                Tecnica = new LinhaConteudoAjustarDTO(),
                Suporte = new LinhaConteudoAjustarDTO(),
                Quantidade = new LinhaConteudoAjustarDTO(),
                Descricao = new LinhaConteudoAjustarDTO()
            };

            var arquivoImportadoMock = new ImportacaoArquivo
            {
                Id = idImportacao,
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Conteudo = JsonConvert.SerializeObject(new List<AcervoArteGraficaLinhaDTO> { linhaErro })
            };

            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterImportacaoPorId(idImportacao))
                .ReturnsAsync(arquivoImportadoMock);

            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.LimiteAcervosImportadosViaPanilha, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "100" });

            // Act
            var resultado = await sut.ObterImportacaoPorId(idImportacao);

            // Assert
            resultado.Erros.Should().HaveCount(1);
            var erroMapeado = resultado.Erros.First();
            erroMapeado.NumeroLinha.Should().Be(2);
            erroMapeado.RetornoErro.ErrosCampos.Should().Contain("Título é obrigatório");
            erroMapeado.RetornoErro.ErrosCampos.Should().Contain("Largura inválida");
            erroMapeado.RetornoErro.ErrosCampos.Should().NotContain("Altura");
        }

        [Fact]
        public async Task DadoLinhaComDadosPreenchidos_QuandoProcessarImportacao_EntaoPopulaAcervoArteGraficaDtoCorretamente()
        {
            // Arrange
            var idImportacao = 1L;
            var linhaPreenchida = new AcervoArteGraficaLinhaDTO
            {
                NumeroLinha = 3,
                PossuiErros = true,
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Obra Teste" },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = "TMB-003" },
                CopiaDigital = new LinhaConteudoAjustarDTO { Conteudo = "Sim" },
                PermiteUsoImagem = new LinhaConteudoAjustarDTO { Conteudo = "Não" },
                EstadoConservacao = new LinhaConteudoAjustarDTO { Conteudo = "Excelente" },
                Cromia = new LinhaConteudoAjustarDTO { Conteudo = "Preto e Branco" },
                Credito = new LinhaConteudoAjustarDTO { Conteudo = "Da Vinci" },
                Quantidade = new LinhaConteudoAjustarDTO { Conteudo = "5" },
                Largura = new LinhaConteudoAjustarDTO(),
                Altura = new LinhaConteudoAjustarDTO(),
                Diametro = new LinhaConteudoAjustarDTO(),
                Localizacao = new LinhaConteudoAjustarDTO(),
                Procedencia = new LinhaConteudoAjustarDTO(),
                Ano = new LinhaConteudoAjustarDTO(),
                Tecnica = new LinhaConteudoAjustarDTO(),
                Suporte = new LinhaConteudoAjustarDTO(),
                Descricao = new LinhaConteudoAjustarDTO()
            };

            var arquivoImportadoMock = new ImportacaoArquivo
            {
                Id = idImportacao,
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Conteudo = JsonConvert.SerializeObject(new List<AcervoArteGraficaLinhaDTO> { linhaPreenchida })
            };

            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterImportacaoPorId(idImportacao))
                .ReturnsAsync(arquivoImportadoMock);

            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.LimiteAcervosImportadosViaPanilha, It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "100" });

            // Setup Mappers Dinâmicos para os domínios
            mapperMock.Setup(m => m.Map<IdNomeDTO>(It.IsAny<object>())).Returns((object source) =>
            {
                var s = source as dynamic;
                return new IdNomeDTO { Id = s.Id, Nome = s.Nome };
            });

            mapperMock.Setup(m => m.Map<IdNomeTipoDTO>(It.IsAny<object>())).Returns((object source) =>
            {
                var s = source as dynamic;
                return new IdNomeTipoDTO { Id = s.Id, Nome = s.Nome, Tipo = s.Tipo };
            });

            servicoConservacaoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoDTO> { new() { Id = 10, Nome = "Excelente" } });
            servicoCromiaMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoDTO> { new() { Id = 20, Nome = "Preto e Branco" } });
            servicoCreditoAutorMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoAuditavelDTO> { new() { Id = 30, Nome = "Da Vinci", Tipo = (int)TipoCreditoAutoria.Credito } });

            // Act
            var resultado = await sut.ObterImportacaoPorId(idImportacao);

            // Assert
            resultado.Erros.Should().HaveCount(1);
            var dtoMapeado = resultado.Erros.First().RetornoObjeto;

            dtoMapeado.Titulo.Should().Be("Obra Teste");
            dtoMapeado.Codigo.Should().Be($"TMB-003{Constantes.SIGLA_ACERVO_ARTE_GRAFICA}");
            dtoMapeado.CopiaDigital.Should().BeTrue();
            dtoMapeado.PermiteUsoImagem.Should().BeFalse();
            dtoMapeado.Quantidade.Should().Be(5);
            dtoMapeado.ConservacaoId.Should().Be(10);
            dtoMapeado.CromiaId.Should().Be(20);
            dtoMapeado.CreditosAutoresIds.Should().Contain(30);
        }

        private static MemoryStream CriarStreamExcelPlanilhaMock(Action<IXLWorksheet> manipulacaoPlanilha)
        {
            var stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Planilha1");
                manipulacaoPlanilha?.Invoke(worksheet);
                workbook.SaveAs(stream);
            }
            stream.Position = 0;
            return stream;
        }

        private static void GerarCabecalhoPlanilha(IXLWorksheet planilha)
        {
            planilha.Cell(1, 1).Value = Constantes.NOME_DA_COLUNA_TITULO;
            planilha.Cell(1, 2).Value = Constantes.NOME_DA_COLUNA_TOMBO;
            planilha.Cell(1, 3).Value = Constantes.NOME_DA_COLUNA_CREDITO;
            planilha.Cell(1, 4).Value = Constantes.NOME_DA_COLUNA_LOCALIZACAO;
            planilha.Cell(1, 5).Value = Constantes.NOME_DA_COLUNA_PROCEDENCIA;
            planilha.Cell(1, 6).Value = Constantes.NOME_DA_COLUNA_ANO;
            planilha.Cell(1, 7).Value = Constantes.NOME_DA_COLUNA_COPIA_DIGITAL;
            planilha.Cell(1, 8).Value = Constantes.NOME_DA_COLUNA_AUTORIZACAO_USO_DE_IMAGEM;
            planilha.Cell(1, 9).Value = Constantes.NOME_DA_COLUNA_ESTADO_DE_CONSERVACAO;
            planilha.Cell(1, 10).Value = Constantes.NOME_DA_COLUNA_CROMIA;
            planilha.Cell(1, 11).Value = Constantes.NOME_DA_COLUNA_DIMENSAO_LARGURA;
            planilha.Cell(1, 12).Value = Constantes.NOME_DA_COLUNA_DIMENSAO_ALTURA;
            planilha.Cell(1, 13).Value = Constantes.NOME_DA_COLUNA_DIMENSAO_DIAMETRO;
            planilha.Cell(1, 14).Value = Constantes.NOME_DA_COLUNA_TECNICA;
            planilha.Cell(1, 15).Value = Constantes.NOME_DA_COLUNA_SUPORTE;
            planilha.Cell(1, 16).Value = Constantes.NOME_DA_COLUNA_QUANTIDADE;
            planilha.Cell(1, 17).Value = Constantes.NOME_DA_COLUNA_DESCRICAO;
        }
    }
}
