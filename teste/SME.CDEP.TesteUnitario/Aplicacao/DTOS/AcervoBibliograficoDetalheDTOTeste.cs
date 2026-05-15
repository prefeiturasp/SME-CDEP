using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoBibliograficoDetalheDtoTeste
    {
        private readonly Faker _faker;

        public AcervoBibliograficoDetalheDtoTeste()
        {
            _faker = new Faker("pt_BR");
        }

        #region Testes de Properties - Classe Base

        [Fact(DisplayName = "Titulo - Quando atribuído - Deve retornar o valor")]
        public void DadoTituloAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var titulo = _faker.Lorem.Sentence();

            dto.Titulo = titulo;

            dto.Titulo.Should().Be(titulo);
        }

        [Fact(DisplayName = "Titulo - Quando nulo - Deve retornar nulo")]
        public void DadoTituloNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.Titulo = null!;

            dto.Titulo.Should().BeNull();
        }

        [Fact(DisplayName = "Codigo - Quando atribuído - Deve retornar o valor")]
        public void DadoCodigoAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var codigo = _faker.Random.AlphaNumeric(10);

            dto.Codigo = codigo;

            dto.Codigo.Should().Be(codigo);
        }

        [Fact(DisplayName = "Codigo - Quando nulo - Deve retornar nulo")]
        public void DadoCodigoNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.Codigo = null!;

            dto.Codigo.Should().BeNull();
        }

        [Fact(DisplayName = "Ano - Quando atribuído - Deve retornar o valor")]
        public void DadoAnoAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var ano = DateTime.Now.Year.ToString();

            dto.Ano = ano;

            dto.Ano.Should().Be(ano);
        }

        [Fact(DisplayName = "Ano - Quando nulo - Deve retornar nulo")]
        public void DadoAnoNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.Ano = null!;

            dto.Ano.Should().BeNull();
        }

        [Fact(DisplayName = "AcervoId - Quando atribuído - Deve retornar o valor")]
        public void DadoAcervoIdAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var acervoId = _faker.Random.Long(1, 1000);

            dto.AcervoId = acervoId;

            dto.AcervoId.Should().Be(acervoId);
        }

        [Fact(DisplayName = "AcervoId - Quando atribuído com zero - Deve retornar zero")]
        public void DadoAcervoIdComZero_QuandoAcessar_EntaoDeveRetornarZero()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.AcervoId = 0;

            dto.AcervoId.Should().Be(0);
        }

        [Fact(DisplayName = "EnderecoImagemPadrao - Quando atribuído - Deve retornar o valor")]
        public void DadoEnderecoImagemPadraoAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var endereco = _faker.Internet.Url();

            dto.EnderecoImagemPadrao = endereco;

            dto.EnderecoImagemPadrao.Should().Be(endereco);
        }

        [Fact(DisplayName = "EnderecoImagemPadrao - Quando nulo - Deve retornar nulo")]
        public void DadoEnderecoImagemPadraoNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.EnderecoImagemPadrao = null!;

            dto.EnderecoImagemPadrao.Should().BeNull();
        }

        [Fact(DisplayName = "SituacaoDisponibilidade - Quando atribuído - Deve retornar o valor")]
        public void DadoSituacaoDisponibilidadeAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var situacao = "Disponível";

            dto.SituacaoDisponibilidade = situacao;

            dto.SituacaoDisponibilidade.Should().Be(situacao);
        }

        [Fact(DisplayName = "SituacaoDisponibilidade - Quando nulo - Deve retornar nulo")]
        public void DadoSituacaoDisponibilidadeNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.SituacaoDisponibilidade = null!;

            dto.SituacaoDisponibilidade.Should().BeNull();
        }

        [Fact(DisplayName = "EstaDisponivel - Quando verdadeiro - Deve retornar verdadeiro")]
        public void DadoEstaDisponivelVerdadeiro_QuandoAcessar_EntaoDeveRetornarVerdadeiro()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.EstaDisponivel = true;

            dto.EstaDisponivel.Should().BeTrue();
        }

        [Fact(DisplayName = "EstaDisponivel - Quando falso - Deve retornar falso")]
        public void DadoEstaDisponivelFalso_QuandoAcessar_EntaoDeveRetornarFalso()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.EstaDisponivel = false;

            dto.EstaDisponivel.Should().BeFalse();
        }

        [Fact(DisplayName = "TemControleDisponibilidade - Quando verdadeiro - Deve retornar verdadeiro")]
        public void DadoTemControleDisponibilidadeVerdadeiro_QuandoAcessar_EntaoDeveRetornarVerdadeiro()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.TemControleDisponibilidade = true;

            dto.TemControleDisponibilidade.Should().BeTrue();
        }

        [Fact(DisplayName = "TemControleDisponibilidade - Quando falso - Deve retornar falso")]
        public void DadoTemControleDisponibilidadeFalso_QuandoAcessar_EntaoDeveRetornarFalso()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.TemControleDisponibilidade = false;

            dto.TemControleDisponibilidade.Should().BeFalse();
        }

        [Fact(DisplayName = "TipoAcervoId - Quando atribuído - Deve retornar o valor")]
        public void DadoTipoAcervoIdAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var tipoAcervoId = _faker.Random.Int(1, 1000);

            dto.TipoAcervoId = tipoAcervoId;

            dto.TipoAcervoId.Should().Be(tipoAcervoId);
        }

        [Fact(DisplayName = "TipoAcervoId - Quando zero - Deve retornar zero")]
        public void DadoTipoAcervoIdComZero_QuandoAcessar_EntaoDeveRetornarZero()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.TipoAcervoId = 0;

            dto.TipoAcervoId.Should().Be(0);
        }

        #endregion

        #region Testes de Properties - Próprias da Classe

        [Fact(DisplayName = "CreditosAutores - Quando atribuído - Deve retornar o valor")]
        public void DadoCreditosAutoresAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var creditos = _faker.Lorem.Text();

            dto.CreditosAutores = creditos;

            dto.CreditosAutores.Should().Be(creditos);
        }

        [Fact(DisplayName = "CreditosAutores - Quando nulo - Deve retornar nulo")]
        public void DadoCreditosAutoresNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.CreditosAutores = null!;

            dto.CreditosAutores.Should().BeNull();
        }

        [Fact(DisplayName = "CreditosAutores - Quando vazio - Deve retornar vazio")]
        public void DadoCreditosAutoresVazio_QuandoAcessar_EntaoDeveRetornarVazio()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.CreditosAutores = string.Empty;

            dto.CreditosAutores.Should().Be(string.Empty);
        }

        [Fact(DisplayName = "SubTitulo - Quando atribuído - Deve retornar o valor")]
        public void DadoSubTituloAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var subTitulo = _faker.Lorem.Sentence();

            dto.SubTitulo = subTitulo;

            dto.SubTitulo.Should().Be(subTitulo);
        }

        [Fact(DisplayName = "SubTitulo - Quando nulo - Deve retornar nulo")]
        public void DadoSubTituloNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.SubTitulo = null!;

            dto.SubTitulo.Should().BeNull();
        }

        [Fact(DisplayName = "Material - Quando atribuído - Deve retornar o valor")]
        public void DadoMaterialAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var material = _faker.Lorem.Word();

            dto.Material = material;

            dto.Material.Should().Be(material);
        }

        [Fact(DisplayName = "Material - Quando nulo - Deve retornar nulo")]
        public void DadoMaterialNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.Material = null!;

            dto.Material.Should().BeNull();
        }

        [Fact(DisplayName = "Editora - Quando atribuído - Deve retornar o valor")]
        public void DadoEditoraAtribuida_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var editora = _faker.Company.CompanyName();

            dto.Editora = editora;

            dto.Editora.Should().Be(editora);
        }

        [Fact(DisplayName = "Editora - Quando nulo - Deve retornar nulo")]
        public void DadoEditoraNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.Editora = null!;

            dto.Editora.Should().BeNull();
        }

        [Fact(DisplayName = "Assuntos - Quando atribuído - Deve retornar o valor")]
        public void DadoAssuntosAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var assuntos = _faker.Lorem.Text();

            dto.Assuntos = assuntos;

            dto.Assuntos.Should().Be(assuntos);
        }

        [Fact(DisplayName = "Assuntos - Quando nulo - Deve retornar nulo")]
        public void DadoAssuntosNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.Assuntos = null!;

            dto.Assuntos.Should().BeNull();
        }

        [Fact(DisplayName = "Edicao - Quando atribuído - Deve retornar o valor")]
        public void DadoEdicaoAtribuida_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var edicao = "1ª Edição";

            dto.Edicao = edicao;

            dto.Edicao.Should().Be(edicao);
        }

        [Fact(DisplayName = "Edicao - Quando nulo - Deve retornar nulo")]
        public void DadoEdicaoNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.Edicao = null!;

            dto.Edicao.Should().BeNull();
        }

        [Fact(DisplayName = "NumeroPagina - Quando atribuído - Deve retornar o valor")]
        public void DadoNumeroPaginaAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var numeroPagina = _faker.Random.Int(1, 5000);

            dto.NumeroPagina = numeroPagina;

            dto.NumeroPagina.Should().Be(numeroPagina);
        }

        [Fact(DisplayName = "NumeroPagina - Quando zero - Deve retornar zero")]
        public void DadoNumeroPaginaComZero_QuandoAcessar_EntaoDeveRetornarZero()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.NumeroPagina = 0;

            dto.NumeroPagina.Should().Be(0);
        }

        [Fact(DisplayName = "NumeroPagina - Quando negativo - Deve retornar negativo")]
        public void DadoNumeroPaginaNegativo_QuandoAcessar_EntaoDeveRetornarNegativo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.NumeroPagina = -1;

            dto.NumeroPagina.Should().Be(-1);
        }

        [Fact(DisplayName = "Dimensoes - Quando atribuído - Deve retornar o valor")]
        public void DadoDimensoesAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var dimensoes = "20x30 cm";

            dto.Dimensoes = dimensoes;

            dto.Dimensoes.Should().Be(dimensoes);
        }

        [Fact(DisplayName = "Dimensoes - Quando nulo - Deve retornar nulo")]
        public void DadoDimensoesNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.Dimensoes = null!;

            dto.Dimensoes.Should().BeNull();
        }

        [Fact(DisplayName = "SerieColecao - Quando atribuído - Deve retornar o valor")]
        public void DadoSerieColecaoAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var serieColecao = _faker.Lorem.Sentence();

            dto.SerieColecao = serieColecao;

            dto.SerieColecao.Should().Be(serieColecao);
        }

        [Fact(DisplayName = "SerieColecao - Quando nulo - Deve retornar nulo")]
        public void DadoSerieColecaoNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.SerieColecao = null!;

            dto.SerieColecao.Should().BeNull();
        }

        [Fact(DisplayName = "Volume - Quando atribuído - Deve retornar o valor")]
        public void DadoVolumeAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var volume = "Volume 1";

            dto.Volume = volume;

            dto.Volume.Should().Be(volume);
        }

        [Fact(DisplayName = "Volume - Quando nulo - Deve retornar nulo")]
        public void DadoVolumeNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.Volume = null!;

            dto.Volume.Should().BeNull();
        }

        [Fact(DisplayName = "Idioma - Quando atribuído - Deve retornar o valor")]
        public void DadoIdiomaAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var idioma = "Português";

            dto.Idioma = idioma;

            dto.Idioma.Should().Be(idioma);
        }

        [Fact(DisplayName = "Idioma - Quando nulo - Deve retornar nulo")]
        public void DadoIdiomaNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.Idioma = null!;

            dto.Idioma.Should().BeNull();
        }

        [Fact(DisplayName = "Localizacao - Quando atribuído - Deve retornar o valor")]
        public void DadoLocalizacaoAtribuida_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var localizacao = "A1-B2";

            dto.Localizacao = localizacao;

            dto.Localizacao.Should().Be(localizacao);
        }

        [Fact(DisplayName = "Localizacao - Quando nulo - Deve retornar nulo")]
        public void DadoLocalizacaoNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.Localizacao = null!;

            dto.Localizacao.Should().BeNull();
        }

        [Fact(DisplayName = "NotasGerais - Quando atribuído - Deve retornar o valor")]
        public void DadoNotasGeraisAtribuidas_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var notas = _faker.Lorem.Paragraph();

            dto.NotasGerais = notas;

            dto.NotasGerais.Should().Be(notas);
        }

        [Fact(DisplayName = "NotasGerais - Quando nulo - Deve retornar nulo")]
        public void DadoNotasGeraisNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.NotasGerais = null!;

            dto.NotasGerais.Should().BeNull();
        }

        [Fact(DisplayName = "Isbn - Quando atribuído - Deve retornar o valor")]
        public void DadoIsbnAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var isbn = "978-3-16-148410-0";

            dto.Isbn = isbn;

            dto.Isbn.Should().Be(isbn);
        }

        [Fact(DisplayName = "Isbn - Quando nulo - Deve retornar nulo")]
        public void DadoIsbnNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            dto.Isbn = null!;

            dto.Isbn.Should().BeNull();
        }

        #endregion

        #region Testes de Múltiplas Propriedades

        [Fact(DisplayName = "Instância - Quando criada - Deve ter todas as properties nulas ou default")]
        public void DadoDtoNova_QuandoCriada_EntaoDeveSerValida()
        {
            var dto = new AcervoBibliograficoDetalheDTO();

            dto.Should().NotBeNull();
            dto.Titulo.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.Ano.Should().BeNull();
            dto.AcervoId.Should().Be(0);
            dto.EnderecoImagemPadrao.Should().BeNull();
            dto.SituacaoDisponibilidade.Should().BeNull();
            dto.EstaDisponivel.Should().BeFalse();
            dto.TemControleDisponibilidade.Should().BeFalse();
            dto.TipoAcervoId.Should().Be(0);
            dto.CreditosAutores.Should().BeNull();
            dto.SubTitulo.Should().BeNull();
            dto.Material.Should().BeNull();
            dto.Editora.Should().BeNull();
            dto.Assuntos.Should().BeNull();
            dto.Edicao.Should().BeNull();
            dto.NumeroPagina.Should().Be(0);
            dto.Dimensoes.Should().BeNull();
            dto.SerieColecao.Should().BeNull();
            dto.Volume.Should().BeNull();
            dto.Idioma.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.NotasGerais.Should().BeNull();
            dto.Isbn.Should().BeNull();
        }

        [Fact(DisplayName = "DTO Completo - Quando todas as propriedades atribuídas - Deve retornar todos os valores")]
        public void DadoDtoComTodasAsPropriedades_QuandoAtribuido_EntaoDeveRetornarTodos()
        {
            var dto = GerarAcervoBibliograficoDetalheDtoCompleto();

            dto.Titulo.Should().NotBeNullOrEmpty();
            dto.Codigo.Should().NotBeNullOrEmpty();
            dto.Ano.Should().NotBeNullOrEmpty();
            dto.AcervoId.Should().BeGreaterThan(0);
            dto.EnderecoImagemPadrao.Should().NotBeNullOrEmpty();
            dto.SituacaoDisponibilidade.Should().NotBeNullOrEmpty();
            dto.EstaDisponivel.Should().BeTrue();
            dto.TemControleDisponibilidade.Should().BeTrue();
            dto.TipoAcervoId.Should().BeGreaterThan(0);
            dto.CreditosAutores.Should().NotBeNullOrEmpty();
            dto.SubTitulo.Should().NotBeNullOrEmpty();
            dto.Material.Should().NotBeNullOrEmpty();
            dto.Editora.Should().NotBeNullOrEmpty();
            dto.Assuntos.Should().NotBeNullOrEmpty();
            dto.Edicao.Should().NotBeNullOrEmpty();
            dto.NumeroPagina.Should().BeGreaterThan(0);
            dto.Dimensoes.Should().NotBeNullOrEmpty();
            dto.SerieColecao.Should().NotBeNullOrEmpty();
            dto.Volume.Should().NotBeNullOrEmpty();
            dto.Idioma.Should().NotBeNullOrEmpty();
            dto.Localizacao.Should().NotBeNullOrEmpty();
            dto.NotasGerais.Should().NotBeNullOrEmpty();
            dto.Isbn.Should().NotBeNullOrEmpty();
        }

        [Fact(DisplayName = "Herança - Quando derivado de AcervoDetalheDTO - Deve herdar todas as propriedades")]
        public void DadoDtoDerivado_QuandoHerdar_EntaoDeveConterPropriedadesDaClasseBase()
        {
            var dto = new AcervoBibliograficoDetalheDTO();

            var baseType = dto.GetType().BaseType;

            baseType.Should().NotBeNull();
            baseType!.Name.Should().Be("AcervoDetalheDTO");
        }

        [Fact(DisplayName = "Properties - Quando modificadas múltiplas vezes - Deve manter último valor")]
        public void DadoPropertiesModificadas_QuandoMudarMultiplasVezes_EntaoDeveMantherUltimoValor()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var primeiroValor = "Primeiro";
            var segundoValor = "Segundo";
            var terceiroValor = "Terceiro";

            dto.CreditosAutores = primeiroValor;
            dto.CreditosAutores = segundoValor;
            dto.CreditosAutores = terceiroValor;

            dto.CreditosAutores.Should().Be(terceiroValor);
        }

        [Fact(DisplayName = "Properties - Quando números grandes - Deve aceitar valores máximos")]
        public void DadoPropertiesComValoresGrandes_QuandoAtribuir_EntaoDeveAceitar()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var acervoIdGrande = long.MaxValue;
            var tipoAcervoIdGrande = int.MaxValue;
            var numeroPaginaGrande = int.MaxValue;

            dto.AcervoId = acervoIdGrande;
            dto.TipoAcervoId = tipoAcervoIdGrande;
            dto.NumeroPagina = numeroPaginaGrande;

            dto.AcervoId.Should().Be(acervoIdGrande);
            dto.TipoAcervoId.Should().Be(tipoAcervoIdGrande);
            dto.NumeroPagina.Should().Be(numeroPaginaGrande);
        }

        [Fact(DisplayName = "Properties - Quando atribuídas com strings longas - Deve aceitar")]
        public void DadoPropertiesComStringsLongas_QuandoAtribuir_EntaoDeveAceitar()
        {
            var dto = new AcervoBibliograficoDetalheDTO();
            var stringLonga = new string('a', 10000);

            dto.CreditosAutores = stringLonga;
            dto.SubTitulo = stringLonga;
            dto.Material = stringLonga;
            dto.Editora = stringLonga;
            dto.Assuntos = stringLonga;
            dto.Edicao = stringLonga;
            dto.Dimensoes = stringLonga;
            dto.SerieColecao = stringLonga;
            dto.Volume = stringLonga;
            dto.Idioma = stringLonga;
            dto.Localizacao = stringLonga;
            dto.NotasGerais = stringLonga;
            dto.Isbn = stringLonga;

            dto.CreditosAutores.Should().HaveLength(10000);
            dto.SubTitulo.Should().HaveLength(10000);
            dto.Material.Should().HaveLength(10000);
            dto.Editora.Should().HaveLength(10000);
            dto.Assuntos.Should().HaveLength(10000);
            dto.Edicao.Should().HaveLength(10000);
            dto.Dimensoes.Should().HaveLength(10000);
            dto.SerieColecao.Should().HaveLength(10000);
            dto.Volume.Should().HaveLength(10000);
            dto.Idioma.Should().HaveLength(10000);
            dto.Localizacao.Should().HaveLength(10000);
            dto.NotasGerais.Should().HaveLength(10000);
            dto.Isbn.Should().HaveLength(10000);
        }

        [Fact(DisplayName = "Comparação - Quando duas instâncias diferentes - Devem ser diferentes")]
        public void DadoDuasInstancias_QuandoComValoresDiferentes_EntaoDevemSerDiferentes()
        {
            var dto1 = GerarAcervoBibliograficoDetalheDtoCompleto();
            var dto2 = new AcervoBibliograficoDetalheDTO { Titulo = "Outro Título" };

            dto1.Titulo.Should().NotBe(dto2.Titulo);
        }

        [Fact(DisplayName = "Tipos de dados - Quando verificar tipos primitivos - Deve ter tipos corretos")]
        public void DadoDtoComPropriedades_QuandoVerificarTipos_EntaoDevemSerCorretosOsTipos()
        {
            var dto = GerarAcervoBibliograficoDetalheDtoCompleto();
            var properties = dto.GetType().GetProperties();

            var stringProperties = properties.Where(p => p.PropertyType == typeof(string)).ToList();
            var intProperties = properties.Where(p => p.PropertyType == typeof(int)).ToList();
            var longProperties = properties.Where(p => p.PropertyType == typeof(long)).ToList();
            var boolProperties = properties.Where(p => p.PropertyType == typeof(bool)).ToList();

            stringProperties.Should().NotBeEmpty();
            intProperties.Should().NotBeEmpty();
            longProperties.Should().NotBeEmpty();
            boolProperties.Should().NotBeEmpty();
        }

        #endregion

        #region Métodos Auxiliares

        private static AcervoBibliograficoDetalheDTO GerarAcervoBibliograficoDetalheDtoCompleto()
        {
            return new Faker<AcervoBibliograficoDetalheDTO>("pt_BR")
                .RuleFor(x => x.Titulo, f => f.Lorem.Sentence())
                .RuleFor(x => x.Codigo, f => f.Random.AlphaNumeric(10))
                .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
                .RuleFor(x => x.AcervoId, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.EnderecoImagemPadrao, f => f.Internet.Url())
                .RuleFor(x => x.SituacaoDisponibilidade, f => "Disponível")
                .RuleFor(x => x.EstaDisponivel, f => true)
                .RuleFor(x => x.TemControleDisponibilidade, f => true)
                .RuleFor(x => x.TipoAcervoId, f => f.Random.Int(1, 10))
                .RuleFor(x => x.CreditosAutores, f => f.Lorem.Text())
                .RuleFor(x => x.SubTitulo, f => f.Lorem.Sentence())
                .RuleFor(x => x.Material, f => f.Lorem.Word())
                .RuleFor(x => x.Editora, f => f.Company.CompanyName())
                .RuleFor(x => x.Assuntos, f => f.Lorem.Text())
                .RuleFor(x => x.Edicao, f => "1ª Edição")
                .RuleFor(x => x.NumeroPagina, f => f.Random.Int(1, 5000))
                .RuleFor(x => x.Dimensoes, f => "20x30 cm")
                .RuleFor(x => x.SerieColecao, f => f.Lorem.Sentence())
                .RuleFor(x => x.Volume, f => "Volume 1")
                .RuleFor(x => x.Idioma, f => "Português")
                .RuleFor(x => x.Localizacao, f => "A1-B2")
                .RuleFor(x => x.NotasGerais, f => f.Lorem.Paragraph())
                .RuleFor(x => x.Isbn, f => "978-3-16-148410-0")
                .Generate();
        }

        #endregion
    }
}
