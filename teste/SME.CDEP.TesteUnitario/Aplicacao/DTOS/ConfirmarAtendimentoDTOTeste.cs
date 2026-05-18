using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using Xunit;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class ConfirmarAtendimentoDTOTeste
    {
        private readonly Faker _faker;

        public ConfirmarAtendimentoDTOTeste()
        {
            _faker = new Faker("pt_BR");
        }

        #region Testes de Construção e Propriedades

        [Fact]
        public void DadoConfirmarAtendimentoDTOSemParâmetros_QuandoConstruir_EntaoDeveTerTodasAsPropriedadesNulas()
        {
            var dto = new ConfirmarAtendimentoDTO();

            dto.Id.Should().Be(0);
            dto.ItemId.Should().Be(0);
        }

        [Fact]
        public void DadoConfirmarAtendimentoDTOComIdDefinido_QuandoAtribuir_EntaoDeveArmazenarValorCorretamente()
        {
            var id = _faker.Random.Long(1, 1000);
            var dto = new ConfirmarAtendimentoDTO();

            dto.Id = id;

            dto.Id.Should().Be(id);
        }

        [Fact]
        public void DadoConfirmarAtendimentoDTOComItemIdDefinido_QuandoAtribuir_EntaoDeveArmazenarValorCorretamente()
        {
            var itemId = _faker.Random.Long(1, 1000);
            var dto = new ConfirmarAtendimentoDTO();

            dto.ItemId = itemId;

            dto.ItemId.Should().Be(itemId);
        }

        [Fact]
        public void DadoConfirmarAtendimentoDTOInicializadoComTodosDados_QuandoConstruir_EntaoDeveTerTodosValoresPreenchidos()
        {
            var id = _faker.Random.Long(1, 1000);
            var itemId = _faker.Random.Long(1, 1000);

            var dto = new ConfirmarAtendimentoDTO
            {
                Id = id,
                ItemId = itemId
            };

            dto.Id.Should().Be(id);
            dto.ItemId.Should().Be(itemId);
        }

        #endregion

        #region Testes de Inicialização com Valores

        [Fact]
        public void DadoConfirmarAtendimentoDTOInicializadoComDadosParciais_QuandoConstruir_EntaoDeveTerApenasValoresDefinidosPreenchidos()
        {
            var id = _faker.Random.Long(1, 1000);

            var dto = new ConfirmarAtendimentoDTO
            {
                Id = id
            };

            dto.Id.Should().Be(id);
            dto.ItemId.Should().Be(0);
        }

        [Fact]
        public void DadoConfirmarAtendimentoDTOInicializadoComSomenteItemId_QuandoConstruir_EntaoDeveTerApenasItemIdPreenchido()
        {
            var itemId = _faker.Random.Long(1, 1000);

            var dto = new ConfirmarAtendimentoDTO
            {
                ItemId = itemId
            };

            dto.Id.Should().Be(0);
            dto.ItemId.Should().Be(itemId);
        }

        #endregion

        #region Testes de Tipos de Dados

        [Theory]
        [InlineData(1L)]
        [InlineData(100L)]
        [InlineData(long.MaxValue)]
        public void DadoConfirmarAtendimentoDTOComDiferentesValoresDeLong_QuandoAtribuirId_EntaoDeveArmazenarCorretamente(long valor)
        {
            var dto = new ConfirmarAtendimentoDTO { Id = valor };

            dto.Id.Should().Be(valor);
        }

        [Theory]
        [InlineData(1L)]
        [InlineData(100L)]
        [InlineData(long.MaxValue)]
        public void DadoConfirmarAtendimentoDTOComDiferentesValoresDeLong_QuandoAtribuirItemId_EntaoDeveArmazenarCorretamente(long valor)
        {
            var dto = new ConfirmarAtendimentoDTO { ItemId = valor };

            dto.ItemId.Should().Be(valor);
        }

        #endregion

        #region Testes de Múltiplas Instâncias

        [Fact]
        public void DadoDuasInstânciasDeConfirmarAtendimentoDTO_QuandoConstruidas_EntaoDevemSerIndependentes()
        {
            var id1 = _faker.Random.Long(1, 100);
            var itemId1 = _faker.Random.Long(101, 200);
            var id2 = _faker.Random.Long(201, 300);
            var itemId2 = _faker.Random.Long(301, 400);

            var dto1 = new ConfirmarAtendimentoDTO
            {
                Id = id1,
                ItemId = itemId1
            };

            var dto2 = new ConfirmarAtendimentoDTO
            {
                Id = id2,
                ItemId = itemId2
            };

            dto1.Id.Should().Be(id1);
            dto2.Id.Should().Be(id2);
            dto1.ItemId.Should().Be(itemId1);
            dto2.ItemId.Should().Be(itemId2);
            dto1.Should().NotBe(dto2);
        }

        [Fact]
        public void DadoListaDeConfirmarAtendimentoDTO_QuandoConstruir_EntaoDeveArmazenarMultiplosItens()
        {
            var listaConfirmacoes = new List<ConfirmarAtendimentoDTO>();

            for (int i = 0; i < 5; i++)
            {
                listaConfirmacoes.Add(new ConfirmarAtendimentoDTO
                {
                    Id = _faker.Random.Long(1, 1000),
                    ItemId = _faker.Random.Long(1, 1000)
                });
            }

            listaConfirmacoes.Should().HaveCount(5);
            listaConfirmacoes.Should().AllSatisfy(item =>
            {
                item.Id.Should().BeGreaterThan(0);
                item.ItemId.Should().BeGreaterThan(0);
            });
        }

        #endregion

        #region Testes de Atualização de Propriedades

        [Fact]
        public void DadoConfirmarAtendimentoDTOComValoresIniciais_QuandoAtualizarTodosDados_EntaoDeveAtualizarCorretamente()
        {
            var idOriginal = _faker.Random.Long(1, 100);
            var itemIdOriginal = _faker.Random.Long(1, 100);
            var dto = new ConfirmarAtendimentoDTO
            {
                Id = idOriginal,
                ItemId = itemIdOriginal
            };

            var novoId = _faker.Random.Long(101, 200);
            var novoItemId = _faker.Random.Long(101, 200);

            dto.Id = novoId;
            dto.ItemId = novoItemId;

            dto.Id.Should().Be(novoId);
            dto.ItemId.Should().Be(novoItemId);
            dto.Id.Should().NotBe(idOriginal);
            dto.ItemId.Should().NotBe(itemIdOriginal);
        }

        [Fact]
        public void DadoConfirmarAtendimentoDTOComValoresPreenchidos_QuandoAtribuirZero_EntaoTodosDevemSerZero()
        {
            var dto = new ConfirmarAtendimentoDTO
            {
                Id = _faker.Random.Long(1, 1000),
                ItemId = _faker.Random.Long(1, 1000)
            };

            dto.Id = 0;
            dto.ItemId = 0;

            dto.Id.Should().Be(0);
            dto.ItemId.Should().Be(0);
        }

        [Fact]
        public void DadoConfirmarAtendimentoDTOAtualizandoApenasId_QuandoAtualizar_EntaoItemIdNaoMuda()
        {
            var itemIdOriginal = _faker.Random.Long(1, 1000);
            var dto = new ConfirmarAtendimentoDTO
            {
                Id = _faker.Random.Long(1, 1000),
                ItemId = itemIdOriginal
            };

            var novoId = _faker.Random.Long(1000, 2000);

            dto.Id = novoId;

            dto.Id.Should().Be(novoId);
            dto.ItemId.Should().Be(itemIdOriginal);
        }

        [Fact]
        public void DadoConfirmarAtendimentoDTOAtualizandoApenasItemId_QuandoAtualizar_EntaoIdNaoMuda()
        {
            var idOriginal = _faker.Random.Long(1, 1000);
            var dto = new ConfirmarAtendimentoDTO
            {
                Id = idOriginal,
                ItemId = _faker.Random.Long(1, 1000)
            };

            var novoItemId = _faker.Random.Long(1000, 2000);

            dto.ItemId = novoItemId;

            dto.Id.Should().Be(idOriginal);
            dto.ItemId.Should().Be(novoItemId);
        }

        #endregion

        #region Testes de Valores Limítrofes

        [Fact]
        public void DadoConfirmarAtendimentoDTOComIdZero_QuandoAtribuir_EntaoDeveArmazenarZero()
        {
            var dto = new ConfirmarAtendimentoDTO { Id = 0 };

            dto.Id.Should().Be(0);
        }

        [Fact]
        public void DadoConfirmarAtendimentoDTOComItemIdZero_QuandoAtribuir_EntaoDeveArmazenarZero()
        {
            var dto = new ConfirmarAtendimentoDTO { ItemId = 0 };

            dto.ItemId.Should().Be(0);
        }

        [Fact]
        public void DadoConfirmarAtendimentoDTOComIdValorMáximo_QuandoAtribuir_EntaoDeveArmazenarValorMáximo()
        {
            var dto = new ConfirmarAtendimentoDTO { Id = long.MaxValue };

            dto.Id.Should().Be(long.MaxValue);
        }

        [Fact]
        public void DadoConfirmarAtendimentoDTOComItemIdValorMáximo_QuandoAtribuir_EntaoDeveArmazenarValorMáximo()
        {
            var dto = new ConfirmarAtendimentoDTO { ItemId = long.MaxValue };

            dto.ItemId.Should().Be(long.MaxValue);
        }

        [Fact]
        public void DadoConfirmarAtendimentoDTOComIdNegativo_QuandoAtribuir_EntaoDeveArmazenarValorNegativo()
        {
            var dto = new ConfirmarAtendimentoDTO { Id = -1 };

            dto.Id.Should().Be(-1);
        }

        [Fact]
        public void DadoConfirmarAtendimentoDTOComItemIdNegativo_QuandoAtribuir_EntaoDeveArmazenarValorNegativo()
        {
            var dto = new ConfirmarAtendimentoDTO { ItemId = -1 };

            dto.ItemId.Should().Be(-1);
        }

        #endregion

        #region Testes de Características Estruturais

        [Fact]
        public void DadoConfirmarAtendimentoDTO_QuandoVerificarTipo_EntaoDeveSerClasseComDuasPropriedades()
        {
            var dtoType = typeof(ConfirmarAtendimentoDTO);

            var propriedades = dtoType.GetProperties();

            propriedades.Should().HaveCount(2);
            propriedades.Should().Contain(p => p.Name == "Id");
            propriedades.Should().Contain(p => p.Name == "ItemId");
        }

        [Fact]
        public void DadoConfirmarAtendimentoDTO_QuandoVerificarPropriedades_EntaoAmbdevemSerLong()
        {
            var dtoType = typeof(ConfirmarAtendimentoDTO);

            var propriedades = dtoType.GetProperties();

            propriedades.Should().AllSatisfy(p =>
            {
                p.PropertyType.Should().Be<long>();
            });
        }

        [Fact]
        public void DadoConfirmarAtendimentoDTO_QuandoVerificarPropriedades_EntaoAmbdevemTerGetterESetter()
        {
            var dtoType = typeof(ConfirmarAtendimentoDTO);

            var propriedades = dtoType.GetProperties();

            propriedades.Should().AllSatisfy(p =>
            {
                p.GetMethod.Should().NotBeNull();
                p.SetMethod.Should().NotBeNull();
            });
        }

        #endregion

        #region Testes de Comportamento

        [Fact]
        public void DadoConfirmarAtendimentoDTOComValoresDefinidos_QuandoAcessarPropriedades_EntaoRetornarValoresArmazenados()
        {
            var id = _faker.Random.Long(1, 1000);
            var itemId = _faker.Random.Long(1, 1000);

            var dto = new ConfirmarAtendimentoDTO
            {
                Id = id,
                ItemId = itemId
            };

            var idRetornado = dto.Id;
            var itemIdRetornado = dto.ItemId;

            idRetornado.Should().Be(id);
            itemIdRetornado.Should().Be(itemId);
        }

        [Fact]
        public void DadoConfirmarAtendimentoDTOAposInstanciacao_QuandoVerificarEstado_EntaoValoresPadraoSeraoZero()
        {
            var dto = new ConfirmarAtendimentoDTO();

            dto.Id.Should().Be(default(long));
            dto.ItemId.Should().Be(default(long));
        }

        #endregion

        #region Testes de Combinações de Valores

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 100)]
        [InlineData(100, 1)]
        [InlineData(long.MaxValue, long.MaxValue)]
        [InlineData(1, long.MaxValue)]
        [InlineData(long.MaxValue, 1)]
        public void DadoConfirmarAtendimentoDTOComDiferentesCombinacoes_QuandoAtribuirValores_EntaoDevemSerArmazenadosCorretamente(long id, long itemId)
        {
            var dto = new ConfirmarAtendimentoDTO
            {
                Id = id,
                ItemId = itemId
            };

            dto.Id.Should().Be(id);
            dto.ItemId.Should().Be(itemId);
        }

        #endregion
    }
}
