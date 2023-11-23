using Bogus.Extensions.Brazil;
using Newtonsoft.Json;
using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
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
                
                if (i.SaoIguais(0))
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
                linha.PossuiErros.ShouldBe(linhasComErros.Any(a=> a.SaoIguais(linha.NumeroLinha)));

                if (linha.NumeroLinha.SaoIguais(3))
                {
                    linha.Ano.PossuiErro.ShouldBeTrue();
                    linha.Ano.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Ano.PossuiErro.ShouldBeFalse();
                    linha.Ano.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha.SaoIguais(5))
                {
                    linha.Material.PossuiErro.ShouldBeTrue();
                    linha.Material.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Material.PossuiErro.ShouldBeFalse();
                    linha.Material.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha.SaoIguais(6))
                {
                    linha.Edicao.PossuiErro.ShouldBeTrue();
                    linha.Edicao.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Edicao.PossuiErro.ShouldBeFalse();
                    linha.Edicao.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha.SaoIguais(8))
                {
                    linha.NumeroPaginas.PossuiErro.ShouldBeTrue();
                    linha.NumeroPaginas.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.NumeroPaginas.PossuiErro.ShouldBeFalse();
                    linha.NumeroPaginas.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha.SaoIguais(9))
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
                linha.Codigo.PossuiErro.ShouldBeFalse();
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
                materiais.Any(a => a.Nome.SaoIguais(materialInserido)).ShouldBeTrue();

                var editoraInserida = linha.Editora.Conteudo;
                var editoras = ObterTodos<Editora>();
                editoras.Any(a => a.Nome.SaoIguais(editoraInserida)).ShouldBeTrue();

                var serieColecaoInserida = linha.SerieColecao.Conteudo;
                var serieColecaos = ObterTodos<SerieColecao>();
                serieColecaos.Any(a => a.Nome.SaoIguais(serieColecaoInserida)).ShouldBeTrue();

                var idiomaInserido = linha.Idioma.Conteudo;
                var idiomas = ObterTodos<Idioma>();
                idiomas.Any(a => a.Nome.SaoIguais(idiomaInserido)).ShouldBeTrue();

                var assuntosInseridos = linha.Assunto.Conteudo.FormatarTextoEmArray().ToArray()
                    .UnificarPipe().SplitPipe().Distinct();
                var assuntos = ObterTodos<Assunto>();
                foreach (var assunto in assuntosInseridos)
                    assuntos.Any(a => a.Nome.SaoIguais(assunto)).ShouldBeTrue();

                var creditoAutorInseridos = linha.Autor.Conteudo.FormatarTextoEmArray().ToArray()
                    .UnificarPipe().SplitPipe().Distinct();
                var creditosAutores = ObterTodos<CreditoAutor>();
                foreach (var creditoAutor in creditoAutorInseridos)
                    creditosAutores.Any(a => a.Nome.SaoIguais(creditoAutor)).ShouldBeTrue();

                creditoAutorInseridos = linha.CoAutor.Conteudo.FormatarTextoEmArray().ToArray()
                    .UnificarPipe().SplitPipe().Distinct();
                foreach (var creditoAutor in creditoAutorInseridos)
                    creditosAutores.Any(a => a.Nome.SaoIguais(creditoAutor)).ShouldBeTrue();
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
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoBibliograficoLinhas);

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
                acervos.Any(a=> a.Titulo.SaoIguais(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.SubTitulo.SaoIguais(linhasComSucesso.SubTitulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.SaoIguais(linhasComSucesso.Codigo.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosBibliograficos.Any(a=> a.MaterialId.SaoIguais(materiais.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Material.Conteudo)).Id)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.EditoraId.SaoIguais(editoras.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Editora.Conteudo)).Id)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.SerieColecaoId.SaoIguais(serieColecoes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.SerieColecao.Conteudo)).Id)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.IdiomaId.SaoIguais(idiomas.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Idioma.Conteudo)).Id)).ShouldBeTrue();
                
                //Campos livres
                acervosBibliograficos.Any(a=> a.Ano.SaoIguais(linhasComSucesso.Ano.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Edicao.SaoIguais(linhasComSucesso.Edicao.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.NumeroPagina.SaoIguais(linhasComSucesso.NumeroPaginas.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Altura.SaoIguais(linhasComSucesso.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Largura.SaoIguais(linhasComSucesso.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Volume.SaoIguais(linhasComSucesso.Volume.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.LocalizacaoCDD.SaoIguais(linhasComSucesso.LocalizacaoCDD.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.LocalizacaoPHA.SaoIguais(linhasComSucesso.LocalizacaoPHA.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.NotasGerais.SaoIguais(linhasComSucesso.NotasGerais.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Isbn.SaoIguais(linhasComSucesso.Isbn.Conteudo)).ShouldBeTrue();
                
                //Assuntos
                var assuntosAInserir = linhasComSucesso.Assunto.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var assunto in assuntosAInserir)
                    assuntos.Any(a=> a.Nome.SaoIguais(assunto)).ShouldBeTrue();
                
                foreach (var acervoBibliograficoAssunto in acervoBibliograficoAssuntos)
                    assuntos.Any(a=> a.Id.SaoIguais(acervoBibliograficoAssunto.AssuntoId)).ShouldBeTrue();
                
                //Autor
                var autorAInserir = linhasComSucesso.Autor.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var autor in autorAInserir)
                    creditoAutores.Any(a=> a.Nome.SaoIguais(autor)).ShouldBeTrue();
                
                foreach (var creditoAutor in acervoCreditoAutors)
                    creditoAutores.Any(a=> a.Id.SaoIguais(creditoAutor.CreditoAutorId)).ShouldBeTrue();
                
                //Coautor
                var coAutorAInserir = linhasComSucesso.CoAutor.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var coautor in coAutorAInserir)
                    creditoAutores.Any(a=> a.Nome.SaoIguais(coautor)).ShouldBeTrue();
                
                //TipoAutoria
                var tipoAutoriaAInserir = linhasComSucesso.TipoAutoria.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var tipoAutoria in tipoAutoriaAInserir)
                    acervoCreditoAutors.Any(a=> a.TipoAutoria.NaoEhNulo() && a.TipoAutoria.SaoIguais(tipoAutoria)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Geral - Com erros em 3 linhas")]
        public async Task Importacao_geral()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var acervoBibliograficoLinhas = GerarAcervoBibliograficoLinhaDTO().Generate(10);
           
            acervoBibliograficoLinhas[3].Largura.Conteudo = "ABC3512";
            acervoBibliograficoLinhas[5].Altura.Conteudo = "1212ABC";
            acervoBibliograficoLinhas[7].Codigo.Conteudo = acervoBibliograficoLinhas[0].Codigo.Conteudo;
            
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
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoBibliograficoLinhas);
            await servicoImportacaoArquivo.AtualizarImportacao(1, JsonConvert.SerializeObject(acervoBibliograficoLinhas), acervoBibliograficoLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);
            var retorno = await servicoImportacaoArquivo.ObterImportacaoPendente();

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
            
            //Retorno front
            retorno.Id.ShouldBe(1);
            retorno.Nome.ShouldNotBeEmpty();
            retorno.TipoAcervo.ShouldBe(TipoAcervo.Bibliografico);
            retorno.DataImportacao.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            
            foreach (var linhaInserida in acervoBibliograficoLinhas.Where(w=> !w.PossuiErros))
            {
                retorno.Sucesso.Any(a=> a.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Tombo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.NumeroLinha.SaoIguais(linhaInserida.NumeroLinha)).ShouldBeTrue();
            }
            
            foreach (var linhaInserida in acervoBibliograficoLinhas.Where(w=> w.PossuiErros))
            {
                retorno.Erros.Any(a=> a.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Tombo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.NumeroLinha.SaoIguais(linhaInserida.NumeroLinha)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoObjeto.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.SubTitulo.SaoIguais(linhaInserida.SubTitulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.MaterialId.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.CoAutores.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.EditoraId.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Ano.SaoIguais(linhaInserida.Ano.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Edicao.SaoIguais(linhaInserida.Edicao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.NumeroPagina.SaoIguais(linhaInserida.NumeroPaginas.Conteudo)).ShouldBeTrue();
               
                if (linhaInserida.Altura.PossuiErro)
                    retorno.Erros.Any(a=> a.RetornoObjeto.Altura.EhNulo()).ShouldBeTrue();
                else
                    retorno.Erros.Any(a=> a.RetornoObjeto.Altura.SaoIguais(linhaInserida.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();

                if (linhaInserida.Largura.PossuiErro)
                    retorno.Erros.Any(a=> a.RetornoObjeto.Largura.EhNulo()).ShouldBeTrue();
                else
                    retorno.Erros.Any(a=> a.RetornoObjeto.Largura.SaoIguais(linhaInserida.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoObjeto.SerieColecaoId.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Volume.SaoIguais(linhaInserida.Volume.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.IdiomaId.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.LocalizacaoCDD.SaoIguais(linhaInserida.LocalizacaoCDD.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.LocalizacaoPHA.SaoIguais(linhaInserida.LocalizacaoPHA.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.NotasGerais.SaoIguais(linhaInserida.NotasGerais.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Isbn.SaoIguais(linhaInserida.Isbn.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Codigo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoErro.Titulo.Conteudo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.SubTitulo.Conteudo.SaoIguais(linhaInserida.SubTitulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.MaterialId.Conteudo.SaoIguais(linhaInserida.Material.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CreditosAutoresIds.Conteudo.SaoIguais(linhaInserida.Autor.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CoAutores.Conteudo.SaoIguais(linhaInserida.CoAutor.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.EditoraId.Conteudo.SaoIguais(linhaInserida.Editora.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.AssuntosIds.Conteudo.SaoIguais(linhaInserida.Assunto.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Ano.Conteudo.SaoIguais(linhaInserida.Ano.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Edicao.Conteudo.SaoIguais(linhaInserida.Edicao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.NumeroPagina.Conteudo.SaoIguais(linhaInserida.NumeroPaginas.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Largura.Conteudo.SaoIguais(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Altura.Conteudo.SaoIguais(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.SerieColecaoId.Conteudo.SaoIguais(linhaInserida.SerieColecao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Volume.Conteudo.SaoIguais(linhaInserida.Volume.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.IdiomaId.Conteudo.SaoIguais(linhaInserida.Idioma.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.LocalizacaoCDD.Conteudo.SaoIguais(linhaInserida.LocalizacaoCDD.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.LocalizacaoPHA.Conteudo.SaoIguais(linhaInserida.LocalizacaoPHA.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.NotasGerais.Conteudo.SaoIguais(linhaInserida.NotasGerais.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Isbn.Conteudo.SaoIguais(linhaInserida.Isbn.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Codigo.Conteudo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
            }

            foreach (var linhasComSucesso in acervoBibliograficoLinhas.Where(w=> !w.PossuiErros))
            {
                //Acervo
                acervos.Any(a=> a.Titulo.SaoIguais(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.SubTitulo.SaoIguais(linhasComSucesso.SubTitulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.SaoIguais(linhasComSucesso.Codigo.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosBibliograficos.Any(a=> a.MaterialId.SaoIguais(materiais.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Material.Conteudo)).Id)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.EditoraId.SaoIguais(editoras.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Editora.Conteudo)).Id)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.SerieColecaoId.SaoIguais(serieColecoes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.SerieColecao.Conteudo)).Id)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.IdiomaId.SaoIguais(idiomas.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Idioma.Conteudo)).Id)).ShouldBeTrue();
                
                //Campos livres
                acervosBibliograficos.Any(a=> a.Ano.SaoIguais(linhasComSucesso.Ano.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Edicao.SaoIguais(linhasComSucesso.Edicao.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.NumeroPagina.SaoIguais(linhasComSucesso.NumeroPaginas.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Altura.SaoIguais(linhasComSucesso.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Largura.SaoIguais(linhasComSucesso.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Volume.SaoIguais(linhasComSucesso.Volume.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.LocalizacaoCDD.SaoIguais(linhasComSucesso.LocalizacaoCDD.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.LocalizacaoPHA.SaoIguais(linhasComSucesso.LocalizacaoPHA.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.NotasGerais.SaoIguais(linhasComSucesso.NotasGerais.Conteudo)).ShouldBeTrue();
                acervosBibliograficos.Any(a=> a.Isbn.SaoIguais(linhasComSucesso.Isbn.Conteudo)).ShouldBeTrue();
                
                //Assuntos
                var assuntosAInserir = linhasComSucesso.Assunto.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var assunto in assuntosAInserir)
                    assuntos.Any(a=> a.Nome.SaoIguais(assunto)).ShouldBeTrue();
                
                foreach (var acervoBibliograficoAssunto in acervoBibliograficoAssuntos)
                    assuntos.Any(a=> a.Id.SaoIguais(acervoBibliograficoAssunto.AssuntoId)).ShouldBeTrue();
                
                //Autor
                var autorAInserir = linhasComSucesso.Autor.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var autor in autorAInserir)
                    creditoAutores.Any(a=> a.Nome.SaoIguais(autor)).ShouldBeTrue();
                
                foreach (var creditoAutor in acervoCreditoAutors)
                    creditoAutores.Any(a=> a.Id.SaoIguais(creditoAutor.CreditoAutorId)).ShouldBeTrue();
                
                //Coautor
                var coAutorAInserir = linhasComSucesso.CoAutor.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var coautor in coAutorAInserir)
                    creditoAutores.Any(a=> a.Nome.SaoIguais(coautor)).ShouldBeTrue();
                
                //TipoAutoria
                var tipoAutoriaAInserir = linhasComSucesso.TipoAutoria.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var tipoAutoria in tipoAutoriaAInserir)
                    acervoCreditoAutors.Any(a=> a.TipoAutoria.NaoEhNulo() && a.TipoAutoria.SaoIguais(tipoAutoria)).ShouldBeTrue();
                
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
                retorno.Sucesso.Any(a=> a.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Tombo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.NumeroLinha.SaoIguais(linhaInserida.NumeroLinha)).ShouldBeTrue();
            }
            
            foreach (var linhaInserida in linhasInseridas.Where(w=> w.PossuiErros))
            {
                retorno.Erros.Any(a=> a.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Tombo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.NumeroLinha.SaoIguais(linhaInserida.NumeroLinha)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoObjeto.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.SubTitulo.SaoIguais(linhaInserida.SubTitulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.CoAutores.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Ano.SaoIguais(linhaInserida.Ano.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Edicao.SaoIguais(linhaInserida.Edicao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.NumeroPagina.SaoIguais(linhaInserida.NumeroPaginas.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Largura.SaoIguais(linhaInserida.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();

                if (linhaInserida.Altura.PossuiErro)
                    retorno.Erros.Any(a=> a.RetornoObjeto.Altura.EhNulo()).ShouldBeTrue();
                else
                    retorno.Erros.Any(a=> a.RetornoObjeto.Altura.SaoIguais(linhaInserida.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoObjeto.Volume.SaoIguais(linhaInserida.Volume.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.LocalizacaoCDD.SaoIguais(linhaInserida.LocalizacaoCDD.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.LocalizacaoPHA.SaoIguais(linhaInserida.LocalizacaoPHA.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.NotasGerais.SaoIguais(linhaInserida.NotasGerais.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Isbn.SaoIguais(linhaInserida.Isbn.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Codigo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoErro.Titulo.Conteudo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.SubTitulo.Conteudo.SaoIguais(linhaInserida.SubTitulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.MaterialId.Conteudo.SaoIguais(linhaInserida.Material.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CreditosAutoresIds.Conteudo.SaoIguais(linhaInserida.Autor.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CoAutores.Conteudo.SaoIguais(linhaInserida.CoAutor.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.EditoraId.Conteudo.SaoIguais(linhaInserida.Editora.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.AssuntosIds.Conteudo.SaoIguais(linhaInserida.Assunto.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Ano.Conteudo.SaoIguais(linhaInserida.Ano.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Edicao.Conteudo.SaoIguais(linhaInserida.Edicao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.NumeroPagina.Conteudo.SaoIguais(linhaInserida.NumeroPaginas.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Largura.Conteudo.SaoIguais(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Altura.Conteudo.SaoIguais(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.SerieColecaoId.Conteudo.SaoIguais(linhaInserida.SerieColecao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Volume.Conteudo.SaoIguais(linhaInserida.Volume.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.IdiomaId.Conteudo.SaoIguais(linhaInserida.Idioma.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.LocalizacaoCDD.Conteudo.SaoIguais(linhaInserida.LocalizacaoCDD.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.LocalizacaoPHA.Conteudo.SaoIguais(linhaInserida.LocalizacaoPHA.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.NotasGerais.Conteudo.SaoIguais(linhaInserida.NotasGerais.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Isbn.Conteudo.SaoIguais(linhaInserida.Isbn.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Codigo.Conteudo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
            }
        }

        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Validar materiais sem caracteres especiais")]
        public async Task Validar_materiais_sem_caracteres_especiais()
        {
            var servicoMaterial = GetServicoMaterial();

            await InserirNaBase(new Material() { Nome = "Português", Tipo = TipoMaterial.BIBLIOGRAFICO });
            await InserirNaBase(new Material() { Nome = "Inglês",Tipo = TipoMaterial.BIBLIOGRAFICO });
            await InserirNaBase(new Material() { Nome = "Geografia", Tipo = TipoMaterial.BIBLIOGRAFICO });
            await InserirNaBase(new Material() { Nome = "Matemática", Tipo = TipoMaterial.BIBLIOGRAFICO});
            
            (await servicoMaterial.ObterPorNomeETipo("PorTuGueS", TipoMaterial.BIBLIOGRAFICO)).ShouldBe(1);
            (await servicoMaterial.ObterPorNomeETipo("PORTUGUES", TipoMaterial.BIBLIOGRAFICO)).ShouldBe(1);
            (await servicoMaterial.ObterPorNomeETipo("PóRTÚGUEs", TipoMaterial.BIBLIOGRAFICO)).ShouldBe(1);
            (await servicoMaterial.ObterPorNomeETipo("PôRTUGüES", TipoMaterial.BIBLIOGRAFICO)).ShouldBe(1);
            
            (await servicoMaterial.ObterPorNomeETipo("InGlêS", TipoMaterial.BIBLIOGRAFICO)).ShouldBe(2);
            (await servicoMaterial.ObterPorNomeETipo("ÍNGLÉS", TipoMaterial.BIBLIOGRAFICO)).ShouldBe(2);
            (await servicoMaterial.ObterPorNomeETipo("ÎNGLÉS", TipoMaterial.BIBLIOGRAFICO)).ShouldBe(2);
            (await servicoMaterial.ObterPorNomeETipo("ÎNGLÈS", TipoMaterial.BIBLIOGRAFICO)).ShouldBe(2);
            
            (await servicoMaterial.ObterPorNomeETipo("GEOGRAFíA", TipoMaterial.BIBLIOGRAFICO)).ShouldBe(3);
            (await servicoMaterial.ObterPorNomeETipo("GEôGRáFíA", TipoMaterial.BIBLIOGRAFICO)).ShouldBe(3);
            (await servicoMaterial.ObterPorNomeETipo("GEÓGRÂFíã", TipoMaterial.BIBLIOGRAFICO)).ShouldBe(3);
            (await servicoMaterial.ObterPorNomeETipo("GÈÓGRÀFÍÂ", TipoMaterial.BIBLIOGRAFICO)).ShouldBe(3);
            
            (await servicoMaterial.ObterPorNomeETipo("MAtemáticA", TipoMaterial.BIBLIOGRAFICO)).ShouldBe(4);
            (await servicoMaterial.ObterPorNomeETipo("MÁTeMÀTIca", TipoMaterial.BIBLIOGRAFICO)).ShouldBe(4);
            (await servicoMaterial.ObterPorNomeETipo("MÃtêmàtíCÂ", TipoMaterial.BIBLIOGRAFICO)).ShouldBe(4);
            (await servicoMaterial.ObterPorNomeETipo("mateMâTícà", TipoMaterial.BIBLIOGRAFICO)).ShouldBe(4);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Validar editoras sem caracteres especiais")]
        public async Task Validar_editoras_sem_caracteres_especiais()
        {
            var servicoEditora = GetServicoEditora();

            await InserirNaBase(new Editora() { Nome = "Santuário", CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789});
            await InserirNaBase(new Editora() { Nome = "Sextânte", CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789 });
            await InserirNaBase(new Editora() { Nome = "Ágape", CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789 });
            await InserirNaBase(new Editora() { Nome = "Pressí", CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789 });
            
            (await servicoEditora.ObterPorNome("Santuarió")).ShouldBe(1);
            (await servicoEditora.ObterPorNome("SANTuàRiO")).ShouldBe(1);
            (await servicoEditora.ObterPorNome("SÁntúaRìo")).ShouldBe(1);
            (await servicoEditora.ObterPorNome("SÀnTÚaRió")).ShouldBe(1);
            
            (await servicoEditora.ObterPorNome("SExTanTé")).ShouldBe(2);
            (await servicoEditora.ObterPorNome("SéxTaNtê")).ShouldBe(2);
            (await servicoEditora.ObterPorNome("SEXTAntè")).ShouldBe(2);
            (await servicoEditora.ObterPorNome("sextantÊ")).ShouldBe(2);
            
            (await servicoEditora.ObterPorNome("AGAPÉ")).ShouldBe(3);
            (await servicoEditora.ObterPorNome("aGapÊ")).ShouldBe(3);
            (await servicoEditora.ObterPorNome("ÂgaPÈ")).ShouldBe(3);
            (await servicoEditora.ObterPorNome("ÂgÃpÊ")).ShouldBe(3);
            
            (await servicoEditora.ObterPorNome("PrÉssÌ")).ShouldBe(4);
            (await servicoEditora.ObterPorNome("prÈssÍ")).ShouldBe(4);
            (await servicoEditora.ObterPorNome("pRÊssi")).ShouldBe(4);
            (await servicoEditora.ObterPorNome("pressÍ")).ShouldBe(4);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Validar séries/coleções sem caracteres especiais")]
        public async Task Validar_serie_colecoes_sem_caracteres_especiais()
        {
            var servicoSerieColecao = GetServicoSerieColecao();

            await InserirNaBase(new SerieColecao() { Nome = "Santuário", CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789});
            await InserirNaBase(new SerieColecao() { Nome = "Sextânte", CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789 });
            await InserirNaBase(new SerieColecao() { Nome = "Ágape", CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789 });
            await InserirNaBase(new SerieColecao() { Nome = "Pressí", CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789 });
            
            (await servicoSerieColecao.ObterPorNome("Santuarió")).ShouldBe(1);
            (await servicoSerieColecao.ObterPorNome("SANTuàRiO")).ShouldBe(1);
            (await servicoSerieColecao.ObterPorNome("SÁntúaRìo")).ShouldBe(1);
            (await servicoSerieColecao.ObterPorNome("SÀnTÚaRió")).ShouldBe(1);
            
            (await servicoSerieColecao.ObterPorNome("SExTanTé")).ShouldBe(2);
            (await servicoSerieColecao.ObterPorNome("SéxTaNtê")).ShouldBe(2);
            (await servicoSerieColecao.ObterPorNome("SEXTAntè")).ShouldBe(2);
            (await servicoSerieColecao.ObterPorNome("sextantÊ")).ShouldBe(2);
            
            (await servicoSerieColecao.ObterPorNome("AGAPÉ")).ShouldBe(3);
            (await servicoSerieColecao.ObterPorNome("aGapÊ")).ShouldBe(3);
            (await servicoSerieColecao.ObterPorNome("ÂgaPÈ")).ShouldBe(3);
            (await servicoSerieColecao.ObterPorNome("ÂgÃpÊ")).ShouldBe(3);
            
            (await servicoSerieColecao.ObterPorNome("PrÉssÌ")).ShouldBe(4);
            (await servicoSerieColecao.ObterPorNome("prÈssÍ")).ShouldBe(4);
            (await servicoSerieColecao.ObterPorNome("pRÊssi")).ShouldBe(4);
            (await servicoSerieColecao.ObterPorNome("pressÍ")).ShouldBe(4);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Validar idiomas sem caracteres especiais")]
        public async Task Validar_idiomas_sem_caracteres_especiais()
        {
            var servicoIdioma = GetServicoIdioma();

            await InserirNaBase(new Idioma() { Nome = "Santuário"});
            await InserirNaBase(new Idioma() { Nome = "Sextânte"});
            await InserirNaBase(new Idioma() { Nome = "Ágape" });
            await InserirNaBase(new Idioma() { Nome = "Pressí" });
            
            (await servicoIdioma.ObterPorNome("Santuarió")).ShouldBe(1);
            (await servicoIdioma.ObterPorNome("SANTuàRiO")).ShouldBe(1);
            (await servicoIdioma.ObterPorNome("SÁntúaRìo")).ShouldBe(1);
            (await servicoIdioma.ObterPorNome("SÀnTÚaRió")).ShouldBe(1);
            
            (await servicoIdioma.ObterPorNome("SExTanTé")).ShouldBe(2);
            (await servicoIdioma.ObterPorNome("SéxTaNtê")).ShouldBe(2);
            (await servicoIdioma.ObterPorNome("SEXTAntè")).ShouldBe(2);
            (await servicoIdioma.ObterPorNome("sextantÊ")).ShouldBe(2);
            
            (await servicoIdioma.ObterPorNome("AGAPÉ")).ShouldBe(3);
            (await servicoIdioma.ObterPorNome("aGapÊ")).ShouldBe(3);
            (await servicoIdioma.ObterPorNome("ÂgaPÈ")).ShouldBe(3);
            (await servicoIdioma.ObterPorNome("ÂgÃpÊ")).ShouldBe(3);
            
            (await servicoIdioma.ObterPorNome("PrÉssÌ")).ShouldBe(4);
            (await servicoIdioma.ObterPorNome("prÈssÍ")).ShouldBe(4);
            (await servicoIdioma.ObterPorNome("pRÊssi")).ShouldBe(4);
            (await servicoIdioma.ObterPorNome("pressÍ")).ShouldBe(4);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Validar assuntos sem caracteres especiais")]
        public async Task Validar_assuntos_sem_caracteres_especiais()
        {
            var servicoAssunto = GetServicoAssunto();

            await InserirNaBase(new Assunto() { Nome = "Santuário", CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789});
            await InserirNaBase(new Assunto() { Nome = "Sextânte", CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789});
            await InserirNaBase(new Assunto() { Nome = "Ágape" , CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789});
            await InserirNaBase(new Assunto() { Nome = "Pressí" , CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789});
            
            (await servicoAssunto.ObterPorNome("Santuarió")).ShouldBe(1);
            (await servicoAssunto.ObterPorNome("SANTuàRiO")).ShouldBe(1);
            (await servicoAssunto.ObterPorNome("SÁntúaRìo")).ShouldBe(1);
            (await servicoAssunto.ObterPorNome("SÀnTÚaRió")).ShouldBe(1);
            
            (await servicoAssunto.ObterPorNome("SExTanTé")).ShouldBe(2);
            (await servicoAssunto.ObterPorNome("SéxTaNtê")).ShouldBe(2);
            (await servicoAssunto.ObterPorNome("SEXTAntè")).ShouldBe(2);
            (await servicoAssunto.ObterPorNome("sextantÊ")).ShouldBe(2);
            
            (await servicoAssunto.ObterPorNome("AGAPÉ")).ShouldBe(3);
            (await servicoAssunto.ObterPorNome("aGapÊ")).ShouldBe(3);
            (await servicoAssunto.ObterPorNome("ÂgaPÈ")).ShouldBe(3);
            (await servicoAssunto.ObterPorNome("ÂgÃpÊ")).ShouldBe(3);
            
            (await servicoAssunto.ObterPorNome("PrÉssÌ")).ShouldBe(4);
            (await servicoAssunto.ObterPorNome("prÈssÍ")).ShouldBe(4);
            (await servicoAssunto.ObterPorNome("pRÊssi")).ShouldBe(4);
            (await servicoAssunto.ObterPorNome("pressÍ")).ShouldBe(4);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Validar autores/coautores sem caracteres especiais")]
        public async Task Validar_autores_coautores_sem_caracteres_especiais()
        {
            var servicoCreditoAutor = GetServicoCreditoAutor();

            await InserirNaBase(new CreditoAutor() { Nome = "Santuário", Tipo = TipoCreditoAutoria.Autoria, CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789});
            await InserirNaBase(new CreditoAutor() { Nome = "Sextânte", Tipo = TipoCreditoAutoria.Autoria, CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789});
            await InserirNaBase(new CreditoAutor() { Nome = "Ágape", Tipo = TipoCreditoAutoria.Autoria , CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789});
            await InserirNaBase(new CreditoAutor() { Nome = "Pressí", Tipo = TipoCreditoAutoria.Autoria , CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789});
            
            (await servicoCreditoAutor.ObterPorNomeETipo("Santuarió",TipoCreditoAutoria.Autoria)).ShouldBe(1);
            (await servicoCreditoAutor.ObterPorNomeETipo("SANTuàRiO",TipoCreditoAutoria.Autoria)).ShouldBe(1);
            (await servicoCreditoAutor.ObterPorNomeETipo("SÁntúaRìo",TipoCreditoAutoria.Autoria)).ShouldBe(1);
            (await servicoCreditoAutor.ObterPorNomeETipo("SÀnTÚaRió",TipoCreditoAutoria.Autoria)).ShouldBe(1);
            
            (await servicoCreditoAutor.ObterPorNomeETipo("SéxTaNtê",TipoCreditoAutoria.Autoria)).ShouldBe(2);
            (await servicoCreditoAutor.ObterPorNomeETipo("SEXTAntè",TipoCreditoAutoria.Autoria)).ShouldBe(2);
            (await servicoCreditoAutor.ObterPorNomeETipo("SExTanTé",TipoCreditoAutoria.Autoria)).ShouldBe(2);
            (await servicoCreditoAutor.ObterPorNomeETipo("sextantÊ",TipoCreditoAutoria.Autoria)).ShouldBe(2);
            
            (await servicoCreditoAutor.ObterPorNomeETipo("AGAPÉ",TipoCreditoAutoria.Autoria)).ShouldBe(3);
            (await servicoCreditoAutor.ObterPorNomeETipo("aGapÊ",TipoCreditoAutoria.Autoria)).ShouldBe(3);
            (await servicoCreditoAutor.ObterPorNomeETipo("ÂgaPÈ",TipoCreditoAutoria.Autoria)).ShouldBe(3);
            (await servicoCreditoAutor.ObterPorNomeETipo("ÂgÃpÊ",TipoCreditoAutoria.Autoria)).ShouldBe(3);
            
            (await servicoCreditoAutor.ObterPorNomeETipo("PrÉssÌ",TipoCreditoAutoria.Autoria)).ShouldBe(4);
            (await servicoCreditoAutor.ObterPorNomeETipo("prÈssÍ",TipoCreditoAutoria.Autoria)).ShouldBe(4);
            (await servicoCreditoAutor.ObterPorNomeETipo("pRÊssi",TipoCreditoAutoria.Autoria)).ShouldBe(4);
            (await servicoCreditoAutor.ObterPorNomeETipo("pressÍ",TipoCreditoAutoria.Autoria)).ShouldBe(4);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Deve permitir remover linha do arquivo")]
        public async Task Deve_permitir_remover_a_linha_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var linhasInseridas = GerarAcervoBibliograficoLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Bibliografico,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            var retorno = await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = 5});
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault(f => f.Id.SaoIguais(1));
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoBibliograficoLinhaDTO>>(arquivo.Conteudo);
            conteudo.Any(a=> a.NumeroLinha.SaoIguais(5)).ShouldBeFalse();
            conteudo.Count().ShouldBe(9);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Não deve permitir remover linha do arquivo")]
        public async Task Nao_deve_permitir_remover_a_linha_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var linhasInseridas = GerarAcervoBibliograficoLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Bibliografico,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = 15}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Não deve permitir remover todas as linhas do arquivo")]
        public async Task Nao_deve_permitir_remover_todas_as_linhas_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var linhasInseridas = GerarAcervoBibliograficoLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Bibliografico,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            for (int i = 1; i < 10; i++)
                (await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = i})).ShouldBe(true);
                
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoBibliograficoLinhaDTO>>(arquivo.Conteudo);
            conteudo.Count().ShouldBe(1);
            
            await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = 10}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Deve permitir atualizar uma linha do arquivo para sucesso e outra fica com erro")]
        public async Task Deve_permitir_atualizar_uma_linha_do_arquivo_para_sucesso_e_outra_fica_com_erro()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var linhasInseridas = GerarAcervoBibliograficoLinhaDTO().Generate(10);
            linhasInseridas[3].PossuiErros = true;
            linhasInseridas[3].Altura.PossuiErro = true;
            linhasInseridas[3].Altura.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.LARGURA);
            linhasInseridas[3].Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.LARGURA); //Mensagem geral
            
            linhasInseridas[9].PossuiErros = true;
            linhasInseridas[9].Material.PossuiErro = true;
            linhasInseridas[9].Material.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_NAO_PREENCHIDO, Dominio.Constantes.Constantes.MATERIAL);
            linhasInseridas[9].Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_NAO_PREENCHIDO, Dominio.Constantes.Constantes.SUPORTE); //Mensagem geral

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Bibliografico,
                Status = ImportacaoStatus.Erros,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.AtualizarLinhaParaSucesso(1, new LinhaDTO(){NumeroLinha = 4});
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoBibliograficoLinhaDTO>>(arquivo.Conteudo);
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).PossuiErros.ShouldBeFalse();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).Mensagem.ShouldBeEmpty();
            
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(10)).PossuiErros.ShouldBeTrue();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(10)).Mensagem.ShouldNotBeEmpty();
            
            arquivo.Status.ShouldBe(ImportacaoStatus.Erros);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Bibliográfico - Deve permitir atualizar linha do arquivo para sucesso")]
        public async Task Deve_permitir_atualizar_linha_do_arquivo_para_sucesso()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoBibliografico();

            var linhasInseridas = GerarAcervoBibliograficoLinhaDTO().Generate(10);
            linhasInseridas[3].PossuiErros = true;
            linhasInseridas[3].Altura.PossuiErro = true;
            linhasInseridas[3].Altura.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.ALTURA);
            linhasInseridas[3].Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.ALTURA); //Mensagem geral
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Bibliografico,
                Status = ImportacaoStatus.Erros,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.AtualizarLinhaParaSucesso(1, new LinhaDTO(){NumeroLinha = 4});
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoBibliograficoLinhaDTO>>(arquivo.Conteudo);
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).PossuiErros.ShouldBeFalse();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).Mensagem.ShouldBeEmpty();
            
            conteudo.Any(a=> a.PossuiErros).ShouldBeFalse();
            conteudo.Any(a=> !a.PossuiErros).ShouldBeTrue();
            
            arquivo.Status.ShouldBe(ImportacaoStatus.Sucesso);
        }
    }
}