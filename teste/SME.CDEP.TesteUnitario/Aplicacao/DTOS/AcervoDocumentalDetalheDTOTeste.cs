using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoDocumentalDetalheDTOTeste
    {
        private AcervoDocumentalDetalheDTO CriarAcervoDocumentalDetalheDTO()
        {
            return new AcervoDocumentalDetalheDTO
            {
                Titulo = "Título do Acervo Documental",
                Codigo = "ACE-DOC-001",
                Ano = "2024",
                AcervoId = 1,
                EnderecoImagemPadrao = "https://exemplo.com/imagem-padrao.jpg",
                SituacaoDisponibilidade = "Disponível",
                EstaDisponivel = true,
                TemControleDisponibilidade = true,
                TipoAcervoId = 1,
                Descricao = "Descrição detalhada do documento",
                CreditosAutores = "Autor 1, Autor 2",
                CodigoNovo = "ACE-DOC-001-NOVO",
                Material = "Papel",
                Idioma = "Português",
                NumeroPagina = 250,
                Volume = "Vol. 1",
                TipoAnexo = "PDF",
                Dimensoes = "21cm x 29.7cm",
                TamanhoArquivo = "5MB",
                Localizacao = "Sala 101 - Prateleira A",
                CopiaDigital = "Sim",
                Conservacao = "Bom",
                AcessosDocumentos = "Público",
                Imagens = new[]
                {
                    new ImagemDTO { Original = "https://exemplo.com/original-1.jpg", Thumbnail = "https://exemplo.com/thumb-1.jpg" },
                    new ImagemDTO { Original = "https://exemplo.com/original-2.jpg", Thumbnail = "https://exemplo.com/thumb-2.jpg" }
                }
            };
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoInstanciar_EntaoDescricaoEhNulavel()
        {
            var dto = new AcervoDocumentalDetalheDTO();

            dto.Descricao.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoInstanciar_EntaoCreditosAutoresEhNulavel()
        {
            var dto = new AcervoDocumentalDetalheDTO();

            dto.CreditosAutores.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoInstanciar_EntaoCodigoNovoEhNulavel()
        {
            var dto = new AcervoDocumentalDetalheDTO();

            dto.CodigoNovo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoInstanciar_EntaoMaterialEhNulavel()
        {
            var dto = new AcervoDocumentalDetalheDTO();

            dto.Material.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoInstanciar_EntaoIdiomaEhNulavel()
        {
            var dto = new AcervoDocumentalDetalheDTO();

            dto.Idioma.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoInstanciar_EntaoNumeroPaginaEhZero()
        {
            var dto = new AcervoDocumentalDetalheDTO();

            dto.NumeroPagina.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoInstanciar_EntaoVolumeEhNulavel()
        {
            var dto = new AcervoDocumentalDetalheDTO();

            dto.Volume.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoInstanciar_EntaoTipoAnexoEhNulavel()
        {
            var dto = new AcervoDocumentalDetalheDTO();

            dto.TipoAnexo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoInstanciar_EntaoDimensoesEhNulavel()
        {
            var dto = new AcervoDocumentalDetalheDTO();

            dto.Dimensoes.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoInstanciar_EntaoTamanhoArquivoEhNulavel()
        {
            var dto = new AcervoDocumentalDetalheDTO();

            dto.TamanhoArquivo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoInstanciar_EntaoLocalizacaoEhNulavel()
        {
            var dto = new AcervoDocumentalDetalheDTO();

            dto.Localizacao.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoInstanciar_EntaoCopiaDigitalEhNulavel()
        {
            var dto = new AcervoDocumentalDetalheDTO();

            dto.CopiaDigital.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoInstanciar_EntaoConservacaoEhNulavel()
        {
            var dto = new AcervoDocumentalDetalheDTO();

            dto.Conservacao.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoInstanciar_EntaoAcessosDocumentosEhNulavel()
        {
            var dto = new AcervoDocumentalDetalheDTO();

            dto.AcessosDocumentos.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoInstanciar_EntaoImagensEhNulavel()
        {
            var dto = new AcervoDocumentalDetalheDTO();

            dto.Imagens.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDefinirDescricao_EntaoDescricaoEhAtribuida()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string descricaoEsperada = "Descrição do documento";

            dto.Descricao = descricaoEsperada;

            dto.Descricao.Should().Be(descricaoEsperada);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDefinirCreditosAutores_EntaoCreditosAutoresEhAtribuido()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string creditosEsperados = "Autor 1, Autor 2, Autor 3";

            dto.CreditosAutores = creditosEsperados;

            dto.CreditosAutores.Should().Be(creditosEsperados);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDefinirCodigoNovo_EntaoCodigoNovoEhAtribuido()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string codigoNovoEsperado = "ACE-DOC-NEW-001";

            dto.CodigoNovo = codigoNovoEsperado;

            dto.CodigoNovo.Should().Be(codigoNovoEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDefinirMaterial_EntaoMaterialEhAtribuido()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string materialEsperado = "Papel Couché";

            dto.Material = materialEsperado;

            dto.Material.Should().Be(materialEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDefinirIdioma_EntaoIdiomaEhAtribuido()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string idiomaEsperado = "Português Brasileiro";

            dto.Idioma = idiomaEsperado;

            dto.Idioma.Should().Be(idiomaEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDefinirNumeroPagina_EntaoNumeroPaginaEhAtribuido()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const int numeroPaginaEsperado = 500;

            dto.NumeroPagina = numeroPaginaEsperado;

            dto.NumeroPagina.Should().Be(numeroPaginaEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDefinirVolume_EntaoVolumeEhAtribuido()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string volumeEsperado = "Vol. 5";

            dto.Volume = volumeEsperado;

            dto.Volume.Should().Be(volumeEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDefinirTipoAnexo_EntaoTipoAnexoEhAtribuido()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string tipoAnexoEsperado = "DOCX";

            dto.TipoAnexo = tipoAnexoEsperado;

            dto.TipoAnexo.Should().Be(tipoAnexoEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDefinirDimensoes_EntaoDimensoesEhAtribuida()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string dimensoesEsperada = "25cm x 35cm x 2cm";

            dto.Dimensoes = dimensoesEsperada;

            dto.Dimensoes.Should().Be(dimensoesEsperada);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDefinirTamanhoArquivo_EntaoTamanhoArquivoEhAtribuido()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string tamanhoEsperado = "15MB";

            dto.TamanhoArquivo = tamanhoEsperado;

            dto.TamanhoArquivo.Should().Be(tamanhoEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDefinirLocalizacao_EntaoLocalizacaoEhAtribuida()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string localizacaoEsperada = "Sala 201 - Arquivo Geral";

            dto.Localizacao = localizacaoEsperada;

            dto.Localizacao.Should().Be(localizacaoEsperada);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDefinirCopiaDigital_EntaoCopiaDigitalEhAtribuida()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string copiaDigitalEsperada = "Sim";

            dto.CopiaDigital = copiaDigitalEsperada;

            dto.CopiaDigital.Should().Be(copiaDigitalEsperada);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDefinirConservacao_EntaoConservacaoEhAtribuida()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string conservacaoEsperada = "Excelente";

            dto.Conservacao = conservacaoEsperada;

            dto.Conservacao.Should().Be(conservacaoEsperada);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDefinirAcessosDocumentos_EntaoAcessosDocumentosEhAtribuido()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string acessosEsperados = "Público, Restrito";

            dto.AcessosDocumentos = acessosEsperados;

            dto.AcessosDocumentos.Should().Be(acessosEsperados);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDefinirImagens_EntaoImagensEhAtribuida()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            var imagensEsperadas = new[]
            {
                new ImagemDTO { Original = "https://exemplo.com/orig.jpg", Thumbnail = "https://exemplo.com/thumb.jpg" }
            };

            dto.Imagens = imagensEsperadas;

            dto.Imagens.Should().BeEquivalentTo(imagensEsperadas);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoUtilizarTodosOsCampos_EntaoTodosCamposSaoAcessiveis()
        {
            var dto = CriarAcervoDocumentalDetalheDTO();

            dto.Titulo.Should().Be("Título do Acervo Documental");
            dto.Codigo.Should().Be("ACE-DOC-001");
            dto.Ano.Should().Be("2024");
            dto.AcervoId.Should().Be(1);
            dto.EnderecoImagemPadrao.Should().Be("https://exemplo.com/imagem-padrao.jpg");
            dto.SituacaoDisponibilidade.Should().Be("Disponível");
            dto.EstaDisponivel.Should().BeTrue();
            dto.TemControleDisponibilidade.Should().BeTrue();
            dto.TipoAcervoId.Should().Be(1);
            dto.Descricao.Should().Be("Descrição detalhada do documento");
            dto.CreditosAutores.Should().Be("Autor 1, Autor 2");
            dto.CodigoNovo.Should().Be("ACE-DOC-001-NOVO");
            dto.Material.Should().Be("Papel");
            dto.Idioma.Should().Be("Português");
            dto.NumeroPagina.Should().Be(250);
            dto.Volume.Should().Be("Vol. 1");
            dto.TipoAnexo.Should().Be("PDF");
            dto.Dimensoes.Should().Be("21cm x 29.7cm");
            dto.TamanhoArquivo.Should().Be("5MB");
            dto.Localizacao.Should().Be("Sala 101 - Prateleira A");
            dto.CopiaDigital.Should().Be("Sim");
            dto.Conservacao.Should().Be("Bom");
            dto.AcessosDocumentos.Should().Be("Público");
            dto.Imagens.Should().HaveCount(2);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDescricaoTemComprimentoMaximo_EntaoDescricaoEhAtribuida()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            var descricaoComprida = new string('a', 1000);

            dto.Descricao = descricaoComprida;

            dto.Descricao.Should().Be(descricaoComprida);
            dto.Descricao.Length.Should().Be(1000);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoCreditosAutoresTemComprimentoMaximo_EntaoCreditosAutoresEhAtribuido()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            var creditosCompridos = new string('b', 500);

            dto.CreditosAutores = creditosCompridos;

            dto.CreditosAutores.Should().Be(creditosCompridos);
            dto.CreditosAutores.Length.Should().Be(500);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDescricaoVazia_EntaoDescricaoEhVazia()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string descricaoVazia = "";

            dto.Descricao = descricaoVazia;

            dto.Descricao.Should().Be(descricaoVazia);
            dto.Descricao.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoCreditosAutoresVazios_EntaoCreditosAutoresEhVazio()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string creditosVazios = "";

            dto.CreditosAutores = creditosVazios;

            dto.CreditosAutores.Should().Be(creditosVazios);
            dto.CreditosAutores.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoNumeroPaginaComValorMaximo_EntaoNumeroPaginaEhArmazenado()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const int numeroPaginaMaximo = int.MaxValue;

            dto.NumeroPagina = numeroPaginaMaximo;

            dto.NumeroPagina.Should().Be(numeroPaginaMaximo);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoNumeroPaginaComValorMinimo_EntaoNumeroPaginaEhArmazenado()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const int numeroPaginaMinimo = int.MinValue;

            dto.NumeroPagina = numeroPaginaMinimo;

            dto.NumeroPagina.Should().Be(numeroPaginaMinimo);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoAtribuirNullAoDescricao_EntaoDescricaoEhNull()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            dto.Descricao = "Descrição";

            dto.Descricao = null;

            dto.Descricao.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoAtribuirNullAoCreditosAutores_EntaoCreditosAutoresEhNull()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            dto.CreditosAutores = "Autor";

            dto.CreditosAutores = null;

            dto.CreditosAutores.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoAtribuirNullAoCodigoNovo_EntaoCodigoNovoEhNull()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            dto.CodigoNovo = "COD-001";

            dto.CodigoNovo = null;

            dto.CodigoNovo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoAtribuirNullAoMaterial_EntaoMaterialEhNull()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            dto.Material = "Papel";

            dto.Material = null;

            dto.Material.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoAtribuirNullAoIdioma_EntaoIdiomaEhNull()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            dto.Idioma = "Português";

            dto.Idioma = null;

            dto.Idioma.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoAtribuirNullAoVolume_EntaoVolumeEhNull()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            dto.Volume = "Vol. 1";

            dto.Volume = null;

            dto.Volume.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoAtribuirNullAoTipoAnexo_EntaoTipoAnexoEhNull()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            dto.TipoAnexo = "PDF";

            dto.TipoAnexo = null;

            dto.TipoAnexo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoAtribuirNullAoDimensoes_EntaoDimensoesEhNull()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            dto.Dimensoes = "21cm x 29.7cm";

            dto.Dimensoes = null;

            dto.Dimensoes.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoAtribuirNullAoTamanhoArquivo_EntaoTamanhoArquivoEhNull()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            dto.TamanhoArquivo = "5MB";

            dto.TamanhoArquivo = null;

            dto.TamanhoArquivo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoAtribuirNullAoLocalizacao_EntaoLocalizacaoEhNull()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            dto.Localizacao = "Sala 101";

            dto.Localizacao = null;

            dto.Localizacao.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoAtribuirNullAoCopiaDigital_EntaoCopiaDigitalEhNull()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            dto.CopiaDigital = "Sim";

            dto.CopiaDigital = null;

            dto.CopiaDigital.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoAtribuirNullAoConservacao_EntaoConservacaoEhNull()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            dto.Conservacao = "Bom";

            dto.Conservacao = null;

            dto.Conservacao.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoAtribuirNullAoAcessosDocumentos_EntaoAcessosDocumentosEhNull()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            dto.AcessosDocumentos = "Público";

            dto.AcessosDocumentos = null;

            dto.AcessosDocumentos.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoAtribuirNullAoImagens_EntaoImagensEhNull()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            dto.Imagens = new[] { new ImagemDTO() };

            dto.Imagens = null;

            dto.Imagens.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDescricaoComCaracteresEspeciais_EntaoDescricaoEhPreservada()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string descricaoComEspeciais = "Descrição com ç, é, ñ e outros caracteres: @#$%";

            dto.Descricao = descricaoComEspeciais;

            dto.Descricao.Should().Be(descricaoComEspeciais);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoCreditosAutoresComMultiplosNomes_EntaoCreditosAutoresEhPreservado()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string creditosMultiplos = "Silva, João; Santos, Maria; Oliveira, Pedro";

            dto.CreditosAutores = creditosMultiplos;

            dto.CreditosAutores.Should().Be(creditosMultiplos);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoMaterialComValoresDistintos_EntaoMaterialEhPreservado()
        {
            var dto1 = new AcervoDocumentalDetalheDTO { Material = "Papel" };
            var dto2 = new AcervoDocumentalDetalheDTO { Material = "Tecido" };
            var dto3 = new AcervoDocumentalDetalheDTO { Material = "Vidro" };

            dto1.Material.Should().Be("Papel");
            dto2.Material.Should().Be("Tecido");
            dto3.Material.Should().Be("Vidro");
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoIdiomaComValoresDistintos_EntaoIdiomaEhPreservado()
        {
            var dto1 = new AcervoDocumentalDetalheDTO { Idioma = "Português" };
            var dto2 = new AcervoDocumentalDetalheDTO { Idioma = "Inglês" };
            var dto3 = new AcervoDocumentalDetalheDTO { Idioma = "Francês" };

            dto1.Idioma.Should().Be("Português");
            dto2.Idioma.Should().Be("Inglês");
            dto3.Idioma.Should().Be("Francês");
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoVolumeComFormatos_EntaoVolumeEhPreservado()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string volumeFormatado = "Vol. 1-2";

            dto.Volume = volumeFormatado;

            dto.Volume.Should().Be(volumeFormatado);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoTipoAnexoComMultiplosFormatos_EntaoTipoAnexoEhPreservado()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string tipoAnexoMultiplo = "PDF, DOCX, TXT";

            dto.TipoAnexo = tipoAnexoMultiplo;

            dto.TipoAnexo.Should().Be(tipoAnexoMultiplo);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDimensoesComMultiplasMedidas_EntaoDimensoesEhPreservada()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string dimensoesCompletas = "21 x 29.7 x 0.5 cm";

            dto.Dimensoes = dimensoesCompletas;

            dto.Dimensoes.Should().Be(dimensoesCompletas);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoTamanhoArquivoComValoresGrandes_EntaoTamanhoArquivoEhPreservado()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string tamanhoGrande = "2.5 GB";

            dto.TamanhoArquivo = tamanhoGrande;

            dto.TamanhoArquivo.Should().Be(tamanhoGrande);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoLocalizacaoComDiversasPartesHierarquicas_EntaoLocalizacaoEhPreservada()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string localizacaoCompleta = "Prédio A / Andar 2 / Sala 201 / Arquivo Geral / Prateleira C";

            dto.Localizacao = localizacaoCompleta;

            dto.Localizacao.Should().Be(localizacaoCompleta);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoConservacaoComValoresDistintos_EntaoConservacaoEhPreservada()
        {
            var dto1 = new AcervoDocumentalDetalheDTO { Conservacao = "Excelente" };
            var dto2 = new AcervoDocumentalDetalheDTO { Conservacao = "Bom" };
            var dto3 = new AcervoDocumentalDetalheDTO { Conservacao = "Precisa Restauro" };

            dto1.Conservacao.Should().Be("Excelente");
            dto2.Conservacao.Should().Be("Bom");
            dto3.Conservacao.Should().Be("Precisa Restauro");
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoCopiaDigitalComValoresDistintos_EntaoCopiaDigitalEhPreservada()
        {
            var dto1 = new AcervoDocumentalDetalheDTO { CopiaDigital = "Sim" };
            var dto2 = new AcervoDocumentalDetalheDTO { CopiaDigital = "Não" };
            var dto3 = new AcervoDocumentalDetalheDTO { CopiaDigital = "Parcial" };

            dto1.CopiaDigital.Should().Be("Sim");
            dto2.CopiaDigital.Should().Be("Não");
            dto3.CopiaDigital.Should().Be("Parcial");
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoAcessosDocumentosComMultiplosNiveis_EntaoAcessosDocumentosEhPreservado()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string acessosMultiplos = "Público, Pesquisadores, Funcionários, Direção";

            dto.AcessosDocumentos = acessosMultiplos;

            dto.AcessosDocumentos.Should().Be(acessosMultiplos);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoImagensVazia_EntaoImagensEhVazia()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            var imagensVazias = new ImagemDTO[] { };

            dto.Imagens = imagensVazias;

            dto.Imagens.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoImagensComUnicaImagem_EntaoImagensTemUmaImagem()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            var imagens = new[]
            {
                new ImagemDTO { Original = "https://exemplo.com/original.jpg", Thumbnail = "https://exemplo.com/thumb.jpg" }
            };

            dto.Imagens = imagens;

            dto.Imagens.Should().HaveCount(1);
            dto.Imagens.First().Original.Should().Be("https://exemplo.com/original.jpg");
            dto.Imagens.First().Thumbnail.Should().Be("https://exemplo.com/thumb.jpg");
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoImagensComMultiplasImagens_EntaoImagensTemMultiplasImagens()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            var imagens = new[]
            {
                new ImagemDTO { Original = "https://exemplo.com/img1.jpg", Thumbnail = "https://exemplo.com/thumb1.jpg" },
                new ImagemDTO { Original = "https://exemplo.com/img2.jpg", Thumbnail = "https://exemplo.com/thumb2.jpg" },
                new ImagemDTO { Original = "https://exemplo.com/img3.jpg", Thumbnail = "https://exemplo.com/thumb3.jpg" }
            };

            dto.Imagens = imagens;

            dto.Imagens.Should().HaveCount(3);
            dto.Imagens.Should().AllSatisfy(img => img.Original.Should().NotBeNullOrEmpty());
            dto.Imagens.Should().AllSatisfy(img => img.Thumbnail.Should().NotBeNullOrEmpty());
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoModificarPropriedades_EntaoPropriedadesSaoAtualizadas()
        {
            var dto = CriarAcervoDocumentalDetalheDTO();
            const string novaDescricao = "Descrição Atualizada";

            dto.Descricao = novaDescricao;

            dto.Descricao.Should().Be(novaDescricao);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoModificarDiversasVezesPropriedades_EntaoUltimoValorEhPreservado()
        {
            var dto = new AcervoDocumentalDetalheDTO();

            dto.Descricao = "Descrição 1";
            dto.Descricao = "Descrição 2";
            dto.Descricao = "Descrição 3";

            dto.Descricao.Should().Be("Descrição 3");
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDefinirTodasAsPropriedades_EntaoTodosOsValoresSaoPreservados()
        {
            var dto = CriarAcervoDocumentalDetalheDTO();

            dto.Descricao.Should().NotBeNull();
            dto.CreditosAutores.Should().NotBeNull();
            dto.CodigoNovo.Should().NotBeNull();
            dto.Material.Should().NotBeNull();
            dto.Idioma.Should().NotBeNull();
            dto.NumeroPagina.Should().BeGreaterThan(0);
            dto.Volume.Should().NotBeNull();
            dto.TipoAnexo.Should().NotBeNull();
            dto.Dimensoes.Should().NotBeNull();
            dto.TamanhoArquivo.Should().NotBeNull();
            dto.Localizacao.Should().NotBeNull();
            dto.CopiaDigital.Should().NotBeNull();
            dto.Conservacao.Should().NotBeNull();
            dto.AcessosDocumentos.Should().NotBeNull();
            dto.Imagens.Should().NotBeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoHerancaDeProp_EntaoPropriedadesHerdadasSaoAcessadas()
        {
            var dto = new AcervoDocumentalDetalheDTO
            {
                Titulo = "Título Herdado",
                Codigo = "COD-HERANCA",
                Ano = "2024",
                AcervoId = 50,
                EnderecoImagemPadrao = "https://exemplo.com/imagem.jpg",
                SituacaoDisponibilidade = "Disponível",
                EstaDisponivel = true,
                TemControleDisponibilidade = true,
                TipoAcervoId = 2,
                Descricao = "Descrição nova"
            };

            dto.Titulo.Should().Be("Título Herdado");
            dto.Codigo.Should().Be("COD-HERANCA");
            dto.Ano.Should().Be("2024");
            dto.AcervoId.Should().Be(50);
            dto.EnderecoImagemPadrao.Should().Be("https://exemplo.com/imagem.jpg");
            dto.SituacaoDisponibilidade.Should().Be("Disponível");
            dto.EstaDisponivel.Should().BeTrue();
            dto.TemControleDisponibilidade.Should().BeTrue();
            dto.TipoAcervoId.Should().Be(2);
            dto.Descricao.Should().Be("Descrição nova");
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoAcessarPropriedadesComConstrutorVazio_EntaoPropriedadesNaoLancamExcecao()
        {
            var exception = Record.Exception(() =>
            {
                var dto = new AcervoDocumentalDetalheDTO();
                _ = dto.Descricao;
                _ = dto.CreditosAutores;
                _ = dto.CodigoNovo;
                _ = dto.Material;
                _ = dto.Idioma;
                _ = dto.NumeroPagina;
                _ = dto.Volume;
                _ = dto.TipoAnexo;
                _ = dto.Dimensoes;
                _ = dto.TamanhoArquivo;
                _ = dto.Localizacao;
                _ = dto.CopiaDigital;
                _ = dto.Conservacao;
                _ = dto.AcessosDocumentos;
                _ = dto.Imagens;
            });

            exception.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoCompararDuasInstancias_EntaoSaoInstanciasDistintas()
        {
            var dto1 = CriarAcervoDocumentalDetalheDTO();
            var dto2 = CriarAcervoDocumentalDetalheDTO();

            dto1.Should().NotBeSameAs(dto2);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoNumeroPaginaMuda_EntaoNovoValorEhArmazenado()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            dto.NumeroPagina = 100;

            dto.NumeroPagina = 200;

            dto.NumeroPagina.Should().Be(200);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoInicializarComValoresDefault_EntaoPropriedadesTemValoresCorretos()
        {
            var dto = new AcervoDocumentalDetalheDTO
            {
                Titulo = "Título",
                Codigo = "COD001",
                Ano = "2024",
                AcervoId = 1,
                TipoAcervoId = 1,
                Descricao = "Descrição",
                NumeroPagina = 100
            };

            dto.Titulo.Should().Be("Título");
            dto.Codigo.Should().Be("COD001");
            dto.Ano.Should().Be("2024");
            dto.AcervoId.Should().Be(1);
            dto.TipoAcervoId.Should().Be(1);
            dto.Descricao.Should().Be("Descrição");
            dto.NumeroPagina.Should().Be(100);
            dto.CreditosAutores.Should().BeNull();
            dto.CodigoNovo.Should().BeNull();
            dto.Material.Should().BeNull();
            dto.Idioma.Should().BeNull();
            dto.Volume.Should().BeNull();
            dto.TipoAnexo.Should().BeNull();
            dto.Dimensoes.Should().BeNull();
            dto.TamanhoArquivo.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.Conservacao.Should().BeNull();
            dto.AcessosDocumentos.Should().BeNull();
            dto.Imagens.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoTodasAsPropriedadesSaoDefinidas_EntaoTodosCamposSaoAcessados()
        {
            var imagens = new[]
            {
                new ImagemDTO { Original = "https://exemplo.com/original.jpg", Thumbnail = "https://exemplo.com/thumb.jpg" }
            };

            var dto = new AcervoDocumentalDetalheDTO
            {
                Titulo = "Documento Completo",
                Codigo = "DOC-FULL",
                Ano = "2024",
                AcervoId = 999,
                EnderecoImagemPadrao = "https://exemplo.com/padrao.jpg",
                SituacaoDisponibilidade = "Disponível",
                EstaDisponivel = true,
                TemControleDisponibilidade = true,
                TipoAcervoId = 5,
                Descricao = "Descrição Completa",
                CreditosAutores = "Todos os Autores",
                CodigoNovo = "DOC-FULL-NEW",
                Material = "Papel Premium",
                Idioma = "Português Brasileiro",
                NumeroPagina = 5000,
                Volume = "Vol. Especial",
                TipoAnexo = "PDF-A",
                Dimensoes = "30 x 40 x 5 cm",
                TamanhoArquivo = "500MB",
                Localizacao = "Arquivo Geral - Setor D",
                CopiaDigital = "Sim",
                Conservacao = "Excelente",
                AcessosDocumentos = "Público Total",
                Imagens = imagens
            };

            dto.Titulo.Should().Be("Documento Completo");
            dto.Codigo.Should().Be("DOC-FULL");
            dto.Ano.Should().Be("2024");
            dto.AcervoId.Should().Be(999);
            dto.EnderecoImagemPadrao.Should().Be("https://exemplo.com/padrao.jpg");
            dto.SituacaoDisponibilidade.Should().Be("Disponível");
            dto.EstaDisponivel.Should().BeTrue();
            dto.TemControleDisponibilidade.Should().BeTrue();
            dto.TipoAcervoId.Should().Be(5);
            dto.Descricao.Should().Be("Descrição Completa");
            dto.CreditosAutores.Should().Be("Todos os Autores");
            dto.CodigoNovo.Should().Be("DOC-FULL-NEW");
            dto.Material.Should().Be("Papel Premium");
            dto.Idioma.Should().Be("Português Brasileiro");
            dto.NumeroPagina.Should().Be(5000);
            dto.Volume.Should().Be("Vol. Especial");
            dto.TipoAnexo.Should().Be("PDF-A");
            dto.Dimensoes.Should().Be("30 x 40 x 5 cm");
            dto.TamanhoArquivo.Should().Be("500MB");
            dto.Localizacao.Should().Be("Arquivo Geral - Setor D");
            dto.CopiaDigital.Should().Be("Sim");
            dto.Conservacao.Should().Be("Excelente");
            dto.AcessosDocumentos.Should().Be("Público Total");
            dto.Imagens.Should().BeEquivalentTo(imagens);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoCodigoNovoVazio_EntaoCodigoNovoEhVazio()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string codigoNovoVazio = "";

            dto.CodigoNovo = codigoNovoVazio;

            dto.CodigoNovo.Should().Be(codigoNovoVazio);
            dto.CodigoNovo.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoMaterialVazio_EntaoMaterialEhVazio()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string materialVazio = "";

            dto.Material = materialVazio;

            dto.Material.Should().Be(materialVazio);
            dto.Material.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoIdiomaVazio_EntaoIdiomaEhVazio()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string idiomaVazio = "";

            dto.Idioma = idiomaVazio;

            dto.Idioma.Should().Be(idiomaVazio);
            dto.Idioma.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoVolumeVazio_EntaoVolumeEhVazio()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string volumeVazio = "";

            dto.Volume = volumeVazio;

            dto.Volume.Should().Be(volumeVazio);
            dto.Volume.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoTipoAnexoVazio_EntaoTipoAnexoEhVazio()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string tipoAnexoVazio = "";

            dto.TipoAnexo = tipoAnexoVazio;

            dto.TipoAnexo.Should().Be(tipoAnexoVazio);
            dto.TipoAnexo.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoDimensoesVazia_EntaoDimensoesEhVazia()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string dimensoesVazia = "";

            dto.Dimensoes = dimensoesVazia;

            dto.Dimensoes.Should().Be(dimensoesVazia);
            dto.Dimensoes.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoTamanhoArquivoVazio_EntaoTamanhoArquivoEhVazio()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string tamanhoVazio = "";

            dto.TamanhoArquivo = tamanhoVazio;

            dto.TamanhoArquivo.Should().Be(tamanhoVazio);
            dto.TamanhoArquivo.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoLocalizacaoVazia_EntaoLocalizacaoEhVazia()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string localizacaoVazia = "";

            dto.Localizacao = localizacaoVazia;

            dto.Localizacao.Should().Be(localizacaoVazia);
            dto.Localizacao.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoCopiaDigitalVazia_EntaoCopiaDigitalEhVazia()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string copiaVazia = "";

            dto.CopiaDigital = copiaVazia;

            dto.CopiaDigital.Should().Be(copiaVazia);
            dto.CopiaDigital.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoConservacaoVazia_EntaoConservacaoEhVazia()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string conservacaoVazia = "";

            dto.Conservacao = conservacaoVazia;

            dto.Conservacao.Should().Be(conservacaoVazia);
            dto.Conservacao.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoAcessosDocumentosVazio_EntaoAcessosDocumentosEhVazio()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            const string acessosVazios = "";

            dto.AcessosDocumentos = acessosVazios;

            dto.AcessosDocumentos.Should().Be(acessosVazios);
            dto.AcessosDocumentos.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoNumeroPaginaZero_EntaoNumeroPaginaEhZero()
        {
            var dto = new AcervoDocumentalDetalheDTO();

            dto.NumeroPagina = 0;

            dto.NumeroPagina.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoImagensComUrlsCompletas_EntaoUrlsEhPreservadas()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            var imagens = new[]
            {
                new ImagemDTO
                {
                    Original = "https://storage.cdn.exemplo.com:8080/acervo/2024/original.jpg?w=2000&h=2000&format=high",
                    Thumbnail = "https://storage.cdn.exemplo.com:8080/acervo/2024/thumb.jpg?w=200&h=200&format=medium"
                }
            };

            dto.Imagens = imagens;

            dto.Imagens.First().Original.Should().Contain("?w=2000");
            dto.Imagens.First().Thumbnail.Should().Contain("?w=200");
        }

        [Fact]
        public void DadoAcervoDocumentalDetalhe_QuandoImagensComNullsInternos_EntaoNullsEhPreservados()
        {
            var dto = new AcervoDocumentalDetalheDTO();
            var imagens = new[]
            {
                new ImagemDTO { Original = null, Thumbnail = "https://exemplo.com/thumb.jpg" },
                new ImagemDTO { Original = "https://exemplo.com/original.jpg", Thumbnail = null }
            };

            dto.Imagens = imagens;

            dto.Imagens.Should().HaveCount(2);
            dto.Imagens.First().Original.Should().BeNull();
            dto.Imagens.Last().Thumbnail.Should().BeNull();
        }
    }
}
