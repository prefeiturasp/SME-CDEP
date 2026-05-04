using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class ArquivoCodigoNomeDTOTeste
    {
        [Fact]
        public void DadoArquivoCodigoNomeDTO_QuandoInstanciar_EntaoTodasAsPropriedadesSaoInicializadasCorretamente()
        {
            var dto = new ArquivoCodigoNomeDTO();

            dto.Should().NotBeNull();
            dto.Nome.Should().BeNull();
            dto.Codigo.Should().Be(default(Guid));
        }

        [Fact]
        public void DadoValorValido_QuandoAtribuirNome_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new ArquivoCodigoNomeDTO();
            var faker = new Faker("pt_BR");
            var nome = faker.System.FileName();

            dto.Nome = nome;

            dto.Nome.Should().Be(nome);
        }

        [Fact]
        public void DadoValorValido_QuandoAtribuirCodigo_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new ArquivoCodigoNomeDTO();
            var codigo = Guid.NewGuid();

            dto.Codigo = codigo;

            dto.Codigo.Should().Be(codigo);
        }

        [Fact]
        public void DadoMultiplosValores_QuandoAtribuirTodasAsPropriedades_EntaoTodosOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new ArquivoCodigoNomeDTO();
            var faker = new Faker("pt_BR");
            var nome = faker.System.FileName();
            var codigo = Guid.NewGuid();

            dto.Nome = nome;
            dto.Codigo = codigo;

            dto.Nome.Should().Be(nome);
            dto.Codigo.Should().Be(codigo);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void DadoValoresNulosOuVazios_QuandoAtribuirNome_EntaoOsValoresSaoArmazenadosCorretamente(string valor)
        {
            var dto = new ArquivoCodigoNomeDTO();

            dto.Nome = valor;

            dto.Nome.Should().Be(valor);
        }

        [Fact]
        public void DadoGuidVazio_QuandoAtribuirCodigo_EntaoOValorVazioEhArmazenado()
        {
            var dto = new ArquivoCodigoNomeDTO();
            var guidVazio = Guid.Empty;

            dto.Codigo = guidVazio;

            dto.Codigo.Should().Be(guidVazio);
        }

        [Fact]
        public void DadoMultiplosGuids_QuandoAtribuirCodigoVariasVezes_EntaoOUltimoValorEhRetido()
        {
            var dto = new ArquivoCodigoNomeDTO();
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

        [Fact]
        public void DadoMultiplosNomes_QuandoAtribuirNomeVariasVezes_EntaoOUltimoValorEhRetido()
        {
            var dto = new ArquivoCodigoNomeDTO();
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
        public void DadoPropriedadeNomeComDados_QuandoAtribuirMultiplesVezes_EntaoOUltimoValorEhRetido()
        {
            var dto = new ArquivoCodigoNomeDTO();
            var faker = new Faker("pt_BR");
            var nome1 = faker.System.FileName();
            var nome2 = faker.System.FileName();

            dto.Nome = nome1;
            dto.Nome = nome2;

            dto.Nome.Should().Be(nome2);
            dto.Nome.Should().NotBe(nome1);
        }

        [Fact]
        public void DadoObjetoComPropriedades_QuandoVerificarTipo_EntaoEhDoTipoArquivoCodigoNomeDTO()
        {
            var dto = new ArquivoCodigoNomeDTO();

            dto.Should().BeOfType<ArquivoCodigoNomeDTO>();
        }

        [Fact]
        public void DadoNomeComCaracteresEspeciais_QuandoAtribuirNome_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new ArquivoCodigoNomeDTO();
            var nome = "arquivo-especial_2024.pdf";

            dto.Nome = nome;

            dto.Nome.Should().Be(nome);
        }

        [Fact]
        public void DadoNomeComEspacos_QuandoAtribuirNome_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new ArquivoCodigoNomeDTO();
            var nome = "arquivo com espacos.docx";

            dto.Nome = nome;

            dto.Nome.Should().Be(nome);
        }
    }
}
