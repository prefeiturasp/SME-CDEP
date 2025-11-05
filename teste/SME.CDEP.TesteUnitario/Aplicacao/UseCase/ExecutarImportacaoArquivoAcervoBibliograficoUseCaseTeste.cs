using AutoMapper;
using Moq;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.Infra.Servicos.Rabbit.Dto;
using System.Reflection;

namespace SME.CDEP.TesteUnitario.Aplicacao.UseCase
{
    public class ExecutarImportacaoArquivoAcervoBibliograficoUseCaseTeste
    {
        private readonly Mock<IRepositorioImportacaoArquivo> _repositorioImportacaoArquivoMock;
        private readonly Mock<IServicoAcervoBibliografico> _servicoAcervoBibliograficoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IServicoMaterial> _servicoMaterialMock;
        private readonly Mock<IServicoEditora> _servicoEditoraMock;
        private readonly Mock<IServicoSerieColecao> _servicoSerieColecaoMock;
        private readonly Mock<IServicoIdioma> _servicoIdiomaMock;
        private readonly Mock<IServicoAssunto> _servicoAssuntoMock;
        private readonly Mock<IServicoCreditoAutor> _servicoCreditoAutorMock;
        private readonly Mock<IServicoConservacao> _servicoConservacaoMock;
        private readonly Mock<IServicoAcessoDocumento> _servicoAcessoDocumentoMock;
        private readonly Mock<IServicoCromia> _servicoCromiaMock;
        private readonly Mock<IServicoSuporte> _servicoSuporteMock;
        private readonly Mock<IServicoFormato> _servicoFormatoMock;
        private readonly Mock<IRepositorioParametroSistema> _repositorioParametroSistemaMock;

        private readonly ExecutarImportacaoArquivoAcervoBibliograficoUseCase _useCase;

        public ExecutarImportacaoArquivoAcervoBibliograficoUseCaseTeste()
        {
            _repositorioImportacaoArquivoMock = new Mock<IRepositorioImportacaoArquivo>();
            _servicoAcervoBibliograficoMock = new Mock<IServicoAcervoBibliografico>();
            _mapperMock = new Mock<IMapper>();
            _servicoMaterialMock = new Mock<IServicoMaterial>();
            _servicoEditoraMock = new Mock<IServicoEditora>();
            _servicoSerieColecaoMock = new Mock<IServicoSerieColecao>();
            _servicoIdiomaMock = new Mock<IServicoIdioma>();
            _servicoAssuntoMock = new Mock<IServicoAssunto>();
            _servicoCreditoAutorMock = new Mock<IServicoCreditoAutor>();
            _servicoConservacaoMock = new Mock<IServicoConservacao>();
            _servicoAcessoDocumentoMock = new Mock<IServicoAcessoDocumento>();
            _servicoCromiaMock = new Mock<IServicoCromia>();
            _servicoSuporteMock = new Mock<IServicoSuporte>();
            _servicoFormatoMock = new Mock<IServicoFormato>();
            _repositorioParametroSistemaMock = new Mock<IRepositorioParametroSistema>();

            _mapperMock.Setup(x => x.Map<IdNomeDTO>(It.IsAny<object>()))
                .Returns<object>(source => new IdNomeDTO { Id = 1, Nome = "Test" });
            _mapperMock.Setup(x => x.Map<IdNomeTipoDTO>(It.IsAny<object>()))
                .Returns<object>(source => new IdNomeTipoDTO { Id = 1, Nome = "Test", Tipo = 1 });

            _useCase = new ExecutarImportacaoArquivoAcervoBibliograficoUseCase(
                _repositorioImportacaoArquivoMock.Object,
                _servicoMaterialMock.Object,
                _servicoEditoraMock.Object,
                _servicoSerieColecaoMock.Object,
                _servicoIdiomaMock.Object,
                _servicoAssuntoMock.Object,
                _servicoCreditoAutorMock.Object,
                _servicoConservacaoMock.Object,
                _servicoAcessoDocumentoMock.Object,
                _servicoCromiaMock.Object,
                _servicoSuporteMock.Object,
                _servicoFormatoMock.Object,
                _servicoAcervoBibliograficoMock.Object,
                _mapperMock.Object,
                _repositorioParametroSistemaMock.Object
            );
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Quando_Servico_Acervo_Bibliografico_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ExecutarImportacaoArquivoAcervoBibliograficoUseCase(
                _repositorioImportacaoArquivoMock.Object,
                _servicoMaterialMock.Object,
                _servicoEditoraMock.Object,
                _servicoSerieColecaoMock.Object,
                _servicoIdiomaMock.Object,
                _servicoAssuntoMock.Object,
                _servicoCreditoAutorMock.Object,
                _servicoConservacaoMock.Object,
                _servicoAcessoDocumentoMock.Object,
                _servicoCromiaMock.Object,
                _servicoSuporteMock.Object,
                _servicoFormatoMock.Object,
                null,
                _mapperMock.Object,
                _repositorioParametroSistemaMock.Object
            ));
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Quando_Mapper_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ExecutarImportacaoArquivoAcervoBibliograficoUseCase(
                _repositorioImportacaoArquivoMock.Object,
                _servicoMaterialMock.Object,
                _servicoEditoraMock.Object,
                _servicoSerieColecaoMock.Object,
                _servicoIdiomaMock.Object,
                _servicoAssuntoMock.Object,
                _servicoCreditoAutorMock.Object,
                _servicoConservacaoMock.Object,
                _servicoAcessoDocumentoMock.Object,
                _servicoCromiaMock.Object,
                _servicoSuporteMock.Object,
                _servicoFormatoMock.Object,
                _servicoAcervoBibliograficoMock.Object,
                null,
                _repositorioParametroSistemaMock.Object
            ));
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Negocio_Exception_Quando_Mensagem_For_Nula()
        {
            var param = new MensagemRabbit { Mensagem = null };

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(param));
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Negocio_Exception_Quando_Mensagem_Nao_For_Um_Long_Valido()
        {
            var param = new MensagemRabbit { Mensagem = "nao-eh-um-long" };

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(param));
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Negocio_Exception_Quando_Importacao_Arquivo_Nao_For_Localizada()
        {
            var param = new MensagemRabbit { Mensagem = "1" };
            _repositorioImportacaoArquivoMock.Setup(r => r.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync((Dominio.Entidades.ImportacaoArquivo)null);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(param));
        }

