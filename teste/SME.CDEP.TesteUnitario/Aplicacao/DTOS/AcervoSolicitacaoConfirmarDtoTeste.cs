using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;
using Xunit;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoSolicitacaoConfirmarDtoTeste
    {
        #region Testes de Propriedade Id

        [Fact]
        public void DadoIdValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var id = 42L;
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.Id = id;

            dto.Id.Should().Be(id);
        }

        [Fact]
        public void DadoAcervoSolicitacaoConfirmarDto_QuandoInstanciar_EntaoIdAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.Id.Should().Be(0);
        }

        [Theory]
        [InlineData(1L)]
        [InlineData(100L)]
        [InlineData(999999L)]
        [InlineData(long.MaxValue)]
        public void DadoDiferentesIds_QuandoAssignar_EntaoRetornaValoresCorretos(long id)
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.Id = id;

            dto.Id.Should().Be(id);
        }

        [Fact]
        public void DadoAcervoSolicitacaoConfirmarDto_QuandoAtribuirValoresMultiplosAoId_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.Id = 10;
            dto.Id = 20;
            dto.Id = 30;

            dto.Id.Should().Be(30);
        }

        #endregion

        #region Testes de Propriedade ItemId

        [Fact]
        public void DadoItemIdValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var itemId = 123L;
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.ItemId = itemId;

            dto.ItemId.Should().Be(itemId);
        }

        [Fact]
        public void DadoAcervoSolicitacaoConfirmarDto_QuandoInstanciar_EntaoItemIdAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.ItemId.Should().Be(0);
        }

        [Theory]
        [InlineData(1L)]
        [InlineData(100L)]
        [InlineData(999999L)]
        [InlineData(long.MaxValue)]
        public void DadoDiferentesItemIds_QuandoAssignar_EntaoRetornaValoresCorretos(long itemId)
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.ItemId = itemId;

            dto.ItemId.Should().Be(itemId);
        }

        [Fact]
        public void DadoAcervoSolicitacaoConfirmarDto_QuandoAtribuirValoresMultiplosAoItemId_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.ItemId = 10;
            dto.ItemId = 20;
            dto.ItemId = 30;

            dto.ItemId.Should().Be(30);
        }

        #endregion

        #region Testes de Propriedade DataVisita (herança)

        [Fact]
        public void DadoDataVisitaValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var dataVisita = new DateTime(2024, 5, 15, 10, 30, 0);
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.DataVisita = dataVisita;

            dto.DataVisita.Should().Be(dataVisita);
        }

        [Fact]
        public void DadoAcervoSolicitacaoConfirmarDto_QuandoInstanciar_EntaoDataVisitaAssumeNulo()
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.DataVisita.Should().BeNull();
        }

        [Fact]
        public void DadoDataVisitaNula_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoConfirmarDto { DataVisita = new DateTime(2024, 5, 15) };

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
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.DataVisita = data;

            dto.DataVisita.Should().Be(data);
        }

        #endregion

        #region Testes de Propriedade DataEmprestimo (herança)

        [Fact]
        public void DadoDataEmprestimoValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var dataEmprestimo = new DateTime(2024, 5, 10, 09, 00, 0);
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.DataEmprestimo = dataEmprestimo;

            dto.DataEmprestimo.Should().Be(dataEmprestimo);
        }

        [Fact]
        public void DadoAcervoSolicitacaoConfirmarDto_QuandoInstanciar_EntaoDataEmprestimoAssumeNulo()
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.DataEmprestimo.Should().BeNull();
        }

        [Fact]
        public void DadoDataEmprestimoNula_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoConfirmarDto { DataEmprestimo = new DateTime(2024, 5, 10) };

            dto.DataEmprestimo = null;

            dto.DataEmprestimo.Should().BeNull();
        }

        [Theory]
        [InlineData("2024-01-10")]
        [InlineData("2024-06-10")]
        [InlineData("2024-12-10")]
        public void DadoDiferentesDataEmprestimos_QuandoAssignar_EntaoRetornaValoresCorretos(string dataString)
        {
            var data = DateTime.Parse(dataString);
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.DataEmprestimo = data;

            dto.DataEmprestimo.Should().Be(data);
        }

        #endregion

        #region Testes de Propriedade DataDevolucao (herança)

        [Fact]
        public void DadoDataDevolucaoValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var dataDevolucao = new DateTime(2024, 6, 10, 14, 30, 0);
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.DataDevolucao = dataDevolucao;

            dto.DataDevolucao.Should().Be(dataDevolucao);
        }

        [Fact]
        public void DadoAcervoSolicitacaoConfirmarDto_QuandoInstanciar_EntaoDataDevolucaoAssumeNulo()
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.DataDevolucao.Should().BeNull();
        }

        [Fact]
        public void DadoDataDevolucaoNula_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoConfirmarDto { DataDevolucao = new DateTime(2024, 6, 10) };

            dto.DataDevolucao = null;

            dto.DataDevolucao.Should().BeNull();
        }

        [Theory]
        [InlineData("2024-01-20")]
        [InlineData("2024-06-20")]
        [InlineData("2024-12-20")]
        public void DadoDiferentesDataDevolucoes_QuandoAssignar_EntaoRetornaValoresCorretos(string dataString)
        {
            var data = DateTime.Parse(dataString);
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.DataDevolucao = data;

            dto.DataDevolucao.Should().Be(data);
        }

        #endregion

        #region Testes de Propriedade TipoAcervo (herança)

        [Fact]
        public void DadoTipoAcervoValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var tipoAcervo = TipoAcervo.Bibliografico;
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.TipoAcervo = tipoAcervo;

            dto.TipoAcervo.Should().Be(tipoAcervo);
        }

        [Fact]
        public void DadoAcervoSolicitacaoConfirmarDto_QuandoInstanciar_EntaoTipoAcervoAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.TipoAcervo.Should().Be(default(TipoAcervo));
        }

        [Theory]
        [InlineData(TipoAcervo.Bibliografico)]
        [InlineData(TipoAcervo.ArtesGraficas)]
        [InlineData(TipoAcervo.DocumentacaoTextual)]
        [InlineData(TipoAcervo.Tridimensional)]
        public void DadoDiferentesTiposAcervo_QuandoAssignar_EntaoRetornaValoresCorretos(TipoAcervo tipoAcervo)
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.TipoAcervo = tipoAcervo;

            dto.TipoAcervo.Should().Be(tipoAcervo);
        }

        [Fact]
        public void DadoAcervoSolicitacaoConfirmarDto_QuandoAtribuirValoresMultiplosAoTipoAcervo_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.TipoAcervo = TipoAcervo.Bibliografico;
            dto.TipoAcervo = TipoAcervo.ArtesGraficas;
            dto.TipoAcervo = TipoAcervo.DocumentacaoTextual;

            dto.TipoAcervo.Should().Be(TipoAcervo.DocumentacaoTextual);
        }

        #endregion

        #region Testes de Propriedade TipoAtendimento (herança)

        [Fact]
        public void DadoTipoAtendimentoValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var tipoAtendimento = TipoAtendimento.Presencial;
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.TipoAtendimento = tipoAtendimento;

            dto.TipoAtendimento.Should().Be(tipoAtendimento);
        }

        [Fact]
        public void DadoAcervoSolicitacaoConfirmarDto_QuandoInstanciar_EntaoTipoAtendimentoAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.TipoAtendimento.Should().Be(default(TipoAtendimento));
        }

        [Theory]
        [InlineData(TipoAtendimento.Presencial)]
        [InlineData(TipoAtendimento.Email)]
        public void DadoDiferentesTiposAtendimento_QuandoAssignar_EntaoRetornaValoresCorretos(TipoAtendimento tipoAtendimento)
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.TipoAtendimento = tipoAtendimento;

            dto.TipoAtendimento.Should().Be(tipoAtendimento);
        }

        [Fact]
        public void DadoAcervoSolicitacaoConfirmarDto_QuandoAtribuirValoresMultiplosAoTipoAtendimento_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.TipoAtendimento = TipoAtendimento.Presencial;
            dto.TipoAtendimento = TipoAtendimento.Email;
            dto.TipoAtendimento = TipoAtendimento.Presencial;

            dto.TipoAtendimento.Should().Be(TipoAtendimento.Presencial);
        }

        #endregion

        #region Testes de Combinações e Integração

        [Fact]
        public void DadoAcervoSolicitacaoConfirmarDto_QuandoInstanciar_EntaoTodosOsPropriedadesAsumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.Id.Should().Be(0);
            dto.ItemId.Should().Be(0);
            dto.DataVisita.Should().BeNull();
            dto.DataEmprestimo.Should().BeNull();
            dto.DataDevolucao.Should().BeNull();
            dto.TipoAcervo.Should().Be(default(TipoAcervo));
            dto.TipoAtendimento.Should().Be(default(TipoAtendimento));
        }

        [Fact]
        public void DadoAcervoSolicitacaoConfirmarDto_QuandoAtribuirTodosOsPropriedades_EntaoRetornaTodosOsValoresAssignados()
        {
            var id = 15L;
            var itemId = 25L;
            var dataVisita = new DateTime(2024, 5, 15);
            var dataEmprestimo = new DateTime(2024, 5, 10);
            var dataDevolucao = new DateTime(2024, 6, 10);
            var tipoAcervo = TipoAcervo.Bibliografico;
            var tipoAtendimento = TipoAtendimento.Presencial;

            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = id,
                ItemId = itemId,
                DataVisita = dataVisita,
                DataEmprestimo = dataEmprestimo,
                DataDevolucao = dataDevolucao,
                TipoAcervo = tipoAcervo,
                TipoAtendimento = tipoAtendimento
            };

            dto.Id.Should().Be(id);
            dto.ItemId.Should().Be(itemId);
            dto.DataVisita.Should().Be(dataVisita);
            dto.DataEmprestimo.Should().Be(dataEmprestimo);
            dto.DataDevolucao.Should().Be(dataDevolucao);
            dto.TipoAcervo.Should().Be(tipoAcervo);
            dto.TipoAtendimento.Should().Be(tipoAtendimento);
        }

        [Fact]
        public void DadoAcervoSolicitacaoConfirmarDto_QuandoModificarPropriedadesSequencialmente_EntaoMantémCoerencia()
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.Id = 1;
            dto.ItemId = 10;
            dto.Id.Should().Be(1);
            dto.ItemId.Should().Be(10);

            dto.TipoAtendimento = TipoAtendimento.Presencial;
            dto.DataVisita = new DateTime(2024, 5, 15);
            dto.TipoAtendimento.Should().Be(TipoAtendimento.Presencial);
            dto.DataVisita.Should().Be(new DateTime(2024, 5, 15));

            dto.DataEmprestimo = new DateTime(2024, 5, 10);
            dto.DataDevolucao = new DateTime(2024, 6, 10);
            dto.TipoAcervo = TipoAcervo.Bibliografico;
            dto.DataEmprestimo.Should().Be(new DateTime(2024, 5, 10));
            dto.DataDevolucao.Should().Be(new DateTime(2024, 6, 10));
            dto.TipoAcervo.Should().Be(TipoAcervo.Bibliografico);
        }

        [Fact]
        public void DadoAcervoSolicitacaoConfirmarDto_QuandoCriarMultiplasInstancias_EntaoSãoIndependentes()
        {
            var dto1 = new AcervoSolicitacaoConfirmarDto
            {
                Id = 1,
                ItemId = 10,
                TipoAtendimento = TipoAtendimento.Presencial,
                TipoAcervo = TipoAcervo.Bibliografico
            };

            var dto2 = new AcervoSolicitacaoConfirmarDto
            {
                Id = 2,
                ItemId = 20,
                TipoAtendimento = TipoAtendimento.Email,
                TipoAcervo = TipoAcervo.ArtesGraficas
            };

            var dto3 = new AcervoSolicitacaoConfirmarDto
            {
                Id = 3,
                ItemId = 30,
                TipoAtendimento = TipoAtendimento.Presencial,
                TipoAcervo = TipoAcervo.DocumentacaoTextual
            };

            dto1.Id.Should().Be(1);
            dto1.ItemId.Should().Be(10);
            dto1.TipoAtendimento.Should().Be(TipoAtendimento.Presencial);

            dto2.Id.Should().Be(2);
            dto2.ItemId.Should().Be(20);
            dto2.TipoAtendimento.Should().Be(TipoAtendimento.Email);

            dto3.Id.Should().Be(3);
            dto3.ItemId.Should().Be(30);
            dto3.TipoAtendimento.Should().Be(TipoAtendimento.Presencial);

            dto1.Id = 100;
            dto2.Id.Should().Be(2);
            dto3.Id.Should().Be(3);
        }

        [Fact]
        public void DadoAcervoSolicitacaoConfirmarDto_QuandoInstanciarComConstrutorPadrao_EntaoTodosOsPropriedadesEstaoAcessiveis()
        {
            var dto = new AcervoSolicitacaoConfirmarDto();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<AcervoSolicitacaoConfirmarDto>();
            dto.Should().BeAssignableTo<DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO>();
        }

        [Fact]
        public void DadoDatasDiferentes_QuandoAssignarDataVIsitaEDataEmprestimoDevolucao_EntaoArmazenaTodasComCoerencia()
        {
            var dataVisita = new DateTime(2024, 5, 15);
            var dataEmprestimo = new DateTime(2024, 5, 10);
            var dataDevolucao = new DateTime(2024, 6, 10);

            var dto = new AcervoSolicitacaoConfirmarDto
            {
                DataVisita = dataVisita,
                DataEmprestimo = dataEmprestimo,
                DataDevolucao = dataDevolucao
            };

            dto.DataVisita.Should().Be(dataVisita);
            dto.DataEmprestimo.Should().Be(dataEmprestimo);
            dto.DataDevolucao.Should().Be(dataDevolucao);
            dto.DataEmprestimo.Should().BeBefore((DateTime)dto.DataDevolucao);
        }

        #endregion

        #region Testes com dados fictícios (Bogus)

        [Fact]
        public void DadoAcervoSolicitacaoConfirmarDtoComDadosFictícios_QuandoAssignar_EntaoArmazenaCorretamente()
        {
            var faker = new Faker();
            var id = faker.Random.Long(1, 10000);
            var itemId = faker.Random.Long(1, 10000);
            var tipoAcervo = faker.PickRandom<TipoAcervo>();
            var tipoAtendimento = faker.PickRandom<TipoAtendimento>();

            var dto = new AcervoSolicitacaoConfirmarDto
            {
                Id = id,
                ItemId = itemId,
                TipoAcervo = tipoAcervo,
                TipoAtendimento = tipoAtendimento,
                DataVisita = faker.Date.Future(),
                DataEmprestimo = faker.Date.Past(),
                DataDevolucao = faker.Date.Future()
            };

            dto.Id.Should().Be(id);
            dto.ItemId.Should().Be(itemId);
            dto.TipoAcervo.Should().Be(tipoAcervo);
            dto.TipoAtendimento.Should().Be(tipoAtendimento);
            dto.DataVisita.Should().NotBeNull();
            dto.DataEmprestimo.Should().NotBeNull();
            dto.DataDevolucao.Should().NotBeNull();
        }

        [Theory]
        [InlineData(TipoAcervo.Bibliografico, TipoAtendimento.Presencial)]
        [InlineData(TipoAcervo.ArtesGraficas, TipoAtendimento.Email)]
        [InlineData(TipoAcervo.DocumentacaoTextual, TipoAtendimento.Presencial)]
        [InlineData(TipoAcervo.Tridimensional, TipoAtendimento.Email)]
        public void DadoCombinacoesDeTiposAcervoEAtendimento_QuandoAssignar_EntaoRetornaValoresCorretos(
            TipoAcervo tipoAcervo, 
            TipoAtendimento tipoAtendimento)
        {
            var dto = new AcervoSolicitacaoConfirmarDto
            {
                TipoAcervo = tipoAcervo,
                TipoAtendimento = tipoAtendimento
            };

            dto.TipoAcervo.Should().Be(tipoAcervo);
            dto.TipoAtendimento.Should().Be(tipoAtendimento);
        }

        #endregion
    }
}
