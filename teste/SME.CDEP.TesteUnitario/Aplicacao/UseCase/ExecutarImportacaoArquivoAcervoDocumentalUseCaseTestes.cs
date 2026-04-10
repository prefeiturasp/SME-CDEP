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
    public class ExecutarImportacaoArquivoAcervoDocumentalUseCaseTestes
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
        private readonly Mock<IServicoAcervoDocumental> servicoAcervoDocumentalMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IRepositorioParametroSistema> repositorioParametroSistemaMock;
        private readonly ExecutarImportacaoArquivoAcervoDocumentalUseCase sut;

        public ExecutarImportacaoArquivoAcervoDocumentalUseCaseTestes()
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
            servicoAcervoDocumentalMock = mocker.GetMock<IServicoAcervoDocumental>();
            mapperMock = mocker.GetMock<IMapper>();
            repositorioParametroSistemaMock = mocker.GetMock<IRepositorioParametroSistema>();

            sut = mocker.CreateInstance<ExecutarImportacaoArquivoAcervoDocumentalUseCase>();

            ConfigurarMocksPadroesDominios();
        }

        [Fact]
        public void DadoDependenciasValidas_QuandoInstanciarUseCase_EntaoRetornaInstanciaComSucesso()
        {
            // Arrange
            Action acao = () => new ExecutarImportacaoArquivoAcervoDocumentalUseCase(
                repositorioImportacaoArquivoMock.Object, servicoMaterialMock.Object, servicoEditoraMock.Object,
                servicoSerieColecaoMock.Object, servicoIdiomaMock.Object, servicoAssuntoMock.Object,
                servicoCreditoAutorMock.Object, servicoConservacaoMock.Object, servicoAcessoDocumentoMock.Object,
                servicoCromiaMock.Object, servicoSuporteMock.Object, servicoFormatoMock.Object,
                servicoAcervoDocumentalMock.Object, mapperMock.Object, repositorioParametroSistemaMock.Object);

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
            var mensagemRabbit = new MensagemRabbit { Mensagem = "ID_INVALIDO" };

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
            var idImportacao = 99L;
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

            var linhas = new List<AcervoDocumentalLinhaDTO> { ObterLinhaDocumentalDTOPreenchida(1) };
            var arquivoImportado = new ImportacaoArquivo
            {
                Id = idImportacao,
                Conteudo = JsonConvert.SerializeObject(linhas),
                TipoAcervo = TipoAcervo.DocumentacaoTextual
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
        public async Task DadoChamadaCarregarDominios_QuandoExecutado_EntaoCarregaDependenciasDocumentais()
        {
            // Act
            await sut.CarregarDominiosDocumentais();

            // Assert
            servicoMaterialMock.Verify(s => s.ObterTodos(), Times.Once);
            servicoCreditoAutorMock.Verify(s => s.ObterTodos(), Times.Once);
            repositorioParametroSistemaMock.Verify(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.LimiteAcervosImportadosViaPanilha, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DadoLinhasSemErro_QuandoPersistenciaAcervo_EntaoInsereAcervoEDefineComoSucesso()
        {
            // Arrange
            await sut.CarregarDominiosDocumentais(); // Carrega os domínios para a validação dos Ids

            var linhaValida = ObterLinhaDocumentalDTOPreenchida(1);
            linhaValida.PossuiErros = false;

            var linhas = new List<AcervoDocumentalLinhaDTO> { linhaValida };

            servicoAcervoDocumentalMock
                .Setup(s => s.Inserir(It.IsAny<AcervoDocumentalCadastroDTO>()))
                .ReturnsAsync(1L);

            // Act
            await sut.PersistenciaAcervo(linhas);

            // Assert
            servicoAcervoDocumentalMock.Verify(s => s.Inserir(It.IsAny<AcervoDocumentalCadastroDTO>()), Times.Once);
            linhaValida.PossuiErros.Should().BeFalse();
            linhaValida.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact]
        public async Task DadoErroNaInsercaoAcervo_QuandoPersistenciaAcervo_EntaoDefineLinhaComoErro()
        {
            // Arrange
            await sut.CarregarDominiosDocumentais(); // Carrega os domínios para evitar a exceção de domínio não encontrado

            var linhaValida = ObterLinhaDocumentalDTOPreenchida(1);
            linhaValida.PossuiErros = false;

            var linhas = new List<AcervoDocumentalLinhaDTO> { linhaValida };
            var mensagemErroBanco = "Erro de banco de dados";

            servicoAcervoDocumentalMock
                .Setup(s => s.Inserir(It.IsAny<AcervoDocumentalCadastroDTO>()))
                .ThrowsAsync(new Exception(mensagemErroBanco));

            // Act
            await sut.PersistenciaAcervo(linhas);

            // Assert
            linhaValida.PossuiErros.Should().BeTrue();
            linhaValida.Status.Should().Be(ImportacaoStatus.Erros);
            linhaValida.Mensagem.Should().Be(mensagemErroBanco);
        }

        [Fact]
        public async Task DadoLinhasComCodigosVazios_QuandoValidarPreenchimento_EntaoDefineErroNosDoisCodigos()
        {
            // Arrange
            await sut.CarregarDominiosDocumentais();

            var linhaInvalida = ObterLinhaDocumentalDTOPreenchida(1);
            linhaInvalida.Codigo.Conteudo = string.Empty;
            linhaInvalida.CodigoNovo.Conteudo = string.Empty;

            var linhas = new List<AcervoDocumentalLinhaDTO> { linhaInvalida };

            // Act
            sut.ValidarPreenchimentoValorFormatoQtdeCaracteres(linhas);

            // Assert
            linhaInvalida.Codigo.PossuiErro.Should().BeTrue();
            linhaInvalida.Codigo.Mensagem.Should().Contain(Constantes.CODIGO_ANTIGO);

            linhaInvalida.CodigoNovo.PossuiErro.Should().BeTrue();
            linhaInvalida.CodigoNovo.Mensagem.Should().Contain(Constantes.CODIGO_NOVO);

            linhaInvalida.PossuiErros.Should().BeTrue();
        }

        [Fact]
        public async Task DadoExcecaoNaoTratadaNaValidacao_QuandoValidarPreenchimento_EntaoDefineLinhaComoErro()
        {
            // Arrange
            await sut.CarregarDominiosDocumentais();

            var linhaComFalhaGrave = new AcervoDocumentalLinhaDTO
            {
                NumeroLinha = 1,
                Titulo = null! // Forçará NullReferenceException durante a validação do Titulo
            };

            var linhas = new List<AcervoDocumentalLinhaDTO> { linhaComFalhaGrave };

            // Act
            sut.ValidarPreenchimentoValorFormatoQtdeCaracteres(linhas);

            // Assert
            linhaComFalhaGrave.PossuiErros.Should().BeTrue();
            linhaComFalhaGrave.Status.Should().Be(ImportacaoStatus.Erros);
            linhaComFalhaGrave.Mensagem.Should().Contain("Ocorreu uma falha inesperada na linha '1'");
        }

        // ================= MÉTODOS PRIVADOS AUXILIARES ================= //

        private void ConfigurarMocksPadroesDominios()
        {
            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "100" });

            // Instanciando os tipos corretos esperados pelas interfaces para evitar erro de Dynamic Dispatch
            var listaIdioma = new List<IdNomeExcluidoDTO> { new IdNomeExcluidoDTO { Id = 1L, Nome = "Português" } };
            servicoIdiomaMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaIdioma);

            var listaMaterial = new List<IdNomeTipoExcluidoDTO> { new IdNomeTipoExcluidoDTO { Id = 2L, Nome = "Papel", Tipo = (int)TipoMaterial.DOCUMENTAL } };
            servicoMaterialMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaMaterial);

            var listaCredito = new List<IdNomeTipoExcluidoAuditavelDTO> { new IdNomeTipoExcluidoAuditavelDTO { Id = 3L, Nome = "Autor Teste", Tipo = (int)TipoCreditoAutoria.Autoria } };
            servicoCreditoAutorMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaCredito);

            var listaAcesso = new List<IdNomeExcluidoDTO> { new IdNomeExcluidoDTO { Id = 4L, Nome = "Público" } };
            servicoAcessoDocumentoMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaAcesso);

            var listaConservacao = new List<IdNomeExcluidoDTO> { new IdNomeExcluidoDTO { Id = 5L, Nome = "Bom" } };
            servicoConservacaoMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaConservacao);

            // Mockando Retornos genéricos vazios com seus tipos concretos
            servicoSuporteMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoDTO>());
            servicoCromiaMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoDTO>());
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

        private AcervoDocumentalLinhaDTO ObterLinhaDocumentalDTOPreenchida(int numeroLinha)
        {
            return new AcervoDocumentalLinhaDTO
            {
                NumeroLinha = numeroLinha,
                PossuiErros = false,
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Documento Histórico", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = "COD-123", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                CodigoNovo = new LinhaConteudoAjustarDTO { Conteudo = "NCOD-123", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                Material = new LinhaConteudoAjustarDTO { Conteudo = "Papel", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                Idioma = new LinhaConteudoAjustarDTO { Conteudo = "Português", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                Autor = new LinhaConteudoAjustarDTO { Conteudo = "Autor Teste", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                Ano = new LinhaConteudoAjustarDTO { Conteudo = "2023", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                NumeroPaginas = new LinhaConteudoAjustarDTO { Conteudo = "100", FormatoTipoDeCampo = Constantes.FORMATO_INTEIRO },
                Volume = new LinhaConteudoAjustarDTO { Conteudo = "1", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                Descricao = new LinhaConteudoAjustarDTO { Conteudo = "Descricao", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                TipoAnexo = new LinhaConteudoAjustarDTO { Conteudo = "Anexo", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                Altura = new LinhaConteudoAjustarDTO { Conteudo = "29,7", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                Largura = new LinhaConteudoAjustarDTO { Conteudo = "21,0", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                TamanhoArquivo = new LinhaConteudoAjustarDTO { Conteudo = "1MB", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                AcessoDocumento = new LinhaConteudoAjustarDTO { Conteudo = "Público", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                Localizacao = new LinhaConteudoAjustarDTO { Conteudo = "Prateleira A", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                CopiaDigital = new LinhaConteudoAjustarDTO { Conteudo = "Sim", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                EstadoConservacao = new LinhaConteudoAjustarDTO { Conteudo = "Bom", FormatoTipoDeCampo = Constantes.FORMATO_STRING }
            };
        }
    }
}