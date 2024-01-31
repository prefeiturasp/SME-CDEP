using Shouldly;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_fazer_manutencao_acervo : TesteBase
    {
        public Ao_fazer_manutencao_acervo(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervoFotograficoDtos = await servicoAcervo.ObterTodos();
            acervoFotograficoDtos.ShouldNotBeNull();
        }

        [Fact(DisplayName = "Acervo - Obter paginado")]
        public async Task Obter_paginado()
        {
            await InserirDadosBasicosAleatorios();
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
        
        [Fact(DisplayName = "Acervo - Alterar com década certa")]
        public async Task Alterar_com_decada_certa()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervoAlterar = (ObterTodos<Acervo>()).FirstOrDefault();
            acervoAlterar.Descricao = faker.Lorem.Text();
            acervoAlterar.Titulo = faker.Lorem.Sentence();
            acervoAlterar.SubTitulo = faker.Lorem.Sentence();
            acervoAlterar.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString();
            acervoAlterar.Ano = "[197-]";
            
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
            acervoAlterado.DataAcervo.ShouldBe(acervoAlterar.DataAcervo);
            acervoAlterado.Ano.ShouldBe(acervoAlterar.Ano);
            acervoAlterado.AnoInicio.ShouldBe(1970);
            acervoAlterado.AnoFim.ShouldBe(1979);
            
            var acervosCreditosAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w=> w.AcervoId == acervoAlterar.Id);
            acervosCreditosAutores.Count().ShouldBe(4);
           
            foreach (var creditoAutorId in acervoAlterar.CreditosAutoresIds)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == creditoAutorId && f.TipoAutoria is null).ShouldBeTrue();

            foreach (var coAutor in acervoAlterar.CoAutores)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == coAutor.CreditoAutorId && f.TipoAutoria == coAutor.TipoAutoria).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo - Alterar com década possível")]
        public async Task Alterar_com_decada_possivel()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervoAlterar = (ObterTodos<Acervo>()).FirstOrDefault();
            acervoAlterar.Descricao = faker.Lorem.Text();
            acervoAlterar.Titulo = faker.Lorem.Sentence();
            acervoAlterar.SubTitulo = faker.Lorem.Sentence();
            acervoAlterar.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString();
            acervoAlterar.Ano = "[197-?]";
            
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
            acervoAlterado.DataAcervo.ShouldBe(acervoAlterar.DataAcervo);
            acervoAlterado.Ano.ShouldBe(acervoAlterar.Ano);
            acervoAlterado.AnoInicio.ShouldBe(1970);
            acervoAlterado.AnoFim.ShouldBe(1979);
            
            var acervosCreditosAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w=> w.AcervoId == acervoAlterar.Id);
            acervosCreditosAutores.Count().ShouldBe(4);
           
            foreach (var creditoAutorId in acervoAlterar.CreditosAutoresIds)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == creditoAutorId && f.TipoAutoria is null).ShouldBeTrue();

            foreach (var coAutor in acervoAlterar.CoAutores)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == coAutor.CreditoAutorId && f.TipoAutoria == coAutor.TipoAutoria).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo - Alterar com século certo")]
        public async Task Alterar_com_seculo_certo()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervoAlterar = (ObterTodos<Acervo>()).FirstOrDefault();
            acervoAlterar.Descricao = faker.Lorem.Text();
            acervoAlterar.Titulo = faker.Lorem.Sentence();
            acervoAlterar.SubTitulo = faker.Lorem.Sentence();
            acervoAlterar.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString();
            acervoAlterar.Ano = "[190--]";
            
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
            acervoAlterado.DataAcervo.ShouldBe(acervoAlterar.DataAcervo);
            acervoAlterado.Ano.ShouldBe(acervoAlterar.Ano);
            acervoAlterado.AnoInicio.ShouldBe(1900);
            acervoAlterado.AnoFim.ShouldBe(1999);
            
            var acervosCreditosAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w=> w.AcervoId == acervoAlterar.Id);
            acervosCreditosAutores.Count().ShouldBe(4);
           
            foreach (var creditoAutorId in acervoAlterar.CreditosAutoresIds)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == creditoAutorId && f.TipoAutoria is null).ShouldBeTrue();

            foreach (var coAutor in acervoAlterar.CoAutores)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == coAutor.CreditoAutorId && f.TipoAutoria == coAutor.TipoAutoria).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo - Alterar com século provável")]
        public async Task Alterar_com_seculo_provavel()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervoAlterar = (ObterTodos<Acervo>()).FirstOrDefault();
            acervoAlterar.Descricao = faker.Lorem.Text();
            acervoAlterar.Titulo = faker.Lorem.Sentence();
            acervoAlterar.SubTitulo = faker.Lorem.Sentence();
            acervoAlterar.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString();
            acervoAlterar.Ano = "[19--?]";
            
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
            acervoAlterado.DataAcervo.ShouldBe(acervoAlterar.DataAcervo);
            acervoAlterado.Ano.ShouldBe(acervoAlterar.Ano);
            acervoAlterado.AnoInicio.ShouldBe(1900);
            acervoAlterado.AnoFim.ShouldBe(1999);
            
            var acervosCreditosAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w=> w.AcervoId == acervoAlterar.Id);
            acervosCreditosAutores.Count().ShouldBe(4);
           
            foreach (var creditoAutorId in acervoAlterar.CreditosAutoresIds)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == creditoAutorId && f.TipoAutoria is null).ShouldBeTrue();

            foreach (var coAutor in acervoAlterar.CoAutores)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == coAutor.CreditoAutorId && f.TipoAutoria == coAutor.TipoAutoria).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo - Alterar com ano exato")]
        public async Task Alterar_com_ano_exato()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervoAlterar = (ObterTodos<Acervo>()).FirstOrDefault();
            acervoAlterar.Descricao = faker.Lorem.Text();
            acervoAlterar.Titulo = faker.Lorem.Sentence();
            acervoAlterar.SubTitulo = faker.Lorem.Sentence();
            acervoAlterar.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString();
            acervoAlterar.Ano = "[1995]";
            
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
            acervoAlterado.DataAcervo.ShouldBe(acervoAlterar.DataAcervo);
            acervoAlterado.Ano.ShouldBe(acervoAlterar.Ano);
            acervoAlterado.AnoInicio.ShouldBe(1995);
            acervoAlterado.AnoFim.ShouldBe(1995);
            
            var acervosCreditosAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w=> w.AcervoId == acervoAlterar.Id);
            acervosCreditosAutores.Count().ShouldBe(4);
           
            foreach (var creditoAutorId in acervoAlterar.CreditosAutoresIds)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == creditoAutorId && f.TipoAutoria is null).ShouldBeTrue();

            foreach (var coAutor in acervoAlterar.CoAutores)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == coAutor.CreditoAutorId && f.TipoAutoria == coAutor.TipoAutoria).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo - Alterar com ano exato e terminando com zero")]
        public async Task Alterar_com_ano_exato_terminando_com_zero()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervoAlterar = (ObterTodos<Acervo>()).FirstOrDefault();
            acervoAlterar.Descricao = faker.Lorem.Text();
            acervoAlterar.Titulo = faker.Lorem.Sentence();
            acervoAlterar.SubTitulo = faker.Lorem.Sentence();
            acervoAlterar.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString();
            acervoAlterar.Ano = "[1990]";
            
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
            acervoAlterado.DataAcervo.ShouldBe(acervoAlterar.DataAcervo);
            acervoAlterado.Ano.ShouldBe(acervoAlterar.Ano);
            acervoAlterado.AnoInicio.ShouldBe(1990);
            acervoAlterado.AnoFim.ShouldBe(1990);
            
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
            await InserirDadosBasicosAleatorios();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervoAlterar = (ObterTodos<Acervo>()).FirstOrDefault();
            acervoAlterar.Descricao = faker.Lorem.Text();
            acervoAlterar.Titulo = faker.Lorem.Sentence();
            acervoAlterar.SubTitulo = faker.Lorem.Sentence();
            acervoAlterar.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString();
            acervoAlterar.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
            
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
            acervoAlterado.DataAcervo.ShouldBe(acervoAlterar.DataAcervo);
            acervoAlterado.Ano.ShouldBe(acervoAlterar.Ano);
            acervoAlterar.CoAutores.Count().ShouldBe(0);
            var acervosCreditosAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w=> w.AcervoId == acervoAlterar.Id);
            acervosCreditosAutores.Count().ShouldBe(2);
           
            foreach (var creditoAutorId in acervoAlterar.CreditosAutoresIds)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == creditoAutorId && f.TipoAutoria is null).ShouldBeTrue();

        }
        
        [Fact(DisplayName = "Acervo - Não deve alterar para um código existente dentro do mesmo acervo")]
        public async Task Nao_deve_alterar_com_codigo_existente_dentro_mesmo_acervo()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervos = ObterTodos<Acervo>();
            var acervoAlterar = (acervos).FirstOrDefault();
            acervoAlterar.Titulo = acervos.LastOrDefault().Titulo;
            acervoAlterar.SubTitulo = acervos.LastOrDefault().SubTitulo;
            acervoAlterar.Codigo = acervos.LastOrDefault().Codigo;
            acervoAlterar.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString();
            acervoAlterar.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
            
            var creditoAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w => w.AcervoId == acervoAlterar.Id);
            acervoAlterar.CreditosAutoresIds = creditoAutores.Take(2).Select(s => s.CreditoAutorId).ToArray();
            acervoAlterar.CoAutores = creditoAutores.Take(2).Select(s=> new CoAutor() { CreditoAutorId = s.CreditoAutorId, TipoAutoria = faker.Lorem.Word().Limite(15)}).ToList();
            
            await servicoAcervo.Alterar(acervoAlterar).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo - Não deve alterar para um ano futuro")]
        public async Task Nao_deve_alterar_para_um_ano_futuro()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();

            var acervos = ObterTodos<Acervo>();
            var acervoAlterar = (acervos).FirstOrDefault();
            acervoAlterar.Titulo = acervos.LastOrDefault().Titulo;
            acervoAlterar.SubTitulo = acervos.LastOrDefault().SubTitulo;
            acervoAlterar.Codigo = acervos.LastOrDefault().Codigo;
            acervoAlterar.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString();
            acervoAlterar.Ano = faker.Date.Future().Year.ToString();
            
            var creditoAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w => w.AcervoId == acervoAlterar.Id);
            acervoAlterar.CreditosAutoresIds = creditoAutores.Take(2).Select(s => s.CreditoAutorId).ToArray();
            acervoAlterar.CoAutores = creditoAutores.Take(2).Select(s=> new CoAutor() { CreditoAutorId = s.CreditoAutorId, TipoAutoria = faker.Lorem.Word().Limite(15)}).ToList();
            
            await servicoAcervo.Alterar(acervoAlterar).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo - Deve alterar para um códigos diferentes")]
        public async Task Deve_alterar_com_codigo_diferentes()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();
            
            var acervos = ObterTodos<Acervo>();
            var acervoAlterar = (acervos).FirstOrDefault();
            acervoAlterar.Titulo = acervos.LastOrDefault().Titulo;
            acervoAlterar.Descricao = faker.Lorem.Text();
            acervoAlterar.SubTitulo = acervos.LastOrDefault().SubTitulo;
            acervoAlterar.Codigo = "196";
            acervoAlterar.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString();
            acervoAlterar.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
            
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
            acervoAlterado.DataAcervo.ShouldBe(acervoAlterar.DataAcervo);
            acervoAlterado.Ano.ShouldBe(acervoAlterar.Ano);
            
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
            await InserirDadosBasicosAleatorios();
            await InserirAcervo(TipoAcervo.DocumentacaoHistorica);
            var servicoAcervo = GetServicoAcervo();
            
            var acervos = ObterTodos<Acervo>();
            var acervoAlterar = (acervos).FirstOrDefault();
            acervoAlterar.Titulo = acervos.LastOrDefault().Titulo;
            acervoAlterar.Descricao = faker.Lorem.Text();
            acervoAlterar.SubTitulo = acervos.LastOrDefault().SubTitulo;
            acervoAlterar.Codigo = acervos.FirstOrDefault().Codigo;
            acervoAlterar.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString();
            acervoAlterar.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
            
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
            acervoAlterado.DataAcervo.ShouldBe(acervoAlterar.DataAcervo);
            acervoAlterado.Ano.ShouldBe(acervoAlterar.Ano);
            
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
            await InserirDadosBasicosAleatorios();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();
            
            var acervos = ObterTodos<Acervo>();
            var acervoAlterar = (acervos).FirstOrDefault();
            acervoAlterar.Titulo = acervos.LastOrDefault().Titulo;
            acervoAlterar.Descricao = faker.Lorem.Text();
            acervoAlterar.SubTitulo = acervos.LastOrDefault().SubTitulo;
            acervoAlterar.Codigo = "196";
            acervoAlterar.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString();
            acervoAlterar.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
            
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
            acervoAlterado.DataAcervo.ShouldBe(acervoAlterar.DataAcervo);
            acervoAlterado.Ano.ShouldBe(acervoAlterar.Ano);
            
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
            await InserirDadosBasicosAleatorios();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();
            
            var acervos = ObterTodos<Acervo>();
            var acervoAlterar = (acervos).FirstOrDefault();
            acervoAlterar.Titulo = acervos.LastOrDefault().Titulo;
            acervoAlterar.Descricao = faker.Lorem.Text();
            acervoAlterar.SubTitulo = acervos.LastOrDefault().SubTitulo;
            acervoAlterar.Codigo = "196";
            acervoAlterar.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString();
            acervoAlterar.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
            
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
            acervoAlterado.DataAcervo.ShouldBe(acervoAlterar.DataAcervo);
            acervoAlterado.Ano.ShouldBe(acervoAlterar.Ano);
            
            var acervosCreditosAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w=> w.AcervoId == acervoAlterar.Id);
            acervosCreditosAutores.Count().ShouldBe(2);
           
            foreach (var creditoAutorId in acervoAlterar.CreditosAutoresIds)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == creditoAutorId && f.TipoAutoria is null).ShouldBeTrue();

            foreach (var coAutor in acervoAlterar.CoAutores)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == coAutor.CreditoAutorId && f.TipoAutoria == coAutor.TipoAutoria).ShouldBeTrue();
        }
        
        //Década e século provável e certo, com ano exato terminando com zero e com ano qualquer
        [Fact(DisplayName = "Acervo - Inserir")]
        public async Task Inserir()
        {
            await InserirDadosBasicosAleatorios();
            var servicoAcervo = GetServicoAcervo();
            var random = new Random();
            
            var acervoInserir = AcervoMock.Instance.GerarAcervo(TipoAcervo.Bibliografico).Generate();
            acervoInserir.CreditosAutoresIds = new long []{1,2,3,4,5};
            acervoInserir.CoAutores = new List<CoAutor>() { new () { CreditoAutorId = random.Next(1,5), TipoAutoria = faker.Lorem.Word().Limite(15)}}.ToList();
            
            var acervoId = await servicoAcervo.Inserir(acervoInserir);
            var acervoinserido = (ObterTodos<Acervo>()).FirstOrDefault(w=> w.Id == acervoId);
            acervoinserido.Codigo.ShouldBe(acervoInserir.Codigo);
            acervoinserido.Titulo.ShouldBe(acervoInserir.Titulo);
            acervoinserido.Descricao.ShouldBe(acervoInserir.Descricao);
            acervoinserido.SubTitulo.ShouldBe(acervoInserir.SubTitulo);
            acervoinserido.DataAcervo.ShouldBe(acervoInserir.DataAcervo);
            acervoinserido.Ano.ShouldBe(acervoInserir.Ano);
            
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
            await InserirDadosBasicosAleatorios();
            var servicoAcervo = GetServicoAcervo();
            
            var acervoInserir = AcervoMock.Instance.GerarAcervo(TipoAcervo.Bibliografico).Generate();
            acervoInserir.CreditosAutoresIds = new long []{1,2,3,4,5};
            acervoInserir.CoAutores = Enumerable.Empty<CoAutor>();
            
            var acervoId = await servicoAcervo.Inserir(acervoInserir);
            var acervoinserido = (ObterTodos<Acervo>()).FirstOrDefault(w=> w.Id == acervoId);
            acervoinserido.Codigo.ShouldBe(acervoInserir.Codigo);
            acervoinserido.Titulo.ShouldBe(acervoInserir.Titulo);
            acervoinserido.Descricao.ShouldBe(acervoInserir.Descricao);
            acervoinserido.SubTitulo.ShouldBe(acervoInserir.SubTitulo);
            acervoinserido.DataAcervo.ShouldBe(acervoInserir.DataAcervo);
            acervoinserido.Ano.ShouldBe(acervoInserir.Ano);
            
            var acervosCreditosAutores = (ObterTodos<AcervoCreditoAutor>()).Where(w=> w.AcervoId == acervoId);
            acervosCreditosAutores.Count().ShouldBe(5);
           
            foreach (var creditoAutorId in acervoInserir.CreditosAutoresIds)
                acervosCreditosAutores.Any(f=> f.CreditoAutorId == creditoAutorId && f.TipoAutoria is null).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo - Não deve inserir código existente dentro de mesmo acervo")]
        public async Task Nao_deve_inserir_codigo_existente_dentro_mesmo_acervo()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervo();
            var servicoAcervo = GetServicoAcervo();
            
            var acervoInserir = AcervoMock.Instance.GerarAcervo(TipoAcervo.Bibliografico).Generate();
            acervoInserir.CreditosAutoresIds = new long []{1,2,3,4,5};
            acervoInserir.CoAutores = new List<CoAutor>() { new () { CreditoAutorId = new Random().Next(1,5), TipoAutoria = faker.Lorem.Word().Limite(15)}}.ToList();

            var acervos = ObterTodos<Acervo>();
            acervoInserir.Codigo = acervos.FirstOrDefault().Codigo;
            await servicoAcervo.Inserir(acervoInserir).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo - Deve inserir com códigos iguais e acervos diferentes")]
        public async Task Deve_inserir_com_codigo_iguais_e_acervos_diferentes()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervo(TipoAcervo.DocumentacaoHistorica);
            var servicoAcervo = GetServicoAcervo();
            
            var acervoInserir = AcervoMock.Instance.GerarAcervo(TipoAcervo.Bibliografico).Generate();
            acervoInserir.CreditosAutoresIds = new long []{1,2,3,4,5};
            acervoInserir.CoAutores = new List<CoAutor>() { new () { CreditoAutorId = new Random().Next(1,5), TipoAutoria = faker.Lorem.Word().Limite(15)}}.ToList();

            var acervos = ObterTodos<Acervo>();
            acervoInserir.Titulo = acervos.LastOrDefault().Titulo;
            acervoInserir.Descricao = faker.Lorem.Text();
            acervoInserir.SubTitulo = acervos.LastOrDefault().SubTitulo;
            acervoInserir.Codigo = acervos.FirstOrDefault().Codigo;
            acervoInserir.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString();
            acervoInserir.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
            
            var acervoId = await servicoAcervo.Inserir(acervoInserir);
            var acervoinserido = (ObterTodos<Acervo>()).FirstOrDefault(w=> w.Id == acervoId);
            acervoinserido.Codigo.ShouldBe(acervoInserir.Codigo);
            acervoinserido.Titulo.ShouldBe(acervoInserir.Titulo);
            acervoinserido.Descricao.ShouldBe(acervoInserir.Descricao);
            acervoinserido.SubTitulo.ShouldBe(acervoInserir.SubTitulo);
            acervoinserido.DataAcervo.ShouldBe(acervoInserir.DataAcervo);
            acervoinserido.Ano.ShouldBe(acervoInserir.Ano);
            
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
            await InserirDadosBasicosAleatorios();

            await InserirAcervo();
            
            var servicoAcervo = GetServicoAcervo();

            var acervo = await servicoAcervo.ObterPorId(1);
            acervo.ShouldNotBeNull();
            acervo.Id.ShouldBe(1);
            acervo.Titulo.ShouldNotBeNull();
            acervo.Codigo.ShouldNotBeNull();
            acervo.SubTitulo.ShouldNotBeNull();
            acervo.DataAcervo.ShouldNotBeNull();
            acervo.Ano.ShouldBeGreaterThan("0");
        }
        
        [Fact(DisplayName = "Acervo - Excluir")]
        public async Task Excluir()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervo();
            
            var servicoAcervo = GetServicoAcervo();

            await servicoAcervo.Excluir(3);

            var acervos = ObterTodos<Acervo>();
            acervos.Count(a=> a.Excluido).ShouldBeEquivalentTo(1);
            acervos.Count(a=> !a.Excluido).ShouldBeEquivalentTo(34);
        }

        [Fact(DisplayName = "Acervo - Gerar anos indexadores iniciais e finais com base no Ano [197-] ou [197-?] ou [19--] ou [19--?] ou [1987]")]
        public async Task Deve_gerar_anos_indexadores_iniciais_finais_com_base_ano()
        {
            var decadaCerta = "[197-]";
            var decadaPossivel = "[197-?]";
            var seculoCerto = "[19--]";
            var seculoPossivel = "[19--?]";
            var anoExato = "[1987]";
            var anoExatoTerminandoComZero = "[1980]";

            var decadaCertaTratada = decadaCerta.ObterAnoNumerico();
            decadaCertaTratada.ShouldBe(1970);
            decadaCertaTratada.ObterFimDaDecadaOuSeculo().ShouldBe(1979);
            
            var decadaPossivelTratada = decadaPossivel.ObterAnoNumerico();
            decadaPossivelTratada.ShouldBe(1970);
            decadaPossivelTratada.ObterFimDaDecadaOuSeculo().ShouldBe(1979);
            
            var seculoCertoTratada = seculoCerto.ObterAnoNumerico();
            seculoCertoTratada.ShouldBe(1900);
            seculoCertoTratada.ObterFimDaDecadaOuSeculo().ShouldBe(1999);
            
            var seculoPossivelTratada = seculoPossivel.ObterAnoNumerico();
            seculoPossivelTratada.ShouldBe(1900);
            seculoPossivelTratada.ObterFimDaDecadaOuSeculo().ShouldBe(1999);
            
            var anoTratado = anoExato.ObterAnoNumerico();
            anoTratado.ShouldBe(1987);
            
            anoTratado = anoExatoTerminandoComZero.ObterAnoNumerico();
            anoTratado.ShouldBe(1980);
            
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
                    DataAcervo = faker.Date.ToString(),
                    Ano = faker.Date.Past().Year.ToString(),
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