using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoDocumentalAlteracaoDTOTeste
    {
        private AcervoDocumentalAlteracaoDTO CriarAcervoDocumentalAlteracaoDTO()
        {
            return new AcervoDocumentalAlteracaoDTO
            {
                Id = 1,
                AcervoId = 10,
                Titulo = "Título do Documento",
                Descricao = "Descrição do documento",
                Codigo = "DOC001",
                CodigoNovo = "DOCN001",
                CreditosAutoresIds = new long[] { 1, 2, 3 },
                CoAutores = new[] { new CoAutorDTO { CreditoAutorNome = "Coautor 1" } },
                SubTitulo = "Subtítulo",
                DataAcervo = "2024-01-15",
                Ano = "2024",
                SituacaoAcervo = SituacaoAcervo.Ativo
            };
        }

        #region Testes de Inicialização

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoInstanciar_EntaoIdTemValorPadrao()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            dto.Id.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoInstanciar_EntaoAcervoIdTemValorPadrao()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            dto.AcervoId.Should().Be(0);
        }

        #endregion

        #region Testes de Validação - Id

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirIdComValido_EntaoIdEhAtribuido()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const long idEsperado = 123;

            dto.Id = idEsperado;

            dto.Id.Should().Be(idEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirIdComValorMaximo_EntaoIdEhArmazenado()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const long idMaximo = long.MaxValue;

            dto.Id = idMaximo;

            dto.Id.Should().Be(idMaximo);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirIdComValorUm_EntaoIdEhArmazenado()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const long idMinimo = 1;

            dto.Id = idMinimo;

            dto.Id.Should().Be(idMinimo);
        }

        #endregion

        #region Testes de Validação - AcervoId

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirAcervoIdComValido_EntaoAcervoIdEhAtribuido()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const long acervoIdEsperado = 50;

            dto.AcervoId = acervoIdEsperado;

            dto.AcervoId.Should().Be(acervoIdEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirAcervoIdComValorMaximo_EntaoAcervoIdEhArmazenado()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const long acervoIdMaximo = long.MaxValue;

            dto.AcervoId = acervoIdMaximo;

            dto.AcervoId.Should().Be(acervoIdMaximo);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirAcervoIdComValorUm_EntaoAcervoIdEhArmazenado()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const long acervoIdMinimo = 1;

            dto.AcervoId = acervoIdMinimo;

            dto.AcervoId.Should().Be(acervoIdMinimo);
        }

        #endregion

        #region Testes de Herança - Propriedades da Classe Base

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoInstanciar_EntaoTituloEhNulavel()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            dto.Titulo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirTitulo_EntaoTituloEhAtribuido()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const string tituloEsperado = "Novo Título";

            dto.Titulo = tituloEsperado;

            dto.Titulo.Should().Be(tituloEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoInstanciar_EntaoAnoEhNulavel()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirAno_EntaoAnoEhAtribuido()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const string anoEsperado = "2025";

            dto.Ano = anoEsperado;

            dto.Ano.Should().Be(anoEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoInstanciar_EntaoDescricaoEhNulavel()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            dto.Descricao.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirDescricao_EntaoDescricaoEhAtribuida()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const string descricaoEsperada = "Nova descrição";

            dto.Descricao = descricaoEsperada;

            dto.Descricao.Should().Be(descricaoEsperada);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoInstanciar_EntaoCodigoEhNulavel()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            dto.Codigo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirCodigo_EntaoCodigoEhAtribuido()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const string codigoEsperado = "DOC002";

            dto.Codigo = codigoEsperado;

            dto.Codigo.Should().Be(codigoEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoInstanciar_EntaoCodigoNovoEhNulavel()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            dto.CodigoNovo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirCodigoNovo_EntaoCodigoNovoEhAtribuido()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const string codigoNovoEsperado = "DOCN002";

            dto.CodigoNovo = codigoNovoEsperado;

            dto.CodigoNovo.Should().Be(codigoNovoEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoInstanciar_EntaoCreditosAutoresIdsEhNulavel()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            dto.CreditosAutoresIds.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirCreditosAutoresIds_EntaoCreditosAutoresIdsEhAtribuido()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            var creditosEsperados = new long[] { 1, 2, 3, 4 };

            dto.CreditosAutoresIds = creditosEsperados;

            dto.CreditosAutoresIds.Should().BeEquivalentTo(creditosEsperados);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoInstanciar_EntaoCoAutoresEhNulavel()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            dto.CoAutores.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirCoAutores_EntaoCoAutoresEhAtribuido()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            var coAutoresEsperados = new[] { new CoAutorDTO { CreditoAutorNome = "Coautor 1" }, new CoAutorDTO { CreditoAutorNome = "Coautor 2" } };

            dto.CoAutores = coAutoresEsperados;

            dto.CoAutores.Should().BeEquivalentTo(coAutoresEsperados);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoInstanciar_EntaoSubTituloEhNulavel()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            dto.SubTitulo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirSubTitulo_EntaoSubTituloEhAtribuido()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const string subTituloEsperado = "Novo Subtítulo";

            dto.SubTitulo = subTituloEsperado;

            dto.SubTitulo.Should().Be(subTituloEsperado);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoInstanciar_EntaoDataAcervoEhNulavel()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            dto.DataAcervo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirDataAcervo_EntaoDataAcervoEhAtribuida()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const string dataEsperada = "2024-12-25";

            dto.DataAcervo = dataEsperada;

            dto.DataAcervo.Should().Be(dataEsperada);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoInstanciar_EntaoSituacaoAcervoTemValorPadrao()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            dto.SituacaoAcervo.Should().Be(default(SituacaoAcervo));
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirSituacaoAcervo_EntaoSituacaoAcervoEhAtribuida()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const SituacaoAcervo situacaoEsperada = SituacaoAcervo.Inativo;

            dto.SituacaoAcervo = situacaoEsperada;

            dto.SituacaoAcervo.Should().Be(situacaoEsperada);
        }

        #endregion

        #region Testes Integrados - Todos os Campos

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoUtilizarTodosOsCampos_EntaoTodosCamposSaoAcessiveis()
        {
            var dto = CriarAcervoDocumentalAlteracaoDTO();

            dto.Id.Should().Be(1);
            dto.AcervoId.Should().Be(10);
            dto.Titulo.Should().Be("Título do Documento");
            dto.Descricao.Should().Be("Descrição do documento");
            dto.Codigo.Should().Be("DOC001");
            dto.CodigoNovo.Should().Be("DOCN001");
            dto.CreditosAutoresIds.Should().BeEquivalentTo(new long[] { 1, 2, 3 });
            dto.CoAutores.Should().HaveCount(1);
            dto.SubTitulo.Should().Be("Subtítulo");
            dto.DataAcervo.Should().Be("2024-01-15");
            dto.Ano.Should().Be("2024");
            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Ativo);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoInicializarComValoresDefault_EntaoPropriedadesTemValoresCorretos()
        {
            var dto = new AcervoDocumentalAlteracaoDTO
            {
                Id = 100,
                AcervoId = 200,
                Titulo = "Título",
                Ano = "2024"
            };

            dto.Id.Should().Be(100);
            dto.AcervoId.Should().Be(200);
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

        #endregion

        #region Testes de Comprimento Máximo

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoTituloTemComprimentoMaximo_EntaoTituloEhAtribuido()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            var tituloComprido = new string('a', 500);

            dto.Titulo = tituloComprido;

            dto.Titulo.Should().Be(tituloComprido);
            dto.Titulo.Length.Should().Be(500);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoCodigoTemComprimentoMaximo_EntaoCodigoEhAtribuido()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            var codigoComprido = new string('a', 200);

            dto.Codigo = codigoComprido;

            dto.Codigo.Should().Be(codigoComprido);
            dto.Codigo.Length.Should().Be(200);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoCodigoNovoTemComprimentoMaximo_EntaoCodigoNovoEhAtribuido()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            var codigoNovoComprido = new string('a', 200);

            dto.CodigoNovo = codigoNovoComprido;

            dto.CodigoNovo.Should().Be(codigoNovoComprido);
            dto.CodigoNovo.Length.Should().Be(200);
        }

        #endregion

        #region Testes de Valores Vazios

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoTituloVazio_EntaoTituloEhVazio()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const string tituloVazio = "";

            dto.Titulo = tituloVazio;

            dto.Titulo.Should().Be(tituloVazio);
            dto.Titulo.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoAnoVazio_EntaoAnoEhVazio()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const string anoVazio = "";

            dto.Ano = anoVazio;

            dto.Ano.Should().Be(anoVazio);
            dto.Ano.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoCodigoVazio_EntaoCodigoEhVazio()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const string codigoVazio = "";

            dto.Codigo = codigoVazio;

            dto.Codigo.Should().Be(codigoVazio);
            dto.Codigo.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoCodigoNovoVazio_EntaoCodigoNovoEhVazio()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const string codigoNovoVazio = "";

            dto.CodigoNovo = codigoNovoVazio;

            dto.CodigoNovo.Should().Be(codigoNovoVazio);
            dto.CodigoNovo.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoSubTituloVazio_EntaoSubTituloEhVazio()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const string subTituloVazio = "";

            dto.SubTitulo = subTituloVazio;

            dto.SubTitulo.Should().Be(subTituloVazio);
            dto.SubTitulo.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirCreditosAutoresIdsVazio_EntaoCreditosAutoresIdsEhVazio()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            var creditosVazios = new long[] { };

            dto.CreditosAutoresIds = creditosVazios;

            dto.CreditosAutoresIds.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirCoAutoresVazio_EntaoCoAutoresEhVazio()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            var coAutoresVazios = new CoAutorDTO[] { };

            dto.CoAutores = coAutoresVazios;

            dto.CoAutores.Should().BeEmpty();
        }

        #endregion

        #region Testes de Valores Nulos

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDescricaoNula_EntaoDescricaoEhNula()
        {
            var dto = new AcervoDocumentalAlteracaoDTO { Descricao = null };

            dto.Descricao.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoCodigoNulo_EntaoCodigoEhNulo()
        {
            var dto = new AcervoDocumentalAlteracaoDTO { Codigo = null };

            dto.Codigo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoCodigoNovoNulo_EntaoCodigoNovoEhNulo()
        {
            var dto = new AcervoDocumentalAlteracaoDTO { CodigoNovo = null };

            dto.CodigoNovo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoSubTituloNulo_EntaoSubTituloEhNulo()
        {
            var dto = new AcervoDocumentalAlteracaoDTO { SubTitulo = null };

            dto.SubTitulo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDataAcervoNula_EntaoDataAcervoEhNula()
        {
            var dto = new AcervoDocumentalAlteracaoDTO { DataAcervo = null };

            dto.DataAcervo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoAtribuirNullAoCreditosAutoresIds_EntaoCreditosAutoresIdsEhNull()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            dto.CreditosAutoresIds = new long[] { 1, 2 };

            dto.CreditosAutoresIds = null;

            dto.CreditosAutoresIds.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoAtribuirNullAoCoAutores_EntaoCoAutoresEhNull()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            dto.CoAutores = new[] { new CoAutorDTO { CreditoAutorNome = "Autor" } };

            dto.CoAutores = null;

            dto.CoAutores.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoAtribuirNullAoTitulo_EntaoTituloEhNull()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            dto.Titulo = "Título";

            dto.Titulo = null;

            dto.Titulo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoAtribuirNullAoAno_EntaoAnoEhNull()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            dto.Ano = "2024";

            dto.Ano = null;

            dto.Ano.Should().BeNull();
        }

        #endregion

        #region Testes de Mutação de Valores

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoModificarPropriedades_EntaoPropriedadesSaoAtualizadas()
        {
            var dto = CriarAcervoDocumentalAlteracaoDTO();
            const string novoTitulo = "Título Atualizado";

            dto.Titulo = novoTitulo;

            dto.Titulo.Should().Be(novoTitulo);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoModificarDiversasVezesPropriedades_EntaoUltimoValorEhPreservado()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            dto.Titulo = "Primeiro Título";
            dto.Titulo = "Segundo Título";
            dto.Titulo = "Terceiro Título";

            dto.Titulo.Should().Be("Terceiro Título");
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoModificarId_EntaoNovoIdEhArmazenado()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            dto.Id = 100;

            dto.Id = 200;

            dto.Id.Should().Be(200);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoModificarAcervoId_EntaoNovoAcervoIdEhArmazenado()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            dto.AcervoId = 100;

            dto.AcervoId = 200;

            dto.AcervoId.Should().Be(200);
        }

        #endregion

        #region Testes de Caracteres Especiais

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoTituloComValoresEspeciais_EntaoTituloEhPreservado()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const string tituloComEspeciais = "Título com ç, é, ñ e outros caracteres especiais";

            dto.Titulo = tituloComEspeciais;

            dto.Titulo.Should().Be(tituloComEspeciais);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDescricaoGrande_EntaoDescricaoEhPreservada()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            var descricaoGrande = string.Join(" ", Enumerable.Range(1, 100).Select(_ => "Palavra"));

            dto.Descricao = descricaoGrande;

            dto.Descricao.Should().Be(descricaoGrande);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirDescricaoMultilinha_EntaoDescricaoEhPreservada()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const string descricaoMultilinha = "Linha 1\nLinha 2\nLinha 3";

            dto.Descricao = descricaoMultilinha;

            dto.Descricao.Should().Be(descricaoMultilinha);
            dto.Descricao.Should().Contain("\n");
        }

        #endregion

        #region Testes de Arrays e Coleções

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoMultiplosCreditosAutores_EntaoCreditosAutoresIdsTemMultiposValores()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            var creditos = new long[] { 100, 200, 300, 400, 500 };

            dto.CreditosAutoresIds = creditos;

            dto.CreditosAutoresIds.Should().HaveCount(5);
            dto.CreditosAutoresIds.Should().BeEquivalentTo(creditos);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoMultiplosCoAutores_EntaoCoAutoresTemMultiplosValores()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
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
        public void DadoAcervoDocumentalAlteracao_QuandoCreditoAutorComValueMaximo_EntaoCreditoAutorEhArmazenado()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            var creditosComValoresAltos = new long[] { long.MaxValue, long.MinValue, 0 };

            dto.CreditosAutoresIds = creditosComValoresAltos;

            dto.CreditosAutoresIds.Should().BeEquivalentTo(creditosComValoresAltos);
        }

        #endregion

        #region Testes de Dados com Formato

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDataAcervoComFormato_EntaoDataEhPreservada()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const string dataFormatada = "2024-12-31";

            dto.DataAcervo = dataFormatada;

            dto.DataAcervo.Should().Be(dataFormatada);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoAnoComValorNumerico_EntaoAnoEhArmazenado()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const string anoValor = "1995";

            dto.Ano = anoValor;

            dto.Ano.Should().Be(anoValor);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoAnoComApenasNumeros_EntaoAnoEhValidoComoString()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const string anoNumerico = "2023";

            dto.Ano = anoNumerico;

            dto.Ano.Should().Be(anoNumerico);
            dto.Ano.Should().Match("*2023*");
        }

        #endregion

        #region Testes de Situação do Acervo

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirSituacaoAcervoAtivoEInativo_EntaoValoresSaoCorretos()
        {
            var dto1 = new AcervoDocumentalAlteracaoDTO { SituacaoAcervo = SituacaoAcervo.Ativo };
            var dto2 = new AcervoDocumentalAlteracaoDTO { SituacaoAcervo = SituacaoAcervo.Inativo };

            dto1.SituacaoAcervo.Should().Be(SituacaoAcervo.Ativo);
            dto2.SituacaoAcervo.Should().Be(SituacaoAcervo.Inativo);
        }

        #endregion

        #region Testes de Validação de Propriedades

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoVazioComValidacoes_EntaoValidacoesEstaoCorretas()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            var properties = typeof(AcervoDocumentalAlteracaoDTO).GetProperties();
            properties.Should().Contain(p => p.Name == "Id");
            properties.Should().Contain(p => p.Name == "AcervoId");
            properties.Should().NotBeEmpty();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoAcessarPropriedadesComConstrutorVazio_EntaoPropriedadesNaoLancamExcecao()
        {
            var exception = Record.Exception(() =>
            {
                var dto = new AcervoDocumentalAlteracaoDTO();
                _ = dto.Id;
                _ = dto.AcervoId;
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
        public void DadoAcervoDocumentalAlteracao_QuandoTituloNull_EntaoAcessarNaoLancaExcecao()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            var exception = Record.Exception(() => _ = dto.Titulo);

            exception.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoAnoNull_EntaoAcessarNaoLancaExcecao()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            var exception = Record.Exception(() => _ = dto.Ano);

            exception.Should().BeNull();
        }

        #endregion

        #region Testes de Instâncias Distintas

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoCompararDuasInstancias_EntaoSaoInstanciasDistintas()
        {
            var dto1 = CriarAcervoDocumentalAlteracaoDTO();
            var dto2 = CriarAcervoDocumentalAlteracaoDTO();

            dto1.Should().NotBeSameAs(dto2);
        }

        #endregion

        #region Testes de Definição de Todas as Propriedades

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoDefinirTodasAsPropriedades_EntaoTodosOsValoresSaoPreservados()
        {
            var dto = CriarAcervoDocumentalAlteracaoDTO();

            dto.Id.Should().NotBe(0);
            dto.AcervoId.Should().NotBe(0);
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
        public void DadoAcervoDocumentalAlteracao_QuandoTodasAsPropriedadesSaoDefinidas_EntaoTodosCamposSaoAcessados()
        {
            var dto = new AcervoDocumentalAlteracaoDTO
            {
                Id = 999,
                AcervoId = 888,
                Titulo = "Título Completo",
                Codigo = "CODFULL",
                Ano = "2024",
                Descricao = "Descrição Completa",
                CodigoNovo = "CODFULLN",
                SubTitulo = "Subtítulo Completo",
                DataAcervo = "2024-01-01",
                SituacaoAcervo = SituacaoAcervo.Ativo
            };

            dto.Id.Should().Be(999);
            dto.AcervoId.Should().Be(888);
            dto.Titulo.Should().Be("Título Completo");
            dto.Codigo.Should().Be("CODFULL");
            dto.Ano.Should().Be("2024");
            dto.Descricao.Should().Be("Descrição Completa");
            dto.CodigoNovo.Should().Be("CODFULLN");
            dto.SubTitulo.Should().Be("Subtítulo Completo");
            dto.DataAcervo.Should().Be("2024-01-01");
            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Ativo);
        }

        #endregion

        #region Testes de Herança

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoVerificarHeranca_EntaoEhSubclasseDe()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            dto.Should().BeAssignableTo<AcervoDocumentalCadastroDTO>();
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoInicializarComValoresPai_EntaoPropriedadesPaiSaoHerdadas()
        {
            var dto = new AcervoDocumentalAlteracaoDTO
            {
                Titulo = "Título Herdado",
                Ano = "2024",
                Descricao = "Descrição Herdada"
            };

            dto.Titulo.Should().Be("Título Herdado");
            dto.Ano.Should().Be("2024");
            dto.Descricao.Should().Be("Descrição Herdada");
        }

        #endregion

        #region Testes de Valores Extremos

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoIdComValorZero_EntaoIdEhZero()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            dto.Id = 0;

            dto.Id.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoAcervoIdComValorZero_EntaoAcervoIdEhZero()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();

            dto.AcervoId = 0;

            dto.AcervoId.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoIdComValorMinimo_EntaoIdEhArmazenado()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const long idMinimo = long.MinValue;

            dto.Id = idMinimo;

            dto.Id.Should().Be(idMinimo);
        }

        [Fact]
        public void DadoAcervoDocumentalAlteracao_QuandoAcervoIdComValorMinimo_EntaoAcervoIdEhArmazenado()
        {
            var dto = new AcervoDocumentalAlteracaoDTO();
            const long acervoIdMinimo = long.MinValue;

            dto.AcervoId = acervoIdMinimo;

            dto.AcervoId.Should().Be(acervoIdMinimo);
        }

        #endregion
    }
}
