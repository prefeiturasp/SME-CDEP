using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Enumerados;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoBibliograficoDtoTeste
    {
        private readonly Faker _faker;

        public AcervoBibliograficoDtoTeste()
        {
            _faker = new Faker("pt_BR");
        }

        #region Testes de Properties

        [Fact(DisplayName = "Id - Quando atribuído - Deve retornar o valor")]
        public void DadoIdAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var id = _faker.Random.Long(1, 1000);

            dto.Id = id;

            dto.Id.Should().Be(id);
        }

        [Fact(DisplayName = "Id - Quando zero - Deve retornar zero")]
        public void DadoIdComZero_QuandoAcessar_EntaoDeveRetornarZero()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.Id = 0;

            dto.Id.Should().Be(0);
        }

        [Fact(DisplayName = "Id - Quando valor máximo - Deve retornar valor máximo")]
        public void DadoIdComValorMaximo_QuandoAcessar_EntaoDeveRetornarValorMaximo()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.Id = long.MaxValue;

            dto.Id.Should().Be(long.MaxValue);
        }

        [Fact(DisplayName = "AcervoId - Quando atribuído - Deve retornar o valor")]
        public void DadoAcervoIdAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var acervoId = _faker.Random.Long(1, 1000);

            dto.AcervoId = acervoId;

            dto.AcervoId.Should().Be(acervoId);
        }

        [Fact(DisplayName = "AcervoId - Quando zero - Deve retornar zero")]
        public void DadoAcervoIdComZero_QuandoAcessar_EntaoDeveRetornarZero()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.AcervoId = 0;

            dto.AcervoId.Should().Be(0);
        }

        [Fact(DisplayName = "Titulo - Quando atribuído - Deve retornar o valor")]
        public void DadoTituloAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var titulo = _faker.Lorem.Sentence();

            dto.Titulo = titulo;

            dto.Titulo.Should().Be(titulo);
        }

        [Fact(DisplayName = "Titulo - Quando vazio - Deve retornar vazio")]
        public void DadoTituloVazio_QuandoAcessar_EntaoDeveRetornarVazio()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.Titulo = string.Empty;

            dto.Titulo.Should().Be(string.Empty);
        }

        [Fact(DisplayName = "SubTitulo - Quando atribuído - Deve retornar o valor")]
        public void DadoSubTituloAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var subTitulo = _faker.Lorem.Sentence();

            dto.SubTitulo = subTitulo;

            dto.SubTitulo.Should().Be(subTitulo);
        }

        [Fact(DisplayName = "SubTitulo - Quando nulo - Deve retornar nulo")]
        public void DadoSubTituloNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.SubTitulo = null!;

            dto.SubTitulo.Should().BeNull();
        }

        [Fact(DisplayName = "TipoAcervoId - Quando atribuído - Deve retornar o valor")]
        public void DadoTipoAcervoIdAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var tipoAcervoId = _faker.Random.Long(1, 100);

            dto.TipoAcervoId = tipoAcervoId;

            dto.TipoAcervoId.Should().Be(tipoAcervoId);
        }

        [Fact(DisplayName = "TipoAcervoId - Quando zero - Deve retornar zero")]
        public void DadoTipoAcervoIdComZero_QuandoAcessar_EntaoDeveRetornarZero()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.TipoAcervoId = 0;

            dto.TipoAcervoId.Should().Be(0);
        }

        [Fact(DisplayName = "Codigo - Quando atribuído - Deve retornar o valor")]
        public void DadoCodigoAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var codigo = _faker.Random.AlphaNumeric(10);

            dto.Codigo = codigo;

            dto.Codigo.Should().Be(codigo);
        }

        [Fact(DisplayName = "Codigo - Quando vazio - Deve retornar vazio")]
        public void DadoCodigoVazio_QuandoAcessar_EntaoDeveRetornarVazio()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.Codigo = string.Empty;

            dto.Codigo.Should().Be(string.Empty);
        }

        [Fact(DisplayName = "MaterialId - Quando atribuído com valor - Deve retornar o valor")]
        public void DadoMaterialIdAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var materialId = _faker.Random.Long(1, 100);

            dto.MaterialId = materialId;

            dto.MaterialId.Should().Be(materialId);
        }

        [Fact(DisplayName = "MaterialId - Quando nulo - Deve retornar nulo")]
        public void DadoMaterialIdNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.MaterialId = null;

            dto.MaterialId.Should().BeNull();
        }

        [Fact(DisplayName = "EditoraId - Quando atribuído com valor - Deve retornar o valor")]
        public void DadoEditoraIdAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var editoraId = _faker.Random.Long(1, 100);

            dto.EditoraId = editoraId;

            dto.EditoraId.Should().Be(editoraId);
        }

        [Fact(DisplayName = "EditoraId - Quando nulo - Deve retornar nulo")]
        public void DadoEditoraIdNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.EditoraId = null;

            dto.EditoraId.Should().BeNull();
        }

        [Fact(DisplayName = "AssuntosIds - Quando atribuído - Deve retornar o array")]
        public void DadoAssuntosIdsAtribuido_QuandoAcessar_EntaoDeveRetornarOArray()
        {
            var dto = new AcervoBibliograficoDTO();
            var assuntosIds = new long[] { 1, 2, 3, 4, 5 };

            dto.AssuntosIds = assuntosIds;

            dto.AssuntosIds.Should().BeEquivalentTo(assuntosIds);
        }

        [Fact(DisplayName = "AssuntosIds - Quando array vazio - Deve retornar array vazio")]
        public void DadoAssuntosIdsVazio_QuandoAcessar_EntaoDeveRetornarArrayVazio()
        {
            var dto = new AcervoBibliograficoDTO();
            var assuntosIds = Array.Empty<long>();

            dto.AssuntosIds = assuntosIds;

            dto.AssuntosIds.Should().BeEmpty();
        }

        [Fact(DisplayName = "Ano - Quando atribuído - Deve retornar o valor")]
        public void DadoAnoAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var ano = _faker.Date.Past().Year.ToString();

            dto.Ano = ano;

            dto.Ano.Should().Be(ano);
        }

        [Fact(DisplayName = "Ano - Quando nulo - Deve retornar nulo")]
        public void DadoAnoNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.Ano = null;

            dto.Ano.Should().BeNull();
        }

        [Fact(DisplayName = "Edicao - Quando atribuído - Deve retornar o valor")]
        public void DadoEdicaoAtribuida_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var edicao = "1ª Edição";

            dto.Edicao = edicao;

            dto.Edicao.Should().Be(edicao);
        }

        [Fact(DisplayName = "Edicao - Quando nulo - Deve retornar nulo")]
        public void DadoEdicaoNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.Edicao = null;

            dto.Edicao.Should().BeNull();
        }

        [Fact(DisplayName = "NumeroPagina - Quando atribuído - Deve retornar o valor")]
        public void DadoNumeroPaginaAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var numeroPagina = _faker.Random.Int(1, 5000);

            dto.NumeroPagina = numeroPagina;

            dto.NumeroPagina.Should().Be(numeroPagina);
        }

        [Fact(DisplayName = "NumeroPagina - Quando nulo - Deve retornar nulo")]
        public void DadoNumeroPaginaNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.NumeroPagina = null;

            dto.NumeroPagina.Should().BeNull();
        }

        [Fact(DisplayName = "NumeroPagina - Quando zero - Deve retornar zero")]
        public void DadoNumeroPaginaComZero_QuandoAcessar_EntaoDeveRetornarZero()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.NumeroPagina = 0;

            dto.NumeroPagina.Should().Be(0);
        }

        [Fact(DisplayName = "Largura - Quando atribuído - Deve retornar o valor")]
        public void DadoLarguraAtribuida_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var largura = _faker.Random.Double(10, 50).ToString("F2");

            dto.Largura = largura;

            dto.Largura.Should().Be(largura);
        }

        [Fact(DisplayName = "Largura - Quando nulo - Deve retornar nulo")]
        public void DadoLarguraNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.Largura = null;

            dto.Largura.Should().BeNull();
        }

        [Fact(DisplayName = "Altura - Quando atribuído - Deve retornar o valor")]
        public void DadoAlturaAtribuida_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var altura = _faker.Random.Double(10, 50).ToString("F2");

            dto.Altura = altura;

            dto.Altura.Should().Be(altura);
        }

        [Fact(DisplayName = "Altura - Quando nulo - Deve retornar nulo")]
        public void DadoAlturaNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.Altura = null;

            dto.Altura.Should().BeNull();
        }

        [Fact(DisplayName = "SerieColecaoId - Quando atribuído - Deve retornar o valor")]
        public void DadoSerieColecaoIdAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var serieColecaoId = _faker.Random.Long(1, 100);

            dto.SerieColecaoId = serieColecaoId;

            dto.SerieColecaoId.Should().Be(serieColecaoId);
        }

        [Fact(DisplayName = "SerieColecaoId - Quando nulo - Deve retornar nulo")]
        public void DadoSerieColecaoIdNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.SerieColecaoId = null;

            dto.SerieColecaoId.Should().BeNull();
        }

        [Fact(DisplayName = "Volume - Quando atribuído - Deve retornar o valor")]
        public void DadoVolumeAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var volume = "Volume 1";

            dto.Volume = volume;

            dto.Volume.Should().Be(volume);
        }

        [Fact(DisplayName = "Volume - Quando nulo - Deve retornar nulo")]
        public void DadoVolumeNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.Volume = null;

            dto.Volume.Should().BeNull();
        }

        [Fact(DisplayName = "IdiomaId - Quando atribuído - Deve retornar o valor")]
        public void DadoIdiomaIdAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var idiomaId = _faker.Random.Long(1, 100);

            dto.IdiomaId = idiomaId;

            dto.IdiomaId.Should().Be(idiomaId);
        }

        [Fact(DisplayName = "IdiomaId - Quando zero - Deve retornar zero")]
        public void DadoIdiomaIdComZero_QuandoAcessar_EntaoDeveRetornarZero()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.IdiomaId = 0;

            dto.IdiomaId.Should().Be(0);
        }

        [Fact(DisplayName = "LocalizacaoCDD - Quando atribuído - Deve retornar o valor")]
        public void DadoLocalizacaoCDDAtribuida_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var localizacao = "000.00";

            dto.LocalizacaoCDD = localizacao;

            dto.LocalizacaoCDD.Should().Be(localizacao);
        }

        [Fact(DisplayName = "LocalizacaoCDD - Quando vazio - Deve retornar vazio")]
        public void DadoLocalizacaoCDDVazio_QuandoAcessar_EntaoDeveRetornarVazio()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.LocalizacaoCDD = string.Empty;

            dto.LocalizacaoCDD.Should().Be(string.Empty);
        }

        [Fact(DisplayName = "LocalizacaoPHA - Quando atribuído - Deve retornar o valor")]
        public void DadoLocalizacaoPHAAtribuida_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var localizacao = "A1-B2";

            dto.LocalizacaoPHA = localizacao;

            dto.LocalizacaoPHA.Should().Be(localizacao);
        }

        [Fact(DisplayName = "LocalizacaoPHA - Quando nulo - Deve retornar nulo")]
        public void DadoLocalizacaoPHANulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.LocalizacaoPHA = null;

            dto.LocalizacaoPHA.Should().BeNull();
        }

        [Fact(DisplayName = "NotasGerais - Quando atribuído - Deve retornar o valor")]
        public void DadoNotasGeraisAtribuidas_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var notas = _faker.Lorem.Paragraph();

            dto.NotasGerais = notas;

            dto.NotasGerais.Should().Be(notas);
        }

        [Fact(DisplayName = "NotasGerais - Quando nulo - Deve retornar nulo")]
        public void DadoNotasGeraisNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.NotasGerais = null;

            dto.NotasGerais.Should().BeNull();
        }

        [Fact(DisplayName = "Isbn - Quando atribuído - Deve retornar o valor")]
        public void DadoIsbnAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var isbn = "978-3-16-148410-0";

            dto.Isbn = isbn;

            dto.Isbn.Should().Be(isbn);
        }

        [Fact(DisplayName = "Isbn - Quando nulo - Deve retornar nulo")]
        public void DadoIsbnNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.Isbn = null;

            dto.Isbn.Should().BeNull();
        }

        [Fact(DisplayName = "Auditoria - Quando atribuído - Deve retornar o valor")]
        public void DadoAuditoriaAtribuida_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var auditoria = new AuditoriaDTO();

            dto.Auditoria = auditoria;

            dto.Auditoria.Should().Be(auditoria);
        }

        [Fact(DisplayName = "CreditosAutoresIds - Quando atribuído - Deve retornar o array")]
        public void DadoCreditosAutoresIdsAtribuido_QuandoAcessar_EntaoDeveRetornarOArray()
        {
            var dto = new AcervoBibliograficoDTO();
            var creditosIds = new long[] { 1, 2, 3 };

            dto.CreditosAutoresIds = creditosIds;

            dto.CreditosAutoresIds.Should().BeEquivalentTo(creditosIds);
        }

        [Fact(DisplayName = "CreditosAutoresIds - Quando array vazio - Deve retornar array vazio")]
        public void DadoCreditosAutoresIdsVazio_QuandoAcessar_EntaoDeveRetornarArrayVazio()
        {
            var dto = new AcervoBibliograficoDTO();
            var creditosIds = Array.Empty<long>();

            dto.CreditosAutoresIds = creditosIds;

            dto.CreditosAutoresIds.Should().BeEmpty();
        }

        [Fact(DisplayName = "CoAutores - Quando atribuído - Deve retornar o array")]
        public void DadoCoAutoresAtribuido_QuandoAcessar_EntaoDeveRetornarOArray()
        {
            var dto = new AcervoBibliograficoDTO();
            var coAutores = new CoAutorDTO[] { new CoAutorDTO() };

            dto.CoAutores = coAutores;

            dto.CoAutores.Should().BeEquivalentTo(coAutores);
        }

        [Fact(DisplayName = "CoAutores - Quando array vazio - Deve retornar array vazio")]
        public void DadoCoAutoresVazio_QuandoAcessar_EntaoDeveRetornarArrayVazio()
        {
            var dto = new AcervoBibliograficoDTO();
            var coAutores = Array.Empty<CoAutorDTO>();

            dto.CoAutores = coAutores;

            dto.CoAutores.Should().BeEmpty();
        }

        [Fact(DisplayName = "CoAutores - Quando nulo - Deve retornar nulo")]
        public void DadoCoAutoresNulo_QuandoAcessar_EntaoDeveRetornarNulo()
        {
            var dto = new AcervoBibliograficoDTO();
            dto.CoAutores = null;

            dto.CoAutores.Should().BeNull();
        }

        [Fact(DisplayName = "SituacaoSaldo - Quando atribuído - Deve retornar o valor")]
        public void DadoSituacaoSaldoAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var situacao = SituacaoSaldo.DISPONIVEL;

            dto.SituacaoSaldo = situacao;

            dto.SituacaoSaldo.Should().Be(situacao);
        }

        [Theory(DisplayName = "SituacaoSaldo - Quando diferentes enumerados - Deve aceitar todos")]
        [InlineData(SituacaoSaldo.DISPONIVEL)]
        [InlineData(SituacaoSaldo.EMPRESTADO)]
        [InlineData(SituacaoSaldo.RESERVADO)]
        [InlineData(SituacaoSaldo.INDISPONIVEL_PARA_RESERVA_EMPRESTIMO)]
        public void DadoSituacaoSaldoComDiferentesEnumerados_QuandoAtribuir_EntaoDeveSerValido(SituacaoSaldo situacao)
        {
            var dto = new AcervoBibliograficoDTO();

            dto.SituacaoSaldo = situacao;

            dto.SituacaoSaldo.Should().Be(situacao);
        }

        [Fact(DisplayName = "SituacaoAcervo - Quando atribuído - Deve retornar o valor")]
        public void DadoSituacaoAcervoAtribuido_QuandoAcessar_EntaoDeveRetornarOValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var situacao = SituacaoAcervo.Ativo;

            dto.SituacaoAcervo = situacao;

            dto.SituacaoAcervo.Should().Be(situacao);
        }

        [Theory(DisplayName = "SituacaoAcervo - Quando diferentes enumerados - Deve aceitar todos")]
        [InlineData(SituacaoAcervo.Ativo)]
        [InlineData(SituacaoAcervo.Inativo)]
        public void DadoSituacaoAcervoComDiferentesEnumerados_QuandoAtribuir_EntaoDeveSerValido(SituacaoAcervo situacao)
        {
            var dto = new AcervoBibliograficoDTO();

            dto.SituacaoAcervo = situacao;

            dto.SituacaoAcervo.Should().Be(situacao);
        }

        #endregion

        #region Testes de Múltiplas Propriedades   

        [Fact(DisplayName = "DTO Completo - Quando todas as propriedades atribuídas - Deve retornar todos os valores")]
        public void DadoDtoComTodasAsPropriedades_QuandoAtribuido_EntaoDeveRetornarTodos()
        {
            var dto = GerarAcervoBibliograficoDtoCompleto();

            dto.Id.Should().BeGreaterThan(0);
            dto.AcervoId.Should().BeGreaterThan(0);
            dto.Titulo.Should().NotBeNullOrEmpty();
            dto.SubTitulo.Should().NotBeNullOrEmpty();
            dto.TipoAcervoId.Should().BeGreaterThan(0);
            dto.Codigo.Should().NotBeNullOrEmpty();
            dto.MaterialId.Should().NotBeNull();
            dto.EditoraId.Should().NotBeNull();
            dto.AssuntosIds.Should().NotBeEmpty();
            dto.Ano.Should().NotBeNullOrEmpty();
            dto.Edicao.Should().NotBeNullOrEmpty();
            dto.NumeroPagina.Should().NotBeNull();
            dto.Largura.Should().NotBeNullOrEmpty();
            dto.Altura.Should().NotBeNullOrEmpty();
            dto.SerieColecaoId.Should().NotBeNull();
            dto.Volume.Should().NotBeNullOrEmpty();
            dto.IdiomaId.Should().BeGreaterThan(0);
            dto.LocalizacaoCDD.Should().NotBeNullOrEmpty();
            dto.LocalizacaoPHA.Should().NotBeNullOrEmpty();
            dto.NotasGerais.Should().NotBeNullOrEmpty();
            dto.Isbn.Should().NotBeNullOrEmpty();
            dto.Auditoria.Should().NotBeNull();
            dto.CreditosAutoresIds.Should().NotBeEmpty();
            dto.CoAutores.Should().NotBeEmpty();
            dto.SituacaoSaldo.Should().Be(SituacaoSaldo.DISPONIVEL);
            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Ativo);
        }

        [Fact(DisplayName = "Múltiplas atribuições - Quando modificar múltiplas vezes - Deve manter último valor")]
        public void DadoPropertiesModificadas_QuandoMudarMultiplasVezes_EntaoDeveMantherUltimoValor()
        {
            var dto = new AcervoBibliograficoDTO();
            var primeiroTitulo = "Primeiro Título";
            var segundoTitulo = "Segundo Título";
            var terceiroTitulo = "Terceiro Título";

            dto.Titulo = primeiroTitulo;
            dto.Titulo = segundoTitulo;
            dto.Titulo = terceiroTitulo;

            dto.Titulo.Should().Be(terceiroTitulo);
        }

        [Fact(DisplayName = "Valores máximos - Quando atribuir valores máximos - Deve aceitar")]
        public void DadoPropertiesComValoresMaximos_QuandoAtribuir_EntaoDeveAceitar()
        {
            var dto = new AcervoBibliograficoDTO();

            dto.Id = long.MaxValue;
            dto.AcervoId = long.MaxValue;
            dto.TipoAcervoId = long.MaxValue;
            dto.MaterialId = long.MaxValue;
            dto.EditoraId = long.MaxValue;
            dto.SerieColecaoId = long.MaxValue;
            dto.IdiomaId = long.MaxValue;
            dto.NumeroPagina = int.MaxValue;

            dto.Id.Should().Be(long.MaxValue);
            dto.AcervoId.Should().Be(long.MaxValue);
            dto.TipoAcervoId.Should().Be(long.MaxValue);
            dto.MaterialId.Should().Be(long.MaxValue);
            dto.EditoraId.Should().Be(long.MaxValue);
            dto.SerieColecaoId.Should().Be(long.MaxValue);
            dto.IdiomaId.Should().Be(long.MaxValue);
            dto.NumeroPagina.Should().Be(int.MaxValue);
        }

        [Fact(DisplayName = "Strings longas - Quando atribuir strings longas - Deve aceitar")]
        public void DadoPropertiesComStringsLongas_QuandoAtribuir_EntaoDeveAceitar()
        {
            var dto = new AcervoBibliograficoDTO();
            var stringLonga = new string('a', 10000);

            dto.Titulo = stringLonga;
            dto.SubTitulo = stringLonga;
            dto.Codigo = stringLonga;
            dto.Ano = stringLonga;
            dto.Edicao = stringLonga;
            dto.Largura = stringLonga;
            dto.Altura = stringLonga;
            dto.Volume = stringLonga;
            dto.LocalizacaoCDD = stringLonga;
            dto.LocalizacaoPHA = stringLonga;
            dto.NotasGerais = stringLonga;
            dto.Isbn = stringLonga;

            dto.Titulo.Should().HaveLength(10000);
            dto.SubTitulo.Should().HaveLength(10000);
            dto.Codigo.Should().HaveLength(10000);
            dto.Ano.Should().HaveLength(10000);
            dto.Edicao.Should().HaveLength(10000);
            dto.Largura.Should().HaveLength(10000);
            dto.Altura.Should().HaveLength(10000);
            dto.Volume.Should().HaveLength(10000);
            dto.LocalizacaoCDD.Should().HaveLength(10000);
            dto.LocalizacaoPHA.Should().HaveLength(10000);
            dto.NotasGerais.Should().HaveLength(10000);
            dto.Isbn.Should().HaveLength(10000);
        }

        [Fact(DisplayName = "Arrays com múltiplos itens - Quando atribuir arrays grandes - Deve aceitar")]
        public void DadoArraysComMultiplosItens_QuandoAtribuir_EntaoDeveAceitar()
        {
            var dto = new AcervoBibliograficoDTO();
            var assuntosIds = Enumerable.Range(1, 1000).Select(x => (long)x).ToArray();
            var creditosIds = Enumerable.Range(1, 500).Select(x => (long)x).ToArray();
            var coAutores = Enumerable.Range(1, 100).Select(x => new CoAutorDTO()).ToArray();

            dto.AssuntosIds = assuntosIds;
            dto.CreditosAutoresIds = creditosIds;
            dto.CoAutores = coAutores;

            dto.AssuntosIds.Should().HaveCount(1000);
            dto.CreditosAutoresIds.Should().HaveCount(500);
            dto.CoAutores.Should().HaveCount(100);
        }

        [Fact(DisplayName = "DTO com propriedades opcionais nulas - Quando instanciado - Deve permitir")]
        public void DadoDtoComPropriedadesOpcionaisNulas_QuandoInstanciar_EntaoDevePermitir()
        {
            var dto = new AcervoBibliograficoDTO
            {
                Id = 1,
                AcervoId = 1,
                Titulo = "Teste",
                TipoAcervoId = 1,
                IdiomaId = 1,
                LocalizacaoCDD = "000.00",
                MaterialId = null,
                EditoraId = null,
                AssuntosIds = Array.Empty<long>(),
                Ano = null,
                Edicao = null,
                NumeroPagina = null,
                Largura = null,
                Altura = null,
                SerieColecaoId = null,
                Volume = null,
                LocalizacaoPHA = null,
                NotasGerais = null,
                Isbn = null,
                Auditoria = new AuditoriaDTO(),
                CreditosAutoresIds = Array.Empty<long>(),
                CoAutores = null
            };

            dto.Should().NotBeNull();
            dto.MaterialId.Should().BeNull();
            dto.EditoraId.Should().BeNull();
            dto.AssuntosIds.Should().BeEmpty();
        }

        [Fact(DisplayName = "Clonagem de valores - Quando copiar propriedades entre DTOs - Deve manter valores")]
        public void DadoDtosComValores_QuandoCopiarPropriedades_EntaoDeveMantherValores()
        {
            var dto1 = GerarAcervoBibliograficoDtoCompleto();
            var dto2 = new AcervoBibliograficoDTO();

            dto2.Id = dto1.Id;
            dto2.AcervoId = dto1.AcervoId;
            dto2.Titulo = dto1.Titulo;
            dto2.SubTitulo = dto1.SubTitulo;
            dto2.TipoAcervoId = dto1.TipoAcervoId;
            dto2.Codigo = dto1.Codigo;
            dto2.MaterialId = dto1.MaterialId;
            dto2.EditoraId = dto1.EditoraId;
            dto2.AssuntosIds = dto1.AssuntosIds;
            dto2.Ano = dto1.Ano;
            dto2.Edicao = dto1.Edicao;
            dto2.NumeroPagina = dto1.NumeroPagina;
            dto2.Largura = dto1.Largura;
            dto2.Altura = dto1.Altura;
            dto2.SerieColecaoId = dto1.SerieColecaoId;
            dto2.Volume = dto1.Volume;
            dto2.IdiomaId = dto1.IdiomaId;
            dto2.LocalizacaoCDD = dto1.LocalizacaoCDD;
            dto2.LocalizacaoPHA = dto1.LocalizacaoPHA;
            dto2.NotasGerais = dto1.NotasGerais;
            dto2.Isbn = dto1.Isbn;
            dto2.Auditoria = dto1.Auditoria;
            dto2.CreditosAutoresIds = dto1.CreditosAutoresIds;
            dto2.CoAutores = dto1.CoAutores;
            dto2.SituacaoSaldo = dto1.SituacaoSaldo;
            dto2.SituacaoAcervo = dto1.SituacaoAcervo;

            dto2.Should().BeEquivalentTo(dto1);
        }

        [Fact(DisplayName = "Inicialização com objeto anônimo - Quando usar object initializer - Deve aceitar")]
        public void DadoDtoComObjectInitializer_QuandoInstanciar_EntaoDeveAceitar()
        {
            var dto = new AcervoBibliograficoDTO
            {
                Id = 1,
                AcervoId = 2,
                Titulo = "Título",
                SubTitulo = "SubTítulo",
                TipoAcervoId = 3,
                Codigo = "COD001",
                MaterialId = 4,
                EditoraId = 5,
                AssuntosIds = new long[] { 1, 2, 3 },
                Ano = "2024",
                Edicao = "1ª",
                NumeroPagina = 300,
                Largura = "20,00",
                Altura = "30,00",
                SerieColecaoId = 6,
                Volume = "Vol 1",
                IdiomaId = 7,
                LocalizacaoCDD = "000.00",
                LocalizacaoPHA = "A1",
                NotasGerais = "Notas",
                Isbn = "978-3-16-148410-0",
                Auditoria = new AuditoriaDTO(),
                CreditosAutoresIds = new long[] { 1 },
                CoAutores = new CoAutorDTO[] { new CoAutorDTO() },
                SituacaoSaldo = SituacaoSaldo.DISPONIVEL,
                SituacaoAcervo = SituacaoAcervo.Ativo
            };

            dto.Should().NotBeNull();
            dto.Id.Should().Be(1);
            dto.AcervoId.Should().Be(2);
            dto.Titulo.Should().Be("Título");
        }

        #endregion

        #region Métodos Auxiliares

        private static AcervoBibliograficoDTO GerarAcervoBibliograficoDtoCompleto()
        {
            return new Faker<AcervoBibliograficoDTO>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.AcervoId, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.Titulo, f => f.Lorem.Sentence())
                .RuleFor(x => x.SubTitulo, f => f.Lorem.Sentence())
                .RuleFor(x => x.TipoAcervoId, f => f.Random.Long(1, 100))
                .RuleFor(x => x.Codigo, f => f.Random.AlphaNumeric(10))
                .RuleFor(x => x.MaterialId, f => f.Random.Long(1, 100))
                .RuleFor(x => x.EditoraId, f => f.Random.Long(1, 100))
                .RuleFor(x => x.AssuntosIds, f => new long[] { f.Random.Long(1, 100), f.Random.Long(1, 100) })
                .RuleFor(x => x.Ano, f => f.Date.Past().Year.ToString())
                .RuleFor(x => x.Edicao, f => $"{f.Random.Int(1, 10)}ª Edição")
                .RuleFor(x => x.NumeroPagina, f => f.Random.Int(10, 1000))
                .RuleFor(x => x.Largura, f => f.Random.Double(10, 50).ToString("F2"))
                .RuleFor(x => x.Altura, f => f.Random.Double(10, 50).ToString("F2"))
                .RuleFor(x => x.SerieColecaoId, f => f.Random.Long(1, 100))
                .RuleFor(x => x.Volume, f => $"Volume {f.Random.Int(1, 10)}")
                .RuleFor(x => x.IdiomaId, f => f.Random.Long(1, 100))
                .RuleFor(x => x.LocalizacaoCDD, f => f.Random.Replace("###.##"))
                .RuleFor(x => x.LocalizacaoPHA, f => f.Random.AlphaNumeric(10))
                .RuleFor(x => x.NotasGerais, f => f.Lorem.Paragraph())
                .RuleFor(x => x.Isbn, f => "978-3-16-148410-0")
                .RuleFor(x => x.Auditoria, f => new AuditoriaDTO())
                .RuleFor(x => x.CreditosAutoresIds, f => new long[] { f.Random.Long(1, 100) })
                .RuleFor(x => x.CoAutores, f => new CoAutorDTO[] { new CoAutorDTO() })
                .RuleFor(x => x.SituacaoSaldo, f => SituacaoSaldo.DISPONIVEL)
                .RuleFor(x => x.SituacaoAcervo, f => SituacaoAcervo.Ativo)
                .Generate();
        }

        #endregion
    }
}
