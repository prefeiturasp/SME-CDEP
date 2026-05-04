using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoTridimensionalDetalheDTOTeste
    {
        private readonly Faker faker;

        public AcervoTridimensionalDetalheDTOTeste()
        {
            faker = new Faker("pt_BR");
        }

        #region Testes de Criação e Inicialização

        [Fact(DisplayName = "DTO - Deve criar instância com todos os parâmetros")]
        public void Deve_criar_instancia_com_todos_parametros()
        {
            var titulo = faker.Lorem.Sentence();
            var codigo = faker.Random.AlphaNumeric(10);
            var ano = faker.Date.Recent().Year.ToString();
            var acervoId = faker.Random.Long(1, 1000);
            var enderecoImagemPadrao = faker.Internet.Url();
            var situacaoDisponibilidade = faker.Lorem.Word();
            var estaDisponivel = true;
            var temControleDisponibilidade = false;
            var tipoAcervoId = 3;
            var descricao = faker.Lorem.Paragraphs(1);
            var dataAcervo = faker.Date.Recent().ToString("dd/MM/yyyy");
            var procedencia = faker.Lorem.Word();
            var conservacao = faker.Lorem.Word();
            var quantidade = faker.Random.Long(1, 1000);
            var dimensoes = faker.Random.Double(0.01, 100).ToString();
            var imagens = new[]
            {
                new ImagemDTO { Original = faker.Internet.Url(), Thumbnail = faker.Internet.Url() },
                new ImagemDTO { Original = faker.Internet.Url(), Thumbnail = faker.Internet.Url() }
            };

            var dto = new AcervoTridimensionalDetalheDTO
            {
                Titulo = titulo,
                Codigo = codigo,
                Ano = ano,
                AcervoId = acervoId,
                EnderecoImagemPadrao = enderecoImagemPadrao,
                SituacaoDisponibilidade = situacaoDisponibilidade,
                EstaDisponivel = estaDisponivel,
                TemControleDisponibilidade = temControleDisponibilidade,
                TipoAcervoId = tipoAcervoId,
                Descricao = descricao,
                DataAcervo = dataAcervo,
                Procedencia = procedencia,
                Conservacao = conservacao,
                Quantidade = quantidade,
                Dimensoes = dimensoes,
                Imagens = imagens
            };

            dto.Titulo.Should().Be(titulo);
            dto.Codigo.Should().Be(codigo);
            dto.Ano.Should().Be(ano);
            dto.AcervoId.Should().Be(acervoId);
            dto.EnderecoImagemPadrao.Should().Be(enderecoImagemPadrao);
            dto.SituacaoDisponibilidade.Should().Be(situacaoDisponibilidade);
            dto.EstaDisponivel.Should().Be(estaDisponivel);
            dto.TemControleDisponibilidade.Should().Be(temControleDisponibilidade);
            dto.TipoAcervoId.Should().Be(tipoAcervoId);
            dto.Descricao.Should().Be(descricao);
            dto.DataAcervo.Should().Be(dataAcervo);
            dto.Procedencia.Should().Be(procedencia);
            dto.Conservacao.Should().Be(conservacao);
            dto.Quantidade.Should().Be(quantidade);
            dto.Dimensoes.Should().Be(dimensoes);
            dto.Imagens.Should().Equal(imagens);
        }

        [Fact(DisplayName = "DTO - Deve criar instância com propriedades padrão")]
        public void Deve_criar_instancia_com_propriedades_padrao()
        {
            var dto = new AcervoTridimensionalDetalheDTO();

            dto.Titulo.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.Ano.Should().BeNull();
            dto.AcervoId.Should().Be(0);
            dto.EnderecoImagemPadrao.Should().BeNull();
            dto.SituacaoDisponibilidade.Should().BeNull();
            dto.EstaDisponivel.Should().Be(false);
            dto.TemControleDisponibilidade.Should().Be(false);
            dto.TipoAcervoId.Should().Be(0);
            dto.Descricao.Should().BeNull();
            dto.DataAcervo.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.Conservacao.Should().BeNull();
            dto.Quantidade.Should().Be(0);
            dto.Dimensoes.Should().BeNull();
            dto.Imagens.Should().BeNull();
        }

        #endregion

        #region Testes de Propriedade Descricao (Específica do DetalheDTO)

        [Fact(DisplayName = "DTO - Deve permitir modificação de Descricao após criação")]
        public void Deve_permitir_modificacao_descricao_apos_criacao()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var novaDescricao = faker.Lorem.Paragraphs(2);

            dto.Descricao = novaDescricao;

            dto.Descricao.Should().Be(novaDescricao);
        }

        [Fact(DisplayName = "DTO - Deve suportar Descricao vazia")]
        public void Deve_suportar_descricao_vazia()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                Descricao = string.Empty
            };

            dto.Descricao.Should().Be(string.Empty);
        }

        [Fact(DisplayName = "DTO - Deve suportar Descricao nula")]
        public void Deve_suportar_descricao_nula()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                Descricao = null
            };

            dto.Descricao.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve suportar Descricao com comprimento máximo")]
        public void Deve_suportar_descricao_comprimento_maximo()
        {
            var descricaoMaxima = faker.Lorem.Paragraphs(10);

            var dto = new AcervoTridimensionalDetalheDTO
            {
                Descricao = descricaoMaxima
            };

            dto.Descricao.Should().NotBeNullOrEmpty();
            dto.Descricao.Should().Be(descricaoMaxima);
        }

        [Fact(DisplayName = "DTO - Deve suportar Descricao com caracteres especiais")]
        public void Deve_suportar_descricao_caracteres_especiais()
        {
            var descricoesEspeciais = new[]
            {
                "Descrição com acentuação",
                "Escrita em português - com hífen",
                "Texto com \"aspas\"",
                "Múltiplas linhas\ncom quebra"
            };

            var dto = new AcervoTridimensionalDetalheDTO();
            foreach (var descricao in descricoesEspeciais)
            {
                dto.Descricao = descricao;
                dto.Descricao.Should().Be(descricao);
            }
        }

        [Fact(DisplayName = "DTO - Deve permitir atualizar Descricao múltiplas vezes")]
        public void Deve_permitir_atualizar_descricao_multiplas_vezes()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var descricao1 = faker.Lorem.Paragraphs(1);
            var descricao2 = faker.Lorem.Paragraphs(2);
            var descricao3 = faker.Lorem.Paragraphs(3);

            dto.Descricao = descricao1;
            dto.Descricao.Should().Be(descricao1);

            dto.Descricao = descricao2;
            dto.Descricao.Should().Be(descricao2);

            dto.Descricao = descricao3;
            dto.Descricao.Should().Be(descricao3);
        }

        #endregion

        #region Testes de Propriedade DataAcervo (Específica do DetalheDTO)

        [Fact(DisplayName = "DTO - Deve permitir modificação de DataAcervo após criação")]
        public void Deve_permitir_modificacao_data_acervo_apos_criacao()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var novaDataAcervo = faker.Date.Recent().ToString("dd/MM/yyyy");

            dto.DataAcervo = novaDataAcervo;

            dto.DataAcervo.Should().Be(novaDataAcervo);
        }

        [Fact(DisplayName = "DTO - Deve suportar DataAcervo nula")]
        public void Deve_suportar_data_acervo_nula()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                DataAcervo = null
            };

            dto.DataAcervo.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve suportar DataAcervo vazia")]
        public void Deve_suportar_data_acervo_vazia()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                DataAcervo = string.Empty
            };

            dto.DataAcervo.Should().Be(string.Empty);
        }

        [Fact(DisplayName = "DTO - Deve suportar DataAcervo com diferentes formatos")]
        public void Deve_suportar_data_acervo_diferentes_formatos()
        {
            var datasValidas = new[]
            {
                "01/01/2024",
                "31/12/2023",
                "15/06/2024",
                "2024",
                "06/2024"
            };

            var dto = new AcervoTridimensionalDetalheDTO();
            foreach (var data in datasValidas)
            {
                dto.DataAcervo = data;
                dto.DataAcervo.Should().Be(data);
            }
        }

        [Fact(DisplayName = "DTO - Deve permitir atualizar DataAcervo múltiplas vezes")]
        public void Deve_permitir_atualizar_data_acervo_multiplas_vezes()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var data1 = faker.Date.Past().ToString("dd/MM/yyyy");
            var data2 = faker.Date.Recent().ToString("dd/MM/yyyy");
            var data3 = faker.Date.Future().ToString("dd/MM/yyyy");

            dto.DataAcervo = data1;
            dto.DataAcervo.Should().Be(data1);

            dto.DataAcervo = data2;
            dto.DataAcervo.Should().Be(data2);

            dto.DataAcervo = data3;
            dto.DataAcervo.Should().Be(data3);
        }

        #endregion

        #region Testes de Propriedade Procedencia (Específica do DetalheDTO)

        [Fact(DisplayName = "DTO - Deve permitir modificação de Procedencia após criação")]
        public void Deve_permitir_modificacao_procedencia_apos_criacao()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var novaProcedencia = faker.Lorem.Word();

            dto.Procedencia = novaProcedencia;

            dto.Procedencia.Should().Be(novaProcedencia);
        }

        [Fact(DisplayName = "DTO - Deve suportar Procedencia vazia")]
        public void Deve_suportar_procedencia_vazia()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                Procedencia = string.Empty
            };

            dto.Procedencia.Should().Be(string.Empty);
        }

        [Fact(DisplayName = "DTO - Deve suportar Procedencia nula")]
        public void Deve_suportar_procedencia_nula()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                Procedencia = null
            };

            dto.Procedencia.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve suportar Procedencia com comprimento máximo")]
        public void Deve_suportar_procedencia_comprimento_maximo()
        {
            var procedenciaMaxima = faker.Random.String2(200);

            var dto = new AcervoTridimensionalDetalheDTO
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

            var dto = new AcervoTridimensionalDetalheDTO();
            foreach (var procedencia in procedenciasEspeciais)
            {
                dto.Procedencia = procedencia;
                dto.Procedencia.Should().Be(procedencia);
            }
        }

        [Fact(DisplayName = "DTO - Deve permitir atualizar Procedencia múltiplas vezes")]
        public void Deve_permitir_atualizar_procedencia_multiplas_vezes()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
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

        #region Testes de Propriedade Conservacao (Específica do DetalheDTO)

        [Fact(DisplayName = "DTO - Deve permitir modificação de Conservacao após criação")]
        public void Deve_permitir_modificacao_conservacao_apos_criacao()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var novaConservacao = faker.Lorem.Word();

            dto.Conservacao = novaConservacao;

            dto.Conservacao.Should().Be(novaConservacao);
        }

        [Fact(DisplayName = "DTO - Deve suportar Conservacao vazia")]
        public void Deve_suportar_conservacao_vazia()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                Conservacao = string.Empty
            };

            dto.Conservacao.Should().Be(string.Empty);
        }

        [Fact(DisplayName = "DTO - Deve suportar Conservacao nula")]
        public void Deve_suportar_conservacao_nula()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                Conservacao = null
            };

            dto.Conservacao.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve suportar Conservacao com valores diversos")]
        public void Deve_suportar_conservacao_valores_diversos()
        {
            var conservacoesValidas = new[]
            {
                "Excelente",
                "Boa",
                "Regular",
                "Péssima",
                "Em restauração"
            };

            var dto = new AcervoTridimensionalDetalheDTO();
            foreach (var conservacao in conservacoesValidas)
            {
                dto.Conservacao = conservacao;
                dto.Conservacao.Should().Be(conservacao);
            }
        }

        [Fact(DisplayName = "DTO - Deve permitir atualizar Conservacao múltiplas vezes")]
        public void Deve_permitir_atualizar_conservacao_multiplas_vezes()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var conservacao1 = faker.Lorem.Word();
            var conservacao2 = faker.Lorem.Word();
            var conservacao3 = faker.Lorem.Word();

            dto.Conservacao = conservacao1;
            dto.Conservacao.Should().Be(conservacao1);

            dto.Conservacao = conservacao2;
            dto.Conservacao.Should().Be(conservacao2);

            dto.Conservacao = conservacao3;
            dto.Conservacao.Should().Be(conservacao3);
        }

        #endregion

        #region Testes de Propriedade Quantidade (Específica do DetalheDTO)

        [Fact(DisplayName = "DTO - Deve permitir modificação de Quantidade após criação")]
        public void Deve_permitir_modificacao_quantidade_apos_criacao()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var novaQuantidade = faker.Random.Long(1, 1000);

            dto.Quantidade = novaQuantidade;

            dto.Quantidade.Should().Be(novaQuantidade);
        }

        [Fact(DisplayName = "DTO - Deve suportar Quantidade com valor zero")]
        public void Deve_suportar_quantidade_zero()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                Quantidade = 0
            };

            dto.Quantidade.Should().Be(0);
        }

        [Fact(DisplayName = "DTO - Deve suportar Quantidade com valor 1")]
        public void Deve_suportar_quantidade_valor_minimo()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                Quantidade = 1
            };

            dto.Quantidade.Should().Be(1);
        }

        [Fact(DisplayName = "DTO - Deve suportar Quantidade com valor máximo")]
        public void Deve_suportar_quantidade_valor_maximo()
        {
            var quantidadeMaxima = long.MaxValue;

            var dto = new AcervoTridimensionalDetalheDTO
            {
                Quantidade = quantidadeMaxima
            };

            dto.Quantidade.Should().Be(quantidadeMaxima);
        }

        [Fact(DisplayName = "DTO - Deve permitir atualizar Quantidade múltiplas vezes")]
        public void Deve_permitir_atualizar_quantidade_multiplas_vezes()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var quantidade1 = faker.Random.Long(1, 100);
            var quantidade2 = faker.Random.Long(101, 1000);
            var quantidade3 = faker.Random.Long(1001, 10000);

            dto.Quantidade = quantidade1;
            dto.Quantidade.Should().Be(quantidade1);

            dto.Quantidade = quantidade2;
            dto.Quantidade.Should().Be(quantidade2);

            dto.Quantidade = quantidade3;
            dto.Quantidade.Should().Be(quantidade3);
        }

        #endregion

        #region Testes de Propriedade Dimensoes (Específica do DetalheDTO)

        [Fact(DisplayName = "DTO - Deve permitir modificação de Dimensoes após criação")]
        public void Deve_permitir_modificacao_dimensoes_apos_criacao()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var novasDimensoes = "10cm x 20cm x 30cm";

            dto.Dimensoes = novasDimensoes;

            dto.Dimensoes.Should().Be(novasDimensoes);
        }

        [Fact(DisplayName = "DTO - Deve suportar Dimensoes vazia")]
        public void Deve_suportar_dimensoes_vazia()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                Dimensoes = string.Empty
            };

            dto.Dimensoes.Should().Be(string.Empty);
        }

        [Fact(DisplayName = "DTO - Deve suportar Dimensoes nula")]
        public void Deve_suportar_dimensoes_nula()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                Dimensoes = null
            };

            dto.Dimensoes.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve suportar Dimensoes com diferentes formatos")]
        public void Deve_suportar_dimensoes_diferentes_formatos()
        {
            var dimensoesValidas = new[]
            {
                "10 x 20 x 30",
                "10cm x 20cm x 30cm",
                "10m x 20m",
                "Diâmetro: 50cm",
                "Largura: 100cm, Altura: 200cm"
            };

            var dto = new AcervoTridimensionalDetalheDTO();
            foreach (var dimensao in dimensoesValidas)
            {
                dto.Dimensoes = dimensao;
                dto.Dimensoes.Should().Be(dimensao);
            }
        }

        [Fact(DisplayName = "DTO - Deve permitir atualizar Dimensoes múltiplas vezes")]
        public void Deve_permitir_atualizar_dimensoes_multiplas_vezes()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var dimensoes1 = faker.Random.Double(0.01, 100).ToString();
            var dimensoes2 = "10cm x 20cm x 30cm";
            var dimensoes3 = faker.Random.Double(0.01, 100).ToString();

            dto.Dimensoes = dimensoes1;
            dto.Dimensoes.Should().Be(dimensoes1);

            dto.Dimensoes = dimensoes2;
            dto.Dimensoes.Should().Be(dimensoes2);

            dto.Dimensoes = dimensoes3;
            dto.Dimensoes.Should().Be(dimensoes3);
        }

        [Fact(DisplayName = "DTO - Deve suportar Dimensoes com caracteres especiais")]
        public void Deve_suportar_dimensoes_caracteres_especiais()
        {
            var dimensoesEspeciais = new[]
            {
                "10.5cm",
                "20,5cm",
                "1/2 metro",
                "Ø (diâmetro)"
            };

            var dto = new AcervoTridimensionalDetalheDTO();
            foreach (var dimensao in dimensoesEspeciais)
            {
                dto.Dimensoes = dimensao;
                dto.Dimensoes.Should().Be(dimensao);
            }
        }

        #endregion

        #region Testes de Propriedade Imagens (Específica do DetalheDTO)

        [Fact(DisplayName = "DTO - Deve permitir modificação de Imagens após criação")]
        public void Deve_permitir_modificacao_imagens_apos_criacao()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var novasImagens = new[]
            {
                new ImagemDTO { Original = faker.Internet.Url(), Thumbnail = faker.Internet.Url() }
            };

            dto.Imagens = novasImagens;

            dto.Imagens.Should().Equal(novasImagens);
        }

        [Fact(DisplayName = "DTO - Deve suportar Imagens nula")]
        public void Deve_suportar_imagens_nula()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                Imagens = null
            };

            dto.Imagens.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve suportar Imagens vazia")]
        public void Deve_suportar_imagens_vazia()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                Imagens = Array.Empty<ImagemDTO>()
            };

            dto.Imagens.Should().BeEmpty();
        }

        [Fact(DisplayName = "DTO - Deve suportar Imagens com uma imagem")]
        public void Deve_suportar_imagens_uma_imagem()
        {
            var imagens = new[]
            {
                new ImagemDTO { Original = faker.Internet.Url(), Thumbnail = faker.Internet.Url() }
            };

            var dto = new AcervoTridimensionalDetalheDTO
            {
                Imagens = imagens
            };

            dto.Imagens.Should().HaveCount(1);
            dto.Imagens.Should().Equal(imagens);
        }

        [Fact(DisplayName = "DTO - Deve suportar Imagens com múltiplas imagens")]
        public void Deve_suportar_imagens_multiplas_imagens()
        {
            var imagens = new[]
            {
                new ImagemDTO { Original = faker.Internet.Url(), Thumbnail = faker.Internet.Url() },
                new ImagemDTO { Original = faker.Internet.Url(), Thumbnail = faker.Internet.Url() },
                new ImagemDTO { Original = faker.Internet.Url(), Thumbnail = faker.Internet.Url() },
                new ImagemDTO { Original = faker.Internet.Url(), Thumbnail = faker.Internet.Url() },
                new ImagemDTO { Original = faker.Internet.Url(), Thumbnail = faker.Internet.Url() }
            };

            var dto = new AcervoTridimensionalDetalheDTO
            {
                Imagens = imagens
            };

            dto.Imagens.Should().HaveCount(5);
            dto.Imagens.Should().Equal(imagens);
        }

        [Fact(DisplayName = "DTO - Deve permitir atualizar Imagens múltiplas vezes")]
        public void Deve_permitir_atualizar_imagens_multiplas_vezes()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var imagens1 = new[]
            {
                new ImagemDTO { Original = faker.Internet.Url(), Thumbnail = faker.Internet.Url() }
            };
            var imagens2 = new[]
            {
                new ImagemDTO { Original = faker.Internet.Url(), Thumbnail = faker.Internet.Url() },
                new ImagemDTO { Original = faker.Internet.Url(), Thumbnail = faker.Internet.Url() }
            };

            dto.Imagens = imagens1;
            dto.Imagens.Should().HaveCount(1);

            dto.Imagens = imagens2;
            dto.Imagens.Should().HaveCount(2);
        }

        [Fact(DisplayName = "DTO - Deve manter dados corretos das imagens")]
        public void Deve_manter_dados_corretos_imagens()
        {
            var urlOriginal1 = faker.Internet.Url();
            var urlThumbnail1 = faker.Internet.Url();
            var urlOriginal2 = faker.Internet.Url();
            var urlThumbnail2 = faker.Internet.Url();

            var imagens = new[]
            {
                new ImagemDTO { Original = urlOriginal1, Thumbnail = urlThumbnail1 },
                new ImagemDTO { Original = urlOriginal2, Thumbnail = urlThumbnail2 }
            };

            var dto = new AcervoTridimensionalDetalheDTO
            {
                Imagens = imagens
            };

            dto.Imagens[0].Original.Should().Be(urlOriginal1);
            dto.Imagens[0].Thumbnail.Should().Be(urlThumbnail1);
            dto.Imagens[1].Original.Should().Be(urlOriginal2);
            dto.Imagens[1].Thumbnail.Should().Be(urlThumbnail2);
        }

        [Fact(DisplayName = "DTO - Deve suportar ImagemDTO com URLs nulas")]
        public void Deve_suportar_imagem_dto_urls_nulas()
        {
            var imagens = new[]
            {
                new ImagemDTO { Original = null, Thumbnail = null }
            };

            var dto = new AcervoTridimensionalDetalheDTO
            {
                Imagens = imagens
            };

            dto.Imagens[0].Original.Should().BeNull();
            dto.Imagens[0].Thumbnail.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve permitir ImagemDTO com URLs vazias")]
        public void Deve_permitir_imagem_dto_urls_vazias()
        {
            var imagens = new[]
            {
                new ImagemDTO { Original = string.Empty, Thumbnail = string.Empty }
            };

            var dto = new AcervoTridimensionalDetalheDTO
            {
                Imagens = imagens
            };

            dto.Imagens[0].Original.Should().Be(string.Empty);
            dto.Imagens[0].Thumbnail.Should().Be(string.Empty);
        }

        #endregion

        #region Testes de Propriedades Herdadas (AcervoDetalheDTO)

        [Fact(DisplayName = "DTO - Deve permitir modificação de Titulo (herdado) após criação")]
        public void Deve_permitir_modificacao_titulo_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var novoTitulo = faker.Lorem.Sentence();

            dto.Titulo = novoTitulo;

            dto.Titulo.Should().Be(novoTitulo);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Codigo (herdado) após criação")]
        public void Deve_permitir_modificacao_codigo_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var novoCodigo = faker.Random.AlphaNumeric(10);

            dto.Codigo = novoCodigo;

            dto.Codigo.Should().Be(novoCodigo);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Ano (herdado) após criação")]
        public void Deve_permitir_modificacao_ano_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var novoAno = faker.Date.Recent().Year.ToString();

            dto.Ano = novoAno;

            dto.Ano.Should().Be(novoAno);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de AcervoId (herdado) após criação")]
        public void Deve_permitir_modificacao_acervo_id_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var novoAcervoId = faker.Random.Long(1, 1000);

            dto.AcervoId = novoAcervoId;

            dto.AcervoId.Should().Be(novoAcervoId);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de EnderecoImagemPadrao (herdado) após criação")]
        public void Deve_permitir_modificacao_endereco_imagem_padrao_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var novoEndereco = faker.Internet.Url();

            dto.EnderecoImagemPadrao = novoEndereco;

            dto.EnderecoImagemPadrao.Should().Be(novoEndereco);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de SituacaoDisponibilidade (herdado) após criação")]
        public void Deve_permitir_modificacao_situacao_disponibilidade_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var novaSituacao = faker.Lorem.Word();

            dto.SituacaoDisponibilidade = novaSituacao;

            dto.SituacaoDisponibilidade.Should().Be(novaSituacao);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de EstaDisponivel (herdado) após criação")]
        public void Deve_permitir_modificacao_esta_disponivel_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalDetalheDTO { EstaDisponivel = false };

            dto.EstaDisponivel = true;

            dto.EstaDisponivel.Should().Be(true);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de TemControleDisponibilidade (herdado) após criação")]
        public void Deve_permitir_modificacao_tem_controle_disponibilidade_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalDetalheDTO { TemControleDisponibilidade = false };

            dto.TemControleDisponibilidade = true;

            dto.TemControleDisponibilidade.Should().Be(true);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de TipoAcervoId (herdado) após criação")]
        public void Deve_permitir_modificacao_tipo_acervo_id_herdado_apos_criacao()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var novoTipoAcervoId = 3;

            dto.TipoAcervoId = novoTipoAcervoId;

            dto.TipoAcervoId.Should().Be(novoTipoAcervoId);
        }

        #endregion

        #region Testes de Herança e Cobertura Completa

        [Fact(DisplayName = "DTO - Deve instanciar como derivada de AcervoDetalheDTO")]
        public void Deve_instanciar_como_derivada_de_acervo_detalhe_dto()
        {
            var dto = new AcervoTridimensionalDetalheDTO();

            dto.Should().BeAssignableTo<AcervoDetalheDTO>();
        }

        [Fact(DisplayName = "DTO - Deve suportar todas as propriedades simultaneamente")]
        public void Deve_suportar_todas_propriedades_simultaneamente()
        {
            var imagens = new[]
            {
                new ImagemDTO { Original = faker.Internet.Url(), Thumbnail = faker.Internet.Url() },
                new ImagemDTO { Original = faker.Internet.Url(), Thumbnail = faker.Internet.Url() }
            };

            var dto = new AcervoTridimensionalDetalheDTO
            {
                Titulo = faker.Lorem.Sentence(),
                Codigo = faker.Random.AlphaNumeric(10),
                Ano = faker.Date.Recent().Year.ToString(),
                AcervoId = faker.Random.Long(1, 1000),
                EnderecoImagemPadrao = faker.Internet.Url(),
                SituacaoDisponibilidade = faker.Lorem.Word(),
                EstaDisponivel = true,
                TemControleDisponibilidade = false,
                TipoAcervoId = 3,
                Descricao = faker.Lorem.Paragraphs(1),
                DataAcervo = faker.Date.Recent().ToString("dd/MM/yyyy"),
                Procedencia = faker.Lorem.Word(),
                Conservacao = faker.Lorem.Word(),
                Quantidade = faker.Random.Long(1, 1000),
                Dimensoes = "10cm x 20cm x 30cm",
                Imagens = imagens
            };

            dto.Titulo.Should().NotBeNull();
            dto.Codigo.Should().NotBeNull();
            dto.Ano.Should().NotBeNull();
            dto.AcervoId.Should().BeGreaterThan(0);
            dto.EnderecoImagemPadrao.Should().NotBeNull();
            dto.SituacaoDisponibilidade.Should().NotBeNull();
            dto.EstaDisponivel.Should().Be(true);
            dto.TemControleDisponibilidade.Should().Be(false);
            dto.TipoAcervoId.Should().Be(3);
            dto.Descricao.Should().NotBeNull();
            dto.DataAcervo.Should().NotBeNull();
            dto.Procedencia.Should().NotBeNull();
            dto.Conservacao.Should().NotBeNull();
            dto.Quantidade.Should().BeGreaterThan(0);
            dto.Dimensoes.Should().NotBeNull();
            dto.Imagens.Should().NotBeNull();
        }

        [Fact(DisplayName = "DTO - Cobertura 100% - Todos os getters e setters")]
        public void Cobertura_100_porcento_todos_getters_setters()
        {
            var dto = new AcervoTridimensionalDetalheDTO();
            var tituloValor = faker.Lorem.Sentence();
            var codigoValor = faker.Random.AlphaNumeric(10);
            var anoValor = faker.Date.Recent().Year.ToString();
            var acervoIdValor = faker.Random.Long(1, 1000);
            var enderecoImagemPadraoValor = faker.Internet.Url();
            var situacaoDisponibilidadeValor = faker.Lorem.Word();
            var estaDisponibilValor = true;
            var temControleDisponibilidadeValor = false;
            var tipoAcervoIdValor = 3;
            var descricaoValor = faker.Lorem.Paragraphs(1);
            var dataAcervoValor = faker.Date.Recent().ToString("dd/MM/yyyy");
            var procedenciaValor = faker.Lorem.Word();
            var conservacaoValor = faker.Lorem.Word();
            var quantidadeValor = faker.Random.Long(1, 1000);
            var dimensoesValor = "10cm x 20cm x 30cm";
            var imagensValor = new[]
            {
                new ImagemDTO { Original = faker.Internet.Url(), Thumbnail = faker.Internet.Url() }
            };

            dto.Titulo = tituloValor;
            dto.Codigo = codigoValor;
            dto.Ano = anoValor;
            dto.AcervoId = acervoIdValor;
            dto.EnderecoImagemPadrao = enderecoImagemPadraoValor;
            dto.SituacaoDisponibilidade = situacaoDisponibilidadeValor;
            dto.EstaDisponivel = estaDisponibilValor;
            dto.TemControleDisponibilidade = temControleDisponibilidadeValor;
            dto.TipoAcervoId = tipoAcervoIdValor;
            dto.Descricao = descricaoValor;
            dto.DataAcervo = dataAcervoValor;
            dto.Procedencia = procedenciaValor;
            dto.Conservacao = conservacaoValor;
            dto.Quantidade = quantidadeValor;
            dto.Dimensoes = dimensoesValor;
            dto.Imagens = imagensValor;

            dto.Titulo.Should().Be(tituloValor);
            dto.Codigo.Should().Be(codigoValor);
            dto.Ano.Should().Be(anoValor);
            dto.AcervoId.Should().Be(acervoIdValor);
            dto.EnderecoImagemPadrao.Should().Be(enderecoImagemPadraoValor);
            dto.SituacaoDisponibilidade.Should().Be(situacaoDisponibilidadeValor);
            dto.EstaDisponivel.Should().Be(estaDisponibilValor);
            dto.TemControleDisponibilidade.Should().Be(temControleDisponibilidadeValor);
            dto.TipoAcervoId.Should().Be(tipoAcervoIdValor);
            dto.Descricao.Should().Be(descricaoValor);
            dto.DataAcervo.Should().Be(dataAcervoValor);
            dto.Procedencia.Should().Be(procedenciaValor);
            dto.Conservacao.Should().Be(conservacaoValor);
            dto.Quantidade.Should().Be(quantidadeValor);
            dto.Dimensoes.Should().Be(dimensoesValor);
            dto.Imagens.Should().Equal(imagensValor);
        }

        [Fact(DisplayName = "DTO - Deve permitir valores nulos e não-nulos intercalados")]
        public void Deve_permitir_valores_nulos_nao_nulos_intercalados()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                Titulo = faker.Lorem.Sentence(),
                Codigo = null,
                Ano = faker.Date.Recent().Year.ToString(),
                EnderecoImagemPadrao = null,
                Descricao = faker.Lorem.Paragraphs(1),
                DataAcervo = null,
                Procedencia = faker.Lorem.Word(),
                Conservacao = null,
                Dimensoes = faker.Random.Double(0.01, 100).ToString(),
                Imagens = null
            };

            dto.Titulo.Should().NotBeNull();
            dto.Codigo.Should().BeNull();
            dto.Ano.Should().NotBeNull();
            dto.EnderecoImagemPadrao.Should().BeNull();
            dto.Descricao.Should().NotBeNull();
            dto.DataAcervo.Should().BeNull();
            dto.Procedencia.Should().NotBeNull();
            dto.Conservacao.Should().BeNull();
            dto.Dimensoes.Should().NotBeNull();
            dto.Imagens.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve permitir reinicializar propriedades para nulo")]
        public void Deve_permitir_reinicializar_propriedades_para_nulo()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                Titulo = faker.Lorem.Sentence(),
                Codigo = faker.Random.AlphaNumeric(10),
                Ano = faker.Date.Recent().Year.ToString(),
                EnderecoImagemPadrao = faker.Internet.Url(),
                SituacaoDisponibilidade = faker.Lorem.Word(),
                Descricao = faker.Lorem.Paragraphs(1),
                DataAcervo = faker.Date.Recent().ToString("dd/MM/yyyy"),
                Procedencia = faker.Lorem.Word(),
                Conservacao = faker.Lorem.Word(),
                Dimensoes = faker.Random.Double(0.01, 100).ToString()
            };

            dto.Titulo = null;
            dto.Codigo = null;
            dto.Ano = null;
            dto.EnderecoImagemPadrao = null;
            dto.SituacaoDisponibilidade = null;
            dto.Descricao = null;
            dto.DataAcervo = null;
            dto.Procedencia = null;
            dto.Conservacao = null;
            dto.Dimensoes = null;

            dto.Titulo.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.Ano.Should().BeNull();
            dto.EnderecoImagemPadrao.Should().BeNull();
            dto.SituacaoDisponibilidade.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.DataAcervo.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.Conservacao.Should().BeNull();
            dto.Dimensoes.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve permitir múltiplas instâncias independentes")]
        public void Deve_permitir_multiplas_instancias_independentes()
        {
            var dto1 = new AcervoTridimensionalDetalheDTO
            {
                Titulo = "Escultura 1",
                Procedencia = "Rio de Janeiro",
                Conservacao = "Excelente",
                Quantidade = 1
            };

            var dto2 = new AcervoTridimensionalDetalheDTO
            {
                Titulo = "Escultura 2",
                Procedencia = "São Paulo",
                Conservacao = "Boa",
                Quantidade = 2
            };

            dto1.Titulo.Should().Be("Escultura 1");
            dto1.Procedencia.Should().Be("Rio de Janeiro");
            dto1.Conservacao.Should().Be("Excelente");
            dto1.Quantidade.Should().Be(1);

            dto2.Titulo.Should().Be("Escultura 2");
            dto2.Procedencia.Should().Be("São Paulo");
            dto2.Conservacao.Should().Be("Boa");
            dto2.Quantidade.Should().Be(2);

            dto1.Procedencia.Should().NotBe(dto2.Procedencia);
        }

        [Fact(DisplayName = "DTO - Deve preservar valores ao atualizar múltiplas propriedades")]
        public void Deve_preservar_valores_ao_atualizar_multiplas_propriedades()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                Titulo = "Título Original",
                Procedencia = "Procedência Original",
                Conservacao = "Original",
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

        [Fact(DisplayName = "DTO - Deve permitir alternância de valores booleanos")]
        public void Deve_permitir_alternancia_valores_booleanos()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                EstaDisponivel = false,
                TemControleDisponibilidade = false
            };

            dto.EstaDisponivel.Should().Be(false);
            dto.TemControleDisponibilidade.Should().Be(false);

            dto.EstaDisponivel = true;
            dto.EstaDisponivel.Should().Be(true);

            dto.TemControleDisponibilidade = true;
            dto.TemControleDisponibilidade.Should().Be(true);

            dto.EstaDisponivel = false;
            dto.EstaDisponivel.Should().Be(false);

            dto.TemControleDisponibilidade = false;
            dto.TemControleDisponibilidade.Should().Be(false);
        }

        [Fact(DisplayName = "DTO - Deve suportar valores numéricos em AcervoId")]
        public void Deve_suportar_valores_numericos_acervo_id()
        {
            var dto = new AcervoTridimensionalDetalheDTO
            {
                AcervoId = faker.Random.Long(1, long.MaxValue)
            };

            dto.AcervoId.Should().BeGreaterThan(0);
        }

        [Fact(DisplayName = "DTO - Deve suportar valores numéricos em TipoAcervoId")]
        public void Deve_suportar_valores_numericos_tipo_acervo_id()
        {
            var tiposValidos = new[] { 1, 2, 3, 4, 5, 6 };
            var dto = new AcervoTridimensionalDetalheDTO();

            foreach (var tipo in tiposValidos)
            {
                dto.TipoAcervoId = tipo;
                dto.TipoAcervoId.Should().Be(tipo);
            }
        }

        [Fact(DisplayName = "DTO - Deve permitir atualização sequencial de todas as propriedades")]
        public void Deve_permitir_atualizacao_sequencial_todas_propriedades()
        {
            var dto = new AcervoTridimensionalDetalheDTO();

            dto.Titulo = faker.Lorem.Sentence();
            dto.Titulo.Should().NotBeNull();

            dto.Codigo = faker.Random.AlphaNumeric(10);
            dto.Codigo.Should().NotBeNull();

            dto.Ano = faker.Date.Recent().Year.ToString();
            dto.Ano.Should().NotBeNull();

            dto.AcervoId = faker.Random.Long(1, 1000);
            dto.AcervoId.Should().BeGreaterThan(0);

            dto.EnderecoImagemPadrao = faker.Internet.Url();
            dto.EnderecoImagemPadrao.Should().NotBeNull();

            dto.SituacaoDisponibilidade = faker.Lorem.Word();
            dto.SituacaoDisponibilidade.Should().NotBeNull();

            dto.EstaDisponivel = true;
            dto.EstaDisponivel.Should().Be(true);

            dto.TemControleDisponibilidade = true;
            dto.TemControleDisponibilidade.Should().Be(true);

            dto.TipoAcervoId = 3;
            dto.TipoAcervoId.Should().Be(3);

            dto.Descricao = faker.Lorem.Paragraphs(1);
            dto.Descricao.Should().NotBeNull();

            dto.DataAcervo = faker.Date.Recent().ToString("dd/MM/yyyy");
            dto.DataAcervo.Should().NotBeNull();

            dto.Procedencia = faker.Lorem.Word();
            dto.Procedencia.Should().NotBeNull();

            dto.Conservacao = faker.Lorem.Word();
            dto.Conservacao.Should().NotBeNull();

            dto.Quantidade = faker.Random.Long(1, 1000);
            dto.Quantidade.Should().BeGreaterThan(0);

            dto.Dimensoes = "10cm x 20cm x 30cm";
            dto.Dimensoes.Should().NotBeNull();

            dto.Imagens = new[] { new ImagemDTO { Original = faker.Internet.Url(), Thumbnail = faker.Internet.Url() } };
            dto.Imagens.Should().NotBeNull();
        }

        [Fact(DisplayName = "DTO - Deve manter consistência entre leitura e escrita")]
        public void Deve_manter_consistencia_entre_leitura_escrita()
        {
            var dto = new AcervoTridimensionalDetalheDTO();

            dto.Quantidade = 50;
            var primeiraLeitura = dto.Quantidade;
            primeiraLeitura.Should().Be(50);

            dto.Quantidade = 100;
            var segundaLeitura = dto.Quantidade;
            segundaLeitura.Should().Be(100);

            primeiraLeitura.Should().NotBe(segundaLeitura);
        }

        #endregion
    }
}
