using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;

namespace SME.CDEP.TesteIntegracao.Usuario
{
    public class Ao_fazer_manutencao_formato : TesteBase
    {
        public Ao_fazer_manutencao_formato(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Formato - Inserir")]
        public async Task Inserir()
        {
            var servicoFormato = GetServicoFormato();

            var formato = await servicoFormato.Inserir(new FormatoDTO(){Nome = ConstantesTestes.PAPEL});
            formato.ShouldBeGreaterThan(0);
            var obterTodos = ObterTodos<Formato>();
            obterTodos.Count.ShouldBe(1);
        }

        [Fact(DisplayName = "Formato - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirFormatos();
            var servicoFormato = GetServicoFormato();

            var formatos = await servicoFormato.ObterTodos();
            formatos.ShouldNotBeNull();
            formatos.Count.ShouldBe(3);
        }

        [Fact(DisplayName = "Formato - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirFormatos();
            
            var servicoFormato = GetServicoFormato();

            var formato = await servicoFormato.ObterPorId(1);
            formato.ShouldNotBeNull();
            formato.Id.ShouldBe(1);
            formato.Nome.ShouldBe(ConstantesTestes.PAPEL);
        }

        [Fact(DisplayName = "Formato - Atualizar")]
        public async Task Atualizar()
        {
            await InserirFormatos();
            
            var servicoFormato = GetServicoFormato();

            var formato = await servicoFormato.ObterPorId(3);
            formato.Nome = ConstantesTestes.IMPRESSO;
            
            var acessosDocumentosDto = await servicoFormato.Alterar(formato);
            
            acessosDocumentosDto.ShouldNotBeNull();
            acessosDocumentosDto.Nome = ConstantesTestes.IMPRESSO;
        }
        
        [Fact(DisplayName = "Formato - Excluir")]
        public async Task Excluir()
        {
            await InserirFormatos();
            
            var servicoFormato = GetServicoFormato();

            await servicoFormato.Excluir(3);

            var acessosDocumentos = ObterTodos<Formato>();
            acessosDocumentos.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            acessosDocumentos.Count(a=> !a.Excluido).ShouldBeEquivalentTo(2);
        }

        private async Task InserirFormatos()
        {
            await InserirNaBase(new Formato() { Nome = ConstantesTestes.PAPEL });
            await InserirNaBase(new Formato() { Nome = ConstantesTestes.DIGITAL });
            await InserirNaBase(new Formato() { Nome = ConstantesTestes.PAPEL_DIGITAL });
        }
    }
}