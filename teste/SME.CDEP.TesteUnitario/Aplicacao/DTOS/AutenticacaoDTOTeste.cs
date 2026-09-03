using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AutenticacaoDtoTeste
    {
        [Fact]
        public void DadoParametroValido_QuandoInstanciarDTO_EntaoTodasAsPropriedadesPodemSerAtribuidas()
        {
            var login = "usuario_teste";
            var senha = "senha123";

            var dto = new AutenticacaoDTO
            {
                Login = login,
                Senha = senha
            };

            dto.Should().NotBeNull();
            dto.Login.Should().Be(login);
            dto.Senha.Should().Be(senha);
        }

        [Fact]
        public void DadoLoginComValor_QuandoInstanciarDTO_EntaoPropriedadeLoginArmazenaCorretamente()
        {
            var login = "usuario@sistema";

            var dto = new AutenticacaoDTO { Login = login };

            dto.Login.Should().Be(login);
        }

        [Fact]
        public void DadoLoginNulo_QuandoInstanciarDTO_EntaoPropriedadeLoginPermiteNulo()
        {
            var dto = new AutenticacaoDTO { Login = null! };

            dto.Login.Should().BeNull();
        }

        [Fact]
        public void DadoLoginVazio_QuandoInstanciarDTO_EntaoPropriedadeLoginArmazenaVazio()
        {
            var dto = new AutenticacaoDTO { Login = string.Empty };

            dto.Login.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoSenhaComValor_QuandoInstanciarDTO_EntaoPropriedadeSenhaArmazenaCorretamente()
        {
            var senha = "Abc@1234";

            var dto = new AutenticacaoDTO { Senha = senha };

            dto.Senha.Should().Be(senha);
        }

        [Fact]
        public void DadoSenhaNula_QuandoInstanciarDTO_EntaoPropriedadeSenhaPermiteNula()
        {
            var dto = new AutenticacaoDTO { Senha = null! };

            dto.Senha.Should().BeNull();
        }

        [Fact]
        public void DadoSenhaVazia_QuandoInstanciarDTO_EntaoPropriedadeSenhaArmazenaVazia()
        {
            var dto = new AutenticacaoDTO { Senha = string.Empty };

            dto.Senha.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoDTOSemValoresAtribuidos_QuandoInstanciar_EntaoPropriedadesTemValoresPadrao()
        {
            var dto = new AutenticacaoDTO();

            dto.Login.Should().BeNull();
            dto.Senha.Should().BeNull();
        }

        [Fact]
        public void DadoDoisDTOsComMesmosValores_QuandoComparados_EntaoSaoEquivalentes()
        {
            var login = "teste_usuario";
            var senha = "senha_teste";

            var dto1 = new AutenticacaoDTO { Login = login, Senha = senha };
            var dto2 = new AutenticacaoDTO { Login = login, Senha = senha };

            dto1.Should().BeEquivalentTo(dto2);
        }

        [Fact]
        public void DadoDTOInstanciado_QuandoAlterarPropriedades_EntaoNovasPropriedadesSaoArmazenadas()
        {
            var dto = new AutenticacaoDTO { Login = "original", Senha = "senha_original" };
            var novoLogin = "novo_usuario";
            var novaSenha = "nova_senha";

            dto.Login = novoLogin;
            dto.Senha = novaSenha;

            dto.Login.Should().Be(novoLogin);
            dto.Senha.Should().Be(novaSenha);
        }

        [Fact]
        public void DadoDTOComPropriedadesNulas_QuandoAlterarParaValoresValidos_EntaoPropriedadesArmazenamCorretamente()
        {
            var dto = new AutenticacaoDTO { Login = null!, Senha = null! };
            var novoLogin = "usuario_novo";
            var novaSenha = "senha_nova";

            dto.Login = novoLogin;
            dto.Senha = novaSenha;

            dto.Login.Should().Be(novoLogin);
            dto.Senha.Should().Be(novaSenha);
        }

        [Fact]
        public void DadoLoginComCaracteresEspeciais_QuandoInstanciarDTO_EntaoPropriedadeLoginArmazenaCorretamente()
        {
            var login = "usuario.nome-teste_123@dominio";

            var dto = new AutenticacaoDTO { Login = login };

            dto.Login.Should().Be(login);
        }

        [Fact]
        public void DadoSenhaComCaracteresEspeciais_QuandoInstanciarDTO_EntaoPropriedadeSenhaArmazenaCorretamente()
        {
            var senha = "P@ss!word#2024&";

            var dto = new AutenticacaoDTO { Senha = senha };

            dto.Senha.Should().Be(senha);
        }

        [Fact]
        public void DadoLoginComEspacos_QuandoInstanciarDTO_EntaoPropriedadeLoginMantemEspacos()
        {
            var login = "   usuario com espacos   ";

            var dto = new AutenticacaoDTO { Login = login };

            dto.Login.Should().Be(login);
        }

        [Fact]
        public void DadoSenhaComEspacos_QuandoInstanciarDTO_EntaoPropriedadeSenhaMantemEspacos()
        {
            var senha = "   senha com espacos   ";

            var dto = new AutenticacaoDTO { Senha = senha };

            dto.Senha.Should().Be(senha);
        }

        [Fact]
        public void DadoDTOInstanciado_QuandoVerificarTipo_EntaoEhDaTipoAutenticacaoDTO()
        {
            var dto = new AutenticacaoDTO();

            dto.Should().BeOfType<AutenticacaoDTO>();
        }

        [Theory]
        [InlineData("usuario1")]
        [InlineData("user_123")]
        [InlineData("teste@sistema")]
        [InlineData("login.completo")]
        [InlineData("user")]
        public void DadoLoginComValoresVariados_QuandoInstanciarDTO_EntaoArmazenaCorretamente(string loginValor)
        {
            var dto = new AutenticacaoDTO { Login = loginValor };

            dto.Login.Should().Be(loginValor);
        }

        [Theory]
        [InlineData("1234")]
        [InlineData("senha")]
        [InlineData("Abc@1234")]
        [InlineData("password_123")]
        [InlineData("test")]
        public void DadoSenhaComValoresVariados_QuandoInstanciarDTO_EntaoArmazenaCorretamente(string senhaValor)
        {
            var dto = new AutenticacaoDTO { Senha = senhaValor };

            dto.Senha.Should().Be(senhaValor);
        }

        [Fact]
        public void DadoMultiplosDTOsInstanciados_QuandoVerificarIndependencia_EntaoCadaDTOMantemSeusPropriosValores()
        {
            var dto1 = new AutenticacaoDTO { Login = "usuario1", Senha = "senha1" };
            var dto2 = new AutenticacaoDTO { Login = "usuario2", Senha = "senha2" };

            dto1.Login.Should().Be("usuario1");
            dto2.Login.Should().Be("usuario2");
            dto1.Senha.Should().Be("senha1");
            dto2.Senha.Should().Be("senha2");
            dto1.Should().NotBeEquivalentTo(dto2);
        }

        [Fact]
        public void DadoDTOComLoginMinimo_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var loginMinimo = "abcde";

            var dto = new AutenticacaoDTO { Login = loginMinimo };

            dto.Login.Should().HaveLength(5).And.Be(loginMinimo);
        }

        [Fact]
        public void DadoDTOComSenhaMinima_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var senhaMinima = "abcd";

            var dto = new AutenticacaoDTO { Senha = senhaMinima };

            dto.Senha.Should().HaveLength(4).And.Be(senhaMinima);
        }

        [Fact]
        public void DadoDTOComLoginMaior_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var loginGrande = "usuario_teste_com_nome_bem_grande_123456";

            var dto = new AutenticacaoDTO { Login = loginGrande };

            dto.Login.Should().Be(loginGrande);
        }

        [Fact]
        public void DadoDTOComSenhaMaior_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var senhaGrande = "senha_super_segura_com_muitos_caracteres_2024!";

            var dto = new AutenticacaoDTO { Senha = senhaGrande };

            dto.Senha.Should().Be(senhaGrande);
        }

        [Fact]
        public void DadoAutenticacaoDTOComTodasAsPropriedades_QuandoAcessarPropriedades_EntaoValoresSaoCorretos()
        {
            var login = "usuario_final";
            var senha = "senha_final";

            var dto = new AutenticacaoDTO
            {
                Login = login,
                Senha = senha
            };

            dto.Should().NotBeNull();
            dto.Login.Should().Be(login);
            dto.Senha.Should().Be(senha);
            dto.Should().BeOfType<AutenticacaoDTO>();
        }

        [Fact]
        public void DadoLoginComOnzeCaracteres_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var login = "usuario_test";

            var dto = new AutenticacaoDTO { Login = login };

            dto.Login.Should().HaveLength(login.Length).And.Be(login);
        }

        [Fact]
        public void DadoSenhaComDezCaracteres_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var senha = "senha123456";

            var dto = new AutenticacaoDTO { Senha = senha };

            dto.Senha.Should().HaveLength(senha.Length).And.Be(senha);
        }

        [Fact]
        public void DadoAutenticacaoDTOInstanciada_QuandoAlterarLoginVazioParaComValor_EntaoNovoValorEhArmazenado()
        {
            var dto = new AutenticacaoDTO { Login = string.Empty };

            dto.Login = "novo_usuario";

            dto.Login.Should().Be("novo_usuario");
        }

        [Fact]
        public void DadoAutenticacaoDTOInstanciada_QuandoAlterarSenhaVaziaParaComValor_EntaoNovoValorEhArmazenado()
        {
            var dto = new AutenticacaoDTO { Senha = string.Empty };

            dto.Senha = "nova_senha";

            dto.Senha.Should().Be("nova_senha");
        }

        [Fact]
        public void DadoAutenticacaoDTOComValores_QuandoAlterarParaVazio_EntaoValorVazioEhArmazenado()
        {
            var dto = new AutenticacaoDTO { Login = "usuario", Senha = "senha" };

            dto.Login = string.Empty;
            dto.Senha = string.Empty;

            dto.Login.Should().Be(string.Empty);
            dto.Senha.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoAutenticacaoDTOComValores_QuandoAlterarParaNulo_EntaoValorNuloEhArmazenado()
        {
            var dto = new AutenticacaoDTO { Login = "usuario", Senha = "senha" };

            dto.Login = null!;
            dto.Senha = null!;

            dto.Login.Should().BeNull();
            dto.Senha.Should().BeNull();
        }

        [Fact]
        public void DadoLoginComCaracteresAcentuados_QuandoInstanciarDTO_EntaoPropriedadeLoginArmazenaCorretamente()
        {
            var login = "usuário_tëste";

            var dto = new AutenticacaoDTO { Login = login };

            dto.Login.Should().Be(login);
        }

        [Fact]
        public void DadoSenhaComCaracteresAcentuados_QuandoInstanciarDTO_EntaoPropriedadeSenhaArmazenaCorretamente()
        {
            var senha = "sënhä_tëste";

            var dto = new AutenticacaoDTO { Senha = senha };

            dto.Senha.Should().Be(senha);
        }

        [Fact]
        public void DadoTresAutenticacoesDTODiferentes_QuandoComparar_EntaoSaoIdentificadasAsDiferencas()
        {
            var dto1 = new AutenticacaoDTO { Login = "user1", Senha = "pass1" };
            var dto2 = new AutenticacaoDTO { Login = "user2", Senha = "pass2" };
            var dto3 = new AutenticacaoDTO { Login = "user1", Senha = "pass2" };

            dto1.Should().NotBeEquivalentTo(dto2);
            dto1.Should().NotBeEquivalentTo(dto3);
            dto2.Should().NotBeEquivalentTo(dto3);
        }

        [Fact]
        public void DadoAutenticacaoDTOComLoginNumerico_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var login = "12345";

            var dto = new AutenticacaoDTO { Login = login };

            dto.Login.Should().Be(login);
        }

        [Fact]
        public void DadoAutenticacaoDTOComSenhaNumerico_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var senha = "1234";

            var dto = new AutenticacaoDTO { Senha = senha };

            dto.Senha.Should().Be(senha);
        }

        [Fact]
        public void DadoAutenticacaoDTOInstanciada_QuandoVerificarPropriedadesComValores_EntaoTodasSaoPreenchidas()
        {
            var login = "usuario_completo";
            var senha = "senha_segura_123";

            var dto = new AutenticacaoDTO
            {
                Login = login,
                Senha = senha
            };

            dto.Login.Should().NotBeNullOrEmpty().And.Be(login);
            dto.Senha.Should().NotBeNullOrEmpty().And.Be(senha);
        }
    }
}
