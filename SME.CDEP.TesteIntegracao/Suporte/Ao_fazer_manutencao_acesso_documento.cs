using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;

namespace SME.CDEP.TesteIntegracao.Usuario
{
    public class Ao_fazer_manutencao_suporte : TesteBase
    {
        public Ao_fazer_manutencao_suporte(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Suporte - Inserir")]
        public async Task Inserir()
        {
            var servicoSuporte = GetServicoSuporte();

            var suporte = await servicoSuporte.Inserir(new SuporteDTO(){Nome = ConstantesTestes.DIGITAL_E_FISICO, TipoSuporte = (int)TipoSuporte.VIDEO});
            suporte.ShouldBeGreaterThan(0);
            var obterTodos = ObterTodos<Suporte>();
            obterTodos.Count.ShouldBe(1);
        }

        [Fact(DisplayName = "Suporte - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirSuporte();
            var servicoSuporte = GetServicoSuporte();

            var suporteDtos = await servicoSuporte.ObterTodos();
            suporteDtos.ShouldNotBeNull();
            suporteDtos.Count.ShouldBe(5);
        }

        [Fact(DisplayName = "Suporte - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirSuporte();
            
            var servicoSuporte = GetServicoSuporte();

            var suporte = await servicoSuporte.ObterPorId(1);
            suporte.ShouldNotBeNull();
            suporte.Id.ShouldBe(1);
            suporte.Nome.ShouldBe(ConstantesTestes.PAPEL);
        }

        [Fact(DisplayName = "Suporte - Atualizar")]
        public async Task Atualizar()
        {
            await InserirSuporte();
            
            var servicoSuporte = GetServicoSuporte();

            var suporte = await servicoSuporte.ObterPorId(3);
            suporte.Nome = ConstantesTestes.OUTROS;
            
            var suporteAlterado = await servicoSuporte.Alterar(suporte);
            
            suporteAlterado.ShouldNotBeNull();
            suporteAlterado.Nome = ConstantesTestes.OUTROS;
        }
        
        [Fact(DisplayName = "Suporte - Excluir")]
        public async Task Excluir()
        {
            await InserirSuporte();
            
            var servicoSuporte = GetServicoSuporte();

            await servicoSuporte.Excluir(3);

            var acessosDocumentos = ObterTodos<Suporte>();
            acessosDocumentos.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            acessosDocumentos.Count(a=> !a.Excluido).ShouldBeEquivalentTo(4);
        }

        private async Task InserirSuporte()
        {
            await InserirNaBase(new Suporte() { Nome = ConstantesTestes.PAPEL, TipoSuporte = TipoSuporte.IMAGEM});
            await InserirNaBase(new Suporte() { Nome = ConstantesTestes.DIGITAL, TipoSuporte = TipoSuporte.IMAGEM});
            await InserirNaBase(new Suporte() { Nome = ConstantesTestes.PAPEL_DIGITAL, TipoSuporte = TipoSuporte.IMAGEM});
            await InserirNaBase(new Suporte() { Nome = ConstantesTestes.VHS, TipoSuporte = TipoSuporte.VIDEO});
            await InserirNaBase(new Suporte() { Nome = ConstantesTestes.DVD, TipoSuporte = TipoSuporte.VIDEO});
        }
    }
}