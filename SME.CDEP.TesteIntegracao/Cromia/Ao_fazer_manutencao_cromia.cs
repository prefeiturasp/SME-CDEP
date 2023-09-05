using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;

namespace SME.CDEP.TesteIntegracao.Usuario
{
    public class Ao_fazer_manutencao_cromia : TesteBase
    {
        public Ao_fazer_manutencao_cromia(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Cromia - Inserir")]
        public async Task Inserir()
        {
            var servicoCromia = GetServicoCromia();

            var cromia = await servicoCromia.Inserir(new IdNomeExcluidoDTO(){Nome = ConstantesTestes.PB});
            cromia.ShouldBeGreaterThan(0);
            var obterTodos = ObterTodos<Cromia>();
            obterTodos.Count().ShouldBe(1);
        }
        
        [Fact(DisplayName = "Cromia - Não deve inserir pois já existe cadastro com esse nome")]
        public async Task Nao_deve_inserir()
        {
            await InserirCromias();
            
            var servicoCromia = GetServicoCromia();

            await servicoCromia.Inserir(new IdNomeExcluidoDTO(){Nome = ConstantesTestes.PB}).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Cromia - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirCromias();
            var servicoCromia = GetServicoCromia();

            var cromiaDTO = await servicoCromia.ObterTodos();
            cromiaDTO.ShouldNotBeNull();
            cromiaDTO.Count().ShouldBe(2);
        }

        [Fact(DisplayName = "Cromia - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirCromias();
            
            var servicoCromia = GetServicoCromia();

            var cromiaDTO = await servicoCromia.ObterPorId(1);
            cromiaDTO.ShouldNotBeNull();
            cromiaDTO.Id.ShouldBe(1);
            cromiaDTO.Nome.ShouldBe(ConstantesTestes.COLOR);
        }

        [Fact(DisplayName = "Cromia - Atualizar")]
        public async Task Atualizar()
        {
            await InserirCromias();
            
            var servicoCromia = GetServicoCromia();

            var cromiaDTO = await servicoCromia.ObterPorId(2);
            cromiaDTO.Nome = ConstantesTestes.TRANSPARENTE;
            
            var cromiaAlteradaDTO = await servicoCromia.Alterar(cromiaDTO);
            
            cromiaAlteradaDTO.ShouldNotBeNull();
            cromiaAlteradaDTO.Nome = ConstantesTestes.TRANSPARENTE;
        }
        
        [Fact(DisplayName = "Cromia - Não deve alterar pois já existe cadastro com esse nome")]
        public async Task Nao_deve_atualizar_para_cadastros_duplicados()
        {
            await InserirCromias();
            
            var servicoCromia = GetServicoCromia();

            var cromiaDTO = await servicoCromia.ObterPorId(2);
            cromiaDTO.Nome = ConstantesTestes.COLOR;
            
            await servicoCromia.Alterar(cromiaDTO).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Cromia - Excluir")]
        public async Task Excluir()
        {
            await InserirCromias();
            
            var servicoCromia = GetServicoCromia();

            await servicoCromia.Excluir(2);

            var cromias = ObterTodos<Cromia>();
            cromias.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            cromias.Count(a=> !a.Excluido).ShouldBeEquivalentTo(1);
        }

        private async Task InserirCromias()
        {
            await InserirNaBase(new Cromia() { Nome = ConstantesTestes.COLOR });
            await InserirNaBase(new Cromia() { Nome = ConstantesTestes.PB });
        }
    }
}