        [Fact]
        public void Obter_CoAutores_Tipo_Autoria_Deve_Retornar_Nulo_Quando_CoAutores_Nao_Preenchidos()
        {
            var coautores = "";
            var tiposAutoria = "tipo1";

            var resultado = _useCase.ObterCoAutoresTipoAutoria(coautores, tiposAutoria);

            Assert.Null(resultado);
        }
        
        [Fact]
        public async Task Validar_Preenchimento_Valor_Formato_Qtde_Caracteres_Deve_Marcar_Como_Erro_Quando_Titulo_Exceder_Limite()
        {
            var linha = new AcervoBibliograficoLinhaDTO
            {
                NumeroLinha = 1,
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = new string('a', 251), LimiteCaracteres = 250 },
                SubTitulo = new LinhaConteudoAjustarDTO { Conteudo = "Subtitulo", LimiteCaracteres = 250 },
                Material = new LinhaConteudoAjustarDTO { Conteudo = "MATERIAL_VALIDO", LimiteCaracteres = 50 },
                Autor = new LinhaConteudoAjustarDTO { Conteudo = "AUTOR_VALIDO", LimiteCaracteres = 250 },
                CoAutor = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 250 },
                TipoAutoria = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 250 },
                Editora = new LinhaConteudoAjustarDTO { Conteudo = "EDITORA_VALIDA", LimiteCaracteres = 250 },
                Assunto = new LinhaConteudoAjustarDTO { Conteudo = "ASSUNTO_VALIDO", LimiteCaracteres = 250 },
                Ano = new LinhaConteudoAjustarDTO { Conteudo = "2023", LimiteCaracteres = 4 },
                Edicao = new LinhaConteudoAjustarDTO { Conteudo = "1", LimiteCaracteres = 100 },
                NumeroPaginas = new LinhaConteudoAjustarDTO { Conteudo = "100", LimiteCaracteres = 10 },
                Largura = new LinhaConteudoAjustarDTO { Conteudo = "10", LimiteCaracteres = 10 },
                Altura = new LinhaConteudoAjustarDTO { Conteudo = "15", LimiteCaracteres = 10 },
                SerieColecao = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 250 },
                Volume = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 50 },
                Idioma = new LinhaConteudoAjustarDTO { Conteudo = "IDIOMA_VALIDO", LimiteCaracteres = 100 },
                LocalizacaoCDD = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 100 },
                LocalizacaoPHA = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 100 },
                NotasGerais = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 1000 },
                Isbn = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 17 },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 150 }
            };
            var linhas = new List<AcervoBibliograficoLinhaDTO> { linha };

            _servicoMaterialMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoDTO> { new IdNomeTipoExcluidoDTO { Id = 1, Nome = "MATERIAL_VALIDO", Tipo = (int)TipoMaterial.BIBLIOGRAFICO } });
            _servicoCreditoAutorMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoAuditavelDTO> {
                new IdNomeTipoExcluidoAuditavelDTO { Id = 1, Nome = "AUTOR_VALIDO", Tipo = (int)TipoCreditoAutoria.Autoria },
                new IdNomeTipoExcluidoAuditavelDTO { Id = 2, Nome = "COAUTOR_VALIDO", Tipo = (int)TipoCreditoAutoria.Coautor }
            });
            _servicoEditoraMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO> { new IdNomeExcluidoAuditavelDTO { Id = 1, Nome = "EDITORA_VALIDA" } });
            _servicoAssuntoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO> { new IdNomeExcluidoAuditavelDTO { Id = 1, Nome = "ASSUNTO_VALIDO" } });
            _servicoIdiomaMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoDTO> { new IdNomeExcluidoDTO { Id = 1, Nome = "IDIOMA_VALIDO" } });
            _repositorioParametroSistemaMock.Setup(r => r.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "100" });

            var metodoCarregarDominios = _useCase.GetType().GetMethod("CarregarTodosOsDominios", BindingFlags.NonPublic | BindingFlags.Instance);
            await (Task)metodoCarregarDominios.Invoke(_useCase, null);

            _useCase.ValidarPreenchimentoValorFormatoQtdeCaracteres(linhas);

            Assert.True(linha.Titulo.PossuiErro);
            Assert.True(linha.PossuiErros);
            Assert.Contains("O campo 'Título' atingiu o limite de caracteres", linha.Titulo.Mensagem);
        }

        [Fact]
        public async void Validar_Preenchimento_Valor_Formato_Qtde_Caracteres_Deve_Marcar_Como_Erro_Quando_Material_For_Invalido()
        {
            var linha = new AcervoBibliograficoLinhaDTO
            {
                NumeroLinha = 1,
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Título Válido", LimiteCaracteres = 250 },
                Material = new LinhaConteudoAjustarDTO { Conteudo = "MATERIAL_INVALIDO", LimiteCaracteres = 50 },
                Autor = new LinhaConteudoAjustarDTO { Conteudo = "Autor Válido", LimiteCaracteres = 250 },
                CoAutor = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 250 },
                TipoAutoria = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 250 },
                Editora = new LinhaConteudoAjustarDTO { Conteudo = "Editora Válida", LimiteCaracteres = 250 },
                Assunto = new LinhaConteudoAjustarDTO { Conteudo = "Assunto Válido", LimiteCaracteres = 250 },
                Ano = new LinhaConteudoAjustarDTO { Conteudo = "2023", LimiteCaracteres = 4 },
                Edicao = new LinhaConteudoAjustarDTO { Conteudo = "1", LimiteCaracteres = 100 },
                NumeroPaginas = new LinhaConteudoAjustarDTO { Conteudo = "100", LimiteCaracteres = 10 },
                Largura = new LinhaConteudoAjustarDTO { Conteudo = "10", LimiteCaracteres = 10 },
                Altura = new LinhaConteudoAjustarDTO { Conteudo = "15", LimiteCaracteres = 10 },
                SerieColecao = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 250 },
                Volume = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 50 },
                Idioma = new LinhaConteudoAjustarDTO { Conteudo = "IDIOMA_VALIDO", LimiteCaracteres = 100 },
                LocalizacaoCDD = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 100 },
                LocalizacaoPHA = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 100 },
                NotasGerais = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 1000 },
                Isbn = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 17 },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 150 },
                SubTitulo = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 250 }
            };
            var linhas = new List<AcervoBibliograficoLinhaDTO> { linha };

            _servicoMaterialMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoDTO> {  new IdNomeTipoExcluidoDTO { Id = 1, Nome = "MATERIAL_VALIDO", Tipo = (int)TipoMaterial.BIBLIOGRAFICO }});
            _servicoCreditoAutorMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoAuditavelDTO> {  new IdNomeTipoExcluidoAuditavelDTO { Id = 1, Nome = "Autor Válido", Tipo = (int)TipoCreditoAutoria.Autoria } });
            _servicoEditoraMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO> { new IdNomeExcluidoAuditavelDTO { Id = 1, Nome = "Editora Válida" } });
            _servicoAssuntoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO> { new IdNomeExcluidoAuditavelDTO { Id = 1, Nome = "Assunto Válido" }  });
            _servicoIdiomaMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoDTO> { new IdNomeExcluidoDTO { Id = 1, Nome = "IDIOMA_VALIDO" } });
            _repositorioParametroSistemaMock.Setup(r => r.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>())).ReturnsAsync(new ParametroSistema { Valor = "100" });

            var metodoCarregarDominios = _useCase.GetType().GetMethod("CarregarTodosOsDominios", BindingFlags.NonPublic | BindingFlags.Instance);
            await(Task)metodoCarregarDominios.Invoke(_useCase, null);

            _useCase.ValidarPreenchimentoValorFormatoQtdeCaracteres(linhas);

            Assert.True(linha.Material.PossuiErro);
            Assert.True(linha.PossuiErros);
        }

        [Fact]
        public async Task Validar_Preenchimento_Valor_Formato_Qtde_Caracteres_Deve_Marcar_Como_Erro_Quando_CoAutor_Nao_Estiver_Preenchido_E_Tipo_Autoria_Estiver()
        {
            var linha = new AcervoBibliograficoLinhaDTO
            {
                NumeroLinha = 1,
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Titulo Válido", LimiteCaracteres = 100 },
                Material = new LinhaConteudoAjustarDTO { Conteudo = "MATERIAL_VALIDO", LimiteCaracteres = 100 },
                Autor = new LinhaConteudoAjustarDTO { Conteudo = "Autor Válido", LimiteCaracteres = 100 },
                CoAutor = new LinhaConteudoAjustarDTO { Conteudo = "" },
                TipoAutoria = new LinhaConteudoAjustarDTO { Conteudo = "Algum Tipo" },
                Editora = new LinhaConteudoAjustarDTO { Conteudo = "Editora Válida", LimiteCaracteres = 100 },
                Assunto = new LinhaConteudoAjustarDTO { Conteudo = "Assunto Válido", LimiteCaracteres = 100 },
                Ano = new LinhaConteudoAjustarDTO { Conteudo = "2023", LimiteCaracteres = 4 },
                Edicao = new LinhaConteudoAjustarDTO { Conteudo = "1", LimiteCaracteres = 100 },
                NumeroPaginas = new LinhaConteudoAjustarDTO { Conteudo = "100", LimiteCaracteres = 10 },
                Largura = new LinhaConteudoAjustarDTO { Conteudo = "10", LimiteCaracteres = 10 },
                Altura = new LinhaConteudoAjustarDTO { Conteudo = "15", LimiteCaracteres = 10 },
                SerieColecao = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 250 },
                Volume = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 50 },
                Idioma = new LinhaConteudoAjustarDTO { Conteudo = "IDIOMA_VALIDO", LimiteCaracteres = 100 },
                LocalizacaoCDD = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 100 },
                LocalizacaoPHA = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 100 },
                NotasGerais = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 1000 },
                Isbn = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 17 },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 150 },
                SubTitulo = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 250 }
            };
            var linhas = new List<AcervoBibliograficoLinhaDTO> { linha };

            _servicoMaterialMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoDTO> { new IdNomeTipoExcluidoDTO { Id = 1, Nome = "MATERIAL_VALIDO", Tipo = (int)TipoMaterial.BIBLIOGRAFICO } });
            _servicoCreditoAutorMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoAuditavelDTO> { new IdNomeTipoExcluidoAuditavelDTO { Id = 1, Nome = "Autor Válido", Tipo = (int)TipoCreditoAutoria.Autoria },
                new IdNomeTipoExcluidoAuditavelDTO { Id = 3, Nome = "Algum Tipo", Tipo = (int)TipoCreditoAutoria.Credito }
            });
            _servicoEditoraMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO> { new IdNomeExcluidoAuditavelDTO { Id = 1, Nome = "Editora Válida" } });
            _servicoAssuntoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO> { new IdNomeExcluidoAuditavelDTO { Id = 1, Nome = "Assunto Válido" } });
            _servicoIdiomaMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoDTO> { new IdNomeExcluidoDTO { Id = 1, Nome = "IDIOMA_VALIDO" } });
            _repositorioParametroSistemaMock.Setup(r => r.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>())).ReturnsAsync(new ParametroSistema { Valor = "100" });

            var metodoCarregarDominios = _useCase.GetType().GetMethod("CarregarTodosOsDominios", BindingFlags.NonPublic | BindingFlags.Instance);
            await (Task)metodoCarregarDominios.Invoke(_useCase, null);

            _useCase.ValidarPreenchimentoValorFormatoQtdeCaracteres(linhas);

            Assert.True(linha.TipoAutoria.PossuiErro);
            Assert.Contains(Constantes.CAMPO_COAUTOR_SEM_PREENCHIMENTO_E_TIPO_AUTORIA_PREENCHIDO, linha.TipoAutoria.Mensagem);
        }

        [Fact]
        public async Task Validar_Preenchimento_Valor_Formato_Qtde_Caracteres_Deve_Marcar_Como_Erro_Quando_Mais_Tipos_Autoria_Que_CoAutores()
        {
            var linha = new AcervoBibliograficoLinhaDTO
            {
                NumeroLinha = 1,
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Titulo Válido", LimiteCaracteres = 100 },
                Material = new LinhaConteudoAjustarDTO { Conteudo = "MATERIAL_VALIDO", LimiteCaracteres = 100 },
                Autor = new LinhaConteudoAjustarDTO { Conteudo = "Autor Válido", LimiteCaracteres = 100 },
                CoAutor = new LinhaConteudoAjustarDTO { Conteudo = "CoAutor1" },
                TipoAutoria = new LinhaConteudoAjustarDTO { Conteudo = "Tipo1|Tipo2" },
                Editora = new LinhaConteudoAjustarDTO { Conteudo = "Editora Válida", LimiteCaracteres = 100 },
                Assunto = new LinhaConteudoAjustarDTO { Conteudo = "Assunto Válido", LimiteCaracteres = 100 },
                Idioma = new LinhaConteudoAjustarDTO { Conteudo = "IDIOMA_VALIDO", LimiteCaracteres = 100 },
                 SubTitulo = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 250 }
            };
            var linhas = new List<AcervoBibliograficoLinhaDTO> { linha };

            _servicoMaterialMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoDTO> { new IdNomeTipoExcluidoDTO { Id = 1, Nome = "MATERIAL_VALIDO", Tipo = (int)TipoMaterial.BIBLIOGRAFICO } });
            _servicoCreditoAutorMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoAuditavelDTO> {
                new IdNomeTipoExcluidoAuditavelDTO { Id = 1, Nome = "Autor Válido", Tipo = (int)TipoCreditoAutoria.Autoria },
                new IdNomeTipoExcluidoAuditavelDTO { Id = 2, Nome = "CoAutor1", Tipo = (int)TipoCreditoAutoria.Coautor },
                new IdNomeTipoExcluidoAuditavelDTO { Id = 3, Nome = "Tipo1", Tipo = (int)TipoCreditoAutoria.Credito },
                new IdNomeTipoExcluidoAuditavelDTO { Id = 4, Nome = "Tipo2", Tipo = (int)TipoCreditoAutoria.Credito }
            });

            _servicoEditoraMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO> { new IdNomeExcluidoAuditavelDTO { Id = 1, Nome = "Editora Válida" } });
            _servicoAssuntoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO> { new IdNomeExcluidoAuditavelDTO { Id = 1, Nome = "Assunto Válido" } });
            _servicoIdiomaMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoDTO> { new IdNomeExcluidoDTO { Id = 1, Nome = "IDIOMA_VALIDO" } });
            _repositorioParametroSistemaMock.Setup(r => r.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>())).ReturnsAsync(new ParametroSistema { Valor = "100" });

            var metodoCarregarDominios = _useCase.GetType().GetMethod("CarregarTodosOsDominios", BindingFlags.NonPublic | BindingFlags.Instance);
            await (Task)metodoCarregarDominios.Invoke(_useCase, null);

            var metodoObterCoAutores = _useCase.GetType().GetMethod("ObterCoAutores", BindingFlags.NonPublic | BindingFlags.Instance);
            await (Task)metodoObterCoAutores.Invoke(_useCase, new object[] { TipoCreditoAutoria.Coautor });

            _useCase.ValidarPreenchimentoValorFormatoQtdeCaracteres(linhas);

            Assert.True(linha.TipoAutoria.PossuiErro);
            Assert.Contains(Constantes.TEMOS_MAIS_TIPO_AUTORIA_QUE_COAUTORES, linha.TipoAutoria.Mensagem);
        }

        [Fact]
        public async Task Validar_Preenchimento_Valor_Formato_Qtde_Caracteres_Deve_Marcar_Linha_Como_Erro_Quando_Ocorrer_Excecao_Interna()
        {
            var linhaComErro = new AcervoBibliograficoLinhaDTO
            {
                NumeroLinha = 1,
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Titulo Válido", LimiteCaracteres = 100 },
                Material = new LinhaConteudoAjustarDTO { Conteudo = "MATERIAL_VALIDO", LimiteCaracteres = 100 },
                Autor = new LinhaConteudoAjustarDTO { Conteudo = "Autor Válido", LimiteCaracteres = 100 },
                Editora = new LinhaConteudoAjustarDTO { Conteudo = "Editora Válida", LimiteCaracteres = 100 },
                Assunto = new LinhaConteudoAjustarDTO { Conteudo = "Assunto Válido", LimiteCaracteres = 100 },
                Ano = new LinhaConteudoAjustarDTO { Conteudo = "2023", LimiteCaracteres = 4 },
                Edicao = new LinhaConteudoAjustarDTO { Conteudo = "1", LimiteCaracteres = 100 },
                NumeroPaginas = new LinhaConteudoAjustarDTO { Conteudo = "100", LimiteCaracteres = 10 },
                Largura = new LinhaConteudoAjustarDTO { Conteudo = "10", LimiteCaracteres = 10 },
                Altura = new LinhaConteudoAjustarDTO { Conteudo = "15", LimiteCaracteres = 10 },
                SerieColecao = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 250 },
                Volume = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 50 },
                Idioma = new LinhaConteudoAjustarDTO { Conteudo = "IDIOMA_VALIDO", LimiteCaracteres = 100 },
                LocalizacaoCDD = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 100 },
                LocalizacaoPHA = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 100 },
                NotasGerais = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 1000 },
                Isbn = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 17 },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = "", LimiteCaracteres = 150 }
            };

            var linhas = new List<AcervoBibliograficoLinhaDTO> { linhaComErro };

            _servicoMaterialMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoDTO> { new IdNomeTipoExcluidoDTO { Id = 1, Nome = "MATERIAL_VALIDO", Tipo = (int)TipoMaterial.BIBLIOGRAFICO } });
            _servicoCreditoAutorMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeTipoExcluidoAuditavelDTO> { new IdNomeTipoExcluidoAuditavelDTO { Id = 1, Nome = "Autor Válido", Tipo = (int)TipoCreditoAutoria.Autoria } });
            _servicoEditoraMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO> { new IdNomeExcluidoAuditavelDTO { Id = 1, Nome = "Editora Válida" } });
            _servicoAssuntoMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoAuditavelDTO> { new IdNomeExcluidoAuditavelDTO { Id = 1, Nome = "Assunto Válido" } });
            _servicoIdiomaMock.Setup(s => s.ObterTodos()).ReturnsAsync(new List<IdNomeExcluidoDTO> { new IdNomeExcluidoDTO { Id = 1, Nome = "IDIOMA_VALIDO" } });

            _repositorioParametroSistemaMock.Setup(r => r.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), It.IsAny<int>()))
                .ReturnsAsync(new ParametroSistema { Valor = "100" });

            var metodoCarregarDominios = _useCase.GetType().GetMethod("CarregarTodosOsDominios", BindingFlags.NonPublic | BindingFlags.Instance);
            await (Task)metodoCarregarDominios.Invoke(_useCase, null);

            linhas[0].Titulo = null;

            _useCase.ValidarPreenchimentoValorFormatoQtdeCaracteres(linhas);

            Assert.True(linhaComErro.PossuiErros);
            Assert.Contains("Ocorreu uma falha inesperada na linha '1'", linhaComErro.Mensagem);
            Assert.Contains("Object reference not set to an instance of an object", linhaComErro.Mensagem);
        }
    }
}
