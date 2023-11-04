using Bogus.Extensions.Brazil;
using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.TesteIntegracao.Constantes;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_fazer_manutencao_importacao_arquivo_acervo_fotografico : TesteBase
    {
        public Ao_fazer_manutencao_importacao_arquivo_acervo_fotografico(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Validar ObterCoAutoresTipoAutoria com tipo autoria nos 3 primeiros coautores")]
        public async Task Validar_obter_coautores_tipo_autoria_com_tipo_autoria_nos_tres_primeiros_coautores()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var acervoBibliograficoLinha = new AcervoBibliograficoLinhaDTO()
            {
                CoAutor = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Address.SecondaryAddress().Limite(200)}|{faker.Address.FullAddress().Limite(200)}|{faker.Address.StreetAddress().Limite(200)}|{faker.Address.Country().Limite(200)}|{faker.Address.City().Limite(200)}|{faker.Address.State().Limite(200)}",
                    LimiteCaracteres = Constantes.ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                TipoAutoria = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Company.CompanyName().Limite(15)}|{faker.Company.CompanySuffix().Limite(15)}|{faker.Company.Cnpj().Limite(15)}",
                    LimiteCaracteres = Constantes.ConstantesTestes.CARACTERES_PERMITIDOS_15
                }
            };

            var tiposAutorias = acervoBibliograficoLinha.TipoAutoria.Conteudo.FormatarTextoEmArray().ToList();

            var coautoresNumerados = acervoBibliograficoLinha.CoAutor.Conteudo.FormatarTextoEmArray()
                .Select((coautoresEmTexto, indice) => new IdNomeTipoDTO { Id = indice + 10, Nome = coautoresEmTexto }).ToList();
                
            servicoImportacaoArquivo.DefinirCreditosAutores(coautoresNumerados); 
            
            var coautores = servicoImportacaoArquivo.ObterCoAutoresTipoAutoria(acervoBibliograficoLinha.CoAutor.Conteudo, acervoBibliograficoLinha.TipoAutoria.Conteudo);
            coautores.ShouldNotBeNull();

            for (int i = 0; i < coautores.Count(); i++)
            {
                coautores[i].CreditoAutorId.ShouldBe(coautoresNumerados[i].Id);
                
                if (i <= 2)
                    coautores[i].TipoAutoria.ShouldBe(tiposAutorias[i]);
                else
                    coautores[i].TipoAutoria.ShouldBeNull();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Validar ObterCoAutoresTipoAutoria com tipo autoria somente no primeiro coautor")]
        public async Task Validar_obter_coautores_tipo_autoria_com_tipo_autoria_no_primeiro_coautor()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var acervoBibliograficoLinha = new AcervoBibliograficoLinhaDTO()
            {
                CoAutor = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Address.SecondaryAddress().Limite(200)}|{faker.Address.FullAddress().Limite(200)}|{faker.Address.StreetAddress().Limite(200)}|{faker.Address.Country().Limite(200)}|{faker.Address.City().Limite(200)}|{faker.Address.State().Limite(200)}",
                    LimiteCaracteres = Constantes.ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                TipoAutoria = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Company.CompanyName().Limite(15)}",
                    LimiteCaracteres = Constantes.ConstantesTestes.CARACTERES_PERMITIDOS_15
                }
            };

            var tiposAutorias = acervoBibliograficoLinha.TipoAutoria.Conteudo.FormatarTextoEmArray().ToList();

            var coautoresNumerados = acervoBibliograficoLinha.CoAutor.Conteudo.FormatarTextoEmArray()
                .Select((coautoresEmTexto, indice) => new IdNomeTipoDTO { Id = indice + 10, Nome = coautoresEmTexto }).ToList();
                
            servicoImportacaoArquivo.DefinirCreditosAutores(coautoresNumerados); 
            
            var coautores = servicoImportacaoArquivo.ObterCoAutoresTipoAutoria(acervoBibliograficoLinha.CoAutor.Conteudo, acervoBibliograficoLinha.TipoAutoria.Conteudo);
            coautores.ShouldNotBeNull();

            for (int i = 0; i < coautores.Count(); i++)
            {
                coautores[i].CreditoAutorId.ShouldBe(coautoresNumerados[i].Id);
                
                if (i == 0)
                    coautores[i].TipoAutoria.ShouldBe(tiposAutorias[i]);
                else
                    coautores[i].TipoAutoria.ShouldBeNull();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Validar ObterCoAutoresTipoAutoria sem tipo autoria")]
        public async Task Validar_obter_coautores_tipo_autoria_sem_tipo_autoria()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var acervoBibliograficoLinha = new AcervoBibliograficoLinhaDTO()
            {
                CoAutor = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Address.SecondaryAddress().Limite(200)}|{faker.Address.FullAddress().Limite(200)}|{faker.Address.StreetAddress().Limite(200)}|{faker.Address.Country().Limite(200)}|{faker.Address.City().Limite(200)}|{faker.Address.State().Limite(200)}",
                    LimiteCaracteres = Constantes.ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                TipoAutoria = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = string.Empty,
                    LimiteCaracteres = Constantes.ConstantesTestes.CARACTERES_PERMITIDOS_15
                }
            };

            var coautoresNumerados = acervoBibliograficoLinha.CoAutor.Conteudo.FormatarTextoEmArray()
                .Select((coautoresEmTexto, indice) => new IdNomeTipoDTO { Id = indice + 10, Nome = coautoresEmTexto }).ToList();
                
            servicoImportacaoArquivo.DefinirCreditosAutores(coautoresNumerados); 
            
            var coautores = servicoImportacaoArquivo.ObterCoAutoresTipoAutoria(acervoBibliograficoLinha.CoAutor.Conteudo, acervoBibliograficoLinha.TipoAutoria.Conteudo);
            coautores.ShouldNotBeNull();

            for (int i = 0; i < coautores.Count(); i++)
            {
                coautores[i].CreditoAutorId.ShouldBe(coautoresNumerados[i].Id);
                coautores[i].TipoAutoria.ShouldBeNull();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Validar ObterCoAutoresTipoAutoria com tipo autoria e coautores iguais")]
        public async Task Validar_obter_coautores_tipo_autoria_com_tipo_autoria_iguais_ao_numero_decoautores()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var acervoBibliograficoLinha = new AcervoBibliograficoLinhaDTO()
            {
                CoAutor = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Address.SecondaryAddress().Limite(200)}|{faker.Address.FullAddress().Limite(200)}|{faker.Address.StreetAddress().Limite(200)}|{faker.Address.Country().Limite(200)}|{faker.Address.City().Limite(200)}|{faker.Address.State().Limite(200)}",
                    LimiteCaracteres = Constantes.ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                TipoAutoria = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Company.CompanyName().Limite(15)}|{faker.Company.CompanySuffix().Limite(15)}|{faker.Company.Cnpj().Limite(15)}|{faker.Company.Bs().Limite(15)}|{faker.Company.CatchPhrase().Limite(15)}|{faker.Commerce.Locale.Limite(15)}",
                    LimiteCaracteres = Constantes.ConstantesTestes.CARACTERES_PERMITIDOS_15
                }
            };

            var tiposAutorias = acervoBibliograficoLinha.TipoAutoria.Conteudo.FormatarTextoEmArray().ToList();

            var coautoresNumerados = acervoBibliograficoLinha.CoAutor.Conteudo.FormatarTextoEmArray()
                .Select((coautoresEmTexto, indice) => new IdNomeTipoDTO { Id = indice + 10, Nome = coautoresEmTexto }).ToList();
                
            servicoImportacaoArquivo.DefinirCreditosAutores(coautoresNumerados); 
            
            var coautores = servicoImportacaoArquivo.ObterCoAutoresTipoAutoria(acervoBibliograficoLinha.CoAutor.Conteudo, acervoBibliograficoLinha.TipoAutoria.Conteudo);
            coautores.ShouldNotBeNull();

            for (int i = 0; i < coautores.Count(); i++)
            {
                coautores[i].CreditoAutorId.ShouldBe(coautoresNumerados[i].Id);
                coautores[i].TipoAutoria.ShouldBe(tiposAutorias[i]);
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - ValidarPreenchimentoValorFormatoQtdeCaracteres - Tipo autoria")]
        public async Task Validar_preenchimento_valor_formato_qtde_caracteres()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var random = new Random();

            var acervoBibliograficoLinha = new AcervoBibliograficoLinhaDTO()
            {
                Titulo = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                    EhCampoObrigatorio = true
                },
                SubTitulo = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                },
                Material = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                    EhCampoObrigatorio = true
                },
                Autor = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Person.FirstName.Limite(200)}|{faker.Person.LastName.Limite(200)}|{faker.Person.FullName.Limite(200)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                    EhCampoObrigatorio = true
                },
                CoAutor = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Address.SecondaryAddress().Limite(200)}|{faker.Address.FullAddress().Limite(200)}|{faker.Address.StreetAddress().Limite(200)}|{faker.Address.Country().Limite(200)}|{faker.Address.City().Limite(200)}|{faker.Address.State().Limite(200)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                TipoAutoria = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Company.CompanyName().Limite(15)}|{faker.Lorem.Sentence()}|{faker.Company.Cnpj().Limite(15)}|{faker.Company.Bs().Limite(15)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15
                },
                Editora = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Commerce.Department().Limite(200),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                Assunto = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Address.StreetAddress().Limite(200)}|{faker.Address.City().Limite(200)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                    EhCampoObrigatorio = true
                },
                Ano = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Date.Recent().Year.ToString(),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                    EhCampoObrigatorio = true
                },
                Edicao = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(15),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                },
                NumeroPaginas = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = random.Next(15,55).ToString(),
                    FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
                },
                Altura = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = random.Next(15,55).ToString(),
                    FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
                },
                Largura = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = random.Next(15,55).ToString(),
                    FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
                },
                SerieColecao = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(200),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                },
                Volume = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(15),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                },
                Idioma = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                    EhCampoObrigatorio = true
                },
                LocalizacaoCDD = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                    EhCampoObrigatorio = true
                },
                LocalizacaoPHA = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                    EhCampoObrigatorio = true
                },
                NotasGerais = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                },
                Isbn = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                },
                Tombo = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                    EhCampoObrigatorio = true
                }
            };
           
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(new List<AcervoBibliograficoLinhaDTO>() { acervoBibliograficoLinha });
            acervoBibliograficoLinha.TipoAutoria.Validado.ShouldBeFalse();
            acervoBibliograficoLinha.TipoAutoria.Mensagem.ShouldNotBeEmpty();
            
            acervoBibliograficoLinha.Titulo.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.SubTitulo.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Material.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Autor.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.CoAutor.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.NumeroPaginas.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Altura.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Largura.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Editora.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Assunto.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Edicao.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Ano.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.SerieColecao.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Volume.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Idioma.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.LocalizacaoCDD.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.LocalizacaoPHA.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.NotasGerais.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Isbn.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Tombo.Validado.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - ValidarPreenchimentoValorFormatoQtdeCaracteres - Edição")]
        public async Task Validar_preenchimento_valor_formato_qtde_caracteres_edicao()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var random = new Random();

            var acervoBibliograficoLinha = new AcervoBibliograficoLinhaDTO()
            {
                Titulo = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Text().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                    EhCampoObrigatorio = true
                },
                SubTitulo = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                },
                Material = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                    EhCampoObrigatorio = true
                },
                Autor = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Person.FirstName.Limite(200)}|{faker.Person.LastName.Limite(200)}|{faker.Person.FullName.Limite(200)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                    EhCampoObrigatorio = true
                },
                CoAutor = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Address.SecondaryAddress().Limite(200)}|{faker.Address.FullAddress().Limite(200)}|{faker.Address.StreetAddress().Limite(200)}|{faker.Address.Country().Limite(200)}|{faker.Address.City().Limite(200)}|{faker.Address.State().Limite(200)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                TipoAutoria = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Company.CompanyName().Limite(15)}|{faker.Lorem.Sentence().Limite(15)}|{faker.Company.Cnpj().Limite(15)}|{faker.Company.Bs().Limite(15)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15
                },
                Editora = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Commerce.Department().Limite(200),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                Assunto = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Address.StreetAddress().Limite(200)}|{faker.Address.City().Limite(200)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                    EhCampoObrigatorio = true
                },
                Ano = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Date.Recent().Year.ToString(),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                    EhCampoObrigatorio = true
                },
                Edicao = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Text(),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                },
                NumeroPaginas = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = random.Next(15,55).ToString(),
                    FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
                },
                Altura = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = random.Next(15,55).ToString(),
                    FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
                },
                Largura = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = random.Next(15,55).ToString(),
                    FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
                },
                SerieColecao = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(200),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                },
                Volume = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(15),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                },
                Idioma = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                    EhCampoObrigatorio = true
                },
                LocalizacaoCDD = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                    EhCampoObrigatorio = true
                },
                LocalizacaoPHA = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                    EhCampoObrigatorio = true
                },
                NotasGerais = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                },
                Isbn = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                },
                Tombo = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                    EhCampoObrigatorio = true
                }
            };
           
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(new List<AcervoBibliograficoLinhaDTO>() { acervoBibliograficoLinha });
            acervoBibliograficoLinha.Edicao.Validado.ShouldBeFalse();
            acervoBibliograficoLinha.Edicao.Mensagem.ShouldNotBeEmpty();
            
            acervoBibliograficoLinha.Titulo.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.SubTitulo.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Material.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Autor.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.CoAutor.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.NumeroPaginas.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Altura.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Largura.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Editora.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Assunto.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Ano.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.SerieColecao.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Volume.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Idioma.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.LocalizacaoCDD.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.LocalizacaoPHA.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.NotasGerais.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Isbn.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Tombo.Validado.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - ValidarPreenchimentoValorFormatoQtdeCaracteres - Número de Páginas, Altura e Largura")]
        public async Task Validar_preenchimento_valor_formato_qtde_caracteres_numero_paginas_altura_largura()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var random = new Random();

            var acervoBibliograficoLinha = new AcervoBibliograficoLinhaDTO()
            {
                Titulo = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Text().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                    EhCampoObrigatorio = true
                },
                SubTitulo = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                },
                Material = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                    EhCampoObrigatorio = true
                },
                Autor = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Person.FirstName.Limite(200)}|{faker.Person.LastName.Limite(200)}|{faker.Person.FullName.Limite(200)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                    EhCampoObrigatorio = true
                },
                CoAutor = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Address.SecondaryAddress().Limite(200)}|{faker.Address.FullAddress().Limite(200)}|{faker.Address.StreetAddress().Limite(200)}|{faker.Address.Country().Limite(200)}|{faker.Address.City().Limite(200)}|{faker.Address.State().Limite(200)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                TipoAutoria = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Company.CompanyName().Limite(15)}|{faker.Lorem.Sentence().Limite(15)}|{faker.Company.Cnpj().Limite(15)}|{faker.Company.Bs().Limite(15)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15
                },
                Editora = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Commerce.Department().Limite(200),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                Assunto = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Address.StreetAddress().Limite(200)}|{faker.Address.City().Limite(200)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                    EhCampoObrigatorio = true
                },
                Ano = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Date.Recent().Year.ToString(),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                    EhCampoObrigatorio = true
                },
                Edicao = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Text().Limite(15),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                },
                NumeroPaginas = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Text(),
                    FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
                },
                Altura = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Text(),
                    FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
                },
                Largura = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Text(),
                    FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
                },
                SerieColecao = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(200),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                },
                Volume = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(15),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                },
                Idioma = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                    EhCampoObrigatorio = true
                },
                LocalizacaoCDD = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                    EhCampoObrigatorio = true
                },
                LocalizacaoPHA = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                    EhCampoObrigatorio = true
                },
                NotasGerais = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                },
                Isbn = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                },
                Tombo = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                    EhCampoObrigatorio = true
                }
            };
           
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(new List<AcervoBibliograficoLinhaDTO>() { acervoBibliograficoLinha });
            acervoBibliograficoLinha.NumeroPaginas.Validado.ShouldBeFalse();
            acervoBibliograficoLinha.NumeroPaginas.Mensagem.ShouldNotBeEmpty();
                
            acervoBibliograficoLinha.Altura.Validado.ShouldBeFalse();
            acervoBibliograficoLinha.Altura.Mensagem.ShouldNotBeEmpty();
                
            acervoBibliograficoLinha.Largura.Validado.ShouldBeFalse();
            acervoBibliograficoLinha.Largura.Mensagem.ShouldNotBeEmpty();
            
            acervoBibliograficoLinha.Titulo.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.SubTitulo.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Material.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Autor.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.CoAutor.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.TipoAutoria.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Editora.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Assunto.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Ano.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Edicao.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.SerieColecao.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Volume.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Idioma.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.LocalizacaoCDD.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.LocalizacaoPHA.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.NotasGerais.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Isbn.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Tombo.Validado.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - ValidarPreenchimentoValorFormatoQtdeCaracteres - Material")]
        public async Task Validar_preenchimento_valor_formato_qtde_caracteres_material()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var random = new Random();

            var acervoBibliograficoLinha = new AcervoBibliograficoLinhaDTO()
            {
                Titulo = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Text().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                    EhCampoObrigatorio = true
                },
                SubTitulo = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                },
                Material = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = string.Empty,
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                    EhCampoObrigatorio = true
                },
                Autor = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Person.FirstName.Limite(200)}|{faker.Person.LastName.Limite(200)}|{faker.Person.FullName.Limite(200)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                    EhCampoObrigatorio = true
                },
                CoAutor = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Address.SecondaryAddress().Limite(200)}|{faker.Address.FullAddress().Limite(200)}|{faker.Address.StreetAddress().Limite(200)}|{faker.Address.Country().Limite(200)}|{faker.Address.City().Limite(200)}|{faker.Address.State().Limite(200)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                TipoAutoria = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Company.CompanyName().Limite(15)}|{faker.Lorem.Sentence().Limite(15)}|{faker.Company.Cnpj().Limite(15)}|{faker.Company.Bs().Limite(15)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15
                },
                Editora = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Commerce.Department().Limite(200),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                Assunto = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Address.StreetAddress().Limite(200)}|{faker.Address.City().Limite(200)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                    EhCampoObrigatorio = true
                },
                Ano = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Date.Recent().Year.ToString(),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                    EhCampoObrigatorio = true
                },
                Edicao = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Text().Limite(15),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                },
                NumeroPaginas = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = random.Next(15,55).ToString(),
                    FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
                },
                Altura = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = random.Next(15,55).ToString(),
                    FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
                },
                Largura = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = random.Next(15,55).ToString(),
                    FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
                },
                SerieColecao = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(200),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                },
                Volume = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(15),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                },
                Idioma = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                    EhCampoObrigatorio = true
                },
                LocalizacaoCDD = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                    EhCampoObrigatorio = true
                },
                LocalizacaoPHA = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                    EhCampoObrigatorio = true
                },
                NotasGerais = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                },
                Isbn = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                },
                Tombo = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                    EhCampoObrigatorio = true
                }
            };
           
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(new List<AcervoBibliograficoLinhaDTO>() { acervoBibliograficoLinha });
            acervoBibliograficoLinha.Material.Validado.ShouldBeFalse();
            acervoBibliograficoLinha.Material.Mensagem.ShouldNotBeEmpty();
            
            acervoBibliograficoLinha.Titulo.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.SubTitulo.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Autor.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.CoAutor.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.NumeroPaginas.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Altura.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Edicao.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Largura.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Editora.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Assunto.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Ano.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.SerieColecao.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Volume.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Idioma.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.LocalizacaoCDD.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.LocalizacaoPHA.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.NotasGerais.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Isbn.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Tombo.Validado.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - ValidarPreenchimentoValorFormatoQtdeCaracteres - Volume")]
        public async Task Validar_preenchimento_valor_formato_qtde_caracteres_volume()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var random = new Random();

            var acervoBibliograficoLinha = new AcervoBibliograficoLinhaDTO()
            {
                Titulo = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Text().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                    EhCampoObrigatorio = true
                },
                SubTitulo = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                },
                Material = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                    EhCampoObrigatorio = true
                },
                Autor = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Person.FirstName.Limite(200)}|{faker.Person.LastName.Limite(200)}|{faker.Person.FullName.Limite(200)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                    EhCampoObrigatorio = true
                },
                CoAutor = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Address.SecondaryAddress().Limite(200)}|{faker.Address.FullAddress().Limite(200)}|{faker.Address.StreetAddress().Limite(200)}|{faker.Address.Country().Limite(200)}|{faker.Address.City().Limite(200)}|{faker.Address.State().Limite(200)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                TipoAutoria = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Company.CompanyName().Limite(15)}|{faker.Lorem.Sentence().Limite(15)}|{faker.Company.Cnpj().Limite(15)}|{faker.Company.Bs().Limite(15)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15
                },
                Editora = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Commerce.Department().Limite(200),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                Assunto = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Address.StreetAddress().Limite(200)}|{faker.Address.City().Limite(200)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                    EhCampoObrigatorio = true
                },
                Ano = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Date.Recent().Year.ToString(),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                    EhCampoObrigatorio = true
                },
                Edicao = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Text().Limite(15),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                },
                NumeroPaginas = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = random.Next(15,55).ToString(),
                    FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
                },
                Altura = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = random.Next(15,55).ToString(),
                    FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
                },
                Largura = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = random.Next(15,55).ToString(),
                    FormatoTipoDeCampo = ConstantesTestes.FORMATO_DOUBLE
                },
                SerieColecao = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(200),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200,
                },
                Volume = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Text(),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                },
                Idioma = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                    EhCampoObrigatorio = true
                },
                LocalizacaoCDD = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                    EhCampoObrigatorio = true
                },
                LocalizacaoPHA = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                    EhCampoObrigatorio = true
                },
                NotasGerais = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(500),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_500,
                },
                Isbn = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                },
                Tombo = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = faker.Lorem.Sentence().Limite(50),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_50,
                    EhCampoObrigatorio = true
                }
            };
           
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(new List<AcervoBibliograficoLinhaDTO>() { acervoBibliograficoLinha });
            acervoBibliograficoLinha.Volume.Validado.ShouldBeFalse();
            acervoBibliograficoLinha.Volume.Mensagem.ShouldNotBeEmpty();
            
            acervoBibliograficoLinha.Titulo.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.SubTitulo.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Material.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Autor.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.CoAutor.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.NumeroPaginas.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Altura.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Edicao.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Largura.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Editora.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Assunto.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Ano.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.SerieColecao.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Idioma.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.LocalizacaoCDD.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.LocalizacaoPHA.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.NotasGerais.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Isbn.Validado.ShouldBeTrue();
            acervoBibliograficoLinha.Tombo.Validado.ShouldBeTrue();
        }
    }
}