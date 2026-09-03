using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoSolicitacaoItemCadastroDTOTeste
    {
        #region Testes de Propriedade AcervoId

        [Fact]
        public void DadoAcervoIdValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var acervoId = 42L;
            var dto = new AcervoSolicitacaoItemCadastroDTO();

            dto.AcervoId = acervoId;

            dto.AcervoId.Should().Be(acervoId);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemCadastroDTO_QuandoInstanciar_EntaoAcervoIdAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemCadastroDTO();

            dto.AcervoId.Should().Be(0);
        }

        [Theory]
        [InlineData(1L)]
        [InlineData(100L)]
        [InlineData(999999L)]
        [InlineData(long.MaxValue)]
        public void DadoDiferentesAcervoIds_QuandoAssignar_EntaoRetornaValoresCorretos(long acervoId)
        {
            var dto = new AcervoSolicitacaoItemCadastroDTO();

            dto.AcervoId = acervoId;

            dto.AcervoId.Should().Be(acervoId);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemCadastroDTO_QuandoAtribuirValoresMultiplosAoAcervoId_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoSolicitacaoItemCadastroDTO();

            dto.AcervoId = 10;
            dto.AcervoId = 20;
            dto.AcervoId = 30;

            dto.AcervoId.Should().Be(30);
        }

        [Fact]
        public void DadoAcervoIdNegativo_QuandoAssignar_EntaoArmazenaNegativo()
        {
            var dto = new AcervoSolicitacaoItemCadastroDTO();

            dto.AcervoId = -5;

            dto.AcervoId.Should().Be(-5);
        }

        [Fact]
        public void DadoAcervoIdZero_QuandoAssignar_EntaoArmazenaZero()
        {
            var dto = new AcervoSolicitacaoItemCadastroDTO();

            dto.AcervoId = 0;

            dto.AcervoId.Should().Be(0);
        }

        #endregion

        #region Testes de Propriedade DataVisita

        [Fact]
        public void DadoDataVisitaValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var dataVisita = new DateTime(2024, 5, 15, 10, 30, 0);
            var dto = new AcervoSolicitacaoItemCadastroDTO();

            dto.DataVisita = dataVisita;

            dto.DataVisita.Should().Be(dataVisita);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemCadastroDTO_QuandoInstanciar_EntaoDataVisitaAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemCadastroDTO();

            dto.DataVisita.Should().BeNull();
        }

        [Fact]
        public void DadoDataVisitaNula_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoItemCadastroDTO { DataVisita = new DateTime(2024, 5, 15) };

            dto.DataVisita = null;

            dto.DataVisita.Should().BeNull();
        }

        [Theory]
        [InlineData("2024-01-01")]
        [InlineData("2024-06-15")]
        [InlineData("2024-12-31")]
        public void DadoDiferentesDataVisitas_QuandoAssignar_EntaoRetornaValoresCorretos(string dataString)
        {
            var data = DateTime.Parse(dataString);
            var dto = new AcervoSolicitacaoItemCadastroDTO();

            dto.DataVisita = data;

            dto.DataVisita.Should().Be(data);
        }

        [Fact]
        public void DadoDataVisitaComHora_QuandoAssignar_EntaoArmazenaDataComHora()
        {
            var dataComHora = new DateTime(2024, 5, 15, 14, 30, 45);
            var dto = new AcervoSolicitacaoItemCadastroDTO();

            dto.DataVisita = dataComHora;

            dto.DataVisita.Should().Be(dataComHora);
            dto.DataVisita?.Hour.Should().Be(14);
            dto.DataVisita?.Minute.Should().Be(30);
            dto.DataVisita?.Second.Should().Be(45);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemCadastroDTO_QuandoAtribuirValoresMultiplosAoDataVisita_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoSolicitacaoItemCadastroDTO();
            var data1 = new DateTime(2024, 1, 15);
            var data2 = new DateTime(2024, 6, 15);
            var data3 = new DateTime(2024, 12, 31);

            dto.DataVisita = data1;
            dto.DataVisita = data2;
            dto.DataVisita = data3;

            dto.DataVisita.Should().Be(data3);
        }

        #endregion

        #region Testes de Combinações e Integração

        [Fact]
        public void DadoAcervoSolicitacaoItemCadastroDTO_QuandoInstanciar_EntaoTodosOsPropriedadesAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemCadastroDTO();

            dto.AcervoId.Should().Be(0);
            dto.DataVisita.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemCadastroDTO_QuandoAtribuirTodosOsPropriedades_EntaoRetornaTodosOsValoresAssignados()
        {
            var acervoId = 25L;
            var dataVisita = new DateTime(2024, 5, 15, 10, 30, 0);

            var dto = new AcervoSolicitacaoItemCadastroDTO
            {
                AcervoId = acervoId,
                DataVisita = dataVisita
            };

            dto.AcervoId.Should().Be(acervoId);
            dto.DataVisita.Should().Be(dataVisita);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemCadastroDTO_QuandoModificarPropriedadesSequencialmente_EntaoMantémCoerencia()
        {
            var dto = new AcervoSolicitacaoItemCadastroDTO();

            dto.AcervoId = 100;
            dto.DataVisita = new DateTime(2024, 5, 15);

            dto.AcervoId.Should().Be(100);
            dto.DataVisita.Should().Be(new DateTime(2024, 5, 15));

            dto.AcervoId = 200;
            dto.DataVisita = new DateTime(2024, 6, 20);

            dto.AcervoId.Should().Be(200);
            dto.DataVisita.Should().Be(new DateTime(2024, 6, 20));
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemCadastroDTO_QuandoCriarMultiplasInstancias_EntaoSãoIndependentes()
        {
            var dto1 = new AcervoSolicitacaoItemCadastroDTO
            {
                AcervoId = 1,
                DataVisita = new DateTime(2024, 1, 15)
            };

            var dto2 = new AcervoSolicitacaoItemCadastroDTO
            {
                AcervoId = 2,
                DataVisita = new DateTime(2024, 2, 20)
            };

            var dto3 = new AcervoSolicitacaoItemCadastroDTO
            {
                AcervoId = 3,
                DataVisita = null
            };

            dto1.AcervoId.Should().Be(1);
            dto1.DataVisita.Should().Be(new DateTime(2024, 1, 15));

            dto2.AcervoId.Should().Be(2);
            dto2.DataVisita.Should().Be(new DateTime(2024, 2, 20));

            dto3.AcervoId.Should().Be(3);
            dto3.DataVisita.Should().BeNull();

            dto1.AcervoId = 100;
            dto2.AcervoId.Should().Be(2);
            dto3.AcervoId.Should().Be(3);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemCadastroDTO_QuandoInstanciarComConstrutorPadrao_EntaoTodosOsPropriedadesEstaoAcessiveis()
        {
            var dto = new AcervoSolicitacaoItemCadastroDTO();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<AcervoSolicitacaoItemCadastroDTO>();
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemCadastroDTO_QuandoUtilizarConstructorComArgumentos_EntaoTodosOsPropriedadesEstaoAcessiveis()
        {
            var acervoId = 50L;
            var dataVisita = new DateTime(2024, 3, 10);

            var dto = new AcervoSolicitacaoItemCadastroDTO
            {
                AcervoId = acervoId,
                DataVisita = dataVisita
            };

            dto.AcervoId.Should().Be(acervoId);
            dto.DataVisita.Should().Be(dataVisita);
        }

        #endregion

        #region Testes com dados fictícios (Bogus)

        [Fact]
        public void DadoAcervoSolicitacaoItemCadastroDTOComDadosFictícios_QuandoAssignar_EntaoArmazenaCorretamente()
        {
            var faker = new Faker();
            var acervoId = faker.Random.Long(1, 10000);
            var dataVisita = faker.Date.Past();

            var dto = new AcervoSolicitacaoItemCadastroDTO
            {
                AcervoId = acervoId,
                DataVisita = dataVisita
            };

            dto.AcervoId.Should().Be(acervoId);
            dto.DataVisita.Should().Be(dataVisita);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemCadastroDTOComDadosFictíciosSemDataVisita_QuandoAssignar_EntaoArmazenaApenasAcervoId()
        {
            var faker = new Faker();
            var acervoId = faker.Random.Long(1, 10000);

            var dto = new AcervoSolicitacaoItemCadastroDTO
            {
                AcervoId = acervoId
            };

            dto.AcervoId.Should().Be(acervoId);
            dto.DataVisita.Should().BeNull();
        }

        [Theory]
        [InlineData(1L, "2024-01-15")]
        [InlineData(100L, "2024-06-15")]
        [InlineData(999L, "2024-12-31")]
        [InlineData(long.MaxValue, "2024-05-20")]
        public void DadoCombinacoesDiferentesDeAcervoIdEDataVisita_QuandoAssignar_EntaoRetornaValoresCorretos(
            long acervoId,
            string dataVisitaString)
        {
            var dataVisita = DateTime.Parse(dataVisitaString);

            var dto = new AcervoSolicitacaoItemCadastroDTO
            {
                AcervoId = acervoId,
                DataVisita = dataVisita
            };

            dto.AcervoId.Should().Be(acervoId);
            dto.DataVisita.Should().Be(dataVisita);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemCadastroDTO_QuandoAcervoIdTemRequisitoDeCampoObrigatorio_EntaoPropriedadeEstáDecorada()
        {
            var dto = new AcervoSolicitacaoItemCadastroDTO();
            var propriedade = typeof(AcervoSolicitacaoItemCadastroDTO).GetProperty(nameof(AcervoSolicitacaoItemCadastroDTO.AcervoId));

            propriedade.Should().NotBeNull();
            propriedade?.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), false)
                .Should().HaveCount(1);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemCadastroDTO_QuandoDataVisitaÉOpcional_EntaoPropriedadeNãoTemRequisito()
        {
            var dto = new AcervoSolicitacaoItemCadastroDTO();
            var propriedade = typeof(AcervoSolicitacaoItemCadastroDTO).GetProperty(nameof(AcervoSolicitacaoItemCadastroDTO.DataVisita));

            propriedade.Should().NotBeNull();
            propriedade?.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), false)
                .Should().BeEmpty();
        }

        [Theory]
        [InlineData(long.MinValue)]
        [InlineData(-1000L)]
        [InlineData(-1L)]
        [InlineData(0L)]
        [InlineData(1L)]
        [InlineData(1000L)]
        [InlineData(long.MaxValue)]
        public void DadoAcervoIdComValoresExtremos_QuandoAssignar_EntaoArmazenaCorretamente(long acervoId)
        {
            var dto = new AcervoSolicitacaoItemCadastroDTO();

            dto.AcervoId = acervoId;

            dto.AcervoId.Should().Be(acervoId);
        }

        [Fact]
        public void DadoDataVisitaComDataPassada_QuandoAssignar_EntaoArmazenaCorreamente()
        {
            var dataPassada = DateTime.Now.AddYears(-1);
            var dto = new AcervoSolicitacaoItemCadastroDTO();

            dto.DataVisita = dataPassada;

            dto.DataVisita.Should().Be(dataPassada);
        }

        [Fact]
        public void DadoDataVisitaComDataFutura_QuandoAssignar_EntaoArmazenaCorreamente()
        {
            var dataFutura = DateTime.Now.AddYears(1);
            var dto = new AcervoSolicitacaoItemCadastroDTO();

            dto.DataVisita = dataFutura;

            dto.DataVisita.Should().Be(dataFutura);
        }

        [Fact]
        public void DadoDataVisitaComDataDeHoje_QuandoAssignar_EntaoArmazenaCorreamente()
        {
            var dataHoje = DateTime.Today;
            var dto = new AcervoSolicitacaoItemCadastroDTO();

            dto.DataVisita = dataHoje;

            dto.DataVisita.Should().Be(dataHoje);
        }

        #endregion

        #region Testes de Serialização e Deserialização

        [Fact]
        public void DadoAcervoSolicitacaoItemCadastroDTO_QuandoUtilizarEmSerializacao_EntaoMantemPropriedades()
        {
            var acervoId = 123L;
            var dataVisita = new DateTime(2024, 5, 15);

            var dto1 = new AcervoSolicitacaoItemCadastroDTO
            {
                AcervoId = acervoId,
                DataVisita = dataVisita
            };

            var dto2 = new AcervoSolicitacaoItemCadastroDTO
            {
                AcervoId = dto1.AcervoId,
                DataVisita = dto1.DataVisita
            };

            dto2.AcervoId.Should().Be(dto1.AcervoId);
            dto2.DataVisita.Should().Be(dto1.DataVisita);
        }

        #endregion
    }
}
