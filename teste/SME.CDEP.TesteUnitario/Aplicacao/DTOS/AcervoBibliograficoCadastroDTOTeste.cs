using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Enumerados;
using SME.CDEP.Infra.Dominio.Enumerados;
using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoBibliograficoCadastroDtoTeste
    {
        private readonly Bogus.Faker _faker;

        public AcervoBibliograficoCadastroDtoTeste()
        {
            _faker = new Bogus.Faker("pt_BR");
        }

        [Fact(DisplayName = "MaterialId - Quando obrigatório e não informado - Deve retornar erro")]
        public void DadoMaterialIdNaoInformado_QuandoValidar_EntaoDeveRetornarErro()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.MaterialId = 0;
            var validationContext = new ValidationContext(dto);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeFalse();
            results.Should().Contain(r => r.ErrorMessage!.Contains("identificador do material do acervo bibliográfico"));
        }

        [Fact(DisplayName = "MaterialId - Quando valor negativo - Deve retornar erro")]
        public void DadoMaterialIdNegativo_QuandoValidar_EntaoDeveRetornarErro()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.MaterialId = -1;
            var validationContext = new ValidationContext(dto);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeFalse();
            results.Should().Contain(r => r.ErrorMessage!.Contains("maior que zero"));
        }

        [Fact(DisplayName = "MaterialId - Quando valor máximo permitido - Deve ser válido")]
        public void DadoMaterialIdComValorMaximo_QuandoValidar_EntaoDeveSerValido()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.MaterialId = long.MaxValue;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "IdiomaId - Quando obrigatório e não informado - Deve retornar erro")]
        public void DadoIdiomaIdNaoInformado_QuandoValidar_EntaoDeveRetornarErro()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.IdiomaId = 0;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeFalse();
            results.Should().Contain(r => r.ErrorMessage!.Contains("identificador do idioma"));
        }

        [Fact(DisplayName = "IdiomaId - Quando valor negativo - Deve retornar erro")]
        public void DadoIdiomaIdNegativo_QuandoValidar_EntaoDeveRetornarErro()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.IdiomaId = -5;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeFalse();
            results.Should().Contain(r => r.ErrorMessage!.Contains("maior que zero"));
        }

        [Fact(DisplayName = "LocalizacaoCDD - Quando obrigatório e não informado - Deve retornar erro")]
        public void DadoLocalizacaoCDDNaoInformada_QuandoValidar_EntaoDeveRetornarErro()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.LocalizacaoCDD = null!;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeFalse();
            results.Should().Contain(r => r.ErrorMessage!.Contains("localizção CDD"));
        }

        [Fact(DisplayName = "LocalizacaoCDD - Quando excede limite de caracteres - Deve retornar erro")]
        public void DadoLocalizacaoCDDComMaisDeCaracteres_QuandoValidar_EntaoDeveRetornarErro()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.LocalizacaoCDD = _faker.Lorem.Letter(51);
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeFalse();
            results.Should().Contain(r => r.ErrorMessage!.Contains("não pode conter mais que 50 caracteres"));
        }

        [Fact(DisplayName = "LocalizacaoCDD - Quando exatamente 50 caracteres - Deve ser válido")]
        public void DadoLocalizacaoCDDCom50Caracteres_QuandoValidar_EntaoDeveSerValido()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.LocalizacaoCDD = _faker.Lorem.Letter(50);
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "LocalizacaoCDD - Quando vazio - Deve retornar erro")]
        public void DadoLocalizacaoCDDVazio_QuandoValidar_EntaoDeveRetornarErro()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.LocalizacaoCDD = string.Empty;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeFalse();
            results.Should().Contain(r => r.ErrorMessage!.Contains("localizção CDD"));
        }

        [Fact(DisplayName = "EditoraId - Quando não informado - Deve ser nulo e válido")]
        public void DadoEditoraIdNaoInformado_QuandoValidar_EntaoDeveSerValido()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.EditoraId = null;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "AssuntosIds - Quando array vazio - Deve ser válido")]
        public void DadoAssuntosIdsVazio_QuandoValidar_EntaoDeveSerValido()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.AssuntosIds = Array.Empty<long>();
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "AssuntosIds - Quando múltiplos ids - Deve ser válido")]
        public void DadoAssuntosIdsComMultiplosIds_QuandoValidar_EntaoDeveSerValido()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.AssuntosIds = new long[] { 1, 2, 3, 4, 5 };
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "Edicao - Quando não informado - Deve ser nulo")]
        public void DadoEdicaoNaoInformada_QuandoValidar_EntaoDeveSerValido()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.Edicao = null;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "NumeroPagina - Quando não informado - Deve ser nulo")]
        public void DadoNumeroPaginaNaoInformado_QuandoValidar_EntaoDeveSerValido()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.NumeroPagina = null;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "Largura - Quando não informado - Deve ser nulo")]
        public void DadoLarguraNaoInformada_QuandoValidar_EntaoDeveSerValido()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.Largura = null;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "Altura - Quando não informado - Deve ser nulo")]
        public void DadoAlturaNaoInformada_QuandoValidar_EntaoDeveSerValido()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.Altura = null;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "SerieColecaoId - Quando não informado - Deve ser nulo")]
        public void DadoSerieColecaoIdNaoInformado_QuandoValidar_EntaoDeveSerValido()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.SerieColecaoId = null;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "Volume - Quando não informado - Deve ser nulo")]
        public void DadoVolumeNaoInformado_QuandoValidar_EntaoDeveSerValido()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.Volume = null;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "LocalizacaoPHA - Quando não informado - Deve ser nulo")]
        public void DadoLocalizacaoPHANaoInformada_QuandoValidar_EntaoDeveSerValido()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.LocalizacaoPHA = null;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "NotasGerais - Quando não informado - Deve ser nulo")]
        public void DadoNotasGeraisNaoInformadas_QuandoValidar_EntaoDeveSerValido()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.NotasGerais = null;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "Isbn - Quando não informado - Deve ser nulo")]
        public void DadoIsbnNaoInformado_QuandoValidar_EntaoDeveSerValido()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.Isbn = null;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "SituacaoSaldo - Quando não informado - Deve ter valor padrão DISPONIVEL")]
        public void DadoSituacaoSaldoNaoInformada_QuandoCriar_EntaoDeveSerDisponivel()
        {
            // Arrange & Act
            var dto = new AcervoBibliograficoCadastroDTO
            {
                Titulo = _faker.Lorem.Sentence(),
                Ano = DateTime.Now.Year.ToString(),
                MaterialId = 1,
                IdiomaId = 1,
                LocalizacaoCDD = "000.00"
            };

            // Assert
            dto.SituacaoSaldo.Should().Be(SituacaoSaldo.DISPONIVEL);
        }

        [Theory(DisplayName = "SituacaoSaldo - Quando diferentes enumerados - Deve aceitar todos")]
        [InlineData(SituacaoSaldo.DISPONIVEL)]
        [InlineData(SituacaoSaldo.EMPRESTADO)]
        [InlineData(SituacaoSaldo.RESERVADO)]
        [InlineData(SituacaoSaldo.INDISPONIVEL_PARA_RESERVA_EMPRESTIMO)]
        public void DadoSituacaoSaldoComDiferentesEnumerados_QuandoAtribuir_EntaoDeveSerValido(SituacaoSaldo situacao)
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.SituacaoSaldo = situacao;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            dto.SituacaoSaldo.Should().Be(situacao);
        }

        [Fact(DisplayName = "SituacaoAcervo - Quando não informado - Deve ter valor padrão Ativo")]
        public void DadoSituacaoAcervoNaoInformada_QuandoCriar_EntaoDeveSerAtivo()
        {
            // Arrange & Act
            var dto = new AcervoBibliograficoCadastroDTO
            {
                Titulo = _faker.Lorem.Sentence(),
                Ano = DateTime.Now.Year.ToString(),
                MaterialId = 1,
                IdiomaId = 1,
                LocalizacaoCDD = "000.00"
            };

            // Assert
            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Ativo);
        }

        [Theory(DisplayName = "SituacaoAcervo - Quando diferentes enumerados - Deve aceitar todos")]
        [InlineData(SituacaoAcervo.Ativo)]
        [InlineData(SituacaoAcervo.Inativo)]
        public void DadoSituacaoAcervoComDiferentesEnumerados_QuandoAtribuir_EntaoDeveSerValido(SituacaoAcervo situacao)
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.SituacaoAcervo = situacao;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            dto.SituacaoAcervo.Should().Be(situacao);
        }

        [Fact(DisplayName = "Herança - Quando derivado de AcervoCadastroDTO - Deve herdar Titulo obrigatório")]
        public void DadoDtoComTituloNaoInformado_QuandoValidarHeranca_EntaoDeveRetornarErro()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.Titulo = null!;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeFalse();
            results.Should().Contain(r => r.ErrorMessage!.Contains("título do acervo"));
        }

        [Fact(DisplayName = "Herança - Quando derivado de AcervoCadastroDTO - Deve herdar Ano obrigatório")]
        public void DadoDtoComAnoNaoInformado_QuandoValidarHeranca_EntaoDeveRetornarErro()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.Ano = null!;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeFalse();
            results.Should().Contain(r => r.ErrorMessage!.Contains("ano do acervo"));
        }

        [Fact(DisplayName = "Herança - Quando propriedades opcionais da classe base - Deve ser válido")]
        public void DadoPropriedadesOpcionaisDaClasseBase_QuandoNaoInformadas_EntaoDeveSerValido()
        {
            // Arrange
            var dto = GerarAcervoBibliograficoCadastroDtoValido();
            dto.Descricao = null;
            dto.Codigo = null;
            dto.CodigoNovo = null;
            dto.CreditosAutoresIds = null;
            dto.CoAutores = null;
            dto.SubTitulo = null;
            dto.DataAcervo = null;
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "DTO Completo - Quando todas as propriedades informadas - Deve ser válido")]
        public void DadoDtoComTodasAsPropriedades_QuandoValidar_EntaoDeveSerValido()
        {
            // Arrange
            var dto = new AcervoBibliograficoCadastroDTO
            {
                Titulo = _faker.Lorem.Sentence(),
                Descricao = _faker.Lorem.Paragraph(),
                Codigo = _faker.Random.AlphaNumeric(10),
                CodigoNovo = _faker.Random.AlphaNumeric(10),
                CreditosAutoresIds = new long[] { 1, 2 },
                SubTitulo = _faker.Lorem.Sentence(),
                DataAcervo = DateTime.Now.ToString("dd/MM/yyyy"),
                Ano = DateTime.Now.Year.ToString(),
                SituacaoAcervo = SituacaoAcervo.Ativo,
                MaterialId = 1,
                EditoraId = 1,
                AssuntosIds = new long[] { 1, 2, 3 },
                Edicao = "1ª Edição",
                NumeroPagina = 300,
                Largura = "20,00",
                Altura = "30,00",
                SerieColecaoId = 1,
                Volume = "Volume 1",
                IdiomaId = 1,
                LocalizacaoCDD = "000.00",
                LocalizacaoPHA = "A1-B2",
                NotasGerais = "Notas importantes",
                Isbn = "978-3-16-148410-0",
                SituacaoSaldo = SituacaoSaldo.DISPONIVEL
            };
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "DTO Mínimo - Quando apenas obrigatórios informados - Deve ser válido")]
        public void DadoDtoComApenasObrigatorios_QuandoValidar_EntaoDeveSerValido()
        {
            // Arrange
            var dto = new AcervoBibliograficoCadastroDTO
            {
                Titulo = _faker.Lorem.Sentence(),
                Ano = DateTime.Now.Year.ToString(),
                MaterialId = 1,
                IdiomaId = 1,
                LocalizacaoCDD = "000.00"
            };
            var validationContext = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeTrue();
            results.Should().BeEmpty();
            dto.SituacaoSaldo.Should().Be(SituacaoSaldo.DISPONIVEL);
            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Ativo);
        }

        [Fact(DisplayName = "Múltiplos erros - Quando várias validações falham - Deve retornar todos os erros")]
        public void DadoDtoComMultiplosErros_QuandoValidar_EntaoDeveRetornarTodosOsErros()
        {
            // Arrange
            var dto = new AcervoBibliograficoCadastroDTO
            {
                Titulo = null!,
                Ano = null!,
                MaterialId = -5,
                IdiomaId = 0,
                LocalizacaoCDD = _faker.Lorem.Letter(51)
            };
            var validationContext = new ValidationContext(dto);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            // Act
            var ehValido = Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

            // Assert
            ehValido.Should().BeFalse();
            results.Should().HaveCountGreaterThanOrEqualTo(2);
        }

        private AcervoBibliograficoCadastroDTO GerarAcervoBibliograficoCadastroDtoValido()
        {
            return new Bogus.Faker<AcervoBibliograficoCadastroDTO>("pt_BR")
                .RuleFor(x => x.Titulo, f =>
                {
                    var titulo = f.Lorem.Sentence(3);
                    return titulo.Length > 500 ? titulo.Substring(0, 500) : titulo;
                })
                .RuleFor(x => x.Descricao, f => f.Lorem.Paragraph())
                .RuleFor(x => x.Codigo, f => f.Random.AlphaNumeric(10))
                .RuleFor(x => x.CodigoNovo, f => f.Random.AlphaNumeric(10))
                .RuleFor(x => x.CreditosAutoresIds, f => new long[] { f.Random.Long(1, 100) })
                .RuleFor(x => x.SubTitulo, f => f.Lorem.Sentence())
                .RuleFor(x => x.DataAcervo, f => f.Date.Past().ToString("dd/MM/yyyy"))
                .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
                .RuleFor(x => x.SituacaoAcervo, f => SituacaoAcervo.Ativo)
                .RuleFor(x => x.MaterialId, f => f.Random.Long(1, 100))
                .RuleFor(x => x.EditoraId, f => f.Random.Long(1, 100))
                .RuleFor(x => x.AssuntosIds, f => new long[] { f.Random.Long(1, 100), f.Random.Long(1, 100) })
                .RuleFor(x => x.Edicao, f => $"{f.Random.Int(1, 10)}ª Edição")
                .RuleFor(x => x.NumeroPagina, f => f.Random.Int(10, 1000))
                .RuleFor(x => x.Largura, f => f.Random.Double(10, 50).ToString("F2"))
                .RuleFor(x => x.Altura, f => f.Random.Double(10, 50).ToString("F2"))
                .RuleFor(x => x.SerieColecaoId, f => f.Random.Long(1, 100))
                .RuleFor(x => x.Volume, f => $"Volume {f.Random.Int(1, 10)}")
                .RuleFor(x => x.IdiomaId, f => f.Random.Long(1, 100))
                .RuleFor(x => x.LocalizacaoCDD, f => f.Random.Replace("###.##"))
                .RuleFor(x => x.LocalizacaoPHA, f => f.Random.AlphaNumeric(10))
                .RuleFor(x => x.NotasGerais, f => f.Lorem.Paragraph())
                .RuleFor(x => x.Isbn, f => "978-3-16-148410-0")
                .RuleFor(x => x.SituacaoSaldo, f => SituacaoSaldo.DISPONIVEL)
                .Generate();
        }
    }
}