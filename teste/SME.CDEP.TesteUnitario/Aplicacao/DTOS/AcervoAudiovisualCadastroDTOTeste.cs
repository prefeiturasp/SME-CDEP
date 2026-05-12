using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoAudiovisualCadastroDTOTeste
    {
        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Deve conter propriedade Localizacao")]
        public void DadoDTOAcervoAudiovisualCadastro_QuandoCriar_EntaoContemPropriedadeLocalizacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO();

            typeof(AcervoAudiovisualCadastroDTO).GetProperty("Localizacao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Deve conter propriedade Procedencia")]
        public void DadoDTOAcervoAudiovisualCadastro_QuandoCriar_EntaoContemPropriedadeProcedencia()
        {
            var dto = new AcervoAudiovisualCadastroDTO();

            typeof(AcervoAudiovisualCadastroDTO).GetProperty("Procedencia").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Deve conter propriedade Copia")]
        public void DadoDTOAcervoAudiovisualCadastro_QuandoCriar_EntaoContemPropriedadeCopia()
        {
            var dto = new AcervoAudiovisualCadastroDTO();

            typeof(AcervoAudiovisualCadastroDTO).GetProperty("Copia").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Deve conter propriedade PermiteUsoImagem")]
        public void DadoDTOAcervoAudiovisualCadastro_QuandoCriar_EntaoContemPropriedadePermiteUsoImagem()
        {
            var dto = new AcervoAudiovisualCadastroDTO();

            typeof(AcervoAudiovisualCadastroDTO).GetProperty("PermiteUsoImagem").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Deve conter propriedade ConservacaoId")]
        public void DadoDTOAcervoAudiovisualCadastro_QuandoCriar_EntaoContemPropriedadeConservacaoId()
        {
            var dto = new AcervoAudiovisualCadastroDTO();

            typeof(AcervoAudiovisualCadastroDTO).GetProperty("ConservacaoId").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Deve conter propriedade SuporteId")]
        public void DadoDTOAcervoAudiovisualCadastro_QuandoCriar_EntaoContemPropriedadeSuporteId()
        {
            var dto = new AcervoAudiovisualCadastroDTO();

            typeof(AcervoAudiovisualCadastroDTO).GetProperty("SuporteId").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Deve conter propriedade Duracao")]
        public void DadoDTOAcervoAudiovisualCadastro_QuandoCriar_EntaoContemPropriedadeDuracao()
        {
            var dto = new AcervoAudiovisualCadastroDTO();

            typeof(AcervoAudiovisualCadastroDTO).GetProperty("Duracao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Deve conter propriedade CromiaId")]
        public void DadoDTOAcervoAudiovisualCadastro_QuandoCriar_EntaoContemPropriedadeCromiaId()
        {
            var dto = new AcervoAudiovisualCadastroDTO();

            typeof(AcervoAudiovisualCadastroDTO).GetProperty("CromiaId").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Deve conter propriedade TamanhoArquivo")]
        public void DadoDTOAcervoAudiovisualCadastro_QuandoCriar_EntaoContemPropriedadeTamanhoArquivo()
        {
            var dto = new AcervoAudiovisualCadastroDTO();

            typeof(AcervoAudiovisualCadastroDTO).GetProperty("TamanhoArquivo").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Deve conter propriedade Acessibilidade")]
        public void DadoDTOAcervoAudiovisualCadastro_QuandoCriar_EntaoContemPropriedadeAcessibilidade()
        {
            var dto = new AcervoAudiovisualCadastroDTO();

            typeof(AcervoAudiovisualCadastroDTO).GetProperty("Acessibilidade").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Deve conter propriedade Disponibilizacao")]
        public void DadoDTOAcervoAudiovisualCadastro_QuandoCriar_EntaoContemPropriedadeDisponibilizacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO();

            typeof(AcervoAudiovisualCadastroDTO).GetProperty("Disponibilizacao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Localizacao com valor válido dentro do limite deve passar validação")]
        public void DadoLocalizacaoComValorValidoDentroDoLimite_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO { Localizacao = "Localização válida", SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("localização do acervo audiovisual não pode conter mais que 100 caracteres")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Localizacao não deve permitir mais de 100 caracteres")]
        public void DadoLocalizacaoComMaisDe100Caracteres_QuandoValidar_EntaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO 
            { 
                Localizacao = new string('a', 101), 
                SuporteId = 1 
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("A localização do acervo audiovisual não pode conter mais que 100 caracteres"));
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Localizacao com exatamente 100 caracteres deve passar validação")]
        public void DadoLocalizacaoComExatamente100Caracteres_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO 
            { 
                Localizacao = new string('a', 100), 
                SuporteId = 1 
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("localização do acervo audiovisual não pode conter mais que 100 caracteres")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Localizacao null deve passar validação")]
        public void DadoLocalizacaoNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO { Localizacao = null, SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("localização do acervo audiovisual")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Procedencia com valor válido dentro do limite deve passar validação")]
        public void DadoProcedenciaComValorValidoDentroDoLimite_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO { Procedencia = "Procedência válida", SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("procedência do acervo audiovisual não pode conter mais que 200 caracteres")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Procedencia não deve permitir mais de 200 caracteres")]
        public void DadoProcedenciaComMaisDe200Caracteres_QuandoValidar_EntaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO 
            { 
                Procedencia = new string('a', 201), 
                SuporteId = 1 
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("A procedência do acervo audiovisual não pode conter mais que 200 caracteres"));
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Procedencia com exatamente 200 caracteres deve passar validação")]
        public void DadoProcedenciaComExatamente200Caracteres_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO 
            { 
                Procedencia = new string('a', 200), 
                SuporteId = 1 
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("procedência do acervo audiovisual não pode conter mais que 200 caracteres")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Procedencia null deve passar validação")]
        public void DadoProcedenciaNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO { Procedencia = null, SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("procedência do acervo audiovisual")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Copia com valor válido dentro do limite deve passar validação")]
        public void DadoCopiaComValorValidoDentroDoLimite_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO { Copia = "Cópia válida", SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("cópia do acervo audiovisual não pode conter mais que 100 caracteres")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Copia não deve permitir mais de 100 caracteres")]
        public void DadoCopiaComMaisDe100Caracteres_QuandoValidar_EntaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO 
            { 
                Copia = new string('a', 101), 
                SuporteId = 1 
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("A cópia do acervo audiovisual não pode conter mais que 100 caracteres"));
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Copia com exatamente 100 caracteres deve passar validação")]
        public void DadoCopiaComExatamente100Caracteres_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO 
            { 
                Copia = new string('a', 100), 
                SuporteId = 1 
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("cópia do acervo audiovisual não pode conter mais que 100 caracteres")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Copia null deve passar validação")]
        public void DadoCopaiaNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO { Copia = null, SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("cópia do acervo audiovisual")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - PermiteUsoImagem deve aceitar true")]
        public void DadoPermiteUsoImagemComTrue_QuandoValidar_EntaoDeveAceitarSemErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO { PermiteUsoImagem = true, SuporteId = 1, Titulo = "Título do acervo", Ano = "2024" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - PermiteUsoImagem deve aceitar false")]
        public void DadoPermiteUsoImagemComFalse_QuandoValidar_EntaoDeveAceitarSemErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO { PermiteUsoImagem = false, SuporteId = 1, Titulo = "Título do acervo", Ano = "2024" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - ConservacaoId com valor válido deve passar validação")]
        public void DadoConservacaoIdComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO { ConservacaoId = 5, SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("conservação")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - ConservacaoId null deve passar validação")]
        public void DadoConservacaoIdNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO { ConservacaoId = null, SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("conservação")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - SuporteId obrigatório")]
        public void DadoSuporteIdNaoPreenchido_QuandoValidar_EntaoDeveRetornarErroObrigatorio()
        {
            var dto = new AcervoAudiovisualCadastroDTO();
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("O identificador do suporte do acervo audiovisual deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - SuporteId com valor válido deve passar validação")]
        public void DadoSuporteIdComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO { SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("suporte do acervo audiovisual")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - SuporteId não deve permitir valor zero")]
        public void DadoSuporteIdComValorZero_QuandoValidar_EntaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO { SuporteId = 0 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("O identificador do suporte do acervo audiovisual deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - SuporteId não deve permitir valor negativo")]
        public void DadoSuporteIdComValorNegativo_QuandoValidar_EntaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO { SuporteId = -5 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("O identificador do suporte do acervo audiovisual deve ser maior que zero"));
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - SuporteId deve permitir valores máximos para long")]
        public void DadoSuporteIdComValorMaximoParaLong_QuandoValidar_EntaoDeveAceitarSemErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO { SuporteId = long.MaxValue };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("suporte do acervo audiovisual")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Duracao com valor válido dentro do limite deve passar validação")]
        public void DadoDuracaoComValorValidoDentroDoLimite_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO { Duracao = "02:30:45", SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("duração do acervo audiovisual não pode conter mais que 15 caracteres")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Duracao não deve permitir mais de 15 caracteres")]
        public void DadoDuracaoComMaisDe15Caracteres_QuandoValidar_EntaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO 
            { 
                Duracao = new string('a', 16), 
                SuporteId = 1 
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("A duração do acervo audiovisual não pode conter mais que 15 caracteres"));
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Duracao com exatamente 15 caracteres deve passar validação")]
        public void DadoDuracaoComExatamente15Caracteres_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO 
            { 
                Duracao = new string('a', 15), 
                SuporteId = 1 
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("duração do acervo audiovisual não pode conter mais que 15 caracteres")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Duracao null deve passar validação")]
        public void DadoDuracaoNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO { Duracao = null, SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("duração do acervo audiovisual")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - CromiaId com valor válido deve passar validação")]
        public void DadoCromiaIdComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO { CromiaId = 3, SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("cromia")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - CromiaId null deve passar validação")]
        public void DadoCromiaIdNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO { CromiaId = null, SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("cromia")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - TamanhoArquivo com valor válido dentro do limite deve passar validação")]
        public void DadoTamanhoArquivoComValorValidoDentroDoLimite_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO { TamanhoArquivo = "1.5GB", SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("tamanho do arquivo do acervo audiovisual não pode conter mais que 15 caracteres")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - TamanhoArquivo não deve permitir mais de 15 caracteres")]
        public void DadoTamanhoArquivoComMaisDe15Caracteres_QuandoValidar_EntaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO 
            { 
                TamanhoArquivo = new string('a', 16), 
                SuporteId = 1 
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("A tamanho do arquivo do acervo audiovisual não pode conter mais que 15 caracteres"));
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - TamanhoArquivo com exatamente 15 caracteres deve passar validação")]
        public void DadoTamanhoArquivoComExatamente15Caracteres_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO 
            { 
                TamanhoArquivo = new string('a', 15), 
                SuporteId = 1 
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("tamanho do arquivo do acervo audiovisual não pode conter mais que 15 caracteres")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - TamanhoArquivo null deve passar validação")]
        public void DadoTamanhoArquivoNulo_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO { TamanhoArquivo = null, SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("tamanho do arquivo do acervo audiovisual")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Acessibilidade com valor válido dentro do limite deve passar validação")]
        public void DadoAcessibilidadeComValorValidoDentroDoLimite_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO { Acessibilidade = "Legenda em português", SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("acessibilidade do acervo audiovisual não pode conter mais que 100 caracteres")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Acessibilidade não deve permitir mais de 100 caracteres")]
        public void DadoAcessibilidadeComMaisDe100Caracteres_QuandoValidar_EntaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO 
            { 
                Acessibilidade = new string('a', 101), 
                SuporteId = 1 
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("A acessibilidade do acervo audiovisual não pode conter mais que 100 caracteres"));
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Acessibilidade com exatamente 100 caracteres deve passar validação")]
        public void DadoAcessibilidadeComExatamente100Caracteres_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO 
            { 
                Acessibilidade = new string('a', 100), 
                SuporteId = 1 
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("acessibilidade do acervo audiovisual não pode conter mais que 100 caracteres")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Acessibilidade null deve passar validação")]
        public void DadoAcessibilidadeNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO { Acessibilidade = null, SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("acessibilidade do acervo audiovisual")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Disponibilizacao com valor válido dentro do limite deve passar validação")]
        public void DadoDisponibilizacaoComValorValidoDentroDoLimite_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO { Disponibilizacao = "Disponível para consulta", SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("disponibilização do acervo audiovisual não pode conter mais que 200 caracteres")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Disponibilizacao não deve permitir mais de 200 caracteres")]
        public void DadoDisponibilizacaoComMaisDe200Caracteres_QuandoValidar_EntaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualCadastroDTO 
            { 
                Disponibilizacao = new string('a', 201), 
                SuporteId = 1 
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("A disponibilização do acervo audiovisual não pode conter mais que 200 caracteres"));
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Disponibilizacao com exatamente 200 caracteres deve passar validação")]
        public void DadoDisponibilizacaoComExatamente200Caracteres_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO 
            { 
                Disponibilizacao = new string('a', 200), 
                SuporteId = 1 
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("disponibilização do acervo audiovisual não pode conter mais que 200 caracteres")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Disponibilizacao null deve passar validação")]
        public void DadoDisponibilizacaoNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualCadastroDTO { Disponibilizacao = null, SuporteId = 1 };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage.Contains("disponibilização do acervo audiovisual")).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Herda de AcervoCadastroDTO")]
        public void DadoDTOAcervoAudiovisualCadastro_QuandoCriar_EntaoDeveHerdarDeAcervoCadastroDTO()
        {
            var dto = new AcervoAudiovisualCadastroDTO();

            dto.Should().BeAssignableTo<AcervoCadastroDTO>();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Deve permitir atribuição de valores às propriedades")]
        public void DadoDTOAcervoAudiovisualCadastro_QuandoAtribuirValores_EntaoDeveFazerCorretamente()
        {
            var localizacao = "Prateleira A";
            var procedencia = "Acervo pessoal";
            var copia = "Cópia digital";
            var permiteUsoImagem = true;
            var conservacaoId = 2L;
            var suporteId = 3L;
            var duracao = "01:30:00";
            var cromiaId = 1L;
            var tamanhoArquivo = "500MB";
            var acessibilidade = "Legendado";
            var disponibilizacao = "Público";

            var dto = new AcervoAudiovisualCadastroDTO
            {
                Localizacao = localizacao,
                Procedencia = procedencia,
                Copia = copia,
                PermiteUsoImagem = permiteUsoImagem,
                ConservacaoId = conservacaoId,
                SuporteId = suporteId,
                Duracao = duracao,
                CromiaId = cromiaId,
                TamanhoArquivo = tamanhoArquivo,
                Acessibilidade = acessibilidade,
                Disponibilizacao = disponibilizacao
            };

            dto.Localizacao.Should().Be(localizacao);
            dto.Procedencia.Should().Be(procedencia);
            dto.Copia.Should().Be(copia);
            dto.PermiteUsoImagem.Should().Be(permiteUsoImagem);
            dto.ConservacaoId.Should().Be(conservacaoId);
            dto.SuporteId.Should().Be(suporteId);
            dto.Duracao.Should().Be(duracao);
            dto.CromiaId.Should().Be(cromiaId);
            dto.TamanhoArquivo.Should().Be(tamanhoArquivo);
            dto.Acessibilidade.Should().Be(acessibilidade);
            dto.Disponibilizacao.Should().Be(disponibilizacao);
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Propriedades devem ser públicas")]
        public void DadoDTOAcervoAudiovisualCadastro_QuandoVerificarVisibilidadePropriedades_EntaoDeveSerPublicas()
        {
            var tipo = typeof(AcervoAudiovisualCadastroDTO);

            var localizacaoProperty = tipo.GetProperty("Localizacao");
            var procedenciaProperty = tipo.GetProperty("Procedencia");
            var copiaProperty = tipo.GetProperty("Copia");
            var permiteUsoImagemProperty = tipo.GetProperty("PermiteUsoImagem");
            var conservacaoIdProperty = tipo.GetProperty("ConservacaoId");
            var suporteIdProperty = tipo.GetProperty("SuporteId");
            var duracaoProperty = tipo.GetProperty("Duracao");
            var cromiaIdProperty = tipo.GetProperty("CromiaId");
            var tamanhoArquivoProperty = tipo.GetProperty("TamanhoArquivo");
            var acessibilidadeProperty = tipo.GetProperty("Acessibilidade");
            var disponibilizacaoProperty = tipo.GetProperty("Disponibilizacao");

            localizacaoProperty.Should().NotBeNull();
            localizacaoProperty!.CanRead.Should().BeTrue();
            localizacaoProperty.CanWrite.Should().BeTrue();
            
            procedenciaProperty.Should().NotBeNull();
            procedenciaProperty!.CanRead.Should().BeTrue();
            procedenciaProperty.CanWrite.Should().BeTrue();
            
            copiaProperty.Should().NotBeNull();
            copiaProperty!.CanRead.Should().BeTrue();
            copiaProperty.CanWrite.Should().BeTrue();
            
            permiteUsoImagemProperty.Should().NotBeNull();
            permiteUsoImagemProperty!.CanRead.Should().BeTrue();
            permiteUsoImagemProperty.CanWrite.Should().BeTrue();
            
            conservacaoIdProperty.Should().NotBeNull();
            conservacaoIdProperty!.CanRead.Should().BeTrue();
            conservacaoIdProperty.CanWrite.Should().BeTrue();
            
            suporteIdProperty.Should().NotBeNull();
            suporteIdProperty!.CanRead.Should().BeTrue();
            suporteIdProperty.CanWrite.Should().BeTrue();
            
            duracaoProperty.Should().NotBeNull();
            duracaoProperty!.CanRead.Should().BeTrue();
            duracaoProperty.CanWrite.Should().BeTrue();
            
            cromiaIdProperty.Should().NotBeNull();
            cromiaIdProperty!.CanRead.Should().BeTrue();
            cromiaIdProperty.CanWrite.Should().BeTrue();
            
            tamanhoArquivoProperty.Should().NotBeNull();
            tamanhoArquivoProperty!.CanRead.Should().BeTrue();
            tamanhoArquivoProperty.CanWrite.Should().BeTrue();
            
            acessibilidadeProperty.Should().NotBeNull();
            acessibilidadeProperty!.CanRead.Should().BeTrue();
            acessibilidadeProperty.CanWrite.Should().BeTrue();
            
            disponibilizacaoProperty.Should().NotBeNull();
            disponibilizacaoProperty!.CanRead.Should().BeTrue();
            disponibilizacaoProperty.CanWrite.Should().BeTrue();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Atributos MaxLength devem estar presentes em strings")]
        public void DadoDTOAcervoAudiovisualCadastro_QuandoVerificarAtributos_EntaoDeveConterMaxLengthAttribute()
        {
            var tipo = typeof(AcervoAudiovisualCadastroDTO);

            var localizacaoProperty = tipo.GetProperty("Localizacao");
            var procedenciaProperty = tipo.GetProperty("Procedencia");
            var copiaProperty = tipo.GetProperty("Copia");
            var duracaoProperty = tipo.GetProperty("Duracao");
            var tamanhoArquivoProperty = tipo.GetProperty("TamanhoArquivo");
            var acessibilidadeProperty = tipo.GetProperty("Acessibilidade");
            var disponibilizacaoProperty = tipo.GetProperty("Disponibilizacao");

            localizacaoProperty.Should().NotBeNull();
            var localizacaoAttributes = localizacaoProperty!.GetCustomAttributes(typeof(MaxLengthAttribute), true);
            
            procedenciaProperty.Should().NotBeNull();
            var procedenciaAttributes = procedenciaProperty!.GetCustomAttributes(typeof(MaxLengthAttribute), true);
            
            copiaProperty.Should().NotBeNull();
            var copiaAttributes = copiaProperty!.GetCustomAttributes(typeof(MaxLengthAttribute), true);
            
            duracaoProperty.Should().NotBeNull();
            var duracaoAttributes = duracaoProperty!.GetCustomAttributes(typeof(MaxLengthAttribute), true);
            
            tamanhoArquivoProperty.Should().NotBeNull();
            var tamanhoArquivoAttributes = tamanhoArquivoProperty!.GetCustomAttributes(typeof(MaxLengthAttribute), true);
            
            acessibilidadeProperty.Should().NotBeNull();
            var acessibilidadeAttributes = acessibilidadeProperty!.GetCustomAttributes(typeof(MaxLengthAttribute), true);
            
            disponibilizacaoProperty.Should().NotBeNull();
            var disponibilizacaoAttributes = disponibilizacaoProperty!.GetCustomAttributes(typeof(MaxLengthAttribute), true);

            localizacaoAttributes.Should().NotBeNullOrEmpty();
            procedenciaAttributes.Should().NotBeNullOrEmpty();
            copiaAttributes.Should().NotBeNullOrEmpty();
            duracaoAttributes.Should().NotBeNullOrEmpty();
            tamanhoArquivoAttributes.Should().NotBeNullOrEmpty();
            acessibilidadeAttributes.Should().NotBeNullOrEmpty();
            disponibilizacaoAttributes.Should().NotBeNullOrEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - SuporteId deve ter atributo Required")]
        public void DadoDTOAcervoAudiovisualCadastro_QuandoVerificarAtributos_EntaoSuporteIdDeveConterRequired()
        {
            var tipo = typeof(AcervoAudiovisualCadastroDTO);
            var suporteIdProperty = tipo.GetProperty("SuporteId");

            suporteIdProperty.Should().NotBeNull();
            var requiredAttributes = suporteIdProperty!.GetCustomAttributes(typeof(RequiredAttribute), true);
            var rangeAttributes = suporteIdProperty.GetCustomAttributes(typeof(RangeAttribute), true);

            requiredAttributes.Should().NotBeNullOrEmpty();
            rangeAttributes.Should().NotBeNullOrEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Mensagens de validação devem estar corretas")]
        public void DadoDTOAcervoAudiovisualCadastro_QuandoValidarComErros_EntaoMensagensEstaoCorretas()
        {
            var dto = new AcervoAudiovisualCadastroDTO
            {
                Localizacao = new string('a', 101),
                Procedencia = new string('b', 201),
                Copia = new string('c', 101),
                SuporteId = 0
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().NotBeEmpty();
            results.Should().Contain(r => r.ErrorMessage.Contains("localização"));
            results.Should().Contain(r => r.ErrorMessage.Contains("procedência"));
            results.Should().Contain(r => r.ErrorMessage.Contains("cópia"));
            results.Should().Contain(r => r.ErrorMessage.Contains("suporte"));
        }

        [Fact(DisplayName = "AcervoAudiovisualCadastroDTO - Validação completa com todos os campos válidos")]
        public void DadoDTOAcervoAudiovisualCadastroComTodosCamposValidos_QuandoValidar_EntaoDevePassar()
        {
            var dto = new AcervoAudiovisualCadastroDTO
            {
                Localizacao = "Localização válida",
                Procedencia = "Procedência válida",
                Copia = "Cópia válida",
                PermiteUsoImagem = true,
                ConservacaoId = 1,
                SuporteId = 1,
                Duracao = "02:30:00",
                CromiaId = 1,
                TamanhoArquivo = "1.5GB",
                Acessibilidade = "Legendado",
                Disponibilizacao = "Público"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.MemberNames.Contains("SuporteId")).Should().BeEmpty();
        }
    }
}
