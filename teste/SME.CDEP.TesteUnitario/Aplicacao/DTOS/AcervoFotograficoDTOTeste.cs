using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoFotograficoDTOTeste
    {
        #region Id

        [Fact]
        public void DadoIdVazio_QuandoCriarDTO_EntaoIdDeveSerZero()
        {
            var dto = new AcervoFotograficoDTO();

            dto.Id.Should().Be(0);
        }

        [Fact]
        public void DadoId_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var id = new Faker().Random.Long(1, 1000);
            var dto = new AcervoFotograficoDTO { Id = id };

            dto.Id.Should().Be(id);
        }

        [Fact]
        public void DadoIdMaximo_QuandoAtribuirLongMaxValue_EntaoDeveArmazenar()
        {
            var dto = new AcervoFotograficoDTO { Id = long.MaxValue };

            dto.Id.Should().Be(long.MaxValue);
        }

        #endregion

        #region AcervoId

        [Fact]
        public void DadoAcervoIdVazio_QuandoCriarDTO_EntaoAcervoIdDeveSerZero()
        {
            var dto = new AcervoFotograficoDTO();

            dto.AcervoId.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoId_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var acervoId = new Faker().Random.Long(1, 1000);
            var dto = new AcervoFotograficoDTO { AcervoId = acervoId };

            dto.AcervoId.Should().Be(acervoId);
        }

        [Fact]
        public void DadoAcervoIdMaximo_QuandoAtribuirLongMaxValue_EntaoDeveArmazenar()
        {
            var dto = new AcervoFotograficoDTO { AcervoId = long.MaxValue };

            dto.AcervoId.Should().Be(long.MaxValue);
        }

        #endregion

        #region Titulo

        [Fact]
        public void DadoTituloVazio_QuandoCriarDTO_EntaoTituloDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.Titulo.Should().BeNull();
        }

        [Fact]
        public void DadoTitulo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var titulo = new Faker().Lorem.Sentence();
            var dto = new AcervoFotograficoDTO { Titulo = titulo };

            dto.Titulo.Should().Be(titulo);
        }

        [Fact]
        public void DadoTituloVazio_QuandoAtribuirString_EntaoDeveArmazenarVazio()
        {
            var dto = new AcervoFotograficoDTO { Titulo = string.Empty };

            dto.Titulo.Should().Be(string.Empty);
        }

        #endregion

        #region TipoAcervoId

        [Fact]
        public void DadoTipoAcervoIdVazio_QuandoCriarDTO_EntaoTipoAcervoIdDeveSerZero()
        {
            var dto = new AcervoFotograficoDTO();

            dto.TipoAcervoId.Should().Be(0);
        }

        [Fact]
        public void DadoTipoAcervoId_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var tipoAcervoId = new Faker().Random.Long(1, 1000);
            var dto = new AcervoFotograficoDTO { TipoAcervoId = tipoAcervoId };

            dto.TipoAcervoId.Should().Be(tipoAcervoId);
        }

        #endregion

        #region Codigo

        [Fact]
        public void DadoCodigoVazio_QuandoCriarDTO_EntaoCodigoDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.Codigo.Should().BeNull();
        }

        [Fact]
        public void DadoCodigo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var codigo = new Faker().Random.String(10);
            var dto = new AcervoFotograficoDTO { Codigo = codigo };

            dto.Codigo.Should().Be(codigo);
        }

        #endregion

        #region Localizacao

        [Fact]
        public void DadoLocalizacaoVazia_QuandoCriarDTO_EntaoLocalizacaoDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.Localizacao.Should().BeNull();
        }

        [Fact]
        public void DadoLocalizacao_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var localizacao = new Faker().Lorem.Sentence(3);
            var dto = new AcervoFotograficoDTO { Localizacao = localizacao };

            dto.Localizacao.Should().Be(localizacao);
        }

        #endregion

        #region Procedencia

        [Fact]
        public void DadoProcedenciaVazia_QuandoCriarDTO_EntaoProcedenciaDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.Procedencia.Should().BeNull();
        }

        [Fact]
        public void DadoProcedencia_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var procedencia = new Faker().Lorem.Sentence();
            var dto = new AcervoFotograficoDTO { Procedencia = procedencia };

            dto.Procedencia.Should().Be(procedencia);
        }

        #endregion

        #region DataAcervo

        [Fact]
        public void DadoDataAcervoVazia_QuandoCriarDTO_EntaoDataAcervoDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.DataAcervo.Should().BeNull();
        }

        [Fact]
        public void DadoDataAcervo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var dataAcervo = new Faker().Date.Past().ToString("dd/MM/yyyy");
            var dto = new AcervoFotograficoDTO { DataAcervo = dataAcervo };

            dto.DataAcervo.Should().Be(dataAcervo);
        }

        #endregion

        #region CopiaDigital

        [Fact]
        public void DadoCopiaDigitalVazia_QuandoCriarDTO_EntaoCopiaDigitalDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.CopiaDigital.Should().BeNull();
        }

        [Fact]
        public void DadoCopiaDigitalTrue_QuandoAtribuir_EntaoDeveArmazenarTrue()
        {
            var dto = new AcervoFotograficoDTO { CopiaDigital = true };

            dto.CopiaDigital.Should().Be(true);
        }

        [Fact]
        public void DadoCopiaDigitalFalse_QuandoAtribuir_EntaoDeveArmazenarFalse()
        {
            var dto = new AcervoFotograficoDTO { CopiaDigital = false };

            dto.CopiaDigital.Should().Be(false);
        }

        #endregion

        #region PermiteUsoImagem

        [Fact]
        public void DadoPermiteUsoImagemVazia_QuandoCriarDTO_EntaoPermiteUsoImagemDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.PermiteUsoImagem.Should().BeNull();
        }

        [Fact]
        public void DadoPermiteUsoImagemTrue_QuandoAtribuir_EntaoDeveArmazenarTrue()
        {
            var dto = new AcervoFotograficoDTO { PermiteUsoImagem = true };

            dto.PermiteUsoImagem.Should().Be(true);
        }

        [Fact]
        public void DadoPermiteUsoImagemFalse_QuandoAtribuir_EntaoDeveArmazenarFalse()
        {
            var dto = new AcervoFotograficoDTO { PermiteUsoImagem = false };

            dto.PermiteUsoImagem.Should().Be(false);
        }

        #endregion

        #region ConservacaoId

        [Fact]
        public void DadoConservacaoIdVazio_QuandoCriarDTO_EntaoConservacaoIdDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.ConservacaoId.Should().BeNull();
        }

        [Fact]
        public void DadoConservacaoId_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var conservacaoId = new Faker().Random.Long(1, 1000);
            var dto = new AcervoFotograficoDTO { ConservacaoId = conservacaoId };

            dto.ConservacaoId.Should().Be(conservacaoId);
        }

        [Fact]
        public void DadoConservacaoIdMaximo_QuandoAtribuirLongMaxValue_EntaoDeveArmazenar()
        {
            var dto = new AcervoFotograficoDTO { ConservacaoId = long.MaxValue };

            dto.ConservacaoId.Should().Be(long.MaxValue);
        }

        #endregion

        #region Descricao

        [Fact]
        public void DadoDescricaoVazia_QuandoCriarDTO_EntaoDescricaoDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.Descricao.Should().BeNull();
        }

        [Fact]
        public void DadoDescricao_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var descricao = new Faker().Lorem.Paragraph();
            var dto = new AcervoFotograficoDTO { Descricao = descricao };

            dto.Descricao.Should().Be(descricao);
        }

        #endregion

        #region Quantidade

        [Fact]
        public void DadoQuantidadeVazia_QuandoCriarDTO_EntaoQuantidadeDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.Quantidade.Should().BeNull();
        }

        [Fact]
        public void DadoQuantidade_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var quantidade = new Faker().Random.Long(1, 10000);
            var dto = new AcervoFotograficoDTO { Quantidade = quantidade };

            dto.Quantidade.Should().Be(quantidade);
        }

        [Fact]
        public void DadoQuantidadeMaxima_QuandoAtribuirLongMaxValue_EntaoDeveArmazenar()
        {
            var dto = new AcervoFotograficoDTO { Quantidade = long.MaxValue };

            dto.Quantidade.Should().Be(long.MaxValue);
        }

        #endregion

        #region Largura

        [Fact]
        public void DadoLarguraVazia_QuandoCriarDTO_EntaoLarguraDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.Largura.Should().BeNull();
        }

        [Fact]
        public void DadoLargura_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var largura = new Faker().Random.String(10);
            var dto = new AcervoFotograficoDTO { Largura = largura };

            dto.Largura.Should().Be(largura);
        }

        #endregion

        #region Altura

        [Fact]
        public void DadoAlturaVazia_QuandoCriarDTO_EntaoAlturaDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.Altura.Should().BeNull();
        }

        [Fact]
        public void DadoAltura_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var altura = new Faker().Random.String(10);
            var dto = new AcervoFotograficoDTO { Altura = altura };

            dto.Altura.Should().Be(altura);
        }

        #endregion

        #region SuporteId

        [Fact]
        public void DadoSuporteIdVazio_QuandoCriarDTO_EntaoSuporteIdDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.SuporteId.Should().BeNull();
        }

        [Fact]
        public void DadoSuporteId_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var suporteId = new Faker().Random.Long(1, 1000);
            var dto = new AcervoFotograficoDTO { SuporteId = suporteId };

            dto.SuporteId.Should().Be(suporteId);
        }

        #endregion

        #region FormatoId

        [Fact]
        public void DadoFormatoIdVazio_QuandoCriarDTO_EntaoFormatoIdDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.FormatoId.Should().BeNull();
        }

        [Fact]
        public void DadoFormatoId_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var formatoId = new Faker().Random.Long(1, 1000);
            var dto = new AcervoFotograficoDTO { FormatoId = formatoId };

            dto.FormatoId.Should().Be(formatoId);
        }

        #endregion

        #region CromiaId

        [Fact]
        public void DadoCromiaIdVazio_QuandoCriarDTO_EntaoCromiaIdDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.CromiaId.Should().BeNull();
        }

        [Fact]
        public void DadoCromiaId_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var cromiaId = new Faker().Random.Long(1, 1000);
            var dto = new AcervoFotograficoDTO { CromiaId = cromiaId };

            dto.CromiaId.Should().Be(cromiaId);
        }

        #endregion

        #region Resolucao

        [Fact]
        public void DadoResolucaoVazia_QuandoCriarDTO_EntaoResolucaoDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.Resolucao.Should().BeNull();
        }

        [Fact]
        public void DadoResolucao_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var resolucao = new Faker().Random.String(15);
            var dto = new AcervoFotograficoDTO { Resolucao = resolucao };

            dto.Resolucao.Should().Be(resolucao);
        }

        #endregion

        #region TamanhoArquivo

        [Fact]
        public void DadoTamanhoArquivoVazio_QuandoCriarDTO_EntaoTamanhoArquivoDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.TamanhoArquivo.Should().BeNull();
        }

        [Fact]
        public void DadoTamanhoArquivo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var tamanhoArquivo = new Faker().Random.String(15);
            var dto = new AcervoFotograficoDTO { TamanhoArquivo = tamanhoArquivo };

            dto.TamanhoArquivo.Should().Be(tamanhoArquivo);
        }

        #endregion

        #region Arquivos

        [Fact]
        public void DadoArquivosVazio_QuandoCriarDTO_EntaoArquivosDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.Arquivos.Should().BeNull();
        }

        [Fact]
        public void DadoArquivos_QuandoAtribuirArray_EntaoDeveArmazenarCorretamente()
        {
            var arquivos = new[] 
            { 
                new ArquivoResumidoDTO { Id = 1, Nome = "Arquivo1" },
                new ArquivoResumidoDTO { Id = 2, Nome = "Arquivo2" }
            };
            var dto = new AcervoFotograficoDTO { Arquivos = arquivos };

            dto.Arquivos.Should().HaveCount(2);
            dto.Arquivos.Should().BeEquivalentTo(arquivos);
        }

        [Fact]
        public void DadoArquivosVazio_QuandoAtribuirArrayVazio_EntaoDeveArmazenarVazio()
        {
            var arquivos = Array.Empty<ArquivoResumidoDTO>();
            var dto = new AcervoFotograficoDTO { Arquivos = arquivos };

            dto.Arquivos.Should().BeEmpty();
        }

        #endregion

        #region Auditoria

        [Fact]
        public void DadoAuditoriaVazia_QuandoCriarDTO_EntaoAuditoriaDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.Auditoria.Should().BeNull();
        }

        [Fact]
        public void DadoAuditoria_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var auditoria = new AuditoriaDTO
            {
                CriadoEm = DateTime.Now,
                CriadoPor = "Usuario1",
                AlteradoEm = DateTime.Now,
                AlteradoPor = "Usuario2"
            };
            var dto = new AcervoFotograficoDTO { Auditoria = auditoria };

            dto.Auditoria.Should().NotBeNull();
            dto.Auditoria.CriadoPor.Should().Be("Usuario1");
            dto.Auditoria.AlteradoPor.Should().Be("Usuario2");
        }

        #endregion

        #region CreditosAutoresIds

        [Fact]
        public void DadoCreditosAutoresIdsVazio_QuandoCriarDTO_EntaoCreditosAutoresIdsDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.CreditosAutoresIds.Should().BeNull();
        }

        [Fact]
        public void DadoCreditosAutoresIds_QuandoAtribuirArray_EntaoDeveArmazenarCorretamente()
        {
            var creditosAutoresIds = new[] { 1L, 2L, 3L };
            var dto = new AcervoFotograficoDTO { CreditosAutoresIds = creditosAutoresIds };

            dto.CreditosAutoresIds.Should().BeEquivalentTo(creditosAutoresIds);
        }

        [Fact]
        public void DadoCreditosAutoresIdsVazio_QuandoAtribuirArrayVazio_EntaoDeveArmazenarVazio()
        {
            var creditosAutoresIds = Array.Empty<long>();
            var dto = new AcervoFotograficoDTO { CreditosAutoresIds = creditosAutoresIds };

            dto.CreditosAutoresIds.Should().BeEmpty();
        }

        #endregion

        #region Ano

        [Fact]
        public void DadoAnoVazio_QuandoCriarDTO_EntaoAnoDeveSerNull()
        {
            var dto = new AcervoFotograficoDTO();

            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoAno_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var ano = new Faker().Date.Past().Year.ToString();
            var dto = new AcervoFotograficoDTO { Ano = ano };

            dto.Ano.Should().Be(ano);
        }

        #endregion

        #region SituacaoAcervo

        [Fact]
        public void DadoSituacaoAcervoVazia_QuandoCriarDTO_EntaoSituacaoAcervoDeveSerValorPadrao()
        {
            var dto = new AcervoFotograficoDTO();

            dto.SituacaoAcervo.Should().Be(default(SituacaoAcervo));
        }

        [Fact]
        public void DadoSituacaoAcervo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var situacao = SituacaoAcervo.Ativo;
            var dto = new AcervoFotograficoDTO { SituacaoAcervo = situacao };

            dto.SituacaoAcervo.Should().Be(situacao);
        }

        [Fact]
        public void DadoSituacaoAcervoInativo_QuandoAtribuir_EntaoDeveArmazenarInativo()
        {
            var dto = new AcervoFotograficoDTO { SituacaoAcervo = SituacaoAcervo.Inativo };

            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Inativo);
        }

        #endregion

        #region Testes de Integração - Múltiplas Propriedades

        [Fact]
        public void DadoDTOCompletoValido_QuandoInstanciarComTodosOsParametros_EntaoDeveArmazenarTodosCorretamente()
        {
            var faker = new Faker("pt_BR");
            var id = faker.Random.Long(1, 1000);
            var acervoId = faker.Random.Long(1, 1000);
            var titulo = faker.Lorem.Sentence();
            var tipoAcervoId = faker.Random.Long(1, 1000);
            var codigo = faker.Random.String(10);
            var localizacao = faker.Lorem.Sentence();
            var procedencia = faker.Lorem.Sentence();
            var dataAcervo = faker.Date.Past().ToString("dd/MM/yyyy");
            var copiaDigital = faker.Random.Bool();
            var permiteUsoImagem = faker.Random.Bool();
            var conservacaoId = faker.Random.Long(1, 1000);
            var descricao = faker.Lorem.Paragraph();
            var quantidade = faker.Random.Long(1, 10000);
            var largura = faker.Random.String(10);
            var altura = faker.Random.String(10);
            var suporteId = faker.Random.Long(1, 1000);
            var formatoId = faker.Random.Long(1, 1000);
            var cromiaId = faker.Random.Long(1, 1000);
            var resolucao = faker.Random.String(15);
            var tamanhoArquivo = faker.Random.String(15);
            var arquivos = new[] 
            { 
                new ArquivoResumidoDTO { Id = 1, Nome = "Arquivo1" },
                new ArquivoResumidoDTO { Id = 2, Nome = "Arquivo2" }
            };
            var auditoria = new AuditoriaDTO
            {
                CriadoEm = DateTime.Now,
                CriadoPor = "Usuario1"
            };
            var creditosAutoresIds = new[] { 1L, 2L, 3L };
            var ano = faker.Date.Past().Year.ToString();
            var situacao = SituacaoAcervo.Ativo;

            var dto = new AcervoFotograficoDTO
            {
                Id = id,
                AcervoId = acervoId,
                Titulo = titulo,
                TipoAcervoId = tipoAcervoId,
                Codigo = codigo,
                Localizacao = localizacao,
                Procedencia = procedencia,
                DataAcervo = dataAcervo,
                CopiaDigital = copiaDigital,
                PermiteUsoImagem = permiteUsoImagem,
                ConservacaoId = conservacaoId,
                Descricao = descricao,
                Quantidade = quantidade,
                Largura = largura,
                Altura = altura,
                SuporteId = suporteId,
                FormatoId = formatoId,
                CromiaId = cromiaId,
                Resolucao = resolucao,
                TamanhoArquivo = tamanhoArquivo,
                Arquivos = arquivos,
                Auditoria = auditoria,
                CreditosAutoresIds = creditosAutoresIds,
                Ano = ano,
                SituacaoAcervo = situacao
            };

            dto.Id.Should().Be(id);
            dto.AcervoId.Should().Be(acervoId);
            dto.Titulo.Should().Be(titulo);
            dto.TipoAcervoId.Should().Be(tipoAcervoId);
            dto.Codigo.Should().Be(codigo);
            dto.Localizacao.Should().Be(localizacao);
            dto.Procedencia.Should().Be(procedencia);
            dto.DataAcervo.Should().Be(dataAcervo);
            dto.CopiaDigital.Should().Be(copiaDigital);
            dto.PermiteUsoImagem.Should().Be(permiteUsoImagem);
            dto.ConservacaoId.Should().Be(conservacaoId);
            dto.Descricao.Should().Be(descricao);
            dto.Quantidade.Should().Be(quantidade);
            dto.Largura.Should().Be(largura);
            dto.Altura.Should().Be(altura);
            dto.SuporteId.Should().Be(suporteId);
            dto.FormatoId.Should().Be(formatoId);
            dto.CromiaId.Should().Be(cromiaId);
            dto.Resolucao.Should().Be(resolucao);
            dto.TamanhoArquivo.Should().Be(tamanhoArquivo);
            dto.Arquivos.Should().BeEquivalentTo(arquivos);
            dto.Auditoria.Should().NotBeNull();
            dto.CreditosAutoresIds.Should().BeEquivalentTo(creditosAutoresIds);
            dto.Ano.Should().Be(ano);
            dto.SituacaoAcervo.Should().Be(situacao);
        }

        [Fact]
        public void DadoDTOVazio_QuandoInstanciarSemParametros_EntaoDeveSerValido()
        {
            var dto = new AcervoFotograficoDTO();

            dto.Should().NotBeNull();
            dto.Id.Should().Be(0);
            dto.AcervoId.Should().Be(0);
            dto.Titulo.Should().BeNull();
            dto.TipoAcervoId.Should().Be(0);
            dto.Codigo.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.DataAcervo.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.PermiteUsoImagem.Should().BeNull();
            dto.ConservacaoId.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.Quantidade.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.SuporteId.Should().BeNull();
            dto.FormatoId.Should().BeNull();
            dto.CromiaId.Should().BeNull();
            dto.Resolucao.Should().BeNull();
            dto.TamanhoArquivo.Should().BeNull();
            dto.Arquivos.Should().BeNull();
            dto.Auditoria.Should().BeNull();
            dto.CreditosAutoresIds.Should().BeNull();
            dto.Ano.Should().BeNull();
            dto.SituacaoAcervo.Should().Be(default(SituacaoAcervo));
        }

        [Fact]
        public void DadoDTOComValoresNulos_QuandoAtribuirExplicitamente_EntaoDeveArmazenarNull()
        {
            var dto = new AcervoFotograficoDTO
            {
                Titulo = null,
                Codigo = null,
                Localizacao = null,
                Procedencia = null,
                DataAcervo = null,
                CopiaDigital = null,
                PermiteUsoImagem = null,
                ConservacaoId = null,
                Descricao = null,
                Quantidade = null,
                Largura = null,
                Altura = null,
                SuporteId = null,
                FormatoId = null,
                CromiaId = null,
                Resolucao = null,
                TamanhoArquivo = null,
                Arquivos = null,
                Auditoria = null,
                CreditosAutoresIds = null,
                Ano = null
            };

            dto.Titulo.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.DataAcervo.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.PermiteUsoImagem.Should().BeNull();
            dto.ConservacaoId.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.Quantidade.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.SuporteId.Should().BeNull();
            dto.FormatoId.Should().BeNull();
            dto.CromiaId.Should().BeNull();
            dto.Resolucao.Should().BeNull();
            dto.TamanhoArquivo.Should().BeNull();
            dto.Arquivos.Should().BeNull();
            dto.Auditoria.Should().BeNull();
            dto.CreditosAutoresIds.Should().BeNull();
            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComMultiplosArquivos_QuandoAtribuirVariosArquivos_EntaoDeveArmazenarTodos()
        {
            var faker = new Faker("pt_BR");
            var arquivos = Enumerable.Range(1, 5)
                .Select(i => new ArquivoResumidoDTO 
                { 
                    Id = i,
                    Nome = $"Arquivo{i}.jpg" 
                })
                .ToArray();

            var dto = new AcervoFotograficoDTO { Arquivos = arquivos };

            dto.Arquivos.Should().HaveCount(5);
            dto.Arquivos.Should().AllSatisfy(arq => arq.Nome.Should().EndWith(".jpg"));
        }

        [Fact]
        public void DadoDTOComMultiplosCreditosAutores_QuandoAtribuirVariosCreditos_EntaoDeveArmazenarTodos()
        {
            var creditosAutoresIds = new[] { 1L, 2L, 3L, 4L, 5L };
            var dto = new AcervoFotograficoDTO { CreditosAutoresIds = creditosAutoresIds };

            dto.CreditosAutoresIds.Should().HaveCount(5);
            dto.CreditosAutoresIds.Should().Contain(new[] { 1L, 2L, 3L, 4L, 5L });
        }

        [Fact]
        public void DadoDTOComTodosValoresNulos_QuandoAcesarAuditoria_EntaoDeveRetornarNull()
        {
            var dto = new AcervoFotograficoDTO();

            var exception = Record.Exception(() =>
            {
                _ = dto.Auditoria;
            });

            exception.Should().BeNull();
            dto.Auditoria.Should().BeNull();
        }

        [Fact]
        public void DadoDTOInstanciado_QuandoAcessarMultiplasVezes_EntaoValoresPermanecem()
        {
            var titulo = "Titulo Teste";
            var codigo = "COD001";
            var dto = new AcervoFotograficoDTO
            {
                Titulo = titulo,
                Codigo = codigo
            };

            var titulo1 = dto.Titulo;
            var codigo1 = dto.Codigo;
            var titulo2 = dto.Titulo;
            var codigo2 = dto.Codigo;

            titulo1.Should().Be(titulo);
            codigo1.Should().Be(codigo);
            titulo2.Should().Be(titulo);
            codigo2.Should().Be(codigo);
        }

        [Fact]
        public void DadoPropriedadesDoBoolNullavel_QuandoAlternarValores_EntaoAlternaCorretamente()
        {
            var dto = new AcervoFotograficoDTO();

            dto.CopiaDigital = true;
            dto.CopiaDigital.Should().Be(true);

            dto.CopiaDigital = false;
            dto.CopiaDigital.Should().Be(false);

            dto.CopiaDigital = null;
            dto.CopiaDigital.Should().BeNull();
        }

        [Fact]
        public void DadoResolucaoETamanhoArquivo_QuandoAtribuirValoresValidos_EntaoArmazenamCorretamente()
        {
            var resolucao = "300DPI";
            var tamanhoArquivo = "5MB";
            var dto = new AcervoFotograficoDTO
            {
                Resolucao = resolucao,
                TamanhoArquivo = tamanhoArquivo
            };

            dto.Resolucao.Should().Be(resolucao);
            dto.TamanhoArquivo.Should().Be(tamanhoArquivo);
        }

        [Fact]
        public void DadoDimensoes_QuandoAtribuirLarguraEAltura_EntaoArmazenamCorretamente()
        {
            var largura = "20cm";
            var altura = "30cm";
            var dto = new AcervoFotograficoDTO
            {
                Largura = largura,
                Altura = altura
            };

            dto.Largura.Should().Be(largura);
            dto.Altura.Should().Be(altura);
        }

        [Fact]
        public void DadoIdsComValoresSequenciais_QuandoAtribuir_EntaoArmazenamCorretamente()
        {
            var id = 1L;
            var acervoId = 2L;
            var tipoAcervoId = 3L;
            var conservacaoId = 4L;
            var suporteId = 5L;
            var formatoId = 6L;
            var cromiaId = 7L;

            var dto = new AcervoFotograficoDTO
            {
                Id = id,
                AcervoId = acervoId,
                TipoAcervoId = tipoAcervoId,
                ConservacaoId = conservacaoId,
                SuporteId = suporteId,
                FormatoId = formatoId,
                CromiaId = cromiaId
            };

            dto.Id.Should().Be(id);
            dto.AcervoId.Should().Be(acervoId);
            dto.TipoAcervoId.Should().Be(tipoAcervoId);
            dto.ConservacaoId.Should().Be(conservacaoId);
            dto.SuporteId.Should().Be(suporteId);
            dto.FormatoId.Should().Be(formatoId);
            dto.CromiaId.Should().Be(cromiaId);
        }

        [Fact]
        public void DadoDuasInstancias_QuandoComMesmosValores_EntaoSaoInstanciasDistintas()
        {
            var dto1 = new AcervoFotograficoDTO
            {
                Id = 1,
                AcervoId = 2,
                Titulo = "Título"
            };

            var dto2 = new AcervoFotograficoDTO
            {
                Id = 1,
                AcervoId = 2,
                Titulo = "Título"
            };

            dto1.Should().NotBeSameAs(dto2);
            dto1.Id.Should().Be(dto2.Id);
            dto1.Titulo.Should().Be(dto2.Titulo);
        }

        #endregion
    }
}
