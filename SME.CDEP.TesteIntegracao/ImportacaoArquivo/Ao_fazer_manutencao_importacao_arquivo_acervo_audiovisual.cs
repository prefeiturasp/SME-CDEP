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
                    linha.Duracao.PossuiErro.ShouldBeTrue();
                    linha.Duracao.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Duracao.PossuiErro.ShouldBeFalse();
                    linha.Duracao.Mensagem.ShouldBeEmpty();
                }
                
                if (linha.NumeroLinha == 8)
                {
                    linha.Copia.PossuiErro.ShouldBeTrue();
                    linha.Copia.Mensagem.ShouldNotBeEmpty();
                }
                else
                {
                    linha.Copia.PossuiErro.ShouldBeFalse();
                    linha.Copia.Mensagem.ShouldBeEmpty();
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
                acervos.Any(a=> a.Titulo.Equals(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.Equals(linhasComSucesso.Tombo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.Equals(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosAudiovisual.Any(a=> a.SuporteId == suportes.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Suporte.Conteudo)).Id).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.CromiaId == cromias.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Cromia.Conteudo)).Id).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.ConservacaoId == conservacoes.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.EstadoConservacao.Conteudo)).Id).ShouldBeTrue();
                
                //Campos livres
                acervosAudiovisual.Any(a=> a.Localizacao == linhasComSucesso.Localizacao.Conteudo).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Procedencia == linhasComSucesso.Procedencia.Conteudo).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.DataAcervo == linhasComSucesso.Data.Conteudo).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Copia == linhasComSucesso.Copia.Conteudo).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.PermiteUsoImagem == linhasComSucesso.AutorizacaoUsoDeImagem.Conteudo.Equals("Sim")).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.TamanhoArquivo == linhasComSucesso.TamanhoArquivo.Conteudo).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Acessibilidade == linhasComSucesso.Acessibilidade.Conteudo).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Disponibilizacao == linhasComSucesso.Disponibilizacao.Conteudo).ShouldBeTrue();
                
                //Crédito
                var creditoAInserir = linhasComSucesso.Credito.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var credito in creditoAInserir)
                    creditoAutores.Any(a=> a.Nome.Equals(credito)).ShouldBeTrue();
                
                foreach (var creditoAutor in acervoCreditoAutors)
                    creditoAutores.Any(a=> a.Id == creditoAutor.CreditoAutorId).ShouldBeTrue();
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
                acervos.Any(a=> a.Titulo.Equals(linhasComSucesso.Titulo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Codigo.Equals(linhasComSucesso.Tombo.Conteudo)).ShouldBeTrue();
                acervos.Any(a=> a.Descricao.Equals(linhasComSucesso.Descricao.Conteudo)).ShouldBeTrue();  
                
                //Referência 1:1
                acervosAudiovisual.Any(a=> a.SuporteId == suportes.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Suporte.Conteudo)).Id).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.CromiaId == cromias.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.Cromia.Conteudo)).Id).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.ConservacaoId == conservacoes.FirstOrDefault(f=> f.Nome.Equals(linhasComSucesso.EstadoConservacao.Conteudo)).Id).ShouldBeTrue();
                
                //Campos livres
                acervosAudiovisual.Any(a=> a.Localizacao == linhasComSucesso.Localizacao.Conteudo).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Procedencia == linhasComSucesso.Procedencia.Conteudo).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.DataAcervo == linhasComSucesso.Data.Conteudo).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Copia == linhasComSucesso.Copia.Conteudo).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.PermiteUsoImagem == linhasComSucesso.AutorizacaoUsoDeImagem.Conteudo.Equals("Sim")).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.TamanhoArquivo == linhasComSucesso.TamanhoArquivo.Conteudo).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Acessibilidade == linhasComSucesso.Acessibilidade.Conteudo).ShouldBeTrue();
                acervosAudiovisual.Any(a=> a.Disponibilizacao == linhasComSucesso.Disponibilizacao.Conteudo).ShouldBeTrue();
                
                //Crédito
                var creditoAInserir = linhasComSucesso.Credito.Conteudo.FormatarTextoEmArray().ToArray().UnificarPipe().SplitPipe().Distinct();
                
                foreach (var credito in creditoAInserir)
                    creditoAutores.Any(a=> a.Nome.Equals(credito)).ShouldBeTrue();
                
                foreach (var creditoAutor in acervoCreditoAutors)
                    creditoAutores.Any(a=> a.Id == creditoAutor.CreditoAutorId).ShouldBeTrue();
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
                retorno.Sucesso.Any(a=> a.Titulo.Conteudo.Equals(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Tombo.Conteudo.Equals(linhaInserida.Tombo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Credito.Conteudo.Equals(linhaInserida.Credito.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Localizacao.Conteudo.Equals(linhaInserida.Localizacao.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Procedencia.Conteudo.Equals(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Data.Conteudo.Equals(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Copia.Conteudo.Equals(linhaInserida.Copia.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.AutorizacaoUsoDeImagem.Conteudo.Equals(linhaInserida.AutorizacaoUsoDeImagem.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.EstadoConservacao.Conteudo.Equals(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Descricao.Conteudo.Equals(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Suporte.Conteudo.Equals(linhaInserida.Suporte.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Duracao.Conteudo.Equals(linhaInserida.Duracao.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Cromia.Conteudo.Equals(linhaInserida.Cromia.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.TamanhoArquivo.Conteudo.Equals(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Acessibilidade.Conteudo.Equals(linhaInserida.Acessibilidade.Conteudo)).ShouldBeTrue();
                retorno.Sucesso.Any(a=> a.Disponibilizacao.Conteudo.Equals(linhaInserida.Disponibilizacao.Conteudo)).ShouldBeTrue();
            }
            
            foreach (var linhaInserida in linhasInseridas.Where(w=> w.PossuiErros))
            {
                retorno.Erros.Any(a=> a.Titulo.Conteudo.Equals(linhaInserida.Titulo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Tombo.Conteudo.Equals(linhaInserida.Tombo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Credito.Conteudo.Equals(linhaInserida.Credito.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Localizacao.Conteudo.Equals(linhaInserida.Localizacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Procedencia.Conteudo.Equals(linhaInserida.Procedencia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Data.Conteudo.Equals(linhaInserida.Data.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Copia.Conteudo.Equals(linhaInserida.Copia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.AutorizacaoUsoDeImagem.Conteudo.Equals(linhaInserida.AutorizacaoUsoDeImagem.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.EstadoConservacao.Conteudo.Equals(linhaInserida.EstadoConservacao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Descricao.Conteudo.Equals(linhaInserida.Descricao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Suporte.Conteudo.Equals(linhaInserida.Suporte.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Duracao.Conteudo.Equals(linhaInserida.Duracao.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Cromia.Conteudo.Equals(linhaInserida.Cromia.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.TamanhoArquivo.Conteudo.Equals(linhaInserida.TamanhoArquivo.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Acessibilidade.Conteudo.Equals(linhaInserida.Acessibilidade.Conteudo)).ShouldBeTrue();
                retorno.Erros.Any(a=> a.Disponibilizacao.Conteudo.Equals(linhaInserida.Disponibilizacao.Conteudo)).ShouldBeTrue();
            }
        }
    }
}