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
    public class Ao_fazer_manutencao_editora : TesteBase
    {
        public Ao_fazer_manutencao_editora(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Editora - Inserir")]
        public async Task Inserir()
        {
            var servicoEditora = GetServicoEditora();
            
            var editoras = await servicoEditora.Inserir(new IdNomeExcluidoAuditavelDTO() {Nome = ConstantesTestes.PB});
            
            editoras.ShouldBeGreaterThan(0);
            var obterTodos = ObterTodos<Editora>();
            obterTodos.Count().ShouldBe(1);
        }
      
        [Fact(DisplayName = "Editora - Não deve inserir pois já existe cadastro com esse nome")]
        public async Task Nao_deve_inserir()
        {
            await InserirEditora();
            
            var servicoEditora = GetServicoEditora();
            
            await servicoEditora.Inserir(new IdNomeExcluidoAuditavelDTO() {Nome = ConstantesTestes.PB}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Editora - Não deve inserir com nome nulo")]
        public async Task Nao_deve_inserir_nulo()
        {
            var servicoEditora = GetServicoEditora();
            
            await servicoEditora.Inserir(new IdNomeExcluidoAuditavelDTO()).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Editora - Não deve inserir com nome vazio")]
        public async Task Nao_deve_inserir_vazio()
        {
            var servicoEditora = GetServicoEditora();
	
            await servicoEditora.Inserir(new IdNomeExcluidoAuditavelDTO() { Nome = "     "}).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Editora - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirEditora();
            var servicoEditora = GetServicoEditora();

            var editoraDTO = await servicoEditora.ObterTodos();
            editoraDTO.ShouldNotBeNull();
            editoraDTO.Count().ShouldBe(2);
        }

        [Fact(DisplayName = "Editora - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirEditora();
            
            var servicoEditora = GetServicoEditora();

            var editoraDTO = await servicoEditora.ObterPorId(1);
            editoraDTO.ShouldNotBeNull();
            editoraDTO.Id.ShouldBe(1);
            editoraDTO.Nome.ShouldBe(ConstantesTestes.COLOR);
        }

        [Fact(DisplayName = "Editora - Atualizar")]
        public async Task Atualizar()
        {
            await InserirEditora();
            
            var servicoEditora = GetServicoEditora();

            var editoraDTO = await servicoEditora.ObterPorId(2);
            editoraDTO.Nome = ConstantesTestes.TRANSPARENTE;
            
            var editoraAlteradaDTO = await servicoEditora.Alterar(editoraDTO);
            
            editoraAlteradaDTO.ShouldNotBeNull();
            editoraAlteradaDTO.Nome = ConstantesTestes.TRANSPARENTE;
        }
        
        [Fact(DisplayName = "Editora - Não deve alterar pois já existe cadastro com esse nome")]
        public async Task Nao_deve_atualizar_para_cadastros_duplicados()
        {
            await InserirEditora();
            
            var servicoEditora = GetServicoEditora();

            var editoraDTO = await servicoEditora.ObterPorId(2);
            editoraDTO.Nome = ConstantesTestes.COLOR;
            
            await servicoEditora.Alterar(editoraDTO).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Editora - Não deve alterar pois já o nome é nulo")]
        public async Task Nao_deve_atualizar_para_nome_nulo()
        {
            await InserirEditora();
            
            var servicoEditora = GetServicoEditora();

            var editoraDTO = await servicoEditora.ObterPorId(2);
            editoraDTO.Nome = null;
            
            await servicoEditora.Alterar(editoraDTO).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Editora - Não deve alterar pois já o nome é vazio")]
        public async Task Nao_deve_atualizar_para_nome_vazio()
        {
            await InserirEditora();
            
            var servicoEditora = GetServicoEditora();

            var editoraDTO = await servicoEditora.ObterPorId(2);
            editoraDTO.Nome = "            ";
            
            await servicoEditora.Alterar(editoraDTO).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Editora - Excluir")]
        public async Task Excluir()
        {
            await InserirEditora();
            
            var servicoEditora = GetServicoEditora();

            await servicoEditora.Excluir(2);

            var editoras = ObterTodos<Editora>();
            editoras.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            editoras.Count(a=> !a.Excluido).ShouldBeEquivalentTo(1);
        }
        
        [Fact(DisplayName = "Editora - Pesquisar por Nome")]
        public async Task Pesquisar_por_nome()
        {
            await InserirNaBase(new Editora() 
            { 
                Nome = ConstantesTestes.LIVRO,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new Editora() 
            { 
                Nome = ConstantesTestes.PAPEL,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new Editora() 
            { 
                Nome = ConstantesTestes.COLOR,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new Editora() 
            { 
                Nome = ConstantesTestes.VOB,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            var servicoEditora = GetServicoEditora();

            var retorno = await servicoEditora.ObterPaginado("o");
            retorno.Items.Count().ShouldBe(ConstantesTestes.QUANTIDADE_3);
        }

        private async Task InserirEditora()
        {
            await InserirNaBase(new Editora() 
            { 
                Nome = ConstantesTestes.COLOR,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new Editora() 
            { 
                Nome = ConstantesTestes.PB,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
        }
    }
}