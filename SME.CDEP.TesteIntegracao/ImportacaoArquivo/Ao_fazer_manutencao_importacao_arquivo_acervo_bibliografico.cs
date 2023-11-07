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
    public class Ao_fazer_manutencao_importacao_arquivo_acervo_bibliografico : TesteBase
    {
        public Ao_fazer_manutencao_importacao_arquivo_acervo_bibliografico(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - ObterCoAutoresTipoAutoria com tipo autoria nos 3 primeiros coautores")]
        public async Task Validar_obter_coautores_tipo_autoria_com_tipo_autoria_nos_tres_primeiros_coautores()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var acervoBibliograficoLinha = new AcervoBibliograficoLinhaDTO()
            {
                CoAutor = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Lorem.Text().Limite(200)}|{faker.Lorem.Sentence().Limite(200)}|{faker.Lorem.Word().Limite(200)}|{faker.Lorem.Letter().Limite(200)}|{faker.Lorem.Slug().Limite(200)}|{faker.Lorem.Lines().Limite(200)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                TipoAutoria = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Lorem.Text().Limite(15)}|{faker.Lorem.Word().Limite(15)}|{faker.Lorem.Sentence().Limite(15)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15
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
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                TipoAutoria = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Company.CompanyName().Limite(15)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15
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
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                TipoAutoria = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = string.Empty,
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15
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
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_200
                },
                TipoAutoria = new LinhaConteudoAjustarDTO()
                {
                    Conteudo = $"{faker.Company.CompanyName().Limite(15)}|{faker.Company.CompanySuffix().Limite(15)}|{faker.Company.Cnpj().Limite(15)}|{faker.Company.Bs().Limite(15)}|{faker.Company.CatchPhrase().Limite(15)}|{faker.Commerce.Locale.Limite(15)}",
                    LimiteCaracteres = ConstantesTestes.CARACTERES_PERMITIDOS_15
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
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - ValidarPreenchimentoValorFormatoQtdeCaracteres - Com erros: Ano, Material, Edição, Número de Páginas e Volume")]
        public async Task Validar_preenchimento_valor_formato_qtde_caracteres()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var acervoBibliograficoLinhas = GerarAcervoBibliograficoLinhaDTO().Generate(10);

            acervoBibliograficoLinhas[2].Ano.Conteudo = faker.Lorem.Paragraph();
            acervoBibliograficoLinhas[4].Material.Conteudo = string.Empty;
            acervoBibliograficoLinhas[5].Edicao.Conteudo = faker.Lorem.Paragraph();
            acervoBibliograficoLinhas[7].NumeroPaginas.Conteudo = faker.Lorem.Paragraph();
            acervoBibliograficoLinhas[8].Volume.Conteudo = faker.Lorem.Paragraph();
            var linhasComErros = new[] { 3, 5, 6, 8, 9 };
            
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoBibliograficoLinhas);

            foreach (var linha in acervoBibliograficoLinhas)
            {
                linha.PossuiErros.ShouldBe(linhasComErros.Any(a=> a == linha.NumeroLinha));

                if (linha.NumeroLinha == 3)
                {
                    linha.Ano.PossuiErro.ShouldBeTrue();
                    linha.Ano.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Ano.PossuiErro.ShouldBeFalse();
                    linha.Ano.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha == 5)
                {
                    linha.Material.PossuiErro.ShouldBeTrue();
                    linha.Material.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Material.PossuiErro.ShouldBeFalse();
                    linha.Material.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha == 6)
                {
                    linha.Edicao.PossuiErro.ShouldBeTrue();
                    linha.Edicao.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Edicao.PossuiErro.ShouldBeFalse();
                    linha.Edicao.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha == 8)
                {
                    linha.NumeroPaginas.PossuiErro.ShouldBeTrue();
                    linha.NumeroPaginas.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.NumeroPaginas.PossuiErro.ShouldBeFalse();
                    linha.NumeroPaginas.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha == 9)
                {
                    linha.Volume.PossuiErro.ShouldBeTrue();
                    linha.Volume.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Volume.PossuiErro.ShouldBeFalse();
                    linha.Volume.Mensagem.ShouldBeEmpty();
                }
                
                linha.TipoAutoria.PossuiErro.ShouldBeFalse();
                linha.Titulo.PossuiErro.ShouldBeFalse();
                linha.SubTitulo.PossuiErro.ShouldBeFalse();
                linha.Autor.PossuiErro.ShouldBeFalse();
                linha.CoAutor.PossuiErro.ShouldBeFalse();
                linha.Altura.PossuiErro.ShouldBeFalse();
                linha.Largura.PossuiErro.ShouldBeFalse();
                linha.Editora.PossuiErro.ShouldBeFalse();
                linha.Assunto.PossuiErro.ShouldBeFalse();
                linha.SerieColecao.PossuiErro.ShouldBeFalse();
                linha.Idioma.PossuiErro.ShouldBeFalse();
                linha.LocalizacaoCDD.PossuiErro.ShouldBeFalse();
                linha.LocalizacaoPHA.PossuiErro.ShouldBeFalse();
                linha.NotasGerais.PossuiErro.ShouldBeFalse();
                linha.Isbn.PossuiErro.ShouldBeFalse();
                linha.Tombo.PossuiErro.ShouldBeFalse();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - ValidacaoObterOuInserirDominios")]
        public async Task Validacao_obter_ou_inserir_dominios()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var acervoBibliograficoLinhas = GerarAcervoBibliograficoLinhaDTO().Generate(10);
           
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoBibliograficoLinhas);

            foreach (var linha in acervoBibliograficoLinhas)
            {
                var materialInserido = linha.Material.Conteudo;
                var materiais = ObterTodos<Material>();
                materiais.Any(a => a.Nome.Equals(materialInserido)).ShouldBeTrue();

                var editoraInserida = linha.Editora.Conteudo;
                var editoras = ObterTodos<Editora>();
                editoras.Any(a => a.Nome.Equals(editoraInserida)).ShouldBeTrue();

                var serieColecaoInserida = linha.SerieColecao.Conteudo;
                var serieColecaos = ObterTodos<SerieColecao>();
                serieColecaos.Any(a => a.Nome.Equals(serieColecaoInserida)).ShouldBeTrue();

                var idiomaInserido = linha.Idioma.Conteudo;
                var idiomas = ObterTodos<Idioma>();
                idiomas.Any(a => a.Nome.Equals(idiomaInserido)).ShouldBeTrue();

                var assuntosInseridos = linha.Assunto.Conteudo.FormatarTextoEmArray().ToArray()
                    .UnificarPipe().SplitPipe().Distinct();
                var assuntos = ObterTodos<Assunto>();
                foreach (var assunto in assuntosInseridos)
                    assuntos.Any(a => a.Nome.Equals(assunto)).ShouldBeTrue();

                var creditoAutorInseridos = linha.Autor.Conteudo.FormatarTextoEmArray().ToArray()
                    .UnificarPipe().SplitPipe().Distinct();
                var creditosAutores = ObterTodos<CreditoAutor>();
                foreach (var creditoAutor in creditoAutorInseridos)
                    creditosAutores.Any(a => a.Nome.Equals(creditoAutor)).ShouldBeTrue();

                creditoAutorInseridos = linha.CoAutor.Conteudo.FormatarTextoEmArray().ToArray()
                    .UnificarPipe().SplitPipe().Distinct();
                foreach (var creditoAutor in creditoAutorInseridos)
                    creditosAutores.Any(a => a.Nome.Equals(creditoAutor)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - PersistenciaAcervobibliografico")]
        public async Task Persistencia_acervo_bibliografico()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var acervoBibliograficoLinhas = GerarAcervoBibliograficoLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Bibliografico,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoBibliograficoLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoBibliograficoLinhas);
            await servicoImportacaoArquivo.PersistenciaAcervoBibliografico(acervoBibliograficoLinhas,1);

            var acervos = ObterTodos<Acervo>();
            var acervosBibliograficos = ObterTodos<AcervoBibliografico>();
            var materiais = ObterTodos<Material>();
            var editoras = ObterTodos<Editora>();
            var serieColecoes = ObterTodos<SerieColecao>();
            var idiomas = ObterTodos<Idioma>();
            var assuntos = ObterTodos<Assunto>();
            var acervoBibliograficoAssuntos = ObterTodos<AcervoBibliograficoAssunto>();
            var acervoCreditoAutors = ObterTodos<AcervoCreditoAutor>();
            var creditoAutores = ObterTodos<CreditoAutor>();
            
            //Acervos inseridos
            acervos.ShouldNotBeNull();
            acervos.Count().ShouldBe(10);
            
            //Acervos bibliográficos inseridos
            acervosBibliograficos.ShouldNotBeNull();
            acervosBibliograficos.Count().ShouldBe(10);
            
            //Linhas com erros
            acervoBibliograficoLinhas.Count(w=> !w.PossuiErros).ShouldBe(10);
            acervoBibliograficoLinhas.Count(w=> w.PossuiErros).ShouldBe(0);

            foreach (var linhasComSucesso in acervoBibliograficoLinhas.Where(w=> !w.PossuiErros))
            {
                //Acervo
                acervos.Any(a=> a.Titulo.Equals(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.SubTitulo.Equals(linhasComSucesso.SubTitulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.Equals(linhasComSucesso.Tombo.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosBibliograficos.Any(a=> a.MaterialId == materiais.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Material.Conteudo)).Id).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.EditoraId == editoras.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Editora.Conteudo)).Id).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.SerieColecaoId == serieColecoes.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.SerieColecao.Conteudo)).Id).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.IdiomaId == idiomas.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Idioma.Conteudo)).Id).ShouldBeTrue();
                
                //Campos livres
                acervosBibliograficos.Any(a=> a.Ano == linhasComSucesso.Ano.Conteudo).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Edicao == linhasComSucesso.Edicao.Conteudo).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.NumeroPagina == double.Parse(linhasComSucesso.NumeroPaginas.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Altura == double.Parse(linhasComSucesso.Altura.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Largura == double.Parse(linhasComSucesso.Largura.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Volume == linhasComSucesso.Volume.Conteudo).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.LocalizacaoCDD == linhasComSucesso.LocalizacaoCDD.Conteudo).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.LocalizacaoPHA == linhasComSucesso.LocalizacaoPHA.Conteudo).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.NotasGerais == linhasComSucesso.NotasGerais.Conteudo).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Isbn == linhasComSucesso.Isbn.Conteudo).ShouldBeTrue();
                
                //Assuntos
                var assuntosAInserir = linhasComSucesso.Assunto.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var assunto in assuntosAInserir)
                    assuntos.Any(a=> a.Nome.Equals(assunto)).ShouldBeTrue();
                
                foreach (var acervoBibliograficoAssunto in acervoBibliograficoAssuntos)
                    assuntos.Any(a=> a.Id == acervoBibliograficoAssunto.AssuntoId).ShouldBeTrue();
                
                //Autor
                var autorAInserir = linhasComSucesso.Autor.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var autor in autorAInserir)
                    creditoAutores.Any(a=> a.Nome.Equals(autor)).ShouldBeTrue();
                
                foreach (var creditoAutor in acervoCreditoAutors)
                    creditoAutores.Any(a=> a.Id == creditoAutor.CreditoAutorId).ShouldBeTrue();
                
                //Coautor
                var coAutorAInserir = linhasComSucesso.CoAutor.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var coautor in coAutorAInserir)
                    creditoAutores.Any(a=> a.Nome.Equals(coautor)).ShouldBeTrue();
                
                //TipoAutoria
                var tipoAutoriaAInserir = linhasComSucesso.TipoAutoria.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var tipoAutoria in tipoAutoriaAInserir)
                    acervoCreditoAutors.Any(a=> a.TipoAutoria.NaoEhNulo() && a.TipoAutoria.Equals(tipoAutoria)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Geral - Com erros em 3 linhas")]
        public async Task Importacao_geral()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var acervoBibliograficoLinhas = GerarAcervoBibliograficoLinhaDTO().Generate(10);
           
            acervoBibliograficoLinhas[3].Largura.Conteudo = "ABC3512";
            acervoBibliograficoLinhas[5].Altura.Conteudo = "1212ABC";
            acervoBibliograficoLinhas[7].Tombo.Conteudo = acervoBibliograficoLinhas[0].Tombo.Conteudo;
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Bibliografico,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoBibliograficoLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoBibliograficoLinhas);
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoBibliograficoLinhas );
            await servicoImportacaoArquivo.PersistenciaAcervoBibliografico(acervoBibliograficoLinhas,1);

            var acervos = ObterTodos<Acervo>();
            var acervosBibliograficos = ObterTodos<AcervoBibliografico>();
            var materiais = ObterTodos<Material>();
            var editoras = ObterTodos<Editora>();
            var serieColecoes = ObterTodos<SerieColecao>();
            var idiomas = ObterTodos<Idioma>();
            var assuntos = ObterTodos<Assunto>();
            var acervoBibliograficoAssuntos = ObterTodos<AcervoBibliograficoAssunto>();
            var acervoCreditoAutors = ObterTodos<AcervoCreditoAutor>();
            var creditoAutores = ObterTodos<CreditoAutor>();
            
            //Acervos inseridos
            acervos.ShouldNotBeNull();
            acervos.Count().ShouldBe(7);
            
            //Acervos bibliográficos inseridos
            acervosBibliograficos.ShouldNotBeNull();
            acervosBibliograficos.Count().ShouldBe(7);
            
            //Linhas com erros
            acervoBibliograficoLinhas.Count(w=> !w.PossuiErros).ShouldBe(7);
            acervoBibliograficoLinhas.Count(w=> w.PossuiErros).ShouldBe(3);

            foreach (var linhasComSucesso in acervoBibliograficoLinhas.Where(w=> !w.PossuiErros))
            {
                //Acervo
                acervos.Any(a=> a.Titulo.Equals(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.SubTitulo.Equals(linhasComSucesso.SubTitulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.Equals(linhasComSucesso.Tombo.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosBibliograficos.Any(a=> a.MaterialId == materiais.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Material.Conteudo)).Id).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.EditoraId == editoras.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Editora.Conteudo)).Id).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.SerieColecaoId == serieColecoes.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.SerieColecao.Conteudo)).Id).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.IdiomaId == idiomas.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Idioma.Conteudo)).Id).ShouldBeTrue();
                
                //Campos livres
                acervosBibliograficos.Any(a=> a.Ano == linhasComSucesso.Ano.Conteudo).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Edicao == linhasComSucesso.Edicao.Conteudo).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.NumeroPagina == double.Parse(linhasComSucesso.NumeroPaginas.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Altura == double.Parse(linhasComSucesso.Altura.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Largura == double.Parse(linhasComSucesso.Largura.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Volume == linhasComSucesso.Volume.Conteudo).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.LocalizacaoCDD == linhasComSucesso.LocalizacaoCDD.Conteudo).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.LocalizacaoPHA == linhasComSucesso.LocalizacaoPHA.Conteudo).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.NotasGerais == linhasComSucesso.NotasGerais.Conteudo).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Isbn == linhasComSucesso.Isbn.Conteudo).ShouldBeTrue();
                
                //Assuntos
                var assuntosAInserir = linhasComSucesso.Assunto.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var assunto in assuntosAInserir)
                    assuntos.Any(a=> a.Nome.Equals(assunto)).ShouldBeTrue();
                
                foreach (var acervoBibliograficoAssunto in acervoBibliograficoAssuntos)
                    assuntos.Any(a=> a.Id == acervoBibliograficoAssunto.AssuntoId).ShouldBeTrue();
                
                //Autor
                var autorAInserir = linhasComSucesso.Autor.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var autor in autorAInserir)
                    creditoAutores.Any(a=> a.Nome.Equals(autor)).ShouldBeTrue();
                
                foreach (var creditoAutor in acervoCreditoAutors)
                    creditoAutores.Any(a=> a.Id == creditoAutor.CreditoAutorId).ShouldBeTrue();
                
                //Coautor
                var coAutorAInserir = linhasComSucesso.CoAutor.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var coautor in coAutorAInserir)
                    creditoAutores.Any(a=> a.Nome.Equals(coautor)).ShouldBeTrue();
                
                //TipoAutoria
                var tipoAutoriaAInserir = linhasComSucesso.TipoAutoria.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var tipoAutoria in tipoAutoriaAInserir)
                    acervoCreditoAutors.Any(a=> a.TipoAutoria.NaoEhNulo() && a.TipoAutoria.Equals(tipoAutoria)).ShouldBeTrue();
                
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Obter importação pendente com Erros")]
        public async Task Obter_importacao_pendente_com_erros()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var linhasInseridas = GerarAcervoBibliograficoLinhaDTO().Generate(10);

            linhasInseridas[3].PossuiErros = true;
            linhasInseridas[3].Volume.PossuiErro = true;
            linhasInseridas[3].Volume.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.VOLUME);
            
            linhasInseridas[9].PossuiErros = true;
            linhasInseridas[9].Altura.PossuiErro = true;
            linhasInseridas[9].Altura.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.ALTURA);
           
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Bibliografico,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            var retorno = await servicoImportacaoArquivo.ObterImportacaoPendente();
            retorno.ShouldNotBeNull();
            retorno.Sucesso.Count().ShouldBe(8);
            retorno.Erros.Count().ShouldBe(2);

            foreach (var linhaInserida in linhasInseridas.Where(w=> !w.PossuiErros))
            {
                retorno.Sucesso.Any(a=> a.Titulo.Conteudo.Equals(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.SubTitulo.Conteudo.Equals(linhaInserida.SubTitulo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Material.Conteudo.Equals(linhaInserida.Material.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Autor.Conteudo.Equals(linhaInserida.Autor.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.CoAutor.Conteudo.Equals(linhaInserida.CoAutor.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.TipoAutoria.Conteudo.Equals(linhaInserida.TipoAutoria.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Editora.Conteudo.Equals(linhaInserida.Editora.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Assunto.Conteudo.Equals(linhaInserida.Assunto.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Ano.Conteudo.Equals(linhaInserida.Ano.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Edicao.Conteudo.Equals(linhaInserida.Edicao.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.NumeroPaginas.Conteudo.Equals(linhaInserida.NumeroPaginas.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Altura.Conteudo.Equals(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Largura.Conteudo.Equals(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.SerieColecao.Conteudo.Equals(linhaInserida.SerieColecao.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Volume.Conteudo.Equals(linhaInserida.Volume.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Idioma.Conteudo.Equals(linhaInserida.Idioma.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.LocalizacaoCDD.Conteudo.Equals(linhaInserida.LocalizacaoCDD.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.LocalizacaoPHA.Conteudo.Equals(linhaInserida.LocalizacaoPHA.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.NotasGerais.Conteudo.Equals(linhaInserida.NotasGerais.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Isbn.Conteudo.Equals(linhaInserida.Isbn.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Tombo.Conteudo.Equals(linhaInserida.Tombo.Conteudo)).ShouldBeTrue();
            }
            
            foreach (var linhaInserida in linhasInseridas.Where(w=> w.PossuiErros))
            {
                retorno.Erros.Any(a=> a.Titulo.Conteudo.Equals(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.SubTitulo.Conteudo.Equals(linhaInserida.SubTitulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Material.Conteudo.Equals(linhaInserida.Material.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Autor.Conteudo.Equals(linhaInserida.Autor.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.CoAutor.Conteudo.Equals(linhaInserida.CoAutor.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.TipoAutoria.Conteudo.Equals(linhaInserida.TipoAutoria.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Editora.Conteudo.Equals(linhaInserida.Editora.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Assunto.Conteudo.Equals(linhaInserida.Assunto.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Ano.Conteudo.Equals(linhaInserida.Ano.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Edicao.Conteudo.Equals(linhaInserida.Edicao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.NumeroPaginas.Conteudo.Equals(linhaInserida.NumeroPaginas.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Altura.Conteudo.Equals(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Largura.Conteudo.Equals(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.SerieColecao.Conteudo.Equals(linhaInserida.SerieColecao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Volume.Conteudo.Equals(linhaInserida.Volume.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Idioma.Conteudo.Equals(linhaInserida.Idioma.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.LocalizacaoCDD.Conteudo.Equals(linhaInserida.LocalizacaoCDD.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.LocalizacaoPHA.Conteudo.Equals(linhaInserida.LocalizacaoPHA.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.NotasGerais.Conteudo.Equals(linhaInserida.NotasGerais.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Isbn.Conteudo.Equals(linhaInserida.Isbn.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Tombo.Conteudo.Equals(linhaInserida.Tombo.Conteudo)).ShouldBeTrue();
            }
        }
    }
}