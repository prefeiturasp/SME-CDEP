using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTOTeste
    {
        [Fact]
        public void DadoDTOValido_QuandoCriar_EntaoDevePossuirPropriedades()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO>();
        }

        [Fact]
        public void DadoDataVisitaDefinida_QuandoAtribuir_EntaoDataVisitaDeveSerSetada()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();
            var dataVisitaEsperada = new DateTime(2025, 5, 4, 10, 30, 0);

            dto.DataVisita = dataVisitaEsperada;

            dto.DataVisita.Should().Be(dataVisitaEsperada);
        }

        [Fact]
        public void DadoDataVisitaNula_QuandoAtribuir_EntaoDataVisitaDeveSerNula()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();

            dto.DataVisita = null;

            dto.DataVisita.Should().BeNull();
        }

        [Fact]
        public void DadoDataEmprestrimoDefinida_QuandoAtribuir_EntaoDataEmprestrimoDeveSerSetada()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();
            var dataEmprestrimoEsperada = new DateTime(2025, 5, 5, 14, 15, 0);

            dto.DataEmprestimo = dataEmprestrimoEsperada;

            dto.DataEmprestimo.Should().Be(dataEmprestrimoEsperada);
        }

        [Fact]
        public void DadoDataEmprestrimoNula_QuandoAtribuir_EntaoDataEmprestrimoDeveSerNula()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();

            dto.DataEmprestimo = null;

            dto.DataEmprestimo.Should().BeNull();
        }

        [Fact]
        public void DadoDataDevolucaoDefinida_QuandoAtribuir_EntaoDataDevolucaoDeveSerSetada()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();
            var dataDevolucaoEsperada = new DateTime(2025, 5, 6, 16, 45, 0);

            dto.DataDevolucao = dataDevolucaoEsperada;

            dto.DataDevolucao.Should().Be(dataDevolucaoEsperada);
        }

        [Fact]
        public void DadoDataDevolucaoNula_QuandoAtribuir_EntaoDataDevolucaoDeveSerNula()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();

            dto.DataDevolucao = null;

            dto.DataDevolucao.Should().BeNull();
        }

        [Fact]
        public void DadoTipoAcervoDefinido_QuandoAtribuir_EntaoTipoAcervoDeveSerSetado()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();
            var tipoAcervoEsperado = TipoAcervo.Bibliografico;

            dto.TipoAcervo = tipoAcervoEsperado;

            dto.TipoAcervo.Should().Be(tipoAcervoEsperado);
        }

        [Fact]
        public void DadoTipoAcervoComOutroValor_QuandoAtribuir_EntaoTipoAcervoDeveSerAlterado()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();
            dto.TipoAcervo = TipoAcervo.Bibliografico;
            var tipoAcervoNovoEsperado = TipoAcervo.DocumentacaoTextual;

            dto.TipoAcervo = tipoAcervoNovoEsperado;

            dto.TipoAcervo.Should().Be(tipoAcervoNovoEsperado);
            dto.TipoAcervo.Should().NotBe(TipoAcervo.Bibliografico);
        }

        [Fact]
        public void DadoTipoAcervoArtesGraficas_QuandoAtribuir_EntaoTipoAcervoDeveSerSetado()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();

            dto.TipoAcervo = TipoAcervo.ArtesGraficas;

            dto.TipoAcervo.Should().Be(TipoAcervo.ArtesGraficas);
        }

        [Fact]
        public void DadoTipoAcervoAudiovisual_QuandoAtribuir_EntaoTipoAcervoDeveSerSetado()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();

            dto.TipoAcervo = TipoAcervo.Audiovisual;

            dto.TipoAcervo.Should().Be(TipoAcervo.Audiovisual);
        }

        [Fact]
        public void DadoTipoAcervoFotografico_QuandoAtribuir_EntaoTipoAcervoDeveSerSetado()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();

            dto.TipoAcervo = TipoAcervo.Fotografico;

            dto.TipoAcervo.Should().Be(TipoAcervo.Fotografico);
        }

        [Fact]
        public void DadoTipoAcervoTridimensional_QuandoAtribuir_EntaoTipoAcervoDeveSerSetado()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();

            dto.TipoAcervo = TipoAcervo.Tridimensional;

            dto.TipoAcervo.Should().Be(TipoAcervo.Tridimensional);
        }

        [Fact]
        public void DadoTipoAtendimentoDefinido_QuandoAtribuir_EntaoTipoAtendimentoDeveSerSetado()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();
            var tipoAtendimentoEsperado = TipoAtendimento.Email;

            dto.TipoAtendimento = tipoAtendimentoEsperado;

            dto.TipoAtendimento.Should().Be(tipoAtendimentoEsperado);
        }

        [Fact]
        public void DadoTipoAtendimentoPresencial_QuandoAtribuir_EntaoTipoAtendimentoDeveSerPresencial()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();

            dto.TipoAtendimento = TipoAtendimento.Presencial;

            dto.TipoAtendimento.Should().Be(TipoAtendimento.Presencial);
        }

        [Fact]
        public void DadoTipoAtendimentoEmail_QuandoAtribuir_EntaoTipoAtendimentoDeveSerEmail()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();

            dto.TipoAtendimento = TipoAtendimento.Email;

            dto.TipoAtendimento.Should().Be(TipoAtendimento.Email);
        }

        [Fact]
        public void DadoDTOComTodasAsPropriedadesDefinidas_QuandoVerificar_EntaoTodasDeveramEstarSetadas()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();
            var dataVisita = new DateTime(2025, 5, 4);
            var dataEmprestimo = new DateTime(2025, 5, 5);
            var dataDevolucao = new DateTime(2025, 5, 6);
            var tipoAcervo = TipoAcervo.Audiovisual;
            var tipoAtendimento = TipoAtendimento.Presencial;

            dto.DataVisita = dataVisita;
            dto.DataEmprestimo = dataEmprestimo;
            dto.DataDevolucao = dataDevolucao;
            dto.TipoAcervo = tipoAcervo;
            dto.TipoAtendimento = tipoAtendimento;

            dto.DataVisita.Should().Be(dataVisita);
            dto.DataEmprestimo.Should().Be(dataEmprestimo);
            dto.DataDevolucao.Should().Be(dataDevolucao);
            dto.TipoAcervo.Should().Be(tipoAcervo);
            dto.TipoAtendimento.Should().Be(tipoAtendimento);
        }

        [Fact]
        public void DadoMultiplosDTOs_QuandoCriar_EntaoTodosDevemSerIndependentes()
        {
            var dto1 = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO
            {
                DataVisita = new DateTime(2025, 5, 4),
                TipoAtendimento = TipoAtendimento.Email
            };

            var dto2 = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO
            {
                DataVisita = new DateTime(2025, 5, 5),
                TipoAtendimento = TipoAtendimento.Presencial
            };

            dto1.Should().NotBeSameAs(dto2);
            dto1.DataVisita.Should().NotBe(dto2.DataVisita);
            dto1.TipoAtendimento.Should().NotBe(dto2.TipoAtendimento);
        }

        [Fact]
        public void DadoDTOComValoresAlterados_QuandoVerificar_EntaoAlteracoesDeveramSerReflexas()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();
            var dataInicial = new DateTime(2025, 5, 4);
            var dataAlterada = new DateTime(2025, 5, 10);

            dto.DataVisita = dataInicial;
            dto.DataVisita = dataAlterada;

            dto.DataVisita.Should().Be(dataAlterada);
            dto.DataVisita.Should().NotBe(dataInicial);
        }

        [Fact]
        public void DadoDTOUsandoFaker_QuandoGerarDados_EntaoDeveSerPreenchidoCorretamente()
        {
            var faker = new Faker<DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO>("pt_BR")
                .RuleFor(x => x.DataVisita, f => f.Date.Past())
                .RuleFor(x => x.DataEmprestimo, f => f.Date.Past())
                .RuleFor(x => x.DataDevolucao, f => f.Date.Recent())
                .RuleFor(x => x.TipoAcervo, f => f.Random.Enum<TipoAcervo>())
                .RuleFor(x => x.TipoAtendimento, f => f.Random.Enum<TipoAtendimento>());

            var dto = faker.Generate();

            dto.Should().NotBeNull();
            dto.DataVisita.Should().NotBe(null);
            dto.DataEmprestimo.Should().NotBe(null);
            dto.DataDevolucao.Should().NotBe(null);
        }

        [Fact]
        public void DadoListaDeTresDTOs_QuandoGerarMultiplos_EntaoTodosDevemSerValidos()
        {
            var faker = new Faker<DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO>("pt_BR")
                .RuleFor(x => x.DataVisita, f => f.Date.Past())
                .RuleFor(x => x.TipoAtendimento, f => f.Random.Enum<TipoAtendimento>());

            var dtos = faker.Generate(3).ToList();

            dtos.Should().HaveCount(3);
            dtos.Should().AllSatisfy(dto =>
            {
                dto.DataVisita.Should().NotBe(null);
                dto.Should().NotBeNull();
            });
        }

        [Fact]
        public void DadoDTOComPropriedadesDefinidas_QuandoAcessar_EntaoTodasDeveramEstarDisponíveis()
        {
            var propriedades = typeof(DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO).GetProperties();

            propriedades.Should().NotBeEmpty();
            propriedades.Should().Contain(p => p.Name == "DataVisita");
            propriedades.Should().Contain(p => p.Name == "DataEmprestimo");
            propriedades.Should().Contain(p => p.Name == "DataDevolucao");
            propriedades.Should().Contain(p => p.Name == "TipoAcervo");
            propriedades.Should().Contain(p => p.Name == "TipoAtendimento");
        }

        [Fact]
        public void DadoDTOComDatasSequenciais_QuandoVerificarEquivalencia_EntaoDeveSerEquivalente()
        {
            var dataVisita = new DateTime(2025, 5, 4);
            var dataEmprestimo = new DateTime(2025, 5, 5);
            var dataDevolucao = new DateTime(2025, 5, 6);

            var dto1 = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO
            {
                DataVisita = dataVisita,
                DataEmprestimo = dataEmprestimo,
                DataDevolucao = dataDevolucao,
                TipoAcervo = TipoAcervo.Bibliografico,
                TipoAtendimento = TipoAtendimento.Email
            };

            var dto2 = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO
            {
                DataVisita = dataVisita,
                DataEmprestimo = dataEmprestimo,
                DataDevolucao = dataDevolucao,
                TipoAcervo = TipoAcervo.Bibliografico,
                TipoAtendimento = TipoAtendimento.Email
            };

            dto1.Should().BeEquivalentTo(dto2);
        }

        [Fact]
        public void DadoDTOComDataVisitaNoFuturo_QuandoAtribuir_EntaoDataVisitaDeveSerNoFuturo()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();
            var futuro = DateTime.Now.AddDays(30);

            dto.DataVisita = futuro;

            dto.DataVisita.Should().BeAfter(DateTime.Now);
            dto.DataVisita.Should().Be(futuro);
        }

        [Fact]
        public void DadoDTOComDataVisitaNoPassado_QuandoAtribuir_EntaoDataVisitaDeveSerNoPassado()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();
            var passado = DateTime.Now.AddDays(-30);

            dto.DataVisita = passado;

            dto.DataVisita.Should().BeBefore(DateTime.Now);
            dto.DataVisita.Should().Be(passado);
        }

        [Fact]
        public void DadoTipoAtendimentoComValorAlterado_QuandoVerificar_EntaoAlteracaoDeveSerReflexo()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();
            dto.TipoAtendimento = TipoAtendimento.Email;
            var novoTipo = TipoAtendimento.Presencial;

            dto.TipoAtendimento = novoTipo;

            dto.TipoAtendimento.Should().Be(novoTipo);
            dto.TipoAtendimento.Should().NotBe(TipoAtendimento.Email);
        }

        [Fact]
        public void DadoTodasAsMesmasDatasCom_QuandoAtribuir_EntaoDeveSuportarDataHora()
        {
            var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO();
            var dataDiaInteiro = new DateTime(2025, 5, 4, 23, 59, 59);

            dto.DataVisita = dataDiaInteiro;

            dto.DataVisita.Should().Be(dataDiaInteiro);
            dto.DataVisita.Value.Hour.Should().Be(23);
            dto.DataVisita.Value.Minute.Should().Be(59);
            dto.DataVisita.Value.Second.Should().Be(59);
        }

        [Fact]
        public void DadoDTOComTiposAcervoTodosValidos_QuandoIterar_EntaoTodosDevemEstarPresentes()
        {
            var tiposAcervo = new[]
            {
                TipoAcervo.Bibliografico,
                TipoAcervo.DocumentacaoTextual,
                TipoAcervo.ArtesGraficas,
                TipoAcervo.Audiovisual,
                TipoAcervo.Fotografico,
                TipoAcervo.Tridimensional
            };

            foreach (var tipo in tiposAcervo)
            {
                var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO { TipoAcervo = tipo };
                dto.TipoAcervo.Should().Be(tipo);
            }
        }

        [Fact]
        public void DadoDTOComTiposAtendimentoTodosValidos_QuandoIterar_EntaoTodosDevemEstarPresentes()
        {
            var tiposAtendimento = new[]
            {
                TipoAtendimento.Email,
                TipoAtendimento.Presencial
            };

            foreach (var tipo in tiposAtendimento)
            {
                var dto = new DataVisitaEmprestimoDevolucaoTipoAcervoAtendimentoDTO { TipoAtendimento = tipo };
                dto.TipoAtendimento.Should().Be(tipo);
            }
        }
    }
}
