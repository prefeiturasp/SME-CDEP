using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class BaseDTOTeste
    {
        private readonly Faker<BaseDTOImplementacao> faker;

        public BaseDTOTeste()
        {
            faker = new Faker<BaseDTOImplementacao>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.Excluido, f => f.Random.Bool());
        }

        [Fact]
        public void DadoDTOValido_QuandoInstanciado_EntaoIdDeveSerPreenchido()
        {
            var id = 123L;
            var dto = new BaseDTOImplementacao
            {
                Id = id,
                Excluido = false
            };

            dto.Id.Should().Be(id);
        }

        [Fact]
        public void DadoDTOValido_QuandoInstanciado_EntaoExcluídoDeveSerPreenchido()
        {
            var excluido = true;
            var dto = new BaseDTOImplementacao
            {
                Id = 1L,
                Excluido = excluido
            };

            dto.Excluido.Should().Be(excluido);
        }

        [Fact]
        public void DadoDTOValido_QuandoInstanciado_EntaoTodosAtributosDevemSerPreenchidos()
        {
            var id = 456L;
            var excluido = false;

            var dto = new BaseDTOImplementacao
            {
                Id = id,
                Excluido = excluido
            };

            dto.Id.Should().Be(id);
            dto.Excluido.Should().Be(excluido);
        }

        [Fact]
        public void DadoDTOComIdZero_QuandoInstanciado_EntaoIdDeveSerZero()
        {
            var dto = new BaseDTOImplementacao
            {
                Id = 0,
                Excluido = false
            };

            dto.Id.Should().Be(0);
        }

        [Fact]
        public void DadoDTOComIdMaximo_QuandoInstanciado_EntaoIdDeveSerPreservado()
        {
            var idMaximo = long.MaxValue;
            var dto = new BaseDTOImplementacao
            {
                Id = idMaximo,
                Excluido = false
            };

            dto.Id.Should().Be(idMaximo);
        }

        [Fact]
        public void DadoDTOComIdNegativo_QuandoInstanciado_EntaoIdDeveSerPreservado()
        {
            var idNegativo = -100L;
            var dto = new BaseDTOImplementacao
            {
                Id = idNegativo,
                Excluido = false
            };

            dto.Id.Should().Be(idNegativo);
        }

        [Fact]
        public void DadoDTOExcluido_QuandoVerificarAtributoExcluido_EntaoDeveRetornarTrue()
        {
            var dto = new BaseDTOImplementacao
            {
                Id = 1L,
                Excluido = true
            };

            dto.Excluido.Should().BeTrue();
        }

        [Fact]
        public void DadoDTONaoExcluido_QuandoVerificarAtributoExcluido_EntaoDeveRetornarFalse()
        {
            var dto = new BaseDTOImplementacao
            {
                Id = 1L,
                Excluido = false
            };

            dto.Excluido.Should().BeFalse();
        }

        [Fact]
        public void DadoDTOCriadoComBogus_QuandoInstanciado_EntaoTodosAtributosDevemSerValidados()
        {
            var dto = faker.Generate();

            dto.Id.Should().BeGreaterThanOrEqualTo(1);
            dto.Excluido.GetType().Should().Be<bool>();
        }

        [Fact]
        public void DadoMultiplosDTOs_QuandoComparadosValores_EntaoDevemConterIdDistintos()
        {
            var dto1 = faker.Generate();
            var dto2 = faker.Generate();

            dto1.Id.Should().NotBe(dto2.Id);
        }

        [Fact]
        public void DadoDTOComIdPositivo_QuandoInstanciado_EntaoIdDeveSerMaiorQueZero()
        {
            var dto = new BaseDTOImplementacao
            {
                Id = 999L,
                Excluido = false
            };

            dto.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public void DadoDTOAlternandoValoresExcluido_QuandoModificado_EntaoDeveReflitirANovaAtribuicao()
        {
            var dto = new BaseDTOImplementacao
            {
                Id = 1L,
                Excluido = false
            };

            dto.Excluido.Should().BeFalse();

            dto.Excluido = true;
            dto.Excluido.Should().BeTrue();

            dto.Excluido = false;
            dto.Excluido.Should().BeFalse();
        }

        [Fact]
        public void DadoDTOAlternandoValoresId_QuandoModificado_EntaoDeveReflitirANovaAtribuicao()
        {
            var dto = new BaseDTOImplementacao
            {
                Id = 1L,
                Excluido = false
            };

            dto.Id.Should().Be(1L);

            dto.Id = 100L;
            dto.Id.Should().Be(100L);

            dto.Id = 999L;
            dto.Id.Should().Be(999L);
        }

        private class BaseDTOImplementacao : BaseDTO
        {
        }
    }
}
