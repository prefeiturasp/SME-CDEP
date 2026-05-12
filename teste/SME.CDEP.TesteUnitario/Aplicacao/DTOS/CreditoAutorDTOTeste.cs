using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class CreditoAutorDTOTeste
    {
        [Fact]
        public void DadoDTOValido_QuandoCriar_EntaoDevePossuirNomeProperty()
        {
            var dto = new CreditoAutorDTO();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<CreditoAutorDTO>();
        }

        [Fact]
        public void DadoNome_QuandoAtribuir_EntaoDevePossuirNomeSetado()
        {
            var dto = new CreditoAutorDTO();
            var nomeEsperado = "João Silva";

            dto.nome = nomeEsperado;

            dto.nome.Should().Be(nomeEsperado);
        }

        [Fact]
        public void DadoDTOComNomeBranco_QuandoVerificar_EntaoNomeDeveSerBranco()
        {
            var dto = new CreditoAutorDTO();
            dto.nome = "";

            dto.nome.Should().BeEmpty();
        }

        [Fact]
        public void DadoDTOComNomeNulo_QuandoVerificar_EntaoNomeDeveSerNulo()
        {
            var dto = new CreditoAutorDTO();
            dto.nome = null;

            dto.nome.Should().BeNull();
        }

        [Fact]
        public void DadoNomeComEspacosEmBranco_QuandoAtribuir_EntaoDeveMantidoComEspacos()
        {
            var dto = new CreditoAutorDTO();
            var nomeComEspacos = "  Maria Santos  ";

            dto.nome = nomeComEspacos;

            dto.nome.Should().Be(nomeComEspacos);
        }

        [Fact]
        public void DadoDTOComIDHerdadoDeBaseDTO_QuandoAtribuir_EntaoIdDeveSerSetado()
        {
            var dto = new CreditoAutorDTO();
            var idEsperado = 123L;

            dto.Id = idEsperado;

            dto.Id.Should().Be(idEsperado);
        }

        [Fact]
        public void DadoDTOComExcluidoHerdadoDeBaseDTO_QuandoAtribuir_EntaoExcluidoDeveSerSetado()
        {
            var dto = new CreditoAutorDTO();

            dto.Excluido = true;

            dto.Excluido.Should().BeTrue();
        }

        [Fact]
        public void DadoDTOComPropriedadesHerdadasDeBaseAuditavelDTO_QuandoAtribuir_EntaoTodasDeveramSerSetadas()
        {
            var dto = new CreditoAutorDTO();
            var agora = DateTime.UtcNow;
            var nomeAutor = "Carlos Junior";
            var nomeUsuario = "carlos.junior";
            var loginUsuario = "cjunior";

            dto.nome = nomeAutor;
            dto.CriadoEm = agora;
            dto.CriadoPor = nomeUsuario;
            dto.CriadoLogin = loginUsuario;
            dto.AlteradoEm = agora;
            dto.AlteradoPor = nomeUsuario;
            dto.AlteradoLogin = loginUsuario;

            dto.nome.Should().Be(nomeAutor);
            dto.CriadoEm.Should().Be(agora);
            dto.CriadoPor.Should().Be(nomeUsuario);
            dto.CriadoLogin.Should().Be(loginUsuario);
            dto.AlteradoEm.Should().Be(agora);
            dto.AlteradoPor.Should().Be(nomeUsuario);
            dto.AlteradoLogin.Should().Be(loginUsuario);
        }

        [Fact]
        public void DadoMultiplosDTOs_QuandoCriar_EntaoTodosDevemSerIndependentes()
        {
            var dto1 = new CreditoAutorDTO { nome = "Autor Um", Id = 1 };
            var dto2 = new CreditoAutorDTO { nome = "Autor Dois", Id = 2 };

            dto1.nome.Should().NotBe(dto2.nome);
            dto1.Id.Should().NotBe(dto2.Id);
            dto1.Should().NotBeSameAs(dto2);
        }

        [Fact]
        public void DadoDTOComValoresAlterados_QuandoVerificar_EntaoAlteracoesDeveramSerReflexos()
        {
            var dto = new CreditoAutorDTO();
            var nomeInicial = "Pedro";
            var nomeAlterado = "Pedro Silva";

            dto.nome = nomeInicial;
            dto.nome = nomeAlterado;

            dto.nome.Should().Be(nomeAlterado);
            dto.nome.Should().NotBe(nomeInicial);
        }

        [Fact]
        public void DadoDTOComNomeGrandeTamanho_QuandoAtribuir_EntaoDeveSuportarGrandesTextos()
        {
            var dto = new CreditoAutorDTO();
            var nomeLongo = string.Join(" ", new Faker("pt_BR").Lorem.Words(500));

            dto.nome = nomeLongo;

            dto.nome.Should().Be(nomeLongo);
            dto.nome.Length.Should().BeGreaterThan(100);
        }

        [Fact]
        public void DadoDTOComNomeCaracteresEspeciais_QuandoAtribuir_EntaoDeveSuportarCaracteresEspeciais()
        {
            var dto = new CreditoAutorDTO();
            var nomeComEspeciais = "José da Silva-Oliveira & Cia. Ltda.";

            dto.nome = nomeComEspeciais;

            dto.nome.Should().Be(nomeComEspeciais);
        }

        [Fact]
        public void DadoDTOcomValoresNull_QuandoVerificarPropriedadesAuditoria_EntaoDevePermitirNull()
        {
            var dto = new CreditoAutorDTO();

            dto.AlteradoEm = null;
            dto.AlteradoPor = null;
            dto.AlteradoLogin = null;

            dto.AlteradoEm.Should().BeNull();
            dto.AlteradoPor.Should().BeNull();
            dto.AlteradoLogin.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComDatasCriacao_QuandoVerificar_EntaoPropriedadesNaoDevemSerNulas()
        {
            var dto = new CreditoAutorDTO();
            var dataCriacao = new DateTime(2025, 5, 4, 10, 30, 0);

            dto.CriadoEm = dataCriacao;

            dto.CriadoEm.Should().NotBe(null);
            dto.CriadoEm.Should().Be(dataCriacao);
        }

        [Fact]
        public void DadoDTOUsandoFaker_QuandoGerarDados_EntaoDeveSerPreenchidoCorretamente()
        {
            var faker = new Faker<CreditoAutorDTO>("pt_BR")
                .RuleFor(x => x.nome, f => f.Company.CompanyName())
                .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.Excluido, f => f.Random.Bool())
                .RuleFor(x => x.CriadoEm, f => f.Date.Past())
                .RuleFor(x => x.CriadoPor, f => f.Name.FullName())
                .RuleFor(x => x.CriadoLogin, f => f.Internet.UserName())
                .RuleFor(x => x.AlteradoEm, f => f.Date.Recent())
                .RuleFor(x => x.AlteradoPor, f => f.Name.FullName())
                .RuleFor(x => x.AlteradoLogin, f => f.Internet.UserName());

            var dto = faker.Generate();

            dto.Should().NotBeNull();
            dto.nome.Should().NotBeNullOrEmpty();
            dto.Id.Should().BeGreaterThan(0);
            dto.CriadoEm.Should().NotBe(default(DateTime));
            dto.CriadoPor.Should().NotBeNullOrEmpty();
            dto.CriadoLogin.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void DadoListaDeTrsDTOs_QuandoGerarMultiplos_EntaoTodosDevemSerValidos()
        {
            var faker = new Faker<CreditoAutorDTO>("pt_BR")
                .RuleFor(x => x.nome, f => f.Company.CompanyName())
                .RuleFor(x => x.Id, f => f.Random.Long(1, 1000));

            var dtos = faker.Generate(3).ToList();

            dtos.Should().HaveCount(3);
            dtos.Should().AllSatisfy(dto =>
            {
                dto.nome.Should().NotBeNullOrEmpty();
                dto.Id.Should().BeGreaterThan(0);
            });
        }

        [Fact]
        public void DadoDTOComExcluidoFalso_QuandoVerificar_EntaoExcluidoDeveSerFalso()
        {
            var dto = new CreditoAutorDTO();
            dto.Excluido = false;

            dto.Excluido.Should().BeFalse();
        }

        [Fact]
        public void DadoDTOComExcluidoVerdadeiro_QuandoVerificar_EntaoExcluidoDeveSerVerdadeiro()
        {
            var dto = new CreditoAutorDTO();
            dto.Excluido = true;

            dto.Excluido.Should().BeTrue();
        }

        [Fact]
        public void DadoDTOComDataCriacaoeDados_QuandoVerificarEquivalencia_EntaoDeveSerEquivalente()
        {
            var dto1 = new CreditoAutorDTO
            {
                Id = 1,
                nome = "Mesmo Nome",
                CriadoEm = new DateTime(2025, 1, 1),
                CriadoPor = "Sistema",
                CriadoLogin = "sistema"
            };

            var dto2 = new CreditoAutorDTO
            {
                Id = 1,
                nome = "Mesmo Nome",
                CriadoEm = new DateTime(2025, 1, 1),
                CriadoPor = "Sistema",
                CriadoLogin = "sistema"
            };

            dto1.Should().BeEquivalentTo(dto2);
        }

        [Fact]
        public void DadoDTOComPropriedadesDefinidas_QuandoAcessar_EntaoTodasDeveramEstarDisponíveis()
        {
            var propriedades = typeof(CreditoAutorDTO).GetProperties();

            propriedades.Should().NotBeEmpty();
            propriedades.Should().Contain(p => p.Name == "nome");
            propriedades.Should().Contain(p => p.Name == "Id");
            propriedades.Should().Contain(p => p.Name == "Excluido");
            propriedades.Should().Contain(p => p.Name == "CriadoEm");
            propriedades.Should().Contain(p => p.Name == "CriadoPor");
            propriedades.Should().Contain(p => p.Name == "CriadoLogin");
        }
    }
}
