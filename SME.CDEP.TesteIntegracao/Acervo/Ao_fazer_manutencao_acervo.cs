using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;
using Xunit.Sdk;

namespace SME.CDEP.TesteIntegracao
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
        
        [Fact(DisplayName = "Acervo - Alterar")]
        public async Task Alterar()
        {
            await InserirDadosBasicos();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervo = await servicoAcervo.Alterar(new Acervo()
            {
                Id = 1,
                Codigo = "150",
                Titulo = string.Format(ConstantesTestes.TITULO_X,150),
                CreditosAutoresIds = new long[]{1,2},
                TipoAcervoId = (int)TipoAcervo.Fotografico,  
                CriadoPor = ConstantesTestes.SISTEMA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            acervo.ShouldNotBeNull();
            var acervosCreditosAutores = ObterTodos<AcervoCreditoAutor>();
            acervosCreditosAutores.Where(w=> w.AcervoId == 1).Count().ShouldBe(2);
            acervosCreditosAutores.Where(w=> w.AcervoId == 1).FirstOrDefault().CreditoAutorId.ShouldBe(1);
            acervosCreditosAutores.Where(w=> w.AcervoId == 1).LastOrDefault().CreditoAutorId.ShouldBe(2);
        }
        
        [Fact(DisplayName = "Acervo - Alterar adicionando mais créditos/autores")]
        public async Task Alterar_adicionando_mais_creditos_autores()
        {
            await InserirDadosBasicos();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervo = await servicoAcervo.Alterar(new Acervo()
            {
                Id = 1,
                Codigo = "150",
                Titulo = string.Format(ConstantesTestes.TITULO_X,150),
                CreditosAutoresIds = new long[]{1,2,3,4,5},
                TipoAcervoId = (int)TipoAcervo.Fotografico,  
                CriadoPor = ConstantesTestes.SISTEMA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            acervo.ShouldNotBeNull();
            var acervosCreditosAutores = ObterTodos<AcervoCreditoAutor>();
            acervosCreditosAutores.Where(w=> w.AcervoId == 1).Count().ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo - Alterar excluindo todos e adicionando um créditos/autores")]
        public async Task Alterar_excluindo_todos_adicionando_um_creditos_autores()
        {
            await InserirDadosBasicos();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervo = await servicoAcervo.Alterar(new Acervo()
            {
                Id = 1,
                Codigo = "150",
                Titulo = string.Format(ConstantesTestes.TITULO_X,150),
                CreditosAutoresIds = new long[]{5},
                TipoAcervoId = (int)TipoAcervo.Fotografico,  
                CriadoPor = ConstantesTestes.SISTEMA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoLogin = ConstantesTestes.LOGIN_123456789
            });
            
            acervo.ShouldNotBeNull();
            var acervosCreditosAutores = ObterTodos<AcervoCreditoAutor>();
            acervosCreditosAutores.Where(w=> w.AcervoId == 1).Count().ShouldBe(1);
        }
        
        [Fact(DisplayName = "Acervo - Inserir")]
        public async Task Inserir()
        {
            await InserirDadosBasicos();
            var servicoAcervo = GetServicoAcervo();

            var acervo = await servicoAcervo.Inserir(new Acervo()
            {
                Codigo = "1",
                Titulo = string.Format(ConstantesTestes.TITULO_X,1),
                CreditosAutoresIds = new long[]{1,2},
                TipoAcervoId = (int)TipoAcervo.Fotografico,    
            });
            
            acervo.ShouldBeGreaterThan(0);
            var acervos = ObterTodos<Acervo>();
            acervos.Count().ShouldBe(1);
            
            var acervosCreditosAutores = ObterTodos<AcervoCreditoAutor>();
            acervosCreditosAutores.Count().ShouldBe(2);
        }
        
        [Fact(DisplayName = "Acervo - Não deve inserir pois já existe cadastro com esse título")]
        public async Task Nao_deve_inserir_duplicado()
        {
            await InserirDadosBasicos();

            await InserirAcervo();

            var servicoAcervo = GetServicoAcervo();
            
            await servicoAcervo.Inserir(new Acervo()
            {
                Codigo = "1",
                Titulo = string.Format(ConstantesTestes.TITULO_X,1),
                CreditosAutoresIds = new long[]{1,2},
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
            for (int j = 1; j <= 35; j++)
            {
                await InserirNaBase(new Acervo()
                {
                    Codigo = j.ToString(),
                    Titulo = string.Format(ConstantesTestes.TITULO_X, j),
                    TipoAcervoId = (int)TipoAcervo.Fotografico,
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoLogin = ConstantesTestes.LOGIN_123456789
                });
                
                await InserirNaBase(new AcervoCreditoAutor()
                {
                    AcervoId = j,
                    CreditoAutorId = 1
                });
                
                await InserirNaBase(new AcervoCreditoAutor()
                {
                    AcervoId = j,
                    CreditoAutorId = 2
                });
                
                await InserirNaBase(new AcervoCreditoAutor()
                {
                    AcervoId = j,
                    CreditoAutorId = 3
                });
            }
        }
    }
}