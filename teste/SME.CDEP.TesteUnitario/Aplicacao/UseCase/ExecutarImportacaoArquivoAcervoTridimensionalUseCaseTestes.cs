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
    public class ExecutarImportacaoArquivoAcervoTridimensionalUseCaseTestes
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
        private readonly Mock<IServicoAcervoTridimensional> servicoAcervoTridimensionalMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IRepositorioParametroSistema> repositorioParametroSistemaMock;
        private readonly ExecutarImportacaoArquivoAcervoTridimensionalUseCase sut;

        public ExecutarImportacaoArquivoAcervoTridimensionalUseCaseTestes()
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
            servicoAcervoTridimensionalMock = mocker.GetMock<IServicoAcervoTridimensional>();
            mapperMock = mocker.GetMock<IMapper>();
            repositorioParametroSistemaMock = mocker.GetMock<IRepositorioParametroSistema>();

            sut = mocker.CreateInstance<ExecutarImportacaoArquivoAcervoTridimensionalUseCase>();

            ConfigurarMocksPadroesDominios();
        }

        [Fact]
        public void DadoDependenciasValidas_QuandoInstanciarUseCase_EntaoRetornaInstanciaComSucesso()
        {
            // Arrange
            Action acao = () => new ExecutarImportacaoArquivoAcervoTridimensionalUseCase(
                repositorioImportacaoArquivoMock.Object, servicoMaterialMock.Object, servicoEditoraMock.Object,
                servicoSerieColecaoMock.Object, servicoIdiomaMock.Object, servicoAssuntoMock.Object,
                servicoCreditoAutorMock.Object, servicoConservacaoMock.Object, servicoAcessoDocumentoMock.Object,
                servicoCromiaMock.Object, servicoSuporteMock.Object, servicoFormatoMock.Object,
                servicoAcervoTridimensionalMock.Object, mapperMock.Object, repositorioParametroSistemaMock.Object);

            // Act & Assert
            acao.Should().NotThrow();
            sut.Should().NotBeNull();
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

            var linhas = new List<AcervoTridimensionalLinhaDTO> { ObterLinhaTridimensionalDTOPreenchida(1) };
            var arquivoImportado = new ImportacaoArquivo
            {
                Id = idImportacao,
                Conteudo = JsonConvert.SerializeObject(linhas),
                TipoAcervo = TipoAcervo.Tridimensional
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

            var linhaComErro = ObterLinhaTridimensionalDTOPreenchida(1);

            // Forçamos um erro na revalidação usando um domínio de conservação inexistente no mock
            linhaComErro.EstadoConservacao.Conteudo = "CONSERVACAO_INEXISTENTE";

            var linhas = new List<AcervoTridimensionalLinhaDTO> { linhaComErro };
            var arquivoImportado = new ImportacaoArquivo
            {
                Id = idImportacao,
                Conteudo = JsonConvert.SerializeObject(linhas),
                TipoAcervo = TipoAcervo.Tridimensional
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
        public async Task DadoChamadaCarregarDominios_QuandoExecutado_EntaoCarregaDependenciasGerais()
        {
            // Arrange (Mocks já configurados no construtor)

            // Act
            await sut.CarregarDominiosTridimensionais();

            // Assert
            servicoConservacaoMock.Verify(s => s.ObterTodos(), Times.Once);
            repositorioParametroSistemaMock.Verify(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.LimiteAcervosImportadosViaPanilha, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DadoLinhasSemErro_QuandoPersistenciaAcervo_EntaoInsereAcervoEDefineComoSucesso()
        {
            // Arrange
            await sut.CarregarDominiosTridimensionais();

            var linhaValida = ObterLinhaTridimensionalDTOPreenchida(1);
            linhaValida.PossuiErros = false;

            var linhas = new List<AcervoTridimensionalLinhaDTO> { linhaValida };

            servicoAcervoTridimensionalMock
                .Setup(s => s.Inserir(It.IsAny<AcervoTridimensionalCadastroDTO>()))
                .ReturnsAsync(1L);

            // Act
            await sut.PersistenciaAcervo(linhas);

            // Assert
            servicoAcervoTridimensionalMock.Verify(s => s.Inserir(It.IsAny<AcervoTridimensionalCadastroDTO>()), Times.Once);
            linhaValida.PossuiErros.Should().BeFalse();
            linhaValida.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact]
        public async Task DadoErroNaInsercaoAcervo_QuandoPersistenciaAcervo_EntaoDefineLinhaComoErro()
        {
            // Arrange
            await sut.CarregarDominiosTridimensionais();

            var linhaValida = ObterLinhaTridimensionalDTOPreenchida(1);
            linhaValida.PossuiErros = false;

            var linhas = new List<AcervoTridimensionalLinhaDTO> { linhaValida };
            var mensagemErro = "Erro interno ao salvar no banco";

            servicoAcervoTridimensionalMock
                .Setup(s => s.Inserir(It.IsAny<AcervoTridimensionalCadastroDTO>()))
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
            var linhaComFalhaGrave = new AcervoTridimensionalLinhaDTO
            {
                NumeroLinha = 1,
                // NullReferenceException forçado pela falta da instanciação
                Titulo = null!
            };

            var linhas = new List<AcervoTridimensionalLinhaDTO> { linhaComFalhaGrave };

            // Act
            sut.ValidarPreenchimentoValorFormatoQtdeCaracteres(linhas);

            // Assert
            linhaComFalhaGrave.PossuiErros.Should().BeTrue();
            linhaComFalhaGrave.Status.Should().Be(ImportacaoStatus.Erros);
            linhaComFalhaGrave.Mensagem.Should().Contain("Ocorreu uma falha inesperada na linha");
        }

        [Fact]
        public void DadoLinhaComLimitesExcedidos_QuandoValidarPreenchimento_EntaoDefinePossuiErrosComoTrue()
        {
            // Arrange
            var linhaInvalida = ObterLinhaTridimensionalDTOPreenchida(1);
            linhaInvalida.Titulo.Conteudo = new string('A', 501); // Excede limite típico de 500

            var linhas = new List<AcervoTridimensionalLinhaDTO> { linhaInvalida };

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

            var listaConservacao = new List<IdNomeExcluidoDTO> { new IdNomeExcluidoDTO { Id = 1L, Nome = "Bom" } };
            servicoConservacaoMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaConservacao);

            // Mockando Retornos genéricos vazios com seus tipos concretos para o restante das interfaces base
            servicoCreditoAutorMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoAuditavelDTO>());
            servicoSuporteMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoDTO>());
            servicoCromiaMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoDTO>());
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

        private AcervoTridimensionalLinhaDTO ObterLinhaTridimensionalDTOPreenchida(int numeroLinha)
        {
            return new AcervoTridimensionalLinhaDTO
            {
                NumeroLinha = numeroLinha,
                PossuiErros = false,
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Peça Histórica", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 500 },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = "COD-TR1", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 15 },
                Procedencia = new LinhaConteudoAjustarDTO { Conteudo = "Doação", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 200 },
                Ano = new LinhaConteudoAjustarDTO { Conteudo = "2020", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 7 },
                EstadoConservacao = new LinhaConteudoAjustarDTO { Conteudo = "Bom", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 500 },
                Quantidade = new LinhaConteudoAjustarDTO { Conteudo = "1", FormatoTipoDeCampo = Constantes.FORMATO_INTEIRO, LimiteCaracteres = 10 },
                Descricao = new LinhaConteudoAjustarDTO { Conteudo = "Descrição da peça", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 500 },
                Largura = new LinhaConteudoAjustarDTO { Conteudo = "50.0", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 15 },
                Altura = new LinhaConteudoAjustarDTO { Conteudo = "100.0", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 15 },
                Profundidade = new LinhaConteudoAjustarDTO { Conteudo = "40.0", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 15 },
                Diametro = new LinhaConteudoAjustarDTO { Conteudo = "0", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 15 }
            };
        }
    }
}