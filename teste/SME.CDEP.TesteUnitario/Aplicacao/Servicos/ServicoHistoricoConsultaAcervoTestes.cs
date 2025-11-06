using AutoMapper;
using Bogus;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.Servicos
{
    public class ServicoHistoricoConsultaAcervoTestes
    {
        private readonly ServicoHistoricoConsultaAcervo _servicoHistoricoConsultaAcervo;
        private readonly Mock<IRepositorioHistoricoConsultaAcervo> _repositorioHistoricoConsultaAcervoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Faker _faker;

        public ServicoHistoricoConsultaAcervoTestes()
        {
            var mocker = new AutoMocker();
            _faker = new();
            _repositorioHistoricoConsultaAcervoMock = mocker.GetMock<IRepositorioHistoricoConsultaAcervo>();
            _mapperMock = mocker.GetMock<IMapper>();
            _servicoHistoricoConsultaAcervo = mocker.CreateInstance<ServicoHistoricoConsultaAcervo>();
        }

        [Fact]
        public async Task DadoUmHistoricoConsultaAcervoDto_QuandoInserirAsync_EntaoDeveRetornarHistoricoConsultaAcervoDtoInserido()
        {
            // Arrange
            var historicoConsultaAcervoDto = new HistoricoConsultaAcervoDto
            {
                AnoFinal = (short?)_faker.Random.Int(2000, 2024),
                AnoInicial = (short?)_faker.Random.Int(1980, 1999),
                DataConsulta = _faker.Date.Recent(),
                Id = 0,
                QuantidadeResultados = _faker.Random.Int(1, 100),
                TermoPesquisado = _faker.Lorem.Word(),
                TipoAcervo = _faker.PickRandom<TipoAcervo>(),
            };
            var historicoConsultaAcervoEntity = new HistoricoConsultaAcervo
            {
                AnoFinal = historicoConsultaAcervoDto.AnoFinal,
                AnoInicial = historicoConsultaAcervoDto.AnoInicial,
                DataConsulta = historicoConsultaAcervoDto.DataConsulta,
                Id = _faker.Random.Long(1, 1000),
                QuantidadeResultados = historicoConsultaAcervoDto.QuantidadeResultados,
                TermoPesquisado = historicoConsultaAcervoDto.TermoPesquisado,
                TipoAcervo = historicoConsultaAcervoDto.TipoAcervo
            };
            _mapperMock.Setup(m => m.Map<HistoricoConsultaAcervo>(historicoConsultaAcervoDto))
                .Returns(historicoConsultaAcervoEntity);
            _repositorioHistoricoConsultaAcervoMock.Setup(r => r.Inserir(historicoConsultaAcervoEntity))
                .ReturnsAsync(historicoConsultaAcervoEntity.Id);
            _mapperMock.Setup(m => m.Map<HistoricoConsultaAcervoDto>(historicoConsultaAcervoEntity))
                .Returns(historicoConsultaAcervoDto);

            // Act
            var resultado = await _servicoHistoricoConsultaAcervo.InserirAsync(historicoConsultaAcervoDto);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEquivalentTo(historicoConsultaAcervoDto);
            _repositorioHistoricoConsultaAcervoMock.Verify(r => r.Inserir(historicoConsultaAcervoEntity), Times.Once);
        }
    }
}
