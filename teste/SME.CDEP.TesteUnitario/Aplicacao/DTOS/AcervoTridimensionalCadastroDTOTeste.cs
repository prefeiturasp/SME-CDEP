using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoTridimensionalCadastroDTOTeste
    {
        private readonly Faker faker;

        public AcervoTridimensionalCadastroDTOTeste()
        {
            faker = new Faker("pt_BR");
        }

        #region Testes de Criação e Inicialização

        [Fact(DisplayName = "DTO - Deve criar instância com todos os parâmetros")]
        public void Deve_criar_instancia_com_todos_parametros()
        {
            var titulo = faker.Lorem.Sentence();
            var descricao = faker.Lorem.Paragraphs(1);
            var codigo = faker.Random.AlphaNumeric(10);
            var codigoNovo = faker.Random.AlphaNumeric(10);
            var creditosAutoresIds = new long[] { 1, 2, 3 };
            var subTitulo = faker.Lorem.Sentence();
            var dataAcervo = faker.Date.Recent().ToString("dd/MM/yyyy");
            var ano = faker.Date.Recent().Year.ToString();
            var procedencia = faker.Lorem.Word();
            var conservacaoId = faker.Random.Long(1, 1000);
            var quantidade = faker.Random.Int(1, 1000);
            var largura = faker.Random.Double(0.01, 100).ToString();
            var altura = faker.Random.Double(0.01, 100).ToString();
            var profundidade = faker.Random.Double(0.01, 100).ToString();
            var diametro = faker.Random.Double(0.01, 100).ToString();
            var arquivos = new long[] { 10, 20, 30 };

            var dto = new AcervoTridimensionalCadastroDTO
            {
                Titulo = titulo,
                Descricao = descricao,
                Codigo = codigo,
                CodigoNovo = codigoNovo,
                CreditosAutoresIds = creditosAutoresIds,
                SubTitulo = subTitulo,
                DataAcervo = dataAcervo,
                Ano = ano,
                SituacaoAcervo = SituacaoAcervo.Ativo,
                Procedencia = procedencia,
                ConservacaoId = conservacaoId,
                Quantidade = quantidade,
                Largura = largura,
                Altura = altura,
                Profundidade = profundidade,
                Diametro = diametro,
                Arquivos = arquivos
            };

            dto.Titulo.Should().Be(titulo);
            dto.Descricao.Should().Be(descricao);
            dto.Codigo.Should().Be(codigo);
            dto.CodigoNovo.Should().Be(codigoNovo);
            dto.CreditosAutoresIds.Should().Equal(creditosAutoresIds);
            dto.SubTitulo.Should().Be(subTitulo);
            dto.DataAcervo.Should().Be(dataAcervo);
            dto.Ano.Should().Be(ano);
            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Ativo);
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
            var dto = new AcervoTridimensionalCadastroDTO();

            dto.Titulo.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.CodigoNovo.Should().BeNull();
            dto.CreditosAutoresIds.Should().BeNull();
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

        #region Testes de Propriedade Procedencia

        [Fact(DisplayName = "DTO - Deve permitir modificação de Procedencia após criação")]
        public void Deve_permitir_modificacao_procedencia_apos_criacao()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var novaProcedencia = faker.Lorem.Word();

            dto.Procedencia = novaProcedencia;

            dto.Procedencia.Should().Be(novaProcedencia);
        }

        [Fact(DisplayName = "DTO - Deve suportar Procedencia vazia")]
        public void Deve_suportar_procedencia_vazia()
        {
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Procedencia = string.Empty
            };

            dto.Procedencia.Should().Be(string.Empty);
        }

        [Fact(DisplayName = "DTO - Deve suportar Procedencia nula")]
        public void Deve_suportar_procedencia_nula()
        {
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Procedencia = null
            };

            dto.Procedencia.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve suportar Procedencia com comprimento máximo (200 caracteres)")]
        public void Deve_suportar_procedencia_comprimento_maximo()
        {
            var procedenciaMaxima = faker.Random.String2(200);

            var dto = new AcervoTridimensionalCadastroDTO
            {
                Procedencia = procedenciaMaxima
            };

            dto.Procedencia.Should().HaveLength(200);
            dto.Procedencia.Should().Be(procedenciaMaxima);
        }

        [Fact(DisplayName = "DTO - Deve suportar Procedencia com caracteres especiais")]
        public void Deve_suportar_procedencia_caracteres_especiais()
        {
            var procedenciasEspeciais = new[]
            {
                "São Paulo - SP",
                "Recife/PE",
                "Portugal & Brasil",
                "José da Silva's Collection"
            };

            var dto = new AcervoTridimensionalCadastroDTO();
            foreach (var procedencia in procedenciasEspeciais)
            {
                dto.Procedencia = procedencia;
                dto.Procedencia.Should().Be(procedencia);
            }
        }

        [Fact(DisplayName = "DTO - Deve permitir atualizar Procedencia múltiplas vezes")]
        public void Deve_permitir_atualizar_procedencia_multiplas_vezes()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var procedencia1 = faker.Lorem.Word();
            var procedencia2 = faker.Lorem.Word();
            var procedencia3 = faker.Lorem.Word();

            dto.Procedencia = procedencia1;
            dto.Procedencia.Should().Be(procedencia1);

            dto.Procedencia = procedencia2;
            dto.Procedencia.Should().Be(procedencia2);

            dto.Procedencia = procedencia3;
            dto.Procedencia.Should().Be(procedencia3);
        }

        #endregion

        #region Testes de Propriedade ConservacaoId

        [Fact(DisplayName = "DTO - Deve permitir modificação de ConservacaoId após criação")]
        public void Deve_permitir_modificacao_conservacao_id_apos_criacao()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var novaConservacaoId = faker.Random.Long(1, 1000);

            dto.ConservacaoId = novaConservacaoId;

            dto.ConservacaoId.Should().Be(novaConservacaoId);
        }

        [Fact(DisplayName = "DTO - Deve suportar ConservacaoId com valor 1")]
        public void Deve_suportar_conservacao_id_valor_minimo()
        {
            var dto = new AcervoTridimensionalCadastroDTO
            {
                ConservacaoId = 1
            };

            dto.ConservacaoId.Should().Be(1);
        }

        [Fact(DisplayName = "DTO - Deve suportar ConservacaoId com valor máximo")]
        public void Deve_suportar_conservacao_id_valor_maximo()
        {
            var conservacaoIdMaxima = long.MaxValue;

            var dto = new AcervoTridimensionalCadastroDTO
            {
                ConservacaoId = conservacaoIdMaxima
            };

            dto.ConservacaoId.Should().Be(conservacaoIdMaxima);
        }

        [Fact(DisplayName = "DTO - Deve permitir atualizar ConservacaoId múltiplas vezes")]
        public void Deve_permitir_atualizar_conservacao_id_multiplas_vezes()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var conservacaoId1 = faker.Random.Long(1, 100);
            var conservacaoId2 = faker.Random.Long(101, 1000);

            dto.ConservacaoId = conservacaoId1;
            dto.ConservacaoId.Should().Be(conservacaoId1);

            dto.ConservacaoId = conservacaoId2;
            dto.ConservacaoId.Should().Be(conservacaoId2);
        }

        [Fact(DisplayName = "DTO - Deve suportar ConservacaoId com valor zero")]
        public void Deve_suportar_conservacao_id_zero()
        {
            var dto = new AcervoTridimensionalCadastroDTO
            {
                ConservacaoId = 0
            };

            dto.ConservacaoId.Should().Be(0);
        }

        #endregion

        #region Testes de Propriedade Quantidade

        [Fact(DisplayName = "DTO - Deve permitir modificação de Quantidade após criação")]
        public void Deve_permitir_modificacao_quantidade_apos_criacao()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var novaQuantidade = faker.Random.Int(1, 1000);

            dto.Quantidade = novaQuantidade;

            dto.Quantidade.Should().Be(novaQuantidade);
        }

        [Fact(DisplayName = "DTO - Deve suportar Quantidade com valor 1")]
        public void Deve_suportar_quantidade_valor_minimo()
        {
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Quantidade = 1
            };

            dto.Quantidade.Should().Be(1);
        }

        [Fact(DisplayName = "DTO - Deve suportar Quantidade com valor máximo")]
        public void Deve_suportar_quantidade_valor_maximo()
        {
            var quantidadeMaxima = int.MaxValue;

            var dto = new AcervoTridimensionalCadastroDTO
            {
                Quantidade = quantidadeMaxima
            };

            dto.Quantidade.Should().Be(quantidadeMaxima);
        }

        [Fact(DisplayName = "DTO - Deve permitir atualizar Quantidade múltiplas vezes")]
        public void Deve_permitir_atualizar_quantidade_multiplas_vezes()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var quantidade1 = faker.Random.Int(1, 100);
            var quantidade2 = faker.Random.Int(101, 1000);

            dto.Quantidade = quantidade1;
            dto.Quantidade.Should().Be(quantidade1);

            dto.Quantidade = quantidade2;
            dto.Quantidade.Should().Be(quantidade2);
        }

        [Fact(DisplayName = "DTO - Deve suportar Quantidade com valor zero")]
        public void Deve_suportar_quantidade_zero()
        {
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Quantidade = 0
            };

            dto.Quantidade.Should().Be(0);
        }

        #endregion

        #region Testes de Propriedades de Dimensão

        [Fact(DisplayName = "DTO - Deve permitir modificação de Largura após criação")]
        public void Deve_permitir_modificacao_largura_apos_criacao()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var novaLargura = faker.Random.Double(0.01, 100).ToString();

            dto.Largura = novaLargura;

            dto.Largura.Should().Be(novaLargura);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Altura após criação")]
        public void Deve_permitir_modificacao_altura_apos_criacao()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var novaAltura = faker.Random.Double(0.01, 100).ToString();

            dto.Altura = novaAltura;

            dto.Altura.Should().Be(novaAltura);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Profundidade após criação")]
        public void Deve_permitir_modificacao_profundidade_apos_criacao()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var novaProfundidade = faker.Random.Double(0.01, 100).ToString();

            dto.Profundidade = novaProfundidade;

            dto.Profundidade.Should().Be(novaProfundidade);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Diametro após criação")]
        public void Deve_permitir_modificacao_diametro_apos_criacao()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var novoDiametro = faker.Random.Double(0.01, 100).ToString();

            dto.Diametro = novoDiametro;

            dto.Diametro.Should().Be(novoDiametro);
        }

        [Fact(DisplayName = "DTO - Deve suportar dimensões nulas")]
        public void Deve_suportar_dimensoes_nulas()
        {
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Largura = null,
                Altura = null,
                Profundidade = null,
                Diametro = null
            };

            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Profundidade.Should().BeNull();
            dto.Diametro.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve suportar dimensões vazias")]
        public void Deve_suportar_dimensoes_vazias()
        {
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Largura = string.Empty,
                Altura = string.Empty,
                Profundidade = string.Empty,
                Diametro = string.Empty
            };

            dto.Largura.Should().Be(string.Empty);
            dto.Altura.Should().Be(string.Empty);
            dto.Profundidade.Should().Be(string.Empty);
            dto.Diametro.Should().Be(string.Empty);
        }

        [Fact(DisplayName = "DTO - Deve suportar dimensões com valores numéricos")]
        public void Deve_suportar_dimensoes_valores_numericos()
        {
            var valoresNumericos = new[] { "10.5", "20", "0.1", "100.999" };

            var dto = new AcervoTridimensionalCadastroDTO
            {
                Largura = valoresNumericos[0],
                Altura = valoresNumericos[1],
                Profundidade = valoresNumericos[2],
                Diametro = valoresNumericos[3]
            };

            dto.Largura.Should().Be(valoresNumericos[0]);
            dto.Altura.Should().Be(valoresNumericos[1]);
            dto.Profundidade.Should().Be(valoresNumericos[2]);
            dto.Diametro.Should().Be(valoresNumericos[3]);
        }

        [Fact(DisplayName = "DTO - Deve permitir atualizar todas as dimensões simultaneamente")]
        public void Deve_permitir_atualizar_todas_dimensoes_simultaneamente()
        {
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Largura = "10",
                Altura = "20",
                Profundidade = "30",
                Diametro = "5"
            };

            dto.Largura.Should().Be("10");
            dto.Altura.Should().Be("20");
            dto.Profundidade.Should().Be("30");
            dto.Diametro.Should().Be("5");
        }

        [Fact(DisplayName = "DTO - Deve permitir reinicializar dimensões para nulo")]
        public void Deve_permitir_reinicializar_dimensoes_para_nulo()
        {
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Largura = "10",
                Altura = "20",
                Profundidade = "30",
                Diametro = "5"
            };

            dto.Largura = null;
            dto.Altura = null;
            dto.Profundidade = null;
            dto.Diametro = null;

            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Profundidade.Should().BeNull();
            dto.Diametro.Should().BeNull();
        }

        #endregion

        #region Testes de Propriedade Arquivos

        [Fact(DisplayName = "DTO - Deve permitir modificação de Arquivos após criação")]
        public void Deve_permitir_modificacao_arquivos_apos_criacao()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var novosArquivos = new long[] { 1, 2, 3 };

            dto.Arquivos = novosArquivos;

            dto.Arquivos.Should().Equal(novosArquivos);
        }

        [Fact(DisplayName = "DTO - Deve suportar Arquivos nulo")]
        public void Deve_suportar_arquivos_nulo()
        {
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Arquivos = null
            };

            dto.Arquivos.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve suportar Arquivos vazio")]
        public void Deve_suportar_arquivos_vazio()
        {
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Arquivos = Array.Empty<long>()
            };

            dto.Arquivos.Should().BeEmpty();
        }

        [Fact(DisplayName = "DTO - Deve suportar Arquivos com um elemento")]
        public void Deve_suportar_arquivos_um_elemento()
        {
            var arquivos = new long[] { 123 };

            var dto = new AcervoTridimensionalCadastroDTO
            {
                Arquivos = arquivos
            };

            dto.Arquivos.Should().HaveCount(1);
            dto.Arquivos.Should().Equal(123);
        }

        [Fact(DisplayName = "DTO - Deve suportar Arquivos com múltiplos elementos")]
        public void Deve_suportar_arquivos_multiplos_elementos()
        {
            var arquivos = new long[] { 1, 2, 3, 4, 5 };

            var dto = new AcervoTridimensionalCadastroDTO
            {
                Arquivos = arquivos
            };

            dto.Arquivos.Should().HaveCount(5);
            dto.Arquivos.Should().Equal(arquivos);
        }

        [Fact(DisplayName = "DTO - Deve permitir atualizar Arquivos múltiplas vezes")]
        public void Deve_permitir_atualizar_arquivos_multiplas_vezes()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var arquivos1 = new long[] { 1, 2 };
            var arquivos2 = new long[] { 3, 4, 5 };

            dto.Arquivos = arquivos1;
            dto.Arquivos.Should().Equal(arquivos1);

            dto.Arquivos = arquivos2;
            dto.Arquivos.Should().Equal(arquivos2);
        }

        [Fact(DisplayName = "DTO - Deve suportar Arquivos com valores máximos")]
        public void Deve_suportar_arquivos_valores_maximos()
        {
            var arquivos = new long[] { long.MaxValue, long.MaxValue - 1 };

            var dto = new AcervoTridimensionalCadastroDTO
            {
                Arquivos = arquivos
            };

            dto.Arquivos.Should().Equal(arquivos);
        }

        #endregion

        #region Testes de Propriedades Herdadas

        [Fact(DisplayName = "DTO - Deve permitir modificação de Titulo (herdado) após criação")]
        public void Deve_permitir_modificacao_titulo_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var novoTitulo = faker.Lorem.Sentence();

            dto.Titulo = novoTitulo;

            dto.Titulo.Should().Be(novoTitulo);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Descricao (herdado) após criação")]
        public void Deve_permitir_modificacao_descricao_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var novaDescricao = faker.Lorem.Paragraphs(2);

            dto.Descricao = novaDescricao;

            dto.Descricao.Should().Be(novaDescricao);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Codigo (herdado) após criação")]
        public void Deve_permitir_modificacao_codigo_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var novoCodigo = faker.Random.AlphaNumeric(10);

            dto.Codigo = novoCodigo;

            dto.Codigo.Should().Be(novoCodigo);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de CodigoNovo (herdado) após criação")]
        public void Deve_permitir_modificacao_codigo_novo_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var novoCodigoNovo = faker.Random.AlphaNumeric(10);

            dto.CodigoNovo = novoCodigoNovo;

            dto.CodigoNovo.Should().Be(novoCodigoNovo);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de CreditosAutoresIds (herdado) após criação")]
        public void Deve_permitir_modificacao_creditos_autores_ids_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var novosCreditosAutoresIds = new long[] { 1, 2, 3 };

            dto.CreditosAutoresIds = novosCreditosAutoresIds;

            dto.CreditosAutoresIds.Should().Equal(novosCreditosAutoresIds);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de SubTitulo (herdado) após criação")]
        public void Deve_permitir_modificacao_subtitulo_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var novoSubTitulo = faker.Lorem.Sentence();

            dto.SubTitulo = novoSubTitulo;

            dto.SubTitulo.Should().Be(novoSubTitulo);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de DataAcervo (herdado) após criação")]
        public void Deve_permitir_modificacao_data_acervo_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var novaDataAcervo = faker.Date.Recent().ToString("dd/MM/yyyy");

            dto.DataAcervo = novaDataAcervo;

            dto.DataAcervo.Should().Be(novaDataAcervo);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Ano (herdado) após criação")]
        public void Deve_permitir_modificacao_ano_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var novoAno = faker.Date.Recent().Year.ToString();

            dto.Ano = novoAno;

            dto.Ano.Should().Be(novoAno);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de SituacaoAcervo (herdado) após criação")]
        public void Deve_permitir_modificacao_situacao_acervo_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var novaSituacaoAcervo = SituacaoAcervo.Inativo;

            dto.SituacaoAcervo = novaSituacaoAcervo;

            dto.SituacaoAcervo.Should().Be(novaSituacaoAcervo);
        }

        #endregion

        #region Testes de Herança e Cobertura Completa

        [Fact(DisplayName = "DTO - Deve instanciar como derivada de AcervoCadastroDTO")]
        public void Deve_instanciar_como_derivada_de_acervo_cadastro_dto()
        {
            var dto = new AcervoTridimensionalCadastroDTO();

            dto.Should().BeAssignableTo<AcervoCadastroDTO>();
        }

        [Fact(DisplayName = "DTO - Deve suportar todas as propriedades simultaneously")]
        public void Deve_suportar_todas_propriedades_simultaneously()
        {
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Titulo = faker.Lorem.Sentence(),
                Descricao = faker.Lorem.Paragraphs(1),
                Codigo = faker.Random.AlphaNumeric(10),
                CodigoNovo = faker.Random.AlphaNumeric(10),
                CreditosAutoresIds = new long[] { 1, 2 },
                SubTitulo = faker.Lorem.Sentence(),
                DataAcervo = faker.Date.Recent().ToString("dd/MM/yyyy"),
                Ano = faker.Date.Recent().Year.ToString(),
                SituacaoAcervo = SituacaoAcervo.Ativo,
                Procedencia = faker.Lorem.Word(),
                ConservacaoId = faker.Random.Long(1, 1000),
                Quantidade = faker.Random.Int(1, 1000),
                Largura = faker.Random.Double(0.01, 100).ToString(),
                Altura = faker.Random.Double(0.01, 100).ToString(),
                Profundidade = faker.Random.Double(0.01, 100).ToString(),
                Diametro = faker.Random.Double(0.01, 100).ToString(),
                Arquivos = new long[] { 10, 20 }
            };

            dto.Titulo.Should().NotBeNull();
            dto.Descricao.Should().NotBeNull();
            dto.Codigo.Should().NotBeNull();
            dto.CodigoNovo.Should().NotBeNull();
            dto.CreditosAutoresIds.Should().NotBeNull();
            dto.SubTitulo.Should().NotBeNull();
            dto.DataAcervo.Should().NotBeNull();
            dto.Ano.Should().NotBeNull();
            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Ativo);
            dto.Procedencia.Should().NotBeNull();
            dto.ConservacaoId.Should().BeGreaterThan(0);
            dto.Quantidade.Should().BeGreaterThan(0);
            dto.Largura.Should().NotBeNull();
            dto.Altura.Should().NotBeNull();
            dto.Profundidade.Should().NotBeNull();
            dto.Diametro.Should().NotBeNull();
            dto.Arquivos.Should().NotBeNull();
        }

        [Fact(DisplayName = "DTO - Cobertura 100% - Todos os getters e setters")]
        public void Cobertura_100_porcento_todos_getters_setters()
        {
            var dto = new AcervoTridimensionalCadastroDTO();
            var tituloValor = faker.Lorem.Sentence();
            var descricaoValor = faker.Lorem.Paragraphs(1);
            var codigoValor = faker.Random.AlphaNumeric(10);
            var codigoNovoValor = faker.Random.AlphaNumeric(10);
            var creditosAutoresIdsValor = new long[] { 1, 2, 3 };
            var subTituloValor = faker.Lorem.Sentence();
            var dataAcervoValor = faker.Date.Recent().ToString("dd/MM/yyyy");
            var anoValor = faker.Date.Recent().Year.ToString();
            var situacaoAcervoValor = SituacaoAcervo.Ativo;
            var procedenciaValor = faker.Lorem.Word();
            var conservacaoIdValor = faker.Random.Long(1, 1000);
            var quantidadeValor = faker.Random.Int(1, 1000);
            var larguraValor = faker.Random.Double(0.01, 100).ToString();
            var alturaValor = faker.Random.Double(0.01, 100).ToString();
            var profundidadeValor = faker.Random.Double(0.01, 100).ToString();
            var diametroValor = faker.Random.Double(0.01, 100).ToString();
            var arquivosValor = new long[] { 10, 20, 30 };

            dto.Titulo = tituloValor;
            dto.Descricao = descricaoValor;
            dto.Codigo = codigoValor;
            dto.CodigoNovo = codigoNovoValor;
            dto.CreditosAutoresIds = creditosAutoresIdsValor;
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

            dto.Titulo.Should().Be(tituloValor);
            dto.Descricao.Should().Be(descricaoValor);
            dto.Codigo.Should().Be(codigoValor);
            dto.CodigoNovo.Should().Be(codigoNovoValor);
            dto.CreditosAutoresIds.Should().Equal(creditosAutoresIdsValor);
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
        }

        [Fact(DisplayName = "DTO - Deve permitir valores nulos e não-nulos intercalados")]
        public void Deve_permitir_valores_nulos_nao_nulos_intercalados()
        {
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Titulo = faker.Lorem.Sentence(),
                Descricao = null,
                Codigo = faker.Random.AlphaNumeric(10),
                CodigoNovo = null,
                SubTitulo = faker.Lorem.Sentence(),
                DataAcervo = null,
                Procedencia = faker.Lorem.Word(),
                Largura = null,
                Altura = faker.Random.Double(0.01, 100).ToString(),
                Profundidade = null,
                Diametro = faker.Random.Double(0.01, 100).ToString(),
                Arquivos = null
            };

            dto.Titulo.Should().NotBeNull();
            dto.Descricao.Should().BeNull();
            dto.Codigo.Should().NotBeNull();
            dto.CodigoNovo.Should().BeNull();
            dto.SubTitulo.Should().NotBeNull();
            dto.DataAcervo.Should().BeNull();
            dto.Procedencia.Should().NotBeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().NotBeNull();
            dto.Profundidade.Should().BeNull();
            dto.Diametro.Should().NotBeNull();
            dto.Arquivos.Should().BeNull();
        }

        [Theory(DisplayName = "DTO - Deve suportar todos os valores de SituacaoAcervo")]
        [InlineData(SituacaoAcervo.Ativo)]
        [InlineData(SituacaoAcervo.Inativo)]
        public void Deve_suportar_todos_valores_situacao_acervo(SituacaoAcervo situacao)
        {
            var dto = new AcervoTridimensionalCadastroDTO
            {
                SituacaoAcervo = situacao
            };

            dto.SituacaoAcervo.Should().Be(situacao);
        }

        [Fact(DisplayName = "DTO - Deve permitir reinicializar propriedades para nulo")]
        public void Deve_permitir_reinicializar_propriedades_para_nulo()
        {
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Titulo = faker.Lorem.Sentence(),
                Descricao = faker.Lorem.Paragraphs(1),
                Codigo = faker.Random.AlphaNumeric(10),
                CodigoNovo = faker.Random.AlphaNumeric(10),
                SubTitulo = faker.Lorem.Sentence(),
                DataAcervo = faker.Date.Recent().ToString("dd/MM/yyyy"),
                Procedencia = faker.Lorem.Word(),
                Largura = faker.Random.Double(0.01, 100).ToString(),
                Altura = faker.Random.Double(0.01, 100).ToString(),
                Profundidade = faker.Random.Double(0.01, 100).ToString(),
                Diametro = faker.Random.Double(0.01, 100).ToString()
            };

            dto.Titulo = null;
            dto.Descricao = null;
            dto.Codigo = null;
            dto.CodigoNovo = null;
            dto.SubTitulo = null;
            dto.DataAcervo = null;
            dto.Procedencia = null;
            dto.Largura = null;
            dto.Altura = null;
            dto.Profundidade = null;
            dto.Diametro = null;

            dto.Titulo.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.CodigoNovo.Should().BeNull();
            dto.SubTitulo.Should().BeNull();
            dto.DataAcervo.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Profundidade.Should().BeNull();
            dto.Diametro.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve permitir múltiplas instâncias independentes")]
        public void Deve_permitir_multiplas_instancias_independentes()
        {
            var dto1 = new AcervoTridimensionalCadastroDTO
            {
                Titulo = "Escultura 1",
                Procedencia = "Rio de Janeiro",
                ConservacaoId = 1,
                Quantidade = 1
            };

            var dto2 = new AcervoTridimensionalCadastroDTO
            {
                Titulo = "Escultura 2",
                Procedencia = "São Paulo",
                ConservacaoId = 2,
                Quantidade = 2
            };

            dto1.Titulo.Should().Be("Escultura 1");
            dto1.Procedencia.Should().Be("Rio de Janeiro");
            dto1.ConservacaoId.Should().Be(1);
            dto1.Quantidade.Should().Be(1);

            dto2.Titulo.Should().Be("Escultura 2");
            dto2.Procedencia.Should().Be("São Paulo");
            dto2.ConservacaoId.Should().Be(2);
            dto2.Quantidade.Should().Be(2);

            dto1.Procedencia.Should().NotBe(dto2.Procedencia);
        }

        [Fact(DisplayName = "DTO - Deve preservar valores ao atualizar múltiplas propriedades")]
        public void Deve_preservar_valores_ao_atualizar_multiplas_propriedades()
        {
            var dto = new AcervoTridimensionalCadastroDTO
            {
                Titulo = "Título Original",
                Procedencia = "Procedência Original",
                ConservacaoId = 1,
                Quantidade = 5
            };

            var tituloOriginal = dto.Titulo;
            var procedenciaOriginal = dto.Procedencia;
            var quantidadeOriginal = dto.Quantidade;

            dto.Titulo = "Título Modificado";

            tituloOriginal.Should().Be("Título Original");
            dto.Titulo.Should().Be("Título Modificado");
            dto.Procedencia.Should().Be(procedenciaOriginal);
            dto.Quantidade.Should().Be(quantidadeOriginal);
        }

        #endregion
    }
}
