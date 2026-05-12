using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoBibliograficoLinhaRetornoDtoTeste
    {
        private AcervoBibliograficoLinhaRetornoDTO CriarAcervoBibliograficoLinhaRetornoDTO()
        {
            return new AcervoBibliograficoLinhaRetornoDTO
            {
                Titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Título", PossuiErro = false, Mensagem = string.Empty },
                SubTitulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Subtítulo", PossuiErro = false, Mensagem = string.Empty },
                MaterialId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1", PossuiErro = false, Mensagem = string.Empty },
                CreditosAutoresIds = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1,2,3", PossuiErro = false, Mensagem = string.Empty },
                CoAutores = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Coautor1", PossuiErro = false, Mensagem = string.Empty },
                TipoAutoria = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Autor", PossuiErro = false, Mensagem = string.Empty },
                EditoraId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1", PossuiErro = false, Mensagem = string.Empty },
                AssuntosIds = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1,2", PossuiErro = false, Mensagem = string.Empty },
                Ano = new LinhaConteudoAjustarRetornoDTO { Conteudo = "2024", PossuiErro = false, Mensagem = string.Empty },
                Edicao = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1ª", PossuiErro = false, Mensagem = string.Empty },
                NumeroPagina = new LinhaConteudoAjustarRetornoDTO { Conteudo = "200", PossuiErro = false, Mensagem = string.Empty },
                Altura = new LinhaConteudoAjustarRetornoDTO { Conteudo = "25", PossuiErro = false, Mensagem = string.Empty },
                Largura = new LinhaConteudoAjustarRetornoDTO { Conteudo = "15", PossuiErro = false, Mensagem = string.Empty },
                SerieColecaoId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1", PossuiErro = false, Mensagem = string.Empty },
                Volume = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1", PossuiErro = false, Mensagem = string.Empty },
                IdiomaId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1", PossuiErro = false, Mensagem = string.Empty },
                LocalizacaoCDD = new LinhaConteudoAjustarRetornoDTO { Conteudo = "000.00", PossuiErro = false, Mensagem = string.Empty },
                LocalizacaoPHA = new LinhaConteudoAjustarRetornoDTO { Conteudo = "PHA123", PossuiErro = false, Mensagem = string.Empty },
                NotasGerais = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Notas", PossuiErro = false, Mensagem = string.Empty },
                Isbn = new LinhaConteudoAjustarRetornoDTO { Conteudo = "978-0-123456-78-9", PossuiErro = false, Mensagem = string.Empty },
                Codigo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "COD123", PossuiErro = false, Mensagem = string.Empty },
                Status = ImportacaoStatus.Sucesso,
                NumeroLinha = 1,
                Mensagem = string.Empty,
                ErrosCampos = new string[] { }
            };
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoTituloEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.Titulo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoSubTituloEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.SubTitulo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoMaterialIdEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.MaterialId.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoCreditosAutoresIdsEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.CreditosAutoresIds.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoCoAutoresEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.CoAutores.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoTipoAutoriaEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.TipoAutoria.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoEditoraIdEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.EditoraId.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoAssuntosIdsEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.AssuntosIds.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoAnoEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoEdicaoEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.Edicao.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoNumeroPaginaEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.NumeroPagina.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoAlturaEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.Altura.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoLarguraEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.Largura.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoSerieColecaoIdEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.SerieColecaoId.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoVolumeEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.Volume.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoIdiomaIdEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.IdiomaId.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoLocalizacaoCDDEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.LocalizacaoCDD.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoLocalizacaoPHAEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.LocalizacaoPHA.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoNotasGeraisEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.NotasGerais.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoIsbnEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.Isbn.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoInstanciar_EntaoCodigoEhNulavel()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();

            dto.Codigo.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirTitulo_EntaoTituloEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Novo Título", PossuiErro = false };

            dto.Titulo = titulo;

            dto.Titulo.Should().Be(titulo);
            dto.Titulo.Conteudo.Should().Be("Novo Título");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirSubTitulo_EntaoSubTituloEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var subTitulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Novo Subtítulo", PossuiErro = false };

            dto.SubTitulo = subTitulo;

            dto.SubTitulo.Should().Be(subTitulo);
            dto.SubTitulo.Conteudo.Should().Be("Novo Subtítulo");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirMaterialId_EntaoMaterialIdEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var materialId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "5", PossuiErro = false };

            dto.MaterialId = materialId;

            dto.MaterialId.Should().Be(materialId);
            dto.MaterialId.Conteudo.Should().Be("5");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirCreditosAutoresIds_EntaoCreditosAutoresIdsEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var creditosAutoresIds = new LinhaConteudoAjustarRetornoDTO { Conteudo = "4,5,6", PossuiErro = false };

            dto.CreditosAutoresIds = creditosAutoresIds;

            dto.CreditosAutoresIds.Should().Be(creditosAutoresIds);
            dto.CreditosAutoresIds.Conteudo.Should().Be("4,5,6");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirCoAutores_EntaoCoAutoresEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var coAutores = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Coautor2", PossuiErro = false };

            dto.CoAutores = coAutores;

            dto.CoAutores.Should().Be(coAutores);
            dto.CoAutores.Conteudo.Should().Be("Coautor2");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirTipoAutoria_EntaoTipoAutoriaEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var tipoAutoria = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Organizador", PossuiErro = false };

            dto.TipoAutoria = tipoAutoria;

            dto.TipoAutoria.Should().Be(tipoAutoria);
            dto.TipoAutoria.Conteudo.Should().Be("Organizador");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirEditoraId_EntaoEditoraIdEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var editoraId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "3", PossuiErro = false };

            dto.EditoraId = editoraId;

            dto.EditoraId.Should().Be(editoraId);
            dto.EditoraId.Conteudo.Should().Be("3");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirAssuntosIds_EntaoAssuntosIdsEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var assuntosIds = new LinhaConteudoAjustarRetornoDTO { Conteudo = "3,4,5", PossuiErro = false };

            dto.AssuntosIds = assuntosIds;

            dto.AssuntosIds.Should().Be(assuntosIds);
            dto.AssuntosIds.Conteudo.Should().Be("3,4,5");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirAno_EntaoAnoEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var ano = new LinhaConteudoAjustarRetornoDTO { Conteudo = "2023", PossuiErro = false };

            dto.Ano = ano;

            dto.Ano.Should().Be(ano);
            dto.Ano.Conteudo.Should().Be("2023");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirEdicao_EntaoEdicaoEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var edicao = new LinhaConteudoAjustarRetornoDTO { Conteudo = "2ª", PossuiErro = false };

            dto.Edicao = edicao;

            dto.Edicao.Should().Be(edicao);
            dto.Edicao.Conteudo.Should().Be("2ª");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirNumeroPagina_EntaoNumeroPaginaEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var numeroPagina = new LinhaConteudoAjustarRetornoDTO { Conteudo = "300", PossuiErro = false };

            dto.NumeroPagina = numeroPagina;

            dto.NumeroPagina.Should().Be(numeroPagina);
            dto.NumeroPagina.Conteudo.Should().Be("300");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirAltura_EntaoAlturaEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var altura = new LinhaConteudoAjustarRetornoDTO { Conteudo = "30", PossuiErro = false };

            dto.Altura = altura;

            dto.Altura.Should().Be(altura);
            dto.Altura.Conteudo.Should().Be("30");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirLargura_EntaoLarguraEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var largura = new LinhaConteudoAjustarRetornoDTO { Conteudo = "20", PossuiErro = false };

            dto.Largura = largura;

            dto.Largura.Should().Be(largura);
            dto.Largura.Conteudo.Should().Be("20");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirSerieColecaoId_EntaoSerieColecaoIdEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var serieColecaoId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "2", PossuiErro = false };

            dto.SerieColecaoId = serieColecaoId;

            dto.SerieColecaoId.Should().Be(serieColecaoId);
            dto.SerieColecaoId.Conteudo.Should().Be("2");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirVolume_EntaoVolumeEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var volume = new LinhaConteudoAjustarRetornoDTO { Conteudo = "2", PossuiErro = false };

            dto.Volume = volume;

            dto.Volume.Should().Be(volume);
            dto.Volume.Conteudo.Should().Be("2");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirIdiomaId_EntaoIdiomaIdEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var idiomaId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "2", PossuiErro = false };

            dto.IdiomaId = idiomaId;

            dto.IdiomaId.Should().Be(idiomaId);
            dto.IdiomaId.Conteudo.Should().Be("2");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirLocalizacaoCDD_EntaoLocalizacaoCDDEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var localizacaoCDD = new LinhaConteudoAjustarRetornoDTO { Conteudo = "111.11", PossuiErro = false };

            dto.LocalizacaoCDD = localizacaoCDD;

            dto.LocalizacaoCDD.Should().Be(localizacaoCDD);
            dto.LocalizacaoCDD.Conteudo.Should().Be("111.11");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirLocalizacaoPHA_EntaoLocalizacaoPHAEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var localizacaoPHA = new LinhaConteudoAjustarRetornoDTO { Conteudo = "PHA456", PossuiErro = false };

            dto.LocalizacaoPHA = localizacaoPHA;

            dto.LocalizacaoPHA.Should().Be(localizacaoPHA);
            dto.LocalizacaoPHA.Conteudo.Should().Be("PHA456");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirNotasGerais_EntaoNotasGeraisEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var notasGerais = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Notas atualizadas", PossuiErro = false };

            dto.NotasGerais = notasGerais;

            dto.NotasGerais.Should().Be(notasGerais);
            dto.NotasGerais.Conteudo.Should().Be("Notas atualizadas");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirIsbn_EntaoIsbnEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var isbn = new LinhaConteudoAjustarRetornoDTO { Conteudo = "978-0-987654-32-1", PossuiErro = false };

            dto.Isbn = isbn;

            dto.Isbn.Should().Be(isbn);
            dto.Isbn.Conteudo.Should().Be("978-0-987654-32-1");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirCodigo_EntaoCodigoEhAtribuido()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO();
            var codigo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "COD456", PossuiErro = false };

            dto.Codigo = codigo;

            dto.Codigo.Should().Be(codigo);
            dto.Codigo.Conteudo.Should().Be("COD456");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoUtilizarTodosOsCampos_EntaoTodosCamposSaoAcessiveis()
        {
            var dto = CriarAcervoBibliograficoLinhaRetornoDTO();

            dto.Titulo.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.SubTitulo.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.MaterialId.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.CreditosAutoresIds.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.CoAutores.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.TipoAutoria.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.EditoraId.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.AssuntosIds.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.Ano.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.Edicao.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.NumeroPagina.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.Altura.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.Largura.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.SerieColecaoId.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.Volume.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.IdiomaId.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.LocalizacaoCDD.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.LocalizacaoPHA.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.NotasGerais.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.Isbn.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
            dto.Codigo.Should().NotBeNull().And.BeOfType<LinhaConteudoAjustarRetornoDTO>();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoHerdarDeAcervoLinhaRetornoDTO_EntaoTemPropriedadesHerdadas()
        {
            var dto = CriarAcervoBibliograficoLinhaRetornoDTO();

            dto.Should().BeAssignableTo<AcervoLinhaRetornoDTO>();
            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.NumeroLinha.Should().Be(1);
            dto.Mensagem.Should().BeEmpty();
            dto.ErrosCampos.Should().NotBeNull();
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirPropriedadesComErros_EntaoPropriedadesArmazenamErros()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO
            {
                Titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Título", PossuiErro = true, Mensagem = "Erro no título" },
                Status = ImportacaoStatus.Erros,
                NumeroLinha = 5,
                Mensagem = "Linha com erros",
                ErrosCampos = new[] { "Titulo", "SubTitulo" }
            };

            dto.Titulo.PossuiErro.Should().BeTrue();
            dto.Titulo.Mensagem.Should().Be("Erro no título");
            dto.Status.Should().Be(ImportacaoStatus.Erros);
            dto.Mensagem.Should().Be("Linha com erros");
            dto.ErrosCampos.Should().ContainInOrder("Titulo", "SubTitulo");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoModificarPropriedades_EntaoPropriedadesSaoAtualizadas()
        {
            var dto = CriarAcervoBibliograficoLinhaRetornoDTO();

            var novoTitulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Novo Título", PossuiErro = false };
            dto.Titulo = novoTitulo;

            dto.Titulo.Conteudo.Should().Be("Novo Título");
        }

        [Fact]
        public void DadoAcervoBibliograficoLinhaRetorno_QuandoDefinirPropriedadesBaseNula_EntaoPropriedadesBasePodemSerNulas()
        {
            var dto = new AcervoBibliograficoLinhaRetornoDTO
            {
                Mensagem = null!,
                ErrosCampos = null!
            };

            dto.Mensagem.Should().BeNull();
            dto.ErrosCampos.Should().BeNull();
        }
    }
}
