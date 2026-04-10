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
    public class ExecutarImportacaoArquivoAcervoAudiovisualUseCaseTestes
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
        private readonly Mock<IServicoAcervoAudiovisual> servicoAcervoAudiovisualMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IRepositorioParametroSistema> repositorioParametroSistemaMock;
        private readonly ExecutarImportacaoArquivoAcervoAudiovisualUseCase sut;

        public ExecutarImportacaoArquivoAcervoAudiovisualUseCaseTestes()
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
            servicoAcervoAudiovisualMock = mocker.GetMock<IServicoAcervoAudiovisual>();
            mapperMock = mocker.GetMock<IMapper>();
            repositorioParametroSistemaMock = mocker.GetMock<IRepositorioParametroSistema>();

            sut = mocker.CreateInstance<ExecutarImportacaoArquivoAcervoAudiovisualUseCase>();

            ConfigurarMocksPadroesDominios();
        }

        [Fact]
        public void DadoDependenciasValidas_QuandoInstanciarUseCase_EntaoRetornaInstanciaComSucesso()
        {
            // Arrange
            Action acao = () => new ExecutarImportacaoArquivoAcervoAudiovisualUseCase(
                repositorioImportacaoArquivoMock.Object, servicoMaterialMock.Object, servicoEditoraMock.Object,
                servicoSerieColecaoMock.Object, servicoIdiomaMock.Object, servicoAssuntoMock.Object,
                servicoCreditoAutorMock.Object, servicoConservacaoMock.Object, servicoAcessoDocumentoMock.Object,
                servicoCromiaMock.Object, servicoSuporteMock.Object, servicoFormatoMock.Object,
                servicoAcervoAudiovisualMock.Object, mapperMock.Object, repositorioParametroSistemaMock.Object);

            // Act & Assert
            acao.Should().NotThrow();
            sut.Should().NotBeNull();
        }

        [Fact]
        public void DadoServicoAcervoAudiovisualNulo_QuandoInstanciarUseCase_EntaoLancaArgumentNullException()
        {
            // Arrange
            Action acao = () => new ExecutarImportacaoArquivoAcervoAudiovisualUseCase(
                repositorioImportacaoArquivoMock.Object, servicoMaterialMock.Object, servicoEditoraMock.Object,
                servicoSerieColecaoMock.Object, servicoIdiomaMock.Object, servicoAssuntoMock.Object,
                servicoCreditoAutorMock.Object, servicoConservacaoMock.Object, servicoAcessoDocumentoMock.Object,
                servicoCromiaMock.Object, servicoSuporteMock.Object, servicoFormatoMock.Object,
                null!, mapperMock.Object, repositorioParametroSistemaMock.Object);

            // Act & Assert
            acao.Should().Throw<ArgumentNullException>().WithParameterName("servicoAcervoAudiovisual");
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
            var mensagemRabbit = new MensagemRabbit { Mensagem = "INVALIDO_NAO_NUMERICO" };

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

            var linhas = new List<AcervoAudiovisualLinhaDTO> { ObterLinhaAudiovisualDTOPreenchida(1) };
            var arquivoImportado = new ImportacaoArquivo
            {
                Id = idImportacao,
                Conteudo = JsonConvert.SerializeObject(linhas),
                TipoAcervo = TipoAcervo.Audiovisual
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

            var linhaComErro = ObterLinhaAudiovisualDTOPreenchida(1);

            linhaComErro.Suporte.Conteudo = "SUPORTE_INEXISTENTE";

            var linhas = new List<AcervoAudiovisualLinhaDTO> { linhaComErro };
            var arquivoImportado = new ImportacaoArquivo
            {
                Id = idImportacao,
                Conteudo = JsonConvert.SerializeObject(linhas),
                TipoAcervo = TipoAcervo.Audiovisual
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
        public async Task DadoChamadaCarregarDominios_QuandoExecutado_EntaoCarregaDependenciasAudiovisuais()
        {
            // Act
            await sut.CarregarDominiosAudiovisuais();

            // Assert
            servicoCreditoAutorMock.Verify(s => s.ObterTodos(), Times.Once);
            servicoSuporteMock.Verify(s => s.ObterTodos(), Times.Once);
            repositorioParametroSistemaMock.Verify(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.LimiteAcervosImportadosViaPanilha, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DadoLinhasSemErro_QuandoPersistenciaAcervo_EntaoInsereAcervoEDefineComoSucesso()
        {
            // Arrange
            await sut.CarregarDominiosAudiovisuais();

            var linhaValida = ObterLinhaAudiovisualDTOPreenchida(1);
            linhaValida.PossuiErros = false;

            var linhas = new List<AcervoAudiovisualLinhaDTO> { linhaValida };

            servicoAcervoAudiovisualMock
                .Setup(s => s.Inserir(It.IsAny<AcervoAudiovisualCadastroDTO>()))
                .ReturnsAsync(1L);

            // Act
            await sut.PersistenciaAcervo(linhas);

            // Assert
            servicoAcervoAudiovisualMock.Verify(s => s.Inserir(It.IsAny<AcervoAudiovisualCadastroDTO>()), Times.Once);
            linhaValida.PossuiErros.Should().BeFalse();
            linhaValida.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact]
        public async Task DadoErroNaInsercaoAcervo_QuandoPersistenciaAcervo_EntaoDefineLinhaComoErro()
        {
            // Arrange
            await sut.CarregarDominiosAudiovisuais();

            var linhaValida = ObterLinhaAudiovisualDTOPreenchida(1);
            linhaValida.PossuiErros = false;

            var linhas = new List<AcervoAudiovisualLinhaDTO> { linhaValida };
            var mensagemErro = "Falha ao gravar registro de acervo audiovisual";

            servicoAcervoAudiovisualMock
                .Setup(s => s.Inserir(It.IsAny<AcervoAudiovisualCadastroDTO>()))
                .ThrowsAsync(new Exception(mensagemErro));

            // Act
            await sut.PersistenciaAcervo(linhas);

            // Assert
            linhaValida.PossuiErros.Should().BeTrue();
            linhaValida.Status.Should().Be(ImportacaoStatus.Erros);
            linhaValida.Mensagem.Should().Be(mensagemErro);
        }

        [Fact]
        public async Task DadoLinhaComLimitesExcedidos_QuandoValidarPreenchimento_EntaoDefinePossuiErrosComoTrue()
        {
            // Arrange
            await sut.CarregarDominiosAudiovisuais();

            var linhaInvalida = ObterLinhaAudiovisualDTOPreenchida(1);
            linhaInvalida.Titulo.Conteudo = new string('A', 501); // Excede o limite provável de 500

            var linhas = new List<AcervoAudiovisualLinhaDTO> { linhaInvalida };

            // Act
            sut.ValidarPreenchimentoValorFormatoQtdeCaracteres(linhas);

            // Assert
            linhaInvalida.Titulo.PossuiErro.Should().BeTrue();
            linhaInvalida.PossuiErros.Should().BeTrue();
        }

        [Fact]
        public void DadoExcecaoNaoTratadaNaValidacao_QuandoValidarPreenchimento_EntaoDefineLinhaComoErroOuLancaExcecao()
        {
            // Arrange
            var linhaComFalhaGrave = new AcervoAudiovisualLinhaDTO
            {
                NumeroLinha = 1,
                Titulo = null!
            };

            var linhas = new List<AcervoAudiovisualLinhaDTO> { linhaComFalhaGrave };

            // Act
            sut.ValidarPreenchimentoValorFormatoQtdeCaracteres(linhas);

            // Assert
            linhaComFalhaGrave.PossuiErros.Should().BeTrue();
            linhaComFalhaGrave.Status.Should().Be(ImportacaoStatus.Erros);
            linhaComFalhaGrave.Mensagem.Should().Contain("Ocorreu uma falha inesperada na linha");
        }

        // ================= MÉTODOS PRIVADOS AUXILIARES ================= //

        private void ConfigurarMocksPadroesDominios()
        {
            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "100" });

            var listaCredito = new List<IdNomeTipoExcluidoAuditavelDTO> { new IdNomeTipoExcluidoAuditavelDTO { Id = 1L, Nome = "Autor Audiovisual", Tipo = (int)TipoCreditoAutoria.Credito } };
            servicoCreditoAutorMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaCredito);

            var listaSuportes = new List<IdNomeTipoExcluidoDTO> { new IdNomeTipoExcluidoDTO { Id = 2L, Nome = "Digital", Tipo = (int)TipoSuporte.VIDEO } };
            servicoSuporteMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaSuportes);

            var listaConservacao = new List<IdNomeExcluidoDTO> { new IdNomeExcluidoDTO { Id = 3L, Nome = "Bom" } };
            servicoConservacaoMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaConservacao);

            var listaCromia = new List<IdNomeExcluidoDTO> { new IdNomeExcluidoDTO { Id = 4L, Nome = "Colorido" } };
            servicoCromiaMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaCromia);

            servicoIdiomaMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoDTO>());
            servicoMaterialMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoDTO>());
            servicoAcessoDocumentoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoDTO>());
            servicoEditoraMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO>());
            servicoSerieColecaoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO>());
            servicoAssuntoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO>());
            servicoFormatoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoDTO>());

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

        private AcervoAudiovisualLinhaDTO ObterLinhaAudiovisualDTOPreenchida(int numeroLinha)
        {
            return new AcervoAudiovisualLinhaDTO
            {
                NumeroLinha = numeroLinha,
                PossuiErros = false,
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Obra Audiovisual", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 500 },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = "COD-AV1", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 15 },
                Credito = new LinhaConteudoAjustarDTO { Conteudo = "Autor Audiovisual", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 200 },
                Localizacao = new LinhaConteudoAjustarDTO { Conteudo = "Sala 2", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 100 },
                Procedencia = new LinhaConteudoAjustarDTO { Conteudo = "Doação", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 200 },
                Ano = new LinhaConteudoAjustarDTO { Conteudo = "2021", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 7 },
                Copia = new LinhaConteudoAjustarDTO { Conteudo = "Cópia 1", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 100 },
                PermiteUsoImagem = new LinhaConteudoAjustarDTO { Conteudo = "Sim", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 3 },
                EstadoConservacao = new LinhaConteudoAjustarDTO { Conteudo = "Bom", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 500 },
                Descricao = new LinhaConteudoAjustarDTO { Conteudo = "Descrição do vídeo", FormatoTipoDeCampo = Constantes.FORMATO_STRING },
                Suporte = new LinhaConteudoAjustarDTO { Conteudo = "Digital", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 500 },
                Duracao = new LinhaConteudoAjustarDTO { Conteudo = "01:30:00", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 15 },
                Cromia = new LinhaConteudoAjustarDTO { Conteudo = "Colorido", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 500 },
                TamanhoArquivo = new LinhaConteudoAjustarDTO { Conteudo = "2GB", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 15 },
                Acessibilidade = new LinhaConteudoAjustarDTO { Conteudo = "Libras", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 100 },
                Disponibilizacao = new LinhaConteudoAjustarDTO { Conteudo = "Internet", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 100 }
            };
        }
    }
}