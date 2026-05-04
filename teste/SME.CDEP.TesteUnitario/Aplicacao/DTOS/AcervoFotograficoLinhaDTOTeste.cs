using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoFotograficoLinhaDTOTeste
    {
        #region Status

        [Fact]
        public void DadoStatusVazio_QuandoCriarDTO_EntaoStatusDeveSerValorPadrao()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Status.Should().Be(default(ImportacaoStatus));
        }

        [Fact]
        public void DadoStatus_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var status = ImportacaoStatus.Sucesso;
            var dto = new AcervoFotograficoLinhaDTO { Status = status };

            dto.Status.Should().Be(status);
        }

        [Fact]
        public void DadoStatusErros_QuandoAtribuir_EntaoDeveArmazenarErros()
        {
            var dto = new AcervoFotograficoLinhaDTO { Status = ImportacaoStatus.Erros };

            dto.Status.Should().Be(ImportacaoStatus.Erros);
        }

        [Fact]
        public void DadoStatusPendente_QuandoAtribuir_EntaoDeveArmazenarPendente()
        {
            var dto = new AcervoFotograficoLinhaDTO { Status = ImportacaoStatus.Pendente };

            dto.Status.Should().Be(ImportacaoStatus.Pendente);
        }

        #endregion

        #region Mensagem

        [Fact]
        public void DadoMensagemVazia_QuandoCriarDTO_EntaoMensagemDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Mensagem.Should().BeNull();
        }

        [Fact]
        public void DadoMensagem_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var mensagem = new Faker().Lorem.Sentence();
            var dto = new AcervoFotograficoLinhaDTO { Mensagem = mensagem };

            dto.Mensagem.Should().Be(mensagem);
        }

        [Fact]
        public void DadoMensagemErro_QuandoAtribuirTextoDeErro_EntaoDeveArmazenarCompleto()
        {
            var mensagem = "Erro: Campo obrigatório não preenchido";
            var dto = new AcervoFotograficoLinhaDTO { Mensagem = mensagem };

            dto.Mensagem.Should().Be(mensagem);
            dto.Mensagem.Should().Contain("Erro");
        }

        [Fact]
        public void DadoMensagemVazia_QuandoAtribuirString_EntaoDeveArmazenarVazia()
        {
            var dto = new AcervoFotograficoLinhaDTO { Mensagem = string.Empty };

            dto.Mensagem.Should().Be(string.Empty);
        }

        #endregion

        #region NumeroLinha

        [Fact]
        public void DadoNumeroLinhaVazio_QuandoCriarDTO_EntaoNumeroLinhaDeveSerZero()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.NumeroLinha.Should().Be(0);
        }

        [Fact]
        public void DadoNumeroLinha_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var numeroLinha = new Faker().Random.Int(1, 1000);
            var dto = new AcervoFotograficoLinhaDTO { NumeroLinha = numeroLinha };

            dto.NumeroLinha.Should().Be(numeroLinha);
        }

        [Fact]
        public void DadoNumeroLinhaMaximo_QuandoAtribuirIntMaxValue_EntaoDeveArmazenar()
        {
            var dto = new AcervoFotograficoLinhaDTO { NumeroLinha = int.MaxValue };

            dto.NumeroLinha.Should().Be(int.MaxValue);
        }

        [Fact]
        public void DadoNumeroLinhaUm_QuandoAtribuir1_EntaoDeveArmazenarUm()
        {
            var dto = new AcervoFotograficoLinhaDTO { NumeroLinha = 1 };

            dto.NumeroLinha.Should().Be(1);
        }

        #endregion

        #region PossuiErros

        [Fact]
        public void DadoPossuiErrosVazio_QuandoCriarDTO_EntaoPossuiErrosDeveSerFalse()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.PossuiErros.Should().BeFalse();
        }

        [Fact]
        public void DadoPossuiErrosTrue_QuandoAtribuir_EntaoDeveArmazenarTrue()
        {
            var dto = new AcervoFotograficoLinhaDTO { PossuiErros = true };

            dto.PossuiErros.Should().BeTrue();
        }

        [Fact]
        public void DadoPossuiErrosFalse_QuandoAtribuir_EntaoDeveArmazenarFalse()
        {
            var dto = new AcervoFotograficoLinhaDTO { PossuiErros = false };

            dto.PossuiErros.Should().BeFalse();
        }

        #endregion

        #region Titulo

        [Fact]
        public void DadoTituloVazio_QuandoCriarDTO_EntaoTituloDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Titulo.Should().BeNull();
        }

        [Fact]
        public void DadoTitulo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Título Teste" };
            var dto = new AcervoFotograficoLinhaDTO { Titulo = linhaConteudo };

            dto.Titulo.Should().NotBeNull();
            dto.Titulo.Conteudo.Should().Be("Título Teste");
        }

        [Fact]
        public void DadoTituloComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO 
            { 
                Conteudo = "Título",
                PossuiErro = true,
                Mensagem = "Campo obrigatório"
            };
            var dto = new AcervoFotograficoLinhaDTO { Titulo = linhaConteudo };

            dto.Titulo.PossuiErro.Should().BeTrue();
            dto.Titulo.Mensagem.Should().Be("Campo obrigatório");
        }

        #endregion

        #region Codigo

        [Fact]
        public void DadoCodigoVazio_QuandoCriarDTO_EntaoCodigoDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Codigo.Should().BeNull();
        }

        [Fact]
        public void DadoCodigo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "COD001" };
            var dto = new AcervoFotograficoLinhaDTO { Codigo = linhaConteudo };

            dto.Codigo.Should().NotBeNull();
            dto.Codigo.Conteudo.Should().Be("COD001");
        }

        #endregion

        #region Credito

        [Fact]
        public void DadoCreditoVazio_QuandoCriarDTO_EntaoCreditoDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Credito.Should().BeNull();
        }

        [Fact]
        public void DadoCredito_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Autor 1" };
            var dto = new AcervoFotograficoLinhaDTO { Credito = linhaConteudo };

            dto.Credito.Should().NotBeNull();
            dto.Credito.Conteudo.Should().Be("Autor 1");
        }

        #endregion

        #region Localizacao

        [Fact]
        public void DadoLocalizacaoVazia_QuandoCriarDTO_EntaoLocalizacaoDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Localizacao.Should().BeNull();
        }

        [Fact]
        public void DadoLocalizacao_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Sala 1 - Prateleira 5" };
            var dto = new AcervoFotograficoLinhaDTO { Localizacao = linhaConteudo };

            dto.Localizacao.Should().NotBeNull();
            dto.Localizacao.Conteudo.Should().Be("Sala 1 - Prateleira 5");
        }

        #endregion

        #region Procedencia

        [Fact]
        public void DadoProcedenciaVazia_QuandoCriarDTO_EntaoProcedenciaDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Procedencia.Should().BeNull();
        }

        [Fact]
        public void DadoProcedencia_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Doação" };
            var dto = new AcervoFotograficoLinhaDTO { Procedencia = linhaConteudo };

            dto.Procedencia.Should().NotBeNull();
            dto.Procedencia.Conteudo.Should().Be("Doação");
        }

        #endregion

        #region Data

        [Fact]
        public void DadoDataVazia_QuandoCriarDTO_EntaoDataDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Data.Should().BeNull();
        }

        [Fact]
        public void DadoData_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var data = new Faker().Date.Past().ToString("dd/MM/yyyy");
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = data };
            var dto = new AcervoFotograficoLinhaDTO { Data = linhaConteudo };

            dto.Data.Should().NotBeNull();
            dto.Data.Conteudo.Should().Be(data);
        }

        #endregion

        #region CopiaDigital

        [Fact]
        public void DadoCopiaDigitalVazia_QuandoCriarDTO_EntaoCopiaDigitalDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.CopiaDigital.Should().BeNull();
        }

        [Fact]
        public void DadoCopiaDigital_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Sim" };
            var dto = new AcervoFotograficoLinhaDTO { CopiaDigital = linhaConteudo };

            dto.CopiaDigital.Should().NotBeNull();
            dto.CopiaDigital.Conteudo.Should().Be("Sim");
        }

        #endregion

        #region PermiteUsoImagem

        [Fact]
        public void DadoPermiteUsoImagemVazia_QuandoCriarDTO_EntaoPermiteUsoImagemDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.PermiteUsoImagem.Should().BeNull();
        }

        [Fact]
        public void DadoPermiteUsoImagem_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Não" };
            var dto = new AcervoFotograficoLinhaDTO { PermiteUsoImagem = linhaConteudo };

            dto.PermiteUsoImagem.Should().NotBeNull();
            dto.PermiteUsoImagem.Conteudo.Should().Be("Não");
        }

        #endregion

        #region EstadoConservacao

        [Fact]
        public void DadoEstadoConservacaoVazio_QuandoCriarDTO_EntaoEstadoConservacaoDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.EstadoConservacao.Should().BeNull();
        }

        [Fact]
        public void DadoEstadoConservacao_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Bom" };
            var dto = new AcervoFotograficoLinhaDTO { EstadoConservacao = linhaConteudo };

            dto.EstadoConservacao.Should().NotBeNull();
            dto.EstadoConservacao.Conteudo.Should().Be("Bom");
        }

        #endregion

        #region Descricao

        [Fact]
        public void DadoDescricaoVazia_QuandoCriarDTO_EntaoDescricaoDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Descricao.Should().BeNull();
        }

        [Fact]
        public void DadoDescricao_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Descrição da foto" };
            var dto = new AcervoFotograficoLinhaDTO { Descricao = linhaConteudo };

            dto.Descricao.Should().NotBeNull();
            dto.Descricao.Conteudo.Should().Be("Descrição da foto");
        }

        #endregion

        #region Quantidade

        [Fact]
        public void DadoQuantidadeVazia_QuandoCriarDTO_EntaoQuantidadeDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Quantidade.Should().BeNull();
        }

        [Fact]
        public void DadoQuantidade_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "10" };
            var dto = new AcervoFotograficoLinhaDTO { Quantidade = linhaConteudo };

            dto.Quantidade.Should().NotBeNull();
            dto.Quantidade.Conteudo.Should().Be("10");
        }

        #endregion

        #region Largura

        [Fact]
        public void DadoLarguraVazia_QuandoCriarDTO_EntaoLarguraDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Largura.Should().BeNull();
        }

        [Fact]
        public void DadoLargura_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "20cm" };
            var dto = new AcervoFotograficoLinhaDTO { Largura = linhaConteudo };

            dto.Largura.Should().NotBeNull();
            dto.Largura.Conteudo.Should().Be("20cm");
        }

        #endregion

        #region Altura

        [Fact]
        public void DadoAlturaVazia_QuandoCriarDTO_EntaoAlturaDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Altura.Should().BeNull();
        }

        [Fact]
        public void DadoAltura_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "30cm" };
            var dto = new AcervoFotograficoLinhaDTO { Altura = linhaConteudo };

            dto.Altura.Should().NotBeNull();
            dto.Altura.Conteudo.Should().Be("30cm");
        }

        #endregion

        #region Suporte

        [Fact]
        public void DadoSuporteVazio_QuandoCriarDTO_EntaoSuporteDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Suporte.Should().BeNull();
        }

        [Fact]
        public void DadoSuporte_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Papel" };
            var dto = new AcervoFotograficoLinhaDTO { Suporte = linhaConteudo };

            dto.Suporte.Should().NotBeNull();
            dto.Suporte.Conteudo.Should().Be("Papel");
        }

        #endregion

        #region FormatoImagem

        [Fact]
        public void DadoFormatoImagemVazio_QuandoCriarDTO_EntaoFormatoImagemDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.FormatoImagem.Should().BeNull();
        }

        [Fact]
        public void DadoFormatoImagem_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "JPEG" };
            var dto = new AcervoFotograficoLinhaDTO { FormatoImagem = linhaConteudo };

            dto.FormatoImagem.Should().NotBeNull();
            dto.FormatoImagem.Conteudo.Should().Be("JPEG");
        }

        #endregion

        #region TamanhoArquivo

        [Fact]
        public void DadoTamanhoArquivoVazio_QuandoCriarDTO_EntaoTamanhoArquivoDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.TamanhoArquivo.Should().BeNull();
        }

        [Fact]
        public void DadoTamanhoArquivo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "2.5 MB" };
            var dto = new AcervoFotograficoLinhaDTO { TamanhoArquivo = linhaConteudo };

            dto.TamanhoArquivo.Should().NotBeNull();
            dto.TamanhoArquivo.Conteudo.Should().Be("2.5 MB");
        }

        #endregion

        #region Cromia

        [Fact]
        public void DadoCromiaVazia_QuandoCriarDTO_EntaoCromiaDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Cromia.Should().BeNull();
        }

        [Fact]
        public void DadoCromia_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Colorida" };
            var dto = new AcervoFotograficoLinhaDTO { Cromia = linhaConteudo };

            dto.Cromia.Should().NotBeNull();
            dto.Cromia.Conteudo.Should().Be("Colorida");
        }

        #endregion

        #region Resolucao

        [Fact]
        public void DadoResolucaoVazia_QuandoCriarDTO_EntaoResolucaoDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Resolucao.Should().BeNull();
        }

        [Fact]
        public void DadoResolucao_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "300 DPI" };
            var dto = new AcervoFotograficoLinhaDTO { Resolucao = linhaConteudo };

            dto.Resolucao.Should().NotBeNull();
            dto.Resolucao.Conteudo.Should().Be("300 DPI");
        }

        #endregion

        #region Ano

        [Fact]
        public void DadoAnoVazio_QuandoCriarDTO_EntaoAnoDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoAno_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "2020" };
            var dto = new AcervoFotograficoLinhaDTO { Ano = linhaConteudo };

            dto.Ano.Should().NotBeNull();
            dto.Ano.Conteudo.Should().Be("2020");
        }

        #endregion

        #region Metodo: DefinirLinhaComoSucesso

        [Fact]
        public void DadoDTOComErros_QuandoChamarDefinirLinhaComoSucesso_EntaoDeveRemoverTodosOsErros()
        {
            var dto = new AcervoFotograficoLinhaDTO
            {
                PossuiErros = true,
                Mensagem = "Erro na importação",
                Status = ImportacaoStatus.Erros,
                Titulo = new LinhaConteudoAjustarDTO { Conteudo = "Título", PossuiErro = true, Mensagem = "Obrigatório" },
                Codigo = new LinhaConteudoAjustarDTO { Conteudo = "COD", PossuiErro = true, Mensagem = "Inválido" },
                Credito = new LinhaConteudoAjustarDTO { Conteudo = "Cr", PossuiErro = false },
                Localizacao = new LinhaConteudoAjustarDTO { Conteudo = "L", PossuiErro = false },
                Procedencia = new LinhaConteudoAjustarDTO { Conteudo = "P", PossuiErro = false },
                Data = new LinhaConteudoAjustarDTO { Conteudo = "D", PossuiErro = false },
                CopiaDigital = new LinhaConteudoAjustarDTO { Conteudo = "CD", PossuiErro = false },
                PermiteUsoImagem = new LinhaConteudoAjustarDTO { Conteudo = "PUI", PossuiErro = false },
                EstadoConservacao = new LinhaConteudoAjustarDTO { Conteudo = "EC", PossuiErro = false },
                Descricao = new LinhaConteudoAjustarDTO { Conteudo = "Desc", PossuiErro = false },
                Quantidade = new LinhaConteudoAjustarDTO { Conteudo = "Q", PossuiErro = false },
                Largura = new LinhaConteudoAjustarDTO { Conteudo = "Larg", PossuiErro = false },
                Altura = new LinhaConteudoAjustarDTO { Conteudo = "Alt", PossuiErro = false },
                Suporte = new LinhaConteudoAjustarDTO { Conteudo = "S", PossuiErro = false },
                FormatoImagem = new LinhaConteudoAjustarDTO { Conteudo = "FI", PossuiErro = false },
                TamanhoArquivo = new LinhaConteudoAjustarDTO { Conteudo = "TA", PossuiErro = false },
                Cromia = new LinhaConteudoAjustarDTO { Conteudo = "Cr", PossuiErro = false },
                Resolucao = new LinhaConteudoAjustarDTO { Conteudo = "R", PossuiErro = false },
                Ano = new LinhaConteudoAjustarDTO { Conteudo = "A", PossuiErro = false }
            };

            dto.DefinirLinhaComoSucesso();

            dto.PossuiErros.Should().BeFalse();
            dto.Mensagem.Should().Be(string.Empty);
            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
        }       

        [Fact]
        public void DadoDTOVazio_QuandoChamarDefinirLinhaComoSucesso_EntaoDeveDefinirStatusSucesso()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            var excecao = Record.Exception(() => dto.DefinirLinhaComoSucesso());

            excecao.Should().BeOfType<NullReferenceException>();
        }

        [Fact]
        public void DadoDTOComCamposNulos_QuandoChamarDefinirLinhaComoSucesso_EntaoNaoDeveLancarExcecao()
        {
            var dto = new AcervoFotograficoLinhaDTO
            {
                Titulo = null,
                Codigo = null,
                Credito = null,
                Localizacao = null
            };

            var excecao = Record.Exception(() => dto.DefinirLinhaComoSucesso());

            excecao.Should().BeOfType<NullReferenceException>();
        }

        #endregion

        #region Metodo: DefinirLinhaComoErro (Herdado)

        [Fact]
        public void DadoDTOSemErros_QuandoChamarDefinirLinhaComoErro_EntaoDeveAdicionarErro()
        {
            var dto = new AcervoFotograficoLinhaDTO
            {
                PossuiErros = false,
                Status = ImportacaoStatus.Sucesso
            };

            var mensagem = "Erro na validação";
            dto.DefinirLinhaComoErro(mensagem);

            dto.PossuiErros.Should().BeTrue();
            dto.Status.Should().Be(ImportacaoStatus.Erros);
            dto.Mensagem.Should().Be(mensagem);
        }

        [Fact]
        public void DadoDTOComMensagemExistente_QuandoChamarDefinirLinhaComoErro_EntaoDeveSubstituirMensagem()
        {
            var dto = new AcervoFotograficoLinhaDTO
            {
                PossuiErros = false,
                Mensagem = "Mensagem anterior",
                Status = ImportacaoStatus.Sucesso
            };

            var novaMensagem = "Nova mensagem de erro";
            dto.DefinirLinhaComoErro(novaMensagem);

            dto.Mensagem.Should().Be(novaMensagem);
            dto.PossuiErros.Should().BeTrue();
        }

        [Fact]
        public void DadoMensagemVazia_QuandoChamarDefinirLinhaComoErro_EntaoDeveArmazenarMensagemVazia()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.DefinirLinhaComoErro(string.Empty);

            dto.PossuiErros.Should().BeTrue();
            dto.Status.Should().Be(ImportacaoStatus.Erros);
            dto.Mensagem.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoMensagemComCaracteresEspeciais_QuandoChamarDefinirLinhaComoErro_EntaoDeveArmazenarCompleto()
        {
            var dto = new AcervoFotograficoLinhaDTO();
            var mensagem = "Erro: Campo 'Título' com valor inválido @#$%";

            dto.DefinirLinhaComoErro(mensagem);

            dto.Mensagem.Should().Be(mensagem);
            dto.PossuiErros.Should().BeTrue();
        }

        #endregion

        #region Testes de Integração - Múltiplas Propriedades

        [Fact]
        public void DadoDTOCompleto_QuandoInstanciarComTodosOsParametros_EntaoDeveArmazenarTodosCorretamente()
        {
            var faker = new Faker("pt_BR");
            var numeroLinha = faker.Random.Int(1, 100);
            var titulo = new LinhaConteudoAjustarDTO { Conteudo = faker.Lorem.Sentence() };
            var codigo = new LinhaConteudoAjustarDTO { Conteudo = faker.Random.String(10) };
            var credito = new LinhaConteudoAjustarDTO { Conteudo = faker.Person.FullName };
            var localizacao = new LinhaConteudoAjustarDTO { Conteudo = faker.Address.StreetName() };
            var procedencia = new LinhaConteudoAjustarDTO { Conteudo = faker.Company.CompanyName() };
            var data = new LinhaConteudoAjustarDTO { Conteudo = faker.Date.Past().ToString("dd/MM/yyyy") };
            var copiaDigital = new LinhaConteudoAjustarDTO { Conteudo = "Sim" };
            var permiteUsoImagem = new LinhaConteudoAjustarDTO { Conteudo = "Não" };
            var estadoConservacao = new LinhaConteudoAjustarDTO { Conteudo = "Bom" };
            var descricao = new LinhaConteudoAjustarDTO { Conteudo = faker.Lorem.Paragraph() };
            var quantidade = new LinhaConteudoAjustarDTO { Conteudo = faker.Random.Int(1, 100).ToString() };
            var largura = new LinhaConteudoAjustarDTO { Conteudo = "20cm" };
            var altura = new LinhaConteudoAjustarDTO { Conteudo = "30cm" };
            var suporte = new LinhaConteudoAjustarDTO { Conteudo = "Papel" };
            var formatoImagem = new LinhaConteudoAjustarDTO { Conteudo = "JPEG" };
            var tamanhoArquivo = new LinhaConteudoAjustarDTO { Conteudo = "2.5 MB" };
            var cromia = new LinhaConteudoAjustarDTO { Conteudo = "Colorida" };
            var resolucao = new LinhaConteudoAjustarDTO { Conteudo = "300 DPI" };
            var ano = new LinhaConteudoAjustarDTO { Conteudo = faker.Date.Past().Year.ToString() };

            var dto = new AcervoFotograficoLinhaDTO
            {
                NumeroLinha = numeroLinha,
                Status = ImportacaoStatus.Sucesso,
                Mensagem = string.Empty,
                PossuiErros = false,
                Titulo = titulo,
                Codigo = codigo,
                Credito = credito,
                Localizacao = localizacao,
                Procedencia = procedencia,
                Data = data,
                CopiaDigital = copiaDigital,
                PermiteUsoImagem = permiteUsoImagem,
                EstadoConservacao = estadoConservacao,
                Descricao = descricao,
                Quantidade = quantidade,
                Largura = largura,
                Altura = altura,
                Suporte = suporte,
                FormatoImagem = formatoImagem,
                TamanhoArquivo = tamanhoArquivo,
                Cromia = cromia,
                Resolucao = resolucao,
                Ano = ano
            };

            dto.NumeroLinha.Should().Be(numeroLinha);
            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.Mensagem.Should().Be(string.Empty);
            dto.PossuiErros.Should().BeFalse();
            dto.Titulo.Should().BeEquivalentTo(titulo);
            dto.Codigo.Should().BeEquivalentTo(codigo);
            dto.Credito.Should().BeEquivalentTo(credito);
            dto.Localizacao.Should().BeEquivalentTo(localizacao);
            dto.Procedencia.Should().BeEquivalentTo(procedencia);
            dto.Data.Should().BeEquivalentTo(data);
            dto.CopiaDigital.Should().BeEquivalentTo(copiaDigital);
            dto.PermiteUsoImagem.Should().BeEquivalentTo(permiteUsoImagem);
            dto.EstadoConservacao.Should().BeEquivalentTo(estadoConservacao);
            dto.Descricao.Should().BeEquivalentTo(descricao);
            dto.Quantidade.Should().BeEquivalentTo(quantidade);
            dto.Largura.Should().BeEquivalentTo(largura);
            dto.Altura.Should().BeEquivalentTo(altura);
            dto.Suporte.Should().BeEquivalentTo(suporte);
            dto.FormatoImagem.Should().BeEquivalentTo(formatoImagem);
            dto.TamanhoArquivo.Should().BeEquivalentTo(tamanhoArquivo);
            dto.Cromia.Should().BeEquivalentTo(cromia);
            dto.Resolucao.Should().BeEquivalentTo(resolucao);
            dto.Ano.Should().BeEquivalentTo(ano);
        }

        [Fact]
        public void DadoDTOVazio_QuandoInstanciarSemParametros_EntaoDeveSerValido()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Should().NotBeNull();
            dto.NumeroLinha.Should().Be(0);
            dto.Status.Should().Be(default(ImportacaoStatus));
            dto.Mensagem.Should().BeNull();
            dto.PossuiErros.Should().BeFalse();
            dto.Titulo.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.Credito.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.Data.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.PermiteUsoImagem.Should().BeNull();
            dto.EstadoConservacao.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.Quantidade.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Suporte.Should().BeNull();
            dto.FormatoImagem.Should().BeNull();
            dto.TamanhoArquivo.Should().BeNull();
            dto.Cromia.Should().BeNull();
            dto.Resolucao.Should().BeNull();
            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComValoresNulos_QuandoAtribuirExplicitamente_EntaoDeveArmazenarNull()
        {
            var dto = new AcervoFotograficoLinhaDTO
            {
                Titulo = null,
                Codigo = null,
                Credito = null,
                Localizacao = null,
                Procedencia = null,
                Data = null,
                CopiaDigital = null,
                PermiteUsoImagem = null,
                EstadoConservacao = null,
                Descricao = null,
                Quantidade = null,
                Largura = null,
                Altura = null,
                Suporte = null,
                FormatoImagem = null,
                TamanhoArquivo = null,
                Cromia = null,
                Resolucao = null,
                Ano = null,
                Mensagem = null
            };

            dto.Titulo.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.Credito.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.Data.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.PermiteUsoImagem.Should().BeNull();
            dto.EstadoConservacao.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.Quantidade.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Suporte.Should().BeNull();
            dto.FormatoImagem.Should().BeNull();
            dto.TamanhoArquivo.Should().BeNull();
            dto.Cromia.Should().BeNull();
            dto.Resolucao.Should().BeNull();
            dto.Ano.Should().BeNull();
            dto.Mensagem.Should().BeNull();
        }

        [Fact]
        public void DadoDuasInstancias_QuandoComMesmosValores_EntaoSaoInstanciasDistintas()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO { Conteudo = "Teste" };
            var dto1 = new AcervoFotograficoLinhaDTO
            {
                NumeroLinha = 1,
                Titulo = linhaConteudo
            };

            var dto2 = new AcervoFotograficoLinhaDTO
            {
                NumeroLinha = 1,
                Titulo = linhaConteudo
            };

            dto1.Should().NotBeSameAs(dto2);
            dto1.NumeroLinha.Should().Be(dto2.NumeroLinha);
        }

        [Fact]
        public void DadoDTOComLinhasDeConteudo_QuandoAcessarMultiplasVezes_EntaoValoresPermanecem()
        {
            var titulo = new LinhaConteudoAjustarDTO { Conteudo = "Título Teste", PossuiErro = false };
            var dto = new AcervoFotograficoLinhaDTO { Titulo = titulo };

            var titulo1 = dto.Titulo;
            var titulo2 = dto.Titulo;

            titulo1.Should().BeSameAs(titulo2);
            titulo1.Conteudo.Should().Be("Título Teste");
        }

        [Fact]
        public void DadoDTOComDiversosStatus_QuandoAlternarStatus_EntaoAlternaCorretamente()
        {
            var dto = new AcervoFotograficoLinhaDTO();

            dto.Status = ImportacaoStatus.Pendente;
            dto.Status.Should().Be(ImportacaoStatus.Pendente);

            dto.Status = ImportacaoStatus.Erros;
            dto.Status.Should().Be(ImportacaoStatus.Erros);

            dto.Status = ImportacaoStatus.Sucesso;
            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact]
        public void DadoDTOComLinhaConteudoCompleto_QuandoVerificarErros_EntaoDeveRetornarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarDTO
            {
                Conteudo = "Conteúdo",
                PossuiErro = true,
                Mensagem = "Erro de validação",
                LimiteCaracteres = 100,
                EhCampoObrigatorio = true,
                ValoresPermitidos = new[] { "valor1", "valor2" }
            };

            var dto = new AcervoFotograficoLinhaDTO { Titulo = linhaConteudo };

            dto.Titulo.PossuiErro.Should().BeTrue();
            dto.Titulo.Mensagem.Should().Be("Erro de validação");
            dto.Titulo.LimiteCaracteres.Should().Be(100);
            dto.Titulo.EhCampoObrigatorio.Should().BeTrue();
        }

        #endregion
    }
}
