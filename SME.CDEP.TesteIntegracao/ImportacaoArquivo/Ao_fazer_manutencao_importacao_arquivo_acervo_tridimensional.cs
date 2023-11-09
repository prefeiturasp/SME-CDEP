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
    public class Ao_fazer_manutencao_importacao_arquivo_acervo_tridimensional : TesteBase
    {
        public Ao_fazer_manutencao_importacao_arquivo_acervo_tridimensional(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - ValidarPreenchimentoValorFormatoQtdeCaracteres - Com erros: Titulo, Descrição, Data, Quantidade e Tombo")]
        public async Task Validar_preenchimento_valor_formato_qtde_caracteres()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoTridimensional();

            var acervoTridimensionalLinhas = GerarAcervoTridimensionalLinhaDTO().Generate(10);

            acervoTridimensionalLinhas[2].Titulo.Conteudo = string.Empty;
            acervoTridimensionalLinhas[4].Descricao.Conteudo = string.Empty;
            acervoTridimensionalLinhas[5].Data.Conteudo = faker.Lorem.Paragraph();
            acervoTridimensionalLinhas[7].Quantidade.Conteudo = faker.Lorem.Paragraph();
            acervoTridimensionalLinhas[8].Tombo.Conteudo = string.Empty;
            var linhasComErros = new[] { 3, 5, 6, 8, 9 };
            
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
                    linha.Tombo.PossuiErro.ShouldBeTrue();
                    linha.Tombo.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Tombo.PossuiErro.ShouldBeFalse();
                    linha.Tombo.Mensagem.ShouldBeEmpty();
                }
                   
                linha.Procedencia.PossuiErro.ShouldBeFalse();
                linha.EstadoConservacao.PossuiErro.ShouldBeFalse();
                linha.Largura.PossuiErro.ShouldBeFalse();
                linha.Altura.PossuiErro.ShouldBeFalse();
                linha.Profundidade.PossuiErro.ShouldBeFalse();
                linha.Diametro.PossuiErro.ShouldBeFalse();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - ValidacaoObterOuInserirDominios")]
        public async Task Validacao_obter_ou_inserir_dominios()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoTridimensional();
        
            var acervoTridimensionalLinhas = GerarAcervoTridimensionalLinhaDTO().Generate(10);
           
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoTridimensionalLinhas);
        
            foreach (var linha in acervoTridimensionalLinhas)
            {
                var conservacoesInseridas = linha.EstadoConservacao.Conteudo;
                var conservacoes = ObterTodos<Conservacao>();
                conservacoes.Any(a => a.Nome.SaoIguais(conservacoesInseridas)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - PersistenciaAcervo")]
        public async Task Persistencia_acervo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoTridimensional();
        
            var acervoTridimensionalLinhas = GerarAcervoTridimensionalLinhaDTO().Generate(10);
        
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Tridimensional,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoTridimensionalLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoTridimensionalLinhas);
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoTridimensionalLinhas);
        
            var acervos = ObterTodos<Acervo>();
            var acervosTridimensional = ObterTodos<AcervoTridimensional>();
            var acervoCreditoAutors = ObterTodos<AcervoCreditoAutor>();
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
                acervos.Any(a=> a.Codigo.SaoIguais(linhasComSucesso.Tombo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.SaoIguais(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosTridimensional.Any(a=> a.ConservacaoId.SaoIguais(conservacoes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.EstadoConservacao.Conteudo)).Id)).ShouldBeTrue();
                
                //Campos livres
                acervosTridimensional.Any(a=> a.Procedencia.SaoIguais(linhasComSucesso.Procedencia.Conteudo)).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.DataAcervo.SaoIguais(linhasComSucesso.Data.Conteudo)).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Quantidade.SaoIguais(linhasComSucesso.Quantidade.Conteudo.ObterLongoPorValorDoCampo())).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Largura.SaoIguais(linhasComSucesso.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Altura.SaoIguais(linhasComSucesso.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Profundidade.SaoIguais(linhasComSucesso.Profundidade.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Diametro.SaoIguais(linhasComSucesso.Diametro.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - Geral - Com erros em 3 linhas")]
        public async Task Importacao_geral()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoTridimensional();
        
            var acervoTridimensionalLinhas = GerarAcervoTridimensionalLinhaDTO().Generate(10);
           
            acervoTridimensionalLinhas[3].Descricao.Conteudo = string.Empty;
            acervoTridimensionalLinhas[5].Quantidade.Conteudo = faker.Lorem.Paragraph();
            acervoTridimensionalLinhas[7].Tombo.Conteudo = acervoTridimensionalLinhas[0].Tombo.Conteudo;
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Tridimensional,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoTridimensionalLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoTridimensionalLinhas);
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoTridimensionalLinhas );
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoTridimensionalLinhas);
        
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
        
            foreach (var linhasComSucesso in acervoTridimensionalLinhas.Where(w=> !w.PossuiErros))
            {
                //Acervo
                acervos.Any(a=> a.Titulo.SaoIguais(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.SaoIguais(linhasComSucesso.Tombo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.SaoIguais(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosTridimensional.Any(a=> a.ConservacaoId.SaoIguais(conservacoes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.EstadoConservacao.Conteudo)).Id)).ShouldBeTrue();
                
                //Campos livres
                acervosTridimensional.Any(a=> a.Procedencia.SaoIguais(linhasComSucesso.Procedencia.Conteudo)).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.DataAcervo.SaoIguais(linhasComSucesso.Data.Conteudo)).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Quantidade.SaoIguais(linhasComSucesso.Quantidade.Conteudo.ObterLongoPorValorDoCampo())).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Largura.SaoIguais(linhasComSucesso.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Altura.SaoIguais(linhasComSucesso.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Profundidade.SaoIguais(linhasComSucesso.Profundidade.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosTridimensional.Any(a=> a.Diametro.SaoIguais(linhasComSucesso.Diametro.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Tridimensional - Obter importação pendente com Erros")]
        public async Task Obter_importacao_pendente_com_erros()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoTridimensional();
        
            var linhasInseridas = GerarAcervoTridimensionalLinhaDTO().Generate(10);
        
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
                retorno.Sucesso.Any(a=> a.Titulo.Conteudo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Tombo.Conteudo.SaoIguais(linhaInserida.Tombo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Procedencia.Conteudo.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Data.Conteudo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.EstadoConservacao.Conteudo.SaoIguais(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Quantidade.Conteudo.SaoIguais(linhaInserida.Quantidade.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Descricao.Conteudo.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Largura.Conteudo.SaoIguais(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Altura.Conteudo.SaoIguais(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Profundidade.Conteudo.SaoIguais(linhaInserida.Profundidade.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Diametro.Conteudo.SaoIguais(linhaInserida.Diametro.Conteudo)).ShouldBeTrue();
            }
            
            foreach (var linhaInserida in linhasInseridas.Where(w=> w.PossuiErros))
            {
                retorno.Erros.Any(a=> a.Titulo.Conteudo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Tombo.Conteudo.SaoIguais(linhaInserida.Tombo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Procedencia.Conteudo.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Data.Conteudo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.EstadoConservacao.Conteudo.SaoIguais(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Quantidade.Conteudo.SaoIguais(linhaInserida.Quantidade.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Descricao.Conteudo.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Largura.Conteudo.SaoIguais(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Altura.Conteudo.SaoIguais(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Profundidade.Conteudo.SaoIguais(linhaInserida.Profundidade.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Diametro.Conteudo.SaoIguais(linhaInserida.Diametro.Conteudo)).ShouldBeTrue();
            }
        }
    }
}