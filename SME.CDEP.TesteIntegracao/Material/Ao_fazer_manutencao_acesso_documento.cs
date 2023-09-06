using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_fazer_manutencao_material : TesteBase
    {
        public Ao_fazer_manutencao_material(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Material - Inserir")]
        public async Task Inserir()
        {
            var servicoMaterial = GetServicoMaterial();

            var material = await servicoMaterial.Inserir(new IdNomeTipoExcluidoDTO(){Nome = ConstantesTestes.APOSTILA});
            material.ShouldBeGreaterThan(0);
            var obterTodos = ObterTodos<Material>();
            obterTodos.Count.ShouldBe(1);
        }
        
        [Fact(DisplayName = "Material - Não deve inserir pois já existe cadastro com esse nome")]
        public async Task Nao_deve_para_cadastros_duplicados()
        {
            await InserirMaterial();
            
            var servicoMaterial = GetServicoMaterial();

            await servicoMaterial.Inserir(new IdNomeTipoExcluidoDTO(){Nome = ConstantesTestes.APOSTILA, Tipo = (int)TipoMaterial.DOCUMENTAL}).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Material - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirMaterial();
            var servicoMaterial = GetServicoMaterial();

            var materiais = await servicoMaterial.ObterTodos();
            materiais.ShouldNotBeNull();
            materiais.Count.ShouldBe(8);
        }

        [Fact(DisplayName = "Material - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirMaterial();
            
            var servicoMaterial = GetServicoMaterial();

            var material = await servicoMaterial.ObterPorId(1);
            material.ShouldNotBeNull();
            material.Id.ShouldBe(1);
            material.Nome.ShouldBe(ConstantesTestes.APOSTILA);
        }

        [Fact(DisplayName = "Material - Atualizar")]
        public async Task Atualizar()
        {
            await InserirMaterial();
            
            var servicoMaterial = GetServicoMaterial();

            var material = await servicoMaterial.ObterPorId(3);
            material.Nome = ConstantesTestes.OUTROS;
            
            var materialAlterado = await servicoMaterial.Alterar(material);
            
            materialAlterado.ShouldNotBeNull();
            materialAlterado.Nome = ConstantesTestes.OUTROS;
        }
        
        [Fact(DisplayName = "Material - Não deve alterar pois já existe cadastro com esse nome")]
        public async Task Nao_deve_atualizar_para_cadastros_duplicados()
        {
            await InserirMaterial();
            
            var servicoMaterial = GetServicoMaterial();

            var material = await servicoMaterial.ObterPorId(3);
            material.Nome = ConstantesTestes.PERIODICO;
            
            await servicoMaterial.Alterar(material).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Material - Excluir")]
        public async Task Excluir()
        {
            await InserirMaterial();
            
            var servicoMaterial = GetServicoMaterial();

            await servicoMaterial.Excluir(3);

            var materiais = ObterTodos<Material>();
            materiais.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            materiais.Count(a=> !a.Excluido).ShouldBeEquivalentTo(7);
        }

        private async Task InserirMaterial()
        {
            await InserirNaBase(new Material() { Nome = ConstantesTestes.APOSTILA, Tipo = TipoMaterial.DOCUMENTAL});
            await InserirNaBase(new Material() { Nome = ConstantesTestes.LIVRO, Tipo = TipoMaterial.DOCUMENTAL});
            await InserirNaBase(new Material() { Nome = ConstantesTestes.CADERNO, Tipo = TipoMaterial.DOCUMENTAL});
            await InserirNaBase(new Material() { Nome = ConstantesTestes.PERIODICO, Tipo = TipoMaterial.DOCUMENTAL});
            await InserirNaBase(new Material() { Nome = ConstantesTestes.REVISTA, Tipo = TipoMaterial.DOCUMENTAL});
            await InserirNaBase(new Material() { Nome = ConstantesTestes.LIVRO, Tipo = TipoMaterial.BIBLIOGRAFICO});
            await InserirNaBase(new Material() { Nome = ConstantesTestes.TESE, Tipo = TipoMaterial.BIBLIOGRAFICO});
            await InserirNaBase(new Material() { Nome = ConstantesTestes.PERIODICO, Tipo = TipoMaterial.BIBLIOGRAFICO});
        }
    }
}