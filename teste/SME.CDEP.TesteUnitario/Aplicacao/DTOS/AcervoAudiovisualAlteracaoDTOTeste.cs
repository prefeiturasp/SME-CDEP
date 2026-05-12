using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoAudiovisualAlteracaoDTOTeste
    {
        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - Deve conter propriedade Id")]
        public void DadoDTOAcervoAudiovisualAlteracao_QuandoCriar_EntaoContemPropriedadeId()
        {
            var dto = new AcervoAudiovisualAlteracaoDTO();

            typeof(AcervoAudiovisualAlteracaoDTO).GetProperty("Id").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - Deve conter propriedade AcervoId")]
        public void DadoDTOAcervoAudiovisualAlteracao_QuandoCriar_EntaoContemPropriedadeAcervoId()
        {
            var dto = new AcervoAudiovisualAlteracaoDTO();

            typeof(AcervoAudiovisualAlteracaoDTO).GetProperty("AcervoId").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - Id com valor válido deve passar validação")]
        public void DadoIdComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualAlteracaoDTO { Id = 1, AcervoId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("identificador do acervo arte gráfica") ?? false).Should().BeEmpty();
            results.Where(r => r.ErrorMessage?.Contains("identificador do acervo") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - Id não deve permitir valor zero")]
        public void DadoIdComValorZero_QuandoValidar_EntaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualAlteracaoDTO { Id = 0, AcervoId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage != null && r.ErrorMessage.Contains("O identificador do acervo arte gráfica deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - Id não deve permitir valor negativo")]
        public void DadoIdComValorNegativo_QuandoValidar_EntaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualAlteracaoDTO { Id = -1, AcervoId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage != null && r.ErrorMessage.Contains("O identificador do acervo arte gráfica deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - Id obrigatório")]
        public void DadoIdNaoPreenchido_QuandoValidar_EntaoDeveRetornarErroObrigatorio()
        {
            var dto = new AcervoAudiovisualAlteracaoDTO { AcervoId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage != null && r.ErrorMessage.Contains("O identificador do acervo arte gráfica deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - AcervoId com valor válido deve passar validação")]
        public void DadoAcervoIdComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualAlteracaoDTO { Id = 1, AcervoId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("identificador do acervo") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - AcervoId não deve permitir valor zero")]
        public void DadoAcervoIdComValorZero_QuandoValidar_EntaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualAlteracaoDTO { Id = 1, AcervoId = 0 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage != null && r.ErrorMessage.Contains("O identificador do acervo deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - AcervoId não deve permitir valor negativo")]
        public void DadoAcervoIdComValorNegativo_QuandoValidar_EntaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualAlteracaoDTO { Id = 1, AcervoId = -5 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage != null && r.ErrorMessage.Contains("O identificador do acervo deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - AcervoId obrigatório")]
        public void DadoAcervoIdNaoPreenchido_QuandoValidar_EntaoDeveRetornarErroObrigatorio()
        {
            var dto = new AcervoAudiovisualAlteracaoDTO { Id = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage != null && r.ErrorMessage.Contains("O identificador do acervo deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - Herda de AcervoAudiovisualCadastroDTO")]
        public void DadoDTOAcervoAudiovisualAlteracao_QuandoCriar_EntaoDeveHerdarDeAcervoAudiovisualCadastroDTO()
        {
            var dto = new AcervoAudiovisualAlteracaoDTO();

            dto.Should().BeAssignableTo<AcervoAudiovisualCadastroDTO>();
        }

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - Deve conter propriedades herdadas de AcervoAudiovisualCadastroDTO")]
        public void DadoDTOAcervoAudiovisualAlteracao_QuandoCriar_EntaoDeveConterPropriedadesHerdadas()
        {
            var tipo = typeof(AcervoAudiovisualAlteracaoDTO);

            tipo.GetProperty("Localizacao").Should().NotBeNull();
            tipo.GetProperty("Procedencia").Should().NotBeNull();
            tipo.GetProperty("Copia").Should().NotBeNull();
            tipo.GetProperty("PermiteUsoImagem").Should().NotBeNull();
            tipo.GetProperty("ConservacaoId").Should().NotBeNull();
            tipo.GetProperty("SuporteId").Should().NotBeNull();
            tipo.GetProperty("Duracao").Should().NotBeNull();
            tipo.GetProperty("CromiaId").Should().NotBeNull();
            tipo.GetProperty("TamanhoArquivo").Should().NotBeNull();
            tipo.GetProperty("Acessibilidade").Should().NotBeNull();
            tipo.GetProperty("Disponibilizacao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - Deve permitir atribuição de valores às propriedades")]
        public void DadoDTOAcervoAudiovisualAlteracao_QuandoAtribuirValores_EntaoDeveDefinirCorretamente()
        {
            var id = 100L;
            var acervoId = 50L;
            var titulo = "Título do Audiovisual";
            var codigo = "AUD001";

            var dto = new AcervoAudiovisualAlteracaoDTO
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

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - Deve permitir valores máximos para long")]
        public void DadoDTOAcervoAudiovisualAlteracao_QuandoAtribuirValoresMaximos_EntaoDeveAceitarSemErro()
        {
            var dtoComValoresMaximos = new AcervoAudiovisualAlteracaoDTO
            {
                Id = long.MaxValue,
                AcervoId = long.MaxValue
            };
            var context = new ValidationContext(dtoComValoresMaximos);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dtoComValoresMaximos, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("O identificador do acervo deve ser maior que zero") ?? false).Should().BeEmpty();
            results.Where(r => r.ErrorMessage?.Contains("O identificador do acervo arte gráfica deve ser maior que zero") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - Deve validar Id e AcervoId independentemente")]
        public void DadoDTOAcervoAudiovisualAlteracao_QuandoIdValidoMasAcervoIdInvalido_EntaoDeveRetornarApenasErroDoAcervoId()
        {
            var dto = new AcervoAudiovisualAlteracaoDTO 
            { 
                Id = 1, 
                AcervoId = 0,
                Titulo = "Teste",
                Ano = "2024",
                SuporteId = 1
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage != null && r.ErrorMessage.Contains("O identificador do acervo deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - Propriedades devem ser públicas")]
        public void DadoDTOAcervoAudiovisualAlteracao_QuandoVerificarVisibilidadePropriedades_EntaoDeveSerPublicas()
        {
            var tipo = typeof(AcervoAudiovisualAlteracaoDTO);

            var idProperty = tipo.GetProperty("Id");
            var acervoIdProperty = tipo.GetProperty("AcervoId");

            idProperty?.CanRead.Should().BeTrue();
            idProperty?.CanWrite.Should().BeTrue();
            acervoIdProperty?.CanRead.Should().BeTrue();
            acervoIdProperty?.CanWrite.Should().BeTrue();
        }

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - Atributos Required devem estar presentes")]
        public void DadoDTOAcervoAudiovisualAlteracao_QuandoVerificarAtributos_EntaoDeveConterRequiredAttribute()
        {
            var tipo = typeof(AcervoAudiovisualAlteracaoDTO);
            var idProperty = tipo.GetProperty("Id");
            var acervoIdProperty = tipo.GetProperty("AcervoId");

            var idAttributes = idProperty?.GetCustomAttributes(typeof(RequiredAttribute), true);
            var acervoIdAttributes = acervoIdProperty?.GetCustomAttributes(typeof(RequiredAttribute), true);

            idAttributes.Should().NotBeNullOrEmpty();
            acervoIdAttributes.Should().NotBeNullOrEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - Atributos Range devem estar presentes")]
        public void DadoDTOAcervoAudiovisualAlteracao_QuandoVerificarAtributos_EntaoDeveConterRangeAttribute()
        {
            var tipo = typeof(AcervoAudiovisualAlteracaoDTO);
            var idProperty = tipo.GetProperty("Id");
            var acervoIdProperty = tipo.GetProperty("AcervoId");

            var idAttributes = idProperty?.GetCustomAttributes(typeof(RangeAttribute), true);
            var acervoIdAttributes = acervoIdProperty?.GetCustomAttributes(typeof(RangeAttribute), true);

            idAttributes.Should().NotBeNullOrEmpty();
            acervoIdAttributes.Should().NotBeNullOrEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualAlteracaoDTO - Mensagens de validação devem ser corretas")]
        public void DadoDTOAcervoAudiovisualAlteracao_QuandoValidarComErro_EntaoMensagensSaoCorretas()
        {
            var dto = new AcervoAudiovisualAlteracaoDTO();
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().HaveCountGreaterThan(1);
            results.Should().Contain(r => r.ErrorMessage != null && r.ErrorMessage.Contains("O identificador do acervo arte gráfica deve ser maior que zero"));
            results.Should().Contain(r => r.ErrorMessage != null && r.ErrorMessage.Contains("O identificador do acervo deve ser maior que zero"));
        }
    }
}
