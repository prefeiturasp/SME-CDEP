using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class DadosUsuarioDTOTeste
    {
        [Fact]
        public void DadoDTOValido_QuandoCriar_EntaoDevePossuirPropriedades()
        {
            var dto = new DadosUsuarioDTO();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<DadosUsuarioDTO>();
        }

        [Fact]
        public void DadoNomeDefinido_QuandoAtribuir_EntaoNomeDeveSerSetado()
        {
            var dto = new DadosUsuarioDTO();
            var nomeEsperado = "João da Silva";

            dto.Nome = nomeEsperado;

            dto.Nome.Should().Be(nomeEsperado);
        }

        [Fact]
        public void DadoNomeNulo_QuandoAtribuir_EntaoNomeDeveSerNulo()
        {
            var dto = new DadosUsuarioDTO();

            dto.Nome = null;

            dto.Nome.Should().BeNull();
        }

        [Fact]
        public void DadoCpfDefinido_QuandoAtribuir_EntaoCpfDeveSerSetado()
        {
            var dto = new DadosUsuarioDTO();
            var cpfEsperado = "123.456.789-09";

            dto.Cpf = cpfEsperado;

            dto.Cpf.Should().Be(cpfEsperado);
        }

        [Fact]
        public void DadoCpfNulo_QuandoAtribuir_EntaoCpfDeveSerNulo()
        {
            var dto = new DadosUsuarioDTO();

            dto.Cpf = null;

            dto.Cpf.Should().BeNull();
        }

        [Fact]
        public void DadoLoginDefinido_QuandoAtribuir_EntaoLoginDeveSerSetado()
        {
            var dto = new DadosUsuarioDTO();
            var loginEsperado = "joao.silva";

            dto.Login = loginEsperado;

            dto.Login.Should().Be(loginEsperado);
        }

        [Fact]
        public void DadoLoginNulo_QuandoAtribuir_EntaoLoginDeveSerNulo()
        {
            var dto = new DadosUsuarioDTO();

            dto.Login = null;

            dto.Login.Should().BeNull();
        }

        [Fact]
        public void DadoEmailDefinido_QuandoAtribuir_EntaoEmailDeveSerSetado()
        {
            var dto = new DadosUsuarioDTO();
            var emailEsperado = "joao@example.com";

            dto.Email = emailEsperado;

            dto.Email.Should().Be(emailEsperado);
        }

        [Fact]
        public void DadoEmailNulo_QuandoAtribuir_EntaoEmailDeveSerNulo()
        {
            var dto = new DadosUsuarioDTO();

            dto.Email = null;

            dto.Email.Should().BeNull();
        }

        [Fact]
        public void DadoTelefoneDefinido_QuandoAtribuir_EntaoTelefoneDeveSerSetado()
        {
            var dto = new DadosUsuarioDTO();
            var telefoneEsperado = "(11) 98765-4321";

            dto.Telefone = telefoneEsperado;

            dto.Telefone.Should().Be(telefoneEsperado);
        }

        [Fact]
        public void DadoTelefoneNulo_QuandoAtribuir_EntaoTelefoneDeveSerNulo()
        {
            var dto = new DadosUsuarioDTO();

            dto.Telefone = null;

            dto.Telefone.Should().BeNull();
        }

        [Fact]
        public void DadoEnderecoDefinido_QuandoAtribuir_EntaoEnderecoDeveSerSetado()
        {
            var dto = new DadosUsuarioDTO();
            var enderecoEsperado = "Rua das Flores";

            dto.Endereco = enderecoEsperado;

            dto.Endereco.Should().Be(enderecoEsperado);
        }

        [Fact]
        public void DadoEnderecoNulo_QuandoAtribuir_EntaoEnderecoDeveSerNulo()
        {
            var dto = new DadosUsuarioDTO();

            dto.Endereco = null;

            dto.Endereco.Should().BeNull();
        }

        [Fact]
        public void DadoNumeroDefinido_QuandoAtribuir_EntaoNumeroDeveSerSetado()
        {
            var dto = new DadosUsuarioDTO();
            var numeroEsperado = "123";

            dto.Numero = numeroEsperado;

            dto.Numero.Should().Be(numeroEsperado);
        }

        [Fact]
        public void DadoNumeroNulo_QuandoAtribuir_EntaoNumeroDeveSerNulo()
        {
            var dto = new DadosUsuarioDTO();

            dto.Numero = null;

            dto.Numero.Should().BeNull();
        }

        [Fact]
        public void DadoComplementoDefinido_QuandoAtribuir_EntaoComplementoDeveSerSetado()
        {
            var dto = new DadosUsuarioDTO();
            var complementoEsperado = "Apto 456";

            dto.Complemento = complementoEsperado;

            dto.Complemento.Should().Be(complementoEsperado);
        }

        [Fact]
        public void DadoComplementoNulo_QuandoAtribuir_EntaoComplementoDeveSerNulo()
        {
            var dto = new DadosUsuarioDTO();

            dto.Complemento = null;

            dto.Complemento.Should().BeNull();
        }

        [Fact]
        public void DadoBairroDefinido_QuandoAtribuir_EntaoBairroDeveSerSetado()
        {
            var dto = new DadosUsuarioDTO();
            var bairroEsperado = "Centro";

            dto.Bairro = bairroEsperado;

            dto.Bairro.Should().Be(bairroEsperado);
        }

        [Fact]
        public void DadoBairroNulo_QuandoAtribuir_EntaoBairroDeveSerNulo()
        {
            var dto = new DadosUsuarioDTO();

            dto.Bairro = null;

            dto.Bairro.Should().BeNull();
        }

        [Fact]
        public void DadoCepDefinido_QuandoAtribuir_EntaoCepDeveSerSetado()
        {
            var dto = new DadosUsuarioDTO();
            var cepEsperado = "01234-567";

            dto.Cep = cepEsperado;

            dto.Cep.Should().Be(cepEsperado);
        }

        [Fact]
        public void DadoCepNulo_QuandoAtribuir_EntaoCepDeveSerNulo()
        {
            var dto = new DadosUsuarioDTO();

            dto.Cep = null;

            dto.Cep.Should().BeNull();
        }

        [Fact]
        public void DadoCidadeDefinida_QuandoAtribuir_EntaoCidadeDeveSerSetada()
        {
            var dto = new DadosUsuarioDTO();
            var cidadeEsperada = "São Paulo";

            dto.Cidade = cidadeEsperada;

            dto.Cidade.Should().Be(cidadeEsperada);
        }

        [Fact]
        public void DadoCidadeNula_QuandoAtribuir_EntaoCidadeDeveSerNula()
        {
            var dto = new DadosUsuarioDTO();

            dto.Cidade = null;

            dto.Cidade.Should().BeNull();
        }

        [Fact]
        public void DadoEstadoDefinido_QuandoAtribuir_EntaoEstadoDeveSerSetado()
        {
            var dto = new DadosUsuarioDTO();
            var estadoEsperado = "SP";

            dto.Estado = estadoEsperado;

            dto.Estado.Should().Be(estadoEsperado);
        }

        [Fact]
        public void DadoEstadoNulo_QuandoAtribuir_EntaoEstadoDeveSerNulo()
        {
            var dto = new DadosUsuarioDTO();

            dto.Estado = null;

            dto.Estado.Should().BeNull();
        }

        [Fact]
        public void DadoTipoDefinido_QuandoAtribuir_EntaoTipoDeveSerSetado()
        {
            var dto = new DadosUsuarioDTO();
            var tipoEsperado = 1;

            dto.Tipo = tipoEsperado;

            dto.Tipo.Should().Be(tipoEsperado);
        }

        [Fact]
        public void DadoTipoZero_QuandoAtribuir_EntaoTipoDeveSerZero()
        {
            var dto = new DadosUsuarioDTO();

            dto.Tipo = 0;

            dto.Tipo.Should().Be(0);
        }

        [Fact]
        public void DadoInstituicaoDefinida_QuandoAtribuir_EntaoInstituicaoDeveSerSetada()
        {
            var dto = new DadosUsuarioDTO();
            var instituicaoEsperada = "Prefeitura de São Paulo";

            dto.Instituicao = instituicaoEsperada;

            dto.Instituicao.Should().Be(instituicaoEsperada);
        }

        [Fact]
        public void DadoInstituicaoNula_QuandoAtribuir_EntaoInstituicaoDeveSerNula()
        {
            var dto = new DadosUsuarioDTO();

            dto.Instituicao = null;

            dto.Instituicao.Should().BeNull();
        }

        [Fact]
        public void DadoMultiplosDTOs_QuandoCriar_EntaoTodosDevemSerIndependentes()
        {
            var dto1 = new DadosUsuarioDTO
            {
                Nome = "João",
                Cpf = "123.456.789-01",
                Email = "joao@test.com",
                Tipo = 1
            };
            var dto2 = new DadosUsuarioDTO
            {
                Nome = "Maria",
                Cpf = "987.654.321-09",
                Email = "maria@test.com",
                Tipo = 2
            };

            dto1.Should().NotBeSameAs(dto2);
            dto1.Nome.Should().NotBe(dto2.Nome);
            dto1.Cpf.Should().NotBe(dto2.Cpf);
            dto1.Email.Should().NotBe(dto2.Email);
            dto1.Tipo.Should().NotBe(dto2.Tipo);
        }

        [Fact]
        public void DadoDTOComValoresAlterados_QuandoVerificar_EntaoAlteracoesDeveramSerReflexas()
        {
            var dto = new DadosUsuarioDTO();
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
            var faker = new Faker<DadosUsuarioDTO>("pt_BR")
                .RuleFor(x => x.Nome, f => f.Name.FullName())
                .RuleFor(x => x.Cpf, f => f.Person.Cpf())
                .RuleFor(x => x.Login, f => f.Internet.UserName())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Telefone, f => f.Phone.PhoneNumber("(##) 9####-####"))
                .RuleFor(x => x.Endereco, f => f.Address.StreetAddress())
                .RuleFor(x => x.Numero, f => f.Address.BuildingNumber())
                .RuleFor(x => x.Complemento, f => f.Address.SecondaryAddress())
                .RuleFor(x => x.Bairro, f => f.Address.City())
                .RuleFor(x => x.Cep, f => f.Address.ZipCode("##.###-###"))
                .RuleFor(x => x.Cidade, f => f.Address.City())
                .RuleFor(x => x.Estado, f => f.Address.StateAbbr())
                .RuleFor(x => x.Tipo, f => f.Random.Int(0, 5))
                .RuleFor(x => x.Instituicao, f => f.Company.CompanyName());

            var dto = faker.Generate();

            dto.Should().NotBeNull();
            dto.Nome.Should().NotBeNullOrEmpty();
            dto.Cpf.Should().NotBeNullOrEmpty();
            dto.Login.Should().NotBeNullOrEmpty();
            dto.Email.Should().NotBeNullOrEmpty();
            dto.Telefone.Should().NotBeNullOrEmpty();
            dto.Endereco.Should().NotBeNullOrEmpty();
            dto.Numero.Should().NotBeNullOrEmpty();
            dto.Complemento.Should().NotBeNullOrEmpty();
            dto.Bairro.Should().NotBeNullOrEmpty();
            dto.Cep.Should().NotBeNullOrEmpty();
            dto.Cidade.Should().NotBeNullOrEmpty();
            dto.Estado.Should().NotBeNullOrEmpty();
            dto.Tipo.Should().BeGreaterThanOrEqualTo(0);
            dto.Instituicao.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void DadoListaDeTresDTOs_QuandoGerarMultiplos_EntaoTodosDevemSerValidos()
        {
            var faker = new Faker<DadosUsuarioDTO>("pt_BR")
                .RuleFor(x => x.Nome, f => f.Name.FullName())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Cpf, f => f.Person.Cpf())
                .RuleFor(x => x.Tipo, f => f.Random.Int(0, 5));

            var dtos = faker.Generate(3).ToList();

            dtos.Should().HaveCount(3);
            dtos.Should().AllSatisfy(dto =>
            {
                dto.Nome.Should().NotBeNullOrEmpty();
                dto.Email.Should().NotBeNullOrEmpty();
                dto.Cpf.Should().NotBeNullOrEmpty();
                dto.Tipo.Should().BeGreaterThanOrEqualTo(0);
            });
        }

        [Fact]
        public void DadoDTOComPropriedadesDefinidas_QuandoAcessar_EntaoTodasDeveramEstarDisponíveis()
        {
            var propriedades = typeof(DadosUsuarioDTO).GetProperties();

            propriedades.Should().NotBeEmpty();
            propriedades.Should().Contain(p => p.Name == "Nome");
            propriedades.Should().Contain(p => p.Name == "Cpf");
            propriedades.Should().Contain(p => p.Name == "Login");
            propriedades.Should().Contain(p => p.Name == "Email");
            propriedades.Should().Contain(p => p.Name == "Telefone");
            propriedades.Should().Contain(p => p.Name == "Endereco");
            propriedades.Should().Contain(p => p.Name == "Numero");
            propriedades.Should().Contain(p => p.Name == "Complemento");
            propriedades.Should().Contain(p => p.Name == "Bairro");
            propriedades.Should().Contain(p => p.Name == "Cep");
            propriedades.Should().Contain(p => p.Name == "Cidade");
            propriedades.Should().Contain(p => p.Name == "Estado");
            propriedades.Should().Contain(p => p.Name == "Tipo");
            propriedades.Should().Contain(p => p.Name == "Instituicao");
        }

        [Fact]
        public void DadoDTOComDadosCompletos_QuandoVerificarEquivalencia_EntaoDeveSerEquivalente()
        {
            var dto1 = new DadosUsuarioDTO
            {
                Nome = "Carlos José",
                Cpf = "123.456.789-09",
                Login = "carlos.jose",
                Email = "carlos@test.com",
                Telefone = "(11) 98765-4321",
                Endereco = "Rua A",
                Numero = "100",
                Complemento = "Apt 1",
                Bairro = "Centro",
                Cep = "01000-000",
                Cidade = "São Paulo",
                Estado = "SP",
                Tipo = 1,
                Instituicao = "Prefeitura"
            };

            var dto2 = new DadosUsuarioDTO
            {
                Nome = "Carlos José",
                Cpf = "123.456.789-09",
                Login = "carlos.jose",
                Email = "carlos@test.com",
                Telefone = "(11) 98765-4321",
                Endereco = "Rua A",
                Numero = "100",
                Complemento = "Apt 1",
                Bairro = "Centro",
                Cep = "01000-000",
                Cidade = "São Paulo",
                Estado = "SP",
                Tipo = 1,
                Instituicao = "Prefeitura"
            };

            dto1.Should().BeEquivalentTo(dto2);
        }

        [Fact]
        public void DadoNomeComEspacosEmBranco_QuandoAtribuir_EntaoDeveMantidoComEspacos()
        {
            var dto = new DadosUsuarioDTO();
            var nomeComEspacos = "  Maria Santos  ";

            dto.Nome = nomeComEspacos;

            dto.Nome.Should().Be(nomeComEspacos);
        }

        [Fact]
        public void DadoDTOComNomeGrandeTamanho_QuandoAtribuir_EntaoDeveSuportarGrandesTextos()
        {
            var dto = new DadosUsuarioDTO();
            var nomeLongo = string.Join(" ", new Faker("pt_BR").Lorem.Words(100));

            dto.Nome = nomeLongo;

            dto.Nome.Should().Be(nomeLongo);
            dto.Nome.Length.Should().BeGreaterThan(50);
        }

        [Fact]
        public void DadoDTOComCaracteresEspeciais_QuandoAtribuir_EntaoDeveSuportarCaracteresEspeciais()
        {
            var dto = new DadosUsuarioDTO();
            var nomeComEspeciais = "José da Silva-Oliveira & Cia. Ltda.";
            var emailComEspeciais = "usuario+teste@example.com.br";

            dto.Nome = nomeComEspeciais;
            dto.Email = emailComEspeciais;

            dto.Nome.Should().Be(nomeComEspeciais);
            dto.Email.Should().Be(emailComEspeciais);
        }

        [Fact]
        public void DadoTipoComValoresNumerosAltos_QuandoAtribuir_EntaoDeveSuportarValoresAltos()
        {
            var dto = new DadosUsuarioDTO();
            var tipoAlto = int.MaxValue;

            dto.Tipo = tipoAlto;

            dto.Tipo.Should().Be(tipoAlto);
        }

        [Fact]
        public void DadoTipoComValoresNumerosNegativos_QuandoAtribuir_EntaoDeveSuportarValoresNegativos()
        {
            var dto = new DadosUsuarioDTO();
            var tipoNegativo = -1;

            dto.Tipo = tipoNegativo;

            dto.Tipo.Should().Be(tipoNegativo);
        }

        [Fact]
        public void DadoDTOComStringVazia_QuandoAtribuir_EntaoStringVaziaDeveSerMantida()
        {
            var dto = new DadosUsuarioDTO();
            dto.Nome = "";
            dto.Cpf = "";
            dto.Login = "";

            dto.Nome.Should().BeEmpty();
            dto.Cpf.Should().BeEmpty();
            dto.Login.Should().BeEmpty();
        }

        [Fact]
        public void DadoDTOComUnicaragemEspecial_QuandoAtribuir_EntaoDeveSuportarUnicode()
        {
            var dto = new DadosUsuarioDTO();
            var nomeUnicode = "Thiago 日本語 Ñoño ñ";

            dto.Nome = nomeUnicode;

            dto.Nome.Should().Be(nomeUnicode);
        }

        [Fact]
        public void DadoDTOComMultiplosCamposNulos_QuandoVerificar_EntaoTodosDevemSerNulos()
        {
            var dto = new DadosUsuarioDTO();
            dto.Nome = null;
            dto.Cpf = null;
            dto.Login = null;
            dto.Email = null;
            dto.Telefone = null;
            dto.Endereco = null;
            dto.Numero = null;
            dto.Complemento = null;
            dto.Bairro = null;
            dto.Cep = null;
            dto.Cidade = null;
            dto.Estado = null;
            dto.Instituicao = null;

            dto.Nome.Should().BeNull();
            dto.Cpf.Should().BeNull();
            dto.Login.Should().BeNull();
            dto.Email.Should().BeNull();
            dto.Telefone.Should().BeNull();
            dto.Endereco.Should().BeNull();
            dto.Numero.Should().BeNull();
            dto.Complemento.Should().BeNull();
            dto.Bairro.Should().BeNull();
            dto.Cep.Should().BeNull();
            dto.Cidade.Should().BeNull();
            dto.Estado.Should().BeNull();
            dto.Instituicao.Should().BeNull();
        }

        [Fact]
        public void DadoDTOApenasComTipo_QuandoCriar_EntaoApenastTipoDeveSerInitializado()
        {
            var dto = new DadosUsuarioDTO { Tipo = 5 };

            dto.Tipo.Should().Be(5);
            dto.Nome.Should().BeNull();
            dto.Cpf.Should().BeNull();
            dto.Email.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComValoresAlteradosSequencialmente_QuandoVerificar_EntaoUltimaAlteracaoDevePredominar()
        {
            var dto = new DadosUsuarioDTO();

            dto.Tipo = 1;
            dto.Tipo = 2;
            dto.Tipo = 3;
            dto.Tipo = 4;

            dto.Tipo.Should().Be(4);
        }

        [Fact]
        public void DadoDTOComEnderecoCompleto_QuandoVerificar_EntaoTodosOsCamposDeEnderecoDevemEstarsSetados()
        {
            var dto = new DadosUsuarioDTO();
            dto.Endereco = "Rua Principal";
            dto.Numero = "100";
            dto.Complemento = "Apt 5";
            dto.Bairro = "Centro";
            dto.Cep = "01000-000";
            dto.Cidade = "São Paulo";
            dto.Estado = "SP";

            dto.Endereco.Should().Be("Rua Principal");
            dto.Numero.Should().Be("100");
            dto.Complemento.Should().Be("Apt 5");
            dto.Bairro.Should().Be("Centro");
            dto.Cep.Should().Be("01000-000");
            dto.Cidade.Should().Be("São Paulo");
            dto.Estado.Should().Be("SP");
        }

        [Fact]
        public void DadoDTOComContatoCompleto_QuandoVerificar_EntaoTodosOsCamposDeContatoDevemEstarsSetados()
        {
            var dto = new DadosUsuarioDTO();
            dto.Nome = "Ana Silva";
            dto.Email = "ana@example.com";
            dto.Telefone = "(11) 99999-8888";
            dto.Login = "ana.silva";

            dto.Nome.Should().Be("Ana Silva");
            dto.Email.Should().Be("ana@example.com");
            dto.Telefone.Should().Be("(11) 99999-8888");
            dto.Login.Should().Be("ana.silva");
        }

        [Fact]
        public void DadoDTOComCpfDocumentacao_QuandoVerificar_EntaoCpfDeveSerVarioFormatos()
        {
            var dto = new DadosUsuarioDTO();
            
            dto.Cpf = "12345678901";
            dto.Cpf.Should().Be("12345678901");

            dto.Cpf = "123.456.789-01";
            dto.Cpf.Should().Be("123.456.789-01");

            dto.Cpf = null;
            dto.Cpf.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComTesteDeInitializacao_QuandoCriar_EntaoNaoDevePossuirValoresPadrao()
        {
            var dto = new DadosUsuarioDTO();

            dto.Nome.Should().BeNull();
            dto.Cpf.Should().BeNull();
            dto.Login.Should().BeNull();
            dto.Email.Should().BeNull();
            dto.Telefone.Should().BeNull();
            dto.Endereco.Should().BeNull();
            dto.Numero.Should().BeNull();
            dto.Complemento.Should().BeNull();
            dto.Bairro.Should().BeNull();
            dto.Cep.Should().BeNull();
            dto.Cidade.Should().BeNull();
            dto.Estado.Should().BeNull();
            dto.Instituicao.Should().BeNull();

            dto.Tipo.Should().Be(0);
        }
    }
}
