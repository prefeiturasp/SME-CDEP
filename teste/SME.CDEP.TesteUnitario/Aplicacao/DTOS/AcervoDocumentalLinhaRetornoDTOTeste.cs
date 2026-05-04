using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoDocumentalLinhaRetornoDTOTeste
    {
        private AcervoDocumentalLinhaRetornoDTO CriarAcervoDocumentalLinhaRetornoCompleto()
        {
            return new AcervoDocumentalLinhaRetornoDTO
            {
                Status = ImportacaoStatus.Sucesso,
                Mensagem = string.Empty,
                NumeroLinha = 5,
                ErrosCampos = null,
                Titulo = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "Título do Documento",
                    PossuiErro = false,
                    Mensagem = string.Empty
                },
                Codigo = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "DOC-001",
                    PossuiErro = false,
                    Mensagem = string.Empty
                },
                CodigoNovo = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "DOC-001-NEW",
                    PossuiErro = false,
                    Mensagem = string.Empty
                },
                MaterialId = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "1",
                    PossuiErro = false,
                    Mensagem = string.Empty
                },
                IdiomaId = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "2",
                    PossuiErro = false,
                    Mensagem = string.Empty
                },
                CreditosAutoresIds = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "1,2,3",
                    PossuiErro = false,
                    Mensagem = string.Empty
                },
                Ano = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "2024",
                    PossuiErro = false,
                    Mensagem = string.Empty
                },
                NumeroPagina = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "250",
                    PossuiErro = false,
                    Mensagem = string.Empty
                },
                Volume = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "Vol. 1",
                    PossuiErro = false,
                    Mensagem = string.Empty
                },
                Descricao = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "Descrição do documento",
                    PossuiErro = false,
                    Mensagem = string.Empty
                },
                TipoAnexo = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "PDF",
                    PossuiErro = false,
                    Mensagem = string.Empty
                },
                Altura = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "29.7cm",
                    PossuiErro = false,
                    Mensagem = string.Empty
                },
                Largura = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "21cm",
                    PossuiErro = false,
                    Mensagem = string.Empty
                },
                TamanhoArquivo = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "5MB",
                    PossuiErro = false,
                    Mensagem = string.Empty
                },
                AcessoDocumentosIds = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "1",
                    PossuiErro = false,
                    Mensagem = string.Empty
                },
                Localizacao = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "Sala 101 - Prateleira A",
                    PossuiErro = false,
                    Mensagem = string.Empty
                },
                CopiaDigital = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "Sim",
                    PossuiErro = false,
                    Mensagem = string.Empty
                },
                ConservacaoId = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "1",
                    PossuiErro = false,
                    Mensagem = string.Empty
                }
            };
        }

        #region Testes da Classe Base (AcervoLinhaRetornoDTO)

        [Fact]
        public void DadoAcervoDocumentalLinhaRetorno_QuandoInstanciar_EntaoStatusEhPadraoMaisDefault()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.Status.Should().Be(default(ImportacaoStatus));
        }

        [Fact]
        public void DadoAcervoDocumentalLinhaRetorno_QuandoInstanciar_EntaoMensagemEhNula()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.Mensagem.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalLinhaRetorno_QuandoInstanciar_EntaoNumeroLinhaEhZero()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.NumeroLinha.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoDocumentalLinhaRetorno_QuandoInstanciar_EntaoErrosCamposEhNulo()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.ErrosCampos.Should().BeNull();
        }

        [Fact]
        public void DadoStatus_QuandoDefinirSucesso_EntaoStatusEhSucesso()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.Status = ImportacaoStatus.Sucesso;

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact]
        public void DadoStatus_QuandoDefinirErro_EntaoStatusEhErro()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.Status = ImportacaoStatus.Erros;

            dto.Status.Should().Be(ImportacaoStatus.Erros);
        }

        [Fact]
        public void DadoStatus_QuandoDefinirPendente_EntaoStatusEhPendente()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.Status = ImportacaoStatus.Pendente;

            dto.Status.Should().Be(ImportacaoStatus.Pendente);
        }

        [Fact]
        public void DadoMensagem_QuandoDefinirValor_EntaoMensagemEhAtribuida()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            const string mensagemEsperada = "Importação realizada com sucesso";

            dto.Mensagem = mensagemEsperada;

            dto.Mensagem.Should().Be(mensagemEsperada);
        }

        [Fact]
        public void DadoMensagem_QuandoDefinirVazia_EntaoMensagemEhVazia()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.Mensagem = string.Empty;

            dto.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoNumeroLinha_QuandoDefinirValor_EntaoNumeroLinhaEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            const int numeroLinhaEsperado = 42;

            dto.NumeroLinha = numeroLinhaEsperado;

            dto.NumeroLinha.Should().Be(numeroLinhaEsperado);
        }

        [Fact]
        public void DadoErrosCampos_QuandoDefinirValor_EntaoErrosCamposEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var erros = new[] { "Título", "Código" };

            dto.ErrosCampos = erros;

            dto.ErrosCampos.Should().BeEquivalentTo(erros);
        }

        [Fact]
        public void DadoErrosCampos_QuandoDefinirVazio_EntaoErrosCamposEhVazio()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.ErrosCampos = new string[] { };

            dto.ErrosCampos.Should().BeEmpty();
        }

        [Fact]
        public void DadoNumeroLinhaMaior_QuandoAtribuir_EntaoArmazenaCorretamente()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.NumeroLinha = 999999;

            dto.NumeroLinha.Should().Be(999999);
        }

        [Fact]
        public void DadoMensagemComComprimento_QuandoAtribuir_EntaoArmazenaCompleta()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var mensagemLonga = new string('a', 1000);

            dto.Mensagem = mensagemLonga;

            dto.Mensagem.Length.Should().Be(1000);
        }

        #endregion

        #region Testes das Propriedades LinhaConteudoAjustarRetornoDTO

        [Fact]
        public void DadoTitulo_QuandoInstanciar_EntaoTituloEhNulo()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.Titulo.Should().BeNull();
        }

        [Fact]
        public void DadoCodigo_QuandoInstanciar_EntaoCodigoEhNulo()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.Codigo.Should().BeNull();
        }

        [Fact]
        public void DadoCodigoNovo_QuandoInstanciar_EntaoCodigoNovoEhNulo()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.CodigoNovo.Should().BeNull();
        }

        [Fact]
        public void DadoMaterialId_QuandoInstanciar_EntaoMaterialIdEhNulo()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.MaterialId.Should().BeNull();
        }

        [Fact]
        public void DadoIdiomaId_QuandoInstanciar_EntaoIdiomaIdEhNulo()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.IdiomaId.Should().BeNull();
        }

        [Fact]
        public void DadoCreditosAutoresIds_QuandoInstanciar_EntaoCreditosAutoresIdsEhNulo()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.CreditosAutoresIds.Should().BeNull();
        }

        [Fact]
        public void DadoAno_QuandoInstanciar_EntaoAnoEhNulo()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoNumeroPagina_QuandoInstanciar_EntaoNumeroPaginaEhNulo()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.NumeroPagina.Should().BeNull();
        }

        [Fact]
        public void DadoVolume_QuandoInstanciar_EntaoVolumeEhNulo()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.Volume.Should().BeNull();
        }

        [Fact]
        public void DadoDescricao_QuandoInstanciar_EntaoDescricaoEhNula()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.Descricao.Should().BeNull();
        }

        [Fact]
        public void DadoTipoAnexo_QuandoInstanciar_EntaoTipoAnexoEhNulo()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.TipoAnexo.Should().BeNull();
        }

        [Fact]
        public void DadoAltura_QuandoInstanciar_EntaoAlturaEhNula()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.Altura.Should().BeNull();
        }

        [Fact]
        public void DadoLargura_QuandoInstanciar_EntaoLarguraEhNula()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.Largura.Should().BeNull();
        }

        [Fact]
        public void DadoTamanhoArquivo_QuandoInstanciar_EntaoTamanhoArquivoEhNulo()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.TamanhoArquivo.Should().BeNull();
        }

        [Fact]
        public void DadoAcessoDocumentosIds_QuandoInstanciar_EntaoAcessoDocumentosIdsEhNulo()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.AcessoDocumentosIds.Should().BeNull();
        }

        [Fact]
        public void DadoLocalizacao_QuandoInstanciar_EntaoLocalizacaoEhNula()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.Localizacao.Should().BeNull();
        }

        [Fact]
        public void DadoCopiaDigital_QuandoInstanciar_EntaoCopiaDigitalEhNula()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.CopiaDigital.Should().BeNull();
        }

        [Fact]
        public void DadoConservacaoId_QuandoInstanciar_EntaoConservacaoIdEhNulo()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.ConservacaoId.Should().BeNull();
        }

        [Fact]
        public void DadoTitulo_QuandoDefinirValor_EntaoTituloEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
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
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "COD-123",
                PossuiErro = false,
                Mensagem = string.Empty
            };

            dto.Codigo = linhaConteudo;

            dto.Codigo.Should().NotBeNull();
            dto.Codigo.Conteudo.Should().Be("COD-123");
            dto.Codigo.PossuiErro.Should().BeFalse();
        }

        [Fact]
        public void DadoCodigoNovo_QuandoDefinirValor_EntaoCodigoNovoEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "COD-NEW-123" };

            dto.CodigoNovo = linhaConteudo;

            dto.CodigoNovo.Should().NotBeNull();
            dto.CodigoNovo.Conteudo.Should().Be("COD-NEW-123");
        }

        [Fact]
        public void DadoMaterialId_QuandoDefinirValor_EntaoMaterialIdEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1" };

            dto.MaterialId = linhaConteudo;

            dto.MaterialId.Should().NotBeNull();
            dto.MaterialId.Conteudo.Should().Be("1");
        }

        [Fact]
        public void DadoIdiomaId_QuandoDefinirValor_EntaoIdiomaIdEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "2" };

            dto.IdiomaId = linhaConteudo;

            dto.IdiomaId.Should().NotBeNull();
            dto.IdiomaId.Conteudo.Should().Be("2");
        }

        [Fact]
        public void DadoCreditosAutoresIds_QuandoDefinirValor_EntaoCreditosAutoresIdsEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1,2,3" };

            dto.CreditosAutoresIds = linhaConteudo;

            dto.CreditosAutoresIds.Should().NotBeNull();
            dto.CreditosAutoresIds.Conteudo.Should().Be("1,2,3");
        }

        [Fact]
        public void DadoAno_QuandoDefinirValor_EntaoAnoEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "2024" };

            dto.Ano = linhaConteudo;

            dto.Ano.Should().NotBeNull();
            dto.Ano.Conteudo.Should().Be("2024");
        }

        [Fact]
        public void DadoNumeroPagina_QuandoDefinirValor_EntaoNumeroPaginaEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "250",
            };

            dto.NumeroPagina = linhaConteudo;

            dto.NumeroPagina.Should().NotBeNull();
            dto.NumeroPagina.Conteudo.Should().Be("250");
        }

        [Fact]
        public void DadoVolume_QuandoDefinirValor_EntaoVolumeEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Vol. 1" };

            dto.Volume = linhaConteudo;

            dto.Volume.Should().NotBeNull();
            dto.Volume.Conteudo.Should().Be("Vol. 1");
        }

        [Fact]
        public void DadoDescricao_QuandoDefinirValor_EntaoDescricaoEhAtribuida()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Descrição completa do documento" };

            dto.Descricao = linhaConteudo;

            dto.Descricao.Should().NotBeNull();
            dto.Descricao.Conteudo.Should().Be("Descrição completa do documento");
        }

        [Fact]
        public void DadoTipoAnexo_QuandoDefinirValor_EntaoTipoAnexoEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "PDF" };

            dto.TipoAnexo = linhaConteudo;

            dto.TipoAnexo.Should().NotBeNull();
            dto.TipoAnexo.Conteudo.Should().Be("PDF");
        }

        [Fact]
        public void DadoAltura_QuandoDefinirValor_EntaoAlturaEhAtribuida()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "29.7cm" };

            dto.Altura = linhaConteudo;

            dto.Altura.Should().NotBeNull();
            dto.Altura.Conteudo.Should().Be("29.7cm");
        }

        [Fact]
        public void DadoLargura_QuandoDefinirValor_EntaoLarguraEhAtribuida()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "21cm" };

            dto.Largura = linhaConteudo;

            dto.Largura.Should().NotBeNull();
            dto.Largura.Conteudo.Should().Be("21cm");
        }

        [Fact]
        public void DadoTamanhoArquivo_QuandoDefinirValor_EntaoTamanhoArquivoEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "5MB" };

            dto.TamanhoArquivo = linhaConteudo;

            dto.TamanhoArquivo.Should().NotBeNull();
            dto.TamanhoArquivo.Conteudo.Should().Be("5MB");
        }

        [Fact]
        public void DadoAcessoDocumentosIds_QuandoDefinirValor_EntaoAcessoDocumentosIdsEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1" };

            dto.AcessoDocumentosIds = linhaConteudo;

            dto.AcessoDocumentosIds.Should().NotBeNull();
            dto.AcessoDocumentosIds.Conteudo.Should().Be("1");
        }

        [Fact]
        public void DadoLocalizacao_QuandoDefinirValor_EntaoLocalizacaoEhAtribuida()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Sala 101" };

            dto.Localizacao = linhaConteudo;

            dto.Localizacao.Should().NotBeNull();
            dto.Localizacao.Conteudo.Should().Be("Sala 101");
        }

        [Fact]
        public void DadoCopiaDigital_QuandoDefinirValor_EntaoCopiaDigitalEhAtribuida()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Sim" };

            dto.CopiaDigital = linhaConteudo;

            dto.CopiaDigital.Should().NotBeNull();
            dto.CopiaDigital.Conteudo.Should().Be("Sim");
        }

        [Fact]
        public void DadoConservacaoId_QuandoDefinirValor_EntaoConservacaoIdEhAtribuido()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1" };

            dto.ConservacaoId = linhaConteudo;

            dto.ConservacaoId.Should().NotBeNull();
            dto.ConservacaoId.Conteudo.Should().Be("1");
        }

        #endregion

        #region Testes Integrados e Casos de Uso

        [Fact]
        public void DadoAcervoDocumentalLinhaRetornoCompleta_QuandoVerificarTodos_EntaoTodosOsCamposAcessiveis()
        {
            var dto = CriarAcervoDocumentalLinhaRetornoCompleto();

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.Mensagem.Should().BeEmpty();
            dto.NumeroLinha.Should().Be(5);
            dto.Titulo.Should().NotBeNull();
            dto.Codigo.Should().NotBeNull();
            dto.CodigoNovo.Should().NotBeNull();
            dto.MaterialId.Should().NotBeNull();
            dto.IdiomaId.Should().NotBeNull();
            dto.CreditosAutoresIds.Should().NotBeNull();
            dto.Ano.Should().NotBeNull();
            dto.NumeroPagina.Should().NotBeNull();
            dto.Volume.Should().NotBeNull();
            dto.Descricao.Should().NotBeNull();
            dto.TipoAnexo.Should().NotBeNull();
            dto.Altura.Should().NotBeNull();
            dto.Largura.Should().NotBeNull();
            dto.TamanhoArquivo.Should().NotBeNull();
            dto.AcessoDocumentosIds.Should().NotBeNull();
            dto.Localizacao.Should().NotBeNull();
            dto.CopiaDigital.Should().NotBeNull();
            dto.ConservacaoId.Should().NotBeNull();
        }

        [Fact]
        public void DadoLinhaRetornoSemErros_QuandoVerificar_EntaoTodosOsCamposSaoSemErros()
        {
            var dto = CriarAcervoDocumentalLinhaRetornoCompleto();

            dto.Titulo.PossuiErro.Should().BeFalse();
            dto.Codigo.PossuiErro.Should().BeFalse();
            dto.CodigoNovo.PossuiErro.Should().BeFalse();
            dto.MaterialId.PossuiErro.Should().BeFalse();
            dto.IdiomaId.PossuiErro.Should().BeFalse();
            dto.CreditosAutoresIds.PossuiErro.Should().BeFalse();
            dto.Ano.PossuiErro.Should().BeFalse();
            dto.NumeroPagina.PossuiErro.Should().BeFalse();
            dto.Volume.PossuiErro.Should().BeFalse();
            dto.Descricao.PossuiErro.Should().BeFalse();
            dto.TipoAnexo.PossuiErro.Should().BeFalse();
            dto.Altura.PossuiErro.Should().BeFalse();
            dto.Largura.PossuiErro.Should().BeFalse();
            dto.TamanhoArquivo.PossuiErro.Should().BeFalse();
            dto.AcessoDocumentosIds.PossuiErro.Should().BeFalse();
            dto.Localizacao.PossuiErro.Should().BeFalse();
            dto.CopiaDigital.PossuiErro.Should().BeFalse();
            dto.ConservacaoId.PossuiErro.Should().BeFalse();
        }

        [Fact]
        public void DadoLinhaRetornoComErros_QuandoDefinirPossuiErro_EntaoPossuiErroEhTrue()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                Titulo = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "Título",
                    PossuiErro = true,
                    Mensagem = "Campo obrigatório"
                }
            };

            dto.Titulo.PossuiErro.Should().BeTrue();
            dto.Titulo.Mensagem.Should().Be("Campo obrigatório");
        }

        [Fact]
        public void DadoMultiplasLinhasRetorno_QuandoProcessadas_EntaoCadaUmaTemEstadoIndependente()
        {
            var linha1 = new AcervoDocumentalLinhaRetornoDTO
            {
                NumeroLinha = 1,
                Status = ImportacaoStatus.Sucesso,
                Titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Título 1" }
            };
            var linha2 = new AcervoDocumentalLinhaRetornoDTO
            {
                NumeroLinha = 2,
                Status = ImportacaoStatus.Erros,
                Titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Título 2", PossuiErro = true }
            };

            linha1.Status.Should().Be(ImportacaoStatus.Sucesso);
            linha2.Status.Should().Be(ImportacaoStatus.Erros);
            linha1.Titulo.PossuiErro.Should().BeFalse();
            linha2.Titulo.PossuiErro.Should().BeTrue();
        }

        [Fact]
        public void DadoLinhaRetornoComConteudoVazio_QuandoVerificar_EntaoAceitaConteudoVazio()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                Titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = string.Empty }
            };

            dto.Titulo.Conteudo.Should().BeEmpty();
        }

        [Fact]
        public void DadoLinhaRetornoComCaracteresEspeciais_QuandoArmazenar_EntaoPreservaCaracteres()
        {
            var conteudoComEspeciais = "Título com ç, é, ñ e símbolos @#$%";
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                Titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = conteudoComEspeciais }
            };

            dto.Titulo.Conteudo.Should().Be(conteudoComEspeciais);
        }

        [Fact]
        public void DadoMaterialIdComId_QuandoDefinir_EntaoArmazenaIdCorretamente()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                MaterialId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "123" }
            };

            dto.MaterialId.Conteudo.Should().Be("123");
        }

        [Fact]
        public void DadoIdiomaIdComId_QuandoDefinir_EntaoArmazenaIdCorretamente()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                IdiomaId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "456" }
            };

            dto.IdiomaId.Conteudo.Should().Be("456");
        }

        [Fact]
        public void DadoCreditosAutoresIdsComMultiplos_QuandoDefinir_EntaoArmazenaMultiplosIds()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                CreditosAutoresIds = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1,2,3,4,5" }
            };

            dto.CreditosAutoresIds.Conteudo.Should().Be("1,2,3,4,5");
        }

        [Fact]
        public void DadoAcessoDocumentosIdsComId_QuandoDefinir_EntaoArmazenaIdCorretamente()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                AcessoDocumentosIds = new LinhaConteudoAjustarRetornoDTO { Conteudo = "789" }
            };

            dto.AcessoDocumentosIds.Conteudo.Should().Be("789");
        }

        [Fact]
        public void DadoConservacaoIdComId_QuandoDefinir_EntaoArmazenaIdCorretamente()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                ConservacaoId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "2" }
            };

            dto.ConservacaoId.Conteudo.Should().Be("2");
        }

        [Fact]
        public void DadoLinhaRetornoComErrosCampos_QuandoDefinir_EntaoErrosCamposEhAtribuido()
        {
            var erros = new[] { "Título", "Código", "MaterialId" };
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                ErrosCampos = erros
            };

            dto.ErrosCampos.Should().BeEquivalentTo(erros);
            dto.ErrosCampos.Should().HaveCount(3);
        }

        [Fact]
        public void DadoLinhaRetornoComErrosCamposVazio_QuandoDefinir_EntaoErrosCamposEhVazio()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                ErrosCampos = new string[] { }
            };

            dto.ErrosCampos.Should().BeEmpty();
        }

        [Fact]
        public void DadoTituloComMensagemDeErro_QuandoDefinir_EntaoMensagemEhArmazenada()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                Titulo = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "",
                    PossuiErro = true,
                    Mensagem = "Campo Título é obrigatório"
                }
            };

            dto.Titulo.Mensagem.Should().Be("Campo Título é obrigatório");
        }

        [Fact]
        public void DadoCodigoComMensagemDeErro_QuandoDefinir_EntaoMensagemEhArmazenada()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                Codigo = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "",
                    PossuiErro = true,
                    Mensagem = "Código já existe no sistema"
                }
            };

            dto.Codigo.Mensagem.Should().Be("Código já existe no sistema");
        }

        [Fact]
        public void DadoMaterialIdComMensagemDeErro_QuandoDefinir_EntaoMensagemEhArmazenada()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                MaterialId = new LinhaConteudoAjustarRetornoDTO
                {
                    Conteudo = "999",
                    PossuiErro = true,
                    Mensagem = "Material não encontrado"
                }
            };

            dto.MaterialId.Mensagem.Should().Be("Material não encontrado");
        }

        [Fact]
        public void DadoConteudoLongo_QuandoArmazenarEmLinhaRetorno_EntaoArmazenaCompleto()
        {
            var conteudoLongo = new string('x', 5000);
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                Descricao = new LinhaConteudoAjustarRetornoDTO { Conteudo = conteudoLongo }
            };

            dto.Descricao.Conteudo.Length.Should().Be(5000);
            dto.Descricao.Conteudo.Should().Be(conteudoLongo);
        }

        [Fact]
        public void DadoNumeroLinhaComValorMaximo_QuandoAtribuir_EntaoArmazenaCorretamente()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                NumeroLinha = int.MaxValue
            };

            dto.NumeroLinha.Should().Be(int.MaxValue);
        }

        [Fact]
        public void DadoPropriedadesHerdadas_QuandoAcessar_EntaoPropriedadesBaseSaoAcessadas()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                Status = ImportacaoStatus.Sucesso,
                NumeroLinha = 10,
                Mensagem = "Sucesso",
                ErrosCampos = new[] { "Campo1" }
            };

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.NumeroLinha.Should().Be(10);
            dto.Mensagem.Should().Be("Sucesso");
            dto.ErrosCampos.Should().Contain("Campo1");
        }

        [Fact]
        public void DadoLinhaRetornoCompleta_QuandoAcessarTodasAsPropriedades_EntaoNenhumaLancaExcecao()
        {
            var exception = Record.Exception(() =>
            {
                var dto = CriarAcervoDocumentalLinhaRetornoCompleto();

                _ = dto.Status;
                _ = dto.NumeroLinha;
                _ = dto.Mensagem;
                _ = dto.ErrosCampos;
                _ = dto.Titulo;
                _ = dto.Codigo;
                _ = dto.CodigoNovo;
                _ = dto.MaterialId;
                _ = dto.IdiomaId;
                _ = dto.CreditosAutoresIds;
                _ = dto.Ano;
                _ = dto.NumeroPagina;
                _ = dto.Volume;
                _ = dto.Descricao;
                _ = dto.TipoAnexo;
                _ = dto.Altura;
                _ = dto.Largura;
                _ = dto.TamanhoArquivo;
                _ = dto.AcessoDocumentosIds;
                _ = dto.Localizacao;
                _ = dto.CopiaDigital;
                _ = dto.ConservacaoId;
            });

            exception.Should().BeNull();
        }

        [Fact]
        public void DadoModificarPropriedadesVariasVezes_QuandoAcessar_EntaoUltimoValorEhPreservado()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO();

            dto.Titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Título 1" };
            dto.Titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Título 2" };
            dto.Titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Título Final" };

            dto.Titulo.Conteudo.Should().Be("Título Final");
        }

        [Fact]
        public void DadoDuasInstanciasDistintas_QuandoCriar_EntaoSaoIndependentes()
        {
            var dto1 = new AcervoDocumentalLinhaRetornoDTO
            {
                NumeroLinha = 1,
                Titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Título 1" }
            };
            var dto2 = new AcervoDocumentalLinhaRetornoDTO
            {
                NumeroLinha = 2,
                Titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Título 2" }
            };

            dto1.NumeroLinha.Should().Be(1);
            dto2.NumeroLinha.Should().Be(2);
            dto1.Titulo.Conteudo.Should().Be("Título 1");
            dto2.Titulo.Conteudo.Should().Be("Título 2");
        }

        [Fact]
        public void DadoLinhaRetornoCompleta_QuandoCompararValores_EntaoTodosOsValoresSaoIguais()
        {
            var dto1 = CriarAcervoDocumentalLinhaRetornoCompleto();
            var dto2 = CriarAcervoDocumentalLinhaRetornoCompleto();

            dto1.Status.Should().Be(dto2.Status);
            dto1.NumeroLinha.Should().Be(dto2.NumeroLinha);
            dto1.Titulo.Conteudo.Should().Be(dto2.Titulo.Conteudo);
        }

        [Fact]
        public void DadoIdsNumericosDistintos_QuandoDefinirEmPropriedades_EntaoSaoArmazenadosCorretamente()
        {
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                MaterialId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1" },
                IdiomaId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "2" },
                ConservacaoId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "3" }
            };

            dto.MaterialId.Conteudo.Should().Be("1");
            dto.IdiomaId.Conteudo.Should().Be("2");
            dto.ConservacaoId.Conteudo.Should().Be("3");
        }

        [Fact]
        public void DadoCreditosAutoresIdsComComaSeparado_QuandoDefinir_EntaoPreservaFormatacao()
        {
            var creditosIds = "10,20,30,40,50";
            var dto = new AcervoDocumentalLinhaRetornoDTO
            {
                CreditosAutoresIds = new LinhaConteudoAjustarRetornoDTO { Conteudo = creditosIds }
            };

            dto.CreditosAutoresIds.Conteudo.Should().Be(creditosIds);
            dto.CreditosAutoresIds.Conteudo.Should().Contain(",");
        }

        [Fact]
        public void DadoStatusComTodosOsValoresEnum_QuandoAtribuir_EntaoTodosArmazenam()
        {
            var dto1 = new AcervoDocumentalLinhaRetornoDTO { Status = ImportacaoStatus.Sucesso };
            var dto2 = new AcervoDocumentalLinhaRetornoDTO { Status = ImportacaoStatus.Erros };
            var dto3 = new AcervoDocumentalLinhaRetornoDTO { Status = ImportacaoStatus.Pendente };

            dto1.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto2.Status.Should().Be(ImportacaoStatus.Erros);
            dto3.Status.Should().Be(ImportacaoStatus.Pendente);
        }

        [Fact]
        public void DadoErrosCamposComMultiplosNomes_QuandoDefinir_EntaoTodosArmazenam()
        {
            var errosCampos = new[] { "Titulo", "Codigo", "MaterialId", "IdiomaId", "ConservacaoId" };
            var dto = new AcervoDocumentalLinhaRetornoDTO { ErrosCampos = errosCampos };

            dto.ErrosCampos.Should().HaveCount(5);
            dto.ErrosCampos.Should().Contain("Titulo");
            dto.ErrosCampos.Should().Contain("Codigo");
        }

        #endregion
    }
}
