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
    public class Ao_fazer_manutencao_acervo_fotografico : TesteBase
    {
        public Ao_fazer_manutencao_acervo_fotografico(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo fotográfico - Obter por Id")]
        public async Task Obter_por_id()
        {
            await InserirDadosBasicos();
            await InserirAcervoFotografico();
            var servicoAcervoFotografico = GetServicoAcervoFotografico();

            var acervoFotograficoDtos = await servicoAcervoFotografico.ObterPorId(5);
            acervoFotograficoDtos.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirDadosBasicos();
            await InserirAcervoFotografico();
            var servicoAcervoFotografico = GetServicoAcervoFotografico();

            var acervoFotograficoDtos = await servicoAcervoFotografico.ObterTodos();
            acervoFotograficoDtos.ShouldNotBeNull();
            // acervoFotograficoDtos.FirstOrDefault().CreditosAutoresIds.Any().ShouldBeTrue(); //Agrupar conforme ServicoAcervoAuditavel
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Atualizar (Adicionando 4 novos arquivos, sendo 1 existente)")]
        public async Task Atualizar_com_4_novos()
        {
            await InserirDadosBasicos();

            await InserirAcervoFotografico();
            
            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var servicoAcervoFotografico = GetServicoAcervoFotografico();

            var acervoFotograficoAlteracaoDto = new AcervoFotograficoAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.FT",
                Titulo = string.Format(ConstantesTestes.TITULO_X, 100),
                CreditosAutoresIds = new long[]{1,2,3,4,5},
                Descricao = string.Format(ConstantesTestes.DESCRICAO_X, 100),
                Localizacao = string.Format(ConstantesTestes.LOCALIZACAO_X, 100),
                Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X, 100),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = 1,
                Quantidade = 25,
                Largura = 35,
                Altura = 45,
                SuporteId = 2,
                FormatoId = 3,
                CromiaId = 4,
                Resolucao = string.Format(ConstantesTestes.RESOLUCAO_X, 100),
                TamanhoArquivo = string.Format(ConstantesTestes.TAMANHO_ARQUIVO_X_MB, 100),
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoFotografico.Alterar(acervoFotograficoAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoFotograficoAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Codigo.Equals(acervoFotograficoAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Fotografico);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            
            var acervoFotografico = ObterTodos<AcervoFotografico>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoFotografico.Descricao.ShouldBe(acervoFotograficoAlteracaoDto.Descricao);
            acervoFotografico.Localizacao.ShouldBe(acervoFotograficoAlteracaoDto.Localizacao);
            acervoFotografico.Procedencia.ShouldBe(acervoFotograficoAlteracaoDto.Procedencia);
            acervoFotografico.DataAcervo.ShouldBe(acervoFotograficoAlteracaoDto.DataAcervo);
            acervoFotografico.CopiaDigital.ShouldBe(acervoFotograficoAlteracaoDto.CopiaDigital);
            acervoFotografico.PermiteUsoImagem.ShouldBe(acervoFotograficoAlteracaoDto.PermiteUsoImagem);
            acervoFotografico.ConservacaoId.ShouldBe(acervoFotograficoAlteracaoDto.ConservacaoId);
            acervoFotografico.Quantidade.ShouldBe(acervoFotograficoAlteracaoDto.Quantidade);
            acervoFotografico.Largura.ShouldBe(acervoFotograficoAlteracaoDto.Largura.Value);
            acervoFotografico.Altura.ShouldBe(acervoFotograficoAlteracaoDto.Altura.Value);
            acervoFotografico.SuporteId.ShouldBe(acervoFotograficoAlteracaoDto.SuporteId);
            acervoFotografico.FormatoId.ShouldBe(acervoFotograficoAlteracaoDto.FormatoId);
            acervoFotografico.CromiaId.ShouldBe(acervoFotograficoAlteracaoDto.CromiaId);
            acervoFotografico.Resolucao.ShouldBe(acervoFotograficoAlteracaoDto.Resolucao);
            acervoFotografico.TamanhoArquivo.ShouldBe(acervoFotograficoAlteracaoDto.TamanhoArquivo);
            
            var acervoFotograficoArquivos = ObterTodos<AcervoFotograficoArquivo>();
            var acervoFotograficoArquivosInseridos = acervoFotograficoArquivos.Where(w => w.AcervoFotograficoId == acervoFotografico.Id);
            acervoFotograficoArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Atualizar (Removendo 1 arquivo, adicionando 5 novos)")]
        public async Task Atualizar_removendo_1_()
        {
            await InserirDadosBasicos();

            await InserirAcervoFotografico();
            
            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.OrderByDescending(o=> o.Id).Take(5).Select(s => s.Id).ToArray();
            
            var servicoAcervoFotografico = GetServicoAcervoFotografico();

            var acervoFotograficoAlteracaoDto = new AcervoFotograficoAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.FT",
                Titulo = string.Format(ConstantesTestes.TITULO_X, 100),
                CreditosAutoresIds = new long[]{3,4,5},
                Descricao = string.Format(ConstantesTestes.DESCRICAO_X, 100),
                Localizacao = string.Format(ConstantesTestes.LOCALIZACAO_X, 100),
                Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X, 100),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = 1,
                Quantidade = 25,
                Largura = 35,
                Altura = 45,
                SuporteId = 2,
                FormatoId = 3,
                CromiaId = 4,
                Resolucao = string.Format(ConstantesTestes.RESOLUCAO_X, 100),
                TamanhoArquivo = string.Format(ConstantesTestes.TAMANHO_ARQUIVO_X_MB, 100),
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoFotografico.Alterar(acervoFotograficoAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoFotograficoAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Codigo.Equals(acervoFotograficoAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Fotografico);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            
            var acervoFotografico = ObterTodos<AcervoFotografico>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoFotografico.Descricao.ShouldBe(acervoFotograficoAlteracaoDto.Descricao);
            acervoFotografico.Localizacao.ShouldBe(acervoFotograficoAlteracaoDto.Localizacao);
            acervoFotografico.Procedencia.ShouldBe(acervoFotograficoAlteracaoDto.Procedencia);
            acervoFotografico.DataAcervo.ShouldBe(acervoFotograficoAlteracaoDto.DataAcervo);
            acervoFotografico.CopiaDigital.ShouldBe(acervoFotograficoAlteracaoDto.CopiaDigital);
            acervoFotografico.PermiteUsoImagem.ShouldBe(acervoFotograficoAlteracaoDto.PermiteUsoImagem);
            acervoFotografico.ConservacaoId.ShouldBe(acervoFotograficoAlteracaoDto.ConservacaoId);
            acervoFotografico.Quantidade.ShouldBe(acervoFotograficoAlteracaoDto.Quantidade);
            acervoFotografico.Largura.ShouldBe(acervoFotograficoAlteracaoDto.Largura.Value);
            acervoFotografico.Altura.ShouldBe(acervoFotograficoAlteracaoDto.Altura.Value);
            acervoFotografico.SuporteId.ShouldBe(acervoFotograficoAlteracaoDto.SuporteId);
            acervoFotografico.FormatoId.ShouldBe(acervoFotograficoAlteracaoDto.FormatoId);
            acervoFotografico.CromiaId.ShouldBe(acervoFotograficoAlteracaoDto.CromiaId);
            acervoFotografico.Resolucao.ShouldBe(acervoFotograficoAlteracaoDto.Resolucao);
            acervoFotografico.TamanhoArquivo.ShouldBe(acervoFotograficoAlteracaoDto.TamanhoArquivo);
            
            var acervoFotograficoArquivos = ObterTodos<AcervoFotograficoArquivo>();
            var acervoFotograficoArquivosInseridos = acervoFotograficoArquivos.Where(w => w.AcervoFotograficoId == acervoFotografico.Id);
            acervoFotograficoArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(3);
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Atualizar (Removendo todos)")]
        public async Task Atualizar_removendo_todos()
        {
            await InserirDadosBasicos();

            await InserirAcervoFotografico();
            
            var arquivosSelecionados = Array.Empty<long>();
            
            var servicoAcervoFotografico = GetServicoAcervoFotografico();

            var acervoFotograficoAlteracaoDto = new AcervoFotograficoAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.FT",
                Titulo = string.Format(ConstantesTestes.TITULO_X, 100),
                CreditosAutoresIds = new long[]{4,5},
                Descricao = string.Format(ConstantesTestes.DESCRICAO_X, 100),
                Localizacao = string.Format(ConstantesTestes.LOCALIZACAO_X, 100),
                Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X, 100),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = 1,
                Quantidade = 25,
                Largura = 35,
                Altura = 45,
                SuporteId = 2,
                FormatoId = 3,
                CromiaId = 4,
                Resolucao = string.Format(ConstantesTestes.RESOLUCAO_X, 100),
                TamanhoArquivo = string.Format(ConstantesTestes.TAMANHO_ARQUIVO_X_MB, 100),
                Arquivos = arquivosSelecionados
            };
                
            await servicoAcervoFotografico.Alterar(acervoFotograficoAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoFotograficoAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Codigo.Equals(acervoFotograficoAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Fotografico);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            
            var acervoFotografico = ObterTodos<AcervoFotografico>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoFotografico.Descricao.ShouldBe(acervoFotograficoAlteracaoDto.Descricao);
            acervoFotografico.Localizacao.ShouldBe(acervoFotograficoAlteracaoDto.Localizacao);
            acervoFotografico.Procedencia.ShouldBe(acervoFotograficoAlteracaoDto.Procedencia);
            acervoFotografico.DataAcervo.ShouldBe(acervoFotograficoAlteracaoDto.DataAcervo);
            acervoFotografico.CopiaDigital.ShouldBe(acervoFotograficoAlteracaoDto.CopiaDigital);
            acervoFotografico.PermiteUsoImagem.ShouldBe(acervoFotograficoAlteracaoDto.PermiteUsoImagem);
            acervoFotografico.ConservacaoId.ShouldBe(acervoFotograficoAlteracaoDto.ConservacaoId);
            acervoFotografico.Quantidade.ShouldBe(acervoFotograficoAlteracaoDto.Quantidade);
            acervoFotografico.Largura.ShouldBe(acervoFotograficoAlteracaoDto.Largura.Value);
            acervoFotografico.Altura.ShouldBe(acervoFotograficoAlteracaoDto.Altura.Value);
            acervoFotografico.SuporteId.ShouldBe(acervoFotograficoAlteracaoDto.SuporteId);
            acervoFotografico.FormatoId.ShouldBe(acervoFotograficoAlteracaoDto.FormatoId);
            acervoFotografico.CromiaId.ShouldBe(acervoFotograficoAlteracaoDto.CromiaId);
            acervoFotografico.Resolucao.ShouldBe(acervoFotograficoAlteracaoDto.Resolucao);
            acervoFotografico.TamanhoArquivo.ShouldBe(acervoFotograficoAlteracaoDto.TamanhoArquivo);
            
            var acervoFotograficoArquivos = ObterTodos<AcervoFotograficoArquivo>();
            var acervoFotograficoArquivosInseridos = acervoFotograficoArquivos.Where(w => w.AcervoFotograficoId == acervoFotografico.Id);
            acervoFotograficoArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(4);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Inserir")]
        public async Task Inserir()
        {
            await InserirDadosBasicos();

            await InserirAcervoFotografico();
            
            var servicoAcervoFotografico = GetServicoAcervoFotografico();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();

            var acervoFotograficoDto = new AcervoFotograficoCadastroDTO()
            {
                Codigo = "100",
                Titulo = string.Format(ConstantesTestes.TITULO_X, 100),
                CreditosAutoresIds = new long[]{new Random().Next(1, 5),new Random().Next(1, 5)},
                Descricao = string.Format(ConstantesTestes.DESCRICAO_X, 100),
                Localizacao = string.Format(ConstantesTestes.LOCALIZACAO_X, 100),
                Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X, 100),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                SuporteId = random.Next(1, 5),
                FormatoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Resolucao = string.Format(ConstantesTestes.RESOLUCAO_X, 100),
                TamanhoArquivo = string.Format(ConstantesTestes.TAMANHO_ARQUIVO_X_MB, 100),
                Arquivos = arquivosSelecionados
            };
            
            var acervoFotograficoInserido = await servicoAcervoFotografico.Inserir(acervoFotograficoDto);
            acervoFotograficoInserido.ShouldBeGreaterThan(1);

            var acervo = ObterTodos<Acervo>().LastOrDefault();
            acervo.Titulo.Equals(acervoFotograficoDto.Titulo).ShouldBeTrue();
            acervo.Codigo.Equals($"{acervoFotograficoDto.Codigo}.FT").ShouldBeTrue();
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Fotografico);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeFalse();
            acervo.AlteradoPor.ShouldBeNull();
            
            var acervoFotografico = ObterTodos<AcervoFotografico>().LastOrDefault();
            acervoFotografico.Descricao.ShouldBe(acervoFotograficoDto.Descricao);
            acervoFotografico.Localizacao.ShouldBe(acervoFotograficoDto.Localizacao);
            acervoFotografico.Procedencia.ShouldBe(acervoFotograficoDto.Procedencia);
            acervoFotografico.DataAcervo.ShouldBe(acervoFotograficoDto.DataAcervo);
            acervoFotografico.CopiaDigital.ShouldBe(acervoFotograficoDto.CopiaDigital);
            acervoFotografico.PermiteUsoImagem.ShouldBe(acervoFotograficoDto.PermiteUsoImagem);
            acervoFotografico.ConservacaoId.ShouldBe(acervoFotograficoDto.ConservacaoId);
            acervoFotografico.Quantidade.ShouldBe(acervoFotograficoDto.Quantidade);
            acervoFotografico.Largura.ShouldBe(acervoFotograficoDto.Largura.Value);
            acervoFotografico.Altura.ShouldBe(acervoFotograficoDto.Altura.Value);
            acervoFotografico.SuporteId.ShouldBe(acervoFotograficoDto.SuporteId);
            acervoFotografico.FormatoId.ShouldBe(acervoFotograficoDto.FormatoId);
            acervoFotografico.CromiaId.ShouldBe(acervoFotograficoDto.CromiaId);
            acervoFotografico.Resolucao.ShouldBe(acervoFotograficoDto.Resolucao);
            acervoFotografico.TamanhoArquivo.ShouldBe(acervoFotograficoDto.TamanhoArquivo);
            
            var acervoFotograficoArquivos = ObterTodos<AcervoFotograficoArquivo>();
            var acervoFotograficoArquivosInseridos = acervoFotograficoArquivos.Where(w => w.AcervoFotograficoId == acervoFotografico.Id);
            acervoFotograficoArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
        }
        
        [Fact(DisplayName = "Acervo fotográfico - Não deve inserir Tombo duplicado")]
        public async Task Nao_deve_inserir_duplicado()
        {
            await InserirDadosBasicos();

            await InserirAcervoFotografico();

            var servicoAcervoFotografico = GetServicoAcervoFotografico();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();

            var acervoFotograficoDto = new AcervoFotograficoCadastroDTO()
            {
                Codigo = "1",
                Titulo = string.Format(ConstantesTestes.TITULO_X, 100),
                CreditosAutoresIds = new long[]{new Random().Next(1, 5),new Random().Next(1, 5)},
                Descricao = string.Format(ConstantesTestes.DESCRICAO_X, 100),
                Localizacao = string.Format(ConstantesTestes.LOCALIZACAO_X, 100),
                Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X, 100),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                SuporteId = random.Next(1, 5),
                FormatoId = random.Next(1, 5),
                CromiaId = random.Next(1, 5),
                Resolucao = string.Format(ConstantesTestes.RESOLUCAO_X, 100),
                TamanhoArquivo = string.Format(ConstantesTestes.TAMANHO_ARQUIVO_X_MB, 100),
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoFotografico.Inserir(acervoFotograficoDto).ShouldThrowAsync<NegocioException>();
           
        }

        private async Task InserirAcervoFotografico()
        {
            var random = new Random();

            for (int j = 1; j <= 35; j++)
            {
                await InserirNaBase(new Acervo()
                {
                    Codigo = $"{j.ToString()}.FT",
                    Titulo = string.Format(ConstantesTestes.TITULO_X, j),
                    CreditosAutoresIds = new long[]{new Random().Next(1, 5),new Random().Next(1, 5)},
                    TipoAcervoId = (int)TipoAcervo.Fotografico,
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia().AddMinutes(-15),
                    CriadoLogin = ConstantesTestes.LOGIN_123456789,
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    AlteradoPor = ConstantesTestes.SISTEMA,
                    AlteradoLogin = ConstantesTestes.LOGIN_123456789
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
                    Localizacao = string.Format(ConstantesTestes.LOCALIZACAO_X, j),
                    Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X,j),
                    DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                    CopiaDigital = true,
                    PermiteUsoImagem = true,
                    ConservacaoId = random.Next(1,5),
                    Descricao = string.Format(ConstantesTestes.DESCRICAO_X,j),
                    Quantidade = random.Next(15,55),
                    Largura = random.Next(15,55),
                    Altura = random.Next(15,55),
                    SuporteId = random.Next(1,5),
                    FormatoId = random.Next(1,5),
                    CromiaId = random.Next(1,5),
                    Resolucao = string.Format(ConstantesTestes.RESOLUCAO_X,j),
                    TamanhoArquivo = string.Format(ConstantesTestes.TAMANHO_ARQUIVO_X_MB,j),
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
                    AlteradoEm = DateTimeExtension.HorarioBrasilia(),
                    AlteradoPor = ConstantesTestes.SISTEMA,
                    AlteradoLogin = ConstantesTestes.LOGIN_123456789
                });
                
                await InserirNaBase(new AcervoFotograficoArquivo()
                {
                    ArquivoId = j,
                    AcervoFotograficoId = j
                });
            }
        }
    }
}