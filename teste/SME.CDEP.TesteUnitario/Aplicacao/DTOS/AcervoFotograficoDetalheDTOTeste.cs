using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoFotograficoDetalheDtoTeste
    {
        #region Descricao

        [Fact]
        public void DadoDescricaoVazia_QuandoCriarDTO_EntaoDescricaoDeveSerNull()
        {
            var dto = new AcervoFotograficoDetalheDTO();

            dto.Descricao.Should().BeNull();
        }

        [Fact]
        public void DadoDescricao_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var descricao = new Faker().Lorem.Paragraph();
            var dto = new AcervoFotograficoDetalheDTO { Descricao = descricao };

            dto.Descricao.Should().Be(descricao);
        }

        [Fact]
        public void DadoDescricaoComCaracteresEspeciais_QuandoAtribuir_EntaoDeveArmazenarCompleto()
        {
            var descricao = "Descrição com &quot;aspas&quot;, números 123 e símbolos @#$%.";
            var dto = new AcervoFotograficoDetalheDTO { Descricao = descricao };

            dto.Descricao.Should().Be(descricao);
        }

        [Fact]
        public void DadoDescricaoVazia_QuandoAtribuirString_EntaoDeveArmazenarVazia()
        {
            var dto = new AcervoFotograficoDetalheDTO { Descricao = string.Empty };

            dto.Descricao.Should().Be(string.Empty);
        }

        #endregion

        #region CreditosAutores

        [Fact]
        public void DadoCreditosAutoresVazio_QuandoCriarDTO_EntaoCreditosAutoresDeveSerNull()
        {
            var dto = new AcervoFotograficoDetalheDTO();

            dto.CreditosAutores.Should().BeNull();
        }

        [Fact]
        public void DadoCreditosAutores_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var creditosAutores = new Faker().Lorem.Sentence();
            var dto = new AcervoFotograficoDetalheDTO { CreditosAutores = creditosAutores };

            dto.CreditosAutores.Should().Be(creditosAutores);
        }

        [Fact]
        public void DadoCreditosAutoresComMultiplosAutores_QuandoAtribuir_EntaoDeveArmazenarTodos()
        {
            var creditosAutores = "Autor 1 | Autor 2 | Autor 3";
            var dto = new AcervoFotograficoDetalheDTO { CreditosAutores = creditosAutores };

            dto.CreditosAutores.Should().Be(creditosAutores);
            dto.CreditosAutores.Should().Contain("|");
        }

        #endregion

        #region DataAcervo

        [Fact]
        public void DadoDataAcervoVazia_QuandoCriarDTO_EntaoDataAcervoDeveSerNull()
        {
            var dto = new AcervoFotograficoDetalheDTO();

            dto.DataAcervo.Should().BeNull();
        }

        [Fact]
        public void DadoDataAcervo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var dataAcervo = new Faker().Date.Past().ToString("dd/MM/yyyy");
            var dto = new AcervoFotograficoDetalheDTO { DataAcervo = dataAcervo };

            dto.DataAcervo.Should().Be(dataAcervo);
        }

        [Fact]
        public void DadoDataAcervoComFormatoCompleto_QuandoAtribuir_EntaoDeveArmazenarComTempo()
        {
            var dataAcervo = "01/01/2020 10:30:45";
            var dto = new AcervoFotograficoDetalheDTO { DataAcervo = dataAcervo };

            dto.DataAcervo.Should().Be(dataAcervo);
        }

        #endregion

        #region Localizacao

        [Fact]
        public void DadoLocalizacaoVazia_QuandoCriarDTO_EntaoLocalizacaoDeveSerNull()
        {
            var dto = new AcervoFotograficoDetalheDTO();

            dto.Localizacao.Should().BeNull();
        }

        [Fact]
        public void DadoLocalizacao_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var localizacao = new Faker().Address.StreetName();
            var dto = new AcervoFotograficoDetalheDTO { Localizacao = localizacao };

            dto.Localizacao.Should().Be(localizacao);
        }

        [Fact]
        public void DadoLocalizacaoComPrefixoSigla_QuandoAtribuir_EntaoDeveArmazenarCompleto()
        {
            var localizacao = "Sala 1 - Prateleira 5";
            var dto = new AcervoFotograficoDetalheDTO { Localizacao = localizacao };

            dto.Localizacao.Should().Be(localizacao);
        }

        #endregion

        #region Procedencia

        [Fact]
        public void DadoProcedenciaVazia_QuandoCriarDTO_EntaoProcedenciaDeveSerNull()
        {
            var dto = new AcervoFotograficoDetalheDTO();

            dto.Procedencia.Should().BeNull();
        }

        [Fact]
        public void DadoProcedencia_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var procedencia = new Faker().Company.CompanyName();
            var dto = new AcervoFotograficoDetalheDTO { Procedencia = procedencia };

            dto.Procedencia.Should().Be(procedencia);
        }

        [Fact]
        public void DadoProcedenciaComCaracteresEspeciais_QuandoAtribuir_EntaoDeveArmazenar()
        {
            var procedencia = "Procedência - Doação: CEDEP/SME-SP";
            var dto = new AcervoFotograficoDetalheDTO { Procedencia = procedencia };

            dto.Procedencia.Should().Be(procedencia);
        }

        #endregion

        #region CopiaDigital

        [Fact]
        public void DadoCopiaDigitalVazia_QuandoCriarDTO_EntaoCopiaDigitalDeveSerNull()
        {
            var dto = new AcervoFotograficoDetalheDTO();

            dto.CopiaDigital.Should().BeNull();
        }

        [Fact]
        public void DadoCopiaDigitalSim_QuandoAtribuirSim_EntaoDeveArmazenarSim()
        {
            var dto = new AcervoFotograficoDetalheDTO { CopiaDigital = "Sim" };

            dto.CopiaDigital.Should().Be("Sim");
        }

        [Fact]
        public void DadoCopiaDigitalNao_QuandoAtribuirNao_EntaoDeveArmazenarNao()
        {
            var dto = new AcervoFotograficoDetalheDTO { CopiaDigital = "Não" };

            dto.CopiaDigital.Should().Be("Não");
        }

        [Fact]
        public void DadoCopiaDigitalValor_QuandoAtribuirSimOuNao_EntaoDeveArmazenarCorretamente()
        {
            var valor = new Faker().PickRandom("Sim", "Não");
            var dto = new AcervoFotograficoDetalheDTO { CopiaDigital = valor };

            dto.CopiaDigital.Should().BeOneOf("Sim", "Não");
            dto.CopiaDigital.Should().Be(valor);
        }

        #endregion

        #region PermiteUsoImagem

        [Fact]
        public void DadoPermiteUsoImagemVazia_QuandoCriarDTO_EntaoPermiteUsoImagemDeveSerNull()
        {
            var dto = new AcervoFotograficoDetalheDTO();

            dto.PermiteUsoImagem.Should().BeNull();
        }

        [Fact]
        public void DadoPermiteUsoImagemSim_QuandoAtribuirSim_EntaoDeveArmazenarSim()
        {
            var dto = new AcervoFotograficoDetalheDTO { PermiteUsoImagem = "Sim" };

            dto.PermiteUsoImagem.Should().Be("Sim");
        }

        [Fact]
        public void DadoPermiteUsoImagemNao_QuandoAtribuirNao_EntaoDeveArmazenarNao()
        {
            var dto = new AcervoFotograficoDetalheDTO { PermiteUsoImagem = "Não" };

            dto.PermiteUsoImagem.Should().Be("Não");
        }

        [Fact]
        public void DadoPermiteUsoImagemValor_QuandoAtribuirSimOuNao_EntaoDeveArmazenarCorretamente()
        {
            var valor = new Faker().PickRandom("Sim", "Não");
            var dto = new AcervoFotograficoDetalheDTO { PermiteUsoImagem = valor };

            dto.PermiteUsoImagem.Should().BeOneOf("Sim", "Não");
            dto.PermiteUsoImagem.Should().Be(valor);
        }

        #endregion

        #region Conservacao

        [Fact]
        public void DadoConservacaoVazia_QuandoCriarDTO_EntaoConservacaoDeveSerNull()
        {
            var dto = new AcervoFotograficoDetalheDTO();

            dto.Conservacao.Should().BeNull();
        }

        [Fact]
        public void DadoConservacao_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var conservacao = new Faker().PickRandom("Excelente", "Bom", "Razoável", "Ruim");
            var dto = new AcervoFotograficoDetalheDTO { Conservacao = conservacao };

            dto.Conservacao.Should().Be(conservacao);
        }

        [Fact]
        public void DadoConservacaoComEspacos_QuandoAtribuir_EntaoDeveArmazenarComEspacos()
        {
            var conservacao = "Estado de Conservação Bom";
            var dto = new AcervoFotograficoDetalheDTO { Conservacao = conservacao };

            dto.Conservacao.Should().Be(conservacao);
        }

        #endregion

        #region Quantidade

        [Fact]
        public void DadoQuantidadeVazia_QuandoCriarDTO_EntaoQuantidadeDeveSerZero()
        {
            var dto = new AcervoFotograficoDetalheDTO();

            dto.Quantidade.Should().Be(0);
        }

        [Fact]
        public void DadoQuantidadeValida_QuandoAtribuir_EntaoDeveArmazenarCorretamente()
        {
            var quantidade = new Faker().Random.Long(1, 10000);
            var dto = new AcervoFotograficoDetalheDTO { Quantidade = quantidade };

            dto.Quantidade.Should().Be(quantidade);
        }

        [Fact]
        public void DadoQuantidadeGrande_QuandoAtribuirLongMaxValue_EntaoDeveArmazenar()
        {
            var dto = new AcervoFotograficoDetalheDTO { Quantidade = long.MaxValue };

            dto.Quantidade.Should().Be(long.MaxValue);
        }

        [Fact]
        public void DadoQuantidadeUm_QuandoAtribuir1_EntaoDeveArmazenarUm()
        {
            var dto = new AcervoFotograficoDetalheDTO { Quantidade = 1 };

            dto.Quantidade.Should().Be(1);
        }

        #endregion

        #region Dimensoes

        [Fact]
        public void DadoDimensoesVazia_QuandoCriarDTO_EntaoDimensoesDeveSerNull()
        {
            var dto = new AcervoFotograficoDetalheDTO();

            dto.Dimensoes.Should().BeNull();
        }

        [Fact]
        public void DadoDimensoes_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var dimensoes = new Faker().Random.String(20);
            var dto = new AcervoFotograficoDetalheDTO { Dimensoes = dimensoes };

            dto.Dimensoes.Should().Be(dimensoes);
        }

        [Fact]
        public void DadoDimensoesComUnidade_QuandoAtribuir_EntaoDeveArmazenarCompleto()
        {
            var dimensoes = "20cm x 30cm";
            var dto = new AcervoFotograficoDetalheDTO { Dimensoes = dimensoes };

            dto.Dimensoes.Should().Be(dimensoes);
        }

        #endregion

        #region Suporte

        [Fact]
        public void DadoSuporteVazio_QuandoCriarDTO_EntaoSuporteDeveSerNull()
        {
            var dto = new AcervoFotograficoDetalheDTO();

            dto.Suporte.Should().BeNull();
        }

        [Fact]
        public void DadoSuporte_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var suporte = new Faker().PickRandom("Papel", "Cartão", "Tecido", "Vidro");
            var dto = new AcervoFotograficoDetalheDTO { Suporte = suporte };

            dto.Suporte.Should().Be(suporte);
        }

        [Fact]
        public void DadoSuporteComEspacos_QuandoAtribuir_EntaoDeveArmazenarComEspacos()
        {
            var suporte = "Papel Fotográfico";
            var dto = new AcervoFotograficoDetalheDTO { Suporte = suporte };

            dto.Suporte.Should().Be(suporte);
        }

        #endregion

        #region Formato

        [Fact]
        public void DadoFormatoVazio_QuandoCriarDTO_EntaoFormatoDeveSerNull()
        {
            var dto = new AcervoFotograficoDetalheDTO();

            dto.Formato.Should().BeNull();
        }

        [Fact]
        public void DadoFormato_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var formato = new Faker().PickRandom("JPEG", "PNG", "TIFF", "PDF");
            var dto = new AcervoFotograficoDetalheDTO { Formato = formato };

            dto.Formato.Should().Be(formato);
        }

        [Fact]
        public void DadoFormatoComMaiusculas_QuandoAtribuir_EntaoDeveArmazenarComMaiusculas()
        {
            var formato = "JPG";
            var dto = new AcervoFotograficoDetalheDTO { Formato = formato };

            dto.Formato.Should().Be(formato);
        }

        #endregion

        #region TamanhoArquivo

        [Fact]
        public void DadoTamanhoArquivoVazio_QuandoCriarDTO_EntaoTamanhoArquivoDeveSerNull()
        {
            var dto = new AcervoFotograficoDetalheDTO();

            dto.TamanhoArquivo.Should().BeNull();
        }

        [Fact]
        public void DadoTamanhoArquivo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var tamanhoArquivo = "2.5 MB";
            var dto = new AcervoFotograficoDetalheDTO { TamanhoArquivo = tamanhoArquivo };

            dto.TamanhoArquivo.Should().Be(tamanhoArquivo);
        }

        [Fact]
        public void DadoTamanhoArquivoComUnidade_QuandoAtribuir_EntaoDeveArmazenarCompleto()
        {
            var tamanhoArquivo = "10 GB";
            var dto = new AcervoFotograficoDetalheDTO { TamanhoArquivo = tamanhoArquivo };

            dto.TamanhoArquivo.Should().Be(tamanhoArquivo);
        }

        #endregion

        #region Cromia

        [Fact]
        public void DadoCromiaVazia_QuandoCriarDTO_EntaoCromiaDeveSerNull()
        {
            var dto = new AcervoFotograficoDetalheDTO();

            dto.Cromia.Should().BeNull();
        }

        [Fact]
        public void DadoCromia_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var cromia = new Faker().PickRandom("Colorida", "Preto e Branco", "Sépia");
            var dto = new AcervoFotograficoDetalheDTO { Cromia = cromia };

            dto.Cromia.Should().Be(cromia);
        }

        [Fact]
        public void DadoCromiaComEspacos_QuandoAtribuir_EntaoDeveArmazenarComEspacos()
        {
            var cromia = "Preto e Branco";
            var dto = new AcervoFotograficoDetalheDTO { Cromia = cromia };

            dto.Cromia.Should().Be(cromia);
        }

        #endregion

        #region Resolucao

        [Fact]
        public void DadoResolucaoVazia_QuandoCriarDTO_EntaoResolucaoDeveSerNull()
        {
            var dto = new AcervoFotograficoDetalheDTO();

            dto.Resolucao.Should().BeNull();
        }

        [Fact]
        public void DadoResolucao_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var resolucao = "300 DPI";
            var dto = new AcervoFotograficoDetalheDTO { Resolucao = resolucao };

            dto.Resolucao.Should().Be(resolucao);
        }

        [Fact]
        public void DadoResolucaoComNumeros_QuandoAtribuir_EntaoDeveArmazenarCompleto()
        {
            var resolucao = "1920x1080";
            var dto = new AcervoFotograficoDetalheDTO { Resolucao = resolucao };

            dto.Resolucao.Should().Be(resolucao);
        }

        #endregion

        #region Imagens

        [Fact]
        public void DadoImagensVazio_QuandoCriarDTO_EntaoImagensDeveSerNull()
        {
            var dto = new AcervoFotograficoDetalheDTO();

            dto.Imagens.Should().BeNull();
        }

        [Fact]
        public void DadoImagens_QuandoAtribuirArray_EntaoDeveArmazenarCorretamente()
        {
            var imagens = new[]
            {
                new ImagemDTO { Original = "IMG001", Thumbnail = "THB001" },
                new ImagemDTO { Original = "IMG002", Thumbnail = "THB002" }
            };
            var dto = new AcervoFotograficoDetalheDTO { Imagens = imagens };

            dto.Imagens.Should().HaveCount(2);
            dto.Imagens.Should().BeEquivalentTo(imagens);
        }

        [Fact]
        public void DadoImagensVazio_QuandoAtribuirArrayVazio_EntaoDeveArmazenarVazio()
        {
            var imagens = Array.Empty<ImagemDTO>();
            var dto = new AcervoFotograficoDetalheDTO { Imagens = imagens };

            dto.Imagens.Should().BeEmpty();
        }

        [Fact]
        public void DadoImagensUnica_QuandoAtribuirUmaImagem_EntaoDeveArmazenarUma()
        {
            var imagens = new[] 
            { 
                new ImagemDTO { Original = "IMG001", Thumbnail = "THB001" } 
            };
            var dto = new AcervoFotograficoDetalheDTO { Imagens = imagens };

            dto.Imagens.Should().HaveCount(1);
        }

        #endregion       

        #region Testes de Integração - Múltiplas Propriedades

        [Fact]
        public void DadoDTOCompletoValido_QuandoInstanciarComTodosOsParametros_EntaoDeveArmazenarTodosCorretamente()
        {
            var faker = new Faker("pt_BR");
            var descricao = faker.Lorem.Paragraph();
            var creditosAutores = "Autor 1 | Autor 2";
            var dataAcervo = faker.Date.Past().ToString("dd/MM/yyyy");
            var localizacao = faker.Address.StreetName();
            var procedencia = faker.Company.CompanyName();
            var copiaDigital = "Sim";
            var permiteUsoImagem = "Não";
            var conservacao = "Bom";
            var quantidade = faker.Random.Long(1, 1000);
            var dimensoes = "20cm x 30cm";
            var suporte = "Papel";
            var formato = "JPEG";
            var tamanhoArquivo = "2.5 MB";
            var cromia = "Colorida";
            var resolucao = "300 DPI";
            var imagens = new[] 
            { 
                new ImagemDTO { Original = "IMG001", Thumbnail = "THB001" } 
            };
            var acervoId = faker.Random.Long(1, 1000);
            var titulo = faker.Lorem.Sentence();
            var tipoAcervoId = faker.Random.Int(1, 100);
            var codigo = faker.Random.String(10);

            var dto = new AcervoFotograficoDetalheDTO
            {
                AcervoId = acervoId,
                Titulo = titulo,
                TipoAcervoId = tipoAcervoId,
                Codigo = codigo,
                Descricao = descricao,
                CreditosAutores = creditosAutores,
                DataAcervo = dataAcervo,
                Localizacao = localizacao,
                Procedencia = procedencia,
                CopiaDigital = copiaDigital,
                PermiteUsoImagem = permiteUsoImagem,
                Conservacao = conservacao,
                Quantidade = quantidade,
                Dimensoes = dimensoes,
                Suporte = suporte,
                Formato = formato,
                TamanhoArquivo = tamanhoArquivo,
                Cromia = cromia,
                Resolucao = resolucao,
                Imagens = imagens
            };

            dto.AcervoId.Should().Be(acervoId);
            dto.Titulo.Should().Be(titulo);
            dto.TipoAcervoId.Should().Be(tipoAcervoId);
            dto.Codigo.Should().Be(codigo);
            dto.Descricao.Should().Be(descricao);
            dto.CreditosAutores.Should().Be(creditosAutores);
            dto.DataAcervo.Should().Be(dataAcervo);
            dto.Localizacao.Should().Be(localizacao);
            dto.Procedencia.Should().Be(procedencia);
            dto.CopiaDigital.Should().Be(copiaDigital);
            dto.PermiteUsoImagem.Should().Be(permiteUsoImagem);
            dto.Conservacao.Should().Be(conservacao);
            dto.Quantidade.Should().Be(quantidade);
            dto.Dimensoes.Should().Be(dimensoes);
            dto.Suporte.Should().Be(suporte);
            dto.Formato.Should().Be(formato);
            dto.TamanhoArquivo.Should().Be(tamanhoArquivo);
            dto.Cromia.Should().Be(cromia);
            dto.Resolucao.Should().Be(resolucao);
            dto.Imagens.Should().BeEquivalentTo(imagens);
        }

        [Fact]
        public void DadoDTOVazio_QuandoInstanciarSemParametros_EntaoDeveSerValido()
        {
            var dto = new AcervoFotograficoDetalheDTO();

            dto.Should().NotBeNull();
            dto.Descricao.Should().BeNull();
            dto.CreditosAutores.Should().BeNull();
            dto.DataAcervo.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.PermiteUsoImagem.Should().BeNull();
            dto.Conservacao.Should().BeNull();
            dto.Quantidade.Should().Be(0);
            dto.Dimensoes.Should().BeNull();
            dto.Suporte.Should().BeNull();
            dto.Formato.Should().BeNull();
            dto.TamanhoArquivo.Should().BeNull();
            dto.Cromia.Should().BeNull();
            dto.Resolucao.Should().BeNull();
            dto.Imagens.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComValoresNulos_QuandoAtribuirExplicitamente_EntaoDeveArmazenarNull()
        {
            var dto = new AcervoFotograficoDetalheDTO
            {
                Descricao = null!,
                CreditosAutores = null!,
                DataAcervo = null!,
                Localizacao = null!,
                Procedencia = null!,
                CopiaDigital = null!,
                PermiteUsoImagem = null!,
                Conservacao = null!,
                Dimensoes = null!,
                Suporte = null!,
                Formato = null!,
                TamanhoArquivo = null!,
                Cromia = null!,
                Resolucao = null!,
                Imagens = null!
            };

            dto.Descricao.Should().BeNull();
            dto.CreditosAutores.Should().BeNull();
            dto.DataAcervo.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.PermiteUsoImagem.Should().BeNull();
            dto.Conservacao.Should().BeNull();
            dto.Dimensoes.Should().BeNull();
            dto.Suporte.Should().BeNull();
            dto.Formato.Should().BeNull();
            dto.TamanhoArquivo.Should().BeNull();
            dto.Cromia.Should().BeNull();
            dto.Resolucao.Should().BeNull();
            dto.Imagens.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComValoresVazios_QuandoAtribuirStringsVazias_EntaoDeveArmazenarVazio()
        {
            var dto = new AcervoFotograficoDetalheDTO
            {
                Descricao = string.Empty,
                CreditosAutores = string.Empty,
                DataAcervo = string.Empty,
                Localizacao = string.Empty,
                Procedencia = string.Empty,
                CopiaDigital = string.Empty,
                PermiteUsoImagem = string.Empty,
                Conservacao = string.Empty,
                Dimensoes = string.Empty,
                Suporte = string.Empty,
                Formato = string.Empty,
                TamanhoArquivo = string.Empty,
                Cromia = string.Empty,
                Resolucao = string.Empty
            };

            dto.Descricao.Should().Be(string.Empty);
            dto.CreditosAutores.Should().Be(string.Empty);
            dto.DataAcervo.Should().Be(string.Empty);
            dto.Localizacao.Should().Be(string.Empty);
            dto.Procedencia.Should().Be(string.Empty);
            dto.CopiaDigital.Should().Be(string.Empty);
            dto.PermiteUsoImagem.Should().Be(string.Empty);
            dto.Conservacao.Should().Be(string.Empty);
            dto.Dimensoes.Should().Be(string.Empty);
            dto.Suporte.Should().Be(string.Empty);
            dto.Formato.Should().Be(string.Empty);
            dto.TamanhoArquivo.Should().Be(string.Empty);
            dto.Cromia.Should().Be(string.Empty);
            dto.Resolucao.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoDTOComMultiplasImagens_QuandoAtribuirVariasImagens_EntaoDeveArmazenarTodas()
        {
            var imagens = Enumerable.Range(1, 10)
                .Select(i => new ImagemDTO 
                { 
                    Original = $"IMG{i:D3}", 
                    Thumbnail = $"THB{i:D3}" 
                })
                .ToArray();

            var dto = new AcervoFotograficoDetalheDTO { Imagens = imagens };

            dto.Imagens.Should().HaveCount(10);
            dto.Imagens.Should().AllSatisfy(img => img.Original.Should().StartWith("IMG"));
            dto.Imagens.Should().AllSatisfy(img => img.Thumbnail.Should().StartWith("THB"));
        }

        #endregion
    }
}
