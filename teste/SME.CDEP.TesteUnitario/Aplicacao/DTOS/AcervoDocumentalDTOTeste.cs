using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoDocumentalDtoTeste
    {
        private static AcervoDocumentalDTO CriarAcervoDocumentalDTOCompleto()
        {
            return new AcervoDocumentalDTO
            {
                Id = 1,
                AcervoId = 10,
                Titulo = "Documento Importante",
                TipoAcervoId = 1,
                Codigo = "DOC-001",
                CodigoNovo = "DOC-001-NEW",
                MaterialId = 5,
                IdiomaId = 3,
                Ano = "2024",
                NumeroPagina = 250,
                Volume = "Vol. 1",
                Descricao = "Descrição completa do documento",
                TipoAnexo = "PDF",
                Largura = "21cm",
                Altura = "29.7cm",
                TamanhoArquivo = "5MB",
                Localizacao = "Sala 101 - Prateleira A",
                CopiaDigital = true,
                ConservacaoId = 2,
                Arquivos = new[]
                {
                    new ArquivoResumidoDTO { Id = 1, Nome = "arquivo1.pdf" },
                    new ArquivoResumidoDTO { Id = 2, Nome = "arquivo2.pdf" }
                },
                AcessoDocumentosIds = new[] { 1L, 2L },
                Auditoria = new AuditoriaDTO
                {
                    CriadoEm = System.DateTime.Now,
                    CriadoPor = "Usuario1",
                    CriadoLogin = "login1",
                    AlteradoEm = System.DateTime.Now,
                    AlteradoPor = "Usuario2",
                    AlteradoLogin = "login2"
                },
                CreditosAutoresIds = new[] { 1L, 2L, 3L },
                CapaDocumento = "https://exemplo.com/capa.jpg",
                SituacaoAcervo = SituacaoAcervo.Ativo
            };
        }

        [Fact]
        public void DadoAcervoDocumentalDTO_QuandoInstanciar_EntaoTodosPropriedadesSaoNulavelOuPadrao()
        {
            // Arrange & Act
            var dto = new AcervoDocumentalDTO();

            // Assert
            dto.Id.Should().Be(0);
            dto.AcervoId.Should().Be(0);
            dto.Titulo.Should().BeNull();
            dto.TipoAcervoId.Should().Be(0);
            dto.Codigo.Should().BeNull();
            dto.CodigoNovo.Should().BeNull();
            dto.MaterialId.Should().BeNull();
            dto.IdiomaId.Should().BeNull();
            dto.Ano.Should().BeNull();
            dto.NumeroPagina.Should().BeNull();
            dto.Volume.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.TipoAnexo.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.TamanhoArquivo.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.ConservacaoId.Should().BeNull();
            dto.Arquivos.Should().BeNull();
            dto.AcessoDocumentosIds.Should().BeNull();
            dto.Auditoria.Should().BeNull();
            dto.CreditosAutoresIds.Should().BeNull();
            dto.CapaDocumento.Should().BeNull();
            dto.SituacaoAcervo.Should().Be(default(SituacaoAcervo));
        }

        [Fact]
        public void DadoId_QuandoDefinirValor_EntaoIdEhAtribuido()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const long idEsperado = 999;

            // Act
            dto.Id = idEsperado;

            // Assert
            dto.Id.Should().Be(idEsperado);
        }

        [Fact]
        public void DadoAcervoId_QuandoDefinirValor_EntaoAcervoIdEhAtribuido()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const long acervoIdEsperado = 100;

            // Act
            dto.AcervoId = acervoIdEsperado;

            // Assert
            dto.AcervoId.Should().Be(acervoIdEsperado);
        }

        [Fact]
        public void DadoTitulo_QuandoDefinirValor_EntaoTituloEhAtribuido()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const string tituloEsperado = "Título do Documento";

            // Act
            dto.Titulo = tituloEsperado;

            // Assert
            dto.Titulo.Should().Be(tituloEsperado);
        }

        [Fact]
        public void DadoTipoAcervoId_QuandoDefinirValor_EntaoTipoAcervoIdEhAtribuido()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const long tipoAcervoIdEsperado = 5;

            // Act
            dto.TipoAcervoId = tipoAcervoIdEsperado;

            // Assert
            dto.TipoAcervoId.Should().Be(tipoAcervoIdEsperado);
        }

        [Fact]
        public void DadoCodigo_QuandoDefinirValor_EntaoCodigoEhAtribuido()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const string codigoEsperado = "DOC-001";

            // Act
            dto.Codigo = codigoEsperado;

            // Assert
            dto.Codigo.Should().Be(codigoEsperado);
        }

        [Fact]
        public void DadoCodigoNovo_QuandoDefinirValor_EntaoCodigoNovoEhAtribuido()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const string codigoNovoEsperado = "DOC-001-NEW";

            // Act
            dto.CodigoNovo = codigoNovoEsperado;

            // Assert
            dto.CodigoNovo.Should().Be(codigoNovoEsperado);
        }

        [Fact]
        public void DadoMaterialId_QuandoDefinirValor_EntaoMaterialIdEhAtribuido()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const long materialIdEsperado = 10;

            // Act
            dto.MaterialId = materialIdEsperado;

            // Assert
            dto.MaterialId.Should().Be(materialIdEsperado);
        }

        [Fact]
        public void DadoIdiomaId_QuandoDefinirValor_EntaoIdiomaIdEhAtribuido()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const long idiomaIdEsperado = 3;

            // Act
            dto.IdiomaId = idiomaIdEsperado;

            // Assert
            dto.IdiomaId.Should().Be(idiomaIdEsperado);
        }

        [Fact]
        public void DadoAno_QuandoDefinirValor_EntaoAnoEhAtribuido()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const string anoEsperado = "2024";

            // Act
            dto.Ano = anoEsperado;

            // Assert
            dto.Ano.Should().Be(anoEsperado);
        }

        [Fact]
        public void DadoNumeroPagina_QuandoDefinirValor_EntaoNumeroPaginaEhAtribuido()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const int numeroPaginaEsperado = 500;

            // Act
            dto.NumeroPagina = numeroPaginaEsperado;

            // Assert
            dto.NumeroPagina.Should().Be(numeroPaginaEsperado);
        }

        [Fact]
        public void DadoVolume_QuandoDefinirValor_EntaoVolumeEhAtribuido()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const string volumeEsperado = "Vol. 5";

            // Act
            dto.Volume = volumeEsperado;

            // Assert
            dto.Volume.Should().Be(volumeEsperado);
        }

        [Fact]
        public void DadoDescricao_QuandoDefinirValor_EntaoDescricaoEhAtribuida()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const string descricaoEsperada = "Descrição detalhada";

            // Act
            dto.Descricao = descricaoEsperada;

            // Assert
            dto.Descricao.Should().Be(descricaoEsperada);
        }

        [Fact]
        public void DadoTipoAnexo_QuandoDefinirValor_EntaoTipoAnexoEhAtribuido()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const string tipoAnexoEsperado = "PDF";

            // Act
            dto.TipoAnexo = tipoAnexoEsperado;

            // Assert
            dto.TipoAnexo.Should().Be(tipoAnexoEsperado);
        }

        [Fact]
        public void DadoLargura_QuandoDefinirValor_EntaoLarguraEhAtribuida()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const string larguraEsperada = "21cm";

            // Act
            dto.Largura = larguraEsperada;

            // Assert
            dto.Largura.Should().Be(larguraEsperada);
        }

        [Fact]
        public void DadoAltura_QuandoDefinirValor_EntaoAlturaEhAtribuida()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const string alturaEsperada = "29.7cm";

            // Act
            dto.Altura = alturaEsperada;

            // Assert
            dto.Altura.Should().Be(alturaEsperada);
        }

        [Fact]
        public void DadoTamanhoArquivo_QuandoDefinirValor_EntaoTamanhoArquivoEhAtribuido()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const string tamanhoArquivoEsperado = "15MB";

            // Act
            dto.TamanhoArquivo = tamanhoArquivoEsperado;

            // Assert
            dto.TamanhoArquivo.Should().Be(tamanhoArquivoEsperado);
        }

        [Fact]
        public void DadoLocalizacao_QuandoDefinirValor_EntaoLocalizacaoEhAtribuida()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const string localizacaoEsperada = "Sala 201 - Arquivo Geral";

            // Act
            dto.Localizacao = localizacaoEsperada;

            // Assert
            dto.Localizacao.Should().Be(localizacaoEsperada);
        }

        [Fact]
        public void DadoCopiaDigital_QuandoDefinirValor_EntaoCopiaDigitalEhAtribuida()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const bool copiaDigitalEsperada = true;

            // Act
            dto.CopiaDigital = copiaDigitalEsperada;

            // Assert
            dto.CopiaDigital.Should().Be(copiaDigitalEsperada);
        }

        [Fact]
        public void DadoConservacaoId_QuandoDefinirValor_EntaoConservacaoIdEhAtribuido()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const long conservacaoIdEsperado = 3;

            // Act
            dto.ConservacaoId = conservacaoIdEsperado;

            // Assert
            dto.ConservacaoId.Should().Be(conservacaoIdEsperado);
        }

        [Fact]
        public void DadoArquivos_QuandoDefinirValor_EntaoArquivosEhAtribuido()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            var arquivosEsperados = new[]
            {
                new ArquivoResumidoDTO { Id = 1, Nome = "arquivo1.pdf" },
                new ArquivoResumidoDTO { Id = 2, Nome = "arquivo2.pdf" }
            };

            // Act
            dto.Arquivos = arquivosEsperados;

            // Assert
            dto.Arquivos.Should().BeEquivalentTo(arquivosEsperados);
            dto.Arquivos.Should().HaveCount(2);
        }

        [Fact]
        public void DadoAcessoDocumentosIds_QuandoDefinirValor_EntaoAcessoDocumentosIdsEhAtribuido()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            var acessoDocumentosIdsEsperado = new[] { 1L, 2L, 3L };

            // Act
            dto.AcessoDocumentosIds = acessoDocumentosIdsEsperado;

            // Assert
            dto.AcessoDocumentosIds.Should().BeEquivalentTo(acessoDocumentosIdsEsperado);
            dto.AcessoDocumentosIds.Should().HaveCount(3);
        }

        [Fact]
        public void DadoAuditoria_QuandoDefinirValor_EntaoAuditoriaEhAtribuida()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            var auditoriaEsperada = new AuditoriaDTO
            {
                CriadoEm = System.DateTime.Now,
                CriadoPor = "Usuario1"
            };

            // Act
            dto.Auditoria = auditoriaEsperada;

            // Assert
            dto.Auditoria.Should().BeEquivalentTo(auditoriaEsperada);
            dto.Auditoria.CriadoPor.Should().Be("Usuario1");
        }

        [Fact]
        public void DadoCreditosAutoresIds_QuandoDefinirValor_EntaoCreditosAutoresIdsEhAtribuido()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            var creditosAutoresIdsEsperado = new[] { 1L, 2L, 3L, 4L };

            // Act
            dto.CreditosAutoresIds = creditosAutoresIdsEsperado;

            // Assert
            dto.CreditosAutoresIds.Should().BeEquivalentTo(creditosAutoresIdsEsperado);
            dto.CreditosAutoresIds.Should().HaveCount(4);
        }

        [Fact]
        public void DadoCapaDocumento_QuandoDefinirValor_EntaoCapaDocumentoEhAtribuida()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const string capaDocumentoEsperada = "https://exemplo.com/capa.jpg";

            // Act
            dto.CapaDocumento = capaDocumentoEsperada;

            // Assert
            dto.CapaDocumento.Should().Be(capaDocumentoEsperada);
        }

        [Fact]
        public void DadoSituacaoAcervo_QuandoDefinirValor_EntaoSituacaoAcervoEhAtribuida()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            var situacaoEsperada = SituacaoAcervo.Ativo;

            // Act
            dto.SituacaoAcervo = situacaoEsperada;

            // Assert
            dto.SituacaoAcervo.Should().Be(situacaoEsperada);
        }

        [Fact]
        public void DadoAcervoDocumentalDTO_QuandoDefiniTodosOsValores_EntaoTodosCamposSaoAcessiveis()
        {
            // Arrange & Act
            var dto = CriarAcervoDocumentalDTOCompleto();

            // Assert
            dto.Id.Should().Be(1);
            dto.AcervoId.Should().Be(10);
            dto.Titulo.Should().Be("Documento Importante");
            dto.TipoAcervoId.Should().Be(1);
            dto.Codigo.Should().Be("DOC-001");
            dto.CodigoNovo.Should().Be("DOC-001-NEW");
            dto.MaterialId.Should().Be(5);
            dto.IdiomaId.Should().Be(3);
            dto.Ano.Should().Be("2024");
            dto.NumeroPagina.Should().Be(250);
            dto.Volume.Should().Be("Vol. 1");
            dto.Descricao.Should().Be("Descrição completa do documento");
            dto.TipoAnexo.Should().Be("PDF");
            dto.Largura.Should().Be("21cm");
            dto.Altura.Should().Be("29.7cm");
            dto.TamanhoArquivo.Should().Be("5MB");
            dto.Localizacao.Should().Be("Sala 101 - Prateleira A");
            dto.CopiaDigital.Should().Be(true);
            dto.ConservacaoId.Should().Be(2);
            dto.Arquivos.Should().HaveCount(2);
            dto.AcessoDocumentosIds.Should().HaveCount(2);
            dto.CreditosAutoresIds.Should().HaveCount(3);
            dto.CapaDocumento.Should().Be("https://exemplo.com/capa.jpg");
            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Ativo);
            dto.Auditoria.Should().NotBeNull();
        }

        [Fact]
        public void DadoNulosEValoresOpcionais_QuandoNaoDefinir_EntaoApoiaPropriedadesNulaveis()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO
            {
                Id = 1,
                AcervoId = 10,
                Titulo = "Teste",
                TipoAcervoId = 1,
                Codigo = "TST"
            };

            // Act & Assert
            dto.MaterialId.Should().BeNull();
            dto.IdiomaId.Should().BeNull();
            dto.Ano.Should().BeNull();
            dto.NumeroPagina.Should().BeNull();
            dto.Volume.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.ConservacaoId.Should().BeNull();
            dto.Arquivos.Should().BeNull();
            dto.AcessoDocumentosIds.Should().BeNull();
        }

        [Fact]
        public void DadoNumeroPaginaNegativo_QuandoAtribuir_EntaoValorEhArmazenado()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const int numeroNegativo = -5;

            // Act
            dto.NumeroPagina = numeroNegativo;

            // Assert
            dto.NumeroPagina.Should().Be(numeroNegativo);
        }

        [Fact]
        public void DadoNumeroPaginaZero_QuandoAtribuir_EntaoValorEhArmazenado()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            const int numeroZero = 0;

            // Act
            dto.NumeroPagina = numeroZero;

            // Assert
            dto.NumeroPagina.Should().Be(numeroZero);
        }

        [Fact]
        public void DadoNumeroPaginaMaximo_QuandoAtribuir_EntaoValorEhArmazenado()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            var numeroMaximo = int.MaxValue;

            // Act
            dto.NumeroPagina = numeroMaximo;

            // Assert
            dto.NumeroPagina.Should().Be(numeroMaximo);
        }

        [Fact]
        public void DadoStringComComprimentoMaximo_QuandoAtribuirTitulo_EntaoTituloEhArmazenado()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            var tituloComprido = new string('a', 1000);

            // Act
            dto.Titulo = tituloComprido;

            // Assert
            dto.Titulo.Should().Be(tituloComprido);
            dto.Titulo!.Length.Should().Be(1000);
        }

        [Fact]
        public void DadoArquivosVazio_QuandoAtribuir_EntaoArrayVazioEhArmazenado()
        {
            // Arrange
            var dto = new AcervoDocumentalDTO();
            var arquivosVazios = Array.Empty<ArquivoResumidoDTO>();

            // Act
            dto.Arquivos = arquivosVazios;

            // Assert
            dto.Arquivos.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcessoDocumentosVazio_QuandoAtribuir_EntaoArrayVazioEhArmazenado()
        {
            var dto = new AcervoDocumentalDTO();
            var acessoVazio = Array.Empty<long>();

            dto.AcessoDocumentosIds = acessoVazio;

            dto.AcessoDocumentosIds.Should().BeEmpty();
        }

        [Fact]
        public void DadoCreditosVazio_QuandoAtribuir_EntaoArrayVazioEhArmazenado()
        {
            var dto = new AcervoDocumentalDTO();
            var creditosVazios = Array.Empty<long>();

            dto.CreditosAutoresIds = creditosVazios;

            dto.CreditosAutoresIds.Should().BeEmpty();
        }

        [Fact]
        public void DadoCopiaDigitalFalse_QuandoAtribuir_EntaoValorFalseEhArmazenado()
        {
            var dto = new AcervoDocumentalDTO();

            dto.CopiaDigital = false;

            dto.CopiaDigital.Should().Be(false);
        }

        [Fact]
        public void DadoMultiplosAcessosDocumentos_QuandoAtribuir_EntaoTodosIdsArmazenados()
        {
            var dto = new AcervoDocumentalDTO();
            var acessosMultiplos = new[] { 1L, 2L, 3L, 4L, 5L, 6L, 7L, 8L, 9L, 10L };

            dto.AcessoDocumentosIds = acessosMultiplos;

            dto.AcessoDocumentosIds.Should().HaveCount(10);
            dto.AcessoDocumentosIds.Should().ContainInOrder(acessosMultiplos);
        }

        [Fact]
        public void DadoMultiplosCreditosAutores_QuandoAtribuir_EntaoTodosIdsArmazenados()
        {
            var dto = new AcervoDocumentalDTO();
            var creditosMultiplos = new[] { 10L, 20L, 30L, 40L, 50L };

            dto.CreditosAutoresIds = creditosMultiplos;

            dto.CreditosAutoresIds.Should().HaveCount(5);
            dto.CreditosAutoresIds.Should().ContainInOrder(creditosMultiplos);
        }

        [Fact]
        public void DadoArquivosComIdSeqencial_QuandoAtribuir_EntaoTodosArquivosArmazenados()
        {
            var dto = new AcervoDocumentalDTO();
            var arquivos = new[]
            {
                new ArquivoResumidoDTO { Id = 1, Nome = "arquivo1.pdf" },
                new ArquivoResumidoDTO { Id = 2, Nome = "arquivo2.pdf" },
                new ArquivoResumidoDTO { Id = 3, Nome = "arquivo3.pdf" }
            };

            dto.Arquivos = arquivos;

            dto.Arquivos.Should().HaveCount(3);
            for (int i = 0; i < arquivos.Length; i++)
            {
                dto.Arquivos![i].Id.Should().Be(arquivos[i].Id);
                dto.Arquivos[i].Nome.Should().Be(arquivos[i].Nome);
            }
        }

        [Fact]
        public void DadoSituacaoInativa_QuandoAtribuir_EntaoSituacaoInativaEhArmazenada()
        {
            var dto = new AcervoDocumentalDTO();

            dto.SituacaoAcervo = SituacaoAcervo.Inativo;

            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Inativo);
        }

        [Fact]
        public void DadoIdsMaximos_QuandoAtribuir_EntaoValoresMaximosArmazenados()
        {
            var dto = new AcervoDocumentalDTO();
            var idMaximo = long.MaxValue;

            dto.Id = idMaximo;
            dto.AcervoId = idMaximo;
            dto.TipoAcervoId = idMaximo;

            dto.Id.Should().Be(idMaximo);
            dto.AcervoId.Should().Be(idMaximo);
            dto.TipoAcervoId.Should().Be(idMaximo);
        }
        
        [Fact]
        public void DadoCaracteresEspeciais_QuandoAtribuirTexto_EntaoCaracteresPreservados()
        {
            var dto = new AcervoDocumentalDTO();
            var textosEspeciais = "Documento com ç, é, ñ, @, #, $, %, &, etc.";

            dto.Titulo = textosEspeciais;
            dto.Descricao = textosEspeciais;

            dto.Titulo.Should().Contain("ç");
            dto.Descricao.Should().Contain("é");
        }

        [Fact]
        public void DadoValoresNulosAtribuidos_QuandoAlterarNovaVez_EntaoNovosValoresReplazamAnigos()
        {
            var dto = CriarAcervoDocumentalDTOCompleto();

            dto.Titulo = "Novo Título";
            dto.Descricao = null!;
            dto.Volume = "Vol. 2";

            dto.Titulo.Should().Be("Novo Título");
            dto.Descricao.Should().BeNull();
            dto.Volume.Should().Be("Vol. 2");
        }

        [Fact]
        public void DadoDoisDTOsComMesmosValores_QuandoCompararPropriedades_EntaoPropriedadesIguais()
        {
            var dto1 = CriarAcervoDocumentalDTOCompleto();
            var dto2 = CriarAcervoDocumentalDTOCompleto();

            dto1.Id.Should().Be(dto2.Id);
            dto1.Titulo.Should().Be(dto2.Titulo);
            dto1.Codigo.Should().Be(dto2.Codigo);
        }

        [Fact]
        public void DadoIdiomaId_QuandoAtribuirVariosValores_EntaoValoresDistintosArmazenados()
        {
            var dto1 = new AcervoDocumentalDTO { IdiomaId = 1 };
            var dto2 = new AcervoDocumentalDTO { IdiomaId = 2 };
            var dto3 = new AcervoDocumentalDTO { IdiomaId = 3 };

            dto1.IdiomaId.Should().Be(1);
            dto2.IdiomaId.Should().Be(2);
            dto3.IdiomaId.Should().Be(3);
        }

        [Fact]
        public void DadoMaterialId_QuandoAtribuirVariosValores_EntaoValoresDistintosArmazenados()
        {
            var dto1 = new AcervoDocumentalDTO { MaterialId = 100 };
            var dto2 = new AcervoDocumentalDTO { MaterialId = 200 };

            dto1.MaterialId.Should().Be(100);
            dto2.MaterialId.Should().Be(200);
        }

        [Fact]
        public void DadoConservacaoId_QuandoAtribuirVariosValores_EntaoValoresDistintosArmazenados()
        {
            var dto1 = new AcervoDocumentalDTO { ConservacaoId = 1 };
            var dto2 = new AcervoDocumentalDTO { ConservacaoId = 2 };
            var dto3 = new AcervoDocumentalDTO { ConservacaoId = 3 };

            dto1.ConservacaoId.Should().Be(1);
            dto2.ConservacaoId.Should().Be(2);
            dto3.ConservacaoId.Should().Be(3);
        }

        [Fact]
        public void DadoVolumeComFormatos_QuandoAtribuirVarios_EntaoFormatosPreservados()
        {
            var dto1 = new AcervoDocumentalDTO { Volume = "Vol. 1-2" };
            var dto2 = new AcervoDocumentalDTO { Volume = "Tomo III" };
            var dto3 = new AcervoDocumentalDTO { Volume = "Volume Especial" };

            dto1.Volume.Should().Be("Vol. 1-2");
            dto2.Volume.Should().Be("Tomo III");
            dto3.Volume.Should().Be("Volume Especial");
        }

        [Fact]
        public void DadoTipoAnexoVariados_QuandoAtribuir_EntaoTiposPreservados()
        {
            var tiposAnexo = new[] { "PDF", "DOCX", "TXT", "RTF", "PNG", "JPG" };
            
            foreach (var tipo in tiposAnexo)
            {
                var dto = new AcervoDocumentalDTO { TipoAnexo = tipo };
                dto.TipoAnexo.Should().Be(tipo);
            }
        }
    }
}
