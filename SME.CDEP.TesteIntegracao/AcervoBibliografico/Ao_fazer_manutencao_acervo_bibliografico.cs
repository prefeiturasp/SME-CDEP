using Bogus;
using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using SME.CDEP.TesteIntegracao.Constantes;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_fazer_manutencao_acervo_bibliografico : TesteBase
    {
        public Ao_fazer_manutencao_acervo_bibliografico(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo bibliografico - Obter por Id")]
        public async Task Obter_por_id()
        {
            await InserirDadosBasicos();
            await InserirAcervoBibliografico();
            var servicoAcervoBibliografico = GetServicoAcervoBibliografico();

            var acervoBibliograficoDto = await servicoAcervoBibliografico.ObterPorId(5);
            acervoBibliograficoDto.ShouldNotBeNull();
            acervoBibliograficoDto.CreditosAutoresIds.Any().ShouldBeTrue();
            acervoBibliograficoDto.CoAutores.Any().ShouldBeTrue();
            acervoBibliograficoDto.CoAutores.Any(a=> a.TipoAutoria.NaoEhNulo()).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Obter por Id sem coAutor")]
        public async Task Obter_por_id_sem_coautor()
        {
            await InserirDadosBasicos();
            await InserirAcervoBibliografico(false);
            var servicoAcervoBibliografico = GetServicoAcervoBibliografico();

            var acervo = await servicoAcervoBibliografico.ObterPorId(5);
            acervo.ShouldNotBeNull();
            acervo.CreditosAutoresIds.Any().ShouldBeTrue();
            acervo.CoAutores.Any().ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirDadosBasicos();
            await InserirAcervoBibliografico();
            var servicoAcervoBibliografico = GetServicoAcervoBibliografico();

            var acervoBibliograficoDtos = await servicoAcervoBibliografico.ObterTodos();
            acervoBibliograficoDtos.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Atualizar - CoAutores: removendo 3 e adicionando 1 novo")]
        public async Task Atualizar_removendo_3_e_inserindo_1_coautor()
        {
            await InserirDadosBasicos();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var servicoAcervobibliografico = GetServicoAcervoBibliografico();
            
            var acervoBibliograficoCompleto = GerarAcervoBibliografico().Generate();
            
            var acervoAtual = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            
            var acervoBibliograficoAlteracaoDto = mapper.Map<AcervoBibliograficoAlteracaoDTO>(acervoBibliograficoCompleto);
            acervoBibliograficoAlteracaoDto.CreditosAutoresIds = new long[] { 1, 2, 3 };
            acervoBibliograficoAlteracaoDto.CoAutores = new CoAutorDTO[] { new () { CreditoAutorId = 4, TipoAutoria = faker.Lorem.Word().Limite(15)} };
            acervoBibliograficoAlteracaoDto.AssuntosIds = new long[] { 1, 2 };
            acervoBibliograficoAlteracaoDto.Id = 3;
            acervoBibliograficoAlteracaoDto.Titulo = acervoBibliograficoCompleto.Acervo.Titulo;
            acervoBibliograficoAlteracaoDto.Codigo = acervoAtual.Codigo;
            acervoBibliograficoAlteracaoDto.SubTitulo = acervoBibliograficoCompleto.Acervo.SubTitulo;
            acervoBibliograficoAlteracaoDto.AcervoId = acervoAtual.Id;
            
            await servicoAcervobibliografico.Alterar(acervoBibliograficoAlteracaoDto);

            var acervoAlterado = (ObterTodos<Acervo>()).FirstOrDefault(f=> f.Id == 3);
            acervoAlterado.Titulo.Equals(acervoBibliograficoAlteracaoDto.Titulo).ShouldBeTrue();
            acervoAlterado.Codigo.Equals(acervoBibliograficoAlteracaoDto.Codigo).ShouldBeTrue();
            acervoAlterado.SubTitulo.Equals(acervoBibliograficoAlteracaoDto.SubTitulo).ShouldBeTrue();
            acervoAlterado.TipoAcervoId.ShouldBe((int)TipoAcervo.Bibliografico);
            acervoAlterado.CriadoLogin.ShouldNotBeEmpty();
            acervoAlterado.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervoAlterado.CriadoPor.ShouldNotBeEmpty();
            acervoAlterado.AlteradoLogin.ShouldNotBeNull();
            acervoAlterado.AlteradoEm.HasValue.ShouldBeTrue();
            acervoAlterado.AlteradoPor.ShouldNotBeNull();
            
           var acervoBibliografico = ObterTodos<AcervoBibliografico>().FirstOrDefault(w=> w.AcervoId == 3);
           acervoBibliografico.MaterialId.ShouldBe(acervoBibliograficoAlteracaoDto.MaterialId);
           acervoBibliografico.EditoraId.ShouldBe(acervoBibliograficoAlteracaoDto.EditoraId);
           acervoBibliografico.Ano.ShouldBe(acervoBibliograficoAlteracaoDto.Ano);
           acervoBibliografico.Edicao.ShouldBe(acervoBibliograficoAlteracaoDto.Edicao);
           acervoBibliografico.NumeroPagina.ShouldBe(acervoBibliograficoAlteracaoDto.NumeroPagina);
           acervoBibliografico.Largura.ShouldBe(acervoBibliograficoAlteracaoDto.Largura.Value);
           acervoBibliografico.Altura.ShouldBe(acervoBibliograficoAlteracaoDto.Altura.Value);
           acervoBibliografico.Volume.ShouldBe(acervoBibliograficoAlteracaoDto.Volume);
           acervoBibliografico.IdiomaId.ShouldBe(acervoBibliograficoAlteracaoDto.IdiomaId);
           acervoBibliografico.LocalizacaoCDD.ShouldBe(acervoBibliograficoAlteracaoDto.LocalizacaoCDD);
           acervoBibliografico.LocalizacaoPHA.ShouldBe(acervoBibliograficoAlteracaoDto.LocalizacaoPHA);
           acervoBibliografico.NotasGerais.ShouldBe(acervoBibliograficoAlteracaoDto.NotasGerais);
           acervoBibliografico.Isbn.ShouldBe(acervoBibliograficoAlteracaoDto.Isbn);
            
            var acervoBibliograficoAssuntos = ObterTodos<AcervoBibliograficoAssunto>();
            var acervoBibliograficoAssuntoInseridos = acervoBibliograficoAssuntos.Where(w => w.AcervoBibliograficoId == acervoBibliografico.Id);
            acervoBibliograficoAssuntoInseridos.Count().ShouldBe(acervoBibliograficoAlteracaoDto.AssuntosIds.Count());

            foreach (var assuntoId in acervoBibliograficoAlteracaoDto.AssuntosIds)
                acervoBibliograficoAssuntoInseridos.Any(a=> a.AssuntoId == assuntoId).ShouldBeTrue();
            
            var autoresCoAutores = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            
            var autores = autoresCoAutores.Where(w => w.TipoAutoria.EhNulo());
            autores.Count().ShouldBe(3);
            foreach (var autorIdInserido in acervoBibliograficoAlteracaoDto.CreditosAutoresIds)
                autores.Any(a=> a.CreditoAutorId == autorIdInserido).ShouldBeTrue();
            
            var coAutores = autoresCoAutores.Where(w => w.TipoAutoria.NaoEhNulo());
            coAutores.Count().ShouldBe(1);
            foreach (var coAutorInserido in acervoBibliograficoAlteracaoDto.CoAutores)
            {
                coAutores.Any(a => a.CreditoAutorId == coAutorInserido.CreditoAutorId).ShouldBeTrue();
                coAutores.Any(a => a.TipoAutoria == coAutorInserido.TipoAutoria).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Atualizar - CoAutores: removendo 3 e adicionando 1 novo e Código")]
        public async Task Atualizar_removendo_3_e_inserindo_1_coautor_e_codigo()
        {
            await InserirDadosBasicos();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var servicoAcervobibliografico = GetServicoAcervoBibliografico();
            
            var acervoBibliograficoCompleto = GerarAcervoBibliografico().Generate();
            
            var acervoAtual = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            
            var acervoBibliograficoAlteracaoDto = mapper.Map<AcervoBibliograficoAlteracaoDTO>(acervoBibliograficoCompleto);
            acervoBibliograficoAlteracaoDto.CreditosAutoresIds = new long[] { 1, 2, 3 };
            acervoBibliograficoAlteracaoDto.CoAutores = new CoAutorDTO[] { new () { CreditoAutorId = 4, TipoAutoria = faker.Lorem.Word().Limite(15)} };
            acervoBibliograficoAlteracaoDto.AssuntosIds = new long[] { 1, 2 };
            acervoBibliograficoAlteracaoDto.Id = 3;
            acervoBibliograficoAlteracaoDto.Titulo = acervoBibliograficoCompleto.Acervo.Titulo;
            acervoBibliograficoAlteracaoDto.Codigo = new Random().Next(100,200).ToString();
            acervoBibliograficoAlteracaoDto.SubTitulo = acervoBibliograficoCompleto.Acervo.SubTitulo;
            acervoBibliograficoAlteracaoDto.AcervoId = acervoAtual.Id;
            
            await servicoAcervobibliografico.Alterar(acervoBibliograficoAlteracaoDto);

            var acervoAlterado = (ObterTodos<Acervo>()).FirstOrDefault(f=> f.Id == 3);
            acervoAlterado.Titulo.Equals(acervoBibliograficoAlteracaoDto.Titulo).ShouldBeTrue();
            acervoAlterado.Codigo.Equals(acervoBibliograficoAlteracaoDto.Codigo).ShouldBeTrue();
            acervoAlterado.SubTitulo.Equals(acervoBibliograficoAlteracaoDto.SubTitulo).ShouldBeTrue();
            acervoAlterado.TipoAcervoId.ShouldBe((int)TipoAcervo.Bibliografico);
            acervoAlterado.CriadoLogin.ShouldNotBeEmpty();
            acervoAlterado.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervoAlterado.CriadoPor.ShouldNotBeEmpty();
            acervoAlterado.AlteradoLogin.ShouldNotBeNull();
            acervoAlterado.AlteradoEm.HasValue.ShouldBeTrue();
            acervoAlterado.AlteradoPor.ShouldNotBeNull();
            
           var acervoBibliografico = ObterTodos<AcervoBibliografico>().FirstOrDefault(w=> w.AcervoId == 3);
           acervoBibliografico.MaterialId.ShouldBe(acervoBibliograficoAlteracaoDto.MaterialId);
           acervoBibliografico.EditoraId.ShouldBe(acervoBibliograficoAlteracaoDto.EditoraId);
           acervoBibliografico.Ano.ShouldBe(acervoBibliograficoAlteracaoDto.Ano);
           acervoBibliografico.Edicao.ShouldBe(acervoBibliograficoAlteracaoDto.Edicao);
           acervoBibliografico.NumeroPagina.ShouldBe(acervoBibliograficoAlteracaoDto.NumeroPagina);
           acervoBibliografico.Largura.ShouldBe(acervoBibliograficoAlteracaoDto.Largura.Value);
           acervoBibliografico.Altura.ShouldBe(acervoBibliograficoAlteracaoDto.Altura.Value);
           acervoBibliografico.Volume.ShouldBe(acervoBibliograficoAlteracaoDto.Volume);
           acervoBibliografico.IdiomaId.ShouldBe(acervoBibliograficoAlteracaoDto.IdiomaId);
           acervoBibliografico.LocalizacaoCDD.ShouldBe(acervoBibliograficoAlteracaoDto.LocalizacaoCDD);
           acervoBibliografico.LocalizacaoPHA.ShouldBe(acervoBibliograficoAlteracaoDto.LocalizacaoPHA);
           acervoBibliografico.NotasGerais.ShouldBe(acervoBibliograficoAlteracaoDto.NotasGerais);
           acervoBibliografico.Isbn.ShouldBe(acervoBibliograficoAlteracaoDto.Isbn);
            
            var acervoBibliograficoAssuntos = ObterTodos<AcervoBibliograficoAssunto>();
            var acervoBibliograficoAssuntoInseridos = acervoBibliograficoAssuntos.Where(w => w.AcervoBibliograficoId == acervoBibliografico.Id);
            acervoBibliograficoAssuntoInseridos.Count().ShouldBe(acervoBibliograficoAlteracaoDto.AssuntosIds.Count());

            foreach (var assuntoId in acervoBibliograficoAlteracaoDto.AssuntosIds)
                acervoBibliograficoAssuntoInseridos.Any(a=> a.AssuntoId == assuntoId).ShouldBeTrue();
            
            var autoresCoAutores = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            
            var autores = autoresCoAutores.Where(w => w.TipoAutoria.EhNulo());
            autores.Count().ShouldBe(3);
            foreach (var autorIdInserido in acervoBibliograficoAlteracaoDto.CreditosAutoresIds)
                autores.Any(a=> a.CreditoAutorId == autorIdInserido).ShouldBeTrue();
            
            var coAutores = autoresCoAutores.Where(w => w.TipoAutoria.NaoEhNulo());
            coAutores.Count().ShouldBe(1);
            foreach (var coAutorInserido in acervoBibliograficoAlteracaoDto.CoAutores)
            {
                coAutores.Any(a => a.CreditoAutorId == coAutorInserido.CreditoAutorId).ShouldBeTrue();
                coAutores.Any(a => a.TipoAutoria == coAutorInserido.TipoAutoria).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Não deve atualizar sem Código")]
        public async Task Nao_deve_atualizar_sem_codigo()
        {
            await InserirDadosBasicos();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var servicoAcervobibliografico = GetServicoAcervoBibliografico();
            
            var acervoBibliograficoCompleto = GerarAcervoBibliografico().Generate();
            
            var acervoAtual = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            
            var acervoBibliograficoAlteracaoDto = mapper.Map<AcervoBibliograficoAlteracaoDTO>(acervoBibliograficoCompleto);
            acervoBibliograficoAlteracaoDto.CreditosAutoresIds = new long[] { 1, 2, 3 };
            acervoBibliograficoAlteracaoDto.CoAutores = new CoAutorDTO[] { new () { CreditoAutorId = 4, TipoAutoria = faker.Lorem.Word().Limite(15)} };
            acervoBibliograficoAlteracaoDto.AssuntosIds = new long[] { 1, 2 };
            acervoBibliograficoAlteracaoDto.Id = 3;
            acervoBibliograficoAlteracaoDto.Titulo = acervoBibliograficoCompleto.Acervo.Titulo;
            acervoBibliograficoAlteracaoDto.SubTitulo = acervoBibliograficoCompleto.Acervo.SubTitulo;
            acervoBibliograficoAlteracaoDto.AcervoId = acervoAtual.Id;
            
            await servicoAcervobibliografico.Alterar(acervoBibliograficoAlteracaoDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Não deve atualizar com Código duplicado")]
        public async Task Nao_deve_atualizar_com_codigo_duplicado()
        {
            await InserirDadosBasicos();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var servicoAcervobibliografico = GetServicoAcervoBibliografico();
            
            var acervoBibliograficoCompleto = GerarAcervoBibliografico().Generate();
            
            var acervoAtual = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            
            var acervoBibliograficoAlteracaoDto = mapper.Map<AcervoBibliograficoAlteracaoDTO>(acervoBibliograficoCompleto);
            acervoBibliograficoAlteracaoDto.CreditosAutoresIds = new long[] { 1, 2, 3 };
            acervoBibliograficoAlteracaoDto.CoAutores = new CoAutorDTO[] { new () { CreditoAutorId = 4, TipoAutoria = faker.Lorem.Word().Limite(15)} };
            acervoBibliograficoAlteracaoDto.AssuntosIds = new long[] { 1, 2 };
            acervoBibliograficoAlteracaoDto.Id = 3;
            acervoBibliograficoAlteracaoDto.Titulo = acervoBibliograficoCompleto.Acervo.Titulo;
            acervoBibliograficoAlteracaoDto.Codigo = "1";
            acervoBibliograficoAlteracaoDto.SubTitulo = acervoBibliograficoCompleto.Acervo.SubTitulo;
            acervoBibliograficoAlteracaoDto.AcervoId = acervoAtual.Id;
            
            await servicoAcervobibliografico.Alterar(acervoBibliograficoAlteracaoDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Atualizar removendo todos os CoAutores")]
        public async Task Atualizar_removendo_todos_coAutores()
        {
           await InserirDadosBasicos();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var servicoAcervobibliografico = GetServicoAcervoBibliografico();
            
            var acervoBibliograficoCompleto = GerarAcervoBibliografico().Generate();
            
            var acervoAtual = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            
            var acervoBibliograficoAlteracaoDto = mapper.Map<AcervoBibliograficoAlteracaoDTO>(acervoBibliograficoCompleto);
            acervoBibliograficoAlteracaoDto.CreditosAutoresIds = new long[] { 1, 2, 3 };
            acervoBibliograficoAlteracaoDto.CoAutores = null;
            acervoBibliograficoAlteracaoDto.AssuntosIds = new long[] { 1, 2 };
            acervoBibliograficoAlteracaoDto.Id = 3;
            acervoBibliograficoAlteracaoDto.Titulo = acervoBibliograficoCompleto.Acervo.Titulo;
            acervoBibliograficoAlteracaoDto.Codigo = acervoAtual.Codigo;
            acervoBibliograficoAlteracaoDto.SubTitulo = acervoBibliograficoCompleto.Acervo.SubTitulo;
            acervoBibliograficoAlteracaoDto.AcervoId = acervoAtual.Id;
            
            await servicoAcervobibliografico.Alterar(acervoBibliograficoAlteracaoDto);

            var acervoAlterado = (ObterTodos<Acervo>()).FirstOrDefault(f=> f.Id == 3);
            acervoAlterado.Titulo.Equals(acervoBibliograficoAlteracaoDto.Titulo).ShouldBeTrue();
            acervoAlterado.Codigo.Equals(acervoBibliograficoAlteracaoDto.Codigo).ShouldBeTrue();
            acervoAlterado.SubTitulo.Equals(acervoBibliograficoAlteracaoDto.SubTitulo).ShouldBeTrue();
            acervoAlterado.TipoAcervoId.ShouldBe((int)TipoAcervo.Bibliografico);
            acervoAlterado.CriadoLogin.ShouldNotBeEmpty();
            acervoAlterado.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervoAlterado.CriadoPor.ShouldNotBeEmpty();
            acervoAlterado.AlteradoLogin.ShouldNotBeNull();
            acervoAlterado.AlteradoEm.HasValue.ShouldBeTrue();
            acervoAlterado.AlteradoPor.ShouldNotBeNull();
            
           var acervoBibliografico = ObterTodos<AcervoBibliografico>().FirstOrDefault(w=> w.AcervoId == 3);
           acervoBibliografico.MaterialId.ShouldBe(acervoBibliograficoAlteracaoDto.MaterialId);
           acervoBibliografico.EditoraId.ShouldBe(acervoBibliograficoAlteracaoDto.EditoraId);
           acervoBibliografico.Ano.ShouldBe(acervoBibliograficoAlteracaoDto.Ano);
           acervoBibliografico.Edicao.ShouldBe(acervoBibliograficoAlteracaoDto.Edicao);
           acervoBibliografico.NumeroPagina.ShouldBe(acervoBibliograficoAlteracaoDto.NumeroPagina);
           acervoBibliografico.Largura.ShouldBe(acervoBibliograficoAlteracaoDto.Largura.Value);
           acervoBibliografico.Altura.ShouldBe(acervoBibliograficoAlteracaoDto.Altura.Value);
           acervoBibliografico.Volume.ShouldBe(acervoBibliograficoAlteracaoDto.Volume);
           acervoBibliografico.IdiomaId.ShouldBe(acervoBibliograficoAlteracaoDto.IdiomaId);
           acervoBibliografico.LocalizacaoCDD.ShouldBe(acervoBibliograficoAlteracaoDto.LocalizacaoCDD);
           acervoBibliografico.LocalizacaoPHA.ShouldBe(acervoBibliograficoAlteracaoDto.LocalizacaoPHA);
           acervoBibliografico.NotasGerais.ShouldBe(acervoBibliograficoAlteracaoDto.NotasGerais);
           acervoBibliografico.Isbn.ShouldBe(acervoBibliograficoAlteracaoDto.Isbn);
            
            var acervoBibliograficoAssuntos = ObterTodos<AcervoBibliograficoAssunto>();
            var acervoBibliograficoAssuntoInseridos = acervoBibliograficoAssuntos.Where(w => w.AcervoBibliograficoId == acervoBibliografico.Id);
            acervoBibliograficoAssuntoInseridos.Count().ShouldBe(acervoBibliograficoAlteracaoDto.AssuntosIds.Count());

            foreach (var assuntoId in acervoBibliograficoAlteracaoDto.AssuntosIds)
                acervoBibliograficoAssuntoInseridos.Any(a=> a.AssuntoId == assuntoId).ShouldBeTrue();
            
            var autoresCoAutores = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            
            var autores = autoresCoAutores.Where(w => w.TipoAutoria.EhNulo());
            autores.Count().ShouldBe(3);
            foreach (var autorIdInserido in acervoBibliograficoAlteracaoDto.CreditosAutoresIds)
                autores.Any(a=> a.CreditoAutorId == autorIdInserido).ShouldBeTrue();
            
            var coAutores = autoresCoAutores.Where(w => w.TipoAutoria.NaoEhNulo());
            coAutores.Count().ShouldBe(0);
        }
       
        [Fact(DisplayName = "Acervo bibliografico - Inserir")]
        public async Task Inserir()
        {
            await InserirDadosBasicos();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var servicoAcervobibliografico = GetServicoAcervoBibliografico();
            
            var acervoBibliograficoCompleto = GerarAcervoBibliografico().Generate();
            
            var acervoBibliograficoCadastroDto = mapper.Map<AcervoBibliograficoCadastroDTO>(acervoBibliograficoCompleto);
            acervoBibliograficoCadastroDto.CreditosAutoresIds = new long[] { 1, 2, 3 };
            acervoBibliograficoCadastroDto.CoAutores = new CoAutorDTO[] { new () { CreditoAutorId = 4, TipoAutoria = faker.Lorem.Word().Limite(15)} };
            acervoBibliograficoCadastroDto.AssuntosIds = new long[] { 1, 2 };
            acervoBibliograficoCadastroDto.Titulo = acervoBibliograficoCompleto.Acervo.Titulo;
            acervoBibliograficoCadastroDto.Codigo = new Random().Next(100,200).ToString();
            acervoBibliograficoCadastroDto.SubTitulo = acervoBibliograficoCompleto.Acervo.SubTitulo;
            
            await servicoAcervobibliografico.Inserir(acervoBibliograficoCadastroDto);

            var acervoInserido = ObterTodos<Acervo>().OrderBy(o=> o.Id).LastOrDefault();
            acervoInserido.Titulo.Equals(acervoBibliograficoCadastroDto.Titulo).ShouldBeTrue();
            acervoInserido.Codigo.Equals(acervoBibliograficoCadastroDto.Codigo).ShouldBeTrue();
            acervoInserido.SubTitulo.Equals(acervoBibliograficoCadastroDto.SubTitulo).ShouldBeTrue();
            acervoInserido.TipoAcervoId.ShouldBe((int)TipoAcervo.Bibliografico);
            acervoInserido.CriadoLogin.ShouldNotBeEmpty();
            acervoInserido.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervoInserido.CriadoPor.ShouldNotBeEmpty();
            acervoInserido.AlteradoLogin.ShouldBeNull();
            acervoInserido.AlteradoEm.ShouldBeNull();
            acervoInserido.AlteradoPor.ShouldBeNull();
            
           var acervoBibliografico = ObterTodos<AcervoBibliografico>().OrderBy(o=> o.Id).LastOrDefault();
           acervoBibliografico.MaterialId.ShouldBe(acervoBibliograficoCadastroDto.MaterialId);
           acervoBibliografico.EditoraId.ShouldBe(acervoBibliograficoCadastroDto.EditoraId);
           acervoBibliografico.Ano.ShouldBe(acervoBibliograficoCadastroDto.Ano);
           acervoBibliografico.Edicao.ShouldBe(acervoBibliograficoCadastroDto.Edicao);
           acervoBibliografico.NumeroPagina.ShouldBe(acervoBibliograficoCadastroDto.NumeroPagina);
           acervoBibliografico.Largura.ShouldBe(acervoBibliograficoCadastroDto.Largura.Value);
           acervoBibliografico.Altura.ShouldBe(acervoBibliograficoCadastroDto.Altura.Value);
           acervoBibliografico.Volume.ShouldBe(acervoBibliograficoCadastroDto.Volume);
           acervoBibliografico.IdiomaId.ShouldBe(acervoBibliograficoCadastroDto.IdiomaId);
           acervoBibliografico.LocalizacaoCDD.ShouldBe(acervoBibliograficoCadastroDto.LocalizacaoCDD);
           acervoBibliografico.LocalizacaoPHA.ShouldBe(acervoBibliograficoCadastroDto.LocalizacaoPHA);
           acervoBibliografico.NotasGerais.ShouldBe(acervoBibliograficoCadastroDto.NotasGerais);
           acervoBibliografico.Isbn.ShouldBe(acervoBibliograficoCadastroDto.Isbn);
            
            var acervoBibliograficoAssuntos = ObterTodos<AcervoBibliograficoAssunto>();
            var acervoBibliograficoAssuntoInseridos = acervoBibliograficoAssuntos.Where(w => w.AcervoBibliograficoId == acervoBibliografico.Id);
            acervoBibliograficoAssuntoInseridos.Count().ShouldBe(acervoBibliograficoCadastroDto.AssuntosIds.Count());

            foreach (var assuntoId in acervoBibliograficoCadastroDto.AssuntosIds)
                acervoBibliograficoAssuntoInseridos.Any(a=> a.AssuntoId == assuntoId).ShouldBeTrue();
            
            var autoresCoAutores = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == acervoInserido.Id);
            
            var autores = autoresCoAutores.Where(w => w.TipoAutoria.EhNulo());
            autores.Count().ShouldBe(3);
            foreach (var autorIdInserido in acervoBibliograficoCadastroDto.CreditosAutoresIds)
                autores.Any(a=> a.CreditoAutorId == autorIdInserido).ShouldBeTrue();
            
            var coAutores = autoresCoAutores.Where(w => w.TipoAutoria.NaoEhNulo());
            coAutores.Count().ShouldBe(1);
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Inserindo sem CoAutores")]
        public async Task Inserir_sem_coautores()
        {
            await InserirDadosBasicos();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var servicoAcervobibliografico = GetServicoAcervoBibliografico();
            
            var acervoBibliograficoCompleto = GerarAcervoBibliografico().Generate();
            
            var acervoBibliograficoCadastroDto = mapper.Map<AcervoBibliograficoCadastroDTO>(acervoBibliograficoCompleto);
            acervoBibliograficoCadastroDto.CreditosAutoresIds = new long[] { 4,5 };
            acervoBibliograficoCadastroDto.CoAutores = null;
            acervoBibliograficoCadastroDto.AssuntosIds = new long[] { 1, 2 };
            acervoBibliograficoCadastroDto.Titulo = acervoBibliograficoCompleto.Acervo.Titulo;
            acervoBibliograficoCadastroDto.Codigo = new Random().Next(100,200).ToString();
            acervoBibliograficoCadastroDto.SubTitulo = acervoBibliograficoCompleto.Acervo.SubTitulo;
            
            await servicoAcervobibliografico.Inserir(acervoBibliograficoCadastroDto);

            var acervoInserido = ObterTodos<Acervo>().OrderBy(o=> o.Id).LastOrDefault();
            acervoInserido.Titulo.Equals(acervoBibliograficoCadastroDto.Titulo).ShouldBeTrue();
            acervoInserido.Codigo.Equals(acervoBibliograficoCadastroDto.Codigo).ShouldBeTrue();
            acervoInserido.SubTitulo.Equals(acervoBibliograficoCadastroDto.SubTitulo).ShouldBeTrue();
            acervoInserido.TipoAcervoId.ShouldBe((int)TipoAcervo.Bibliografico);
            acervoInserido.CriadoLogin.ShouldNotBeEmpty();
            acervoInserido.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervoInserido.CriadoPor.ShouldNotBeEmpty();
            acervoInserido.AlteradoLogin.ShouldBeNull();
            acervoInserido.AlteradoEm.ShouldBeNull();
            acervoInserido.AlteradoPor.ShouldBeNull();
            
           var acervoBibliografico = ObterTodos<AcervoBibliografico>().OrderBy(o=> o.Id).LastOrDefault();
           acervoBibliografico.MaterialId.ShouldBe(acervoBibliograficoCadastroDto.MaterialId);
           acervoBibliografico.EditoraId.ShouldBe(acervoBibliograficoCadastroDto.EditoraId);
           acervoBibliografico.Ano.ShouldBe(acervoBibliograficoCadastroDto.Ano);
           acervoBibliografico.Edicao.ShouldBe(acervoBibliograficoCadastroDto.Edicao);
           acervoBibliografico.NumeroPagina.ShouldBe(acervoBibliograficoCadastroDto.NumeroPagina);
           acervoBibliografico.Largura.ShouldBe(acervoBibliograficoCadastroDto.Largura.Value);
           acervoBibliografico.Altura.ShouldBe(acervoBibliograficoCadastroDto.Altura.Value);
           acervoBibliografico.Volume.ShouldBe(acervoBibliograficoCadastroDto.Volume);
           acervoBibliografico.IdiomaId.ShouldBe(acervoBibliograficoCadastroDto.IdiomaId);
           acervoBibliografico.LocalizacaoCDD.ShouldBe(acervoBibliograficoCadastroDto.LocalizacaoCDD);
           acervoBibliografico.LocalizacaoPHA.ShouldBe(acervoBibliograficoCadastroDto.LocalizacaoPHA);
           acervoBibliografico.NotasGerais.ShouldBe(acervoBibliograficoCadastroDto.NotasGerais);
           acervoBibliografico.Isbn.ShouldBe(acervoBibliograficoCadastroDto.Isbn);
            
            var acervoBibliograficoAssuntos = ObterTodos<AcervoBibliograficoAssunto>();
            var acervoBibliograficoAssuntoInseridos = acervoBibliograficoAssuntos.Where(w => w.AcervoBibliograficoId == acervoBibliografico.Id);
            acervoBibliograficoAssuntoInseridos.Count().ShouldBe(acervoBibliograficoCadastroDto.AssuntosIds.Count());

            foreach (var assuntoId in acervoBibliograficoCadastroDto.AssuntosIds)
                acervoBibliograficoAssuntoInseridos.Any(a=> a.AssuntoId == assuntoId).ShouldBeTrue();
            
            var autoresCoAutores = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == acervoInserido.Id);
            
            var autores = autoresCoAutores.Where(w => w.TipoAutoria.EhNulo());
            autores.Count().ShouldBe(2);
            foreach (var autorIdInserido in acervoBibliograficoCadastroDto.CreditosAutoresIds)
                autores.Any(a=> a.CreditoAutorId == autorIdInserido).ShouldBeTrue();
            
            var coAutores = autoresCoAutores.Where(w => w.TipoAutoria.NaoEhNulo());
            coAutores.Count().ShouldBe(0);
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Não deve inserir código duplicado")]
        public async Task Nao_deve_inserir_codigo_duplicado()
        {
            await InserirDadosBasicos();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var servicoAcervobibliografico = GetServicoAcervoBibliografico();
            
            var acervoBibliograficoCompleto = GerarAcervoBibliografico().Generate();
            
            var acervoBibliograficoCadastroDto = mapper.Map<AcervoBibliograficoCadastroDTO>(acervoBibliograficoCompleto);
            acervoBibliograficoCadastroDto.CreditosAutoresIds = new long[] { 4,5 };
            acervoBibliograficoCadastroDto.CoAutores = null;
            acervoBibliograficoCadastroDto.AssuntosIds = new long[] { 1, 2 };
            acervoBibliograficoCadastroDto.Titulo = acervoBibliograficoCompleto.Acervo.Titulo;
            acervoBibliograficoCadastroDto.Codigo = new Random().Next(1,35).ToString();
            acervoBibliograficoCadastroDto.SubTitulo = acervoBibliograficoCompleto.Acervo.SubTitulo;
            
            await servicoAcervobibliografico.Inserir(acervoBibliograficoCadastroDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Não deve inserir sem código")]
        public async Task Nao_deve_inserir_sem_codigo()
        {
            await InserirDadosBasicos();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
        
            var servicoAcervobibliografico = GetServicoAcervoBibliografico();
        
            var acervoBibliograficoCompleto = GerarAcervoBibliografico().Generate();
        
            var acervoBibliograficoCadastroDto = mapper.Map<AcervoBibliograficoCadastroDTO>(acervoBibliograficoCompleto);
            acervoBibliograficoCadastroDto.CreditosAutoresIds = new long[] { 4,5 };
            acervoBibliograficoCadastroDto.CoAutores = null;
            acervoBibliograficoCadastroDto.AssuntosIds = new long[] { 1, 2 };
            acervoBibliograficoCadastroDto.Titulo = acervoBibliograficoCompleto.Acervo.Titulo;
            acervoBibliograficoCadastroDto.SubTitulo = acervoBibliograficoCompleto.Acervo.SubTitulo;
        
            await servicoAcervobibliografico.Inserir(acervoBibliograficoCadastroDto).ShouldThrowAsync<NegocioException>();
        }
        
        private async Task InserirAcervoBibliografico(bool inserirCoAutor = true)
        {
            var random = new Random();
            var faker = new Faker("pt_BR");

            for (int j = 1; j <= 35; j++)
            {
                await InserirNaBase(new Acervo()
                {
                    Codigo = j.ToString(),
                    Titulo = faker.Lorem.Sentence().Limite(500),
                    SubTitulo = faker.Lorem.Sentence().Limite(500),
                    TipoAcervoId = (int)TipoAcervo.Bibliografico,
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia().AddMinutes(-15),
                    CriadoLogin = ConstantesTestes.LOGIN_123456789,
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

                if (inserirCoAutor)
                {
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

                    await InserirNaBase(new AcervoCreditoAutor()
                    {
                        AcervoId = j,
                        CreditoAutorId = 3,
                        TipoAutoria = faker.Lorem.Word().Limite(15)
                    });
                }

                await InserirNaBase(new AcervoBibliografico()
                {
                    AcervoId = j,
                    MaterialId = random.Next(1,5),
                    EditoraId = random.Next(1,5),
                    Ano = faker.Date.Recent().Year.ToString(),
                    Edicao = faker.Lorem.Sentence().Limite(15),
                    NumeroPagina = random.Next(15,55),
                    Largura = random.Next(15,55),
                    Altura = random.Next(15,55),
                    SerieColecaoId = random.Next(1,5),
                    Volume = faker.Lorem.Sentence().Limite(15),
                    IdiomaId = random.Next(1,5),
                    LocalizacaoCDD = faker.Lorem.Sentence().Limite(50),
                    LocalizacaoPHA = faker.Lorem.Sentence().Limite(50),
                    NotasGerais = faker.Lorem.Sentence().Limite(500),
                    Isbn = faker.Lorem.Sentence().Limite(50),
                });
                
                await InserirNaBase(new AcervoBibliograficoAssunto()
                {
                    AssuntoId = random.Next(1,5),
                    AcervoBibliograficoId = j
                });
                
                await InserirNaBase(new AcervoBibliograficoAssunto()
                {
                    AssuntoId = random.Next(1,5),
                    AcervoBibliograficoId = j
                });
            }
        }
        
        protected Faker<AcervoBibliografico> GerarAcervoBibliografico()
        {
            var random = new Random();
            var faker = new Faker<AcervoBibliografico>("pt_BR");
            
            faker.RuleFor(x => x.MaterialId, f => random.Next(1,5));
            faker.RuleFor(x => x.EditoraId, f => random.Next(1,5));
            faker.RuleFor(x => x.Ano, f => f.Date.Recent().Year.ToString());
            faker.RuleFor(x => x.Edicao, f => f.Lorem.Sentence().Limite(15));
            faker.RuleFor(x => x.NumeroPagina, f => random.Next(15,55));
            faker.RuleFor(x => x.Largura, f => random.Next(15,55));
            faker.RuleFor(x => x.Altura, f => random.Next(15,55));
            faker.RuleFor(x => x.Volume, f => f.Lorem.Sentence().Limite(15));
            faker.RuleFor(x => x.IdiomaId, f => random.Next(1,5));
            faker.RuleFor(x => x.LocalizacaoCDD, f => f.Lorem.Sentence().Limite(50));
            faker.RuleFor(x => x.LocalizacaoPHA, f => f.Lorem.Sentence().Limite(50));
            faker.RuleFor(x => x.NotasGerais, f => f.Lorem.Text().Limite(500));
            faker.RuleFor(x => x.Isbn, f => f.Lorem.Sentence().Limite(50));
            faker.RuleFor(x => x.Acervo, f => GerarAcervo(TipoAcervo.Bibliografico).Generate());
            return faker;
        }
    }
}