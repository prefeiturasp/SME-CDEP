using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoSolicitacaoRetornoCadastroDtoTeste
    {
        private readonly Faker faker;

        public AcervoSolicitacaoRetornoCadastroDtoTeste()
        {
            faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "DTO - Deve criar instância com todos os parâmetros")]
        public void Deve_criar_instancia_com_todos_parametros()
        {
            var podeCancelarSolicitacao = faker.Random.Bool();
            var item1 = new AcervoSolicitacaoItemRetornoCadastroDTO { Id = 1, Titulo = faker.Lorem.Sentence() };
            var item2 = new AcervoSolicitacaoItemRetornoCadastroDTO { Id = 2, Titulo = faker.Lorem.Sentence() };
            var itens = new List<AcervoSolicitacaoItemRetornoCadastroDTO> { item1, item2 };

            var dto = new AcervoSolicitacaoRetornoCadastroDTO
            {
                PodeCancelarSolicitacao = podeCancelarSolicitacao,
                Itens = itens
            };

            dto.PodeCancelarSolicitacao.Should().Be(podeCancelarSolicitacao);
            dto.Itens.Should().HaveCount(2);
            dto.Itens.Should().BeEquivalentTo(itens);
        }

        [Fact(DisplayName = "DTO - Deve criar instância com propriedades padrão")]
        public void Deve_criar_instancia_com_propriedades_padrao()
        {
            var dto = new AcervoSolicitacaoRetornoCadastroDTO();

            dto.PodeCancelarSolicitacao.Should().BeFalse();
            dto.Itens.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de PodeCancelarSolicitacao para verdadeiro")]
        public void Deve_permitir_modificacao_pode_cancelar_verdadeiro()
        {
            var dto = new AcervoSolicitacaoRetornoCadastroDTO();

            dto.PodeCancelarSolicitacao = true;

            dto.PodeCancelarSolicitacao.Should().BeTrue();
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de PodeCancelarSolicitacao para falso")]
        public void Deve_permitir_modificacao_pode_cancelar_falso()
        {
            var dto = new AcervoSolicitacaoRetornoCadastroDTO { PodeCancelarSolicitacao = true };

            dto.PodeCancelarSolicitacao = false;

            dto.PodeCancelarSolicitacao.Should().BeFalse();
        }

        [Fact(DisplayName = "DTO - Deve suportar lista vazia de itens")]
        public void Deve_suportar_lista_vazia_itens()
        {
            var itensVazio = new List<AcervoSolicitacaoItemRetornoCadastroDTO>();

            var dto = new AcervoSolicitacaoRetornoCadastroDTO
            {
                Itens = itensVazio
            };

            dto.Itens.Should().NotBeNull();
            dto.Itens.Should().HaveCount(0);
        }

        [Fact(DisplayName = "DTO - Deve suportar lista com um único item")]
        public void Deve_suportar_lista_um_item()
        {
            var item = new AcervoSolicitacaoItemRetornoCadastroDTO 
            { 
                Id = 1, 
                Titulo = faker.Lorem.Sentence() 
            };
            var itens = new List<AcervoSolicitacaoItemRetornoCadastroDTO> { item };

            var dto = new AcervoSolicitacaoRetornoCadastroDTO
            {
                Itens = itens
            };

            dto.Itens.Should().HaveCount(1);
            dto.Itens.First().Id.Should().Be(1);
        }

        [Fact(DisplayName = "DTO - Deve suportar lista com múltiplos itens")]
        public void Deve_suportar_lista_multiplos_itens()
        {
            var itens = faker.Make(10, (idx) => new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                Id = idx,
                Titulo = faker.Lorem.Sentence()
            }).ToList();

            var dto = new AcervoSolicitacaoRetornoCadastroDTO
            {
                Itens = itens
            };

            dto.Itens.Should().HaveCount(10);
            dto.Itens.Should().BeEquivalentTo(itens);
        }

        [Fact(DisplayName = "DTO - Deve permitir itens nulos")]
        public void Deve_permitir_itens_nulos()
        {
            var dto = new AcervoSolicitacaoRetornoCadastroDTO
            {
                Itens = null!
            };

            dto.Itens.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Itens após criação")]
        public void Deve_permitir_modificacao_itens_apos_criacao()
        {
            var dto = new AcervoSolicitacaoRetornoCadastroDTO();
            var novoItens = faker.Make(3, (idx) => new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                Id = idx,
                Titulo = faker.Lorem.Sentence()
            }).ToList();

            dto.Itens = novoItens;

            dto.Itens.Should().HaveCount(3);
            dto.Itens.Should().BeEquivalentTo(novoItens);
        }

        [Fact(DisplayName = "DTO - Deve permitir múltiplas instâncias independentes")]
        public void Deve_permitir_multiplas_instancias_independentes()
        {
            var itens1 = new List<AcervoSolicitacaoItemRetornoCadastroDTO>
            {
                new AcervoSolicitacaoItemRetornoCadastroDTO { Id = 1, Titulo = "Item 1" }
            };

            var itens2 = new List<AcervoSolicitacaoItemRetornoCadastroDTO>
            {
                new AcervoSolicitacaoItemRetornoCadastroDTO { Id = 2, Titulo = "Item 2" }
            };

            var dto1 = new AcervoSolicitacaoRetornoCadastroDTO
            {
                PodeCancelarSolicitacao = true,
                Itens = itens1
            };

            var dto2 = new AcervoSolicitacaoRetornoCadastroDTO
            {
                PodeCancelarSolicitacao = false,
                Itens = itens2
            };

            dto1.PodeCancelarSolicitacao.Should().BeTrue();
            dto1.Itens.First().Id.Should().Be(1);

            dto2.PodeCancelarSolicitacao.Should().BeFalse();
            dto2.Itens.First().Id.Should().Be(2);
        }

        [Fact(DisplayName = "DTO - Deve preservar valores ao atualizar múltiplas propriedades")]
        public void Deve_preservar_valores_ao_atualizar_multiplas_propriedades()
        {
            var itens = new List<AcervoSolicitacaoItemRetornoCadastroDTO>
            {
                new AcervoSolicitacaoItemRetornoCadastroDTO { Id = 1, Titulo = "Item 1" }
            };

            var dto = new AcervoSolicitacaoRetornoCadastroDTO
            {
                PodeCancelarSolicitacao = true,
                Itens = itens
            };

            var podeCancelarOriginal = dto.PodeCancelarSolicitacao;
            var itensOriginal = dto.Itens;

            dto.PodeCancelarSolicitacao = false;

            podeCancelarOriginal.Should().BeTrue();
            dto.PodeCancelarSolicitacao.Should().BeFalse();
            dto.Itens.Should().BeEquivalentTo(itensOriginal);
        }

        [Fact(DisplayName = "DTO - Deve suportar itens com dados completos")]
        public void Deve_suportar_itens_com_dados_completos()
        {
            var itens = new List<AcervoSolicitacaoItemRetornoCadastroDTO>
            {
                new AcervoSolicitacaoItemRetornoCadastroDTO
                {
                    Id = 1,
                    Titulo = faker.Lorem.Sentence(),
                    AcervoId = 100,
                    Situacao = "Ativo",
                    EstaDisponivel = true
                },
                new AcervoSolicitacaoItemRetornoCadastroDTO
                {
                    Id = 2,
                    Titulo = faker.Lorem.Sentence(),
                    AcervoId = 101,
                    Situacao = "Inativo",
                    EstaDisponivel = false
                }
            };

            var dto = new AcervoSolicitacaoRetornoCadastroDTO
            {
                PodeCancelarSolicitacao = true,
                Itens = itens
            };

            dto.Itens.Should().HaveCount(2);
            dto.Itens.First().Situacao.Should().Be("Ativo");
            dto.Itens.Last().Situacao.Should().Be("Inativo");
        }

        [Fact(DisplayName = "DTO - Deve permitir PodeCancelarSolicitacao verdadeiro com lista vazia")]
        public void Deve_permitir_pode_cancelar_verdadeiro_com_lista_vazia()
        {
            var dto = new AcervoSolicitacaoRetornoCadastroDTO
            {
                PodeCancelarSolicitacao = true,
                Itens = new List<AcervoSolicitacaoItemRetornoCadastroDTO>()
            };

            dto.PodeCancelarSolicitacao.Should().BeTrue();
            dto.Itens.Should().HaveCount(0);
        }

        [Fact(DisplayName = "DTO - Deve permitir PodeCancelarSolicitacao falso com múltiplos itens")]
        public void Deve_permitir_pode_cancelar_falso_com_multiplos_itens()
        {
            var itens = faker.Make(5, (idx) => new AcervoSolicitacaoItemRetornoCadastroDTO
            {
                Id = idx
            }).ToList();

            var dto = new AcervoSolicitacaoRetornoCadastroDTO
            {
                PodeCancelarSolicitacao = false,
                Itens = itens
            };

            dto.PodeCancelarSolicitacao.Should().BeFalse();
            dto.Itens.Should().HaveCount(5);
        }

        [Fact(DisplayName = "DTO - Deve permitir alterar itens de nulo para lista")]
        public void Deve_permitir_alterar_itens_de_nulo_para_lista()
        {
            var dto = new AcervoSolicitacaoRetornoCadastroDTO();

            dto.Itens.Should().BeNull();

            var novosItens = new List<AcervoSolicitacaoItemRetornoCadastroDTO>
            {
                new AcervoSolicitacaoItemRetornoCadastroDTO { Id = 1 }
            };

            dto.Itens = novosItens;

            dto.Itens.Should().NotBeNull();
            dto.Itens.Should().HaveCount(1);
        }

        [Fact(DisplayName = "DTO - Deve permitir alterar itens de lista para nulo")]
        public void Deve_permitir_alterar_itens_de_lista_para_nulo()
        {
            var itens = new List<AcervoSolicitacaoItemRetornoCadastroDTO>
            {
                new AcervoSolicitacaoItemRetornoCadastroDTO { Id = 1 }
            };

            var dto = new AcervoSolicitacaoRetornoCadastroDTO { Itens = itens };

            dto.Itens.Should().NotBeNull();

            dto.Itens = null!;

            dto.Itens.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve suportar IEnumerable com diferentes implementações")]
        public void Deve_suportar_ienumerable_diferentes_implementacoes()
        {
            var itensArray = new[]
            {
                new AcervoSolicitacaoItemRetornoCadastroDTO { Id = 1, Titulo = "Item 1" },
                new AcervoSolicitacaoItemRetornoCadastroDTO { Id = 2, Titulo = "Item 2" }
            };

            var dtoComArray = new AcervoSolicitacaoRetornoCadastroDTO
            {
                Itens = itensArray
            };

            dtoComArray.Itens.Should().HaveCount(2);
            dtoComArray.Itens.Should().BeEquivalentTo(itensArray);
        }

        [Fact(DisplayName = "DTO - Deve suportar IEnumerable com LINQ")]
        public void Deve_suportar_ienumerable_com_linq()
        {
            var itens = new List<AcervoSolicitacaoItemRetornoCadastroDTO>
            {
                new AcervoSolicitacaoItemRetornoCadastroDTO { Id = 1, Titulo = "Item 1", EstaDisponivel = true },
                new AcervoSolicitacaoItemRetornoCadastroDTO { Id = 2, Titulo = "Item 2", EstaDisponivel = false },
                new AcervoSolicitacaoItemRetornoCadastroDTO { Id = 3, Titulo = "Item 3", EstaDisponivel = true }
            };

            var itensDisponives = itens.Where(x => x.EstaDisponivel).ToList();

            var dto = new AcervoSolicitacaoRetornoCadastroDTO
            {
                PodeCancelarSolicitacao = true,
                Itens = itensDisponives
            };

            dto.Itens.Should().HaveCount(2);
            dto.Itens.All(x => x.EstaDisponivel).Should().BeTrue();
        }

        [Fact(DisplayName = "DTO - Cobertura 100% - Todos os getters e setters")]
        public void Cobertura_100_porcento_todos_getters_setters()
        {
            var dto = new AcervoSolicitacaoRetornoCadastroDTO();
            var podeCancelarSolicitacaoValor = true;
            var itensValor = new List<AcervoSolicitacaoItemRetornoCadastroDTO>
            {
                new AcervoSolicitacaoItemRetornoCadastroDTO
                {
                    Id = 1,
                    Titulo = faker.Lorem.Sentence(),
                    AcervoId = faker.Random.Long(1, 1000),
                    Situacao = faker.Lorem.Word(),
                    TipoAtendimento = faker.Lorem.Word(),
                    EstaDisponivel = faker.Random.Bool()
                },
                new AcervoSolicitacaoItemRetornoCadastroDTO
                {
                    Id = 2,
                    Titulo = faker.Lorem.Sentence(),
                    AcervoId = faker.Random.Long(1, 1000),
                    Situacao = faker.Lorem.Word(),
                    TipoAtendimento = faker.Lorem.Word(),
                    EstaDisponivel = faker.Random.Bool()
                },
                new AcervoSolicitacaoItemRetornoCadastroDTO
                {
                    Id = 3,
                    Titulo = faker.Lorem.Sentence(),
                    AcervoId = faker.Random.Long(1, 1000),
                    Situacao = faker.Lorem.Word(),
                    TipoAtendimento = faker.Lorem.Word(),
                    EstaDisponivel = faker.Random.Bool()
                },
                new AcervoSolicitacaoItemRetornoCadastroDTO
                {
                    Id = 4,
                    Titulo = faker.Lorem.Sentence(),
                    AcervoId = faker.Random.Long(1, 1000),
                    Situacao = faker.Lorem.Word(),
                    TipoAtendimento = faker.Lorem.Word(),
                    EstaDisponivel = faker.Random.Bool()
                },
                new AcervoSolicitacaoItemRetornoCadastroDTO
                {
                    Id = 5,
                    Titulo = faker.Lorem.Sentence(),
                    AcervoId = faker.Random.Long(1, 1000),
                    Situacao = faker.Lorem.Word(),
                    TipoAtendimento = faker.Lorem.Word(),
                    EstaDisponivel = faker.Random.Bool()
                }
            };

            dto.PodeCancelarSolicitacao = podeCancelarSolicitacaoValor;
            dto.Itens = itensValor;

            // Validar setters e getters
            dto.PodeCancelarSolicitacao.Should().Be(podeCancelarSolicitacaoValor);
            dto.PodeCancelarSolicitacao.Should().BeTrue();
            dto.Itens.Should().NotBeNull();
            dto.Itens.Should().HaveCount(5);
            dto.Itens.Should().BeEquivalentTo(itensValor);

            // Validar que cada item foi preservado
            var itensArray = dto.Itens.ToArray();
            itensArray[0].Id.Should().Be(1);
            itensArray[1].Id.Should().Be(2);
            itensArray[2].Id.Should().Be(3);
            itensArray[3].Id.Should().Be(4);
            itensArray[4].Id.Should().Be(5);

            // Validar que todos os títulos foram preenchidos
            itensArray.All(x => !string.IsNullOrEmpty(x.Titulo)).Should().BeTrue();
        }

        [Fact(DisplayName = "DTO - Deve manter consistência entre leitura e escrita")]
        public void Deve_manter_consistencia_entre_leitura_escrita()
        {
            var dto = new AcervoSolicitacaoRetornoCadastroDTO();

            // Escrita
            dto.PodeCancelarSolicitacao = true;
            var primeiraLeitura = dto.PodeCancelarSolicitacao;

            // Verificação
            primeiraLeitura.Should().BeTrue();

            // Nova escrita
            dto.PodeCancelarSolicitacao = false;
            var segundaLeitura = dto.PodeCancelarSolicitacao;

            // Verificação
            segundaLeitura.Should().BeFalse();
        }

        [Fact(DisplayName = "DTO - Deve suportar acesso sequencial às propriedades")]
        public void Deve_suportar_acesso_sequencial_propriedades()
        {
            var dto = new AcervoSolicitacaoRetornoCadastroDTO();

            // Acesso sequencial a getters
            var podeCancel1 = dto.PodeCancelarSolicitacao;
            var itens1 = dto.Itens;
            var podeCancel2 = dto.PodeCancelarSolicitacao;
            var itens2 = dto.Itens;

            podeCancel1.Should().Be(podeCancel2);
            itens1.Should().BeSameAs(itens2);
        }

        [Fact(DisplayName = "DTO - Deve permitir concatenar múltiplas instâncias em coleção")]
        public void Deve_permitir_concatenar_multiplas_instancias_colecao()
        {
            var dto1 = new AcervoSolicitacaoRetornoCadastroDTO
            {
                PodeCancelarSolicitacao = true,
                Itens = new List<AcervoSolicitacaoItemRetornoCadastroDTO> 
                { 
                    new AcervoSolicitacaoItemRetornoCadastroDTO { Id = 1 } 
                }
            };

            var dto2 = new AcervoSolicitacaoRetornoCadastroDTO
            {
                PodeCancelarSolicitacao = false,
                Itens = new List<AcervoSolicitacaoItemRetornoCadastroDTO> 
                { 
                    new AcervoSolicitacaoItemRetornoCadastroDTO { Id = 2 } 
                }
            };

            var dtos = new[] { dto1, dto2 };

            dtos.Should().HaveCount(2);
            dtos[0].PodeCancelarSolicitacao.Should().BeTrue();
            dtos[1].PodeCancelarSolicitacao.Should().BeFalse();
        }
    }
}
