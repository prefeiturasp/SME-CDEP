using AutoMapper;
using Bogus;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Aplicacao.Servicos.Fachadas;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dados;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;
using System.Data;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoManutencaoSolicitacaoAcervoTests
    {
        private readonly AutoMocker _mocker;
        private readonly ServicoManutencaoSolicitacaoAcervo _servico;
        private readonly Faker _faker;
        private readonly Mock<ITransacao> _transacaoMock;
        private readonly Mock<IRepositorioAcervoSolicitacao> _repoSolicitacaoMock;
        private readonly Mock<IRepositorioAcervoSolicitacaoItem> _repoItemMock;
        private readonly Mock<IServicoEvento> _servicoEventoMock;
        private readonly Mock<IServicoAcervoBibliografico> _servicoBiblioMock;
        private readonly Mock<IMapper> _mapperMock;

        public ServicoManutencaoSolicitacaoAcervoTests()
        {
            _mocker = new AutoMocker();
            _faker = new Faker("pt_BR");

            // 1. Configurando Mock de Transação
            _transacaoMock = _mocker.GetMock<ITransacao>();
            _transacaoMock.Setup(t => t.Iniciar()).Returns(new Mock<IDbTransaction>().Object);

            // 2. Criando e Registrando os Facades (Contextos)
            // O AutoMocker preenche os construtores dos records com Mocks automaticamente
            var contextoDados = _mocker.CreateInstance<ContextoDadosAcervoSolicitacao>();
            var contextoInfra = _mocker.CreateInstance<ContextoInfraAcervoSolicitacao>();
            var contextoRegras = _mocker.CreateInstance<ContextoRegrasAcervoSolicitacao>();

            _mocker.Use(contextoDados);
            _mocker.Use(contextoInfra);
            _mocker.Use(contextoRegras);

            // 3. Capturando referências úteis para os Asserts
            _repoSolicitacaoMock = _mocker.GetMock<IRepositorioAcervoSolicitacao>();
            _repoItemMock = _mocker.GetMock<IRepositorioAcervoSolicitacaoItem>();
            _servicoEventoMock = _mocker.GetMock<IServicoEvento>();
            _servicoBiblioMock = _mocker.GetMock<IServicoAcervoBibliografico>();
            _mapperMock = _mocker.GetMock<IMapper>();

            // Setup padrão de usuário logado
            _mocker.GetMock<IServicoUsuario>()
                .Setup(s => s.ObterUsuarioLogado())
                .ReturnsAsync(new UsuarioDTO { Id = 123 });

            // Setup padrão de usuário solicitante
            _mocker.GetMock<IServicoUsuario>()
                .Setup(s => s.ObterPorId(It.IsAny<long>()))
                .ReturnsAsync(new UsuarioDTO { Id = 456 });

            // 4. Instanciando o SUT (System Under Test)
            _servico = _mocker.CreateInstance<ServicoManutencaoSolicitacaoAcervo>();
        }

        #region Testes de Inserir

        [Fact(DisplayName = "Dado solicitação presencial SEM data de visita, Quando Inserir, Então deve ficar PRESENCIAL_ABERTO")]
        public async Task DadoSolicitacaoPresencialSemDataVisita_QuandoInserir_EntaoItemDeveFicarPresencialAberto()
        {
            // Arrange
            var dto = GerarDtoManual(TipoAtendimento.Presencial, TipoAcervo.DocumentacaoTextual);
            var itemDto = dto.Itens.First();
            itemDto.DataVisita = null; // Sem data definida

            ConfigurarMapperInsercao(dto);

            // Act
            await _servico.Inserir(dto);

            // Assert
            _repoItemMock.Verify(x => x.Inserir(It.Is<AcervoSolicitacaoItem>(i =>
                i.Situacao == SituacaoSolicitacaoItem.PRESENCIAL_ABERTO &&
                i.DataVisita == null
            )), Times.Once);

            // Não deve tentar criar evento
            _servicoEventoMock.Verify(x => x.InserirEventoVisita(It.IsAny<DateTime>(), It.IsAny<long>()), Times.Never);
        }

        [Fact(DisplayName = "Dado solicitação com data de empréstimo menor que visita, Quando Inserir, Então deve lançar exceção")]
        public async Task DadoDataEmprestimoMenorQueVisita_QuandoInserir_EntaoDeveLancarExcecao()
        {
            // Arrange
            var dto = GerarDtoManual(TipoAtendimento.Presencial, TipoAcervo.Bibliografico);
            var item = dto.Itens.First();
            item.DataVisita = DateTime.Now.AddDays(5);
            item.DataEmprestimo = DateTime.Now.AddDays(4); // Erro: Empréstimo antes da visita

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.Inserir(dto));

            _repoSolicitacaoMock.Verify(x => x.Inserir(It.IsAny<AcervoSolicitacao>()), Times.Never);
        }

        #endregion

        #region Testes de Alterar

        [Fact(DisplayName = "Dado alteração de data de visita em item existente, Quando Alterar, Então deve atualizar evento")]
        public async Task DadoAlteracaoDeDataVisita_QuandoAlterar_EntaoDeveAtualizarEvento()
        {
            // Arrange
            var dto = GerarDtoManual(TipoAtendimento.Presencial, TipoAcervo.DocumentacaoTextual);
            dto.Id = 10;
            var itemDto = dto.Itens.First();
            itemDto.Id = 50;
            itemDto.DataVisita = DateTime.Now.AddDays(10); // Nova data

            var itemExistente = new AcervoSolicitacaoItem
            {
                Id = 50,
                AcervoSolicitacaoId = 10,
                TipoAtendimento = TipoAtendimento.Presencial,
                DataVisita = DateTime.Now.AddDays(5) // Data antiga
            };

            ConfigurarMockAlteracao(dto, itemExistente);

            // Act
            await _servico.Alterar(dto);

            // Assert
            _servicoEventoMock.Verify(x => x.AtualizarEventoVisita(itemDto.DataVisita.Value, 50), Times.Once);
        }

        [Fact(DisplayName = "Dado adição de novo item na alteração, Quando Alterar, Então deve inserir novo item")]
        public async Task DadoNovoItemNaAlteracao_QuandoAlterar_EntaoDeveInserirItem()
        {
            // Arrange
            var dto = GerarDtoManual(TipoAtendimento.Presencial, TipoAcervo.DocumentacaoTextual);
            dto.Id = 10;
            var itemDto = dto.Itens.First();
            itemDto.Id = null; // Item novo (sem ID)

            // Setup: Solicitação existe, mas item não está na lista de "Atuais"
            _repoSolicitacaoMock.Setup(r => r.ObterPorId(10)).ReturnsAsync(new AcervoSolicitacao { Id = 10 });
            _repoItemMock.Setup(r => r.ObterItensPorSolicitacaoId(10)).ReturnsAsync([]);

            _mapperMock.Setup(m => m.Map<AcervoSolicitacaoItem>(It.IsAny<AcervoSolicitacaoItemManualDTO>()))
                .Returns(new AcervoSolicitacaoItem { TipoAtendimento = TipoAtendimento.Presencial, DataVisita = itemDto.DataVisita });

            // Act
            await _servico.Alterar(dto);

            // Assert
            _repoItemMock.Verify(x => x.Inserir(It.IsAny<AcervoSolicitacaoItem>()), Times.Once);
            _servicoEventoMock.Verify(x => x.InserirEventoVisita(It.IsAny<DateTime>(), It.IsAny<long>()), Times.Once);
        }

        [Fact(DisplayName = "Dado solicitação inexistente, Quando Alterar, Então deve lançar exceção")]
        public async Task DadoSolicitacaoInexistente_QuandoAlterar_EntaoDeveLancarExcecao()
        {
            // Arrange
            var dto = GerarDtoManual();

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _servico.Alterar(dto));
        }

        #endregion

        // --- Helpers ---

        private AcervoSolicitacaoManualDTO GerarDtoManual(TipoAtendimento tipo = TipoAtendimento.Presencial, TipoAcervo acervo = TipoAcervo.Bibliografico)
        {
            var item = new AcervoSolicitacaoItemManualDTO
            {
                AcervoId = _faker.Random.Int(1, 100),
                TipoAtendimento = tipo,
                TipoAcervo = acervo,
                DataVisita = tipo == TipoAtendimento.Presencial ? DateTime.Now.AddDays(2) : null
            };

            if (acervo == TipoAcervo.Bibliografico && tipo == TipoAtendimento.Presencial)
            {
                item.DataEmprestimo = DateTime.Now.AddDays(2);
                item.DataDevolucao = DateTime.Now.AddDays(5);
            }

            return new AcervoSolicitacaoManualDTO
            {
                Id = _faker.Random.Int(1, 100),
                UsuarioId = 1,
                DataSolicitacao = DateTime.Now,
                Itens = new[] { item }
            };
        }

        private void ConfigurarMapperInsercao(AcervoSolicitacaoManualDTO dto)
        {
            // Mock do mapeamento Header
            var entidade = new AcervoSolicitacao
            {
                Id = dto.Id,
                Itens = dto.Itens.Select(i => new AcervoSolicitacaoItem
                {
                    AcervoId = i.AcervoId,
                    TipoAtendimento = i.TipoAtendimento,
                    DataVisita = i.DataVisita
                }).ToList()
            };

            _mapperMock.Setup(m => m.Map<AcervoSolicitacao>(dto)).Returns(entidade);
            _repoSolicitacaoMock.Setup(r => r.Inserir(It.IsAny<AcervoSolicitacao>())).ReturnsAsync(dto.Id);
        }

        private void ConfigurarMockAlteracao(AcervoSolicitacaoManualDTO dto, AcervoSolicitacaoItem itemExistente)
        {
            _repoSolicitacaoMock.Setup(r => r.ObterPorId(dto.Id))
                .ReturnsAsync(new AcervoSolicitacao { Id = dto.Id });

            _repoItemMock.Setup(r => r.ObterItensPorSolicitacaoId(dto.Id))
                .ReturnsAsync(new List<AcervoSolicitacaoItem> { itemExistente });

            // Mock do mapeamento para o loop do Alterar
            _mapperMock.Setup(m => m.Map<AcervoSolicitacaoItem>(It.IsAny<AcervoSolicitacaoItemManualDTO>()))
                .Returns((AcervoSolicitacaoItemManualDTO source) => new AcervoSolicitacaoItem
                {
                    Id = source.Id ?? 0,
                    AcervoId = source.AcervoId,
                    TipoAtendimento = source.TipoAtendimento,
                    DataVisita = source.DataVisita
                });
        }
    }
}
