using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AutenticacaoRevalidarDtoTeste
    {
        [Fact]
        public void DadoTokenValido_QuandoInstanciarDTO_EntaoPropriedadeArmazenaCorretamente()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";

            var dto = new AutenticacaoRevalidarDTO { Token = token };

            dto.Token.Should().Be(token);
        }

        [Fact]
        public void DadoTokenComValor_QuandoInstanciarDTO_EntaoPropriedadeTokenPermiteAtribuicao()
        {
            var token = "token_revalidacao_123";

            var dto = new AutenticacaoRevalidarDTO
            {
                Token = token
            };

            dto.Should().NotBeNull();
            dto.Token.Should().Be(token);
        }

        [Fact]
        public void DadoTokenNulo_QuandoInstanciarDTO_EntaoPropriedadeTokenPermiteNulo()
        {
            var dto = new AutenticacaoRevalidarDTO { Token = null! };

            dto.Token.Should().BeNull();
        }

        [Fact]
        public void DadoTokenVazio_QuandoInstanciarDTO_EntaoPropriedadeTokenArmazenaVazio()
        {
            var dto = new AutenticacaoRevalidarDTO { Token = string.Empty };

            dto.Token.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoDTOSemValoresAtribuidos_QuandoInstanciar_EntaoPropriedadeTokenTemValorPadrao()
        {
            var dto = new AutenticacaoRevalidarDTO();

            dto.Token.Should().BeNull();
        }

        [Fact]
        public void DadoDoisDTOsComMesmoToken_QuandoComparados_EntaoSaoEquivalentes()
        {
            var token = "token_identico_para_teste";

            var dto1 = new AutenticacaoRevalidarDTO { Token = token };
            var dto2 = new AutenticacaoRevalidarDTO { Token = token };

            dto1.Should().BeEquivalentTo(dto2);
        }

        [Fact]
        public void DadoDTOInstanciado_QuandoAlterarPropriedadeToken_EntaoNovoTokenEhArmazenado()
        {
            var dto = new AutenticacaoRevalidarDTO { Token = "token_original" };
            var novoToken = "token_novo";

            dto.Token = novoToken;

            dto.Token.Should().Be(novoToken);
        }

        [Fact]
        public void DadoDTOComTokenNulo_QuandoAlterarParaValorValido_EntaoNovoTokenEhArmazenado()
        {
            var dto = new AutenticacaoRevalidarDTO { Token = null! };
            var novoToken = "token_novo_atribuido";

            dto.Token = novoToken;

            dto.Token.Should().Be(novoToken);
        }

        [Fact]
        public void DadoTokenComCaracteresEspeciais_QuandoInstanciarDTO_EntaoPropriedadeTokenArmazenaCorretamente()
        {
            var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJjbGFpbXMiOlsiYWRtaW4iLCJ1c2VyIl19.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

            var dto = new AutenticacaoRevalidarDTO { Token = token };

            dto.Token.Should().Be(token);
        }

        [Fact]
        public void DadoTokenComEspacos_QuandoInstanciarDTO_EntaoPropriedadeTokenMantemEspacos()
        {
            var token = "   token_com_espacos   ";

            var dto = new AutenticacaoRevalidarDTO { Token = token };

            dto.Token.Should().Be(token);
        }

        [Fact]
        public void DadoDTOInstanciado_QuandoVerificarTipo_EntaoEhDaTipoAutenticacaoRevalidarDTO()
        {
            var dto = new AutenticacaoRevalidarDTO();

            dto.Should().BeOfType<AutenticacaoRevalidarDTO>();
        }

        [Theory]
        [InlineData("token1")]
        [InlineData("token_teste_123")]
        [InlineData("abc123def456")]
        [InlineData("token-com-hifen")]
        [InlineData("token_com_underscore")]
        public void DadoTokenComValoresVariados_QuandoInstanciarDTO_EntaoArmazenaCorretamente(string tokenValor)
        {
            var dto = new AutenticacaoRevalidarDTO { Token = tokenValor };

            dto.Token.Should().Be(tokenValor);
        }

        [Fact]
        public void DadoDTOComTokenGrande_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var tokenGrande = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

            var dto = new AutenticacaoRevalidarDTO { Token = tokenGrande };

            dto.Token.Should().Be(tokenGrande);
        }

        [Fact]
        public void DadoDTOInstanciado_QuandoAlterarTokenVazioParaComValor_EntaoNovoValorEhArmazenado()
        {
            var dto = new AutenticacaoRevalidarDTO { Token = string.Empty };

            dto.Token = "novo_token";

            dto.Token.Should().Be("novo_token");
        }

        [Fact]
        public void DadoDTOComToken_QuandoAlterarParaVazio_EntaoValorVazioEhArmazenado()
        {
            var dto = new AutenticacaoRevalidarDTO { Token = "token_original" };

            dto.Token = string.Empty;

            dto.Token.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoDTOComToken_QuandoAlterarParaNulo_EntaoValorNuloEhArmazenado()
        {
            var dto = new AutenticacaoRevalidarDTO { Token = "token_original" };

            dto.Token = null!;

            dto.Token.Should().BeNull();
        }

        [Fact]
        public void DadoTokenComCaracteresAcentuados_QuandoInstanciarDTO_EntaoPropriedadeTokenArmazenaCorretamente()
        {
            var token = "token_têste_ñoño";

            var dto = new AutenticacaoRevalidarDTO { Token = token };

            dto.Token.Should().Be(token);
        }

        [Fact]
        public void DadoMultiplosAutoenticacaoRevalidarDTOs_QuandoVerificarIndependencia_EntaoCadaDTOMantemSeusOwnTokens()
        {
            var dto1 = new AutenticacaoRevalidarDTO { Token = "token_1" };
            var dto2 = new AutenticacaoRevalidarDTO { Token = "token_2" };

            dto1.Token.Should().Be("token_1");
            dto2.Token.Should().Be("token_2");
            dto1.Should().NotBeEquivalentTo(dto2);
        }

        [Fact]
        public void DadoTokenComCaracteresNumericosEAlfabeticos_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var token = "token_abc123xyz456";

            var dto = new AutenticacaoRevalidarDTO { Token = token };

            dto.Token.Should().Be(token);
        }

        [Fact]
        public void DadoDTOComTokenMinimo_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var tokenMinimo = "a";

            var dto = new AutenticacaoRevalidarDTO { Token = tokenMinimo };

            dto.Token.Should().HaveLength(1).And.Be(tokenMinimo);
        }

        [Fact]
        public void DadoAutenticacaoRevalidarDTOComTodasAsPropriedades_QuandoAcessarPropriedades_EntaoValoresArmazenados()
        {
            var token = "token_final_teste";

            var dto = new AutenticacaoRevalidarDTO
            {
                Token = token
            };

            dto.Should().NotBeNull();
            dto.Token.Should().Be(token);
            dto.Should().BeOfType<AutenticacaoRevalidarDTO>();
        }

        [Fact]
        public void DadoTresAutenticacaoRevalidarDTOsDiferentes_QuandoComparar_EntaoSaoIdentificadasAsDiferencas()
        {
            var dto1 = new AutenticacaoRevalidarDTO { Token = "token_1" };
            var dto2 = new AutenticacaoRevalidarDTO { Token = "token_2" };
            var dto3 = new AutenticacaoRevalidarDTO { Token = "token_1" };

            dto1.Should().NotBeEquivalentTo(dto2);
            dto1.Should().BeEquivalentTo(dto3);
            dto2.Should().NotBeEquivalentTo(dto3);
        }

        [Fact]
        public void DadoTokenComUmCaracter_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var token = "t";

            var dto = new AutenticacaoRevalidarDTO { Token = token };

            dto.Token.Should().HaveLength(1).And.Be(token);
        }

        [Fact]
        public void DadoAutenticacaoRevalidarDTOInstanciada_QuandoAlterarTokenMultiplaVezes_EntaoUltimoValorEhArmazenado()
        {
            var dto = new AutenticacaoRevalidarDTO();

            dto.Token = "token_1";
            dto.Token.Should().Be("token_1");

            dto.Token = "token_2";
            dto.Token.Should().Be("token_2");

            dto.Token = "token_3";
            dto.Token.Should().Be("token_3");
        }

        [Fact]
        public void DadoDTOComTokenNuloEVazio_QuandoAlteracoes_EntaoPropriedadePermiteTransicoes()
        {
            var dto = new AutenticacaoRevalidarDTO { Token = null! };

            dto.Token.Should().BeNull();

            dto.Token = string.Empty;
            dto.Token.Should().Be(string.Empty);

            dto.Token = "token_final";
            dto.Token.Should().Be("token_final");

            dto.Token = null!;
            dto.Token.Should().BeNull();
        }

        [Fact]
        public void DadoAutenticacaoRevalidarDTOComToken_QuandoVerificarPropriedadeToken_EntaoTemAtributoRequired()
        {
            var propriedade = typeof(AutenticacaoRevalidarDTO).GetProperty(nameof(AutenticacaoRevalidarDTO.Token));

            propriedade.Should().NotBeNull();
            var atributosRequired = propriedade.GetCustomAttributes(typeof(RequiredAttribute), false);
            atributosRequired.Should().HaveCount(1);
            atributosRequired[0].Should().BeOfType<RequiredAttribute>();
        }

        [Fact]
        public void DadoAtributoRequired_QuandoVerificarMensagemDeErro_EntaoMensagemEhCorreta()
        {
            var propriedade = typeof(AutenticacaoRevalidarDTO).GetProperty(nameof(AutenticacaoRevalidarDTO.Token))!;
            var atributoRequired = (RequiredAttribute)propriedade.GetCustomAttributes(typeof(RequiredAttribute), false)[0];

            atributoRequired.ErrorMessage.Should().Be("Informe o token para revalidar");
        }

        [Fact]
        public void DadoDTOInstanciado_QuandoVerificarPropriedadeToken_EntaoExisteEhPublica()
        {
            var propriedade = typeof(AutenticacaoRevalidarDTO).GetProperty(nameof(AutenticacaoRevalidarDTO.Token));

            propriedade.Should().NotBeNull();
            propriedade.CanRead.Should().BeTrue();
            propriedade.CanWrite.Should().BeTrue();
        }

        [Fact]
        public void DadoTokenComSomentePontos_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var token = "...";

            var dto = new AutenticacaoRevalidarDTO { Token = token };

            dto.Token.Should().Be(token);
        }

        [Fact]
        public void DadoTokenComBarrasECifrao_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var token = "/token/$/special\\";

            var dto = new AutenticacaoRevalidarDTO { Token = token };

            dto.Token.Should().Be(token);
        }

        [Fact]
        public void DadoDTOComTokenVazio_QuandoAcessarPropriedade_EntaoRetornaStringVazia()
        {
            var dto = new AutenticacaoRevalidarDTO { Token = string.Empty };

            dto.Token.Should().NotBeNull();
            dto.Token.Should().BeEmpty();
        }

        [Fact]
        public void DadoAutenticacaoRevalidarDTOComValor_QuandoInstanciarSemParametros_EntaoPropriedadeEhNula()
        {
            // Arrange & Act
            var dto = new AutenticacaoRevalidarDTO();

            // Assert
            dto.Token.Should().BeNull("ao instanciar sem parâmetros, Token deve ser nulo por padrão");
            dto.Should().BeOfType<AutenticacaoRevalidarDTO>("a instância deve ser do tipo AutenticacaoRevalidarDTO");
        }

        [Fact]
        public void DadoTokenComJWT_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var tokenJwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.TJVA95OrM7E2cBab30RMHrHDcEfxjoYZgeFONFh7HgQ";

            var dto = new AutenticacaoRevalidarDTO { Token = tokenJwt };

            dto.Token.Should().Be(tokenJwt);
            dto.Token.Should().Contain(".");
        }
    }
}
