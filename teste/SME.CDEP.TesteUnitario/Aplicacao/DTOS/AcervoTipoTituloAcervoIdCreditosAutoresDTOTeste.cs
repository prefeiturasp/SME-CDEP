using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoTipoTituloAcervoIdCreditosAutoresDtoTeste
    {
        private readonly Faker faker;

        public AcervoTipoTituloAcervoIdCreditosAutoresDtoTeste()
        {
            faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "DTO - Deve criar instância com todos os parâmetros")]
        public void Deve_criar_instancia_com_todos_parametros()
        {
            var tipoAcervo = faker.Lorem.Word();
            var acervoId = faker.Random.Long(1, 1000);
            var titulo = faker.Lorem.Sentence();
            var situacaoDisponibilidade = faker.Lorem.Word();
            var estaDisponivel = faker.Random.Bool();
            var temControleDisponibilidade = faker.Random.Bool();
            var autoresCreditos = faker.Make(3, () => faker.Name.FullName()).ToArray();
            var tipoAcervoId = TipoAcervo.Bibliografico;

            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                TipoAcervo = tipoAcervo,
                AcervoId = acervoId,
                Titulo = titulo,
                SituacaoDisponibilidade = situacaoDisponibilidade,
                EstaDisponivel = estaDisponivel,
                TemControleDisponibilidade = temControleDisponibilidade,
                AutoresCreditos = autoresCreditos,
                TipoAcervoId = tipoAcervoId
            };

            dto.TipoAcervo.Should().Be(tipoAcervo);
            dto.AcervoId.Should().Be(acervoId);
            dto.Titulo.Should().Be(titulo);
            dto.SituacaoDisponibilidade.Should().Be(situacaoDisponibilidade);
            dto.EstaDisponivel.Should().Be(estaDisponivel);
            dto.TemControleDisponibilidade.Should().Be(temControleDisponibilidade);
            dto.AutoresCreditos.Should().BeEquivalentTo(autoresCreditos);
            dto.TipoAcervoId.Should().Be(tipoAcervoId);
        }

        [Fact(DisplayName = "DTO - Deve criar instância com propriedades padrão")]
        public void Deve_criar_instancia_com_propriedades_padrao()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO();

            dto.TipoAcervo.Should().BeNull();
            dto.AcervoId.Should().Be(0);
            dto.Titulo.Should().BeNull();
            dto.SituacaoDisponibilidade.Should().BeNull();
            dto.EstaDisponivel.Should().BeFalse();
            dto.TemControleDisponibilidade.Should().BeFalse();
            dto.AutoresCreditos.Should().BeNull();
            dto.TipoAcervoId.Should().Be(default(TipoAcervo));
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de TipoAcervo após criação")]
        public void Deve_permitir_modificacao_tipo_acervo_apos_criacao()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO();
            var novoTipoAcervo = faker.Lorem.Word();

            dto.TipoAcervo = novoTipoAcervo;

            dto.TipoAcervo.Should().Be(novoTipoAcervo);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de AcervoId após criação")]
        public void Deve_permitir_modificacao_acervo_id_apos_criacao()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO();
            var novoAcervoId = faker.Random.Long(1, 1000);

            dto.AcervoId = novoAcervoId;

            dto.AcervoId.Should().Be(novoAcervoId);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Titulo após criação")]
        public void Deve_permitir_modificacao_titulo_apos_criacao()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO();
            var novoTitulo = faker.Lorem.Sentence();

            dto.Titulo = novoTitulo;

            dto.Titulo.Should().Be(novoTitulo);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de SituacaoDisponibilidade após criação")]
        public void Deve_permitir_modificacao_situacao_disponibilidade_apos_criacao()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO();
            var novaSituacao = faker.Lorem.Word();

            dto.SituacaoDisponibilidade = novaSituacao;

            dto.SituacaoDisponibilidade.Should().Be(novaSituacao);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de EstaDisponivel para verdadeiro")]
        public void Deve_permitir_modificacao_esta_disponivel_verdadeiro()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO();

            dto.EstaDisponivel = true;

            dto.EstaDisponivel.Should().BeTrue();
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de EstaDisponivel para falso")]
        public void Deve_permitir_modificacao_esta_disponivel_falso()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO { EstaDisponivel = true };

            dto.EstaDisponivel = false;

            dto.EstaDisponivel.Should().BeFalse();
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de TemControleDisponibilidade para verdadeiro")]
        public void Deve_permitir_modificacao_tem_controle_disponibilidade_verdadeiro()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO();

            dto.TemControleDisponibilidade = true;

            dto.TemControleDisponibilidade.Should().BeTrue();
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de TemControleDisponibilidade para falso")]
        public void Deve_permitir_modificacao_tem_controle_disponibilidade_falso()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO { TemControleDisponibilidade = true };

            dto.TemControleDisponibilidade = false;

            dto.TemControleDisponibilidade.Should().BeFalse();
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de AutoresCreditos após criação")]
        public void Deve_permitir_modificacao_autores_creditos_apos_criacao()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO();
            var novosAutoresCreditos = faker.Make(2, () => faker.Name.FullName()).ToArray();

            dto.AutoresCreditos = novosAutoresCreditos;

            dto.AutoresCreditos.Should().BeEquivalentTo(novosAutoresCreditos);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de TipoAcervoId após criação")]
        public void Deve_permitir_modificacao_tipo_acervo_id_apos_criacao()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO();
            var novoTipoAcervoId = TipoAcervo.Tridimensional;

            dto.TipoAcervoId = novoTipoAcervoId;

            dto.TipoAcervoId.Should().Be(novoTipoAcervoId);
        }

        [Fact(DisplayName = "DTO - Deve suportar AcervoId com valor zero")]
        public void Deve_suportar_acervo_id_zero()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                AcervoId = 0
            };

            dto.AcervoId.Should().Be(0);
        }

        [Fact(DisplayName = "DTO - Deve suportar AcervoId com valor máximo")]
        public void Deve_suportar_acervo_id_valor_maximo()
        {
            var acervoIdMaximo = long.MaxValue;

            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                AcervoId = acervoIdMaximo
            };

            dto.AcervoId.Should().Be(acervoIdMaximo);
        }

        [Fact(DisplayName = "DTO - Deve suportar strings vazias")]
        public void Deve_suportar_strings_vazias()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                TipoAcervo = string.Empty,
                Titulo = string.Empty,
                SituacaoDisponibilidade = string.Empty
            };

            dto.TipoAcervo.Should().Be(string.Empty);
            dto.Titulo.Should().Be(string.Empty);
            dto.SituacaoDisponibilidade.Should().Be(string.Empty);
        }

        [Fact(DisplayName = "DTO - Deve suportar strings com espaços em branco")]
        public void Deve_suportar_strings_com_espacos_branco()
        {
            var stringComEspacos = "  Texto com espaços  ";

            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                TipoAcervo = stringComEspacos,
                Titulo = stringComEspacos,
                SituacaoDisponibilidade = stringComEspacos
            };

            dto.TipoAcervo.Should().Be(stringComEspacos);
            dto.Titulo.Should().Be(stringComEspacos);
            dto.SituacaoDisponibilidade.Should().Be(stringComEspacos);
        }

        [Fact(DisplayName = "DTO - Deve suportar strings longas")]
        public void Deve_suportar_strings_longas()
        {
            var stringLonga = faker.Lorem.Paragraphs(5);

            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                TipoAcervo = stringLonga,
                Titulo = stringLonga,
                SituacaoDisponibilidade = stringLonga
            };

            dto.TipoAcervo.Should().Be(stringLonga);
            dto.Titulo.Should().Be(stringLonga);
            dto.SituacaoDisponibilidade.Should().Be(stringLonga);
        }

        [Fact(DisplayName = "DTO - Deve permitir AutoresCreditos nulo")]
        public void Deve_permitir_autores_creditos_nulo()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                AutoresCreditos = null!
            };

            dto.AutoresCreditos.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve suportar array vazio de AutoresCreditos")]
        public void Deve_suportar_array_vazio_autores_creditos()
        {
            var autoresVazio = Array.Empty<string>();

            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                AutoresCreditos = autoresVazio
            };

            dto.AutoresCreditos.Should().NotBeNull();
            dto.AutoresCreditos.Should().HaveCount(0);
        }

        [Fact(DisplayName = "DTO - Deve suportar array com um único autor")]
        public void Deve_suportar_array_um_autor()
        {
            var autores = new[] { faker.Name.FullName() };

            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                AutoresCreditos = autores
            };

            dto.AutoresCreditos.Should().HaveCount(1);
        }

        [Fact(DisplayName = "DTO - Deve suportar array com múltiplos autores")]
        public void Deve_suportar_array_multiplos_autores()
        {
            var autores = faker.Make(5, () => faker.Name.FullName()).ToArray();

            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                AutoresCreditos = autores
            };

            dto.AutoresCreditos.Should().HaveCount(5);
            dto.AutoresCreditos.Should().BeEquivalentTo(autores);
        }

        [Fact(DisplayName = "DTO - Deve suportar autores com caracteres especiais")]
        public void Deve_suportar_autores_com_caracteres_especiais()
        {
            var autoresEspeciais = new[]
            {
                "José da Silva",
                "François Müller",
                "李明 (Li Ming)",
                "O'Connor"
            };

            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                AutoresCreditos = autoresEspeciais
            };

            dto.AutoresCreditos.Should().BeEquivalentTo(autoresEspeciais);
        }

        [Theory(DisplayName = "DTO - Deve suportar todos os tipos de acervo")]
        [InlineData(TipoAcervo.Bibliografico)]
        [InlineData(TipoAcervo.Tridimensional)]
        [InlineData(TipoAcervo.Fotografico)]
        [InlineData(TipoAcervo.Audiovisual)]
        [InlineData(TipoAcervo.ArtesGraficas)]
        [InlineData(TipoAcervo.DocumentacaoTextual)]
        public void Deve_suportar_todos_tipos_acervo(TipoAcervo tipoAcervo)
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                TipoAcervoId = tipoAcervo
            };

            dto.TipoAcervoId.Should().Be(tipoAcervo);
        }

        [Fact(DisplayName = "DTO - Deve permitir múltiplas instâncias independentes")]
        public void Deve_permitir_multiplas_instancias_independentes()
        {
            var dto1 = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                AcervoId = 1,
                TipoAcervo = "Livro",
                Titulo = "Título 1",
                TipoAcervoId = TipoAcervo.Bibliografico,
                AutoresCreditos = new[] { "Autor 1" }
            };

            var dto2 = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                AcervoId = 2,
                TipoAcervo = "Periódico",
                Titulo = "Título 2",
                TipoAcervoId = TipoAcervo.Tridimensional,
                AutoresCreditos = new[] { "Autor 2" }
            };

            dto1.AcervoId.Should().Be(1);
            dto1.Titulo.Should().Be("Título 1");
            dto1.TipoAcervoId.Should().Be(TipoAcervo.Bibliografico);

            dto2.AcervoId.Should().Be(2);
            dto2.Titulo.Should().Be("Título 2");
            dto2.TipoAcervoId.Should().Be(TipoAcervo.Tridimensional);

            dto1.AcervoId.Should().NotBe(dto2.AcervoId);
        }

        [Fact(DisplayName = "DTO - Deve preservar valores ao atualizar múltiplas propriedades")]
        public void Deve_preservar_valores_ao_atualizar_multiplas_propriedades()
        {
            var autoresOriginal = new[] { "Autor 1", "Autor 2" };

            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                AcervoId = 1,
                TipoAcervo = "Livro",
                Titulo = "Título Original",
                TipoAcervoId = TipoAcervo.Bibliografico,
                AutoresCreditos = autoresOriginal,
                EstaDisponivel = true
            };

            var tituloOriginal = dto.Titulo;
            var tipoAcervoOriginal = dto.TipoAcervoId;
            var autoresOriginaisCopia = dto.AutoresCreditos;

            dto.Titulo = "Título Modificado";

            tituloOriginal.Should().Be("Título Original");
            dto.Titulo.Should().Be("Título Modificado");
            dto.TipoAcervoId.Should().Be(tipoAcervoOriginal);
            dto.AutoresCreditos.Should().BeEquivalentTo(autoresOriginaisCopia);
            dto.EstaDisponivel.Should().BeTrue();
        }

        [Fact(DisplayName = "DTO - Deve permitir valores nulos e não-nulos intercalados")]
        public void Deve_permitir_valores_nulos_nao_nulos_intercalados()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                TipoAcervo = "Livro",
                Titulo = null!,
                SituacaoDisponibilidade = "Disponível",
                AutoresCreditos = null!
            };

            dto.TipoAcervo.Should().NotBeNull();
            dto.Titulo.Should().BeNull();
            dto.SituacaoDisponibilidade.Should().NotBeNull();
            dto.AutoresCreditos.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve permitir reinicializar propriedades para nulo")]
        public void Deve_permitir_reinicializar_propriedades_para_nulo()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                TipoAcervo = "Livro",
                Titulo = "Título",
                SituacaoDisponibilidade = "Disponível",
                AutoresCreditos = new[] { "Autor 1" }
            };

            dto.TipoAcervo = null!;
            dto.Titulo = null!;
            dto.SituacaoDisponibilidade = null!;
            dto.AutoresCreditos = null!;

            dto.TipoAcervo.Should().BeNull();
            dto.Titulo.Should().BeNull();
            dto.SituacaoDisponibilidade.Should().BeNull();
            dto.AutoresCreditos.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve manter consistência entre leitura e escrita")]
        public void Deve_manter_consistencia_entre_leitura_escrita()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO();

            dto.AcervoId = 50;
            var primeiraLeitura = dto.AcervoId;
            primeiraLeitura.Should().Be(50);

            dto.AcervoId = 100;
            var segundaLeitura = dto.AcervoId;
            segundaLeitura.Should().Be(100);

            primeiraLeitura.Should().NotBe(segundaLeitura);
        }

        [Fact(DisplayName = "DTO - Deve suportar acesso sequencial às propriedades")]
        public void Deve_suportar_acesso_sequencial_propriedades()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                AcervoId = 1,
                TipoAcervo = "Livro",
                Titulo = "Teste",
                EstaDisponivel = true,
                TemControleDisponibilidade = false
            };

            var acervoId1 = dto.AcervoId;
            var tipoAcervo1 = dto.TipoAcervo;
            var titulo1 = dto.Titulo;
            var estaDisponivel1 = dto.EstaDisponivel;
            var temControle1 = dto.TemControleDisponibilidade;

            var acervoId2 = dto.AcervoId;
            var tipoAcervo2 = dto.TipoAcervo;
            var titulo2 = dto.Titulo;
            var estaDisponivel2 = dto.EstaDisponivel;
            var temControle2 = dto.TemControleDisponibilidade;

            acervoId1.Should().Be(acervoId2);
            tipoAcervo1.Should().Be(tipoAcervo2);
            titulo1.Should().Be(titulo2);
            estaDisponivel1.Should().Be(estaDisponivel2);
            temControle1.Should().Be(temControle2);
        }

        [Fact(DisplayName = "DTO - Deve permitir concatenar múltiplas instâncias em coleção")]
        public void Deve_permitir_concatenar_multiplas_instancias_colecao()
        {
            var dto1 = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                AcervoId = 1,
                Titulo = "Título 1",
                TipoAcervoId = TipoAcervo.Bibliografico,
                AutoresCreditos = new[] { "Autor 1" }
            };

            var dto2 = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                AcervoId = 2,
                Titulo = "Título 2",
                TipoAcervoId = TipoAcervo.Tridimensional,
                AutoresCreditos = new[] { "Autor 2" }
            };

            var dtos = new[] { dto1, dto2 };

            dtos.Should().HaveCount(2);
            dtos[0].AcervoId.Should().Be(1);
            dtos[1].AcervoId.Should().Be(2);
        }

        [Fact(DisplayName = "DTO - Deve permitir alternância entre tipos de acervo")]
        public void Deve_permitir_alternancia_entre_tipos_acervo()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO { TipoAcervoId = TipoAcervo.Bibliografico };

            dto.TipoAcervoId.Should().Be(TipoAcervo.Bibliografico);

            dto.TipoAcervoId = TipoAcervo.Tridimensional;
            dto.TipoAcervoId.Should().Be(TipoAcervo.Tridimensional);

            dto.TipoAcervoId = TipoAcervo.Fotografico;
            dto.TipoAcervoId.Should().Be(TipoAcervo.Fotografico);
        }

        [Fact(DisplayName = "DTO - Deve permitir atualização sequencial de todas as propriedades")]
        public void Deve_permitir_atualizacao_sequencial_todas_propriedades()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO();

            dto.AcervoId = 100;
            dto.AcervoId.Should().Be(100);

            dto.TipoAcervo = "Livro";
            dto.TipoAcervo.Should().Be("Livro");

            dto.TipoAcervoId = TipoAcervo.Bibliografico;
            dto.TipoAcervoId.Should().Be(TipoAcervo.Bibliografico);

            dto.Titulo = "Novo Título";
            dto.Titulo.Should().Be("Novo Título");

            dto.SituacaoDisponibilidade = "Disponível";
            dto.SituacaoDisponibilidade.Should().Be("Disponível");

            dto.EstaDisponivel = true;
            dto.EstaDisponivel.Should().BeTrue();

            dto.TemControleDisponibilidade = true;
            dto.TemControleDisponibilidade.Should().BeTrue();

            dto.AutoresCreditos = new[] { "Autor 1", "Autor 2" };
            dto.AutoresCreditos.Should().HaveCount(2);
        }

        [Fact(DisplayName = "DTO - Deve suportar combinações de booleanos")]
        public void Deve_suportar_combinacoes_booleanos()
        {
            // Ambos verdadeiros
            var dto1 = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                EstaDisponivel = true,
                TemControleDisponibilidade = true
            };
            dto1.EstaDisponivel.Should().BeTrue();
            dto1.TemControleDisponibilidade.Should().BeTrue();

            // Primeiro verdadeiro, segundo falso
            var dto2 = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                EstaDisponivel = true,
                TemControleDisponibilidade = false
            };
            dto2.EstaDisponivel.Should().BeTrue();
            dto2.TemControleDisponibilidade.Should().BeFalse();

            // Primeiro falso, segundo verdadeiro
            var dto3 = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                EstaDisponivel = false,
                TemControleDisponibilidade = true
            };
            dto3.EstaDisponivel.Should().BeFalse();
            dto3.TemControleDisponibilidade.Should().BeTrue();

            // Ambos falsos
            var dto4 = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                EstaDisponivel = false,
                TemControleDisponibilidade = false
            };
            dto4.EstaDisponivel.Should().BeFalse();
            dto4.TemControleDisponibilidade.Should().BeFalse();
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação alternada de AutoresCreditos")]
        public void Deve_permitir_modificacao_alternada_autores_creditos()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO();

            var autoresSet1 = new[] { "Autor 1", "Autor 2" };
            dto.AutoresCreditos = autoresSet1;
            dto.AutoresCreditos.Should().BeEquivalentTo(autoresSet1);

            var autoresSet2 = new[] { "Autor 3", "Autor 4", "Autor 5" };
            dto.AutoresCreditos = autoresSet2;
            dto.AutoresCreditos.Should().BeEquivalentTo(autoresSet2);

            dto.AutoresCreditos = null!;
            dto.AutoresCreditos.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve suportar SituacaoDisponibilidade com valores diversos")]
        public void Deve_suportar_situacao_disponibilidade_valores_diversos()
        {
            var situacoes = new[] { "Disponível", "Indisponível", "Em reparo", "Em processamento", "Sem acesso" };

            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO();

            foreach (var situacao in situacoes)
            {
                dto.SituacaoDisponibilidade = situacao;
                dto.SituacaoDisponibilidade.Should().Be(situacao);
            }
        }

        [Fact(DisplayName = "DTO - Cobertura 100% - Todos os getters e setters")]
        public void Cobertura_100_porcento_todos_getters_setters()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO();

            // Valores para atribuição
            var tipoAcervoValor = faker.Lorem.Word();
            var acervoIdValor = faker.Random.Long(1, 1000);
            var tituloValor = faker.Lorem.Sentence();
            var situacaoDisponibilidadeValor = faker.Lorem.Word();
            var estaDisponivelValor = faker.Random.Bool();
            var temControleDisponibilidadeValor = faker.Random.Bool();
            var autoresCreditosValor = faker.Make(3, () => faker.Name.FullName()).ToArray();
            var tipoAcervoIdValor = TipoAcervo.Tridimensional;

            // Atribuição de todos os setters
            dto.TipoAcervo = tipoAcervoValor;
            dto.AcervoId = acervoIdValor;
            dto.Titulo = tituloValor;
            dto.SituacaoDisponibilidade = situacaoDisponibilidadeValor;
            dto.EstaDisponivel = estaDisponivelValor;
            dto.TemControleDisponibilidade = temControleDisponibilidadeValor;
            dto.AutoresCreditos = autoresCreditosValor;
            dto.TipoAcervoId = tipoAcervoIdValor;

            // Validação de todos os getters
            dto.TipoAcervo.Should().Be(tipoAcervoValor);
            dto.AcervoId.Should().Be(acervoIdValor);
            dto.Titulo.Should().Be(tituloValor);
            dto.SituacaoDisponibilidade.Should().Be(situacaoDisponibilidadeValor);
            dto.EstaDisponivel.Should().Be(estaDisponivelValor);
            dto.TemControleDisponibilidade.Should().Be(temControleDisponibilidadeValor);
            dto.AutoresCreditos.Should().BeEquivalentTo(autoresCreditosValor);
            dto.TipoAcervoId.Should().Be(tipoAcervoIdValor);

            // Validações adicionais
            dto.Should().NotBeNull();
            dto.AcervoId.Should().BeGreaterThan(0);
            dto.TipoAcervo.Should().NotBeEmpty();
            dto.Titulo.Should().NotBeEmpty();
            dto.SituacaoDisponibilidade.Should().NotBeEmpty();
            dto.AutoresCreditos.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "DTO - Deve permitir array de autores vazio com booleanos verdadeiros")]
        public void Deve_permitir_array_autores_vazio_booleanos_verdadeiros()
        {
            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                AutoresCreditos = Array.Empty<string>(),
                EstaDisponivel = true,
                TemControleDisponibilidade = true
            };

            dto.AutoresCreditos.Should().HaveCount(0);
            dto.EstaDisponivel.Should().BeTrue();
            dto.TemControleDisponibilidade.Should().BeTrue();
        }

        [Fact(DisplayName = "DTO - Deve permitir múltiplas leituras sequenciais de AutoresCreditos")]
        public void Deve_permitir_multiplas_leituras_sequenciais_autores_creditos()
        {
            var autores = new[] { "Autor 1", "Autor 2", "Autor 3" };

            var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                AutoresCreditos = autores
            };

            var leitura1 = dto.AutoresCreditos;
            var leitura2 = dto.AutoresCreditos;
            var leitura3 = dto.AutoresCreditos;

            leitura1.Should().BeEquivalentTo(autores);
            leitura2.Should().BeEquivalentTo(autores);
            leitura3.Should().BeEquivalentTo(autores);

            leitura1.Should().BeSameAs(leitura2);
            leitura2.Should().BeSameAs(leitura3);
        }

        [Fact(DisplayName = "DTO - Deve preservar tipos de enum corretamente")]
        public void Deve_preservar_tipos_enum_corretamente()
        {
            var tiposAcervo = new[] 
            { 
                TipoAcervo.Bibliografico, 
                TipoAcervo.Tridimensional, 
                TipoAcervo.Fotografico,
                TipoAcervo.Audiovisual,
                TipoAcervo.ArtesGraficas,
                TipoAcervo.DocumentacaoTextual
            };

            foreach (var tipo in tiposAcervo)
            {
                var dto = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
                {
                    TipoAcervoId = tipo
                };

                dto.TipoAcervoId.Should().Be(tipo);
                ((object)dto.TipoAcervoId).Should().BeOfType<TipoAcervo>();
            }
        }

        [Fact(DisplayName = "DTO - Deve suportar integridade de dados entre instâncias")]
        public void Deve_suportar_integridade_dados_entre_instancias()
        {
            var autores1 = new[] { "Autor 1" };
            var autores2 = new[] { "Autor 2", "Autor 3" };

            var dto1 = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                AcervoId = 1,
                AutoresCreditos = autores1
            };

            var dto2 = new AcervoTipoTituloAcervoIdCreditosAutoresDTO
            {
                AcervoId = 2,
                AutoresCreditos = autores2
            };

            dto1.AutoresCreditos.Should().BeEquivalentTo(autores1);
            dto2.AutoresCreditos.Should().BeEquivalentTo(autores2);

            dto1.AutoresCreditos.Should().NotBeEquivalentTo(dto2.AutoresCreditos);
            dto1.AcervoId.Should().NotBe(dto2.AcervoId);
        }
    }
}
