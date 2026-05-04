using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoArteGraficaAlteracaoDTOTeste
    {
        #region Testes da Propriedade Id

        [Fact]
        public void DadoIdComValorValido_QuandoInstanciar_EntaoPropriedadeIdEhPreenchida()
        {
            var id = 123L;
            var dto = new AcervoArteGraficaAlteracaoDTO { Id = id };

            dto.Id.Should().Be(id);
        }

        [Fact]
        public void DadoIdComValorZero_QuandoValidarDataAnnotation_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO { Id = 0 };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaAlteracaoDTO.Id)));
        }

        [Fact]
        public void DadoIdComValorNegativo_QuandoValidarDataAnnotation_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO { Id = -1L };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaAlteracaoDTO.Id)));
        }

        [Fact]
        public void DadoIdComValorMaximo_QuandoValidarDataAnnotation_EntaoPassaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO
            {
                Id = long.MaxValue,
                AcervoId = 1L,
                Titulo = "Obra",
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = 1L,
                CromiaId = 1L,
                SuporteId = 1L,
                Quantidade = 1L
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeTrue();
        }

        [Fact]
        public void DadoIdNaoPreenchido_QuandoValidarDataAnnotation_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO { AcervoId = 1L };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
        }

        #endregion

        #region Testes da Propriedade AcervoId

        [Fact]
        public void DadoAcervoIdComValorValido_QuandoInstanciar_EntaoPropriedadeAcervoIdEhPreenchida()
        {
            var acervoId = 456L;
            var dto = new AcervoArteGraficaAlteracaoDTO { AcervoId = acervoId, Id = 1L };

            dto.AcervoId.Should().Be(acervoId);
        }

        [Fact]
        public void DadoAcervoIdComValorZero_QuandoValidarDataAnnotation_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO { AcervoId = 0, Id = 1L };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaAlteracaoDTO.AcervoId)));
        }

        [Fact]
        public void DadoAcervoIdComValorNegativo_QuandoValidarDataAnnotation_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO { AcervoId = -5L, Id = 1L };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaAlteracaoDTO.AcervoId)));
        }

        [Fact]
        public void DadoAcervoIdComValorMaximo_QuandoValidarDataAnnotation_EntaoPassaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO
            {
                AcervoId = long.MaxValue,
                Id = 1L,
                Titulo = "Obra",
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = 1L,
                CromiaId = 1L,
                SuporteId = 1L,
                Quantidade = 1L
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeTrue();
        }

        [Fact]
        public void DadoAcervoIdNaoPreenchido_QuandoValidarDataAnnotation_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO { Id = 1L };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
        }

        #endregion

        #region Testes de Herança - Propriedades de AcervoCadastroDTO

        [Fact]
        public void DadoPropriedadesHerdadas_QuandoInstanciar_EntaoPermitePreenchimentoDePropriedadesBase()
        {
            var titulo = "Título da Obra";
            var descricao = "Descrição da Obra";
            var codigo = "COD-001.AG";
            var procedencia = "Doação";
            var ano = "2024";

            var dto = new AcervoArteGraficaAlteracaoDTO
            {
                Id = 1L,
                AcervoId = 1L,
                Titulo = titulo,
                Descricao = descricao,
                Codigo = codigo,
                Procedencia = procedencia,
                Ano = ano,
                ConservacaoId = 1L,
                CromiaId = 1L,
                SuporteId = 1L,
                Quantidade = 1L
            };

            dto.Titulo.Should().Be(titulo);
            dto.Descricao.Should().Be(descricao);
            dto.Codigo.Should().Be(codigo);
            dto.Procedencia.Should().Be(procedencia);
            dto.Ano.Should().Be(ano);
        }

        [Fact]
        public void DadoPropriedadesEspecificasArteGrafica_QuandoInstanciar_EntaoPermitePreenchimento()
        {
            var localizacao = "Gaveta A";
            var copiaDigital = true;
            var permiteUsoImagem = false;
            var conservacaoId = 1L;
            var cromiaId = 2L;
            var largura = "10.5";
            var altura = "15.5";
            var diametro = "5.0";
            var tecnica = "Litografia";
            var suporteId = 3L;
            var quantidade = 5L;
            var arquivos = new long[] { 1L, 2L, 3L };

            var dto = new AcervoArteGraficaAlteracaoDTO
            {
                Id = 1L,
                AcervoId = 1L,
                Titulo = "Obra",
                Ano = "2024",
                Procedencia = "Doação",
                Localizacao = localizacao,
                CopiaDigital = copiaDigital,
                PermiteUsoImagem = permiteUsoImagem,
                ConservacaoId = conservacaoId,
                CromiaId = cromiaId,
                Largura = largura,
                Altura = altura,
                Diametro = diametro,
                Tecnica = tecnica,
                SuporteId = suporteId,
                Quantidade = quantidade,
                Arquivos = arquivos
            };

            dto.Localizacao.Should().Be(localizacao);
            dto.CopiaDigital.Should().Be(copiaDigital);
            dto.PermiteUsoImagem.Should().Be(permiteUsoImagem);
            dto.ConservacaoId.Should().Be(conservacaoId);
            dto.CromiaId.Should().Be(cromiaId);
            dto.Largura.Should().Be(largura);
            dto.Altura.Should().Be(altura);
            dto.Diametro.Should().Be(diametro);
            dto.Tecnica.Should().Be(tecnica);
            dto.SuporteId.Should().Be(suporteId);
            dto.Quantidade.Should().Be(quantidade);
            dto.Arquivos.Should().BeEquivalentTo(arquivos);
        }

        #endregion

        #region Testes de Combinação de Validações

        [Fact]
        public void DadoTodosCamposObrigatoriosPreenchidos_QuandoValidar_EntaoPassaEmTodasAsValidacoes()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO
            {
                Id = 1L,
                AcervoId = 1L,
                Titulo = "Obra Arte Gráfica",
                Ano = "2024",
                Codigo = "COD-001.AG",
                Procedencia = "Doação",
                ConservacaoId = 1L,
                CromiaId = 1L,
                SuporteId = 1L,
                Quantidade = 1L
            };

            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeTrue();
            resultados.Should().BeEmpty();
        }

        [Fact]
        public void DadoCamposObrigatoriosNaoPreenchidos_QuandoValidar_EntaoFalhaEmMultiplasValidacoes()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO();
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Count.Should().BeGreaterThan(0);
        }

        #endregion

        #region Testes de Valores Limites

        [Fact]
        public void DadoIdComValor1_QuandoValidar_EntaoPassaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO
            {
                Id = 1L,
                AcervoId = 1L,
                Titulo = "Obra",
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = 1L,
                CromiaId = 1L,
                SuporteId = 1L,
                Quantidade = 1L
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeTrue();
        }

        [Fact]
        public void DadoAcervoIdComValor1_QuandoValidar_EntaoPassaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO
            {
                Id = 1L,
                AcervoId = 1L,
                Titulo = "Obra",
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = 1L,
                CromiaId = 1L,
                SuporteId = 1L,
                Quantidade = 1L
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeTrue();
        }

        #endregion

        #region Testes de Propriedades Opcionais

        [Fact]
        public void DadoPropriedadesOpcionaisNulas_QuandoValidar_EntaoPassaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO
            {
                Id = 1L,
                AcervoId = 1L,
                Titulo = "Obra",
                Ano = "2024",
                Codigo = "COD-001",
                Procedencia = "Doação",
                ConservacaoId = 1L,
                CromiaId = 1L,
                SuporteId = 1L,
                Quantidade = 1L,
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

        [Fact]
        public void DadoPropriedadesOpcionaisVazias_QuandoValidar_EntaoPassaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO
            {
                Id = 1L,
                AcervoId = 1L,
                Titulo = "Obra",
                Ano = "2024",
                Codigo = "COD-001",
                Procedencia = "Doação",
                ConservacaoId = 1L,
                CromiaId = 1L,
                SuporteId = 1L,
                Quantidade = 1L,
                Localizacao = "",
                Largura = "",
                Altura = "",
                Diametro = "",
                Tecnica = "",
                Arquivos = new long[] { },
                Descricao = ""
            };

            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeTrue();
        }

        #endregion

        #region Testes de Mensagens de Validação

        [Fact]
        public void DadoIdNaoInformado_QuandoValidar_EntaoMostrarMensagemEsperada()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO { AcervoId = 1L };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            Validator.TryValidateObject(dto, contexto, resultados, true);

            var erro = resultados.FirstOrDefault(r => r.MemberNames.Contains(nameof(AcervoArteGraficaAlteracaoDTO.Id)));
            erro.Should().NotBeNull();
            erro!.ErrorMessage.Should().Contain("O identificador do acervo arte gráfica deve ser maior que zero");
        }

        [Fact]
        public void DadoAcervoIdNaoInformado_QuandoValidar_EntaoMostrarMensagemEsperada()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO { Id = 1L };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            Validator.TryValidateObject(dto, contexto, resultados, true);

            var erro = resultados.FirstOrDefault(r => r.MemberNames.Contains(nameof(AcervoArteGraficaAlteracaoDTO.AcervoId)));
            erro.Should().NotBeNull();
            erro!.ErrorMessage.Should().Contain("O identificador do acervo deve ser maior que zero");
        }

        [Fact]
        public void DadoIdMenorQueUm_QuandoValidar_EntaoMostrarMensagemEsperadaPorRange()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO { Id = 0, AcervoId = 1L };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            Validator.TryValidateObject(dto, contexto, resultados, true);

            var erro = resultados.FirstOrDefault(r => r.MemberNames.Contains(nameof(AcervoArteGraficaAlteracaoDTO.Id)));
            erro.Should().NotBeNull();
            erro!.ErrorMessage.Should().Contain("deve ser maior que zero");
        }

        [Fact]
        public void DadoAcervoIdMenorQueUm_QuandoValidar_EntaoMostrarMensagemEsperadaPorRange()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO { Id = 1L, AcervoId = 0 };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            Validator.TryValidateObject(dto, contexto, resultados, true);

            var erro = resultados.FirstOrDefault(r => r.MemberNames.Contains(nameof(AcervoArteGraficaAlteracaoDTO.AcervoId)));
            erro.Should().NotBeNull();
            erro!.ErrorMessage.Should().Contain("deve ser maior que zero");
        }

        #endregion

        #region Testes de Acessibilidade de Propriedades

        [Fact]
        public void DadoInstancia_QuandoVerificarSePropriedadesExistem_EntaoTodosOsGettersESettersEstaDisponíveis()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO();
            var tipo = typeof(AcervoArteGraficaAlteracaoDTO);
            var propriedades = tipo.GetProperties();

            propriedades.Should().NotBeEmpty();
            propriedades.Select(p => p.Name).Should().Contain(new[] { "Id", "AcervoId" });
        }

        #endregion

        #region Testes de Instanciação e Inicialização

        [Fact]
        public void DadoConstrutorPadrao_QuandoChamar_EntaoInstanciaComSucesso()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<AcervoArteGraficaAlteracaoDTO>();
        }

        [Fact]
        public void DadoInstancia_QuandoCopiarPropriedades_EntaoValoresMantemSemCorrupcao()
        {
            var dto1 = new AcervoArteGraficaAlteracaoDTO { Id = 100L, AcervoId = 200L };

            var dto2 = new AcervoArteGraficaAlteracaoDTO { Id = dto1.Id, AcervoId = dto1.AcervoId };

            dto2.Id.Should().Be(dto1.Id);
            dto2.AcervoId.Should().Be(dto1.AcervoId);
        }

        #endregion

        #region Testes de Campos Obrigatórios Herdados

        [Fact]
        public void DadoTituloNaoPreenchido_QuandoValidar_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO
            {
                Id = 1L,
                AcervoId = 1L,
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = 1L,
                CromiaId = 1L,
                SuporteId = 1L,
                Quantidade = 1L
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaAlteracaoDTO.Titulo)));
        }

        [Fact]
        public void DadoAnoNaoPreenchido_QuandoValidar_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO
            {
                Id = 1L,
                AcervoId = 1L,
                Titulo = "Obra",
                Procedencia = "Doação",
                ConservacaoId = 1L,
                CromiaId = 1L,
                SuporteId = 1L,
                Quantidade = 1L
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaAlteracaoDTO.Ano)));
        }

        [Fact]
        public void DadoProcedenciaNaoPreenchida_QuandoValidar_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO
            {
                Id = 1L,
                AcervoId = 1L,
                Titulo = "Obra",
                Ano = "2024",
                ConservacaoId = 1L,
                CromiaId = 1L,
                SuporteId = 1L,
                Quantidade = 1L
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaAlteracaoDTO.Procedencia)));
        }

        [Fact]
        public void DadoConservacaoIdNaoPreenchido_QuandoValidar_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO
            {
                Id = 1L,
                AcervoId = 1L,
                Titulo = "Obra",
                Ano = "2024",
                Procedencia = "Doação",
                CromiaId = 1L,
                SuporteId = 1L,
                Quantidade = 1L
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaAlteracaoDTO.ConservacaoId)));
        }

        [Fact]
        public void DadoCromiaIdNaoPreenchido_QuandoValidar_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO
            {
                Id = 1L,
                AcervoId = 1L,
                Titulo = "Obra",
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = 1L,
                SuporteId = 1L,
                Quantidade = 1L
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaAlteracaoDTO.CromiaId)));
        }

        [Fact]
        public void DadoSuporteIdNaoPreenchido_QuandoValidar_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO
            {
                Id = 1L,
                AcervoId = 1L,
                Titulo = "Obra",
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = 1L,
                CromiaId = 1L,
                Quantidade = 1L
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaAlteracaoDTO.SuporteId)));
        }

        [Fact]
        public void DadoQuantidadeNaoPreenchida_QuandoValidar_EntaoFalhaValidacao()
        {
            var dto = new AcervoArteGraficaAlteracaoDTO
            {
                Id = 1L,
                AcervoId = 1L,
                Titulo = "Obra",
                Ano = "2024",
                Procedencia = "Doação",
                ConservacaoId = 1L,
                CromiaId = 1L,
                SuporteId = 1L
            };
            var contexto = new ValidationContext(dto);
            var resultados = new List<ValidationResult>();

            var ehValido = Validator.TryValidateObject(dto, contexto, resultados, true);

            ehValido.Should().BeFalse();
            resultados.Should().Contain(r => r.MemberNames.Contains(nameof(AcervoArteGraficaAlteracaoDTO.Quantidade)));
        }

        #endregion
    }
}