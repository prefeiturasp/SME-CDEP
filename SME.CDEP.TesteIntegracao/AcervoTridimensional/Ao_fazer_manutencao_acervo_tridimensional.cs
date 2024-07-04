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
                Ano = faker.Date.Past().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Largura = "50,45",
                Altura = "10,20",
                Diametro = "15,00",	
                Profundidade = "18,20",	
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
            acervoTridimensional.Largura.ShouldBe(acervoTridimensionalAlteracaoDto.Largura);
            acervoTridimensional.Altura.ShouldBe(acervoTridimensionalAlteracaoDto.Altura);
            acervoTridimensional.Profundidade.ShouldBe(acervoTridimensionalAlteracaoDto.Profundidade);
            acervoTridimensional.Diametro.ShouldBe(acervoTridimensionalAlteracaoDto.Diametro);
            
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
                Ano = faker.Date.Past().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Largura = "50,45",
                Altura = "10,20",
                Diametro = "15,40",	
                Profundidade = "18,01",
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
            acervoTridimensional.Largura.ShouldBe(acervoTridimensionalAlteracaoDto.Largura);
            acervoTridimensional.Altura.ShouldBe(acervoTridimensionalAlteracaoDto.Altura);
            acervoTridimensional.Profundidade.ShouldBe(acervoTridimensionalAlteracaoDto.Profundidade);
            acervoTridimensional.Diametro.ShouldBe(acervoTridimensionalAlteracaoDto.Diametro);
            
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
                Ano = faker.Date.Past().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Largura = "50,45",
                Altura = "10,20",
                Diametro = "15,40",	
                Profundidade = "18,01",	
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
            acervoTridimensional.Largura.ShouldBe(acervoTridimensionalAlteracaoDto.Largura);
            acervoTridimensional.Altura.ShouldBe(acervoTridimensionalAlteracaoDto.Altura);
            acervoTridimensional.Profundidade.ShouldBe(acervoTridimensionalAlteracaoDto.Profundidade);
            acervoTridimensional.Diametro.ShouldBe(acervoTridimensionalAlteracaoDto.Diametro);
            
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
                Ano = DateTimeExtension.HorarioBrasilia().AddYears(1).Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Largura = "50,45",
                Altura = "10,20",
                Diametro = "15,40",	
                Profundidade = "18,01",	
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
                Ano = faker.Date.Past().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Largura = "50,45",
                Altura = "10,20",
                Diametro = "15,40",	
                Profundidade = "18,01",	
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
            acervoTridimensional.Largura.ShouldBe(acervoTridimensionalCadastroDto.Largura);
            acervoTridimensional.Altura.ShouldBe(acervoTridimensionalCadastroDto.Altura);
            acervoTridimensional.Profundidade.ShouldBe(acervoTridimensionalCadastroDto.Profundidade);
            acervoTridimensional.Diametro.ShouldBe(acervoTridimensionalCadastroDto.Diametro);
            
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
                Ano = faker.Date.Past().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Largura = "50,45",
                Altura = "10,20",
                Diametro = "15,40",	
                Profundidade = "18,01",	
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
                Ano = faker.Date.Future().Year.ToString(),
                DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"),
                ConservacaoId = random.Next(1, 5),
                Quantidade = random.Next(15, 55),
                Descricao = faker.Lorem.Text(),
                Largura = "50,45",
                Altura = "10,20",
                Diametro = "15,40",	
                Profundidade = "18,01",	
                Arquivos = arquivosSelecionados
            };
            
            await servicoAcervoTridimensional.Inserir(acervoTridimensionalCadastroDto).ShouldThrowAsync<NegocioException>();
           
        }
        
        [Fact(DisplayName = "Acervo Tridimensional - Tratar o retorno de dimensões")]
        public async Task Tratar_retorno_dimensoes()
        {
            var detalhe = new AcervoTridimensionalDetalhe() { Largura = "15,20", Altura = "18,20", Diametro = "23,56", Profundidade = "45,16" };
            detalhe.Dimensoes.ShouldBe("15,20(Largura) x 18,20(Altura) x 45,16(Profundidade) x 23,56(Diâmetro)");
            
            detalhe = new AcervoTridimensionalDetalhe() { Largura = "15,20", Altura = "18,20", Profundidade = "45,16"};
            detalhe.Dimensoes.ShouldBe("15,20(Largura) x 18,20(Altura) x 45,16(Profundidade)");
            
            detalhe = new AcervoTridimensionalDetalhe() { Largura = "15,20", Diametro = "23,56", Profundidade = "45,16"};
            detalhe.Dimensoes.ShouldBe("15,20(Largura) x 45,16(Profundidade) x 23,56(Diâmetro)");
            
            detalhe = new AcervoTridimensionalDetalhe() { Altura = "18,20", Diametro = "23,56" };
            detalhe.Dimensoes.ShouldBe("18,20(Altura) x 23,56(Diâmetro)");
            
            detalhe = new AcervoTridimensionalDetalhe() { Largura = "15,20" };
            detalhe.Dimensoes.ShouldBe("15,20(Largura)");
            
            detalhe = new AcervoTridimensionalDetalhe() { Altura = "18,20" };
            detalhe.Dimensoes.ShouldBe("18,20(Altura)");
            
            detalhe = new AcervoTridimensionalDetalhe() { Diametro = "23,56" };
            detalhe.Dimensoes.ShouldBe("23,56(Diâmetro)");
            
            detalhe = new AcervoTridimensionalDetalhe() { Profundidade = "45,16" };
            detalhe.Dimensoes.ShouldBe("45,16(Profundidade)");
            
            detalhe = new AcervoTridimensionalDetalhe();
            detalhe.Dimensoes.ShouldBe(string.Empty);
        }
    }
}