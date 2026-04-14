using FluentAssertions;
using SME.CDEP.Dominio.Extensions;

namespace SME.CDEP.TesteUnitario.Domain.Extensions
{
    public class DateTimeExtensionTestes
    {
        [Fact]
        public void DadoData_QuandoLocal_EntaoRetornaMesmaData()
        {
            // Arrange
            var data = new DateTime(2026, 4, 13, 10, 0, 0, DateTimeKind.Local);

            // Act
            var resultado = data.Local();

            // Assert
            resultado.Should().Be(data);
        }

        [Fact]
        public void DadoChamada_QuandoHorarioBrasilia_EntaoRetornaDataHoraAtualMenosTresHoras()
        {
            // Arrange
            var dataHoraEsperada = DateTime.UtcNow.AddHours(-3);

            // Act
            var resultado = DateTimeExtension.HorarioBrasilia();

            // Assert
            resultado.Should().BeCloseTo(dataHoraEsperada, TimeSpan.FromSeconds(1));
        }

        [Theory]
        [InlineData(2026, 4, 15, 2026, 4, 12)] // Quarta-feira -> Retorna Domingo anterior
        [InlineData(2026, 4, 12, 2026, 4, 12)] // Domingo -> Retorna mesmo dia
        [InlineData(2026, 4, 13, 2026, 4, 12)] // Segunda-feira -> Retorna Domingo anterior
        public void DadoDataQualquer_QuandoObterDomingo_EntaoRetornaDomingoDaSemana(int ano, int mes, int dia, int anoEsp, int mesEsp, int diaEsp)
        {
            // Arrange
            var data = new DateTime(ano, mes, dia, 0, 0, 0, DateTimeKind.Local);
            var domingoEsperado = new DateTime(anoEsp, mesEsp, diaEsp, 0, 0, 0, DateTimeKind.Local);

            // Act
            var resultado = data.ObterDomingo();

            // Assert
            resultado.Date.Should().Be(domingoEsperado.Date);
        }

        [Theory]
        [InlineData(2026, 4, 15, 2026, 4, 18)] // Quarta-feira -> Retorna Sábado seguinte
        [InlineData(2026, 4, 18, 2026, 4, 18)] // Sábado -> Retorna mesmo dia
        [InlineData(2026, 4, 17, 2026, 4, 18)] // Sexta-feira -> Retorna Sábado seguinte
        public void DadoDataQualquer_QuandoObterSabado_EntaoRetornaSabadoDaSemana(int ano, int mes, int dia, int anoEsp, int mesEsp, int diaEsp)
        {
            // Arrange
            var data = new DateTime(ano, mes, dia, 0, 0, 0, DateTimeKind.Local);
            var sabadoEsperado = new DateTime(anoEsp, mesEsp, diaEsp, 0, 0, 0, DateTimeKind.Local);

            // Act
            var resultado = data.ObterSabado();

            // Assert
            resultado.Date.Should().Be(sabadoEsperado.Date);
        }

        [Theory]
        [InlineData(2026, 4, 18, true)]  // Sábado
        [InlineData(2026, 4, 19, true)]  // Domingo
        [InlineData(2026, 4, 15, false)] // Quarta-feira
        public void DadoData_QuandoFimDeSemana_EntaoRetornaResultadoEsperado(int ano, int mes, int dia, bool esperado)
        {
            // Arrange
            var data = new DateTime(ano, mes, dia, 0, 0, 0, DateTimeKind.Local);

            // Act
            var resultado = data.FimDeSemana();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData(2026, 4, 19, true)]  // Domingo
        [InlineData(2026, 4, 18, false)] // Sábado
        [InlineData(2026, 4, 15, false)] // Quarta-feira
        public void DadoData_QuandoDomingo_EntaoRetornaResultadoEsperado(int ano, int mes, int dia, bool esperado)
        {
            // Arrange
            var data = new DateTime(ano, mes, dia, 0, 0, 0, DateTimeKind.Local);

            // Act
            var resultado = data.Domingo();

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData(2026, 1, 15, 1)] // Janeiro -> Semestre 1
        [InlineData(2026, 6, 30, 1)] // Junho -> Semestre 1
        [InlineData(2026, 7, 1, 2)]  // Julho -> Semestre 2
        [InlineData(2026, 12, 31, 2)] // Dezembro -> Semestre 2
        public void DadoData_QuandoSemestre_EntaoRetornaSemestreCorreto(int ano, int mes, int dia, int semestreEsperado)
        {
            // Arrange
            var data = new DateTime(ano, mes, dia, 0, 0, 0, DateTimeKind.Local);

            // Act
            var resultado = data.Semestre();

            // Assert
            resultado.Should().Be(semestreEsperado);
        }

