using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
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

            var suporte = await servicoSuporte.Inserir(new IdNomeTipoExcluidoDTO(){Nome = ConstantesTestes.DIGITAL_E_FISICO, Tipo = (int)TipoSuporte.VIDEO});
            suporte.ShouldBeGreaterThan(0);
            var obterTodos = ObterTodos<Suporte>();
            obterTodos.Count().ShouldBe(1);
        }
        
        [Fact(DisplayName = "Suporte - Não deve inserir pois já existe cadastro com esse nome")]
        public async Task Nao_deve_para_cadastros_duplicados()
        {
            await InserirSuporte();
            
            var servicoSuporte = GetServicoSuporte();

            await servicoSuporte.Inserir(new IdNomeTipoExcluidoDTO(){Nome = ConstantesTestes.PAPEL, Tipo = (int)TipoSuporte.IMAGEM}).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Suporte - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirSuporte();
            var servicoSuporte = GetServicoSuporte();

            var suporteDtos = await servicoSuporte.ObterTodos();
            suporteDtos.ShouldNotBeNull();
            suporteDtos.Count().ShouldBe(5);
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
        
        [Fact(DisplayName = "Suporte - Não deve alterar pois já existe cadastro com esse nome")]
        public async Task Nao_deve_atualizar_para_cadastros_duplicados()
        {
            await InserirSuporte();
            
            var servicoSuporte = GetServicoSuporte();

            var suporte = await servicoSuporte.ObterPorId(3);
            suporte.Nome = ConstantesTestes.PAPEL;
            
            await servicoSuporte.Alterar(suporte).ShouldThrowAsync<NegocioException>();
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
            await InserirNaBase(new Suporte() { Nome = ConstantesTestes.PAPEL, Tipo = TipoSuporte.IMAGEM});
            await InserirNaBase(new Suporte() { Nome = ConstantesTestes.DIGITAL, Tipo = TipoSuporte.IMAGEM});
            await InserirNaBase(new Suporte() { Nome = ConstantesTestes.PAPEL_DIGITAL, Tipo = TipoSuporte.IMAGEM});
            await InserirNaBase(new Suporte() { Nome = ConstantesTestes.VHS, Tipo = TipoSuporte.VIDEO});
            await InserirNaBase(new Suporte() { Nome = ConstantesTestes.DVD, Tipo = TipoSuporte.VIDEO});
        }
    }
}