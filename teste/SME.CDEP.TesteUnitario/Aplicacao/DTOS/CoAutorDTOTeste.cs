using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using Xunit;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class CoAutorDTOTeste
    {
        private readonly Faker _faker;

        public CoAutorDTOTeste()
        {
            _faker = new Faker("pt_BR");
        }

        #region Testes de Construção e Propriedades

        [Fact]
        public void DadoCoAutorDTOSemParâmetros_QuandoConstruir_EntaoDeveTerTodasAsPropriedadesNulas()
        {
            var dto = new CoAutorDTO();

            dto.CreditoAutorId.Should().BeNull();
            dto.TipoAutoria.Should().BeNull();
            dto.CreditoAutorNome.Should().BeNull();
        }

        [Fact]
        public void DadoCoAutorDTOComCreditoAutorIdDefinido_QuandoAtribuir_EntaoDeveArmazenarValorCorretamente()
        {
            var creditoAutorId = _faker.Random.Long(1, 1000);
            var dto = new CoAutorDTO();

            dto.CreditoAutorId = creditoAutorId;

            dto.CreditoAutorId.Should().Be(creditoAutorId);
        }

        [Fact]
        public void DadoCoAutorDTOComCreditoAutorIdNulo_QuandoAtribuir_EntaoDeveArmazenarNulo()
        {
            var dto = new CoAutorDTO { CreditoAutorId = _faker.Random.Long(1, 1000) };

            dto.CreditoAutorId = null;

            dto.CreditoAutorId.Should().BeNull();
        }

        [Fact]
        public void DadoCoAutorDTOComTipoAutoriaDefinido_QuandoAtribuir_EntaoDeveArmazenarValorCorretamente()
        {
            var tipoAutoria = _faker.Lorem.Word();
            var dto = new CoAutorDTO();

            dto.TipoAutoria = tipoAutoria;

            dto.TipoAutoria.Should().Be(tipoAutoria);
        }

        [Fact]
        public void DadoCoAutorDTOComTipoAutoriaNulo_QuandoAtribuir_EntaoDeveArmazenarNulo()
        {
            var dto = new CoAutorDTO { TipoAutoria = _faker.Lorem.Word() };

            dto.TipoAutoria = null;

            dto.TipoAutoria.Should().BeNull();
        }

        [Fact]
        public void DadoCoAutorDTOComCreditoAutorNomeDefinido_QuandoAtribuir_EntaoDeveArmazenarValorCorretamente()
        {
            var creditoAutorNome = _faker.Person.FullName;
            var dto = new CoAutorDTO();

            dto.CreditoAutorNome = creditoAutorNome;

            dto.CreditoAutorNome.Should().Be(creditoAutorNome);
        }

        [Fact]
        public void DadoCoAutorDTOComCreditoAutorNomeNulo_QuandoAtribuir_EntaoDeveArmazenarNulo()
        {
            var dto = new CoAutorDTO { CreditoAutorNome = _faker.Person.FullName };

            dto.CreditoAutorNome = null;

            dto.CreditoAutorNome.Should().BeNull();
        }

        #endregion

        #region Testes de Inicialização com Valores

        [Fact]
        public void DadoCoAutorDTOInicializadoComTodosDados_QuandoConstruir_EntaoDeveTerTodosValoresPreenchidos()
        {
            var creditoAutorId = _faker.Random.Long(1, 1000);
            var tipoAutoria = _faker.Lorem.Word();
            var creditoAutorNome = _faker.Person.FullName;

            var dto = new CoAutorDTO
            {
                CreditoAutorId = creditoAutorId,
                TipoAutoria = tipoAutoria,
                CreditoAutorNome = creditoAutorNome
            };

            dto.CreditoAutorId.Should().Be(creditoAutorId);
            dto.TipoAutoria.Should().Be(tipoAutoria);
            dto.CreditoAutorNome.Should().Be(creditoAutorNome);
        }

        [Fact]
        public void DadoCoAutorDTOInicializadoComDadosParciais_QuandoConstruir_EntaoDeveTerApenasValoresDefinidosPreenchidos()
        {
            var creditoAutorId = _faker.Random.Long(1, 1000);

            var dto = new CoAutorDTO
            {
                CreditoAutorId = creditoAutorId
            };

            dto.CreditoAutorId.Should().Be(creditoAutorId);
            dto.TipoAutoria.Should().BeNull();
            dto.CreditoAutorNome.Should().BeNull();
        }

        #endregion

        #region Testes de Tipos de Dados

        [Theory]
        [InlineData(1L)]
        [InlineData(100L)]
        [InlineData(long.MaxValue)]
        public void DadoCoAutorDTOComDiferentesValoresDeLong_QuandoAtribuirCreditoAutorId_EntaoDeveArmazenarCorretamente(long valor)
        {
            var dto = new CoAutorDTO { CreditoAutorId = valor };

            dto.CreditoAutorId.Should().Be(valor);
        }

        [Theory]
        [InlineData("Autor")]
        [InlineData("Co-autor")]
        [InlineData("Editor")]
        [InlineData("Tradutor")]
        [InlineData("")]
        public void DadoCoAutorDTOComDiferentesTiposDeAutoria_QuandoAtribuirTipoAutoria_EntaoDeveArmazenarCorretamente(string tipo)
        {
            var dto = new CoAutorDTO { TipoAutoria = tipo };

            dto.TipoAutoria.Should().Be(tipo);
        }

        [Theory]
        [InlineData("João Silva")]
        [InlineData("Maria Santos")]
        [InlineData("Pedro Oliveira")]
        [InlineData("")]
        public void DadoCoAutorDTOComDiferentesNomes_QuandoAtribuirCreditoAutorNome_EntaoDeveArmazenarCorretamente(string nome)
        {
            var dto = new CoAutorDTO { CreditoAutorNome = nome };

            dto.CreditoAutorNome.Should().Be(nome);
        }

        #endregion

        #region Testes de Múltiplas Instâncias

        [Fact]
        public void DadoDuasInstânciasDeCoAutorDTO_QuandoConstruidas_EntaoDevemSerIndependentes()
        {
            var creditoAutorId1 = _faker.Random.Long(1, 100);
            var creditoAutorId2 = _faker.Random.Long(101, 1000);
            var tipoAutoria1 = _faker.Lorem.Word();
            var tipoAutoria2 = _faker.Lorem.Word();

            var dto1 = new CoAutorDTO
            {
                CreditoAutorId = creditoAutorId1,
                TipoAutoria = tipoAutoria1
            };

            var dto2 = new CoAutorDTO
            {
                CreditoAutorId = creditoAutorId2,
                TipoAutoria = tipoAutoria2
            };

            dto1.CreditoAutorId.Should().Be(creditoAutorId1);
            dto2.CreditoAutorId.Should().Be(creditoAutorId2);
            dto1.TipoAutoria.Should().Be(tipoAutoria1);
            dto2.TipoAutoria.Should().Be(tipoAutoria2);
            dto1.Should().NotBe(dto2);
        }

        [Fact]
        public void DadoListaDeCoAutoresDTO_QuandoConstruir_EntaoDeveArmazenarMultiplosItens()
        {
            var listaCoAutores = new List<CoAutorDTO>();

            for (int i = 0; i < 5; i++)
            {
                listaCoAutores.Add(new CoAutorDTO
                {
                    CreditoAutorId = _faker.Random.Long(1, 1000),
                    TipoAutoria = _faker.Lorem.Word(),
                    CreditoAutorNome = _faker.Person.FullName
                });
            }

            listaCoAutores.Should().HaveCount(5);
            listaCoAutores.Should().AllSatisfy(item =>
            {
                item.CreditoAutorId.Should().NotBeNull();
                item.TipoAutoria.Should().NotBeNullOrEmpty();
                item.CreditoAutorNome.Should().NotBeNullOrEmpty();
            });
        }

        #endregion

        #region Testes de Atualização de Propriedades

        [Fact]
        public void DadoCoAutorDTOComValoresIniciais_QuandoAtualizarTodosDados_EntaoDeveMantêSomenteCreditoAutorId()
        {
            var creditoAutorIdOriginal = _faker.Random.Long(1, 1000);
            var dto = new CoAutorDTO
            {
                CreditoAutorId = creditoAutorIdOriginal,
                TipoAutoria = _faker.Lorem.Word(),
                CreditoAutorNome = _faker.Person.FullName
            };

            var novoTipoAutoria = _faker.Lorem.Word();
            var novoNome = _faker.Person.FullName;

            dto.TipoAutoria = novoTipoAutoria;
            dto.CreditoAutorNome = novoNome;

            dto.CreditoAutorId.Should().Be(creditoAutorIdOriginal);
            dto.TipoAutoria.Should().Be(novoTipoAutoria);
            dto.CreditoAutorNome.Should().Be(novoNome);
        }

        [Fact]
        public void DadoCoAutorDTOComValoresPreenchidos_QuandoLimparTodosValores_EntaoTodosDevemSerNulos()
        {
            var dto = new CoAutorDTO
            {
                CreditoAutorId = _faker.Random.Long(1, 1000),
                TipoAutoria = _faker.Lorem.Word(),
                CreditoAutorNome = _faker.Person.FullName
            };

            dto.CreditoAutorId = null;
            dto.TipoAutoria = null;
            dto.CreditoAutorNome = null;

            dto.CreditoAutorId.Should().BeNull();
            dto.TipoAutoria.Should().BeNull();
            dto.CreditoAutorNome.Should().BeNull();
        }

        #endregion

        #region Testes de Valores Limítrofes

        [Fact]
        public void DadoCoAutorDTOComCreditoAutorIdZero_QuandoAtribuir_EntaoDeveArmazenaValorZero()
        {
            var dto = new CoAutorDTO { CreditoAutorId = 0 };

            dto.CreditoAutorId.Should().Be(0);
        }

        [Fact]
        public void DadoCoAutorDTOComCreditoAutorIdValorMáximo_QuandoAtribuir_EntaoDeveArmazenarValorMáximo()
        {
            var dto = new CoAutorDTO { CreditoAutorId = long.MaxValue };

            dto.CreditoAutorId.Should().Be(long.MaxValue);
        }

        [Fact]
        public void DadoCoAutorDTOComTipoAutoriaVazio_QuandoAtribuir_EntaoDeveArmazenarStringVazia()
        {
            var dto = new CoAutorDTO { TipoAutoria = string.Empty };

            dto.TipoAutoria.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoCoAutorDTOComCreditoAutorNomeVazio_QuandoAtribuir_EntaoDeveArmazenarStringVazia()
        {
            var dto = new CoAutorDTO { CreditoAutorNome = string.Empty };

            dto.CreditoAutorNome.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoCoAutorDTOComTipoAutoriaComEspacos_QuandoAtribuir_EntaoDeveArmazenarComEspacos()
        {
            var tipoAutoriaComEspacos = "   Tipo Autoria   ";

            var dto = new CoAutorDTO { TipoAutoria = tipoAutoriaComEspacos };

            dto.TipoAutoria.Should().Be(tipoAutoriaComEspacos);
        }

        [Fact]
        public void DadoCoAutorDTOComNomeComEspacos_QuandoAtribuir_EntaoDeveArmazenarComEspacos()
        {
            var nomeComEspacos = "   Nome do Autor   ";

            var dto = new CoAutorDTO { CreditoAutorNome = nomeComEspacos };

            dto.CreditoAutorNome.Should().Be(nomeComEspacos);
        }

        #endregion

        #region Testes de Caracteres Especiais

        [Theory]
        [InlineData("Autoria@")]
        [InlineData("Autoria#123")]
        [InlineData("Autoria-")]
        [InlineData("Autoria_Especial")]
        public void DadoCoAutorDTOComTipoAutoriaCaracteresEspeciais_QuandoAtribuir_EntaoDeveArmazenarCorretamente(string tipo)
        {
            var dto = new CoAutorDTO { TipoAutoria = tipo };

            dto.TipoAutoria.Should().Be(tipo);
        }

        [Theory]
        [InlineData("João")]
        [InlineData("José")]
        [InlineData("François")]
        [InlineData("Müller")]
        public void DadoCoAutorDTOComNomeCaracteresAcentuados_QuandoAtribuir_EntaoDeveArmazenarCorretamente(string nome)
        {
            var dto = new CoAutorDTO { CreditoAutorNome = nome };

            dto.CreditoAutorNome.Should().Be(nome);
        }

        #endregion
    }
}
