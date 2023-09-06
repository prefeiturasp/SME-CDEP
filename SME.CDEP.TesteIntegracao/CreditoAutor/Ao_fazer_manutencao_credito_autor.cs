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
    public class Ao_fazer_manutencao_credito_autor : TesteBase
    {
        public Ao_fazer_manutencao_credito_autor(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "CréditoAutor - Inserir")]
        public async Task Inserir()
        {
            var servicoCreditoAutor = GetServicoCreditoAutor();
            
            var credito = await servicoCreditoAutor.Inserir(new IdNomeTipoExcluidoAuditavelDTO() {Nome = ConstantesTestes.PB, Tipo = (int)TipoCreditoAutoria.Credito});
            
            credito.ShouldBeGreaterThan(0);
            var obterTodos = ObterTodos<CreditoAutor>();
            obterTodos.Count.ShouldBe(1);
        }
      
        [Fact(DisplayName = "CréditoAutor - Não deve inserir pois já existe cadastro com esse nome")]
        public async Task Nao_deve_inserir()
        {
            await InserirCredito();
            
            var servicoCreditoAutor = GetServicoCreditoAutor();
            
            await servicoCreditoAutor.Inserir(new IdNomeTipoExcluidoAuditavelDTO() {Nome = ConstantesTestes.PB, Tipo = (int)TipoCreditoAutoria.Credito}).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "CréditoAutor - Não deve inserir com nome nulo")]
        public async Task Nao_deve_inserir_nulo()
        {
            var servicoCreditoAutor = GetServicoCreditoAutor();
            
            await servicoCreditoAutor.Inserir(new IdNomeTipoExcluidoAuditavelDTO()).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "CréditoAutor - Não deve inserir com nome vazio")]
        public async Task Nao_deve_inserir_vazio()
        {
            var servicoCreditoAutor = GetServicoCreditoAutor();
            
            await servicoCreditoAutor.Inserir(new IdNomeTipoExcluidoAuditavelDTO() {Nome = "     "}).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "CréditoAutor - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirCredito();
            var servicoCreditoAutor = GetServicoCreditoAutor();

            var creditoDTO = await servicoCreditoAutor.ObterTodos();
            creditoDTO.ShouldNotBeNull();
            creditoDTO.Count.ShouldBe(2);
        }

        [Fact(DisplayName = "CréditoAutor - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirCredito();
            
            var servicoCreditoAutor = GetServicoCreditoAutor();

            var creditoDTO = await servicoCreditoAutor.ObterPorId(1);
            creditoDTO.ShouldNotBeNull();
            creditoDTO.Id.ShouldBe(1);
            creditoDTO.Nome.ShouldBe(ConstantesTestes.COLOR);
        }

        [Fact(DisplayName = "CréditoAutor - Atualizar")]
        public async Task Atualizar()
        {
            await InserirCredito();
            
            var servicoCreditoAutor = GetServicoCreditoAutor();

            var creditoDTO = await servicoCreditoAutor.ObterPorId(2);
            creditoDTO.Nome = ConstantesTestes.TRANSPARENTE;
            
            var creditoAlteradaDTO = await servicoCreditoAutor.Alterar(creditoDTO);
            
            creditoAlteradaDTO.ShouldNotBeNull();
            creditoAlteradaDTO.Nome = ConstantesTestes.TRANSPARENTE;
        }
        
        [Fact(DisplayName = "CréditoAutor - Não deve alterar pois já existe cadastro com esse nome")]
        public async Task Nao_deve_atualizar_para_cadastros_duplicados()
        {
            await InserirCredito();
            
            var servicoCreditoAutor = GetServicoCreditoAutor();

            var creditoDTO = await servicoCreditoAutor.ObterPorId(2);
            creditoDTO.Nome = ConstantesTestes.COLOR;
            
            await servicoCreditoAutor.Alterar(creditoDTO).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "CréditoAutor - Não deve alterar pois já o nome é nulo")]
        public async Task Nao_deve_atualizar_para_nome_nulo()
        {
            await InserirCredito();
            
            var servicoCreditoAutor = GetServicoCreditoAutor();

            var creditoDTO = await servicoCreditoAutor.ObterPorId(2);
            creditoDTO.Nome = null;
            
            await servicoCreditoAutor.Alterar(creditoDTO).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "CréditoAutor - Não deve alterar pois já o nome é vazio")]
        public async Task Nao_deve_atualizar_para_nome_vazio()
        {
            await InserirCredito();
            
            var servicoCreditoAutor = GetServicoCreditoAutor();

            var creditoDTO = await servicoCreditoAutor.ObterPorId(2);
            creditoDTO.Nome = "     ";
            
            await servicoCreditoAutor.Alterar(creditoDTO).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "CréditoAutor - Excluir")]
        public async Task Excluir()
        {
            await InserirCredito();
            
            var servicoCreditoAutor = GetServicoCreditoAutor();

            await servicoCreditoAutor.Excluir(2);

            var creditos = ObterTodos<CreditoAutor>();
            creditos.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            creditos.Count(a=> !a.Excluido).ShouldBeEquivalentTo(1);
        }
        
        [Fact(DisplayName = "CréditoAutor - Pesquisar por Nome")]
        public async Task Pesquisar_por_nome()
        {
            var servicoCreditoAutor = GetServicoCreditoAutor();

            await InserirNaBase(new CreditoAutor() 
            { 
                Nome = ConstantesTestes.LIVRO,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new CreditoAutor() 
            { 
                Nome = ConstantesTestes.PAPEL,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new CreditoAutor() 
            { 
                Nome = ConstantesTestes.COLOR,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new CreditoAutor() 
            { 
                Nome = ConstantesTestes.VOB,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            var retorno = await servicoCreditoAutor.ObterPaginado("o");
            retorno.Items.Count().ShouldBe(ConstantesTestes.QUANTIDADE_3);
        }

        private async Task InserirCredito()
        {
            await InserirNaBase(new CreditoAutor() 
            { 
                Nome = ConstantesTestes.COLOR,
                CriadoPor = ConstantesTestes.SISTEMA, 
                Tipo = TipoCreditoAutoria.Credito,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new CreditoAutor() 
            { 
                Nome = ConstantesTestes.PB,
                Tipo = TipoCreditoAutoria.Autoria,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
        }
    }
}