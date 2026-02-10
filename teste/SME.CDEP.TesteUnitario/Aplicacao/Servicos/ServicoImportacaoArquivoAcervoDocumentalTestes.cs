using Bogus;
using Moq;
using Moq.AutoMock;
using Newtonsoft.Json;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoImportacaoArquivoAcervoDocumentalTestes
    {
        private readonly AutoMocker _mocker;
        private readonly ServicoImportacaoArquivoAcervoDocumental _servico;
        private readonly Faker _faker;

        public ServicoImportacaoArquivoAcervoDocumentalTestes()
        {
            _mocker = new AutoMocker();
            _faker = new Faker("pt_BR");
            _servico = _mocker.CreateInstance<ServicoImportacaoArquivoAcervoDocumental>();

            // Mock padrão necessário para a classe base
            _mocker.GetMock<IRepositorioParametroSistema>()
               .Setup(x => x.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
               .ReturnsAsync(new ParametroSistema { Valor = "1000" });
        }

        [Fact]
        public async Task DadoQueArquivoExisteELinhaNaoEhUnica_QuandoRemoverLinhaDoArquivo_EntaoDeveRemoverESalvar()
        {
            // Arrange
            long idImportacao = 10;
            int linhaParaRemover = 1;

            // Cria lista com 2 linhas
            var linhas = new List<AcervoDocumentalLinhaDTO>
            {
                GerarLinhaDocumentalFake(1),
                GerarLinhaDocumentalFake(2)
            };

            var conteudoJson = JsonConvert.SerializeObject(linhas);

            var arquivo = new ImportacaoArquivo
            {
                Id = idImportacao,
                TipoAcervo = TipoAcervo.DocumentacaoTextual,
                Conteudo = conteudoJson,
                Status = ImportacaoStatus.Erros
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterPorId(idImportacao))
                .ReturnsAsync(arquivo);

            var linhaDto = new LinhaDTO { NumeroLinha = linhaParaRemover };

            // Act
            var resultado = await _servico.RemoverLinhaDoArquivo(idImportacao, linhaDto);

            // Assert
            Assert.True(resultado);

            // Verifica se salvou mantendo apenas a linha 2
            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Verify(x => x.Salvar(It.Is<ImportacaoArquivo>(imp =>
                    imp.Id == idImportacao &&
                    !imp.Conteudo.Contains($"\"NumeroLinha\":{linhaParaRemover}") && // Linha 1 removida
                    imp.Conteudo.Contains("\"NumeroLinha\":2") // Linha 2 mantida
                )), Times.Once);
        }

        [Fact]
        public async Task DadoQueLinhaEhCorrigidaENaoRestamErros_QuandoAtualizarLinhaParaSucesso_EntaoDeveSalvarComStatusSucesso()
        {
            // Arrange
            long idImportacao = 20;
            int linhaAlvo = 1;

            // Prepara uma linha que estava com erro
            var linha = GerarLinhaDocumentalFake(linhaAlvo);
            linha.PossuiErros = true;
            linha.Titulo.PossuiErro = true; // Simula erro específico

            var conteudoJson = JsonConvert.SerializeObject(new List<AcervoDocumentalLinhaDTO> { linha });

            var arquivo = new ImportacaoArquivo
            {
                Id = idImportacao,
                TipoAcervo = TipoAcervo.DocumentacaoTextual,
                Conteudo = conteudoJson,
                Status = ImportacaoStatus.Erros
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterPorId(idImportacao))
                .ReturnsAsync(arquivo);

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Salvar(It.IsAny<ImportacaoArquivo>()))
                .ReturnsAsync(idImportacao);

            var linhaDto = new LinhaDTO { NumeroLinha = linhaAlvo };

            // Act
            var resultado = await _servico.AtualizarLinhaParaSucesso(idImportacao, linhaDto);

            // Assert
            Assert.True(resultado);

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Verify(x => x.Salvar(It.Is<ImportacaoArquivo>(imp =>
                    imp.Id == idImportacao &&
                    imp.Status == ImportacaoStatus.Sucesso && // Deve virar Sucesso
                    imp.Conteudo.Contains("\"PossuiErros\":false") && // Flag de erro removida
                    imp.Conteudo.Contains($"\"NumeroLinha\":{linhaAlvo}")
                )), Times.Once);
        }

        [Fact]
        public async Task DadoQueLinhaEhCorrigidaMasExistemOutrosErros_QuandoAtualizarLinhaParaSucesso_EntaoDeveManterStatusErros()
        {
            // Arrange
            long idImportacao = 30;
            int linhaAlvo = 1;
            int linhaComErro = 2;

            // Linha 1: Com erro, será corrigida
            var linha1 = GerarLinhaDocumentalFake(linhaAlvo);
            linha1.PossuiErros = true;

            // Linha 2: Com erro, permanecerá assim
            var linha2 = GerarLinhaDocumentalFake(linhaComErro);
            linha2.PossuiErros = true;

            var conteudoJson = JsonConvert.SerializeObject(new List<AcervoDocumentalLinhaDTO> { linha1, linha2 });

            var arquivo = new ImportacaoArquivo
            {
                Id = idImportacao,
                TipoAcervo = TipoAcervo.DocumentacaoTextual,
                Conteudo = conteudoJson,
                Status = ImportacaoStatus.Erros
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterPorId(idImportacao))
                .ReturnsAsync(arquivo);

            var linhaDto = new LinhaDTO { NumeroLinha = linhaAlvo };

            // Act
            var resultado = await _servico.AtualizarLinhaParaSucesso(idImportacao, linhaDto);

            // Assert
            Assert.True(resultado);

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Verify(x => x.Salvar(It.Is<ImportacaoArquivo>(imp =>
                    imp.Id == idImportacao &&
                    imp.Status == ImportacaoStatus.Erros && // Continua com erro por causa da linha 2
                    imp.Conteudo.Contains($"\"NumeroLinha\":{linhaAlvo}") &&
                    imp.Conteudo.Contains($"\"NumeroLinha\":{linhaComErro}")
                )), Times.Once);
        }
        [Fact]
        public async Task DadoQueNaoExisteImportacaoPendente_QuandoObterImportacaoPendente_EntaoDeveRetornarNulo()
        {
            // Act
            var resultado = await _servico.ObterImportacaoPendente();

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task DadoQueExisteImportacaoPendente_QuandoObterImportacaoPendente_EntaoDeveRetornarDTOComSucessoEErrosSeparados()
        {
            // Arrange
            var linhaSucesso = GerarLinhaDocumentalFake(1);
            var linhaErro = GerarLinhaDocumentalFake(2);
            linhaErro.PossuiErros = true;
            linhaErro.Titulo.PossuiErro = true;
            linhaErro.Titulo.Mensagem = "Erro no Título";

            var linhas = new List<AcervoDocumentalLinhaDTO> { linhaSucesso, linhaErro };
            var conteudoJson = JsonConvert.SerializeObject(linhas);

            var importacaoArquivo = new ImportacaoArquivo
            {
                Id = 1,
                Nome = "documentos.xlsx",
                TipoAcervo = TipoAcervo.DocumentacaoTextual,
                Status = ImportacaoStatus.Pendente,
                Conteudo = conteudoJson,
                CriadoEm = DateTime.Now
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterUltimaImportacao(TipoAcervo.DocumentacaoTextual))
                .ReturnsAsync(importacaoArquivo);

            ConfigurarMocksDaBase();

            // Act
            var resultado = await _servico.ObterImportacaoPendente();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(importacaoArquivo.Id, resultado.Id);

            // Valida linha de sucesso
            Assert.Single(resultado.Sucesso);
            var sucesso = resultado.Sucesso.First();
            Assert.Equal(1, sucesso.NumeroLinha);
            // Valida se o Tombo/Código foi montado corretamente (pelo método privado ObterCodigo)
            // No helper "ANTIGO" e "NOVO" são usados
            Assert.Equal("ANTIGO/NOVO", sucesso.Tombo);

            // Valida linha de erro
            Assert.Single(resultado.Erros);
            Assert.Equal(2, resultado.Erros.First().NumeroLinha);
            Assert.Contains("Erro no Título", resultado.Erros.First().RetornoErro.ErrosCampos);
        }

        [Fact]
        public async Task DadoQueImportacaoNaoExiste_QuandoObterImportacaoPorId_EntaoDeveRetornarNulo()
        {
            // Arrange
            long id = 99;

            // Act
            var resultado = await _servico.ObterImportacaoPorId(id);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task DadoQueLinhaPossuiTodosOsErrosPossiveis_QuandoObterImportacaoPendente_EntaoDeveMapearTodasAsMensagensDeErro()
        {
            // Arrange
            // Cria uma linha com TODOS os campos com erro para testar todos os IFs do ObterMensagemErroLinha
            var linhaCaos = new AcervoDocumentalLinhaDTO
            {
                NumeroLinha = 1,
                PossuiErros = true,
                Titulo = CriarCampoComErro("Erro Título"),
                Codigo = CriarCampoComErro("Erro Código"),
                CodigoNovo = CriarCampoComErro("Erro Código Novo"),
                Material = CriarCampoComErro("Erro Material"),
                Idioma = CriarCampoComErro("Erro Idioma"),
                Autor = CriarCampoComErro("Erro Autor"),
                Ano = CriarCampoComErro("Erro Ano"),
                NumeroPaginas = CriarCampoComErro("Erro Páginas"),
                Volume = CriarCampoComErro("Erro Volume"),
                Descricao = CriarCampoComErro("Erro Descrição"),
                TipoAnexo = CriarCampoComErro("Erro Tipo Anexo"),
                Largura = CriarCampoComErro("Erro Largura"),
                Altura = CriarCampoComErro("Erro Altura"),
                TamanhoArquivo = CriarCampoComErro("Erro Tamanho"),
                AcessoDocumento = CriarCampoComErro("Erro Acesso"),
                Localizacao = CriarCampoComErro("Erro Localização"),
                CopiaDigital = CriarCampoComErro("Erro Cópia"),
                EstadoConservacao = CriarCampoComErro("Erro Conservação")
            };

            var conteudoJson = JsonConvert.SerializeObject(new List<AcervoDocumentalLinhaDTO> { linhaCaos });
            var importacaoArquivo = new ImportacaoArquivo
            {
                Id = 1,
                TipoAcervo = TipoAcervo.DocumentacaoTextual,
                Conteudo = conteudoJson
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterUltimaImportacao(TipoAcervo.DocumentacaoTextual))
                .ReturnsAsync(importacaoArquivo);

            ConfigurarMocksDaBase();

            // Act
            var resultado = await _servico.ObterImportacaoPendente();

            // Assert
            Assert.NotNull(resultado);
            var linhaErro = resultado.Erros.First();
            var mensagens = linhaErro.RetornoErro.ErrosCampos;

            // Verifica se as 18 mensagens foram capturadas (total de ifs no método privado)
            Assert.Equal(18, mensagens.Length);

            Assert.Contains("Erro Título", mensagens);
            Assert.Contains("Erro Código Novo", mensagens);
            Assert.Contains("Erro Conservação", mensagens);
        }

        #region Helpers

        private AcervoDocumentalLinhaDTO GerarLinhaDocumentalFake(int numeroLinha)
        {
            // Simulação básica do DTO para permitir a serialização
            return new AcervoDocumentalLinhaDTO
            {
                NumeroLinha = numeroLinha,
                PossuiErros = false,
                // Inicializa propriedades complexas para evitar NullReference caso necessário
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = _faker.Lorem.Sentence() },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = "ANTIGO" },
                CodigoNovo = new LinhaConteudoAjustarDTO { Conteudo = "NOVO" },
                Material = new LinhaConteudoAjustarDTO(),
                Idioma = new LinhaConteudoAjustarDTO(),
                Autor = new LinhaConteudoAjustarDTO(),
                Ano = new LinhaConteudoAjustarDTO(),
                NumeroPaginas = new LinhaConteudoAjustarDTO(),
                Volume = new LinhaConteudoAjustarDTO(),
                Descricao = new LinhaConteudoAjustarDTO(),
                TipoAnexo = new LinhaConteudoAjustarDTO(),
                Largura = new LinhaConteudoAjustarDTO(),
                Altura = new LinhaConteudoAjustarDTO(),
                TamanhoArquivo = new LinhaConteudoAjustarDTO(),
                AcessoDocumento = new LinhaConteudoAjustarDTO(),
                Localizacao = new LinhaConteudoAjustarDTO(),
                CopiaDigital = new LinhaConteudoAjustarDTO(),
                EstadoConservacao = new LinhaConteudoAjustarDTO()
            };
        }

        private void ConfigurarMocksDaBase()
        {
            _mocker.GetMock<IServicoCreditoAutor>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoMaterial>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoEditora>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoSerieColecao>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoIdioma>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoAssunto>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoConservacao>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoAcessoDocumento>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoCromia>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoSuporte>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoFormato>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
        }

        private LinhaConteudoAjustarDTO CriarCampoComErro(string mensagem)
        {
            return new LinhaConteudoAjustarDTO
            {
                Conteudo = "Valor Inválido",
                PossuiErro = true,
                Mensagem = mensagem
            };
        }

        #endregion
    }
}
