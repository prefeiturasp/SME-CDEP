using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class ArquivoDtoTeste
    {
        [Fact]
        public void DadoArquivoDTO_QuandoInstanciar_EntaoTodasAsPropriedadesSaoInicializadasCorretamente()
        {
            var dto = new ArquivoDTO();

            dto.Should().NotBeNull();
            dto.Nome.Should().BeNull();
            dto.Codigo.Should().Be(Guid.Empty);
            dto.TipoConteudo.Should().BeNull();
            dto.Tipo.Should().Be(default(TipoArquivo));
            dto.Id.Should().Be(0);
            dto.Excluido.Should().BeFalse();
        }

        [Fact]
        public void DadoValorValido_QuandoAtribuirNome_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new ArquivoDTO();
            var faker = new Faker("pt_BR");
            var nome = faker.System.FileName();

            dto.Nome = nome;

            dto.Nome.Should().Be(nome);
        }

        [Fact]
        public void DadoValorValido_QuandoAtribuirCodigo_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new ArquivoDTO();
            var codigo = Guid.NewGuid();

            dto.Codigo = codigo;

            dto.Codigo.Should().Be(codigo);
        }

        [Fact]
        public void DadoValorValido_QuandoAtribuirTipoConteudo_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new ArquivoDTO();
            var tipoConteudo = "application/pdf";

            dto.TipoConteudo = tipoConteudo;

            dto.TipoConteudo.Should().Be(tipoConteudo);
        }

        [Fact]
        public void DadoValorValido_QuandoAtribuirTipo_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new ArquivoDTO();
            var tipo = TipoArquivo.AcervoFotografico;

            dto.Tipo = tipo;

            dto.Tipo.Should().Be(tipo);
        }

        [Fact]
        public void DadoMultiplosValores_QuandoAtribuirTodasAsPropriedades_EntaoTodosOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new ArquivoDTO();
            var faker = new Faker("pt_BR");
            var nome = faker.System.FileName();
            var codigo = Guid.NewGuid();
            var tipoConteudo = "application/pdf";
            var tipo = TipoArquivo.AcervoDocumental;

            dto.Nome = nome;
            dto.Codigo = codigo;
            dto.TipoConteudo = tipoConteudo;
            dto.Tipo = tipo;

            dto.Nome.Should().Be(nome);
            dto.Codigo.Should().Be(codigo);
            dto.TipoConteudo.Should().Be(tipoConteudo);
            dto.Tipo.Should().Be(tipo);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void DadoValoresNulosOuVazios_QuandoAtribuirNome_EntaoOsValoresSaoArmazenadosCorretamente(string? valor)
        {
            var dto = new ArquivoDTO();

            dto.Nome = valor!;

            dto.Nome.Should().Be(valor);
        }

        [Fact]
        public void DadoGuidVazio_QuandoAtribuirCodigo_EntaoOValorVazioEhArmazenado()
        {
            var dto = new ArquivoDTO();
            var guidVazio = Guid.Empty;

            dto.Codigo = guidVazio;

            dto.Codigo.Should().Be(guidVazio);
        }

        [Fact]
        public void DadoMultiplosGuids_QuandoAtribuirCodigoVariasVezes_EntaoOUltimoValorEhRetido()
        {
            var dto = new ArquivoDTO();
            var codigo1 = Guid.NewGuid();
            var codigo2 = Guid.NewGuid();
            var codigo3 = Guid.NewGuid();

            dto.Codigo = codigo1;
            dto.Codigo = codigo2;
            dto.Codigo = codigo3;

            dto.Codigo.Should().Be(codigo3);
            dto.Codigo.Should().NotBe(codigo1);
            dto.Codigo.Should().NotBe(codigo2);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void DadoValoresNulosOuVazios_QuandoAtribuirTipoConteudo_EntaoOsValoresSaoArmazenadosCorretamente(string? valor)
        {
            var dto = new ArquivoDTO();

            dto.TipoConteudo = valor!;

            dto.TipoConteudo.Should().Be(valor);
        }

        [Theory]
        [InlineData(TipoArquivo.Temp)]
        [InlineData(TipoArquivo.Editor)]
        [InlineData(TipoArquivo.AcervoFotografico)]
        [InlineData(TipoArquivo.AcervoArteGrafica)]
        [InlineData(TipoArquivo.AcervoTridimensional)]
        [InlineData(TipoArquivo.AcervoDocumental)]
        [InlineData(TipoArquivo.Sistema)]
        public void DadoTiposArquivoDiferentes_QuandoAtribuir_EntaoTodosOsValoresSaoArmazenadosCorretamente(TipoArquivo tipo)
        {
            var dto = new ArquivoDTO();

            dto.Tipo = tipo;

            dto.Tipo.Should().Be(tipo);
        }

        [Fact]
        public void DadoMultiplosNomes_QuandoAtribuirNomeVariasVezes_EntaoOUltimoValorEhRetido()
        {
            var dto = new ArquivoDTO();
            var faker = new Faker("pt_BR");
            var nome1 = faker.System.FileName();
            var nome2 = faker.System.FileName();
            var nome3 = faker.System.FileName();

            dto.Nome = nome1;
            dto.Nome = nome2;
            dto.Nome = nome3;

            dto.Nome.Should().Be(nome3);
            dto.Nome.Should().NotBe(nome1);
            dto.Nome.Should().NotBe(nome2);
        }

        [Fact]
        public void DadoMultiplosConteudos_QuandoAtribuirTipoConteudoVariasVezes_EntaoOUltimoValorEhRetido()
        {
            var dto = new ArquivoDTO();
            var conteudo1 = "application/pdf";
            var conteudo2 = "application/msword";
            var conteudo3 = "image/jpeg";

            dto.TipoConteudo = conteudo1;
            dto.TipoConteudo = conteudo2;
            dto.TipoConteudo = conteudo3;

            dto.TipoConteudo.Should().Be(conteudo3);
            dto.TipoConteudo.Should().NotBe(conteudo1);
            dto.TipoConteudo.Should().NotBe(conteudo2);
        }

        [Fact]
        public void DadoMultiplosTipos_QuandoAtribuirTipoVariasVezes_EntaoOUltimoValorEhRetido()
        {
            var dto = new ArquivoDTO();
            var tipo1 = TipoArquivo.Temp;
            var tipo2 = TipoArquivo.Editor;
            var tipo3 = TipoArquivo.AcervoFotografico;

            dto.Tipo = tipo1;
            dto.Tipo = tipo2;
            dto.Tipo = tipo3;

            dto.Tipo.Should().Be(tipo3);
            dto.Tipo.Should().NotBe(tipo1);
            dto.Tipo.Should().NotBe(tipo2);
        }

        [Fact]
        public void DadoObjetoComPropriedades_QuandoVerificarTipo_EntaoEhDoTipoArquivoDTO()
        {
            var dto = new ArquivoDTO();

            dto.Should().BeOfType<ArquivoDTO>();
        }

        [Fact]
        public void DadoObjetoArquivoDTO_QuandoVerificarHeranca_EntaoEhDerivaDoBaseAuditavelDTO()
        {
            var dto = new ArquivoDTO();

            dto.Should().BeAssignableTo<BaseAuditavelDTO>();
        }

        [Fact]
        public void DadoNomeComCaracteresEspeciais_QuandoAtribuirNome_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new ArquivoDTO();
            var nome = "arquivo-especial_2024.pdf";

            dto.Nome = nome;

            dto.Nome.Should().Be(nome);
        }

        [Fact]
        public void DadoNomeComEspacos_QuandoAtribuirNome_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new ArquivoDTO();
            var nome = "arquivo com espacos.docx";

            dto.Nome = nome;

            dto.Nome.Should().Be(nome);
        }

        [Fact]
        public void DadoTipoConteudoComCaracteresEspeciais_QuandoAtribuir_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new ArquivoDTO();
            var tipoConteudo = "application/vnd.ms-excel";

            dto.TipoConteudo = tipoConteudo;

            dto.TipoConteudo.Should().Be(tipoConteudo);
        }

        [Fact]
        public void DadoVariosTiposConteudoValidos_QuandoAtribuir_EntaoTodosArmazenam()
        {
            var dto = new ArquivoDTO();
            var tiposConteudo = new[] 
            { 
                "text/plain", 
                "text/html", 
                "application/json", 
                "image/png", 
                "image/gif",
                "video/mp4"
            };

            foreach (var tipoConteudo in tiposConteudo)
            {
                dto.TipoConteudo = tipoConteudo;
                dto.TipoConteudo.Should().Be(tipoConteudo);
            }
        }

        [Fact]
        public void DadoPropriedadesDoBaseAuditavelDTO_QuandoAtribuir_EntaoSaoArmazenadosCorretamente()
        {
            var dto = new ArquivoDTO();
            var id = 123L;
            var excluido = true;
            var criadoEm = DateTime.Now;
            var criadoPor = "usuario_teste";
            var criadoLogin = "usuario.teste";
            var alteradoEm = DateTime.Now.AddDays(1);
            var alteradoPor = "usuario_alteracao";
            var alteradoLogin = "usuario.alteracao";

            dto.Id = id;
            dto.Excluido = excluido;
            dto.CriadoEm = criadoEm;
            dto.CriadoPor = criadoPor;
            dto.CriadoLogin = criadoLogin;
            dto.AlteradoEm = alteradoEm;
            dto.AlteradoPor = alteradoPor;
            dto.AlteradoLogin = alteradoLogin;

            dto.Id.Should().Be(id);
            dto.Excluido.Should().Be(excluido);
            dto.CriadoEm.Should().Be(criadoEm);
            dto.CriadoPor.Should().Be(criadoPor);
            dto.CriadoLogin.Should().Be(criadoLogin);
            dto.AlteradoEm.Should().Be(alteradoEm);
            dto.AlteradoPor.Should().Be(alteradoPor);
            dto.AlteradoLogin.Should().Be(alteradoLogin);
        }

        [Fact]
        public void DadoTodosOsValoresValidosCompletos_QuandoInstanciarEpopularDTO_EntaoTodosDadosArmazenamCorretos()
        {
            var faker = new Faker("pt_BR");
            var dto = new ArquivoDTO();
            var nome = faker.System.FileName();
            var codigo = Guid.NewGuid();
            var tipoConteudo = "application/pdf";
            var tipo = TipoArquivo.AcervoDocumental;
            var id = faker.Random.Long(1, 10000);
            var excluido = false;
            var criadoEm = faker.Date.Past();
            var criadoPor = faker.Name.FullName();
            var criadoLogin = faker.Internet.UserName();
            var alteradoEm = faker.Date.Recent();
            var alteradoPor = faker.Name.FullName();
            var alteradoLogin = faker.Internet.UserName();

            dto.Nome = nome;
            dto.Codigo = codigo;
            dto.TipoConteudo = tipoConteudo;
            dto.Tipo = tipo;
            dto.Id = id;
            dto.Excluido = excluido;
            dto.CriadoEm = criadoEm;
            dto.CriadoPor = criadoPor;
            dto.CriadoLogin = criadoLogin;
            dto.AlteradoEm = alteradoEm;
            dto.AlteradoPor = alteradoPor;
            dto.AlteradoLogin = alteradoLogin;

            dto.Nome.Should().Be(nome);
            dto.Codigo.Should().Be(codigo);
            dto.TipoConteudo.Should().Be(tipoConteudo);
            dto.Tipo.Should().Be(tipo);
            dto.Id.Should().Be(id);
            dto.Excluido.Should().Be(excluido);
            dto.CriadoEm.Should().Be(criadoEm);
            dto.CriadoPor.Should().Be(criadoPor);
            dto.CriadoLogin.Should().Be(criadoLogin);
            dto.AlteradoEm.Should().Be(alteradoEm);
            dto.AlteradoPor.Should().Be(alteradoPor);
            dto.AlteradoLogin.Should().Be(alteradoLogin);
        }

        [Fact]
        public void DadoNomeComUnicode_QuandoAtribuir_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new ArquivoDTO();
            var nome = "documento_josé_2024.pdf";

            dto.Nome = nome;

            dto.Nome.Should().Be(nome);
        }

        [Fact]
        public void DadoGuidComValorMaximo_QuandoAtribuir_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new ArquivoDTO();
            var guidMaximo = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");

            dto.Codigo = guidMaximo;

            dto.Codigo.Should().Be(guidMaximo);
        }

        [Fact]
        public void DadoAlteradoEmNulo_QuandoAtribuir_EntaoValorNuloEhArmazenado()
        {
            var dto = new ArquivoDTO();

            dto.AlteradoEm = null;

            dto.AlteradoEm.Should().BeNull();
        }

        [Fact]
        public void DadoAlteradoPorNulo_QuandoAtribuir_EntaoValorNuloEhArmazenado()
        {
            var dto = new ArquivoDTO();

            dto.AlteradoPor = null;

            dto.AlteradoPor.Should().BeNull();
        }

        [Fact]
        public void DadoAlteradoLoginNulo_QuandoAtribuir_EntaoValorNuloEhArmazenado()
        {
            var dto = new ArquivoDTO();

            dto.AlteradoLogin = null;

            dto.AlteradoLogin.Should().BeNull();
        }
    }
}
