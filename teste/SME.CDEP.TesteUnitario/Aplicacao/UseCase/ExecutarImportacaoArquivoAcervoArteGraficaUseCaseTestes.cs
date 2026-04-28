using AutoMapper;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase
{
    public class ExecutarImportacaoArquivoAcervoArteGraficaUseCaseTestes
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
        private readonly Mock<IServicoAcervoArteGrafica> servicoAcervoArteGraficaMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IRepositorioParametroSistema> repositorioParametroSistemaMock;
        private readonly ExecutarImportacaoArquivoAcervoArteGraficaUseCase sut;

        public ExecutarImportacaoArquivoAcervoArteGraficaUseCaseTestes()
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
            servicoAcervoArteGraficaMock = mocker.GetMock<IServicoAcervoArteGrafica>();
            mapperMock = mocker.GetMock<IMapper>();
            repositorioParametroSistemaMock = mocker.GetMock<IRepositorioParametroSistema>();

            sut = mocker.CreateInstance<ExecutarImportacaoArquivoAcervoArteGraficaUseCase>();

            ConfigurarMocksPadroesDominios();
        }

        [Fact]
        public void DadoDependenciasValidas_QuandoInstanciarUseCase_EntaoRetornaInstanciaComSucesso()
        {
            // Arrange
            Action acao = () => new ExecutarImportacaoArquivoAcervoArteGraficaUseCase(
                repositorioImportacaoArquivoMock.Object, servicoMaterialMock.Object, servicoEditoraMock.Object,
                servicoSerieColecaoMock.Object, servicoIdiomaMock.Object, servicoAssuntoMock.Object,
                servicoCreditoAutorMock.Object, servicoConservacaoMock.Object, servicoAcessoDocumentoMock.Object,
                servicoCromiaMock.Object, servicoSuporteMock.Object, servicoFormatoMock.Object,
                servicoAcervoArteGraficaMock.Object, mapperMock.Object, repositorioParametroSistemaMock.Object);

            // Act & Assert
            acao.Should().NotThrow();
            sut.Should().NotBeNull();
        }

        [Fact]
        public void DadoServicoAcervoArteGraficaNulo_QuandoInstanciarUseCase_EntaoLancaArgumentNullException()
        {
            // Arrange
            Action acao = () => new ExecutarImportacaoArquivoAcervoArteGraficaUseCase(
                repositorioImportacaoArquivoMock.Object, servicoMaterialMock.Object, servicoEditoraMock.Object,
                servicoSerieColecaoMock.Object, servicoIdiomaMock.Object, servicoAssuntoMock.Object,
                servicoCreditoAutorMock.Object, servicoConservacaoMock.Object, servicoAcessoDocumentoMock.Object,
                servicoCromiaMock.Object, servicoSuporteMock.Object, servicoFormatoMock.Object,
                null!, mapperMock.Object, repositorioParametroSistemaMock.Object);

            // Act & Assert
            acao.Should().Throw<ArgumentNullException>().WithParameterName("servicoAcervoArteGrafica");
        }

        [Fact]
        public async Task DadoMensagemRabbitComParametroNulo_QuandoExecutar_EntaoLancaNegocioException()
        {
            // Arrange
            var mensagemRabbit = new MensagemRabbit { Mensagem = null! };

            // Act
            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.PARAMETROS_INVALIDOS);
        }

        [Fact]
        public async Task DadoMensagemRabbitComParametroNaoNumerico_QuandoExecutar_EntaoLancaNegocioException()
        {
            // Arrange
            var mensagemRabbit = new MensagemRabbit { Mensagem = "NaoNumerico" };

            // Act
            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.PARAMETROS_INVALIDOS);
        }

        [Fact]
        public async Task DadoImportacaoNaoLocalizada_QuandoExecutar_EntaoLancaNegocioException()
        {
            // Arrange
            var idImportacao = 10L;
            var mensagemRabbit = new MensagemRabbit { Mensagem = idImportacao.ToString() };

            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterPorId(idImportacao))
                .ReturnsAsync((ImportacaoArquivo)null!);

            // Act
            Func<Task> acao = async () => await sut.Executar(mensagemRabbit);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.IMPORTACAO_NAO_LOCALIZADA);
        }

        [Fact]
        public async Task DadoImportacaoComLinhasComSucesso_QuandoExecutar_EntaoAtualizaStatusParaSucesso()
        {
            // Arrange
            var idImportacao = 1L;
            var mensagemRabbit = new MensagemRabbit { Mensagem = idImportacao.ToString() };

            var linhas = new List<AcervoArteGraficaLinhaDTO> { ObterLinhaArteGraficaDTOPreenchida(1) };
            var arquivoImportado = new ImportacaoArquivo
            {
                Id = idImportacao,
                Conteudo = JsonConvert.SerializeObject(linhas),
                TipoAcervo = TipoAcervo.ArtesGraficas
            };

            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterPorId(idImportacao))
                .ReturnsAsync(arquivoImportado);

            // Act
            var resultado = await sut.Executar(mensagemRabbit);

            // Assert
            resultado.Should().BeTrue();
            repositorioImportacaoArquivoMock.Verify(r => r.Salvar(It.Is<ImportacaoArquivo>(a =>
                a.Status == ImportacaoStatus.Sucesso)), Times.Once);
        }

        [Fact]
        public async Task DadoImportacaoComLinhasComErro_QuandoExecutar_EntaoAtualizaStatusParaErros()
        {
            // Arrange
            var idImportacao = 1L;
            var mensagemRabbit = new MensagemRabbit { Mensagem = idImportacao.ToString() };

            var linhaComErro = ObterLinhaArteGraficaDTOPreenchida(1);

            // Forçamos um erro de negócio atribuindo um valor que não existe nos mocks de domínio
            linhaComErro.Cromia.Conteudo = "CROMIA_INEXISTENTE";

            var linhas = new List<AcervoArteGraficaLinhaDTO> { linhaComErro };
            var arquivoImportado = new ImportacaoArquivo
            {
                Id = idImportacao,
                Conteudo = JsonConvert.SerializeObject(linhas),
                TipoAcervo = TipoAcervo.ArtesGraficas
            };

            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterPorId(idImportacao))
                .ReturnsAsync(arquivoImportado);

            // Act
            var resultado = await sut.Executar(mensagemRabbit);

            // Assert
            resultado.Should().BeTrue();
            repositorioImportacaoArquivoMock.Verify(r => r.Salvar(It.Is<ImportacaoArquivo>(a =>
                a.Status == ImportacaoStatus.Erros)), Times.Once);
        }

        [Fact]
        public async Task DadoChamadaCarregarDominios_QuandoExecutado_EntaoCarregaDependenciasArteGrafica()
        {
            // Arrange (Mocks já configurados no construtor)

            // Act
            await sut.CarregarDominiosArteGrafica();

            // Assert
            servicoCreditoAutorMock.Verify(s => s.ObterTodos(), Times.Once);
            servicoSuporteMock.Verify(s => s.ObterTodos(), Times.Once);
            repositorioParametroSistemaMock.Verify(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.LimiteAcervosImportadosViaPanilha, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DadoLinhasSemErro_QuandoPersistenciaAcervo_EntaoInsereAcervoEDefineComoSucesso()
        {
            // Arrange
            await sut.CarregarDominiosArteGrafica();

            var linhaValida = ObterLinhaArteGraficaDTOPreenchida(1);
            linhaValida.PossuiErros = false;

            var linhas = new List<AcervoArteGraficaLinhaDTO> { linhaValida };

            servicoAcervoArteGraficaMock
                .Setup(s => s.Inserir(It.IsAny<AcervoArteGraficaCadastroDTO>()))
                .ReturnsAsync(1L);

            // Act
            await sut.PersistenciaAcervo(linhas);

            // Assert
            servicoAcervoArteGraficaMock.Verify(s => s.Inserir(It.IsAny<AcervoArteGraficaCadastroDTO>()), Times.Once);
            linhaValida.PossuiErros.Should().BeFalse();
            linhaValida.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact]
        public async Task DadoErroNaInsercaoAcervo_QuandoPersistenciaAcervo_EntaoDefineLinhaComoErro()
        {
            // Arrange
            await sut.CarregarDominiosArteGrafica();

            var linhaValida = ObterLinhaArteGraficaDTOPreenchida(1);
            linhaValida.PossuiErros = false;

            var linhas = new List<AcervoArteGraficaLinhaDTO> { linhaValida };
            var mensagemErro = "Erro ao persistir banco";

            servicoAcervoArteGraficaMock
                .Setup(s => s.Inserir(It.IsAny<AcervoArteGraficaCadastroDTO>()))
                .ThrowsAsync(new Exception(mensagemErro));

            // Act
            await sut.PersistenciaAcervo(linhas);

            // Assert
            linhaValida.PossuiErros.Should().BeTrue();
            linhaValida.Status.Should().Be(ImportacaoStatus.Erros);
            linhaValida.Mensagem.Should().Be(mensagemErro);
        }

        [Fact]
        public void DadoExcecaoNaoTratadaNaValidacao_QuandoValidarPreenchimento_EntaoDefineLinhaComoErro()
        {
            // Arrange
            var linhaComFalhaGrave = new AcervoArteGraficaLinhaDTO
            {
                NumeroLinha = 1,
                // Provocará NullReferenceException durante a validação por não estar instanciado
                Titulo = null!
            };

            var linhas = new List<AcervoArteGraficaLinhaDTO> { linhaComFalhaGrave };

            // Act
            sut.ValidarPreenchimentoValorFormatoQtdeCaracteres(linhas);

            // Assert
            // O SUT captura internamente a exception e atualiza o objeto DTO
            linhaComFalhaGrave.PossuiErros.Should().BeTrue();
            linhaComFalhaGrave.Status.Should().Be(ImportacaoStatus.Erros);
            linhaComFalhaGrave.Mensagem.Should().Contain("Ocorreu uma falha inesperada na linha");
        }

        [Fact]
        public void DadoLinhaComLimitesExcedidos_QuandoValidarPreenchimento_EntaoDefinePossuiErrosComoTrue()
        {
            // Arrange
            var linhaInvalida = ObterLinhaArteGraficaDTOPreenchida(1);
            linhaInvalida.Titulo.Conteudo = new string('A', 501); // Excede o limite de 500

            var linhas = new List<AcervoArteGraficaLinhaDTO> { linhaInvalida };

            // Act
            sut.ValidarPreenchimentoValorFormatoQtdeCaracteres(linhas);

            // Assert
            linhaInvalida.Titulo.PossuiErro.Should().BeTrue();
            linhaInvalida.PossuiErros.Should().BeTrue();
        }

        // ================= MÉTODOS PRIVADOS AUXILIARES ================= //

        private void ConfigurarMocksPadroesDominios()
        {
            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "100" });

            var listaCredito = new List<IdNomeTipoExcluidoAuditavelDTO> { new IdNomeTipoExcluidoAuditavelDTO { Id = 1L, Nome = "Autor Grafico", Tipo = (int)TipoCreditoAutoria.Credito } };
            servicoCreditoAutorMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaCredito);

            var listaSuportes = new List<IdNomeTipoExcluidoDTO> { new IdNomeTipoExcluidoDTO { Id = 2L, Nome = "Tela", Tipo = (int)TipoSuporte.IMAGEM } };
            servicoSuporteMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaSuportes);

            var listaConservacao = new List<IdNomeExcluidoDTO> { new IdNomeExcluidoDTO { Id = 3L, Nome = "Bom" } };
            servicoConservacaoMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaConservacao);

            var listaCromia = new List<IdNomeExcluidoDTO> { new IdNomeExcluidoDTO { Id = 4L, Nome = "Monocromático" } };
            servicoCromiaMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaCromia);

            // Mockando Retornos genéricos vazios com seus tipos concretos para o restante
            servicoIdiomaMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoDTO>());
            servicoMaterialMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoDTO>());
            servicoAcessoDocumentoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoDTO>());
            servicoEditoraMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO>());
            servicoSerieColecaoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO>());
            servicoAssuntoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO>());
            servicoFormatoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoDTO>());

            // Mapeador simulando o comportamento do AutoMapper
            mapperMock.Setup(m => m.Map<IdNomeDTO>(It.IsAny<object>())).Returns((object src) =>
            {
                var s = src as dynamic;
                return new IdNomeDTO { Id = s.Id, Nome = s.Nome };
            });

            mapperMock.Setup(m => m.Map<IdNomeTipoDTO>(It.IsAny<object>())).Returns((object src) =>
            {
                var s = src as dynamic;
                return new IdNomeTipoDTO { Id = s.Id, Nome = s.Nome, Tipo = s.Tipo };
            });
        }

        private AcervoArteGraficaLinhaDTO ObterLinhaArteGraficaDTOPreenchida(int numeroLinha)
        {
            return new AcervoArteGraficaLinhaDTO
            {
                NumeroLinha = numeroLinha,
                PossuiErros = false,
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Obra Arte", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 500 },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = "COD-AG1", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 15 },
                Credito = new LinhaConteudoAjustarDTO { Conteudo = "Autor Grafico", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 200 },
                Localizacao = new LinhaConteudoAjustarDTO { Conteudo = "Sala 1", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 100 },
                Procedencia = new LinhaConteudoAjustarDTO { Conteudo = "Doação", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 200 },
                Ano = new LinhaConteudoAjustarDTO { Conteudo = "2020", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 7 },
                CopiaDigital = new LinhaConteudoAjustarDTO { Conteudo = "Sim", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 3 },
                PermiteUsoImagem = new LinhaConteudoAjustarDTO { Conteudo = "Sim", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 3 },
                EstadoConservacao = new LinhaConteudoAjustarDTO { Conteudo = "Bom", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 500 },
                Cromia = new LinhaConteudoAjustarDTO { Conteudo = "Monocromático", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 500 },
                Largura = new LinhaConteudoAjustarDTO { Conteudo = "15.0", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                Altura = new LinhaConteudoAjustarDTO { Conteudo = "20.0", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                Diametro = new LinhaConteudoAjustarDTO { Conteudo = "0", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                Tecnica = new LinhaConteudoAjustarDTO { Conteudo = "Aquarela", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 100 },
                Suporte = new LinhaConteudoAjustarDTO { Conteudo = "Tela", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 500 },
                Quantidade = new LinhaConteudoAjustarDTO { Conteudo = "1", FormatoTipoDeCampo = Constantes.FORMATO_INTEIRO },
                Descricao = new LinhaConteudoAjustarDTO { Conteudo = "Descrição da obra", FormatoTipoDeCampo = Constantes.FORMATO_STRING }
            };
        }
    }
}