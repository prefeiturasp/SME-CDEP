using Bogus;
using Shouldly;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
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

            var acervos = ObterTodos<Acervo>();

            var acervoFotograficoDtos = await servicoAcervo.ObterPorFiltro(
                (int)TipoAcervo.Bibliografico, acervos.FirstOrDefault().Titulo.Substring(1), null, string.Empty);
            
            acervoFotograficoDtos.ShouldNotBeNull();
            acervoFotograficoDtos.TotalPaginas.ShouldBeGreaterThan(0);
            acervoFotograficoDtos.TotalRegistros.ShouldBeLessThanOrEqualTo(35);
            acervoFotograficoDtos.Items.Count().ShouldBeLessThanOrEqualTo(10);
        }
        
        [Fact(DisplayName = "Acervo - Alterar")]
        public async Task Alterar()
        {
            await InserirDadosBasicos();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervoAlterar = (ObterTodos<Acervo>()).FirstOrDefault();
            acervoAlterar.Descricao = faker.Lorem.Text();
            acervoAlterar.Titulo = faker.Lorem.Sentence();
            acervoAlterar.SubTitulo = faker.Lorem.Sentence();
            
            var creditoAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w => w.AcervoId == acervoAlterar.Id);
            acervoAlterar.CreditosAutoresIds = creditoAutores.Take(2).Select(s => s.CreditoAutorId).ToArray();
            acervoAlterar.CoAutores = creditoAutores.Take(2).Select(s=> new CoAutor() { CreditoAutorId = s.CreditoAutorId, TipoAutoria = faker.Lorem.Word().Limite(15)}).ToList();
            
            var acervo = await servicoAcervo.Alterar(acervoAlterar);
            var acervoAlterado = (ObterTodos<Acervo>()).FirstOrDefault(w=> w.Id == acervoAlterar.Id);
            acervoAlterado.Codigo.ShouldBe(acervoAlterar.Codigo);
            acervoAlterado.CodigoNovo.ShouldBe(acervoAlterar.CodigoNovo);
            acervoAlterado.Titulo.ShouldBe(acervoAlterar.Titulo);
            acervoAlterado.Descricao.ShouldBe(acervoAlterar.Descricao);
            acervoAlterado.SubTitulo.ShouldBe(acervoAlterar.SubTitulo);
            
            var acervosCreditosAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w=> w.AcervoId == acervoAlterar.Id);
            acervosCreditosAutores.Count().ShouldBe(4);
           
            foreach (var creditoAutorId in acervoAlterar.CreditosAutoresIds)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == creditoAutorId && f.TipoAutoria is null).ShouldBeTrue();

            foreach (var coAutor in acervoAlterar.CoAutores)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == coAutor.CreditoAutorId && f.TipoAutoria == coAutor.TipoAutoria).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo - Alterar para sem CoAutor")]
        public async Task Alterar_sem_coautor()
        {
            await InserirDadosBasicos();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervoAlterar = (ObterTodos<Acervo>()).FirstOrDefault();
            acervoAlterar.Descricao = faker.Lorem.Text();
            acervoAlterar.Titulo = faker.Lorem.Sentence();
            acervoAlterar.SubTitulo = faker.Lorem.Sentence();
            
            var creditoAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w => w.AcervoId == acervoAlterar.Id);
            acervoAlterar.CreditosAutoresIds = creditoAutores.Take(2).Select(s => s.CreditoAutorId).ToArray();
            acervoAlterar.CoAutores = Enumerable.Empty<CoAutor>();
            
            var acervo = await servicoAcervo.Alterar(acervoAlterar);
            var acervoAlterado = (ObterTodos<Acervo>()).FirstOrDefault(w=> w.Id == acervoAlterar.Id);
            acervoAlterado.Codigo.ShouldBe(acervoAlterar.Codigo);
            acervoAlterado.CodigoNovo.ShouldBe(acervoAlterar.CodigoNovo);
            acervoAlterado.Titulo.ShouldBe(acervoAlterar.Titulo);
            acervoAlterado.Descricao.ShouldBe(acervoAlterar.Descricao);
            acervoAlterado.SubTitulo.ShouldBe(acervoAlterar.SubTitulo);
            acervoAlterar.CoAutores.Count().ShouldBe(0);
            var acervosCreditosAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w=> w.AcervoId == acervoAlterar.Id);
            acervosCreditosAutores.Count().ShouldBe(2);
           
            foreach (var creditoAutorId in acervoAlterar.CreditosAutoresIds)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == creditoAutorId && f.TipoAutoria is null).ShouldBeTrue();

        }
        
        [Fact(DisplayName = "Acervo - Não deve alterar para um código existente dentro do mesmo acervo")]
        public async Task Nao_deve_alterar_com_codigo_existente_dentro_mesmo_acervo()
        {
            await InserirDadosBasicos();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervos = ObterTodos<Acervo>();
            var acervoAlterar = (acervos).FirstOrDefault();
            acervoAlterar.Titulo = acervos.LastOrDefault().Titulo;
            acervoAlterar.SubTitulo = acervos.LastOrDefault().SubTitulo;
            acervoAlterar.Codigo = acervos.LastOrDefault().Codigo;
            
            var creditoAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w => w.AcervoId == acervoAlterar.Id);
            acervoAlterar.CreditosAutoresIds = creditoAutores.Take(2).Select(s => s.CreditoAutorId).ToArray();
            acervoAlterar.CoAutores = creditoAutores.Take(2).Select(s=> new CoAutor() { CreditoAutorId = s.CreditoAutorId, TipoAutoria = faker.Lorem.Word().Limite(15)}).ToList();
            
            await servicoAcervo.Alterar(acervoAlterar).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo - Deve alterar para um códigos diferentes")]
        public async Task Deve_alterar_com_codigo_diferentes()
        {
            await InserirDadosBasicos();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();
            
            var acervos = ObterTodos<Acervo>();
            var acervoAlterar = (acervos).FirstOrDefault();
            acervoAlterar.Titulo = acervos.LastOrDefault().Titulo;
            acervoAlterar.Descricao = faker.Lorem.Text();
            acervoAlterar.SubTitulo = acervos.LastOrDefault().SubTitulo;
            acervoAlterar.Codigo = "196";
            
            var creditoAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w => w.AcervoId == acervoAlterar.Id);
            acervoAlterar.CreditosAutoresIds = creditoAutores.Take(2).Select(s => s.CreditoAutorId).ToArray();
            acervoAlterar.CoAutores = creditoAutores.Take(2).Select(s=> new CoAutor() { CreditoAutorId = s.CreditoAutorId, TipoAutoria = faker.Lorem.Word()}).ToList();
            
            var acervo = await servicoAcervo.Alterar(acervoAlterar);
            var acervoAlterado = (ObterTodos<Acervo>()).FirstOrDefault(w=> w.Id == acervoAlterar.Id);
            acervoAlterado.Codigo.ShouldBe(acervoAlterar.Codigo);
            acervoAlterado.CodigoNovo.ShouldBe(acervoAlterar.CodigoNovo);
            acervoAlterado.Titulo.ShouldBe(acervoAlterar.Titulo);
            acervoAlterado.Descricao.ShouldBe(acervoAlterar.Descricao);
            acervoAlterado.SubTitulo.ShouldBe(acervoAlterar.SubTitulo);
            
            var acervosCreditosAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w=> w.AcervoId == acervoAlterar.Id);
            acervosCreditosAutores.Count().ShouldBe(4);
           
            foreach (var creditoAutorId in acervoAlterar.CreditosAutoresIds)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == creditoAutorId && f.TipoAutoria is null).ShouldBeTrue();

            foreach (var coAutor in acervoAlterar.CoAutores)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == coAutor.CreditoAutorId && f.TipoAutoria == coAutor.TipoAutoria).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo - Deve alterar para códigos iguais em acervos diferentes")]
        public async Task Deve_alterar_com_codigo_iguais_e_acervos_diferentes()
        {
            await InserirDadosBasicos();
            await InserirAcervo(TipoAcervo.DocumentacaoHistorica);
            var servicoAcervo = GetServicoAcervo();
            
            var acervos = ObterTodos<Acervo>();
            var acervoAlterar = (acervos).FirstOrDefault();
            acervoAlterar.Titulo = acervos.LastOrDefault().Titulo;
            acervoAlterar.Descricao = faker.Lorem.Text();
            acervoAlterar.SubTitulo = acervos.LastOrDefault().SubTitulo;
            acervoAlterar.Codigo = acervos.FirstOrDefault().Codigo;
            
            var creditoAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w => w.AcervoId == acervoAlterar.Id);
            acervoAlterar.CreditosAutoresIds = creditoAutores.Take(2).Select(s => s.CreditoAutorId).ToArray();
            acervoAlterar.CoAutores = creditoAutores.Take(2).Select(s=> new CoAutor() { CreditoAutorId = s.CreditoAutorId, TipoAutoria = faker.Lorem.Word()}).ToList();
            
            var acervo = await servicoAcervo.Alterar(acervoAlterar);
            var acervoAlterado = (ObterTodos<Acervo>()).FirstOrDefault(w=> w.Id == acervoAlterar.Id);
            acervoAlterado.Codigo.ShouldBe(acervoAlterar.Codigo);
            acervoAlterado.CodigoNovo.ShouldBe(acervoAlterar.CodigoNovo);
            acervoAlterado.Titulo.ShouldBe(acervoAlterar.Titulo);
            acervoAlterado.Descricao.ShouldBe(acervoAlterar.Descricao);
            acervoAlterado.SubTitulo.ShouldBe(acervoAlterar.SubTitulo);
            
            var acervosCreditosAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w=> w.AcervoId == acervoAlterar.Id);
            acervosCreditosAutores.Count().ShouldBe(4);
           
            foreach (var creditoAutorId in acervoAlterar.CreditosAutoresIds)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == creditoAutorId && f.TipoAutoria is null).ShouldBeTrue();

            foreach (var coAutor in acervoAlterar.CoAutores)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == coAutor.CreditoAutorId && f.TipoAutoria == coAutor.TipoAutoria).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo - Alterar adicionando mais créditos/autores")]
        public async Task Alterar_adicionando_mais_creditos_autores()
        {
            await InserirDadosBasicos();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();
            
            var acervos = ObterTodos<Acervo>();
            var acervoAlterar = (acervos).FirstOrDefault();
            acervoAlterar.Titulo = acervos.LastOrDefault().Titulo;
            acervoAlterar.Descricao = faker.Lorem.Text();
            acervoAlterar.SubTitulo = acervos.LastOrDefault().SubTitulo;
            acervoAlterar.Codigo = "196";
            
            var creditoAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w => w.AcervoId == acervoAlterar.Id);
            acervoAlterar.CreditosAutoresIds = creditoAutores.Take(3).Select(s => s.CreditoAutorId).ToArray();
            acervoAlterar.CoAutores = creditoAutores.Take(3).Select(s=> new CoAutor() { CreditoAutorId = s.CreditoAutorId, TipoAutoria = faker.Lorem.Word().Limite(15)}).ToList();
            
            var acervo = await servicoAcervo.Alterar(acervoAlterar);
            var acervoAlterado = (ObterTodos<Acervo>()).FirstOrDefault(w=> w.Id == acervoAlterar.Id);
            acervoAlterado.Codigo.ShouldBe(acervoAlterar.Codigo);
            acervoAlterado.CodigoNovo.ShouldBe(acervoAlterar.CodigoNovo);
            acervoAlterado.Titulo.ShouldBe(acervoAlterar.Titulo);
            acervoAlterado.Descricao.ShouldBe(acervoAlterar.Descricao);
            acervoAlterado.SubTitulo.ShouldBe(acervoAlterar.SubTitulo);
            
            var acervosCreditosAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w=> w.AcervoId == acervoAlterar.Id);
            acervosCreditosAutores.Count().ShouldBe(6);

            foreach (var creditoAutorId in acervoAlterar.CreditosAutoresIds)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == creditoAutorId && f.TipoAutoria is null).ShouldBeTrue();

            foreach (var coAutor in acervoAlterar.CoAutores)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == coAutor.CreditoAutorId && f.TipoAutoria == coAutor.TipoAutoria).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo - Alterar excluindo todos e adicionando um créditos/autores")]
        public async Task Alterar_excluindo_todos_adicionando_um_creditos_autores()
        {
            await InserirDadosBasicos();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();
            
            var acervos = ObterTodos<Acervo>();
            var acervoAlterar = (acervos).FirstOrDefault();
            acervoAlterar.Titulo = acervos.LastOrDefault().Titulo;
            acervoAlterar.Descricao = faker.Lorem.Text();
            acervoAlterar.SubTitulo = acervos.LastOrDefault().SubTitulo;
            acervoAlterar.Codigo = "196";
            
            var creditoAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w => w.AcervoId == acervoAlterar.Id);
            acervoAlterar.CreditosAutoresIds = creditoAutores.Take(1).Select(s => s.CreditoAutorId).ToArray();
            acervoAlterar.CoAutores = creditoAutores.Take(1).Select(s=> new CoAutor() { CreditoAutorId = s.CreditoAutorId, TipoAutoria = faker.Lorem.Word().Limite(15)}).ToList();
            
            var acervo = await servicoAcervo.Alterar(acervoAlterar);
            var acervoAlterado = (ObterTodos<Acervo>()).FirstOrDefault(w=> w.Id == acervoAlterar.Id);
            acervoAlterado.Codigo.ShouldBe(acervoAlterar.Codigo);
            acervoAlterado.CodigoNovo.ShouldBe(acervoAlterar.CodigoNovo);
            acervoAlterado.Titulo.ShouldBe(acervoAlterar.Titulo);
            acervoAlterado.Descricao.ShouldBe(acervoAlterar.Descricao);
            acervoAlterado.SubTitulo.ShouldBe(acervoAlterar.SubTitulo);
            
            var acervosCreditosAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w=> w.AcervoId == acervoAlterar.Id);
            acervosCreditosAutores.Count().ShouldBe(2);
           
            foreach (var creditoAutorId in acervoAlterar.CreditosAutoresIds)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == creditoAutorId && f.TipoAutoria is null).ShouldBeTrue();

            foreach (var coAutor in acervoAlterar.CoAutores)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == coAutor.CreditoAutorId && f.TipoAutoria == coAutor.TipoAutoria).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo - Inserir")]
        public async Task Inserir()
        {
            await InserirDadosBasicos();
            var servicoAcervo = GetServicoAcervo();
            var random = new Random();
            
            var acervoInserir = GerarAcervo(TipoAcervo.Bibliografico).Generate();
            acervoInserir.CreditosAutoresIds = new long []{1,2,3,4,5};
            acervoInserir.CoAutores = new List<CoAutor>() { new () { CreditoAutorId = random.Next(1,5), TipoAutoria = faker.Lorem.Word().Limite(15)}}.ToList();
            
            var acervoId = await servicoAcervo.Inserir(acervoInserir);
            var acervoinserido = (ObterTodos<Acervo>()).FirstOrDefault(w=> w.Id == acervoId);
            acervoinserido.Codigo.ShouldBe(acervoInserir.Codigo);
            acervoinserido.Titulo.ShouldBe(acervoInserir.Titulo);
            acervoinserido.Descricao.ShouldBe(acervoInserir.Descricao);
            acervoinserido.SubTitulo.ShouldBe(acervoInserir.SubTitulo);
            
            var acervosCreditosAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w=> w.AcervoId == acervoId);
            acervosCreditosAutores.Count().ShouldBe(6);
           
            foreach (var creditoAutorId in acervoInserir.CreditosAutoresIds)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == creditoAutorId && f.TipoAutoria is null).ShouldBeTrue();

            foreach (var coAutor in acervoInserir.CoAutores)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == coAutor.CreditoAutorId && f.TipoAutoria == coAutor.TipoAutoria).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo - Inserir sem CoAutor")]
        public async Task Inserir_sem_coAutor()
        {
            await InserirDadosBasicos();
            var servicoAcervo = GetServicoAcervo();
            
            var acervoInserir = GerarAcervo(TipoAcervo.Bibliografico).Generate();
            acervoInserir.CreditosAutoresIds = new long []{1,2,3,4,5};
            acervoInserir.CoAutores = Enumerable.Empty<CoAutor>();
            
            var acervoId = await servicoAcervo.Inserir(acervoInserir);
            var acervoinserido = (ObterTodos<Acervo>()).FirstOrDefault(w=> w.Id == acervoId);
            acervoinserido.Codigo.ShouldBe(acervoInserir.Codigo);
            acervoinserido.Titulo.ShouldBe(acervoInserir.Titulo);
            acervoinserido.Descricao.ShouldBe(acervoInserir.Descricao);
            acervoinserido.SubTitulo.ShouldBe(acervoInserir.SubTitulo);
            
            var acervosCreditosAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w=> w.AcervoId == acervoId);
            acervosCreditosAutores.Count().ShouldBe(5);
           
            foreach (var creditoAutorId in acervoInserir.CreditosAutoresIds)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == creditoAutorId && f.TipoAutoria is null).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo - Não deve inserir código existente dentro de mesmo acervo")]
        public async Task Nao_deve_inserir_codigo_existente_dentro_mesmo_acervo()
        {
            await InserirDadosBasicos();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();
            
            var acervoInserir = GerarAcervo(TipoAcervo.Bibliografico).Generate();
            acervoInserir.CreditosAutoresIds = new long []{1,2,3,4,5};
            acervoInserir.CoAutores = new List<CoAutor>() { new () { CreditoAutorId = new Random().Next(1,5), TipoAutoria = faker.Lorem.Word().Limite(15)}}.ToList();

            var acervos = ObterTodos<Acervo>();
            acervoInserir.Titulo = acervos.LastOrDefault().Titulo;
            acervoInserir.SubTitulo = acervos.LastOrDefault().SubTitulo;
            acervoInserir.Codigo = acervos.LastOrDefault().Codigo;
            
            await servicoAcervo.Inserir(acervoInserir).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo - Deve inserir com códigos iguais e acervos diferentes")]
        public async Task Deve_inserir_com_codigo_iguais_e_acervos_diferentes()
        {
            await InserirDadosBasicos();
            await InserirAcervo(TipoAcervo.DocumentacaoHistorica);
            var servicoAcervo = GetServicoAcervo();
            
            var acervoInserir = GerarAcervo(TipoAcervo.Bibliografico).Generate();
            acervoInserir.CreditosAutoresIds = new long []{1,2,3,4,5};
            acervoInserir.CoAutores = new List<CoAutor>() { new () { CreditoAutorId = new Random().Next(1,5), TipoAutoria = faker.Lorem.Word().Limite(15)}}.ToList();

            var acervos = ObterTodos<Acervo>();
            acervoInserir.Titulo = acervos.LastOrDefault().Titulo;
            acervoInserir.Descricao = faker.Lorem.Text();
            acervoInserir.SubTitulo = acervos.LastOrDefault().SubTitulo;
            acervoInserir.Codigo = acervos.FirstOrDefault().Codigo;

            var acervoId = await servicoAcervo.Inserir(acervoInserir);
            var acervoinserido = (ObterTodos<Acervo>()).FirstOrDefault(w=> w.Id == acervoId);
            acervoinserido.Codigo.ShouldBe(acervoInserir.Codigo);
            acervoinserido.Titulo.ShouldBe(acervoInserir.Titulo);
            acervoinserido.Descricao.ShouldBe(acervoInserir.Descricao);
            acervoinserido.SubTitulo.ShouldBe(acervoInserir.SubTitulo);
            
            var acervosCreditosAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w=> w.AcervoId == acervoId);
            acervosCreditosAutores.Count().ShouldBe(6);
           
            foreach (var creditoAutorId in acervoInserir.CreditosAutoresIds)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == creditoAutorId && f.TipoAutoria is null).ShouldBeTrue();

            foreach (var coAutor in acervoInserir.CoAutores)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == coAutor.CreditoAutorId && f.TipoAutoria == coAutor.TipoAutoria).ShouldBeTrue();
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
            acervo.Titulo.ShouldNotBeNull();
            acervo.Codigo.ShouldNotBeNull();
            acervo.SubTitulo.ShouldNotBeNull();
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

        private async Task InserirAcervo(TipoAcervo tipoAcervo = TipoAcervo.Bibliografico)
        {
            for (int j = 1; j <= 35; j++)
            {
                await InserirNaBase(new Acervo()
                {
                    Codigo = j.ToString(),
                    Titulo = faker.Lorem.Sentence(),
                    Descricao = faker.Lorem.Text(),
                    SubTitulo = faker.Lorem.Sentence(),
                    TipoAcervoId = (int)tipoAcervo,
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
                
                await InserirNaBase(new AcervoCreditoAutor()
                {
                    AcervoId = j,
                    CreditoAutorId = 1,
                    TipoAutoria = faker.Lorem.Word().Limite(15)
                });
                
                await InserirNaBase(new AcervoCreditoAutor()
                {
                    AcervoId = j,
                    CreditoAutorId = 2,
                    TipoAutoria = faker.Lorem.Word().Limite(15)
                });
            }
        }
    }
}