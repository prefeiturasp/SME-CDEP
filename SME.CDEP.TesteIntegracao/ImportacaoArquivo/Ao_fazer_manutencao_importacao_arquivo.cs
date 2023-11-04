using Bogus;
using Bogus.Extensions.Brazil;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Newtonsoft.Json;
using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_fazer_manutencao_importacao_arquivo : TesteBase
    {
        public Ao_fazer_manutencao_importacao_arquivo(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Importação Arquivo - Obter última importação")]
        public async Task Obter_ultima_importacao()
        {
            await InserirImportacaoArquivo();

            var servicoImportacaoArquivo = GetServicoImportacaoArquivo();

            var ultimaImportacao = await servicoImportacaoArquivo.ObterUltimaImportacao();
            
            ultimaImportacao.ShouldNotBeNull();
            ultimaImportacao.Conteudo.ShouldNotBeEmpty();
            ultimaImportacao.Nome.ShouldNotBeEmpty();
            ultimaImportacao.TipoAcervo.EhAcervoBibliografico();
            ultimaImportacao.StatusArquivo.EhPendente();
            var linhas = JsonConvert.DeserializeObject<IEnumerable<AcervoBibliograficoLinhaDTO>>(ultimaImportacao.Conteudo);
            linhas.ShouldNotBeNull();
            linhas.Count().ShouldBe(10);
        }
        
        [Fact(DisplayName = "Importação Arquivo - Alterar")]
        public async Task Alterar()
        {
            await InserirImportacaoArquivo();

            var servicoImportacaoArquivo = GetServicoImportacaoArquivo();
           
            var importacaoArquivoAlterar = ObterTodos<ImportacaoArquivo>().FirstOrDefault();
            importacaoArquivoAlterar.Status = ImportacaoStatus.Sucesso;

            await servicoImportacaoArquivo.Alterar(importacaoArquivoAlterar);
            
            var importacaoArquivoAlterado = (ObterTodos<ImportacaoArquivo>()).FirstOrDefault(w=> w.Id == importacaoArquivoAlterar.Id);
            importacaoArquivoAlterado.Nome.ShouldBe(importacaoArquivoAlterar.Nome);
            importacaoArquivoAlterado.Conteudo.ShouldBe(importacaoArquivoAlterar.Conteudo);
            importacaoArquivoAlterado.TipoAcervo.ShouldBe(importacaoArquivoAlterar.TipoAcervo);
            importacaoArquivoAlterado.Status.ShouldBe(importacaoArquivoAlterar.Status);
            importacaoArquivoAlterado.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            importacaoArquivoAlterado.CriadoPor.ShouldNotBeEmpty();
            importacaoArquivoAlterado.CriadoLogin.ShouldNotBeEmpty();
            importacaoArquivoAlterado.AlteradoEm.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            importacaoArquivoAlterado.AlteradoPor.ShouldNotBeEmpty();
            importacaoArquivoAlterado.AlteradoLogin.ShouldNotBeEmpty();
        }
        
        [Fact(DisplayName = "Importação Arquivo - Inserir")]
        public async Task Inserir()
        {
            await InserirImportacaoArquivo();

            var servicoImportacaoArquivo = GetServicoImportacaoArquivo();

            var importacaoArquivoInserir = GerarImportacaoArquivo(TipoAcervo.Bibliografico).Generate();
            importacaoArquivoInserir.AlteradoEm = null;
            importacaoArquivoInserir.AlteradoPor = string.Empty;
            importacaoArquivoInserir.AlteradoLogin = string.Empty;

            await servicoImportacaoArquivo.Inserir(importacaoArquivoInserir);
            
            var importacaoArquivoInserido = (ObterTodos<ImportacaoArquivo>()).FirstOrDefault(w=> w.Id == importacaoArquivoInserir.Id);
            importacaoArquivoInserido.Nome.ShouldBe(importacaoArquivoInserir.Nome);
            importacaoArquivoInserido.Conteudo.ShouldBe(importacaoArquivoInserir.Conteudo);
            importacaoArquivoInserido.TipoAcervo.ShouldBe(importacaoArquivoInserir.TipoAcervo);
            importacaoArquivoInserido.Status.ShouldBe(importacaoArquivoInserir.Status);
            importacaoArquivoInserido.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            importacaoArquivoInserido.CriadoPor.ShouldNotBeEmpty();
            importacaoArquivoInserido.CriadoLogin.ShouldNotBeEmpty();
            importacaoArquivoInserido.AlteradoEm.ShouldBeNull();
            importacaoArquivoInserido.AlteradoPor.ShouldBeEmpty();
            importacaoArquivoInserido.AlteradoLogin.ShouldBeEmpty();
        }
        
        [Fact(DisplayName = "Importação Arquivo - Excluir")]
        public async Task Excluir()
        {
            await InserirImportacaoArquivo();

            var servicoImportacaoArquivo = GetServicoImportacaoArquivo();
           
            var importacaoArquivoAlterar = ObterTodos<ImportacaoArquivo>().LastOrDefault();

            await servicoImportacaoArquivo.Excluir(importacaoArquivoAlterar.Id);
            
            var importacaoArquivoAlterado = ObterTodos<ImportacaoArquivo>().FirstOrDefault(w=> w.Id == importacaoArquivoAlterar.Id);
            importacaoArquivoAlterado.Nome.ShouldBe(importacaoArquivoAlterar.Nome);
            importacaoArquivoAlterado.Conteudo.ShouldBe(importacaoArquivoAlterar.Conteudo);
            importacaoArquivoAlterado.TipoAcervo.ShouldBe(importacaoArquivoAlterar.TipoAcervo);
            importacaoArquivoAlterado.Status.ShouldBe(importacaoArquivoAlterar.Status);
            importacaoArquivoAlterado.Excluido.ShouldBeTrue();
            importacaoArquivoAlterado.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            importacaoArquivoAlterado.CriadoPor.ShouldNotBeEmpty();
            importacaoArquivoAlterado.CriadoLogin.ShouldNotBeEmpty();
            importacaoArquivoAlterado.AlteradoEm.Value.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            importacaoArquivoAlterado.AlteradoPor.ShouldNotBeEmpty();
            importacaoArquivoAlterado.AlteradoLogin.ShouldNotBeEmpty();
        }

        private async Task InserirImportacaoArquivo(TipoAcervo tipoAcervo = TipoAcervo.Bibliografico)
        {
            var importacaoArquivos = GerarImportacaoArquivo(tipoAcervo).Generate(10);

            foreach (var importacaoArquivo in importacaoArquivos)
                await InserirNaBase(importacaoArquivo);
        }
    }
}