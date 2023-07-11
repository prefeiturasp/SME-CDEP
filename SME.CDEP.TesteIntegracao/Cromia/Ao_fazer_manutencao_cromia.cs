using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
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
            obterTodos.Count.ShouldBe(1);
        }

        [Fact(DisplayName = "Cromia - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirCromias();
            var servicoCromia = GetServicoCromia();

            var acessoDocumentoDtos = await servicoCromia.ObterTodos();
            acessoDocumentoDtos.ShouldNotBeNull();
            acessoDocumentoDtos.Count.ShouldBe(2);
        }

        [Fact(DisplayName = "Cromia - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirCromias();
            
            var servicoCromia = GetServicoCromia();

            var acessoDocumentoDto = await servicoCromia.ObterPorId(1);
            acessoDocumentoDto.ShouldNotBeNull();
            acessoDocumentoDto.Id.ShouldBe(1);
            acessoDocumentoDto.Nome.ShouldBe(ConstantesTestes.COLOR);
        }

        [Fact(DisplayName = "Cromia - Atualizar")]
        public async Task Atualizar()
        {
            await InserirCromias();
            
            var servicoCromia = GetServicoCromia();

            var acessoDocumentoDto = await servicoCromia.ObterPorId(2);
            acessoDocumentoDto.Nome = ConstantesTestes.TRANSPARENTE;
            
            var acessosDocumentosDto = await servicoCromia.Alterar(acessoDocumentoDto);
            
            acessosDocumentosDto.ShouldNotBeNull();
            acessosDocumentosDto.Nome = ConstantesTestes.TRANSPARENTE;
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