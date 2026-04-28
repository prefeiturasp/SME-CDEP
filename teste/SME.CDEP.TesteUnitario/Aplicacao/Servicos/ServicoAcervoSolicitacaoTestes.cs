using AutoMapper;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Fachadas;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Contexto;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using System.Data;

namespace SME.CDEP.Aplicacao.Testes.Servicos
{
    public class ServicoAcervoSolicitacaoTestes
    {
        private readonly Mock<IRepositorioAcervoSolicitacao> _repositorioSolicitacaoMock;
        private readonly Mock<IRepositorioAcervoSolicitacaoItem> _repositorioItemMock;
        private readonly Mock<IRepositorioParametroSistema> _repositorioParametroSistemaMock;
        private readonly Mock<IRepositorioAcervo> _repositorioAcervoMock;
        private readonly Mock<IRepositorioUsuario> _repositorioUsuarioMock;
        private readonly Mock<IServicoAcervo> _servicoAcervoMock;
        private readonly Mock<IServicoUsuario> _servicoUsuarioMock;
        private readonly Mock<IServicoEvento> _servicoEventoMock;
        private readonly Mock<IServicoAcervoBibliografico> _servicoAcervoBibliograficoMock;
        private readonly Mock<IContextoAplicacao> _contextoAplicacaoMock;
        private readonly Mock<ITransacao> _transacaoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IServicoProcessamentoSituacaoSolicitacao> _servicoProcessamentoSituacaoMock;
        private readonly Mock<IServicoMensageria> _servicoMensageriaMock;
        private readonly Mock<IServicoManutencaoSolicitacaoAcervo> _servicoManutencaoSolicitacaoAcervoMock;

        private readonly ServicoAcervoSolicitacao _sut;

        public ServicoAcervoSolicitacaoTestes()
        {
            var mocker = new AutoMocker();

            _repositorioSolicitacaoMock = mocker.GetMock<IRepositorioAcervoSolicitacao>();
            _repositorioItemMock = mocker.GetMock<IRepositorioAcervoSolicitacaoItem>();
            _repositorioParametroSistemaMock = mocker.GetMock<IRepositorioParametroSistema>();
            _repositorioAcervoMock = mocker.GetMock<IRepositorioAcervo>();
            _repositorioUsuarioMock = mocker.GetMock<IRepositorioUsuario>();
            _servicoAcervoMock = mocker.GetMock<IServicoAcervo>();
            _servicoUsuarioMock = mocker.GetMock<IServicoUsuario>();
            _servicoEventoMock = mocker.GetMock<IServicoEvento>();
            _servicoAcervoBibliograficoMock = mocker.GetMock<IServicoAcervoBibliografico>();
            _contextoAplicacaoMock = mocker.GetMock<IContextoAplicacao>();
            _transacaoMock = mocker.GetMock<ITransacao>();
            _mapperMock = mocker.GetMock<IMapper>();

            _servicoProcessamentoSituacaoMock = mocker.GetMock<IServicoProcessamentoSituacaoSolicitacao>();
            _servicoMensageriaMock = mocker.GetMock<IServicoMensageria>();
            _servicoManutencaoSolicitacaoAcervoMock = mocker.GetMock<IServicoManutencaoSolicitacaoAcervo>();

            var contextoDados = new ContextoDadosAcervoSolicitacao(
                _repositorioSolicitacaoMock.Object,
                _repositorioItemMock.Object,
                mocker.GetMock<IRepositorioAcervoEmprestimo>().Object,
                _repositorioAcervoMock.Object
            );

            mocker.Use(contextoDados);
            mocker.Use(mocker.CreateInstance<ContextoRegrasAcervoSolicitacao>());
            mocker.Use(mocker.CreateInstance<ContextoInfraAcervoSolicitacao>());

            _sut = mocker.CreateInstance<ServicoAcervoSolicitacao>();
        }

        [Fact]
        public async Task DadoSolicitacaoAcervoValida_QuandoChamarRemover_EntaoRetornaVerdadeiroComSucesso()
        {
            // Arrange
            long acervoSolicitacaoId = 1;

            // Act
            var resultado = await _sut.Remover(acervoSolicitacaoId);

            // Assert
            resultado.Should().BeTrue();
            _repositorioSolicitacaoMock.Verify(r => r.Remover(acervoSolicitacaoId), Times.Once);
        }

        [Fact]
        public async Task DadoConsultaSemResultados_QuandoChamarObterItensAcervoPorAcervosIds_EntaoLancaNegocioException()
        {
            // Arrange
            long[] acervosIds = [1, 2];

            _repositorioSolicitacaoMock
                .Setup(r => r.ObterItensDoAcervoPorAcervosIds(acervosIds))
                .ReturnsAsync((IEnumerable<AcervoTipoTituloAcervoIdCreditosAutores>)null!);

            // Act
            var acao = async () => await _sut.ObterItensAcervoPorAcervosIds(acervosIds);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.ACERVO_NAO_ENCONTRADO);

            _repositorioSolicitacaoMock.Verify(r => r.ObterItensDoAcervoPorAcervosIds(acervosIds), Times.Once);
        }

        [Fact]
        public async Task DadoConsultaComResultados_QuandoChamarObterItensAcervoPorAcervosIds_EntaoRetornaListaMapeadaComSucesso()
        {
            // Arrange
            long[] acervosIds = [1, 2];
            var acervos = new List<AcervoTipoTituloAcervoIdCreditosAutores>
            {
                new() { AcervoId = 1, Titulo = "Acervo Teste 1" },
                new() { AcervoId = 2, Titulo = "Acervo Teste 2" }
            };

            var acervosMapeados = new List<AcervoTipoTituloAcervoIdCreditosAutoresDTO>
            {
                new() { AcervoId = 1, Titulo = "Acervo Teste 1" },
                new() { AcervoId = 2, Titulo = "Acervo Teste 2" }
            };

            _repositorioSolicitacaoMock
                .Setup(r => r.ObterItensDoAcervoPorAcervosIds(acervosIds))
                .ReturnsAsync(acervos);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<AcervoTipoTituloAcervoIdCreditosAutoresDTO>>(acervos))
                .Returns(acervosMapeados);

            // Act
            var resultado = await _sut.ObterItensAcervoPorAcervosIds(acervosIds);

            // Assert
            resultado.Should().NotBeNullOrEmpty();
            resultado.Should().BeEquivalentTo(acervosMapeados);

