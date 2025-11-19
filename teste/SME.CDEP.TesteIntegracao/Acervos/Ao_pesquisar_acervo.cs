using Bogus;
using Dapper;
using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao.Acervos
{
    public class Ao_pesquisar_acervo : TesteBase
    {
        private readonly IServicoAcervo _servicoAcervo;
        private readonly Faker _faker;

        public Ao_pesquisar_acervo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            _servicoAcervo = GetServicoAcervo();
            _faker = new Faker();
        }

        [Fact(DisplayName = "Acervo - Pesquisar por texto livre quando nao tiver retorno")]
        public async Task Pesquisar_por_texto_livre_quando_nao_tiver_retorno()
        {
            // Arrange
            await InserirDadosBasicosAleatorios();
            var quantidadeInicial = await _collectionFixture.Database.Conexao.QueryFirstAsync<int>("SELECT COUNT(1) FROM historico_consultas_acervos");
            var filtro = new FiltroTextoLivreTipoAcervoDTO
            {
                AnoFinal = _faker.Random.Int(2000, 2025),
                AnoInicial = _faker.Random.Int(1975, 2000),
                TextoLivre = _faker.Lorem.Word(),
                TipoAcervo = _faker.PickRandom<TipoAcervo>()
            };

            // Act
            var resultado = await _servicoAcervo.ObterPorTextoLivreETipoAcervo(filtro);

            // Assert
            var historicoConsultasAcervos = await _collectionFixture.Database.Conexao.QueryAsync<HistoricoConsultaAcervo>("SELECT * FROM historico_consultas_acervos");
            historicoConsultasAcervos.FirstOrDefault(h => h.TermoPesquisado == filtro.TextoLivre && h.TipoAcervo == filtro.TipoAcervo).ShouldNotBeNull();
        }
    }
}