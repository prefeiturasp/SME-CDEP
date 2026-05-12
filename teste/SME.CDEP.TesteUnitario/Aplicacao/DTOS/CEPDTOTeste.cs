using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class CEPDTOTeste
    {
        private readonly Faker<CEPDTO> faker;

        public CEPDTOTeste()
        {
            faker = new Faker<CEPDTO>("pt_BR")
                .RuleFor(x => x.Cep, f => f.Address.ZipCode("#####-###"))
                .RuleFor(x => x.Logradouro, f => f.Address.StreetAddress())
                .RuleFor(x => x.Complemento, f => f.Address.SecondaryAddress())
                .RuleFor(x => x.Bairro, f => f.Address.SecondaryAddress())
                .RuleFor(x => x.Localidade, f => f.Address.City())
                .RuleFor(x => x.UF, f => f.Address.StateAbbr());
        }

        [Fact]
        public void DadoDTOValido_QuandoInstanciado_EntaoCepDeveSerPreenchido()
        {
            var cep = "01310-100";
            var dto = new CEPDTO
            {
                Cep = cep,
                Logradouro = "Avenida Paulista",
                Complemento = "Apt. 1000",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                UF = "SP"
            };

            dto.Cep.Should().Be(cep);
        }

        [Fact]
        public void DadoDTOValido_QuandoInstanciado_EntaoLogradouroDeveSerPreenchido()
        {
            var logradouro = "Avenida Paulista";
            var dto = new CEPDTO
            {
                Cep = "01310-100",
                Logradouro = logradouro,
                Complemento = "Apt. 1000",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                UF = "SP"
            };

            dto.Logradouro.Should().Be(logradouro);
        }

        [Fact]
        public void DadoDTOValido_QuandoInstanciado_EntaoComplementoDeveSerPreenchido()
        {
            var complemento = "Apt. 1000";
            var dto = new CEPDTO
            {
                Cep = "01310-100",
                Logradouro = "Avenida Paulista",
                Complemento = complemento,
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                UF = "SP"
            };

            dto.Complemento.Should().Be(complemento);
        }

        [Fact]
        public void DadoDTOValido_QuandoInstanciado_EntaoBairroDeveSerPreenchido()
        {
            var bairro = "Bela Vista";
            var dto = new CEPDTO
            {
                Cep = "01310-100",
                Logradouro = "Avenida Paulista",
                Complemento = "Apt. 1000",
                Bairro = bairro,
                Localidade = "São Paulo",
                UF = "SP"
            };

            dto.Bairro.Should().Be(bairro);
        }

        [Fact]
        public void DadoDTOValido_QuandoInstanciado_EntaoLocalidadeDeveSerPreenchida()
        {
            var localidade = "São Paulo";
            var dto = new CEPDTO
            {
                Cep = "01310-100",
                Logradouro = "Avenida Paulista",
                Complemento = "Apt. 1000",
                Bairro = "Bela Vista",
                Localidade = localidade,
                UF = "SP"
            };

            dto.Localidade.Should().Be(localidade);
        }

        [Fact]
        public void DadoDTOValido_QuandoInstanciado_EntaoUFDeveSerPreenchido()
        {
            var uf = "SP";
            var dto = new CEPDTO
            {
                Cep = "01310-100",
                Logradouro = "Avenida Paulista",
                Complemento = "Apt. 1000",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                UF = uf
            };

            dto.UF.Should().Be(uf);
        }

        [Fact]
        public void DadoDTOValido_QuandoInstanciado_EntaoTodosAtributosDevemSerPreenchidos()
        {
            var cep = "01310-100";
            var logradouro = "Avenida Paulista";
            var complemento = "Apt. 1000";
            var bairro = "Bela Vista";
            var localidade = "São Paulo";
            var uf = "SP";

            var dto = new CEPDTO
            {
                Cep = cep,
                Logradouro = logradouro,
                Complemento = complemento,
                Bairro = bairro,
                Localidade = localidade,
                UF = uf
            };

            dto.Cep.Should().Be(cep);
            dto.Logradouro.Should().Be(logradouro);
            dto.Complemento.Should().Be(complemento);
            dto.Bairro.Should().Be(bairro);
            dto.Localidade.Should().Be(localidade);
            dto.UF.Should().Be(uf);
        }

        [Fact]
        public void DadoDTOComCepNulo_QuandoInstanciado_EntaoCepDeveSerNulo()
        {
            var dto = new CEPDTO
            {
                Cep = null,
                Logradouro = "Avenida Paulista",
                Complemento = "Apt. 1000",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                UF = "SP"
            };

            dto.Cep.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComLogradouroNulo_QuandoInstanciado_EntaoLogradouroDeveSerNulo()
        {
            var dto = new CEPDTO
            {
                Cep = "01310-100",
                Logradouro = null,
                Complemento = "Apt. 1000",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                UF = "SP"
            };

            dto.Logradouro.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComComplementoNulo_QuandoInstanciado_EntaoComplementoDeveSerNulo()
        {
            var dto = new CEPDTO
            {
                Cep = "01310-100",
                Logradouro = "Avenida Paulista",
                Complemento = null,
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                UF = "SP"
            };

            dto.Complemento.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComBairroNulo_QuandoInstanciado_EntaoBairroDeveSerNulo()
        {
            var dto = new CEPDTO
            {
                Cep = "01310-100",
                Logradouro = "Avenida Paulista",
                Complemento = "Apt. 1000",
                Bairro = null,
                Localidade = "São Paulo",
                UF = "SP"
            };

            dto.Bairro.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComLocalidadeNula_QuandoInstanciado_EntaoLocalidadeDeveSerNula()
        {
            var dto = new CEPDTO
            {
                Cep = "01310-100",
                Logradouro = "Avenida Paulista",
                Complemento = "Apt. 1000",
                Bairro = "Bela Vista",
                Localidade = null,
                UF = "SP"
            };

            dto.Localidade.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComUFNulo_QuandoInstanciado_EntaoUFDeveSerNulo()
        {
            var dto = new CEPDTO
            {
                Cep = "01310-100",
                Logradouro = "Avenida Paulista",
                Complemento = "Apt. 1000",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                UF = null
            };

            dto.UF.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComStringVazia_QuandoInstanciado_EntaoAtributosDevemSerVazios()
        {
            var dto = new CEPDTO
            {
                Cep = string.Empty,
                Logradouro = string.Empty,
                Complemento = string.Empty,
                Bairro = string.Empty,
                Localidade = string.Empty,
                UF = string.Empty
            };

            dto.Cep.Should().Be(string.Empty);
            dto.Logradouro.Should().Be(string.Empty);
            dto.Complemento.Should().Be(string.Empty);
            dto.Bairro.Should().Be(string.Empty);
            dto.Localidade.Should().Be(string.Empty);
            dto.UF.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoDTOComCaracteresEspeciais_QuandoInstanciado_EntaoCaracteresDevemSerPreservados()
        {
            var cepComEspeciais = "01310-100";
            var logradouroComEspeciais = "Avenida Paulista, nº 1000";
            var complementoComEspeciais = "Apt. 1000 - Bloco A";
            var bairroComEspeciais = "Bela Vista (Centro)";
            var localidadeComEspeciais = "São Paulo-SP";
            var ufComEspeciais = "SP";

            var dto = new CEPDTO
            {
                Cep = cepComEspeciais,
                Logradouro = logradouroComEspeciais,
                Complemento = complementoComEspeciais,
                Bairro = bairroComEspeciais,
                Localidade = localidadeComEspeciais,
                UF = ufComEspeciais
            };

            dto.Cep.Should().Be(cepComEspeciais);
            dto.Logradouro.Should().Be(logradouroComEspeciais);
            dto.Complemento.Should().Be(complementoComEspeciais);
            dto.Bairro.Should().Be(bairroComEspeciais);
            dto.Localidade.Should().Be(localidadeComEspeciais);
            dto.UF.Should().Be(ufComEspeciais);
        }

        [Fact]
        public void DadoDTOCriadoComBogus_QuandoInstanciado_EntaoTodosAtributosDevemSerValidados()
        {
            var dto = faker.Generate();

            dto.Cep.Should().NotBeNullOrEmpty();
            dto.Logradouro.Should().NotBeNullOrEmpty();
            dto.Complemento.Should().NotBeNullOrEmpty();
            dto.Bairro.Should().NotBeNullOrEmpty();
            dto.Localidade.Should().NotBeNullOrEmpty();
            dto.UF.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void DadoMultiplosDTOs_QuandoComparadosValores_EntaoDevemConterPropriedadesDistintas()
        {
            var dto1 = faker.Generate();
            var dto2 = faker.Generate();

            dto1.Cep.Should().NotBe(dto2.Cep);
            dto1.Logradouro.Should().NotBe(dto2.Logradouro);
        }

        [Fact]
        public void DadoDTOAlternandoValoresCep_QuandoModificado_EntaoDeveReflitirANovaAtribuicao()
        {
            var dto = new CEPDTO
            {
                Cep = "01310-100",
                Logradouro = "Avenida Paulista",
                Complemento = "Apt. 1000",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                UF = "SP"
            };

            dto.Cep.Should().Be("01310-100");

            dto.Cep = "20040020";
            dto.Cep.Should().Be("20040020");

            dto.Cep = "70070930";
            dto.Cep.Should().Be("70070930");
        }

        [Fact]
        public void DadoDTOAlternandoValoresLogradouro_QuandoModificado_EntaoDeveReflitirANovaAtribuicao()
        {
            var dto = new CEPDTO
            {
                Cep = "01310-100",
                Logradouro = "Avenida Paulista",
                Complemento = "Apt. 1000",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                UF = "SP"
            };

            dto.Logradouro.Should().Be("Avenida Paulista");

            dto.Logradouro = "Rua Augusta";
            dto.Logradouro.Should().Be("Rua Augusta");

            dto.Logradouro = "Avenida Imigrantes";
            dto.Logradouro.Should().Be("Avenida Imigrantes");
        }

        [Fact]
        public void DadoDTOAlternandoValoresComplemento_QuandoModificado_EntaoDeveReflitirANovaAtribuicao()
        {
            var dto = new CEPDTO
            {
                Cep = "01310-100",
                Logradouro = "Avenida Paulista",
                Complemento = "Apt. 1000",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                UF = "SP"
            };

            dto.Complemento.Should().Be("Apt. 1000");

            dto.Complemento = "Apt. 2000";
            dto.Complemento.Should().Be("Apt. 2000");

            dto.Complemento = "Sala 500";
            dto.Complemento.Should().Be("Sala 500");
        }

        [Fact]
        public void DadoDTOAlternandoValoresBairro_QuandoModificado_EntaoDeveReflitirANovaAtribuicao()
        {
            var dto = new CEPDTO
            {
                Cep = "01310-100",
                Logradouro = "Avenida Paulista",
                Complemento = "Apt. 1000",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                UF = "SP"
            };

            dto.Bairro.Should().Be("Bela Vista");

            dto.Bairro = "Centro";
            dto.Bairro.Should().Be("Centro");

            dto.Bairro = "Zona Sul";
            dto.Bairro.Should().Be("Zona Sul");
        }

        [Fact]
        public void DadoDTOAlternandoValoresLocalidade_QuandoModificado_EntaoDeveReflitirANovaAtribuicao()
        {
            var dto = new CEPDTO
            {
                Cep = "01310-100",
                Logradouro = "Avenida Paulista",
                Complemento = "Apt. 1000",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                UF = "SP"
            };

            dto.Localidade.Should().Be("São Paulo");

            dto.Localidade = "Rio de Janeiro";
            dto.Localidade.Should().Be("Rio de Janeiro");

            dto.Localidade = "Belo Horizonte";
            dto.Localidade.Should().Be("Belo Horizonte");
        }

        [Fact]
        public void DadoDTOAlternandoValoresUF_QuandoModificado_EntaoDeveReflitirANovaAtribuicao()
        {
            var dto = new CEPDTO
            {
                Cep = "01310-100",
                Logradouro = "Avenida Paulista",
                Complemento = "Apt. 1000",
                Bairro = "Bela Vista",
                Localidade = "São Paulo",
                UF = "SP"
            };

            dto.UF.Should().Be("SP");

            dto.UF = "RJ";
            dto.UF.Should().Be("RJ");

            dto.UF = "MG";
            dto.UF.Should().Be("MG");
        }

        [Fact]
        public void DadoDTOVazio_QuandoInstanciado_EntaoTodosAtributosDevemSerNulos()
        {
            var dto = new CEPDTO();

            dto.Cep.Should().BeNull();
            dto.Logradouro.Should().BeNull();
            dto.Complemento.Should().BeNull();
            dto.Bairro.Should().BeNull();
            dto.Localidade.Should().BeNull();
            dto.UF.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComStringWhitespace_QuandoInstanciado_EntaoStringDeveSerPreservada()
        {
            var dto = new CEPDTO
            {
                Cep = "   ",
                Logradouro = "\t",
                Complemento = "\n",
                Bairro = "  Bairro  ",
                Localidade = " Localidade ",
                UF = " SP "
            };

            dto.Cep.Should().Be("   ");
            dto.Logradouro.Should().Be("\t");
            dto.Complemento.Should().Be("\n");
            dto.Bairro.Should().Be("  Bairro  ");
            dto.Localidade.Should().Be(" Localidade ");
            dto.UF.Should().Be(" SP ");
        }

        [Fact]
        public void DadoDTOComValoresMaximos_QuandoInstanciado_EntaoTodosAtributosDevemSerPreservados()
        {
            var cepLongo = new string('0', 1000);
            var logradouroLongo = new string('A', 1000);
            var complementoLongo = new string('B', 1000);
            var bairroLongo = new string('C', 1000);
            var localidadeLonga = new string('D', 1000);
            var ufLongo = new string('E', 1000);

            var dto = new CEPDTO
            {
                Cep = cepLongo,
                Logradouro = logradouroLongo,
                Complemento = complementoLongo,
                Bairro = bairroLongo,
                Localidade = localidadeLonga,
                UF = ufLongo
            };

            dto.Cep.Should().Be(cepLongo);
            dto.Logradouro.Should().Be(logradouroLongo);
            dto.Complemento.Should().Be(complementoLongo);
            dto.Bairro.Should().Be(bairroLongo);
            dto.Localidade.Should().Be(localidadeLonga);
            dto.UF.Should().Be(ufLongo);
        }
    }
}
