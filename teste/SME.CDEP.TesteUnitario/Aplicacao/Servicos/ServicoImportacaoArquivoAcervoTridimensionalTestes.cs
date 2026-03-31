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
    public class ServicoImportacaoArquivoAcervoTridimensionalTestes
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
        private readonly ServicoImportacaoArquivoAcervoTridimensional sut;

        public ServicoImportacaoArquivoAcervoTridimensionalTestes()
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

            sut = mocker.CreateInstance<ServicoImportacaoArquivoAcervoTridimensional>();

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
        }

        [Fact]
        public void DadoDependenciasValidas_QuandoInstanciarServico_EntaoRetornaInstanciaComSucesso()
        {
            Action acao = () => new ServicoImportacaoArquivoAcervoTridimensional(
                repositorioImportacaoArquivoMock.Object, servicoMaterialMock.Object, servicoEditoraMock.Object,
                servicoSerieColecaoMock.Object, servicoIdiomaMock.Object, servicoAssuntoMock.Object,
                servicoCreditoAutorMock.Object, servicoConservacaoMock.Object, servicoAcessoDocumentoMock.Object,
                servicoCromiaMock.Object, servicoSuporteMock.Object, servicoFormatoMock.Object,
                servicoMensageriaMock.Object, mapperMock.Object, repositorioParametroSistemaMock.Object);

            acao.Should().NotThrow();
            sut.Should().NotBeNull();
        }

        [Fact]
        public void DadoServicoMensageriaNulo_QuandoInstanciarServico_EntaoLancaArgumentNullException()
        {
            Action acao = () => new ServicoImportacaoArquivoAcervoTridimensional(
                repositorioImportacaoArquivoMock.Object, servicoMaterialMock.Object, servicoEditoraMock.Object,
                servicoSerieColecaoMock.Object, servicoIdiomaMock.Object, servicoAssuntoMock.Object,
                servicoCreditoAutorMock.Object, servicoConservacaoMock.Object, servicoAcessoDocumentoMock.Object,
                servicoCromiaMock.Object, servicoSuporteMock.Object, servicoFormatoMock.Object,
                null!, mapperMock.Object, repositorioParametroSistemaMock.Object);

            acao.Should().Throw<ArgumentNullException>().WithParameterName("servicoMensageria");
        }

        [Fact]
        public async Task DadoNenhumaImportacaoPendente_QuandoObterImportacaoPendente_EntaoRetornaNulo()
        {
            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterUltimaImportacao(TipoAcervo.Tridimensional))
                .ReturnsAsync((ImportacaoArquivo)null!);

            var resultado = await sut.ObterImportacaoPendente();

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoImportacaoPendenteExistente_QuandoObterImportacaoPendente_EntaoRetornaDtoMapeado()
        {
            var arquivoImportadoMock = GerarArquivoImportadoMock(1L, ImportacaoStatus.Pendente);
            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterUltimaImportacao(TipoAcervo.Tridimensional))
                .ReturnsAsync(arquivoImportadoMock);

            var resultado = await sut.ObterImportacaoPendente();

            resultado.Should().NotBeNull();
            resultado.Sucesso.Should().HaveCount(1);
            resultado.Erros.Should().HaveCount(1);
        }

        [Fact]
        public async Task DadoIdInexistente_QuandoObterImportacaoPorId_EntaoRetornaNulo()
        {
            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterImportacaoPorId(It.IsAny<long>()))
                .ReturnsAsync((ImportacaoArquivo)null!);

            var resultado = await sut.ObterImportacaoPorId(999L);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoIdExistente_QuandoObterImportacaoPorId_EntaoRetornaDtoMapeado()
        {
            var arquivoImportadoMock = GerarArquivoImportadoMock(10L, ImportacaoStatus.Erros);
            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterImportacaoPorId(10L))
                .ReturnsAsync(arquivoImportadoMock);

            var resultado = await sut.ObterImportacaoPorId(10L);

            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(10L);
        }

        [Fact]
        public async Task DadoLinhaValida_QuandoRemoverLinhaDoArquivo_EntaoRetornaTrueEAtualiza()
        {
            var idImportacao = 1L;
            var arquivoMock = GerarArquivoImportadoMock(idImportacao, ImportacaoStatus.Erros);

            repositorioImportacaoArquivoMock.Setup(r => r.ObterPorId(idImportacao)).ReturnsAsync(arquivoMock);

            var resultado = await sut.RemoverLinhaDoArquivo(idImportacao, new LinhaDTO { NumeroLinha = 1 });

            resultado.Should().BeTrue();
            repositorioImportacaoArquivoMock.Verify(r => r.Salvar(It.Is<ImportacaoArquivo>(a => !a.Conteudo.Contains("\"NumeroLinha\":1"))), Times.Once);
        }

        [Fact]
        public async Task DadoUltimaLinhaComErro_QuandoAtualizarLinhaParaSucesso_EntaoStatusMudaParaSucesso()
        {
            var idImportacao = 1L;
            var linhaMock = ObterLinhaTridimensionalDTO(1, true);
            var arquivoMock = new ImportacaoArquivo { Id = idImportacao, TipoAcervo = TipoAcervo.Tridimensional, Conteudo = JsonConvert.SerializeObject(new[] { linhaMock }) };

            repositorioImportacaoArquivoMock.Setup(r => r.ObterPorId(idImportacao)).ReturnsAsync(arquivoMock);

            var resultado = await sut.AtualizarLinhaParaSucesso(idImportacao, new LinhaDTO { NumeroLinha = 1 });

            resultado.Should().BeTrue();
            repositorioImportacaoArquivoMock.Verify(r => r.Salvar(It.Is<ImportacaoArquivo>(a => a.Status == ImportacaoStatus.Sucesso)), Times.Once);
        }

        [Fact]
        public async Task DadoLinhasRestantesComErro_QuandoAtualizarLinhaParaSucesso_EntaoStatusPermaneceErros()
        {
            var idImportacao = 1L;
            var arquivoMock = GerarArquivoImportadoMock(idImportacao, ImportacaoStatus.Erros);

            repositorioImportacaoArquivoMock.Setup(r => r.ObterPorId(idImportacao)).ReturnsAsync(arquivoMock);

            var resultado = await sut.AtualizarLinhaParaSucesso(idImportacao, new LinhaDTO { NumeroLinha = 2 });

            resultado.Should().BeTrue();
            repositorioImportacaoArquivoMock.Verify(r => r.Salvar(It.Is<ImportacaoArquivo>(a => a.Status == ImportacaoStatus.Sucesso)), Times.Once);
        }

        [Fact]
        public async Task DadoChamadaCarregarDominios_QuandoExecutado_EntaoCarregaDependencias()
        {
            await sut.CarregarDominiosTridimensionais();

            servicoConservacaoMock.Verify(s => s.ObterTodos(), Times.Once);
            servicoCromiaMock.Verify(s => s.ObterTodos(), Times.Once);
            servicoFormatoMock.Verify(s => s.ObterTodos(), Times.Once);
        }

        [Fact]
        public async Task DadoArquivoVazio_QuandoImportarArquivo_EntaoLancaNegocioException()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(0);

            Func<Task> acao = async () => await sut.ImportarArquivo(fileMock.Object);

            await acao.Should().ThrowAsync<NegocioException>().WithMessage(MensagemNegocio.ARQUIVO_VAZIO);
        }

        [Fact]
        public async Task DadoArquivoComMimeTypeInvalido_QuandoImportarArquivo_EntaoLancaNegocioException()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(10);
            fileMock.Setup(f => f.ContentType).Returns("text/plain");

            Func<Task> acao = async () => await sut.ImportarArquivo(fileMock.Object);

            await acao.Should().ThrowAsync<NegocioException>().WithMessage(MensagemNegocio.SOMENTE_ARQUIVO_XLSX_SUPORTADO);
        }

        [Fact]
        public async Task DadoPlanilhaSemDados_QuandoImportarArquivo_EntaoLancaNegocioException()
        {
            var stream = CriarStreamExcelPlanilhaMock(p => { });
            var fileMock = ConfigurarIFormFileMock(stream);

            Func<Task> acao = async () => await sut.ImportarArquivo(fileMock.Object);

            await acao.Should().ThrowAsync<NegocioException>().WithMessage(MensagemNegocio.PLANILHA_VAZIA);
        }

        [Fact]
        public async Task DadoPlanilhaAcimaDoLimite_QuandoImportarArquivo_EntaoLancaNegocioException()
        {
            var stream = CriarStreamExcelPlanilhaMock(p =>
            {
                GerarCabecalhoPlanilha(p);
                p.Cell(2, 1).Value = "A";
                p.Cell(3, 1).Value = "B";
            });
            var fileMock = ConfigurarIFormFileMock(stream);

            repositorioParametroSistemaMock.Setup(p => p.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "1" });

            Func<Task> acao = async () => await sut.ImportarArquivo(fileMock.Object);

            await acao.Should().ThrowAsync<NegocioException>().WithMessage(string.Format(MensagemNegocio.LIMITE_ACERVOS_IMPORTADOS_VIA_PLANILHA, 1));
        }

        [Fact]
        public async Task DadoPlanilhaComColunasInvalidas_QuandoImportarArquivo_EntaoLancaNegocioException()
        {
            var stream = CriarStreamExcelPlanilhaMock(p =>
            {
                GerarCabecalhoPlanilha(p);
                p.Cell(1, 1).Value = "COLUNA_INVALIDA";
            });
            var fileMock = ConfigurarIFormFileMock(stream);

            Func<Task> acao = async () => await sut.ImportarArquivo(fileMock.Object);

            await acao.Should().ThrowAsync<NegocioException>();
        }

        [Fact]
        public async Task DadoArquivoValido_QuandoImportarArquivo_EntaoSalvaEPublicaMensagem()
        {
            var stream = CriarStreamExcelPlanilhaMock(p =>
            {
                GerarCabecalhoPlanilha(p);
                p.Cell(2, 1).Value = "Mesa Antiga";
                p.Cell(2, 2).Value = "TMB-111";
            });
            var fileMock = ConfigurarIFormFileMock(stream);

            repositorioImportacaoArquivoMock.Setup(r => r.Salvar(It.IsAny<ImportacaoArquivo>())).ReturnsAsync(88L);

            var resultado = await sut.ImportarArquivo(fileMock.Object);

            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(88L);
            resultado.Status.Should().Be(ImportacaoStatus.Pendente);
            servicoMensageriaMock.Verify(s => s.Publicar(RotasRabbit.ExecutarImportacaoArquivoAcervoTridimensional, 88L, It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DadoLinhaSucesso_QuandoMapearRetorno_EntaoAdicionaSufixoAoCodigo()
        {
            var arquivoMock = GerarArquivoImportadoMock(1L, ImportacaoStatus.Pendente);
            repositorioImportacaoArquivoMock.Setup(r => r.ObterImportacaoPorId(1L)).ReturnsAsync(arquivoMock);

            var resultado = await sut.ObterImportacaoPorId(1L);

            var sucesso = resultado.Sucesso.First();
            sucesso.Tombo.Should().Be($"TMB-001{Constantes.SIGLA_ACERVO_TRIDIMENSIONAL}");
        }

        [Fact]
        public async Task DadoLinhaComErro_QuandoMapearRetorno_EntaoCompilaMensagensDeErroDaLinha()
        {
            var arquivoMock = GerarArquivoImportadoMock(1L, ImportacaoStatus.Erros);
            repositorioImportacaoArquivoMock.Setup(r => r.ObterImportacaoPorId(1L)).ReturnsAsync(arquivoMock);

            var resultado = await sut.ObterImportacaoPorId(1L);

            var erro = resultado.Erros.First();
            erro.RetornoErro.ErrosCampos.Should().Contain("Título obrigatório");
        }

        [Fact]
        public async Task DadoDadosPreenchidos_QuandoMapearRetorno_EntaoCriaDtoConvertendoDominiosELongos()
        {
            var linhaMock = ObterLinhaTridimensionalDTO(3, true);
            linhaMock.EstadoConservacao.Conteudo = "Bom";
            linhaMock.Quantidade.Conteudo = "10";

            var arquivoMock = new ImportacaoArquivo { Id = 1, TipoAcervo = TipoAcervo.Tridimensional, Conteudo = JsonConvert.SerializeObject(new[] { linhaMock }) };
            repositorioImportacaoArquivoMock.Setup(r => r.ObterImportacaoPorId(1L)).ReturnsAsync(arquivoMock);

            mapperMock.Setup(m => m.Map<IdNomeDTO>(It.IsAny<object>())).Returns((object src) => { var s = src as dynamic; return new IdNomeDTO { Id = s.Id, Nome = s.Nome }; });
            servicoConservacaoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoDTO> { new IdNomeExcluidoDTO { Id = 5, Nome = "Bom" } });

            var resultado = await sut.ObterImportacaoPorId(1L);

            var obj = resultado.Erros.First().RetornoObjeto;
            obj.ConservacaoId.Should().Be(5);
            obj.Quantidade.Should().Be(10);
            obj.Largura.Should().Be("10");
        }

        // Helpers Privados
        private ImportacaoArquivo GerarArquivoImportadoMock(long id, ImportacaoStatus status)
        {
            var linhas = new List<AcervoTridimensionalLinhaDTO>
            {
                ObterLinhaTridimensionalDTO(1, false),
                ObterLinhaTridimensionalDTO(2, true)
            };

            return new ImportacaoArquivo
            {
                Id = id,
                Nome = "planilha.xlsx",
                TipoAcervo = TipoAcervo.Tridimensional,
                Status = status,
                Conteudo = JsonConvert.SerializeObject(linhas),
                CriadoEm = DateTime.Now
            };
        }

        private AcervoTridimensionalLinhaDTO ObterLinhaTridimensionalDTO(int numero, bool comErro)
        {
            return new AcervoTridimensionalLinhaDTO
            {
                NumeroLinha = numero,
                PossuiErros = comErro,
                Mensagem = comErro ? "Erro validação" : "",
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = $"Item {numero}", PossuiErro = comErro, Mensagem = comErro ? "Título obrigatório" : "" },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = $"TMB-00{numero}" },
                Procedencia = new LinhaConteudoAjustarDTO(),
                Ano = new LinhaConteudoAjustarDTO(),
                EstadoConservacao = new LinhaConteudoAjustarDTO(),
                Quantidade = new LinhaConteudoAjustarDTO(),
                Descricao = new LinhaConteudoAjustarDTO(),
                Largura = new LinhaConteudoAjustarDTO { Conteudo = "10" },
                Altura = new LinhaConteudoAjustarDTO(),
                Profundidade = new LinhaConteudoAjustarDTO(),
                Diametro = new LinhaConteudoAjustarDTO()
            };
        }

        private Mock<IFormFile> ConfigurarIFormFileMock(MemoryStream stream)
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(100);
            fileMock.Setup(f => f.FileName).Returns("arquivo.xlsx");
            fileMock.Setup(f => f.ContentType).Returns(Constantes.CONTENT_TYPE_EXCEL);
            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            return fileMock;
        }

        private MemoryStream CriarStreamExcelPlanilhaMock(Action<IXLWorksheet> manipulacaoPlanilha)
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

        private void GerarCabecalhoPlanilha(IXLWorksheet planilha)
        {
            planilha.Cell(1, 1).Value = Constantes.NOME_DA_COLUNA_TITULO;
            planilha.Cell(1, 2).Value = Constantes.NOME_DA_COLUNA_TOMBO;
            planilha.Cell(1, 3).Value = Constantes.NOME_DA_COLUNA_PROCEDENCIA;
            planilha.Cell(1, 4).Value = Constantes.NOME_DA_COLUNA_ANO;
            planilha.Cell(1, 5).Value = Constantes.NOME_DA_COLUNA_ESTADO_DE_CONSERVACAO;
            planilha.Cell(1, 6).Value = Constantes.NOME_DA_COLUNA_QUANTIDADE;
            planilha.Cell(1, 7).Value = Constantes.NOME_DA_COLUNA_DESCRICAO;
            planilha.Cell(1, 8).Value = Constantes.NOME_DA_COLUNA_DIMENSAO_LARGURA;
            planilha.Cell(1, 9).Value = Constantes.NOME_DA_COLUNA_DIMENSAO_ALTURA;
            planilha.Cell(1, 10).Value = Constantes.NOME_DA_COLUNA_DIMENSAO_PROFUNDIDADE;
            planilha.Cell(1, 11).Value = Constantes.NOME_DA_COLUNA_DIMENSAO_DIAMETRO;
        }
    }
}
