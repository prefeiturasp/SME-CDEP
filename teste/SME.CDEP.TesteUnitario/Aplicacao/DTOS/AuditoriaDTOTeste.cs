using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AuditoriaDtoTeste
    {
        [Fact]
        public void DadoParametroValido_QuandoInstanciarDTO_EntaoTodasAsPropriedadesPodemSerAtribuidas()
        {
            var criadoEm = new DateTime(2024, 5, 1, 10, 30, 0);
            var criadoPor = "Sistema de Teste";
            var criadoLogin = "usuario.teste";
            var alteradoEm = new DateTime(2024, 5, 2, 14, 45, 0);
            var alteradoPor = "Usuario Teste";
            var alteradoLogin = "usuario_teste";

            var dto = new AuditoriaDTO
            {
                CriadoEm = criadoEm,
                CriadoPor = criadoPor,
                CriadoLogin = criadoLogin,
                AlteradoEm = alteradoEm,
                AlteradoPor = alteradoPor,
                AlteradoLogin = alteradoLogin
            };

            dto.Should().NotBeNull();
            dto.CriadoEm.Should().Be(criadoEm);
            dto.CriadoPor.Should().Be(criadoPor);
            dto.CriadoLogin.Should().Be(criadoLogin);
            dto.AlteradoEm.Should().Be(alteradoEm);
            dto.AlteradoPor.Should().Be(alteradoPor);
            dto.AlteradoLogin.Should().Be(alteradoLogin);
        }

        [Fact]
        public void DadoCriadoEmComValor_QuandoInstanciarDTO_EntaoPropriedadeArmazenaCorretamente()
        {
            var criadoEm = new DateTime(2024, 1, 15, 09, 00, 0);

            var dto = new AuditoriaDTO { CriadoEm = criadoEm };

            dto.CriadoEm.Should().Be(criadoEm);
        }

        [Fact]
        public void DadoCriadoPorComValor_QuandoInstanciarDTO_EntaoPropriedadeArmazenaCorretamente()
        {
            var criadoPor = "Sistema de Teste";

            var dto = new AuditoriaDTO { CriadoPor = criadoPor };

            dto.CriadoPor.Should().Be(criadoPor);
        }

        [Fact]
        public void DadoCriadoLoginComValor_QuandoInstanciarDTO_EntaoPropriedadeArmazenaCorretamente()
        {
            var criadoLogin = "usuario.teste";

            var dto = new AuditoriaDTO { CriadoLogin = criadoLogin };

            dto.CriadoLogin.Should().Be(criadoLogin);
        }

        [Fact]
        public void DadoAlteradoEmComValor_QuandoInstanciarDTO_EntaoPropriedadeArmazenaCorretamente()
        {
            var alteradoEm = new DateTime(2024, 5, 10, 15, 30, 0);

            var dto = new AuditoriaDTO { AlteradoEm = alteradoEm };

            dto.AlteradoEm.Should().Be(alteradoEm);
        }

        [Fact]
        public void DadoAlteradoEmNulo_QuandoInstanciarDTO_EntaoPropriedadePermiteNulo()
        {
            var dto = new AuditoriaDTO { AlteradoEm = null };

            dto.AlteradoEm.Should().BeNull();
        }

        [Fact]
        public void DadoAlteradoPorComValor_QuandoInstanciarDTO_EntaoPropriedadeArmazenaCorretamente()
        {
            var alteradoPor = "Usuario Alteracao";

            var dto = new AuditoriaDTO { AlteradoPor = alteradoPor };

            dto.AlteradoPor.Should().Be(alteradoPor);
        }

        [Fact]
        public void DadoAlteradoPorNulo_QuandoInstanciarDTO_EntaoPropriedadePermiteNulo()
        {
            var dto = new AuditoriaDTO { AlteradoPor = null };

            dto.AlteradoPor.Should().BeNull();
        }

        [Fact]
        public void DadoAlteradoLoginComValor_QuandoInstanciarDTO_EntaoPropriedadeArmazenaCorretamente()
        {
            var alteradoLogin = "usuario.alteracao";

            var dto = new AuditoriaDTO { AlteradoLogin = alteradoLogin };

            dto.AlteradoLogin.Should().Be(alteradoLogin);
        }

        [Fact]
        public void DadoAlteradoLoginNulo_QuandoInstanciarDTO_EntaoPropriedadePermiteNulo()
        {
            var dto = new AuditoriaDTO { AlteradoLogin = null };

            dto.AlteradoLogin.Should().BeNull();
        }

        [Fact]
        public void DadoDTOSemValoresAtribuidos_QuandoInstanciar_EntaoPropriedadesTemValoresPadrao()
        {
            var dto = new AuditoriaDTO();

            dto.CriadoEm.Should().Be(default(DateTime));
            dto.CriadoPor.Should().BeNull();
            dto.CriadoLogin.Should().BeNull();
            dto.AlteradoEm.Should().BeNull();
            dto.AlteradoPor.Should().BeNull();
            dto.AlteradoLogin.Should().BeNull();
        }

        [Fact]
        public void DadoEntidadeAuditavelValida_QuandoConverterParaDTO_EntaoMapeiaCorretamente()
        {
            var criadoEm = new DateTime(2024, 3, 20, 10, 00, 0);
            var criadoPor = "Sistema";
            var criadoLogin = "sistema_123";
            var alteradoEm = new DateTime(2024, 3, 25, 14, 30, 0);
            var alteradoPor = "Usuario Teste";
            var alteradoLogin = "usuario.teste";

            var entidade = new EntidadeAuditoriaTesteFake
            {
                CriadoEm = criadoEm,
                CriadoPor = criadoPor,
                CriadoLogin = criadoLogin,
                AlteradoEm = alteradoEm,
                AlteradoPor = alteradoPor,
                AlteradoLogin = alteradoLogin
            };

            AuditoriaDTO dto = (AuditoriaDTO)entidade;

            dto.Should().NotBeNull();
            dto.CriadoEm.Should().Be(criadoEm);
            dto.CriadoPor.Should().Be(criadoPor);
            dto.CriadoLogin.Should().Be(criadoLogin);
            dto.AlteradoEm.Should().Be(alteradoEm);
            dto.AlteradoPor.Should().Be(alteradoPor);
            dto.AlteradoLogin.Should().Be(alteradoLogin);
        }

        [Fact]
        public void DadoEntidadeAuditavelComAlteracaoNula_QuandoConverterParaDTO_EntaoPropriedadesAlteracaoSaoNulas()
        {
            var criadoEm = new DateTime(2024, 3, 20, 10, 00, 0);
            var criadoPor = "Sistema";
            var criadoLogin = "sistema_123";

            var entidade = new EntidadeAuditoriaTesteFake
            {
                CriadoEm = criadoEm,
                CriadoPor = criadoPor,
                CriadoLogin = criadoLogin,
                AlteradoEm = null,
                AlteradoPor = null,
                AlteradoLogin = null
            };

            AuditoriaDTO dto = (AuditoriaDTO)entidade;

            dto.Should().NotBeNull();
            dto.CriadoEm.Should().Be(criadoEm);
            dto.CriadoPor.Should().Be(criadoPor);
            dto.CriadoLogin.Should().Be(criadoLogin);
            dto.AlteradoEm.Should().BeNull();
            dto.AlteradoPor.Should().BeNull();
            dto.AlteradoLogin.Should().BeNull();
        }

        [Fact]
        public void DadoEntidadeNula_QuandoConverterParaDTO_EntaoRetornaNulo()
        {
            EntidadeAuditoriaTesteFake entidade = null!;

            AuditoriaDTO dto = (AuditoriaDTO)entidade;

            dto.Should().BeNull();
        }

        [Fact]
        public void DadoDoisDTOsComMesmosValores_QuandoComparados_EntaoSaoEquivalentes()
        {
            var criadoEm = new DateTime(2024, 5, 1, 10, 30, 0);
            var criadoPor = "Sistema Teste";
            var criadoLogin = "sistema_teste";
            var alteradoEm = new DateTime(2024, 5, 2, 14, 45, 0);
            var alteradoPor = "Usuario Teste";
            var alteradoLogin = "usuario_teste";

            var dto1 = new AuditoriaDTO
            {
                CriadoEm = criadoEm,
                CriadoPor = criadoPor,
                CriadoLogin = criadoLogin,
                AlteradoEm = alteradoEm,
                AlteradoPor = alteradoPor,
                AlteradoLogin = alteradoLogin
            };

            var dto2 = new AuditoriaDTO
            {
                CriadoEm = criadoEm,
                CriadoPor = criadoPor,
                CriadoLogin = criadoLogin,
                AlteradoEm = alteradoEm,
                AlteradoPor = alteradoPor,
                AlteradoLogin = alteradoLogin
            };

            dto1.Should().BeEquivalentTo(dto2);
        }

        [Fact]
        public void DadoDTOInstanciado_QuandoAlterarPropriedades_EntaoNovasPropriedadesSaoArmazenadas()
        {
            var dto = new AuditoriaDTO
            {
                CriadoEm = new DateTime(2024, 1, 1),
                CriadoPor = "Usuario Original",
                CriadoLogin = "original",
                AlteradoEm = new DateTime(2024, 1, 2),
                AlteradoPor = "Alterado Original",
                AlteradoLogin = "alterado_original"
            };

            var novoAlteradoEm = new DateTime(2024, 6, 1);
            var novoAlteradoPor = "Usuario Novo";
            var novoAlteradoLogin = "novo";

            dto.AlteradoEm = novoAlteradoEm;
            dto.AlteradoPor = novoAlteradoPor;
            dto.AlteradoLogin = novoAlteradoLogin;

            dto.AlteradoEm.Should().Be(novoAlteradoEm);
            dto.AlteradoPor.Should().Be(novoAlteradoPor);
            dto.AlteradoLogin.Should().Be(novoAlteradoLogin);
            dto.CriadoEm.Should().Be(new DateTime(2024, 1, 1));
            dto.CriadoPor.Should().Be("Usuario Original");
            dto.CriadoLogin.Should().Be("original");
        }

        [Fact]
        public void DadoEntidadeComDadosCompletos_QuandoConverterParaDTO_EntaoTodosOsCamposSaoPreenchidos()
        {
            var entidade = new EntidadeAuditoriaTesteFake
            {
                CriadoEm = new DateTime(2024, 2, 14, 08, 30, 00),
                CriadoPor = "Admin",
                CriadoLogin = "admin_123",
                AlteradoEm = new DateTime(2024, 2, 28, 16, 45, 30),
                AlteradoPor = "Gerente",
                AlteradoLogin = "gerente_456"
            };

            AuditoriaDTO dto = (AuditoriaDTO)entidade;

            dto.CriadoEm.Year.Should().Be(2024);
            dto.CriadoEm.Month.Should().Be(2);
            dto.CriadoEm.Day.Should().Be(14);
            dto.CriadoEm.Hour.Should().Be(8);
            dto.CriadoEm.Minute.Should().Be(30);
            dto.CriadoPor.Should().Be("Admin");
            dto.CriadoLogin.Should().Be("admin_123");
            dto.AlteradoEm!.Value.Year.Should().Be(2024);
            dto.AlteradoEm.Value.Month.Should().Be(2);
            dto.AlteradoEm.Value.Day.Should().Be(28);
            dto.AlteradoEm.Value.Hour.Should().Be(16);
            dto.AlteradoEm.Value.Minute.Should().Be(45);
            dto.AlteradoPor.Should().Be("Gerente");
            dto.AlteradoLogin.Should().Be("gerente_456");
        }

        [Fact]
        public void DadoDTOComPropriedadesAuditoria_QuandoVerificarTipo_EntaoEhDaTipoAuditoriaDTO()
        {
            var dto = new AuditoriaDTO();

            dto.Should().BeOfType<AuditoriaDTO>();
        }

        [Fact]
        public void DadoCriadoPorComValorVazio_QuandoInstanciarDTO_EntaoArmazenaVazio()
        {
            var criadoPor = string.Empty;

            var dto = new AuditoriaDTO { CriadoPor = criadoPor };

            dto.CriadoPor.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoCriadoLoginComValorVazio_QuandoInstanciarDTO_EntaoArmazenaVazio()
        {
            var criadoLogin = string.Empty;

            var dto = new AuditoriaDTO { CriadoLogin = criadoLogin };

            dto.CriadoLogin.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoAlteradoPorComValorVazio_QuandoInstanciarDTO_EntaoArmazenaVazio()
        {
            var alteradoPor = string.Empty;

            var dto = new AuditoriaDTO { AlteradoPor = alteradoPor };

            dto.AlteradoPor.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoAlteradoLoginComValorVazio_QuandoInstanciarDTO_EntaoArmazenaVazio()
        {
            var alteradoLogin = string.Empty;

            var dto = new AuditoriaDTO { AlteradoLogin = alteradoLogin };

            dto.AlteradoLogin.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoCriadoPorComCaracteresEspeciais_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var criadoPor = "José da Silva São Paulo";

            var dto = new AuditoriaDTO { CriadoPor = criadoPor };

            dto.CriadoPor.Should().Be(criadoPor);
        }

        [Fact]
        public void DadoAlteradoPorComCaracteresEspeciais_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var alteradoPor = "Maria Ção da Costa";

            var dto = new AuditoriaDTO { AlteradoPor = alteradoPor };

            dto.AlteradoPor.Should().Be(alteradoPor);
        }

        [Fact]
        public void DadoCriadoLoginComCaracteresEspeciais_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var criadoLogin = "usuario.nome_teste-123";

            var dto = new AuditoriaDTO { CriadoLogin = criadoLogin };

            dto.CriadoLogin.Should().Be(criadoLogin);
        }

        [Fact]
        public void DadoAlteradoLoginComCaracteresEspeciais_QuandoInstanciarDTO_EntaoArmazenaCorretamente()
        {
            var alteradoLogin = "usuario.alteracao-456";

            var dto = new AuditoriaDTO { AlteradoLogin = alteradoLogin };

            dto.AlteradoLogin.Should().Be(alteradoLogin);
        }

        [Fact]
        public void DadoEntidadeComAlteracaoCompleta_QuandoConverterParaDTO_EntaoMapeiaTodosOsDados()
        {
            var entidade = new EntidadeAuditoriaTesteFake
            {
                CriadoEm = DateTime.MinValue.AddYears(1),
                CriadoPor = "Criador",
                CriadoLogin = "criador",
                AlteradoEm = DateTime.MaxValue.AddYears(-1),
                AlteradoPor = "Alterador",
                AlteradoLogin = "alterador"
            };

            AuditoriaDTO dto = (AuditoriaDTO)entidade;

            dto.Should().NotBeNull();
            dto.CriadoEm.Should().Be(DateTime.MinValue.AddYears(1));
            dto.AlteradoEm.Should().Be(DateTime.MaxValue.AddYears(-1));
        }

        [Theory]
        [InlineData("admin")]
        [InlineData("user_123")]
        [InlineData("teste@sistema")]
        [InlineData("login.completo")]
        public void DadoCriadoLoginComValoresVariados_QuandoInstanciarDTO_EntaoArmazenaCorretamente(string criadoLogin)
        {
            var dto = new AuditoriaDTO { CriadoLogin = criadoLogin };

            dto.CriadoLogin.Should().Be(criadoLogin);
        }

        [Theory]
        [InlineData("admin")]
        [InlineData("user_123")]
        [InlineData("alteracao@sistema")]
        [InlineData("login.completo")]
        public void DadoAlteradoLoginComValoresVariados_QuandoInstanciarDTO_EntaoArmazenaCorretamente(string alteradoLogin)
        {
            var dto = new AuditoriaDTO { AlteradoLogin = alteradoLogin };

            dto.AlteradoLogin.Should().Be(alteradoLogin);
        }

        [Fact]
        public void DadoMultiplosConversionDeEntidades_QuandoConverterParaDTO_EntaoSempreMapeiaCertamente()
        {
            var entidade1 = new EntidadeAuditoriaTesteFake
            {
                CriadoEm = new DateTime(2024, 1, 1),
                CriadoPor = "User1",
                CriadoLogin = "user1",
                AlteradoEm = null,
                AlteradoPor = null,
                AlteradoLogin = null
            };

            var entidade2 = new EntidadeAuditoriaTesteFake
            {
                CriadoEm = new DateTime(2024, 6, 15),
                CriadoPor = "User2",
                CriadoLogin = "user2",
                AlteradoEm = new DateTime(2024, 6, 20),
                AlteradoPor = "User2Updated",
                AlteradoLogin = "user2_updated"
            };

            var dto1 = (AuditoriaDTO)entidade1;
            var dto2 = (AuditoriaDTO)entidade2;

            dto1.CriadoPor.Should().Be("User1");
            dto1.AlteradoEm.Should().BeNull();
            dto2.CriadoPor.Should().Be("User2");
            dto2.AlteradoEm.Should().NotBeNull();
            dto1.Should().NotBeEquivalentTo(dto2);
        }
        private class EntidadeAuditoriaTesteFake : EntidadeBaseAuditavel
        {
            public new long Id { get; set; } = 0L;
        }
    }
}
