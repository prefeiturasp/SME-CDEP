using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;
using Xunit.Sdk;

namespace SME.CDEP.TesteIntegracao.Usuario
{
    public class Ao_fazer_manutencao_acervo : TesteBase
    {
        public Ao_fazer_manutencao_acervo(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirDadosBasicos();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervoFotograficoDtos = await servicoAcervo.ObterTodos();
            acervoFotograficoDtos.ShouldNotBeNull();
        }

        [Fact(DisplayName = "Acervo - Obter paginado")]
        public async Task Obter_paginado()
        {
            await InserirDadosBasicos();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervoFotograficoDtos = await servicoAcervo.ObterPorFiltro(
                (int)TipoAcervo.Fotografico, "títu", null, string.Empty);
            
            acervoFotograficoDtos.ShouldNotBeNull();
            acervoFotograficoDtos.TotalPaginas.ShouldBe(4);
            acervoFotograficoDtos.TotalRegistros.ShouldBe(35);
            acervoFotograficoDtos.Items.Count().ShouldBe(10);
        }
        
        [Fact(DisplayName = "Acervo - Inserir")]
        public async Task Inserir()
        {
            await InserirDadosBasicos();
            var servicoAcervo = GetServicoAcervo();

            var acervo = await servicoAcervo.Inserir(new Acervo()
            {
                Codigo = string.Format(ConstantesTestes.CODIGO_X,1),
                Titulo = string.Format(ConstantesTestes.TITULO_X,1),
                CreditoAutorId = new Random().Next(1, 5),
                TipoAcervoId = (int)TipoAcervo.Fotografico,    
            });
            
            acervo.ShouldBeGreaterThan(0);
            var acervos = ObterTodos<Acervo>();
            acervos.Count().ShouldBe(1);
        }
        
        [Fact(DisplayName = "Acervo - Não deve inserir pois já existe cadastro com esse título")]
        public async Task Nao_deve_inserir_duplicado()
        {
            await InserirDadosBasicos();

            await InserirAcervo();

            var servicoAcervo = GetServicoAcervo();

            await servicoAcervo.Inserir(new Acervo()
            {
                Codigo = string.Format(ConstantesTestes.CODIGO_X,1),
                Titulo = string.Format(ConstantesTestes.TITULO_X,1),
                CreditoAutorId = new Random().Next(1, 5),
                TipoAcervoId = (int)TipoAcervo.Fotografico,    
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo - Obter por id")]
        public async Task Obter_por_id()
        {
            await InserirDadosBasicos();

            await InserirAcervo();
            
            var servicoAcervo = GetServicoAcervo();

            var acervo = await servicoAcervo.ObterPorId(1);
            acervo.ShouldNotBeNull();
            acervo.Id.ShouldBe(1);
            acervo.Titulo.ShouldBe(string.Format(ConstantesTestes.TITULO_X, 1));
        }
        
        [Fact(DisplayName = "Acervo - Atualizar")]
        public async Task Atualizar()
        {
            // await InserirDadosBasicos();
            //
            // await InserirAcervo();
            //
            // var servicoAcervo = GetServicoAcervo();
            //
            // var acervoDto = await servicoAcervo.ObterPorId(3);
            // acervoDto.Titulo = string.Format(ConstantesTestes.TITULO_X, 100);
            //
            // var acervoAlterado = await servicoAcervo.Alterar(acervoDto);
            //
            // acervoAlterado.ShouldNotBeNull();
            // acervoAlterado.Titulo = string.Format(ConstantesTestes.TITULO_X, 100);
        }
        
        [Fact(DisplayName = "Acervo - Excluir")]
        public async Task Excluir()
        {
            await InserirDadosBasicos();

            await InserirAcervo();
            
            var servicoAcervo = GetServicoAcervo();

            await servicoAcervo.Excluir(3);

            var acervos = ObterTodos<Acervo>();
            acervos.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            acervos.Count(a=> !a.Excluido).ShouldBeEquivalentTo(34);
        }

        private async Task InserirAcervo()
        {
            var random = new Random();

            for (int j = 1; j <= 35; j++)
            {
                await InserirNaBase(new Acervo()
                {
                    Codigo = string.Format(ConstantesTestes.CODIGO_X, j),
                    Titulo = string.Format(ConstantesTestes.TITULO_X, j),
                    CreditoAutorId = random.Next(1, 5),
                    TipoAcervoId = (int)TipoAcervo.Fotografico,
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoLogin = ConstantesTestes.LOGIN_123456789
                });
            }
        }
    }
}