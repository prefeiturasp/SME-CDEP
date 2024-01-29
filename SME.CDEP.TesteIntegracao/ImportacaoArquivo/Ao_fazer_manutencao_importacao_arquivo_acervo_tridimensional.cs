using System.Globalization;
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
    public class Ao_fazer_manutencao_importacao_arquivo_acervo_tridimensional : TesteBase
    {
        public Ao_fazer_manutencao_importacao_arquivo_acervo_tridimensional(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - ValidarPreenchimentoValorFormatoQtdeCaracteres - Com erros: Titulo, Descrição, Data, Quantidade e Tombo")]
        public async Task Validar_preenchimento_valor_formato_qtde_caracteres()
        {
            await InserirDadosBasicos();
            
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoTridimensional();

            var acervoTridimensionalLinhas = AcervoTridimensionalLinhaMock.GerarAcervoTridimensionalLinhaDTO().Generate(10);

            acervoTridimensionalLinhas[2].Titulo.Conteudo = string.Empty;
            acervoTridimensionalLinhas[4].Descricao.Conteudo = string.Empty;
            acervoTridimensionalLinhas[5].Data.Conteudo = faker.Lorem.Paragraph();
            acervoTridimensionalLinhas[7].Quantidade.Conteudo = faker.Lorem.Paragraph();
            acervoTridimensionalLinhas[8].Codigo.Conteudo = string.Empty;
            var linhasComErros = new[] { 3, 5, 6, 8, 9 };
            
            await servicoImportacaoArquivo.CarregarDominios();
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoTridimensionalLinhas);

            foreach (var linha in acervoTridimensionalLinhas)
            {
                linha.PossuiErros.ShouldBe(linhasComErros.Any(a=> a.SaoIguais(linha.NumeroLinha)));

                if (linha.NumeroLinha.SaoIguais(3))
                {
                    linha.Titulo.PossuiErro.ShouldBeTrue();
                    linha.Titulo.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Titulo.PossuiErro.ShouldBeFalse();
                    linha.Titulo.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha.SaoIguais(5))
                {
                    linha.Descricao.PossuiErro.ShouldBeTrue();
                    linha.Descricao.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Descricao.PossuiErro.ShouldBeFalse();
                    linha.Descricao.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha.SaoIguais(6))
                {
                    linha.Data.PossuiErro.ShouldBeTrue();
                    linha.Data.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Data.PossuiErro.ShouldBeFalse();
                    linha.Data.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha.SaoIguais(8))
                {
                    linha.Quantidade.PossuiErro.ShouldBeTrue();
                    linha.Quantidade.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Quantidade.PossuiErro.ShouldBeFalse();
                    linha.Quantidade.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha.SaoIguais(9))
                {
                    linha.Codigo.PossuiErro.ShouldBeTrue();
                    linha.Codigo.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Codigo.PossuiErro.ShouldBeFalse();
                    linha.Codigo.Mensagem.ShouldBeEmpty();
                }
                   
                linha.Ano.PossuiErro.ShouldBeFalse();
                linha.Procedencia.PossuiErro.ShouldBeFalse();
                linha.EstadoConservacao.PossuiErro.ShouldBeFalse();
                linha.Largura.PossuiErro.ShouldBeFalse();
                linha.Altura.PossuiErro.ShouldBeFalse();
                linha.Profundidade.PossuiErro.ShouldBeFalse();
                linha.Diametro.PossuiErro.ShouldBeFalse();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - ValidarPreenchimentoValorFormatoQtdeCaracteres - Deve apresentar erro de estado de conservação inexistente")]
        public async Task Validar_preenchimento_valor_formato_qtde_caracteres_estado_conservacao_inexistente()
        {
            await InserirDadosBasicos();
            
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoTridimensional();

            var acervoTridimensionalLinhas = AcervoTridimensionalLinhaMock.GerarAcervoTridimensionalLinhaDTO().Generate(10);

            acervoTridimensionalLinhas[2].Titulo.Conteudo = string.Empty;
            acervoTridimensionalLinhas[4].Descricao.Conteudo = string.Empty;
            acervoTridimensionalLinhas[5].Data.Conteudo = faker.Lorem.Paragraph();
            acervoTridimensionalLinhas[7].Quantidade.Conteudo = faker.Lorem.Paragraph();
            acervoTridimensionalLinhas[8].Codigo.Conteudo = string.Empty;
            acervoTridimensionalLinhas[8].EstadoConservacao.Conteudo = "Desconhecido";
            var linhasComErros = new[] { 3, 5, 6, 8, 9 };
            
            await servicoImportacaoArquivo.CarregarDominios();
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoTridimensionalLinhas);

            foreach (var linha in acervoTridimensionalLinhas)
            {
                linha.PossuiErros.ShouldBe(linhasComErros.Any(a=> a.SaoIguais(linha.NumeroLinha)));

                if (linha.NumeroLinha.SaoIguais(3))
                {
                    linha.Titulo.PossuiErro.ShouldBeTrue();
                    linha.Titulo.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Titulo.PossuiErro.ShouldBeFalse();
                    linha.Titulo.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha.SaoIguais(5))
                {
                    linha.Descricao.PossuiErro.ShouldBeTrue();
                    linha.Descricao.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Descricao.PossuiErro.ShouldBeFalse();
                    linha.Descricao.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha.SaoIguais(6))
                {
                    linha.Data.PossuiErro.ShouldBeTrue();
                    linha.Data.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Data.PossuiErro.ShouldBeFalse();
                    linha.Data.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha.SaoIguais(8))
                {
                    linha.Quantidade.PossuiErro.ShouldBeTrue();
                    linha.Quantidade.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Quantidade.PossuiErro.ShouldBeFalse();
                    linha.Quantidade.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha.SaoIguais(9))
                {
                    linha.Codigo.PossuiErro.ShouldBeTrue();
                    linha.Codigo.Mensagem.ShouldNotBeEmpty();
                    linha.EstadoConservacao.PossuiErro.ShouldBeTrue();
                    linha.EstadoConservacao.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Codigo.PossuiErro.ShouldBeFalse();
                    linha.Codigo.Mensagem.ShouldBeEmpty();
                    linha.EstadoConservacao.PossuiErro.ShouldBeFalse();
                    linha.EstadoConservacao.Mensagem.ShouldBeEmpty();
                }
                   
                linha.Ano.PossuiErro.ShouldBeFalse();
                linha.Procedencia.PossuiErro.ShouldBeFalse();
                linha.Largura.PossuiErro.ShouldBeFalse();
                linha.Altura.PossuiErro.ShouldBeFalse();
                linha.Profundidade.PossuiErro.ShouldBeFalse();
                linha.Diametro.PossuiErro.ShouldBeFalse();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - PersistenciaAcervo")]
        public async Task Persistencia_acervo()
        {
            await InserirDadosBasicos();

            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoTridimensional();
        
            var acervoTridimensionalLinhas = AcervoTridimensionalLinhaMock.GerarAcervoTridimensionalLinhaDTO().Generate(10);

            acervoTridimensionalLinhas[0].Profundidade.Conteudo = "11,22"; 
        
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Tridimensional,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoTridimensionalLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            await servicoImportacaoArquivo.CarregarDominios();
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoTridimensionalLinhas);
        
            var acervos = ObterTodos<Acervo>();
            var acervosTridimensional = ObterTodos<AcervoTridimensional>();
            var conservacoes = ObterTodos<Conservacao>();
            
            //Acervos inseridos
            acervos.ShouldNotBeNull();
            acervos.Count().ShouldBe(10);
            
            //Acervos auxiliares inseridos
            acervosTridimensional.ShouldNotBeNull();
            acervosTridimensional.Count().ShouldBe(10);
            
            //Linhas com erros
            acervoTridimensionalLinhas.Count(w=> !w.PossuiErros).ShouldBe(10);
            acervoTridimensionalLinhas.Count(w=> w.PossuiErros).ShouldBe(0);
        
            foreach (var linhasComSucesso in acervoTridimensionalLinhas.Where(w=> !w.PossuiErros))
            {
                //Acervo
                acervos.Any(a=> a.Titulo.SaoIguais(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.SaoIguais(linhasComSucesso.Codigo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.SaoIguais(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue(); 
                acervos.Any(a=> a.DataAcervo.SaoIguais(linhasComSucesso.Data.Conteudo)).ShouldBeTrue(); 
                acervos.Any(a=> a.Ano.SaoIguais(linhasComSucesso.Ano.Conteudo.ConverterParaInteiro())).ShouldBeTrue(); 
                
                //Referência 1:1
                acervosTridimensional.Any(a=> a.ConservacaoId.SaoIguais(conservacoes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.EstadoConservacao.Conteudo)).Id)).ShouldBeTrue();
                
                //Campos livres
                acervosTridimensional.Any(a=> a.Procedencia.SaoIguais(linhasComSucesso.Procedencia.Conteudo)).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Quantidade.SaoIguais(linhasComSucesso.Quantidade.Conteudo.ConverterParaInteiro())).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Largura.SaoIguais(linhasComSucesso.Largura.Conteudo)).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Altura.SaoIguais(linhasComSucesso.Altura.Conteudo)).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Profundidade.SaoIguais(linhasComSucesso.Profundidade.Conteudo)).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Diametro.SaoIguais(linhasComSucesso.Diametro.Conteudo)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - Geral - Com erros em 3 linhas")]
        public async Task Importacao_geral_com_erros_em_3_linhas()
        {
            await InserirDadosBasicos();

            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoTridimensional();
        
            var acervoTridimensionalLinhas = AcervoTridimensionalLinhaMock.GerarAcervoTridimensionalLinhaDTO().Generate(10);
           
            acervoTridimensionalLinhas[3].Altura.Conteudo = "10";
            acervoTridimensionalLinhas[5].Largura.Conteudo = "10.30";
            acervoTridimensionalLinhas[7].Profundidade.Conteudo = "10,30d";
            acervoTridimensionalLinhas[7].Diametro.Conteudo = "abc";
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Tridimensional,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoTridimensionalLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            await servicoImportacaoArquivo.CarregarDominios();
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoTridimensionalLinhas);
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoTridimensionalLinhas);
            await servicoImportacaoArquivo.AtualizarImportacao(1, JsonConvert.SerializeObject(acervoTridimensionalLinhas), acervoTridimensionalLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);
            var retorno = await servicoImportacaoArquivo.ObterImportacaoPendente();

            var acervos = ObterTodos<Acervo>();
            var acervosTridimensional = ObterTodos<AcervoTridimensional>();
            var conservacoes = ObterTodos<Conservacao>();
            
            //Acervos inseridos
            acervos.ShouldNotBeNull();
            acervos.Count().ShouldBe(7);
            
            //Acervos auxiliares inseridos
            acervosTridimensional.ShouldNotBeNull();
            acervosTridimensional.Count().ShouldBe(7);
            
            //Linhas com erros
            acervoTridimensionalLinhas.Count(w=> !w.PossuiErros).ShouldBe(7);
            acervoTridimensionalLinhas.Count(w=> w.PossuiErros).ShouldBe(3);
            
            //Retorno front
            retorno.Id.ShouldBe(1);
            retorno.Nome.ShouldNotBeEmpty();
            retorno.TipoAcervo.ShouldBe(TipoAcervo.Tridimensional);
            retorno.DataImportacao.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            
            foreach (var linhaInserida in acervoTridimensionalLinhas.Where(w=> !w.PossuiErros))
            {
                retorno.Sucesso.Any(a=> a.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Tombo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.NumeroLinha.SaoIguais(linhaInserida.NumeroLinha)).ShouldBeTrue();
            }
            
            foreach (var linhaInserida in acervoTridimensionalLinhas.Where(w=> w.PossuiErros))
            {
                retorno.Erros.Any(a=> a.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Tombo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.NumeroLinha.SaoIguais(linhaInserida.NumeroLinha)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoObjeto.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Codigo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Procedencia.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Ano.SaoIguais(linhaInserida.Ano.Conteudo.ConverterParaInteiro())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.DataAcervo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.ConservacaoId.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Largura.SaoIguais(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Altura.SaoIguais(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Diametro.SaoIguais(linhaInserida.Diametro.Conteudo)).ShouldBeTrue();

                if (linhaInserida.Quantidade.PossuiErro)
                    retorno.Erros.Any(a=> a.RetornoObjeto.Quantidade.EhNulo()).ShouldBeTrue();
                else
                    retorno.Erros.Any(a=> a.RetornoObjeto.Quantidade.SaoIguais(linhaInserida.Quantidade.Conteudo.ObterLongoPorValorDoCampo())).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoObjeto.Descricao.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoErro.Titulo.Conteudo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Codigo.Conteudo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Procedencia.Conteudo.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Ano.Conteudo.SaoIguais(linhaInserida.Ano.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.DataAcervo.Conteudo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.ConservacaoId.Conteudo.SaoIguais(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Largura.Conteudo.SaoIguais(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Altura.Conteudo.SaoIguais(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Diametro.Conteudo.SaoIguais(linhaInserida.Diametro.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Quantidade.Conteudo.SaoIguais(linhaInserida.Quantidade.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Descricao.Conteudo.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
            }
        
            foreach (var linhasComSucesso in acervoTridimensionalLinhas.Where(w=> !w.PossuiErros))
            {
                //Acervo
                acervos.Any(a=> a.Titulo.SaoIguais(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.SaoIguais(linhasComSucesso.Codigo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.SaoIguais(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                acervos.Any(a=> a.DataAcervo.SaoIguais(linhasComSucesso.Data.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Ano.SaoIguais(linhasComSucesso.Ano.Conteudo.ConverterParaInteiro())).ShouldBeTrue();
                
                //Referência 1:1
                acervosTridimensional.Any(a=> a.ConservacaoId.SaoIguais(conservacoes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.EstadoConservacao.Conteudo)).Id)).ShouldBeTrue();
                
                //Campos livres
                acervosTridimensional.Any(a=> a.Procedencia.SaoIguais(linhasComSucesso.Procedencia.Conteudo)).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Quantidade.SaoIguais(linhasComSucesso.Quantidade.Conteudo.ConverterParaInteiro())).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Largura.SaoIguais(linhasComSucesso.Largura.Conteudo)).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Altura.SaoIguais(linhasComSucesso.Altura.Conteudo)).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Profundidade.SaoIguais(linhasComSucesso.Profundidade.Conteudo)).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Diametro.SaoIguais(linhasComSucesso.Diametro.Conteudo)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - Geral - Com erros em 3 linhas")]
        public async Task Importacao_geral_com_3_linhas_erradas()
        {
            await InserirDadosBasicos();

            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoTridimensional();
        
            var acervoTridimensionalLinhas = AcervoTridimensionalLinhaMock.GerarAcervoTridimensionalLinhaDTO().Generate(10);
           
            acervoTridimensionalLinhas[3].Descricao.Conteudo = string.Empty;
            acervoTridimensionalLinhas[5].Quantidade.Conteudo = faker.Lorem.Paragraph();
            acervoTridimensionalLinhas[7].Codigo.Conteudo = acervoTridimensionalLinhas[0].Codigo.Conteudo;
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Tridimensional,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoTridimensionalLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            await servicoImportacaoArquivo.CarregarDominios();
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoTridimensionalLinhas);
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoTridimensionalLinhas);
            await servicoImportacaoArquivo.AtualizarImportacao(1, JsonConvert.SerializeObject(acervoTridimensionalLinhas), acervoTridimensionalLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);
            var retorno = await servicoImportacaoArquivo.ObterImportacaoPendente();

            var acervos = ObterTodos<Acervo>();
            var acervosTridimensional = ObterTodos<AcervoTridimensional>();
            var conservacoes = ObterTodos<Conservacao>();
            
            //Acervos inseridos
            acervos.ShouldNotBeNull();
            acervos.Count().ShouldBe(7);
            
            //Acervos auxiliares inseridos
            acervosTridimensional.ShouldNotBeNull();
            acervosTridimensional.Count().ShouldBe(7);
            
            //Linhas com erros
            acervoTridimensionalLinhas.Count(w=> !w.PossuiErros).ShouldBe(7);
            acervoTridimensionalLinhas.Count(w=> w.PossuiErros).ShouldBe(3);
            
            //Retorno front
            retorno.Id.ShouldBe(1);
            retorno.Nome.ShouldNotBeEmpty();
            retorno.TipoAcervo.ShouldBe(TipoAcervo.Tridimensional);
            retorno.DataImportacao.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            
            foreach (var linhaInserida in acervoTridimensionalLinhas.Where(w=> !w.PossuiErros))
            {
                retorno.Sucesso.Any(a=> a.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Tombo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.NumeroLinha.SaoIguais(linhaInserida.NumeroLinha)).ShouldBeTrue();
            }
            
            foreach (var linhaInserida in acervoTridimensionalLinhas.Where(w=> w.PossuiErros))
            {
                retorno.Erros.Any(a=> a.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Tombo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.NumeroLinha.SaoIguais(linhaInserida.NumeroLinha)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoObjeto.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Codigo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Procedencia.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Ano.SaoIguais(linhaInserida.Ano.Conteudo.ConverterParaInteiro())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.DataAcervo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.ConservacaoId.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Largura.SaoIguais(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Altura.SaoIguais(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Diametro.SaoIguais(linhaInserida.Diametro.Conteudo)).ShouldBeTrue();

                if (linhaInserida.Quantidade.PossuiErro)
                    retorno.Erros.Any(a=> a.RetornoObjeto.Quantidade.EhNulo()).ShouldBeTrue();
                else
                    retorno.Erros.Any(a=> a.RetornoObjeto.Quantidade.SaoIguais(linhaInserida.Quantidade.Conteudo.ObterLongoPorValorDoCampo())).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoObjeto.Descricao.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoErro.Titulo.Conteudo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Codigo.Conteudo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Procedencia.Conteudo.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Ano.Conteudo.SaoIguais(linhaInserida.Ano.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.DataAcervo.Conteudo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.ConservacaoId.Conteudo.SaoIguais(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Largura.Conteudo.SaoIguais(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Altura.Conteudo.SaoIguais(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Diametro.Conteudo.SaoIguais(linhaInserida.Diametro.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Quantidade.Conteudo.SaoIguais(linhaInserida.Quantidade.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Descricao.Conteudo.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
            }
        
            foreach (var linhasComSucesso in acervoTridimensionalLinhas.Where(w=> !w.PossuiErros))
            {
                //Acervo
                acervos.Any(a=> a.Titulo.SaoIguais(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.SaoIguais(linhasComSucesso.Codigo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.SaoIguais(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                acervos.Any(a=> a.DataAcervo.SaoIguais(linhasComSucesso.Data.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Ano.SaoIguais(linhasComSucesso.Ano.Conteudo.ConverterParaInteiro())).ShouldBeTrue();
                
                //Referência 1:1
                acervosTridimensional.Any(a=> a.ConservacaoId.SaoIguais(conservacoes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.EstadoConservacao.Conteudo)).Id)).ShouldBeTrue();
                
                //Campos livres
                acervosTridimensional.Any(a=> a.Procedencia.SaoIguais(linhasComSucesso.Procedencia.Conteudo)).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Quantidade.SaoIguais(linhasComSucesso.Quantidade.Conteudo.ConverterParaInteiro())).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Largura.SaoIguais(linhasComSucesso.Largura.Conteudo)).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Altura.SaoIguais(linhasComSucesso.Altura.Conteudo)).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Profundidade.SaoIguais(linhasComSucesso.Profundidade.Conteudo)).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Diametro.SaoIguais(linhasComSucesso.Diametro.Conteudo)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - Obter importação pendente com Erros")]
        public async Task Obter_importacao_pendente_com_erros()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoTridimensional();
        
            var linhasInseridas = AcervoTridimensionalLinhaMock.GerarAcervoTridimensionalLinhaDTO().Generate(10);
        
            linhasInseridas[3].PossuiErros = true;
            linhasInseridas[3].Data.PossuiErro = true;
            linhasInseridas[3].Data.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_ATINGIU_LIMITE_CARACTERES, Dominio.Constantes.Constantes.DATA);
            
            linhasInseridas[9].PossuiErros = true;
            linhasInseridas[9].EstadoConservacao.PossuiErro = true;
            linhasInseridas[9].EstadoConservacao.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_NAO_PREENCHIDO, Dominio.Constantes.Constantes.SUPORTE);
           
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Tridimensional,
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
                retorno.Erros.Any(a=> a.RetornoObjeto.Codigo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Procedencia.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Ano.SaoIguais(linhaInserida.Ano.Conteudo.ConverterParaInteiro())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.DataAcervo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Largura.SaoIguais(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Altura.SaoIguais(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Diametro.SaoIguais(linhaInserida.Diametro.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Quantidade.SaoIguais(linhaInserida.Quantidade.Conteudo.ObterLongoPorValorDoCampo())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Descricao.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoErro.Titulo.Conteudo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Codigo.Conteudo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Procedencia.Conteudo.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Ano.Conteudo.SaoIguais(linhaInserida.Ano.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.DataAcervo.Conteudo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.ConservacaoId.Conteudo.SaoIguais(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Largura.Conteudo.SaoIguais(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Altura.Conteudo.SaoIguais(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Diametro.Conteudo.SaoIguais(linhaInserida.Diametro.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Quantidade.Conteudo.SaoIguais(linhaInserida.Quantidade.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Descricao.Conteudo.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - Deve permitir remover linha do arquivo")]
        public async Task Deve_permitir_remover_a_linha_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoTridimensional();

            var linhasInseridas = AcervoTridimensionalLinhaMock.GerarAcervoTridimensionalLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Tridimensional,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            var retorno = await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = 5});
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault(f => f.Id.SaoIguais(1));
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoTridimensionalLinhaDTO>>(arquivo.Conteudo);
            conteudo.Any(a=> a.NumeroLinha.SaoIguais(5)).ShouldBeFalse();
            conteudo.Count().ShouldBe(9);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - Não deve permitir remover linha do arquivo")]
        public async Task Nao_deve_permitir_remover_a_linha_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoTridimensional();

            var linhasInseridas = AcervoTridimensionalLinhaMock.GerarAcervoTridimensionalLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Tridimensional,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = 15}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - Não deve permitir remover todas as linhas do arquivo")]
        public async Task Nao_deve_permitir_remover_todas_as_linhas_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoTridimensional();

            var linhasInseridas = AcervoTridimensionalLinhaMock.GerarAcervoTridimensionalLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Tridimensional,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            for (int i = 1; i < 10; i++)
                (await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = i})).ShouldBe(true);
                
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoTridimensionalLinhaDTO>>(arquivo.Conteudo);
            conteudo.Count().ShouldBe(1);
            
            await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = 10}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - Deve permitir atualizar uma linha do arquivo para sucesso e outra fica com erro")]
        public async Task Deve_permitir_atualizar_uma_linha_do_arquivo_para_sucesso_e_outra_fica_com_erro()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoTridimensional();

            var linhasInseridas = AcervoTridimensionalLinhaMock.GerarAcervoTridimensionalLinhaDTO().Generate(10);
            linhasInseridas[3].PossuiErros = true;
            linhasInseridas[3].Altura.PossuiErro = true;
            linhasInseridas[3].Altura.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.LARGURA);
            linhasInseridas[3].Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.LARGURA); //Mensagem geral
            
            linhasInseridas[9].PossuiErros = true;
            linhasInseridas[9].Altura.PossuiErro = true;
            linhasInseridas[9].Altura.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_NAO_PREENCHIDO, Dominio.Constantes.Constantes.ALTURA);
            linhasInseridas[9].Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_NAO_PREENCHIDO, Dominio.Constantes.Constantes.SUPORTE); //Mensagem geral

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Tridimensional,
                Status = ImportacaoStatus.Erros,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.AtualizarLinhaParaSucesso(1, new LinhaDTO(){NumeroLinha = 4});
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoTridimensionalLinhaDTO>>(arquivo.Conteudo);
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).PossuiErros.ShouldBeFalse();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).Mensagem.ShouldBeEmpty();
            
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(10)).PossuiErros.ShouldBeTrue();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(10)).Mensagem.ShouldNotBeEmpty();
            
            arquivo.Status.ShouldBe(ImportacaoStatus.Erros);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - Deve permitir atualizar linha do arquivo para sucesso")]
        public async Task Deve_permitir_atualizar_linha_do_arquivo_para_sucesso()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoTridimensional();

            var linhasInseridas = AcervoTridimensionalLinhaMock.GerarAcervoTridimensionalLinhaDTO().Generate(10);
            linhasInseridas[3].PossuiErros = true;
            linhasInseridas[3].Altura.PossuiErro = true;
            linhasInseridas[3].Altura.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.ALTURA);
            linhasInseridas[3].Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.ALTURA); //Mensagem geral
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Tridimensional,
                Status = ImportacaoStatus.Erros,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.AtualizarLinhaParaSucesso(1, new LinhaDTO(){NumeroLinha = 4});
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoTridimensionalLinhaDTO>>(arquivo.Conteudo);
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).PossuiErros.ShouldBeFalse();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).Mensagem.ShouldBeEmpty();
            
            conteudo.Any(a=> a.PossuiErros).ShouldBeFalse();
            conteudo.Any(a=> !a.PossuiErros).ShouldBeTrue();
            
            arquivo.Status.ShouldBe(ImportacaoStatus.Sucesso);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - Validação de RetornoObjeto")]
        public async Task Validacao_retorno_objeto()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoTridimensional();

            var linhasInseridas = AcervoTridimensionalLinhaMock.GerarAcervoTridimensionalLinhaDTO().Generate(10);
            
            linhasInseridas.Add(new AcervoTridimensionalLinhaDTO()
            {
                PossuiErros = true,
                Titulo = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Profundidade = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Procedencia = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Ano = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Data = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Descricao = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Diametro = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Quantidade = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Altura = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Largura = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                EstadoConservacao = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Codigo = new LinhaConteudoAjustarDTO() { PossuiErro = true},
            });
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Tridimensional,
                Status = ImportacaoStatus.Erros,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            var retorno = await servicoImportacaoArquivo.ObterImportacaoPendente();
            foreach (var erro in retorno.Erros)
            {
                erro.RetornoObjeto.Titulo.ShouldBeNull();
                erro.RetornoObjeto.Procedencia.ShouldBeNull();
                erro.RetornoObjeto.Ano.ShouldBeNull();
                erro.RetornoObjeto.DataAcervo.ShouldBeNull();
                erro.RetornoObjeto.Descricao.ShouldBeNull();
                erro.RetornoObjeto.Quantidade.ShouldBeNull();
                erro.RetornoObjeto.Profundidade.ShouldBeNull();
                erro.RetornoObjeto.Diametro.ShouldBeNull();
                erro.RetornoObjeto.Altura.ShouldBeNull();
                erro.RetornoObjeto.Largura.ShouldBeNull();
                erro.RetornoObjeto.ConservacaoId.ShouldBeNull();
                erro.RetornoObjeto.ConservacaoId.ShouldBeNull();
                erro.RetornoObjeto.Codigo.ShouldBeNull();
            }
            retorno.ShouldNotBeNull();
        }

        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - Validação de campos de altura, largura, profundidade e diâmetro")]
        public async Task Validacao_campos_altura_largura_profundidade_diametro()
        {
            "12".TratarLiteralComoDecimalComCasasDecimais().ShouldBe("12,00");
            "15,1".TratarLiteralComoDecimalComCasasDecimais().ShouldBe("15,10");
            "25.2".TratarLiteralComoDecimalComCasasDecimais().ShouldBe("25,20");
            "15,10".TratarLiteralComoDecimalComCasasDecimais().ShouldBe("15,10");
            "25.20".TratarLiteralComoDecimalComCasasDecimais().ShouldBe("25,20");
            "25.2b".TratarLiteralComoDecimalComCasasDecimais().ShouldBe("25.2b"); 
            "qualquer valor".TratarLiteralComoDecimalComCasasDecimais().ShouldBe("qualquer valor");
            "1.450,30".TratarLiteralComoDecimalComCasasDecimais().ShouldBe("1.450,30");
        }
    }
}