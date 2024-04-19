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
    public class Ao_fazer_manutencao_acervo_arte_grafica : TesteBase
    {
        public Ao_fazer_manutencao_acervo_arte_grafica(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Obter por Id")]
        public async Task Obter_por_id()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervoArteGrafica();
            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();

            var acervoArteGraficaDto = await servicoAcervoArteGrafica.ObterPorId(5);
            acervoArteGraficaDto.CreditosAutoresIds.Any().ShouldBeTrue();
            acervoArteGraficaDto.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Obter por Id sem credor")]
        public async Task Obter_por_id_sem_credor()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervoArteGrafica(false);
            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();

            var acervoArteGraficaDto = await servicoAcervoArteGrafica.ObterPorId(5);
            acervoArteGraficaDto.ShouldNotBeNull();
            acervoArteGraficaDto.CreditosAutoresIds.Any().ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervoArteGrafica();
            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();

            var acervoArteGraficaDtos = await servicoAcervoArteGrafica.ObterTodos();
            acervoArteGraficaDtos.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Atualizar (Adicionando 4 novos arquivos, sendo 1 existente)")]
        public async Task Atualizar_com_4_novos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoArteGrafica();
            
            var random = new Random();
            
            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();

            var acervoArteGraficaAlteracaoDto = new AcervoArteGraficaAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.AG",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{4,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = DateTimeExtension.HorarioBrasilia().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Largura = "50,45",
                Altura = "10,20",
                Diametro = "15,40",
                Tecnica = faker.Lorem.Text().Limite(100),
                SuporteId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoArteGrafica.Alterar(acervoArteGraficaAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoArteGraficaAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoArteGraficaAlteracaoDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals(acervoArteGraficaAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.DataAcervo.ShouldBe(acervoArteGraficaAlteracaoDto.DataAcervo);
            acervo.Ano.ShouldBe(acervoArteGraficaAlteracaoDto.Ano);
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.ArtesGraficas);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            
           var acervoArteGrafica = ObterTodos<AcervoArteGrafica>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoArteGrafica.Localizacao.ShouldBe(acervoArteGraficaAlteracaoDto.Localizacao);
            acervoArteGrafica.Procedencia.ShouldBe(acervoArteGraficaAlteracaoDto.Procedencia);
            acervoArteGrafica.CopiaDigital.ShouldBe(acervoArteGraficaAlteracaoDto.CopiaDigital);
            acervoArteGrafica.PermiteUsoImagem.ShouldBe(acervoArteGraficaAlteracaoDto.PermiteUsoImagem);
            acervoArteGrafica.ConservacaoId.ShouldBe(acervoArteGraficaAlteracaoDto.ConservacaoId);
            acervoArteGrafica.CromiaId.ShouldBe(acervoArteGraficaAlteracaoDto.CromiaId);
            acervoArteGrafica.Largura.ShouldBe(acervoArteGraficaAlteracaoDto.Largura);
            acervoArteGrafica.Altura.ShouldBe(acervoArteGraficaAlteracaoDto.Altura);
            acervoArteGrafica.Diametro.ShouldBe(acervoArteGraficaAlteracaoDto.Diametro);
            acervoArteGrafica.Tecnica.ShouldBe(acervoArteGraficaAlteracaoDto.Tecnica);
            acervoArteGrafica.SuporteId.ShouldBe(acervoArteGraficaAlteracaoDto.SuporteId);
            acervoArteGrafica.Quantidade.ShouldBe(acervoArteGraficaAlteracaoDto.Quantidade);
            
            var acervoArteGraficaArquivos = ObterTodos<AcervoArteGraficaArquivo>();
            var acervoArteGraficaArquivosInseridos = acervoArteGraficaArquivos.Where(w => w.AcervoArteGraficaId == acervoArteGrafica.Id);
            // acervoArteGraficaArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.NaoEhNulo()).ShouldBe(arquivosSelecionados.Count());
            // acervoArteGraficaArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.EhNulo()).ShouldBe(0);
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(4);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Atualizar (Removendo 1 arquivo, adicionando 5 novos)")]
        public async Task Atualizar_removendo_1_()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoArteGrafica();
            
            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.OrderByDescending(o=> o.Id).Take(5).Select(s => s.Id).ToArray();
            
            var random = new Random();
            
            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();

            var acervoArteGraficaAlteracaoDto = new AcervoArteGraficaAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.AG",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{2,3},
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = DateTimeExtension.HorarioBrasilia().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Largura = "50,45",
                Altura = "10,20",
                Diametro = "15,40",
                Tecnica = faker.Lorem.Text().Limite(100),
                SuporteId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados
            };
                
            await servicoAcervoArteGrafica.Alterar(acervoArteGraficaAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoArteGraficaAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoArteGraficaAlteracaoDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals(acervoArteGraficaAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.DataAcervo.ShouldBe(acervoArteGraficaAlteracaoDto.DataAcervo);
            acervo.Ano.ShouldBe(acervoArteGraficaAlteracaoDto.Ano);
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.ArtesGraficas);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            
            var acervoArteGrafica = ObterTodos<AcervoArteGrafica>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoArteGrafica.Localizacao.ShouldBe(acervoArteGraficaAlteracaoDto.Localizacao);
            acervoArteGrafica.Procedencia.ShouldBe(acervoArteGraficaAlteracaoDto.Procedencia);
            acervoArteGrafica.CopiaDigital.ShouldBe(acervoArteGraficaAlteracaoDto.CopiaDigital);
            acervoArteGrafica.PermiteUsoImagem.ShouldBe(acervoArteGraficaAlteracaoDto.PermiteUsoImagem);
            acervoArteGrafica.ConservacaoId.ShouldBe(acervoArteGraficaAlteracaoDto.ConservacaoId);
            acervoArteGrafica.CromiaId.ShouldBe(acervoArteGraficaAlteracaoDto.CromiaId);
            acervoArteGrafica.Largura.ShouldBe(acervoArteGraficaAlteracaoDto.Largura);
            acervoArteGrafica.Altura.ShouldBe(acervoArteGraficaAlteracaoDto.Altura);
            acervoArteGrafica.Diametro.ShouldBe(acervoArteGraficaAlteracaoDto.Diametro);
            acervoArteGrafica.Tecnica.ShouldBe(acervoArteGraficaAlteracaoDto.Tecnica);
            acervoArteGrafica.SuporteId.ShouldBe(acervoArteGraficaAlteracaoDto.SuporteId);
            acervoArteGrafica.Quantidade.ShouldBe(acervoArteGraficaAlteracaoDto.Quantidade);
            
            var acervoArteGraficaArquivos = ObterTodos<AcervoArteGraficaArquivo>();
            var acervoArteGraficaArquivosInseridos = acervoArteGraficaArquivos.Where(w => w.AcervoArteGraficaId == acervoArteGrafica.Id);
            // acervoArteGraficaArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.NaoEhNulo()).ShouldBe(arquivosSelecionados.Count());
            // acervoArteGraficaArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.EhNulo()).ShouldBe(0);
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(2);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(3);
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Atualizar (Removendo todos)")]
        public async Task Atualizar_removendo_todos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoArteGrafica();
            
            var random = new Random();
            
            var arquivosSelecionados = Array.Empty<long>();

            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();

            var acervoArteGraficaAlteracaoDto = new AcervoArteGraficaAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.AG",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{1,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = DateTimeExtension.HorarioBrasilia().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Largura = "50,45",
                Altura = "10,20",
                Diametro = "15,40",
                Tecnica = faker.Lorem.Text().Limite(100),
                SuporteId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados
            };
                
            await servicoAcervoArteGrafica.Alterar(acervoArteGraficaAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoArteGraficaAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoArteGraficaAlteracaoDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals(acervoArteGraficaAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.DataAcervo.ShouldBe(acervoArteGraficaAlteracaoDto.DataAcervo);
            acervo.Ano.ShouldBe(acervoArteGraficaAlteracaoDto.Ano);
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.ArtesGraficas);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            
            var acervoArteGrafica = ObterTodos<AcervoArteGrafica>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoArteGrafica.Localizacao.ShouldBe(acervoArteGraficaAlteracaoDto.Localizacao);
            acervoArteGrafica.Procedencia.ShouldBe(acervoArteGraficaAlteracaoDto.Procedencia);
            acervoArteGrafica.CopiaDigital.ShouldBe(acervoArteGraficaAlteracaoDto.CopiaDigital);
            acervoArteGrafica.PermiteUsoImagem.ShouldBe(acervoArteGraficaAlteracaoDto.PermiteUsoImagem);
            acervoArteGrafica.ConservacaoId.ShouldBe(acervoArteGraficaAlteracaoDto.ConservacaoId);
            acervoArteGrafica.CromiaId.ShouldBe(acervoArteGraficaAlteracaoDto.CromiaId);
            acervoArteGrafica.Largura.ShouldBe(acervoArteGraficaAlteracaoDto.Largura);
            acervoArteGrafica.Altura.ShouldBe(acervoArteGraficaAlteracaoDto.Altura);
            acervoArteGrafica.Diametro.ShouldBe(acervoArteGraficaAlteracaoDto.Diametro);
            acervoArteGrafica.Tecnica.ShouldBe(acervoArteGraficaAlteracaoDto.Tecnica);
            acervoArteGrafica.SuporteId.ShouldBe(acervoArteGraficaAlteracaoDto.SuporteId);
            acervoArteGrafica.Quantidade.ShouldBe(acervoArteGraficaAlteracaoDto.Quantidade);
            
            var acervoArteGraficaArquivos = ObterTodos<AcervoArteGraficaArquivo>();
            var acervoArteGraficaArquivosInseridos = acervoArteGraficaArquivos.Where(w => w.AcervoArteGraficaId == acervoArteGrafica.Id);
            acervoArteGraficaArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.NaoEhNulo()).ShouldBe(arquivosSelecionados.Count());
            acervoArteGraficaArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.EhNulo()).ShouldBe(0);
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(1);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Atualizar (Removendo todos) - largura, altura e diâmetro como literais")]
        public async Task Atualizar_removendo_todos_com_largura_altura_diametro_como_literais()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoArteGrafica();
            
            var random = new Random();
            
            var arquivosSelecionados = Array.Empty<long>();

            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();

            var acervoArteGraficaAlteracaoDto = new AcervoArteGraficaAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.AG",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{1,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = DateTimeExtension.HorarioBrasilia().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Largura = "50,45",
                Altura = "10,20",
                Diametro ="15,40",
                Tecnica = faker.Lorem.Text().Limite(100),
                SuporteId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados
            };
                
            await servicoAcervoArteGrafica.Alterar(acervoArteGraficaAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoArteGraficaAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoArteGraficaAlteracaoDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals(acervoArteGraficaAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.DataAcervo.ShouldBe(acervoArteGraficaAlteracaoDto.DataAcervo);
            acervo.Ano.ShouldBe(acervoArteGraficaAlteracaoDto.Ano);
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.ArtesGraficas);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            
            var acervoArteGrafica = ObterTodos<AcervoArteGrafica>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoArteGrafica.Localizacao.ShouldBe(acervoArteGraficaAlteracaoDto.Localizacao);
            acervoArteGrafica.Procedencia.ShouldBe(acervoArteGraficaAlteracaoDto.Procedencia);
            acervoArteGrafica.CopiaDigital.ShouldBe(acervoArteGraficaAlteracaoDto.CopiaDigital);
            acervoArteGrafica.PermiteUsoImagem.ShouldBe(acervoArteGraficaAlteracaoDto.PermiteUsoImagem);
            acervoArteGrafica.ConservacaoId.ShouldBe(acervoArteGraficaAlteracaoDto.ConservacaoId);
            acervoArteGrafica.CromiaId.ShouldBe(acervoArteGraficaAlteracaoDto.CromiaId);
            acervoArteGrafica.Largura.ShouldBe(acervoArteGraficaAlteracaoDto.Largura);
            acervoArteGrafica.Altura.ShouldBe(acervoArteGraficaAlteracaoDto.Altura);
            acervoArteGrafica.Diametro.ShouldBe(acervoArteGraficaAlteracaoDto.Diametro);
            acervoArteGrafica.Tecnica.ShouldBe(acervoArteGraficaAlteracaoDto.Tecnica);
            acervoArteGrafica.SuporteId.ShouldBe(acervoArteGraficaAlteracaoDto.SuporteId);
            acervoArteGrafica.Quantidade.ShouldBe(acervoArteGraficaAlteracaoDto.Quantidade);
            
            var acervoArteGraficaArquivos = ObterTodos<AcervoArteGraficaArquivo>();
            var acervoArteGraficaArquivosInseridos = acervoArteGraficaArquivos.Where(w => w.AcervoArteGraficaId == acervoArteGrafica.Id);
            acervoArteGraficaArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.NaoEhNulo()).ShouldBe(arquivosSelecionados.Count());
            acervoArteGraficaArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.EhNulo()).ShouldBe(0);
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(1);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Não deve atualizar com ano futuro")]
        public async Task Nao_deve_atualizar_ano_futuro()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoArteGrafica();
            
            var random = new Random();
            
            var arquivosSelecionados = Array.Empty<long>();

            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();

            var acervoArteGraficaAlteracaoDto = new AcervoArteGraficaAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.AG",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{1,5},
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = DateTimeExtension.HorarioBrasilia().AddYears(1).Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Largura = "15,55",
                Altura = "12,30",
                Diametro = "16,45",
                Tecnica = faker.Lorem.Text().Limite(100),
                SuporteId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados
            };
                
            await servicoAcervoArteGrafica.Alterar(acervoArteGraficaAlteracaoDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Inserir")]
        public async Task Inserir()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoArteGrafica();

            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();

            var acervoArteGraficaCadastroDto = new AcervoArteGraficaCadastroDTO()
            {
                Codigo = "100",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{3,4},
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = DateTimeExtension.HorarioBrasilia().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Largura = "50,45",
                Altura = "10,20",
                Diametro = "15,40",
                Tecnica = faker.Lorem.Text().Limite(100),
                SuporteId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados
            };
            
            var acervoArteGraficaInserido = await servicoAcervoArteGrafica.Inserir(acervoArteGraficaCadastroDto);
            acervoArteGraficaInserido.ShouldBeGreaterThan(1);

            var acervo = ObterTodos<Acervo>().LastOrDefault();
            acervo.Titulo.Equals(acervoArteGraficaCadastroDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoArteGraficaCadastroDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals($"{acervoArteGraficaCadastroDto.Codigo}.AG").ShouldBeTrue();
            acervo.DataAcervo.ShouldBe(acervoArteGraficaCadastroDto.DataAcervo);
            acervo.Ano.ShouldBe(acervoArteGraficaCadastroDto.Ano);
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.ArtesGraficas);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeFalse();
            acervo.AlteradoPor.ShouldBeNull();
            
            var acervoArteGrafica = ObterTodos<AcervoArteGrafica>().LastOrDefault();
            acervoArteGrafica.Localizacao.ShouldBe(acervoArteGraficaCadastroDto.Localizacao);
            acervoArteGrafica.Procedencia.ShouldBe(acervoArteGraficaCadastroDto.Procedencia);
            acervoArteGrafica.CopiaDigital.ShouldBe(acervoArteGraficaCadastroDto.CopiaDigital);
            acervoArteGrafica.PermiteUsoImagem.ShouldBe(acervoArteGraficaCadastroDto.PermiteUsoImagem);
            acervoArteGrafica.ConservacaoId.ShouldBe(acervoArteGraficaCadastroDto.ConservacaoId);
            acervoArteGrafica.CromiaId.ShouldBe(acervoArteGraficaCadastroDto.CromiaId);
            acervoArteGrafica.Largura.ShouldBe(acervoArteGraficaCadastroDto.Largura);
            acervoArteGrafica.Altura.ShouldBe(acervoArteGraficaCadastroDto.Altura);
            acervoArteGrafica.Diametro.ShouldBe(acervoArteGraficaCadastroDto.Diametro);
            acervoArteGrafica.Tecnica.ShouldBe(acervoArteGraficaCadastroDto.Tecnica);
            acervoArteGrafica.SuporteId.ShouldBe(acervoArteGraficaCadastroDto.SuporteId);
            acervoArteGrafica.Quantidade.ShouldBe(acervoArteGraficaCadastroDto.Quantidade);
            
            var acervoArteGraficaArquivos = ObterTodos<AcervoArteGraficaArquivo>();
            var acervoArteGraficaArquivosInseridos = acervoArteGraficaArquivos.Where(w => w.AcervoArteGraficaId == acervoArteGrafica.Id);
            // acervoArteGraficaArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.NaoEhNulo()).ShouldBe(arquivosSelecionados.Count());
            // acervoArteGraficaArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.EhNulo()).ShouldBe(0);
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == acervoArteGrafica.AcervoId);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(3);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(4);
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Inserir - largura, altura e diâmetro como literais")]
        public async Task Inserir_com_largura_altura_e_diametro_como_literais()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoArteGrafica();

            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();

            var acervoArteGraficaCadastroDto = new AcervoArteGraficaCadastroDTO()
            {
                Codigo = "100",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{3,4},
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = DateTimeExtension.HorarioBrasilia().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Largura = "10,20",
                Altura = "30,15",
                Diametro = "45,32",
                Tecnica = faker.Lorem.Text().Limite(100),
                SuporteId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados
            };
            
            var acervoArteGraficaInserido = await servicoAcervoArteGrafica.Inserir(acervoArteGraficaCadastroDto);
            acervoArteGraficaInserido.ShouldBeGreaterThan(1);

            var acervo = ObterTodos<Acervo>().LastOrDefault();
            acervo.Titulo.Equals(acervoArteGraficaCadastroDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoArteGraficaCadastroDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals($"{acervoArteGraficaCadastroDto.Codigo}.AG").ShouldBeTrue();
            acervo.DataAcervo.ShouldBe(acervoArteGraficaCadastroDto.DataAcervo);
            acervo.Ano.ShouldBe(acervoArteGraficaCadastroDto.Ano);
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.ArtesGraficas);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeFalse();
            acervo.AlteradoPor.ShouldBeNull();
            
            var acervoArteGrafica = ObterTodos<AcervoArteGrafica>().LastOrDefault();
            acervoArteGrafica.Localizacao.ShouldBe(acervoArteGraficaCadastroDto.Localizacao);
            acervoArteGrafica.Procedencia.ShouldBe(acervoArteGraficaCadastroDto.Procedencia);
            acervoArteGrafica.CopiaDigital.ShouldBe(acervoArteGraficaCadastroDto.CopiaDigital);
            acervoArteGrafica.PermiteUsoImagem.ShouldBe(acervoArteGraficaCadastroDto.PermiteUsoImagem);
            acervoArteGrafica.ConservacaoId.ShouldBe(acervoArteGraficaCadastroDto.ConservacaoId);
            acervoArteGrafica.CromiaId.ShouldBe(acervoArteGraficaCadastroDto.CromiaId);
            acervoArteGrafica.Largura.ShouldBe(acervoArteGraficaCadastroDto.Largura);
            acervoArteGrafica.Altura.ShouldBe(acervoArteGraficaCadastroDto.Altura);
            acervoArteGrafica.Diametro.ShouldBe(acervoArteGraficaCadastroDto.Diametro);
            acervoArteGrafica.Tecnica.ShouldBe(acervoArteGraficaCadastroDto.Tecnica);
            acervoArteGrafica.SuporteId.ShouldBe(acervoArteGraficaCadastroDto.SuporteId);
            acervoArteGrafica.Quantidade.ShouldBe(acervoArteGraficaCadastroDto.Quantidade);
            
            var acervoArteGraficaArquivos = ObterTodos<AcervoArteGraficaArquivo>();
            var acervoArteGraficaArquivosInseridos = acervoArteGraficaArquivos.Where(w => w.AcervoArteGraficaId == acervoArteGrafica.Id);
            // acervoArteGraficaArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.NaoEhNulo()).ShouldBe(arquivosSelecionados.Count());
            // acervoArteGraficaArquivosInseridos.Count(w=> w.ArquivoMiniaturaId.EhNulo()).ShouldBe(0);
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == acervoArteGrafica.AcervoId);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(3);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(4);
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Não deve inserir Tombo duplicado")]
        public async Task Nao_deve_inserir_duplicado()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoArteGrafica();

            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();

            var acervoArteGraficaCadastroDto = new AcervoArteGraficaCadastroDTO()
            {
                Codigo = "1",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{new Random().Next(1, 5),new Random().Next(1, 5)},
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = DateTimeExtension.HorarioBrasilia().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Largura = "10,20",
                Altura = "30,15",
                Diametro = "45,32",
                Tecnica = faker.Lorem.Text().Limite(100),
                SuporteId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoArteGrafica.Inserir(acervoArteGraficaCadastroDto).ShouldThrowAsync<NegocioException>();
           
        }

        [Fact(DisplayName = "Acervo Arte Gráfica - Não deve inserir com ano futuro")]
        public async Task Nao_deve_inserir_com_ano_futuro()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoArteGrafica();

            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();

            var acervoArteGraficaCadastroDto = new AcervoArteGraficaCadastroDTO()
            {
                Codigo = "1",
                Titulo = faker.Lorem.Text().Limite(500),
                CreditosAutoresIds = new long[]{new Random().Next(1, 5),new Random().Next(1, 5)},
                Localizacao = faker.Lorem.Text().Limite(100),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = faker.Date.Future().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Largura = "10,20",
                Altura = "30,15",
                Diametro = "45,32",
                Tecnica = faker.Lorem.Text().Limite(100),
                SuporteId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoArteGrafica.Inserir(acervoArteGraficaCadastroDto).ShouldThrowAsync<NegocioException>();
           
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Removendo acentos")]
        public async Task Remover_acentos()
        {
            var palavrasAcentuadas = new List<string>
            {
                "Lamentável", "Caráter","Crédito", "Sétimo","Piauí","Raízes", "Herói","Estória","Esdrúxulo", "Teiús",
                "Câmara","Âmago","Pêsames","Mês","Pôr","Pôde",
                 "Não", "Anões", "Informação","Mamãe","Eleições", "Sótão", "À propaganda"
            };
            
            var palavrasNaoAcentuadas = new List<string>
            {
                "Credito", "Setimo", "Mes", "Nao", "Lamentavel", "Teius", "Piaui", "Heroi","Anoes", 
                "Camara","Raizes","Por","Pode","Carater","Amago","Pesames","Estoria","Esdruxulo", "Informacao","Mamae"
                ,"Eleicoes","Sotao", "A propaganda"
            };

            foreach (var palavrasAcentuada in palavrasAcentuadas)
            {
                var removidoAcentuacao = palavrasAcentuada.RemoverAcentuacao();
                palavrasNaoAcentuadas.Any(a=> a.SaoIguais(removidoAcentuacao)).ShouldBeTrue();
            }
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Tratar o retorno de dimensões")]
        public async Task Tratar_retorno_dimensoes()
        {
            var detalhe = new AcervoArteGraficaDetalhe() { Largura = "15,20", Altura = "18,20", Diametro = "23,56" };
            detalhe.Dimensoes.ShouldBe("15,20(Largura) x 18,20(Altura) x 23,56(Diâmetro)");
            
            detalhe = new AcervoArteGraficaDetalhe() { Largura = "15,20", Altura = "18,20" };
            detalhe.Dimensoes.ShouldBe("15,20(Largura) x 18,20(Altura)");
            
            detalhe = new AcervoArteGraficaDetalhe() { Largura = "15,20", Diametro = "23,56" };
            detalhe.Dimensoes.ShouldBe("15,20(Largura) x 23,56(Diâmetro)");
            
            detalhe = new AcervoArteGraficaDetalhe() { Altura = "18,20", Diametro = "23,56" };
            detalhe.Dimensoes.ShouldBe("18,20(Altura) x 23,56(Diâmetro)");
            
            detalhe = new AcervoArteGraficaDetalhe() { Largura = "15,20" };
            detalhe.Dimensoes.ShouldBe("15,20(Largura)");
            
            detalhe = new AcervoArteGraficaDetalhe() { Altura = "18,20" };
            detalhe.Dimensoes.ShouldBe("18,20(Altura)");
            
            detalhe = new AcervoArteGraficaDetalhe() { Diametro = "23,56" };
            detalhe.Dimensoes.ShouldBe("23,56(Diâmetro)");
            
            detalhe = new AcervoArteGraficaDetalhe();
            detalhe.Dimensoes.ShouldBe(string.Empty);
        }
        
        private async Task InserirAcervoArteGrafica(bool inserirCredor = true)
        {
            var random = new Random();

            var arquivoId = 1;

            for (int j = 1; j <= 35; j++)
            {
                await InserirNaBase(new Acervo()
                {
                    Codigo = $"{j.ToString()}.AG",
                    Titulo = faker.Lorem.Text().Limite(500),
                    Descricao = faker.Lorem.Text(),
                    TipoAcervoId = (int)TipoAcervo.ArtesGraficas,
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia().AddMinutes(-15),
                    CriadoLogin = ConstantesTestes.LOGIN_123456789,
                    DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                    Ano = DateTimeExtension.HorarioBrasilia().Year.ToString(),
                });

                if (inserirCredor)
                {
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

                await InserirNaBase(new AcervoArteGrafica()
                {
                    AcervoId = j,
                    Localizacao = faker.Lorem.Text().Limite(100),
                    Procedencia = faker.Lorem.Text().Limite(200),
                    CopiaDigital = true,
                    PermiteUsoImagem = true,
                    ConservacaoId = random.Next(1,5),
                    CromiaId = random.Next(1,5),
                    Largura = "10,20",
                    Altura = "30,15",
                    Diametro = "45,32",
                    Tecnica = faker.Lorem.Text().Limite(100),
                    SuporteId = random.Next(1,5),
                    Quantidade = random.Next(15,55),
                });
                
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
                
                await InserirNaBase(new AcervoArteGraficaArquivo()
                {
                    ArquivoId = arquivoId-1,
                    AcervoArteGraficaId = j,
                    ArquivoMiniaturaId = arquivoId
                });
                
                arquivoId++;
            }
        }
    }
}