using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoImportacaoArquivoAcervoTeste
    {
        private readonly AutoMocker _mocker;
        private readonly ServicoImportacaoArquivoAcervo _servico;

        public ServicoImportacaoArquivoAcervoTeste()
        {
            _mocker = new AutoMocker();
            _servico = _mocker.CreateInstance<ServicoImportacaoArquivoAcervo>();

            // Configuração padrão para permitir a leitura do limite de linhas
            _mocker.GetMock<IRepositorioParametroSistema>()
                .Setup(x => x.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "1000" });
        }

        #region Testes Excluir

        [Fact]
        public async Task DadoIdValido_QuandoExcluir_EntaoDeveRetornarVerdadeiro()
        {
            // Arrange
            long idImportacao = 1;
            var importacaoArquivo = new ImportacaoArquivo
            {
                Id = idImportacao,
                Nome = "teste.xlsx",
                TipoAcervo = TipoAcervo.Bibliografico,
                Status = ImportacaoStatus.Pendente,
                Conteudo = "[]"
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterPorId(idImportacao))
                .ReturnsAsync(importacaoArquivo);

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Remover(idImportacao))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _servico.Excluir(idImportacao);

            // Assert
            Assert.True(resultado);
            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Verify(x => x.Remover(idImportacao), Times.Once);
        }

        [Fact]
        public async Task DadoIdInvalido_QuandoExcluir_EntaoDeveRetornarVerdadeiro()
        {
            // Arrange
            long idImportacao = 999;

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Remover(idImportacao))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _servico.Excluir(idImportacao);

            // Assert
            Assert.True(resultado);
            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Verify(x => x.Remover(idImportacao), Times.Once);
        }

        [Fact]
        public async Task DadoIdZero_QuandoExcluir_EntaoDeveRetornarVerdadeiro()
        {
            // Arrange
            long idImportacao = 0;

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Remover(idImportacao))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _servico.Excluir(idImportacao);

            // Assert
            Assert.True(resultado);
            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Verify(x => x.Remover(idImportacao), Times.Once);
        }

        [Fact]
        public async Task DadoQueRepositorioLancaExcecao_QuandoExcluir_EntaoDeveLancarExcecao()
        {
            // Arrange
            long idImportacao = 1;
            var mensagemErro = "Erro ao remover";

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Remover(idImportacao))
                .ThrowsAsync(new Exception(mensagemErro));

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<Exception>(() => _servico.Excluir(idImportacao));
            Assert.Equal(mensagemErro, excecao.Message);
        }

        [Fact]
        public async Task DadoIdNegativo_QuandoExcluir_EntaoDeveRetornarVerdadeiro()
        {
            // Arrange
            long idImportacao = -1;

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Remover(idImportacao))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _servico.Excluir(idImportacao);

            // Assert
            Assert.True(resultado);
            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Verify(x => x.Remover(idImportacao), Times.Once);
        }

        [Fact]
        public async Task DadoQueMetodoEhChamadoVariasVezes_QuandoExcluir_EntaoDeveChamarRepositorioVariasVezes()
        {
            // Arrange
            var ids = new long[] { 1, 2, 3, 4, 5 };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Remover(It.IsAny<long>()))
                .Returns(Task.CompletedTask);

            // Act
            foreach (var id in ids)
            {
                await _servico.Excluir(id);
            }

            // Assert
            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Verify(x => x.Remover(It.IsAny<long>()), Times.Exactly(ids.Length));
        }

        [Fact]
        public async Task DadoQueRepositorioRetornaSuccesso_QuandoExcluir_EntaoDevePropagarlReturn()
        {
            // Arrange
            long idImportacao = 100;
            var resultado = true;

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Remover(idImportacao))
                .Returns(Task.CompletedTask);

            // Act
            var retorno = await _servico.Excluir(idImportacao);

            // Assert
            Assert.Equal(resultado, retorno);
        }

        [Fact]
        public async Task DadoQueRepositorioBuscaImportacao_QuandoExcluir_EntaoDevePropagarlReturn()
        {
            // Arrange
            long idImportacao = 50;

            var importacaoArquivo = new ImportacaoArquivo
            {
                Id = idImportacao,
                Nome = "teste_importacao.xlsx",
                TipoAcervo = TipoAcervo.Bibliografico,
                Status = ImportacaoStatus.Sucesso,
                Conteudo = "[{\"NumeroLinha\": 2}]"
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterPorId(idImportacao))
                .ReturnsAsync(importacaoArquivo);

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Remover(idImportacao))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _servico.Excluir(idImportacao);

            // Assert
            Assert.True(resultado);
            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Verify(x => x.Remover(idImportacao), Times.Once);
        }

        #endregion

        #region Testes Construtor

        [Fact]
        public void Construtor_DeveCriarInstanciaComSucessoComTodosOsParametrosValidos()
        {
            // Act & Assert
            Assert.NotNull(_servico);
        }

        [Fact]
        public void Construtor_DeveHerdarDeServicoImportacaoArquivoBase()
        {
            // Act & Assert
            Assert.IsType<ServicoImportacaoArquivoBase>(_servico, exactMatch: false);
        }

        [Fact]
        public void Construtor_DeveImplementarIServicoImportacaoArquivoAcervo()
        {
            // Act & Assert
            Assert.IsType<IServicoImportacaoArquivoAcervo>(_servico, exactMatch: false);
        }

        #endregion

        #region Testes Métodos Herdados

        [Fact]
        public async Task DadoQueServicoBaseTemMetodoRemover_QuandoExcluirEhChamado_EntaoDeveUtilizarMetodoRemover()
        {
            // Arrange
            long idImportacao = 1;

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Remover(idImportacao))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _servico.Excluir(idImportacao);

            // Assert
            Assert.True(resultado);
        }

        #endregion

        #region Testes Integração com Base

        [Fact]
        public async Task DadoQueServicoTeracessoARepositorio_QuandoExcluir_EntaoDevePassarIdCorreto()
        {
            // Arrange
            long idEsperado = 42;

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Remover(idEsperado))
                .Returns(Task.CompletedTask);

            // Act
            await _servico.Excluir(idEsperado);

            // Assert
            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Verify(x => x.Remover(It.Is<long>(id => id == idEsperado)), Times.Once);
        }

        [Fact]
        public async Task DadoQueMultiplasInstanciasDoServico_QuandoExcluir_EntaoDeveFuncionarIndependentemente()
        {
            // Arrange
            var servico1 = _mocker.CreateInstance<ServicoImportacaoArquivoAcervo>();
            var servico2 = _mocker.CreateInstance<ServicoImportacaoArquivoAcervo>();

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Remover(It.IsAny<long>()))
                .Returns(Task.CompletedTask);

            // Act
            var resultado1 = await servico1.Excluir(1);
            var resultado2 = await servico2.Excluir(2);

            // Assert
            Assert.True(resultado1);
            Assert.True(resultado2);

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Verify(x => x.Remover(It.IsAny<long>()), Times.Exactly(2));
        }

        [Fact]
        public async Task DadoQueServicoTeracessoAOutrosDependencias_QuandoExcluir_EntaoNaoDeveAfetar()
        {
            // Arrange
            long idImportacao = 1;

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Remover(idImportacao))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _servico.Excluir(idImportacao);

            // Assert
            Assert.True(resultado);

            // Verifica que outras dependências não foram afetadas
            _mocker.GetMock<IServicoMaterial>().VerifyNoOtherCalls();
        }

        #endregion

        #region Testes Comportamento Assíncrono

        [Fact]
        public async Task DadoQueMetodoEhAssincrono_QuandoExcluir_EntaoDeveCompleteATask()
        {
            // Arrange
            long idImportacao = 1;

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Remover(idImportacao))
                .Returns(Task.CompletedTask);

            // Act
            var tarefa = _servico.Excluir(idImportacao);

            // Assert
            await Assert.IsType<Task<bool>>(tarefa);
            var resultado = await tarefa;
            Assert.True(resultado);
        }

        [Fact]
        public async Task DadoQueMultiplasChamamdasAsincrona_QuandoExcluir_EntaoDeveTratarTodasCorretamente()
        {
            // Arrange
            var ids = new long[] { 1, 2, 3 };
            var tarefas = new List<Task<bool>>();

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Remover(It.IsAny<long>()))
                .Returns(Task.CompletedTask);

            // Act
            foreach (var id in ids)
            {
                tarefas.Add(_servico.Excluir(id));
            }

            var resultados = await Task.WhenAll(tarefas);

            // Assert
            Assert.All(resultados, resultado => Assert.True(resultado));
            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Verify(x => x.Remover(It.IsAny<long>()), Times.Exactly(ids.Length));
        }

        #endregion
    }
}
