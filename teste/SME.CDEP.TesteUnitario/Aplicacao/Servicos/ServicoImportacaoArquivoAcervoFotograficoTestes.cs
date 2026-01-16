using Bogus;
using ClosedXML.Excel;
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
    public class ServicoImportacaoArquivoAcervoFotograficoTestes
    {
        private readonly AutoMocker _mocker;
        private readonly ServicoImportacaoArquivoAcervoFotografico _servico;
        private readonly Faker _faker;

        public ServicoImportacaoArquivoAcervoFotograficoTestes()
        {
            _mocker = new AutoMocker();
            _faker = new Faker("pt_BR");
            _servico = _mocker.CreateInstance<ServicoImportacaoArquivoAcervoFotografico>();

            // Configuração padrão para evitar erros no construtor da base
            _mocker.GetMock<IRepositorioParametroSistema>()
               .Setup(x => x.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
               .ReturnsAsync(new ParametroSistema { Valor = "1000" });
        }

        [Fact]
        public async Task RemoverLinhaDoArquivo_DadoQueArquivoExisteELinhaNaoEhUnica_QuandoExecutado_EntaoDeveRemoverLinhaESalvar()
        {
            // Arrange
            long idImportacao = 10;
            int linhaParaRemover = 1;

            // Cria lista com 2 linhas (1 e 2)
            var linhas = new List<AcervoFotograficoLinhaDTO>
            {
                GerarLinhaFotograficaFake(1),
                GerarLinhaFotograficaFake(2)
            };

            var conteudoJson = JsonConvert.SerializeObject(linhas);

            var arquivo = new ImportacaoArquivo
            {
                Id = idImportacao,
                TipoAcervo = TipoAcervo.Fotografico,
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

            // Verifica se o método Salvar foi chamado com o JSON contendo apenas a linha 2
            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Verify(x => x.Salvar(It.Is<ImportacaoArquivo>(imp =>
                    imp.Id == idImportacao &&
                    !imp.Conteudo.Contains($"\"NumeroLinha\":{linhaParaRemover}") && // Linha 1 removida
                    imp.Conteudo.Contains("\"NumeroLinha\":2") // Linha 2 mantida
                )), Times.Once);
        }

        [Fact]
        public async Task RemoverLinhaDoArquivo_DadoQueEhAUnicaLinhaDoArquivo_QuandoExecutado_EntaoDeveLancarNegocioException()
        {
            // Arrange
            long idImportacao = 10;
            // Cria lista com apenas 1 linha
            var linhas = new List<AcervoFotograficoLinhaDTO> { GerarLinhaFotograficaFake(1) };

            var arquivo = new ImportacaoArquivo
            {
                Id = idImportacao,
                TipoAcervo = TipoAcervo.Fotografico,
                Conteudo = JsonConvert.SerializeObject(linhas)
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterPorId(idImportacao))
                .ReturnsAsync(arquivo);

            var linhaDto = new LinhaDTO { NumeroLinha = 1 };

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() =>
                _servico.RemoverLinhaDoArquivo(idImportacao, linhaDto));

            Assert.Equal(Constantes.NAO_EH_POSSIVEL_EXCLUIR_A_UNICA_LINHA_DO_ARQUIVO, excecao.Message);
        }

        [Fact]
        public async Task RemoverLinhaDoArquivo_DadoQueLinhaNaoExisteNoJson_QuandoExecutado_EntaoDeveLancarNegocioException()
        {
            // Arrange
            long idImportacao = 10;
            // Cria lista com linha 1, mas vamos tentar remover a 99
            var linhas = new List<AcervoFotograficoLinhaDTO> { GerarLinhaFotograficaFake(1), GerarLinhaFotograficaFake(2) };

            var arquivo = new ImportacaoArquivo
            {
                Id = idImportacao,
                TipoAcervo = TipoAcervo.Fotografico,
                Conteudo = JsonConvert.SerializeObject(linhas)
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterPorId(idImportacao))
                .ReturnsAsync(arquivo);

            var linhaDto = new LinhaDTO { NumeroLinha = 99 };

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() =>
                _servico.RemoverLinhaDoArquivo(idImportacao, linhaDto));

            Assert.Equal(Constantes.A_LINHA_INFORMADA_NAO_EXISTE_NO_ARQUIVO, excecao.Message);
        }

        [Fact]
        public async Task RemoverLinhaDoArquivo_DadoQueArquivoNaoExiste_QuandoExecutado_EntaoDeveLancarNegocioException()
        {
            // Arrange

            var linhaDto = new LinhaDTO { NumeroLinha = 1 };

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() =>
                _servico.RemoverLinhaDoArquivo(1, linhaDto));

            Assert.Equal(Constantes.ARQUIVO_NAO_ENCONTRADO, excecao.Message);
        }

        [Fact]
        public async Task RemoverLinhaDoArquivo_DadoQueArquivoTemConteudoInvalido_QuandoExecutado_EntaoDeveLancarNegocioException()
        {
            // Arrange
            var arquivo = new ImportacaoArquivo
            {
                Id = 1,
                TipoAcervo = TipoAcervo.Fotografico
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterPorId(1))
                .ReturnsAsync(arquivo);

            var linhaDto = new LinhaDTO { NumeroLinha = 1 };

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() =>
                _servico.RemoverLinhaDoArquivo(1, linhaDto));

            Assert.Equal(Constantes.CONTEUDO_DO_ARQUIVO_INVALIDO, excecao.Message);
        }

        [Fact]
        public async Task RemoverLinhaDoArquivo_DadoQueTipoAcervoEstaIncorreto_QuandoExecutado_EntaoDeveLancarNegocioException()
        {
            // Arrange
            var arquivo = new ImportacaoArquivo
            {
                Id = 1,
                TipoAcervo = TipoAcervo.Bibliografico, // Tipo errado (Esperado: Fotografico)
                Conteudo = "[]"
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterPorId(1))
                .ReturnsAsync(arquivo);

            var linhaDto = new LinhaDTO { NumeroLinha = 1 };

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() =>
                _servico.RemoverLinhaDoArquivo(1, linhaDto));

            Assert.Contains("Fotográfico", excecao.Message);
        }

        [Fact]
        public async Task AtualizarLinhaParaSucesso_DadoQueLinhaEhCorrigidaENaoHaOutrosErros_QuandoExecutado_EntaoDeveSalvarComStatusSucesso()
        {
            // Arrange
            long idImportacao = 10;
            int linhaAlvo = 1;

            // Prepara linha com erro
            var linhaComErro = GerarLinhaFotograficaFake(linhaAlvo);
            linhaComErro.PossuiErros = true;
            linhaComErro.Status = ImportacaoStatus.Erros;
            linhaComErro.Titulo.PossuiErro = true; // Simula um campo com erro específico

            var linhas = new List<AcervoFotograficoLinhaDTO> { linhaComErro };
            var conteudoJson = JsonConvert.SerializeObject(linhas);

            var arquivo = new ImportacaoArquivo
            {
                Id = idImportacao,
                TipoAcervo = TipoAcervo.Fotografico,
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
                    imp.Status == ImportacaoStatus.Sucesso && // Status virou Sucesso pois era a única linha
                    imp.Conteudo.Contains("\"PossuiErros\":false") && // Flag de erro foi removida
                    imp.Conteudo.Contains($"\"NumeroLinha\":{linhaAlvo}")
                )), Times.Once);
        }

        [Fact]
        public async Task AtualizarLinhaParaSucesso_DadoQueLinhaEhCorrigidaMasExistemOutrosErros_QuandoExecutado_EntaoDeveManterStatusErros()
        {
            // Arrange
            long idImportacao = 20;
            int linhaAlvo = 1;
            int linhaComOutroErro = 2;

            // Linha 1: Com erro, será corrigida
            var linha1 = GerarLinhaFotograficaFake(linhaAlvo);
            linha1.PossuiErros = true;

            // Linha 2: Com erro, permanecerá com erro
            var linha2 = GerarLinhaFotograficaFake(linhaComOutroErro);
            linha2.PossuiErros = true;

            var linhas = new List<AcervoFotograficoLinhaDTO> { linha1, linha2 };
            var conteudoJson = JsonConvert.SerializeObject(linhas);

            var arquivo = new ImportacaoArquivo
            {
                Id = idImportacao,
                TipoAcervo = TipoAcervo.Fotografico,
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
                    imp.Status == ImportacaoStatus.Erros && // Status continua Erros por causa da linha 2
                    imp.Conteudo.Contains($"\"NumeroLinha\":{linhaAlvo}") && // Linha 1 existe
                    imp.Conteudo.Contains($"\"NumeroLinha\":{linhaComOutroErro}") // Linha 2 existe
                )), Times.Once);
        }
        [Fact]
        public async Task ObterImportacaoPendente_DadoQueNaoExisteImportacaoPendente_QuandoExecutado_EntaoDeveRetornarNulo()
        {
            // Arrange

            // Act
            var resultado = await _servico.ObterImportacaoPendente();

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task ObterImportacaoPendente_DadoQueExisteImportacao_QuandoExecutado_EntaoDeveRetornarDTOComSucessoEErrosSeparados()
        {
            // Arrange
            var linhaSucesso = GerarLinhaFotograficaFake(1);
            var linhaErro = GerarLinhaFotograficaFake(2);
            linhaErro.PossuiErros = true;
            linhaErro.Titulo.PossuiErro = true;
            linhaErro.Titulo.Mensagem = "Erro no Título";

            var linhas = new List<AcervoFotograficoLinhaDTO> { linhaSucesso, linhaErro };
            var conteudoJson = JsonConvert.SerializeObject(linhas);

            var importacaoArquivo = new ImportacaoArquivo
            {
                Id = 1,
                Nome = "fotos.xlsx",
                TipoAcervo = TipoAcervo.Fotografico,
                Status = ImportacaoStatus.Pendente,
                Conteudo = conteudoJson,
                CriadoEm = DateTime.Now
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterUltimaImportacao(TipoAcervo.Fotografico))
                .ReturnsAsync(importacaoArquivo);

            ConfigurarMocksDaBase();

            // Act
            var resultado = await _servico.ObterImportacaoPendente();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(importacaoArquivo.Id, resultado.Id);

            // Valida linha de sucesso
            Assert.Single(resultado.Sucesso);
            Assert.Equal(1, resultado.Sucesso.First().NumeroLinha);

            // Valida linha de erro
            Assert.Single(resultado.Erros);
            Assert.Equal(2, resultado.Erros.First().NumeroLinha);
            Assert.Contains("Erro no Título", resultado.Erros.First().RetornoErro.ErrosCampos);
        }

        [Fact]
        public async Task ObterImportacaoPorId_DadoQueImportacaoNaoExiste_QuandoExecutado_EntaoDeveRetornarNulo()
        {
            // Arrange
            long id = 99;

            // Act
            var resultado = await _servico.ObterImportacaoPorId(id);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task ObterImportacaoPendente_DadoQueLinhaPossuiTodosOsErrosPossiveis_QuandoExecutado_EntaoDeveMapearTodasAsMensagensDeErro()
        {
            // Arrange
            // Cria uma linha com TODOS os campos com erro para testar todos os IFs do ObterMensagemErroLinha
            var linhaCaos = new AcervoFotograficoLinhaDTO
            {
                NumeroLinha = 1,
                PossuiErros = true,
                Titulo = CriarCampoComErro("Erro Título"),
                Codigo = CriarCampoComErro("Erro Código"),
                Credito = CriarCampoComErro("Erro Crédito"),
                Localizacao = CriarCampoComErro("Erro Localização"),
                Procedencia = CriarCampoComErro("Erro Procedência"),
                Ano = CriarCampoComErro("Erro Ano"),
                Data = CriarCampoComErro("Erro Data"),
                CopiaDigital = CriarCampoComErro("Erro Cópia Digital"),
                PermiteUsoImagem = CriarCampoComErro("Erro Uso Imagem"),
                EstadoConservacao = CriarCampoComErro("Erro Conservação"),
                Descricao = CriarCampoComErro("Erro Descrição"),
                Suporte = CriarCampoComErro("Erro Suporte"),
                Quantidade = CriarCampoComErro("Erro Quantidade"),
                Cromia = CriarCampoComErro("Erro Cromia"),
                TamanhoArquivo = CriarCampoComErro("Erro Tamanho Arquivo"),
                Largura = CriarCampoComErro("Erro Largura"),
                Altura = CriarCampoComErro("Erro Altura"),
                FormatoImagem = CriarCampoComErro("Erro Formato"),
                Resolucao = CriarCampoComErro("Erro Resolução")
            };

            var conteudoJson = JsonConvert.SerializeObject(new List<AcervoFotograficoLinhaDTO> { linhaCaos });
            var importacaoArquivo = new ImportacaoArquivo
            {
                Id = 1,
                TipoAcervo = TipoAcervo.Fotografico,
                Conteudo = conteudoJson
            };

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.ObterUltimaImportacao(TipoAcervo.Fotografico))
                .ReturnsAsync(importacaoArquivo);

            ConfigurarMocksDaBase();

            // Act
            var resultado = await _servico.ObterImportacaoPendente();

            // Assert
            Assert.NotNull(resultado);
            var linhaErro = resultado.Erros.First();
            var mensagens = linhaErro.RetornoErro.ErrosCampos;

            // Verifica se as 19 mensagens foram capturadas
            Assert.Equal(19, mensagens.Length);

            // Verificação amostral para garantir ordem e presença
            Assert.Contains("Erro Título", mensagens);
            Assert.Contains("Erro Código", mensagens);
            Assert.Contains("Erro Resolução", mensagens);
        }
        [Fact]
        public async Task ImportarArquivo_DadoQueArquivoEhValido_QuandoExecutado_EntaoDeveSalvarEPublicarNaFila()
        {
            // Arrange
            var arquivoExcel = GerarArquivoExcelFotograficoValido();
            long idGerado = 123;

            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Setup(x => x.Salvar(It.IsAny<ImportacaoArquivo>()))
                .ReturnsAsync(idGerado);

            // Act
            var resultado = await _servico.ImportarArquivo(arquivoExcel);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idGerado, resultado.Id);
            Assert.Equal(Constantes.PLANILHA_ACERVO_FOTOGRAFICO, resultado.Nome); // O mock gera com esse nome
            Assert.Equal(ImportacaoStatus.Pendente, resultado.Status);
            Assert.Equal(TipoAcervo.Fotografico, resultado.TipoAcervo);

            // 1. Verifica se salvou com o conteúdo correto (checamos se leu o Título da planilha)
            _mocker.GetMock<IRepositorioImportacaoArquivo>()
                .Verify(x => x.Salvar(It.Is<ImportacaoArquivo>(imp =>
                    imp.Nome == Constantes.PLANILHA_ACERVO_FOTOGRAFICO &&
                    imp.TipoAcervo == TipoAcervo.Fotografico &&
                    imp.Status == ImportacaoStatus.Pendente &&
                    imp.Conteudo.Contains("Foto Histórica") // Valor inserido no Helper
                )), Times.Once);

            // 2. Verifica se publicou na fila correta
            _mocker.GetMock<IServicoMensageria>()
                .Verify(x => x.Publicar(
                    RotasRabbit.ExecutarImportacaoArquivoAcervoFotografico,
                    idGerado,
                    It.IsAny<Guid>()
                ), Times.Once);
        }

        [Fact]
        public async Task ImportarArquivo_DadoQueArquivoEhNulo_QuandoExecutado_EntaoDeveLancarNegocioException()
        {
            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _servico.ImportarArquivo(null));
            // A mensagem vem da classe base, validamos se o tipo da exceção está correto
            Assert.NotNull(excecao);
        }

        [Fact]
        public async Task ImportarArquivo_DadoQuePlanilhaTemCabecalhoInvalido_QuandoExecutado_EntaoDeveLancarNegocioException()
        {
            // Arrange
            // Gera arquivo com nome da coluna TITULO errado
            var arquivoExcel = GerarArquivoExcelFotograficoValido(alterarCabecalho: true);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.ImportarArquivo(arquivoExcel));
        }

        [Fact]
        public async Task ImportarArquivo_DadoQuePlanilhaEstaVaziaSemDados_QuandoExecutado_EntaoDeveLancarNegocioException()
        {
            // Arrange
            // Gera arquivo com 0 linhas de dados
            var arquivoExcel = GerarArquivoExcelFotograficoValido(quantidadeLinhas: 0);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.ImportarArquivo(arquivoExcel));
        }

        [Fact]
        public async Task ImportarArquivo_DadoQuePlanilhaExcedeLimiteConfigurado_QuandoExecutado_EntaoDeveLancarNegocioException()
        {
            // Arrange
            // Configura o parametro de sistema para permitir apenas 1 linha
            _mocker.GetMock<IRepositorioParametroSistema>()
               .Setup(x => x.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
               .ReturnsAsync(new ParametroSistema { Valor = "1" });

            // Gera arquivo com 2 linhas de dados
            var arquivoExcel = GerarArquivoExcelFotograficoValido(quantidadeLinhas: 2);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.ImportarArquivo(arquivoExcel));
        }

        #region Helpers
        private IFormFile GerarArquivoExcelFotograficoValido(bool alterarCabecalho = false, int quantidadeLinhas = 1)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Planilha1");

            // 1. Configurar Cabeçalhos (Baseado na ordem chamada em LerPlanilha e ValidarOrdemColunas)
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_TITULO, alterarCabecalho ? "TITULO_ERRADO" : Constantes.NOME_DA_COLUNA_TITULO);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_CODIGO, Constantes.NOME_DA_COLUNA_TOMBO); // Nota: Código mapeia para TOMBO nas constantes do validador
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_CREDITO, Constantes.NOME_DA_COLUNA_CREDITO);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_LOCALIZACAO, Constantes.NOME_DA_COLUNA_LOCALIZACAO);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_PROCEDENCIA, Constantes.NOME_DA_COLUNA_PROCEDENCIA);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_ANO, Constantes.NOME_DA_COLUNA_ANO);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_DATA, Constantes.NOME_DA_COLUNA_DATA);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_COPIA_DIGITAL, Constantes.NOME_DA_COLUNA_COPIA_DIGITAL);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_AUTORIZACAO_USO_DE_IMAGEM, Constantes.NOME_DA_COLUNA_AUTORIZACAO_USO_DE_IMAGEM);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_ESTADO_CONSERVACAO, Constantes.NOME_DA_COLUNA_ESTADO_DE_CONSERVACAO);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_DESCRICAO, Constantes.NOME_DA_COLUNA_DESCRICAO);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_QUANTIDADE, Constantes.NOME_DA_COLUNA_QUANTIDADE);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_LARGURA, Constantes.NOME_DA_COLUNA_DIMENSAO_LARGURA);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_ALTURA, Constantes.NOME_DA_COLUNA_DIMENSAO_ALTURA);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_SUPORTE, Constantes.NOME_DA_COLUNA_SUPORTE);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_FORMATO_IMAGEM, Constantes.NOME_DA_COLUNA_FORMATO_DA_IMAGEM);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_TAMANHO_ARQUIVO, Constantes.NOME_DA_COLUNA_TAMANHO_DO_ARQUIVO);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_CROMIA, Constantes.NOME_DA_COLUNA_CROMIA);
            AdicionarCabecalho(worksheet, Constantes.ACERVO_FOTOGRAFICO_CAMPO_RESOLUCAO, Constantes.NOME_DA_COLUNA_RESOLUCAO);

            // 2. Adicionar Dados
            for (int i = 0; i < quantidadeLinhas; i++)
            {
                int linha = Constantes.INICIO_LINHA_DADOS + i;
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_TITULO).Value = "Foto Histórica";
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_CODIGO).Value = "FT001";
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_CREDITO).Value = "Fotografo X";
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_LOCALIZACAO).Value = "Gaveta 1";
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_PROCEDENCIA).Value = "Doação";
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_ANO).Value = "1990";
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_DATA).Value = "01/01/1990";
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_COPIA_DIGITAL).Value = "Sim";
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_AUTORIZACAO_USO_DE_IMAGEM).Value = "Não";
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_ESTADO_CONSERVACAO).Value = "Bom";
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_DESCRICAO).Value = "Foto de teste";
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_QUANTIDADE).Value = "1";
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_LARGURA).Value = "10,00"; // Formato esperado pelo regex (com vírgula)
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_ALTURA).Value = "15,00";
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_SUPORTE).Value = "Papel";
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_FORMATO_IMAGEM).Value = "JPEG";
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_TAMANHO_ARQUIVO).Value = "2MB";
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_CROMIA).Value = "Colorido";
                worksheet.Cell(linha, Constantes.ACERVO_FOTOGRAFICO_CAMPO_RESOLUCAO).Value = "300dpi";
            }

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.OpenReadStream()).Returns(stream);
            fileMock.Setup(_ => _.FileName).Returns(Constantes.PLANILHA_ACERVO_FOTOGRAFICO);
            fileMock.Setup(_ => _.Length).Returns(stream.Length);
            fileMock.Setup(_ => _.ContentType).Returns(Constantes.CONTENT_TYPE_EXCEL);

            return fileMock.Object;
        }

        private void AdicionarCabecalho(IXLWorksheet worksheet, int coluna, string valor)
        {
            worksheet.Cell(Constantes.INICIO_LINHA_TITULO, coluna).Value = valor;
        }

        private AcervoFotograficoLinhaDTO GerarLinhaFotograficaFake(int numeroLinha)
        {
            return new AcervoFotograficoLinhaDTO
            {
                NumeroLinha = numeroLinha,
                PossuiErros = false,
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = _faker.Lorem.Sentence() },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = _faker.Random.AlphaNumeric(10) },
                // Inicializar demais objetos para evitar NullReference caso o código acesse propriedades aninhadas
                Credito = new LinhaConteudoAjustarDTO(),
                Localizacao = new LinhaConteudoAjustarDTO(),
                Procedencia = new LinhaConteudoAjustarDTO(),
                Data = new LinhaConteudoAjustarDTO(),
                CopiaDigital = new LinhaConteudoAjustarDTO(),
                PermiteUsoImagem = new LinhaConteudoAjustarDTO(),
                EstadoConservacao = new LinhaConteudoAjustarDTO(),
                Descricao = new LinhaConteudoAjustarDTO(),
                Quantidade = new LinhaConteudoAjustarDTO(),
                Largura = new LinhaConteudoAjustarDTO(),
                Altura = new LinhaConteudoAjustarDTO(),
                Suporte = new LinhaConteudoAjustarDTO(),
                FormatoImagem = new LinhaConteudoAjustarDTO(),
                TamanhoArquivo = new LinhaConteudoAjustarDTO(),
                Cromia = new LinhaConteudoAjustarDTO(),
                Resolucao = new LinhaConteudoAjustarDTO(),
                Ano = new LinhaConteudoAjustarDTO()
            };
        }

        private void ConfigurarMocksDaBase()
        {
            // Retorna listas vazias para evitar NullReferenceException nos métodos da classe base que carregam domínios
            _mocker.GetMock<IServicoCreditoAutor>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoFormato>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoSuporte>().Setup(x => x.ObterTodos()).ReturnsAsync([]);

            // Outros mocks que podem ser chamados pelo CarregarTodosOsDominios
            _mocker.GetMock<IServicoMaterial>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoEditora>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoSerieColecao>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoIdioma>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoAssunto>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoConservacao>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoAcessoDocumento>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
            _mocker.GetMock<IServicoCromia>().Setup(x => x.ObterTodos()).ReturnsAsync([]);
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
