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
        private readonly Faker _faker;

        public Ao_executar_consolidacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            servicoDeConsolidacao = GetServicoDeConsolidacao();
            _faker = new Faker();
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
                    DataConsulta = limiteInicial.AddDays(_faker.Random.Int(0, diasDiferenca)),
                    QuantidadeResultados = 10,
                    TipoAcervo = TipoAcervo.ArtesGraficas
                },
                new ()
                {
                    TermoPesquisado = "Teste 1",
                    DataConsulta = limiteInicial.AddDays(_faker.Random.Int(0, diasDiferenca)),
                    QuantidadeResultados = 10,
                    TipoAcervo = TipoAcervo.Fotografico
                },
                new ()
                {
                    TermoPesquisado = "Teste 2",
                    DataConsulta = limiteInicial.AddDays(_faker.Random.Int(0, diasDiferenca)),
                    QuantidadeResultados = 5,
                    TipoAcervo = TipoAcervo.ArtesGraficas
                },
                new ()
                {
                    TermoPesquisado = "Teste 2",
                    DataConsulta = limiteInicial.AddDays(_faker.Random.Int(0, diasDiferenca)),
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

        [Fact(DisplayName = "Consolidação - Não consolidar quando não houver dados no historico de consultas do dia anterior")]
        public async Task Nao_consolidar_quando_nao_houver_dados_no_historico_de_consultas_do_dia_anterior()
        {
            // Act
            await InserirDadosBasicosAleatorios();
            await _collectionFixture.Database.Conexao.ExecuteAsync("TRUNCATE TABLE historico_consultas_acervos");
            await _collectionFixture.Database.Conexao.ExecuteAsync("TRUNCATE TABLE sumario_consultas_mensal");
            // Act
            await servicoDeConsolidacao.ConsolidarMesDoHistoricoDeConsultasDoDiaAnteriorAsync();
            // Assert
            var resultadosConsolidados = await _collectionFixture.Database.Conexao.QueryAsync<SumarioConsultasMensalTeste>(
                @"SELECT mes_referencia MesReferencia, total_consultas TotalConsultas FROM sumario_consultas_mensal");
            resultadosConsolidados.Count().ShouldBe(0);
        }

        [Fact(DisplayName = "Consolidação - Consolidar mês das solicitações de acervos do dia anterior")]
        public async Task Consolidar_mes_das_solicitacoes_de_acervos_do_dia_anterior()
        {
            // Act
            _collectionFixture.Database.LimparBase();
            await InserirDadosBasicosAleatorios();
            await InserirAcervoTridimensional();
            var (inicioMes, fimMes) = ObterIntervaloDoMesDeOntem();
            var quantidadeDeDias = fimMes.Subtract(inicioMes).Days;

            for (int i = 0; i < quantidadeDeDias; i++)
            {
                await InserirAcervoSolicitacao(ObterAcervoSolicitacao(inicioMes.AddDays(i)));
            }
            await InserirAcervoSolicitacao(ObterAcervoSolicitacao(fimMes)); // Fora do período
            await InserirAcervoSolicitacao(ObterAcervoSolicitacao(inicioMes.AddDays(-1))); // Fora do período

            // Act
            await servicoDeConsolidacao.ConsolidarMesDasSolicitacoesDeAcervosDoDiaAnteriorAsync();

            // Assert
            var resultadosConsolidados = await _collectionFixture.Database.Conexao.QueryAsync<SumarioConsultasMensalTeste>(
                @"SELECT mes_referencia MesReferencia, total_solicitacoes TotalConsultas FROM sumario_solicitacoes_mensal order by MesReferencia");

            resultadosConsolidados.Count().ShouldBe(1);
            resultadosConsolidados.ToList().FirstOrDefault()!.MesReferencia.ShouldBe(inicioMes);
            resultadosConsolidados.ToList().FirstOrDefault()!.TotalConsultas.ShouldBe(quantidadeDeDias*3);
        }

        [Fact(DisplayName = "Consolidação - Deve incrementar total quando mes de solicitacao de acervo ja consolidado")]
        public async Task Deve_incrementar_total_quando_mes_de_solicitacao_de_acervo_ja_consolidado()
        {
            // Act
            _collectionFixture.Database.LimparBase();
            await InserirDadosBasicosAleatorios();
            await InserirAcervoTridimensional();
            var (inicioMes, fimMes) = ObterIntervaloDoMesDeOntem();
            var quantidadeDeDias = fimMes.Subtract(inicioMes).Days;
            for (int i = 0; i < quantidadeDeDias; i++)
            {
                await InserirAcervoSolicitacao(ObterAcervoSolicitacao(inicioMes.AddDays(i)));
            }
            // Primeira consolidação
            await servicoDeConsolidacao.ConsolidarMesDasSolicitacoesDeAcervosDoDiaAnteriorAsync();
            // Inserir mais solicitações no mesmo mês
            for (int i = 0; i < quantidadeDeDias; i++)
            {
                await InserirAcervoSolicitacao(ObterAcervoSolicitacao(inicioMes.AddDays(i)));
            }
            // Segunda consolidação
            await servicoDeConsolidacao.ConsolidarMesDasSolicitacoesDeAcervosDoDiaAnteriorAsync();
            // Assert
            var resultadosConsolidados = await _collectionFixture.Database.Conexao.QueryAsync<SumarioConsultasMensalTeste>(
                @"SELECT mes_referencia MesReferencia, total_solicitacoes TotalConsultas FROM sumario_solicitacoes_mensal order by MesReferencia");
            resultadosConsolidados.Count().ShouldBe(1);
            resultadosConsolidados.ToList().FirstOrDefault()!.MesReferencia.ShouldBe(inicioMes);
            resultadosConsolidados.ToList().FirstOrDefault()!.TotalConsultas.ShouldBe(quantidadeDeDias * 6);
        }

        private static (DateTime Inicio, DateTime Fim) ObterIntervaloDoMesDeOntem()
        {
            var dataOntem = DateTime.UtcNow.AddDays(-1);
            var inicioMes = new DateTime(dataOntem.Year, dataOntem.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var fimMes = inicioMes.AddMonths(1);
            return (inicioMes, fimMes);
        }

        private class SumarioConsultasMensalTeste
        {
            public DateTime MesReferencia { get; set; }
            public int TotalConsultas { get; set; }
        }
    }
}