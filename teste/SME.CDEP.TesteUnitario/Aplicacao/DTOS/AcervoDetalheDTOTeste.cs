using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoDetalheDtoTeste
    {
        private AcervoDetalheDTO CriarAcervoDetalheDTO()
        {
            return new AcervoDetalheDTO
            {
                Titulo = "Título do Acervo",
                Codigo = "COD001",
                Ano = "2024",
                AcervoId = 1,
                EnderecoImagemPadrao = "https://exemplo.com/imagem.jpg",
                SituacaoDisponibilidade = "Disponível",
                EstaDisponivel = true,
                TemControleDisponibilidade = true,
                TipoAcervoId = 1
            };
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoInstanciar_EntaoTituloEhNulavel()
        {
            var dto = new AcervoDetalheDTO();

            dto.Titulo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoInstanciar_EntaoCodigoEhNulavel()
        {
            var dto = new AcervoDetalheDTO();

            dto.Codigo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoInstanciar_EntaoAnoEhNulavel()
        {
            var dto = new AcervoDetalheDTO();

            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoInstanciar_EntaoAcervoIdTemValorPadrao()
        {
            var dto = new AcervoDetalheDTO();

            dto.AcervoId.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoInstanciar_EntaoEnderecoImagemPadraoEhNulavel()
        {
            var dto = new AcervoDetalheDTO();

            dto.EnderecoImagemPadrao.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoInstanciar_EntaoSituacaoDisponibilidadeEhNulavel()
        {
            var dto = new AcervoDetalheDTO();

            dto.SituacaoDisponibilidade.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoInstanciar_EntaoEstaDisponivelTemValorPadrao()
        {
            var dto = new AcervoDetalheDTO();

            dto.EstaDisponivel.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoInstanciar_EntaoTemControleDisponibilidadeTemValorPadrao()
        {
            var dto = new AcervoDetalheDTO();

            dto.TemControleDisponibilidade.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoInstanciar_EntaoTipoAcervoIdTemValorPadrao()
        {
            var dto = new AcervoDetalheDTO();

            dto.TipoAcervoId.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoDefinirTitulo_EntaoTituloEhAtribuido()
        {
            var dto = new AcervoDetalheDTO();
            const string tituloEsperado = "Novo Título";

            dto.Titulo = tituloEsperado;

            dto.Titulo.Should().Be(tituloEsperado);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoDefinirCodigo_EntaoCodigoEhAtribuido()
        {
            var dto = new AcervoDetalheDTO();
            const string codigoEsperado = "COD002";

            dto.Codigo = codigoEsperado;

            dto.Codigo.Should().Be(codigoEsperado);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoDefinirAno_EntaoAnoEhAtribuido()
        {
            var dto = new AcervoDetalheDTO();
            const string anoEsperado = "2025";

            dto.Ano = anoEsperado;

            dto.Ano.Should().Be(anoEsperado);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoDefinirAcervoId_EntaoAcervoIdEhAtribuido()
        {
            var dto = new AcervoDetalheDTO();
            const long acervoIdEsperado = 123;

            dto.AcervoId = acervoIdEsperado;

            dto.AcervoId.Should().Be(acervoIdEsperado);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoDefinirEnderecoImagemPadrao_EntaoEnderecoImagemPadraoEhAtribuido()
        {
            var dto = new AcervoDetalheDTO();
            const string enderecoEsperado = "https://exemplo.com/imagem.jpg";

            dto.EnderecoImagemPadrao = enderecoEsperado;

            dto.EnderecoImagemPadrao.Should().Be(enderecoEsperado);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoDefinirSituacaoDisponibilidade_EntaoSituacaoDisponibilidadeEhAtribuida()
        {
            var dto = new AcervoDetalheDTO();
            const string situacaoEsperada = "Disponível";

            dto.SituacaoDisponibilidade = situacaoEsperada;

            dto.SituacaoDisponibilidade.Should().Be(situacaoEsperada);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoDefinirEstaDisponivel_EntaoEstaDisponivelEhAtribuido()
        {
            var dto = new AcervoDetalheDTO();

            dto.EstaDisponivel = true;

            dto.EstaDisponivel.Should().BeTrue();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoDefinirEstaDisponivelFalso_EntaoEstaDisponivelEhFalso()
        {
            var dto = new AcervoDetalheDTO();

            dto.EstaDisponivel = false;

            dto.EstaDisponivel.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoDefinirTemControleDisponibilidade_EntaoTemControleDisponibilidadeEhAtribuido()
        {
            var dto = new AcervoDetalheDTO();

            dto.TemControleDisponibilidade = true;

            dto.TemControleDisponibilidade.Should().BeTrue();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoDefinirTemControleDisponibilidadeFalso_EntaoTemControleDisponibilidadeEhFalso()
        {
            var dto = new AcervoDetalheDTO();

            dto.TemControleDisponibilidade = false;

            dto.TemControleDisponibilidade.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoDefinirTipoAcervoId_EntaoTipoAcervoIdEhAtribuido()
        {
            var dto = new AcervoDetalheDTO();
            const int tipoAcervoIdEsperado = 5;

            dto.TipoAcervoId = tipoAcervoIdEsperado;

            dto.TipoAcervoId.Should().Be(tipoAcervoIdEsperado);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoUtilizarTodosOsCampos_EntaoTodosCamposSaoAcessiveis()
        {
            var dto = CriarAcervoDetalheDTO();

            dto.Titulo.Should().Be("Título do Acervo");
            dto.Codigo.Should().Be("COD001");
            dto.Ano.Should().Be("2024");
            dto.AcervoId.Should().Be(1);
            dto.EnderecoImagemPadrao.Should().Be("https://exemplo.com/imagem.jpg");
            dto.SituacaoDisponibilidade.Should().Be("Disponível");
            dto.EstaDisponivel.Should().BeTrue();
            dto.TemControleDisponibilidade.Should().BeTrue();
            dto.TipoAcervoId.Should().Be(1);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoTituloTemComprimentoMaximo_EntaoTituloEhAtribuido()
        {
            var dto = new AcervoDetalheDTO();
            var tituloComprido = new string('a', 500);

            dto.Titulo = tituloComprido;

            dto.Titulo.Should().Be(tituloComprido);
            dto.Titulo.Length.Should().Be(500);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoCodigoTemComprimentoMaximo_EntaoCodigoEhAtribuido()
        {
            var dto = new AcervoDetalheDTO();
            var codigoComprido = new string('a', 200);

            dto.Codigo = codigoComprido;

            dto.Codigo.Should().Be(codigoComprido);
            dto.Codigo.Length.Should().Be(200);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoVazioComValidacoes_EntaoValidacoesEstaoCorretas()
        {
            var properties = typeof(AcervoDetalheDTO).GetProperties();
            properties.Should().Contain(p => p.Name == "Titulo");
            properties.Should().Contain(p => p.Name == "Codigo");
            properties.Should().Contain(p => p.Name == "Ano");
            properties.Should().Contain(p => p.Name == "AcervoId");
            properties.Should().Contain(p => p.Name == "EnderecoImagemPadrao");
            properties.Should().Contain(p => p.Name == "SituacaoDisponibilidade");
            properties.Should().Contain(p => p.Name == "EstaDisponivel");
            properties.Should().Contain(p => p.Name == "TemControleDisponibilidade");
            properties.Should().Contain(p => p.Name == "TipoAcervoId");
            properties.Should().HaveCount(9);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoTituloVazio_EntaoTituloEhVazio()
        {
            var dto = new AcervoDetalheDTO();
            const string tituloVazio = "";

            dto.Titulo = tituloVazio;

            dto.Titulo.Should().Be(tituloVazio);
            dto.Titulo.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoCodigoVazio_EntaoCodigoEhVazio()
        {
            var dto = new AcervoDetalheDTO();
            const string codigoVazio = "";

            dto.Codigo = codigoVazio;

            dto.Codigo.Should().Be(codigoVazio);
            dto.Codigo.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoAnoVazio_EntaoAnoEhVazio()
        {
            var dto = new AcervoDetalheDTO();
            const string anoVazio = "";

            dto.Ano = anoVazio;

            dto.Ano.Should().Be(anoVazio);
            dto.Ano.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoAcervoIdComValorMaximo_EntaoAcervoIdEhArmazenado()
        {
            var dto = new AcervoDetalheDTO();
            const long acervoIdMaximo = long.MaxValue;

            dto.AcervoId = acervoIdMaximo;

            dto.AcervoId.Should().Be(acervoIdMaximo);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoAcervoIdComValorMinimo_EntaoAcervoIdEhArmazenado()
        {
            var dto = new AcervoDetalheDTO();
            const long acervoIdMinimo = long.MinValue;

            dto.AcervoId = acervoIdMinimo;

            dto.AcervoId.Should().Be(acervoIdMinimo);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoTipoAcervoIdComValorMaximo_EntaoTipoAcervoIdEhArmazenado()
        {
            var dto = new AcervoDetalheDTO();
            const int tipoAcervoIdMaximo = int.MaxValue;

            dto.TipoAcervoId = tipoAcervoIdMaximo;

            dto.TipoAcervoId.Should().Be(tipoAcervoIdMaximo);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoTipoAcervoIdComValorMinimo_EntaoTipoAcervoIdEhArmazenado()
        {
            var dto = new AcervoDetalheDTO();
            const int tipoAcervoIdMinimo = int.MinValue;

            dto.TipoAcervoId = tipoAcervoIdMinimo;

            dto.TipoAcervoId.Should().Be(tipoAcervoIdMinimo);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoModificarPropriedades_EntaoPropriedadesSaoAtualizadas()
        {
            var dto = CriarAcervoDetalheDTO();
            const string novoTitulo = "Título Atualizado";

            dto.Titulo = novoTitulo;

            dto.Titulo.Should().Be(novoTitulo);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoDefinirTodasAsPropriedades_EntaoTodosOsValoresSaoPreservados()
        {
            var dto = CriarAcervoDetalheDTO();

            dto.Titulo.Should().NotBeNull();
            dto.Codigo.Should().NotBeNull();
            dto.Ano.Should().NotBeNull();
            dto.AcervoId.Should().Be(1);
            dto.EnderecoImagemPadrao.Should().NotBeNull();
            dto.SituacaoDisponibilidade.Should().NotBeNull();
            dto.EstaDisponivel.Should().BeTrue();
            dto.TemControleDisponibilidade.Should().BeTrue();
            dto.TipoAcervoId.Should().Be(1);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoEnderecoImagemPadraoNulo_EntaoEnderecoImagemPadraoEhNulo()
        {
            var dto = new AcervoDetalheDTO { EnderecoImagemPadrao = null! };

            dto.EnderecoImagemPadrao.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoSituacaoDisponibilidadeNula_EntaoSituacaoDisponibilidadeEhNula()
        {
            var dto = new AcervoDetalheDTO { SituacaoDisponibilidade = null! };

            dto.SituacaoDisponibilidade.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoTituloComValoresEspeciais_EntaoTituloEhPreservado()
        {
            var dto = new AcervoDetalheDTO();
            const string tituloComEspeciais = "Título com ç, é, ñ e outros caracteres especiais";

            dto.Titulo = tituloComEspeciais;

            dto.Titulo.Should().Be(tituloComEspeciais);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoEnderecoImagemComCaracteresEspeciais_EntaoEnderecoEhPreservado()
        {
            var dto = new AcervoDetalheDTO();
            const string enderecoComEspeciais = "https://exemplo.com/imagens/acervo-2024_versão-final.jpg";

            dto.EnderecoImagemPadrao = enderecoComEspeciais;

            dto.EnderecoImagemPadrao.Should().Be(enderecoComEspeciais);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoSituacaoDisponibilidadeComValoresDistintos_EntaoValoresSaoCorretos()
        {
            var dto1 = new AcervoDetalheDTO { SituacaoDisponibilidade = "Disponível" };
            var dto2 = new AcervoDetalheDTO { SituacaoDisponibilidade = "Emprestado" };
            var dto3 = new AcervoDetalheDTO { SituacaoDisponibilidade = "Indisponível" };

            dto1.SituacaoDisponibilidade.Should().Be("Disponível");
            dto2.SituacaoDisponibilidade.Should().Be("Emprestado");
            dto3.SituacaoDisponibilidade.Should().Be("Indisponível");
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoAnoComApenasNumeros_EntaoAnoEhValidoComoString()
        {
            var dto = new AcervoDetalheDTO();
            const string anoNumerico = "2023";

            dto.Ano = anoNumerico;

            dto.Ano.Should().Be(anoNumerico);
            dto.Ano.Should().Match("*2023*");
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoEstaDisponivelMuda_EntaoValorEhAlterado()
        {
            var dto = new AcervoDetalheDTO();
            dto.EstaDisponivel = true;

            dto.EstaDisponivel = false;

            dto.EstaDisponivel.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoTemControleDisponibilidadeMuda_EntaoValorEhAlterado()
        {
            var dto = new AcervoDetalheDTO();
            dto.TemControleDisponibilidade = true;

            dto.TemControleDisponibilidade = false;

            dto.TemControleDisponibilidade.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoAcervoIdMuda_EntaoNovoValorEhArmazenado()
        {
            var dto = new AcervoDetalheDTO();
            dto.AcervoId = 100;

            dto.AcervoId = 200;

            dto.AcervoId.Should().Be(200);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoTipoAcervoIdMuda_EntaoNovoValorEhArmazenado()
        {
            var dto = new AcervoDetalheDTO();
            dto.TipoAcervoId = 1;

            dto.TipoAcervoId = 2;

            dto.TipoAcervoId.Should().Be(2);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoAcessarPropriedadesComConstrutorVazio_EntaoPropriedadesNaoLancamExcecao()
        {
            var exception = Record.Exception(() =>
            {
                var dto = new AcervoDetalheDTO();
                _ = dto.Titulo;
                _ = dto.Codigo;
                _ = dto.Ano;
                _ = dto.AcervoId;
                _ = dto.EnderecoImagemPadrao;
                _ = dto.SituacaoDisponibilidade;
                _ = dto.EstaDisponivel;
                _ = dto.TemControleDisponibilidade;
                _ = dto.TipoAcervoId;
            });

            exception.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoModificarDiversasVezesPropriedades_EntaoUltimoValorEhPreservado()
        {
            var dto = new AcervoDetalheDTO();

            dto.Titulo = "Primeiro Título";
            dto.Titulo = "Segundo Título";
            dto.Titulo = "Terceiro Título";

            dto.Titulo.Should().Be("Terceiro Título");
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoCompararDuasInstancias_EntaoSaoInstanciasDistintas()
        {
            var dto1 = CriarAcervoDetalheDTO();
            var dto2 = CriarAcervoDetalheDTO();

            dto1.Should().NotBeSameAs(dto2);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoAtribuirNullAoTitulo_EntaoTituloEhNull()
        {
            var dto = new AcervoDetalheDTO();
            dto.Titulo = "Título";

            dto.Titulo = null!;

            dto.Titulo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoAtribuirNullAoCodigo_EntaoCodigoEhNull()
        {
            var dto = new AcervoDetalheDTO();
            dto.Codigo = "COD001";

            dto.Codigo = null!;

            dto.Codigo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoAtribuirNullAoAno_EntaoAnoEhNull()
        {
            var dto = new AcervoDetalheDTO();
            dto.Ano = "2024";

            dto.Ano = null!;

            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoAtribuirNullAoEnderecoImagemPadrao_EntaoEnderecoEhNull()
        {
            var dto = new AcervoDetalheDTO();
            dto.EnderecoImagemPadrao = "https://exemplo.com/imagem.jpg";

            dto.EnderecoImagemPadrao = null!;

            dto.EnderecoImagemPadrao.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoAtribuirNullAoSituacaoDisponibilidade_EntaoSituacaoEhNull()
        {
            var dto = new AcervoDetalheDTO();
            dto.SituacaoDisponibilidade = "Disponível";

            dto.SituacaoDisponibilidade = null!;

            dto.SituacaoDisponibilidade.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoTituloNull_EntaoAcessarNaoLancaExcecao()
        {
            var dto = new AcervoDetalheDTO();

            var exception = Record.Exception(() => _ = dto.Titulo);

            exception.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoCodigoNull_EntaoAcessarNaoLancaExcecao()
        {
            var dto = new AcervoDetalheDTO();

            var exception = Record.Exception(() => _ = dto.Codigo);

            exception.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoAnoNull_EntaoAcessarNaoLancaExcecao()
        {
            var dto = new AcervoDetalheDTO();

            var exception = Record.Exception(() => _ = dto.Ano);

            exception.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoInicializarComValoresDefault_EntaoPropriedadesTemValoresCorretos()
        {
            var dto = new AcervoDetalheDTO
            {
                Titulo = "Título",
                Codigo = "COD001",
                Ano = "2024"
            };

            dto.Titulo.Should().Be("Título");
            dto.Codigo.Should().Be("COD001");
            dto.Ano.Should().Be("2024");
            dto.AcervoId.Should().Be(0);
            dto.EnderecoImagemPadrao.Should().BeNull();
            dto.SituacaoDisponibilidade.Should().BeNull();
            dto.EstaDisponivel.Should().BeFalse();
            dto.TemControleDisponibilidade.Should().BeFalse();
            dto.TipoAcervoId.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoTodasAsPropriedadesSaoDefinidas_EntaoTodosCamposSaoAcessados()
        {
            var dto = new AcervoDetalheDTO
            {
                Titulo = "Título Completo",
                Codigo = "CODFULL",
                Ano = "2024",
                AcervoId = 999,
                EnderecoImagemPadrao = "https://cdn.exemplo.com/imagem.jpg",
                SituacaoDisponibilidade = "Emprestado",
                EstaDisponivel = false,
                TemControleDisponibilidade = true,
                TipoAcervoId = 10
            };

            dto.Titulo.Should().Be("Título Completo");
            dto.Codigo.Should().Be("CODFULL");
            dto.Ano.Should().Be("2024");
            dto.AcervoId.Should().Be(999);
            dto.EnderecoImagemPadrao.Should().Be("https://cdn.exemplo.com/imagem.jpg");
            dto.SituacaoDisponibilidade.Should().Be("Emprestado");
            dto.EstaDisponivel.Should().BeFalse();
            dto.TemControleDisponibilidade.Should().BeTrue();
            dto.TipoAcervoId.Should().Be(10);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoEnderecoImagemVazio_EntaoEnderecoEhVazio()
        {
            var dto = new AcervoDetalheDTO();
            const string enderecoVazio = "";

            dto.EnderecoImagemPadrao = enderecoVazio;

            dto.EnderecoImagemPadrao.Should().Be(enderecoVazio);
            dto.EnderecoImagemPadrao.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoSituacaoDisponibilidadeVazia_EntaoSituacaoEhVazia()
        {
            var dto = new AcervoDetalheDTO();
            const string situacaoVazia = "";

            dto.SituacaoDisponibilidade = situacaoVazia;

            dto.SituacaoDisponibilidade.Should().Be(situacaoVazia);
            dto.SituacaoDisponibilidade.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoAcervoIdZero_EntaoAcervoIdEhZero()
        {
            var dto = new AcervoDetalheDTO();

            dto.AcervoId = 0;

            dto.AcervoId.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoTipoAcervoIdZero_EntaoTipoAcervoIdEhZero()
        {
            var dto = new AcervoDetalheDTO();

            dto.TipoAcervoId = 0;

            dto.TipoAcervoId.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoDefinirEstaDisponivelDiversasVezes_EntaoUltimoValorEhPreservado()
        {
            var dto = new AcervoDetalheDTO();

            dto.EstaDisponivel = true;
            dto.EstaDisponivel = false;
            dto.EstaDisponivel = true;

            dto.EstaDisponivel.Should().BeTrue();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoDefinirTemControleDisponibilidadeDiversasVezes_EntaoUltimoValorEhPreservado()
        {
            var dto = new AcervoDetalheDTO();

            dto.TemControleDisponibilidade = true;
            dto.TemControleDisponibilidade = false;
            dto.TemControleDisponibilidade = true;

            dto.TemControleDisponibilidade.Should().BeTrue();
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoEnderecoImagemComURLCompleta_EntaoEnderecoEhPreservado()
        {
            var dto = new AcervoDetalheDTO();
            const string urlCompleta = "https://storage.exemplo.com:8080/acervo/2024/imagens/acervo-123-v2.jpg?w=800&h=600";

            dto.EnderecoImagemPadrao = urlCompleta;

            dto.EnderecoImagemPadrao.Should().Be(urlCompleta);
        }

        [Fact]
        public void DadoAcervoDetalhe_QuandoCodigoComPrefixoENumero_EntaoCodigoEhArmazenado()
        {
            var dto = new AcervoDetalheDTO();
            const string codigoComPrefixo = "ACE-2024-00123";

            dto.Codigo = codigoComPrefixo;

            dto.Codigo.Should().Be(codigoComPrefixo);
        }
    }
}
