using Bogus;
using Dapper;
using Shouldly;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao.Consolidacoes
{
    public class Ao_executar_consolidacao : TesteBase
    {
        private readonly IServicoDeConsolidacao servicoDeConsolidacao;
        private readonly Faker faker;

        public Ao_executar_consolidacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            servicoDeConsolidacao = GetServicoDeConsolidacao();
            faker = new Faker();
        }

        [Fact(DisplayName = "Consolidação - Consolidar mes do historico de consultas do dia anterior")]
        public async Task Consolidar_mes_do_historico_de_consultas_do_dia_anterior()
        {
            // Act
            await InserirDadosBasicosAleatorios();
            await _collectionFixture.Database.Conexao.ExecuteAsync("TRUNCATE TABLE historico_consultas_acervos");

            var dataOntem = DateTime.UtcNow.AddDays(-1);
            var limiteInicial = new DateTime(dataOntem.Year, dataOntem.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var limiteFinal = dataOntem;
            var diasDiferenca = (dataOntem - limiteInicial).Days;

            var historicoConsultasAcervos = new List<HistoricoConsultaAcervo>
            {
                new ()
                {
                    TermoPesquisado = "Teste 1",
                    DataConsulta = limiteInicial.AddDays(faker.Random.Int(0, diasDiferenca)),
                    QuantidadeResultados = 10,
                    TipoAcervo = TipoAcervo.ArtesGraficas
                },
                new ()
                {
                    TermoPesquisado = "Teste 1",
                    DataConsulta = limiteInicial.AddDays(faker.Random.Int(0, diasDiferenca)),
                    QuantidadeResultados = 10,
                    TipoAcervo = TipoAcervo.Fotografico
                },
                new ()
                {
                    TermoPesquisado = "Teste 2",
                    DataConsulta = limiteInicial.AddDays(faker.Random.Int(0, diasDiferenca)),
                    QuantidadeResultados = 5,
                    TipoAcervo = TipoAcervo.ArtesGraficas
                },
                new ()
                {
                    TermoPesquisado = "Teste 2",
                    DataConsulta = limiteInicial.AddDays(faker.Random.Int(0, diasDiferenca)),
                    QuantidadeResultados = 15,
                    TipoAcervo = TipoAcervo.ArtesGraficas
                },
                new ()
                {
                    TermoPesquisado = "Teste 3",
                    DataConsulta = limiteInicial.AddDays(-1),
                    QuantidadeResultados = 20,
                    TipoAcervo = TipoAcervo.Fotografico
                }
            };

            foreach (var historico in historicoConsultasAcervos)
                await InserirNaBase(historico);

            // Act
            await servicoDeConsolidacao.ConsolidarMesDoHistoricoDeConsultasDoDiaAnteriorAsync();

            // Assert
            var resultadosConsolidados = await _collectionFixture.Database.Conexao.QueryAsync<SumarioConsultasMensalTeste>(
                @"SELECT mes_referencia MesReferencia, total_consultas TotalConsultas FROM sumario_consultas_mensal");

            resultadosConsolidados.Count().ShouldBe(1);
            resultadosConsolidados.ToList().FirstOrDefault()!.MesReferencia.ShouldBe(new DateTime(dataOntem.Year, dataOntem.Month, 1));
            resultadosConsolidados.ToList().FirstOrDefault()!.TotalConsultas.ShouldBe(4);
        }

        private class SumarioConsultasMensalTeste
        {
            public DateTime MesReferencia { get; set; }
            public int TotalConsultas { get; set; }
        }
    }
}