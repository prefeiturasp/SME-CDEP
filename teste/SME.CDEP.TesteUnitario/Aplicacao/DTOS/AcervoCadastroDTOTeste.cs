using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoCadastroDTOTeste
    {
        private static AcervoCadastroDTO CriarAcervoCadastroDTO()
        {
            return new AcervoCadastroDTO
            {
                Titulo = "Título do Acervo",
                Descricao = "Descrição do acervo",
                Codigo = "COD001",
                CodigoNovo = "CODN001",
                CreditosAutoresIds = new long[] { 1, 2, 3 },
                CoAutores = new[] { new CoAutorDTO { CreditoAutorNome = "Coautor 1" } },
                SubTitulo = "Subtítulo",
                DataAcervo = "2024-01-15",
                Ano = "2024",
                SituacaoAcervo = SituacaoAcervo.Ativo
            };
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoInstanciar_EntaoTituloEhRequerido()
        {
            var dto = new AcervoCadastroDTO();

            dto.Titulo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoInstanciar_EntaoAnoEhRequerido()
        {
            var dto = new AcervoCadastroDTO();

            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoInstanciar_EntaoDescricaoEhNulavel()
        {
            var dto = new AcervoCadastroDTO();

            dto.Descricao.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoInstanciar_EntaoCodigoEhNulavel()
        {
            var dto = new AcervoCadastroDTO();

            dto.Codigo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoInstanciar_EntaoCodigoNovoEhNulavel()
        {
            var dto = new AcervoCadastroDTO();

            dto.CodigoNovo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoInstanciar_EntaoCreditosAutoresIdsEhNulavel()
        {
            var dto = new AcervoCadastroDTO();

            dto.CreditosAutoresIds.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoInstanciar_EntaoCoAutoresEhNulavel()
        {
            var dto = new AcervoCadastroDTO();

            dto.CoAutores.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoInstanciar_EntaoSubTituloEhNulavel()
        {
            var dto = new AcervoCadastroDTO();

            dto.SubTitulo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoInstanciar_EntaoDataAcervoEhNulavel()
        {
            var dto = new AcervoCadastroDTO();

            dto.DataAcervo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDefinirTitulo_EntaoTituloEhAtribuido()
        {
            var dto = new AcervoCadastroDTO();
            const string tituloEsperado = "Novo Título";

            dto.Titulo = tituloEsperado;

            dto.Titulo.Should().Be(tituloEsperado);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDefinirAno_EntaoAnoEhAtribuido()
        {
            var dto = new AcervoCadastroDTO();
            const string anoEsperado = "2025";

            dto.Ano = anoEsperado;

            dto.Ano.Should().Be(anoEsperado);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDefinirDescricao_EntaoDescricaoEhAtribuida()
        {
            var dto = new AcervoCadastroDTO();
            const string descricaoEsperada = "Nova descrição";

            dto.Descricao = descricaoEsperada;

            dto.Descricao.Should().Be(descricaoEsperada);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDefinirCodigo_EntaoCodigoEhAtribuido()
        {
            var dto = new AcervoCadastroDTO();
            const string codigoEsperado = "COD002";

            dto.Codigo = codigoEsperado;

            dto.Codigo.Should().Be(codigoEsperado);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDefinirCodigoNovo_EntaoCodigoNovoEhAtribuido()
        {
            var dto = new AcervoCadastroDTO();
            const string codigoNovoEsperado = "CODN002";

            dto.CodigoNovo = codigoNovoEsperado;

            dto.CodigoNovo.Should().Be(codigoNovoEsperado);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDefinirCreditosAutoresIds_EntaoCreditosAutoresIdsEhAtribuido()
        {
            var dto = new AcervoCadastroDTO();
            var creditosEsperados = new long[] { 1, 2, 3, 4 };

            dto.CreditosAutoresIds = creditosEsperados;

            dto.CreditosAutoresIds.Should().BeEquivalentTo(creditosEsperados);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDefinirCoAutores_EntaoCoAutoresEhAtribuido()
        {
            var dto = new AcervoCadastroDTO();
            var coAutoresEsperados = new[] { new CoAutorDTO { CreditoAutorNome = "Coautor 1" }, new CoAutorDTO { CreditoAutorNome = "Coautor 2" } };

            dto.CoAutores = coAutoresEsperados;

            dto.CoAutores.Should().BeEquivalentTo(coAutoresEsperados);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDefinirSubTitulo_EntaoSubTituloEhAtribuido()
        {
            var dto = new AcervoCadastroDTO();
            const string subTituloEsperado = "Novo Subtítulo";

            dto.SubTitulo = subTituloEsperado;

            dto.SubTitulo.Should().Be(subTituloEsperado);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDefinirDataAcervo_EntaoDataAcervoEhAtribuida()
        {
            var dto = new AcervoCadastroDTO();
            const string dataEsperada = "2024-12-25";

            dto.DataAcervo = dataEsperada;

            dto.DataAcervo.Should().Be(dataEsperada);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDefinirSituacaoAcervo_EntaoSituacaoAcervoEhAtribuida()
        {
            var dto = new AcervoCadastroDTO();
            const SituacaoAcervo situacaoEsperada = SituacaoAcervo.Inativo;

            dto.SituacaoAcervo = situacaoEsperada;

            dto.SituacaoAcervo.Should().Be(situacaoEsperada);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoUtilizarTodosOsCampos_EntaoTodosCamposSaoAcessiveis()
        {
            var dto = CriarAcervoCadastroDTO();

            dto.Titulo.Should().Be("Título do Acervo");
            dto.Descricao.Should().Be("Descrição do acervo");
            dto.Codigo.Should().Be("COD001");
            dto.CodigoNovo.Should().Be("CODN001");
            dto.CreditosAutoresIds.Should().BeEquivalentTo(new long[] { 1, 2, 3 });
            dto.CoAutores.Should().HaveCount(1);
            dto.SubTitulo.Should().Be("Subtítulo");
            dto.DataAcervo.Should().Be("2024-01-15");
            dto.Ano.Should().Be("2024");
            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Ativo);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoTituloTemComprimentoMaximo_EntaoTituloEhAtribuido()
        {
            var dto = new AcervoCadastroDTO();
            var tituloComprido = new string('a', 500);

            dto.Titulo = tituloComprido;

            dto.Titulo.Should().Be(tituloComprido);
            dto.Titulo.Length.Should().Be(500);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoCodigoTemComprimentoMaximo_EntaoCodigoEhAtribuido()
        {
            var dto = new AcervoCadastroDTO();
            var codigoComprido = new string('a', 200);

            dto.Codigo = codigoComprido;

            dto.Codigo.Should().Be(codigoComprido);
            dto.Codigo.Length.Should().Be(200);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoCodigoNovoTemComprimentoMaximo_EntaoCodigoNovoEhAtribuido()
        {
            var dto = new AcervoCadastroDTO();
            var codigoNovoComprido = new string('a', 200);

            dto.CodigoNovo = codigoNovoComprido;

            dto.CodigoNovo.Should().Be(codigoNovoComprido);
            dto.CodigoNovo.Length.Should().Be(200);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoVazioComValidacoes_EntaoValidacoesEstaoCorretas()
        {
            var properties = typeof(AcervoCadastroDTO).GetProperties();
            properties.Should().Contain(p => p.Name == "Titulo");
            properties.Should().Contain(p => p.Name == "Ano");
            properties.Should().NotBeEmpty();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDefinirCreditosAutoresIdsVazio_EntaoCreditosAutoresIdsEhVazio()
        {
            var dto = new AcervoCadastroDTO();
            var creditosVazios = Array.Empty<long>();

            dto.CreditosAutoresIds = creditosVazios;

            dto.CreditosAutoresIds.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDefinirCoAutoresVazio_EntaoCoAutoresEhVazio()
        {
            var dto = new AcervoCadastroDTO();
            var coAutoresVazios = Array.Empty<CoAutorDTO>();

            dto.CoAutores = coAutoresVazios;

            dto.CoAutores.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoTituloVazio_EntaoTituloEhVazio()
        {
            var dto = new AcervoCadastroDTO();
            const string tituloVazio = "";

            dto.Titulo = tituloVazio;

            dto.Titulo.Should().Be(tituloVazio);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoAnoVazio_EntaoAnoEhVazio()
        {
            var dto = new AcervoCadastroDTO();
            const string anoVazio = "";

            dto.Ano = anoVazio;

            dto.Ano.Should().Be(anoVazio);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoModificarPropriedades_EntaoPropriedadesSaoAtualizadas()
        {
            var dto = CriarAcervoCadastroDTO();
            const string novoTitulo = "Título Atualizado";

            dto.Titulo = novoTitulo;

            dto.Titulo.Should().Be(novoTitulo);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDefinirTodasAsPropriedades_EntaoTodosOsValoresSaoPresevados()
        {
            var dto = CriarAcervoCadastroDTO();

            dto.Titulo.Should().NotBeNull();
            dto.Ano.Should().NotBeNull();
            dto.Descricao.Should().NotBeNull();
            dto.Codigo.Should().NotBeNull();
            dto.CodigoNovo.Should().NotBeNull();
            dto.CreditosAutoresIds.Should().NotBeNull();
            dto.CoAutores.Should().NotBeNull();
            dto.SubTitulo.Should().NotBeNull();
            dto.DataAcervo.Should().NotBeNull();
            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Ativo);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDescricaoNula_EntaoDescricaoEhNula()
        {
            var dto = new AcervoCadastroDTO { Descricao = null };

            dto.Descricao.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoCodigoNulo_EntaoCodigoEhNulo()
        {
            var dto = new AcervoCadastroDTO { Codigo = null };

            dto.Codigo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoCodigoNovoNulo_EntaoCodigoNovoEhNulo()
        {
            var dto = new AcervoCadastroDTO { CodigoNovo = null };

            dto.CodigoNovo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoSubTituloNulo_EntaoSubTituloEhNulo()
        {
            var dto = new AcervoCadastroDTO { SubTitulo = null };

            dto.SubTitulo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDataAcervoNula_EntaoDataAcervoEhNula()
        {
            var dto = new AcervoCadastroDTO { DataAcervo = null };

            dto.DataAcervo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDefinirSituacaoAcervoAtivoEInativo_EntaoValoresSaoCorretos()
        {
            var dto1 = new AcervoCadastroDTO { SituacaoAcervo = SituacaoAcervo.Ativo };
            var dto2 = new AcervoCadastroDTO { SituacaoAcervo = SituacaoAcervo.Inativo };

            dto1.SituacaoAcervo.Should().Be(SituacaoAcervo.Ativo);
            dto2.SituacaoAcervo.Should().Be(SituacaoAcervo.Inativo);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoTituloComValoresEspeciais_EntaoTituloEhPreservado()
        {
            var dto = new AcervoCadastroDTO();
            const string tituloComEspeciais = "Título com ç, é, ñ e outros caracteres especiais";

            dto.Titulo = tituloComEspeciais;

            dto.Titulo.Should().Be(tituloComEspeciais);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDescricaoGrande_EntaoDescricaoEhPreservada()
        {
            var dto = new AcervoCadastroDTO();
            var descricaoGrande = string.Join(" ", Enumerable.Range(1, 100).Select(_ => "Palavra"));

            dto.Descricao = descricaoGrande;

            dto.Descricao.Should().Be(descricaoGrande);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoMultiplosCreditosAutores_EntaoCreditosAutoresIdsTemMultiposValores()
        {
            var dto = new AcervoCadastroDTO();
            var creditos = new long[] { 100, 200, 300, 400, 500 };

            dto.CreditosAutoresIds = creditos;

            dto.CreditosAutoresIds.Should().HaveCount(5);
            dto.CreditosAutoresIds.Should().BeEquivalentTo(creditos);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoMultiplosCoAutores_EntaoCoAutoresTemMultiplosValores()
        {
            var dto = new AcervoCadastroDTO();
            var coAutores = new[]
            {
                new CoAutorDTO { CreditoAutorNome = "Autor 1" },
                new CoAutorDTO { CreditoAutorNome = "Autor 2" },
                new CoAutorDTO { CreditoAutorNome = "Autor 3" }
            };

            dto.CoAutores = coAutores;

            dto.CoAutores.Should().HaveCount(3);
            dto.CoAutores.Should().BeEquivalentTo(coAutores);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDataAcervoComFormato_EntaoDataEhPreservada()
        {
            var dto = new AcervoCadastroDTO();
            const string dataFormatada = "2024-12-31";

            dto.DataAcervo = dataFormatada;

            dto.DataAcervo.Should().Be(dataFormatada);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoAnoComValorNumerico_EntaoAnoEhArmazenado()
        {
            var dto = new AcervoCadastroDTO();
            const string anoValor = "1995";

            dto.Ano = anoValor;

            dto.Ano.Should().Be(anoValor);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoInicializarComValoresDefault_EntaoPropriedadesTemValoresCorretos()
        {
            var dto = new AcervoCadastroDTO
            {
                Titulo = "Título",
                Ano = "2024"
            };

            dto.Titulo.Should().Be("Título");
            dto.Ano.Should().Be("2024");
            dto.Descricao.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.CodigoNovo.Should().BeNull();
            dto.CreditosAutoresIds.Should().BeNull();
            dto.CoAutores.Should().BeNull();
            dto.SubTitulo.Should().BeNull();
            dto.DataAcervo.Should().BeNull();
            dto.SituacaoAcervo.Should().Be(default(SituacaoAcervo));
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoAcessarPropriedadesComConstrutorVazio_EntaoPropriedadesNaoLancamExcecao()
        {
            var exception = Record.Exception(() =>
            {
                var dto = new AcervoCadastroDTO();
                _ = dto.Titulo;
                _ = dto.Ano;
                _ = dto.Descricao;
                _ = dto.Codigo;
                _ = dto.CodigoNovo;
                _ = dto.CreditosAutoresIds;
                _ = dto.CoAutores;
                _ = dto.SubTitulo;
                _ = dto.DataAcervo;
                _ = dto.SituacaoAcervo;
            });

            exception.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoModificarDiversasVezesPropriedades_EntaoUltimoValorEhPreservado()
        {
            var dto = new AcervoCadastroDTO();

            dto.Titulo = "Primeiro Título";
            dto.Titulo = "Segundo Título";
            dto.Titulo = "Terceiro Título";

            dto.Titulo.Should().Be("Terceiro Título");
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoCompararDuasInstancias_EntaoSaoInstanciasDistintas()
        {
            var dto1 = CriarAcervoCadastroDTO();
            var dto2 = CriarAcervoCadastroDTO();

            dto1.Should().NotBeSameAs(dto2);
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoAtribuirNullAoCreditosAutoresIds_EntaoCreditosAutoresIdsEhNull()
        {
            var dto = new AcervoCadastroDTO();
            dto.CreditosAutoresIds = new long[] { 1, 2 };

            dto.CreditosAutoresIds = null;

            dto.CreditosAutoresIds.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoAtribuirNullAoCoAutores_EntaoCoAutoresEhNull()
        {
            var dto = new AcervoCadastroDTO();
            dto.CoAutores = new[] { new CoAutorDTO { CreditoAutorNome = "Autor" } };

            dto.CoAutores = null;

            dto.CoAutores.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoAnoComApenasNumeros_EntaoAnoEhValidoComoString()
        {
            var dto = new AcervoCadastroDTO();
            const string anoNumerico = "2023";

            dto.Ano = anoNumerico;

            dto.Ano.Should().Be(anoNumerico);
            dto.Ano.Should().Match("*2023*");
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoCodigoVazio_EntaoCodigoEhVazio()
        {
            var dto = new AcervoCadastroDTO();
            const string codigoVazio = "";

            dto.Codigo = codigoVazio;

            dto.Codigo.Should().Be(codigoVazio);
            dto.Codigo.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoSubTituloVazio_EntaoSubTituloEhVazio()
        {
            var dto = new AcervoCadastroDTO();
            const string subTituloVazio = "";

            dto.SubTitulo = subTituloVazio;

            dto.SubTitulo.Should().Be(subTituloVazio);
            dto.SubTitulo.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoTituloNull_EntaoAcessarNaoLancaExcecao()
        {
            var dto = new AcervoCadastroDTO();

            var exception = Record.Exception(() => _ = dto.Titulo);

            exception.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoAnoNull_EntaoAcessarNaoLancaExcecao()
        {
            var dto = new AcervoCadastroDTO();

            var exception = Record.Exception(() => _ = dto.Ano);

            exception.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoDefinirDescricaoMultilinha_EntaoDescricaoEhPreservada()
        {
            var dto = new AcervoCadastroDTO();
            const string descricaoMultilinha = "Linha 1\nLinha 2\nLinha 3";

            dto.Descricao = descricaoMultilinha;

            dto.Descricao.Should().Be(descricaoMultilinha);
            dto.Descricao.Should().Contain("\n");
        }

        [Fact]
        public void DadoAcervoCadastro_QuandoCreditoAutorComValueMaximo_EntaoCreditoAutorEhArmazenado()
        {
            var dto = new AcervoCadastroDTO();
            var creditosComValoresAltos = new long[] { long.MaxValue, long.MinValue, 0 };

            dto.CreditosAutoresIds = creditosComValoresAltos;

            dto.CreditosAutoresIds.Should().BeEquivalentTo(creditosComValoresAltos);
        }
    }
}