        [Theory]
        [InlineData(2026, 4, 15, 1, 2026, 4, 14)] // Quarta-feira, menos 1 dia util = Terça-feira
        [InlineData(2026, 4, 15, 2, 2026, 4, 13)] // Quarta-feira, menos 2 dias uteis = Segunda-feira
        [InlineData(2026, 4, 13, 2, 2026, 4, 9)]  // Segunda-feira, menos 2 dias uteis = Quinta-feira da semana anterior (pula FDS)
        public void DadoData_QuandoDiaRetroativo_EntaoRetornaDataPulandoFinaisDeSemana(int ano, int mes, int dia, int nrDias, int anoEsp, int mesEsp, int diaEsp)
        {
            // Arrange
            var data = new DateTime(ano, mes, dia, 0, 0, 0, DateTimeKind.Local);
            var dataEsperada = new DateTime(anoEsp, mesEsp, diaEsp, 0, 0, 0, DateTimeKind.Local);

            // Act
            var resultado = data.DiaRetroativo(nrDias);

            // Assert
            resultado.Should().Be(dataEsperada);
        }

        [Theory]
        [InlineData(2026, 4, 15, 2026, 4, 12)] // Quarta-feira -> Retorna Domingo anterior
        [InlineData(2026, 4, 12, 2026, 4, 12)] // Domingo -> Retorna mesmo dia
        [InlineData(2026, 4, 13, 2026, 4, 12)] // Segunda-feira -> Retorna Domingo anterior
        public void DadoData_QuandoObterDomingoRetroativo_EntaoRetornaDomingoCorreto(int ano, int mes, int dia, int anoEsp, int mesEsp, int diaEsp)
        {
            // Arrange
            var data = new DateTime(ano, mes, dia, 0, 0, 0, DateTimeKind.Local);
            var domingoEsperado = new DateTime(anoEsp, mesEsp, diaEsp, 0, 0, 0, DateTimeKind.Local);

            // Act
            var resultado = data.ObterDomingoRetroativo();

            // Assert
            resultado.Date.Should().Be(domingoEsperado.Date);
        }

        [Fact]
        public void DadoDatasNulaveis_QuandoVerificarEhMaiorOuIgualQue_EntaoRetornaFalsoSeUmaForNula()
        {
            // Arrange
            DateTime? dataNula = null;
            DateTime? dataValida = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Local);

            // Act & Assert
            dataNula.EhMaiorOuIgualQue(dataValida).Should().BeFalse();
            dataValida.EhMaiorOuIgualQue(dataNula).Should().BeFalse();
            dataNula.EhMaiorOuIgualQue(dataNula).Should().BeFalse();
        }

        [Theory]
        [InlineData(2026, 4, 15, 2026, 4, 14, true)]  // Avaliada > Referencia
        [InlineData(2026, 4, 15, 2026, 4, 15, true)]  // Avaliada == Referencia
        [InlineData(2026, 4, 15, 2026, 4, 16, false)] // Avaliada < Referencia
        public void DadoDatasValidas_QuandoEhMaiorOuIgualQue_EntaoRetornaResultadoEsperado(int anoAv, int mesAv, int diaAv, int anoRef, int mesRef, int diaRef, bool esperado)
        {
            // Arrange
            DateTime? dataAvaliada = new DateTime(anoAv, mesAv, diaAv, 0, 0, 0, DateTimeKind.Local);
            DateTime? dataReferencia = new DateTime(anoRef, mesRef, diaRef, 0, 0, 0, DateTimeKind.Local);

            // Act
            var resultado = dataAvaliada.EhMaiorOuIgualQue(dataReferencia);

            // Assert
            resultado.Should().Be(esperado);
        }

