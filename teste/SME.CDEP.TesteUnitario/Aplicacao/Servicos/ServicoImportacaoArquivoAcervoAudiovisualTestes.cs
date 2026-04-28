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
    public class ServicoImportacaoArquivoAcervoAudiovisualTestes
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
        private readonly ServicoImportacaoArquivoAcervoAudiovisual sut;

        public ServicoImportacaoArquivoAcervoAudiovisualTestes()
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

            sut = mocker.CreateInstance<ServicoImportacaoArquivoAcervoAudiovisual>();

            // Configuração padrão comum a vários testes
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
            Action acao = () => new ServicoImportacaoArquivoAcervoAudiovisual(
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
            Action acao = () => new ServicoImportacaoArquivoAcervoAudiovisual(
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
                .Setup(r => r.ObterUltimaImportacao(TipoAcervo.Audiovisual))
                .ReturnsAsync((ImportacaoArquivo)null!);

            var resultado = await sut.ObterImportacaoPendente();

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DadoImportacaoPendenteExistente_QuandoObterImportacaoPendente_EntaoRetornaDtoMapeado()
        {
            var arquivoImportadoMock = GerarArquivoImportadoMock(99, ImportacaoStatus.Pendente);
            repositorioImportacaoArquivoMock
                .Setup(r => r.ObterUltimaImportacao(TipoAcervo.Audiovisual))
                .ReturnsAsync(arquivoImportadoMock);

            var resultado = await sut.ObterImportacaoPendente();

            resultado.Should().NotBeNull();
            resultado.Sucesso.Should().HaveCount(1);
            resultado.Erros.Should().HaveCount(1);
            resultado.Erros.First().RetornoErro.ErrosCampos.Should().Contain("Título obrigatório");
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
            var linhaMock = ObterLinhaAudiovisualDTO(1, true);
            var arquivoMock = new ImportacaoArquivo { Id = idImportacao, TipoAcervo = TipoAcervo.Audiovisual, Conteudo = JsonConvert.SerializeObject(new[] { linhaMock }) };

            repositorioImportacaoArquivoMock.Setup(r => r.ObterPorId(idImportacao)).ReturnsAsync(arquivoMock);

            var resultado = await sut.AtualizarLinhaParaSucesso(idImportacao, new LinhaDTO { NumeroLinha = 1 });

            resultado.Should().BeTrue();
            repositorioImportacaoArquivoMock.Verify(r => r.Salvar(It.Is<ImportacaoArquivo>(a => a.Status == ImportacaoStatus.Sucesso)), Times.Once);
        }

        [Fact]
        public async Task DadoLinhasRestantesComErro_QuandoAtualizarLinhaParaSucesso_EntaoStatusPermaneceErros()
        {
            var idImportacao = 1L;
            var arquivoMock = GerarArquivoImportadoMock(idImportacao, ImportacaoStatus.Erros); // Possui linha 1 sucesso, linha 2 erro

            repositorioImportacaoArquivoMock.Setup(r => r.ObterPorId(idImportacao)).ReturnsAsync(arquivoMock);

            var resultado = await sut.AtualizarLinhaParaSucesso(idImportacao, new LinhaDTO { NumeroLinha = 2 });

            resultado.Should().BeTrue();
            repositorioImportacaoArquivoMock.Verify(r => r.Salvar(It.Is<ImportacaoArquivo>(a => a.Status == ImportacaoStatus.Sucesso)), Times.Once);
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
                p.Cell(2, 1).Value = "Dado";
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
                p.Cell(2, 1).Value = "Vídeo Institucional";
                p.Cell(2, 2).Value = "TMB-111";
            });
            var fileMock = ConfigurarIFormFileMock(stream);

            repositorioImportacaoArquivoMock.Setup(r => r.Salvar(It.IsAny<ImportacaoArquivo>())).ReturnsAsync(88L);

            var resultado = await sut.ImportarArquivo(fileMock.Object);

            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(88L);
            servicoMensageriaMock.Verify(s => s.Publicar(RotasRabbit.ExecutarImportacaoArquivoAcervoAudiovisual, 88L, It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DadoObterImportacao_QuandoExecutar_EntaoDeveCarregarDominiosNecessarios()
        {
            var arquivoMock = GerarArquivoImportadoMock(1L, ImportacaoStatus.Pendente);
            repositorioImportacaoArquivoMock.Setup(r => r.ObterImportacaoPorId(1L)).ReturnsAsync(arquivoMock);

            await sut.ObterImportacaoPorId(1L);

            servicoSuporteMock.Verify(s => s.ObterTodos(), Times.Once);
            servicoCreditoAutorMock.Verify(s => s.ObterTodos(), Times.Once);
        }

        [Fact]
        public async Task DadoLinhaSucesso_QuandoMapearRetorno_EntaoAdicionaSufixoAoCodigo()
        {
            var arquivoMock = GerarArquivoImportadoMock(1L, ImportacaoStatus.Pendente); // Linha 1 é sucesso, TMB-001
            repositorioImportacaoArquivoMock.Setup(r => r.ObterImportacaoPorId(1L)).ReturnsAsync(arquivoMock);

            var resultado = await sut.ObterImportacaoPorId(1L);

            var sucesso = resultado.Sucesso.First();
            sucesso.Tombo.Should().Be($"TMB-001{Constantes.SIGLA_ACERVO_AUDIOVISUAL}");
        }

        [Fact]
        public async Task DadoLinhaComErro_QuandoMapearRetorno_EntaoCompilaMensagensDeErroDaLinha()
        {
            var arquivoMock = GerarArquivoImportadoMock(1L, ImportacaoStatus.Erros); // Linha 2 tem erro
            repositorioImportacaoArquivoMock.Setup(r => r.ObterImportacaoPorId(1L)).ReturnsAsync(arquivoMock);

            var resultado = await sut.ObterImportacaoPorId(1L);

            var erro = resultado.Erros.First();
            erro.RetornoErro.ErrosCampos.Should().Contain("Título obrigatório");
        }

        [Fact]
        public async Task DadoDadosPreenchidos_QuandoMapearRetorno_EntaoCriaDtoConvertendoSimNaoEDominios()
        {
            var linhaMock = ObterLinhaAudiovisualDTO(3, true);
            linhaMock.PermiteUsoImagem.Conteudo = "Sim";
            linhaMock.Suporte.Conteudo = "VHS";
            linhaMock.Credito.Conteudo = "Diretor X";

            var arquivoMock = new ImportacaoArquivo { Id = 1, TipoAcervo = TipoAcervo.Audiovisual, Conteudo = JsonConvert.SerializeObject(new[] { linhaMock }) };
            repositorioImportacaoArquivoMock.Setup(r => r.ObterImportacaoPorId(1L)).ReturnsAsync(arquivoMock);

            mapperMock.Setup(m => m.Map<IdNomeTipoDTO>(It.IsAny<object>())).Returns((object src) => { var s = src as dynamic; return new IdNomeTipoDTO { Id = s.Id, Nome = s.Nome, Tipo = s.Tipo }; });
            servicoSuporteMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoDTO> { new IdNomeTipoExcluidoDTO { Id = 10, Nome = "VHS", Tipo = (int)TipoSuporte.VIDEO } });
            servicoCreditoAutorMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoAuditavelDTO> { new IdNomeTipoExcluidoAuditavelDTO { Id = 20, Nome = "Diretor X", Tipo = (int)TipoCreditoAutoria.Credito } });

            var resultado = await sut.ObterImportacaoPorId(1L);

            var obj = resultado.Erros.First().RetornoObjeto;
            obj.PermiteUsoImagem.Should().BeTrue();
            obj.SuporteId.Should().Be(10);
            obj.CreditosAutoresIds.Should().Contain(20);
        }

        // Helpers Privados
        private ImportacaoArquivo GerarArquivoImportadoMock(long id, ImportacaoStatus status)
        {
            var linhas = new List<AcervoAudiovisualLinhaDTO>
            {
                ObterLinhaAudiovisualDTO(1, false),
                ObterLinhaAudiovisualDTO(2, true)
            };

            return new ImportacaoArquivo
            {
                Id = id,
                Nome = "planilha.xlsx",
                TipoAcervo = TipoAcervo.Audiovisual,
                Status = status,
                Conteudo = JsonConvert.SerializeObject(linhas),
                CriadoEm = DateTime.Now
            };
        }

        private static AcervoAudiovisualLinhaDTO ObterLinhaAudiovisualDTO(int numero, bool comErro)
        {
            return new AcervoAudiovisualLinhaDTO
            {
                NumeroLinha = numero,
                PossuiErros = comErro,
                Mensagem = comErro ? "Erro validação" : "",
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = $"Video {numero}", PossuiErro = comErro, Mensagem = comErro ? "Título obrigatório" : "" },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = $"TMB-00{numero}" },
                Credito = new LinhaConteudoAjustarDTO(),
                Localizacao = new LinhaConteudoAjustarDTO(),
                Procedencia = new LinhaConteudoAjustarDTO(),
                Copia = new LinhaConteudoAjustarDTO(),
                PermiteUsoImagem = new LinhaConteudoAjustarDTO(),
                EstadoConservacao = new LinhaConteudoAjustarDTO(),
                Descricao = new LinhaConteudoAjustarDTO(),
                Suporte = new LinhaConteudoAjustarDTO(),
                Duracao = new LinhaConteudoAjustarDTO(),
                Cromia = new LinhaConteudoAjustarDTO(),
                TamanhoArquivo = new LinhaConteudoAjustarDTO(),
                Acessibilidade = new LinhaConteudoAjustarDTO(),
                Disponibilizacao = new LinhaConteudoAjustarDTO(),
                Ano = new LinhaConteudoAjustarDTO()
            };
        }

        private static Mock<IFormFile> ConfigurarIFormFileMock(MemoryStream stream)
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(100);
            fileMock.Setup(f => f.FileName).Returns("arquivo.xlsx");
            fileMock.Setup(f => f.ContentType).Returns(Constantes.CONTENT_TYPE_EXCEL);
            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            return fileMock;
        }

        private static MemoryStream CriarStreamExcelPlanilhaMock(Action<IXLWorksheet> manipulacaoPlanilha)
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

        private static void GerarCabecalhoPlanilha(IXLWorksheet planilha)
        {
            planilha.Cell(1, 1).Value = Constantes.NOME_DA_COLUNA_TITULO;
            planilha.Cell(1, 2).Value = Constantes.NOME_DA_COLUNA_TOMBO;
            planilha.Cell(1, 3).Value = Constantes.NOME_DA_COLUNA_CREDITO;
            planilha.Cell(1, 4).Value = Constantes.NOME_DA_COLUNA_LOCALIZACAO;
            planilha.Cell(1, 5).Value = Constantes.NOME_DA_COLUNA_PROCEDENCIA;
            planilha.Cell(1, 6).Value = Constantes.NOME_DA_COLUNA_ANO;
            planilha.Cell(1, 7).Value = Constantes.NOME_DA_COLUNA_COPIA;
            planilha.Cell(1, 8).Value = Constantes.NOME_DA_COLUNA_AUTORIZACAO_USO_DE_IMAGEM;
            planilha.Cell(1, 9).Value = Constantes.NOME_DA_COLUNA_ESTADO_DE_CONSERVACAO;
            planilha.Cell(1, 10).Value = Constantes.NOME_DA_COLUNA_DESCRICAO;
            planilha.Cell(1, 11).Value = Constantes.NOME_DA_COLUNA_SUPORTE;
            planilha.Cell(1, 12).Value = Constantes.NOME_DA_COLUNA_DURACAO;
            planilha.Cell(1, 13).Value = Constantes.NOME_DA_COLUNA_CROMIA;
            planilha.Cell(1, 14).Value = Constantes.NOME_DA_COLUNA_TAMANHO_DO_ARQUIVO;
            planilha.Cell(1, 15).Value = Constantes.NOME_DA_COLUNA_ACESSIBILIDADE;
            planilha.Cell(1, 16).Value = Constantes.NOME_DA_COLUNA_DISPONIBILIZACAO;
        }
    }
}
