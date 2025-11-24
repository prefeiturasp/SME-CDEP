using Dapper;
using Shouldly;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao.PainelGerencial
{
    public class QuantidadeDePesquisasMensaisTestes(CollectionFixture collectionFixture) : TesteBase(collectionFixture)
    {
        [Fact]
        public async Task AoObterQuantidadeDePesquisasMensaisDeveRetornarDadosCorretamente()
        {
            // Arrange
            await InserirSumarioConsultasMensais();
            var servicoPainelGerencial = GetServicoPainelGerencial();
            // Act
            var resposta = await servicoPainelGerencial.ObterQuantidadePesquisasMensaisDoAnoAtualAsync();
            // Assert
            resposta.Count.ShouldBe(12);
            var respostaPrimeiroMes = resposta[0];
            respostaPrimeiroMes.Id.ShouldBe(1);
            respostaPrimeiroMes.Nome.ShouldBe("Janeiro");
            respostaPrimeiroMes.Valor.ShouldBe(100);

            var respostaSegundoMes = resposta[1];
            respostaSegundoMes.Id.ShouldBe(2);
            respostaSegundoMes.Nome.ShouldBe("Fevereiro");
            respostaSegundoMes.Valor.ShouldBe(200);

            var respostaTerceiroMes = resposta[2];
            respostaTerceiroMes.Id.ShouldBe(3);
            respostaTerceiroMes.Nome.ShouldBe("Março");
            respostaTerceiroMes.Valor.ShouldBe(300);

            var respostaQuartoMes = resposta[3];
            respostaQuartoMes.Id.ShouldBe(4);
            respostaQuartoMes.Nome.ShouldBe("Abril");
            respostaQuartoMes.Valor.ShouldBe(400);

            var respostaQuintoMes = resposta[4];
            respostaQuintoMes.Id.ShouldBe(5);
            respostaQuintoMes.Nome.ShouldBe("Maio");
            respostaQuintoMes.Valor.ShouldBe(500);

            var respostaSextoMes = resposta[5];
            respostaSextoMes.Id.ShouldBe(6);
            respostaSextoMes.Nome.ShouldBe("Junho");
            respostaSextoMes.Valor.ShouldBe(600);

            var respostaSetimoMes = resposta[6];
            respostaSetimoMes.Id.ShouldBe(7);
            respostaSetimoMes.Nome.ShouldBe("Julho");
            respostaSetimoMes.Valor.ShouldBe(700);

            var respostaOitavoMes = resposta[7];
            respostaOitavoMes.Id.ShouldBe(8);
            respostaOitavoMes.Nome.ShouldBe("Agosto");
            respostaOitavoMes.Valor.ShouldBe(800);

            var respostaNonoMes = resposta[8];
            respostaNonoMes.Id.ShouldBe(9);
            respostaNonoMes.Nome.ShouldBe("Setembro");
            respostaNonoMes.Valor.ShouldBe(900);

            var respostaDecimoMes = resposta[9];
            respostaDecimoMes.Id.ShouldBe(10);
            respostaDecimoMes.Nome.ShouldBe("Outubro");
            respostaDecimoMes.Valor.ShouldBe(1000);

            var respostaDecimoPrimeiroMes = resposta[10];
            respostaDecimoPrimeiroMes.Id.ShouldBe(11);
            respostaDecimoPrimeiroMes.Nome.ShouldBe("Novembro");
            respostaDecimoPrimeiroMes.Valor.ShouldBe(1100);

            var respostaDecimoSegundoMes = resposta[11];
            respostaDecimoSegundoMes.Id.ShouldBe(12);
            respostaDecimoSegundoMes.Nome.ShouldBe("Dezembro");
            respostaDecimoSegundoMes.Valor.ShouldBe(1200);

            // Verifica se os dados do ano anterior não estão sendo retornados
            foreach (var item in resposta)
            {
                item.Valor.ShouldNotBe(50);
            }
        }

        private async Task InserirSumarioConsultasMensais()
        {
            var anoAtual = DateTime.Now.Year;
            for (int mes = 1; mes <= 12; mes++)
            {
                var sumario = new SumarioConsultaMensal
                {
                    MesReferencia = new DateOnly(anoAtual, mes, 1),
                    TotalConsultas = mes * 100,
                    DataUltimaAtualizacao = DateTime.UtcNow
                };
                await InserirNaBase(sumario);
            }
            for (int mes = 1; mes <= 12; mes++)
            {
                var sumario = new SumarioConsultaMensal
                {
                    MesReferencia = new DateOnly(anoAtual - 1, mes, 1),
                    TotalConsultas = 50,
                    DataUltimaAtualizacao = DateTime.UtcNow
                };
                await InserirNaBase(sumario);
            }
        }

        private async Task InserirNaBase(SumarioConsultaMensal sumario)
        {
            const string insertQuery = @"
                INSERT INTO sumario_consultas_mensal (mes_referencia, total_consultas, data_ultima_atualizacao)
                VALUES (@MesReferencia, @TotalConsultas, @DataUltimaAtualizacao)";

            var parameters = new
            {
                sumario.MesReferencia,
                sumario.TotalConsultas,
                sumario.DataUltimaAtualizacao
            };

            await _collectionFixture.Database.Conexao.ExecuteAsync(insertQuery, parameters);
        }
    }
}