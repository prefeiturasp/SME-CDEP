using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoDocumentalLinhaDtoTeste
    {
        private static AcervoDocumentalLinhaDTO CriarAcervoDocumentalLinhaCompleto()
        {
            return new AcervoDocumentalLinhaDTO
            {
                Status = ImportacaoStatus.Sucesso,
                Mensagem = string.Empty,
                NumeroLinha = 5,
                PossuiErros = false,
                Titulo = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "Título do Documento",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 500,
                    EhCampoObrigatorio = true
                },
                Codigo = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "DOC-001",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 50,
                    EhCampoObrigatorio = true
                },
                CodigoNovo = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "DOC-001-NEW",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 50,
                    EhCampoObrigatorio = false
                },
                Material = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "Papel",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 100,
                    EhCampoObrigatorio = false
                },
                Idioma = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "Português",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 100,
                    EhCampoObrigatorio = false
                },
                Autor = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "João Silva",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 500,
                    EhCampoObrigatorio = false
                },
                Ano = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "2024",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 4,
                    EhCampoObrigatorio = false
                },
                NumeroPaginas = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "250",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 10,
                    EhCampoObrigatorio = false,
                    FormatoTipoDeCampo = "número"
                },
                Volume = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "Vol. 1",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 50,
                    EhCampoObrigatorio = false
                },
                Descricao = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "Descrição do documento",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 2000,
                    EhCampoObrigatorio = false
                },
                TipoAnexo = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "PDF",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 50,
                    EhCampoObrigatorio = false
                },
                Altura = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "29.7cm",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 50,
                    EhCampoObrigatorio = false
                },
                Largura = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "21cm",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 50,
                    EhCampoObrigatorio = false
                },
                TamanhoArquivo = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "5MB",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 50,
                    EhCampoObrigatorio = false
                },
                AcessoDocumento = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "Público",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 100,
                    EhCampoObrigatorio = false
                },
                Localizacao = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "Sala 101 - Prateleira A",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 500,
                    EhCampoObrigatorio = false
                },
                CopiaDigital = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "Sim",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 10,
                    EhCampoObrigatorio = false
                },
                EstadoConservacao = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "Bom",
                    PossuiErro = false,
                    Mensagem = string.Empty,
                    LimiteCaracteres = 100,
                    EhCampoObrigatorio = false
                }
            };
        }

        #region Testes da Classe Base (AcervoLinhaDTO)

        [Fact]
        public void DadoAcervoDocumentalLinha_QuandoInstanciar_EntaoStatusEhPadraoMaisDefaulPadrao()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.Status.Should().Be(default(ImportacaoStatus));
        }

        [Fact]
        public void DadoAcervoDocumentalLinha_QuandoInstanciar_EntaoMensagemEhNula()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.Mensagem.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalLinha_QuandoInstanciar_EntaoNumeroLinhaEhZero()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.NumeroLinha.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoDocumentalLinha_QuandoInstanciar_EntaoPossuiErrosEhFalse()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.PossuiErros.Should().BeFalse();
        }

        [Fact]
        public void DadoStatus_QuandoDefinirSucesso_EntaoStatusEhSucesso()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.Status = ImportacaoStatus.Sucesso;

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact]
        public void DadoStatus_QuandoDefinirErro_EntaoStatusEhErro()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.Status = ImportacaoStatus.Erros;

            dto.Status.Should().Be(ImportacaoStatus.Erros);
        }

        [Fact]
        public void DadoStatus_QuandoDefinirPendente_EntaoStatusEhPendente()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.Status = ImportacaoStatus.Pendente;

            dto.Status.Should().Be(ImportacaoStatus.Pendente);
        }

        [Fact]
        public void DadoMensagem_QuandoDefinirValor_EntaoMensagemEhAtribuida()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            const string mensagemEsperada = "Importação realizada com sucesso";

            dto.Mensagem = mensagemEsperada;

            dto.Mensagem.Should().Be(mensagemEsperada);
        }

        [Fact]
        public void DadoMensagem_QuandoDefinirVazia_EntaoMensagemEhVazia()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.Mensagem = string.Empty;

            dto.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoNumeroLinha_QuandoDefinirValor_EntaoNumeroLinhaEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            const int numeroLinhaEsperado = 42;

            dto.NumeroLinha = numeroLinhaEsperado;

            dto.NumeroLinha.Should().Be(numeroLinhaEsperado);
        }

        [Fact]
        public void DadoPossuiErros_QuandoDefinirTrue_EntaoPossuiErrosEhTrue()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.PossuiErros = true;

            dto.PossuiErros.Should().BeTrue();
        }

        [Fact]
        public void DadoDefinirLinhaComoErro_QuandoChamar_EntaoStatusEhErroEMensagemEhAtribuida()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            const string mensagemErro = "Campo obrigatório não preenchido";

            dto.DefinirLinhaComoErro(mensagemErro);

            dto.PossuiErros.Should().BeTrue();
            dto.Status.Should().Be(ImportacaoStatus.Erros);
            dto.Mensagem.Should().Be(mensagemErro);
        }

        #endregion

        #region Testes das Propriedades LinhaConteudoAjustarDTO

        [Fact]
        public void DadoTitulo_QuandoInstanciar_EntaoTituloEhNulo()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.Titulo.Should().BeNull();
        }

        [Fact]
        public void DadoCodigo_QuandoInstanciar_EntaoCodigoEhNulo()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.Codigo.Should().BeNull();
        }

        [Fact]
        public void DadoCodigoNovo_QuandoInstanciar_EntaoCodigoNovoEhNulo()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.CodigoNovo.Should().BeNull();
        }

        [Fact]
        public void DadoMaterial_QuandoInstanciar_EntaoMaterialEhNulo()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.Material.Should().BeNull();
        }

        [Fact]
        public void DadoIdioma_QuandoInstanciar_EntaoIdiomaEhNulo()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.Idioma.Should().BeNull();
        }

        [Fact]
        public void DadoAutor_QuandoInstanciar_EntaoAutorEhNulo()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.Autor.Should().BeNull();
        }

        [Fact]
        public void DadoAno_QuandoInstanciar_EntaoAnoEhNulo()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoNumeroPaginas_QuandoInstanciar_EntaoNumeroPaginasEhNulo()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.NumeroPaginas.Should().BeNull();
        }

        [Fact]
        public void DadoVolume_QuandoInstanciar_EntaoVolumeEhNulo()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.Volume.Should().BeNull();
        }

        [Fact]
        public void DadoDescricao_QuandoInstanciar_EntaoDescricaoEhNula()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.Descricao.Should().BeNull();
        }

        [Fact]
        public void DadoTipoAnexo_QuandoInstanciar_EntaoTipoAnexoEhNulo()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.TipoAnexo.Should().BeNull();
        }

        [Fact]
        public void DadoAltura_QuandoInstanciar_EntaoAlturaEhNula()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.Altura.Should().BeNull();
        }

        [Fact]
        public void DadoLargura_QuandoInstanciar_EntaoLarguraEhNula()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.Largura.Should().BeNull();
        }

        [Fact]
        public void DadoTamanhoArquivo_QuandoInstanciar_EntaoTamanhoArquivoEhNulo()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.TamanhoArquivo.Should().BeNull();
        }

        [Fact]
        public void DadoAcessoDocumento_QuandoInstanciar_EntaoAcessoDocumentoEhNulo()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.AcessoDocumento.Should().BeNull();
        }

        [Fact]
        public void DadoLocalizacao_QuandoInstanciar_EntaoLocalizacaoEhNula()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.Localizacao.Should().BeNull();
        }

        [Fact]
        public void DadoCopiaDigital_QuandoInstanciar_EntaoCopiaDigitalEhNula()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.CopiaDigital.Should().BeNull();
        }

        [Fact]
        public void DadoEstadoConservacao_QuandoInstanciar_EntaoEstadoConservacaoEhNulo()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.EstadoConservacao.Should().BeNull();
        }

        [Fact]
        public void DadoTitulo_QuandoDefinirValor_EntaoTituloEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO
            {
                Conteudo = "Título Teste",
                PossuiErro = false,
                Mensagem = string.Empty
            };

            dto.Titulo = linhaConteudo;

            dto.Titulo.Should().NotBeNull();
            dto.Titulo.Conteudo.Should().Be("Título Teste");
            dto.Titulo.PossuiErro.Should().BeFalse();
        }

        [Fact]
        public void DadoCodigo_QuandoDefinirValor_EntaoCodigoEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO
            {
                Conteudo = "COD-123",
                PossuiErro = false,
                Mensagem = string.Empty,
                EhCampoObrigatorio = true
            };

            dto.Codigo = linhaConteudo;

            dto.Codigo.Should().NotBeNull();
            dto.Codigo.Conteudo.Should().Be("COD-123");
            dto.Codigo.EhCampoObrigatorio.Should().BeTrue();
        }

        [Fact]
        public void DadoCodigoNovo_QuandoDefinirValor_EntaoCodigoNovoEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "COD-NEW-123" };

            dto.CodigoNovo = linhaConteudo;

            dto.CodigoNovo.Should().NotBeNull();
            dto.CodigoNovo.Conteudo.Should().Be("COD-NEW-123");
        }

        [Fact]
        public void DadoMaterial_QuandoDefinirValor_EntaoMaterialEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Papel" };

            dto.Material = linhaConteudo;

            dto.Material.Should().NotBeNull();
            dto.Material.Conteudo.Should().Be("Papel");
        }

        [Fact]
        public void DadoIdioma_QuandoDefinirValor_EntaoIdiomaEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Português" };

            dto.Idioma = linhaConteudo;

            dto.Idioma.Should().NotBeNull();
            dto.Idioma.Conteudo.Should().Be("Português");
        }

        [Fact]
        public void DadoAutor_QuandoDefinirValor_EntaoAutorEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Silva, João" };

            dto.Autor = linhaConteudo;

            dto.Autor.Should().NotBeNull();
            dto.Autor.Conteudo.Should().Be("Silva, João");
        }

        [Fact]
        public void DadoAno_QuandoDefinirValor_EntaoAnoEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "2024" };

            dto.Ano = linhaConteudo;

            dto.Ano.Should().NotBeNull();
            dto.Ano.Conteudo.Should().Be("2024");
        }

        [Fact]
        public void DadoNumeroPaginas_QuandoDefinirValor_EntaoNumeroPaginasEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO
            {
                Conteudo = "250",
                FormatoTipoDeCampo = "número"
            };

            dto.NumeroPaginas = linhaConteudo;

            dto.NumeroPaginas.Should().NotBeNull();
            dto.NumeroPaginas.Conteudo.Should().Be("250");
            dto.NumeroPaginas.FormatoTipoDeCampo.Should().Be("número");
        }

        [Fact]
        public void DadoVolume_QuandoDefinirValor_EntaoVolumeEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Vol. 1" };

            dto.Volume = linhaConteudo;

            dto.Volume.Should().NotBeNull();
            dto.Volume.Conteudo.Should().Be("Vol. 1");
        }

        [Fact]
        public void DadoDescricao_QuandoDefinirValor_EntaoDescricaoEhAtribuida()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Descrição completa do documento" };

            dto.Descricao = linhaConteudo;

            dto.Descricao.Should().NotBeNull();
            dto.Descricao.Conteudo.Should().Be("Descrição completa do documento");
        }

        [Fact]
        public void DadoTipoAnexo_QuandoDefinirValor_EntaoTipoAnexoEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "PDF" };

            dto.TipoAnexo = linhaConteudo;

            dto.TipoAnexo.Should().NotBeNull();
            dto.TipoAnexo.Conteudo.Should().Be("PDF");
        }

        [Fact]
        public void DadoAltura_QuandoDefinirValor_EntaoAlturaEhAtribuida()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "29.7cm" };

            dto.Altura = linhaConteudo;

            dto.Altura.Should().NotBeNull();
            dto.Altura.Conteudo.Should().Be("29.7cm");
        }

        [Fact]
        public void DadoLargura_QuandoDefinirValor_EntaoLarguraEhAtribuida()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "21cm" };

            dto.Largura = linhaConteudo;

            dto.Largura.Should().NotBeNull();
            dto.Largura.Conteudo.Should().Be("21cm");
        }

        [Fact]
        public void DadoTamanhoArquivo_QuandoDefinirValor_EntaoTamanhoArquivoEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "5MB" };

            dto.TamanhoArquivo = linhaConteudo;

            dto.TamanhoArquivo.Should().NotBeNull();
            dto.TamanhoArquivo.Conteudo.Should().Be("5MB");
        }

        [Fact]
        public void DadoAcessoDocumento_QuandoDefinirValor_EntaoAcessoDocumentoEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Público" };

            dto.AcessoDocumento = linhaConteudo;

            dto.AcessoDocumento.Should().NotBeNull();
            dto.AcessoDocumento.Conteudo.Should().Be("Público");
        }

        [Fact]
        public void DadoLocalizacao_QuandoDefinirValor_EntaoLocalizacaoEhAtribuida()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Sala 101" };

            dto.Localizacao = linhaConteudo;

            dto.Localizacao.Should().NotBeNull();
            dto.Localizacao.Conteudo.Should().Be("Sala 101");
        }

        [Fact]
        public void DadoCopiaDigital_QuandoDefinirValor_EntaoCopiaDigitalEhAtribuida()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Sim" };

            dto.CopiaDigital = linhaConteudo;

            dto.CopiaDigital.Should().NotBeNull();
            dto.CopiaDigital.Conteudo.Should().Be("Sim");
        }

        [Fact]
        public void DadoEstadoConservacao_QuandoDefinirValor_EntaoEstadoConservacaoEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Bom" };

            dto.EstadoConservacao = linhaConteudo;

            dto.EstadoConservacao.Should().NotBeNull();
            dto.EstadoConservacao.Conteudo.Should().Be("Bom");
        }

        #endregion

        #region Testes do Método DefinirLinhaComoSucesso

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoPossuiErrosEhFalse()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.PossuiErros = true;

            dto.DefinirLinhaComoSucesso();

            dto.PossuiErros.Should().BeFalse();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoMensagemEhVazia()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.Mensagem = "Alguma mensagem";

            dto.DefinirLinhaComoSucesso();

            dto.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoStatusEhSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.Status = ImportacaoStatus.Pendente;

            dto.DefinirLinhaComoSucesso();

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoTituloDefinidoComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.Titulo.PossuiErro = true;
            dto.Titulo.Mensagem = "Erro no título";

            dto.DefinirLinhaComoSucesso();

            dto.Titulo.PossuiErro.Should().BeFalse();
            dto.Titulo.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoCodigoDefinidoComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.Codigo.PossuiErro = true;
            dto.Codigo.Mensagem = "Código inválido";

            dto.DefinirLinhaComoSucesso();

            dto.Codigo.PossuiErro.Should().BeFalse();
            dto.Codigo.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoCodigoNovoDefinidoComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.CodigoNovo.PossuiErro = true;
            dto.CodigoNovo.Mensagem = "Erro no código novo";

            dto.DefinirLinhaComoSucesso();

            dto.CodigoNovo.PossuiErro.Should().BeFalse();
            dto.CodigoNovo.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoMaterialDefinidoComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.Material.PossuiErro = true;
            dto.Material.Mensagem = "Material inválido";

            dto.DefinirLinhaComoSucesso();

            dto.Material.PossuiErro.Should().BeFalse();
            dto.Material.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoIdiomaDefinidoComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.Idioma.PossuiErro = true;
            dto.Idioma.Mensagem = "Idioma não permitido";

            dto.DefinirLinhaComoSucesso();

            dto.Idioma.PossuiErro.Should().BeFalse();
            dto.Idioma.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoAutorDefinidoComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.Autor.PossuiErro = true;
            dto.Autor.Mensagem = "Autor inválido";

            dto.DefinirLinhaComoSucesso();

            dto.Autor.PossuiErro.Should().BeFalse();
            dto.Autor.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoAnoDefinidoComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.Ano.PossuiErro = true;
            dto.Ano.Mensagem = "Ano inválido";

            dto.DefinirLinhaComoSucesso();

            dto.Ano.PossuiErro.Should().BeFalse();
            dto.Ano.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoNumeroPaginasDefinidoComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.NumeroPaginas.PossuiErro = true;
            dto.NumeroPaginas.Mensagem = "Número de páginas inválido";

            dto.DefinirLinhaComoSucesso();

            dto.NumeroPaginas.PossuiErro.Should().BeFalse();
            dto.NumeroPaginas.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoVolumeDefinidoComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.Volume.PossuiErro = true;
            dto.Volume.Mensagem = "Volume inválido";

            dto.DefinirLinhaComoSucesso();

            dto.Volume.PossuiErro.Should().BeFalse();
            dto.Volume.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoDescricaoDefinidaComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.Descricao.PossuiErro = true;
            dto.Descricao.Mensagem = "Descrição inválida";

            dto.DefinirLinhaComoSucesso();

            dto.Descricao.PossuiErro.Should().BeFalse();
            dto.Descricao.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoTipoAnexoDefinidoComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.TipoAnexo.PossuiErro = true;
            dto.TipoAnexo.Mensagem = "Tipo anexo inválido";

            dto.DefinirLinhaComoSucesso();

            dto.TipoAnexo.PossuiErro.Should().BeFalse();
            dto.TipoAnexo.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoAlturaDefinidaComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.Altura.PossuiErro = true;
            dto.Altura.Mensagem = "Altura inválida";

            dto.DefinirLinhaComoSucesso();

            dto.Altura.PossuiErro.Should().BeFalse();
            dto.Altura.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoLarguraDefinidaComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.Largura.PossuiErro = true;
            dto.Largura.Mensagem = "Largura inválida";

            dto.DefinirLinhaComoSucesso();

            dto.Largura.PossuiErro.Should().BeFalse();
            dto.Largura.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoTamanhoArquivoDefinidoComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.TamanhoArquivo.PossuiErro = true;
            dto.TamanhoArquivo.Mensagem = "Tamanho arquivo inválido";

            dto.DefinirLinhaComoSucesso();

            dto.TamanhoArquivo.PossuiErro.Should().BeFalse();
            dto.TamanhoArquivo.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoAcessoDocumentoDefinidoComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.AcessoDocumento.PossuiErro = true;
            dto.AcessoDocumento.Mensagem = "Acesso documento inválido";

            dto.DefinirLinhaComoSucesso();

            dto.AcessoDocumento.PossuiErro.Should().BeFalse();
            dto.AcessoDocumento.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoLocalizacaoDefinidaComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.Localizacao.PossuiErro = true;
            dto.Localizacao.Mensagem = "Localização inválida";

            dto.DefinirLinhaComoSucesso();

            dto.Localizacao.PossuiErro.Should().BeFalse();
            dto.Localizacao.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoCopiaDigitalDefinidaComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.CopiaDigital.PossuiErro = true;
            dto.CopiaDigital.Mensagem = "Cópia digital inválida";

            dto.DefinirLinhaComoSucesso();

            dto.CopiaDigital.PossuiErro.Should().BeFalse();
            dto.CopiaDigital.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoChamar_EntaoEstadoConservacaoDefinidoComoSucesso()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.EstadoConservacao.PossuiErro = true;
            dto.EstadoConservacao.Mensagem = "Estado conservação inválido";

            dto.DefinirLinhaComoSucesso();

            dto.EstadoConservacao.PossuiErro.Should().BeFalse();
            dto.EstadoConservacao.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucesso_QuandoTodosOsCamposTemErros_EntaoTodosSaoLimpos()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();
            dto.PossuiErros = true;
            dto.Mensagem = "Linha com erros";
            dto.Status = ImportacaoStatus.Erros;

            foreach (var propriedade in typeof(AcervoDocumentalLinhaDTO).GetProperties())
            {
                if (propriedade.PropertyType == typeof(LinhaConteudoAjustarDTO))
                {
                    var linhaConteudo = (LinhaConteudoAjustarDTO)propriedade.GetValue(dto)!;
                    if (linhaConteudo != null)
                    {
                        linhaConteudo.PossuiErro = true;
                        linhaConteudo.Mensagem = "Erro";
                    }
                }
            }

            dto.DefinirLinhaComoSucesso();

            dto.PossuiErros.Should().BeFalse();
            dto.Mensagem.Should().BeEmpty();
            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        #endregion

        #region Testes Integrados e Casos de Uso

        [Fact]
        public void DadoAcervoDocumentalLinhaCompleta_QuandoVerificarTodos_EntaoTodosOsCamposAcessiveis()
        {
            var dto = CriarAcervoDocumentalLinhaCompleto();

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.Mensagem.Should().BeEmpty();
            dto.NumeroLinha.Should().Be(5);
            dto.PossuiErros.Should().BeFalse();
            dto.Titulo.Should().NotBeNull();
            dto.Codigo.Should().NotBeNull();
            dto.CodigoNovo.Should().NotBeNull();
            dto.Material.Should().NotBeNull();
            dto.Idioma.Should().NotBeNull();
            dto.Autor.Should().NotBeNull();
            dto.Ano.Should().NotBeNull();
            dto.NumeroPaginas.Should().NotBeNull();
            dto.Volume.Should().NotBeNull();
            dto.Descricao.Should().NotBeNull();
            dto.TipoAnexo.Should().NotBeNull();
            dto.Altura.Should().NotBeNull();
            dto.Largura.Should().NotBeNull();
            dto.TamanhoArquivo.Should().NotBeNull();
            dto.AcessoDocumento.Should().NotBeNull();
            dto.Localizacao.Should().NotBeNull();
            dto.CopiaDigital.Should().NotBeNull();
            dto.EstadoConservacao.Should().NotBeNull();
        }

        [Fact]
        public void DadoLinhaSemErros_QuandoDefinirComoSucesso_EntaoLinhaFinalizaComSucesso()
        {
            var dto = new AcervoDocumentalLinhaDTO
            {
                NumeroLinha = 1,
                Status = ImportacaoStatus.Pendente,
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Título" },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = "COD" },
                CodigoNovo = new LinhaConteudoAjustarDTO { Conteudo = "COD-NEW" },
                Material = new LinhaConteudoAjustarDTO { Conteudo = "Material" },
                Idioma = new LinhaConteudoAjustarDTO { Conteudo = "Idioma" },
                Autor = new LinhaConteudoAjustarDTO { Conteudo = "Autor" },
                Ano = new LinhaConteudoAjustarDTO { Conteudo = "2024" },
                NumeroPaginas = new LinhaConteudoAjustarDTO { Conteudo = "100" },
                Volume = new LinhaConteudoAjustarDTO { Conteudo = "Vol" },
                Descricao = new LinhaConteudoAjustarDTO { Conteudo = "Descrição" },
                TipoAnexo = new LinhaConteudoAjustarDTO { Conteudo = "PDF" },
                Altura = new LinhaConteudoAjustarDTO { Conteudo = "29.7" },
                Largura = new LinhaConteudoAjustarDTO { Conteudo = "21" },
                TamanhoArquivo = new LinhaConteudoAjustarDTO { Conteudo = "5MB" },
                AcessoDocumento = new LinhaConteudoAjustarDTO { Conteudo = "Público" },
                Localizacao = new LinhaConteudoAjustarDTO { Conteudo = "Sala" },
                CopiaDigital = new LinhaConteudoAjustarDTO { Conteudo = "Sim" },
                EstadoConservacao = new LinhaConteudoAjustarDTO { Conteudo = "Bom" }
            };

            dto.DefinirLinhaComoSucesso();

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.Mensagem.Should().BeEmpty();
            dto.PossuiErros.Should().BeFalse();
        }

        [Fact]
        public void DadoLinhaComErro_QuandoDefinirErro_EntaoLinhaFinalizaComErro()
        {
            var dto = new AcervoDocumentalLinhaDTO
            {
                NumeroLinha = 1,
                Status = ImportacaoStatus.Pendente
            };
            const string mensagemErro = "Campo obrigatório Título não foi preenchido";

            dto.DefinirLinhaComoErro(mensagemErro);

            dto.Status.Should().Be(ImportacaoStatus.Erros);
            dto.Mensagem.Should().Be(mensagemErro);
            dto.PossuiErros.Should().BeTrue();
        }

        [Fact]
        public void DadoMultiplasLinhas_QuandoProcessadas_EntaoCadaUmaTemEstadoIndependente()
        {
            var linha1 = new AcervoDocumentalLinhaDTO
            {
                NumeroLinha = 1,
                Titulo = new LinhaConteudoAjustarDTO(),
                Codigo = new LinhaConteudoAjustarDTO(),
                CodigoNovo = new LinhaConteudoAjustarDTO(),
                Material = new LinhaConteudoAjustarDTO(),
                Idioma = new LinhaConteudoAjustarDTO(),
                Autor = new LinhaConteudoAjustarDTO(),
                Ano = new LinhaConteudoAjustarDTO(),
                NumeroPaginas = new LinhaConteudoAjustarDTO(),
                Volume = new LinhaConteudoAjustarDTO(),
                Descricao = new LinhaConteudoAjustarDTO(),
                TipoAnexo = new LinhaConteudoAjustarDTO(),
                Altura = new LinhaConteudoAjustarDTO(),
                Largura = new LinhaConteudoAjustarDTO(),
                TamanhoArquivo = new LinhaConteudoAjustarDTO(),
                AcessoDocumento = new LinhaConteudoAjustarDTO(),
                Localizacao = new LinhaConteudoAjustarDTO(),
                CopiaDigital = new LinhaConteudoAjustarDTO(),
                EstadoConservacao = new LinhaConteudoAjustarDTO()
            };
            var linha2 = new AcervoDocumentalLinhaDTO
            {
                NumeroLinha = 2,
                Titulo = new LinhaConteudoAjustarDTO(),
                Codigo = new LinhaConteudoAjustarDTO(),
                CodigoNovo = new LinhaConteudoAjustarDTO(),
                Material = new LinhaConteudoAjustarDTO(),
                Idioma = new LinhaConteudoAjustarDTO(),
                Autor = new LinhaConteudoAjustarDTO(),
                Ano = new LinhaConteudoAjustarDTO(),
                NumeroPaginas = new LinhaConteudoAjustarDTO(),
                Volume = new LinhaConteudoAjustarDTO(),
                Descricao = new LinhaConteudoAjustarDTO(),
                TipoAnexo = new LinhaConteudoAjustarDTO(),
                Altura = new LinhaConteudoAjustarDTO(),
                Largura = new LinhaConteudoAjustarDTO(),
                TamanhoArquivo = new LinhaConteudoAjustarDTO(),
                AcessoDocumento = new LinhaConteudoAjustarDTO(),
                Localizacao = new LinhaConteudoAjustarDTO(),
                CopiaDigital = new LinhaConteudoAjustarDTO(),
                EstadoConservacao = new LinhaConteudoAjustarDTO()
            };

            linha1.DefinirLinhaComoErro("Erro na linha 1");
            linha2.DefinirLinhaComoSucesso();

            linha1.Status.Should().Be(ImportacaoStatus.Erros);
            linha2.Status.Should().Be(ImportacaoStatus.Sucesso);
            linha1.Mensagem.Should().Be("Erro na linha 1");
            linha2.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoLinhaComConteudoVazio_QuandoVerificar_EntaoAceitaConteudoVazio()
        {
            var dto = new AcervoDocumentalLinhaDTO
            {
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = string.Empty }
            };

            dto.Titulo.Conteudo.Should().BeEmpty();
        }

        [Fact]
        public void DadoLinhaComCaracteresEspeciais_QuandoArmazenar_EntaoPreservaCaracteres()
        {
            var conteudoComEspeciais = "Título com ç, é, ñ e símbolos @#$%";
            var dto = new AcervoDocumentalLinhaDTO
            {
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = conteudoComEspeciais }
            };

            dto.Titulo.Conteudo.Should().Be(conteudoComEspeciais);
        }

        [Fact]
        public void DadoLinhaComLimiteCaracteres_QuandoDefinir_EntaoLimiteEhArmazenado()
        {
            var dto = new AcervoDocumentalLinhaDTO
            {
                Titulo = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "Título",
                    LimiteCaracteres = 500
                }
            };

            dto.Titulo.LimiteCaracteres.Should().Be(500);
        }

        [Fact]
        public void DadoLinhaComValidacao_QuandoDefinir_EntaoValidacaoEhArmazenada()
        {
            var expressaoRegular = @"^[0-9]{4}$";
            var dto = new AcervoDocumentalLinhaDTO
            {
                Ano = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "2024",
                    ValidarComExpressaoRegular = expressaoRegular,
                    MensagemValidacao = "Ano deve ter 4 dígitos"
                }
            };

            dto.Ano.ValidarComExpressaoRegular.Should().Be(expressaoRegular);
            dto.Ano.MensagemValidacao.Should().Be("Ano deve ter 4 dígitos");
        }

        [Fact]
        public void DadoLinhaComValoresPermitidos_QuandoDefinir_EntaoValoresArmazenados()
        {
            var valoresPermitidos = new[] { "Papel", "Tecido", "Vidro" };
            var dto = new AcervoDocumentalLinhaDTO
            {
                Material = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "Papel",
                    ValoresPermitidos = valoresPermitidos
                }
            };

            dto.Material.ValoresPermitidos.Should().BeEquivalentTo(valoresPermitidos);
        }

        [Fact]
        public void DadoLinhaComCampoObrigatorio_QuandoDefinir_EntaoMarcarComoObrigatorio()
        {
            var dto = new AcervoDocumentalLinhaDTO
            {
                Titulo = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "Título",
                    EhCampoObrigatorio = true
                }
            };

            dto.Titulo.EhCampoObrigatorio.Should().BeTrue();
        }

        [Fact]
        public void DadoLinhaComNovoRegistro_QuandoDefinir_EntaoPermiteNovoRegistro()
        {
            var dto = new AcervoDocumentalLinhaDTO
            {
                Titulo = new LinhaConteudoAjustarDTO
                {
                    Conteudo = "Título Novo",
                    PermiteNovoRegistro = true
                }
            };

            dto.Titulo.PermiteNovoRegistro.Should().BeTrue();
        }

        [Fact]
        public void DadoNumeroLinhaMaior_QuandoAtribuir_EntaoArmazenaCorretamente()
        {
            var dto = new AcervoDocumentalLinhaDTO();

            dto.NumeroLinha = 999999;

            dto.NumeroLinha.Should().Be(999999);
        }

        [Fact]
        public void DadoMensagemComComprimento_QuandoAtribuir_EntaoArmazenaCompleta()
        {
            var dto = new AcervoDocumentalLinhaDTO();
            var mensagemLonga = new string('a', 1000);

            dto.Mensagem = mensagemLonga;

            dto.Mensagem.Length.Should().Be(1000);
        }

        [Fact]
        public void DadoConteudoComComprimento_QuandoAtribuir_EntaoArmazenaCompleto()
        {
            var conteudoLongo = new string('x', 5000);
            var dto = new AcervoDocumentalLinhaDTO
            {
                Descricao = new LinhaConteudoAjustarDTO
                {
                    Conteudo = conteudoLongo
                }
            };

            dto.Descricao.Conteudo.Length.Should().Be(5000);
        }

        #endregion
    }
}
