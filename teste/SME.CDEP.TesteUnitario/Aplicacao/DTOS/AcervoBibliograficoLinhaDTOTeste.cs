using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoBibliograficoLinhaDTOTeste
    {
        private AcervoBibliograficoLinhaDTO CriarAcervoBibliograficoLinhaDTO()
        {
            return new AcervoBibliograficoLinhaDTO
            {
                Titulo = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro Título" },
                SubTitulo = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro SubTítulo" },
                Material = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro Material" },
                Autor = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro Autor" },
                CoAutor = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro CoAutor" },
                TipoAutoria = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro TipoAutoria" },
                Editora = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro Editora" },
                Assunto = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro Assunto" },
                Ano = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro Ano" },
                Edicao = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro Edicao" },
                NumeroPaginas = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro NumeroPaginas" },
                Altura = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro Altura" },
                Largura = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro Largura" },
                SerieColecao = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro SerieColecao" },
                Volume = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro Volume" },
                Idioma = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro Idioma" },
                LocalizacaoCDD = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro LocalizacaoCDD" },
                LocalizacaoPHA = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro LocalizacaoPHA" },
                NotasGerais = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro NotasGerais" },
                Isbn = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro Isbn" },
                Codigo = new LinhaConteudoAjustarDTO { PossuiErro = true, Mensagem = "Erro Codigo" },
                PossuiErros = true,
                Mensagem = "Erros encontrados",
                Status = ImportacaoStatus.Erros,
                NumeroLinha = 1
            };
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDaTitulo()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.Titulo.PossuiErro.Should().BeTrue();
            dto.Titulo.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.Titulo.PossuiErro.Should().BeFalse();
            dto.Titulo.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDoSubTitulo()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.SubTitulo.PossuiErro.Should().BeTrue();
            dto.SubTitulo.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.SubTitulo.PossuiErro.Should().BeFalse();
            dto.SubTitulo.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDaMaterial()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.Material.PossuiErro.Should().BeTrue();
            dto.Material.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.Material.PossuiErro.Should().BeFalse();
            dto.Material.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDoAutor()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.Autor.PossuiErro.Should().BeTrue();
            dto.Autor.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.Autor.PossuiErro.Should().BeFalse();
            dto.Autor.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDoCoAutor()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.CoAutor.PossuiErro.Should().BeTrue();
            dto.CoAutor.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.CoAutor.PossuiErro.Should().BeFalse();
            dto.CoAutor.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDaTipoAutoria()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.TipoAutoria.PossuiErro.Should().BeTrue();
            dto.TipoAutoria.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.TipoAutoria.PossuiErro.Should().BeFalse();
            dto.TipoAutoria.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDaEditora()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.Editora.PossuiErro.Should().BeTrue();
            dto.Editora.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.Editora.PossuiErro.Should().BeFalse();
            dto.Editora.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDoAssunto()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.Assunto.PossuiErro.Should().BeTrue();
            dto.Assunto.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.Assunto.PossuiErro.Should().BeFalse();
            dto.Assunto.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDoAno()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.Ano.PossuiErro.Should().BeTrue();
            dto.Ano.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.Ano.PossuiErro.Should().BeFalse();
            dto.Ano.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDaEdicao()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.Edicao.PossuiErro.Should().BeTrue();
            dto.Edicao.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.Edicao.PossuiErro.Should().BeFalse();
            dto.Edicao.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDoNumeroPaginas()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.NumeroPaginas.PossuiErro.Should().BeTrue();
            dto.NumeroPaginas.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.NumeroPaginas.PossuiErro.Should().BeFalse();
            dto.NumeroPaginas.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDaAltura()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.Altura.PossuiErro.Should().BeTrue();
            dto.Altura.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.Altura.PossuiErro.Should().BeFalse();
            dto.Altura.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDaLargura()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.Largura.PossuiErro.Should().BeTrue();
            dto.Largura.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.Largura.PossuiErro.Should().BeFalse();
            dto.Largura.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDaSerieColecao()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.SerieColecao.PossuiErro.Should().BeTrue();
            dto.SerieColecao.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.SerieColecao.PossuiErro.Should().BeFalse();
            dto.SerieColecao.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDoVolume()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.Volume.PossuiErro.Should().BeTrue();
            dto.Volume.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.Volume.PossuiErro.Should().BeFalse();
            dto.Volume.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDoIdioma()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.Idioma.PossuiErro.Should().BeTrue();
            dto.Idioma.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.Idioma.PossuiErro.Should().BeFalse();
            dto.Idioma.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDaLocalizacaoCDD()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.LocalizacaoCDD.PossuiErro.Should().BeTrue();
            dto.LocalizacaoCDD.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.LocalizacaoCDD.PossuiErro.Should().BeFalse();
            dto.LocalizacaoCDD.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDaLocalizacaoPHA()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.LocalizacaoPHA.PossuiErro.Should().BeTrue();
            dto.LocalizacaoPHA.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.LocalizacaoPHA.PossuiErro.Should().BeFalse();
            dto.LocalizacaoPHA.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosdasNotasGerais()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.NotasGerais.PossuiErro.Should().BeTrue();
            dto.NotasGerais.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.NotasGerais.PossuiErro.Should().BeFalse();
            dto.NotasGerais.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDoIsbn()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.Isbn.PossuiErro.Should().BeTrue();
            dto.Isbn.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.Isbn.PossuiErro.Should().BeFalse();
            dto.Isbn.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDoCodigoComErro()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.Codigo.PossuiErro.Should().BeTrue();
            dto.Codigo.Mensagem.Should().NotBeEmpty();

            dto.DefinirLinhaComoSucesso();

            dto.Codigo.PossuiErro.Should().BeFalse();
            dto.Codigo.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDaLinhaBase()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.PossuiErros.Should().BeTrue();
            dto.Mensagem.Should().NotBeEmpty();
            dto.Status.Should().Be(ImportacaoStatus.Erros);

            dto.DefinirLinhaComoSucesso();

            dto.PossuiErros.Should().BeFalse();
            dto.Mensagem.Should().BeEmpty();
            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoPreservaNumeroLinha()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            var numeroLinhaEsperado = 42;
            dto.NumeroLinha = numeroLinhaEsperado;

            dto.DefinirLinhaComoSucesso();

            dto.NumeroLinha.Should().Be(numeroLinhaEsperado);
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaErrosDeTodosOsCampos()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();

            dto.DefinirLinhaComoSucesso();

            dto.Titulo.PossuiErro.Should().BeFalse();
            dto.SubTitulo.PossuiErro.Should().BeFalse();
            dto.Material.PossuiErro.Should().BeFalse();
            dto.Autor.PossuiErro.Should().BeFalse();
            dto.CoAutor.PossuiErro.Should().BeFalse();
            dto.TipoAutoria.PossuiErro.Should().BeFalse();
            dto.Editora.PossuiErro.Should().BeFalse();
            dto.Assunto.PossuiErro.Should().BeFalse();
            dto.Ano.PossuiErro.Should().BeFalse();
            dto.Edicao.PossuiErro.Should().BeFalse();
            dto.NumeroPaginas.PossuiErro.Should().BeFalse();
            dto.Altura.PossuiErro.Should().BeFalse();
            dto.Largura.PossuiErro.Should().BeFalse();
            dto.SerieColecao.PossuiErro.Should().BeFalse();
            dto.Volume.PossuiErro.Should().BeFalse();
            dto.Idioma.PossuiErro.Should().BeFalse();
            dto.LocalizacaoCDD.PossuiErro.Should().BeFalse();
            dto.LocalizacaoPHA.PossuiErro.Should().BeFalse();
            dto.NotasGerais.PossuiErro.Should().BeFalse();
            dto.Isbn.PossuiErro.Should().BeFalse();
            dto.Codigo.PossuiErro.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoLimpaTodasAsMensagensDeErro()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();

            dto.DefinirLinhaComoSucesso();

            dto.Titulo.Mensagem.Should().BeEmpty();
            dto.SubTitulo.Mensagem.Should().BeEmpty();
            dto.Material.Mensagem.Should().BeEmpty();
            dto.Autor.Mensagem.Should().BeEmpty();
            dto.CoAutor.Mensagem.Should().BeEmpty();
            dto.TipoAutoria.Mensagem.Should().BeEmpty();
            dto.Editora.Mensagem.Should().BeEmpty();
            dto.Assunto.Mensagem.Should().BeEmpty();
            dto.Ano.Mensagem.Should().BeEmpty();
            dto.Edicao.Mensagem.Should().BeEmpty();
            dto.NumeroPaginas.Mensagem.Should().BeEmpty();
            dto.Altura.Mensagem.Should().BeEmpty();
            dto.Largura.Mensagem.Should().BeEmpty();
            dto.SerieColecao.Mensagem.Should().BeEmpty();
            dto.Volume.Mensagem.Should().BeEmpty();
            dto.Idioma.Mensagem.Should().BeEmpty();
            dto.LocalizacaoCDD.Mensagem.Should().BeEmpty();
            dto.LocalizacaoPHA.Mensagem.Should().BeEmpty();
            dto.NotasGerais.Mensagem.Should().BeEmpty();
            dto.Isbn.Mensagem.Should().BeEmpty();
            dto.Codigo.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoAcervoBibliografico_QuandoDefinirLinhaComoSucesso_EntaoMudaStatusParaSucesso()
        {
            var dto = CriarAcervoBibliograficoLinhaDTO();
            dto.Status.Should().Be(ImportacaoStatus.Erros);

            dto.DefinirLinhaComoSucesso();

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact]
        public void DadoAcervoBibliograficoCamposCamNull_QuandoDefinirLinhaComoSucesso_EntaoNaoLancaExcecao()
        {
            var dto = new AcervoBibliograficoLinhaDTO
            {
                Titulo = null,
                SubTitulo = null,
                Material = null,
                Autor = null,
                CoAutor = null,
                TipoAutoria = null,
                Editora = null,
                Assunto = null,
                Ano = null,
                Edicao = null,
                NumeroPaginas = null,
                Altura = null,
                Largura = null,
                SerieColecao = null,
                Volume = null,
                Idioma = null,
                LocalizacaoCDD = null,
                LocalizacaoPHA = null,
                NotasGerais = null,
                Isbn = null,
                Codigo = null,
                PossuiErros = true,
                Mensagem = "Erro",
                Status = ImportacaoStatus.Erros
            };

            var exception = Record.Exception(() => dto.DefinirLinhaComoSucesso());
            exception.Should().NotBeNull().And.BeOfType<NullReferenceException>();
        }
    }
}