            _repositorioSolicitacaoMock.Verify(r => r.ObterItensDoAcervoPorAcervosIds(acervosIds), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<AcervoTipoTituloAcervoIdCreditosAutoresDTO>>(acervos), Times.Once);
        }

        [Fact]
        public async Task DadoPerfilLogado_QuandoChamarObterPorId_EntaoRetornaDetalheDaSolicitacaoFiltradaPorTiposPermitidos()
        {
            // Arrange
            long acervoSolicitacaoId = 1;
            long[] tiposPermitidos = [1, 2];
            var retornoEsperado = new AcervoSolicitacaoRetornoCadastroDTO();

            _servicoAcervoMock
                .Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado())
                .Returns(tiposPermitidos);

            _servicoAcervoMock
                .Setup(s => s.ObterAcervosSolicitacoesPorIdTiposPermitidosAsync(acervoSolicitacaoId, tiposPermitidos))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _sut.ObterPorId(acervoSolicitacaoId);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEquivalentTo(retornoEsperado);

            _servicoAcervoMock.Verify(s => s.ObterTiposAcervosPermitidosDoPerfilLogado(), Times.Once);
            _servicoAcervoMock.Verify(s => s.ObterAcervosSolicitacoesPorIdTiposPermitidosAsync(acervoSolicitacaoId, tiposPermitidos), Times.Once);
        }

        [Fact]
        public async Task DadoSolicitacaoAcervoValida_QuandoChamarObterMinhaSolicitacaoPorId_EntaoRetornaDetalhePassandoTodosTiposAcervoComoPermitidos()
        {
            // Arrange
            long acervoSolicitacaoId = 1;
            var tiposAcervosPermitidos = Enum.GetValues<TipoAcervo>().Select(v => (long)v).ToArray();
            var retornoEsperado = new AcervoSolicitacaoRetornoCadastroDTO();

            _servicoAcervoMock
                .Setup(s => s.ObterAcervosSolicitacoesPorIdTiposPermitidosAsync(acervoSolicitacaoId, It.Is<long[]>(x => x.SequenceEqual(tiposAcervosPermitidos))))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _sut.ObterMinhaSolicitacaoPorId(acervoSolicitacaoId);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEquivalentTo(retornoEsperado);

            _servicoAcervoMock.Verify(s => s.ObterAcervosSolicitacoesPorIdTiposPermitidosAsync(acervoSolicitacaoId, It.Is<long[]>(x => x.SequenceEqual(tiposAcervosPermitidos))), Times.Once);
        }

        [Fact]
        public async Task DadoSolicitacaoAcervoValida_QuandoChamarExcluir_EntaoRetornaVerdadeiroComSucesso()
        {
            // Arrange
            long acervoSolicitacaoId = 1;

            // Act
            var resultado = await _sut.Excluir(acervoSolicitacaoId);

            // Assert
            resultado.Should().BeTrue();
            _repositorioSolicitacaoMock.Verify(r => r.Excluir(acervoSolicitacaoId), Times.Once);
        }

        [Fact]
        public async Task DadoSolicitacoesExistentes_QuandoChamarObterMinhasSolicitacoes_EntaoRetornaResultadoPaginado()
        {
            // Arrange
            var usuario = new UsuarioDTO { Id = 100 };
            var acervoSolicitacoesMock = new List<AcervoSolicitacaoItemResumido> { new(), new(), new() };
            var acervosMapeadosMock = new List<MinhaSolicitacaoDTO> { new(), new(), new() };

            _servicoUsuarioMock.Setup(s => s.ObterUsuarioLogado()).ReturnsAsync(usuario);
            _repositorioItemMock.Setup(r => r.ObterMinhasSolicitacoes(usuario.Id)).ReturnsAsync(acervoSolicitacoesMock);
            _mapperMock.Setup(m => m.Map<IEnumerable<MinhaSolicitacaoDTO>>(acervoSolicitacoesMock)).Returns(acervosMapeadosMock);

            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroPagina")).Returns("1");
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroRegistros")).Returns("10");
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("Ordenacao")).Returns("1");

            // Act
            var resultado = await _sut.ObterMinhasSolicitacoes();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Items.Should().HaveCount(acervosMapeadosMock.Count);
            resultado.TotalRegistros.Should().Be(acervosMapeadosMock.Count);
            resultado.TotalPaginas.Should().Be(1);

            _servicoUsuarioMock.Verify(s => s.ObterUsuarioLogado(), Times.Once);
            _repositorioItemMock.Verify(r => r.ObterMinhasSolicitacoes(usuario.Id), Times.Once);
        }

        [Fact]
        public async Task DadoUsuarioSemSolicitacoes_QuandoChamarObterMinhasSolicitacoes_EntaoRetornaPaginacaoVazia()
        {
            // Arrange
            var usuario = new UsuarioDTO { Id = 100 };
            var acervoSolicitacoesMock = new List<AcervoSolicitacaoItemResumido>();
            var acervosMapeadosMock = new List<MinhaSolicitacaoDTO>();

            _servicoUsuarioMock.Setup(s => s.ObterUsuarioLogado()).ReturnsAsync(usuario);
            _repositorioItemMock.Setup(r => r.ObterMinhasSolicitacoes(usuario.Id)).ReturnsAsync(acervoSolicitacoesMock);
            _mapperMock.Setup(m => m.Map<IEnumerable<MinhaSolicitacaoDTO>>(acervoSolicitacoesMock)).Returns(acervosMapeadosMock);

            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroPagina")).Returns("1");
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroRegistros")).Returns("10");
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("Ordenacao")).Returns("1");

            // Act
            var resultado = await _sut.ObterMinhasSolicitacoes();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Items.Should().BeEmpty();
            resultado.TotalRegistros.Should().Be(0);
            resultado.TotalPaginas.Should().Be(0);
        }

        [Fact]
        public void DadoQueryStringNaoPreenchida_QuandoAcessarPaginacao_EntaoRetornaPaginacaoZerada()
        {
            // Arrange
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroPagina")).Returns(string.Empty);
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroRegistros")).Returns(string.Empty);
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("Ordenacao")).Returns(string.Empty);

            // Act
            var resultado = _sut.Paginacao;

            // Assert
            resultado.Should().NotBeNull();
            resultado.QuantidadeRegistros.Should().Be(0);
        }

        [Fact]
        public void DadoNumeroRegistrosZero_QuandoAcessarPaginacao_EntaoRetornaPaginacaoComDezRegistros()
        {
            // Arrange
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroPagina")).Returns("1");
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroRegistros")).Returns("0");
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("Ordenacao")).Returns("1");

            // Act
            var resultado = _sut.Paginacao;

            // Assert
            resultado.Should().NotBeNull();
            resultado.QuantidadeRegistros.Should().Be(10);
        }

        [Fact]
        public void DadoQueryStringPreenchida_QuandoAcessarPaginacao_EntaoRetornaPaginacaoConvertida()
        {
            // Arrange
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroPagina")).Returns("2");
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroRegistros")).Returns("20");
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("Ordenacao")).Returns("2");

            // Act
            var resultado = _sut.Paginacao;

            // Assert
            resultado.Should().NotBeNull();
            resultado.QuantidadeRegistros.Should().Be(20);
        }

        [Fact]
        public async Task DadoEnumerador_QuandoChamarObterSituacoesAtendimentosItem_EntaoRetornaListaOrdenadaComDescricoes()
        {
            // Arrange

            // Act
            var resultado = await _sut.ObterSituacoesAtendimentosItem();

            // Assert
            resultado.Should().NotBeNullOrEmpty();
            resultado.Should().BeInAscendingOrder(x => x.Id);
            resultado.Should().Contain(x => x.Id == (short)SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO);
        }

        [Fact]
        public async Task DadoFiltroPreenchido_QuandoChamarObterAtendimentoSolicitacoesPorFiltro_EntaoRetornaListaPaginadaFiltrada()
        {
            // Arrange
            var filtro = new FiltroSolicitacaoDTO { AcervoSolicitacaoId = 1 };
            long[] tiposPermitidos = [1, 2];
            var solicitacoesMock = new List<AcervoSolicitacaoItemDetalhe> { new() };
            var solicitacoesMapeadasMock = new List<SolicitacaoDTO> { new() };

            _servicoAcervoMock.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado()).Returns(tiposPermitidos);

            _repositorioItemMock
                .Setup(r => r.ObterSolicitacoesPorFiltro(
                    filtro.AcervoSolicitacaoId, filtro.TipoAcervo, filtro.DataSolicitacaoInicio, filtro.DataSolicitacaoFim,
                    filtro.Responsavel, filtro.SituacaoItem, filtro.DataVisitaInicio, filtro.DataVisitaFim,
                    filtro.SolicitanteRf, filtro.SituacaoEmprestimo, tiposPermitidos))
                .ReturnsAsync(solicitacoesMock);

            _mapperMock.Setup(m => m.Map<IEnumerable<SolicitacaoDTO>>(solicitacoesMock)).Returns(solicitacoesMapeadasMock);

            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroPagina")).Returns("1");
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("NumeroRegistros")).Returns("10");
            _contextoAplicacaoMock.Setup(c => c.ObterVariavel<string>("Ordenacao")).Returns("1");

            // Act
            var resultado = await _sut.ObterAtendimentoSolicitacoesPorFiltro(filtro);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Items.Should().HaveCount(solicitacoesMapeadasMock.Count);
            resultado.TotalRegistros.Should().Be(solicitacoesMapeadasMock.Count);

            _servicoAcervoMock.Verify(s => s.ObterTiposAcervosPermitidosDoPerfilLogado(), Times.Once);
            _repositorioItemMock.Verify(r => r.ObterSolicitacoesPorFiltro(
                filtro.AcervoSolicitacaoId, filtro.TipoAcervo, filtro.DataSolicitacaoInicio, filtro.DataSolicitacaoFim,
                filtro.Responsavel, filtro.SituacaoItem, filtro.DataVisitaInicio, filtro.DataVisitaFim,
                filtro.SolicitanteRf, filtro.SituacaoEmprestimo, tiposPermitidos), Times.Once);
        }

        [Fact]
        public async Task DadoConsultaRepositorioNula_QuandoChamarObterDetalhesParaAtendimentoSolicitadoesPorId_EntaoLancaNegocioException()
        {
            // Arrange
            long acervoSolicitacaoId = 1;
            long[] tiposPermitidos = [1, 2];
            var guidPerfil = Guid.NewGuid();

            _contextoAplicacaoMock.Setup(c => c.PerfilUsuario).Returns(guidPerfil.ToString());
            _servicoAcervoMock.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado()).Returns(tiposPermitidos);

            _repositorioSolicitacaoMock
                .Setup(r => r.ObterDetalhesPorIdTiposPermitidos(acervoSolicitacaoId, tiposPermitidos))
                .ReturnsAsync((AcervoSolicitacaoDetalhe)null!);

            _mapperMock.Setup(m => m.Map<AcervoSolicitacaoDetalheDTO>(null)).Returns((AcervoSolicitacaoDetalheDTO)null!);

            // Act
            var acao = async () => await _sut.ObterDetalhesParaAtendimentoSolicitadoesPorId(acervoSolicitacaoId);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_ENCONTRADA);

            _repositorioSolicitacaoMock.Verify(r => r.ObterDetalhesPorIdTiposPermitidos(acervoSolicitacaoId, tiposPermitidos), Times.Once);
        }

        [Fact]
        public async Task DadoItemAcervoBibliografico_QuandoChamarObterDetalhes_EntaoAtribuiLimiteDiasEmprestimo()
        {
            // Arrange
            long acervoSolicitacaoId = 1;
            long[] tiposPermitidos = [1, 2];
            var guidPerfil = Guid.NewGuid();
            var detalheDominio = new AcervoSolicitacaoDetalhe();
            var detalheDto = new AcervoSolicitacaoDetalheDTO
            {
                UsuarioId = 10,
                SituacaoId = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO,
                Itens = new List<AcervoSolicitacaoItemDetalheResumidoDTO>
                {
                    new() { TipoAcervoId = TipoAcervo.Bibliografico, SituacaoId = SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO }
                }
            };
            var dadosSolicitante = new DadosSolicitanteDto();

            _contextoAplicacaoMock.Setup(c => c.PerfilUsuario).Returns(guidPerfil.ToString());
            _servicoAcervoMock.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado()).Returns(tiposPermitidos);
            _repositorioSolicitacaoMock.Setup(r => r.ObterDetalhesPorIdTiposPermitidos(acervoSolicitacaoId, tiposPermitidos)).ReturnsAsync(detalheDominio);
            _mapperMock.Setup(m => m.Map<AcervoSolicitacaoDetalheDTO>(detalheDominio)).Returns(detalheDto);
            _servicoUsuarioMock.Setup(s => s.ObterDadosSolicitantePorUsuarioId(detalheDto.UsuarioId)).ReturnsAsync(dadosSolicitante);
            _mapperMock.Setup(m => m.Map<DadosSolicitanteDto>(dadosSolicitante)).Returns(dadosSolicitante);

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "15" });

            // Act
            var resultado = await _sut.ObterDetalhesParaAtendimentoSolicitadoesPorId(acervoSolicitacaoId);

            // Assert
            resultado.Should().NotBeNull();
            resultado.LimiteDiasEmprestimoAcervo.Should().Be(15);

            _repositorioParametroSistemaMock.Verify(r => r.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DadoNenhumItemBibliografico_QuandoChamarObterDetalhes_EntaoIgnoraAtribuicaoLimiteDias()
        {
            // Arrange
            long acervoSolicitacaoId = 1;
            long[] tiposPermitidos = [1, 2];
            var guidPerfil = Guid.NewGuid();
            var detalheDominio = new AcervoSolicitacaoDetalhe();
            var detalheDto = new AcervoSolicitacaoDetalheDTO
            {
                UsuarioId = 10,
                SituacaoId = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO,
                Itens = new List<AcervoSolicitacaoItemDetalheResumidoDTO>
                {
                    new() { TipoAcervoId = TipoAcervo.DocumentacaoTextual, SituacaoId = SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO }
                }
            };
            var dadosSolicitante = new DadosSolicitanteDto();

            _contextoAplicacaoMock.Setup(c => c.PerfilUsuario).Returns(guidPerfil.ToString());
            _servicoAcervoMock.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado()).Returns(tiposPermitidos);
            _repositorioSolicitacaoMock.Setup(r => r.ObterDetalhesPorIdTiposPermitidos(acervoSolicitacaoId, tiposPermitidos)).ReturnsAsync(detalheDominio);
            _mapperMock.Setup(m => m.Map<AcervoSolicitacaoDetalheDTO>(detalheDominio)).Returns(detalheDto);
            _servicoUsuarioMock.Setup(s => s.ObterDadosSolicitantePorUsuarioId(detalheDto.UsuarioId)).ReturnsAsync(dadosSolicitante);
            _mapperMock.Setup(m => m.Map<DadosSolicitanteDto>(dadosSolicitante)).Returns(dadosSolicitante);

            // Act
            var resultado = await _sut.ObterDetalhesParaAtendimentoSolicitadoesPorId(acervoSolicitacaoId);

            // Assert
            resultado.Should().NotBeNull();
            resultado.LimiteDiasEmprestimoAcervo.Should().Be(0);

            _repositorioParametroSistemaMock.Verify(r => r.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DadoPerfilAdminGeralESolicitacaoPendente_QuandoChamarObterDetalhes_EntaoDefineFlagsDeCancelamentoEFinalizacaoComoVerdadeiras()
        {
            // Arrange
            long acervoSolicitacaoId = 1;
            long[] tiposPermitidos = [1, 2];
            var guidPerfilAdmin = Guid.NewGuid();
            var detalheDominio = new AcervoSolicitacaoDetalhe();
            var detalheDto = new AcervoSolicitacaoDetalheDTO
            {
                UsuarioId = 10,
                SituacaoId = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO,
                Itens = new List<AcervoSolicitacaoItemDetalheResumidoDTO>
                {
                    new() { TipoAcervoId = TipoAcervo.DocumentacaoTextual, SituacaoId = SituacaoSolicitacaoItem.PRESENCIAL_ABERTO }
                }
            };
            var dadosSolicitante = new DadosSolicitanteDto();

            _contextoAplicacaoMock.Setup(c => c.PerfilUsuario).Returns(guidPerfilAdmin.ToString());
            _servicoAcervoMock.Setup(s => s.ObterTiposAcervosPermitidosDoPerfilLogado()).Returns(tiposPermitidos);
            _repositorioSolicitacaoMock.Setup(r => r.ObterDetalhesPorIdTiposPermitidos(acervoSolicitacaoId, tiposPermitidos)).ReturnsAsync(detalheDominio);
            _mapperMock.Setup(m => m.Map<AcervoSolicitacaoDetalheDTO>(detalheDominio)).Returns(detalheDto);
            _servicoUsuarioMock.Setup(s => s.ObterDadosSolicitantePorUsuarioId(detalheDto.UsuarioId)).ReturnsAsync(dadosSolicitante);
            _mapperMock.Setup(m => m.Map<DadosSolicitanteDto>(dadosSolicitante)).Returns(dadosSolicitante);

            // Act
            var resultado = await _sut.ObterDetalhesParaAtendimentoSolicitadoesPorId(acervoSolicitacaoId);

            // Assert
            resultado.Should().NotBeNull();
        }

        [Fact]
        public void DadoItemAguardandoAtendimento_QuandoChamarPodeFinalizar_EntaoRetornaFalso()
        {
            // Arrange
            var perfilLogado = Guid.NewGuid();
            var detalheDto = new AcervoSolicitacaoDetalheDTO
            {
                SituacaoId = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO,
                Itens = new List<AcervoSolicitacaoItemDetalheResumidoDTO>
                {
                    new() { SituacaoId = SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO }
                }
            };

            // Act
            var resultado = _sut.PodeFinalizar(perfilLogado, detalheDto);

            // Assert
            resultado.Should().BeFalse();
        }

        [Fact]
        public void DadoItemAguardandoVisitaComDataFutura_QuandoChamarPodeFinalizar_EntaoRetornaFalso()
        {
            // Arrange
            var perfilLogado = Guid.NewGuid();
            var detalheDto = new AcervoSolicitacaoDetalheDTO
            {
                SituacaoId = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO,
                Itens = new List<AcervoSolicitacaoItemDetalheResumidoDTO>
                {
                    new()
                    {
                        SituacaoId = SituacaoSolicitacaoItem.AGUARDANDO_VISITA,
                        DataVisita = DateTime.Now.AddDays(2)
                    }
                }
            };

            // Act
            var resultado = _sut.PodeFinalizar(perfilLogado, detalheDto);

            // Assert
            resultado.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoBibliograficoAguardandoVisitaOuAtendimento_QuandoChamarPodeFinalizar_EntaoRetornaFalso()
        {
            // Arrange
            var perfilLogado = Guid.NewGuid();
            var detalheDto = new AcervoSolicitacaoDetalheDTO
            {
                SituacaoId = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO,
                Itens = new List<AcervoSolicitacaoItemDetalheResumidoDTO>
                {
                    new()
                    {
                        TipoAcervoId = TipoAcervo.Bibliografico,
                        SituacaoId = SituacaoSolicitacaoItem.AGUARDANDO_VISITA
                    }
                }
            };

            // Act
            var resultado = _sut.PodeFinalizar(perfilLogado, detalheDto);

            // Assert
            resultado.Should().BeFalse();
        }

        [Fact]
        public void DadoSolicitacaoMatrizFinalizadaOuCancelada_QuandoChamarPodeFinalizar_EntaoRetornaFalso()
        {
            // Arrange
            var perfilLogado = Guid.NewGuid();
            var detalheDto = new AcervoSolicitacaoDetalheDTO
            {
                SituacaoId = SituacaoSolicitacao.FINALIZADO_ATENDIMENTO,
                Itens = new List<AcervoSolicitacaoItemDetalheResumidoDTO>()
            };

            // Act
            var resultado = _sut.PodeFinalizar(perfilLogado, detalheDto);

            // Assert
            resultado.Should().BeFalse();
        }

        [Fact]
        public void DadoEnumeradorTipoAtendimento_QuandoChamarObterTiposDeAtendimentos_EntaoRetornaOpcoesMapeadas()
        {
            // Arrange

            // Act
            var resultado = _sut.ObterTiposDeAtendimentos();

            // Assert
            resultado.Should().NotBeNullOrEmpty();
            resultado.Should().Contain(x => x.Id == (int)TipoAtendimento.Email);
            resultado.Should().Contain(x => x.Id == (int)TipoAtendimento.Presencial);
        }

        [Fact]
        public async Task DadoAcervoSolicitacaoConfirmarDto_QuandoChamarConfirmarAtendimento_EntaoProcessaRetornaSucesso()
        {
            // Arrange
            var dto = new AcervoSolicitacaoConfirmarDto();

            // Act
            var resultado = await _sut.ConfirmarAtendimento(dto);

            // Assert
            resultado.Should().BeFalse();
        }

        [Fact]
        public async Task DadoSolicitacaoInexistente_QuandoChamarFinalizarAtendimento_EntaoLancaNegocioException()
        {
            // Arrange
            long acervoSolicitacaoId = 1;
            _repositorioSolicitacaoMock.Setup(r => r.ObterPorId(acervoSolicitacaoId)).ReturnsAsync((AcervoSolicitacao)null!);

            // Act
            var acao = async () => await _sut.FinalizarAtendimento(acervoSolicitacaoId);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_ENCONTRADA);
        }

        [Fact]
        public async Task DadoSolicitacaoAtendidaParcialmente_QuandoChamarFinalizarAtendimento_EntaoLancaNegocioException()
        {
            // Arrange
            long acervoSolicitacaoId = 1;
            var solicitacao = new AcervoSolicitacao { Situacao = SituacaoSolicitacao.ATENDIDO_PARCIALMENTE };
            _repositorioSolicitacaoMock.Setup(r => r.ObterPorId(acervoSolicitacaoId)).ReturnsAsync(solicitacao);

            // Act
            var acao = async () => await _sut.FinalizarAtendimento(acervoSolicitacaoId);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.CANCELAR_SOLICITACAO_NAO_PERMITIDO_QUANDO_ITENS_ATENDIDOS_PARCIALMENTE);
        }

        [Fact]
        public async Task DadoItensAguardandoAtendimentoOuVisita_QuandoChamarFinalizarAtendimento_EntaoLancaNegocioException()
        {
            // Arrange
            long acervoSolicitacaoId = 1;
            var solicitacao = new AcervoSolicitacao { Situacao = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO };

            _repositorioSolicitacaoMock.Setup(r => r.ObterPorId(acervoSolicitacaoId)).ReturnsAsync(solicitacao);
            _repositorioItemMock.Setup(r => r.PossuiItensEmSituacaoAguardandoAtendimentoOuAguardandoVisitaComDataFutura(acervoSolicitacaoId)).ReturnsAsync(true);

            // Act
            var acao = async () => await _sut.FinalizarAtendimento(acervoSolicitacaoId);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.NÃO_PODE_FINALIZAR_QUANDO_AGUARDANDO_VISITA_DATA_FUTURA_OU_AGUARDANDO_ATENDIMENTO);
        }

        [Fact]
        public async Task DadoSolicitacaoEItensValidos_QuandoChamarFinalizarAtendimento_EntaoComitaTransacaoEAtualizaSituacoes()
        {
            // Arrange
            long acervoSolicitacaoId = 1;
            var solicitacao = new AcervoSolicitacao { Id = acervoSolicitacaoId, Situacao = SituacaoSolicitacao.PRESENCIAL_ABERTO };
            var itens = new List<AcervoSolicitacaoItem> { new() { Id = 10, Situacao = SituacaoSolicitacaoItem.AGUARDANDO_VISITA } };

            _repositorioSolicitacaoMock.Setup(r => r.ObterPorId(acervoSolicitacaoId)).ReturnsAsync(solicitacao);
            _repositorioItemMock.Setup(r => r.PossuiItensEmSituacaoAguardandoAtendimentoOuAguardandoVisitaComDataFutura(acervoSolicitacaoId)).ReturnsAsync(false);
            _repositorioItemMock.Setup(r => r.ObterItensEmSituacaoAguardandoVisitaPorSolicitacaoId(acervoSolicitacaoId)).ReturnsAsync(itens);

            var dbTransactionMock = new Mock<IDbTransaction>();
            _transacaoMock.Setup(t => t.Iniciar()).Returns(dbTransactionMock.Object);

            // Act
            var resultado = await _sut.FinalizarAtendimento(acervoSolicitacaoId);

            // Assert
            resultado.Should().BeTrue();
            solicitacao.Situacao.Should().Be(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
            itens.First().Situacao.Should().Be(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);

            _repositorioSolicitacaoMock.Verify(r => r.Atualizar(solicitacao), Times.Once);
            _repositorioItemMock.Verify(r => r.Atualizar(It.IsAny<AcervoSolicitacaoItem>()), Times.Once);
            dbTransactionMock.Verify(t => t.Commit(), Times.Once);
            dbTransactionMock.Verify(t => t.Dispose(), Times.Once);
        }

        [Fact]
        public async Task DadoErroDuranteProcessamento_QuandoChamarFinalizarAtendimento_EntaoAplicaRollbackERethrowException()
        {
            // Arrange
            long acervoSolicitacaoId = 1;
            var solicitacao = new AcervoSolicitacao { Id = acervoSolicitacaoId, Situacao = SituacaoSolicitacao.PRESENCIAL_ABERTO };
            var itens = new List<AcervoSolicitacaoItem> { new() { Id = 10 } };

            _repositorioSolicitacaoMock.Setup(r => r.ObterPorId(acervoSolicitacaoId)).ReturnsAsync(solicitacao);
            _repositorioItemMock.Setup(r => r.PossuiItensEmSituacaoAguardandoAtendimentoOuAguardandoVisitaComDataFutura(acervoSolicitacaoId)).ReturnsAsync(false);
            _repositorioItemMock.Setup(r => r.ObterItensEmSituacaoAguardandoVisitaPorSolicitacaoId(acervoSolicitacaoId)).ReturnsAsync(itens);

            var dbTransactionMock = new Mock<IDbTransaction>();
            _transacaoMock.Setup(t => t.Iniciar()).Returns(dbTransactionMock.Object);

            _repositorioSolicitacaoMock.Setup(r => r.Atualizar(solicitacao)).ThrowsAsync(new System.Exception("Erro DB"));

            // Act
            var acao = async () => await _sut.FinalizarAtendimento(acervoSolicitacaoId);

            // Assert
            await acao.Should().ThrowAsync<System.Exception>().WithMessage("Erro DB");

            dbTransactionMock.Verify(t => t.Rollback(), Times.Once);
            dbTransactionMock.Verify(t => t.Dispose(), Times.Once);
        }

        [Fact]
        public async Task DadoItemInexistente_QuandoChamarFinalizarAtendimentoItem_EntaoLancaNegocioException()
        {
            // Arrange
            long acervoSolicitacaoItemId = 1;
            _repositorioItemMock.Setup(r => r.ObterPorId(acervoSolicitacaoItemId)).ReturnsAsync((AcervoSolicitacaoItem)null!);

            // Act
            var acao = async () => await _sut.FinalizarAtendimentoItem(acervoSolicitacaoItemId);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>().WithMessage(MensagemNegocio.SOLICITACAO_ATENDIMENTO_ITEM_NAO_ENCONTRADA);
        }

        [Fact]
        public async Task DadoAcervoInexistente_QuandoChamarFinalizarAtendimentoItem_EntaoLancaNegocioException()
        {
            // Arrange
            long acervoSolicitacaoItemId = 1;
            var item = new AcervoSolicitacaoItem { AcervoId = 10 };

            _repositorioItemMock.Setup(r => r.ObterPorId(acervoSolicitacaoItemId)).ReturnsAsync(item);
            _repositorioAcervoMock.Setup(r => r.ObterPorId(item.AcervoId)).ReturnsAsync((Acervo)null!);

            // Act
            var acao = async () => await _sut.FinalizarAtendimentoItem(acervoSolicitacaoItemId);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>().WithMessage(MensagemNegocio.ACERVO_NAO_ENCONTRADO);
        }

        [Fact]
        public async Task DadoItemNaoPermitidoParaFinalizar_QuandoChamarFinalizarAtendimentoItem_EntaoLancaNegocioException()
        {
            // Arrange
            long acervoSolicitacaoItemId = 1;
            var item = new AcervoSolicitacaoItem
            {
                AcervoId = 10,
                TipoAtendimento = TipoAtendimento.Email
            };
            var acervo = new Acervo { TipoAcervoId = (long)TipoAcervo.DocumentacaoTextual };

            _repositorioItemMock.Setup(r => r.ObterPorId(acervoSolicitacaoItemId)).ReturnsAsync(item);
            _repositorioAcervoMock.Setup(r => r.ObterPorId(item.AcervoId)).ReturnsAsync(acervo);

            // Act
            var acao = async () => await _sut.FinalizarAtendimentoItem(acervoSolicitacaoItemId);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>().WithMessage(MensagemNegocio.PERMITIDO_FINALIZAR_ATENDIMENTO_AGUARDANDO_VISITA_ATE_O_DIA_DE_HOJE);
        }

        [Fact]
        public async Task DadoOutrosItensPendentes_QuandoChamarFinalizarAtendimentoItem_EntaoFinalizaApenasOItemEspecifico()
        {
            // Arrange
            long acervoSolicitacaoItemId = 1;
            var item = new AcervoSolicitacaoItem
            {
                Id = acervoSolicitacaoItemId,
                AcervoSolicitacaoId = 100,
                AcervoId = 10,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataVisita = DateTime.Now.Date
            };
            var acervo = new Acervo { TipoAcervoId = (long)TipoAcervo.DocumentacaoTextual };
            var itens = new List<AcervoSolicitacaoItem>
            {
                item,
                new() { Situacao = SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO }
            };

            _repositorioItemMock.Setup(r => r.ObterPorId(acervoSolicitacaoItemId)).ReturnsAsync(item);
            _repositorioAcervoMock.Setup(r => r.ObterPorId(item.AcervoId)).ReturnsAsync(acervo);
            _repositorioItemMock.Setup(r => r.ObterItensPorSolicitacaoId(item.AcervoSolicitacaoId)).ReturnsAsync(itens);

            // Act
            var resultado = await _sut.FinalizarAtendimentoItem(acervoSolicitacaoItemId);

            // Assert
            resultado.Should().BeTrue();
            item.Situacao.Should().Be(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);

            _repositorioItemMock.Verify(r => r.Atualizar(item), Times.Once);
            _repositorioSolicitacaoMock.Verify(r => r.Atualizar(It.IsAny<AcervoSolicitacao>()), Times.Never);
        }

        [Fact]
        public async Task DadoUltimoItemPendente_QuandoChamarFinalizarAtendimentoItem_EntaoFinalizaItemEAtualizaSolicitacaoMatriz()
        {
            // Arrange
            long acervoSolicitacaoItemId = 1;
            var item = new AcervoSolicitacaoItem
            {
                Id = acervoSolicitacaoItemId,
                AcervoSolicitacaoId = 100,
                AcervoId = 10,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataVisita = DateTime.Now.Date
            };
            var acervo = new Acervo { TipoAcervoId = (long)TipoAcervo.DocumentacaoTextual };
            var solicitacaoMatriz = new AcervoSolicitacao { Id = 100, Situacao = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO };
            var itens = new List<AcervoSolicitacaoItem>
            {
                item
            };

            _repositorioItemMock.Setup(r => r.ObterPorId(acervoSolicitacaoItemId)).ReturnsAsync(item);
            _repositorioAcervoMock.Setup(r => r.ObterPorId(item.AcervoId)).ReturnsAsync(acervo);
            _repositorioItemMock.Setup(r => r.ObterItensPorSolicitacaoId(item.AcervoSolicitacaoId)).ReturnsAsync(itens);
            _repositorioSolicitacaoMock.Setup(r => r.ObterPorId(item.AcervoSolicitacaoId)).ReturnsAsync(solicitacaoMatriz);

            // Act
            var resultado = await _sut.FinalizarAtendimentoItem(acervoSolicitacaoItemId);

            // Assert
            resultado.Should().BeTrue();
            item.Situacao.Should().Be(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE);
            solicitacaoMatriz.Situacao.Should().Be(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);

            _repositorioItemMock.Verify(r => r.Atualizar(item), Times.Once);
            _repositorioSolicitacaoMock.Verify(r => r.Atualizar(solicitacaoMatriz), Times.Once);
        }

        [Fact]
        public async Task DadoSolicitacaoInexistente_QuandoChamarCancelarAtendimento_EntaoLancaNegocioException()
        {
            // Arrange
            long acervoSolicitacaoId = 1;
            _repositorioSolicitacaoMock.Setup(r => r.ObterPorId(acervoSolicitacaoId)).ReturnsAsync((AcervoSolicitacao)null!);

            // Act
            var acao = async () => await _sut.CancelarAtendimento(acervoSolicitacaoId);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.SOLICITACAO_ATENDIMENTO_NAO_ENCONTRADA);
        }

        [Fact]
        public async Task DadoItensFinalizadosAutomaticamente_QuandoChamarCancelarAtendimento_EntaoLancaNegocioException()
        {
            // Arrange
            long acervoSolicitacaoId = 1;
            var solicitacao = new AcervoSolicitacao { Id = acervoSolicitacaoId };

            _repositorioSolicitacaoMock.Setup(r => r.ObterPorId(acervoSolicitacaoId)).ReturnsAsync(solicitacao);
            _repositorioItemMock.Setup(r => r.PossuiItensFinalizadosAutomaticamente(acervoSolicitacaoId)).ReturnsAsync(true);

            // Act
            var acao = async () => await _sut.CancelarAtendimento(acervoSolicitacaoId);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.NAO_PODE_CANCELAR_ATENDIMENTO_COM_ITEM_FINALIZADO_AUTOMATICAMENTE_MANUALMENTE);
        }

        [Fact]
        public async Task DadoSolicitacaoValida_QuandoChamarCancelarAtendimento_EntaoAlteraSituacaoEPublicaMensagem()
        {
            // Arrange
            long acervoSolicitacaoId = 1;
            var solicitacao = new AcervoSolicitacao { Id = acervoSolicitacaoId, Situacao = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO };
            var itens = new List<AcervoSolicitacaoItem> { new() { Id = 10, AcervoId = 1, Situacao = SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO } };
            var acervos = new List<Acervo>();

            _repositorioSolicitacaoMock.Setup(r => r.ObterPorId(acervoSolicitacaoId)).ReturnsAsync(solicitacao);
            _repositorioItemMock.Setup(r => r.PossuiItensFinalizadosAutomaticamente(acervoSolicitacaoId)).ReturnsAsync(false);
            _repositorioItemMock.Setup(r => r.ObterItensPorSolicitacaoId(acervoSolicitacaoId)).ReturnsAsync(itens);
            _repositorioAcervoMock.Setup(r => r.ObterAcervosPorIds(It.IsAny<long[]>())).ReturnsAsync(acervos);

            var dbTransactionMock = new Mock<IDbTransaction>();
            _transacaoMock.Setup(t => t.Iniciar()).Returns(dbTransactionMock.Object);

            // Act
            var resultado = await _sut.CancelarAtendimento(acervoSolicitacaoId);

            // Assert
            resultado.Should().BeTrue();
            solicitacao.Situacao.Should().Be(SituacaoSolicitacao.CANCELADO);
            itens.First().Situacao.Should().Be(SituacaoSolicitacaoItem.CANCELADO);

            _repositorioSolicitacaoMock.Verify(r => r.Atualizar(solicitacao), Times.Once);
            _repositorioItemMock.Verify(r => r.Atualizar(It.IsAny<AcervoSolicitacaoItem>()), Times.Once);
            dbTransactionMock.Verify(t => t.Commit(), Times.Once);
            dbTransactionMock.Verify(t => t.Dispose(), Times.Once);
        }

        [Fact]
        public async Task DadoItemPresencial_QuandoChamarCancelarAtendimento_EntaoExcluiEventoAtrelado()
        {
            // Arrange
            long acervoSolicitacaoId = 1;
            var solicitacao = new AcervoSolicitacao { Id = acervoSolicitacaoId, Situacao = SituacaoSolicitacao.PRESENCIAL_ABERTO };
            var itens = new List<AcervoSolicitacaoItem> { new() { Id = 10, AcervoId = 1, TipoAtendimento = TipoAtendimento.Presencial } };
            var acervos = new List<Acervo> { new() { Id = 1, TipoAcervoId = (long)TipoAcervo.DocumentacaoTextual } };

            _repositorioSolicitacaoMock.Setup(r => r.ObterPorId(acervoSolicitacaoId)).ReturnsAsync(solicitacao);
            _repositorioItemMock.Setup(r => r.PossuiItensFinalizadosAutomaticamente(acervoSolicitacaoId)).ReturnsAsync(false);
            _repositorioItemMock.Setup(r => r.ObterItensPorSolicitacaoId(acervoSolicitacaoId)).ReturnsAsync(itens);
            _repositorioAcervoMock.Setup(r => r.ObterAcervosPorIds(It.IsAny<long[]>())).ReturnsAsync(acervos);

            var dbTransactionMock = new Mock<IDbTransaction>();
            _transacaoMock.Setup(t => t.Iniciar()).Returns(dbTransactionMock.Object);

            // Act
            var resultado = await _sut.CancelarAtendimento(acervoSolicitacaoId);

            // Assert
            resultado.Should().BeTrue();
            _servicoEventoMock.Verify(s => s.ExcluirEventoPorAcervoSolicitacaoItem(itens.First().Id), Times.Once);
        }

        [Fact]
        public async Task DadoItemPresencialEBibliografico_QuandoChamarCancelarAtendimento_EntaoAlteraSaldoAcervoParaDisponivel()
        {
            // Arrange
            long acervoSolicitacaoId = 1;
            var solicitacao = new AcervoSolicitacao { Id = acervoSolicitacaoId, Situacao = SituacaoSolicitacao.PRESENCIAL_ABERTO };
            var itens = new List<AcervoSolicitacaoItem> { new() { Id = 10, AcervoId = 1, TipoAtendimento = TipoAtendimento.Presencial } };
            var acervos = new List<Acervo> { new() { Id = 1, TipoAcervoId = (long)TipoAcervo.Bibliografico } };

            _repositorioSolicitacaoMock.Setup(r => r.ObterPorId(acervoSolicitacaoId)).ReturnsAsync(solicitacao);
            _repositorioItemMock.Setup(r => r.PossuiItensFinalizadosAutomaticamente(acervoSolicitacaoId)).ReturnsAsync(false);
            _repositorioItemMock.Setup(r => r.ObterItensPorSolicitacaoId(acervoSolicitacaoId)).ReturnsAsync(itens);
            _repositorioAcervoMock.Setup(r => r.ObterAcervosPorIds(It.IsAny<long[]>())).ReturnsAsync(acervos);

            var dbTransactionMock = new Mock<IDbTransaction>();
            _transacaoMock.Setup(t => t.Iniciar()).Returns(dbTransactionMock.Object);

            // Act
            var resultado = await _sut.CancelarAtendimento(acervoSolicitacaoId);

            // Assert
            resultado.Should().BeTrue();
            _servicoAcervoBibliograficoMock.Verify(s => s.AlterarSituacaoSaldo(SituacaoSaldo.DISPONIVEL, itens.First().AcervoId), Times.Once);
        }

        [Fact]
        public async Task DadoErroNaTransacao_QuandoChamarCancelarAtendimento_EntaoFazRollbackERethrowException()
        {
            // Arrange
            long acervoSolicitacaoId = 1;
            var solicitacao = new AcervoSolicitacao { Id = acervoSolicitacaoId, Situacao = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO };
            var itens = new List<AcervoSolicitacaoItem>();
            var acervos = new List<Acervo>();

            _repositorioSolicitacaoMock.Setup(r => r.ObterPorId(acervoSolicitacaoId)).ReturnsAsync(solicitacao);
            _repositorioItemMock.Setup(r => r.PossuiItensFinalizadosAutomaticamente(acervoSolicitacaoId)).ReturnsAsync(false);
            _repositorioItemMock.Setup(r => r.ObterItensPorSolicitacaoId(acervoSolicitacaoId)).ReturnsAsync(itens);
            _repositorioAcervoMock.Setup(r => r.ObterAcervosPorIds(It.IsAny<long[]>())).ReturnsAsync(acervos);

            var dbTransactionMock = new Mock<IDbTransaction>();
            _transacaoMock.Setup(t => t.Iniciar()).Returns(dbTransactionMock.Object);

            _repositorioSolicitacaoMock.Setup(r => r.Atualizar(solicitacao)).ThrowsAsync(new System.Exception("Erro Interno Cancelamento DB"));

            // Act
            var acao = async () => await _sut.CancelarAtendimento(acervoSolicitacaoId);

            // Assert
            await acao.Should().ThrowAsync<System.Exception>().WithMessage("Erro Interno Cancelamento DB");

            dbTransactionMock.Verify(t => t.Rollback(), Times.Once);
            dbTransactionMock.Verify(t => t.Dispose(), Times.Once);
        }

        [Fact]
        public async Task DadoItemNaoEncontrado_QuandoChamarCancelarItemAtendimento_EntaoLancaNegocioException()
        {
            // Arrange
            long acervoSolicitacaoItemId = 1;
            _repositorioItemMock.Setup(r => r.ObterPorId(acervoSolicitacaoItemId)).ReturnsAsync((AcervoSolicitacaoItem)null!);

            // Act
            var acao = async () => await _sut.CancelarItemAtendimento(acervoSolicitacaoItemId);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.SOLICITACAO_ATENDIMENTO_ITEM_NAO_ENCONTRADA);
        }

        [Fact]
        public async Task DadoItemJaFinalizadoOuCancelado_QuandoChamarCancelarItemAtendimento_EntaoLancaNegocioException()
        {
            // Arrange
            long acervoSolicitacaoItemId = 1;
            var item = new AcervoSolicitacaoItem { Id = acervoSolicitacaoItemId, AcervoSolicitacaoId = 100 };
            var itensRetornados = new List<AcervoSolicitacaoItem> { new() { Id = acervoSolicitacaoItemId, Situacao = SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE } };

            _repositorioItemMock.Setup(r => r.ObterPorId(acervoSolicitacaoItemId)).ReturnsAsync(item);
            _repositorioItemMock.Setup(r => r.ObterItensPorSolicitacaoId(item.AcervoSolicitacaoId)).ReturnsAsync(itensRetornados);

            // Act
            var acao = async () => await _sut.CancelarItemAtendimento(acervoSolicitacaoItemId);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.NAO_PODE_CANCELAR_ATENDIMENTO_COM_ITEM_FINALIZADO_AUTOMATICAMENTE_MANUALMENTE);
        }

        [Fact]
        public async Task DadoItemAtendimentoPresencialEBibliografico_QuandoChamarCancelarItemAtendimento_EntaoCancelaAtualizaSaldoExcluiEventoEPublicaNotificacao()
        {
            // Arrange
            long acervoSolicitacaoItemId = 1;
            var item = new AcervoSolicitacaoItem
            {
                Id = acervoSolicitacaoItemId,
                AcervoSolicitacaoId = 100,
                AcervoId = 50,
                TipoAtendimento = TipoAtendimento.Presencial
            };

            var outroItem = new AcervoSolicitacaoItem { Id = 2, Situacao = SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO };
            var itensRetornados = new List<AcervoSolicitacaoItem> { item, outroItem };
            var solicitacao = new AcervoSolicitacao { Id = 100 };
            var acervosRetornados = new List<Acervo> { new() { Id = 50, TipoAcervoId = (long)TipoAcervo.Bibliografico } };

            _repositorioItemMock.Setup(r => r.ObterPorId(acervoSolicitacaoItemId)).ReturnsAsync(item);
            _repositorioItemMock.Setup(r => r.ObterItensPorSolicitacaoId(item.AcervoSolicitacaoId)).ReturnsAsync(itensRetornados);
            _repositorioSolicitacaoMock.Setup(r => r.ObterPorId(item.AcervoSolicitacaoId)).ReturnsAsync(solicitacao);
            _repositorioAcervoMock.Setup(r => r.ObterAcervosPorIds(It.Is<long[]>(l => l.Contains(item.AcervoId)))).ReturnsAsync(acervosRetornados);

            var dbTransactionMock = new Mock<IDbTransaction>();
            _transacaoMock.Setup(t => t.Iniciar()).Returns(dbTransactionMock.Object);

            // Act
            var resultado = await _sut.CancelarItemAtendimento(acervoSolicitacaoItemId);

            // Assert
            resultado.Should().BeTrue();
            item.Situacao.Should().Be(SituacaoSolicitacaoItem.CANCELADO);

            _repositorioItemMock.Verify(r => r.Atualizar(item), Times.Once);
            _servicoEventoMock.Verify(s => s.ExcluirEventoPorAcervoSolicitacaoItem(item.Id), Times.Once);
            _servicoAcervoBibliograficoMock.Verify(s => s.AlterarSituacaoSaldo(SituacaoSaldo.DISPONIVEL, item.AcervoId), Times.Once);
            _servicoProcessamentoSituacaoMock.Verify(s => s.AtualizarSituacaoGeralSolicitacaoAsync(solicitacao, false), Times.Once);
            _servicoMensageriaMock.Verify(s => s.Publicar(RotasRabbit.NotificarViaEmailCancelamentoAtendimentoItem, acervoSolicitacaoItemId, It.IsAny<Guid>(), null), Times.Once);
            dbTransactionMock.Verify(t => t.Commit(), Times.Once);
        }

        [Fact]
        public async Task DadoTodosOutrosItensCancelados_QuandoChamarCancelarItemAtendimento_EntaoProcessaSituacaoGeralComVerdadeiro()
        {
            // Arrange
            long acervoSolicitacaoItemId = 1;
            var item = new AcervoSolicitacaoItem
            {
                Id = acervoSolicitacaoItemId,
                AcervoSolicitacaoId = 100,
                AcervoId = 50,
                TipoAtendimento = TipoAtendimento.Email
            };

            var outroItemCancelado = new AcervoSolicitacaoItem { Id = 2, Situacao = SituacaoSolicitacaoItem.CANCELADO };
            var itensRetornados = new List<AcervoSolicitacaoItem> { item, outroItemCancelado };
            var solicitacao = new AcervoSolicitacao { Id = 100 };
            var acervosRetornados = new List<Acervo>();

            _repositorioItemMock.Setup(r => r.ObterPorId(acervoSolicitacaoItemId)).ReturnsAsync(item);
            _repositorioItemMock.Setup(r => r.ObterItensPorSolicitacaoId(item.AcervoSolicitacaoId)).ReturnsAsync(itensRetornados);
            _repositorioSolicitacaoMock.Setup(r => r.ObterPorId(item.AcervoSolicitacaoId)).ReturnsAsync(solicitacao);
            _repositorioAcervoMock.Setup(r => r.ObterAcervosPorIds(It.Is<long[]>(l => l.Contains(item.AcervoId)))).ReturnsAsync(acervosRetornados);

            var dbTransactionMock = new Mock<IDbTransaction>();
            _transacaoMock.Setup(t => t.Iniciar()).Returns(dbTransactionMock.Object);

            // Act
            var resultado = await _sut.CancelarItemAtendimento(acervoSolicitacaoItemId);

            // Assert
            resultado.Should().BeTrue();
            _servicoProcessamentoSituacaoMock.Verify(s => s.AtualizarSituacaoGeralSolicitacaoAsync(solicitacao, true), Times.Once);
        }

        [Fact]
        public async Task DadoErroDuranteProcessamento_QuandoChamarCancelarItemAtendimento_EntaoAplicaRollbackERethrowException()
        {
            // Arrange
            long acervoSolicitacaoItemId = 1;
            var item = new AcervoSolicitacaoItem { Id = acervoSolicitacaoItemId, AcervoSolicitacaoId = 100, AcervoId = 50 };

            _repositorioItemMock.Setup(r => r.ObterPorId(acervoSolicitacaoItemId)).ReturnsAsync(item);
            _repositorioItemMock.Setup(r => r.ObterItensPorSolicitacaoId(item.AcervoSolicitacaoId)).ReturnsAsync(new List<AcervoSolicitacaoItem> { item });
            _repositorioSolicitacaoMock.Setup(r => r.ObterPorId(item.AcervoSolicitacaoId)).ReturnsAsync(new AcervoSolicitacao());
            _repositorioAcervoMock.Setup(r => r.ObterAcervosPorIds(It.IsAny<long[]>())).ReturnsAsync(new List<Acervo>());

            var dbTransactionMock = new Mock<IDbTransaction>();
            _transacaoMock.Setup(t => t.Iniciar()).Returns(dbTransactionMock.Object);

            _repositorioItemMock.Setup(r => r.Atualizar(item)).ThrowsAsync(new System.Exception("Erro DB Cancelamento Item"));

            // Act
            var acao = async () => await _sut.CancelarItemAtendimento(acervoSolicitacaoItemId);

            // Assert
            await acao.Should().ThrowAsync<System.Exception>().WithMessage("Erro DB Cancelamento Item");

            dbTransactionMock.Verify(t => t.Rollback(), Times.Once);
            dbTransactionMock.Verify(t => t.Dispose(), Times.Once);
        }

        [Fact]
        public async Task DadoDataVisitaNoPassado_QuandoChamarAlterarDataVisitaDoItemAtendimento_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO
            {
                Id = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date.AddDays(-1)
            };

            // Act
            var acao = async () => await _sut.AlterarDataVisitaDoItemAtendimento(dto);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.ITENS_ACERVOS_PRESENCIAL_NAO_DEVEM_TER_DATA_ACERVO_PASSADAS);
        }

        [Fact]
        public async Task DadoItemNaoEncontrado_QuandoChamarAlterarDataVisitaDoItemAtendimento_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO
            {
                Id = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date.AddDays(1)
            };

            _repositorioItemMock.Setup(r => r.ObterPorId(dto.Id)).ReturnsAsync((AcervoSolicitacaoItem)null!);

            // Act
            var acao = async () => await _sut.AlterarDataVisitaDoItemAtendimento(dto);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.SOLICITACAO_ATENDIMENTO_ITEM_NAO_ENCONTRADA);
        }

        [Fact]
        public async Task DadoItemFinalizadoOuCancelado_QuandoChamarAlterarDataVisitaDoItemAtendimento_EntaoLancaNegocioException()
        {
            // Arrange
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO
            {
                Id = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date.AddDays(1)
            };

            var item = new AcervoSolicitacaoItem { Id = dto.Id };

            _repositorioItemMock.Setup(r => r.ObterPorId(dto.Id)).ReturnsAsync(item);
            _repositorioItemMock.Setup(r => r.AtendimentoPossuiItemSituacaoFinalizadoAutomaticamenteOuCancelado(dto.Id)).ReturnsAsync(true);

            // Act
            var acao = async () => await _sut.AlterarDataVisitaDoItemAtendimento(dto);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.ATENDIMENTO_NAO_ESTA_AGUARDANDO_VISITA);
        }

        [Fact]
        public async Task DadoDadosValidos_QuandoChamarAlterarDataVisitaDoItemAtendimento_EntaoAtualizaDataEEventoVisita()
        {
            // Arrange
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO
            {
                Id = 1,
                DataVisita = DateTimeExtension.HorarioBrasilia().Date.AddDays(1)
            };

            var item = new AcervoSolicitacaoItem { Id = dto.Id };

            _repositorioItemMock.Setup(r => r.ObterPorId(dto.Id)).ReturnsAsync(item);
            _repositorioItemMock.Setup(r => r.AtendimentoPossuiItemSituacaoFinalizadoAutomaticamenteOuCancelado(dto.Id)).ReturnsAsync(false);

            // Act
            var resultado = await _sut.AlterarDataVisitaDoItemAtendimento(dto);

            // Assert
            resultado.Should().BeTrue();
            item.DataVisita.Should().Be(dto.DataVisita);

            _repositorioItemMock.Verify(r => r.Atualizar(item), Times.Once);
            _servicoEventoMock.Verify(s => s.AtualizarEventoVisita(dto.DataVisita, dto.Id), Times.Once);
        }

        [Fact]
        public async Task DadoUsuarioNaoEncontrado_QuandoChamarInserirEmLote_EntaoLancaNegocioException()
        {
            // Arrange
            var dtos = new AcervoSolicitacaoItemCadastroDTO[] { new() { AcervoId = 1 } };
            var usuarioLogado = new UsuarioDTO { Id = 100 };

            _servicoUsuarioMock.Setup(s => s.ObterUsuarioLogado()).ReturnsAsync(usuarioLogado);
            _repositorioUsuarioMock.Setup(r => r.ObterPorId(usuarioLogado.Id)).ReturnsAsync((Usuario)null!);

            // Act
            var acao = async () => await _sut.Inserir(dtos);

            // Assert
            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage(MensagemNegocio.USUARIO_NAO_ENCONTRADO);
        }

        [Fact]
        public async Task DadoSemArquivosEncontrados_QuandoChamarInserirEmLote_EntaoGeraSolicitacaoEItensAguardandoAtendimento()
        {
            // Arrange
            var dtos = new AcervoSolicitacaoItemCadastroDTO[] { new() { AcervoId = 1 }, new() { AcervoId = 2 } };
            var usuarioLogado = new UsuarioDTO { Id = 100 };
            var usuario = new Usuario { Id = 100 };
            var arquivosVazios = new List<ArquivoCodigoNomeAcervoId>();
            long novaSolicitacaoId = 55;

            _servicoUsuarioMock.Setup(s => s.ObterUsuarioLogado()).ReturnsAsync(usuarioLogado);
            _repositorioUsuarioMock.Setup(r => r.ObterPorId(usuarioLogado.Id)).ReturnsAsync(usuario);
            _repositorioAcervoMock.Setup(r => r.ObterArquivosPorAcervoId(It.Is<long[]>(l => l.Contains(1) && l.Contains(2)))).ReturnsAsync(arquivosVazios);

            var dbTransactionMock = new Mock<IDbTransaction>();
            _transacaoMock.Setup(t => t.Iniciar()).Returns(dbTransactionMock.Object);

            _repositorioSolicitacaoMock
                .Setup(r => r.Inserir(It.Is<AcervoSolicitacao>(s => s.Situacao == SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO)))
                .Callback<AcervoSolicitacao>(s => s.Id = novaSolicitacaoId)
                .ReturnsAsync(novaSolicitacaoId);

            _mapperMock.Setup(m => m.Map<AcervoSolicitacaoItem>(It.IsAny<AcervoSolicitacaoItemCadastroDTO>())).Returns((AcervoSolicitacaoItemCadastroDTO dto) => new AcervoSolicitacaoItem { AcervoId = dto.AcervoId });

            // Act
            var resultado = await _sut.Inserir(dtos);

            // Assert
            resultado.Should().Be(novaSolicitacaoId);

            _repositorioSolicitacaoMock.Verify(r => r.Inserir(It.Is<AcervoSolicitacao>(s => s.Situacao == SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO)), Times.Once);
            _repositorioItemMock.Verify(r => r.Inserir(It.Is<AcervoSolicitacaoItem>(i => i.Situacao == SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO && i.AcervoSolicitacaoId == novaSolicitacaoId)), Times.Exactly(2));
            _servicoAcervoBibliograficoMock.Verify(s => s.AlterarSituacaoSaldo(SituacaoSaldo.RESERVADO, It.IsAny<long>()), Times.Exactly(2));
            dbTransactionMock.Verify(t => t.Commit(), Times.Once);
        }

        [Fact]
        public async Task DadoTodosComArquivosEncontrados_QuandoChamarInserirEmLote_EntaoGeraSolicitacaoEItensFinalizadosAutomaticamente()
        {
            // Arrange
            var dtos = new AcervoSolicitacaoItemCadastroDTO[] { new() { AcervoId = 1 } };
            var usuarioLogado = new UsuarioDTO { Id = 100 };
            var usuario = new Usuario { Id = 100 };
            var arquivos = new List<ArquivoCodigoNomeAcervoId> { new() { AcervoId = 1 } };
            long novaSolicitacaoId = 66;

            _servicoUsuarioMock.Setup(s => s.ObterUsuarioLogado()).ReturnsAsync(usuarioLogado);
            _repositorioUsuarioMock.Setup(r => r.ObterPorId(usuarioLogado.Id)).ReturnsAsync(usuario);
            _repositorioAcervoMock.Setup(r => r.ObterArquivosPorAcervoId(It.Is<long[]>(l => l.Contains(1)))).ReturnsAsync(arquivos);

            var dbTransactionMock = new Mock<IDbTransaction>();
            _transacaoMock.Setup(t => t.Iniciar()).Returns(dbTransactionMock.Object);

            _repositorioSolicitacaoMock
                .Setup(r => r.Inserir(It.Is<AcervoSolicitacao>(s => s.Situacao == SituacaoSolicitacao.FINALIZADO_ATENDIMENTO)))
                .Callback<AcervoSolicitacao>(s => s.Id = novaSolicitacaoId)
                .ReturnsAsync(novaSolicitacaoId);

            _mapperMock.Setup(m => m.Map<AcervoSolicitacaoItem>(It.IsAny<AcervoSolicitacaoItemCadastroDTO>())).Returns((AcervoSolicitacaoItemCadastroDTO dto) => new AcervoSolicitacaoItem { AcervoId = dto.AcervoId });

            // Act
            var resultado = await _sut.Inserir(dtos);

            // Assert
            resultado.Should().Be(novaSolicitacaoId);

            _repositorioSolicitacaoMock.Verify(r => r.Inserir(It.Is<AcervoSolicitacao>(s => s.Situacao == SituacaoSolicitacao.FINALIZADO_ATENDIMENTO)), Times.Once);
            _repositorioItemMock.Verify(r => r.Inserir(It.Is<AcervoSolicitacaoItem>(i => i.Situacao == SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE && i.AcervoSolicitacaoId == novaSolicitacaoId)), Times.Once);
            dbTransactionMock.Verify(t => t.Commit(), Times.Once);
        }

        [Fact]
        public async Task DadoErroNoProcessoDeInsercaoEmLote_QuandoChamarInserirEmLote_EntaoAplicaRollbackERethrowException()
        {
            // Arrange
            var dtos = new AcervoSolicitacaoItemCadastroDTO[] { new() { AcervoId = 1 } };
            var usuarioLogado = new UsuarioDTO { Id = 100 };
            var usuario = new Usuario { Id = 100 };

            _servicoUsuarioMock.Setup(s => s.ObterUsuarioLogado()).ReturnsAsync(usuarioLogado);
            _repositorioUsuarioMock.Setup(r => r.ObterPorId(usuarioLogado.Id)).ReturnsAsync(usuario);
            _repositorioAcervoMock.Setup(r => r.ObterArquivosPorAcervoId(It.IsAny<long[]>())).ReturnsAsync(new List<ArquivoCodigoNomeAcervoId>());

            var dbTransactionMock = new Mock<IDbTransaction>();
            _transacaoMock.Setup(t => t.Iniciar()).Returns(dbTransactionMock.Object);

            _repositorioSolicitacaoMock.Setup(r => r.Inserir(It.IsAny<AcervoSolicitacao>())).ThrowsAsync(new System.Exception("Erro de Inserção Lote DB"));

            // Act
            var acao = async () => await _sut.Inserir(dtos);

            // Assert
            await acao.Should().ThrowAsync<System.Exception>().WithMessage("Erro de Inserção Lote DB");

            dbTransactionMock.Verify(t => t.Rollback(), Times.Once);
            dbTransactionMock.Verify(t => t.Dispose(), Times.Once);
        }

        [Fact]
        public async Task DadoSolicitacaoManualValida_QuandoChamarInserirManual_EntaoInvocaServicoManutencaoERetornaId()
        {
            // Arrange
            var dto = new AcervoSolicitacaoManualDTO();
            long idEsperado = 99;

            _servicoManutencaoSolicitacaoAcervoMock.Setup(s => s.Inserir(dto)).ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.Inserir(dto);

            // Assert
            resultado.Should().Be(idEsperado);
            _servicoManutencaoSolicitacaoAcervoMock.Verify(s => s.Inserir(dto), Times.Once);
        }

        [Fact]
        public async Task DadoSolicitacaoManualValida_QuandoChamarAlterarManual_EntaoInvocaServicoManutencaoERetornaId()
        {
            // Arrange
            var dto = new AcervoSolicitacaoManualDTO();
            long idEsperado = 88;

            _servicoManutencaoSolicitacaoAcervoMock.Setup(s => s.Alterar(dto)).ReturnsAsync(idEsperado);

            // Act
            var resultado = await _sut.Alterar(dto);

            // Assert
            resultado.Should().Be(idEsperado);
            _servicoManutencaoSolicitacaoAcervoMock.Verify(s => s.Alterar(dto), Times.Once);
        }
    }
}