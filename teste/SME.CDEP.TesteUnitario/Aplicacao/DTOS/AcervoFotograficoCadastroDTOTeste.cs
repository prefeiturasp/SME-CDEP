using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoFotograficoCadastroDtoTeste
    {
        #region Localizacao

        [Fact]
        public void DadoLocalizacaoVazia_QuandoCriarDTO_EntaoLocalizacaoDeveSerNull()
        {
            var dto = new AcervoFotograficoCadastroDTO();

            dto.Localizacao.Should().BeNull();
        }

        [Fact]
        public void DadoLocalizacao_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var localizacao = new Faker().Lorem.Sentence(3);
            var dto = new AcervoFotograficoCadastroDTO { Localizacao = localizacao };

            dto.Localizacao.Should().Be(localizacao);
        }

        [Fact]
        public void DadoLocalizacaoComMaxLength_QuandoAtribuir100Caracteres_EntaoDeveArmazenar()
        {
            var localizacao = new Faker().Random.String(100);
            var dto = new AcervoFotograficoCadastroDTO { Localizacao = localizacao };

            dto.Localizacao.Should().HaveLength(100);
        }

        [Fact]
        public void DadoLocalizacaoVazia_QuandoAtribuirString_EntaoDeveArmazenarVazia()
        {
            var dto = new AcervoFotograficoCadastroDTO { Localizacao = string.Empty };

            dto.Localizacao.Should().Be(string.Empty);
        }

        #endregion

        #region Procedencia

        [Fact]
        public void DadoProcedenciaVazia_QuandoCriarDTO_EntaoProcedenciaDeveSerNull()
        {
            var dto = new AcervoFotograficoCadastroDTO();

            dto.Procedencia.Should().BeNull();
        }

        [Fact]
        public void DadoProcedencia_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var procedencia = new Faker().Lorem.Sentence();
            var dto = new AcervoFotograficoCadastroDTO { Procedencia = procedencia };

            dto.Procedencia.Should().Be(procedencia);
        }

        [Fact]
        public void DadoProcedenciaComMaxLength_QuandoAtribuir200Caracteres_EntaoDeveArmazenar()
        {
            var procedencia = new Faker().Random.String(200);
            var dto = new AcervoFotograficoCadastroDTO { Procedencia = procedencia };

            dto.Procedencia.Should().HaveLength(200);
        }

        #endregion

        #region CopiaDigital

        [Fact]
        public void DadoCopiaDigitalVazia_QuandoCriarDTO_EntaoCopiaDigitalDeveSerNull()
        {
            var dto = new AcervoFotograficoCadastroDTO();

            dto.CopiaDigital.Should().BeNull();
        }

        [Fact]
        public void DadoCopiaDigitalTrue_QuandoAtribuir_EntaoDeveArmazenarTrue()
        {
            var dto = new AcervoFotograficoCadastroDTO { CopiaDigital = true };

            dto.CopiaDigital.Should().Be(true);
        }

        [Fact]
        public void DadoCopiaDigitalFalse_QuandoAtribuir_EntaoDeveArmazenarFalse()
        {
            var dto = new AcervoFotograficoCadastroDTO { CopiaDigital = false };

            dto.CopiaDigital.Should().Be(false);
        }

        #endregion

        #region PermiteUsoImagem

        [Fact]
        public void DadoPermiteUsoImagemVazia_QuandoCriarDTO_EntaoPermiteUsoImagemDeveSerNull()
        {
            var dto = new AcervoFotograficoCadastroDTO();

            dto.PermiteUsoImagem.Should().BeNull();
        }

        [Fact]
        public void DadoPermiteUsoImagemTrue_QuandoAtribuir_EntaoDeveArmazenarTrue()
        {
            var dto = new AcervoFotograficoCadastroDTO { PermiteUsoImagem = true };

            dto.PermiteUsoImagem.Should().Be(true);
        }

        [Fact]
        public void DadoPermiteUsoImagemFalse_QuandoAtribuir_EntaoDeveArmazenarFalse()
        {
            var dto = new AcervoFotograficoCadastroDTO { PermiteUsoImagem = false };
           
            dto.PermiteUsoImagem.Should().Be(false);
        }

        #endregion

        #region ConservacaoId

        [Fact]
        public void DadoConservacaoIdVazio_QuandoCriarDTO_EntaoConservacaoIdDeveSerZero()
        {
            var dto = new AcervoFotograficoCadastroDTO();

            dto.ConservacaoId.Should().Be(0);
        }

        [Fact]
        public void DadoConservacaoIdValido_QuandoAtribuir_EntaoDeveArmazenarCorretamente()
        {
            var conservacaoId = new Faker().Random.Long(1, 1000);
            var dto = new AcervoFotograficoCadastroDTO { ConservacaoId = conservacaoId };

            dto.ConservacaoId.Should().Be(conservacaoId);
        }

        [Fact]
        public void DadoConservacaoIdGrande_QuandoAtribuirLongMaxValue_EntaoDeveArmazenar()
        {
            var dto = new AcervoFotograficoCadastroDTO { ConservacaoId = long.MaxValue };

            dto.ConservacaoId.Should().Be(long.MaxValue);
        }

        #endregion

        #region Quantidade

        [Fact]
        public void DadoQuantidadeVazia_QuandoCriarDTO_EntaoQuantidadeDeveSerZero()
        {
            var dto = new AcervoFotograficoCadastroDTO();

            dto.Quantidade.Should().Be(0);
        }

        [Fact]
        public void DadoQuantidadeValida_QuandoAtribuir_EntaoDeveArmazenarCorretamente()
        {
            var quantidade = new Faker().Random.Int(1, 1000);
            var dto = new AcervoFotograficoCadastroDTO { Quantidade = quantidade };

            dto.Quantidade.Should().Be(quantidade);
        }

        [Fact]
        public void DadoQuantidadeMaxima_QuandoAtribuirIntMaxValue_EntaoDeveArmazenar()
        {
            var dto = new AcervoFotograficoCadastroDTO { Quantidade = int.MaxValue };

            dto.Quantidade.Should().Be(int.MaxValue);
        }

        #endregion

        #region Largura

        [Fact]
        public void DadoLarguraVazia_QuandoCriarDTO_EntaoLarguraDeveSerNull()
        {
            var dto = new AcervoFotograficoCadastroDTO();

            dto.Largura.Should().BeNull();
        }

        [Fact]
        public void DadoLargura_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var largura = new Faker().Random.String(10);
            var dto = new AcervoFotograficoCadastroDTO { Largura = largura };

            dto.Largura.Should().Be(largura);
        }

        [Fact]
        public void DadoLarguraVazia_QuandoAtribuirString_EntaoDeveArmazenarVazia()
        {
            var dto = new AcervoFotograficoCadastroDTO { Largura = string.Empty };

            dto.Largura.Should().Be(string.Empty);
        }

        #endregion

        #region Altura

        [Fact]
        public void DadoAlturaVazia_QuandoCriarDTO_EntaoAlturaDeveSerNull()
        {           
            var dto = new AcervoFotograficoCadastroDTO();

            dto.Altura.Should().BeNull();
        }

        [Fact]
        public void DadoAltura_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var altura = new Faker().Random.String(10);
            var dto = new AcervoFotograficoCadastroDTO { Altura = altura };

            dto.Altura.Should().Be(altura);
        }

        [Fact]
        public void DadoAlturaVazia_QuandoAtribuirString_EntaoDeveArmazenarVazia()
        {
            var dto = new AcervoFotograficoCadastroDTO { Altura = string.Empty };

            dto.Altura.Should().Be(string.Empty);
        }

        #endregion

        #region SuporteId

        [Fact]
        public void DadoSuporteIdVazio_QuandoCriarDTO_EntaoSuporteIdDeveSerZero()
        {
            var dto = new AcervoFotograficoCadastroDTO();

            dto.SuporteId.Should().Be(0);
        }

        [Fact]
        public void DadoSuporteIdValido_QuandoAtribuir_EntaoDeveArmazenarCorretamente()
        {
            var suporteId = new Faker().Random.Long(1, 1000);
            var dto = new AcervoFotograficoCadastroDTO { SuporteId = suporteId };

            dto.SuporteId.Should().Be(suporteId);
        }

        [Fact]
        public void DadoSuporteIdGrande_QuandoAtribuirLongMaxValue_EntaoDeveArmazenar()
        {
            var dto = new AcervoFotograficoCadastroDTO { SuporteId = long.MaxValue };

            dto.SuporteId.Should().Be(long.MaxValue);
        }

        #endregion

        #region FormatoId

        [Fact]
        public void DadoFormatoIdVazio_QuandoCriarDTO_EntaoFormatoIdDeveSerZero()
        {
            var dto = new AcervoFotograficoCadastroDTO();

            dto.FormatoId.Should().Be(0);
        }

        [Fact]
        public void DadoFormatoIdValido_QuandoAtribuir_EntaoDeveArmazenarCorretamente()
        {
            var formatoId = new Faker().Random.Long(1, 1000);
            var dto = new AcervoFotograficoCadastroDTO { FormatoId = formatoId };

            dto.FormatoId.Should().Be(formatoId);
        }

        [Fact]
        public void DadoFormatoIdGrande_QuandoAtribuirLongMaxValue_EntaoDeveArmazenar()
        {
            var dto = new AcervoFotograficoCadastroDTO { FormatoId = long.MaxValue };

            dto.FormatoId.Should().Be(long.MaxValue);
        }

        #endregion

        #region TamanhoArquivo

        [Fact]
        public void DadoTamanhoArquivoVazio_QuandoCriarDTO_EntaoTamanhoArquivoDeveSerNull()
        {
            var dto = new AcervoFotograficoCadastroDTO();

            dto.TamanhoArquivo.Should().BeNull();
        }

        [Fact]
        public void DadoTamanhoArquivo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var tamanhoArquivo = new Faker().Random.String(15);
            var dto = new AcervoFotograficoCadastroDTO { TamanhoArquivo = tamanhoArquivo };

            dto.TamanhoArquivo.Should().Be(tamanhoArquivo);
        }

        [Fact]
        public void DadoTamanhoArquivoMaxLength_QuandoAtribuir15Caracteres_EntaoDeveArmazenar()
        {
            var tamanhoArquivo = new Faker().Random.String(15);
            var dto = new AcervoFotograficoCadastroDTO { TamanhoArquivo = tamanhoArquivo };

            dto.TamanhoArquivo.Should().HaveLength(15);
        }

        #endregion

        #region CromiaId

        [Fact]
        public void DadoCromiaIdVazio_QuandoCriarDTO_EntaoCromiaIdDeveSerZero()
        {
            var dto = new AcervoFotograficoCadastroDTO();

            dto.CromiaId.Should().Be(0);
        }

        [Fact]
        public void DadoCromiaIdValido_QuandoAtribuir_EntaoDeveArmazenarCorretamente()
        {
            var cromiaId = new Faker().Random.Long(1, 1000);
            var dto = new AcervoFotograficoCadastroDTO { CromiaId = cromiaId };

            dto.CromiaId.Should().Be(cromiaId);
        }

        [Fact]
        public void DadoCromiaIdGrande_QuandoAtribuirLongMaxValue_EntaoDeveArmazenar()
        {
            var dto = new AcervoFotograficoCadastroDTO { CromiaId = long.MaxValue };

            dto.CromiaId.Should().Be(long.MaxValue);
        }

        #endregion

        #region Resolucao

        [Fact]
        public void DadoResolucaoVazia_QuandoCriarDTO_EntaoResolucaoDeveSerNull()
        {
            var dto = new AcervoFotograficoCadastroDTO();

            dto.Resolucao.Should().BeNull();
        }

        [Fact]
        public void DadoResolucao_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var resolucao = new Faker().Random.String(15);
            var dto = new AcervoFotograficoCadastroDTO { Resolucao = resolucao };

            dto.Resolucao.Should().Be(resolucao);
        }

        [Fact]
        public void DadoResolucaoMaxLength_QuandoAtribuir15Caracteres_EntaoDeveArmazenar()
        {
            var resolucao = new Faker().Random.String(15);
            var dto = new AcervoFotograficoCadastroDTO { Resolucao = resolucao };

            dto.Resolucao.Should().HaveLength(15);
        }

        #endregion

        #region Arquivos

        [Fact]
        public void DadoArquivosVazio_QuandoCriarDTO_EntaoArquivosDeveSerNull()
        {
            var dto = new AcervoFotograficoCadastroDTO();

            dto.Arquivos.Should().BeNull();
        }

        [Fact]
        public void DadoArquivos_QuandoAtribuirArray_EntaoDeveArmazenarCorretamente()
        {
            var arquivos = new[] { 1L, 2L, 3L };
            var dto = new AcervoFotograficoCadastroDTO { Arquivos = arquivos };

            dto.Arquivos.Should().BeEquivalentTo(arquivos);
        }

        [Fact]
        public void DadoArquivosVazio_QuandoAtribuirArrayVazio_EntaoDeveArmazenarVazio()
        {
            var arquivos = new long[] { };
            var dto = new AcervoFotograficoCadastroDTO { Arquivos = arquivos };

            dto.Arquivos.Should().BeEmpty();
        }

        [Fact]
        public void DadoArquivosUnico_QuandoAtribuirUmElemento_EntaoDeveArmazenarUmElemento()
        {
            var arquivos = new[] { 100L };
            var dto = new AcervoFotograficoCadastroDTO { Arquivos = arquivos };

            dto.Arquivos.Should().HaveCount(1);
            dto.Arquivos![0].Should().Be(100L);
        }

        #endregion

        #region Heranca - Propriedades Herdadas

        [Fact]
        public void DadoTitulo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var titulo = new Faker().Lorem.Sentence();
            var dto = new AcervoFotograficoCadastroDTO { Titulo = titulo };

            dto.Titulo.Should().Be(titulo);
        }

        [Fact]
        public void DadoDescricao_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var descricao = new Faker().Lorem.Paragraph();
            var dto = new AcervoFotograficoCadastroDTO { Descricao = descricao };

            dto.Descricao.Should().Be(descricao);
        }

        [Fact]
        public void DadoCodigo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        { 
            var codigo = new Faker().Random.String(10);
            var dto = new AcervoFotograficoCadastroDTO { Codigo = codigo };

            dto.Codigo.Should().Be(codigo);
        }

        [Fact]
        public void DadoCodigoNovo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var codigoNovo = new Faker().Random.String(10);
            var dto = new AcervoFotograficoCadastroDTO { CodigoNovo = codigoNovo };

            dto.CodigoNovo.Should().Be(codigoNovo);
        }

        [Fact]
        public void DadoCreditosAutoresIds_QuandoAtribuirArray_EntaoDeveArmazenarCorretamente()
        {
            var creditosAutoresIds = new[] { 1L, 2L, 3L };
            var dto = new AcervoFotograficoCadastroDTO { CreditosAutoresIds = creditosAutoresIds };

            dto.CreditosAutoresIds.Should().BeEquivalentTo(creditosAutoresIds);
        }

        [Fact]
        public void DadoCoAutores_QuandoAtribuirArray_EntaoDeveArmazenarCorretamente()
        {
            var coAutores = new[] { new CoAutorDTO { CreditoAutorNome = "Autor 1" } };
            var dto = new AcervoFotograficoCadastroDTO { CoAutores = coAutores };

            dto.CoAutores.Should().BeEquivalentTo(coAutores);
        }

        [Fact]
        public void DadoSubTitulo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var subTitulo = new Faker().Lorem.Sentence();
            var dto = new AcervoFotograficoCadastroDTO { SubTitulo = subTitulo };

            dto.SubTitulo.Should().Be(subTitulo);
        }

        [Fact]
        public void DadoDataAcervo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var dataAcervo = new Faker().Date.Past().ToString("dd/MM/yyyy");
            var dto = new AcervoFotograficoCadastroDTO { DataAcervo = dataAcervo };

            dto.DataAcervo.Should().Be(dataAcervo);
        }

        [Fact]
        public void DadoAno_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var ano = new Faker().Date.Past().Year.ToString();
            var dto = new AcervoFotograficoCadastroDTO { Ano = ano };

            dto.Ano.Should().Be(ano);
        }

        [Fact]
        public void DadoSituacaoAcervo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var situacao = SituacaoAcervo.Ativo;
            var dto = new AcervoFotograficoCadastroDTO { SituacaoAcervo = situacao };

            dto.SituacaoAcervo.Should().Be(situacao);
        }

        #endregion

        #region Testes de Integração - Múltiplas Propriedades

        [Fact]
        public void DadoDTOCompletoValido_QuandoInstanciarComTodosOsParametros_EntaoDeveArmazenarTodosCorretamente()
        {
            var faker = new Faker("pt_BR");
            var titulo = faker.Lorem.Sentence();
            var descricao = faker.Lorem.Paragraph();
            var localizacao = faker.Lorem.Sentence();
            var procedencia = faker.Lorem.Sentence();
            var quantidade = faker.Random.Int(1, 1000);
            var largura = faker.Random.String(10);
            var altura = faker.Random.String(10);
            var suporteId = faker.Random.Long(1, 1000);
            var formatoId = faker.Random.Long(1, 1000);
            var cromiaId = faker.Random.Long(1, 1000);
            var conservacaoId = faker.Random.Long(1, 1000);
            var tamanhoArquivo = faker.Random.String(10);
            var resolucao = faker.Random.String(10);
            var copiaDigital = faker.Random.Bool();
            var permiteUsoImagem = faker.Random.Bool();
            var arquivos = new[] { 1L, 2L, 3L };
            var ano = faker.Date.Past().Year.ToString();

            var dto = new AcervoFotograficoCadastroDTO
            {
                Titulo = titulo,
                Descricao = descricao,
                Localizacao = localizacao,
                Procedencia = procedencia,
                Quantidade = quantidade,
                Largura = largura,
                Altura = altura,
                SuporteId = suporteId,
                FormatoId = formatoId,
                CromiaId = cromiaId,
                ConservacaoId = conservacaoId,
                TamanhoArquivo = tamanhoArquivo,
                Resolucao = resolucao,
                CopiaDigital = copiaDigital,
                PermiteUsoImagem = permiteUsoImagem,
                Arquivos = arquivos,
                Ano = ano
            };

            dto.Titulo.Should().Be(titulo);
            dto.Descricao.Should().Be(descricao);
            dto.Localizacao.Should().Be(localizacao);
            dto.Procedencia.Should().Be(procedencia);
            dto.Quantidade.Should().Be(quantidade);
            dto.Largura.Should().Be(largura);
            dto.Altura.Should().Be(altura);
            dto.SuporteId.Should().Be(suporteId);
            dto.FormatoId.Should().Be(formatoId);
            dto.CromiaId.Should().Be(cromiaId);
            dto.ConservacaoId.Should().Be(conservacaoId);
            dto.TamanhoArquivo.Should().Be(tamanhoArquivo);
            dto.Resolucao.Should().Be(resolucao);
            dto.CopiaDigital.Should().Be(copiaDigital);
            dto.PermiteUsoImagem.Should().Be(permiteUsoImagem);
            dto.Arquivos.Should().BeEquivalentTo(arquivos);
            dto.Ano.Should().Be(ano);
        }

        [Fact]
        public void DadoDTOVazio_QuandoInstanciarSemParametros_EntaoDeveSerValido()
        {
            var dto = new AcervoFotograficoCadastroDTO();

            dto.Should().NotBeNull();
            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.PermiteUsoImagem.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Arquivos.Should().BeNull();
            dto.TamanhoArquivo.Should().BeNull();
            dto.Resolucao.Should().BeNull();
            dto.ConservacaoId.Should().Be(0);
            dto.Quantidade.Should().Be(0);
            dto.SuporteId.Should().Be(0);
            dto.FormatoId.Should().Be(0);
            dto.CromiaId.Should().Be(0);
        }

        [Fact]
        public void DadoDTOComValoresNulos_QuandoAtribuirExplicitamente_EntaoDeveArmazenarNull()
        {
            var dto = new AcervoFotograficoCadastroDTO
            {
                Localizacao = null,
                Procedencia = null!,
                CopiaDigital = null,
                PermiteUsoImagem = null,
                Largura = null,
                Altura = null,
                Arquivos = null
            };

            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.PermiteUsoImagem.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Arquivos.Should().BeNull();
        }

        #endregion
    }
}
