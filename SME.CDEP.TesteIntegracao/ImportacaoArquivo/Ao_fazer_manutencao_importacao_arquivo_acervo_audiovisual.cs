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
    public class Ao_fazer_manutencao_importacao_arquivo_acervo_audiovisual : TesteBase
    {
        public Ao_fazer_manutencao_importacao_arquivo_acervo_audiovisual(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Importação Arquivo Acervo Audiovisual - ValidarPreenchimentoValorFormatoQtdeCaracteres - Com erros: Titulo, Suporte, Duracao, Cópia e Tombo")]
        public async Task Validar_preenchimento_valor_formato_qtde_caracteres()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoAudiovisual();

            var acervoAudiovisualLinhas = GerarAcervoAudiovisualLinhaDTO().Generate(10);

            acervoAudiovisualLinhas[2].Titulo.Conteudo = string.Empty;
            acervoAudiovisualLinhas[4].Suporte.Conteudo = string.Empty;
            acervoAudiovisualLinhas[5].Duracao.Conteudo = faker.Lorem.Paragraph();
            acervoAudiovisualLinhas[7].Copia.Conteudo = faker.Lorem.Paragraph();
            acervoAudiovisualLinhas[8].Tombo.Conteudo = string.Empty;
            var linhasComErros = new[] { 3, 5, 6, 8, 9 };
            
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoAudiovisualLinhas);

            foreach (var linha in acervoAudiovisualLinhas)
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
                    linha.Duracao.PossuiErro.ShouldBeTrue();
                    linha.Duracao.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Duracao.PossuiErro.ShouldBeFalse();
                    linha.Duracao.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha.SaoIguais(8))
                {
                    linha.Copia.PossuiErro.ShouldBeTrue();
                    linha.Copia.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Copia.PossuiErro.ShouldBeFalse();
                    linha.Copia.Mensagem.ShouldBeEmpty();
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
                
                linha.Credito.PossuiErro.ShouldBeFalse();
                linha.Localizacao.PossuiErro.ShouldBeFalse();
                linha.Procedencia.PossuiErro.ShouldBeFalse();
                linha.Data.PossuiErro.ShouldBeFalse();
                linha.AutorizacaoUsoDeImagem.PossuiErro.ShouldBeFalse();
                linha.EstadoConservacao.PossuiErro.ShouldBeFalse();
                linha.Descricao.PossuiErro.ShouldBeFalse();
                linha.Cromia.PossuiErro.ShouldBeFalse();
                linha.TamanhoArquivo.PossuiErro.ShouldBeFalse();
                linha.Acessibilidade.PossuiErro.ShouldBeFalse();
                linha.Disponibilizacao.PossuiErro.ShouldBeFalse();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Audiovisual - ValidacaoObterOuInserirDominios")]
        public async Task Validacao_obter_ou_inserir_dominios()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoAudiovisual();
        
            var acervoAudiovisualLinhas = GerarAcervoAudiovisualLinhaDTO().Generate(10);
           
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoAudiovisualLinhas);
        
            foreach (var linha in acervoAudiovisualLinhas)
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
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Audiovisual - PersistenciaAcervo")]
        public async Task Persistencia_acervo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoAudiovisual();
        
            var acervoAudiovisualLinhas = GerarAcervoAudiovisualLinhaDTO().Generate(10);
        
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Audiovisual,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoAudiovisualLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoAudiovisualLinhas);
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoAudiovisualLinhas);
        
            var acervos = ObterTodos<Acervo>();
            var acervosAudiovisual = ObterTodos<AcervoAudiovisual>();
            var suportes = ObterTodos<Suporte>();
            var cromias = ObterTodos<Cromia>();
            var acervoCreditoAutors = ObterTodos<AcervoCreditoAutor>();
            var creditoAutores = ObterTodos<CreditoAutor>();
            var conservacoes = ObterTodos<Conservacao>();
            
            //Acervos inseridos
            acervos.ShouldNotBeNull();
            acervos.Count().ShouldBe(10);
            
            //Acervos auxiliares inseridos
            acervosAudiovisual.ShouldNotBeNull();
            acervosAudiovisual.Count().ShouldBe(10);
            
            //Linhas com erros
            acervoAudiovisualLinhas.Count(w=> !w.PossuiErros).ShouldBe(10);
            acervoAudiovisualLinhas.Count(w=> w.PossuiErros).ShouldBe(0);
        
            foreach (var linhasComSucesso in acervoAudiovisualLinhas.Where(w=> !w.PossuiErros))
            {
                //Acervo
                acervos.Any(a=> a.Titulo.SaoIguais(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.SaoIguais(linhasComSucesso.Tombo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.SaoIguais(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosAudiovisual.Any(a=> a.SuporteId.SaoIguais(suportes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Suporte.Conteudo)).Id)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.CromiaId.SaoIguais(cromias.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Cromia.Conteudo)).Id)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.ConservacaoId.SaoIguais(conservacoes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.EstadoConservacao.Conteudo)).Id)).ShouldBeTrue();
                
                //Campos livres
                acervosAudiovisual.Any(a=> a.Localizacao.SaoIguais(linhasComSucesso.Localizacao.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Procedencia.SaoIguais(linhasComSucesso.Procedencia.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.DataAcervo.SaoIguais(linhasComSucesso.Data.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Copia.SaoIguais(linhasComSucesso.Copia.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.PermiteUsoImagem.SaoIguais(linhasComSucesso.AutorizacaoUsoDeImagem.Conteudo.EhOpcaoSim())).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.TamanhoArquivo.SaoIguais(linhasComSucesso.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Acessibilidade.SaoIguais(linhasComSucesso.Acessibilidade.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Disponibilizacao.SaoIguais(linhasComSucesso.Disponibilizacao.Conteudo)).ShouldBeTrue();
                
                //Crédito
                var creditoAInserir = linhasComSucesso.Credito.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var credito in creditoAInserir)
                    creditoAutores.Any(a=> a.Nome.SaoIguais(credito)).ShouldBeTrue();
                
                foreach (var creditoAutor in acervoCreditoAutors)
                    creditoAutores.Any(a=> a.Id.SaoIguais(creditoAutor.CreditoAutorId)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Audiovisual - Geral - Com erros em 3 linhas")]
        public async Task Importacao_geral()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoAudiovisual();
        
            var acervoAudiovisualLinhas = GerarAcervoAudiovisualLinhaDTO().Generate(10);
           
            acervoAudiovisualLinhas[3].Descricao.Conteudo = string.Empty;
            acervoAudiovisualLinhas[5].Duracao.Conteudo = faker.Lorem.Paragraph();
            acervoAudiovisualLinhas[7].Tombo.Conteudo = acervoAudiovisualLinhas[0].Tombo.Conteudo;
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Audiovisual,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoAudiovisualLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoAudiovisualLinhas);
            await servicoImportacaoArquivo.ValidacaoObterOuInserirDominios(acervoAudiovisualLinhas );
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoAudiovisualLinhas);
        
             var acervos = ObterTodos<Acervo>();
            var acervosAudiovisual = ObterTodos<AcervoAudiovisual>();
            var suportes = ObterTodos<Suporte>();
            var cromias = ObterTodos<Cromia>();
            var acervoCreditoAutors = ObterTodos<AcervoCreditoAutor>();
            var creditoAutores = ObterTodos<CreditoAutor>();
            var conservacoes = ObterTodos<Conservacao>();
            
            //Acervos inseridos
            acervos.ShouldNotBeNull();
            acervos.Count().ShouldBe(7);
            
            //Acervos auxiliares inseridos
            acervosAudiovisual.ShouldNotBeNull();
            acervosAudiovisual.Count().ShouldBe(7);
            
            //Linhas com erros
            acervoAudiovisualLinhas.Count(w=> !w.PossuiErros).ShouldBe(7);
            acervoAudiovisualLinhas.Count(w=> w.PossuiErros).ShouldBe(3);
        
            foreach (var linhasComSucesso in acervoAudiovisualLinhas.Where(w=> !w.PossuiErros))
            {
                //Acervo
                acervos.Any(a=> a.Titulo.SaoIguais(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.SaoIguais(linhasComSucesso.Tombo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.SaoIguais(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosAudiovisual.Any(a=> a.SuporteId.SaoIguais(suportes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Suporte.Conteudo)).Id)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.CromiaId.SaoIguais(cromias.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Cromia.Conteudo)).Id)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.ConservacaoId.SaoIguais(conservacoes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.EstadoConservacao.Conteudo)).Id)).ShouldBeTrue();
                
                //Campos livres
                acervosAudiovisual.Any(a=> a.Localizacao.SaoIguais(linhasComSucesso.Localizacao.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Procedencia.SaoIguais(linhasComSucesso.Procedencia.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.DataAcervo.SaoIguais(linhasComSucesso.Data.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Copia.SaoIguais(linhasComSucesso.Copia.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.PermiteUsoImagem.SaoIguais(linhasComSucesso.AutorizacaoUsoDeImagem.Conteudo.EhOpcaoSim())).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.TamanhoArquivo.SaoIguais(linhasComSucesso.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Acessibilidade.SaoIguais(linhasComSucesso.Acessibilidade.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Disponibilizacao.SaoIguais(linhasComSucesso.Disponibilizacao.Conteudo)).ShouldBeTrue();
                
                //Crédito
                var creditoAInserir = linhasComSucesso.Credito.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var credito in creditoAInserir)
                    creditoAutores.Any(a=> a.Nome.SaoIguais(credito)).ShouldBeTrue();
                
                foreach (var creditoAutor in acervoCreditoAutors)
                    creditoAutores.Any(a=> a.Id.SaoIguais(creditoAutor.CreditoAutorId)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Audiovisual - Obter importação pendente com Erros")]
        public async Task Obter_importacao_pendente_com_erros()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoAudiovisual();
        
            var linhasInseridas = GerarAcervoAudiovisualLinhaDTO().Generate(10);
        
            linhasInseridas[3].PossuiErros = true;
            linhasInseridas[3].Duracao.PossuiErro = true;
            linhasInseridas[3].Duracao.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_ATINGIU_LIMITE_CARACTERES, Dominio.Constantes.Constantes.DURACAO);
            
            linhasInseridas[9].PossuiErros = true;
            linhasInseridas[9].Suporte.PossuiErro = true;
            linhasInseridas[9].Suporte.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_NAO_PREENCHIDO, Dominio.Constantes.Constantes.SUPORTE);
           
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Audiovisual,
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
                retorno.Sucesso.Any(a=> a.Credito.Conteudo.SaoIguais(linhaInserida.Credito.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Localizacao.Conteudo.SaoIguais(linhaInserida.Localizacao.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Procedencia.Conteudo.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Data.Conteudo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Copia.Conteudo.SaoIguais(linhaInserida.Copia.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.AutorizacaoUsoDeImagem.Conteudo.SaoIguais(linhaInserida.AutorizacaoUsoDeImagem.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.EstadoConservacao.Conteudo.SaoIguais(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Descricao.Conteudo.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Suporte.Conteudo.SaoIguais(linhaInserida.Suporte.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Duracao.Conteudo.SaoIguais(linhaInserida.Duracao.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Cromia.Conteudo.SaoIguais(linhaInserida.Cromia.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.TamanhoArquivo.Conteudo.SaoIguais(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Acessibilidade.Conteudo.SaoIguais(linhaInserida.Acessibilidade.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Disponibilizacao.Conteudo.SaoIguais(linhaInserida.Disponibilizacao.Conteudo)).ShouldBeTrue();
            }
            
            foreach (var linhaInserida in linhasInseridas.Where(w=> w.PossuiErros))
            {
                retorno.Erros.Any(a=> a.Titulo.Conteudo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Tombo.Conteudo.SaoIguais(linhaInserida.Tombo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Credito.Conteudo.SaoIguais(linhaInserida.Credito.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Localizacao.Conteudo.SaoIguais(linhaInserida.Localizacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Procedencia.Conteudo.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Data.Conteudo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Copia.Conteudo.SaoIguais(linhaInserida.Copia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.AutorizacaoUsoDeImagem.Conteudo.SaoIguais(linhaInserida.AutorizacaoUsoDeImagem.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.EstadoConservacao.Conteudo.SaoIguais(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Descricao.Conteudo.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Suporte.Conteudo.SaoIguais(linhaInserida.Suporte.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Duracao.Conteudo.SaoIguais(linhaInserida.Duracao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Cromia.Conteudo.SaoIguais(linhaInserida.Cromia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.TamanhoArquivo.Conteudo.SaoIguais(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Acessibilidade.Conteudo.SaoIguais(linhaInserida.Acessibilidade.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Disponibilizacao.Conteudo.SaoIguais(linhaInserida.Disponibilizacao.Conteudo)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Audiovisual - Deve permitir remover linha do arquivo")]
        public async Task Deve_permitir_remover_a_linha_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoAudiovisual();

            var linhasInseridas = GerarAcervoAudiovisualLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Audiovisual,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            var retorno = await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, 5);
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault(f => f.Id.SaoIguais(1));
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoAudiovisualLinhaDTO>>(arquivo.Conteudo);
            conteudo.Any(a=> a.NumeroLinha.SaoIguais(5)).ShouldBeFalse();
            conteudo.Count().ShouldBe(9);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Audiovisual - Não deve permitir remover linha do arquivo")]
        public async Task Nao_deve_permitir_remover_a_linha_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoAudiovisual();

            var linhasInseridas = GerarAcervoAudiovisualLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Audiovisual,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, 15).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Audiovisual - Não deve permitir remover todas as linhas do arquivo")]
        public async Task Nao_deve_permitir_remover_todas_as_linhas_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoAudiovisual();

            var linhasInseridas = GerarAcervoAudiovisualLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Audiovisual,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            for (int i = 1; i < 10; i++)
                (await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, i)).ShouldBe(true);
                
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoAudiovisualLinhaDTO>>(arquivo.Conteudo);
            conteudo.Count().ShouldBe(1);
            
            await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, 10).ShouldThrowAsync<NegocioException>();
        }
    }
}