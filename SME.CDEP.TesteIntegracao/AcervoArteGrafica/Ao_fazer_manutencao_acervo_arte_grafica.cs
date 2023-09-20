using AutoMapper;
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
    public class Ao_fazer_manutencao_acervo_arte_grafica : TesteBase
    {
        public Ao_fazer_manutencao_acervo_arte_grafica(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Obter por Id")]
        public async Task Obter_por_id()
        {
            await InserirDadosBasicos();
            await InserirAcervoArteGrafica();
            var servicoAcervoArteGrafica = getServicoAcervoArteGrafica();

            var acervoArteGraficaDto = await servicoAcervoArteGrafica.ObterPorId(5);
            acervoArteGraficaDto.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirDadosBasicos();
            await InserirAcervoArteGrafica();
            var servicoAcervoArteGrafica = getServicoAcervoArteGrafica();

            var acervoArteGraficaDtos = await servicoAcervoArteGrafica.ObterTodos();
            acervoArteGraficaDtos.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Atualizar (Adicionando 4 novos arquivos, sendo 1 existente)")]
        public async Task Atualizar_com_4_novos()
        {
            await InserirDadosBasicos();

            await InserirAcervoArteGrafica();
            
            var random = new Random();
            
            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var servicoAcervoArteGrafica = getServicoAcervoArteGrafica();

            var acervoArteGraficaAlteracaoDto = new AcervoArteGraficaAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100AG",
                Titulo = string.Format(ConstantesTestes.TITULO_X, 100),
                CreditosAutoresIds = new long[]{4,5},
                Localizacao = string.Format(ConstantesTestes.LOCALIZACAO_X, 100),
                Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X, 100),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Diametro = random.Next(15, 55),
                Tecnica = string.Format(ConstantesTestes.TECNICA_X, 100),
                SuporteId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = string.Format(ConstantesTestes.DESCRICAO_X, 100),
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoArteGrafica.Alterar(acervoArteGraficaAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoArteGraficaAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Codigo.Equals(acervoArteGraficaAlteracaoDto.Codigo).ShouldBeTrue();
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
            acervoArteGrafica.DataAcervo.ShouldBe(acervoArteGraficaAlteracaoDto.DataAcervo);
            acervoArteGrafica.CopiaDigital.ShouldBe(acervoArteGraficaAlteracaoDto.CopiaDigital);
            acervoArteGrafica.PermiteUsoImagem.ShouldBe(acervoArteGraficaAlteracaoDto.PermiteUsoImagem);
            acervoArteGrafica.ConservacaoId.ShouldBe(acervoArteGraficaAlteracaoDto.ConservacaoId);
            acervoArteGrafica.CromiaId.ShouldBe(acervoArteGraficaAlteracaoDto.CromiaId);
            acervoArteGrafica.Largura.ShouldBe(acervoArteGraficaAlteracaoDto.Largura.Value);
            acervoArteGrafica.Altura.ShouldBe(acervoArteGraficaAlteracaoDto.Altura.Value);
            acervoArteGrafica.Diametro.ShouldBe(acervoArteGraficaAlteracaoDto.Diametro.Value);
            acervoArteGrafica.Tecnica.ShouldBe(acervoArteGraficaAlteracaoDto.Tecnica);
            acervoArteGrafica.SuporteId.ShouldBe(acervoArteGraficaAlteracaoDto.SuporteId);
            acervoArteGrafica.Quantidade.ShouldBe(acervoArteGraficaAlteracaoDto.Quantidade);
            acervoArteGrafica.Descricao.ShouldBe(acervoArteGraficaAlteracaoDto.Descricao);
            
            var acervoArteGraficaArquivos = ObterTodos<AcervoArteGraficaArquivo>();
            var acervoFotograficoArquivosInseridos = acervoArteGraficaArquivos.Where(w => w.AcervoArteGraficaId == acervoArteGrafica.Id);
            acervoFotograficoArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(4);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Atualizar (Removendo 1 arquivo, adicionando 5 novos)")]
        public async Task Atualizar_removendo_1_()
        {
            await InserirDadosBasicos();

            await InserirAcervoArteGrafica();
            
            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.OrderByDescending(o=> o.Id).Take(5).Select(s => s.Id).ToArray();
            
            var random = new Random();
            
            var servicoAcervoArteGrafica = getServicoAcervoArteGrafica();

            var acervoArteGraficaAlteracaoDto = new AcervoArteGraficaAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100AG",
                Titulo = string.Format(ConstantesTestes.TITULO_X, 100),
                CreditosAutoresIds = new long[]{2,3},
                Localizacao = string.Format(ConstantesTestes.LOCALIZACAO_X, 100),
                Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X, 100),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Diametro = random.Next(15, 55),
                Tecnica = string.Format(ConstantesTestes.TECNICA_X, 100),
                SuporteId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = string.Format(ConstantesTestes.DESCRICAO_X, 100),
                Arquivos = arquivosSelecionados
            };
                
            await servicoAcervoArteGrafica.Alterar(acervoArteGraficaAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoArteGraficaAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Codigo.Equals(acervoArteGraficaAlteracaoDto.Codigo).ShouldBeTrue();
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
            acervoArteGrafica.DataAcervo.ShouldBe(acervoArteGraficaAlteracaoDto.DataAcervo);
            acervoArteGrafica.CopiaDigital.ShouldBe(acervoArteGraficaAlteracaoDto.CopiaDigital);
            acervoArteGrafica.PermiteUsoImagem.ShouldBe(acervoArteGraficaAlteracaoDto.PermiteUsoImagem);
            acervoArteGrafica.ConservacaoId.ShouldBe(acervoArteGraficaAlteracaoDto.ConservacaoId);
            acervoArteGrafica.CromiaId.ShouldBe(acervoArteGraficaAlteracaoDto.CromiaId);
            acervoArteGrafica.Largura.ShouldBe(acervoArteGraficaAlteracaoDto.Largura.Value);
            acervoArteGrafica.Altura.ShouldBe(acervoArteGraficaAlteracaoDto.Altura.Value);
            acervoArteGrafica.Diametro.ShouldBe(acervoArteGraficaAlteracaoDto.Diametro.Value);
            acervoArteGrafica.Tecnica.ShouldBe(acervoArteGraficaAlteracaoDto.Tecnica);
            acervoArteGrafica.SuporteId.ShouldBe(acervoArteGraficaAlteracaoDto.SuporteId);
            acervoArteGrafica.Quantidade.ShouldBe(acervoArteGraficaAlteracaoDto.Quantidade);
            acervoArteGrafica.Descricao.ShouldBe(acervoArteGraficaAlteracaoDto.Descricao);
            
            var acervoArteGraficaArquivos = ObterTodos<AcervoArteGraficaArquivo>();
            var acervoFotograficoArquivosInseridos = acervoArteGraficaArquivos.Where(w => w.AcervoArteGraficaId == acervoArteGrafica.Id);
            acervoFotograficoArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(2);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(3);
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Atualizar (Removendo todos)")]
        public async Task Atualizar_removendo_todos()
        {
            await InserirDadosBasicos();

            await InserirAcervoArteGrafica();
            
            var random = new Random();
            
            var arquivosSelecionados = Array.Empty<long>();

            var servicoAcervoArteGrafica = getServicoAcervoArteGrafica();

            var acervoArteGraficaAlteracaoDto = new AcervoArteGraficaAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100AG",
                Titulo = string.Format(ConstantesTestes.TITULO_X, 100),
                CreditosAutoresIds = new long[]{1,5},
                Localizacao = string.Format(ConstantesTestes.LOCALIZACAO_X, 100),
                Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X, 100),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Diametro = random.Next(15, 55),
                Tecnica = string.Format(ConstantesTestes.TECNICA_X, 100),
                SuporteId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = string.Format(ConstantesTestes.DESCRICAO_X, 100),
                Arquivos = arquivosSelecionados
            };
                
            await servicoAcervoArteGrafica.Alterar(acervoArteGraficaAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoArteGraficaAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Codigo.Equals(acervoArteGraficaAlteracaoDto.Codigo).ShouldBeTrue();
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
            acervoArteGrafica.DataAcervo.ShouldBe(acervoArteGraficaAlteracaoDto.DataAcervo);
            acervoArteGrafica.CopiaDigital.ShouldBe(acervoArteGraficaAlteracaoDto.CopiaDigital);
            acervoArteGrafica.PermiteUsoImagem.ShouldBe(acervoArteGraficaAlteracaoDto.PermiteUsoImagem);
            acervoArteGrafica.ConservacaoId.ShouldBe(acervoArteGraficaAlteracaoDto.ConservacaoId);
            acervoArteGrafica.CromiaId.ShouldBe(acervoArteGraficaAlteracaoDto.CromiaId);
            acervoArteGrafica.Largura.ShouldBe(acervoArteGraficaAlteracaoDto.Largura.Value);
            acervoArteGrafica.Altura.ShouldBe(acervoArteGraficaAlteracaoDto.Altura.Value);
            acervoArteGrafica.Diametro.ShouldBe(acervoArteGraficaAlteracaoDto.Diametro.Value);
            acervoArteGrafica.Tecnica.ShouldBe(acervoArteGraficaAlteracaoDto.Tecnica);
            acervoArteGrafica.SuporteId.ShouldBe(acervoArteGraficaAlteracaoDto.SuporteId);
            acervoArteGrafica.Quantidade.ShouldBe(acervoArteGraficaAlteracaoDto.Quantidade);
            acervoArteGrafica.Descricao.ShouldBe(acervoArteGraficaAlteracaoDto.Descricao);
            
            var acervoArteGraficaArquivos = ObterTodos<AcervoArteGraficaArquivo>();
            var acervoFotograficoArquivosInseridos = acervoArteGraficaArquivos.Where(w => w.AcervoArteGraficaId == acervoArteGrafica.Id);
            acervoFotograficoArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(1);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Inserir")]
        public async Task Inserir()
        {
            await InserirDadosBasicos();

            await InserirAcervoArteGrafica();

            var servicoAcervoArteGrafica = getServicoAcervoArteGrafica();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();

            var acervoArteGraficaCadastroDto = new AcervoArteGraficaCadastroDTO()
            {
                Codigo = "100",
                Titulo = string.Format(ConstantesTestes.TITULO_X, 100),
                CreditosAutoresIds = new long[]{3,4},
                Localizacao = string.Format(ConstantesTestes.LOCALIZACAO_X, 100),
                Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X, 100),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Diametro = random.Next(15, 55),
                Tecnica = string.Format(ConstantesTestes.TECNICA_X, 100),
                SuporteId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = string.Format(ConstantesTestes.DESCRICAO_X, 100),
                Arquivos = arquivosSelecionados
            };
            
            var acervoArteGraficaInserido = await servicoAcervoArteGrafica.Inserir(acervoArteGraficaCadastroDto);
            acervoArteGraficaInserido.ShouldBeGreaterThan(1);

            var acervo = ObterTodos<Acervo>().LastOrDefault();
            acervo.Titulo.Equals(acervoArteGraficaCadastroDto.Titulo).ShouldBeTrue();
            acervo.Codigo.Equals($"{acervoArteGraficaCadastroDto.Codigo}AG").ShouldBeTrue();
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
            acervoArteGrafica.DataAcervo.ShouldBe(acervoArteGraficaCadastroDto.DataAcervo);
            acervoArteGrafica.CopiaDigital.ShouldBe(acervoArteGraficaCadastroDto.CopiaDigital);
            acervoArteGrafica.PermiteUsoImagem.ShouldBe(acervoArteGraficaCadastroDto.PermiteUsoImagem);
            acervoArteGrafica.ConservacaoId.ShouldBe(acervoArteGraficaCadastroDto.ConservacaoId);
            acervoArteGrafica.CromiaId.ShouldBe(acervoArteGraficaCadastroDto.CromiaId);
            acervoArteGrafica.Largura.ShouldBe(acervoArteGraficaCadastroDto.Largura.Value);
            acervoArteGrafica.Altura.ShouldBe(acervoArteGraficaCadastroDto.Altura.Value);
            acervoArteGrafica.Diametro.ShouldBe(acervoArteGraficaCadastroDto.Diametro.Value);
            acervoArteGrafica.Tecnica.ShouldBe(acervoArteGraficaCadastroDto.Tecnica);
            acervoArteGrafica.SuporteId.ShouldBe(acervoArteGraficaCadastroDto.SuporteId);
            acervoArteGrafica.Quantidade.ShouldBe(acervoArteGraficaCadastroDto.Quantidade);
            acervoArteGrafica.Descricao.ShouldBe(acervoArteGraficaCadastroDto.Descricao);
            
            var acervoArteGraficaArquivos = ObterTodos<AcervoArteGraficaArquivo>();
            var acervoArteGraficaArquivosInseridos = acervoArteGraficaArquivos.Where(w => w.AcervoArteGraficaId == acervoArteGrafica.Id);
            acervoArteGraficaArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == acervoArteGrafica.AcervoId);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(3);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(4);
        }
        
        [Fact(DisplayName = "Acervo Arte Gráfica - Não deve inserir Tombo duplicado")]
        public async Task Nao_deve_inserir_duplicado()
        {
            await InserirDadosBasicos();

            await InserirAcervoArteGrafica();

            var servicoAcervoArteGrafica = getServicoAcervoArteGrafica();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();

            var acervoArteGraficaCadastroDto = new AcervoArteGraficaCadastroDTO()
            {
                Codigo = "1",
                Titulo = string.Format(ConstantesTestes.TITULO_X, 100),
                CreditosAutoresIds = new long[]{new Random().Next(1, 5),new Random().Next(1, 5)},
                Localizacao = string.Format(ConstantesTestes.LOCALIZACAO_X, 100),
                Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X, 100),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Diametro = random.Next(15, 55),
                Tecnica = string.Format(ConstantesTestes.TECNICA_X, 100),
                SuporteId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = string.Format(ConstantesTestes.DESCRICAO_X, 100),
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoArteGrafica.Inserir(acervoArteGraficaCadastroDto).ShouldThrowAsync<NegocioException>();
           
        }

        private async Task InserirAcervoArteGrafica()
        {
            var random = new Random();

            for (int j = 1; j <= 35; j++)
            {
                await InserirNaBase(new Acervo()
                {
                    Codigo = $"{j.ToString()}AG",
                    Titulo = string.Format(ConstantesTestes.TITULO_X, j),
                    TipoAcervoId = (int)TipoAcervo.ArtesGraficas,
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

                await InserirNaBase(new AcervoArteGrafica()
                {
                    AcervoId = j,
                    Localizacao = string.Format(ConstantesTestes.LOCALIZACAO_X, j),
                    Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X,j),
                    DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                    CopiaDigital = true,
                    PermiteUsoImagem = true,
                    ConservacaoId = random.Next(1,5),
                    CromiaId = random.Next(1,5),
                    Largura = random.Next(15,55),
                    Altura = random.Next(15,55),
                    Diametro = random.Next(15,55),
                    Tecnica = string.Format(ConstantesTestes.TECNICA_X,j),
                    SuporteId = random.Next(1,5),
                    Quantidade = random.Next(15,55),
                    Descricao = string.Format(ConstantesTestes.DESCRICAO_X,j)
                });
                
                await InserirNaBase(new Arquivo()
                {
                    Nome = string.Format(ConstantesTestes.ARQUIVO_X,j),
                    Codigo = Guid.NewGuid(),
                    Tipo = TipoArquivo.AcervoFotografico,
                    TipoConteudo = ConstantesTestes.MIME_TYPE_JPG,
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia().AddMinutes(-15),
                    CriadoLogin = ConstantesTestes.LOGIN_123456789,
                });
                
                await InserirNaBase(new AcervoArteGraficaArquivo()
                {
                    ArquivoId = j,
                    AcervoArteGraficaId = j
                });
            }
        }
    }
}