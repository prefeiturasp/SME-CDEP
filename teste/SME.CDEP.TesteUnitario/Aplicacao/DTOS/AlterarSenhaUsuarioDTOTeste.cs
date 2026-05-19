using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AlterarSenhaUsuarioDtoTeste
    {
        [Fact]
        public void DadoAlterarSenhaUsuarioDTO_QuandoInstanciar_EntaoTodasAsPropriedadesSaoInicializadasCorretamente()
        {
            var dto = new AlterarSenhaUsuarioDTO();

            dto.Should().NotBeNull();
            dto.SenhaAtual.Should().BeNull();
            dto.SenhaNova.Should().BeNull();
            dto.ConfirmarSenha.Should().BeNull();
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirSenhaAtual_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new AlterarSenhaUsuarioDTO();
            var faker = new Faker();
            var senhaAtual = faker.Internet.Password();

            dto.SenhaAtual = senhaAtual;

            dto.SenhaAtual.Should().Be(senhaAtual);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirSenhaNova_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new AlterarSenhaUsuarioDTO();
            var faker = new Faker();
            var senhaNova = faker.Internet.Password();

            dto.SenhaNova = senhaNova;

            dto.SenhaNova.Should().Be(senhaNova);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirConfirmarSenha_EntaoOValorEhArmazenadoCorretamente()
        {
            var dto = new AlterarSenhaUsuarioDTO();
            var faker = new Faker();
            var confirmarSenha = faker.Internet.Password();

            dto.ConfirmarSenha = confirmarSenha;

            dto.ConfirmarSenha.Should().Be(confirmarSenha);
        }

        [Fact]
        public void DadoMultiplosValores_QuandoAtribuirTodasAsPropriedades_EntaoTodosOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AlterarSenhaUsuarioDTO();
            var faker = new Faker();
            var senhaAtual = faker.Internet.Password();
            var senhaNova = faker.Internet.Password();
            var confirmarSenha = faker.Internet.Password();

            dto.SenhaAtual = senhaAtual;
            dto.SenhaNova = senhaNova;
            dto.ConfirmarSenha = confirmarSenha;

            dto.SenhaAtual.Should().Be(senhaAtual);
            dto.SenhaNova.Should().Be(senhaNova);
            dto.ConfirmarSenha.Should().Be(confirmarSenha);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void DadoValoresNulosOuVazios_QuandoAtribuirSenhaAtual_EntaoOsValoresSaoArmazenadosCorretamente(string? valor)
        {
            var dto = new AlterarSenhaUsuarioDTO();

            dto.SenhaAtual = valor!;

            dto.SenhaAtual.Should().Be(valor);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void DadoValoresNulosOuVazios_QuandoAtribuirSenhaNova_EntaoOsValoresSaoArmazenadosCorretamente(string? valor)
        {
            var dto = new AlterarSenhaUsuarioDTO();

            dto.SenhaNova = valor!;

            dto.SenhaNova.Should().Be(valor);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void DadoValoresNulosOuVazios_QuandoAtribuirConfirmarSenha_EntaoOsValoresSaoArmazenadosCorretamente(string? valor)
        {
            var dto = new AlterarSenhaUsuarioDTO();

            dto.ConfirmarSenha = valor!;

            dto.ConfirmarSenha.Should().Be(valor);
        }

        [Fact]
        public void DadoSenhasComCaracteresEspeciais_QuandoAtribuirPropriedades_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AlterarSenhaUsuarioDTO();
            var senhaComEspeciais = "P@ssw0rd!#$%&*";

            dto.SenhaAtual = senhaComEspeciais;
            dto.SenhaNova = senhaComEspeciais;
            dto.ConfirmarSenha = senhaComEspeciais;

            dto.SenhaAtual.Should().Be(senhaComEspeciais);
            dto.SenhaNova.Should().Be(senhaComEspeciais);
            dto.ConfirmarSenha.Should().Be(senhaComEspeciais);
        }

        [Fact]
        public void DadoSenhasComEspacos_QuandoAtribuirPropriedades_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AlterarSenhaUsuarioDTO();
            var senhaComEspacos = "senha com espacos";

            dto.SenhaAtual = senhaComEspacos;
            dto.SenhaNova = senhaComEspacos;
            dto.ConfirmarSenha = senhaComEspacos;

            dto.SenhaAtual.Should().Be(senhaComEspacos);
            dto.SenhaNova.Should().Be(senhaComEspacos);
            dto.ConfirmarSenha.Should().Be(senhaComEspacos);
        }

        [Fact]
        public void DadoSenhasComUnicode_QuandoAtribuirPropriedades_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AlterarSenhaUsuarioDTO();
            var senhaComUnicode = "sênha_ção_123";

            dto.SenhaAtual = senhaComUnicode;
            dto.SenhaNova = senhaComUnicode;
            dto.ConfirmarSenha = senhaComUnicode;

            dto.SenhaAtual.Should().Be(senhaComUnicode);
            dto.SenhaNova.Should().Be(senhaComUnicode);
            dto.ConfirmarSenha.Should().Be(senhaComUnicode);
        }

        [Fact]
        public void DadoSenhasComComprimentoMuitoLongo_QuandoAtribuirPropriedades_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AlterarSenhaUsuarioDTO();
            var senhaLonga = new string('a', 1000);

            dto.SenhaAtual = senhaLonga;
            dto.SenhaNova = senhaLonga;
            dto.ConfirmarSenha = senhaLonga;

            dto.SenhaAtual.Should().Be(senhaLonga);
            dto.SenhaAtual.Should().HaveLength(1000);
            dto.SenhaNova.Should().Be(senhaLonga);
            dto.ConfirmarSenha.Should().Be(senhaLonga);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesMultiplaVezes_EntaoOUltimoValorEhRetido()
        {
            var dto = new AlterarSenhaUsuarioDTO();
            var faker = new Faker();
            var senhaAtual1 = faker.Internet.Password();
            var senhaAtual2 = faker.Internet.Password();
            var senhaNova1 = faker.Internet.Password();
            var senhaNova2 = faker.Internet.Password();
            var confirmarSenha1 = faker.Internet.Password();
            var confirmarSenha2 = faker.Internet.Password();

            dto.SenhaAtual = senhaAtual1;
            dto.SenhaNova = senhaNova1;
            dto.ConfirmarSenha = confirmarSenha1;

            dto.SenhaAtual.Should().Be(senhaAtual1);
            dto.SenhaNova.Should().Be(senhaNova1);
            dto.ConfirmarSenha.Should().Be(confirmarSenha1);

            dto.SenhaAtual = senhaAtual2;
            dto.SenhaNova = senhaNova2;
            dto.ConfirmarSenha = confirmarSenha2;

            dto.SenhaAtual.Should().Be(senhaAtual2);
            dto.SenhaNova.Should().Be(senhaNova2);
            dto.ConfirmarSenha.Should().Be(confirmarSenha2);
        }

        [Fact]
        public void DadoValoresValidos_QuandoAtribuirPropriedadesComInitializerSyntax_EntaoTodosOsValoresSaoArmazenadosCorretamente()
        {
            var faker = new Faker();
            var senhaAtual = faker.Internet.Password();
            var senhaNova = faker.Internet.Password();
            var confirmarSenha = faker.Internet.Password();

            var dto = new AlterarSenhaUsuarioDTO
            {
                SenhaAtual = senhaAtual,
                SenhaNova = senhaNova,
                ConfirmarSenha = confirmarSenha
            };

            dto.SenhaAtual.Should().Be(senhaAtual);
            dto.SenhaNova.Should().Be(senhaNova);
            dto.ConfirmarSenha.Should().Be(confirmarSenha);
        }

        [Fact]
        public void DadoSenhasCom0_QuandoAtribuirPropriedades_EntaoOsValoresSaoArmazenadosCorretamente()
        {
            var dto = new AlterarSenhaUsuarioDTO();
            var senha = "0";

            dto.SenhaAtual = senha;
            dto.SenhaNova = senha;
            dto.ConfirmarSenha = senha;

            dto.SenhaAtual.Should().Be(senha);
            dto.SenhaNova.Should().Be(senha);
            dto.ConfirmarSenha.Should().Be(senha);
        }

        [Fact]
        public void DadoPropriedadesSomenteLeitura_QuandoAcessarPropriedades_EntaoOsValoresSaoRetornadosCorretamente()
        {
            var dto = new AlterarSenhaUsuarioDTO();
            var faker = new Faker();
            var senhaAtual = faker.Internet.Password();
            var senhaNova = faker.Internet.Password();
            var confirmarSenha = faker.Internet.Password();

            dto.SenhaAtual = senhaAtual;
            dto.SenhaNova = senhaNova;
            dto.ConfirmarSenha = confirmarSenha;

            var senhaAtualRecuperada = dto.SenhaAtual;
            var senhaNovaRecuperada = dto.SenhaNova;
            var confirmarSenhaRecuperada = dto.ConfirmarSenha;

            senhaAtualRecuperada.Should().Be(senhaAtual);
            senhaNovaRecuperada.Should().Be(senhaNova);
            confirmarSenhaRecuperada.Should().Be(confirmarSenha);
        }

        [Fact]
        public void DadoMultiplasInstancias_QuandoInstanciarSeparadamente_EntaoNaoCompartilhamDados()
        {
            var dto1 = new AlterarSenhaUsuarioDTO();
            var dto2 = new AlterarSenhaUsuarioDTO();
            var faker = new Faker();
            var senhaAtual1 = faker.Internet.Password();
            var senhaAtual2 = faker.Internet.Password();

            dto1.SenhaAtual = senhaAtual1;
            dto2.SenhaAtual = senhaAtual2;

            dto1.SenhaAtual.Should().Be(senhaAtual1);
            dto2.SenhaAtual.Should().Be(senhaAtual2);
            dto1.SenhaAtual.Should().NotBe(dto2.SenhaAtual);
        }
    }
}
