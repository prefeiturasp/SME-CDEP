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
    public class Ao_fazer_manutencao_importacao_arquivo_acervo_arte_grafica : TesteBase
    {
        public Ao_fazer_manutencao_importacao_arquivo_acervo_arte_grafica(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - ValidarPreenchimentoValorFormatoQtdeCaracteres - Com erros: Titulo, Cromia, Largura, Diâmetro e Tombo")]
        public async Task Validar_preenchimento_valor_formato_qtde_caracteres()
        {
            await InserirDadosBasicos();
            
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoArteGrafica();

            var acervoArteGraficaLinhas = AcervoArteGraficaLinhaMock.GerarAcervoArteGraficaLinhaDTO().Generate(10);

            acervoArteGraficaLinhas[2].Titulo.Conteudo = string.Empty;
            acervoArteGraficaLinhas[4].Cromia.Conteudo = string.Empty;
            acervoArteGraficaLinhas[5].Largura.Conteudo = faker.Lorem.Paragraph();
            acervoArteGraficaLinhas[7].Diametro.Conteudo = faker.Lorem.Paragraph();
            acervoArteGraficaLinhas[8].Quantidade.Conteudo = faker.Lorem.Paragraph();
            var linhasComErros = new[] { 3, 5, 6, 8, 9 };
            
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoArteGraficaLinhas);

            foreach (var linha in acervoArteGraficaLinhas)
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
                    linha.Cromia.PossuiErro.ShouldBeTrue();
                    linha.Cromia.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Cromia.PossuiErro.ShouldBeFalse();
                    linha.Cromia.Mensagem.ShouldBeEmpty();
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
                    linha.Diametro.PossuiErro.ShouldBeTrue();
                    linha.Diametro.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Diametro.PossuiErro.ShouldBeFalse();
                    linha.Diametro.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha.SaoIguais(9))
                {
                    linha.Quantidade.PossuiErro.ShouldBeTrue();
                    linha.Quantidade.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Quantidade.PossuiErro.ShouldBeFalse();
                    linha.Quantidade.Mensagem.ShouldBeEmpty();
                }
                
                linha.CopiaDigital.PossuiErro.ShouldBeFalse();
                linha.Credito.PossuiErro.ShouldBeFalse();
                linha.Localizacao.PossuiErro.ShouldBeFalse();
                linha.Procedencia.PossuiErro.ShouldBeFalse();
                linha.Ano.PossuiErro.ShouldBeFalse();
                linha.Data.PossuiErro.ShouldBeFalse();
                linha.PermiteUsoImagem.PossuiErro.ShouldBeFalse();
                linha.EstadoConservacao.PossuiErro.ShouldBeFalse();
                linha.Altura.PossuiErro.ShouldBeFalse();
                linha.Tecnica.PossuiErro.ShouldBeFalse();
                linha.Suporte.PossuiErro.ShouldBeFalse();
                linha.Codigo.PossuiErro.ShouldBeFalse();
                linha.Descricao.PossuiErro.ShouldBeFalse();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - PersistenciaAcervo")]
        public async Task Persistencia_acervo()
        {
            await InserirDadosBasicos();
            
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoArteGrafica();
        
            var acervoArteGraficaLinhas = AcervoArteGraficaLinhaMock.GerarAcervoArteGraficaLinhaDTO().Generate(10);
            
            var creditos = acervoArteGraficaLinhas
                .SelectMany(acervoArteGraficaLinhaDto => acervoArteGraficaLinhaDto.Credito.Conteudo.FormatarTextoEmArray().UnificarPipe().SplitPipe())
                .Distinct()
                .ToList();

            await InserirCreditosAutorias(creditos);
        
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoArteGraficaLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoArteGraficaLinhas);
        
            var acervos = ObterTodos<Acervo>();
            var acervosArtesGraficas = ObterTodos<AcervoArteGrafica>();
            var suportes = ObterTodos<Suporte>();
            var cromias = ObterTodos<Cromia>();
            var creditoAutores = ObterTodos<CreditoAutor>();
            var conservacoes = ObterTodos<Conservacao>();
            
            //Acervos inseridos
            acervos.ShouldNotBeNull();
            acervos.Count().ShouldBe(10);
            
            //Acervos auxiliares inseridos
            acervosArtesGraficas.ShouldNotBeNull();
            acervosArtesGraficas.Count().ShouldBe(10);
            
            //Linhas com erros
            acervoArteGraficaLinhas.Count(w=> !w.PossuiErros).ShouldBe(10);
            acervoArteGraficaLinhas.Count(w=> w.PossuiErros).ShouldBe(0);
        
            foreach (var linhasComSucesso in acervoArteGraficaLinhas.Where(w=> !w.PossuiErros))
            {
                //Acervo
                acervos.Any(a=> a.Titulo.SaoIguais(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.SaoIguais(linhasComSucesso.Codigo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.SaoIguais(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                acervos.Any(a=> a.DataAcervo.SaoIguais(linhasComSucesso.Data.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Ano.SaoIguais(linhasComSucesso.Ano.Conteudo.ConverterParaInteiro())).ShouldBeTrue();
                
                //Referência 1:1
                acervosArtesGraficas.Any(a=> a.SuporteId.SaoIguais(suportes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Suporte.Conteudo)).Id)).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.CromiaId.SaoIguais(cromias.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Cromia.Conteudo)).Id)).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.ConservacaoId.SaoIguais(conservacoes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.EstadoConservacao.Conteudo)).Id)).ShouldBeTrue();
                
                //Campos livres
                acervosArtesGraficas.Any(a=> a.Localizacao.SaoIguais(linhasComSucesso.Localizacao.Conteudo)).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Procedencia.SaoIguais(linhasComSucesso.Procedencia.Conteudo)).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.CopiaDigital.SaoIguais(linhasComSucesso.CopiaDigital.Conteudo.EhOpcaoSim())).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.PermiteUsoImagem.SaoIguais(linhasComSucesso.PermiteUsoImagem.Conteudo.EhOpcaoSim())).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Altura.SaoIguais(linhasComSucesso.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Largura.SaoIguais(linhasComSucesso.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Diametro.SaoIguais(linhasComSucesso.Diametro.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Quantidade.SaoIguais(linhasComSucesso.Quantidade.Conteudo.ObterLongoPorValorDoCampo())).ShouldBeTrue();
                
                //Crédito
                var creditoAInserir = linhasComSucesso.Credito.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();

                foreach (var credito in creditoAInserir)
                    creditoAutores.Any(a=> a.Nome.SaoIguais(credito)).ShouldBeTrue();	
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - Geral - Com erros em 4 linhas")]
        public async Task Importacao_geral()
        {
            await InserirDadosBasicos();
            
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoArteGrafica();
        
            var acervoArteGraficaLinhas = AcervoArteGraficaLinhaMock.GerarAcervoArteGraficaLinhaDTO().Generate(10);
        
            var creditos = acervoArteGraficaLinhas
                .SelectMany(acervoArteGraficaLinhaDto => acervoArteGraficaLinhaDto.Credito.Conteudo.FormatarTextoEmArray().UnificarPipe().SplitPipe())
                .Distinct()
                .ToList();
            
            await InserirCreditosAutorias(creditos);
            
            acervoArteGraficaLinhas[1].CopiaDigital.Conteudo = acervoArteGraficaLinhas[1].Titulo.Conteudo;
            acervoArteGraficaLinhas[3].Largura.Conteudo = "ABC3512";
            acervoArteGraficaLinhas[5].Altura.Conteudo = "1212ABC";
            acervoArteGraficaLinhas[7].Codigo.Conteudo = acervoArteGraficaLinhas[0].Codigo.Conteudo;
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoArteGraficaLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoArteGraficaLinhas);
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoArteGraficaLinhas);
            await servicoImportacaoArquivo.AtualizarImportacao(1, JsonConvert.SerializeObject(acervoArteGraficaLinhas), acervoArteGraficaLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);
            var retorno = await servicoImportacaoArquivo.ObterImportacaoPendente();
        
             var acervos = ObterTodos<Acervo>();
            var acervosArtesGraficas = ObterTodos<AcervoArteGrafica>();
            var suportes = ObterTodos<Suporte>();
            var cromias = ObterTodos<Cromia>();
            var creditoAutores = ObterTodos<CreditoAutor>();
            var conservacoes = ObterTodos<Conservacao>();
            
            //Acervos inseridos
            acervos.ShouldNotBeNull();
            acervos.Count().ShouldBe(6);
            
            //Acervos auxiliares inseridos
            acervosArtesGraficas.ShouldNotBeNull();
            acervosArtesGraficas.Count().ShouldBe(6);
            
            //Linhas com erros
            acervoArteGraficaLinhas.Count(w=> !w.PossuiErros).ShouldBe(6);
            acervoArteGraficaLinhas.Count(w=> w.PossuiErros).ShouldBe(4);
        
            //Retorno front
            retorno.Id.ShouldBe(1);
            retorno.Nome.ShouldNotBeEmpty();
            retorno.TipoAcervo.ShouldBe(TipoAcervo.ArtesGraficas);
            retorno.DataImportacao.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            
            foreach (var linhaInserida in acervoArteGraficaLinhas.Where(w=> w.PossuiErros))
            {
                retorno.Erros.Any(a=> a.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Tombo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.NumeroLinha.SaoIguais(linhaInserida.NumeroLinha)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoObjeto.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Codigo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Procedencia.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.DataAcervo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Ano.SaoIguais(linhaInserida.Ano.Conteudo.ConverterParaInteiro())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.ConservacaoId.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.CromiaId.NaoEhNulo()).ShouldBeTrue();

                if (linhaInserida.Altura.PossuiErro)
                    retorno.Erros.Any(a=> a.RetornoObjeto.Altura.EhNulo()).ShouldBeTrue();
                else
                    retorno.Erros.Any(a=> a.RetornoObjeto.Altura.SaoIguais(linhaInserida.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();

                if (linhaInserida.Largura.PossuiErro)
                    retorno.Erros.Any(a=> a.RetornoObjeto.Largura.EhNulo()).ShouldBeTrue();
                else
                    retorno.Erros.Any(a=> a.RetornoObjeto.Largura.SaoIguais(linhaInserida.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
               
                retorno.Erros.Any(a=> a.RetornoObjeto.Diametro.SaoIguais(linhaInserida.Diametro.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Tecnica.SaoIguais(linhaInserida.Tecnica.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.SuporteId.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Quantidade.SaoIguais(linhaInserida.Quantidade.Conteudo.ObterLongoPorValorDoCampo())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Descricao.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoErro.Titulo.Conteudo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Codigo.Conteudo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Procedencia.Conteudo.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.DataAcervo.Conteudo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Ano.Conteudo.SaoIguais(linhaInserida.Ano.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CopiaDigital.Conteudo.SaoIguais(linhaInserida.CopiaDigital.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.PermiteUsoImagem.Conteudo.SaoIguais(linhaInserida.PermiteUsoImagem.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.ConservacaoId.Conteudo.SaoIguais(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CromiaId.Conteudo.SaoIguais(linhaInserida.Cromia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Largura.Conteudo.SaoIguais(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Altura.Conteudo.SaoIguais(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Diametro.Conteudo.SaoIguais(linhaInserida.Diametro.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Tecnica.Conteudo.SaoIguais(linhaInserida.Tecnica.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.SuporteId.Conteudo.SaoIguais(linhaInserida.Suporte.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Quantidade.Conteudo.SaoIguais(linhaInserida.Quantidade.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Descricao.Conteudo.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
            }
             
            foreach (var linhasComSucesso in acervoArteGraficaLinhas.Where(w=> !w.PossuiErros))
            {
                retorno.Sucesso.Any(a=> a.Titulo.SaoIguais(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Tombo.SaoIguais(linhasComSucesso.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.NumeroLinha.SaoIguais(linhasComSucesso.NumeroLinha)).ShouldBeTrue();
                
                //Acervo
                acervos.Any(a=> a.Titulo.SaoIguais(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.SaoIguais(linhasComSucesso.Codigo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.SaoIguais(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                acervos.Any(a=> a.DataAcervo.SaoIguais(linhasComSucesso.Data.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Ano.SaoIguais(linhasComSucesso.Ano.Conteudo.ConverterParaInteiro())).ShouldBeTrue();
                
                //Referência 1:1
                acervosArtesGraficas.Any(a=> a.SuporteId.SaoIguais(suportes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Suporte.Conteudo)).Id)).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.CromiaId.SaoIguais(cromias.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Cromia.Conteudo)).Id)).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.ConservacaoId.SaoIguais(conservacoes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.EstadoConservacao.Conteudo)).Id)).ShouldBeTrue();
                
                //Campos livres
                acervosArtesGraficas.Any(a=> a.Localizacao.SaoIguais(linhasComSucesso.Localizacao.Conteudo)).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Procedencia.SaoIguais(linhasComSucesso.Procedencia.Conteudo)).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.CopiaDigital.SaoIguais(linhasComSucesso.CopiaDigital.Conteudo.EhOpcaoSim())).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.PermiteUsoImagem.SaoIguais(linhasComSucesso.PermiteUsoImagem.Conteudo.EhOpcaoSim())).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Altura.SaoIguais(linhasComSucesso.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Largura.SaoIguais(linhasComSucesso.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Diametro.SaoIguais(linhasComSucesso.Diametro.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Quantidade.SaoIguais(linhasComSucesso.Quantidade.Conteudo.ObterLongoPorValorDoCampo())).ShouldBeTrue();
                
                //Crédito
                var creditoAInserir = linhasComSucesso.Credito.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var credito in creditoAInserir)
                    creditoAutores.Any(a=> a.Nome.SaoIguais(credito)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - Obter importação pendente com Erros")]
        public async Task Obter_importacao_pendente_com_erros()
        {
            await InserirDadosBasicos();
            
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoArteGrafica();
        
            var linhasInseridas = AcervoArteGraficaLinhaMock.GerarAcervoArteGraficaLinhaDTO().Generate(10);
        
            linhasInseridas[3].PossuiErros = true;
            linhasInseridas[3].Largura.PossuiErro = true;
            linhasInseridas[3].Largura.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.LARGURA);
            
            linhasInseridas[9].PossuiErros = true;
            linhasInseridas[9].Suporte.PossuiErro = true;
            linhasInseridas[9].Suporte.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_NAO_PREENCHIDO, Dominio.Constantes.Constantes.SUPORTE);
           
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            await InserirCreditosAutorias(linhasInseridas.Select(s => s.Credito.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct().Where(w=> w.EstaPreenchido()));
            
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
                retorno.Erros.Any(a=> a.RetornoObjeto.DataAcervo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Ano.SaoIguais(linhaInserida.Ano.Conteudo.ConverterParaInteiro())).ShouldBeTrue();

                if (linhaInserida.Altura.PossuiErro)
                    retorno.Erros.Any(a=> a.RetornoObjeto.Altura.Value.SaoIguais(0)).ShouldBeTrue();
                else
                    retorno.Erros.Any(a=> a.RetornoObjeto.Altura.SaoIguais(linhaInserida.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoObjeto.Diametro.SaoIguais(linhaInserida.Diametro.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Tecnica.SaoIguais(linhaInserida.Tecnica.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Quantidade.SaoIguais(linhaInserida.Quantidade.Conteudo.ObterLongoPorValorDoCampo())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Descricao.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoErro.Titulo.Conteudo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Codigo.Conteudo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Procedencia.Conteudo.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.DataAcervo.Conteudo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Ano.Conteudo.SaoIguais(linhaInserida.Ano.Conteudo)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoErro.CopiaDigital.Conteudo.SaoIguais(linhaInserida.CopiaDigital.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.PermiteUsoImagem.Conteudo.SaoIguais(linhaInserida.PermiteUsoImagem.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.ConservacaoId.Conteudo.SaoIguais(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CromiaId.Conteudo.SaoIguais(linhaInserida.Cromia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Largura.Conteudo.SaoIguais(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Altura.Conteudo.SaoIguais(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Diametro.Conteudo.SaoIguais(linhaInserida.Diametro.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Tecnica.Conteudo.SaoIguais(linhaInserida.Tecnica.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.SuporteId.Conteudo.SaoIguais(linhaInserida.Suporte.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Quantidade.Conteudo.SaoIguais(linhaInserida.Quantidade.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Descricao.Conteudo.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
            }
        }

        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - Validar Suporte sem caracteres especiais")]
        public async Task Validar_suporte_sem_caracteres_especiais()
        {
            var servicoSuporte = GetServicoSuporte();
        
            await InserirNaBase(new Suporte() { Nome = "Português", Tipo = TipoSuporte.IMAGEM});
            await InserirNaBase(new Suporte() { Nome = "Inglês" , Tipo = TipoSuporte.IMAGEM});
            await InserirNaBase(new Suporte() { Nome = "Geografia", Tipo = TipoSuporte.IMAGEM });
            await InserirNaBase(new Suporte() { Nome = "Matemática", Tipo = TipoSuporte.IMAGEM});
            
            (await servicoSuporte.ObterPorNomeETipo("PorTuGueS",(int)TipoSuporte.IMAGEM)).ShouldBe(1);
            (await servicoSuporte.ObterPorNomeETipo("PORTUGUES",(int)TipoSuporte.IMAGEM)).ShouldBe(1);
            (await servicoSuporte.ObterPorNomeETipo("PóRTÚGUEs",(int)TipoSuporte.IMAGEM)).ShouldBe(1);
            (await servicoSuporte.ObterPorNomeETipo("PôRTUGüES",(int)TipoSuporte.IMAGEM)).ShouldBe(1);
            
            (await servicoSuporte.ObterPorNomeETipo("InGlêS",(int)TipoSuporte.IMAGEM)).ShouldBe(2);
            (await servicoSuporte.ObterPorNomeETipo("ÍNGLÉS",(int)TipoSuporte.IMAGEM)).ShouldBe(2);
            (await servicoSuporte.ObterPorNomeETipo("ÎNGLÉS",(int)TipoSuporte.IMAGEM)).ShouldBe(2);
            (await servicoSuporte.ObterPorNomeETipo("ÎNGLÈS",(int)TipoSuporte.IMAGEM)).ShouldBe(2);
            
            (await servicoSuporte.ObterPorNomeETipo("GEOGRAFíA",(int)TipoSuporte.IMAGEM)).ShouldBe(3);
            (await servicoSuporte.ObterPorNomeETipo("GEôGRáFíA",(int)TipoSuporte.IMAGEM)).ShouldBe(3);
            (await servicoSuporte.ObterPorNomeETipo("GEÓGRÂFíã",(int)TipoSuporte.IMAGEM)).ShouldBe(3);
            (await servicoSuporte.ObterPorNomeETipo("GÈÓGRÀFÍÂ",(int)TipoSuporte.IMAGEM)).ShouldBe(3);
            
            (await servicoSuporte.ObterPorNomeETipo("MAtemáticA",(int)TipoSuporte.IMAGEM)).ShouldBe(4);
            (await servicoSuporte.ObterPorNomeETipo("MÁTeMÀTIca",(int)TipoSuporte.IMAGEM)).ShouldBe(4);
            (await servicoSuporte.ObterPorNomeETipo("MÃtêmàtíCÂ",(int)TipoSuporte.IMAGEM)).ShouldBe(4);
            (await servicoSuporte.ObterPorNomeETipo("mateMâTícà",(int)TipoSuporte.IMAGEM)).ShouldBe(4);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - Validar Cromia sem caracteres especiais")]
        public async Task Validar_cromia_sem_caracteres_especiais()
        {
            var servicoCromia = GetServicoCromia();
        
            await InserirNaBase(new Cromia() { Nome = "Santuário"});
            await InserirNaBase(new Cromia() { Nome = "Sextânte",});
            await InserirNaBase(new Cromia() { Nome = "Ágape"});
            await InserirNaBase(new Cromia() { Nome = "Pressí"});
            
            (await servicoCromia.ObterPorNome("Santuarió")).ShouldBe(1);
            (await servicoCromia.ObterPorNome("SANTuàRiO")).ShouldBe(1);
            (await servicoCromia.ObterPorNome("SÁntúaRìo")).ShouldBe(1);
            (await servicoCromia.ObterPorNome("SÀnTÚaRió")).ShouldBe(1);
            
            (await servicoCromia.ObterPorNome("SExTanTé")).ShouldBe(2);
            (await servicoCromia.ObterPorNome("SéxTaNtê")).ShouldBe(2);
            (await servicoCromia.ObterPorNome("SEXTAntè")).ShouldBe(2);
            (await servicoCromia.ObterPorNome("sextantÊ")).ShouldBe(2);
            
            (await servicoCromia.ObterPorNome("AGAPÉ")).ShouldBe(3);
            (await servicoCromia.ObterPorNome("aGapÊ")).ShouldBe(3);
            (await servicoCromia.ObterPorNome("ÂgaPÈ")).ShouldBe(3);
            (await servicoCromia.ObterPorNome("ÂgÃpÊ")).ShouldBe(3);
            
            (await servicoCromia.ObterPorNome("PrÉssÌ")).ShouldBe(4);
            (await servicoCromia.ObterPorNome("prÈssÍ")).ShouldBe(4);
            (await servicoCromia.ObterPorNome("pRÊssi")).ShouldBe(4);
            (await servicoCromia.ObterPorNome("pressÍ")).ShouldBe(4);
        }

        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - Deve permitir remover linha do arquivo")]
        public async Task Deve_permitir_remover_a_linha_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoArteGrafica();

            var linhasInseridas = AcervoArteGraficaLinhaMock.GerarAcervoArteGraficaLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            var retorno = await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = 5});
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault(f => f.Id.SaoIguais(1));
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoArteGraficaLinhaDTO>>(arquivo.Conteudo);
            conteudo.Any(a=> a.NumeroLinha.SaoIguais(5)).ShouldBeFalse();
            conteudo.Count().ShouldBe(9);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - Não deve permitir remover linha do arquivo")]
        public async Task Nao_deve_permitir_remover_a_linha_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoArteGrafica();

            var linhasInseridas = AcervoArteGraficaLinhaMock.GerarAcervoArteGraficaLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = 15}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - Não deve permitir remover todas as linhas do arquivo")]
        public async Task Nao_deve_permitir_remover_todas_as_linhas_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoArteGrafica();

            var linhasInseridas = AcervoArteGraficaLinhaMock.GerarAcervoArteGraficaLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            for (int i = 1; i < 10; i++)
                (await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = i})).ShouldBe(true);
                
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoArteGraficaLinhaDTO>>(arquivo.Conteudo);
            conteudo.Count().ShouldBe(1);
            
            await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = 10}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - Deve permitir excluir o arquivo - GERAL")]
        public async Task Deve_permitir_excluir_todo_o_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervo();

            var linhasInseridas = AcervoArteGraficaLinhaMock.GerarAcervoArteGraficaLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

           (await servicoImportacaoArquivo.Excluir(1)).ShouldBe(true);
                
            var arquivo = ObterTodos<ImportacaoArquivo>();
            arquivo.Count().ShouldBe(1);
            arquivo.Count(w=> w.Excluido).ShouldBe(1);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - Deve permitir atualizar uma linha do arquivo para sucesso e outra fica com erro")]
        public async Task Deve_permitir_atualizar_uma_linha_do_arquivo_para_sucesso_e_outra_fica_com_erro()
        {
            await InserirDadosBasicos();
            
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoArteGrafica();

            var linhasInseridas = AcervoArteGraficaLinhaMock.GerarAcervoArteGraficaLinhaDTO().Generate(10);
            linhasInseridas[3].PossuiErros = true;
            linhasInseridas[3].Largura.PossuiErro = true;
            linhasInseridas[3].Largura.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.LARGURA);
            linhasInseridas[3].Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.LARGURA); //Mensagem geral
            
            linhasInseridas[9].PossuiErros = true;
            linhasInseridas[9].Suporte.PossuiErro = true;
            linhasInseridas[9].Suporte.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_NAO_PREENCHIDO, Dominio.Constantes.Constantes.SUPORTE);
            linhasInseridas[9].Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_NAO_PREENCHIDO, Dominio.Constantes.Constantes.SUPORTE); //Mensagem geral

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Status = ImportacaoStatus.Erros,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.AtualizarLinhaParaSucesso(1, new LinhaDTO(){NumeroLinha = 4});
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoArteGraficaLinhaDTO>>(arquivo.Conteudo);
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).PossuiErros.ShouldBeFalse();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).Mensagem.ShouldBeEmpty();
            
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(10)).PossuiErros.ShouldBeTrue();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(10)).Mensagem.ShouldNotBeEmpty();
            
            arquivo.Status.ShouldBe(ImportacaoStatus.Erros);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - Deve permitir atualizar linha do arquivo para sucesso")]
        public async Task Deve_permitir_atualizar_linha_do_arquivo_para_sucesso()
        {
            await InserirDadosBasicos();
            
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoArteGrafica();

            var linhasInseridas = AcervoArteGraficaLinhaMock.GerarAcervoArteGraficaLinhaDTO().Generate(10);
            linhasInseridas[3].PossuiErros = true;
            linhasInseridas[3].Largura.PossuiErro = true;
            linhasInseridas[3].Largura.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.LARGURA);
            linhasInseridas[3].Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.LARGURA); //Mensagem geral
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Status = ImportacaoStatus.Erros,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.AtualizarLinhaParaSucesso(1, new LinhaDTO(){NumeroLinha = 4});
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoArteGraficaLinhaDTO>>(arquivo.Conteudo);
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).PossuiErros.ShouldBeFalse();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).Mensagem.ShouldBeEmpty();
            
            conteudo.Any(a=> a.PossuiErros).ShouldBeFalse();
            conteudo.Any(a=> !a.PossuiErros).ShouldBeTrue();
            
            arquivo.Status.ShouldBe(ImportacaoStatus.Sucesso);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - Validação de RetornoObjeto")]
        public async Task Validacao_retorno_objeto()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoArteGrafica();
           
            var linhasInseridas = AcervoArteGraficaLinhaMock.GerarAcervoArteGraficaLinhaDTO().Generate(9);
            linhasInseridas.Add(new AcervoArteGraficaLinhaDTO()
            {
                PossuiErros = true,
                Titulo = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Localizacao = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Codigo = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Procedencia = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Data = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Ano = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                CopiaDigital = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                PermiteUsoImagem = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Largura = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Altura = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Diametro = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Tecnica = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Quantidade = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Descricao = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                EstadoConservacao = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Cromia = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Suporte = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Credito = new LinhaConteudoAjustarDTO() { PossuiErro = true},
            });
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Status = ImportacaoStatus.Erros,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            var retorno = await servicoImportacaoArquivo.ObterImportacaoPendente();
            foreach (var erro in retorno.Erros)
            {
                erro.RetornoObjeto.Titulo.ShouldBeNull();
                erro.RetornoObjeto.Codigo.ShouldBeNull();
                erro.RetornoObjeto.Localizacao.ShouldBeNull();
                erro.RetornoObjeto.Procedencia.ShouldBeNull();
                erro.RetornoObjeto.Ano.ShouldBeNull();
                erro.RetornoObjeto.DataAcervo.ShouldBeNull();
                erro.RetornoObjeto.CopiaDigital.HasValue.ShouldBeFalse();
                erro.RetornoObjeto.PermiteUsoImagem.HasValue.ShouldBeFalse();
                erro.RetornoObjeto.ConservacaoId.ShouldBeNull();
                erro.RetornoObjeto.CromiaId.ShouldBeNull();
                erro.RetornoObjeto.Largura.ShouldBeNull();
                erro.RetornoObjeto.Altura.ShouldBeNull();
                erro.RetornoObjeto.Diametro.ShouldBeNull();
                erro.RetornoObjeto.Tecnica.ShouldBeNull();
                erro.RetornoObjeto.SuporteId.ShouldBeNull();
                erro.RetornoObjeto.Quantidade.ShouldBeNull();
                erro.RetornoObjeto.Descricao.ShouldBeNull();
                erro.RetornoObjeto.CreditosAutoresIds.ShouldBeNull();
            }
            retorno.ShouldNotBeNull();
        }
    }
}