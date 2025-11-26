using Dapper;
using Shouldly;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao.PainelGerencial
{
    public class QuantidadeDeSolicitacoesMensaisTestes(CollectionFixture collectionFixture) : TesteBase(collectionFixture)
    {
        [Fact]
        public async Task AoObterQuantidadeDeSolicitacoesMensaisDeveRetornarDadosCorretamente()
        {
            // Arrange
            await InserirSumarioSolicitacoesMensais();
            var servicoPainelGerencial = GetServicoPainelGerencial();
            // Act
            var resposta = await servicoPainelGerencial.ObterQuantidadeSolicitacoesMensaisDoAnoAtualAsync();
            // Assert
            resposta.Count.ShouldBe(12);
            var respostaPrimeiroMes = resposta[0];
            respostaPrimeiroMes.Id.ShouldBe(1);
            respostaPrimeiroMes.Nome.ShouldBe("Janeiro");
            respostaPrimeiroMes.Valor.ShouldBe(150);
            var respostaSegundoMes = resposta[1];
            respostaSegundoMes.Id.ShouldBe(2);
            respostaSegundoMes.Nome.ShouldBe("Fevereiro");
            respostaSegundoMes.Valor.ShouldBe(250);
            var respostaTerceiroMes = resposta[2];
            respostaTerceiroMes.Id.ShouldBe(3);
            respostaTerceiroMes.Nome.ShouldBe("Março");
            respostaTerceiroMes.Valor.ShouldBe(350);
            var respostaQuartoMes = resposta[3];
            respostaQuartoMes.Id.ShouldBe(4);
            respostaQuartoMes.Nome.ShouldBe("Abril");
            respostaQuartoMes.Valor.ShouldBe(450);
            var respostaQuintoMes = resposta[4];
            respostaQuintoMes.Id.ShouldBe(5);
            respostaQuintoMes.Nome.ShouldBe("Maio");
            respostaQuintoMes.Valor.ShouldBe(550);
            var respostaSextoMes = resposta[5];
            respostaSextoMes.Id.ShouldBe(6);
            respostaSextoMes.Nome.ShouldBe("Junho");
            respostaSextoMes.Valor.ShouldBe(650);
            var respostaSetimoMes = resposta[6];
            respostaSetimoMes.Id.ShouldBe(7);
            respostaSetimoMes.Nome.ShouldBe("Julho");
            respostaSetimoMes.Valor.ShouldBe(750);
            var respostaOitavoMes = resposta[7];
            respostaOitavoMes.Id.ShouldBe(8);
            respostaOitavoMes.Nome.ShouldBe("Agosto");
            respostaOitavoMes.Valor.ShouldBe(850);
            var respostaNonoMes = resposta[8];
            respostaNonoMes.Id.ShouldBe(9);
            respostaNonoMes.Nome.ShouldBe("Setembro");
            respostaNonoMes.Valor.ShouldBe(950);
            var respostaDecimoMes = resposta[9];
            respostaDecimoMes.Id.ShouldBe(10);
            respostaDecimoMes.Nome.ShouldBe("Outubro");
            respostaDecimoMes.Valor.ShouldBe(1050);
            var respostaDecimoPrimeiroMes = resposta[10];
            respostaDecimoPrimeiroMes.Id.ShouldBe(11);
            respostaDecimoPrimeiroMes.Nome.ShouldBe("Novembro");
            respostaDecimoPrimeiroMes.Valor.ShouldBe(1150);
            var respostaDecimoSegundoMes = resposta[11];
            respostaDecimoSegundoMes.Id.ShouldBe(12);
            respostaDecimoSegundoMes.Nome.ShouldBe("Dezembro");
            respostaDecimoSegundoMes.Valor.ShouldBe(1250);

            // Verifica se os dados do ano anterior não estão sendo retornados
            foreach (var item in resposta)
            {
                item.Valor.ShouldNotBe(9999); // Valor do ano anterior inserido na base de teste
            }
        }

        private async Task InserirSumarioSolicitacoesMensais()
        {
            var anoAtual = DateTime.Now.Year;
            var anoAnterior = anoAtual - 1;
            for (int mes = 1; mes <= 12; mes++)
            {
                var sumario = new SumarioSolicitacaoMensal
                {
                    MesReferencia = DateOnly.FromDateTime(new DateTime(anoAtual, mes, 1)),
                    TotalSolicitacoes = mes * 100 + 50,
                    DataUltimaAtualizacao = DateTime.UtcNow
                };
                await InserirNaBase(sumario);
            }
            var sumarioAnoAnterior = new SumarioSolicitacaoMensal
            {
                MesReferencia = DateOnly.FromDateTime(new DateTime(anoAnterior, 6, 1)),
                TotalSolicitacoes = 9999,
                DataUltimaAtualizacao = DateTime.UtcNow
            };
            await InserirNaBase(sumarioAnoAnterior);
        }

        private async Task InserirNaBase(SumarioSolicitacaoMensal sumario)
        {
            const string insertSql = @"
                INSERT INTO sumario_solicitacoes_mensal (mes_referencia, total_solicitacoes, data_ultima_atualizacao)
                VALUES (@MesReferencia, @TotalSolicitacoes, @DataUltimaAtualizacao);";

            var parameters = new
            {
                sumario.MesReferencia,
                sumario.TotalSolicitacoes,
                sumario.DataUltimaAtualizacao
            };

            await _collectionFixture.Database.Conexao.ExecuteAsync(insertSql, parameters);
        }
    }
}
