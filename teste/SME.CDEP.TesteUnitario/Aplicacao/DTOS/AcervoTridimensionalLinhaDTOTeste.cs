using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoTridimensionalLinhaDtoTeste
    {
        [Fact]
        public void DadoAcervoTridimensionalLinhaDTO_QuandoInstanciar_EntaoTodasAsPropriedadesSaoInicializadasCorretamente()
        {
            var dto = new AcervoTridimensionalLinhaDTO();

            dto.Should().NotBeNull();
            dto.Titulo.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.EstadoConservacao.Should().BeNull();
            dto.Quantidade.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Profundidade.Should().BeNull();
            dto.Diametro.Should().BeNull();
            dto.Ano.Should().BeNull();
            dto.Status.Should().Be(default(ImportacaoStatus));
            dto.Mensagem.Should().BeNull();
            dto.NumeroLinha.Should().Be(0);
            dto.PossuiErros.Should().BeFalse();
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesTituloCodigo_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalLinhaDTO();
            var faker = new Faker("pt_BR");
            var titulo = new LinhaConteudoAjustarDTO { Conteudo = faker.Lorem.Sentence() };
            var codigo = new LinhaConteudoAjustarDTO { Conteudo = faker.Random.Replace("TB-####") };

            dto.Titulo = titulo;
            dto.Codigo = codigo;

            dto.Titulo.Should().BeEquivalentTo(titulo);
            dto.Codigo.Should().BeEquivalentTo(codigo);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesProcedenciaEestadoConservacao_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalLinhaDTO();
            var faker = new Faker("pt_BR");
            var procedencia = new LinhaConteudoAjustarDTO { Conteudo = faker.Lorem.Text() };
            var estadoConservacao = new LinhaConteudoAjustarDTO { Conteudo = faker.Lorem.Word() };

            dto.Procedencia = procedencia;
            dto.EstadoConservacao = estadoConservacao;

            dto.Procedencia.Should().BeEquivalentTo(procedencia);
            dto.EstadoConservacao.Should().BeEquivalentTo(estadoConservacao);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesQuantidadeDescricao_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalLinhaDTO();
            var faker = new Faker("pt_BR");
            var quantidade = new LinhaConteudoAjustarDTO { Conteudo = faker.Random.Number(1, 1000).ToString() };
            var descricao = new LinhaConteudoAjustarDTO { Conteudo = faker.Lorem.Paragraph() };

            dto.Quantidade = quantidade;
            dto.Descricao = descricao;

            dto.Quantidade.Should().BeEquivalentTo(quantidade);
            dto.Descricao.Should().BeEquivalentTo(descricao);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesDimensoes_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalLinhaDTO();
            var faker = new Faker();
            var largura = new LinhaConteudoAjustarDTO { Conteudo = faker.Random.Double(0.1, 100).ToString("F2") };
            var altura = new LinhaConteudoAjustarDTO { Conteudo = faker.Random.Double(0.1, 100).ToString("F2") };
            var profundidade = new LinhaConteudoAjustarDTO { Conteudo = faker.Random.Double(0.1, 100).ToString("F2") };
            var diametro = new LinhaConteudoAjustarDTO { Conteudo = faker.Random.Double(0.1, 100).ToString("F2") };

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
            var dto = new AcervoTridimensionalLinhaDTO();
            var faker = new Faker();
            var ano = new LinhaConteudoAjustarDTO { Conteudo = faker.Date.Recent().Year.ToString() };

            dto.Ano = ano;

            dto.Ano.Should().BeEquivalentTo(ano);
        }

        [Fact]
        public void DadoMultiplosValores_QuandoAtribuirTodasAsPropriedades_EntaoTodosOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalLinhaDTO();
            var faker = new Faker("pt_BR");
            var titulo = new LinhaConteudoAjustarDTO { Conteudo = faker.Lorem.Sentence() };
            var codigo = new LinhaConteudoAjustarDTO { Conteudo = faker.Random.Replace("TB-####") };
            var procedencia = new LinhaConteudoAjustarDTO { Conteudo = faker.Lorem.Text() };
            var estadoConservacao = new LinhaConteudoAjustarDTO { Conteudo = faker.Lorem.Word() };
            var quantidade = new LinhaConteudoAjustarDTO { Conteudo = faker.Random.Number(1, 1000).ToString() };
            var descricao = new LinhaConteudoAjustarDTO { Conteudo = faker.Lorem.Paragraph() };
            var largura = new LinhaConteudoAjustarDTO { Conteudo = faker.Random.Double(0.1, 100).ToString("F2") };
            var altura = new LinhaConteudoAjustarDTO { Conteudo = faker.Random.Double(0.1, 100).ToString("F2") };
            var profundidade = new LinhaConteudoAjustarDTO { Conteudo = faker.Random.Double(0.1, 100).ToString("F2") };
            var diametro = new LinhaConteudoAjustarDTO { Conteudo = faker.Random.Double(0.1, 100).ToString("F2") };
            var ano = new LinhaConteudoAjustarDTO { Conteudo = faker.Date.Recent().Year.ToString() };

            dto.Titulo = titulo;
            dto.Codigo = codigo;
            dto.Procedencia = procedencia;
            dto.EstadoConservacao = estadoConservacao;
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
            dto.EstadoConservacao.Should().BeEquivalentTo(estadoConservacao);
            dto.Quantidade.Should().BeEquivalentTo(quantidade);
            dto.Descricao.Should().BeEquivalentTo(descricao);
            dto.Largura.Should().BeEquivalentTo(largura);
            dto.Altura.Should().BeEquivalentTo(altura);
            dto.Profundidade.Should().BeEquivalentTo(profundidade);
            dto.Diametro.Should().BeEquivalentTo(diametro);
            dto.Ano.Should().BeEquivalentTo(ano);
        }

        [Fact]
        public void DadoLinhaComSucesso_QuandoDefinirLinhaComoSucesso_EntaoOsValoresSaoAjustadosCorretamente()
        {
            var dto = new AcervoTridimensionalLinhaDTO
            {
                PossuiErros = true,
                Mensagem = "Erro anterior",
                Status = ImportacaoStatus.Erros,
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Teste", PossuiErro = true, Mensagem = "Erro" },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = "TB-001", PossuiErro = true, Mensagem = "Erro" },
                Procedencia = new LinhaConteudoAjustarDTO { Conteudo = "Procedencia", PossuiErro = true, Mensagem = "Erro" },
                EstadoConservacao = new LinhaConteudoAjustarDTO { Conteudo = "Bom", PossuiErro = true, Mensagem = "Erro" },
                Descricao = new LinhaConteudoAjustarDTO { Conteudo = "Descricao", PossuiErro = true, Mensagem = "Erro" },
                Quantidade = new LinhaConteudoAjustarDTO { Conteudo = "10", PossuiErro = true, Mensagem = "Erro" },
                Altura = new LinhaConteudoAjustarDTO { Conteudo = "10.50", PossuiErro = true, Mensagem = "Erro" },
                Largura = new LinhaConteudoAjustarDTO { Conteudo = "20.50", PossuiErro = true, Mensagem = "Erro" },
                Profundidade = new LinhaConteudoAjustarDTO { Conteudo = "15.50", PossuiErro = true, Mensagem = "Erro" },
                Diametro = new LinhaConteudoAjustarDTO { Conteudo = "5.50", PossuiErro = true, Mensagem = "Erro" }
            };

            dto.DefinirLinhaComoSucesso();

            dto.PossuiErros.Should().BeFalse();
            dto.Mensagem.Should().BeEmpty();
            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            
            dto.Titulo.PossuiErro.Should().BeFalse();
            dto.Titulo.Mensagem.Should().BeEmpty();
            dto.Codigo.PossuiErro.Should().BeFalse();
            dto.Codigo.Mensagem.Should().BeEmpty();
            dto.Procedencia.PossuiErro.Should().BeFalse();
            dto.Procedencia.Mensagem.Should().BeEmpty();
            dto.EstadoConservacao.PossuiErro.Should().BeFalse();
            dto.EstadoConservacao.Mensagem.Should().BeEmpty();
            dto.Descricao.PossuiErro.Should().BeFalse();
            dto.Descricao.Mensagem.Should().BeEmpty();
            dto.Quantidade.PossuiErro.Should().BeFalse();
            dto.Quantidade.Mensagem.Should().BeEmpty();
            dto.Altura.PossuiErro.Should().BeFalse();
            dto.Altura.Mensagem.Should().BeEmpty();
            dto.Largura.PossuiErro.Should().BeFalse();
            dto.Largura.Mensagem.Should().BeEmpty();
            dto.Profundidade.PossuiErro.Should().BeFalse();
            dto.Profundidade.Mensagem.Should().BeEmpty();
            dto.Diametro.PossuiErro.Should().BeFalse();
            dto.Diametro.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoPropriedadesComNull_QuandoDefinirLinhaComoSucesso_EntaoNaoLancaExcecao()
        {
            var dto = new AcervoTridimensionalLinhaDTO
            {
                PossuiErros = true,
                Mensagem = "Erro anterior",
                Status = ImportacaoStatus.Erros,
                Titulo = null!,
                Codigo = null!,
                Procedencia = null!,
                EstadoConservacao = null!,
                Descricao = null!,
                Quantidade = null!,
                Altura = null!,
                Largura = null!,
                Profundidade = null!,
                Diametro = null!,
                Ano = null!
            };

            var action = () => dto.DefinirLinhaComoSucesso();

            action.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesHerdadas_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalLinhaDTO();
            var faker = new Faker();
            var status = ImportacaoStatus.Pendente;
            var mensagem = "Teste mensagem";
            var numeroLinha = faker.Random.Number(1, 1000);
            var possuiErros = true;

            dto.Status = status;
            dto.Mensagem = mensagem;
            dto.NumeroLinha = numeroLinha;
            dto.PossuiErros = possuiErros;

            dto.Status.Should().Be(status);
            dto.Mensagem.Should().Be(mensagem);
            dto.NumeroLinha.Should().Be(numeroLinha);
            dto.PossuiErros.Should().Be(possuiErros);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesNulas_EntaoOsValoresSaoArmazenadosComNull()
        {
            var dto = new AcervoTridimensionalLinhaDTO();

            dto.Titulo = null!;
            dto.Codigo = null!;
            dto.Procedencia = null!;
            dto.EstadoConservacao = null!;
            dto.Quantidade = null!;
            dto.Descricao = null!;
            dto.Largura = null!;
            dto.Altura = null!;
            dto.Profundidade = null!;
            dto.Diametro = null!;
            dto.Ano = null!;

            dto.Titulo.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.EstadoConservacao.Should().BeNull();
            dto.Quantidade.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Profundidade.Should().BeNull();
            dto.Diametro.Should().BeNull();
            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoLinhasMultiplas_QuandoAtribuirPropriedadesIndividualmente_EntaoApsAtribuicaoTodasSaoInicializadasZero()
        {
            var dto1 = new AcervoTridimensionalLinhaDTO();
            var dto2 = new AcervoTridimensionalLinhaDTO();
            var faker = new Faker("pt_BR");

            dto1.NumeroLinha = faker.Random.Number(1, 1000);
            dto2.NumeroLinha = faker.Random.Number(1001, 2000);

            dto1.NumeroLinha.Should().NotBe(dto2.NumeroLinha);
            dto1.NumeroLinha.Should().NotBe(0);
            dto2.NumeroLinha.Should().NotBe(0);
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoStatusEPendente_EntaoMudaParaSucesso()
        {
            var dto = new AcervoTridimensionalLinhaDTO
            {
                Status = ImportacaoStatus.Pendente,
                Titulo = new LinhaConteudoAjustarDTO(),
                Codigo = new LinhaConteudoAjustarDTO(),
                Procedencia = new LinhaConteudoAjustarDTO(),
                EstadoConservacao = new LinhaConteudoAjustarDTO(),
                Descricao = new LinhaConteudoAjustarDTO(),
                Quantidade = new LinhaConteudoAjustarDTO(),
                Altura = new LinhaConteudoAjustarDTO(),
                Largura = new LinhaConteudoAjustarDTO(),
                Profundidade = new LinhaConteudoAjustarDTO(),
                Diametro = new LinhaConteudoAjustarDTO(),
                Ano = new LinhaConteudoAjustarDTO()
            };

            dto.DefinirLinhaComoSucesso();

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact]
        public void DadoLinhaComPropriedades_QuandoVerificaPossuiErrosFalse_EntaoStatusEPendente()
        {
            var dto = new AcervoTridimensionalLinhaDTO();

            dto.PossuiErros.Should().BeFalse();
            dto.Status.Should().Be(default(ImportacaoStatus));
        }
    }
}
