using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoTridimensionalLinhaRetornoDTOTeste
    {
        [Fact]
        public void DadoAcervoTridimensionalLinhaRetornoDTO_QuandoInstanciar_EntaoTodasAsPropriedadesSaoInicializadasCorretamente()
        {
            var dto = new AcervoTridimensionalLinhaRetornoDTO();

            dto.Should().NotBeNull();
            dto.Titulo.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.DataAcervo.Should().BeNull();
            dto.ConservacaoId.Should().BeNull();
            dto.Quantidade.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Profundidade.Should().BeNull();
            dto.Diametro.Should().BeNull();
            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesTituloCodigo_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalLinhaRetornoDTO();
            var faker = new Faker("pt_BR");
            var titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Lorem.Sentence() };
            var codigo = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Random.Replace("TB-####") };

            dto.Titulo = titulo;
            dto.Codigo = codigo;

            dto.Titulo.Should().BeEquivalentTo(titulo);
            dto.Codigo.Should().BeEquivalentTo(codigo);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesProcedenciaEDataAcervo_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalLinhaRetornoDTO();
            var faker = new Faker("pt_BR");
            var procedencia = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Lorem.Text() };
            var dataAcervo = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Date.Recent().ToString("dd/MM/yyyy") };

            dto.Procedencia = procedencia;
            dto.DataAcervo = dataAcervo;

            dto.Procedencia.Should().BeEquivalentTo(procedencia);
            dto.DataAcervo.Should().BeEquivalentTo(dataAcervo);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesConservacaoIdEQuantidade_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalLinhaRetornoDTO();
            var faker = new Faker("pt_BR");
            var conservacaoId = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Random.Number(1, 100).ToString() };
            var quantidade = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Random.Number(1, 1000).ToString() };

            dto.ConservacaoId = conservacaoId;
            dto.Quantidade = quantidade;

            dto.ConservacaoId.Should().BeEquivalentTo(conservacaoId);
            dto.Quantidade.Should().BeEquivalentTo(quantidade);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesDescricao_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new AcervoTridimensionalLinhaRetornoDTO();
            var faker = new Faker("pt_BR");
            var descricao = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Lorem.Paragraph() };

            dto.Descricao = descricao;

            dto.Descricao.Should().BeEquivalentTo(descricao);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesDimensoes_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalLinhaRetornoDTO();
            var faker = new Faker();
            var largura = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Random.Double(0.1, 100).ToString("F2") };
            var altura = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Random.Double(0.1, 100).ToString("F2") };
            var profundidade = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Random.Double(0.1, 100).ToString("F2") };
            var diametro = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Random.Double(0.1, 100).ToString("F2") };

            dto.Largura = largura;
            dto.Altura = altura;
            dto.Profundidade = profundidade;
            dto.Diametro = diametro;

            dto.Largura.Should().BeEquivalentTo(largura);
            dto.Altura.Should().BeEquivalentTo(altura);
            dto.Profundidade.Should().BeEquivalentTo(profundidade);
            dto.Diametro.Should().BeEquivalentTo(diametro);
        }

        [Fact]
        public void DadoValorValido_QuandoAtribuirAno_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new AcervoTridimensionalLinhaRetornoDTO();
            var faker = new Faker();
            var ano = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Date.Recent().Year.ToString() };

            dto.Ano = ano;

            dto.Ano.Should().BeEquivalentTo(ano);
        }

        [Fact]
        public void DadoMultiplosValores_QuandoAtribuirTodasAsPropriedades_EntaoTodosOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalLinhaRetornoDTO();
            var faker = new Faker("pt_BR");
            var titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Lorem.Sentence() };
            var codigo = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Random.Replace("TB-####") };
            var procedencia = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Lorem.Text() };
            var dataAcervo = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Date.Recent().ToString("dd/MM/yyyy") };
            var conservacaoId = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Random.Number(1, 100).ToString() };
            var quantidade = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Random.Number(1, 1000).ToString() };
            var descricao = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Lorem.Paragraph() };
            var largura = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Random.Double(0.1, 100).ToString("F2") };
            var altura = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Random.Double(0.1, 100).ToString("F2") };
            var profundidade = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Random.Double(0.1, 100).ToString("F2") };
            var diametro = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Random.Double(0.1, 100).ToString("F2") };
            var ano = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Date.Recent().Year.ToString() };

            dto.Titulo = titulo;
            dto.Codigo = codigo;
            dto.Procedencia = procedencia;
            dto.DataAcervo = dataAcervo;
            dto.ConservacaoId = conservacaoId;
            dto.Quantidade = quantidade;
            dto.Descricao = descricao;
            dto.Largura = largura;
            dto.Altura = altura;
            dto.Profundidade = profundidade;
            dto.Diametro = diametro;
            dto.Ano = ano;

            dto.Titulo.Should().BeEquivalentTo(titulo);
            dto.Codigo.Should().BeEquivalentTo(codigo);
            dto.Procedencia.Should().BeEquivalentTo(procedencia);
            dto.DataAcervo.Should().BeEquivalentTo(dataAcervo);
            dto.ConservacaoId.Should().BeEquivalentTo(conservacaoId);
            dto.Quantidade.Should().BeEquivalentTo(quantidade);
            dto.Descricao.Should().BeEquivalentTo(descricao);
            dto.Largura.Should().BeEquivalentTo(largura);
            dto.Altura.Should().BeEquivalentTo(altura);
            dto.Profundidade.Should().BeEquivalentTo(profundidade);
            dto.Diametro.Should().BeEquivalentTo(diametro);
            dto.Ano.Should().BeEquivalentTo(ano);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesNulas_EntaoOsValoresSaoArmazenadosComNull()
        {
            var dto = new AcervoTridimensionalLinhaRetornoDTO();

            dto.Titulo = null;
            dto.Codigo = null;
            dto.Procedencia = null;
            dto.DataAcervo = null;
            dto.ConservacaoId = null;
            dto.Quantidade = null;
            dto.Descricao = null;
            dto.Largura = null;
            dto.Altura = null;
            dto.Profundidade = null;
            dto.Diametro = null;
            dto.Ano = null;

            dto.Titulo.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.DataAcervo.Should().BeNull();
            dto.ConservacaoId.Should().BeNull();
            dto.Quantidade.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Profundidade.Should().BeNull();
            dto.Diametro.Should().BeNull();
            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoInstanciasMultiplas_QuandoAtribuirPropriedadesEmCadaUma_EntaoAsProrpiedadesNaoSaoCompartilhadas()
        {
            var dto1 = new AcervoTridimensionalLinhaRetornoDTO();
            var dto2 = new AcervoTridimensionalLinhaRetornoDTO();
            var faker = new Faker("pt_BR");

            var titulo1 = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Lorem.Sentence() };
            var titulo2 = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Lorem.Sentence() };

            dto1.Titulo = titulo1;
            dto2.Titulo = titulo2;

            dto1.Titulo.Should().BeEquivalentTo(titulo1);
            dto2.Titulo.Should().BeEquivalentTo(titulo2);
            dto1.Titulo.Should().NotBeEquivalentTo(titulo2);
        }

        [Fact]
        public void DadoPropriedadeAtribuida_QuandoAtribuirMulitaVezes_EntaoOUltimoValorEhRetido()
        {
            var dto = new AcervoTridimensionalLinhaRetornoDTO();
            var faker = new Faker("pt_BR");

            var titulo1 = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Lorem.Sentence() };
            var titulo2 = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Lorem.Sentence() };
            var titulo3 = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Lorem.Sentence() };

            dto.Titulo = titulo1;
            dto.Titulo = titulo2;
            dto.Titulo = titulo3;

            dto.Titulo.Should().BeEquivalentTo(titulo3);
            dto.Titulo.Should().NotBeEquivalentTo(titulo1);
            dto.Titulo.Should().NotBeEquivalentTo(titulo2);
        }

        [Fact]
        public void DadoPropriedadeTitulo_QuandoVerificaSeEhPublic_EntaoEhAccessivel()
        {
            var dto = new AcervoTridimensionalLinhaRetornoDTO();
            var faker = new Faker("pt_BR");
            var titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Lorem.Sentence() };

            dto.Titulo = titulo;
            var conteudoRetornado = dto.Titulo.Conteudo;

            conteudoRetornado.Should().Be(titulo.Conteudo);
        }

        [Fact]
        public void DadoTodasAsPropriedades_QuandoAtribuirComValoresAleatorios_EntaoODTOEhCriado()
        {
            var dto = new AcervoTridimensionalLinhaRetornoDTO
            {
                Titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Título" },
                Codigo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "TB-001" },
                Procedencia = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Procedência" },
                DataAcervo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "01/01/2024" },
                ConservacaoId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1" },
                Quantidade = new LinhaConteudoAjustarRetornoDTO { Conteudo = "10" },
                Descricao = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Descrição" },
                Largura = new LinhaConteudoAjustarRetornoDTO { Conteudo = "10.50" },
                Altura = new LinhaConteudoAjustarRetornoDTO { Conteudo = "20.50" },
                Profundidade = new LinhaConteudoAjustarRetornoDTO { Conteudo = "15.50" },
                Diametro = new LinhaConteudoAjustarRetornoDTO { Conteudo = "5.50" },
                Ano = new LinhaConteudoAjustarRetornoDTO { Conteudo = "2024" }
            };

            dto.Should().NotBeNull();
            dto.Titulo.Conteudo.Should().Be("Título");
            dto.Codigo.Conteudo.Should().Be("TB-001");
            dto.Procedencia.Conteudo.Should().Be("Procedência");
            dto.DataAcervo.Conteudo.Should().Be("01/01/2024");
            dto.ConservacaoId.Conteudo.Should().Be("1");
            dto.Quantidade.Conteudo.Should().Be("10");
            dto.Descricao.Conteudo.Should().Be("Descrição");
            dto.Largura.Conteudo.Should().Be("10.50");
            dto.Altura.Conteudo.Should().Be("20.50");
            dto.Profundidade.Conteudo.Should().Be("15.50");
            dto.Diametro.Conteudo.Should().Be("5.50");
            dto.Ano.Conteudo.Should().Be("2024");
        }

        [Fact]
        public void DadoDTOComPropriedadesPreenchidas_QuandoVerificaSeEhInstanciaDoTipo_EntaoEhValidado()
        {
            var dto = new AcervoTridimensionalLinhaRetornoDTO
            {
                Titulo = new LinhaConteudoAjustarRetornoDTO()
            };

            dto.Should().BeOfType<AcervoTridimensionalLinhaRetornoDTO>();
            (dto as AcervoTridimensionalLinhaRetornoDTO).Should().NotBeNull();
        }

        [Fact]
        public void DadoPropriedadesComValorVazio_QuandoAtribuirStringVazio_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new AcervoTridimensionalLinhaRetornoDTO();
            var conteudoVazio = new LinhaConteudoAjustarRetornoDTO { Conteudo = string.Empty };

            dto.Titulo = conteudoVazio;

            dto.Titulo.Should().NotBeNull();
            dto.Titulo.Conteudo.Should().BeEmpty();
        }

        [Fact]
        public void DadoPropriedadesDoTipo_QuandoVerificaSeuTipo_EntaoEhLinhaConteudoAjustarRetornoDTO()
        {
            var dto = new AcervoTridimensionalLinhaRetornoDTO();
            var conteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Teste" };

            dto.Titulo = conteudo;

            dto.Titulo.Should().BeOfType<LinhaConteudoAjustarRetornoDTO>();
        }
    }
}
