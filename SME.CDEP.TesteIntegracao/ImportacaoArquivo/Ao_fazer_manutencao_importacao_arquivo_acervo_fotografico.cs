using Bogus.Extensions.Brazil;
using Newtonsoft.Json;
using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Constantes;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_fazer_manutencao_importacao_arquivo_acervo_fotografico : TesteBase
    {
        public Ao_fazer_manutencao_importacao_arquivo_acervo_fotografico(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - ObterCoAutoresTipoAutoria com tipo autoria nos 3 primeiros coautores")]
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
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - ObterCoAutoresTipoAutoria com tipo autoria somente no primeiro coautor")]
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
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - ObterCoAutoresTipoAutoria sem tipo autoria")]
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
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - ObterCoAutoresTipoAutoria com tipo autoria e coautores iguais")]
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
                    Conteudo = faker.Lorem.Sentence().Limite(15),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
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
                    Conteudo = faker.Lorem.Sentence().Limite(15),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
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
                    Conteudo = faker.Lorem.Sentence().Limite(15),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
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
                    Conteudo = faker.Lorem.Sentence().Limite(15),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
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
                NumeroLinha = 1,
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
                    Conteudo = faker.Lorem.Sentence().Limite(15),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
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
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - ValidacaoObterOuInserirDominios")]
        public async Task Validacao_obter_ou_inserir_dominios()
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
                    Conteudo = faker.Lorem.Text().Limite(15),
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
                    Conteudo = faker.Lorem.Sentence().Limite(15),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                    EhCampoObrigatorio = true
                }
            };
           
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(new List<AcervoBibliograficoLinhaDTO>() { acervoBibliograficoLinha });

            var materialInserido = acervoBibliograficoLinha.Material.Conteudo;
            var materiais = ObterTodos<Material>();
            materiais.Any(a=> a.Nome.Equals(materialInserido)).ShouldBeTrue();

            var editoraInserida = acervoBibliograficoLinha.Editora.Conteudo;
            var editoras = ObterTodos<Editora>();
            editoras.Any(a=> a.Nome.Equals(editoraInserida)).ShouldBeTrue();
            
            var serieColecaoInserida = acervoBibliograficoLinha.SerieColecao.Conteudo;
            var serieColecaos = ObterTodos<SerieColecao>();
            serieColecaos.Any(a=> a.Nome.Equals(serieColecaoInserida)).ShouldBeTrue();
            
            var idiomaInserido = acervoBibliograficoLinha.Idioma.Conteudo;
            var idiomas = ObterTodos<Idioma>();
            idiomas.Any(a=> a.Nome.Equals(idiomaInserido)).ShouldBeTrue();
            
            var assuntosInseridos = acervoBibliograficoLinha.Assunto.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
            var assuntos = ObterTodos<Assunto>();
            foreach (var assunto in assuntosInseridos)
                assuntos.Any(a=> a.Nome.Equals(assunto)).ShouldBeTrue();
            
            var creditoAutorInseridos = acervoBibliograficoLinha.Autor.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
            var creditosAutores = ObterTodos<CreditoAutor>();
            foreach (var creditoAutor in creditoAutorInseridos)
                creditosAutores.Any(a=> a.Nome.Equals(creditoAutor)).ShouldBeTrue();
            
            creditoAutorInseridos = acervoBibliograficoLinha.CoAutor.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
            foreach (var creditoAutor in creditoAutorInseridos)
                creditosAutores.Any(a=> a.Nome.Equals(creditoAutor)).ShouldBeTrue();
            
            var tipoAutorias = acervoBibliograficoLinha.TipoAutoria.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
            creditosAutores = ObterTodos<CreditoAutor>();
            foreach (var tipoAutoria in tipoAutorias)
                creditosAutores.Any(a => a.Tipo.Equals(tipoAutoria));
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - PersistenciaAcervobibliografico")]
        public async Task Persistencia_acervo_bibliografico()
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
                    Conteudo = faker.Lorem.Text().Limite(15),
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
                    Conteudo = faker.Lorem.Sentence().Limite(15),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                    EhCampoObrigatorio = true
                }
            };
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Bibliografico,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoBibliograficoLinha),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(new List<AcervoBibliograficoLinhaDTO>() { acervoBibliograficoLinha });
            await servicoImportacaoArquivo.PersistenciaAcervoBibliografico(new List<AcervoBibliograficoLinhaDTO>() { acervoBibliograficoLinha },1);

            var acervosBibliograficos = ObterTodos<AcervoBibliografico>();
            acervosBibliograficos.ShouldNotBeNull();

            var acervoBibliograficoInserido = acervosBibliograficos.FirstOrDefault();
            
            var materiais = ObterTodos<Material>();
            acervoBibliograficoLinha.Material.Conteudo.Equals(materiais.FirstOrDefault().Nome);
            acervoBibliograficoInserido.MaterialId.ShouldBe(acervoBibliograficoInserido.MaterialId);
            
            var editoras = ObterTodos<Editora>();
            acervoBibliograficoLinha.Editora.Conteudo.Equals(editoras.FirstOrDefault().Nome);
            acervoBibliograficoInserido.EditoraId.ShouldBe(acervoBibliograficoInserido.EditoraId);
            
            var serieColecaoInserida = ObterTodos<SerieColecao>();
            acervoBibliograficoLinha.SerieColecao.Conteudo.Equals(serieColecaoInserida.FirstOrDefault().Nome);
            acervoBibliograficoInserido.SerieColecaoId.ShouldBe(acervoBibliograficoInserido.SerieColecaoId);
            
            var idiomaInserido = ObterTodos<Idioma>();
            acervoBibliograficoLinha.Idioma.Conteudo.Equals(idiomaInserido.FirstOrDefault().Nome);
            acervoBibliograficoInserido.IdiomaId.ShouldBe(acervoBibliograficoInserido.IdiomaId); 
            
            var assuntosAInserir = acervoBibliograficoLinha.Assunto.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
            var assuntosInseridos = ObterTodos<Assunto>();
            
            foreach (var assunto in assuntosAInserir)
                assuntosInseridos.Any(a=> a.Nome.Equals(assunto)).ShouldBeTrue();
            
            var acervoBibliograficoAssuntos = ObterTodos<AcervoBibliograficoAssunto>();
            foreach (var acervoBibliograficoAssunto in acervoBibliograficoAssuntos)
                assuntosInseridos.Any(a=> a.Id == acervoBibliograficoAssunto.AssuntoId).ShouldBeTrue();
            
            var autorAInserir = acervoBibliograficoLinha.Autor.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
            var creditoAutoresInseridos = ObterTodos<CreditoAutor>();
            
            foreach (var autor in autorAInserir)
                creditoAutoresInseridos.Any(a=> a.Nome.Equals(autor)).ShouldBeTrue();
            
            var acervoCreditoAutors = ObterTodos<AcervoCreditoAutor>();
            foreach (var creditoAutor in acervoCreditoAutors)
                creditoAutoresInseridos.Any(a=> a.Id == creditoAutor.CreditoAutorId).ShouldBeTrue();
            
            var coAutorAInserir = acervoBibliograficoLinha.CoAutor.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
            foreach (var coautor in coAutorAInserir)
                creditoAutoresInseridos.Any(a=> a.Nome.Equals(coautor)).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Geral")]
        public async Task Importacao_geral()
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
                    Conteudo = faker.Lorem.Text().Limite(15),
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
                    Conteudo = faker.Lorem.Sentence().Limite(15),
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15,
                    EhCampoObrigatorio = true
                }
            };
           
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Bibliografico,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoBibliograficoLinha),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(new List<AcervoBibliograficoLinhaDTO>() { acervoBibliograficoLinha });
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(new List<AcervoBibliograficoLinhaDTO>() { acervoBibliograficoLinha });
            await servicoImportacaoArquivo.PersistenciaAcervoBibliografico(new List<AcervoBibliograficoLinhaDTO>() { acervoBibliograficoLinha },1);

            var acervosBibliograficos = ObterTodos<AcervoBibliografico>();
            acervosBibliograficos.ShouldNotBeNull();

            var acervoBibliograficoInserido = acervosBibliograficos.FirstOrDefault();
            
            var materiais = ObterTodos<Material>();
            acervoBibliograficoLinha.Material.Conteudo.Equals(materiais.FirstOrDefault().Nome);
            acervoBibliograficoInserido.MaterialId.ShouldBe(acervoBibliograficoInserido.MaterialId);
            
            var editoras = ObterTodos<Editora>();
            acervoBibliograficoLinha.Editora.Conteudo.Equals(editoras.FirstOrDefault().Nome);
            acervoBibliograficoInserido.EditoraId.ShouldBe(acervoBibliograficoInserido.EditoraId);
            
            var serieColecaoInserida = ObterTodos<SerieColecao>();
            acervoBibliograficoLinha.SerieColecao.Conteudo.Equals(serieColecaoInserida.FirstOrDefault().Nome);
            acervoBibliograficoInserido.SerieColecaoId.ShouldBe(acervoBibliograficoInserido.SerieColecaoId);
            
            var idiomaInserido = ObterTodos<Idioma>();
            acervoBibliograficoLinha.Idioma.Conteudo.Equals(idiomaInserido.FirstOrDefault().Nome);
            acervoBibliograficoInserido.IdiomaId.ShouldBe(acervoBibliograficoInserido.IdiomaId); 
            
            var assuntosAInserir = acervoBibliograficoLinha.Assunto.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
            var assuntosInseridos = ObterTodos<Assunto>();
            
            foreach (var assunto in assuntosAInserir)
                assuntosInseridos.Any(a=> a.Nome.Equals(assunto)).ShouldBeTrue();
            
            var acervoBibliograficoAssuntos = ObterTodos<AcervoBibliograficoAssunto>();
            foreach (var acervoBibliograficoAssunto in acervoBibliograficoAssuntos)
                assuntosInseridos.Any(a=> a.Id == acervoBibliograficoAssunto.AssuntoId).ShouldBeTrue();
            
            var autorAInserir = acervoBibliograficoLinha.Autor.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
            var creditoAutoresInseridos = ObterTodos<CreditoAutor>();
            
            foreach (var autor in autorAInserir)
                creditoAutoresInseridos.Any(a=> a.Nome.Equals(autor)).ShouldBeTrue();
            
            var acervoCreditoAutors = ObterTodos<AcervoCreditoAutor>();
            foreach (var creditoAutor in acervoCreditoAutors)
                creditoAutoresInseridos.Any(a=> a.Id == creditoAutor.CreditoAutorId).ShouldBeTrue();
            
            var coAutorAInserir = acervoBibliograficoLinha.CoAutor.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
            foreach (var coautor in coAutorAInserir)
                creditoAutoresInseridos.Any(a=> a.Nome.Equals(coautor)).ShouldBeTrue();
        }
    }
}