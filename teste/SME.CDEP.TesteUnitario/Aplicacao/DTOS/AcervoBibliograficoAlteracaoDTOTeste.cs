using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoBibliograficoAlteracaoDTOTeste
    {
        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Deve conter propriedade Id")]
        public void DadoDTOAcervoBibliograficoAlteracao_QuandoCriar_EntaoContemPropriedadeId()
        {
            var dtoType = typeof(AcervoBibliograficoAlteracaoDTO);
            var idProperty = dtoType.GetProperty("Id");

            idProperty.Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Deve conter propriedade AcervoId")]
        public void DadoDTOAcervoBibliograficoAlteracao_QuandoCriar_EntaoContemPropriedadeAcervoId()
        {
            var dtoType = typeof(AcervoBibliograficoAlteracaoDTO);
            var acervoIdProperty = dtoType.GetProperty("AcervoId");

            acervoIdProperty.Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Id com valor válido deve passar validação")]
        public void DadoIdComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO { Id = 1, AcervoId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("identificador do acervo documental")).Should().BeEmpty();
            results.Where(r => r.ErrorMessage.Contains("identificador do acervo")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Id não deve permitir valor zero")]
        public void DadoIdComValorZero_QuandoValidar_EntaoDeveRetornarErro()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO { Id = 0, AcervoId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("O identificador do acervo documental deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Id não deve permitir valor negativo")]
        public void DadoIdComValorNegativo_QuandoValidar_EntaoDeveRetornarErro()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO { Id = -1, AcervoId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("O identificador do acervo documental deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Id obrigatório")]
        public void DadoIdNaoPreenchido_QuandoValidar_EntaoDeveRetornarErroObrigatorio()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO { AcervoId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("O identificador do acervo documental deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - AcervoId com valor válido deve passar validação")]
        public void DadoAcervoIdComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO { Id = 1, AcervoId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("identificador do acervo")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - AcervoId não deve permitir valor zero")]
        public void DadoAcervoIdComValorZero_QuandoValidar_EntaoDeveRetornarErro()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO { Id = 1, AcervoId = 0 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("O identificador do acervo deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - AcervoId não deve permitir valor negativo")]
        public void DadoAcervoIdComValorNegativo_QuandoValidar_EntaoDeveRetornarErro()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO { Id = 1, AcervoId = -5 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("O identificador do acervo deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - AcervoId obrigatório")]
        public void DadoAcervoIdNaoPreenchido_QuandoValidar_EntaoDeveRetornarErroObrigatorio()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO { Id = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("O identificador do acervo deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Herda de AcervoBibliograficoCadastroDTO")]
        public void DadoDTOAcervoBibliograficoAlteracao_QuandoCriar_EntaoDeveHerdarDeAcervoBibliograficoCadastroDTO()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO();

            dto.Should().BeAssignableTo<AcervoBibliograficoCadastroDTO>();
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Deve permitir atribuição de valores às propriedades")]
        public void DadoDTOAcervoBibliograficoAlteracao_QuandoAtribuirValores_EntaoDeveCenasCorretamente()
        {
            var id = 100L;
            var acervoId = 50L;
            var titulo = "Título do Livro";
            var codigo = "BIB001";

            var dto = new AcervoBibliograficoAlteracaoDTO
            {
                Id = id,
                AcervoId = acervoId,
                Titulo = titulo,
                Codigo = codigo
            };

            dto.Id.Should().Be(id);
            dto.AcervoId.Should().Be(acervoId);
            dto.Titulo.Should().Be(titulo);
            dto.Codigo.Should().Be(codigo);
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Deve permitir valores máximos para long")]
        public void DadoDTOAcervoBibliograficoAlteracao_QuandoAtribuirValoresMaximos_EntaoDeveAceitarSemErro()
        {
            var dtoComValoresMaximos = new AcervoBibliograficoAlteracaoDTO
            {
                Id = long.MaxValue,
                AcervoId = long.MaxValue
            };
            var context = new ValidationContext(dtoComValoresMaximos);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dtoComValoresMaximos, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("O identificador do acervo deve ser maior que zero")).Should().BeEmpty();
            results.Where(r => r.ErrorMessage.Contains("O identificador do acervo documental deve ser maior que zero")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Deve validar Id e AcervoId independentemente")]
        public void DadoDTOAcervoBibliograficoAlteracao_QuandoIdValidoMasAcervoIdInvalido_EntaoDeveRetornarApenasErroDoAcervoId()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO 
            { 
                Id = 1, 
                AcervoId = 0,
                Titulo = "Teste",
                MaterialId = 1,
                IdiomaId = 1,
                LocalizacaoCDD = "000.00"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("O identificador do acervo deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Propriedades devem ser públicas")]
        public void DadoDTOAcervoBibliograficoAlteracao_QuandoVerificarVisibilidadePropriedades_EntaoDeveSerPublicas()
        {
            var tipo = typeof(AcervoBibliograficoAlteracaoDTO);
            var idProperty = tipo.GetProperty("Id");
            var acervoIdProperty = tipo.GetProperty("AcervoId");

            idProperty.Should().NotBeNull();
            idProperty!.CanRead.Should().BeTrue();
            idProperty!.CanWrite.Should().BeTrue();

            acervoIdProperty.Should().NotBeNull();
            acervoIdProperty!.CanRead.Should().BeTrue();
            acervoIdProperty!.CanWrite.Should().BeTrue();
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Atributos Required devem estar presentes")]
        public void DadoDTOAcervoBibliograficoAlteracao_QuandoVerificarAtributos_EntaoDeveConterRequiredAttribute()
        {
            var tipo = typeof(AcervoBibliograficoAlteracaoDTO);
            var idProperty = tipo.GetProperty("Id");
            var acervoIdProperty = tipo.GetProperty("AcervoId");

            idProperty.Should().NotBeNull();
            var idAttributes = idProperty!.GetCustomAttributes(typeof(RequiredAttribute), true);
            idAttributes.Should().NotBeNullOrEmpty();

            acervoIdProperty.Should().NotBeNull();
            var acervoIdAttributes = acervoIdProperty!.GetCustomAttributes(typeof(RequiredAttribute), true);
            acervoIdAttributes.Should().NotBeNullOrEmpty();
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Atributos Range devem estar presentes")]
        public void DadoDTOAcervoBibliograficoAlteracao_QuandoVerificarAtributos_EntaoDeveConterRangeAttribute()
        {
            var tipo = typeof(AcervoBibliograficoAlteracaoDTO);
            var idProperty = tipo.GetProperty("Id");
            var acervoIdProperty = tipo.GetProperty("AcervoId");

            idProperty.Should().NotBeNull();
            var idAttributes = idProperty!.GetCustomAttributes(typeof(RangeAttribute), true);
            idAttributes.Should().NotBeNullOrEmpty();

            acervoIdProperty.Should().NotBeNull();
            var acervoIdAttributes = acervoIdProperty!.GetCustomAttributes(typeof(RangeAttribute), true);
            acervoIdAttributes.Should().NotBeNullOrEmpty();
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Mensagens de validação devem ser corretas")]
        public void DadoDTOAcervoBibliograficoAlteracao_QuandoValidarComErro_EntaoMensagensSaoCorretas()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO();
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().HaveCountGreaterThan(1);
            results.Should().Contain(r => r.ErrorMessage.Contains("O identificador do acervo documental deve ser maior que zero"));
            results.Should().Contain(r => r.ErrorMessage.Contains("O identificador do acervo deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Id com valor 1 deve passar validação")]
        public void DadoIdComValor1_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO { Id = 1, AcervoId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.MemberNames.Contains("Id")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - AcervoId com valor 1 deve passar validação")]
        public void DadoAcervoIdComValor1_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO { Id = 1, AcervoId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.MemberNames.Contains("AcervoId")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Id com valor -1 deve gerar erro de Range")]
        public void DadoIdComValorMenosUm_QuandoValidar_EntaoDeveGerarErroDeRange()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO { Id = -1, AcervoId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Equals("O identificador do acervo documental deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - AcervoId com valor -1 deve gerar erro de Range")]
        public void DadoAcervoIdComValorMenosUm_QuandoValidar_EntaoDeveGerarErroDeRange()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO { Id = 1, AcervoId = -1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Equals("O identificador do acervo deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Deve conter propriedades herdadas de AcervoBibliograficoCadastroDTO")]
        public void DadoDTOAcervoBibliograficoAlteracao_QuandoCriar_EntaoDeveConterPropriedadesHerdadas()
        {
            var tipo = typeof(AcervoBibliograficoAlteracaoDTO);
            var bindingFlags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;

            var properties = new[] { "MaterialId", "EditoraId", "AssuntosIds", "Edicao", "NumeroPagina", "Largura", "Altura", "SerieColecaoId", "Volume", "IdiomaId", "LocalizacaoCDD", "LocalizacaoPHA", "NotasGerais", "Isbn", "SituacaoSaldo" };

            foreach (var propertyName in properties)
            {
                var property = tipo.GetProperty(propertyName, bindingFlags);
                property.Should().NotBeNull($"Property {propertyName} should exist");
            }

            var situacaoAcervoProperty = typeof(AcervoBibliograficoCadastroDTO).GetProperty(
                "SituacaoAcervo", 
                bindingFlags | System.Reflection.BindingFlags.DeclaredOnly);
            situacaoAcervoProperty.Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Ambas propriedades devem ser long")]
        public void DadoDTOAcervoBibliograficoAlteracao_QuandoVerificarTipo_EntaoIdEAcervoIdDeveSerLong()
        {
            var tipo = typeof(AcervoBibliograficoAlteracaoDTO);
            var idProperty = tipo.GetProperty("Id");
            var acervoIdProperty = tipo.GetProperty("AcervoId");

            idProperty.Should().NotBeNull();
            idProperty!.PropertyType.Should().Be(typeof(long));

            acervoIdProperty.Should().NotBeNull();
            acervoIdProperty!.PropertyType.Should().Be(typeof(long));
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Validação com múltiplos erros")]
        public void DadoDTOAcervoBibliograficoAlteracao_QuandoAmbosIdInvalidos_EntaoDeveRetornarMultiplosErros()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO { Id = 0, AcervoId = 0 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().HaveCountGreaterThanOrEqualTo(2);
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Id e AcervoId com valores grandes")]
        public void DadoDTOAcervoBibliograficoAlteracao_QuandoIdEAcervoIdComValoresGrandes_EntaoDevePassarValidacao()
        {
            var id = 999999999L;
            var acervoId = 888888888L;
            var dto = new AcervoBibliograficoAlteracaoDTO { Id = id, AcervoId = acervoId };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.MemberNames.Contains("Id") || r.MemberNames.Contains("AcervoId")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Erro de Id não deve afetar AcervoId")]
        public void DadoDTOAcervoBibliograficoAlteracao_QuandoIdComErro_EntaoAcervoIdNaoDeveConterErro()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO { Id = -10, AcervoId = 100 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            var idErrors = results.Where(r => r.ErrorMessage.Contains("identificador do acervo documental")).ToList();
            var acervoIdErrors = results.Where(r => r.ErrorMessage.Contains("identificador do acervo") && 
                                                     !r.ErrorMessage.Contains("documental")).ToList();

            idErrors.Should().NotBeEmpty();
            acervoIdErrors.Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoBibliograficoAlteracaoDTO - Erro de AcervoId não deve afetar Id")]
        public void DadoDTOAcervoBibliograficoAlteracao_QuandoAcervoIdComErro_EntaoIdNaoDeveConterErro()
        {
            var dto = new AcervoBibliograficoAlteracaoDTO { Id = 100, AcervoId = -10 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            var idErrors = results.Where(r => r.ErrorMessage.Contains("identificador do acervo documental")).ToList();
            var acervoIdErrors = results.Where(r => r.ErrorMessage.Contains("O identificador do acervo deve ser maior que zero")).ToList();

            idErrors.Should().BeEmpty();
            acervoIdErrors.Should().NotBeEmpty();
        }
    }
}
