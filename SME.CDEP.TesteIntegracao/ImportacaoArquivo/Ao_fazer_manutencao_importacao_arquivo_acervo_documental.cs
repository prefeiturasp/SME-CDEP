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
                retorno.Sucesso.Any(a=> a.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Tombo.SaoIguais(ObterCodigo(linhaInserida))).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.NumeroLinha.SaoIguais(linhaInserida.NumeroLinha)).ShouldBeTrue();
            }
            
            foreach (var linhaInserida in linhasInseridas.Where(w=> w.PossuiErros))
            {
                retorno.Erros.Any(a=> a.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Tombo.SaoIguais(ObterCodigo(linhaInserida))).ShouldBeTrue();
                retorno.Erros.Any(a=> a.NumeroLinha.SaoIguais(linhaInserida.NumeroLinha)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoObjeto.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Codigo.SaoIguais(linhaInserida.CodigoAntigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.CodigoNovo.SaoIguais(linhaInserida.CodigoNovo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.MaterialId.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.IdiomaId.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.CopiaDigital.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Ano.SaoIguais(linhaInserida.Ano.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.NumeroPagina.SaoIguais(linhaInserida.NumeroPaginas.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Volume.SaoIguais(linhaInserida.Volume.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Descricao.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.TipoAnexo.SaoIguais(linhaInserida.TipoAnexo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Largura.SaoIguais(linhaInserida.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Altura.SaoIguais(linhaInserida.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.TamanhoArquivo.SaoIguais(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.AcessoDocumentosIds.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Localizacao.SaoIguais(linhaInserida.Localizacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.CopiaDigital.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.ConservacaoId.NaoEhNulo()).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoErro.Titulo.Conteudo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CodigoAntigo.Conteudo.SaoIguais(linhaInserida.CodigoAntigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CodigoNovo.Conteudo.SaoIguais(linhaInserida.CodigoNovo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Material.Conteudo.SaoIguais(linhaInserida.Material.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Idioma.Conteudo.SaoIguais(linhaInserida.Idioma.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CopiaDigital.Conteudo.SaoIguais(linhaInserida.CopiaDigital.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Autor.Conteudo.SaoIguais(linhaInserida.Autor.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.EstadoConservacao.Conteudo.SaoIguais(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Ano.Conteudo.SaoIguais(linhaInserida.Ano.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.NumeroPaginas.Conteudo.SaoIguais(linhaInserida.NumeroPaginas.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Volume.Conteudo.SaoIguais(linhaInserida.Volume.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Descricao.Conteudo.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.TipoAnexo.Conteudo.SaoIguais(linhaInserida.TipoAnexo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Largura.Conteudo.SaoIguais(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Altura.Conteudo.SaoIguais(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.TamanhoArquivo.Conteudo.SaoIguais(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.AcessoDocumento.Conteudo.SaoIguais(linhaInserida.AcessoDocumento.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Localizacao.Conteudo.SaoIguais(linhaInserida.Localizacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CopiaDigital.Conteudo.SaoIguais(linhaInserida.CopiaDigital.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.EstadoConservacao.Conteudo.SaoIguais(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
            }
        }
        
        private string ObterCodigo(AcervoDocumentalLinhaDTO s)
        {
            if (s.CodigoAntigo.Conteudo.EstaPreenchido() && s.CodigoNovo.Conteudo.EstaPreenchido())
                return $"{s.CodigoAntigo.Conteudo}/{s.CodigoNovo.Conteudo}";
            
            if (s.CodigoAntigo.Conteudo.EstaPreenchido())
                return s.CodigoAntigo.Conteudo;
            
            return s.CodigoNovo.Conteudo.EstaPreenchido() ? s.CodigoNovo.Conteudo : default;
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
        
        [Fact(DisplayName = "Importação Arquivo Acervo Documental - Deve permitir remover linha do arquivo")]
        public async Task Deve_permitir_remover_a_linha_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoDocumental();

            var linhasInseridas = GerarAcervoDocumentalLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.DocumentacaoHistorica,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            var retorno = await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, 5);
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault(f => f.Id.SaoIguais(1));
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoDocumentalLinhaDTO>>(arquivo.Conteudo);
            conteudo.Any(a=> a.NumeroLinha.SaoIguais(5)).ShouldBeFalse();
            conteudo.Count().ShouldBe(9);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Documental - Não deve permitir remover linha do arquivo")]
        public async Task Nao_deve_permitir_remover_a_linha_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoDocumental();

            var linhasInseridas = GerarAcervoDocumentalLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.DocumentacaoHistorica,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, 15).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Documental - Não deve permitir remover todas as linhas do arquivo")]
        public async Task Nao_deve_permitir_remover_todas_as_linhas_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoDocumental();

            var linhasInseridas = GerarAcervoDocumentalLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.DocumentacaoHistorica,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            for (int i = 1; i < 10; i++)
                (await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, i)).ShouldBe(true);
                
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoDocumentalLinhaDTO>>(arquivo.Conteudo);
            conteudo.Count().ShouldBe(1);
            
            await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, 10).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Documental - Deve permitir atualizar uma linha do arquivo para sucesso e outra fica com erro")]
        public async Task Deve_permitir_atualizar_uma_linha_do_arquivo_para_sucesso_e_outra_fica_com_erro()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoDocumental();

            var linhasInseridas = GerarAcervoDocumentalLinhaDTO().Generate(10);
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
                TipoAcervo = TipoAcervo.DocumentacaoHistorica,
                Status = ImportacaoStatus.Erros,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.AtualizarLinhaParaSucesso(1, 4);
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoDocumentalLinhaDTO>>(arquivo.Conteudo);
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).PossuiErros.ShouldBeFalse();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).Mensagem.ShouldBeEmpty();
            
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(10)).PossuiErros.ShouldBeTrue();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(10)).Mensagem.ShouldNotBeEmpty();
            
            arquivo.Status.ShouldBe(ImportacaoStatus.Erros);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Documental - Deve permitir atualizar linha do arquivo para sucesso")]
        public async Task Deve_permitir_atualizar_linha_do_arquivo_para_sucesso()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoDocumental();

            var linhasInseridas = GerarAcervoDocumentalLinhaDTO().Generate(10);
            linhasInseridas[3].PossuiErros = true;
            linhasInseridas[3].Altura.PossuiErro = true;
            linhasInseridas[3].Altura.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.ALTURA);
            linhasInseridas[3].Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.ALTURA); //Mensagem geral
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.DocumentacaoHistorica,
                Status = ImportacaoStatus.Erros,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.AtualizarLinhaParaSucesso(1, 4);
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoDocumentalLinhaDTO>>(arquivo.Conteudo);
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).PossuiErros.ShouldBeFalse();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).Mensagem.ShouldBeEmpty();
            
            conteudo.Any(a=> a.PossuiErros).ShouldBeFalse();
            conteudo.Any(a=> !a.PossuiErros).ShouldBeTrue();
            
            arquivo.Status.ShouldBe(ImportacaoStatus.Sucesso);
        }
    }
}