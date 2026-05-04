using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoArteGraficaDTOTeste
    {
        #region Testes de Instanciação

        [Fact]
        public void DadoConstrutorPadrao_QuandoChamar_EntaoInstanciaComSucesso()
        {
            var dto = new AcervoArteGraficaDTO();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<AcervoArteGraficaDTO>();
        }

        [Fact]
        public void DadoInstancia_QuandoVerificar_EntaoEhValida()
        {
            var dto = new AcervoArteGraficaDTO();

            dto.Should().BeAssignableTo<AcervoArteGraficaDTO>();
        }

        #endregion

        #region Testes de Propriedades Primitivas

        [Fact]
        public void DadoIdComValor_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Id = 123 };

            dto.Id.Should().Be(123);
        }

        [Fact]
        public void DadoIdComValorZero_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Id = 0 };

            dto.Id.Should().Be(0);
        }

        [Fact]
        public void DadoIdComValorMaximo_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Id = long.MaxValue };

            dto.Id.Should().Be(long.MaxValue);
        }

        [Fact]
        public void DadoAcervoIdComValor_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { AcervoId = 456 };

            dto.AcervoId.Should().Be(456);
        }

        [Fact]
        public void DadoAcervoIdComValorZero_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { AcervoId = 0 };

            dto.AcervoId.Should().Be(0);
        }

        [Fact]
        public void DadoTipoAcervoIdComValor_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { TipoAcervoId = 5 };

            dto.TipoAcervoId.Should().Be(5);
        }

        #endregion

        #region Testes de Propriedades String

        [Fact]
        public void DadoTituloComValor_QuandoAtribuir_EntaoArmazena()
        {
            var titulo = "Obra de Arte Gráfica Importante";
            var dto = new AcervoArteGraficaDTO { Titulo = titulo };

            dto.Titulo.Should().Be(titulo);
        }

        [Fact]
        public void DadoTituloNulo_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Titulo = null };

            dto.Titulo.Should().BeNull();
        }

        [Fact]
        public void DadoTituloVazio_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Titulo = string.Empty };

            dto.Titulo.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoCodigoComValor_QuandoAtribuir_EntaoArmazena()
        {
            var codigo = "AG-2024-001";
            var dto = new AcervoArteGraficaDTO { Codigo = codigo };

            dto.Codigo.Should().Be(codigo);
        }

        [Fact]
        public void DadoCodigoNulo_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Codigo = null };

            dto.Codigo.Should().BeNull();
        }

        [Fact]
        public void DadoLocalizacaoComValor_QuandoAtribuir_EntaoArmazena()
        {
            var localizacao = "Sala 1, Prateleira 5";
            var dto = new AcervoArteGraficaDTO { Localizacao = localizacao };

            dto.Localizacao.Should().Be(localizacao);
        }

        [Fact]
        public void DadoLocalizacaoNula_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Localizacao = null };

            dto.Localizacao.Should().BeNull();
        }

        [Fact]
        public void DadoProcedenciaComValor_QuandoAtribuir_EntaoArmazena()
        {
            var procedencia = "Doação de Acervo Público";
            var dto = new AcervoArteGraficaDTO { Procedencia = procedencia };

            dto.Procedencia.Should().Be(procedencia);
        }

        [Fact]
        public void DadoProcedenciaNula_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Procedencia = null };

            dto.Procedencia.Should().BeNull();
        }

        [Fact]
        public void DadoDescricaoComValor_QuandoAtribuir_EntaoArmazena()
        {
            var descricao = "Descrição detalhada da obra de arte gráfica";
            var dto = new AcervoArteGraficaDTO { Descricao = descricao };

            dto.Descricao.Should().Be(descricao);
        }

        [Fact]
        public void DadoDescricaoNula_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Descricao = null };

            dto.Descricao.Should().BeNull();
        }

        [Fact]
        public void DadoLarguraComValor_QuandoAtribuir_EntaoArmazena()
        {
            var largura = "25.5 cm";
            var dto = new AcervoArteGraficaDTO { Largura = largura };

            dto.Largura.Should().Be(largura);
        }

        [Fact]
        public void DadoLarguraNula_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Largura = null };

            dto.Largura.Should().BeNull();
        }

        [Fact]
        public void DadoAlturaComValor_QuandoAtribuir_EntaoArmazena()
        {
            var altura = "35.0 cm";
            var dto = new AcervoArteGraficaDTO { Altura = altura };

            dto.Altura.Should().Be(altura);
        }

        [Fact]
        public void DadoAlturaNula_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Altura = null };

            dto.Altura.Should().BeNull();
        }

        [Fact]
        public void DadoDiametroComValor_QuandoAtribuir_EntaoArmazena()
        {
            var diametro = "50.0 cm";
            var dto = new AcervoArteGraficaDTO { Diametro = diametro };

            dto.Diametro.Should().Be(diametro);
        }

        [Fact]
        public void DadoDiametroNulo_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Diametro = null };

            dto.Diametro.Should().BeNull();
        }

        [Fact]
        public void DadoTecnicaComValor_QuandoAtribuir_EntaoArmazena()
        {
            var tecnica = "Litografia";
            var dto = new AcervoArteGraficaDTO { Tecnica = tecnica };

            dto.Tecnica.Should().Be(tecnica);
        }

        [Fact]
        public void DadoTecnicaNula_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Tecnica = null };

            dto.Tecnica.Should().BeNull();
        }

        [Fact]
        public void DadoAnoComValor_QuandoAtribuir_EntaoArmazena()
        {
            var ano = "2024";
            var dto = new AcervoArteGraficaDTO { Ano = ano };

            dto.Ano.Should().Be(ano);
        }

        [Fact]
        public void DadoAnoNulo_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Ano = null };

            dto.Ano.Should().BeNull();
        }

        #endregion

        #region Testes de Propriedades Booleanas Anuláveis

        [Fact]
        public void DadoCopiaDigitalComValorTrue_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { CopiaDigital = true };

            dto.CopiaDigital.Should().BeTrue();
        }

        [Fact]
        public void DadoCopiaDigitalComValorFalse_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { CopiaDigital = false };

            dto.CopiaDigital.Should().BeFalse();
        }

        [Fact]
        public void DadoCopiaDigitalNula_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { CopiaDigital = null };

            dto.CopiaDigital.Should().BeNull();
        }

        [Fact]
        public void DadoPermiteUsoImagemComValorTrue_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { PermiteUsoImagem = true };

            dto.PermiteUsoImagem.Should().BeTrue();
        }

        [Fact]
        public void DadoPermiteUsoImagemComValorFalse_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { PermiteUsoImagem = false };

            dto.PermiteUsoImagem.Should().BeFalse();
        }

        [Fact]
        public void DadoPermiteUsoImagemNula_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { PermiteUsoImagem = null };

            dto.PermiteUsoImagem.Should().BeNull();
        }

        #endregion

        #region Testes de Propriedades Long Anuláveis

        [Fact]
        public void DadoConservacaoIdComValor_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { ConservacaoId = 10 };

            dto.ConservacaoId.Should().Be(10);
        }

        [Fact]
        public void DadoConservacaoIdComValorZero_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { ConservacaoId = 0 };

            dto.ConservacaoId.Should().Be(0);
        }

        [Fact]
        public void DadoConservacaoIdNula_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { ConservacaoId = null };

            dto.ConservacaoId.Should().BeNull();
        }

        [Fact]
        public void DadoCromiaIdComValor_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { CromiaId = 15 };

            dto.CromiaId.Should().Be(15);
        }

        [Fact]
        public void DadoCromiaIdComValorZero_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { CromiaId = 0 };

            dto.CromiaId.Should().Be(0);
        }

        [Fact]
        public void DadoCromiaIdNula_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { CromiaId = null };

            dto.CromiaId.Should().BeNull();
        }

        [Fact]
        public void DadoSuporteIdComValor_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { SuporteId = 20 };

            dto.SuporteId.Should().Be(20);
        }

        [Fact]
        public void DadoSuporteIdComValorZero_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { SuporteId = 0 };

            dto.SuporteId.Should().Be(0);
        }

        [Fact]
        public void DadoSuporteIdNula_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { SuporteId = null };

            dto.SuporteId.Should().BeNull();
        }

        [Fact]
        public void DadoQuantidadeComValor_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Quantidade = 5 };

            dto.Quantidade.Should().Be(5);
        }

        [Fact]
        public void DadoQuantidadeComValorZero_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Quantidade = 0 };

            dto.Quantidade.Should().Be(0);
        }

        [Fact]
        public void DadoQuantidadeNula_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Quantidade = null };

            dto.Quantidade.Should().BeNull();
        }

        [Fact]
        public void DadoQuantidadeComValorMaximo_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Quantidade = long.MaxValue };

            dto.Quantidade.Should().Be(long.MaxValue);
        }

        #endregion

        #region Testes de Propriedades de Arrays

        [Fact]
        public void DadoArquivosComValores_QuandoAtribuir_EntaoArmazena()
        {
            var arquivos = new ArquivoResumidoDTO[]
            {
                new ArquivoResumidoDTO { Id = 1, Nome = "arquivo1.jpg", Codigo = Guid.NewGuid() },
                new ArquivoResumidoDTO { Id = 2, Nome = "arquivo2.pdf", Codigo = Guid.NewGuid() }
            };

            var dto = new AcervoArteGraficaDTO { Arquivos = arquivos };

            dto.Arquivos.Should().HaveCount(2);
            dto.Arquivos.Should().BeEquivalentTo(arquivos);
        }

        [Fact]
        public void DadoArquivosVazio_QuandoAtribuir_EntaoArmazena()
        {
            var arquivos = new ArquivoResumidoDTO[] { };
            var dto = new AcervoArteGraficaDTO { Arquivos = arquivos };

            dto.Arquivos.Should().HaveCount(0);
        }

        [Fact]
        public void DadoArquivosNulo_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Arquivos = null };

            dto.Arquivos.Should().BeNull();
        }

        [Fact]
        public void DadoUmArquivo_QuandoAtribuir_EntaoArmazena()
        {
            var arquivo = new ArquivoResumidoDTO 
            { 
                Id = 1, 
                Nome = "obra.jpg", 
                Codigo = Guid.NewGuid() 
            };

            var dto = new AcervoArteGraficaDTO { Arquivos = new[] { arquivo } };

            dto.Arquivos.Should().HaveCount(1);
            dto.Arquivos[0].Id.Should().Be(1);
            dto.Arquivos[0].Nome.Should().Be("obra.jpg");
        }

        [Fact]
        public void DadoCreditosAutoresIdsComValores_QuandoAtribuir_EntaoArmazena()
        {
            var ids = new long[] { 1, 2, 3 };
            var dto = new AcervoArteGraficaDTO { CreditosAutoresIds = ids };

            dto.CreditosAutoresIds.Should().BeEquivalentTo(ids);
        }

        [Fact]
        public void DadoCreditosAutoresIdsVazio_QuandoAtribuir_EntaoArmazena()
        {
            var ids = new long[] { };
            var dto = new AcervoArteGraficaDTO { CreditosAutoresIds = ids };

            dto.CreditosAutoresIds.Should().HaveCount(0);
        }

        [Fact]
        public void DadoCreditosAutoresIdsNulo_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { CreditosAutoresIds = null };

            dto.CreditosAutoresIds.Should().BeNull();
        }

        #endregion

        #region Testes de Propriedades Complexas

        [Fact]
        public void DadoAuditoriaComValor_QuandoAtribuir_EntaoArmazena()
        {
            var auditoria = new AuditoriaDTO 
            {
                CriadoEm = DateTime.Now,
                CriadoPor = "usuario@teste.com"
            };

            var dto = new AcervoArteGraficaDTO { Auditoria = auditoria };

            dto.Auditoria.Should().NotBeNull();
            dto.Auditoria.Should().Be(auditoria);
        }

        [Fact]
        public void DadoAuditoriaNula_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Auditoria = null };

            dto.Auditoria.Should().BeNull();
        }

        [Fact]
        public void DadoSituacaoAcervoAtivo_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { SituacaoAcervo = SituacaoAcervo.Ativo };

            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Ativo);
        }

        [Fact]
        public void DadoSituacaoAcervoInativo_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { SituacaoAcervo = SituacaoAcervo.Inativo };

            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Inativo);
        }

        #endregion

        #region Testes de Múltiplas Propriedades Combinadas

        [Fact]
        public void DadoTodosCamposPreenchidos_QuandoCriar_EntaoTodosArmazenam()
        {
            var arquivos = new ArquivoResumidoDTO[]
            {
                new ArquivoResumidoDTO { Id = 1, Nome = "obra.jpg", Codigo = Guid.NewGuid() }
            };
            var auditoria = new AuditoriaDTO();
            var creditosIds = new long[] { 1, 2, 3 };

            var dto = new AcervoArteGraficaDTO
            {
                Id = 100,
                AcervoId = 50,
                Titulo = "Grande Obra de Arte",
                TipoAcervoId = 3,
                Codigo = "AG-001",
                Localizacao = "Sala Principal",
                Procedencia = "Herança",
                CopiaDigital = true,
                PermiteUsoImagem = false,
                ConservacaoId = 1,
                CromiaId = 2,
                Largura = "30cm",
                Altura = "40cm",
                Diametro = "5cm",
                Tecnica = "Gravura",
                SuporteId = 4,
                Quantidade = 10,
                Descricao = "Descrição completa",
                Arquivos = arquivos,
                Auditoria = auditoria,
                CreditosAutoresIds = creditosIds,
                Ano = "2023",
                SituacaoAcervo = SituacaoAcervo.Ativo
            };

            dto.Id.Should().Be(100);
            dto.AcervoId.Should().Be(50);
            dto.Titulo.Should().Be("Grande Obra de Arte");
            dto.TipoAcervoId.Should().Be(3);
            dto.Codigo.Should().Be("AG-001");
            dto.Localizacao.Should().Be("Sala Principal");
            dto.Procedencia.Should().Be("Herança");
            dto.CopiaDigital.Should().BeTrue();
            dto.PermiteUsoImagem.Should().BeFalse();
            dto.ConservacaoId.Should().Be(1);
            dto.CromiaId.Should().Be(2);
            dto.Largura.Should().Be("30cm");
            dto.Altura.Should().Be("40cm");
            dto.Diametro.Should().Be("5cm");
            dto.Tecnica.Should().Be("Gravura");
            dto.SuporteId.Should().Be(4);
            dto.Quantidade.Should().Be(10);
            dto.Descricao.Should().Be("Descrição completa");
            dto.Arquivos.Should().HaveCount(1);
            dto.Auditoria.Should().Be(auditoria);
            dto.CreditosAutoresIds.Should().HaveCount(3);
            dto.Ano.Should().Be("2023");
            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Ativo);
        }

        [Fact]
        public void DadoMinimoDeCamposPreenchidos_QuandoCriar_EntaoOutrosCamposPermanecemNulos()
        {
            var dto = new AcervoArteGraficaDTO 
            { 
                Titulo = "Título Mínimo",
                TipoAcervoId = 1
            };

            dto.Titulo.Should().Be("Título Mínimo");
            dto.TipoAcervoId.Should().Be(1);
            dto.Id.Should().Be(0);
            dto.AcervoId.Should().Be(0);
            dto.Codigo.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.Arquivos.Should().BeNull();
            dto.Auditoria.Should().BeNull();
        }

        #endregion

        #region Testes de Alteração Sequencial de Propriedades

        [Fact]
        public void DadoPropriedadeAlteradaMultiplaVezes_QuandoAlterar_EntaoMantémUltimoValor()
        {
            var dto = new AcervoArteGraficaDTO();

            dto.Titulo = "Título 1";
            dto.Titulo = "Título 2";
            dto.Titulo = "Título 3";

            dto.Titulo.Should().Be("Título 3");
        }

        [Fact]
        public void DadoIdAlteradoMultiplaVezes_QuandoAlterar_EntaoMantémUltimoValor()
        {
            var dto = new AcervoArteGraficaDTO();

            dto.Id = 1;
            dto.Id = 100;
            dto.Id = 999;

            dto.Id.Should().Be(999);
        }

        [Fact]
        public void DadoBooleanoAlteradoMultiplaVezes_QuandoAlterar_EntaoMantémUltimoValor()
        {
            var dto = new AcervoArteGraficaDTO();

            dto.CopiaDigital = true;
            dto.CopiaDigital = false;
            dto.CopiaDigital = true;

            dto.CopiaDigital.Should().BeTrue();
        }

        #endregion

        #region Testes de Tipos de Propriedades por Reflexão

        [Fact]
        public void DadoPropriedadesString_QuandoVerificar_EntaoSaoTipoString()
        {
            var tipo = typeof(AcervoArteGraficaDTO);

            var propriedadesString = new[]
            {
                "Titulo", "Codigo", "Localizacao", "Procedencia",
                "Largura", "Altura", "Diametro", "Tecnica", "Descricao", "Ano"
            };

            foreach (var propriedade in propriedadesString)
            {
                var prop = tipo.GetProperty(propriedade);
                prop.Should().NotBeNull($"A propriedade {propriedade} deve existir");
                prop!.PropertyType.Should().Be(typeof(string));
            }
        }

        [Fact]
        public void DadoPropriedadesLong_QuandoVerificar_EntaoSaoTipoLong()
        {
            var tipo = typeof(AcervoArteGraficaDTO);

            var propriedadesLong = new[] { "Id", "AcervoId", "TipoAcervoId" };

            foreach (var propriedade in propriedadesLong)
            {
                var prop = tipo.GetProperty(propriedade);
                prop.Should().NotBeNull($"A propriedade {propriedade} deve existir");
                prop!.PropertyType.Should().Be(typeof(long));
            }
        }

        [Fact]
        public void DadoPropriedadesLongAnulavel_QuandoVerificar_EntaoSaoTipoLongAnulavel()
        {
            var tipo = typeof(AcervoArteGraficaDTO);

            var propriedadesLongAnulavel = new[] 
            { 
                "ConservacaoId", "CromiaId", "SuporteId", "Quantidade" 
            };

            foreach (var propriedade in propriedadesLongAnulavel)
            {
                var prop = tipo.GetProperty(propriedade);
                prop.Should().NotBeNull($"A propriedade {propriedade} deve existir");
                prop!.PropertyType.Should().Be(typeof(long?));
            }
        }

        [Fact]
        public void DadoPropriedadesBoolAnulavel_QuandoVerificar_EntaoSaoTipoBoolAnulavel()
        {
            var tipo = typeof(AcervoArteGraficaDTO);

            var propriedadesBoolAnulavel = new[] { "CopiaDigital", "PermiteUsoImagem" };

            foreach (var propriedade in propriedadesBoolAnulavel)
            {
                var prop = tipo.GetProperty(propriedade);
                prop.Should().NotBeNull($"A propriedade {propriedade} deve existir");
                prop!.PropertyType.Should().Be(typeof(bool?));
            }
        }

        [Fact]
        public void DadoPropriedadeArquivos_QuandoVerificar_EntaoEhArrayDeArquivoResumidoDTO()
        {
            var tipo = typeof(AcervoArteGraficaDTO);
            var propriedade = tipo.GetProperty("Arquivos");

            propriedade.Should().NotBeNull();
            propriedade!.PropertyType.Should().Be(typeof(ArquivoResumidoDTO[]));
        }

        [Fact]
        public void DadoPropriedadeAuditoria_QuandoVerificar_EntaoEhTipoAuditoriaDTO()
        {
            var tipo = typeof(AcervoArteGraficaDTO);
            var propriedade = tipo.GetProperty("Auditoria");

            propriedade.Should().NotBeNull();
            propriedade!.PropertyType.Should().Be(typeof(AuditoriaDTO));
        }

        [Fact]
        public void DadoPropriedadeCreditosAutoresIds_QuandoVerificar_EntaoEhArrayDeLong()
        {
            var tipo = typeof(AcervoArteGraficaDTO);
            var propriedade = tipo.GetProperty("CreditosAutoresIds");

            propriedade.Should().NotBeNull();
            propriedade!.PropertyType.Should().Be(typeof(long[]));
        }

        [Fact]
        public void DadoPropriedadeSituacaoAcervo_QuandoVerificar_EntaoEhTipoSituacaoAcervo()
        {
            var tipo = typeof(AcervoArteGraficaDTO);
            var propriedade = tipo.GetProperty("SituacaoAcervo");

            propriedade.Should().NotBeNull();
            propriedade!.PropertyType.Should().Be(typeof(SituacaoAcervo));
        }

        #endregion

        #region Testes de Acessibilidade de Propriedades

        [Fact]
        public void DadoTodasAsPropriedades_QuandoVerificar_EntaoTemGettersESettersPublicos()
        {
            var tipo = typeof(AcervoArteGraficaDTO);
            var propriedades = tipo.GetProperties();

            propriedades.Should().NotBeEmpty();

            foreach (var propriedade in propriedades)
            {
                var getter = propriedade.GetGetMethod();
                var setter = propriedade.GetSetMethod();

                getter.Should().NotBeNull($"Propriedade {propriedade.Name} deve ter getter");
                getter!.IsPublic.Should().BeTrue($"Getter de {propriedade.Name} deve ser público");
                
                setter.Should().NotBeNull($"Propriedade {propriedade.Name} deve ter setter");
                setter!.IsPublic.Should().BeTrue($"Setter de {propriedade.Name} deve ser público");
            }
        }

        [Fact]
        public void DadoClasse_QuandoVerificar_EntaoEhPublica()
        {
            var tipo = typeof(AcervoArteGraficaDTO);

            tipo.IsPublic.Should().BeTrue();
        }

        #endregion

        #region Testes de Reflexão e Estrutura

        [Fact]
        public void DadoClasse_QuandoVerificar_EntaoTemTodasAsPropriedadesEsperadas()
        {
            var tipo = typeof(AcervoArteGraficaDTO);
            var propriedades = tipo.GetProperties();

            var propriedadesEsperadas = new[]
            {
                nameof(AcervoArteGraficaDTO.Id),
                nameof(AcervoArteGraficaDTO.AcervoId),
                nameof(AcervoArteGraficaDTO.Titulo),
                nameof(AcervoArteGraficaDTO.TipoAcervoId),
                nameof(AcervoArteGraficaDTO.Codigo),
                nameof(AcervoArteGraficaDTO.Localizacao),
                nameof(AcervoArteGraficaDTO.Procedencia),
                nameof(AcervoArteGraficaDTO.CopiaDigital),
                nameof(AcervoArteGraficaDTO.PermiteUsoImagem),
                nameof(AcervoArteGraficaDTO.ConservacaoId),
                nameof(AcervoArteGraficaDTO.CromiaId),
                nameof(AcervoArteGraficaDTO.Largura),
                nameof(AcervoArteGraficaDTO.Altura),
                nameof(AcervoArteGraficaDTO.Diametro),
                nameof(AcervoArteGraficaDTO.Tecnica),
                nameof(AcervoArteGraficaDTO.SuporteId),
                nameof(AcervoArteGraficaDTO.Quantidade),
                nameof(AcervoArteGraficaDTO.Descricao),
                nameof(AcervoArteGraficaDTO.Arquivos),
                nameof(AcervoArteGraficaDTO.Auditoria),
                nameof(AcervoArteGraficaDTO.CreditosAutoresIds),
                nameof(AcervoArteGraficaDTO.Ano),
                nameof(AcervoArteGraficaDTO.SituacaoAcervo)
            };

            foreach (var propEsperada in propriedadesEsperadas)
            {
                propriedades.Should().Contain(p => p.Name == propEsperada,
                    $"A propriedade {propEsperada} deve estar presente na classe");
            }
        }

        [Fact]
        public void DadoClasse_QuandoVerificar_EntaoTemQuantidadeCorretaDePropriedades()
        {
            var tipo = typeof(AcervoArteGraficaDTO);
            var propriedades = tipo.GetProperties();

            propriedades.Should().HaveCount(23);
        }

        #endregion

        #region Testes de Valores Extremos

        [Fact]
        public void DadoDescricaoMuitoLonga_QuandoAtribuir_EntaoArmazena()
        {
            var descricaoLonga = new string('A', 10000);
            var dto = new AcervoArteGraficaDTO { Descricao = descricaoLonga };

            dto.Descricao.Should().HaveLength(10000);
            dto.Descricao.Should().Be(descricaoLonga);
        }

        [Fact]
        public void DadoIdComValorNegativo_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Id = -1 };

            dto.Id.Should().Be(-1);
        }

        [Fact]
        public void DadoQuantidadeComValorNegativo_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { Quantidade = -100 };

            dto.Quantidade.Should().Be(-100);
        }

        [Fact]
        public void DadoMultiplosArquivos_QuandoAtribuir_EntaoArmazenaTodos()
        {
            var arquivos = new ArquivoResumidoDTO[1000];
            for (int i = 0; i < 1000; i++)
            {
                arquivos[i] = new ArquivoResumidoDTO 
                { 
                    Id = i, 
                    Nome = $"arquivo_{i}.jpg", 
                    Codigo = Guid.NewGuid() 
                };
            }

            var dto = new AcervoArteGraficaDTO { Arquivos = arquivos };

            dto.Arquivos.Should().HaveCount(1000);
        }

        #endregion

        #region Testes de Valores Limites de Enumeração

        [Fact]
        public void DadoSituacaoAcervoPrimeiroValor_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { SituacaoAcervo = (SituacaoAcervo)1 };

            dto.SituacaoAcervo.Should().Be((SituacaoAcervo)1);
        }

        [Fact]
        public void DadoSituacaoAcervoSegundoValor_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDTO { SituacaoAcervo = (SituacaoAcervo)2 };

            dto.SituacaoAcervo.Should().Be((SituacaoAcervo)2);
        }

        #endregion

        #region Testes de Inicialização com Object Initializer

        [Fact]
        public void DadoObjectInitializerComMultiplosCampos_QuandoUsarParametros_EntaoTodosArmazenam()
        {
            var dto = new AcervoArteGraficaDTO
            {
                Id = 1,
                AcervoId = 2,
                Titulo = "Teste",
                Codigo = "CODE-001"
            };

            dto.Should().NotBeNull();
            dto.Id.Should().Be(1);
            dto.AcervoId.Should().Be(2);
            dto.Titulo.Should().Be("Teste");
            dto.Codigo.Should().Be("CODE-001");
        }

        [Fact]
        public void DadoObjectInitializerVazio_QuandoCriar_EntaoInstanciaComValoresPadrao()
        {
            var dto = new AcervoArteGraficaDTO { };

            dto.Should().NotBeNull();
            dto.Id.Should().Be(0);
            dto.AcervoId.Should().Be(0);
            dto.TipoAcervoId.Should().Be(0);
        }

        #endregion

        #region Testes de Referência de Objetos

        [Fact]
        public void DadoArquivosAtribuidosEModificados_QuandoVerificar_EntaoModificacaoEhVisivel()
        {
            var arquivos = new ArquivoResumidoDTO[]
            {
                new ArquivoResumidoDTO { Id = 1, Nome = "arquivo1.jpg", Codigo = Guid.NewGuid() }
            };

            var dto = new AcervoArteGraficaDTO { Arquivos = arquivos };

            arquivos[0].Nome = "arquivo_modificado.jpg";

            dto.Arquivos[0].Nome.Should().Be("arquivo_modificado.jpg");
        }

        [Fact]
        public void DadoAuditoriaAtribuidaEModificada_QuandoVerificar_EntaoModificacaoEhVisivel()
        {
            var auditoria = new AuditoriaDTO { CriadoPor = "usuario1" };
            var dto = new AcervoArteGraficaDTO { Auditoria = auditoria };

            auditoria.CriadoPor = "usuario2";

            dto.Auditoria.CriadoPor.Should().Be("usuario2");
        }

        #endregion

        #region Testes de Cópia de Valores Entre DTOs

        [Fact]
        public void DadoDuasInstancias_QuandoCopiarPropriedades_EntaoValoresMantemSemCorrupcao()
        {
            var dto1 = new AcervoArteGraficaDTO
            {
                Id = 100,
                AcervoId = 200,
                Titulo = "Obra 1",
                Quantidade = 5
            };

            var dto2 = new AcervoArteGraficaDTO
            {
                Id = dto1.Id,
                AcervoId = dto1.AcervoId,
                Titulo = dto1.Titulo,
                Quantidade = dto1.Quantidade
            };

            dto2.Id.Should().Be(dto1.Id);
            dto2.AcervoId.Should().Be(dto1.AcervoId);
            dto2.Titulo.Should().Be(dto1.Titulo);
            dto2.Quantidade.Should().Be(dto1.Quantidade);
        }

        #endregion
    }
}
