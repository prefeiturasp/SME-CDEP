﻿using Shouldly;
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
    public class Ao_fazer_manutencao_acervo_fotografico : TesteBase
    {
        public Ao_fazer_manutencao_acervo_fotografico(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo fotográfico - Obter por Id")]
        public async Task Obter_por_id()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervoFotografico();
            var servicoAcervoFotografico = GetServicoAcervoFotografico();

            var acervoFotograficoDtos = await servicoAcervoFotografico.ObterPorId(5);
            acervoFotograficoDtos.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervoFotografico();
            var servicoAcervoFotografico = GetServicoAcervoFotografico();

            var acervoFotograficoDtos = await servicoAcervoFotografico.ObterTodos();
            acervoFotograficoDtos.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Atualizar (Adicionando 4 novos arquivos, sendo 1 existente)")]
        public async Task Atualizar_com_4_novos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoFotografico();
            
            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var servicoAcervoFotografico = GetServicoAcervoFotografico();

            var acervoFotograficoAlteracaoDto = new AcervoFotograficoAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.FT",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{1,2,3,4,5},
                Descricao = faker.Lorem.Text(),
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = DateTimeExtension.HorarioBrasilia().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = 1,
                Quantidade = 25,
                Largura = "50,45",
                Altura = "10,20",
                SuporteId = 2,
                FormatoId = 3,
                CromiaId = 4,
                Resolucao = faker.Lorem.Text().Limite(15),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoFotografico.Alterar(acervoFotograficoAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoFotograficoAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoFotograficoAlteracaoDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals(acervoFotograficoAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.DataAcervo.ShouldBe(acervoFotograficoAlteracaoDto.DataAcervo);
            acervo.Ano.ShouldBe(acervoFotograficoAlteracaoDto.Ano);
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Fotografico);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            
            var acervoFotografico = ObterTodos<AcervoFotografico>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoFotografico.Localizacao.ShouldBe(acervoFotograficoAlteracaoDto.Localizacao);
            acervoFotografico.Procedencia.ShouldBe(acervoFotograficoAlteracaoDto.Procedencia);
            acervoFotografico.CopiaDigital.ShouldBe(acervoFotograficoAlteracaoDto.CopiaDigital);
            acervoFotografico.PermiteUsoImagem.ShouldBe(acervoFotograficoAlteracaoDto.PermiteUsoImagem);
            acervoFotografico.ConservacaoId.ShouldBe(acervoFotograficoAlteracaoDto.ConservacaoId);
            acervoFotografico.Quantidade.ShouldBe(acervoFotograficoAlteracaoDto.Quantidade);
            acervoFotografico.Largura.ShouldBe(acervoFotograficoAlteracaoDto.Largura);
            acervoFotografico.Altura.ShouldBe(acervoFotograficoAlteracaoDto.Altura);
            acervoFotografico.SuporteId.ShouldBe(acervoFotograficoAlteracaoDto.SuporteId);
            acervoFotografico.FormatoId.ShouldBe(acervoFotograficoAlteracaoDto.FormatoId);
            acervoFotografico.CromiaId.ShouldBe(acervoFotograficoAlteracaoDto.CromiaId);
            acervoFotografico.Resolucao.ShouldBe(acervoFotograficoAlteracaoDto.Resolucao);
            acervoFotografico.TamanhoArquivo.ShouldBe(acervoFotograficoAlteracaoDto.TamanhoArquivo);
            
            var acervoFotograficoArquivos = ObterTodos<AcervoFotograficoArquivo>();
            var acervoFotograficoArquivosInseridos = acervoFotograficoArquivos.Where(w => w.AcervoFotograficoId == acervoFotografico.Id);
            // acervoFotograficoArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.NaoEhNulo()).ShouldBe(arquivosSelecionados.Count());
            // acervoFotograficoArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.EhNulo()).ShouldBe(0);
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Atualizar (Removendo 1 arquivo, adicionando 5 novos)")]
        public async Task Atualizar_removendo_1_()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoFotografico();
            
            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.OrderByDescending(o=> o.Id).Take(5).Select(s => s.Id).ToArray();
            
            var servicoAcervoFotografico = GetServicoAcervoFotografico();

            var acervoFotograficoAlteracaoDto = new AcervoFotograficoAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.FT",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{3,4,5},
                Descricao = faker.Lorem.Text(),
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = DateTimeExtension.HorarioBrasilia().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = 1,
                Quantidade = 25,
                Largura = "50,45",
                Altura = "10,20",
                SuporteId = 2,
                FormatoId = 3,
                CromiaId = 4,
                Resolucao = faker.Lorem.Text().Limite(15),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoFotografico.Alterar(acervoFotograficoAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoFotograficoAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoFotograficoAlteracaoDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals(acervoFotograficoAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.DataAcervo.ShouldBe(acervoFotograficoAlteracaoDto.DataAcervo);
            acervo.Ano.ShouldBe(acervoFotograficoAlteracaoDto.Ano);
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Fotografico);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            
            var acervoFotografico = ObterTodos<AcervoFotografico>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoFotografico.Localizacao.ShouldBe(acervoFotograficoAlteracaoDto.Localizacao);
            acervoFotografico.Procedencia.ShouldBe(acervoFotograficoAlteracaoDto.Procedencia);
            acervoFotografico.CopiaDigital.ShouldBe(acervoFotograficoAlteracaoDto.CopiaDigital);
            acervoFotografico.PermiteUsoImagem.ShouldBe(acervoFotograficoAlteracaoDto.PermiteUsoImagem);
            acervoFotografico.ConservacaoId.ShouldBe(acervoFotograficoAlteracaoDto.ConservacaoId);
            acervoFotografico.Quantidade.ShouldBe(acervoFotograficoAlteracaoDto.Quantidade);
            acervoFotografico.Largura.ShouldBe(acervoFotograficoAlteracaoDto.Largura);
            acervoFotografico.Altura.ShouldBe(acervoFotograficoAlteracaoDto.Altura);
            acervoFotografico.SuporteId.ShouldBe(acervoFotograficoAlteracaoDto.SuporteId);
            acervoFotografico.FormatoId.ShouldBe(acervoFotograficoAlteracaoDto.FormatoId);
            acervoFotografico.CromiaId.ShouldBe(acervoFotograficoAlteracaoDto.CromiaId);
            acervoFotografico.Resolucao.ShouldBe(acervoFotograficoAlteracaoDto.Resolucao);
            acervoFotografico.TamanhoArquivo.ShouldBe(acervoFotograficoAlteracaoDto.TamanhoArquivo);
            
            var acervoFotograficoArquivos = ObterTodos<AcervoFotograficoArquivo>();
            var acervoFotograficoArquivosInseridos = acervoFotograficoArquivos.Where(w => w.AcervoFotograficoId == acervoFotografico.Id);
            // acervoFotograficoArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.NaoEhNulo()).ShouldBe(arquivosSelecionados.Count());
            // acervoFotograficoArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.EhNulo()).ShouldBe(0);
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(3);
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Atualizar (Removendo todos)")]
        public async Task Atualizar_removendo_todos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoFotografico();
            
            var arquivosSelecionados = Array.Empty<long>();
            
            var servicoAcervoFotografico = GetServicoAcervoFotografico();

            var acervoFotograficoAlteracaoDto = new AcervoFotograficoAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.FT",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{4,5},
                Descricao = faker.Lorem.Text(),
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = DateTimeExtension.HorarioBrasilia().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = 1,
                Quantidade = 25,
                Largura = "50,45",
                Altura = "10,20",
                SuporteId = 2,
                FormatoId = 3,
                CromiaId = 4,
                Resolucao = faker.Lorem.Text().Limite(15),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                Arquivos = arquivosSelecionados
            };
                
            await servicoAcervoFotografico.Alterar(acervoFotograficoAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoFotograficoAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoFotograficoAlteracaoDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals(acervoFotograficoAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.DataAcervo.ShouldBe(acervoFotograficoAlteracaoDto.DataAcervo);
            acervo.Ano.ShouldBe(acervoFotograficoAlteracaoDto.Ano);
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Fotografico);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            
            var acervoFotografico = ObterTodos<AcervoFotografico>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoFotografico.Localizacao.ShouldBe(acervoFotograficoAlteracaoDto.Localizacao);
            acervoFotografico.Procedencia.ShouldBe(acervoFotograficoAlteracaoDto.Procedencia);
            acervoFotografico.CopiaDigital.ShouldBe(acervoFotograficoAlteracaoDto.CopiaDigital);
            acervoFotografico.PermiteUsoImagem.ShouldBe(acervoFotograficoAlteracaoDto.PermiteUsoImagem);
            acervoFotografico.ConservacaoId.ShouldBe(acervoFotograficoAlteracaoDto.ConservacaoId);
            acervoFotografico.Quantidade.ShouldBe(acervoFotograficoAlteracaoDto.Quantidade);
            acervoFotografico.Largura.ShouldBe(acervoFotograficoAlteracaoDto.Largura);
            acervoFotografico.Altura.ShouldBe(acervoFotograficoAlteracaoDto.Altura);
            acervoFotografico.SuporteId.ShouldBe(acervoFotograficoAlteracaoDto.SuporteId);
            acervoFotografico.FormatoId.ShouldBe(acervoFotograficoAlteracaoDto.FormatoId);
            acervoFotografico.CromiaId.ShouldBe(acervoFotograficoAlteracaoDto.CromiaId);
            acervoFotografico.Resolucao.ShouldBe(acervoFotograficoAlteracaoDto.Resolucao);
            acervoFotografico.TamanhoArquivo.ShouldBe(acervoFotograficoAlteracaoDto.TamanhoArquivo);
            
            var acervoFotograficoArquivos = ObterTodos<AcervoFotograficoArquivo>();
            var acervoFotograficoArquivosInseridos = acervoFotograficoArquivos.Where(w => w.AcervoFotograficoId == acervoFotografico.Id);
            acervoFotograficoArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.NaoEhNulo()).ShouldBe(arquivosSelecionados.Count());
            acervoFotograficoArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.EhNulo()).ShouldBe(0);
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(4);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Inserir")]
        public async Task Inserir()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoFotografico();
            
            var servicoAcervoFotografico = GetServicoAcervoFotografico();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();

            var acervoFotograficoDto = new AcervoFotograficoCadastroDTO()
            {
                Codigo = "100",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{new Random().Next(1, 5),new Random().Next(1, 5)},
                Descricao = faker.Lorem.Text(),
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = DateTimeExtension.HorarioBrasilia().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Largura = "50,45",
                Altura = "10,20",
                SuporteId = random.Next(1, 5),
                FormatoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Resolucao = faker.Lorem.Text().Limite(15),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                Arquivos = arquivosSelecionados
            };
            
            var acervoFotograficoInserido = await servicoAcervoFotografico.Inserir(acervoFotograficoDto);
            acervoFotograficoInserido.ShouldBeGreaterThan(1);

            var acervo = ObterTodos<Acervo>().LastOrDefault();
            acervo.Titulo.Equals(acervoFotograficoDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoFotograficoDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals($"{acervoFotograficoDto.Codigo}.FT").ShouldBeTrue();
            acervo.DataAcervo.ShouldBe(acervoFotograficoDto.DataAcervo);
            acervo.Ano.ShouldBe(acervoFotograficoDto.Ano);
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Fotografico);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeFalse();
            acervo.AlteradoPor.ShouldBeNull();
            
            var acervoFotografico = ObterTodos<AcervoFotografico>().LastOrDefault();
            acervoFotografico.Localizacao.ShouldBe(acervoFotograficoDto.Localizacao);
            acervoFotografico.Procedencia.ShouldBe(acervoFotograficoDto.Procedencia);
            acervoFotografico.CopiaDigital.ShouldBe(acervoFotograficoDto.CopiaDigital);
            acervoFotografico.PermiteUsoImagem.ShouldBe(acervoFotograficoDto.PermiteUsoImagem);
            acervoFotografico.ConservacaoId.ShouldBe(acervoFotograficoDto.ConservacaoId);
            acervoFotografico.Quantidade.ShouldBe(acervoFotograficoDto.Quantidade);
            acervoFotografico.Largura.ShouldBe(acervoFotograficoDto.Largura);
            acervoFotografico.Altura.ShouldBe(acervoFotograficoDto.Altura);
            acervoFotografico.SuporteId.ShouldBe(acervoFotograficoDto.SuporteId);
            acervoFotografico.FormatoId.ShouldBe(acervoFotograficoDto.FormatoId);
            acervoFotografico.CromiaId.ShouldBe(acervoFotograficoDto.CromiaId);
            acervoFotografico.Resolucao.ShouldBe(acervoFotograficoDto.Resolucao);
            acervoFotografico.TamanhoArquivo.ShouldBe(acervoFotograficoDto.TamanhoArquivo);
            
            var acervoFotograficoArquivos = ObterTodos<AcervoFotograficoArquivo>();
            var acervoFotograficoArquivosInseridos = acervoFotograficoArquivos.Where(w => w.AcervoFotograficoId == acervoFotografico.Id);
            // acervoFotograficoArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.NaoEhNulo()).ShouldBe(arquivosSelecionados.Count());
            // acervoFotograficoArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.EhNulo()).ShouldBe(0);
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Não deve inserir Tombo duplicado")]
        public async Task Nao_deve_inserir_duplicado()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoFotografico();

            var servicoAcervoFotografico = GetServicoAcervoFotografico();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();

            var acervoFotograficoDto = new AcervoFotograficoCadastroDTO()
            {
                Codigo = "1",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{new Random().Next(1, 5),new Random().Next(1, 5)},
                Descricao = faker.Lorem.Text(),
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = DateTimeExtension.HorarioBrasilia().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Largura = "50,45",
                Altura = "10,20",
                SuporteId = random.Next(1, 5),
                FormatoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Resolucao = faker.Lorem.Text().Limite(15),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoFotografico.Inserir(acervoFotograficoDto).ShouldThrowAsync<NegocioException>();
           
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Não deve inserir com ano futuro")]
        public async Task Nao_deve_inserir_com_ano_futuro()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoFotografico();

            var servicoAcervoFotografico = GetServicoAcervoFotografico();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();

            var acervoFotograficoDto = new AcervoFotograficoCadastroDTO()
            {
                Codigo = "1",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{new Random().Next(1, 5),new Random().Next(1, 5)},
                Descricao = faker.Lorem.Text(),
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = faker.Date.Future().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Largura = "50,45",
                Altura = "10,20",
                SuporteId = random.Next(1, 5),
                FormatoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Resolucao = faker.Lorem.Text().Limite(15),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoFotografico.Inserir(acervoFotograficoDto).ShouldThrowAsync<NegocioException>();
           
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Não deve alterar com ano futuro")]
        public async Task Nao_deve_alterar_com_ano_futuro()
        {
             await InserirDadosBasicosAleatorios();

            await InserirAcervoFotografico();
            
            var arquivosSelecionados = Array.Empty<long>();
            
            var servicoAcervoFotografico = GetServicoAcervoFotografico();

            var acervoFotograficoAlteracaoDto = new AcervoFotograficoAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.FT",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{4,5},
                Descricao = faker.Lorem.Text(),
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = DateTimeExtension.HorarioBrasilia().AddYears(1).Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = 1,
                Quantidade = 25,
                Largura = "50,45",
                Altura = "10,20",
                SuporteId = 2,
                FormatoId = 3,
                CromiaId = 4,
                Resolucao = faker.Lorem.Text().Limite(15),
                TamanhoArquivo = faker.Lorem.Text().Limite(15),
                Arquivos = arquivosSelecionados
            };
                
            await servicoAcervoFotografico.Alterar(acervoFotograficoAlteracaoDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Tratar o retorno de dimensões")]
        public async Task Tratar_retorno_dimensoes()
        {
            var detalhe = new AcervoFotograficoDetalhe() { Largura = "15,20", Altura = "18,20" };
            detalhe.Dimensoes.ShouldBe("15,20(Largura) x 18,20(Altura)");
            
            detalhe = new AcervoFotograficoDetalhe() { Largura = "15,20", Altura = "18,20" };
            detalhe.Dimensoes.ShouldBe("15,20(Largura) x 18,20(Altura)");
            
            detalhe = new AcervoFotograficoDetalhe() { Largura = "15,20"};
            detalhe.Dimensoes.ShouldBe("15,20(Largura)");
            
            detalhe = new AcervoFotograficoDetalhe() { Altura = "18,20" };
            detalhe.Dimensoes.ShouldBe("18,20(Altura)");
            
            detalhe = new AcervoFotograficoDetalhe() { Largura = "15,20" };
            detalhe.Dimensoes.ShouldBe("15,20(Largura)");
            
            detalhe = new AcervoFotograficoDetalhe() { Altura = "18,20" };
            detalhe.Dimensoes.ShouldBe("18,20(Altura)");
            
            detalhe = new AcervoFotograficoDetalhe();
            detalhe.Dimensoes.ShouldBe(string.Empty);
        }

        private async Task InserirAcervoFotografico()
        {
            var random = new Random();

            var arquivoId = 1;
            
            for (int j = 1; j <= 35; j++)
            {
                await InserirNaBase(new Acervo()
                {
                    Codigo = $"{j.ToString()}.FT",
                    Titulo = faker.Lorem.Text().Limite(500),
                    Descricao = faker.Lorem.Text(),
                    CreditosAutoresIds = new long[]{new Random().Next(1, 5),new Random().Next(1, 5)},
                    TipoAcervoId = (int)TipoAcervo.Fotografico,
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia().AddMinutes(-15),
                    CriadoLogin = ConstantesTestes.LOGIN_123456789,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    AlteradoPor = ConstantesTestes.SISTEMA,
                    AlteradoLogin = ConstantesTestes.LOGIN_123456789,
                    Ano = DateTimeExtension.HorarioBrasilia().Year.ToString(),
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

                await InserirNaBase(new AcervoFotografico()
                {
                    AcervoId = j,
                    Localizacao = faker.Lorem.Text().Limite(100),
                    Procedencia = faker.Lorem.Text().Limite(200),
                    CopiaDigital = true,
                    PermiteUsoImagem = true,
                    ConservacaoId = random.Next(1,5),
                    Quantidade = random.Next(15,55),
                    Largura = "50,45",
                    Altura = "10,20",
                    SuporteId = random.Next(1,5),
                    FormatoId = random.Next(1,5),
                    CromiaId = random.Next(1,5),
                    Resolucao = faker.Lorem.Text().Limite(15),
                    TamanhoArquivo = faker.Lorem.Text().Limite(15),
                });
                
                await InserirNaBase(new Arquivo()
                {
                    Nome = $"{faker.Lorem.Text()}_{j}.jpeg",
                    Codigo = Guid.NewGuid(),
                    Tipo = TipoArquivo.AcervoFotografico,
                    TipoConteudo = ConstantesTestes.MIME_TYPE_JPG,
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia().AddMinutes(-15),
                    CriadoLogin = ConstantesTestes.LOGIN_123456789,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    AlteradoPor = ConstantesTestes.SISTEMA,
                    AlteradoLogin = ConstantesTestes.LOGIN_123456789
                });
                
                arquivoId++;
                
                await InserirNaBase(new Arquivo()
                {
                    Nome = $"{faker.Lorem.Text()}_{arquivoId}.jpeg",
                    Codigo = Guid.NewGuid(),
                    Tipo = TipoArquivo.AcervoArteGrafica,
                    TipoConteudo = ConstantesTestes.MIME_TYPE_JPG,
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia().AddMinutes(-15),
                    CriadoLogin = ConstantesTestes.LOGIN_123456789,
                });
                
                await InserirNaBase(new AcervoFotograficoArquivo()
                {
                    ArquivoId = arquivoId-1,
                    AcervoFotograficoId = j,
                    ArquivoMiniaturaId = arquivoId
                });
                
                arquivoId++;
            }
        }
    }
}