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
    public class Ao_fazer_manutencao_acervo_tridimensional : TesteBase
    {
        public Ao_fazer_manutencao_acervo_tridimensional(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo Tridimensional - Obter por Id")]
        public async Task Obter_por_id()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervoTridimensional();
            var servicoAcervoTridimensional = GetServicoAcervoTridimensional();

            var acervoTridimensionalDto = await servicoAcervoTridimensional.ObterPorId(5);
            acervoTridimensionalDto.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo Tridimensional - Obter todos")]
        public async Task Obter_todos()
        {
            await InserirDadosBasicosAleatorios();
            await InserirAcervoTridimensional();
            var servicoAcervoTridimensional = GetServicoAcervoTridimensional();

            var acervoTridimensionalDtos = await servicoAcervoTridimensional.ObterTodos();
            acervoTridimensionalDtos.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo Tridimensional - Atualizar (Adicionando 4 novos arquivos, sendo 1 existente)")]
        public async Task Atualizar_com_4_novos()
        {
            await InserirDadosBasicosAleatorios();

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
                Titulo = faker.Lorem.Text().Limite(500),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = faker.Date.Past().Year,
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Largura = double.Parse("50,45"),
                Altura = double.Parse("10.20"),
                Diametro = double.Parse("1540"),	
                Profundidade = double.Parse("1801"),	
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoTridimensional.Alterar(acervoTridimensionalAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoTridimensionalAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoTridimensionalAlteracaoDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals(acervoTridimensionalAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.DataAcervo.ShouldBe(acervoTridimensionalAlteracaoDto.DataAcervo);
            acervo.Ano.ShouldBe(acervoTridimensionalAlteracaoDto.Ano);
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Tridimensional);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            
           var acervoTridimensional = ObterTodos<AcervoTridimensional>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoTridimensional.Procedencia.ShouldBe(acervoTridimensionalAlteracaoDto.Procedencia);
            acervoTridimensional.ConservacaoId.ShouldBe(acervoTridimensionalAlteracaoDto.ConservacaoId);
            acervoTridimensional.Quantidade.ShouldBe(acervoTridimensionalAlteracaoDto.Quantidade);
            acervoTridimensional.Largura.ShouldBe(acervoTridimensionalAlteracaoDto.Largura.Value);
            acervoTridimensional.Altura.ShouldBe(acervoTridimensionalAlteracaoDto.Altura.FormatarParaDoubleComCasasDecimais());
            acervoTridimensional.Profundidade.ShouldBe(acervoTridimensionalAlteracaoDto.Profundidade.FormatarParaDoubleComCasasDecimais());
            acervoTridimensional.Diametro.ShouldBe(acervoTridimensionalAlteracaoDto.Diametro.FormatarParaDoubleComCasasDecimais());
            
            var acervoTridimensionalArquivos = ObterTodos<AcervoTridimensionalArquivo>();
            var acervoTridimensionalArquivosInseridos = acervoTridimensionalArquivos.Where(w => w.AcervoTridimensionalId == acervoTridimensional.Id);
            acervoTridimensionalArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
        }
        
        [Fact(DisplayName = "Acervo Tridimensional - Atualizar (Removendo 1 arquivo, adicionando 5 novos)")]
        public async Task Atualizar_removendo_1_()
        {
            await InserirDadosBasicosAleatorios();

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
                Titulo = faker.Lorem.Text().Limite(500),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = faker.Date.Past().Year,
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Largura = double.Parse("50,45"),
                Altura = double.Parse("10.20"),
                Diametro = double.Parse("1540"),	
                Profundidade = double.Parse("1801"),
                Arquivos = arquivosSelecionados
            };
                
            await servicoAcervoTridimensional.Alterar(acervoTridimensionalAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoTridimensionalAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoTridimensionalAlteracaoDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals(acervoTridimensionalAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.DataAcervo.ShouldBe(acervoTridimensionalAlteracaoDto.DataAcervo);
            acervo.Ano.ShouldBe(acervoTridimensionalAlteracaoDto.Ano);
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Tridimensional);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            
            var acervoTridimensional = ObterTodos<AcervoTridimensional>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoTridimensional.Procedencia.ShouldBe(acervoTridimensionalAlteracaoDto.Procedencia);
            acervoTridimensional.ConservacaoId.ShouldBe(acervoTridimensionalAlteracaoDto.ConservacaoId);
            acervoTridimensional.Quantidade.ShouldBe(acervoTridimensionalAlteracaoDto.Quantidade);
            acervoTridimensional.Largura.ShouldBe(acervoTridimensionalAlteracaoDto.Largura.Value);
            acervoTridimensional.Altura.ShouldBe(acervoTridimensionalAlteracaoDto.Altura.FormatarParaDoubleComCasasDecimais());
            acervoTridimensional.Profundidade.ShouldBe(acervoTridimensionalAlteracaoDto.Profundidade.FormatarParaDoubleComCasasDecimais());
            acervoTridimensional.Diametro.ShouldBe(acervoTridimensionalAlteracaoDto.Diametro.FormatarParaDoubleComCasasDecimais());
            
            var acervoTridimensionalArquivos = ObterTodos<AcervoTridimensionalArquivo>();
            var acervoTridimensionalArquivosInseridos = acervoTridimensionalArquivos.Where(w => w.AcervoTridimensionalId == acervoTridimensional.Id);
            acervoTridimensionalArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
        }
        
        [Fact(DisplayName = "Acervo Tridimensional - Atualizar (Removendo todos)")]
        public async Task Atualizar_removendo_todos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();
            
            var random = new Random();
            
            var arquivosSelecionados = Array.Empty<long>();

            var servicoAcervoTridimensional = GetServicoAcervoTridimensional();

            var acervoTridimensionalAlteracaoDto = new AcervoTridimensionalAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.TD",
                Titulo = faker.Lorem.Text().Limite(500),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = faker.Date.Past().Year,
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Largura = double.Parse("50,45"),
                Altura = double.Parse("10.20"),
                Diametro = double.Parse("1540"),	
                Profundidade = double.Parse("1801"),	
                Arquivos = arquivosSelecionados
            };
                
            await servicoAcervoTridimensional.Alterar(acervoTridimensionalAlteracaoDto);
            
            var acervo = ObterTodos<Acervo>().FirstOrDefault(w=> w.Id == 3);
            acervo.Titulo.Equals(acervoTridimensionalAlteracaoDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoTridimensionalAlteracaoDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals(acervoTridimensionalAlteracaoDto.Codigo).ShouldBeTrue();
            acervo.DataAcervo.ShouldBe(acervoTridimensionalAlteracaoDto.DataAcervo);
            acervo.Ano.ShouldBe(acervoTridimensionalAlteracaoDto.Ano);
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Tridimensional);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldNotBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeTrue();
            acervo.AlteradoPor.ShouldNotBeNull();
            
            var acervoTridimensional = ObterTodos<AcervoTridimensional>().FirstOrDefault(w=> w.AcervoId == 3);
            acervoTridimensional.Procedencia.ShouldBe(acervoTridimensionalAlteracaoDto.Procedencia);
            acervoTridimensional.ConservacaoId.ShouldBe(acervoTridimensionalAlteracaoDto.ConservacaoId);
            acervoTridimensional.Quantidade.ShouldBe(acervoTridimensionalAlteracaoDto.Quantidade);
            acervoTridimensional.Largura.ShouldBe(acervoTridimensionalAlteracaoDto.Largura.Value);
            acervoTridimensional.Altura.ShouldBe(acervoTridimensionalAlteracaoDto.Altura.FormatarParaDoubleComCasasDecimais());
            acervoTridimensional.Profundidade.ShouldBe(acervoTridimensionalAlteracaoDto.Profundidade.FormatarParaDoubleComCasasDecimais());
            acervoTridimensional.Diametro.ShouldBe(acervoTridimensionalAlteracaoDto.Diametro.FormatarParaDoubleComCasasDecimais());
            
            var acervoTridimensionalArquivos = ObterTodos<AcervoTridimensionalArquivo>();
            var acervoTridimensionalArquivosInseridos = acervoTridimensionalArquivos.Where(w => w.AcervoTridimensionalId == acervoTridimensional.Id);
            acervoTridimensionalArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
        }
        
        [Fact(DisplayName = "Acervo Tridimensional - Não deve atualizar com ano futuro")]
        public async Task Nao_deve_atualizar_com_ano_futuro()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();
            
            var random = new Random();
            
            var arquivosSelecionados = Array.Empty<long>();

            var servicoAcervoTridimensional = GetServicoAcervoTridimensional();

            var acervoTridimensionalAlteracaoDto = new AcervoTridimensionalAlteracaoDTO()
            {
                Id = 3,
                AcervoId = 3,
                Codigo = "100.TD",
                Titulo = faker.Lorem.Text().Limite(500),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = DateTimeExtension.HorarioBrasilia().AddYears(1).Year,
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Largura = double.Parse("50,45"),
                Altura = double.Parse("10.20"),
                Diametro = double.Parse("1540"),	
                Profundidade = double.Parse("1801"),	
                Arquivos = arquivosSelecionados
            };
                
            await servicoAcervoTridimensional.Alterar(acervoTridimensionalAlteracaoDto).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Acervo Tridimensional - Inserir")]
        public async Task Inserir()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var servicoAcervoTridimensional = GetServicoAcervoTridimensional();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();

            var acervoTridimensionalCadastroDto = new AcervoTridimensionalCadastroDTO()
            {
                Codigo = "100",
                Titulo = faker.Lorem.Text().Limite(500),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = faker.Date.Past().Year,
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Largura = double.Parse("50,45"),
                Altura = double.Parse("10.20"),
                Diametro = double.Parse("1540"),	
                Profundidade = double.Parse("1801"),	
                Arquivos = arquivosSelecionados
            };
            
            var acervoTridimensionalInserido = await servicoAcervoTridimensional.Inserir(acervoTridimensionalCadastroDto);
            acervoTridimensionalInserido.ShouldBeGreaterThan(1);

            var acervo = ObterTodos<Acervo>().LastOrDefault();
            acervo.Titulo.Equals(acervoTridimensionalCadastroDto.Titulo).ShouldBeTrue();
            acervo.Descricao.Equals(acervoTridimensionalCadastroDto.Descricao).ShouldBeTrue();
            acervo.Codigo.Equals($"{acervoTridimensionalCadastroDto.Codigo}.TD").ShouldBeTrue();
            acervo.DataAcervo.ShouldBe(acervoTridimensionalCadastroDto.DataAcervo);
            acervo.Ano.ShouldBe(acervoTridimensionalCadastroDto.Ano);
            acervo.TipoAcervoId.ShouldBe((int)TipoAcervo.Tridimensional);
            acervo.CriadoLogin.ShouldNotBeEmpty();
            acervo.CriadoEm.Date.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            acervo.CriadoPor.ShouldNotBeEmpty();
            acervo.AlteradoLogin.ShouldBeNull();
            acervo.AlteradoEm.HasValue.ShouldBeFalse();
            acervo.AlteradoPor.ShouldBeNull();
            
            var acervoTridimensional = ObterTodos<AcervoTridimensional>().LastOrDefault();
            acervoTridimensional.Procedencia.ShouldBe(acervoTridimensionalCadastroDto.Procedencia);
            acervoTridimensional.ConservacaoId.ShouldBe(acervoTridimensionalCadastroDto.ConservacaoId);
            acervoTridimensional.Quantidade.ShouldBe(acervoTridimensionalCadastroDto.Quantidade);
            acervoTridimensional.Largura.ShouldBe(acervoTridimensionalCadastroDto.Largura.Value);
            acervoTridimensional.Altura.ShouldBe(acervoTridimensionalCadastroDto.Altura.FormatarParaDoubleComCasasDecimais());
            acervoTridimensional.Profundidade.ShouldBe(acervoTridimensionalCadastroDto.Profundidade.FormatarParaDoubleComCasasDecimais());
            acervoTridimensional.Diametro.ShouldBe(acervoTridimensionalCadastroDto.Diametro.FormatarParaDoubleComCasasDecimais());
            
            var acervoTridimensionalArquivos = ObterTodos<AcervoTridimensionalArquivo>();
            var acervoTridimensionalArquivosInseridos = acervoTridimensionalArquivos.Where(w => w.AcervoTridimensionalId == acervoTridimensional.Id);
            acervoTridimensionalArquivosInseridos.Count().ShouldBe(arquivosSelecionados.Count());
        }
        
        [Fact(DisplayName = "Acervo Tridimensional - Não deve inserir Tombo duplicado")]
        public async Task Nao_deve_inserir_duplicado()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var servicoAcervoTridimensional = GetServicoAcervoTridimensional();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();

            var acervoTridimensionalCadastroDto = new AcervoTridimensionalCadastroDTO()
            {
                Codigo = "1",
                Titulo = faker.Lorem.Text().Limite(500),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = faker.Date.Past().Year,
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Largura = double.Parse("50,45"),
                Altura = double.Parse("10.20"),
                Diametro = double.Parse("1540"),	
                Profundidade = double.Parse("1801"),	
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoTridimensional.Inserir(acervoTridimensionalCadastroDto).ShouldThrowAsync<NegocioException>();
           
        }
        
        [Fact(DisplayName = "Acervo Tridimensional - Não deve inserir com ano futuro")]
        public async Task Nao_deve_inserir_com_ano_futuro()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var servicoAcervoTridimensional = GetServicoAcervoTridimensional();
            
            var random = new Random();

            var arquivos = ObterTodos<Arquivo>();

            var arquivosSelecionados = arquivos.Take(5).Select(s => s.Id).ToArray();

            var acervoTridimensionalCadastroDto = new AcervoTridimensionalCadastroDTO()
            {
                Codigo = "1",
                Titulo = faker.Lorem.Text().Limite(500),
                Procedencia = faker.Lorem.Text().Limite(200),
                Ano = faker.Date.Future().Year,
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Largura = double.Parse("50,45"),
                Altura = double.Parse("10.20"),
                Diametro = double.Parse("1540"),	
                Profundidade = double.Parse("1801"),	
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoTridimensional.Inserir(acervoTridimensionalCadastroDto).ShouldThrowAsync<NegocioException>();
           
        }

        private async Task InserirAcervoTridimensional()
        {
            var random = new Random();
            
            var arquivoId = 1;

            for (int j = 1; j <= 35; j++)
            {
                await InserirNaBase(new Acervo()
                {
                    Codigo = $"{j.ToString()}.TD",
                    Titulo = faker.Lorem.Text().Limite(500),
                    Descricao = faker.Lorem.Text(),
                    TipoAcervoId = (int)TipoAcervo.Tridimensional,
                    CriadoPor = ConstantesTestes.SISTEMA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia().AddMinutes(-15),
                    CriadoLogin = ConstantesTestes.LOGIN_123456789,
                    Ano = faker.Date.Past().Year,
                    DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                });

                await InserirNaBase(new AcervoTridimensional()
                {
                    AcervoId = j,
                    Procedencia = faker.Lorem.Text().Limite(200),
                    ConservacaoId = random.Next(1,5),
                    Quantidade = random.Next(15,55),
                    Largura = double.Parse("50,45"),
                    Altura = double.Parse("10.20"),
                    Diametro = double.Parse("1540"),	
                    Profundidade = double.Parse("1801"),	
                });
                
                await InserirNaBase(new Arquivo()
                {
                    Nome = faker.Lorem.Text(),
                    Codigo = Guid.NewGuid(),
                    Tipo = TipoArquivo.AcervoTridimensional,
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
                
                await InserirNaBase(new AcervoTridimensionalArquivo()
                {
                    ArquivoId = arquivoId-1,
                    AcervoTridimensionalId = j,
                    ArquivoMiniaturaId = arquivoId
                });
                
                arquivoId++;
            }
        }
    }
}