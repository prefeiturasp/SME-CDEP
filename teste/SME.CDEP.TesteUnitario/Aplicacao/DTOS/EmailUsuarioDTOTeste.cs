using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class EmailUsuarioDtoTeste
    {
        [Fact]
        public void DadoDTOValido_QuandoCriar_EntaoDevePossuirPropriedades()
        {
            var dto = new EmailUsuarioDTO();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<EmailUsuarioDTO>();
        }

        [Fact]
        public void DadoEmailDefinido_QuandoAtribuir_EntaoEmailDeveSerSetado()
        {
            var dto = new EmailUsuarioDTO();
            var emailEsperado = "usuario@example.com";

            dto.Email = emailEsperado;

            dto.Email.Should().Be(emailEsperado);
        }

        [Fact]
        public void DadoEmailNulo_QuandoAtribuir_EntaoEmailDeveSerNulo()
        {
            var dto = new EmailUsuarioDTO();

            dto.Email = null!;

            dto.Email.Should().BeNull();
        }

        [Fact]
        public void DadoEmailVazio_QuandoAtribuir_EntaoEmailDeveSerVazio()
        {
            var dto = new EmailUsuarioDTO();

            dto.Email = "";

            dto.Email.Should().BeEmpty();
        }

        [Fact]
        public void DadoEmailComEspacosEmBranco_QuandoAtribuir_EntaoEmailDeveMantidoComEspacos()
        {
            var dto = new EmailUsuarioDTO();
            var emailComEspacos = "  usuario@example.com  ";

            dto.Email = emailComEspacos;

            dto.Email.Should().Be(emailComEspacos);
        }

        [Fact]
        public void DadoEmailValido_QuandoAtribuir_EntaoEmailComPontosDeveSerSetado()
        {
            var dto = new EmailUsuarioDTO();
            var emailComPontos = "usuario.sobrenome@example.com";

            dto.Email = emailComPontos;

            dto.Email.Should().Be(emailComPontos);
        }

        [Fact]
        public void DadoEmailValido_QuandoAtribuir_EntaoEmailComSubdominioDeveSerSetado()
        {
            var dto = new EmailUsuarioDTO();
            var emailComSubdominio = "usuario@mail.example.com";

            dto.Email = emailComSubdominio;

            dto.Email.Should().Be(emailComSubdominio);
        }

        [Fact]
        public void DadoEmailComMaisDeUmCaracter_QuandoAtribuir_EntaoEmailComPlusDeveSerSetado()
        {
            var dto = new EmailUsuarioDTO();
            var emailComPlus = "usuario+teste@example.com";

            dto.Email = emailComPlus;

            dto.Email.Should().Be(emailComPlus);
        }

        [Fact]
        public void DadoEmailComCaracteresEspeciais_QuandoAtribuir_EntaoEmailComUnderscoreDeveSerSetado()
        {
            var dto = new EmailUsuarioDTO();
            var emailComUnderscore = "usuario_teste@example.com";

            dto.Email = emailComUnderscore;

            dto.Email.Should().Be(emailComUnderscore);
        }

        [Fact]
        public void DadoEmailComCaracteresEspeciais_QuandoAtribuir_EntaoEmailComHifenDeveSerSetado()
        {
            var dto = new EmailUsuarioDTO();
            var emailComHifen = "usuario-teste@example.com";

            dto.Email = emailComHifen;

            dto.Email.Should().Be(emailComHifen);
        }

        [Fact]
        public void DadoEmailComDominioInternacional_QuandoAtribuir_EntaoEmailComExtensaoBrasileiradeveSerSetado()
        {
            var dto = new EmailUsuarioDTO();
            var emailBrasileiro = "usuario@example.com.br";

            dto.Email = emailBrasileiro;

            dto.Email.Should().Be(emailBrasileiro);
        }

        [Fact]
        public void DadoMultiplosDTOs_QuandoCriar_EntaoTodosDevemSerIndependentes()
        {
            var dto1 = new EmailUsuarioDTO { Email = "usuario1@example.com" };
            var dto2 = new EmailUsuarioDTO { Email = "usuario2@example.com" };

            dto1.Email.Should().NotBe(dto2.Email);
            dto1.Should().NotBeSameAs(dto2);
        }

        [Fact]
        public void DadoDTOComEmailAlterado_QuandoVerificar_EntaoAlteracaoDeveSerReflexas()
        {
            var dto = new EmailUsuarioDTO();
            var emailInicial = "usuario1@example.com";
            var emailAlterado = "usuario2@example.com";

            dto.Email = emailInicial;
            dto.Email = emailAlterado;

            dto.Email.Should().Be(emailAlterado);
            dto.Email.Should().NotBe(emailInicial);
        }

        [Fact]
        public void DadoDTOUsandoFaker_QuandoGerarDados_EntaoDeveSerPreenchidoCorretamente()
        {
            var faker = new Faker<EmailUsuarioDTO>("pt_BR")
                .RuleFor(x => x.Email, f => f.Internet.Email());

            var dto = faker.Generate();

            dto.Should().NotBeNull();
            dto.Email.Should().NotBeNullOrEmpty();
            dto.Email.Should().Contain("@");
        }

        [Fact]
        public void DadoListaDeTresDTOs_QuandoGerarMultiplos_EntaoTodosDevemSerValidos()
        {
            var faker = new Faker<EmailUsuarioDTO>("pt_BR")
                .RuleFor(x => x.Email, f => f.Internet.Email());

            var dtos = faker.Generate(3).ToList();

            dtos.Should().HaveCount(3);
            dtos.Should().AllSatisfy(dto =>
            {
                dto.Email.Should().NotBeNullOrEmpty();
                dto.Email.Should().Contain("@");
            });
        }

        [Fact]
        public void DadoDTOComEmailGrandeTamanho_QuandoAtribuir_EntaoDeveSuportarGrandesTextos()
        {
            var dto = new EmailUsuarioDTO();
            var emailLongo = $"{string.Join("", new Faker("pt_BR").Lorem.Words(5))}@longemailaddress.example.com";

            dto.Email = emailLongo;

            dto.Email.Should().Be(emailLongo);
            dto.Email.Length.Should().BeGreaterThan(30);
        }

        [Fact]
        public void DadoDTOComEmailCaracteresUnicode_QuandoAtribuir_EntaoDeveSuportarUnicode()
        {
            var dto = new EmailUsuarioDTO();
            var emailUnicode = "usuário@example.com";

            dto.Email = emailUnicode;

            dto.Email.Should().Be(emailUnicode);
        }

        [Fact]
        public void DadoDTOComMultiplosCamposEmail_QuandoAlterarSequencialmente_EntaoUltimoValorDevePredominar()
        {
            var dto = new EmailUsuarioDTO();

            dto.Email = "email1@example.com";
            dto.Email = "email2@example.com";
            dto.Email = "email3@example.com";
            dto.Email = "email4@example.com";

            dto.Email.Should().Be("email4@example.com");
        }

        [Fact]
        public void DadoDTOComDadosCompletos_QuandoVerificarEquivalencia_EntaoDeveSerEquivalente()
        {
            var dto1 = new EmailUsuarioDTO { Email = "mesmo@example.com" };
            var dto2 = new EmailUsuarioDTO { Email = "mesmo@example.com" };

            dto1.Should().BeEquivalentTo(dto2);
        }

        [Fact]
        public void DadoDTOComEmailDiferente_QuandoVerificarEquivalencia_EntaoNaoDeveSerEquivalente()
        {
            var dto1 = new EmailUsuarioDTO { Email = "email1@example.com" };
            var dto2 = new EmailUsuarioDTO { Email = "email2@example.com" };

            dto1.Should().NotBeEquivalentTo(dto2);
        }

        [Fact]
        public void DadoDTOComPropriedadesDefinidas_QuandoAcessar_EntaoEmailDeveEstarDisponível()
        {
            var propriedades = typeof(EmailUsuarioDTO).GetProperties();

            propriedades.Should().NotBeEmpty();
            propriedades.Should().Contain(p => p.Name == "Email");
        }

        [Fact]
        public void DadoDTOVazio_QuandoCriar_EntaoEmailDeveSerNulo()
        {
            var dto = new EmailUsuarioDTO();

            dto.Email.Should().BeNull();
        }

        [Fact]
        public void DadoEmailComDozeCaracteres_QuandoAtribuir_EntaoDeveSuportarEmailsCurtos()
        {
            var dto = new EmailUsuarioDTO();
            var emailCurto = "a@example.c";

            dto.Email = emailCurto;

            dto.Email.Should().Be(emailCurto);
        }

        [Fact]
        public void DadoEmailComMaisDeUmaArroba_QuandoAtribuir_EntaoDeveSuportarMultiplasArrobas()
        {
            var dto = new EmailUsuarioDTO();
            var emailComMultiplasArrobas = "usuario@@example.com";

            dto.Email = emailComMultiplasArrobas;

            dto.Email.Should().Be(emailComMultiplasArrobas);
        }

        [Fact]
        public void DadoEmailSemDominio_QuandoAtribuir_EntaoDeveSuportarEmailsSemDominio()
        {
            var dto = new EmailUsuarioDTO();
            var emailSemDominio = "usuario@";

            dto.Email = emailSemDominio;

            dto.Email.Should().Be(emailSemDominio);
        }

        [Fact]
        public void DadoEmailSemUsuario_QuandoAtribuir_EntaoDeveSuportarEmailsSemUsuario()
        {
            var dto = new EmailUsuarioDTO();
            var emailSemUsuario = "@example.com";

            dto.Email = emailSemUsuario;

            dto.Email.Should().Be(emailSemUsuario);
        }

        [Fact]
        public void DadoDTOComEmailNumerico_QuandoAtribuir_EntaoDeveSuportarEmailsComNumeros()
        {
            var dto = new EmailUsuarioDTO();
            var emailComNumeros = "usuario123@example456.com";

            dto.Email = emailComNumeros;

            dto.Email.Should().Be(emailComNumeros);
        }

        [Fact]
        public void DadoDTOComEmailMaiusculo_QuandoAtribuir_EntaoEmailDeveMantidoComMaiuscula()
        {
            var dto = new EmailUsuarioDTO();
            var emailMaiusculo = "USUARIO@EXAMPLE.COM";

            dto.Email = emailMaiusculo;

            dto.Email.Should().Be(emailMaiusculo);
        }

        [Fact]
        public void DadoDTOComEmailMinusculo_QuandoAtribuir_EntaoEmailDeveMantidoComMinuscula()
        {
            var dto = new EmailUsuarioDTO();
            var emailMinusculo = "usuario@example.com";

            dto.Email = emailMinusculo;

            dto.Email.Should().Be(emailMinusculo);
        }

        [Fact]
        public void DadoDTOComEmailMisto_QuandoAtribuir_EntaoEmailDeveMantidoComMisto()
        {
            var dto = new EmailUsuarioDTO();
            var emailMisto = "Usuario@Example.Com";

            dto.Email = emailMisto;

            dto.Email.Should().Be(emailMisto);
        }

        [Fact]
        public void DadoDTOComEmailComPonto_QuandoAtribuir_EntaoEmailComPontoNoInícioDeveSerSetado()
        {
            var dto = new EmailUsuarioDTO();
            var emailComPontoInicio = ".usuario@example.com";

            dto.Email = emailComPontoInicio;

            dto.Email.Should().Be(emailComPontoInicio);
        }

        [Fact]
        public void DadoDTOComEmailComPonto_QuandoAtribuir_EntaoEmailComPontoNoFinalDeveSerSetado()
        {
            var dto = new EmailUsuarioDTO();
            var emailComPontoFinal = "usuario.@example.com";

            dto.Email = emailComPontoFinal;

            dto.Email.Should().Be(emailComPontoFinal);
        }

        [Fact]
        public void DadoDTOComEmailIntercalado_QuandoAtribuir_EntaoEmailComPontoIntercaladoDeveSerSetado()
        {
            var dto = new EmailUsuarioDTO();
            var emailComPontoIntercalado = "u.s.u.a.r.i.o@example.com";

            dto.Email = emailComPontoIntercalado;

            dto.Email.Should().Be(emailComPontoIntercalado);
        }

        [Fact]
        public void DadoDTOComTabelaReflection_QuandoAcessar_EntaoDeveRetornarPropriedadeEmail()
        {
            var tipo = typeof(EmailUsuarioDTO);
            var propriedade = tipo.GetProperty("Email");

            propriedade.Should().NotBeNull();
            propriedade.Name.Should().Be("Email");
            propriedade.CanRead.Should().BeTrue();
            propriedade.CanWrite.Should().BeTrue();
        }

        [Fact]
        public void DadoDTOComJsonSerialization_QuandoConverter_EntaoDeveSuportarEmailString()
        {
            var dto = new EmailUsuarioDTO { Email = "teste@example.com" };
            var dtoString = System.Text.Json.JsonSerializer.Serialize(dto);

            dtoString.Should().Contain("Email");
            dtoString.Should().Contain("teste@example.com");
        }

        [Fact]
        public void DadoDTOComJsonDeserialization_QuandoConverter_EntaoDeveRecuperarEmail()
        {
            var json = "{\"Email\":\"teste@example.com\"}";
            var dto = System.Text.Json.JsonSerializer.Deserialize<EmailUsuarioDTO>(json);

            dto.Should().NotBeNull();
            dto.Email.Should().Be("teste@example.com");
        }

        [Fact]
        public void DadoDTOComEmailNull_QuandoAlterar_EntaoDeveMantidoNull()
        {
            var dto = new EmailUsuarioDTO();
            dto.Email = null!;
            var emailAnterior = dto.Email;

            dto.Email = null!;

            dto.Email.Should().Be(emailAnterior);
            dto.Email.Should().BeNull();
        }
    }
}