        [Theory]
        [InlineData(2026, 4, 14, 2026, 4, 15, true)]  // Avaliada < Referencia
        [InlineData(2026, 4, 15, 2026, 4, 15, false)] // Avaliada == Referencia
        [InlineData(2026, 4, 16, 2026, 4, 15, false)] // Avaliada > Referencia
        public void DadoDatasValidas_QuandoEhMenorQue_EntaoRetornaResultadoEsperado(int anoAv, int mesAv, int diaAv, int anoRef, int mesRef, int diaRef, bool esperado)
        {
            // Arrange
            DateTime? dataAvaliada = new DateTime(anoAv, mesAv, diaAv, 0, 0, 0, DateTimeKind.Local);
            DateTime? dataReferencia = new DateTime(anoRef, mesRef, diaRef, 0, 0, 0, DateTimeKind.Local);

            // Act
            var resultadoNulavel = dataAvaliada.EhMenorQue(dataReferencia);
            var resultadoNaoNulavel = dataAvaliada.Value.EhMenorQue(dataReferencia.Value);

            // Assert
            resultadoNulavel.Should().Be(esperado);
            resultadoNaoNulavel.Should().Be(esperado);
        }

        [Theory]
        [InlineData(2026, 4, 14, 2026, 4, 15, true)]  // Avaliada < Referencia
        [InlineData(2026, 4, 15, 2026, 4, 15, true)]  // Avaliada == Referencia
        [InlineData(2026, 4, 16, 2026, 4, 15, false)] // Avaliada > Referencia
        public void DadoDatasValidas_QuandoEhMenorIgualQue_EntaoRetornaResultadoEsperado(int anoAv, int mesAv, int diaAv, int anoRef, int mesRef, int diaRef, bool esperado)
        {
            // Arrange
            DateTime? dataAvaliada = new DateTime(anoAv, mesAv, diaAv, 0, 0, 0, DateTimeKind.Local);
            DateTime? dataReferencia = new DateTime(anoRef, mesRef, diaRef, 0, 0, 0, DateTimeKind.Local);

            // Act
            var resultadoNulavel = dataAvaliada.EhMenorIgualQue(dataReferencia);
            var resultadoNaoNulavel = dataAvaliada.Value.EhMenorIgualQue(dataReferencia.Value);

            // Assert
            resultadoNulavel.Should().Be(esperado);
            resultadoNaoNulavel.Should().Be(esperado);
        }

        [Fact]
        public void DadoDataNula_QuandoAvaliarSeEhDataFutura_EntaoRetornaFalsoParaAmbos()
        {
            // Arrange
            DateTime? dataNula = null;

            // Act
            var ehFutura = dataNula.EhDataFutura();
            var naoEhFutura = dataNula.NaoEhDataFutura();

            // Assert
            ehFutura.Should().BeFalse();
            naoEhFutura.Should().BeFalse();
        }

        [Fact]
        public void DadoData_QuandoEhDataFutura_EntaoAvaliaCorretamenteEmRelacaoABrasilia()
        {
            // Arrange
            DateTime dataFutura = DateTimeExtension.HorarioBrasilia().AddDays(1);
            DateTime dataPassada = DateTimeExtension.HorarioBrasilia().AddDays(-1);

            DateTime? dataFuturaNulavel = dataFutura;
            DateTime? dataPassadaNulavel = dataPassada;

            // Act & Assert
            dataFutura.EhDataFutura().Should().BeTrue();
            dataFutura.NaoEhDataFutura().Should().BeFalse();

            dataPassada.EhDataFutura().Should().BeFalse();
            dataPassada.NaoEhDataFutura().Should().BeTrue();

            dataFuturaNulavel.EhDataFutura().Should().BeTrue();
            dataPassadaNulavel.EhDataFutura().Should().BeFalse();
        }
    }
}