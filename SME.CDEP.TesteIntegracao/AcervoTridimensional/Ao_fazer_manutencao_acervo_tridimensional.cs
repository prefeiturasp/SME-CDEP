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
    public class Ao_fazer_manutencao_acervo_tridimensional : TesteBase
    {
        public Ao_fazer_manutencao_acervo_tridimensional(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo Tridimensional - Obter por Id")]
        public async Task Obter_por_id()
        {
            await InserirDadosBasicos();
            await InserirAcervoTridimensional();
            var servicoAcervoTridimensional = GetServicoAcervoTridimensional();

            var acervoTridimensionalDto = await servicoAcervoTridimensional.ObterPorId(5);
            acervoTridimensionalDto.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo Tridimensional - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirDadosBasicos();
            await InserirAcervoTridimensional();
            var servicoAcervoTridimensional = GetServicoAcervoTridimensional();

            var acervoTridimensionalDtos = await servicoAcervoTridimensional.ObterTodos();
            acervoTridimensionalDtos.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo Tridimensional - Atualizar (Adicionando 4 novos arquivos, sendo 1 existente)")]
        public async Task Atualizar_com_4_novos()
        {
            await InserirDadosBasicos();

            await InserirAcervoTridimensional();
            
            var random = new Random();
            
            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();
            
            var servicoAcervoTridimensional = GetServicoAcervoTridimensional();

            var acervoTridimensionalAlteracaoDto = new AcervoTridimensionalAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.TD",
                Titulo = string.Format(ConstantesTestes.TITULO_X, 100),
                CreditosAutoresIds = new long[]{4,5},
                Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X, 100),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = string.Format(ConstantesTestes.DESCRICAO_X, 100),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Diametro = random.Next(15, 55),
                Profundidade = random.Next(15, 55),
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoTridimensional.Alterar(acervoTridimensionalAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoTridimensionalAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Codigo.Equals(acervoTridimensionalAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Tridimensional);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            
           var acervoTridimensional = ObterTodos<AcervoTridimensional>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoTridimensional.Procedencia.ShouldBe(acervoTridimensionalAlteracaoDto.Procedencia);
            acervoTridimensional.DataAcervo.ShouldBe(acervoTridimensionalAlteracaoDto.DataAcervo);
            acervoTridimensional.ConservacaoId.ShouldBe(acervoTridimensionalAlteracaoDto.ConservacaoId);
            acervoTridimensional.Quantidade.ShouldBe(acervoTridimensionalAlteracaoDto.Quantidade);
            acervoTridimensional.Descricao.ShouldBe(acervoTridimensionalAlteracaoDto.Descricao);
            acervoTridimensional.Largura.ShouldBe(acervoTridimensionalAlteracaoDto.Largura.Value);
            acervoTridimensional.Altura.ShouldBe(acervoTridimensionalAlteracaoDto.Altura.Value);
            acervoTridimensional.Profundidade.ShouldBe(acervoTridimensionalAlteracaoDto.Profundidade.Value);
            acervoTridimensional.Diametro.ShouldBe(acervoTridimensionalAlteracaoDto.Diametro.Value);
            
            var acervoTridimensionalArquivos = ObterTodos<AcervoTridimensionalArquivo>();
            var acervoTridimensionalArquivosInseridos = acervoTridimensionalArquivos.Where(w => w.AcervoTridimensionalId == acervoTridimensional.Id);
            acervoTridimensionalArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(4);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo Tridimensional - Atualizar (Removendo 1 arquivo, adicionando 5 novos)")]
        public async Task Atualizar_removendo_1_()
        {
            await InserirDadosBasicos();

            await InserirAcervoTridimensional();
            
            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.OrderByDescending(o=> o.Id).Take(5).Select(s => s.Id).ToArray();
            
            var random = new Random();
            
            var servicoAcervoTridimensional = GetServicoAcervoTridimensional();

            var acervoTridimensionalAlteracaoDto = new AcervoTridimensionalAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.TD",
                Titulo = string.Format(ConstantesTestes.TITULO_X, 100),
                CreditosAutoresIds = new long[]{2,3},
                Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X, 100),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = string.Format(ConstantesTestes.DESCRICAO_X, 100),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Profundidade = random.Next(15, 55),
                Diametro = random.Next(15, 55),
                Arquivos = arquivosSelecionados
            };
                
            await servicoAcervoTridimensional.Alterar(acervoTridimensionalAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoTridimensionalAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Codigo.Equals(acervoTridimensionalAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Tridimensional);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            
            var acervoTridimensional = ObterTodos<AcervoTridimensional>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoTridimensional.Procedencia.ShouldBe(acervoTridimensionalAlteracaoDto.Procedencia);
            acervoTridimensional.DataAcervo.ShouldBe(acervoTridimensionalAlteracaoDto.DataAcervo);
            acervoTridimensional.ConservacaoId.ShouldBe(acervoTridimensionalAlteracaoDto.ConservacaoId);
            acervoTridimensional.Quantidade.ShouldBe(acervoTridimensionalAlteracaoDto.Quantidade);
            acervoTridimensional.Descricao.ShouldBe(acervoTridimensionalAlteracaoDto.Descricao);
            acervoTridimensional.Largura.ShouldBe(acervoTridimensionalAlteracaoDto.Largura.Value);
            acervoTridimensional.Altura.ShouldBe(acervoTridimensionalAlteracaoDto.Altura.Value);
            acervoTridimensional.Profundidade.ShouldBe(acervoTridimensionalAlteracaoDto.Profundidade.Value);
            acervoTridimensional.Diametro.ShouldBe(acervoTridimensionalAlteracaoDto.Diametro.Value);
            
            var acervoTridimensionalArquivos = ObterTodos<AcervoTridimensionalArquivo>();
            var acervoTridimensionalArquivosInseridos = acervoTridimensionalArquivos.Where(w => w.AcervoTridimensionalId == acervoTridimensional.Id);
            acervoTridimensionalArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(2);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(3);
        }
        
        [Fact(DisplayName = "Acervo Tridimensional - Atualizar (Removendo todos)")]
        public async Task Atualizar_removendo_todos()
        {
            await InserirDadosBasicos();

            await InserirAcervoTridimensional();
            
            var random = new Random();
            
            var arquivosSelecionados = Array.Empty<long>();

            var servicoAcervoTridimensional = GetServicoAcervoTridimensional();

            var acervoTridimensionalAlteracaoDto = new AcervoTridimensionalAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.TD",
                Titulo = string.Format(ConstantesTestes.TITULO_X, 100),
                CreditosAutoresIds = new long[]{1,5},
                Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X, 100),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = string.Format(ConstantesTestes.DESCRICAO_X, 100),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Profundidade = random.Next(15, 55),
                Diametro = random.Next(15, 55),
                Arquivos = arquivosSelecionados
            };
                
            await servicoAcervoTridimensional.Alterar(acervoTridimensionalAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoTridimensionalAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Codigo.Equals(acervoTridimensionalAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Tridimensional);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            
            var acervoTridimensional = ObterTodos<AcervoTridimensional>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoTridimensional.Procedencia.ShouldBe(acervoTridimensionalAlteracaoDto.Procedencia);
            acervoTridimensional.DataAcervo.ShouldBe(acervoTridimensionalAlteracaoDto.DataAcervo);
            acervoTridimensional.ConservacaoId.ShouldBe(acervoTridimensionalAlteracaoDto.ConservacaoId);
            acervoTridimensional.Quantidade.ShouldBe(acervoTridimensionalAlteracaoDto.Quantidade);
            acervoTridimensional.Descricao.ShouldBe(acervoTridimensionalAlteracaoDto.Descricao);
            acervoTridimensional.Largura.ShouldBe(acervoTridimensionalAlteracaoDto.Largura.Value);
            acervoTridimensional.Altura.ShouldBe(acervoTridimensionalAlteracaoDto.Altura.Value);
            acervoTridimensional.Profundidade.ShouldBe(acervoTridimensionalAlteracaoDto.Profundidade.Value);
            acervoTridimensional.Diametro.ShouldBe(acervoTridimensionalAlteracaoDto.Diametro.Value);
            
            var acervoTridimensionalArquivos = ObterTodos<AcervoTridimensionalArquivo>();
            var acervoTridimensionalArquivosInseridos = acervoTridimensionalArquivos.Where(w => w.AcervoTridimensionalId == acervoTridimensional.Id);
            acervoTridimensionalArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == 3);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(1);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(5);
        }
        
        [Fact(DisplayName = "Acervo Tridimensional - Inserir")]
        public async Task Inserir()
        {
            await InserirDadosBasicos();

            await InserirAcervoTridimensional();

            var servicoAcervoTridimensional = GetServicoAcervoTridimensional();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();

            var acervoTridimensionalCadastroDto = new AcervoTridimensionalCadastroDTO()
            {
                Codigo = "100",
                Titulo = string.Format(ConstantesTestes.TITULO_X, 100),
                CreditosAutoresIds = new long[]{3,4},
                Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X, 100),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = string.Format(ConstantesTestes.DESCRICAO_X, 100),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Profundidade = random.Next(15, 55),
                Diametro = random.Next(15, 55),
                Arquivos = arquivosSelecionados
            };
            
            var acervoTridimensionalInserido = await servicoAcervoTridimensional.Inserir(acervoTridimensionalCadastroDto);
            acervoTridimensionalInserido.ShouldBeGreaterThan(1);

            var acervo = ObterTodos<Acervo>().LastOrDefault();
            acervo.Titulo.Equals(acervoTridimensionalCadastroDto.Titulo).ShouldBeTrue();
            acervo.Codigo.Equals($"{acervoTridimensionalCadastroDto.Codigo}.TD").ShouldBeTrue();
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Tridimensional);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeFalse();
            acervo.AlteradoPor.ShouldBeNull();
            
            var acervoTridimensional = ObterTodos<AcervoTridimensional>().LastOrDefault();
            acervoTridimensional.Procedencia.ShouldBe(acervoTridimensionalCadastroDto.Procedencia);
            acervoTridimensional.DataAcervo.ShouldBe(acervoTridimensionalCadastroDto.DataAcervo);
            acervoTridimensional.ConservacaoId.ShouldBe(acervoTridimensionalCadastroDto.ConservacaoId);
            acervoTridimensional.Quantidade.ShouldBe(acervoTridimensionalCadastroDto.Quantidade);
            acervoTridimensional.Descricao.ShouldBe(acervoTridimensionalCadastroDto.Descricao);
            acervoTridimensional.Largura.ShouldBe(acervoTridimensionalCadastroDto.Largura.Value);
            acervoTridimensional.Altura.ShouldBe(acervoTridimensionalCadastroDto.Altura.Value);
            acervoTridimensional.Profundidade.ShouldBe(acervoTridimensionalCadastroDto.Profundidade.Value);
            acervoTridimensional.Diametro.ShouldBe(acervoTridimensionalCadastroDto.Diametro.Value);
            
            var acervoTridimensionalArquivos = ObterTodos<AcervoTridimensionalArquivo>();
            var acervoTridimensionalArquivosInseridos = acervoTridimensionalArquivos.Where(w => w.AcervoTridimensionalId == acervoTridimensional.Id);
            acervoTridimensionalArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
            
            var acervoCreditoAutor = ObterTodos<AcervoCreditoAutor>().Where(w=> w.AcervoId == acervoTridimensional.AcervoId);
            acervoCreditoAutor.Count().ShouldBe(2);
            acervoCreditoAutor.FirstOrDefault().CreditoAutorId.ShouldBe(3);
            acervoCreditoAutor.LastOrDefault().CreditoAutorId.ShouldBe(4);
        }
        
        [Fact(DisplayName = "Acervo Tridimensional - Não deve inserir Tombo duplicado")]
        public async Task Nao_deve_inserir_duplicado()
        {
            await InserirDadosBasicos();

            await InserirAcervoTridimensional();

            var servicoAcervoTridimensional = GetServicoAcervoTridimensional();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();

            var acervoTridimensionalCadastroDto = new AcervoTridimensionalCadastroDTO()
            {
                Codigo = "1",
                Titulo = string.Format(ConstantesTestes.TITULO_X, 100),
                CreditosAutoresIds = new long[]{new Random().Next(1, 5),new Random().Next(1, 5)},
                Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X, 100),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = string.Format(ConstantesTestes.DESCRICAO_X, 100),
                Largura = random.Next(15, 55),
                Altura = random.Next(15, 55),
                Profundidade = random.Next(15, 55),
                Diametro = random.Next(15, 55),
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoTridimensional.Inserir(acervoTridimensionalCadastroDto).ShouldThrowAsync<NegocioException>();
           
        }

        private async Task InserirAcervoTridimensional()
        {
            var random = new Random();

            for (int j = 1; j <= 35; j++)
            {
                await InserirNaBase(new Acervo()
                {
                    Codigo = $"{j.ToString()}.TD",
                    Titulo = string.Format(ConstantesTestes.TITULO_X, j),
                    TipoAcervoId = (int)TipoAcervo.Tridimensional,
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

                await InserirNaBase(new AcervoTridimensional()
                {
                    AcervoId = j,
                    Procedencia = string.Format(ConstantesTestes.PROCEDENCIA_X,j),
                    DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                    ConservacaoId = random.Next(1,5),
                    Quantidade = random.Next(15,55),
                    Descricao = string.Format(ConstantesTestes.DESCRICAO_X,j),
                    Largura = random.Next(15,55),
                    Altura = random.Next(15,55),
                    Profundidade = random.Next(15,55),
                    Diametro = random.Next(15,55),
                });
                
                await InserirNaBase(new Arquivo()
                {
                    Nome = string.Format(ConstantesTestes.ARQUIVO_X,j),
                    Codigo = Guid.NewGuid(),
                    Tipo = TipoArquivo.AcervoTridimensional,
                    TipoConteudo = ConstantesTestes.MIME_TYPE_JPG,
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia().AddMinutes(-15),
                    CriadoLogin = ConstantesTestes.LOGIN_123456789,
                });
                
                await InserirNaBase(new AcervoTridimensionalArquivo()
                {
                    ArquivoId = j,
                    AcervoTridimensionalId = j
                });
            }
        }
    }
}