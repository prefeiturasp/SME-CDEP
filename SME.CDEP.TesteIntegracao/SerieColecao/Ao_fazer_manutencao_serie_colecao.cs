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
    public class Ao_fazer_manutencao_serie_colecao : TesteBase
    {
        public Ao_fazer_manutencao_serie_colecao(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Serie/Colecao - Inserir")]
        public async Task Inserir()
        {
            var servicoSerieColecao = GetServicoSerieColecao();
            
            var serieColecaoInserido = await servicoSerieColecao.Inserir(ObterIdNomeExcluidoAuditavelDTO(ConstantesTestes.PB));
            
            serieColecaoInserido.ShouldBeGreaterThan(0);
            var seriesColecoes = ObterTodos<SerieColecao>();
            seriesColecoes.Count.ShouldBe(1);
        }
      
        [Fact(DisplayName = "Serie/Colecao - Não deve inserir pois já existe cadastro com esse nome")]
        public async Task Nao_deve_inserir()
        {
            await InserirSerieColecao();
            
            var servicoSerieColecao = GetServicoSerieColecao();
            
            await servicoSerieColecao.Inserir(ObterIdNomeExcluidoAuditavelDTO(ConstantesTestes.COLOR)).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Serie/Colecao - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirSerieColecao();
            var servicoSerieColecao = GetServicoSerieColecao();

            var serieColecoes = await servicoSerieColecao.ObterTodos();
            serieColecoes.ShouldNotBeNull();
            serieColecoes.Count.ShouldBe(2);
        }

        [Fact(DisplayName = "Serie/Colecao - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirSerieColecao();
            
            var servicoSerieColecao = GetServicoSerieColecao();

            var serieColecaoDTO = await servicoSerieColecao.ObterPorId(1);
            serieColecaoDTO.ShouldNotBeNull();
            serieColecaoDTO.Id.ShouldBe(1);
            serieColecaoDTO.Nome.ShouldBe(ConstantesTestes.COLOR);
        }

        [Fact(DisplayName = "Serie/Colecao - Atualizar")]
        public async Task Atualizar()
        {
            await InserirSerieColecao();
            
            var servicoSerieColecao = GetServicoSerieColecao();

            var serieColecaoDTO = await servicoSerieColecao.ObterPorId(2);
            serieColecaoDTO.Nome = ConstantesTestes.TRANSPARENTE;
            
            var serieColecaoAlteradoDTO = await servicoSerieColecao.Alterar(serieColecaoDTO);
            
            serieColecaoAlteradoDTO.ShouldNotBeNull();
            serieColecaoAlteradoDTO.Nome = ConstantesTestes.TRANSPARENTE;
        }
        
        [Fact(DisplayName = "Serie/Colecao - Não deve alterar pois já existe cadastro com esse nome")]
        public async Task Nao_deve_atualizar_para_cadastros_duplicados()
        {
            await InserirSerieColecao();
            
            var servicoSerieColecao = GetServicoSerieColecao();

            var serieColecaoDTO = await servicoSerieColecao.ObterPorId(2);
            serieColecaoDTO.Nome = ConstantesTestes.COLOR;
            
            await servicoSerieColecao.Alterar(serieColecaoDTO).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Serie/Colecao - Excluir")]
        public async Task Excluir()
        {
            await InserirSerieColecao();
            
            var servicoSerieColecao = GetServicoSerieColecao();

            await servicoSerieColecao.Excluir(2);

            var serieColecoes = ObterTodos<SerieColecao>();
            serieColecoes.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            serieColecoes.Count(a=> !a.Excluido).ShouldBeEquivalentTo(1);
        }

        private async Task InserirSerieColecao()
        {
            await InserirNaBase(new SerieColecao() 
            { 
                Nome = ConstantesTestes.COLOR,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
            
            await InserirNaBase(new SerieColecao() 
            { 
                Nome = ConstantesTestes.PB,
                CriadoPor = ConstantesTestes.SISTEMA, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, 
                CriadoLogin = ConstantesTestes.LOGIN_123456789 
            });
        }
    }
}