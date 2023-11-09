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
        
        [Fact(DisplayName = "Importação Arquivo Acervo Fotografico - ValidarPreenchimentoValorFormatoQtdeCaracteres - Com erros: Titulo, Suporte, Procedência, Quantidade e Tombo")]
        public async Task Validar_preenchimento_valor_formato_qtde_caracteres()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoFotografico();

            var acervoFotograficoLinhas = GerarAcervoFotograficoLinhaDTO().Generate(10);

            acervoFotograficoLinhas[2].Titulo.Conteudo = string.Empty;
            acervoFotograficoLinhas[4].Suporte.Conteudo = string.Empty;
            acervoFotograficoLinhas[5].Procedencia.Conteudo = faker.Lorem.Paragraph();
            acervoFotograficoLinhas[7].Quantidade.Conteudo = faker.Lorem.Paragraph();
            acervoFotograficoLinhas[8].Tombo.Conteudo = string.Empty;
            var linhasComErros = new[] { 3, 5, 6, 8, 9 };
            
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoFotograficoLinhas);

            foreach (var linha in acervoFotograficoLinhas)
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
                    linha.Suporte.PossuiErro.ShouldBeTrue();
                    linha.Suporte.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Suporte.PossuiErro.ShouldBeFalse();
                    linha.Suporte.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha == 6)
                {
                    linha.Procedencia.PossuiErro.ShouldBeTrue();
                    linha.Procedencia.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Procedencia.PossuiErro.ShouldBeFalse();
                    linha.Procedencia.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha == 8)
                {
                    linha.Quantidade.PossuiErro.ShouldBeTrue();
                    linha.Quantidade.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Quantidade.PossuiErro.ShouldBeFalse();
                    linha.Quantidade.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha == 9)
                {
                    linha.Tombo.PossuiErro.ShouldBeTrue();
                    linha.Tombo.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Tombo.PossuiErro.ShouldBeFalse();
                    linha.Tombo.Mensagem.ShouldBeEmpty();
                }
                   
                linha.Credito.PossuiErro.ShouldBeFalse();
                linha.Localizacao.PossuiErro.ShouldBeFalse();
                linha.Data.PossuiErro.ShouldBeFalse();
                linha.CopiaDigital.PossuiErro.ShouldBeFalse();
                linha.AutorizacaoUsoDeImagem.PossuiErro.ShouldBeFalse();
                linha.EstadoConservacao.PossuiErro.ShouldBeFalse();
                linha.Descricao.PossuiErro.ShouldBeFalse();
                linha.Largura.PossuiErro.ShouldBeFalse();
                linha.Altura.PossuiErro.ShouldBeFalse();
                linha.FormatoImagem.PossuiErro.ShouldBeFalse();
                linha.TamanhoArquivo.PossuiErro.ShouldBeFalse();
                linha.Cromia.PossuiErro.ShouldBeFalse();
                linha.Resolucao.PossuiErro.ShouldBeFalse();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Fotografico - ValidacaoObterOuInserirDominios")]
        public async Task Validacao_obter_ou_inserir_dominios()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoFotografico();
        
            var acervoFotograficoLinhas = GerarAcervoFotograficoLinhaDTO().Generate(10);
           
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoFotograficoLinhas);
        
            foreach (var linha in acervoFotograficoLinhas)
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
                
                var FormatoInseridas = linha.FormatoImagem.Conteudo;
                var formato = ObterTodos<Formato>();
                formato.Any(a => a.Nome.Equals(FormatoInseridas)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Fotografico - PersistenciaAcervo")]
        public async Task Persistencia_acervo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoFotografico();
        
            var acervoFotograficoLinhas = GerarAcervoFotograficoLinhaDTO().Generate(10);
        
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Fotografico,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoFotograficoLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoFotograficoLinhas);
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoFotograficoLinhas);
        
            var acervos = ObterTodos<Acervo>();
            var acervosFotografico = ObterTodos<AcervoFotografico>();
            var suportes = ObterTodos<Suporte>();
            var cromias = ObterTodos<Cromia>();
            var acervoCreditoAutors = ObterTodos<AcervoCreditoAutor>();
            var creditoAutores = ObterTodos<CreditoAutor>();
            var conservacoes = ObterTodos<Conservacao>();
            var formatos = ObterTodos<Formato>();
            
            //Acervos inseridos
            acervos.ShouldNotBeNull();
            acervos.Count().ShouldBe(10);
            
            //Acervos auxiliares inseridos
            acervosFotografico.ShouldNotBeNull();
            acervosFotografico.Count().ShouldBe(10);
            
            //Linhas com erros
            acervoFotograficoLinhas.Count(w=> !w.PossuiErros).ShouldBe(10);
            acervoFotograficoLinhas.Count(w=> w.PossuiErros).ShouldBe(0);
        
            foreach (var linhasComSucesso in acervoFotograficoLinhas.Where(w=> !w.PossuiErros))
            {
                //Acervo
                acervos.Any(a=> a.Titulo.Equals(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.Equals(linhasComSucesso.Tombo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.Equals(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosFotografico.Any(a=> a.SuporteId == suportes.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Suporte.Conteudo)).Id).ShouldBeTrue();
                acervosFotografico.Any(a=> a.CromiaId == cromias.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Cromia.Conteudo)).Id).ShouldBeTrue();
                acervosFotografico.Any(a=> a.ConservacaoId == conservacoes.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.EstadoConservacao.Conteudo)).Id).ShouldBeTrue();
                acervosFotografico.Any(a=> a.FormatoId == formatos.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.FormatoImagem.Conteudo)).Id).ShouldBeTrue();
                
                //Campos livres
                acervosFotografico.Any(a=> a.Localizacao == linhasComSucesso.Localizacao.Conteudo).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Procedencia == linhasComSucesso.Procedencia.Conteudo).ShouldBeTrue();
                acervosFotografico.Any(a=> a.DataAcervo == linhasComSucesso.Data.Conteudo).ShouldBeTrue();
                acervosFotografico.Any(a=> a.CopiaDigital == linhasComSucesso.CopiaDigital.Conteudo.Equals("Sim")).ShouldBeTrue();
                acervosFotografico.Any(a=> a.PermiteUsoImagem == linhasComSucesso.AutorizacaoUsoDeImagem.Conteudo.Equals("Sim")).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Quantidade == long.Parse(linhasComSucesso.Quantidade.Conteudo)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Largura == double.Parse(linhasComSucesso.Largura.Conteudo)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Altura == double.Parse(linhasComSucesso.Altura.Conteudo)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.TamanhoArquivo == linhasComSucesso.TamanhoArquivo.Conteudo).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Resolucao == linhasComSucesso.Resolucao.Conteudo).ShouldBeTrue();
                
                //Crédito
                var creditoAInserir = linhasComSucesso.Credito.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var credito in creditoAInserir)
                    creditoAutores.Any(a=> a.Nome.Equals(credito)).ShouldBeTrue();
                
                foreach (var creditoAutor in acervoCreditoAutors)
                    creditoAutores.Any(a=> a.Id == creditoAutor.CreditoAutorId).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Fotografico - Geral - Com erros em 3 linhas")]
        public async Task Importacao_geral()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoFotografico();
        
            var acervoFotograficoLinhas = GerarAcervoFotograficoLinhaDTO().Generate(10);
           
            acervoFotograficoLinhas[3].Descricao.Conteudo = string.Empty;
            acervoFotograficoLinhas[5].TamanhoArquivo.Conteudo = faker.Lorem.Paragraph();
            acervoFotograficoLinhas[7].Tombo.Conteudo = acervoFotograficoLinhas[0].Tombo.Conteudo;
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Fotografico,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoFotograficoLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoFotograficoLinhas);
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoFotograficoLinhas );
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoFotograficoLinhas);
        
            var acervos = ObterTodos<Acervo>();
            var acervosFotografico = ObterTodos<AcervoFotografico>();
            var suportes = ObterTodos<Suporte>();
            var cromias = ObterTodos<Cromia>();
            var acervoCreditoAutors = ObterTodos<AcervoCreditoAutor>();
            var creditoAutores = ObterTodos<CreditoAutor>();
            var conservacoes = ObterTodos<Conservacao>();
            var formatos = ObterTodos<Formato>();
            
            //Acervos inseridos
            acervos.ShouldNotBeNull();
            acervos.Count().ShouldBe(7);
            
            //Acervos auxiliares inseridos
            acervosFotografico.ShouldNotBeNull();
            acervosFotografico.Count().ShouldBe(7);
            
            //Linhas com erros
            acervoFotograficoLinhas.Count(w=> !w.PossuiErros).ShouldBe(7);
            acervoFotograficoLinhas.Count(w=> w.PossuiErros).ShouldBe(3);
        
            foreach (var linhasComSucesso in acervoFotograficoLinhas.Where(w=> !w.PossuiErros))
            {
                //Acervo
                acervos.Any(a=> a.Titulo.Equals(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.Equals(linhasComSucesso.Tombo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.Equals(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosFotografico.Any(a=> a.SuporteId == suportes.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Suporte.Conteudo)).Id).ShouldBeTrue();
                acervosFotografico.Any(a=> a.CromiaId == cromias.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Cromia.Conteudo)).Id).ShouldBeTrue();
                acervosFotografico.Any(a=> a.ConservacaoId == conservacoes.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.EstadoConservacao.Conteudo)).Id).ShouldBeTrue();
                acervosFotografico.Any(a=> a.FormatoId == formatos.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.FormatoImagem.Conteudo)).Id).ShouldBeTrue();
                
                //Campos livres
                acervosFotografico.Any(a=> a.Localizacao == linhasComSucesso.Localizacao.Conteudo).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Procedencia == linhasComSucesso.Procedencia.Conteudo).ShouldBeTrue();
                acervosFotografico.Any(a=> a.DataAcervo == linhasComSucesso.Data.Conteudo).ShouldBeTrue();
                acervosFotografico.Any(a=> a.CopiaDigital == linhasComSucesso.CopiaDigital.Conteudo.Equals("Sim")).ShouldBeTrue();
                acervosFotografico.Any(a=> a.PermiteUsoImagem == linhasComSucesso.AutorizacaoUsoDeImagem.Conteudo.Equals("Sim")).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Quantidade == long.Parse(linhasComSucesso.Quantidade.Conteudo)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Largura == double.Parse(linhasComSucesso.Largura.Conteudo)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Altura == double.Parse(linhasComSucesso.Altura.Conteudo)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.TamanhoArquivo == linhasComSucesso.TamanhoArquivo.Conteudo).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Resolucao == linhasComSucesso.Resolucao.Conteudo).ShouldBeTrue();
                
                //Crédito
                var creditoAInserir = linhasComSucesso.Credito.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var credito in creditoAInserir)
                    creditoAutores.Any(a=> a.Nome.Equals(credito)).ShouldBeTrue();
                
                foreach (var creditoAutor in acervoCreditoAutors)
                    creditoAutores.Any(a=> a.Id == creditoAutor.CreditoAutorId).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Fotografico - Obter importação pendente com Erros")]
        public async Task Obter_importacao_pendente_com_erros()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoFotografico();
        
            var linhasInseridas = GerarAcervoFotograficoLinhaDTO().Generate(10);
        
            linhasInseridas[3].PossuiErros = true;
            linhasInseridas[3].TamanhoArquivo.PossuiErro = true;
            linhasInseridas[3].TamanhoArquivo.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_ATINGIU_LIMITE_CARACTERES, Dominio.Constantes.Constantes.TAMANHO_ARQUIVO);
            
            linhasInseridas[9].PossuiErros = true;
            linhasInseridas[9].Suporte.PossuiErro = true;
            linhasInseridas[9].Suporte.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_NAO_PREENCHIDO, Dominio.Constantes.Constantes.SUPORTE);
           
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Fotografico,
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
                retorno.Sucesso.Any(a=> a.Descricao.Conteudo.Equals(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Quantidade.Conteudo.Equals(linhaInserida.Quantidade.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Largura.Conteudo.Equals(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Altura.Conteudo.Equals(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Suporte.Conteudo.Equals(linhaInserida.Suporte.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.FormatoImagem.Conteudo.Equals(linhaInserida.FormatoImagem.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.TamanhoArquivo.Conteudo.Equals(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Cromia.Conteudo.Equals(linhaInserida.Cromia.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Resolucao.Conteudo.Equals(linhaInserida.Resolucao.Conteudo)).ShouldBeTrue();
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
                retorno.Erros.Any(a=> a.Descricao.Conteudo.Equals(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Quantidade.Conteudo.Equals(linhaInserida.Quantidade.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Largura.Conteudo.Equals(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Altura.Conteudo.Equals(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Suporte.Conteudo.Equals(linhaInserida.Suporte.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.FormatoImagem.Conteudo.Equals(linhaInserida.FormatoImagem.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.TamanhoArquivo.Conteudo.Equals(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Cromia.Conteudo.Equals(linhaInserida.Cromia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Resolucao.Conteudo.Equals(linhaInserida.Resolucao.Conteudo)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Fotografico - Validar Formatos sem caracteres especiais")]
        public async Task Validar_formato_sem_caracteres_especiais()
        {
            var servicoFormato = GetServicoFormato();
        
            await InserirNaBase(new Formato() { Nome = "Português", Tipo = TipoFormato.ACERVO_FOTOS});
            await InserirNaBase(new Formato() { Nome = "Inglês", Tipo = TipoFormato.ACERVO_FOTOS });
            await InserirNaBase(new Formato() { Nome = "Geografia", Tipo = TipoFormato.ACERVO_FOTOS });
            await InserirNaBase(new Formato() { Nome = "Matemática", Tipo = TipoFormato.ACERVO_FOTOS});
            
            (await servicoFormato.ObterPorNomeETipo("PorTuGueS",(int)TipoFormato.ACERVO_FOTOS)).ShouldBe(1);
            (await servicoFormato.ObterPorNomeETipo("PORTUGUES",(int)TipoFormato.ACERVO_FOTOS)).ShouldBe(1);
            (await servicoFormato.ObterPorNomeETipo("PóRTÚGUEs",(int)TipoFormato.ACERVO_FOTOS)).ShouldBe(1);
            (await servicoFormato.ObterPorNomeETipo("PôRTUGüES",(int)TipoFormato.ACERVO_FOTOS)).ShouldBe(1);
            
            (await servicoFormato.ObterPorNomeETipo("InGlêS",(int)TipoFormato.ACERVO_FOTOS)).ShouldBe(2);
            (await servicoFormato.ObterPorNomeETipo("ÍNGLÉS",(int)TipoFormato.ACERVO_FOTOS)).ShouldBe(2);
            (await servicoFormato.ObterPorNomeETipo("ÎNGLÉS",(int)TipoFormato.ACERVO_FOTOS)).ShouldBe(2);
            (await servicoFormato.ObterPorNomeETipo("ÎNGLÈS",(int)TipoFormato.ACERVO_FOTOS)).ShouldBe(2);
            
            (await servicoFormato.ObterPorNomeETipo("GEOGRAFíA",(int)TipoFormato.ACERVO_FOTOS)).ShouldBe(3);
            (await servicoFormato.ObterPorNomeETipo("GEôGRáFíA",(int)TipoFormato.ACERVO_FOTOS)).ShouldBe(3);
            (await servicoFormato.ObterPorNomeETipo("GEÓGRÂFíã",(int)TipoFormato.ACERVO_FOTOS)).ShouldBe(3);
            (await servicoFormato.ObterPorNomeETipo("GÈÓGRÀFÍÂ",(int)TipoFormato.ACERVO_FOTOS)).ShouldBe(3);
            
            (await servicoFormato.ObterPorNomeETipo("MAtemáticA",(int)TipoFormato.ACERVO_FOTOS)).ShouldBe(4);
            (await servicoFormato.ObterPorNomeETipo("MÁTeMÀTIca",(int)TipoFormato.ACERVO_FOTOS)).ShouldBe(4);
            (await servicoFormato.ObterPorNomeETipo("MÃtêmàtíCÂ",(int)TipoFormato.ACERVO_FOTOS)).ShouldBe(4);
            (await servicoFormato.ObterPorNomeETipo("mateMâTícà",(int)TipoFormato.ACERVO_FOTOS)).ShouldBe(4);
        }
    }
}