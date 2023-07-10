using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;

namespace SME.CDEP.TesteIntegracao.Usuario
{
    public class Ao_fazer_manutencao_conservacao : TesteBase
    {
        public Ao_fazer_manutencao_conservacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Conservacao - Inserir")]
        public async Task Inserir()
        {
            var servicoConservacao = GetServicoConservacao();

            var conservacaoDTO = await servicoConservacao.Inserir(new ConservacaoDTO(){Nome = ConstantesTestes.OTIMO});
            conservacaoDTO.ShouldBeGreaterThan(0);
            var obterTodos = ObterTodos<Conservacao>();
            obterTodos.Count.ShouldBe(1);
        }

        [Fact(DisplayName = "Conservacao - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirConservacao();
            var servicoConservacao = GetServicoConservacao();

            var conservacaoDTOs = await servicoConservacao.ObterTodos();
            conservacaoDTOs.ShouldNotBeNull();
            conservacaoDTOs.Count.ShouldBe(4);
        }

        [Fact(DisplayName = "Conservacao - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirConservacao();
            
            var servicoConservacao = GetServicoConservacao();

            var acessoDocumentoDto = await servicoConservacao.ObterPorId(1);
            acessoDocumentoDto.ShouldNotBeNull();
            acessoDocumentoDto.Id.ShouldBe(1);
            acessoDocumentoDto.Nome.ShouldBe(ConstantesTestes.OTIMO);
        }

        [Fact(DisplayName = "Conservacao - Atualizar")]
        public async Task Atualizar()
        {
            await InserirConservacao();
            
            var servicoConservacao = GetServicoConservacao();

            var acessoDocumentoDto = await servicoConservacao.ObterPorId(3);
            acessoDocumentoDto.Nome = ConstantesTestes.EXCELENTE;
            
            var acessosDocumentosDto = await servicoConservacao.Alterar(acessoDocumentoDto);
            
            acessosDocumentosDto.ShouldNotBeNull();
            acessosDocumentosDto.Nome = ConstantesTestes.EXCELENTE;
        }
        
        [Fact(DisplayName = "Conservacao - Excluir")]
        public async Task Excluir()
        {
            await InserirConservacao();
            
            var servicoConservacao = GetServicoConservacao();

            await servicoConservacao.Excluir(4);

            var acessosDocumentos = ObterTodos<Conservacao>();
            acessosDocumentos.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            acessosDocumentos.Count(a=> !a.Excluido).ShouldBeEquivalentTo(3);
        }

        private async Task InserirConservacao()
        {
            await InserirNaBase(new Conservacao() { Nome = ConstantesTestes.OTIMO });
            await InserirNaBase(new Conservacao() { Nome = ConstantesTestes.BOM });
            await InserirNaBase(new Conservacao() { Nome = ConstantesTestes.REGULAR });
            await InserirNaBase(new Conservacao() { Nome = ConstantesTestes.RUIM });
        }
    }
}