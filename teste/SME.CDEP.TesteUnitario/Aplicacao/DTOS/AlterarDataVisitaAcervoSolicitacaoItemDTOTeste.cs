using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AlterarDataVisitaAcervoSolicitacaoItemDTOTeste
    {
        [Fact]
        public void DadoAlterarDataVisitaAcervoSolicitacaoItemDTO_QuandoInstanciar_EntaoTodasAsPropriedadesSaoInicializadasCorretamente()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();

            dto.Should().NotBeNull();
            dto.Id.Should().Be(0);
            dto.DataVisita.Should().Be(default(DateTime));
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirIdLong_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();
            var faker = new Faker();
            var idValor = faker.Random.Long(1, 10000);

            dto.Id = idValor;

            dto.Id.Should().Be(idValor);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirDataVisita_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();
            var faker = new Faker();
            var dataVisita = faker.Date.Future();

            dto.DataVisita = dataVisita;

            dto.DataVisita.Should().Be(dataVisita);
        }

        [Fact]
        public void DadoMultiplosValores_QuandoAtribuirTodasAsPropriedades_EntaoTodosOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();
            var faker = new Faker();
            var id = faker.Random.Long(1, 10000);
            var dataVisita = faker.Date.Future();

            dto.Id = id;
            dto.DataVisita = dataVisita;

            dto.Id.Should().Be(id);
            dto.DataVisita.Should().Be(dataVisita);
        }

        [Fact]
        public void DadoIdZero_QuandoAtribuirPropriedadeId_EntaoOValorZeroEhArmazenado()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();

            dto.Id = 0;

            dto.Id.Should().Be(0);
        }

        [Fact]
        public void DadoIdNegativo_QuandoAtribuirPropriedadeId_EntaoOValorNegativoEhArmazenado()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();

            dto.Id = -1;

            dto.Id.Should().Be(-1);
        }

        [Fact]
        public void DadoIdMaximo_QuandoAtribuirPropriedadeId_EntaoOValorMaximoEhArmazenado()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();
            var idMaximo = long.MaxValue;

            dto.Id = idMaximo;

            dto.Id.Should().Be(idMaximo);
        }

        [Fact]
        public void DadoIdMinimo_QuandoAtribuirPropriedadeId_EntaoOValorMinimoEhArmazenado()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();
            var idMinimo = long.MinValue;

            dto.Id = idMinimo;

            dto.Id.Should().Be(idMinimo);
        }

        [Fact]
        public void DadoDataVisitaNoPassado_QuandoAtribuirPropriedadeDataVisita_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();
            var dataPassado = DateTime.Now.AddDays(-10);

            dto.DataVisita = dataPassado;

            dto.DataVisita.Should().Be(dataPassado);
        }

        [Fact]
        public void DadoDataVisitaNoFuturo_QuandoAtribuirPropriedadeDataVisita_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();
            var faker = new Faker();
            var dataFuturo = faker.Date.Future();

            dto.DataVisita = dataFuturo;

            dto.DataVisita.Should().Be(dataFuturo);
        }

        [Fact]
        public void DadoDataVisitaHoje_QuandoAtribuirPropriedadeDataVisita_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();
            var hoje = DateTime.Now.Date;

            dto.DataVisita = hoje;

            dto.DataVisita.Date.Should().Be(hoje);
        }

        [Fact]
        public void DadoDataVisitaMeiaNoite_QuandoAtribuirPropriedadeDataVisita_EntaoOValorComHoraEhArmazenadoCorretamente()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();
            var dataMeiaNoite = DateTime.Now.Date.AddHours(0).AddMinutes(0).AddSeconds(0);

            dto.DataVisita = dataMeiaNoite;

            dto.DataVisita.Should().Be(dataMeiaNoite);
        }

        [Fact]
        public void DadoDataVisitaComHora_QuandoAtribuirPropriedadeDataVisita_EntaoOValorComHoraEhArmazenadoCorretamente()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();
            var dataComHora = new DateTime(2025, 6, 15, 14, 30, 45);

            dto.DataVisita = dataComHora;

            dto.DataVisita.Should().Be(dataComHora);
            dto.DataVisita.Hour.Should().Be(14);
            dto.DataVisita.Minute.Should().Be(30);
            dto.DataVisita.Second.Should().Be(45);
        }

        [Fact]
        public void DadoDataVisitaMinima_QuandoAtribuirPropriedadeDataVisita_EntaoOValorMinimoEhArmazenadoCorretamente()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();
            var dataMinima = DateTime.MinValue;

            dto.DataVisita = dataMinima;

            dto.DataVisita.Should().Be(dataMinima);
        }

        [Fact]
        public void DadoDataVisitaMaxima_QuandoAtribuirPropriedadeDataVisita_EntaoOValorMaximoEhArmazenadoCorretamente()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();
            var dataMaxima = DateTime.MaxValue;

            dto.DataVisita = dataMaxima;

            dto.DataVisita.Should().Be(dataMaxima);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesMultiplaVezes_EntaoOUltimoValorEhRetido()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();
            var faker = new Faker();
            var id1 = faker.Random.Long(1, 100);
            var id2 = faker.Random.Long(101, 200);
            var data1 = faker.Date.Past();
            var data2 = faker.Date.Future();

            dto.Id = id1;
            dto.DataVisita = data1;

            dto.Id.Should().Be(id1);
            dto.DataVisita.Should().Be(data1);

            dto.Id = id2;
            dto.DataVisita = data2;

            dto.Id.Should().Be(id2);
            dto.DataVisita.Should().Be(data2);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesComInitializerSyntax_EntaoTodosOsValoresSaoArmazenadosCorretamente()
        {
            var faker = new Faker();
            var id = faker.Random.Long(1, 10000);
            var dataVisita = faker.Date.Future();

            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO
            {
                Id = id,
                DataVisita = dataVisita
            };

            dto.Id.Should().Be(id);
            dto.DataVisita.Should().Be(dataVisita);
        }

        [Fact]
        public void DadoDTO_QuandoAtribuirPropriedadesComValoresGrandes_EntaoOsValoresGrandesSaoArmazenadosCorretamente()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();
            var idGrande = 9223372036854775807; 

            dto.Id = idGrande;

            dto.Id.Should().Be(idGrande);
        }

        [Fact]
        public void DadoDataVisitaAnoAntigo_QuandoAtribuirPropriedadeDataVisita_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();
            var dataAntiga = new DateTime(1900, 1, 1);

            dto.DataVisita = dataAntiga;

            dto.DataVisita.Should().Be(dataAntiga);
            dto.DataVisita.Year.Should().Be(1900);
        }

        [Fact]
        public void DadoDataVisitaAnoDistante_QuandoAtribuirPropriedadeDataVisita_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new AlterarDataVisitaAcervoSolicitacaoItemDTO();
            var dataDistante = new DateTime(2999, 12, 31);

            dto.DataVisita = dataDistante;

            dto.DataVisita.Should().Be(dataDistante);
            dto.DataVisita.Year.Should().Be(2999);
        }
    }
}
