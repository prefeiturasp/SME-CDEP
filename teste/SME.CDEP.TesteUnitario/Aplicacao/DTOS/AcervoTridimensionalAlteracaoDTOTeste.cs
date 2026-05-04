using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoTridimensionalAlteracaoDTOTeste
    {
        private readonly Faker faker;

        public AcervoTridimensionalAlteracaoDTOTeste()
        {
            faker = new Faker("pt_BR");
        }

        #region Testes de Instanciação

        [Fact(DisplayName = "DTO - Deve criar instância com todos os parâmetros")]
        public void Deve_criar_instancia_com_todos_parametros()
        {
            var id = faker.Random.Long(1, 1000);
            var acervoId = faker.Random.Long(1, 1000);
            var titulo = faker.Lorem.Sentence();
            var descricao = faker.Lorem.Paragraph();
            var codigo = faker.Random.AlphaNumeric(10);
            var codigoNovo = faker.Random.AlphaNumeric(10);
            var creditosAutoresIds = faker.Make(3, () => faker.Random.Long(1, 1000)).ToArray();
            var coAutores = faker.Make(2, () => new CoAutorDTO { CreditoAutorId = faker.Random.Long(1, 1000), CreditoAutorNome = faker.Name.FullName() }).ToArray();
            var subTitulo = faker.Lorem.Sentence();
            var dataAcervo = faker.Date.Recent().ToString("dd/MM/yyyy");
            var ano = faker.Date.Recent().Year.ToString();
            var situacaoAcervo = SituacaoAcervo.Ativo;
            var procedencia = faker.Lorem.Words(5).ToString();
            var conservacaoId = faker.Random.Long(1, 1000);
            var quantidade = faker.Random.Int(1, 100);
            var largura = faker.Random.Int(1, 500).ToString();
            var altura = faker.Random.Int(1, 500).ToString();
            var profundidade = faker.Random.Int(1, 500).ToString();
            var diametro = faker.Random.Int(1, 500).ToString();
            var arquivos = faker.Make(2, () => faker.Random.Long(1, 1000)).ToArray();

            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                Id = id,
                AcervoId = acervoId,
                Titulo = titulo,
                Descricao = descricao,
                Codigo = codigo,
                CodigoNovo = codigoNovo,
                CreditosAutoresIds = creditosAutoresIds,
                CoAutores = coAutores,
                SubTitulo = subTitulo,
                DataAcervo = dataAcervo,
                Ano = ano,
                SituacaoAcervo = situacaoAcervo,
                Procedencia = procedencia,
                ConservacaoId = conservacaoId,
                Quantidade = quantidade,
                Largura = largura,
                Altura = altura,
                Profundidade = profundidade,
                Diametro = diametro,
                Arquivos = arquivos
            };

            dto.Id.Should().Be(id);
            dto.AcervoId.Should().Be(acervoId);
            dto.Titulo.Should().Be(titulo);
            dto.Descricao.Should().Be(descricao);
            dto.Codigo.Should().Be(codigo);
            dto.CodigoNovo.Should().Be(codigoNovo);
            dto.CreditosAutoresIds.Should().Equal(creditosAutoresIds);
            dto.CoAutores.Should().Equal(coAutores);
            dto.SubTitulo.Should().Be(subTitulo);
            dto.DataAcervo.Should().Be(dataAcervo);
            dto.Ano.Should().Be(ano);
            dto.SituacaoAcervo.Should().Be(situacaoAcervo);
            dto.Procedencia.Should().Be(procedencia);
            dto.ConservacaoId.Should().Be(conservacaoId);
            dto.Quantidade.Should().Be(quantidade);
            dto.Largura.Should().Be(largura);
            dto.Altura.Should().Be(altura);
            dto.Profundidade.Should().Be(profundidade);
            dto.Diametro.Should().Be(diametro);
            dto.Arquivos.Should().Equal(arquivos);
        }

        [Fact(DisplayName = "DTO - Deve criar instância com propriedades padrão")]
        public void Deve_criar_instancia_com_propriedades_padrao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();

            dto.Id.Should().Be(0);
            dto.AcervoId.Should().Be(0);
            dto.Titulo.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.CodigoNovo.Should().BeNull();
            dto.CreditosAutoresIds.Should().BeNull();
            dto.CoAutores.Should().BeNull();
            dto.SubTitulo.Should().BeNull();
            dto.DataAcervo.Should().BeNull();
            dto.Ano.Should().BeNull();
            dto.SituacaoAcervo.Should().Be(default(SituacaoAcervo));
            dto.Procedencia.Should().BeNull();
            dto.ConservacaoId.Should().Be(0);
            dto.Quantidade.Should().Be(0);
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Profundidade.Should().BeNull();
            dto.Diametro.Should().BeNull();
            dto.Arquivos.Should().BeNull();
        }

        #endregion

        #region Testes de Propriedades Herdadas

        [Fact(DisplayName = "DTO - Deve permitir modificação de Titulo após criação")]
        public void Deve_permitir_modificacao_titulo_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novoTitulo = faker.Lorem.Sentence();

            dto.Titulo = novoTitulo;

            dto.Titulo.Should().Be(novoTitulo);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Descricao após criação")]
        public void Deve_permitir_modificacao_descricao_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novaDescricao = faker.Lorem.Paragraph();

            dto.Descricao = novaDescricao;

            dto.Descricao.Should().Be(novaDescricao);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Codigo após criação")]
        public void Deve_permitir_modificacao_codigo_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novoCodigo = faker.Random.AlphaNumeric(10);

            dto.Codigo = novoCodigo;

            dto.Codigo.Should().Be(novoCodigo);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de CodigoNovo após criação")]
        public void Deve_permitir_modificacao_codigo_novo_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novoCodigoNovo = faker.Random.AlphaNumeric(10);

            dto.CodigoNovo = novoCodigoNovo;

            dto.CodigoNovo.Should().Be(novoCodigoNovo);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de CreditosAutoresIds após criação")]
        public void Deve_permitir_modificacao_creditos_autores_ids_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novosCreditosAutoresIds = faker.Make(3, () => faker.Random.Long(1, 1000)).ToArray();

            dto.CreditosAutoresIds = novosCreditosAutoresIds;

            dto.CreditosAutoresIds.Should().Equal(novosCreditosAutoresIds);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de CoAutores após criação")]
        public void Deve_permitir_modificacao_coautores_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novosCoAutores = faker.Make(2, () => new CoAutorDTO { CreditoAutorId = faker.Random.Long(1, 1000), CreditoAutorNome = faker.Name.FullName() }).ToArray();

            dto.CoAutores = novosCoAutores;

            dto.CoAutores.Should().Equal(novosCoAutores);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de SubTitulo após criação")]
        public void Deve_permitir_modificacao_subtitulo_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novoSubTitulo = faker.Lorem.Sentence();

            dto.SubTitulo = novoSubTitulo;

            dto.SubTitulo.Should().Be(novoSubTitulo);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de DataAcervo após criação")]
        public void Deve_permitir_modificacao_data_acervo_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novaDataAcervo = faker.Date.Recent().ToString("dd/MM/yyyy");

            dto.DataAcervo = novaDataAcervo;

            dto.DataAcervo.Should().Be(novaDataAcervo);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Ano após criação")]
        public void Deve_permitir_modificacao_ano_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novoAno = faker.Date.Recent().Year.ToString();

            dto.Ano = novoAno;

            dto.Ano.Should().Be(novoAno);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de SituacaoAcervo após criação")]
        public void Deve_permitir_modificacao_situacao_acervo_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novaSituacaoAcervo = SituacaoAcervo.Inativo;

            dto.SituacaoAcervo = novaSituacaoAcervo;

            dto.SituacaoAcervo.Should().Be(novaSituacaoAcervo);
        }

        #endregion

        #region Testes de Propriedades Específicas

        [Fact(DisplayName = "DTO - Deve permitir modificação de Id após criação")]
        public void Deve_permitir_modificacao_id_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novoId = faker.Random.Long(1, 1000);

            dto.Id = novoId;

            dto.Id.Should().Be(novoId);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de AcervoId após criação")]
        public void Deve_permitir_modificacao_acervo_id_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novoAcervoId = faker.Random.Long(1, 1000);

            dto.AcervoId = novoAcervoId;

            dto.AcervoId.Should().Be(novoAcervoId);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Procedencia após criação")]
        public void Deve_permitir_modificacao_procedencia_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novaProcedencia = faker.Lorem.Words(5).ToString();

            dto.Procedencia = novaProcedencia;

            dto.Procedencia.Should().Be(novaProcedencia);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de ConservacaoId após criação")]
        public void Deve_permitir_modificacao_conservacao_id_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novaConservacaoId = faker.Random.Long(1, 1000);

            dto.ConservacaoId = novaConservacaoId;

            dto.ConservacaoId.Should().Be(novaConservacaoId);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Quantidade após criação")]
        public void Deve_permitir_modificacao_quantidade_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novaQuantidade = faker.Random.Int(1, 100);

            dto.Quantidade = novaQuantidade;

            dto.Quantidade.Should().Be(novaQuantidade);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Largura após criação")]
        public void Deve_permitir_modificacao_largura_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novaLargura = faker.Random.Int(1, 500).ToString();

            dto.Largura = novaLargura;

            dto.Largura.Should().Be(novaLargura);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Altura após criação")]
        public void Deve_permitir_modificacao_altura_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novaAltura = faker.Random.Int(1, 500).ToString();

            dto.Altura = novaAltura;

            dto.Altura.Should().Be(novaAltura);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Profundidade após criação")]
        public void Deve_permitir_modificacao_profundidade_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novaProfundidade = faker.Random.Int(1, 500).ToString();

            dto.Profundidade = novaProfundidade;

            dto.Profundidade.Should().Be(novaProfundidade);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Diametro após criação")]
        public void Deve_permitir_modificacao_diametro_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novoDiametro = faker.Random.Int(1, 500).ToString();

            dto.Diametro = novoDiametro;

            dto.Diametro.Should().Be(novoDiametro);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Arquivos após criação")]
        public void Deve_permitir_modificacao_arquivos_apos_criacao()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var novosArquivos = faker.Make(2, () => faker.Random.Long(1, 1000)).ToArray();

            dto.Arquivos = novosArquivos;

            dto.Arquivos.Should().Equal(novosArquivos);
        }

        #endregion

        #region Testes de Validação de Tipos

        [Fact(DisplayName = "DTO - Deve suportar Id com valor máximo")]
        public void Deve_suportar_id_valor_maximo()
        {
            var idMaximo = long.MaxValue;

            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                Id = idMaximo
            };

            dto.Id.Should().Be(idMaximo);
        }

        [Fact(DisplayName = "DTO - Deve suportar AcervoId com valor máximo")]
        public void Deve_suportar_acervo_id_valor_maximo()
        {
            var acervoIdMaximo = long.MaxValue;

            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                AcervoId = acervoIdMaximo
            };

            dto.AcervoId.Should().Be(acervoIdMaximo);
        }

        [Fact(DisplayName = "DTO - Deve suportar ConservacaoId com valor máximo")]
        public void Deve_suportar_conservacao_id_valor_maximo()
        {
            var conservacaoIdMaximo = long.MaxValue;

            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                ConservacaoId = conservacaoIdMaximo
            };

            dto.ConservacaoId.Should().Be(conservacaoIdMaximo);
        }

        [Fact(DisplayName = "DTO - Deve suportar Quantidade com valor máximo")]
        public void Deve_suportar_quantidade_valor_maximo()
        {
            var quantidadeMaxima = int.MaxValue;

            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                Quantidade = quantidadeMaxima
            };

            dto.Quantidade.Should().Be(quantidadeMaxima);
        }

        [Fact(DisplayName = "DTO - Deve suportar array vazio de CreditosAutoresIds")]
        public void Deve_suportar_array_vazio_creditos_autores_ids()
        {
            var creditosAutoresVazio = Array.Empty<long>();

            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                CreditosAutoresIds = creditosAutoresVazio
            };

            dto.CreditosAutoresIds.Should().NotBeNull();
            dto.CreditosAutoresIds.Should().HaveCount(0);
        }

        [Fact(DisplayName = "DTO - Deve suportar array com múltiplos CreditosAutoresIds")]
        public void Deve_suportar_array_multiplos_creditos_autores_ids()
        {
            var creditosAutoresIds = faker.Make(5, () => faker.Random.Long(1, 1000)).ToArray();

            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                CreditosAutoresIds = creditosAutoresIds
            };

            dto.CreditosAutoresIds.Should().HaveCount(5);
            dto.CreditosAutoresIds.Should().Equal(creditosAutoresIds);
        }

        [Fact(DisplayName = "DTO - Deve suportar array vazio de CoAutores")]
        public void Deve_suportar_array_vazio_coautores()
        {
            var coAutoresVazio = Array.Empty<CoAutorDTO>();

            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                CoAutores = coAutoresVazio
            };

            dto.CoAutores.Should().NotBeNull();
            dto.CoAutores.Should().HaveCount(0);
        }

        [Fact(DisplayName = "DTO - Deve suportar array com múltiplos CoAutores")]
        public void Deve_suportar_array_multiplos_coautores()
        {
            var coAutores = faker.Make(5, () => new CoAutorDTO { CreditoAutorId = faker.Random.Long(1, 1000), CreditoAutorNome = faker.Name.FullName() }).ToArray();

            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                CoAutores = coAutores
            };

            dto.CoAutores.Should().HaveCount(5);
            dto.CoAutores.Should().Equal(coAutores);
        }

        [Fact(DisplayName = "DTO - Deve suportar array vazio de Arquivos")]
        public void Deve_suportar_array_vazio_arquivos()
        {
            var arquivosVazio = Array.Empty<long>();

            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                Arquivos = arquivosVazio
            };

            dto.Arquivos.Should().NotBeNull();
            dto.Arquivos.Should().HaveCount(0);
        }

        [Fact(DisplayName = "DTO - Deve suportar array com múltiplos Arquivos")]
        public void Deve_suportar_array_multiplos_arquivos()
        {
            var arquivos = faker.Make(5, () => faker.Random.Long(1, 1000)).ToArray();

            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                Arquivos = arquivos
            };

            dto.Arquivos.Should().HaveCount(5);
            dto.Arquivos.Should().Equal(arquivos);
        }

        #endregion

        #region Testes de Strings

        [Fact(DisplayName = "DTO - Deve suportar strings vazias")]
        public void Deve_suportar_strings_vazias()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                Titulo = string.Empty,
                Descricao = string.Empty,
                Codigo = string.Empty,
                CodigoNovo = string.Empty,
                SubTitulo = string.Empty,
                DataAcervo = string.Empty,
                Ano = string.Empty,
                Procedencia = string.Empty,
                Largura = string.Empty,
                Altura = string.Empty,
                Profundidade = string.Empty,
                Diametro = string.Empty
            };

            dto.Titulo.Should().Be(string.Empty);
            dto.Descricao.Should().Be(string.Empty);
            dto.Codigo.Should().Be(string.Empty);
            dto.CodigoNovo.Should().Be(string.Empty);
            dto.SubTitulo.Should().Be(string.Empty);
            dto.DataAcervo.Should().Be(string.Empty);
            dto.Ano.Should().Be(string.Empty);
            dto.Procedencia.Should().Be(string.Empty);
            dto.Largura.Should().Be(string.Empty);
            dto.Altura.Should().Be(string.Empty);
            dto.Profundidade.Should().Be(string.Empty);
            dto.Diametro.Should().Be(string.Empty);
        }

        [Fact(DisplayName = "DTO - Deve suportar strings com espaços em branco")]
        public void Deve_suportar_strings_com_espacos_branco()
        {
            var stringComEspacos = "  Texto com espaços  ";

            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                Titulo = stringComEspacos,
                Descricao = stringComEspacos,
                Procedencia = stringComEspacos,
                Largura = stringComEspacos,
                Altura = stringComEspacos,
                Profundidade = stringComEspacos,
                Diametro = stringComEspacos
            };

            dto.Titulo.Should().Be(stringComEspacos);
            dto.Descricao.Should().Be(stringComEspacos);
            dto.Procedencia.Should().Be(stringComEspacos);
            dto.Largura.Should().Be(stringComEspacos);
            dto.Altura.Should().Be(stringComEspacos);
            dto.Profundidade.Should().Be(stringComEspacos);
            dto.Diametro.Should().Be(stringComEspacos);
        }

        [Fact(DisplayName = "DTO - Deve suportar strings longas")]
        public void Deve_suportar_strings_longas()
        {
            var stringLonga = faker.Lorem.Paragraphs(5);

            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                Titulo = stringLonga,
                Descricao = stringLonga,
                Procedencia = stringLonga
            };

            dto.Titulo.Should().Be(stringLonga);
            dto.Descricao.Should().Be(stringLonga);
            dto.Procedencia.Should().Be(stringLonga);
        }

        [Fact(DisplayName = "DTO - Deve permitir strings com caracteres especiais")]
        public void Deve_permitir_strings_com_caracteres_especiais()
        {
            var stringComEspeciais = "José da Silva - François Müller (李明) - O'Connor";

            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                Titulo = stringComEspeciais,
                Procedencia = stringComEspeciais
            };

            dto.Titulo.Should().Be(stringComEspeciais);
            dto.Procedencia.Should().Be(stringComEspeciais);
        }

        #endregion

        #region Testes de Situação

        [Theory(DisplayName = "DTO - Deve suportar todas as situações de acervo")]
        [InlineData(SituacaoAcervo.Ativo)]
        [InlineData(SituacaoAcervo.Inativo)]
        public void Deve_suportar_todas_situacoes_acervo(SituacaoAcervo situacao)
        {
            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                SituacaoAcervo = situacao
            };

            dto.SituacaoAcervo.Should().Be(situacao);
        }

        #endregion

        #region Testes de Múltiplas Instâncias

        [Fact(DisplayName = "DTO - Deve permitir múltiplas instâncias independentes")]
        public void Deve_permitir_multiplas_instancias_independentes()
        {
            var dto1 = new AcervoTridimensionalAlteracaoDTO
            {
                Id = 1,
                AcervoId = 100,
                Titulo = "Titulo 1",
                Procedencia = "Procedencia 1",
                ConservacaoId = 10,
                Quantidade = 5
            };

            var dto2 = new AcervoTridimensionalAlteracaoDTO
            {
                Id = 2,
                AcervoId = 200,
                Titulo = "Titulo 2",
                Procedencia = "Procedencia 2",
                ConservacaoId = 20,
                Quantidade = 10
            };

            dto1.Id.Should().Be(1);
            dto1.AcervoId.Should().Be(100);
            dto1.Titulo.Should().Be("Titulo 1");
            dto1.Quantidade.Should().Be(5);

            dto2.Id.Should().Be(2);
            dto2.AcervoId.Should().Be(200);
            dto2.Titulo.Should().Be("Titulo 2");
            dto2.Quantidade.Should().Be(10);

            dto1.Id.Should().NotBe(dto2.Id);
        }

        [Fact(DisplayName = "DTO - Deve preservar valores ao atualizar múltiplas propriedades")]
        public void Deve_preservar_valores_ao_atualizar_multiplas_propriedades()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                Id = 1,
                Titulo = "Titulo Original",
                Procedencia = "Procedencia Original",
                ConservacaoId = 10
            };

            var tituloOriginal = dto.Titulo;
            var procedenciaOriginal = dto.Procedencia;
            var conservacaoIdOriginal = dto.ConservacaoId;

            dto.Titulo = "Titulo Modificado";

            tituloOriginal.Should().Be("Titulo Original");
            dto.Titulo.Should().Be("Titulo Modificado");
            dto.Procedencia.Should().Be(procedenciaOriginal);
            dto.ConservacaoId.Should().Be(conservacaoIdOriginal);
        }

        #endregion

        #region Testes de Propriedades Nulas

        [Fact(DisplayName = "DTO - Deve permitir propriedades nulas opcionais")]
        public void Deve_permitir_propriedades_nulas_opcionais()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                Descricao = null,
                Codigo = null,
                CodigoNovo = null,
                CreditosAutoresIds = null,
                CoAutores = null,
                SubTitulo = null,
                DataAcervo = null,
                Largura = null,
                Altura = null,
                Profundidade = null,
                Diametro = null,
                Arquivos = null
            };

            dto.Descricao.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.CodigoNovo.Should().BeNull();
            dto.CreditosAutoresIds.Should().BeNull();
            dto.CoAutores.Should().BeNull();
            dto.SubTitulo.Should().BeNull();
            dto.DataAcervo.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Profundidade.Should().BeNull();
            dto.Diametro.Should().BeNull();
            dto.Arquivos.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve permitir alterar de nulo para valor")]
        public void Deve_permitir_alterar_de_nulo_para_valor()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                Largura = null,
                Altura = null,
                Profundidade = null
            };

            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Profundidade.Should().BeNull();

            var novaLargura = "100";
            var novaAltura = "200";
            var novaProfundidade = "150";

            dto.Largura = novaLargura;
            dto.Altura = novaAltura;
            dto.Profundidade = novaProfundidade;

            dto.Largura.Should().Be(novaLargura);
            dto.Altura.Should().Be(novaAltura);
            dto.Profundidade.Should().Be(novaProfundidade);
        }

        [Fact(DisplayName = "DTO - Deve permitir alterar de valor para nulo")]
        public void Deve_permitir_alterar_de_valor_para_nulo()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                Largura = "100",
                Altura = "200",
                Profundidade = "150"
            };

            dto.Largura = null;
            dto.Altura = null;
            dto.Profundidade = null;

            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Profundidade.Should().BeNull();
        }

        #endregion

        #region Testes de Consistência

        [Fact(DisplayName = "DTO - Deve manter consistência entre leitura e escrita")]
        public void Deve_manter_consistencia_entre_leitura_escrita()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();

            dto.Id = 50;
            var primeiraLeitura = dto.Id;
            primeiraLeitura.Should().Be(50);

            dto.Id = 100;
            var segundaLeitura = dto.Id;
            segundaLeitura.Should().Be(100);

            primeiraLeitura.Should().NotBe(segundaLeitura);
        }

        [Fact(DisplayName = "DTO - Deve suportar acesso sequencial às propriedades")]
        public void Deve_suportar_acesso_sequencial_propriedades()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO
            {
                Id = 1,
                AcervoId = 100,
                Titulo = "Teste",
                Procedencia = "Procedencia"
            };

            var id1 = dto.Id;
            var acervoId1 = dto.AcervoId;
            var titulo1 = dto.Titulo;
            var procedencia1 = dto.Procedencia;

            var id2 = dto.Id;
            var acervoId2 = dto.AcervoId;
            var titulo2 = dto.Titulo;
            var procedencia2 = dto.Procedencia;

            id1.Should().Be(id2);
            acervoId1.Should().Be(acervoId2);
            titulo1.Should().Be(titulo2);
            procedencia1.Should().Be(procedencia2);
        }

        #endregion

        #region Cobertura 100%

        [Fact(DisplayName = "DTO - Cobertura 100% - Todos os getters e setters")]
        public void Cobertura_100_porcento_todos_getters_setters()
        {
            var dto = new AcervoTridimensionalAlteracaoDTO();
            var idValor = faker.Random.Long(1, 1000);
            var acervoIdValor = faker.Random.Long(1, 1000);
            var tituloValor = faker.Lorem.Sentence();
            var descricaoValor = faker.Lorem.Paragraph();
            var codigoValor = faker.Random.AlphaNumeric(10);
            var codigoNovoValor = faker.Random.AlphaNumeric(10);
            var creditosAutoresIdsValor = faker.Make(2, () => faker.Random.Long(1, 1000)).ToArray();
            var coAutoresValor = faker.Make(2, () => new CoAutorDTO { CreditoAutorId = faker.Random.Long(1, 1000), CreditoAutorNome = faker.Name.FullName() }).ToArray();
            var subTituloValor = faker.Lorem.Sentence();
            var dataAcervoValor = faker.Date.Recent().ToString("dd/MM/yyyy");
            var anoValor = faker.Date.Recent().Year.ToString();
            var situacaoAcervoValor = SituacaoAcervo.Ativo;
            var procedenciaValor = faker.Lorem.Words(5).ToString();
            var conservacaoIdValor = faker.Random.Long(1, 1000);
            var quantidadeValor = faker.Random.Int(1, 100);
            var larguraValor = faker.Random.Int(1, 500).ToString();
            var alturaValor = faker.Random.Int(1, 500).ToString();
            var profundidadeValor = faker.Random.Int(1, 500).ToString();
            var diametroValor = faker.Random.Int(1, 500).ToString();
            var arquivosValor = faker.Make(2, () => faker.Random.Long(1, 1000)).ToArray();

            dto.Id = idValor;
            dto.AcervoId = acervoIdValor;
            dto.Titulo = tituloValor;
            dto.Descricao = descricaoValor;
            dto.Codigo = codigoValor;
            dto.CodigoNovo = codigoNovoValor;
            dto.CreditosAutoresIds = creditosAutoresIdsValor;
            dto.CoAutores = coAutoresValor;
            dto.SubTitulo = subTituloValor;
            dto.DataAcervo = dataAcervoValor;
            dto.Ano = anoValor;
            dto.SituacaoAcervo = situacaoAcervoValor;
            dto.Procedencia = procedenciaValor;
            dto.ConservacaoId = conservacaoIdValor;
            dto.Quantidade = quantidadeValor;
            dto.Largura = larguraValor;
            dto.Altura = alturaValor;
            dto.Profundidade = profundidadeValor;
            dto.Diametro = diametroValor;
            dto.Arquivos = arquivosValor;

            dto.Id.Should().Be(idValor);
            dto.AcervoId.Should().Be(acervoIdValor);
            dto.Titulo.Should().Be(tituloValor);
            dto.Descricao.Should().Be(descricaoValor);
            dto.Codigo.Should().Be(codigoValor);
            dto.CodigoNovo.Should().Be(codigoNovoValor);
            dto.CreditosAutoresIds.Should().Equal(creditosAutoresIdsValor);
            dto.CoAutores.Should().Equal(coAutoresValor);
            dto.SubTitulo.Should().Be(subTituloValor);
            dto.DataAcervo.Should().Be(dataAcervoValor);
            dto.Ano.Should().Be(anoValor);
            dto.SituacaoAcervo.Should().Be(situacaoAcervoValor);
            dto.Procedencia.Should().Be(procedenciaValor);
            dto.ConservacaoId.Should().Be(conservacaoIdValor);
            dto.Quantidade.Should().Be(quantidadeValor);
            dto.Largura.Should().Be(larguraValor);
            dto.Altura.Should().Be(alturaValor);
            dto.Profundidade.Should().Be(profundidadeValor);
            dto.Diametro.Should().Be(diametroValor);
            dto.Arquivos.Should().Equal(arquivosValor);

            dto.Should().NotBeNull();
            dto.Id.Should().BeGreaterThan(0);
            dto.AcervoId.Should().BeGreaterThan(0);
            dto.Titulo.Should().NotBeEmpty();
            dto.Procedencia.Should().NotBeEmpty();
            dto.ConservacaoId.Should().BeGreaterThan(0);
            dto.Quantidade.Should().BeGreaterThan(0);
        }

        [Fact(DisplayName = "DTO - Deve permitir concatenar múltiplas instâncias em coleção")]
        public void Deve_permitir_concatenar_multiplas_instancias_colecao()
        {
            var dto1 = new AcervoTridimensionalAlteracaoDTO
            {
                Id = 1,
                AcervoId = 100,
                Titulo = "Titulo 1",
                Procedencia = "Procedencia 1",
                ConservacaoId = 10,
                Quantidade = 5
            };

            var dto2 = new AcervoTridimensionalAlteracaoDTO
            {
                Id = 2,
                AcervoId = 200,
                Titulo = "Titulo 2",
                Procedencia = "Procedencia 2",
                ConservacaoId = 20,
                Quantidade = 10
            };

            var dtos = new[] { dto1, dto2 };

            dtos.Should().HaveCount(2);
            dtos[0].Id.Should().Be(1);
            dtos[1].Id.Should().Be(2);
            dtos[0].Quantidade.Should().Be(5);
            dtos[1].Quantidade.Should().Be(10);
        }

        #endregion
    }
}
