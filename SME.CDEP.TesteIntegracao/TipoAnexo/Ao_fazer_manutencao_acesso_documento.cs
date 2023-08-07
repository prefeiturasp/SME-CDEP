using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;

namespace SME.CDEP.TesteIntegracao.Usuario
{
    public class Ao_fazer_manutencao_tipo_anexo : TesteBase
    {
        public Ao_fazer_manutencao_tipo_anexo(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Tipo Anexo - Inserir")]
        public async Task Inserir()
        {
            var servicoTipoAnexo = GetServicoTipoAnexo();

            var tipoAnexo = await servicoTipoAnexo.Inserir(new BaseComNomeDTO(){Nome = ConstantesTestes.DIGITAL_E_FISICO});
            tipoAnexo.ShouldBeGreaterThan(0);
            var obterTodos = ObterTodos<TipoAnexo>();
            obterTodos.Count.ShouldBe(1);
        }

        [Fact(DisplayName = "Tipo Anexo - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirTipoAnexo();
            var servicoTipoAnexo = GetServicoTipoAnexo();

            var acessoDocumentoDtos = await servicoTipoAnexo.ObterTodos();
            acessoDocumentoDtos.ShouldNotBeNull();
            acessoDocumentoDtos.Count.ShouldBe(7);
        }

        [Fact(DisplayName = "Tipo Anexo - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirTipoAnexo();
            
            var servicoTipoAnexo = GetServicoTipoAnexo();

            var tipoAnexo = await servicoTipoAnexo.ObterPorId(1);
            tipoAnexo.ShouldNotBeNull();
            tipoAnexo.Id.ShouldBe(1);
            tipoAnexo.Nome.ShouldBe(ConstantesTestes.ANEXO);
        }

        [Fact(DisplayName = "Tipo Anexo - Atualizar")]
        public async Task Atualizar()
        {
            await InserirTipoAnexo();
            
            var servicoTipoAnexo = GetServicoTipoAnexo();

            var tipoAnexo = await servicoTipoAnexo.ObterPorId(3);
            tipoAnexo.Nome = ConstantesTestes.OUTROS;
            
            var tipoAnexoAlterado = await servicoTipoAnexo.Alterar(tipoAnexo);
            
            tipoAnexoAlterado.ShouldNotBeNull();
            tipoAnexoAlterado.Nome = ConstantesTestes.OUTROS;
        }
        
        [Fact(DisplayName = "Tipo Anexo - Excluir")]
        public async Task Excluir()
        {
            await InserirTipoAnexo();
            
            var servicoTipoAnexo = GetServicoTipoAnexo();

            await servicoTipoAnexo.Excluir(3);

            var tipoAnexos = ObterTodos<TipoAnexo>();
            tipoAnexos.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            tipoAnexos.Count(a=> !a.Excluido).ShouldBeEquivalentTo(6);
        }

        private async Task InserirTipoAnexo()
        {
            await InserirNaBase(new TipoAnexo() { Nome = ConstantesTestes.ANEXO });
            await InserirNaBase(new TipoAnexo() { Nome = ConstantesTestes.AUDIOVISUAL });
            await InserirNaBase(new TipoAnexo() { Nome = ConstantesTestes.CD });
            await InserirNaBase(new TipoAnexo() { Nome = ConstantesTestes.DISQUETE });
            await InserirNaBase(new TipoAnexo() { Nome = ConstantesTestes.DVD });
            await InserirNaBase(new TipoAnexo() { Nome = ConstantesTestes.ENCARTE });
            await InserirNaBase(new TipoAnexo() { Nome = ConstantesTestes.FITAS_DE_VIDEO });
        }
    }
}