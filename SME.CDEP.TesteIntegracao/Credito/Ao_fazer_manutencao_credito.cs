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
    public class Ao_fazer_manutencao_credito : TesteBase
    {
        public Ao_fazer_manutencao_credito(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Crédito - Inserir")]
        public async Task Inserir()
        {
            var servicoCredito = GetServicoCredito();
            
            var credito = await servicoCredito.Inserir(ObterIdNomeExcluidoAuditavelDTO(ConstantesTestes.PB));
            
            credito.ShouldBeGreaterThan(0);
            var obterTodos = ObterTodos<Credito>();
            obterTodos.Count.ShouldBe(1);
        }
      
        [Fact(DisplayName = "Crédito - Não deve inserir pois já existe cadastro com esse nome")]
        public async Task Nao_deve_inserir()
        {
            await InserirCredito();
            
            var servicoCredito = GetServicoCredito();
            
            await servicoCredito.Inserir(ObterIdNomeExcluidoAuditavelDTO(ConstantesTestes.COLOR)).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Crédito - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirCredito();
            var servicoCredito = GetServicoCredito();

            var creditoDTO = await servicoCredito.ObterTodos();
            creditoDTO.ShouldNotBeNull();
            creditoDTO.Count.ShouldBe(2);
        }

        [Fact(DisplayName = "Crédito - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirCredito();
            
            var servicoCredito = GetServicoCredito();

            var creditoDTO = await servicoCredito.ObterPorId(1);
            creditoDTO.ShouldNotBeNull();
            creditoDTO.Id.ShouldBe(1);
            creditoDTO.Nome.ShouldBe(ConstantesTestes.COLOR);
        }

        [Fact(DisplayName = "Crédito - Atualizar")]
        public async Task Atualizar()
        {
            await InserirCredito();
            
            var servicoCredito = GetServicoCredito();

            var creditoDTO = await servicoCredito.ObterPorId(2);
            creditoDTO.Nome = ConstantesTestes.TRANSPARENTE;
            
            var creditoAlteradaDTO = await servicoCredito.Alterar(creditoDTO);
            
            creditoAlteradaDTO.ShouldNotBeNull();
            creditoAlteradaDTO.Nome = ConstantesTestes.TRANSPARENTE;
        }
        
        [Fact(DisplayName = "Crédito - Não deve alterar pois já existe cadastro com esse nome")]
        public async Task Nao_deve_atualizar_para_cadastros_duplicados()
        {
            await InserirCredito();
            
            var servicoCredito = GetServicoCredito();

            var creditoDTO = await servicoCredito.ObterPorId(2);
            creditoDTO.Nome = ConstantesTestes.COLOR;
            
            await servicoCredito.Alterar(creditoDTO).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Crédito - Excluir")]
        public async Task Excluir()
        {
            await InserirCredito();
            
            var servicoCredito = GetServicoCredito();

            await servicoCredito.Excluir(2);

            var creditos = ObterTodos<Credito>();
            creditos.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            creditos.Count(a=> !a.Excluido).ShouldBeEquivalentTo(1);
        }
        
        [Fact(DisplayName = "Crédito - Pesquisar por Nome")]
        public async Task Pesquisar_por_nome()
        {
            await InserirCredito();
            
            var servicoCredito = GetServicoCredito();

            var retorno = await servicoCredito.PesquisarPorNome(ConstantesTestes.COLOR.Substring(3));
            retorno.ShouldNotBeNull();
            retorno.Nome.ShouldBe(ConstantesTestes.COLOR);
            retorno.Id.ShouldBe(long.Parse(ConstantesTestes.NUMERO_1));
        }

        private async Task InserirCredito()
        {
            await InserirNaBase(new Credito() 
            { 
                Nome = ConstantesTestes.COLOR,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new Credito() 
            { 
                Nome = ConstantesTestes.PB,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
        }
    }
}