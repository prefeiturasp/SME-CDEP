using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Enumerados;
using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoArteGraficaCadastroDTOTeste
    {
        #region Testes de Instanciação

        [Fact]
        public void DadoConstrutorPadrao_QuandoChamar_EntaoInstanciaComSucesso()
        {
            var dto = new AcervoArteGraficaCadastroDTO();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<AcervoArteGraficaCadastroDTO>();
        }

        [Fact]
        public void DadoPropriedadesArteGrafica_QuandoInstanciar_EntaoValoresPadroSaoNulos()
        {
            var dto = new AcervoArteGraficaCadastroDTO();

            dto.Localizacao.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Diametro.Should().BeNull();
            dto.Tecnica.Should().BeNull();
            dto.Arquivos.Should().BeNull();
        }

        #endregion

        #region Testes de Propriedades Específicas da Arte Gráfica

        [Fact]
        public void DadoPropriedadesArteGraficaPreenchidas_QuandoInstanciar_EntaoSaoAtribuidas()
        {
            var dto = new AcervoArteGraficaCadastroDTO
            {
                Localizacao = "Sala 1",
                Largura = "10cm",
                Altura = "20cm",
                Diametro = "5cm",
                Tecnica = "Aquarela",
                Arquivos = new long[] { 1, 2, 3 }
            };

            dto.Localizacao.Should().Be("Sala 1");
            dto.Largura.Should().Be("10cm");
            dto.Altura.Should().Be("20cm");
            dto.Diametro.Should().Be("5cm");
            dto.Tecnica.Should().Be("Aquarela");
            dto.Arquivos.Should().BeEquivalentTo(new long[] { 1, 2, 3 });
        }

        [Fact]
        public void DadoCopiaDigitalEPermiteUsoImagem_QuandoInstanciar_EntaoSaoAtribuidas()
        {
            var dto = new AcervoArteGraficaCadastroDTO
            {
                CopiaDigital = true,
                PermiteUsoImagem = false
            };

            dto.CopiaDigital.Should().BeTrue();
            dto.PermiteUsoImagem.Should().BeFalse();
        }

        #endregion

        #region Testes de Propriedades Herdadas de AcervoCadastroDTO

        [Fact]
        public void DadoPropriedadesHerdadas_QuandoInstanciar_EntaoValoresPadroSaoNulos()
        {
            var dto = new AcervoArteGraficaCadastroDTO();

            dto.Descricao.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.CodigoNovo.Should().BeNull();
            dto.CreditosAutoresIds.Should().BeNull();
            dto.CoAutores.Should().BeNull();
            dto.SubTitulo.Should().BeNull();
            dto.DataAcervo.Should().BeNull();
        }

        [Fact]
        public void DadoPropriedadesHerdadasPreenchidas_QuandoInstanciar_EntaoSaoAtribuidas()
        {
            var dto = new AcervoArteGraficaCadastroDTO
            {
                Titulo = "Título Teste",
                Descricao = "Descrição",
                Codigo = "123",
                CodigoNovo = "456",
                CreditosAutoresIds = new long[] { 1, 2 },
                CoAutores = new CoAutorDTO[] { new CoAutorDTO() },
                SubTitulo = "Sub",
                DataAcervo = "2024-01-01",
                Ano = "2024",
                SituacaoAcervo = SituacaoAcervo.Ativo
            };

            dto.Titulo.Should().Be("Título Teste");
            dto.Descricao.Should().Be("Descrição");
            dto.Codigo.Should().Be("123");
            dto.CodigoNovo.Should().Be("456");
            dto.CreditosAutoresIds.Should().HaveCount(2);
            dto.CoAutores.Should().HaveCount(1);
            dto.SubTitulo.Should().Be("Sub");
            dto.DataAcervo.Should().Be("2024-01-01");
            dto.Ano.Should().Be("2024");
            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Ativo);
        }

        #endregion

        #region Testes de Validação de Data Annotations

        [Fact]
        public void DadoTituloNaoPreenchido_QuandoValidar_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaCadastroDTO
            {
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = 1,
                CromiaId = 2,
                SuporteId = 3,
                Quantidade = 10
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaCadastroDTO.Titulo)));
        }

        [Fact]
        public void DadoAnoNaoPreenchido_QuandoValidar_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaCadastroDTO
            {
                Titulo = "Obra",
                Procedencia = "Doação",
                ConservacaoId = 1,
                CromiaId = 2,
                SuporteId = 3,
                Quantidade = 10
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaCadastroDTO.Ano)));
        }

        [Fact]
        public void DadoProcedenciaNaoPreenchida_QuandoValidar_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaCadastroDTO
            {
                Titulo = "Obra",
                Ano = "2024",
                ConservacaoId = 1,
                CromiaId = 2,
                SuporteId = 3,
                Quantidade = 10
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaCadastroDTO.Procedencia)));
        }

        [Fact]
        public void DadoConservacaoIdComValorZero_QuandoValidar_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaCadastroDTO
            {
                Titulo = "Obra",
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = 0,
                CromiaId = 2,
                SuporteId = 3,
                Quantidade = 10
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaCadastroDTO.ConservacaoId)));
        }

        [Fact]
        public void DadoCromiaIdComValorZero_QuandoValidar_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaCadastroDTO
            {
                Titulo = "Obra",
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = 1,
                CromiaId = 0,
                SuporteId = 3,
                Quantidade = 10
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaCadastroDTO.CromiaId)));
        }

        [Fact]
        public void DadoSuporteIdComValorZero_QuandoValidar_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaCadastroDTO
            {
                Titulo = "Obra",
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = 1,
                CromiaId = 2,
                SuporteId = 0,
                Quantidade = 10
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaCadastroDTO.SuporteId)));
        }

        [Fact]
        public void DadoQuantidadeComValorZero_QuandoValidar_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaCadastroDTO
            {
                Titulo = "Obra",
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = 1,
                CromiaId = 2,
                SuporteId = 3,
                Quantidade = 0
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaCadastroDTO.Quantidade)));
        }

        [Fact]
        public void DadoTodosCamposObrigatoriosPreenchidos_QuandoValidar_EntaoPassaEmTodasAsValidacoes()
        {
            var dto = new AcervoArteGraficaCadastroDTO
            {
                Titulo = "Obra",
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = 1,
                CromiaId = 2,
                SuporteId = 3,
                Quantidade = 10
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeTrue();
            resultados.Should().BeEmpty();
        }

        #endregion

        #region Testes de Atributos de Validação via Reflexão

        [Fact]
        public void DadoTituloPropriedade_QuandoVerificarAtributos_EntaoTemRequiredEMaxLength()
        {
            var tipo = typeof(AcervoArteGraficaCadastroDTO);
            var propriedade = tipo.GetProperty(nameof(AcervoArteGraficaCadastroDTO.Titulo));

            propriedade.Should().NotBeNull();
            propriedade!.GetCustomAttributes(false)
                .Should().Contain(a => a.GetType().Name.Contains("Required"));
            propriedade!.GetCustomAttributes(false)
                .Should().Contain(a => a.GetType().Name.Contains("MaxLength"));
        }

        [Fact]
        public void DadoProcedenciaPropriedade_QuandoVerificarAtributos_EntaoTemRequiredEMaxLength()
        {
            var tipo = typeof(AcervoArteGraficaCadastroDTO);
            var propriedade = tipo.GetProperty(nameof(AcervoArteGraficaCadastroDTO.Procedencia));

            propriedade.Should().NotBeNull();
            propriedade!.GetCustomAttributes(false)
                .Should().Contain(a => a.GetType().Name.Contains("Required"));
            propriedade!.GetCustomAttributes(false)
                .Should().Contain(a => a.GetType().Name.Contains("MaxLength"));
        }

        [Fact]
        public void DadoConservacaoIdPropriedade_QuandoVerificarAtributos_EntaoTemRequiredERange()
        {
            var tipo = typeof(AcervoArteGraficaCadastroDTO);
            var propriedade = tipo.GetProperty(nameof(AcervoArteGraficaCadastroDTO.ConservacaoId));

            propriedade.Should().NotBeNull();
            propriedade!.GetCustomAttributes(false)
                .Should().Contain(a => a.GetType().Name.Contains("Required"));
            propriedade!.GetCustomAttributes(false)
                .Should().Contain(a => a.GetType().Name.Contains("Range"));
        }

        [Fact]
        public void DadoCromiaIdPropriedade_QuandoVerificarAtributos_EntaoTemRequiredERange()
        {
            var tipo = typeof(AcervoArteGraficaCadastroDTO);
            var propriedade = tipo.GetProperty(nameof(AcervoArteGraficaCadastroDTO.CromiaId));

            propriedade.Should().NotBeNull();
            propriedade!.GetCustomAttributes(false)
                .Should().Contain(a => a.GetType().Name.Contains("Required"));
            propriedade!.GetCustomAttributes(false)
                .Should().Contain(a => a.GetType().Name.Contains("Range"));
        }

        [Fact]
        public void DadoSuporteIdPropriedade_QuandoVerificarAtributos_EntaoTemRequiredERange()
        {
            var tipo = typeof(AcervoArteGraficaCadastroDTO);
            var propriedade = tipo.GetProperty(nameof(AcervoArteGraficaCadastroDTO.SuporteId));

            propriedade.Should().NotBeNull();
            propriedade!.GetCustomAttributes(false)
                .Should().Contain(a => a.GetType().Name.Contains("Required"));
            propriedade!.GetCustomAttributes(false)
                .Should().Contain(a => a.GetType().Name.Contains("Range"));
        }

        [Fact]
        public void DadoQuantidadePropriedade_QuandoVerificarAtributos_EntaoTemRequiredERange()
        {
            var tipo = typeof(AcervoArteGraficaCadastroDTO);
            var propriedade = tipo.GetProperty(nameof(AcervoArteGraficaCadastroDTO.Quantidade));

            propriedade.Should().NotBeNull();
            propriedade!.GetCustomAttributes(false)
                .Should().Contain(a => a.GetType().Name.Contains("Required"));
            propriedade!.GetCustomAttributes(false)
                .Should().Contain(a => a.GetType().Name.Contains("Range"));
        }

        #endregion

        #region Testes de Propriedades Opcionais

        [Fact]
        public void DadoPropriedadesOpcionaisNulas_QuandoValidar_EntaoPassaValidacao()
        {
            var dto = new AcervoArteGraficaCadastroDTO
            {
                Titulo = "Obra",
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = 1,
                CromiaId = 2,
                SuporteId = 3,
                Quantidade = 10,
                Localizacao = null,
                Largura = null,
                Altura = null,
                Diametro = null,
                Tecnica = null,
                Arquivos = null,
                Descricao = null
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeTrue();
        }

        #endregion

        #region Testes de Valores Limites

        [Fact]
        public void DadoConservacaoIdComValor1_QuandoValidar_EntaoPassaValidacao()
        {
            var dto = new AcervoArteGraficaCadastroDTO
            {
                Titulo = "Obra",
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = 1,
                CromiaId = 2,
                SuporteId = 3,
                Quantidade = 10
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeTrue();
        }

        [Fact]
        public void DadoConservacaoIdComValorMaximo_QuandoValidar_EntaoPassaValidacao()
        {
            var dto = new AcervoArteGraficaCadastroDTO
            {
                Titulo = "Obra",
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = long.MaxValue,
                CromiaId = 2,
                SuporteId = 3,
                Quantidade = 10
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeTrue();
        }

        #endregion

        #region Testes de Mensagens de Validação

        [Fact]
        public void DadoTituloNaoInformado_QuandoValidar_EntaoMostrarMensagemEsperada()
        {
            var dto = new AcervoArteGraficaCadastroDTO
            {
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = 1,
                CromiaId = 2,
                SuporteId = 3,
                Quantidade = 10
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            Validator.TryValidateObject(dto, contexto, resultados, true);

            var erro = resultados.FirstOrDefault(r => r.MemberNames.Contains(nameof(AcervoArteGraficaCadastroDTO.Titulo)));
            erro.Should().NotBeNull();
            erro!.ErrorMessage.Should().Contain("É necessário informar o título do acervo");
        }

        [Fact]
        public void DadoProcedenciaNaoInformada_QuandoValidar_EntaoMostrarMensagemEsperada()
        {
            var dto = new AcervoArteGraficaCadastroDTO
            {
                Titulo = "Obra",
                Ano = "2024",
                ConservacaoId = 1,
                CromiaId = 2,
                SuporteId = 3,
                Quantidade = 10
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            Validator.TryValidateObject(dto, contexto, resultados, true);

            var erro = resultados.FirstOrDefault(r => r.MemberNames.Contains(nameof(AcervoArteGraficaCadastroDTO.Procedencia)));
            erro.Should().NotBeNull();
            erro!.ErrorMessage.Should().Contain("É necessário informar a procedência do acervo arte gráfica");
        }

        #endregion
    }
}
