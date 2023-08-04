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
    public class Ao_fazer_manutencao_assunto : TesteBase
    {
        public Ao_fazer_manutencao_assunto(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Assunto - Inserir")]
        public async Task Inserir()
        {
            var servicoAssunto = GetServicoAssunto();
            
            var assuntos = await servicoAssunto.Inserir(ObterIdNomeExcluidoAuditavelDTO(ConstantesTestes.PB));
            
            assuntos.ShouldBeGreaterThan(0);
            var obterTodos = ObterTodos<Assunto>();
            obterTodos.Count.ShouldBe(1);
        }
      
        [Fact(DisplayName = "Assunto - Não deve inserir pois já existe cadastro com esse nome")]
        public async Task Nao_deve_inserir()
        {
            await InserirAssunto();
            
            var servicoAssunto = GetServicoAssunto();
            
            await servicoAssunto.Inserir(ObterIdNomeExcluidoAuditavelDTO(ConstantesTestes.COLOR)).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Assunto - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirAssunto();
            var servicoAssunto = GetServicoAssunto();

            var assuntoDTO = await servicoAssunto.ObterTodos();
            assuntoDTO.ShouldNotBeNull();
            assuntoDTO.Count.ShouldBe(2);
        }

        [Fact(DisplayName = "Assunto - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirAssunto();
            
            var servicoAssunto = GetServicoAssunto();

            var assuntoDTO = await servicoAssunto.ObterPorId(1);
            assuntoDTO.ShouldNotBeNull();
            assuntoDTO.Id.ShouldBe(1);
            assuntoDTO.Nome.ShouldBe(ConstantesTestes.COLOR);
        }

        [Fact(DisplayName = "Assunto - Atualizar")]
        public async Task Atualizar()
        {
            await InserirAssunto();
            
            var servicoAssunto = GetServicoAssunto();

            var assuntoDTO = await servicoAssunto.ObterPorId(2);
            assuntoDTO.Nome = ConstantesTestes.TRANSPARENTE;
            
            var editoraAssuntoDTO = await servicoAssunto.Alterar(assuntoDTO);
            
            editoraAssuntoDTO.ShouldNotBeNull();
            editoraAssuntoDTO.Nome = ConstantesTestes.TRANSPARENTE;
        }
        
        [Fact(DisplayName = "Assunto - Não deve alterar pois já existe cadastro com esse nome")]
        public async Task Nao_deve_atualizar_para_cadastros_duplicados()
        {
            await InserirAssunto();
            
            var servicoAssunto = GetServicoAssunto();

            var assuntoDTO = await servicoAssunto.ObterPorId(2);
            assuntoDTO.Nome = ConstantesTestes.COLOR;
            
            await servicoAssunto.Alterar(assuntoDTO).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Assunto - Excluir")]
        public async Task Excluir()
        {
            await InserirAssunto();
            
            var servicoAssunto = GetServicoAssunto();

            await servicoAssunto.Excluir(2);

            var assuntos = ObterTodos<Assunto>();
            assuntos.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            assuntos.Count(a=> !a.Excluido).ShouldBeEquivalentTo(1);
        }

        private async Task InserirAssunto()
        {
            await InserirNaBase(new Assunto() 
            { 
                Nome = ConstantesTestes.COLOR,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new Assunto() 
            { 
                Nome = ConstantesTestes.PB,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
        }
    }
}