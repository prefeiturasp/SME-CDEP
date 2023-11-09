﻿using Bogus.Extensions.Brazil;
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
    public class Ao_fazer_manutencao_importacao_arquivo_acervo_documental : TesteBase
    {
        public Ao_fazer_manutencao_importacao_arquivo_acervo_documental(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Importação Arquivo Acervo Documental - ValidarPreenchimentoValorFormatoQtdeCaracteres - Com erros: Ano, Idioma, Largura, Número de Páginas e Volume")]
        public async Task Validar_preenchimento_valor_formato_qtde_caracteres()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoDocumental();

            var acervoDocumentalLinhas = GerarAcervoDocumentalLinhaDTO().Generate(10);

            acervoDocumentalLinhas[2].Ano.Conteudo = faker.Lorem.Paragraph();
            acervoDocumentalLinhas[4].Idioma.Conteudo = string.Empty;
            acervoDocumentalLinhas[5].Largura.Conteudo = faker.Lorem.Paragraph();
            acervoDocumentalLinhas[7].NumeroPaginas.Conteudo = faker.Lorem.Paragraph();
            acervoDocumentalLinhas[8].Volume.Conteudo = faker.Lorem.Paragraph();
            var linhasComErros = new[] { 3, 5, 6, 8, 9 };
            
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoDocumentalLinhas);

            foreach (var linha in acervoDocumentalLinhas)
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
                    linha.Idioma.PossuiErro.ShouldBeTrue();
                    linha.Idioma.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Idioma.PossuiErro.ShouldBeFalse();
                    linha.Idioma.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha.SaoIguais(6))
                {
                    linha.Largura.PossuiErro.ShouldBeTrue();
                    linha.Largura.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Largura.PossuiErro.ShouldBeFalse();
                    linha.Largura.Mensagem.ShouldBeEmpty();
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
                
                linha.Titulo.PossuiErro.ShouldBeFalse();
                linha.CodigoAntigo.PossuiErro.ShouldBeFalse();
                linha.CodigoNovo.PossuiErro.ShouldBeFalse();
                linha.Material.PossuiErro.ShouldBeFalse();
                linha.Autor.PossuiErro.ShouldBeFalse();
                linha.Descricao.PossuiErro.ShouldBeFalse();
                linha.TipoAnexo.PossuiErro.ShouldBeFalse();
                linha.Altura.PossuiErro.ShouldBeFalse();
                linha.TamanhoArquivo.PossuiErro.ShouldBeFalse();
                linha.AcessoDocumento.PossuiErro.ShouldBeFalse();
                linha.Localizacao.PossuiErro.ShouldBeFalse();
                linha.CopiaDigital.PossuiErro.ShouldBeFalse();
                linha.EstadoConservacao.PossuiErro.ShouldBeFalse();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Documental - ValidacaoObterOuInserirDominios")]
        public async Task Validacao_obter_ou_inserir_dominios()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoDocumental();
        
            var acervoDocumentalLinhas = GerarAcervoDocumentalLinhaDTO().Generate(10);
           
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoDocumentalLinhas);
        
            foreach (var linha in acervoDocumentalLinhas)
            {
                var materialInserido = linha.Material.Conteudo;
                var materiais = ObterTodos<Material>();
                materiais.Any(a => a.Nome.SaoIguais(materialInserido)).ShouldBeTrue();
        
                var idiomaInserido = linha.Idioma.Conteudo;
                var idiomas = ObterTodos<Idioma>();
                idiomas.Any(a => a.Nome.SaoIguais(idiomaInserido)).ShouldBeTrue();
                
                var creditoAutorInseridos = linha.Autor.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                var creditosAutores = ObterTodos<CreditoAutor>();
                foreach (var creditoAutor in creditoAutorInseridos)
                    creditosAutores.Any(a => a.Nome.SaoIguais(creditoAutor)).ShouldBeTrue();
                
                var acessoDocumentosInseridos = linha.AcessoDocumento.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                var acessoDocumentos = ObterTodos<AcessoDocumento>();
                foreach (var acessoDocumentoInserido in acessoDocumentosInseridos)
                    acessoDocumentos.Any(a => a.Nome.SaoIguais(acessoDocumentoInserido)).ShouldBeTrue();
        
                var conservacoesInseridas = linha.EstadoConservacao.Conteudo;
                var conservacoes = ObterTodos<Conservacao>();
                conservacoes.Any(a => a.Nome.SaoIguais(conservacoesInseridas)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Documental - PersistenciaAcervo")]
        public async Task Persistencia_acervo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoDocumental();
        
            var acervoDocumentalLinhas = GerarAcervoDocumentalLinhaDTO().Generate(10);
        
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.DocumentacaoHistorica,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoDocumentalLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoDocumentalLinhas);
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoDocumentalLinhas);
        
            var acervos = ObterTodos<Acervo>();
            var acervosDocumentais = ObterTodos<AcervoDocumental>();
            var materiais = ObterTodos<Material>();
            var idiomas = ObterTodos<Idioma>();
            var acervoDocumentalAcessoDocumentos = ObterTodos<AcervoDocumentalAcessoDocumento>();
            var acessosDocumentos = ObterTodos<AcessoDocumento>();
            var acervoCreditoAutors = ObterTodos<AcervoCreditoAutor>();
            var creditoAutores = ObterTodos<CreditoAutor>();
            var conservacoes = ObterTodos<Conservacao>();
            
            //Acervos inseridos
            acervos.ShouldNotBeNull();
            acervos.Count().ShouldBe(10);
            
            //Acervos Documentais inseridos
            acervosDocumentais.ShouldNotBeNull();
            acervosDocumentais.Count().ShouldBe(10);
            
            //Linhas com erros
            acervoDocumentalLinhas.Count(w=> !w.PossuiErros).ShouldBe(10);
            acervoDocumentalLinhas.Count(w=> w.PossuiErros).ShouldBe(0);
        
            foreach (var linhasComSucesso in acervoDocumentalLinhas.Where(w=> !w.PossuiErros))
            {
                //Acervo
                acervos.Any(a=> a.Titulo.SaoIguais(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.SaoIguais(linhasComSucesso.CodigoAntigo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.CodigoNovo.SaoIguais(linhasComSucesso.CodigoNovo.Conteudo)).ShouldBeTrue();  
                acervos.Any(a=> a.Descricao.SaoIguais(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosDocumentais.Any(a=> a.MaterialId.SaoIguais(materiais.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Material.Conteudo)).Id)).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.IdiomaId.SaoIguais(idiomas.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Idioma.Conteudo)).Id)).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.ConservacaoId.SaoIguais(conservacoes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.EstadoConservacao.Conteudo)).Id)).ShouldBeTrue();
                
                
                //Campos livres
                acervosDocumentais.Any(a=> a.Ano.SaoIguais(linhasComSucesso.Ano.Conteudo)).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.NumeroPagina.SaoIguais(linhasComSucesso.NumeroPaginas.Conteudo)).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.Volume.SaoIguais(linhasComSucesso.Volume.Conteudo)).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.TipoAnexo.SaoIguais(linhasComSucesso.TipoAnexo.Conteudo)).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.Altura.SaoIguais(linhasComSucesso.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.Largura.SaoIguais(linhasComSucesso.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.TamanhoArquivo.SaoIguais(linhasComSucesso.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.Localizacao.SaoIguais(linhasComSucesso.Localizacao.Conteudo)).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.CopiaDigital.SaoIguais(linhasComSucesso.CopiaDigital.Conteudo.EhOpcaoSim())).ShouldBeTrue();
                
                //Autor
                var autorAInserir = linhasComSucesso.Autor.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var autor in autorAInserir)
                    creditoAutores.Any(a=> a.Nome.SaoIguais(autor)).ShouldBeTrue();
                
                foreach (var creditoAutor in acervoCreditoAutors)
                    creditoAutores.Any(a=> a.Id.SaoIguais(creditoAutor.CreditoAutorId)).ShouldBeTrue();
                
                //Acesso Documento
                var acessoDocumentoAInserir = linhasComSucesso.AcessoDocumento.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var acessoDocumento in acessoDocumentoAInserir)
                    acessosDocumentos.Any(a=> a.Nome.SaoIguais(acessoDocumento)).ShouldBeTrue();
                
                //Acesso Documento Acervo Documental
                foreach (var acervoDocumentalAcessoDocumento in acervoDocumentalAcessoDocumentos)
                    acessosDocumentos.Any(a=> a.Id.SaoIguais(acervoDocumentalAcessoDocumento.AcessoDocumentoId)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Documental - Geral - Com erros em 3 linhas")]
        public async Task Importacao_geral()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoDocumental();
        
            var acervoDocumentalLinhas = GerarAcervoDocumentalLinhaDTO().Generate(10);
           
            acervoDocumentalLinhas[3].Largura.Conteudo = "ABC3512";
            acervoDocumentalLinhas[5].Altura.Conteudo = "1212ABC";
            acervoDocumentalLinhas[7].CodigoAntigo.Conteudo = acervoDocumentalLinhas[0].CodigoAntigo.Conteudo;
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.DocumentacaoHistorica,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoDocumentalLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoDocumentalLinhas);
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoDocumentalLinhas );
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoDocumentalLinhas);
        
            var acervos = ObterTodos<Acervo>();
            var acervosDocumentais = ObterTodos<AcervoDocumental>();
            var materiais = ObterTodos<Material>();
            var idiomas = ObterTodos<Idioma>();
            var acervoDocumentalAcessoDocumentos = ObterTodos<AcervoDocumentalAcessoDocumento>();
            var acessosDocumentos = ObterTodos<AcessoDocumento>();
            var acervoCreditoAutors = ObterTodos<AcervoCreditoAutor>();
            var creditoAutores = ObterTodos<CreditoAutor>();
            var conservacoes = ObterTodos<Conservacao>();
            
            //Acervos inseridos
            acervos.ShouldNotBeNull();
            acervos.Count().ShouldBe(7);
            
            //Acervos Documentals inseridos
            acervosDocumentais.ShouldNotBeNull();
            acervosDocumentais.Count().ShouldBe(7);
            
            //Linhas com erros
            acervoDocumentalLinhas.Count(w=> !w.PossuiErros).ShouldBe(7);
            acervoDocumentalLinhas.Count(w=> w.PossuiErros).ShouldBe(3);
        
            foreach (var linhasComSucesso in acervoDocumentalLinhas.Where(w=> !w.PossuiErros))
            {
                //Acervo
                acervos.Any(a=> a.Titulo.SaoIguais(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.SaoIguais(linhasComSucesso.CodigoAntigo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.CodigoNovo.SaoIguais(linhasComSucesso.CodigoNovo.Conteudo)).ShouldBeTrue();  
                acervos.Any(a=> a.Descricao.SaoIguais(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosDocumentais.Any(a=> a.MaterialId.SaoIguais(materiais.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Material.Conteudo)).Id)).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.IdiomaId.SaoIguais(idiomas.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Idioma.Conteudo)).Id)).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.ConservacaoId.SaoIguais(conservacoes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.EstadoConservacao.Conteudo)).Id)).ShouldBeTrue();
                
                
                //Campos livres
                acervosDocumentais.Any(a=> a.Ano.SaoIguais(linhasComSucesso.Ano.Conteudo)).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.NumeroPagina.SaoIguais(linhasComSucesso.NumeroPaginas.Conteudo)).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.Volume.SaoIguais(linhasComSucesso.Volume.Conteudo)).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.TipoAnexo.SaoIguais(linhasComSucesso.TipoAnexo.Conteudo)).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.Altura.SaoIguais(linhasComSucesso.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.Largura.SaoIguais(linhasComSucesso.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.TamanhoArquivo.SaoIguais(linhasComSucesso.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.Localizacao.SaoIguais(linhasComSucesso.Localizacao.Conteudo)).ShouldBeTrue();
                acervosDocumentais.Any(a=> a.CopiaDigital.SaoIguais(linhasComSucesso.CopiaDigital.Conteudo.EhOpcaoSim())).ShouldBeTrue();
                
                //Autor
                var autorAInserir = linhasComSucesso.Autor.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var autor in autorAInserir)
                    creditoAutores.Any(a=> a.Nome.SaoIguais(autor)).ShouldBeTrue();
                
                foreach (var creditoAutor in acervoCreditoAutors)
                    creditoAutores.Any(a=> a.Id.SaoIguais(creditoAutor.CreditoAutorId)).ShouldBeTrue();
                
                //Acesso Documento
                var acessoDocumentoAInserir = linhasComSucesso.AcessoDocumento.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var acessoDocumento in acessoDocumentoAInserir)
                    acessosDocumentos.Any(a=> a.Nome.SaoIguais(acessoDocumento)).ShouldBeTrue();
                
                //Acesso Documento Acervo Documental
                foreach (var acervoDocumentalAcessoDocumento in acervoDocumentalAcessoDocumentos)
                    acessosDocumentos.Any(a=> a.Id.SaoIguais(acervoDocumentalAcessoDocumento.AcessoDocumentoId)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Documental - Obter importação pendente com Erros")]
        public async Task Obter_importacao_pendente_com_erros()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoDocumental();
        
            var linhasInseridas = GerarAcervoDocumentalLinhaDTO().Generate(10);
        
            linhasInseridas[3].PossuiErros = true;
            linhasInseridas[3].Volume.PossuiErro = true;
            linhasInseridas[3].Volume.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.VOLUME);
            
            linhasInseridas[9].PossuiErros = true;
            linhasInseridas[9].Altura.PossuiErro = true;
            linhasInseridas[9].Altura.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.ALTURA);
           
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.DocumentacaoHistorica,
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
                retorno.Sucesso.Any(a=> a.Titulo.Conteudo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.CodigoAntigo.Conteudo.SaoIguais(linhaInserida.CodigoAntigo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.CodigoNovo.Conteudo.SaoIguais(linhaInserida.CodigoNovo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Material.Conteudo.SaoIguais(linhaInserida.Material.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Idioma.Conteudo.SaoIguais(linhaInserida.Idioma.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Autor.Conteudo.SaoIguais(linhaInserida.Autor.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Ano.Conteudo.SaoIguais(linhaInserida.Ano.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.NumeroPaginas.Conteudo.SaoIguais(linhaInserida.NumeroPaginas.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Volume.Conteudo.SaoIguais(linhaInserida.Volume.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Descricao.Conteudo.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.TipoAnexo.Conteudo.SaoIguais(linhaInserida.TipoAnexo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Largura.Conteudo.SaoIguais(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Altura.Conteudo.SaoIguais(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.TamanhoArquivo.Conteudo.SaoIguais(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.AcessoDocumento.Conteudo.SaoIguais(linhaInserida.AcessoDocumento.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Localizacao.Conteudo.SaoIguais(linhaInserida.Localizacao.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.CopiaDigital.Conteudo.SaoIguais(linhaInserida.CopiaDigital.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.EstadoConservacao.Conteudo.SaoIguais(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
            }
            
            foreach (var linhaInserida in linhasInseridas.Where(w=> w.PossuiErros))
            {
                retorno.Erros.Any(a=> a.Titulo.Conteudo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.CodigoAntigo.Conteudo.SaoIguais(linhaInserida.CodigoAntigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.CodigoNovo.Conteudo.SaoIguais(linhaInserida.CodigoNovo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Material.Conteudo.SaoIguais(linhaInserida.Material.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Idioma.Conteudo.SaoIguais(linhaInserida.Idioma.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Autor.Conteudo.SaoIguais(linhaInserida.Autor.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Ano.Conteudo.SaoIguais(linhaInserida.Ano.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.NumeroPaginas.Conteudo.SaoIguais(linhaInserida.NumeroPaginas.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Volume.Conteudo.SaoIguais(linhaInserida.Volume.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Descricao.Conteudo.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.TipoAnexo.Conteudo.SaoIguais(linhaInserida.TipoAnexo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Largura.Conteudo.SaoIguais(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Altura.Conteudo.SaoIguais(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.TamanhoArquivo.Conteudo.SaoIguais(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.AcessoDocumento.Conteudo.SaoIguais(linhaInserida.AcessoDocumento.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Localizacao.Conteudo.SaoIguais(linhaInserida.Localizacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.CopiaDigital.Conteudo.SaoIguais(linhaInserida.CopiaDigital.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.EstadoConservacao.Conteudo.SaoIguais(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Documental - Validar Acesso Documento sem caracteres especiais")]
        public async Task Validar_acesso_documento_sem_caracteres_especiais()
        {
            var servicoAcessoDocumento = GetServicoAcessoDocumento();
        
            await InserirNaBase(new AcessoDocumento() { Nome = "Português" });
            await InserirNaBase(new AcessoDocumento() { Nome = "Inglês" });
            await InserirNaBase(new AcessoDocumento() { Nome = "Geografia" });
            await InserirNaBase(new AcessoDocumento() { Nome = "Matemática"});
            
            (await servicoAcessoDocumento.ObterPorNome("PorTuGueS")).ShouldBe(1);
            (await servicoAcessoDocumento.ObterPorNome("PORTUGUES")).ShouldBe(1);
            (await servicoAcessoDocumento.ObterPorNome("PóRTÚGUEs")).ShouldBe(1);
            (await servicoAcessoDocumento.ObterPorNome("PôRTUGüES")).ShouldBe(1);
            
            (await servicoAcessoDocumento.ObterPorNome("InGlêS")).ShouldBe(2);
            (await servicoAcessoDocumento.ObterPorNome("ÍNGLÉS")).ShouldBe(2);
            (await servicoAcessoDocumento.ObterPorNome("ÎNGLÉS")).ShouldBe(2);
            (await servicoAcessoDocumento.ObterPorNome("ÎNGLÈS")).ShouldBe(2);
            
            (await servicoAcessoDocumento.ObterPorNome("GEOGRAFíA")).ShouldBe(3);
            (await servicoAcessoDocumento.ObterPorNome("GEôGRáFíA")).ShouldBe(3);
            (await servicoAcessoDocumento.ObterPorNome("GEÓGRÂFíã")).ShouldBe(3);
            (await servicoAcessoDocumento.ObterPorNome("GÈÓGRÀFÍÂ")).ShouldBe(3);
            
            (await servicoAcessoDocumento.ObterPorNome("MAtemáticA")).ShouldBe(4);
            (await servicoAcessoDocumento.ObterPorNome("MÁTeMÀTIca")).ShouldBe(4);
            (await servicoAcessoDocumento.ObterPorNome("MÃtêmàtíCÂ")).ShouldBe(4);
            (await servicoAcessoDocumento.ObterPorNome("mateMâTícà")).ShouldBe(4);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Documental - Validar estado de conservação sem caracteres especiais")]
        public async Task Validar_estado_de_conservacao_sem_caracteres_especiais()
        {
            var servicoConservacao = GetServicoConservacao();
        
            await InserirNaBase(new Conservacao() { Nome = "Santuário"});
            await InserirNaBase(new Conservacao() { Nome = "Sextânte",});
            await InserirNaBase(new Conservacao() { Nome = "Ágape"});
            await InserirNaBase(new Conservacao() { Nome = "Pressí"});
            
            (await servicoConservacao.ObterPorNome("Santuarió")).ShouldBe(1);
            (await servicoConservacao.ObterPorNome("SANTuàRiO")).ShouldBe(1);
            (await servicoConservacao.ObterPorNome("SÁntúaRìo")).ShouldBe(1);
            (await servicoConservacao.ObterPorNome("SÀnTÚaRió")).ShouldBe(1);
            
            (await servicoConservacao.ObterPorNome("SExTanTé")).ShouldBe(2);
            (await servicoConservacao.ObterPorNome("SéxTaNtê")).ShouldBe(2);
            (await servicoConservacao.ObterPorNome("SEXTAntè")).ShouldBe(2);
            (await servicoConservacao.ObterPorNome("sextantÊ")).ShouldBe(2);
            
            (await servicoConservacao.ObterPorNome("AGAPÉ")).ShouldBe(3);
            (await servicoConservacao.ObterPorNome("aGapÊ")).ShouldBe(3);
            (await servicoConservacao.ObterPorNome("ÂgaPÈ")).ShouldBe(3);
            (await servicoConservacao.ObterPorNome("ÂgÃpÊ")).ShouldBe(3);
            
            (await servicoConservacao.ObterPorNome("PrÉssÌ")).ShouldBe(4);
            (await servicoConservacao.ObterPorNome("prÈssÍ")).ShouldBe(4);
            (await servicoConservacao.ObterPorNome("pRÊssi")).ShouldBe(4);
            (await servicoConservacao.ObterPorNome("pressÍ")).ShouldBe(4);
        }
    }
}