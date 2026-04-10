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
    public class ExecutarImportacaoArquivoAcervoFotograficoUseCaseTestes
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
        private readonly Mock<IServicoAcervoFotografico> servicoAcervoFotograficoMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IRepositorioParametroSistema> repositorioParametroSistemaMock;
        private readonly ExecutarImportacaoArquivoAcervoFotograficoUseCase sut;

        public ExecutarImportacaoArquivoAcervoFotograficoUseCaseTestes()
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
            servicoAcervoFotograficoMock = mocker.GetMock<IServicoAcervoFotografico>();
            mapperMock = mocker.GetMock<IMapper>();
            repositorioParametroSistemaMock = mocker.GetMock<IRepositorioParametroSistema>();

            sut = mocker.CreateInstance<ExecutarImportacaoArquivoAcervoFotograficoUseCase>();

            ConfigurarMocksPadroesDominios();
        }

        [Fact]
        public void DadoDependenciasValidas_QuandoInstanciarUseCase_EntaoRetornaInstanciaComSucesso()
        {
            // Arrange
            Action acao = () => new ExecutarImportacaoArquivoAcervoFotograficoUseCase(
                repositorioImportacaoArquivoMock.Object, servicoMaterialMock.Object, servicoEditoraMock.Object,
                servicoSerieColecaoMock.Object, servicoIdiomaMock.Object, servicoAssuntoMock.Object,
                servicoCreditoAutorMock.Object, servicoConservacaoMock.Object, servicoAcessoDocumentoMock.Object,
                servicoCromiaMock.Object, servicoSuporteMock.Object, servicoFormatoMock.Object,
                servicoAcervoFotograficoMock.Object, mapperMock.Object, repositorioParametroSistemaMock.Object);

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
            var mensagemRabbit = new MensagemRabbit { Mensagem = "INVALIDO" };

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

            var linhas = new List<AcervoFotograficoLinhaDTO> { ObterLinhaFotograficaDTOPreenchida(1) };
            var arquivoImportado = new ImportacaoArquivo
            {
                Id = idImportacao,
                Conteudo = JsonConvert.SerializeObject(linhas),
                TipoAcervo = TipoAcervo.Fotografico
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
        public async Task DadoChamadaCarregarDominios_QuandoExecutado_EntaoCarregaDependenciasFotograficas()
        {
            // Act
            await sut.CarregarDominiosFotograficos();

            // Assert
            servicoCreditoAutorMock.Verify(s => s.ObterTodos(), Times.Once);
            servicoFormatoMock.Verify(s => s.ObterTodos(), Times.Once);
            servicoSuporteMock.Verify(s => s.ObterTodos(), Times.Once);
            repositorioParametroSistemaMock.Verify(p => p.ObterParametroPorTipoEAno(TipoParametroSistema.LimiteAcervosImportadosViaPanilha, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DadoLinhasSemErro_QuandoPersistenciaAcervo_EntaoInsereAcervoEDefineComoSucesso()
        {
            // Arrange
            await sut.CarregarDominiosFotograficos();

            var linhaValida = ObterLinhaFotograficaDTOPreenchida(1);
            linhaValida.PossuiErros = false;

            var linhas = new List<AcervoFotograficoLinhaDTO> { linhaValida };

            servicoAcervoFotograficoMock
                .Setup(s => s.Inserir(It.IsAny<AcervoFotograficoCadastroDTO>()))
                .ReturnsAsync(1L);

            // Act
            await sut.PersistenciaAcervo(linhas);

            // Assert
            servicoAcervoFotograficoMock.Verify(s => s.Inserir(It.IsAny<AcervoFotograficoCadastroDTO>()), Times.Once);
            linhaValida.PossuiErros.Should().BeFalse();
            linhaValida.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact]
        public async Task DadoErroNaInsercaoAcervo_QuandoPersistenciaAcervo_EntaoDefineLinhaComoErro()
        {
            // Arrange
            await sut.CarregarDominiosFotograficos();

            var linhaValida = ObterLinhaFotograficaDTOPreenchida(1);
            linhaValida.PossuiErros = false;

            var linhas = new List<AcervoFotograficoLinhaDTO> { linhaValida };
            var mensagemErro = "Erro ao persistir imagem";

            servicoAcervoFotograficoMock
                .Setup(s => s.Inserir(It.IsAny<AcervoFotograficoCadastroDTO>()))
                .ThrowsAsync(new Exception(mensagemErro));

            // Act
            await sut.PersistenciaAcervo(linhas);

            // Assert
            linhaValida.PossuiErros.Should().BeTrue();
            linhaValida.Status.Should().Be(ImportacaoStatus.Erros);
            linhaValida.Mensagem.Should().Be(mensagemErro);
        }

        [Fact]
        public async Task DadoLinhaInvalidaComLimitesExcedidos_QuandoValidarPreenchimento_EntaoDefinePossuiErrosComoTrue()
        {
            // Arrange
            await sut.CarregarDominiosFotograficos();

            var linhaInvalida = ObterLinhaFotograficaDTOPreenchida(1);
            linhaInvalida.Titulo.Conteudo = new string('A', 501); // Excede o limite de 500 caracteres

            var linhas = new List<AcervoFotograficoLinhaDTO> { linhaInvalida };

            // Act
            sut.ValidarPreenchimentoValorFormatoQtdeCaracteres(linhas);

            // Assert
            linhaInvalida.Titulo.PossuiErro.Should().BeTrue();
            linhaInvalida.Titulo.Mensagem.Should().Contain(Constantes.TITULO);
            linhaInvalida.PossuiErros.Should().BeTrue();
        }

        [Fact]
        public async Task DadoExcecaoNaoTratadaNaValidacao_QuandoValidarPreenchimento_EntaoDefineLinhaComoErroOuLancaExcecao()
        {
            // Arrange
            await sut.CarregarDominiosFotograficos();

            var linhaComFalhaGrave = new AcervoFotograficoLinhaDTO
            {
                NumeroLinha = 1,
                Titulo = null! // NullReferenceException forcado
            };

            var linhas = new List<AcervoFotograficoLinhaDTO> { linhaComFalhaGrave };

            // Act
            Action acao = () => sut.ValidarPreenchimentoValorFormatoQtdeCaracteres(linhas);

            // Assert
            // Se o Catch conseguir capturar, e usar o string.Format com 1 parametro p/ msg de 2. 
            // Ele lançará um FormatException dentro do catch do SUT.
            acao.Should().Throw<Exception>();
        }

        // ================= MÉTODOS PRIVADOS AUXILIARES ================= //

        private void ConfigurarMocksPadroesDominios()
        {
            repositorioParametroSistemaMock
                .Setup(p => p.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "100" });

            // Instanciando os tipos corretos esperados pelas interfaces para evitar erro de Dynamic Dispatch
            var listaCredito = new List<IdNomeTipoExcluidoAuditavelDTO> { new IdNomeTipoExcluidoAuditavelDTO { Id = 1L, Nome = "Autor Foto", Tipo = (int)TipoCreditoAutoria.Credito } };
            servicoCreditoAutorMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaCredito);

            var listaFormatos = new List<IdNomeTipoExcluidoDTO> { new IdNomeTipoExcluidoDTO { Id = 2L, Nome = "JPEG", Tipo = (int)TipoFormato.ACERVO_FOTOS } };
            servicoFormatoMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaFormatos);

            var listaSuportes = new List<IdNomeTipoExcluidoDTO> { new IdNomeTipoExcluidoDTO { Id = 3L, Nome = "Digital", Tipo = (int)TipoSuporte.IMAGEM } };
            servicoSuporteMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaSuportes);

            var listaConservacao = new List<IdNomeExcluidoDTO> { new IdNomeExcluidoDTO { Id = 4L, Nome = "Bom" } };
            servicoConservacaoMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaConservacao);

            var listaCromia = new List<IdNomeExcluidoDTO> { new IdNomeExcluidoDTO { Id = 5L, Nome = "Colorido" } };
            servicoCromiaMock.Setup(s => s.ObterTodos()).ReturnsAsync(listaCromia);

            // Mockando Retornos genéricos vazios com seus tipos concretos para o restante
            servicoIdiomaMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoDTO>());
            servicoMaterialMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoDTO>());
            servicoAcessoDocumentoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoDTO>());
            servicoEditoraMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO>());
            servicoSerieColecaoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO>());
            servicoAssuntoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO>());

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

        private AcervoFotograficoLinhaDTO ObterLinhaFotograficaDTOPreenchida(int numeroLinha)
        {
            return new AcervoFotograficoLinhaDTO
            {
                NumeroLinha = numeroLinha,
                PossuiErros = false,
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Foto Antiga", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 500 },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = "COD-001", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 15 },
                Credito = new LinhaConteudoAjustarDTO { Conteudo = "Autor Foto", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 200 },
                Localizacao = new LinhaConteudoAjustarDTO { Conteudo = "Gaveta A", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 100 },
                Procedencia = new LinhaConteudoAjustarDTO { Conteudo = "Doação", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 200 },
                Data = new LinhaConteudoAjustarDTO { Conteudo = "01/01/1990", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 10 },
                CopiaDigital = new LinhaConteudoAjustarDTO { Conteudo = "Sim", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 3 },
                PermiteUsoImagem = new LinhaConteudoAjustarDTO { Conteudo = "Sim", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 3 },
                EstadoConservacao = new LinhaConteudoAjustarDTO { Conteudo = "Bom", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 500 },
                Descricao = new LinhaConteudoAjustarDTO { Conteudo = "Imagem de teste", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 500 },
                Quantidade = new LinhaConteudoAjustarDTO { Conteudo = "1", FormatoTipoDeCampo = Constantes.FORMATO_INTEIRO, LimiteCaracteres = 10 },
                Largura = new LinhaConteudoAjustarDTO { Conteudo = "10.0", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 10 },
                Altura = new LinhaConteudoAjustarDTO { Conteudo = "15.0", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 10 },
                Suporte = new LinhaConteudoAjustarDTO { Conteudo = "Digital", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 50 },
                FormatoImagem = new LinhaConteudoAjustarDTO { Conteudo = "JPEG", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 50 },
                TamanhoArquivo = new LinhaConteudoAjustarDTO { Conteudo = "2MB", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 15 },
                Cromia = new LinhaConteudoAjustarDTO { Conteudo = "Colorido", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 50 },
                Resolucao = new LinhaConteudoAjustarDTO { Conteudo = "1080p", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 50 },
                Ano = new LinhaConteudoAjustarDTO { Conteudo = "1990", FormatoTipoDeCampo = Constantes.FORMATO_STRING, LimiteCaracteres = 4 }
            };
        }
    }
}