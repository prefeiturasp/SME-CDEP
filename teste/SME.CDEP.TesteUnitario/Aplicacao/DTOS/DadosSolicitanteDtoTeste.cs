using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class DadosSolicitanteDtoTeste
    {
        [Fact]
        public void DadoDTOValido_QuandoCriar_EntaoDevePossuirPropriedades()
        {
            var dto = new DadosSolicitanteDto();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<DadosSolicitanteDto>();
        }

        [Fact]
        public void DadoIdDefinido_QuandoAtribuir_EntaoIdDeveSerSetado()
        {
            var dto = new DadosSolicitanteDto();
            var idEsperado = 123L;

            dto.Id = idEsperado;

            dto.Id.Should().Be(idEsperado);
        }

        [Fact]
        public void DadoNomeDefinido_QuandoAtribuir_EntaoNomeDeveSerSetado()
        {
            var dto = new DadosSolicitanteDto();
            var nomeEsperado = "João da Silva";

            dto.Nome = nomeEsperado;

            dto.Nome.Should().Be(nomeEsperado);
        }

        [Fact]
        public void DadoNomeNulo_QuandoAtribuir_EntaoNomeDeveSerNulo()
        {
            var dto = new DadosSolicitanteDto();

            dto.Nome = null;

            dto.Nome.Should().BeNull();
        }

        [Fact]
        public void DadoLoginDefinido_QuandoAtribuir_EntaoLoginDeveSerSetado()
        {
            var dto = new DadosSolicitanteDto();
            var loginEsperado = "joao.silva";

            dto.Login = loginEsperado;

            dto.Login.Should().Be(loginEsperado);
        }

        [Fact]
        public void DadoLoginNulo_QuandoAtribuir_EntaoLoginDeveSerNulo()
        {
            var dto = new DadosSolicitanteDto();

            dto.Login = null;

            dto.Login.Should().BeNull();
        }

        [Fact]
        public void DadoTelefoneDefinido_QuandoAtribuir_EntaoTelefoneDeveSerSetado()
        {
            var dto = new DadosSolicitanteDto();
            var telefoneEsperado = "(11) 98765-4321";

            dto.Telefone = telefoneEsperado;

            dto.Telefone.Should().Be(telefoneEsperado);
        }

        [Fact]
        public void DadoTelefoneNulo_QuandoAtribuir_EntaoTelefoneDeveSerNulo()
        {
            var dto = new DadosSolicitanteDto();

            dto.Telefone = null;

            dto.Telefone.Should().BeNull();
        }

        [Fact]
        public void DadoEnderecoDefinido_QuandoAtribuir_EntaoEnderecoDeveSerSetado()
        {
            var dto = new DadosSolicitanteDto();
            var enderecoEsperado = "Rua das Flores";

            dto.Endereco = enderecoEsperado;

            dto.Endereco.Should().Be(enderecoEsperado);
        }

        [Fact]
        public void DadoEnderecoNaoDefinido_QuandoCriar_EntaoEnderecoDeveSerVazio()
        {
            var dto = new DadosSolicitanteDto();

            dto.Endereco.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoEmailDefinido_QuandoAtribuir_EntaoEmailDeveSerSetado()
        {
            var dto = new DadosSolicitanteDto();
            var emailEsperado = "joao@example.com";

            dto.Email = emailEsperado;

            dto.Email.Should().Be(emailEsperado);
        }

        [Fact]
        public void DadoEmailNulo_QuandoAtribuir_EntaoEmailDeveSerNulo()
        {
            var dto = new DadosSolicitanteDto();

            dto.Email = null;

            dto.Email.Should().BeNull();
        }

        [Fact]
        public void DadoTipoDefinido_QuandoAtribuir_EntaoTipoDeveSerSetado()
        {
            var dto = new DadosSolicitanteDto();
            var tipoEsperado = "Pessoa Física";

            dto.Tipo = tipoEsperado;

            dto.Tipo.Should().Be(tipoEsperado);
        }

        [Fact]
        public void DadoTipoNulo_QuandoAtribuir_EntaoTipoDeveSerNulo()
        {
            var dto = new DadosSolicitanteDto();

            dto.Tipo = null;

            dto.Tipo.Should().BeNull();
        }

        [Fact]
        public void DadoTipoIdDefinido_QuandoAtribuir_EntaoTipoIdDeveSerSetado()
        {
            var dto = new DadosSolicitanteDto();
            var tipoIdEsperado = TipoUsuario.CORESSO;

            dto.TipoId = tipoIdEsperado;

            dto.TipoId.Should().Be(tipoIdEsperado);
        }

        [Fact]
        public void DadoObterEnderecoCompletoComTodosOsParametros_QuandoChamar_EntaoEnderecoDeveSerFormatadoCorretamente()
        {
            var dto = new DadosSolicitanteDto();
            dto.Endereco = "Rua das Flores";
            var numero = "123";
            var complemento = "Apto 456";
            var cidade = "São Paulo";
            var estado = "SP";
            var cep = "01234-567";

            var resultado = dto.ObterEnderecoCompleto(numero, complemento, cidade, estado, cep);

            resultado.Should().Be("Rua das Flores, 123 - Apto 456 - São Paulo/SP - 01234-567");
        }

        [Fact]
        public void DadoObterEnderecoCompletoSomenteCidade_QuandoChamar_EntaoEnderecoDeveSerFormatadoComCidade()
        {
            var dto = new DadosSolicitanteDto();
            dto.Endereco = "Avenida Paulista";
            var numero = "1578";
            string complemento = null!;
            string estado = null!;
            var cidade = "São Paulo";

            var resultado = dto.ObterEnderecoCompleto(numero, complemento, estado, null, cidade);

            resultado.Should().Be("Avenida Paulista, 1578 - São Paulo");
        }

        [Fact]
        public void DadoObterEnderecoCompletoApenasPrincipal_QuandoChamar_EntaoEnderecoDeveSerPrincipal()
        {
            var dto = new DadosSolicitanteDto();
            dto.Endereco = "Rua Principal";
            string numero = null!;
            string complemento = null!;
            string cidade = null!;
            string estado = null!;
            string cep = null!;

            var resultado = dto.ObterEnderecoCompleto(numero, complemento, cidade, estado, cep);

            resultado.Should().Be("Rua Principal");
        }

        [Fact]
        public void DadoObterEnderecoCompletoComNumeroEComplemento_QuandoChamar_EntaoEnderecoDeveSerFormatado()
        {
            var dto = new DadosSolicitanteDto();
            dto.Endereco = "Rua das Acácias";
            var numero = "999";
            var complemento = "Casa 2";
            string cidade = null!;
            string estado = null!;
            string cep = null!;

            var resultado = dto.ObterEnderecoCompleto(numero, complemento, cidade, estado, cep);

            resultado.Should().Be("Rua das Acácias, 999 - Casa 2");
        }

        [Fact]
        public void DadoObterEnderecoCompletoComEstado_QuandoChamar_EntaoEnderecoDeveSerFormatadoComEstado()
        {
            var dto = new DadosSolicitanteDto();
            dto.Endereco = "Rua do Comércio";
            var numero = "50";
            string complemento = null!;
            var cidade = "Rio de Janeiro";
            var estado = "RJ";
            string cep = null!;

            var resultado = dto.ObterEnderecoCompleto(numero, complemento, cidade, estado, cep);

            resultado.Should().Be("Rua do Comércio, 50 - Rio de Janeiro/RJ");
        }

        [Fact]
        public void DadoObterEnderecoCompletoComCEP_QuandoChamar_EntaoEnderecoDeveSerFormatadoComCEP()
        {
            var dto = new DadosSolicitanteDto();
            dto.Endereco = "Avenida Brasil";
            var numero = "200";
            var complemento = "Bloco A";
            var cidade = "Brasília";
            var estado = "DF";
            var cep = "70000-000";

            var resultado = dto.ObterEnderecoCompleto(numero, complemento, cidade, estado, cep);

            resultado.Should().Be("Avenida Brasil, 200 - Bloco A - Brasília/DF - 70000-000");
        }

        [Fact]
        public void DadoObterEnderecoCompletoComNumeroVazio_QuandoChamar_EntaoNumeroNaoDeveSerAdicionado()
        {
            var dto = new DadosSolicitanteDto();
            dto.Endereco = "Rua do Teste";
            var numero = "";
            var complemento = "Sala 1";
            string cidade = null!;
            string estado = null!;
            string cep = null!;

            var resultado = dto.ObterEnderecoCompleto(numero, complemento, cidade, estado, cep);

            resultado.Should().Be("Rua do Teste - Sala 1");
        }

        [Fact]
        public void DadoObterEnderecoCompletoComNumeroEspacosBranco_QuandoChamar_EntaoNumeroNaoDeveSerAdicionado()
        {
            var dto = new DadosSolicitanteDto();
            dto.Endereco = "Rua da Esperança";
            var numero = "   ";
            string complemento = null!;
            var cidade = "Salvador";
            string estado = null!;
            string cep = null!;

            var resultado = dto.ObterEnderecoCompleto(numero, complemento, cidade, estado, cep);

            resultado.Should().Be("Rua da Esperança - Salvador");
        }

        [Fact]
        public void DadoObterEnderecoCompletoComComplementoVazio_QuandoChamar_EntaoComplementoNaoDeveSerAdicionado()
        {
            var dto = new DadosSolicitanteDto();
            dto.Endereco = "Rua Teste";
            var numero = "100";
            var complemento = "";
            string cidade = null!;
            string estado = null!;
            string cep = null!;

            var resultado = dto.ObterEnderecoCompleto(numero, complemento, cidade, estado, cep);

            resultado.Should().Be("Rua Teste, 100");
        }

        [Fact]
        public void DadoObterEnderecoCompletoComCidadeVazia_QuandoChamar_EntaoCidadeNaoDeveSerAdicionada()
        {
            var dto = new DadosSolicitanteDto();
            dto.Endereco = "Rua Exemplo";
            var numero = "250";
            var complemento = "Loja B";
            var cidade = "";
            var estado = "MG";
            string cep = null!;

            var resultado = dto.ObterEnderecoCompleto(numero, complemento, cidade, estado, cep);

            resultado.Should().Be("Rua Exemplo, 250 - Loja B/MG");
        }

        [Fact]
        public void DadoObterEnderecoCompletoComEstadoVazio_QuandoChamar_EntaoEstadoNaoDeveSerAdicionado()
        {
            var dto = new DadosSolicitanteDto();
            dto.Endereco = "Rua Modelo";
            var numero = "300";
            string complemento = null!;
            var cidade = "Curitiba";
            var estado = "";
            var cep = "80000-000";

            var resultado = dto.ObterEnderecoCompleto(numero, complemento, cidade, estado, cep);

            resultado.Should().Be("Rua Modelo, 300 - Curitiba - 80000-000");
        }

        [Fact]
        public void DadoObterEnderecoCompletoComCEPVazio_QuandoChamar_EntaoCEPNaoDeveSerAdicionado()
        {
            var dto = new DadosSolicitanteDto();
            dto.Endereco = "Rua Completa";
            var numero = "400";
            var complemento = "Apt 10";
            var cidade = "Belo Horizonte";
            var estado = "MG";
            var cep = "";

            var resultado = dto.ObterEnderecoCompleto(numero, complemento, cidade, estado, cep);

            resultado.Should().Be("Rua Completa, 400 - Apt 10 - Belo Horizonte/MG");
        }

        [Fact]
        public void DadoMultiplosDTOs_QuandoCriar_EntaoTodosDevemSerIndependentes()
        {
            var dto1 = new DadosSolicitanteDto
            {
                Id = 1,
                Nome = "João",
                Email = "joao@test.com",
                TipoId = TipoUsuario.ESTUDANTE
            };
            var dto2 = new DadosSolicitanteDto
            {
                Id = 2,
                Nome = "Maria",
                Email = "maria@test.com",
                TipoId = TipoUsuario.SERVIDOR_PUBLICO
            };

            dto1.Should().NotBeSameAs(dto2);
            dto1.Nome.Should().NotBe(dto2.Nome);
            dto1.Email.Should().NotBe(dto2.Email);
        }

        [Fact]
        public void DadoDTOComValoresAlterados_QuandoVerificar_EntaoAlteracoesDeveramSerReflexas()
        {
            var dto = new DadosSolicitanteDto();
            var nomeInicial = "Pedro";
            var nomeAlterado = "Pedro Silva";

            dto.Nome = nomeInicial;
            dto.Nome = nomeAlterado;

            dto.Nome.Should().Be(nomeAlterado);
            dto.Nome.Should().NotBe(nomeInicial);
        }

        [Fact]
        public void DadoDTOUsandoFaker_QuandoGerarDados_EntaoDeveSerPreenchidoCorretamente()
        {
            var faker = new Faker<DadosSolicitanteDto>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.Nome, f => f.Name.FullName())
                .RuleFor(x => x.Login, f => f.Internet.UserName())
                .RuleFor(x => x.Telefone, f => f.Phone.PhoneNumber("(##) 9####-####"))
                .RuleFor(x => x.Endereco, f => f.Address.StreetAddress())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Tipo, f => f.Random.String2(10))
                .RuleFor(x => x.TipoId, f => f.Random.Enum<TipoUsuario>());

            var dto = faker.Generate();

            dto.Should().NotBeNull();
            dto.Id.Should().BeGreaterThan(0);
            dto.Nome.Should().NotBeNullOrEmpty();
            dto.Login.Should().NotBeNullOrEmpty();
            dto.Telefone.Should().NotBeNullOrEmpty();
            dto.Endereco.Should().NotBeNullOrEmpty();
            dto.Email.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void DadoListaDeTresDTOs_QuandoGerarMultiplos_EntaoTodosDevemSerValidos()
        {
            var faker = new Faker<DadosSolicitanteDto>("pt_BR")
                .RuleFor(x => x.Nome, f => f.Name.FullName())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Id, f => f.Random.Long(1, 1000));

            var dtos = faker.Generate(3).ToList();

            dtos.Should().HaveCount(3);
            dtos.Should().AllSatisfy(dto =>
            {
                dto.Nome.Should().NotBeNullOrEmpty();
                dto.Email.Should().NotBeNullOrEmpty();
                dto.Id.Should().BeGreaterThan(0);
            });
        }

        [Fact]
        public void DadoDTOComPropriedadesDefinidas_QuandoAcessar_EntaoTodasDeveramEstarDisponíveis()
        {
            var propriedades = typeof(DadosSolicitanteDto).GetProperties();

            propriedades.Should().NotBeEmpty();
            propriedades.Should().Contain(p => p.Name == "Id");
            propriedades.Should().Contain(p => p.Name == "Nome");
            propriedades.Should().Contain(p => p.Name == "Login");
            propriedades.Should().Contain(p => p.Name == "Telefone");
            propriedades.Should().Contain(p => p.Name == "Endereco");
            propriedades.Should().Contain(p => p.Name == "Email");
            propriedades.Should().Contain(p => p.Name == "Tipo");
            propriedades.Should().Contain(p => p.Name == "TipoId");
        }

        [Fact]
        public void DadoDTOComDadosCompletos_QuandoVerificarEquivalencia_EntaoDeveSerEquivalente()
        {
            var dto1 = new DadosSolicitanteDto
            {
                Id = 1,
                Nome = "Carlos José",
                Login = "carlos.jose",
                Telefone = "(11) 98765-4321",
                Endereco = "Rua A",
                Email = "carlos@test.com",
                Tipo = "Solicitante",
                TipoId = TipoUsuario.SERVIDOR_PUBLICO
            };

            var dto2 = new DadosSolicitanteDto
            {
                Id = 1,
                Nome = "Carlos José",
                Login = "carlos.jose",
                Telefone = "(11) 98765-4321",
                Endereco = "Rua A",
                Email = "carlos@test.com",
                Tipo = "Solicitante",
                TipoId = TipoUsuario.SERVIDOR_PUBLICO
            };

            dto1.Should().BeEquivalentTo(dto2);
        }

        [Fact]
        public void DadoEnderecoCompleto_QuandoObterEndereco_EntaoDeveRetornarEnderecoAcumulado()
        {
            var dto = new DadosSolicitanteDto();
            dto.Endereco = "Rua Principal";
            var resultado = dto.ObterEnderecoCompleto("100", "Apt 5", "São Paulo", "SP", "01000-000");

            dto.Endereco.Should().Be("Rua Principal, 100 - Apt 5 - São Paulo/SP - 01000-000");
            resultado.Should().Be(dto.Endereco);
        }

        [Fact]
        public void DadoObterEnderecoCompletoIntegrado_QuandoChamarMultiplasVezes_EntaoDeveAcumularResultados()
        {
            var dto = new DadosSolicitanteDto();
            dto.Endereco = "Avenida Z";

            var resultado1 = dto.ObterEnderecoCompleto("500", null, null, null, null);
            var resultado2 = dto.ObterEnderecoCompleto("600", null, null, null, null);

            resultado1.Should().Be("Avenida Z, 500");
            resultado2.Should().Be("Avenida Z, 500, 600");
        }

        [Fact]
        public void DadoObterEnderecoCompletoComEspacosBrancos_QuandoChamarComParametrosEspacos_EntaoParametrosNaoDevemSerAdicionados()
        {
            var dto = new DadosSolicitanteDto();
            dto.Endereco = "Rua Teste Completa";

            var resultado = dto.ObterEnderecoCompleto("  ", "   ", "  ", "  ", "  ");

            resultado.Should().Be("Rua Teste Completa");
        }

        [Fact]
        public void DadoObterEnderecoCompletoComValoresNulos_QuandoChamarComValoresNull_EntaoEnderecoNaoDeveSerAlterado()
        {
            var dto = new DadosSolicitanteDto();
            dto.Endereco = "Rua Original";

            var resultado = dto.ObterEnderecoCompleto(null, null, null, null, null);

            resultado.Should().Be("Rua Original");
        }
    }
}
