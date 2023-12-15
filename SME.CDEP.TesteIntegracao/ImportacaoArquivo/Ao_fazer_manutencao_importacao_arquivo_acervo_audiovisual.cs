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

            var acervoAudiovisualLinhas = AcervoAudiovisualLinhaMock.GerarAcervoAudiovisualLinhaDTO().Generate(10);

            acervoAudiovisualLinhas[2].Titulo.Conteudo = string.Empty;
            acervoAudiovisualLinhas[4].Suporte.Conteudo = string.Empty;
            acervoAudiovisualLinhas[5].Duracao.Conteudo = faker.Lorem.Paragraph();
            acervoAudiovisualLinhas[7].Copia.Conteudo = faker.Lorem.Paragraph();
            acervoAudiovisualLinhas[8].Codigo.Conteudo = string.Empty;
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
                linha.Procedencia.PossuiErro.ShouldBeFalse();
                linha.Ano.PossuiErro.ShouldBeFalse();
                linha.Data.PossuiErro.ShouldBeFalse();
                linha.PermiteUsoImagem.PossuiErro.ShouldBeFalse();
                linha.EstadoConservacao.PossuiErro.ShouldBeFalse();
                linha.Descricao.PossuiErro.ShouldBeFalse();
                linha.Cromia.PossuiErro.ShouldBeFalse();
                linha.TamanhoArquivo.PossuiErro.ShouldBeFalse();
                linha.Acessibilidade.PossuiErro.ShouldBeFalse();
                linha.Disponibilizacao.PossuiErro.ShouldBeFalse();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Audiovisual - PersistenciaAcervo")]
        public async Task Persistencia_acervo()
        {
            await InserirDadosBasicos();

            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoAudiovisual();
        
            var acervoAudiovisualLinhas = AcervoAudiovisualLinhaMock.GerarAcervoAudiovisualLinhaDTO().Generate(10);
        
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Audiovisual,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoAudiovisualLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
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
                acervos.Any(a=> a.Codigo.SaoIguais(linhasComSucesso.Codigo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.SaoIguais(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue(); 
                acervos.Any(a=> a.DataAcervo.SaoIguais(linhasComSucesso.Data.Conteudo)).ShouldBeTrue(); 
                acervos.Any(a=> a.Ano.SaoIguais(linhasComSucesso.Ano.Conteudo.ConverterParaInteiro())).ShouldBeTrue(); 
                
                //Referência 1:1
                acervosAudiovisual.Any(a=> a.SuporteId.SaoIguais(suportes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Suporte.Conteudo)).Id)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.CromiaId.SaoIguais(cromias.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Cromia.Conteudo)).Id)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.ConservacaoId.SaoIguais(conservacoes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.EstadoConservacao.Conteudo)).Id)).ShouldBeTrue();
                
                //Campos livres
                acervosAudiovisual.Any(a=> a.Localizacao.SaoIguais(linhasComSucesso.Localizacao.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Procedencia.SaoIguais(linhasComSucesso.Procedencia.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Copia.SaoIguais(linhasComSucesso.Copia.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.PermiteUsoImagem.SaoIguais(linhasComSucesso.PermiteUsoImagem.Conteudo.EhOpcaoSim())).ShouldBeTrue();
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
        
        [Fact(DisplayName = "Importação Arquivo Acervo Audiovisual - Geral - Com erros em 4 linhas")]
        public async Task Importacao_geral()
        {
            await InserirDadosBasicos();

            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoAudiovisual();
        
            var acervoAudiovisualLinhas = AcervoAudiovisualLinhaMock.GerarAcervoAudiovisualLinhaDTO().Generate(10);
           
            acervoAudiovisualLinhas[1].PermiteUsoImagem.Conteudo = acervoAudiovisualLinhas[1].Titulo.Conteudo;
            acervoAudiovisualLinhas[3].Descricao.Conteudo = string.Empty;
            acervoAudiovisualLinhas[5].Duracao.Conteudo = faker.Lorem.Paragraph();
            acervoAudiovisualLinhas[7].Codigo.Conteudo = acervoAudiovisualLinhas[0].Codigo.Conteudo;
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Audiovisual,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(acervoAudiovisualLinhas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            servicoImportacaoArquivo.ValidarPreenchimentoValorFormatoQtdeCaracteres(acervoAudiovisualLinhas);
            await servicoImportacaoArquivo.PersistenciaAcervo(acervoAudiovisualLinhas);
            await servicoImportacaoArquivo.AtualizarImportacao(1, JsonConvert.SerializeObject(acervoAudiovisualLinhas), acervoAudiovisualLinhas.Any(a=> a.PossuiErros) ? ImportacaoStatus.Erros : ImportacaoStatus.Sucesso);
            var retorno = await servicoImportacaoArquivo.ObterImportacaoPendente();
            
            var acervos = ObterTodos<Acervo>();
            var acervosAudiovisual = ObterTodos<AcervoAudiovisual>();
            var suportes = ObterTodos<Suporte>();
            var cromias = ObterTodos<Cromia>();
            var acervoCreditoAutors = ObterTodos<AcervoCreditoAutor>();
            var creditoAutores = ObterTodos<CreditoAutor>();
            var conservacoes = ObterTodos<Conservacao>();
            
            //Acervos inseridos
            acervos.ShouldNotBeNull();
            acervos.Count().ShouldBe(6);
            
            //Acervos auxiliares inseridos
            acervosAudiovisual.ShouldNotBeNull();
            acervosAudiovisual.Count().ShouldBe(6);
            
            //Linhas com erros
            acervoAudiovisualLinhas.Count(w=> !w.PossuiErros).ShouldBe(6);
            acervoAudiovisualLinhas.Count(w=> w.PossuiErros).ShouldBe(4);
            
            //Retorno front
            retorno.Id.ShouldBe(1);
            retorno.Nome.ShouldNotBeEmpty();
            retorno.TipoAcervo.ShouldBe(TipoAcervo.Audiovisual);
            retorno.DataImportacao.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            
            foreach (var linhaInserida in acervoAudiovisualLinhas.Where(w=> !w.PossuiErros))
            {
                retorno.Sucesso.Any(a=> a.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Tombo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.NumeroLinha.SaoIguais(linhaInserida.NumeroLinha)).ShouldBeTrue();
            }
            
            foreach (var linhaInserida in acervoAudiovisualLinhas.Where(w=> w.PossuiErros))
            {
                retorno.Erros.Any(a=> a.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Tombo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.NumeroLinha.SaoIguais(linhaInserida.NumeroLinha)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoObjeto.Titulo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Codigo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Localizacao.SaoIguais(linhaInserida.Localizacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Procedencia.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Ano.SaoIguais(linhaInserida.Ano.Conteudo.ConverterParaInteiro())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.DataAcervo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Copia.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.PermiteUsoImagem.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.ConservacaoId.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Descricao.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.SuporteId.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Duracao.SaoIguais(linhaInserida.Duracao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.CromiaId.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.TamanhoArquivo.SaoIguais(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Acessibilidade.SaoIguais(linhaInserida.Acessibilidade.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Disponibilizacao.SaoIguais(linhaInserida.Disponibilizacao.Conteudo)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoErro.Titulo.Conteudo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Codigo.Conteudo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CreditosAutoresIds.Conteudo.SaoIguais(linhaInserida.Credito.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Localizacao.Conteudo.SaoIguais(linhaInserida.Localizacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Procedencia.Conteudo.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Ano.Conteudo.SaoIguais(linhaInserida.Ano.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.DataAcervo.Conteudo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Copia.Conteudo.SaoIguais(linhaInserida.Copia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.PermiteUsoImagem.Conteudo.SaoIguais(linhaInserida.PermiteUsoImagem.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.ConservacaoId.Conteudo.SaoIguais(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Descricao.Conteudo.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.SuporteId.Conteudo.SaoIguais(linhaInserida.Suporte.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Duracao.Conteudo.SaoIguais(linhaInserida.Duracao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CromiaId.Conteudo.SaoIguais(linhaInserida.Cromia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.TamanhoArquivo.Conteudo.SaoIguais(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Acessibilidade.Conteudo.SaoIguais(linhaInserida.Acessibilidade.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Disponibilizacao.Conteudo.SaoIguais(linhaInserida.Disponibilizacao.Conteudo)).ShouldBeTrue();
            }
        
            foreach (var linhasComSucesso in acervoAudiovisualLinhas.Where(w=> !w.PossuiErros))
            {
                //Acervo
                acervos.Any(a=> a.Titulo.SaoIguais(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.SaoIguais(linhasComSucesso.Codigo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.SaoIguais(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                acervos.Any(a=> a.DataAcervo.SaoIguais(linhasComSucesso.Data.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Ano.SaoIguais(linhasComSucesso.Data.Conteudo.ConverterParaInteiro())).ShouldBeTrue();
                
                //Referência 1:1
                acervosAudiovisual.Any(a=> a.SuporteId.SaoIguais(suportes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Suporte.Conteudo)).Id)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.CromiaId.SaoIguais(cromias.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.Cromia.Conteudo)).Id)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.ConservacaoId.SaoIguais(conservacoes.FirstOrDefault(f=> f.Nome.SaoIguais(linhasComSucesso.EstadoConservacao.Conteudo)).Id)).ShouldBeTrue();
                
                //Campos livres
                acervosAudiovisual.Any(a=> a.Localizacao.SaoIguais(linhasComSucesso.Localizacao.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Procedencia.SaoIguais(linhasComSucesso.Procedencia.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Copia.SaoIguais(linhasComSucesso.Copia.Conteudo)).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.PermiteUsoImagem.SaoIguais(linhasComSucesso.PermiteUsoImagem.Conteudo.EhOpcaoSim())).ShouldBeTrue();
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
        
            var linhasInseridas = AcervoAudiovisualLinhaMock.GerarAcervoAudiovisualLinhaDTO().Generate(10);
        
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
            
            await InserirCreditosAutorias(linhasInseridas.Select(s => s.Credito.Conteudo).ToArray().UnificarPipe().SplitPipe().Distinct().Where(w => w.EstaPreenchido()));
                
            await InserirConservacoes(linhasInseridas.Select(s => s.EstadoConservacao.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
                
            await InserirCromias(linhasInseridas.Select(s => s.Cromia.Conteudo).Distinct().Where(w=> w.EstaPreenchido()));
            
            await InserirSuportes(linhasInseridas.Select(s => s.Cromia.Conteudo).Distinct().Where(w=> w.EstaPreenchido()), TipoSuporte.VIDEO);
            
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
                retorno.Erros.Any(a=> a.RetornoObjeto.Ano.SaoIguais(linhaInserida.Ano.Conteudo.ConverterParaInteiro())).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.DataAcervo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Copia.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.PermiteUsoImagem.NaoEhNulo()).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Descricao.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Duracao.SaoIguais(linhaInserida.Duracao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.TamanhoArquivo.SaoIguais(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Acessibilidade.SaoIguais(linhaInserida.Acessibilidade.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoObjeto.Disponibilizacao.SaoIguais(linhaInserida.Disponibilizacao.Conteudo)).ShouldBeTrue();
                
                retorno.Erros.Any(a=> a.RetornoErro.Titulo.Conteudo.SaoIguais(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Codigo.Conteudo.SaoIguais(linhaInserida.Codigo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CreditosAutoresIds.Conteudo.SaoIguais(linhaInserida.Credito.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Localizacao.Conteudo.SaoIguais(linhaInserida.Localizacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Procedencia.Conteudo.SaoIguais(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Ano.Conteudo.SaoIguais(linhaInserida.Ano.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.DataAcervo.Conteudo.SaoIguais(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Copia.Conteudo.SaoIguais(linhaInserida.Copia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.PermiteUsoImagem.Conteudo.SaoIguais(linhaInserida.PermiteUsoImagem.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.ConservacaoId.Conteudo.SaoIguais(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Descricao.Conteudo.SaoIguais(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.SuporteId.Conteudo.SaoIguais(linhaInserida.Suporte.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Duracao.Conteudo.SaoIguais(linhaInserida.Duracao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.CromiaId.Conteudo.SaoIguais(linhaInserida.Cromia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.TamanhoArquivo.Conteudo.SaoIguais(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Acessibilidade.Conteudo.SaoIguais(linhaInserida.Acessibilidade.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.RetornoErro.Disponibilizacao.Conteudo.SaoIguais(linhaInserida.Disponibilizacao.Conteudo)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Audiovisual - Deve permitir remover linha do arquivo")]
        public async Task Deve_permitir_remover_a_linha_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoAudiovisual();

            var linhasInseridas = AcervoAudiovisualLinhaMock.GerarAcervoAudiovisualLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Audiovisual,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            var retorno = await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = 5});
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault(f => f.Id.SaoIguais(1));
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoAudiovisualLinhaDTO>>(arquivo.Conteudo);
            conteudo.Any(a=> a.NumeroLinha.SaoIguais(5)).ShouldBeFalse();
            conteudo.Count().ShouldBe(9);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Audiovisual - Não deve permitir remover linha do arquivo")]
        public async Task Nao_deve_permitir_remover_a_linha_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoAudiovisual();

            var linhasInseridas = AcervoAudiovisualLinhaMock.GerarAcervoAudiovisualLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Audiovisual,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = 15}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Audiovisual - Não deve permitir remover todas as linhas do arquivo")]
        public async Task Nao_deve_permitir_remover_todas_as_linhas_do_arquivo()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoAudiovisual();

            var linhasInseridas = AcervoAudiovisualLinhaMock.GerarAcervoAudiovisualLinhaDTO().Generate(10);

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Audiovisual,
                Status = ImportacaoStatus.Pendente,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            for (int i = 1; i < 10; i++)
                (await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = i})).ShouldBe(true);
                
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoAudiovisualLinhaDTO>>(arquivo.Conteudo);
            conteudo.Count().ShouldBe(1);
            
            await servicoImportacaoArquivo.RemoverLinhaDoArquivo(1, new LinhaDTO(){NumeroLinha = 10}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Audiovisual - Deve permitir atualizar uma linha do arquivo para sucesso e outra fica com erro")]
        public async Task Deve_permitir_atualizar_uma_linha_do_arquivo_para_sucesso_e_outra_fica_com_erro()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoAudiovisual();

            var linhasInseridas = AcervoAudiovisualLinhaMock.GerarAcervoAudiovisualLinhaDTO().Generate(10);
            linhasInseridas[3].PossuiErros = true;
            linhasInseridas[3].Cromia.PossuiErro = true;
            linhasInseridas[3].Cromia.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.LARGURA);
            linhasInseridas[3].Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.LARGURA); //Mensagem geral
            
            linhasInseridas[9].PossuiErros = true;
            linhasInseridas[9].Suporte.PossuiErro = true;
            linhasInseridas[9].Suporte.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_NAO_PREENCHIDO, Dominio.Constantes.Constantes.SUPORTE);
            linhasInseridas[9].Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_NAO_PREENCHIDO, Dominio.Constantes.Constantes.SUPORTE); //Mensagem geral

            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Audiovisual,
                Status = ImportacaoStatus.Erros,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.AtualizarLinhaParaSucesso(1, new LinhaDTO(){NumeroLinha = 4});
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoAudiovisualLinhaDTO>>(arquivo.Conteudo);
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).PossuiErros.ShouldBeFalse();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).Mensagem.ShouldBeEmpty();
            
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(10)).PossuiErros.ShouldBeTrue();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(10)).Mensagem.ShouldNotBeEmpty();
            
            arquivo.Status.ShouldBe(ImportacaoStatus.Erros);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Audiovisual - Deve permitir atualizar linha do arquivo para sucesso")]
        public async Task Deve_permitir_atualizar_linha_do_arquivo_para_sucesso()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoAudiovisual();

            var linhasInseridas = AcervoAudiovisualLinhaMock.GerarAcervoAudiovisualLinhaDTO().Generate(10);
            linhasInseridas[3].PossuiErros = true;
            linhasInseridas[3].Cromia.PossuiErro = true;
            linhasInseridas[3].Cromia.Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.LARGURA);
            linhasInseridas[3].Mensagem = string.Format(Dominio.Constantes.Constantes.CAMPO_X_REQUER_UM_VALOR_NUMERICO, Dominio.Constantes.Constantes.LARGURA); //Mensagem geral
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Audiovisual,
                Status = ImportacaoStatus.Erros,
                Conteudo = JsonConvert.SerializeObject(linhasInseridas),
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = ConstantesTestes.SISTEMA, CriadoLogin = ConstantesTestes.LOGIN_123456789
            });

            await servicoImportacaoArquivo.AtualizarLinhaParaSucesso(1, new LinhaDTO(){NumeroLinha = 4});
            var arquivo = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            
            var conteudo = JsonConvert.DeserializeObject<IEnumerable<AcervoAudiovisualLinhaDTO>>(arquivo.Conteudo);
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).PossuiErros.ShouldBeFalse();
            conteudo.FirstOrDefault(a=> a.NumeroLinha.SaoIguais(4)).Mensagem.ShouldBeEmpty();
            
            conteudo.Any(a=> a.PossuiErros).ShouldBeFalse();
            conteudo.Any(a=> !a.PossuiErros).ShouldBeTrue();
            
            arquivo.Status.ShouldBe(ImportacaoStatus.Sucesso);
        }
        
        [Fact(DisplayName = "Importação Arquivo Acervo Audiovisual - Validação de RetornoObjeto")]
        public async Task Validacao_retorno_objeto()
        {
            var servicoImportacaoArquivo = GetServicoImportacaoArquivoAcervoAudiovisual();

            var linhasInseridas = AcervoAudiovisualLinhaMock.GerarAcervoAudiovisualLinhaDTO().Generate(10);
            
            linhasInseridas.Add(new AcervoAudiovisualLinhaDTO()
            {
                PossuiErros = true,
                Titulo = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Localizacao = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Codigo = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Procedencia = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Ano = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Data = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Copia = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                PermiteUsoImagem = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Duracao = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                TamanhoArquivo = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Acessibilidade = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Disponibilizacao = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Descricao = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                EstadoConservacao = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Cromia = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Suporte = new LinhaConteudoAjustarDTO() { PossuiErro = true},
                Credito = new LinhaConteudoAjustarDTO() { PossuiErro = true},
            });
            
            await InserirNaBase(new ImportacaoArquivo()
            {
                Nome = faker.Hacker.Verb(),
                TipoAcervo = TipoAcervo.Audiovisual,
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
                erro.RetornoObjeto.Copia.ShouldBeNull();
                erro.RetornoObjeto.PermiteUsoImagem.HasValue.ShouldBeFalse();
                erro.RetornoObjeto.ConservacaoId.ShouldBeNull();
                erro.RetornoObjeto.CromiaId.ShouldBeNull();
                erro.RetornoObjeto.Duracao.ShouldBeNull();
                erro.RetornoObjeto.TamanhoArquivo.ShouldBeNull();
                erro.RetornoObjeto.Acessibilidade.ShouldBeNull();
                erro.RetornoObjeto.Disponibilizacao.ShouldBeNull();
                erro.RetornoObjeto.SuporteId.ShouldBeNull();
                erro.RetornoObjeto.Descricao.ShouldBeNull();
                erro.RetornoObjeto.CreditosAutoresIds.ShouldBeNull();
            }
            retorno.ShouldNotBeNull();
        }
    }
}