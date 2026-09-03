using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class BaseAuditavelDTOTeste
    {
        private readonly Faker<AuditavelDTOImplementacao> faker;

        public BaseAuditavelDTOTeste()
        {
            faker = new Faker<AuditavelDTOImplementacao>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.Excluido, f => f.Random.Bool())
                .RuleFor(x => x.AlteradoEm, f => f.Date.Recent())
                .RuleFor(x => x.AlteradoPor, f => f.Person.FullName)
                .RuleFor(x => x.AlteradoLogin, f => f.Internet.UserName())
                .RuleFor(x => x.CriadoEm, f => f.Date.Past())
                .RuleFor(x => x.CriadoPor, f => f.Person.FullName)
                .RuleFor(x => x.CriadoLogin, f => f.Internet.UserName());
        }

        [Fact]
        public void DadoDTOValido_QuandoInstanciado_EntaoTodosAtributosDevemSerPreenchidos()
        {
            var id = 123L;
            var excluido = false;
            var alteradoEm = DateTime.Now.AddDays(-1);
            var alteradoPor = "João Silva";
            var alteradoLogin = "joao.silva";
            var criadoEm = DateTime.Now.AddDays(-10);
            var criadoPor = "Sistema";
            var criadoLogin = "sistema";

            var dto = new AuditavelDTOImplementacao
            {
                Id = id,
                Excluido = excluido,
                AlteradoEm = alteradoEm,
                AlteradoPor = alteradoPor,
                AlteradoLogin = alteradoLogin,
                CriadoEm = criadoEm,
                CriadoPor = criadoPor,
                CriadoLogin = criadoLogin
            };

            dto.Id.Should().Be(id);
            dto.Excluido.Should().Be(excluido);
            dto.AlteradoEm.Should().Be(alteradoEm);
            dto.AlteradoPor.Should().Be(alteradoPor);
            dto.AlteradoLogin.Should().Be(alteradoLogin);
            dto.CriadoEm.Should().Be(criadoEm);
            dto.CriadoPor.Should().Be(criadoPor);
            dto.CriadoLogin.Should().Be(criadoLogin);
        }

        [Fact]
        public void DadoDTOComAlteradoEmNulo_QuandoInstanciado_EntaoAlteradoEmDeveSerNulo()
        {
            var dto = new AuditavelDTOImplementacao
            {
                Id = 1L,
                Excluido = false,
                AlteradoEm = null,
                AlteradoPor = "João",
                AlteradoLogin = "joao",
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoLogin = "sistema"
            };

            dto.AlteradoEm.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComAlteradoPorNulo_QuandoInstanciado_EntaoAlteradoPorDeveSerNulo()
        {
            var dto = new AuditavelDTOImplementacao
            {
                Id = 1L,
                Excluido = false,
                AlteradoEm = DateTime.Now,
                AlteradoPor = null,
                AlteradoLogin = "joao",
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoLogin = "sistema"
            };

            dto.AlteradoPor.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComAlteradoLoginNulo_QuandoInstanciado_EntaoAlteradoLoginDeveSerNulo()
        {
            var dto = new AuditavelDTOImplementacao
            {
                Id = 1L,
                Excluido = false,
                AlteradoEm = DateTime.Now,
                AlteradoPor = "João",
                AlteradoLogin = null,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoLogin = "sistema"
            };

            dto.AlteradoLogin.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComExcluido_QuandoVerificarAtributoExcluido_EntaoDeveRetornarTrue()
        {
            var dto = new AuditavelDTOImplementacao
            {
                Id = 1L,
                Excluido = true,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoLogin = "sistema"
            };

            dto.Excluido.Should().BeTrue();
        }

        [Fact]
        public void DadoDTONaoExcluido_QuandoVerificarAtributoExcluido_EntaoDeveRetornarFalse()
        {
            var dto = new AuditavelDTOImplementacao
            {
                Id = 1L,
                Excluido = false,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoLogin = "sistema"
            };

            dto.Excluido.Should().BeFalse();
        }

        [Fact]
        public void DadoDTOCriadoComBogus_QuandoInstanciado_EntaoTodosAtributosDevemSerValidados()
        {
            var dto = faker.Generate();

            dto.Id.Should().BeGreaterThanOrEqualTo(1);
            dto.CriadoEm.Should().BeBefore(DateTime.UtcNow);
            dto.CriadoPor.Should().NotBeNullOrWhiteSpace();
            dto.CriadoLogin.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void DadoMultiplosDTOs_QuandoComparadosValores_EntaoDevemConterPropriedadesDistintas()
        {
            var dto1 = faker.Generate();
            var dto2 = faker.Generate();

            dto1.Id.Should().NotBe(dto2.Id);
        }

        [Fact]
        public void DadoDTOComIdMaximo_QuandoInstanciado_EntaoIdDeveSerPreservado()
        {
            var idMaximo = long.MaxValue;
            var dto = new AuditavelDTOImplementacao
            {
                Id = idMaximo,
                Excluido = false,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoLogin = "sistema"
            };

            dto.Id.Should().Be(idMaximo);
        }

        [Fact]
        public void DadoDTOComIdZero_QuandoInstanciado_EntaoIdDeveSerZero()
        {
            var dto = new AuditavelDTOImplementacao
            {
                Id = 0,
                Excluido = false,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoLogin = "sistema"
            };

            dto.Id.Should().Be(0);
        }

        [Fact]
        public void DadoDTOComDatasDistintas_QuandoComparado_EntaoCriadoEmDeveSerAntesDeAlteradoEm()
        {
            var dataCriacao = DateTime.Now.AddDays(-10);
            var dataAlteracao = DateTime.Now.AddDays(-1);

            var dto = new AuditavelDTOImplementacao
            {
                Id = 1L,
                Excluido = false,
                CriadoEm = dataCriacao,
                CriadoPor = "Sistema",
                CriadoLogin = "sistema",
                AlteradoEm = dataAlteracao,
                AlteradoPor = "João",
                AlteradoLogin = "joao"
            };
            
            dto.CriadoEm.Should().BeBefore(dto.AlteradoEm.Value);
        }

        [Fact]
        public void DadoDTOComStringVazia_QuandoInstanciado_EntaoStringDeveSerVazia()
        {
            var dto = new AuditavelDTOImplementacao
            {
                Id = 1L,
                Excluido = false,
                AlteradoEm = null,
                AlteradoPor = string.Empty,
                AlteradoLogin = string.Empty,
                CriadoEm = DateTime.Now,
                CriadoPor = string.Empty,
                CriadoLogin = string.Empty
            };

            dto.AlteradoPor.Should().Be(string.Empty);
            dto.AlteradoLogin.Should().Be(string.Empty);
            dto.CriadoPor.Should().Be(string.Empty);
            dto.CriadoLogin.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoDTOComStringComCaracteresEspeciais_QuandoInstanciado_EntaoStringDeveSerPreservada()
        {
            var stringComEspeciais = "João Silva @#$%^&*()";
            var dto = new AuditavelDTOImplementacao
            {
                Id = 1L,
                Excluido = false,
                CriadoEm = DateTime.Now,
                CriadoPor = stringComEspeciais,
                CriadoLogin = stringComEspeciais
            };

            dto.CriadoPor.Should().Be(stringComEspeciais);
            dto.CriadoLogin.Should().Be(stringComEspeciais);
        }

        [Fact]
        public void DadoDTOAlterado_QuandoVerificadoAuditoria_EntaoTodosAtributosAuditaveisDevemSerValidos()
        {
            var dataAgora = DateTime.Now;
            var dto = new AuditavelDTOImplementacao
            {
                Id = 100L,
                Excluido = false,
                AlteradoEm = dataAgora.AddHours(-2),
                AlteradoPor = "Maria",
                AlteradoLogin = "maria.santos",
                CriadoEm = dataAgora.AddDays(-30),
                CriadoPor = "Admin",
                CriadoLogin = "admin"
            };

            dto.AlteradoEm.Should().NotBeNull();
            dto.AlteradoPor.Should().NotBeNullOrEmpty();
            dto.AlteradoLogin.Should().NotBeNullOrEmpty();
            dto.CriadoEm.Should().NotBe(default);
            dto.CriadoPor.Should().NotBeNullOrEmpty();
            dto.CriadoLogin.Should().NotBeNullOrEmpty();
        }
        private class AuditavelDTOImplementacao : BaseAuditavelDTO
        {
        }
    }
}
