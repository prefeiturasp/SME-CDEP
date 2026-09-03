using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoDocumentalCadastroDtoTeste
    {
        private static AcervoDocumentalCadastroDTO CriarAcervoDocumentalCadastroDTO()
        {
            return new AcervoDocumentalCadastroDTO
            {
                Titulo = "Título do Acervo Documental",
                Ano = "2024",
                MaterialId = 1,
                IdiomaId = 2,
                NumeroPagina = 150,
                Volume = "Vol. 1",
                TipoAnexo = "PDF",
                Largura = "21cm",
                Altura = "29.7cm",
                TamanhoArquivo = "2.5MB",
                Localizacao = "Sala 101 - Prateleira A",
                CopiaDigital = true,
                ConservacaoId = 3,
                Arquivos = new long[] { 10, 20, 30 },
                AcessoDocumentosIds = new long[] { 100, 200 },
                CapaDocumento = "https://exemplo.com/capa.jpg"
            };
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoInstanciar_EntaoMaterialIdEhNulavel()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.MaterialId.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoInstanciar_EntaoIdiomaIdEhRequerido()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.IdiomaId.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoInstanciar_EntaoNumeroPaginaEhRequerido()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.NumeroPagina.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoInstanciar_EntaoVolumeEhNulavel()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.Volume.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoInstanciar_EntaoTipoAnexoEhNulavel()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.TipoAnexo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoInstanciar_EntaoLarguraEhNulavel()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.Largura.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoInstanciar_EntaoAlturaEhNulavel()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.Altura.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoInstanciar_EntaoTamanhoArquivoEhNulavel()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.TamanhoArquivo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoInstanciar_EntaoLocalizacaoEhNulavel()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.Localizacao.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoInstanciar_EntaoCopiaDigitalEhNulavel()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.CopiaDigital.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoInstanciar_EntaoConservacaoIdEhNulavel()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.ConservacaoId.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoInstanciar_EntaoArquivosEhNulavel()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.Arquivos.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoInstanciar_EntaoAcessoDocumentosIdsEhNulavel()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.AcessoDocumentosIds.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoInstanciar_EntaoCapaDocumentoEhNulavel()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.CapaDocumento.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoDefinirMaterialId_EntaoMaterialIdEhAtribuido()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const long materialIdEsperado = 5;

            dto.MaterialId = materialIdEsperado;

            dto.MaterialId.Should().Be(materialIdEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoDefinirIdiomaId_EntaoIdiomaIdEhAtribuido()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const long idiomaIdEsperado = 10;

            dto.IdiomaId = idiomaIdEsperado;

            dto.IdiomaId.Should().Be(idiomaIdEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoDefinirNumeroPagina_EntaoNumeroPaginaEhAtribuido()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const int numeroPaginaEsperado = 250;

            dto.NumeroPagina = numeroPaginaEsperado;

            dto.NumeroPagina.Should().Be(numeroPaginaEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoDefinirVolume_EntaoVolumeEhAtribuido()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string volumeEsperado = "Vol. 2";

            dto.Volume = volumeEsperado;

            dto.Volume.Should().Be(volumeEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoDefinirTipoAnexo_EntaoTipoAnexoEhAtribuido()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string tipoAnexoEsperado = "DOCX";

            dto.TipoAnexo = tipoAnexoEsperado;

            dto.TipoAnexo.Should().Be(tipoAnexoEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoDefinirLargura_EntaoLarguraEhAtribuida()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string larguraEsperada = "30cm";

            dto.Largura = larguraEsperada;

            dto.Largura.Should().Be(larguraEsperada);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoDefinirAltura_EntaoAlturaEhAtribuida()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string alturaEsperada = "40cm";

            dto.Altura = alturaEsperada;

            dto.Altura.Should().Be(alturaEsperada);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoDefinirTamanhoArquivo_EntaoTamanhoArquivoEhAtribuido()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string tamanhoArquivoEsperado = "5.0GB";

            dto.TamanhoArquivo = tamanhoArquivoEsperado;

            dto.TamanhoArquivo.Should().Be(tamanhoArquivoEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoDefinirLocalizacao_EntaoLocalizacaoEhAtribuida()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string localizacaoEsperada = "Sala 201 - Arquivo B";

            dto.Localizacao = localizacaoEsperada;

            dto.Localizacao.Should().Be(localizacaoEsperada);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoDefinirCopiaDigital_EntaoCopiaDigitalEhAtribuida()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.CopiaDigital = true;

            dto.CopiaDigital.Should().BeTrue();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoDefinirCopiaDigitalFalso_EntaoCopiaDigitalEhFalso()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.CopiaDigital = false;

            dto.CopiaDigital.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoDefinirConservacaoId_EntaoConservacaoIdEhAtribuido()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const long conservacaoIdEsperado = 7;

            dto.ConservacaoId = conservacaoIdEsperado;

            dto.ConservacaoId.Should().Be(conservacaoIdEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoDefinirArquivos_EntaoArquivosEhAtribuido()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            var arquivosEsperados = new long[] { 50, 60, 70, 80 };

            dto.Arquivos = arquivosEsperados;

            dto.Arquivos.Should().BeEquivalentTo(arquivosEsperados);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoDefinirAcessoDocumentosIds_EntaoAcessoDocumentosIdsEhAtribuido()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            var acessoEsperado = new long[] { 500, 600, 700 };

            dto.AcessoDocumentosIds = acessoEsperado;

            dto.AcessoDocumentosIds.Should().BeEquivalentTo(acessoEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoDefinirCapaDocumento_EntaoCapaDocumentoEhAtribuido()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string capaEsperada = "https://exemplo.com/capa-documento.jpg";

            dto.CapaDocumento = capaEsperada;

            dto.CapaDocumento.Should().Be(capaEsperada);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoUtilizarTodosOsCampos_EntaoTodosCamposSaoAcessiveis()
        {
            var dto = CriarAcervoDocumentalCadastroDTO();

            dto.Titulo.Should().Be("Título do Acervo Documental");
            dto.Ano.Should().Be("2024");
            dto.MaterialId.Should().Be(1);
            dto.IdiomaId.Should().Be(2);
            dto.NumeroPagina.Should().Be(150);
            dto.Volume.Should().Be("Vol. 1");
            dto.TipoAnexo.Should().Be("PDF");
            dto.Largura.Should().Be("21cm");
            dto.Altura.Should().Be("29.7cm");
            dto.TamanhoArquivo.Should().Be("2.5MB");
            dto.Localizacao.Should().Be("Sala 101 - Prateleira A");
            dto.CopiaDigital.Should().BeTrue();
            dto.ConservacaoId.Should().Be(3);
            dto.Arquivos.Should().BeEquivalentTo(new long[] { 10, 20, 30 });
            dto.AcessoDocumentosIds.Should().BeEquivalentTo(new long[] { 100, 200 });
            dto.CapaDocumento.Should().Be("https://exemplo.com/capa.jpg");
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoVolumeTemComprimentoMaximo_EntaoVolumeEhAtribuido()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            var volumeComprido = new string('a', 15);

            dto.Volume = volumeComprido;

            dto.Volume.Should().Be(volumeComprido);
            dto.Volume.Length.Should().Be(15);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoTipoAnexoTemComprimentoMaximo_EntaoTipoAnexoEhAtribuido()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            var tipoAnexoComprido = new string('b', 50);

            dto.TipoAnexo = tipoAnexoComprido;

            dto.TipoAnexo.Should().Be(tipoAnexoComprido);
            dto.TipoAnexo.Length.Should().Be(50);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoTamanhoArquivoTemComprimentoMaximo_EntaoTamanhoArquivoEhAtribuido()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            var tamanhoComprido = new string('c', 15);

            dto.TamanhoArquivo = tamanhoComprido;

            dto.TamanhoArquivo.Should().Be(tamanhoComprido);
            dto.TamanhoArquivo.Length.Should().Be(15);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoLocalizacaoTemComprimentoMaximo_EntaoLocalizacaoEhAtribuida()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            var localizacaoComprida = new string('d', 100);

            dto.Localizacao = localizacaoComprida;

            dto.Localizacao.Should().Be(localizacaoComprida);
            dto.Localizacao.Length.Should().Be(100);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoVolumeVazio_EntaoVolumeEhVazio()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string volumeVazio = "";

            dto.Volume = volumeVazio;

            dto.Volume.Should().Be(volumeVazio);
            dto.Volume.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoTipoAnexoVazio_EntaoTipoAnexoEhVazio()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string tipoAnexoVazio = "";

            dto.TipoAnexo = tipoAnexoVazio;

            dto.TipoAnexo.Should().Be(tipoAnexoVazio);
            dto.TipoAnexo.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoTamanhoArquivoVazio_EntaoTamanhoArquivoEhVazio()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string tamanhoVazio = "";

            dto.TamanhoArquivo = tamanhoVazio;

            dto.TamanhoArquivo.Should().Be(tamanhoVazio);
            dto.TamanhoArquivo.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoLocalizacaoVazia_EntaoLocalizacaoEhVazia()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string localizacaoVazia = "";

            dto.Localizacao = localizacaoVazia;

            dto.Localizacao.Should().Be(localizacaoVazia);
            dto.Localizacao.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoVolumeNulo_EntaoVolumeEhNulo()
        {
            var dto = new AcervoDocumentalCadastroDTO { Volume = null };

            dto.Volume.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoTipoAnexoNulo_EntaoTipoAnexoEhNulo()
        {
            var dto = new AcervoDocumentalCadastroDTO { TipoAnexo = null };

            dto.TipoAnexo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoTamanhoArquivoNulo_EntaoTamanhoArquivoEhNulo()
        {
            var dto = new AcervoDocumentalCadastroDTO { TamanhoArquivo = null };

            dto.TamanhoArquivo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoLocalizacaoNula_EntaoLocalizacaoEhNula()
        {
            var dto = new AcervoDocumentalCadastroDTO { Localizacao = null };

            dto.Localizacao.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoCapaDocumentoNula_EntaoCapaDocumentoEhNula()
        {
            var dto = new AcervoDocumentalCadastroDTO { CapaDocumento = null };

            dto.CapaDocumento.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoIdiomaIdComValorMaximo_EntaoIdiomaIdEhArmazenado()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const long idiomaIdMaximo = long.MaxValue;

            dto.IdiomaId = idiomaIdMaximo;

            dto.IdiomaId.Should().Be(idiomaIdMaximo);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoNumeroPaginaComValorMaximo_EntaoNumeroPaginaEhArmazenado()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const int numeroPaginaMaximo = int.MaxValue;

            dto.NumeroPagina = numeroPaginaMaximo;

            dto.NumeroPagina.Should().Be(numeroPaginaMaximo);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoNumeroPaginaComValorNegativo_EntaoNumeroPaginaEhArmazenado()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const int numeroPaginaNegativo = -1;

            dto.NumeroPagina = numeroPaginaNegativo;

            dto.NumeroPagina.Should().Be(numeroPaginaNegativo);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoArquivosVazio_EntaoArquivosEhVazio()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            var arquivosVazios = Array.Empty<long>();

            dto.Arquivos = arquivosVazios;

            dto.Arquivos.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoAcessoDocumentosIdsVazio_EntaoAcessoDocumentosIdsEhVazio()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            var acessoVazio = Array.Empty<long>();

            dto.AcessoDocumentosIds = acessoVazio;

            dto.AcessoDocumentosIds.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoMultiplosArquivos_EntaoArquivosTemMultiplosValores()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            var arquivos = new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            dto.Arquivos = arquivos;

            dto.Arquivos.Should().HaveCount(10);
            dto.Arquivos.Should().BeEquivalentTo(arquivos);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoMultiplosAcessoDocumentos_EntaoAcessoDocumentosIdsTemMultiplosValores()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            var acesso = new long[] { 1000, 2000, 3000, 4000 };

            dto.AcessoDocumentosIds = acesso;

            dto.AcessoDocumentosIds.Should().HaveCount(4);
            dto.AcessoDocumentosIds.Should().BeEquivalentTo(acesso);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoModificarPropriedades_EntaoPropriedadesSaoAtualizadas()
        {
            var dto = CriarAcervoDocumentalCadastroDTO();
            const long novoIdiomaId = 99;

            dto.IdiomaId = novoIdiomaId;

            dto.IdiomaId.Should().Be(novoIdiomaId);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoDefinirTodasAsPropriedades_EntaoTodosOsValoresSaoPreservados()
        {
            var dto = CriarAcervoDocumentalCadastroDTO();

            dto.Titulo.Should().NotBeNull();
            dto.Ano.Should().NotBeNull();
            dto.MaterialId.Should().NotBeNull();
            dto.IdiomaId.Should().BeGreaterThan(0);
            dto.NumeroPagina.Should().BeGreaterThan(0);
            dto.Volume.Should().NotBeNull();
            dto.TipoAnexo.Should().NotBeNull();
            dto.Largura.Should().NotBeNull();
            dto.Altura.Should().NotBeNull();
            dto.TamanhoArquivo.Should().NotBeNull();
            dto.Localizacao.Should().NotBeNull();
            dto.CopiaDigital.Should().BeTrue();
            dto.ConservacaoId.Should().NotBeNull();
            dto.Arquivos.Should().NotBeEmpty();
            dto.AcessoDocumentosIds.Should().NotBeEmpty();
            dto.CapaDocumento.Should().NotBeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoLarguraComValoresEspeciais_EntaoLarguraEhPreservada()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string larguraComEspeciais = "21.5cm x 1/2\"";

            dto.Largura = larguraComEspeciais;

            dto.Largura.Should().Be(larguraComEspeciais);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoLocalizacaoComCaracteresEspeciais_EntaoLocalizacaoEhPreservada()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string localizacaoComEspeciais = "Sala 101-A/Prateleira (B) - Caixa #3";

            dto.Localizacao = localizacaoComEspeciais;

            dto.Localizacao.Should().Be(localizacaoComEspeciais);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoCapaDocumentoComURLCompleta_EntaoCapaDocumentoEhPreservado()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string urlCompleta = "https://cdn.storage.exemplo.com:8080/acervo/2024/capa.jpg?v=2&format=high";

            dto.CapaDocumento = urlCompleta;

            dto.CapaDocumento.Should().Be(urlCompleta);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoTamanhoArquivoComValoresGrandes_EntaoTamanhoEhPreservado()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string tamanhoGrande = "2.5 TB";

            dto.TamanhoArquivo = tamanhoGrande;

            dto.TamanhoArquivo.Should().Be(tamanhoGrande);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoAtribuirNullAoMaterialId_EntaoMaterialIdEhNull()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            dto.MaterialId = 10;

            dto.MaterialId = null;

            dto.MaterialId.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoAtribuirNullAoConservacaoId_EntaoConservacaoIdEhNull()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            dto.ConservacaoId = 5;

            dto.ConservacaoId = null;

            dto.ConservacaoId.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoAtribuirNullAoArquivos_EntaoArquivosEhNull()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            dto.Arquivos = new long[] { 1, 2, 3 };

            dto.Arquivos = null;

            dto.Arquivos.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoAtribuirNullAoCapaDocumento_EntaoCapaDocumentoEhNull()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            dto.CapaDocumento = "https://exemplo.com/capa.jpg";

            dto.CapaDocumento = null;

            dto.CapaDocumento.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoAcessarPropriedadesComConstrutorVazio_EntaoPropriedadesNaoLancamExcecao()
        {
            var exception = Record.Exception(() =>
            {
                var dto = new AcervoDocumentalCadastroDTO();
                _ = dto.MaterialId;
                _ = dto.IdiomaId;
                _ = dto.NumeroPagina;
                _ = dto.Volume;
                _ = dto.TipoAnexo;
                _ = dto.Largura;
                _ = dto.Altura;
                _ = dto.TamanhoArquivo;
                _ = dto.Localizacao;
                _ = dto.CopiaDigital;
                _ = dto.ConservacaoId;
                _ = dto.Arquivos;
                _ = dto.AcessoDocumentosIds;
                _ = dto.CapaDocumento;
            });

            exception.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoModificarDiversasVezesPropriedades_EntaoUltimoValorEhPreservado()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.IdiomaId = 1;
            dto.IdiomaId = 2;
            dto.IdiomaId = 3;

            dto.IdiomaId.Should().Be(3);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoCompararDuasInstancias_EntaoSaoInstanciasDistintas()
        {
            var dto1 = CriarAcervoDocumentalCadastroDTO();
            var dto2 = CriarAcervoDocumentalCadastroDTO();

            dto1.Should().NotBeSameAs(dto2);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoCopiaDigitalMuda_EntaoValorEhAlterado()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            dto.CopiaDigital = true;

            dto.CopiaDigital = false;

            dto.CopiaDigital.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoMaterialIdMuda_EntaoNovoValorEhArmazenado()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            dto.MaterialId = 100;

            dto.MaterialId = 200;

            dto.MaterialId.Should().Be(200);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoNumeroPaginaMuda_EntaoNovoValorEhArmazenado()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            dto.NumeroPagina = 100;

            dto.NumeroPagina = 500;

            dto.NumeroPagina.Should().Be(500);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoConservacaoIdMuda_EntaoNovoValorEhArmazenado()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            dto.ConservacaoId = 1;

            dto.ConservacaoId = 9;

            dto.ConservacaoId.Should().Be(9);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoInicializarComValoresDefault_EntaoPropriedadesTemValoresCorretos()
        {
            var dto = new AcervoDocumentalCadastroDTO
            {
                Titulo = "Título",
                Ano = "2024",
                IdiomaId = 1,
                NumeroPagina = 50
            };

            dto.Titulo.Should().Be("Título");
            dto.Ano.Should().Be("2024");
            dto.IdiomaId.Should().Be(1);
            dto.NumeroPagina.Should().Be(50);
            dto.MaterialId.Should().BeNull();
            dto.Volume.Should().BeNull();
            dto.TipoAnexo.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.TamanhoArquivo.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.ConservacaoId.Should().BeNull();
            dto.Arquivos.Should().BeNull();
            dto.AcessoDocumentosIds.Should().BeNull();
            dto.CapaDocumento.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoTodasAsPropriedadesSaoDefinidas_EntaoTodosCamposSaoAcessados()
        {
            var dto = new AcervoDocumentalCadastroDTO
            {
                Titulo = "Documento Completo",
                Ano = "2024",
                MaterialId = 999,
                IdiomaId = 5,
                NumeroPagina = 1000,
                Volume = "Vol. X",
                TipoAnexo = "PDF",
                Largura = "25cm",
                Altura = "35cm",
                TamanhoArquivo = "10MB",
                Localizacao = "Arquivo Principal",
                CopiaDigital = true,
                ConservacaoId = 2,
                Arquivos = new long[] { 111, 222 },
                AcessoDocumentosIds = new long[] { 333, 444 },
                CapaDocumento = "https://exemplo.com/capa-full.jpg"
            };

            dto.Titulo.Should().Be("Documento Completo");
            dto.Ano.Should().Be("2024");
            dto.MaterialId.Should().Be(999);
            dto.IdiomaId.Should().Be(5);
            dto.NumeroPagina.Should().Be(1000);
            dto.Volume.Should().Be("Vol. X");
            dto.TipoAnexo.Should().Be("PDF");
            dto.Largura.Should().Be("25cm");
            dto.Altura.Should().Be("35cm");
            dto.TamanhoArquivo.Should().Be("10MB");
            dto.Localizacao.Should().Be("Arquivo Principal");
            dto.CopiaDigital.Should().BeTrue();
            dto.ConservacaoId.Should().Be(2);
            dto.Arquivos.Should().BeEquivalentTo(new long[] { 111, 222 });
            dto.AcessoDocumentosIds.Should().BeEquivalentTo(new long[] { 333, 444 });
            dto.CapaDocumento.Should().Be("https://exemplo.com/capa-full.jpg");
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoLarguraVazia_EntaoLarguraEhVazia()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string larguraVazia = "";

            dto.Largura = larguraVazia;

            dto.Largura.Should().Be(larguraVazia);
            dto.Largura.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoAlturaVazia_EntaoAlturaEhVazia()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string alturaVazia = "";

            dto.Altura = alturaVazia;

            dto.Altura.Should().Be(alturaVazia);
            dto.Altura.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoCapaDocumentoVazia_EntaoCapaDocumentoEhVazia()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string capaVazia = "";

            dto.CapaDocumento = capaVazia;

            dto.CapaDocumento.Should().Be(capaVazia);
            dto.CapaDocumento.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoArquivosComValoresMaximos_EntaoArquivosEhArmazenado()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            var arquivosComValoresAltos = new long[] { long.MaxValue, long.MinValue, 0 };

            dto.Arquivos = arquivosComValoresAltos;

            dto.Arquivos.Should().BeEquivalentTo(arquivosComValoresAltos);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoAcessoDocumentosComValoresMaximos_EntaoAcessoEhArmazenado()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            var acessoComValoresAltos = new long[] { long.MaxValue, long.MinValue, 0 };

            dto.AcessoDocumentosIds = acessoComValoresAltos;

            dto.AcessoDocumentosIds.Should().BeEquivalentTo(acessoComValoresAltos);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoIdiomaIdZero_EntaoIdiomaIdEhZero()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.IdiomaId = 0;

            dto.IdiomaId.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoNumeroPaginaZero_EntaoNumeroPaginaEhZero()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.NumeroPagina = 0;

            dto.NumeroPagina.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoMaterialIdZero_EntaoMaterialIdEhZero()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.MaterialId = 0;

            dto.MaterialId.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoCopiaDigitalNull_EntaoCopiaDigitalEhNull()
        {
            var dto = new AcervoDocumentalCadastroDTO();

            dto.CopiaDigital = null;

            dto.CopiaDigital.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoHerancaDeAcervoCadastroDTO_EntaoPropriedadesHerdadasSaoAcessadas()
        {
            var dto = new AcervoDocumentalCadastroDTO
            {
                Titulo = "Acervo com Herança",
                Ano = "2024",
                Descricao = "Descrição herdada",
                Codigo = "COD-HERANCA",
                CodigoNovo = "COD-NOVO-HERANCA",
                SubTitulo = "Subtítulo Herdado",
                DataAcervo = "2024-01-01"
            };

            dto.Titulo.Should().Be("Acervo com Herança");
            dto.Ano.Should().Be("2024");
            dto.Descricao.Should().Be("Descrição herdada");
            dto.Codigo.Should().Be("COD-HERANCA");
            dto.CodigoNovo.Should().Be("COD-NOVO-HERANCA");
            dto.SubTitulo.Should().Be("Subtítulo Herdado");
            dto.DataAcervo.Should().Be("2024-01-01");
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoAtribuirNullAoAcessoDocumentosIds_EntaoAcessoDocumentosIdsEhNull()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            dto.AcessoDocumentosIds = new long[] { 1, 2, 3 };

            dto.AcessoDocumentosIds = null!;

            dto.AcessoDocumentosIds.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoVolumeComNumeros_EntaoVolumeEhArmazenado()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string volumeComNumeros = "Vol. 123";

            dto.Volume = volumeComNumeros;

            dto.Volume.Should().Be(volumeComNumeros);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoTipoAnexoComMultiplasLetra_EntaoTipoAnexoEhArmazenado()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string tipoAnexoMultiplo = "PDF-DOCX-TXT";

            dto.TipoAnexo = tipoAnexoMultiplo;

            dto.TipoAnexo.Should().Be(tipoAnexoMultiplo);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoLarguraComCaractereNumerico_EntaoLarguraEhArmazenada()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string larguraComNumero = "21.5";

            dto.Largura = larguraComNumero;

            dto.Largura.Should().Be(larguraComNumero);
        }

        [Fact]
        public void DadoAcervoDocumentalCadastro_QuandoAlturaComCaractereNumerico_EntaoAlturaEhArmazenada()
        {
            var dto = new AcervoDocumentalCadastroDTO();
            const string alturaComNumero = "29.7";

            dto.Altura = alturaComNumero;

            dto.Altura.Should().Be(alturaComNumero);
        }
    }
}
