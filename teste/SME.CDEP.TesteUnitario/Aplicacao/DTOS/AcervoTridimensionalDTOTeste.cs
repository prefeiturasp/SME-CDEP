using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoTridimensionalDtoTeste
    {
        [Fact]
        public void DadoAcervoTridimensionalDTO_QuandoInstanciar_EntaoTodasAsPropriedadesSaoInicializadasCorretamente()
        {
            var dto = new AcervoTridimensionalDTO();

            dto.Should().NotBeNull();
            dto.Id.Should().Be(0);
            dto.AcervoId.Should().Be(0);
            dto.Titulo.Should().BeNull();
            dto.TipoAcervoId.Should().Be(0);
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
            dto.Arquivos.Should().BeNull();
            dto.Auditoria.Should().BeNull();
            dto.Ano.Should().BeNull();
            dto.SituacaoAcervo.Should().Be(default(SituacaoAcervo));
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesNumericasLong_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalDTO();
            var faker = new Faker();
            var idValor = faker.Random.Long(1, 1000);
            var acervoIdValor = faker.Random.Long(1, 1000);
            var tipoAcervoIdValor = faker.Random.Long(1, 100);
            var quantidadeValor = faker.Random.Long(1, 1000);

            dto.Id = idValor;
            dto.AcervoId = acervoIdValor;
            dto.TipoAcervoId = tipoAcervoIdValor;
            dto.Quantidade = quantidadeValor;

            dto.Id.Should().Be(idValor);
            dto.AcervoId.Should().Be(acervoIdValor);
            dto.TipoAcervoId.Should().Be(tipoAcervoIdValor);
            dto.Quantidade.Should().Be(quantidadeValor);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesString_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalDTO();
            var faker = new Faker("pt_BR");
            var titulo = faker.Lorem.Sentence();
            var codigo = faker.Random.Replace("TB-####");
            var procedencia = faker.Lorem.Text();
            var dataAcervo = faker.Date.Recent().ToString("dd/MM/yyyy");
            var descricao = faker.Lorem.Paragraph();
            var ano = faker.Date.Recent().Year.ToString();

            dto.Titulo = titulo;
            dto.Codigo = codigo;
            dto.Procedencia = procedencia;
            dto.DataAcervo = dataAcervo;
            dto.Descricao = descricao;
            dto.Ano = ano;

            dto.Titulo.Should().Be(titulo);
            dto.Codigo.Should().Be(codigo);
            dto.Procedencia.Should().Be(procedencia);
            dto.DataAcervo.Should().Be(dataAcervo);
            dto.Descricao.Should().Be(descricao);
            dto.Ano.Should().Be(ano);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesDimensoes_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalDTO();
            var faker = new Faker();
            var largura = faker.Random.Double(0.1, 100).ToString("F2");
            var altura = faker.Random.Double(0.1, 100).ToString("F2");
            var profundidade = faker.Random.Double(0.1, 100).ToString("F2");
            var diametro = faker.Random.Double(0.1, 100).ToString("F2");

            dto.Largura = largura;
            dto.Altura = altura;
            dto.Profundidade = profundidade;
            dto.Diametro = diametro;

            dto.Largura.Should().Be(largura);
            dto.Altura.Should().Be(altura);
            dto.Profundidade.Should().Be(profundidade);
            dto.Diametro.Should().Be(diametro);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadeOptional_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalDTO();
            var faker = new Faker();
            var conservacaoId = faker.Random.Long(1, 100);

            dto.ConservacaoId = conservacaoId;

            dto.ConservacaoId.Should().Be(conservacaoId);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirArquivos_EntaoOsArquivosSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalDTO();
            var arquivos = new ArquivoResumidoDTO[]
            {
                new() { },
                new() { }
            };

            dto.Arquivos = arquivos;

            dto.Arquivos.Should().NotBeNull();
            dto.Arquivos.Should().HaveCount(2);
            dto.Arquivos.Should().BeEquivalentTo(arquivos);
        }

        [Fact]
        public void DadoValorValido_QuandoAtribuirAuditoria_EntaoAAuditoriaEhArmazenadaCorretamente()
        {
            var dto = new AcervoTridimensionalDTO();
            var auditoria = new AuditoriaDTO();

            dto.Auditoria = auditoria;

            dto.Auditoria.Should().NotBeNull();
            dto.Auditoria.Should().BeEquivalentTo(auditoria);
        }

        [Fact]
        public void DadoValorValido_QuandoAtribuirSituacaoAcervo_EntaoASituacaoEhArmazenadaCorretamente()
        {
            var dto = new AcervoTridimensionalDTO();
            var situacao = SituacaoAcervo.Ativo;

            dto.SituacaoAcervo = situacao;

            dto.SituacaoAcervo.Should().Be(situacao);
        }

        [Fact]
        public void DadoMultiplosValores_QuandoAtribuirTodasAsPropriedades_EntaoTodosOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalDTO();
            var faker = new Faker("pt_BR");
            var id = faker.Random.Long(1, 1000);
            var acervoId = faker.Random.Long(1, 1000);
            var titulo = faker.Lorem.Sentence();
            var tipoAcervoId = faker.Random.Long(1, 100);
            var codigo = faker.Random.Replace("TB-####");
            var procedencia = faker.Lorem.Text();
            var dataAcervo = faker.Date.Recent().ToString("dd/MM/yyyy");
            var conservacaoId = faker.Random.Long(1, 100);
            var quantidade = faker.Random.Long(1, 1000);
            var descricao = faker.Lorem.Paragraph();
            var largura = faker.Random.Double(0.1, 100).ToString("F2");
            var altura = faker.Random.Double(0.1, 100).ToString("F2");
            var profundidade = faker.Random.Double(0.1, 100).ToString("F2");
            var diametro = faker.Random.Double(0.1, 100).ToString("F2");
            var arquivos = new ArquivoResumidoDTO[] { new(), new() };
            var auditoria = new AuditoriaDTO();
            var ano = faker.Date.Recent().Year.ToString();
            var situacao = SituacaoAcervo.Ativo;

            dto.Id = id;
            dto.AcervoId = acervoId;
            dto.Titulo = titulo;
            dto.TipoAcervoId = tipoAcervoId;
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
            dto.Arquivos = arquivos;
            dto.Auditoria = auditoria;
            dto.Ano = ano;
            dto.SituacaoAcervo = situacao;

            dto.Id.Should().Be(id);
            dto.AcervoId.Should().Be(acervoId);
            dto.Titulo.Should().Be(titulo);
            dto.TipoAcervoId.Should().Be(tipoAcervoId);
            dto.Codigo.Should().Be(codigo);
            dto.Procedencia.Should().Be(procedencia);
            dto.DataAcervo.Should().Be(dataAcervo);
            dto.ConservacaoId.Should().Be(conservacaoId);
            dto.Quantidade.Should().Be(quantidade);
            dto.Descricao.Should().Be(descricao);
            dto.Largura.Should().Be(largura);
            dto.Altura.Should().Be(altura);
            dto.Profundidade.Should().Be(profundidade);
            dto.Diametro.Should().Be(diametro);
            dto.Arquivos.Should().BeEquivalentTo(arquivos);
            dto.Auditoria.Should().BeEquivalentTo(auditoria);
            dto.Ano.Should().Be(ano);
            dto.SituacaoAcervo.Should().Be(situacao);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void DadoValoresNulosOuVazios_QuandoAtribuirPropriedadesString_EntaoOsValoresSaoArmazenadosCorretamente(string? valor)
        {
            var dto = new AcervoTridimensionalDTO();

            dto.Titulo = valor!;
            dto.Codigo = valor!;
            dto.Procedencia = valor!;
            dto.DataAcervo = valor!;
            dto.Descricao = valor!;
            dto.Ano = valor!;

            dto.Titulo.Should().Be(valor);
            dto.Codigo.Should().Be(valor);
            dto.Procedencia.Should().Be(valor);
            dto.DataAcervo.Should().Be(valor);
            dto.Descricao.Should().Be(valor);
            dto.Ano.Should().Be(valor);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1.50")]
        [InlineData("99.99")]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesDimensoesOpcionais_EntaoOsValoresSaoArmazenadosCorretamente(string? valor)
        {
            var dto = new AcervoTridimensionalDTO();

            dto.Largura = valor!;
            dto.Altura = valor!;
            dto.Profundidade = valor!;
            dto.Diametro = valor!;

            dto.Largura.Should().Be(valor);
            dto.Altura.Should().Be(valor);
            dto.Profundidade.Should().Be(valor);
            dto.Diametro.Should().Be(valor);
        }

        [Fact]
        public void DadoArquivosVazio_QuandoAtribuirArquivos_EntaoOArrayVazioEhArmazenado()
        {
            var dto = new AcervoTridimensionalDTO();
            var arquivosVazios = new ArquivoResumidoDTO[] { };

            dto.Arquivos = arquivosVazios;

            dto.Arquivos.Should().NotBeNull();
            dto.Arquivos.Should().BeEmpty();
        }

        [Fact]
        public void DadoValesNulos_QuandoAtribuirArquivosComNull_EntaoNullEhArmazenado()
        {
            var dto = new AcervoTridimensionalDTO();

            dto.Arquivos = null;

            dto.Arquivos.Should().BeNull();
        }

        [Fact]
        public void DadoSituacaoAcervoInativo_QuandoAtribuir_EntaoOValorEhArmazenado()
        {
            var dto = new AcervoTridimensionalDTO();

            dto.SituacaoAcervo = SituacaoAcervo.Inativo;

            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Inativo);
        }

        [Fact]
        public void DadoNumeroZero_QuandoAtribuirPropriedadesNumericasLong_EntaoOValorZeroEhArmazenado()
        {
            var dto = new AcervoTridimensionalDTO();

            dto.Id = 0;
            dto.AcervoId = 0;
            dto.TipoAcervoId = 0;
            dto.Quantidade = 0;

            dto.Id.Should().Be(0);
            dto.AcervoId.Should().Be(0);
            dto.TipoAcervoId.Should().Be(0);
            dto.Quantidade.Should().Be(0);
        }

        [Fact]
        public void DadoValoresNegativosEmNullable_QuandoAtribuirPropriedadesNumericasLong_EntaoOsValoresNegatovosSaoArmazenadosCorretamente()
        {
            var dto = new AcervoTridimensionalDTO();

            dto.ConservacaoId = -1;
            dto.Quantidade = -100;

            dto.ConservacaoId.Should().Be(-1);
            dto.Quantidade.Should().Be(-100);
        }

        [Fact]
        public void DadoPropriedadeAuditoriaComDados_QuandoAtribuirMultiplesVezes_EntaoOUltimoValorEhRetido()
        {
            var dto = new AcervoTridimensionalDTO();
            var auditoria1 = new AuditoriaDTO 
            { 
                CriadoPor = "Usuario1",
                CriadoLogin = "login1",
                CriadoEm = new DateTime(2024, 1, 1)
            };
            var auditoria2 = new AuditoriaDTO 
            { 
                CriadoPor = "Usuario2",
                CriadoLogin = "login2",
                CriadoEm = new DateTime(2024, 12, 31)
            };

            dto.Auditoria = auditoria1;
            dto.Auditoria = auditoria2;

            dto.Auditoria.Should().BeEquivalentTo(auditoria2);
            dto.Auditoria.CriadoPor.Should().Be("Usuario2");
        }
    }
}
