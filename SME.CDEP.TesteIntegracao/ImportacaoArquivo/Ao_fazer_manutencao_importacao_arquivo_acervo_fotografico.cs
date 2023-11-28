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
    public class Ao_fazer_manutencao_importacao_arquivo_acervo_fotografico : TesteBase
    {
        public Ao_fazer_manutencao_importacao_arquivo_acervo_fotografico(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Importação Arquivo Acervo Fotografico - ValidarPreenchimentoValorFormatoQtdeCaracteres - Com erros: Titulo, Suporte, Resolução, Quantidade e Tombo")]
        public async Task Validar_preenchimento_valor_formato_qtde_caracteres()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoFotografico();

            var acervoFotograficoLinhas = AcervoFotograficoLinhaMock.GerarAcervoFotograficoLinhaDTO().Generate(10);

            acervoFotograficoLinhas[2].Titulo.Conteudo = string.Empty;
            acervoFotograficoLinhas[4].Suporte.Conteudo = string.Empty;
            acervoFotograficoLinhas[5].Resolucao.Conteudo = faker.Lorem.Paragraph();
            acervoFotograficoLinhas[7].Quantidade.Conteudo = faker.Lorem.Paragraph();
            acervoFotograficoLinhas[8].Codigo.Conteudo = string.Empty;
            var linhasComErros = new[] { 3, 5, 6, 8, 9 };
            
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoFotograficoLinhas);

            foreach (var linha in acervoFotograficoLinhas)
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
                    linha.Suporte.PossuiErro.ShouldBeTrue();
                    linha.Suporte.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Suporte.PossuiErro.ShouldBeFalse();
                    linha.Suporte.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha.SaoIguais(6))
                {
                    linha.Resolucao.PossuiErro.ShouldBeTrue();
                    linha.Resolucao.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Resolucao.PossuiErro.ShouldBeFalse();
                    linha.Resolucao.Mensagem.ShouldBeEmpty();
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
                   
                linha.Credito.PossuiErro.ShouldBeFalse();
                linha.Localizacao.PossuiErro.ShouldBeFalse();
                linha.Data.PossuiErro.ShouldBeFalse();
                linha.CopiaDigital.PossuiErro.ShouldBeFalse();
                linha.PermiteUsoImagem.PossuiErro.ShouldBeFalse();
                linha.EstadoConservacao.PossuiErro.ShouldBeFalse();
                linha.Descricao.PossuiErro.ShouldBeFalse();
                linha.Largura.PossuiErro.ShouldBeFalse();
                linha.Altura.PossuiErro.ShouldBeFalse();
                linha.FormatoImagem.PossuiErro.ShouldBeFalse();
                linha.TamanhoArquivo.PossuiErro.ShouldBeFalse();
                linha.Cromia.PossuiErro.ShouldBeFalse();
                linha.Procedencia.PossuiErro.ShouldBeFalse();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Fotografico - ValidacaoObterOuInserirDominios")]
        public async Task Validacao_obter_ou_inserir_dominios()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoFotografico();
        
            var acervoFotograficoLinhas = AcervoFotograficoLinhaMock.GerarAcervoFotograficoLinhaDTO().Generate(10);
           
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoFotograficoLinhas);
        
            foreach (var linha in acervoFotograficoLinhas)
            {
                var creditoAutorInseridos = linha.Credito.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                var creditosAutores = ObterTodos<CreditoAutor>();
                foreach (var creditoAutor in creditoAutorInseridos)
                    creditosAutores.Any(a => a.Nome.SaoIguais(creditoAutor)).ShouldBeTrue();
                
                var cromiaInserido = linha.Cromia.Conteudo;
                var cromias = ObterTodos<Cromia>();
                cromias.Any(a => a.Nome.SaoIguais(cromiaInserido)).ShouldBeTrue();
        
                var suporteInserido = linha.Suporte.Conteudo;
                var suportes = ObterTodos<Suporte>();
                suportes.Any(a => a.Nome.SaoIguais(suporteInserido)).ShouldBeTrue();
        
                var conservacoesInseridas = linha.EstadoConservacao.Conteudo;
                var conservacoes = ObterTodos<Conservacao>();
                conservacoes.Any(a => a.Nome.SaoIguais(conservacoesInseridas)).ShouldBeTrue();
                
                var FormatoInseridas = linha.FormatoImagem.Conteudo;
                var formato = ObterTodos<Formato>();
                formato.Any(a => a.Nome.SaoIguais(FormatoInseridas)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Fotografico - PersistenciaAcervo")]
        public async Task Persistencia_acervo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoFotografico();
        
            var acervoFotograficoLinhas = AcervoFotograficoLinhaMock.GerarAcervoFotograficoLinhaDTO().Generate(10);
        
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
                acervos.Any(a=> a.Titulo.SaoIguais(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.SaoIguais(linhasComSucesso.Codigo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.SaoIguais(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosFotografico.Any(a=> a.SuporteId.SaoIguais(suportes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Suporte.Conteudo)).Id)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.CromiaId.SaoIguais(cromias.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Cromia.Conteudo)).Id)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.ConservacaoId.SaoIguais(conservacoes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.EstadoConservacao.Conteudo)).Id)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.FormatoId.SaoIguais(formatos.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.FormatoImagem.Conteudo)).Id)).ShouldBeTrue();
                
                //Campos livres
                acervosFotografico.Any(a=> a.Localizacao.SaoIguais(linhasComSucesso.Localizacao.Conteudo)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Procedencia.SaoIguais(linhasComSucesso.Procedencia.Conteudo)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.DataAcervo.SaoIguais(linhasComSucesso.Data.Conteudo)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.CopiaDigital.SaoIguais(linhasComSucesso.CopiaDigital.Conteudo.EhOpcaoSim())).ShouldBeTrue();
                acervosFotografico.Any(a=> a.PermiteUsoImagem.SaoIguais(linhasComSucesso.PermiteUsoImagem.Conteudo.EhOpcaoSim())).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Quantidade.SaoIguais(linhasComSucesso.Quantidade.Conteudo.ObterLongoPorValorDoCampo())).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Largura.SaoIguais(linhasComSucesso.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Altura.SaoIguais(linhasComSucesso.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosFotografico.Any(a=> a.TamanhoArquivo.SaoIguais(linhasComSucesso.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Resolucao.SaoIguais(linhasComSucesso.Resolucao.Conteudo)).ShouldBeTrue();
                
                //Crédito
                var creditoAInserir = linhasComSucesso.Credito.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var credito in creditoAInserir)
                    creditoAutores.Any(a=> a.Nome.SaoIguais(credito)).ShouldBeTrue();
                
                foreach (var creditoAutor in acervoCreditoAutors)
                    creditoAutores.Any(a=> a.Id.SaoIguais(creditoAutor.CreditoAutorId)).ShouldBeTrue();
            }
        }

        [Fact(DisplayName = "Importação Arquivo Acervo Fotografico - Geral - Com erros em 4 linhas")]
        public async Task Importacao_geral()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoFotografico();
        
            var acervoFotograficoLinhas = AcervoFotograficoLinhaMock.GerarAcervoFotograficoLinhaDTO().Generate(10);
           
            acervoFotograficoLinhas[1].CopiaDigital.Conteudo = acervoFotograficoLinhas[1].Titulo.Conteudo;
            acervoFotograficoLinhas[3].Descricao.Conteudo = string.Empty;
            acervoFotograficoLinhas[5].TamanhoArquivo.Conteudo = faker.Lorem.Paragraph();
            acervoFotograficoLinhas[7].Codigo.Conteudo = acervoFotograficoLinhas[0].Codigo.Conteudo;
            
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
            await servicoImportacaoArquivo.AtualizarImportacao(1, JsonConvert.SerializeObject(acervoFotograficoLinhas), acervoFotograficoLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);
            var retorno = await servicoImportacaoArquivo.ObterImportacaoPendente();
        
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
            acervos.Count().ShouldBe(6);
            
            //Acervos auxiliares inseridos
            acervosFotografico.ShouldNotBeNull();
            acervosFotografico.Count().ShouldBe(6);
            
            //Linhas com erros
            acervoFotograficoLinhas.Count(w=> !w.PossuiErros).ShouldBe(6);
            acervoFotograficoLinhas.Count(w=> w.PossuiErros).ShouldBe(4);
            
            //Retorno front
            retorno.Id.ShouldBe(1);
            retorno.Nome.ShouldNotBeEmpty();
            retorno.TipoAcervo.ShouldBe(TipoAcervo.Fotografico);
            retorno.DataImportacao.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            
            foreach (var linhaInserida in acervoFotograficoLinhas.Where(w=> !w.PossuiErros))
            {
                retorno.Sucesso.Any(a=> a.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Tombo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.NumeroLinha.SaoIguais(linhaInserida.NumeroLinha)).ShouldBeTrue();
            }
            
            foreach (var linhaInserida in acervoFotograficoLinhas.Where(w=> w.PossuiErros))
            {
                retorno.Erros.Any(a=> a.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Tombo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.NumeroLinha.SaoIguais(linhaInserida.NumeroLinha)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoObjeto.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Codigo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Localizacao.SaoIguais(linhaInserida.Localizacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Procedencia.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.DataAcervo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.ConservacaoId.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Descricao.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Quantidade.SaoIguais(linhaInserida.Quantidade.Conteudo.ObterLongoPorValorDoCampo())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Largura.SaoIguais(linhaInserida.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Altura.SaoIguais(linhaInserida.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.SuporteId.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.FormatoId.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.TamanhoArquivo.SaoIguais(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.CromiaId.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Resolucao.SaoIguais(linhaInserida.Resolucao.Conteudo)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoErro.Titulo.Conteudo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Codigo.Conteudo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CreditosAutoresIds.Conteudo.SaoIguais(linhaInserida.Credito.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Localizacao.Conteudo.SaoIguais(linhaInserida.Localizacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Procedencia.Conteudo.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.DataAcervo.Conteudo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CopiaDigital.Conteudo.SaoIguais(linhaInserida.CopiaDigital.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.PermiteUsoImagem.Conteudo.SaoIguais(linhaInserida.PermiteUsoImagem.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.ConservacaoId.Conteudo.SaoIguais(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Descricao.Conteudo.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Quantidade.Conteudo.SaoIguais(linhaInserida.Quantidade.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Largura.Conteudo.SaoIguais(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Altura.Conteudo.SaoIguais(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.SuporteId.Conteudo.SaoIguais(linhaInserida.Suporte.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.FormatoId.Conteudo.SaoIguais(linhaInserida.FormatoImagem.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.TamanhoArquivo.Conteudo.SaoIguais(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CromiaId.Conteudo.SaoIguais(linhaInserida.Cromia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Resolucao.Conteudo.SaoIguais(linhaInserida.Resolucao.Conteudo)).ShouldBeTrue();
            }
        
            foreach (var linhasComSucesso in acervoFotograficoLinhas.Where(w=> !w.PossuiErros))
            {
                //Acervo
                acervos.Any(a=> a.Titulo.SaoIguais(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.SaoIguais(linhasComSucesso.Codigo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.SaoIguais(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosFotografico.Any(a=> a.SuporteId.SaoIguais(suportes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Suporte.Conteudo)).Id)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.CromiaId.SaoIguais(cromias.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Cromia.Conteudo)).Id)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.ConservacaoId.SaoIguais(conservacoes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.EstadoConservacao.Conteudo)).Id)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.FormatoId.SaoIguais(formatos.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.FormatoImagem.Conteudo)).Id)).ShouldBeTrue();
                
                //Campos livres
                acervosFotografico.Any(a=> a.Localizacao.SaoIguais(linhasComSucesso.Localizacao.Conteudo)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Procedencia.SaoIguais(linhasComSucesso.Procedencia.Conteudo)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.DataAcervo.SaoIguais(linhasComSucesso.Data.Conteudo)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.CopiaDigital.SaoIguais(linhasComSucesso.CopiaDigital.Conteudo.EhOpcaoSim())).ShouldBeTrue();
                acervosFotografico.Any(a=> a.PermiteUsoImagem.SaoIguais(linhasComSucesso.PermiteUsoImagem.Conteudo.EhOpcaoSim())).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Quantidade.SaoIguais(linhasComSucesso.Quantidade.Conteudo.ObterLongoPorValorDoCampo())).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Largura.SaoIguais(linhasComSucesso.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Altura.SaoIguais(linhasComSucesso.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                acervosFotografico.Any(a=> a.TamanhoArquivo.SaoIguais(linhasComSucesso.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                acervosFotografico.Any(a=> a.Resolucao.SaoIguais(linhasComSucesso.Resolucao.Conteudo)).ShouldBeTrue();
                
                //Crédito
                var creditoAInserir = linhasComSucesso.Credito.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var credito in creditoAInserir)
                    creditoAutores.Any(a=> a.Nome.SaoIguais(credito)).ShouldBeTrue();
                
                foreach (var creditoAutor in acervoCreditoAutors)
                    creditoAutores.Any(a=> a.Id.SaoIguais(creditoAutor.CreditoAutorId)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Fotografico - Obter importação pendente com Erros")]
        public async Task Obter_importacao_pendente_com_erros()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoFotografico();
        
            var linhasInseridas = AcervoFotograficoLinhaMock.GerarAcervoFotograficoLinhaDTO().Generate(10);
        
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
            
            await InserirConservacoes(linhasInseridas.Select(s => s.EstadoConservacao.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
            await InserirSuportes(linhasInseridas.Select(s => s.Suporte.Conteudo).Distinct().Where(w=> w.EstaPreenchido()), TipoSuporte.IMAGEM);
            await InserirFormatos(linhasInseridas.Select(s => s.FormatoImagem.Conteudo).Distinct().Where(w=> w.EstaPreenchido()), TipoFormato.ACERVO_FOTOS);
            await InserirCromias(linhasInseridas.Select(s => s.Cromia.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
            await InserirCreditosAutorias(linhasInseridas.Select(s => s.Credito.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct().Where(w=> w.EstaPreenchido()), TipoCreditoAutoria.Credito);

            
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
                retorno.Erros.Any(a=> a.RetornoObjeto.Localizacao.SaoIguais(linhaInserida.Localizacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Procedencia.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.DataAcervo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Descricao.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Quantidade.SaoIguais(linhaInserida.Quantidade.Conteudo.ObterLongoPorValorDoCampo())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Largura.SaoIguais(linhaInserida.Largura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Altura.SaoIguais(linhaInserida.Altura.Conteudo.ObterDoubleOuNuloPorValorDoCampo())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.TamanhoArquivo.SaoIguais(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Resolucao.SaoIguais(linhaInserida.Resolucao.Conteudo)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoErro.Titulo.Conteudo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Codigo.Conteudo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CreditosAutoresIds.Conteudo.SaoIguais(linhaInserida.Credito.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Localizacao.Conteudo.SaoIguais(linhaInserida.Localizacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Procedencia.Conteudo.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.DataAcervo.Conteudo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CopiaDigital.Conteudo.SaoIguais(linhaInserida.CopiaDigital.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.PermiteUsoImagem.Conteudo.SaoIguais(linhaInserida.PermiteUsoImagem.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.ConservacaoId.Conteudo.SaoIguais(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Descricao.Conteudo.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Quantidade.Conteudo.SaoIguais(linhaInserida.Quantidade.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Largura.Conteudo.SaoIguais(linhaInserida.Largura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Altura.Conteudo.SaoIguais(linhaInserida.Altura.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.SuporteId.Conteudo.SaoIguais(linhaInserida.Suporte.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.FormatoId.Conteudo.SaoIguais(linhaInserida.FormatoImagem.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.TamanhoArquivo.Conteudo.SaoIguais(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CromiaId.Conteudo.SaoIguais(linhaInserida.Cromia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Resolucao.Conteudo.SaoIguais(linhaInserida.Resolucao.Conteudo)).ShouldBeTrue();
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
        
        [Fact(DisplayName = "Importação Arquivo Acervo Fotográfico - Deve permitir remover linha do arquivo")]
        public async Task Deve_permitir_remover_a_linha_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoFotografico();

            var linhasInseridas = AcervoFotograficoLinhaMock.GerarAcervoFotograficoLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Fotografico,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            var retorno = await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = 5});
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault(f => f.Id.SaoIguais(1));
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoFotograficoLinhaDTO>>(arquivo.Conteudo);
            conteudo.Any(a=> a.NumeroLinha.SaoIguais(5)).ShouldBeFalse();
            conteudo.Count().ShouldBe(9);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Fotográfico - Não deve permitir remover linha do arquivo")]
        public async Task Nao_deve_permitir_remover_a_linha_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoFotografico();

            var linhasInseridas = AcervoFotograficoLinhaMock.GerarAcervoFotograficoLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Fotografico,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = 15}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Fotográfico - Não deve permitir remover todas as linhas do arquivo")]
        public async Task Nao_deve_permitir_remover_todas_as_linhas_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoFotografico();

            var linhasInseridas = AcervoFotograficoLinhaMock.GerarAcervoFotograficoLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Fotografico,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            for (int i = 1; i < 10; i++)
                (await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = i})).ShouldBe(true);
                
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoFotograficoLinhaDTO>>(arquivo.Conteudo);
            conteudo.Count().ShouldBe(1);
            
            await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = 10}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Fotográfico - Deve permitir atualizar uma linha do arquivo para sucesso e outra fica com erro")]
        public async Task Deve_permitir_atualizar_uma_linha_do_arquivo_para_sucesso_e_outra_fica_com_erro()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoFotografico();

            var linhasInseridas = AcervoFotograficoLinhaMock.GerarAcervoFotograficoLinhaDTO().Generate(10);
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
                TipoAcervo = TipoAcervo.Fotografico,
                Status = ImportacaoStatus.Erros,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.AtualizarLinhaParaSucesso(1, new LinhaDTO(){NumeroLinha = 4});
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoFotograficoLinhaDTO>>(arquivo.Conteudo);
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).PossuiErros.ShouldBeFalse();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).Mensagem.ShouldBeEmpty();
            
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(10)).PossuiErros.ShouldBeTrue();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(10)).Mensagem.ShouldNotBeEmpty();
            
            arquivo.Status.ShouldBe(ImportacaoStatus.Erros);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Fotográfico - Deve permitir atualizar linha do arquivo para sucesso")]
        public async Task Deve_permitir_atualizar_linha_do_arquivo_para_sucesso()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoFotografico();

            var linhasInseridas = AcervoFotograficoLinhaMock.GerarAcervoFotograficoLinhaDTO().Generate(10);
            linhasInseridas[3].PossuiErros = true;
            linhasInseridas[3].Altura.PossuiErro = true;
            linhasInseridas[3].Altura.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.ALTURA);
            linhasInseridas[3].Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.ALTURA); //Mensagem geral
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Fotografico,
                Status = ImportacaoStatus.Erros,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.AtualizarLinhaParaSucesso(1, new LinhaDTO(){NumeroLinha = 4});
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoFotograficoLinhaDTO>>(arquivo.Conteudo);
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).PossuiErros.ShouldBeFalse();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).Mensagem.ShouldBeEmpty();
            
            conteudo.Any(a=> a.PossuiErros).ShouldBeFalse();
            conteudo.Any(a=> !a.PossuiErros).ShouldBeTrue();
            
            arquivo.Status.ShouldBe(ImportacaoStatus.Sucesso);
        }
    }
}