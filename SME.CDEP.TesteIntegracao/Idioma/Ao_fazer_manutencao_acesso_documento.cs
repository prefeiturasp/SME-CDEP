using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;

namespace SME.CDEP.TesteIntegracao.Usuario
{
    public class Ao_fazer_manutencao_idioma : TesteBase
    {
        public Ao_fazer_manutencao_idioma(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Idioma - Inserir")]
        public async Task Inserir()
        {
            var servicoIdioma = GetServicoIdioma();

            var idioma = await servicoIdioma.Inserir(new IdiomaDTO(){Nome = ConstantesTestes.PORTUGUES});
            idioma.ShouldBeGreaterThan(0);
            var obterTodos = ObterTodos<Idioma>();
            obterTodos.Count.ShouldBe(1);
        }

        [Fact(DisplayName = "Idioma - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirAcessoDocumentos();
            var servicoAcessoDocumento = GetServicoAcessoDocumento();

            var acessoDocumentoDtos = await servicoAcessoDocumento.ObterTodos();
            acessoDocumentoDtos.ShouldNotBeNull();
            acessoDocumentoDtos.Count.ShouldBe(3);
        }

        [Fact(DisplayName = "Idioma - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirAcessoDocumentos();
            
            var servicoAcessoDocumento = GetServicoAcessoDocumento();

            var acessoDocumentoDto = await servicoAcessoDocumento.ObterPorId(1);
            acessoDocumentoDto.ShouldNotBeNull();
            acessoDocumentoDto.Id.ShouldBe(1);
            acessoDocumentoDto.Nome.ShouldBe(ConstantesTestes.DIGITAL);
        }

        [Fact(DisplayName = "Idioma - Atualizar")]
        public async Task Atualizar()
        {
            await InserirAcessoDocumentos();
            
            var servicoAcessoDocumento = GetServicoAcessoDocumento();

            var acessoDocumentoDto = await servicoAcessoDocumento.ObterPorId(3);
            acessoDocumentoDto.Nome = ConstantesTestes.DIGITAL;
            
            var acessosDocumentosDto = await servicoAcessoDocumento.Alterar(acessoDocumentoDto);
            
            acessosDocumentosDto.ShouldNotBeNull();
            acessosDocumentosDto.Nome = ConstantesTestes.DIGITAL;
        }
        
        [Fact(DisplayName = "Idioma - Excluir")]
        public async Task Excluir()
        {
            await InserirAcessoDocumentos();
            
            var servicoAcessoDocumento = GetServicoAcessoDocumento();

            await servicoAcessoDocumento.Excluir(3);

            var acessosDocumentos = ObterTodos<AcessoDocumento>();
            acessosDocumentos.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            acessosDocumentos.Count(a=> !a.Excluido).ShouldBeEquivalentTo(2);
        }

        private async Task InserirAcessoDocumentos()
        {
            await InserirNaBase(new AcessoDocumento() { Nome = ConstantesTestes.DIGITAL });
            await InserirNaBase(new AcessoDocumento() { Nome = ConstantesTestes.FISICO });
            await InserirNaBase(new AcessoDocumento() { Nome = ConstantesTestes.DIGITAL_E_FISICO });
        }
    }
}