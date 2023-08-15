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
    public class Ao_fazer_manutencao_autor : TesteBase
    {
        public Ao_fazer_manutencao_autor(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Autor - Inserir")]
        public async Task Inserir()
        {
            var servicoAutor = GetServicoAutor();
            
            var autores = await servicoAutor.Inserir(new IdNomeExcluidoAuditavelDTO() {Nome = ConstantesTestes.PB});
            
            autores.ShouldBeGreaterThan(0);
            var obterTodos = ObterTodos<Autor>();
            obterTodos.Count.ShouldBe(1);
        }
      
        [Fact(DisplayName = "Autor - Não deve inserir pois já existe cadastro com esse nome")]
        public async Task Nao_deve_inserir()
        {
            await InserirAutor();
            
            var servicoAutor = GetServicoAutor();
            
            await servicoAutor.Inserir(new IdNomeExcluidoAuditavelDTO() {Nome = ConstantesTestes.PB}).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Autor - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirAutor();
            var servicoAutor = GetServicoAutor();

            var autorDTO = await servicoAutor.ObterTodos();
            autorDTO.ShouldNotBeNull();
            autorDTO.Count.ShouldBe(2);
        }

        [Fact(DisplayName = "Autor - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirAutor();
            
            var servicoAutor = GetServicoAutor();

            var autorDTO = await servicoAutor.ObterPorId(1);
            autorDTO.ShouldNotBeNull();
            autorDTO.Id.ShouldBe(1);
            autorDTO.Nome.ShouldBe(ConstantesTestes.COLOR);
        }

        [Fact(DisplayName = "Autor - Atualizar")]
        public async Task Atualizar()
        {
            await InserirAutor();
            
            var servicoAutor = GetServicoAutor();

            var autorDTO = await servicoAutor.ObterPorId(2);
            autorDTO.Nome = ConstantesTestes.TRANSPARENTE;
            
            var autorAlteradaDTO = await servicoAutor.Alterar(autorDTO);
            
            autorAlteradaDTO.ShouldNotBeNull();
            autorAlteradaDTO.Nome = ConstantesTestes.TRANSPARENTE;
        }
        
        [Fact(DisplayName = "Autor - Não deve alterar pois já existe cadastro com esse nome")]
        public async Task Nao_deve_atualizar_para_cadastros_duplicados()
        {
            await InserirAutor();
            
            var servicoAutor = GetServicoAutor();

            var autorDTO = await servicoAutor.ObterPorId(2);
            autorDTO.Nome = ConstantesTestes.COLOR;
            
            await servicoAutor.Alterar(autorDTO).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Autor - Excluir")]
        public async Task Excluir()
        {
            await InserirAutor();
            
            var servicoAutor = GetServicoAutor();

            await servicoAutor.Excluir(2);

            var autores = ObterTodos<Autor>();
            autores.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            autores.Count(a=> !a.Excluido).ShouldBeEquivalentTo(1);
        }
        
        [Fact(DisplayName = "Autor - Pesquisar por Nome")]
        public async Task Pesquisar_por_nome()
        {
            var servicoAutor = GetServicoAutor();

            await InserirNaBase(new Autor() 
            { 
                Nome = ConstantesTestes.LIVRO,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new Autor() 
            { 
                Nome = ConstantesTestes.PAPEL,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new Autor() 
            { 
                Nome = ConstantesTestes.COLOR,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new Autor() 
            { 
                Nome = ConstantesTestes.VOB,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            var retorno = await servicoAutor.ObterPaginado("o");
            retorno.Items.Count().ShouldBe(ConstantesTestes.QUANTIDADE_3);
        }

        private async Task InserirAutor()
        {
            await InserirNaBase(new Autor() 
            { 
                Nome = ConstantesTestes.COLOR,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new Autor() 
            { 
                Nome = ConstantesTestes.PB,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
        }
    }
}