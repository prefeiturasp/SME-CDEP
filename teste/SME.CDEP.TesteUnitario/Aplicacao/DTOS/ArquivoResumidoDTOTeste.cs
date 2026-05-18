using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using Xunit;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class ArquivoResumidoDTOTeste
    {
        [Fact]
        public void DadoParametroValido_QuandoInstanciarDTO_EntaoTodasAsPropriedadesPodemSerAtribuidas()
        {
            var nome = new Faker().Lorem.Word();
            var codigo = Guid.NewGuid();
            var id = new Faker().Random.Long(1, 1000);

            var dto = new ArquivoResumidoDTO
            {
                Nome = nome,
                Codigo = codigo,
                Id = id
            };

            dto.Should().NotBeNull();
            dto.Nome.Should().Be(nome);
            dto.Codigo.Should().Be(codigo);
            dto.Id.Should().Be(id);
        }

        [Fact]
        public void DadoNomeComValor_QuandoInstanciarDTO_EntaoPropriedadeNomeArmazenaCorretamente()
        {
            var nome = "arquivo_teste.pdf";

            var dto = new ArquivoResumidoDTO { Nome = nome };

            dto.Nome.Should().Be(nome);
        }

        [Fact]
        public void DadoNomeNulo_QuandoInstanciarDTO_EntaoPropriedadeNomePermiteNulo()
        {
            var dto = new ArquivoResumidoDTO { Nome = null };

            dto.Nome.Should().BeNull();
        }

        [Fact]
        public void DadoNomeVazio_QuandoInstanciarDTO_EntaoPropriedadeNomeArmazenaVazio()
        {
            var dto = new ArquivoResumidoDTO { Nome = string.Empty };

            dto.Nome.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoCodigoValido_QuandoInstanciarDTO_EntaoPropriedadeCodigoArmazenaCorretamente()
        {
            var codigo = Guid.NewGuid();

            var dto = new ArquivoResumidoDTO { Codigo = codigo };

            dto.Codigo.Should().Be(codigo);
        }

        [Fact]
        public void DadoCodigoGuidVazio_QuandoInstanciarDTO_EntaoPropriedadeCodigoArmazenaGuidVazio()
        {
            var codigoVazio = Guid.Empty;

            var dto = new ArquivoResumidoDTO { Codigo = codigoVazio };

            dto.Codigo.Should().Be(codigoVazio);
        }

        [Fact]
        public void DadoIdValido_QuandoInstanciarDTO_EntaoPropriedadeIdArmazenaCorretamente()
        {
            var id = 42L;

            var dto = new ArquivoResumidoDTO { Id = id };

            dto.Id.Should().Be(id);
        }

        [Fact]
        public void DadoIdZero_QuandoInstanciarDTO_EntaoPropriedadeIdArmazenaZero()
        {
            var id = 0L;

            var dto = new ArquivoResumidoDTO { Id = id };

            dto.Id.Should().Be(id);
        }

        [Fact]
        public void DadoIdNegativo_QuandoInstanciarDTO_EntaoPropriedadeIdArmazenaNegativo()
        {
            var id = -1L;

            var dto = new ArquivoResumidoDTO { Id = id };

            dto.Id.Should().Be(id);
        }

        [Fact]
        public void DadoDTOSemValoresAtribuidos_QuandoInstanciar_EntaoPropriedadesTemValoresPadrao()
        {
            var dto = new ArquivoResumidoDTO();

            dto.Nome.Should().BeNull();
            dto.Codigo.Should().Be(Guid.Empty);
            dto.Id.Should().Be(0);
        }

        [Fact]
        public void DadoDoisDTOsComMesmosValores_QuandoComparados_EntaoSaoIguais()
        {
            var nome = "arquivo.pdf";
            var codigo = Guid.NewGuid();
            var id = 123L;

            var dto1 = new ArquivoResumidoDTO { Nome = nome, Codigo = codigo, Id = id };
            var dto2 = new ArquivoResumidoDTO { Nome = nome, Codigo = codigo, Id = id };

            dto1.Should().BeEquivalentTo(dto2);
        }

        [Fact]
        public void DadoDTOComMultiplosValoresAleatórios_QuandoInstanciar_EntaoMantemValoresCorretamente()
        {
            var faker = new Faker();
            var nomes = new[] { "doc.pdf", "imagem.jpg", "arquivo.xlsx", null, string.Empty };
            var guids = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.Empty };
            var ids = new[] { 1L, 100L, 0L, -1L, long.MaxValue };

            foreach (var nome in nomes)
            {
                foreach (var codigo in guids)
                {
                    foreach (var id in ids)
                    {
                        var dto = new ArquivoResumidoDTO
                        {
                            Nome = nome,
                            Codigo = codigo,
                            Id = id
                        };

                        dto.Nome.Should().Be(nome);
                        dto.Codigo.Should().Be(codigo);
                        dto.Id.Should().Be(id);
                    }
                }
            }
        }

        [Fact]
        public void DadoDTOInstanciado_QuandoAlterarPropriedades_EntaoNovasPropriedadesSaoArmazenadas()
        {
            var dto = new ArquivoResumidoDTO { Nome = "original", Codigo = Guid.NewGuid(), Id = 1 };
            var novoNome = "alterado";
            var novoCodigo = Guid.NewGuid();
            var novoId = 999L;

            dto.Nome = novoNome;
            dto.Codigo = novoCodigo;
            dto.Id = novoId;

            dto.Nome.Should().Be(novoNome);
            dto.Codigo.Should().Be(novoCodigo);
            dto.Id.Should().Be(novoId);
        }

        [Fact]
        public void DadoDTOComPropriedadesNulas_QuandoAlterarParaValoresValidos_EntaoPropriedadesArmazenamCorretamente()
        {
            var dto = new ArquivoResumidoDTO { Nome = null };
            var novoNome = "novo_nome.pdf";

            dto.Nome = novoNome;

            dto.Nome.Should().Be(novoNome);
        }

        [Fact]
        public void DadoNomeComCaracteresEspeciais_QuandoInstanciarDTO_EntaoPropriedadeNomeArmazenaCorretamente()
        {
            var nome = "arquivo_com-caracteres.especiais_2024!@#.pdf";

            var dto = new ArquivoResumidoDTO { Nome = nome };

            dto.Nome.Should().Be(nome);
        }

        [Fact]
        public void DadoNomeComEspacos_QuandoInstanciarDTO_EntaoPropriedadeNomeMantemEspacos()
        {
            var nome = "   arquivo com espaços   ";

            var dto = new ArquivoResumidoDTO { Nome = nome };

            dto.Nome.Should().Be(nome);
        }

        [Fact]
        public void DadoDTOInstanciado_QuandoVerificarTipo_EntaoEhDaTipoArquivoResumidoDTO()
        {
            var dto = new ArquivoResumidoDTO();

            dto.Should().BeOfType<ArquivoResumidoDTO>();
        }

        [Theory]
        [InlineData(1L)]
        [InlineData(100L)]
        [InlineData(long.MaxValue)]
        [InlineData(0L)]
        public void DadoIdComValoresVariados_QuandoInstanciarDTO_EntaoArmazenaCorretamente(long idValor)
        {
            var dto = new ArquivoResumidoDTO { Id = idValor };

            dto.Id.Should().Be(idValor);
        }

        [Fact]
        public void DadoMultiplosDTOsInstanciados_QuandoVerificarIndependencia_EntaoCadaDTOMantemSeusPropriosValores()
        {
            var dto1 = new ArquivoResumidoDTO { Nome = "arquivo1", Codigo = Guid.NewGuid(), Id = 1 };
            var dto2 = new ArquivoResumidoDTO { Nome = "arquivo2", Codigo = Guid.NewGuid(), Id = 2 };

            dto1.Nome.Should().Be("arquivo1");
            dto2.Nome.Should().Be("arquivo2");
            dto1.Id.Should().Be(1);
            dto2.Id.Should().Be(2);
            dto1.Codigo.Should().NotBe(dto2.Codigo);
        }
    }
}
