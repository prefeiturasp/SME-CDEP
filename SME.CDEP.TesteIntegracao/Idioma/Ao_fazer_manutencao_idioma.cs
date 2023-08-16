using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
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

            var idioma = await servicoIdioma.Inserir(new IdNomeExcluidoDTO(){Nome = ConstantesTestes.PORTUGUES});
            idioma.ShouldBeGreaterThan(0);
            var obterTodos = ObterTodos<Idioma>();
            obterTodos.Count.ShouldBe(1);
        }
        
        [Fact(DisplayName = "Idioma - Não deve inserir pois já existe cadastro com esse nome")]
        public async Task Nao_deve_para_cadastros_duplicados()
        {
            await InserirIdiomas();
            
            var servicoIdioma = GetServicoIdioma();

            await servicoIdioma.Inserir(new IdNomeExcluidoDTO(){Nome = ConstantesTestes.PORTUGUES}).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Idioma - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirIdiomas();
            var servicoIdioma = GetServicoIdioma();

            var idiomas = await servicoIdioma.ObterTodos();
            idiomas.ShouldNotBeNull();
            idiomas.Count.ShouldBe(5);
        }

        [Fact(DisplayName = "Idioma - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirIdiomas();
            
            var servicoIdioma = GetServicoIdioma();

            var idioma = await servicoIdioma.ObterPorId(1);
            idioma.ShouldNotBeNull();
            idioma.Id.ShouldBe(1);
            idioma.Nome.ShouldBe(ConstantesTestes.PORTUGUES);
        }

        [Fact(DisplayName = "Idioma - Atualizar")]
        public async Task Atualizar()
        {
            await InserirIdiomas();
            
            var servicoIdioma = GetServicoIdioma();

            var idioma = await servicoIdioma.ObterPorId(3);
            idioma.Nome = ConstantesTestes.ITALIANO;
            
            var idiomasAlterados = await servicoIdioma.Alterar(idioma);
            
            idiomasAlterados.ShouldNotBeNull();
            idiomasAlterados.Nome = ConstantesTestes.ITALIANO;
        }
        
        [Fact(DisplayName = "Idioma - Não deve alterar pois já existe cadastro com esse nome")]
        public async Task Nao_deve_atualizar_para_cadastros_duplicados()
        {
            await InserirIdiomas();
            
            var servicoIdioma = GetServicoIdioma();

            var idioma = await servicoIdioma.ObterPorId(3);
            idioma.Nome = ConstantesTestes.PORTUGUES;
            
            await servicoIdioma.Alterar(idioma).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Idioma - Excluir")]
        public async Task Excluir()
        {
            await InserirIdiomas();
            
            var servicoIdioma = GetServicoIdioma();

            await servicoIdioma.Excluir(3);

            var idiomas = ObterTodos<Idioma>();
            idiomas.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            idiomas.Count(a=> !a.Excluido).ShouldBeEquivalentTo(4);
        }

        private async Task InserirIdiomas()
        {
            await InserirNaBase(new Idioma() { Nome = ConstantesTestes.PORTUGUES });
            await InserirNaBase(new Idioma() { Nome = ConstantesTestes.INGLES });
            await InserirNaBase(new Idioma() { Nome = ConstantesTestes.ESPANHOL });
            await InserirNaBase(new Idioma() { Nome = ConstantesTestes.FRANCES });
            await InserirNaBase(new Idioma() { Nome = ConstantesTestes.Alemao });
        }
    }
}