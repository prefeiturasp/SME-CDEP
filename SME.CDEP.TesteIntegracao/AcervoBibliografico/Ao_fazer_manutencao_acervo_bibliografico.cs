﻿using Bogus;
using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
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
        private readonly IServicoAcervoBibliografico servicoAcervoBibliografico;

        public Ao_fazer_manutencao_acervo_bibliografico(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            servicoAcervoBibliografico = GetServicoAcervoBibliografico();
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Obter por Id")]
        public async Task Obter_por_id()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervoBibliografico();

            var acervoBibliograficoDto = await servicoAcervoBibliografico.ObterPorId(5);
            acervoBibliograficoDto.ShouldNotBeNull();
            acervoBibliograficoDto.CreditosAutoresIds.Any().ShouldBeTrue();
            acervoBibliograficoDto.CoAutores.Any().ShouldBeTrue();
            acervoBibliograficoDto.CoAutores.Any(a=> a.TipoAutoria.NaoEhNulo()).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Obter por Id sem coAutor")]
        public async Task Obter_por_id_sem_coautor()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervoBibliografico(false);

            var acervo = await servicoAcervoBibliografico.ObterPorId(5);
            acervo.ShouldNotBeNull();
            acervo.CreditosAutoresIds.Any().ShouldBeTrue();
            acervo.CoAutores.Any().ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervoBibliografico();

            var acervoBibliograficoDtos = await servicoAcervoBibliografico.ObterTodos();
            acervoBibliograficoDtos.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Atualizar - CoAutores: removendo 3 e adicionando 1 novo")]
        public async Task Atualizar_removendo_3_e_inserindo_1_coautor()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var acervoBibliograficoCompleto = AcervoBibliograficoMock.Instance.Gerar().Generate();
            
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
            acervoBibliograficoAlteracaoDto.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
            
            await servicoAcervoBibliografico.Alterar(acervoBibliograficoAlteracaoDto);

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
            acervoAlterado.Ano.ShouldBe(acervoBibliograficoAlteracaoDto.Ano);
            
           var acervoBibliografico = ObterTodos<AcervoBibliografico>().FirstOrDefault(w=> w.AcervoId == 3);
           acervoBibliografico.MaterialId.ShouldBe(acervoBibliograficoAlteracaoDto.MaterialId);
           acervoBibliografico.EditoraId.ShouldBe(acervoBibliograficoAlteracaoDto.EditoraId);
           acervoBibliografico.Edicao.ShouldBe(acervoBibliograficoAlteracaoDto.Edicao);
           acervoBibliografico.NumeroPagina.ShouldBe(acervoBibliograficoAlteracaoDto.NumeroPagina);
           acervoBibliografico.Largura.ShouldBe(acervoBibliograficoAlteracaoDto.Largura);
           acervoBibliografico.Altura.ShouldBe(acervoBibliograficoAlteracaoDto.Altura);
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
                autores.Any(a=> a.CreditoAutorId == autorIdInserido && !a.EhCoAutor).ShouldBeTrue();
            
            var coAutores = autoresCoAutores.Where(w => w.TipoAutoria.NaoEhNulo());
            coAutores.Count().ShouldBe(1);
            foreach (var coAutorInserido in acervoBibliograficoAlteracaoDto.CoAutores)
            {
                coAutores.Any(a => a.CreditoAutorId == coAutorInserido.CreditoAutorId && a.EhCoAutor).ShouldBeTrue();
                coAutores.Any(a => a.TipoAutoria == coAutorInserido.TipoAutoria && a.EhCoAutor).ShouldBeTrue();
            }
        }
        
         [Fact(DisplayName = "Acervo bibliografico - Não deve atualizar com ano futuro")]
        public async Task Nao_deve_atualizar_com_ano_futuro()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var acervoBibliograficoCompleto = AcervoBibliograficoMock.Instance.Gerar().Generate();
            
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
            acervoBibliograficoAlteracaoDto.Ano = DateTimeExtension.HorarioBrasilia().AddYears(1).Year.ToString();
            
            await servicoAcervoBibliografico.Alterar(acervoBibliograficoAlteracaoDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Atualizar - CoAutores: removendo 3 e adicionando 1 novo e Código")]
        public async Task Atualizar_removendo_3_e_inserindo_1_coautor_e_codigo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var acervoBibliograficoCompleto = AcervoBibliograficoMock.Instance.Gerar().Generate();
            
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
            acervoBibliograficoAlteracaoDto.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
            
            await servicoAcervoBibliografico.Alterar(acervoBibliograficoAlteracaoDto);

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
           acervoBibliografico.Edicao.ShouldBe(acervoBibliograficoAlteracaoDto.Edicao);
           acervoBibliografico.NumeroPagina.ShouldBe(acervoBibliograficoAlteracaoDto.NumeroPagina);
           acervoBibliografico.Largura.ShouldBe(acervoBibliograficoAlteracaoDto.Largura);
           acervoBibliografico.Altura.ShouldBe(acervoBibliograficoAlteracaoDto.Altura);
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
                autores.Any(a=> a.CreditoAutorId == autorIdInserido && !a.EhCoAutor).ShouldBeTrue();
            
            var coAutores = autoresCoAutores.Where(w => w.TipoAutoria.NaoEhNulo());
            coAutores.Count().ShouldBe(1);
            foreach (var coAutorInserido in acervoBibliograficoAlteracaoDto.CoAutores)
            {
                coAutores.Any(a => a.CreditoAutorId == coAutorInserido.CreditoAutorId).ShouldBeTrue();
                coAutores.Any(a => a.TipoAutoria == coAutorInserido.TipoAutoria && a.EhCoAutor).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Não deve atualizar sem Código")]
        public async Task Nao_deve_atualizar_sem_codigo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var acervoBibliograficoCompleto = AcervoBibliograficoMock.Instance.Gerar().Generate();
            
            var acervoAtual = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            
            var acervoBibliograficoAlteracaoDto = mapper.Map<AcervoBibliograficoAlteracaoDTO>(acervoBibliograficoCompleto);
            acervoBibliograficoAlteracaoDto.CreditosAutoresIds = new long[] { 1, 2, 3 };
            acervoBibliograficoAlteracaoDto.CoAutores = new CoAutorDTO[] { new () { CreditoAutorId = 4, TipoAutoria = faker.Lorem.Word().Limite(15)} };
            acervoBibliograficoAlteracaoDto.AssuntosIds = new long[] { 1, 2 };
            acervoBibliograficoAlteracaoDto.Id = 3;
            acervoBibliograficoAlteracaoDto.Titulo = acervoBibliograficoCompleto.Acervo.Titulo;
            acervoBibliograficoAlteracaoDto.SubTitulo = acervoBibliograficoCompleto.Acervo.SubTitulo;
            acervoBibliograficoAlteracaoDto.AcervoId = acervoAtual.Id;
            acervoBibliograficoAlteracaoDto.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
            
            await servicoAcervoBibliografico.Alterar(acervoBibliograficoAlteracaoDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Não deve atualizar com Código duplicado")]
        public async Task Nao_deve_atualizar_com_codigo_duplicado()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var acervoBibliograficoCompleto = AcervoBibliograficoMock.Instance.Gerar().Generate();
            
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
            acervoBibliograficoAlteracaoDto.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
            
            await servicoAcervoBibliografico.Alterar(acervoBibliograficoAlteracaoDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Atualizar removendo todos os CoAutores")]
        public async Task Atualizar_removendo_todos_coAutores()
        {
           await InserirDadosBasicosAleatorios();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var acervoBibliograficoCompleto = AcervoBibliograficoMock.Instance.Gerar().Generate();
            
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
            acervoBibliograficoAlteracaoDto.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
            
            await servicoAcervoBibliografico.Alterar(acervoBibliograficoAlteracaoDto);

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
           acervoBibliografico.Edicao.ShouldBe(acervoBibliograficoAlteracaoDto.Edicao);
           acervoBibliografico.NumeroPagina.ShouldBe(acervoBibliograficoAlteracaoDto.NumeroPagina);
           acervoBibliografico.Largura.ShouldBe(acervoBibliograficoAlteracaoDto.Largura);
           acervoBibliografico.Altura.ShouldBe(acervoBibliograficoAlteracaoDto.Altura);
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
                autores.Any(a=> a.CreditoAutorId == autorIdInserido && !a.EhCoAutor).ShouldBeTrue();
            
            var coAutores = autoresCoAutores.Where(w => w.TipoAutoria.NaoEhNulo());
            coAutores.Count().ShouldBe(0);
        }
       
        [Fact(DisplayName = "Acervo bibliografico - Inserir")]
        public async Task Inserir()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var acervoBibliograficoCompleto = AcervoBibliograficoMock.Instance.Gerar().Generate();
            
            var acervoBibliograficoCadastroDto = mapper.Map<AcervoBibliograficoCadastroDTO>(acervoBibliograficoCompleto);
            acervoBibliograficoCadastroDto.CreditosAutoresIds = new long[] { 1, 2, 3 };
            acervoBibliograficoCadastroDto.CoAutores = new CoAutorDTO[] { new () { CreditoAutorId = 4, TipoAutoria = faker.Lorem.Word().Limite(15)} };
            acervoBibliograficoCadastroDto.AssuntosIds = new long[] { 1, 2 };
            acervoBibliograficoCadastroDto.Titulo = acervoBibliograficoCompleto.Acervo.Titulo;
            acervoBibliograficoCadastroDto.Codigo = new Random().Next(100,200).ToString();
            acervoBibliograficoCadastroDto.SubTitulo = acervoBibliograficoCompleto.Acervo.SubTitulo;
            acervoBibliograficoCadastroDto.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
            
            await servicoAcervoBibliografico.Inserir(acervoBibliograficoCadastroDto);

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
            acervoInserido.Ano.ShouldBe(acervoBibliograficoCadastroDto.Ano);
            
           var acervoBibliografico = ObterTodos<AcervoBibliografico>().OrderBy(o=> o.Id).LastOrDefault();
           acervoBibliografico.MaterialId.ShouldBe(acervoBibliograficoCadastroDto.MaterialId);
           acervoBibliografico.EditoraId.ShouldBe(acervoBibliograficoCadastroDto.EditoraId);
           acervoBibliografico.Edicao.ShouldBe(acervoBibliograficoCadastroDto.Edicao);
           acervoBibliografico.NumeroPagina.ShouldBe(acervoBibliograficoCadastroDto.NumeroPagina);
           acervoBibliografico.Largura.ShouldBe(acervoBibliograficoCadastroDto.Largura);
           acervoBibliografico.Altura.ShouldBe(acervoBibliograficoCadastroDto.Altura);
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
                autores.Any(a=> a.CreditoAutorId == autorIdInserido && !a.EhCoAutor).ShouldBeTrue();
            
            var coAutores = autoresCoAutores.Where(w => w.TipoAutoria.NaoEhNulo());
            coAutores.Count().ShouldBe(1);
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Inserindo sem CoAutores")]
        public async Task Inserir_sem_coautores()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var acervoBibliograficoCompleto = AcervoBibliograficoMock.Instance.Gerar().Generate();
            
            var acervoBibliograficoCadastroDto = mapper.Map<AcervoBibliograficoCadastroDTO>(acervoBibliograficoCompleto);
            acervoBibliograficoCadastroDto.CreditosAutoresIds = new long[] { 4,5 };
            acervoBibliograficoCadastroDto.CoAutores = null;
            acervoBibliograficoCadastroDto.AssuntosIds = new long[] { 1, 2 };
            acervoBibliograficoCadastroDto.Titulo = acervoBibliograficoCompleto.Acervo.Titulo;
            acervoBibliograficoCadastroDto.Codigo = new Random().Next(100,200).ToString();
            acervoBibliograficoCadastroDto.SubTitulo = acervoBibliograficoCompleto.Acervo.SubTitulo;
            acervoBibliograficoCadastroDto.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
            
            await servicoAcervoBibliografico.Inserir(acervoBibliograficoCadastroDto);

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
            acervoInserido.Ano.ShouldBe(acervoBibliograficoCadastroDto.Ano);
            
           var acervoBibliografico = ObterTodos<AcervoBibliografico>().OrderBy(o=> o.Id).LastOrDefault();
           acervoBibliografico.MaterialId.ShouldBe(acervoBibliograficoCadastroDto.MaterialId);
           acervoBibliografico.EditoraId.ShouldBe(acervoBibliograficoCadastroDto.EditoraId);
           acervoBibliografico.Edicao.ShouldBe(acervoBibliograficoCadastroDto.Edicao);
           acervoBibliografico.NumeroPagina.ShouldBe(acervoBibliograficoCadastroDto.NumeroPagina);
           acervoBibliografico.Largura.ShouldBe(acervoBibliograficoCadastroDto.Largura);
           acervoBibliografico.Altura.ShouldBe(acervoBibliograficoCadastroDto.Altura);
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
            await InserirDadosBasicosAleatorios();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var acervoBibliograficoCompleto = AcervoBibliograficoMock.Instance.Gerar().Generate();
            
            var acervoBibliograficoCadastroDto = mapper.Map<AcervoBibliograficoCadastroDTO>(acervoBibliograficoCompleto);
            acervoBibliograficoCadastroDto.CreditosAutoresIds = new long[] { 4,5 };
            acervoBibliograficoCadastroDto.CoAutores = null;
            acervoBibliograficoCadastroDto.AssuntosIds = new long[] { 1, 2 };
            acervoBibliograficoCadastroDto.Titulo = acervoBibliograficoCompleto.Acervo.Titulo;
            acervoBibliograficoCadastroDto.Codigo = new Random().Next(1,35).ToString();
            acervoBibliograficoCadastroDto.SubTitulo = acervoBibliograficoCompleto.Acervo.SubTitulo;
            acervoBibliograficoCadastroDto.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
            
            await servicoAcervoBibliografico.Inserir(acervoBibliograficoCadastroDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Não deve inserir com ano futuro")]
        public async Task Nao_deve_inserir_com_ano_futuro()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
            
            var acervoBibliograficoCompleto = AcervoBibliograficoMock.Instance.Gerar().Generate();
            
            var acervoBibliograficoCadastroDto = mapper.Map<AcervoBibliograficoCadastroDTO>(acervoBibliograficoCompleto);
            acervoBibliograficoCadastroDto.CreditosAutoresIds = new long[] { 4,5 };
            acervoBibliograficoCadastroDto.CoAutores = null;
            acervoBibliograficoCadastroDto.AssuntosIds = new long[] { 1, 2 };
            acervoBibliograficoCadastroDto.Titulo = acervoBibliograficoCompleto.Acervo.Titulo;
            acervoBibliograficoCadastroDto.Codigo = new Random().Next(1,35).ToString();
            acervoBibliograficoCadastroDto.SubTitulo = acervoBibliograficoCompleto.Acervo.SubTitulo;
            acervoBibliograficoCadastroDto.Ano = faker.Date.Future().Year.ToString();
            
            await servicoAcervoBibliografico.Inserir(acervoBibliograficoCadastroDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Não deve inserir sem código")]
        public async Task Nao_deve_inserir_sem_codigo()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoBibliografico();

            var mapper = GetServicoMapper();
        
            var acervoBibliograficoCompleto = AcervoBibliograficoMock.Instance.Gerar().Generate();
        
            var acervoBibliograficoCadastroDto = mapper.Map<AcervoBibliograficoCadastroDTO>(acervoBibliograficoCompleto);
            acervoBibliograficoCadastroDto.CreditosAutoresIds = new long[] { 4,5 };
            acervoBibliograficoCadastroDto.CoAutores = null;
            acervoBibliograficoCadastroDto.AssuntosIds = new long[] { 1, 2 };
            acervoBibliograficoCadastroDto.Titulo = acervoBibliograficoCompleto.Acervo.Titulo;
            acervoBibliograficoCadastroDto.SubTitulo = acervoBibliograficoCompleto.Acervo.SubTitulo;
            acervoBibliograficoCadastroDto.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
            
            await servicoAcervoBibliografico.Inserir(acervoBibliograficoCadastroDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo bibliografico - Tratar o retorno de dimensões")]
        public async Task Tratar_retorno_dimensoes()
        {
            var detalhe = new AcervoBibliograficoDetalhe() { Largura = "15,20", Altura = "18,20"};
            detalhe.Dimensoes.ShouldBe("15,20(Largura) x 18,20(Altura)");
            
            detalhe = new AcervoBibliograficoDetalhe() { Largura = "15,20", Altura = "18,20"};
            detalhe.Dimensoes.ShouldBe("15,20(Largura) x 18,20(Altura)");
            
            detalhe = new AcervoBibliograficoDetalhe() { Largura = "15,20"};
            detalhe.Dimensoes.ShouldBe("15,20(Largura)");
            
            detalhe = new AcervoBibliograficoDetalhe() { Altura = "18,20"};
            detalhe.Dimensoes.ShouldBe("18,20(Altura)");
            
            detalhe = new AcervoBibliograficoDetalhe() { Largura = "15,20" };
            detalhe.Dimensoes.ShouldBe("15,20(Largura)");
            
            detalhe = new AcervoBibliograficoDetalhe() { Altura = "18,20" };
            detalhe.Dimensoes.ShouldBe("18,20(Altura)");
            
            detalhe = new AcervoBibliograficoDetalhe();
            detalhe.Dimensoes.ShouldBe(string.Empty);
        }
        
        [Theory(DisplayName = "Acervo bibliografico - Deve alterar situacao para reservado/emprestado/disponível")]
        [InlineData(SituacaoSaldo.RESERVADO)]
        [InlineData(SituacaoSaldo.DISPONIVEL)]
        [InlineData(SituacaoSaldo.EMPRESTADO)]
        public async Task Deve_alterar_situacao_saldo(SituacaoSaldo situacaoSaldo)
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoBibliografico();

            await servicoAcervoBibliografico.AlterarSituacaoSaldo(situacaoSaldo,4);
            var acervosBibliograficos = ObterTodos<AcervoBibliografico>();
            acervosBibliograficos.FirstOrDefault(f=> f.Id == 4).SituacaoSaldo.ShouldBe(situacaoSaldo);
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
                        TipoAutoria = faker.Lorem.Word().Limite(15),
                        EhCoAutor = true
                    });

                    await InserirNaBase(new AcervoCreditoAutor()
                    {
                        AcervoId = j,
                        CreditoAutorId = 2,
                        TipoAutoria = faker.Lorem.Word().Limite(15),
                        EhCoAutor = true
                    });

                    await InserirNaBase(new AcervoCreditoAutor()
                    {
                        AcervoId = j,
                        CreditoAutorId = 3,
                        TipoAutoria = faker.Lorem.Word().Limite(15),
                        EhCoAutor = true
                    });
                }

                await InserirNaBase(new AcervoBibliografico()
                {
                    AcervoId = j,
                    MaterialId = random.Next(1,5),
                    EditoraId = random.Next(1,5),
                    Edicao = faker.Lorem.Sentence().Limite(15),
                    NumeroPagina = random.Next(15,55),
                    Largura = "50,45",
                    Altura ="10,20",
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
    }
}