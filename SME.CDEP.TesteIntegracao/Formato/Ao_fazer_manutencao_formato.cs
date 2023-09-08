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
    public class Ao_fazer_manutencao_formato : TesteBase
    {
        public Ao_fazer_manutencao_formato(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Formato - Inserir")]
        public async Task Inserir()
        {
            var servicoFormato = GetServicoFormato();

            var formato = await servicoFormato.Inserir(new IdNomeTipoExcluidoDTO(){Nome = ConstantesTestes.PAPEL});
            formato.ShouldBeGreaterThan(0);
            var obterTodos = ObterTodos<Formato>();
            obterTodos.Count().ShouldBe(1);
        }
        
        [Fact(DisplayName = "Formato - Não deve inserir pois já existe cadastro com esse nome")]
        public async Task Nao_deve_para_cadastros_duplicados()
        {
            await InserirFormatos();
            
            var servicoFormato = GetServicoFormato();

            await servicoFormato.Inserir(new IdNomeTipoExcluidoDTO(){Nome = ConstantesTestes.VOB,Tipo = (int)TipoFormato.ACERVO_AUDIOVISUAL}).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Formato - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirFormatos();
            var servicoFormato = GetServicoFormato();

            var formatos = await servicoFormato.ObterTodos();
            formatos.ShouldNotBeNull();
            formatos.Count().ShouldBe(4);
        }

        [Fact(DisplayName = "Formato - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirFormatos();
            
            var servicoFormato = GetServicoFormato();

            var formato = await servicoFormato.ObterPorId(1);
            formato.ShouldNotBeNull();
            formato.Id.ShouldBe(1);
            formato.Nome.ShouldBe(ConstantesTestes.JPEG);
            formato.Tipo.ShouldBe((int)TipoFormato.ACERVO_FOTOS);
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
        
        [Fact(DisplayName = "Formato - Não deve alterar pois já existe cadastro com esse nome")]
        public async Task Nao_deve_atualizar_para_cadastros_duplicados()
        {
            await InserirFormatos();
            
            var servicoFormato = GetServicoFormato();

            var formato = await servicoFormato.ObterPorId(3);
            formato.Nome = ConstantesTestes.VOB;
            
            await servicoFormato.Alterar(formato).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Formato - Excluir")]
        public async Task Excluir()
        {
            await InserirFormatos();
            
            var servicoFormato = GetServicoFormato();

            await servicoFormato.Excluir(3);

            var acessosDocumentos = ObterTodos<Formato>();
            acessosDocumentos.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            acessosDocumentos.Count(a=> !a.Excluido).ShouldBeEquivalentTo(3);
        }

        private async Task InserirFormatos()
        {
            await InserirNaBase(new Formato() { Nome = ConstantesTestes.JPEG, Tipo = TipoFormato.ACERVO_FOTOS });
            await InserirNaBase(new Formato() { Nome = ConstantesTestes.TIFF, Tipo = TipoFormato.ACERVO_FOTOS });
            await InserirNaBase(new Formato() { Nome = ConstantesTestes.VDF, Tipo = TipoFormato.ACERVO_AUDIOVISUAL });
            await InserirNaBase(new Formato() { Nome = ConstantesTestes.VOB, Tipo = TipoFormato.ACERVO_AUDIOVISUAL });
        }
    }
}