using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoAudiovisualDetalheDTOTeste
    {
        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Deve conter propriedade Descricao")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoCriar_EntaoContemPropriedadeDescricao()
        {
            var dto = new AcervoAudiovisualDetalheDTO();

            typeof(AcervoAudiovisualDetalheDTO).GetProperty("Descricao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Deve conter propriedade CreditosAutores")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoCriar_EntaoContemPropriedadeCreditosAutores()
        {
            var dto = new AcervoAudiovisualDetalheDTO();

            typeof(AcervoAudiovisualDetalheDTO).GetProperty("CreditosAutores").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Deve conter propriedade DataAcervo")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoCriar_EntaoContemPropriedadeDataAcervo()
        {
            var dto = new AcervoAudiovisualDetalheDTO();

            typeof(AcervoAudiovisualDetalheDTO).GetProperty("DataAcervo").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Deve conter propriedade Localizacao")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoCriar_EntaoContemPropriedadeLocalizacao()
        {
            var dto = new AcervoAudiovisualDetalheDTO();

            typeof(AcervoAudiovisualDetalheDTO).GetProperty("Localizacao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Deve conter propriedade Procedencia")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoCriar_EntaoContemPropriedadeProcedencia()
        {
            var dto = new AcervoAudiovisualDetalheDTO();

            typeof(AcervoAudiovisualDetalheDTO).GetProperty("Procedencia").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Deve conter propriedade Copia")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoCriar_EntaoContemPropriedadeCopia()
        {
            var dto = new AcervoAudiovisualDetalheDTO();

            typeof(AcervoAudiovisualDetalheDTO).GetProperty("Copia").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Deve conter propriedade PermiteUsoImagem")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoCriar_EntaoContemPropriedadePermiteUsoImagem()
        {
            var dto = new AcervoAudiovisualDetalheDTO();

            typeof(AcervoAudiovisualDetalheDTO).GetProperty("PermiteUsoImagem").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Deve conter propriedade Conservacao")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoCriar_EntaoContemPropriedadeConservacao()
        {
            var dto = new AcervoAudiovisualDetalheDTO();

            typeof(AcervoAudiovisualDetalheDTO).GetProperty("Conservacao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Deve conter propriedade Cromia")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoCriar_EntaoContemPropriedadeCromia()
        {
            var dto = new AcervoAudiovisualDetalheDTO();

            typeof(AcervoAudiovisualDetalheDTO).GetProperty("Cromia").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Deve conter propriedade Suporte")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoCriar_EntaoContemPropriedadeSuporte()
        {
            var dto = new AcervoAudiovisualDetalheDTO();

            typeof(AcervoAudiovisualDetalheDTO).GetProperty("Suporte").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Deve conter propriedade Duracao")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoCriar_EntaoContemPropriedadeDuracao()
        {
            var dto = new AcervoAudiovisualDetalheDTO();

            typeof(AcervoAudiovisualDetalheDTO).GetProperty("Duracao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Deve conter propriedade TamanhoArquivo")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoCriar_EntaoContemPropriedadeTamanhoArquivo()
        {
            var dto = new AcervoAudiovisualDetalheDTO();

            typeof(AcervoAudiovisualDetalheDTO).GetProperty("TamanhoArquivo").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Deve conter propriedade Acessibilidade")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoCriar_EntaoContemPropriedadeAcessibilidade()
        {
            var dto = new AcervoAudiovisualDetalheDTO();

            typeof(AcervoAudiovisualDetalheDTO).GetProperty("Acessibilidade").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Deve conter propriedade Disponibilizacao")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoCriar_EntaoContemPropriedadeDisponibilizacao()
        {
            var dto = new AcervoAudiovisualDetalheDTO();

            typeof(AcervoAudiovisualDetalheDTO).GetProperty("Disponibilizacao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Herda de AcervoDetalheDTO")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoCriar_EntaoDeveHerdarDeAcervoDetalheDTO()
        {
            var dto = new AcervoAudiovisualDetalheDTO();

            dto.Should().BeAssignableTo<AcervoDetalheDTO>();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Deve conter propriedades herdadas de AcervoDetalheDTO")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoCriar_EntaoDeveConterPropriedadesHerdadas()
        {
            var tipo = typeof(AcervoAudiovisualDetalheDTO);

            tipo.GetProperty("Titulo").Should().NotBeNull();
            tipo.GetProperty("Codigo").Should().NotBeNull();
            tipo.GetProperty("Ano").Should().NotBeNull();
            tipo.GetProperty("AcervoId").Should().NotBeNull();
            tipo.GetProperty("EnderecoImagemPadrao").Should().NotBeNull();
            tipo.GetProperty("SituacaoDisponibilidade").Should().NotBeNull();
            tipo.GetProperty("EstaDisponivel").Should().NotBeNull();
            tipo.GetProperty("TemControleDisponibilidade").Should().NotBeNull();
            tipo.GetProperty("TipoAcervoId").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Descricao com valor válido deve passar validação")]
        public void DadoDescricaoComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Descricao = "Descrição do audiovisual" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("descrição") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Descricao null deve passar validação")]
        public void DadoDescricaoNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Descricao = null };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("descrição") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - CreditosAutores com valor válido deve passar validação")]
        public void DadoCreditosAutoresComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualDetalheDTO { CreditosAutores = "Autor 1, Autor 2" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("créditos") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - CreditosAutores null deve passar validação")]
        public void DadoCreditosAutoresNulo_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualDetalheDTO { CreditosAutores = null };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("créditos") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - DataAcervo com valor válido deve passar validação")]
        public void DadoDataAcervoComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualDetalheDTO { DataAcervo = "2024-04-30" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("data") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - DataAcervo null deve passar validação")]
        public void DadoDataAcervoNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualDetalheDTO { DataAcervo = null };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("data") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Localizacao com valor válido deve passar validação")]
        public void DadoLocalizacaoComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Localizacao = "Prateleira A1" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("localização") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Localizacao null deve passar validação")]
        public void DadoLocalizacaoNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Localizacao = null };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("localização") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Procedencia com valor válido deve passar validação")]
        public void DadoProcedenciaComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Procedencia = "Doação particular" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("procedência") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Procedencia null deve passar validação")]
        public void DadoProcedenciaNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Procedencia = null };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("procedência") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Copia com valor válido deve passar validação")]
        public void DadoCopiaComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Copia = "Cópia digital" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("cópia") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Copia null deve passar validação")]
        public void DadoCopaiaNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Copia = null };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("cópia") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - PermiteUsoImagem deve aceitar true")]
        public void DadoPermiteUsoImagemComTrue_QuandoValidar_EntaoDeveAceitarSemErro()
        {
            var dto = new AcervoAudiovisualDetalheDTO { PermiteUsoImagem = "true" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - PermiteUsoImagem deve aceitar false")]
        public void DadoPermiteUsoImagemComFalse_QuandoValidar_EntaoDeveAceitarSemErro()
        {
            var dto = new AcervoAudiovisualDetalheDTO { PermiteUsoImagem = "false" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - PermiteUsoImagem null deve passar validação")]
        public void DadoPermiteUsoImagemNulo_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualDetalheDTO { PermiteUsoImagem = null };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Conservacao com valor válido deve passar validação")]
        public void DadoConservacaoComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Conservacao = "Excelente" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("conservação") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Conservacao null deve passar validação")]
        public void DadoConservacaoNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Conservacao = null };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("conservação") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Cromia com valor válido deve passar validação")]
        public void DadoCromiaComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Cromia = "Colorido" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("cromia") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Cromia null deve passar validação")]
        public void DadoCromiaNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Cromia = null };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("cromia") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Suporte com valor válido deve passar validação")]
        public void DadoSuporteComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Suporte = "DVD" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("suporte") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Suporte null deve passar validação")]
        public void DadoSuporteNulo_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Suporte = null };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("suporte") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Duracao com valor válido deve passar validação")]
        public void DadoDuracaoComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Duracao = "02:30:45" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("duração") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Duracao null deve passar validação")]
        public void DadoDuracaoNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Duracao = null };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("duração") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - TamanhoArquivo com valor válido deve passar validação")]
        public void DadoTamanhoArquivoComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualDetalheDTO { TamanhoArquivo = "1.5GB" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("tamanho") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - TamanhoArquivo null deve passar validação")]
        public void DadoTamanhoArquivoNulo_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualDetalheDTO { TamanhoArquivo = null };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("tamanho") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Acessibilidade com valor válido deve passar validação")]
        public void DadoAcessibilidadeComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Acessibilidade = "Legendado" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("acessibilidade") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Acessibilidade null deve passar validação")]
        public void DadoAcessibilidadeNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Acessibilidade = null };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("acessibilidade") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Disponibilizacao com valor válido deve passar validação")]
        public void DadoDisponibilizacaoComValorValido_QuandoValidar_EntaoNaoDeveRetornarErro()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Disponibilizacao = "Público" };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("disponibilização") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Disponibilizacao null deve passar validação")]
        public void DadoDisponibilizacaoNula_QuandoValidar_EntaoDevePassarValidacao()
        {
            var dto = new AcervoAudiovisualDetalheDTO { Disponibilizacao = null };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Where(r => r.ErrorMessage?.Contains("disponibilização") ?? false).Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Deve permitir atribuição de valores às propriedades")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoAtribuirValores_EntaoDeveFazerCorretamente()
        {
            var titulo = "Título do Audiovisual";
            var codigo = "AUD001";
            var ano = "2024";
            var acervoId = 1L;
            var descricao = "Descrição do audiovisual";
            var creditosAutores = "Autor 1";
            var dataAcervo = "2024-04-30";
            var localizacao = "Prateleira A";
            var procedencia = "Doação";
            var copia = "Digital";
            var permiteUsoImagem = "true";
            var conservacao = "Excelente";
            var cromia = "Colorido";
            var suporte = "DVD";
            var duracao = "01:30:00";
            var tamanhoArquivo = "500MB";
            var acessibilidade = "Legendado";
            var disponibilizacao = "Público";

            var dto = new AcervoAudiovisualDetalheDTO
            {
                Titulo = titulo,
                Codigo = codigo,
                Ano = ano,
                AcervoId = acervoId,
                Descricao = descricao,
                CreditosAutores = creditosAutores,
                DataAcervo = dataAcervo,
                Localizacao = localizacao,
                Procedencia = procedencia,
                Copia = copia,
                PermiteUsoImagem = permiteUsoImagem,
                Conservacao = conservacao,
                Cromia = cromia,
                Suporte = suporte,
                Duracao = duracao,
                TamanhoArquivo = tamanhoArquivo,
                Acessibilidade = acessibilidade,
                Disponibilizacao = disponibilizacao
            };

            dto.Titulo.Should().Be(titulo);
            dto.Codigo.Should().Be(codigo);
            dto.Ano.Should().Be(ano);
            dto.AcervoId.Should().Be(acervoId);
            dto.Descricao.Should().Be(descricao);
            dto.CreditosAutores.Should().Be(creditosAutores);
            dto.DataAcervo.Should().Be(dataAcervo);
            dto.Localizacao.Should().Be(localizacao);
            dto.Procedencia.Should().Be(procedencia);
            dto.Copia.Should().Be(copia);
            dto.PermiteUsoImagem.Should().Be(permiteUsoImagem);
            dto.Conservacao.Should().Be(conservacao);
            dto.Cromia.Should().Be(cromia);
            dto.Suporte.Should().Be(suporte);
            dto.Duracao.Should().Be(duracao);
            dto.TamanhoArquivo.Should().Be(tamanhoArquivo);
            dto.Acessibilidade.Should().Be(acessibilidade);
            dto.Disponibilizacao.Should().Be(disponibilizacao);
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Propriedades devem ser públicas")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoVerificarVisibilidadePropriedades_EntaoDeveSerPublicas()
        {
            var tipo = typeof(AcervoAudiovisualDetalheDTO);

            var descricaoProperty = tipo.GetProperty("Descricao");
            var creditosAutoresProperty = tipo.GetProperty("CreditosAutores");
            var dataAcervoProperty = tipo.GetProperty("DataAcervo");
            var localizacaoProperty = tipo.GetProperty("Localizacao");
            var procedenciaProperty = tipo.GetProperty("Procedencia");
            var copiaProperty = tipo.GetProperty("Copia");
            var permiteUsoImagemProperty = tipo.GetProperty("PermiteUsoImagem");
            var conservacaoProperty = tipo.GetProperty("Conservacao");
            var cromiaProperty = tipo.GetProperty("Cromia");
            var suporteProperty = tipo.GetProperty("Suporte");
            var duracaoProperty = tipo.GetProperty("Duracao");
            var tamanhoArquivoProperty = tipo.GetProperty("TamanhoArquivo");
            var acessibilidadeProperty = tipo.GetProperty("Acessibilidade");
            var disponibilizacaoProperty = tipo.GetProperty("Disponibilizacao");

            descricaoProperty?.CanRead.Should().BeTrue();
            descricaoProperty?.CanWrite.Should().BeTrue();
            creditosAutoresProperty?.CanRead.Should().BeTrue();
            creditosAutoresProperty?.CanWrite.Should().BeTrue();
            dataAcervoProperty?.CanRead.Should().BeTrue();
            dataAcervoProperty?.CanWrite.Should().BeTrue();
            localizacaoProperty?.CanRead.Should().BeTrue();
            localizacaoProperty?.CanWrite.Should().BeTrue();
            procedenciaProperty?.CanRead.Should().BeTrue();
            procedenciaProperty?.CanWrite.Should().BeTrue();
            copiaProperty?.CanRead.Should().BeTrue();
            copiaProperty?.CanWrite.Should().BeTrue();
            permiteUsoImagemProperty?.CanRead.Should().BeTrue();
            permiteUsoImagemProperty?.CanWrite.Should().BeTrue();
            conservacaoProperty?.CanRead.Should().BeTrue();
            conservacaoProperty?.CanWrite.Should().BeTrue();
            cromiaProperty?.CanRead.Should().BeTrue();
            cromiaProperty?.CanWrite.Should().BeTrue();
            suporteProperty?.CanRead.Should().BeTrue();
            suporteProperty?.CanWrite.Should().BeTrue();
            duracaoProperty?.CanRead.Should().BeTrue();
            duracaoProperty?.CanWrite.Should().BeTrue();
            tamanhoArquivoProperty?.CanRead.Should().BeTrue();
            tamanhoArquivoProperty?.CanWrite.Should().BeTrue();
            acessibilidadeProperty?.CanRead.Should().BeTrue();
            acessibilidadeProperty?.CanWrite.Should().BeTrue();
            disponibilizacaoProperty?.CanRead.Should().BeTrue();
            disponibilizacaoProperty?.CanWrite.Should().BeTrue();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Validação completa com todos os campos válidos")]
        public void DadoDTOAcervoAudiovisualDetalheComTodosCamposValidos_QuandoValidar_EntaoDevePassar()
        {
            var dto = new AcervoAudiovisualDetalheDTO
            {
                Titulo = "Título do Audiovisual",
                Codigo = "AUD001",
                Ano = "2024",
                AcervoId = 1L,
                Descricao = "Descrição completa",
                CreditosAutores = "Autor 1, Autor 2",
                DataAcervo = "2024-04-30",
                Localizacao = "Prateleira A1",
                Procedencia = "Doação particular",
                Copia = "Digital",
                PermiteUsoImagem = "true",
                Conservacao = "Excelente",
                Cromia = "Colorido",
                Suporte = "DVD",
                Duracao = "02:30:00",
                TamanhoArquivo = "1.5GB",
                Acessibilidade = "Legendado",
                Disponibilizacao = "Público"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Instância padrão com valores nulos")]
        public void DadoDTOAcervoAudiovisualDetalheNovembro_QuandoCriar_EntaoTodosValoresSaoNulosOuPadrao()
        {
            var dto = new AcervoAudiovisualDetalheDTO();

            dto.Descricao.Should().BeNull();
            dto.CreditosAutores.Should().BeNull();
            dto.DataAcervo.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.Copia.Should().BeNull();
            dto.PermiteUsoImagem.Should().BeNull();
            dto.Conservacao.Should().BeNull();
            dto.Cromia.Should().BeNull();
            dto.Suporte.Should().BeNull();
            dto.Duracao.Should().BeNull();
            dto.TamanhoArquivo.Should().BeNull();
            dto.Acessibilidade.Should().BeNull();
            dto.Disponibilizacao.Should().BeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Deve herdar corretamente de AcervoDetalheDTO")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoVerificarHeranca_EntaoDeveHerdarCorretamente()
        {
            var tipo = typeof(AcervoAudiovisualDetalheDTO);
            var baseType = tipo.BaseType;

            baseType.Should().Be(typeof(AcervoDetalheDTO));
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Instâncias diferentes devem ser independentes")]
        public void DadoDuasInstanciasDTOAcervoAudiovisualDetalhe_QuandoModificarUma_EntaoOutraNaoDeveSerAfetada()
        {
            var dto1 = new AcervoAudiovisualDetalheDTO 
            { 
                Descricao = "Descrição 1",
                Titulo = "Título 1"
            };
            var dto2 = new AcervoAudiovisualDetalheDTO 
            { 
                Descricao = "Descrição 2",
                Titulo = "Título 2"
            };

            dto1.Descricao.Should().Be("Descrição 1");
            dto2.Descricao.Should().Be("Descrição 2");
            dto1.Titulo.Should().Be("Título 1");
            dto2.Titulo.Should().Be("Título 2");
            dto1.Descricao.Should().NotBe(dto2.Descricao);
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Propriedades podem ser configuradas e relidas")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoConfigurareRelersPropriedades_EntaoDevemRetornarValoresCorretos()
        {
            var dto = new AcervoAudiovisualDetalheDTO();
            var novaDescricao = "Nova descrição";
            var novosTitulo = "Novo título";

            dto.Descricao = novaDescricao;
            dto.Titulo = novosTitulo;

            dto.Descricao.Should().Be(novaDescricao);
            dto.Titulo.Should().Be(novosTitulo);
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Teste com strings vazias")]
        public void DadoDTOAcervoAudiovisualDetalheComStringsVazias_QuandoValidar_EntaoDevePassar()
        {
            var dto = new AcervoAudiovisualDetalheDTO
            {
                Descricao = string.Empty,
                CreditosAutores = string.Empty,
                DataAcervo = string.Empty,
                Localizacao = string.Empty,
                Procedencia = string.Empty,
                Copia = string.Empty,
                PermiteUsoImagem = string.Empty,
                Conservacao = string.Empty,
                Cromia = string.Empty,
                Suporte = string.Empty,
                Duracao = string.Empty,
                TamanhoArquivo = string.Empty,
                Acessibilidade = string.Empty,
                Disponibilizacao = string.Empty
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(dto, context, results, true);

            results.Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDetalheDTO - Teste com valores herdados de AcervoDetalheDTO")]
        public void DadoDTOAcervoAudiovisualDetalhe_QuandoAtribuirValoresHerdados_EntaoDeveFazerCorretamente()
        {
            var enderecoImagem = "http://exemplo.com/imagem.jpg";
            var situacaoDisponibilidade = "Disponível";
            var estaDisponivel = true;
            var temControleDisponibilidade = false;
            var tipoAcervoId = 5;

            var dto = new AcervoAudiovisualDetalheDTO
            {
                EnderecoImagemPadrao = enderecoImagem,
                SituacaoDisponibilidade = situacaoDisponibilidade,
                EstaDisponivel = estaDisponivel,
                TemControleDisponibilidade = temControleDisponibilidade,
                TipoAcervoId = tipoAcervoId
            };

            dto.EnderecoImagemPadrao.Should().Be(enderecoImagem);
            dto.SituacaoDisponibilidade.Should().Be(situacaoDisponibilidade);
            dto.EstaDisponivel.Should().Be(estaDisponivel);
            dto.TemControleDisponibilidade.Should().Be(temControleDisponibilidade);
            dto.TipoAcervoId.Should().Be(tipoAcervoId);
        }
    }
}
