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
    public class Ao_fazer_manutencao_importacao_arquivo_acervo_arte_grafica : TesteBase
    {
        public Ao_fazer_manutencao_importacao_arquivo_acervo_arte_grafica(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - ValidarPreenchimentoValorFormatoQtdeCaracteres - Com erros: Titulo, Cromia, Largura, Diâmetro e Tombo")]
        public async Task Validar_preenchimento_valor_formato_qtde_caracteres()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoArteGrafica();

            var acervoArteGraficaLinhas = GerarAcervoArteGraficaLinhaDTO().Generate(10);

            acervoArteGraficaLinhas[2].Titulo.Conteudo = string.Empty;
            acervoArteGraficaLinhas[4].Cromia.Conteudo = string.Empty;
            acervoArteGraficaLinhas[5].Largura.Conteudo = faker.Lorem.Paragraph();
            acervoArteGraficaLinhas[7].Diametro.Conteudo = faker.Lorem.Paragraph();
            acervoArteGraficaLinhas[8].Quantidade.Conteudo = faker.Lorem.Paragraph();
            var linhasComErros = new[] { 3, 5, 6, 8, 9 };
            
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoArteGraficaLinhas);

            foreach (var linha in acervoArteGraficaLinhas)
            {
                linha.PossuiErros.ShouldBe(linhasComErros.Any(a=> a == linha.NumeroLinha));

                if (linha.NumeroLinha == 3)
                {
                    linha.Titulo.PossuiErro.ShouldBeTrue();
                    linha.Titulo.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Titulo.PossuiErro.ShouldBeFalse();
                    linha.Titulo.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha == 5)
                {
                    linha.Cromia.PossuiErro.ShouldBeTrue();
                    linha.Cromia.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Cromia.PossuiErro.ShouldBeFalse();
                    linha.Cromia.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha == 6)
                {
                    linha.Largura.PossuiErro.ShouldBeTrue();
                    linha.Largura.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Largura.PossuiErro.ShouldBeFalse();
                    linha.Largura.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha == 8)
                {
                    linha.Diametro.PossuiErro.ShouldBeTrue();
                    linha.Diametro.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Diametro.PossuiErro.ShouldBeFalse();
                    linha.Diametro.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha == 9)
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
                linha.Data.PossuiErro.ShouldBeFalse();
                linha.AutorizacaoUsoDeImagem.PossuiErro.ShouldBeFalse();
                linha.EstadoConservacao.PossuiErro.ShouldBeFalse();
                linha.Altura.PossuiErro.ShouldBeFalse();
                linha.Tecnica.PossuiErro.ShouldBeFalse();
                linha.Suporte.PossuiErro.ShouldBeFalse();
                linha.Tombo.PossuiErro.ShouldBeFalse();
                linha.Descricao.PossuiErro.ShouldBeFalse();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - ValidacaoObterOuInserirDominios")]
        public async Task Validacao_obter_ou_inserir_dominios()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoArteGrafica();
        
            var acervoArteGraficaLinhas = GerarAcervoArteGraficaLinhaDTO().Generate(10);
           
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoArteGraficaLinhas);
        
            foreach (var linha in acervoArteGraficaLinhas)
            {
                var creditoAutorInseridos = linha.Credito.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                var creditosAutores = ObterTodos<CreditoAutor>();
                foreach (var creditoAutor in creditoAutorInseridos)
                    creditosAutores.Any(a => a.Nome.Equals(creditoAutor)).ShouldBeTrue();
                
                var cromiaInserido = linha.Cromia.Conteudo;
                var cromias = ObterTodos<Cromia>();
                cromias.Any(a => a.Nome.Equals(cromiaInserido)).ShouldBeTrue();
        
                var suporteInserido = linha.Suporte.Conteudo;
                var suportes = ObterTodos<Suporte>();
                suportes.Any(a => a.Nome.Equals(suporteInserido)).ShouldBeTrue();
        
                var conservacoesInseridas = linha.EstadoConservacao.Conteudo;
                var conservacoes = ObterTodos<Conservacao>();
                conservacoes.Any(a => a.Nome.Equals(conservacoesInseridas)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - PersistenciaAcervo")]
        public async Task Persistencia_acervo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoArteGrafica();
        
            var acervoArteGraficaLinhas = GerarAcervoArteGraficaLinhaDTO().Generate(10);
        
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoArteGraficaLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoArteGraficaLinhas);
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoArteGraficaLinhas);
        
            var acervos = ObterTodos<Acervo>();
            var acervosArtesGraficas = ObterTodos<AcervoArteGrafica>();
            var suportes = ObterTodos<Suporte>();
            var cromias = ObterTodos<Cromia>();
            var acervoCreditoAutors = ObterTodos<AcervoCreditoAutor>();
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
                acervos.Any(a=> a.Titulo.Equals(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.Equals(linhasComSucesso.Tombo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.Equals(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosArtesGraficas.Any(a=> a.SuporteId == suportes.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Suporte.Conteudo)).Id).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.CromiaId == cromias.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Cromia.Conteudo)).Id).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.ConservacaoId == conservacoes.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.EstadoConservacao.Conteudo)).Id).ShouldBeTrue();
                
                //Campos livres
                acervosArtesGraficas.Any(a=> a.Localizacao == linhasComSucesso.Localizacao.Conteudo).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Procedencia == linhasComSucesso.Procedencia.Conteudo).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.DataAcervo == linhasComSucesso.Data.Conteudo).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.CopiaDigital == linhasComSucesso.CopiaDigital.Conteudo.Equals("Sim")).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.PermiteUsoImagem == linhasComSucesso.AutorizacaoUsoDeImagem.Conteudo.Equals("Sim")).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Altura == double.Parse(linhasComSucesso.Altura.Conteudo)).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Largura == double.Parse(linhasComSucesso.Largura.Conteudo)).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Diametro == double.Parse(linhasComSucesso.Diametro.Conteudo)).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Quantidade == long.Parse(linhasComSucesso.Quantidade.Conteudo)).ShouldBeTrue();
                
                //Crédito
                var creditoAInserir = linhasComSucesso.Credito.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var credito in creditoAInserir)
                    creditoAutores.Any(a=> a.Nome.Equals(credito)).ShouldBeTrue();
                
                foreach (var creditoAutor in acervoCreditoAutors)
                    creditoAutores.Any(a=> a.Id == creditoAutor.CreditoAutorId).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - Geral - Com erros em 3 linhas")]
        public async Task Importacao_geral()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoArteGrafica();
        
            var acervoArteGraficaLinhas = GerarAcervoArteGraficaLinhaDTO().Generate(10);
           
            acervoArteGraficaLinhas[3].Largura.Conteudo = "ABC3512";
            acervoArteGraficaLinhas[5].Altura.Conteudo = "1212ABC";
            acervoArteGraficaLinhas[7].Tombo.Conteudo = acervoArteGraficaLinhas[0].Tombo.Conteudo;
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.ArtesGraficas,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoArteGraficaLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoArteGraficaLinhas);
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoArteGraficaLinhas );
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoArteGraficaLinhas);
        
             var acervos = ObterTodos<Acervo>();
            var acervosArtesGraficas = ObterTodos<AcervoArteGrafica>();
            var suportes = ObterTodos<Suporte>();
            var cromias = ObterTodos<Cromia>();
            var acervoCreditoAutors = ObterTodos<AcervoCreditoAutor>();
            var creditoAutores = ObterTodos<CreditoAutor>();
            var conservacoes = ObterTodos<Conservacao>();
            
            //Acervos inseridos
            acervos.ShouldNotBeNull();
            acervos.Count().ShouldBe(7);
            
            //Acervos auxiliares inseridos
            acervosArtesGraficas.ShouldNotBeNull();
            acervosArtesGraficas.Count().ShouldBe(7);
            
            //Linhas com erros
            acervoArteGraficaLinhas.Count(w=> !w.PossuiErros).ShouldBe(7);
            acervoArteGraficaLinhas.Count(w=> w.PossuiErros).ShouldBe(3);
        
            foreach (var linhasComSucesso in acervoArteGraficaLinhas.Where(w=> !w.PossuiErros))
            {
                //Acervo
                acervos.Any(a=> a.Titulo.Equals(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.Equals(linhasComSucesso.Tombo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.Equals(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosArtesGraficas.Any(a=> a.SuporteId == suportes.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Suporte.Conteudo)).Id).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.CromiaId == cromias.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Cromia.Conteudo)).Id).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.ConservacaoId == conservacoes.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.EstadoConservacao.Conteudo)).Id).ShouldBeTrue();
                
                //Campos livres
                acervosArtesGraficas.Any(a=> a.Localizacao == linhasComSucesso.Localizacao.Conteudo).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Procedencia == linhasComSucesso.Procedencia.Conteudo).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.DataAcervo == linhasComSucesso.Data.Conteudo).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.CopiaDigital == linhasComSucesso.CopiaDigital.Conteudo.Equals("Sim")).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.PermiteUsoImagem == linhasComSucesso.AutorizacaoUsoDeImagem.Conteudo.Equals("Sim")).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Altura == double.Parse(linhasComSucesso.Altura.Conteudo)).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Largura == double.Parse(linhasComSucesso.Largura.Conteudo)).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Diametro == double.Parse(linhasComSucesso.Diametro.Conteudo)).ShouldBeTrue();
                acervosArtesGraficas.Any(a=> a.Quantidade == long.Parse(linhasComSucesso.Quantidade.Conteudo)).ShouldBeTrue();
                
                //Crédito
                var creditoAInserir = linhasComSucesso.Credito.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var credito in creditoAInserir)
                    creditoAutores.Any(a=> a.Nome.Equals(credito)).ShouldBeTrue();
                
                foreach (var creditoAutor in acervoCreditoAutors)
                    creditoAutores.Any(a=> a.Id == creditoAutor.CreditoAutorId).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Arte Grafica - Obter importação pendente com Erros")]
        public async Task Obter_importacao_pendente_com_erros()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoArteGrafica();
        
            var linhasInseridas = GerarAcervoArteGraficaLinhaDTO().Generate(10);
        
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
            
            var retorno = await servicoImportacaoArquivo.ObterImportacaoPendente();
            retorno.ShouldNotBeNull();
            retorno.Sucesso.Count().ShouldBe(8);
            retorno.Erros.Count().ShouldBe(2);
        
            foreach (var linhaInserida in linhasInseridas.Where(w=> !w.PossuiErros))
            {
                retorno.Sucesso.Any(a=> a.Titulo.Conteudo.Equals(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Tombo.Conteudo.Equals(linhaInserida.Tombo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Credito.Conteudo.Equals(linhaInserida.Credito.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Localizacao.Conteudo.Equals(linhaInserida.Localizacao.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Procedencia.Conteudo.Equals(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Data.Conteudo.Equals(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.CopiaDigital.Conteudo.Equals(linhaInserida.CopiaDigital.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.AutorizacaoUsoDeImagem.Conteudo.Equals(linhaInserida.AutorizacaoUsoDeImagem.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.EstadoConservacao.Conteudo.Equals(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Cromia.Conteudo.Equals(linhaInserida.Cromia.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Largura.Conteudo.Equals(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Altura.Conteudo.Equals(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Diametro.Conteudo.Equals(linhaInserida.Diametro.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Tecnica.Conteudo.Equals(linhaInserida.Tecnica.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Suporte.Conteudo.Equals(linhaInserida.Suporte.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Quantidade.Conteudo.Equals(linhaInserida.Quantidade.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Descricao.Conteudo.Equals(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
            }
            
            foreach (var linhaInserida in linhasInseridas.Where(w=> w.PossuiErros))
            {
                retorno.Erros.Any(a=> a.Titulo.Conteudo.Equals(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Tombo.Conteudo.Equals(linhaInserida.Tombo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Credito.Conteudo.Equals(linhaInserida.Credito.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Localizacao.Conteudo.Equals(linhaInserida.Localizacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Procedencia.Conteudo.Equals(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Data.Conteudo.Equals(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.CopiaDigital.Conteudo.Equals(linhaInserida.CopiaDigital.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.AutorizacaoUsoDeImagem.Conteudo.Equals(linhaInserida.AutorizacaoUsoDeImagem.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.EstadoConservacao.Conteudo.Equals(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Cromia.Conteudo.Equals(linhaInserida.Cromia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Largura.Conteudo.Equals(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Altura.Conteudo.Equals(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Diametro.Conteudo.Equals(linhaInserida.Diametro.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Tecnica.Conteudo.Equals(linhaInserida.Tecnica.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Suporte.Conteudo.Equals(linhaInserida.Suporte.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Quantidade.Conteudo.Equals(linhaInserida.Quantidade.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Descricao.Conteudo.Equals(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
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
    }
}