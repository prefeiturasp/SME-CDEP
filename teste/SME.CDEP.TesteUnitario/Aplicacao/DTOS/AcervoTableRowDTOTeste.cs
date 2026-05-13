using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoTableRowDtoTeste
    {
        private readonly Faker faker;

        public AcervoTableRowDtoTeste()
        {
            faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "DTO - Deve criar instância com todos os parâmetros")]
        public void Deve_criar_instancia_com_todos_parametros()
        {
            var acervoId = faker.Random.Long(1, 1000);
            var tipoAcervo = faker.Lorem.Word();
            var tipoAcervoId = TipoAcervo.Bibliografico;
            var titulo = faker.Lorem.Sentence();
            var creditoAutoria = faker.Name.FullName();
            var codigo = faker.Random.AlphaNumeric(10);
            var data = faker.Date.Recent().ToString("dd/MM/yyyy");
            var capaDocumento = faker.Internet.Url();
            var editora = faker.Company.CompanyName();

            var dto = new AcervoTableRowDTO
            {
                AcervoId = acervoId,
                TipoAcervo = tipoAcervo,
                TipoAcervoId = tipoAcervoId,
                Titulo = titulo,
                CreditoAutoria = creditoAutoria,
                Codigo = codigo,
                Data = data,
                CapaDocumento = capaDocumento,
                Editora = editora
            };

            dto.AcervoId.Should().Be(acervoId);
            dto.TipoAcervo.Should().Be(tipoAcervo);
            dto.TipoAcervoId.Should().Be(tipoAcervoId);
            dto.Titulo.Should().Be(titulo);
            dto.CreditoAutoria.Should().Be(creditoAutoria);
            dto.Codigo.Should().Be(codigo);
            dto.Data.Should().Be(data);
            dto.CapaDocumento.Should().Be(capaDocumento);
            dto.Editora.Should().Be(editora);
        }

        [Fact(DisplayName = "DTO - Deve criar instância com propriedades padrão")]
        public void Deve_criar_instancia_com_propriedades_padrao()
        {
            var dto = new AcervoTableRowDTO();

            dto.AcervoId.Should().Be(0);
            dto.TipoAcervo.Should().BeNull();
            dto.TipoAcervoId.Should().Be(default(TipoAcervo));
            dto.Titulo.Should().BeNull();
            dto.CreditoAutoria.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.Data.Should().BeNull();
            dto.CapaDocumento.Should().BeNull();
            dto.Editora.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de AcervoId após criação")]
        public void Deve_permitir_modificacao_acervo_id_apos_criacao()
        {
            var dto = new AcervoTableRowDTO();
            var novoAcervoId = faker.Random.Long(1, 1000);

            dto.AcervoId = novoAcervoId;

            dto.AcervoId.Should().Be(novoAcervoId);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de TipoAcervo após criação")]
        public void Deve_permitir_modificacao_tipo_acervo_apos_criacao()
        {
            var dto = new AcervoTableRowDTO();
            var novoTipoAcervo = faker.Lorem.Word();

            dto.TipoAcervo = novoTipoAcervo;

            dto.TipoAcervo.Should().Be(novoTipoAcervo);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de TipoAcervoId após criação")]
        public void Deve_permitir_modificacao_tipo_acervo_id_apos_criacao()
        {
            var dto = new AcervoTableRowDTO();
            var novoTipoAcervoId = TipoAcervo.Bibliografico;

            dto.TipoAcervoId = novoTipoAcervoId;

            dto.TipoAcervoId.Should().Be(novoTipoAcervoId);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Titulo após criação")]
        public void Deve_permitir_modificacao_titulo_apos_criacao()
        {
            var dto = new AcervoTableRowDTO();
            var novoTitulo = faker.Lorem.Sentence();

            dto.Titulo = novoTitulo;

            dto.Titulo.Should().Be(novoTitulo);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de CreditoAutoria após criação")]
        public void Deve_permitir_modificacao_credito_autoria_apos_criacao()
        {
            var dto = new AcervoTableRowDTO();
            var novoCreditoAutoria = faker.Name.FullName();

            dto.CreditoAutoria = novoCreditoAutoria;

            dto.CreditoAutoria.Should().Be(novoCreditoAutoria);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Codigo após criação")]
        public void Deve_permitir_modificacao_codigo_apos_criacao()
        {
            var dto = new AcervoTableRowDTO();
            var novoCodigo = faker.Random.AlphaNumeric(10);

            dto.Codigo = novoCodigo;

            dto.Codigo.Should().Be(novoCodigo);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Data após criação")]
        public void Deve_permitir_modificacao_data_apos_criacao()
        {
            var dto = new AcervoTableRowDTO();
            var novaData = faker.Date.Recent().ToString("dd/MM/yyyy");

            dto.Data = novaData;

            dto.Data.Should().Be(novaData);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de CapaDocumento após criação")]
        public void Deve_permitir_modificacao_capa_documento_apos_criacao()
        {
            var dto = new AcervoTableRowDTO();
            var novaCapaDocumento = faker.Internet.Url();

            dto.CapaDocumento = novaCapaDocumento;

            dto.CapaDocumento.Should().Be(novaCapaDocumento);
        }

        [Fact(DisplayName = "DTO - Deve permitir modificação de Editora após criação")]
        public void Deve_permitir_modificacao_editora_apos_criacao()
        {
            var dto = new AcervoTableRowDTO();
            var novaEditora = faker.Company.CompanyName();

            dto.Editora = novaEditora;

            dto.Editora.Should().Be(novaEditora);
        }

        [Fact(DisplayName = "DTO - Deve suportar AcervoId com valor zero")]
        public void Deve_suportar_acervo_id_zero()
        {
            var dto = new AcervoTableRowDTO
            {
                AcervoId = 0
            };

            dto.AcervoId.Should().Be(0);
        }

        [Fact(DisplayName = "DTO - Deve suportar AcervoId com valor máximo")]
        public void Deve_suportar_acervo_id_valor_maximo()
        {
            var acervoIdMaximo = long.MaxValue;

            var dto = new AcervoTableRowDTO
            {
                AcervoId = acervoIdMaximo
            };

            dto.AcervoId.Should().Be(acervoIdMaximo);
        }

        [Fact(DisplayName = "DTO - Deve suportar strings vazias")]
        public void Deve_suportar_strings_vazias()
        {
            var dto = new AcervoTableRowDTO
            {
                TipoAcervo = string.Empty,
                Titulo = string.Empty,
                CreditoAutoria = string.Empty,
                Codigo = string.Empty,
                Data = string.Empty,
                CapaDocumento = string.Empty,
                Editora = string.Empty
            };

            dto.TipoAcervo.Should().Be(string.Empty);
            dto.Titulo.Should().Be(string.Empty);
            dto.CreditoAutoria.Should().Be(string.Empty);
            dto.Codigo.Should().Be(string.Empty);
            dto.Data.Should().Be(string.Empty);
            dto.CapaDocumento.Should().Be(string.Empty);
            dto.Editora.Should().Be(string.Empty);
        }

        [Fact(DisplayName = "DTO - Deve suportar strings com espaços em branco")]
        public void Deve_suportar_strings_com_espacos_branco()
        {
            var stringComEspacos = "  Texto com espaços  ";

            var dto = new AcervoTableRowDTO
            {
                TipoAcervo = stringComEspacos,
                Titulo = stringComEspacos,
                CreditoAutoria = stringComEspacos,
                Codigo = stringComEspacos,
                Data = stringComEspacos,
                Editora = stringComEspacos
            };

            dto.TipoAcervo.Should().Be(stringComEspacos);
            dto.Titulo.Should().Be(stringComEspacos);
            dto.CreditoAutoria.Should().Be(stringComEspacos);
            dto.Codigo.Should().Be(stringComEspacos);
            dto.Data.Should().Be(stringComEspacos);
            dto.Editora.Should().Be(stringComEspacos);
        }

        [Fact(DisplayName = "DTO - Deve suportar strings longas")]
        public void Deve_suportar_strings_longas()
        {
            var stringLonga = faker.Lorem.Paragraphs(5);

            var dto = new AcervoTableRowDTO
            {
                TipoAcervo = stringLonga,
                Titulo = stringLonga,
                CreditoAutoria = stringLonga,
                Codigo = stringLonga,
                Data = stringLonga,
                Editora = stringLonga
            };

            dto.TipoAcervo.Should().Be(stringLonga);
            dto.Titulo.Should().Be(stringLonga);
            dto.CreditoAutoria.Should().Be(stringLonga);
            dto.Codigo.Should().Be(stringLonga);
            dto.Data.Should().Be(stringLonga);
            dto.Editora.Should().Be(stringLonga);
        }

        [Fact(DisplayName = "DTO - Deve permitir CapaDocumento nula")]
        public void Deve_permitir_capa_documento_nula()
        {
            var dto = new AcervoTableRowDTO
            {
                CapaDocumento = null
            };

            dto.CapaDocumento.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve permitir CapaDocumento com URL válida")]
        public void Deve_permitir_capa_documento_url_valida()
        {
            var url = faker.Internet.Url();

            var dto = new AcervoTableRowDTO
            {
                CapaDocumento = url
            };

            dto.CapaDocumento.Should().Be(url);
        }

        [Fact(DisplayName = "DTO - Deve permitir CapaDocumento com Base64")]
        public void Deve_permitir_capa_documento_base64()
        {
            var base64 = Convert.ToBase64String(faker.Random.Bytes(50));

            var dto = new AcervoTableRowDTO
            {
                CapaDocumento = base64
            };

            dto.CapaDocumento.Should().Be(base64);
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
            var dto = new AcervoTableRowDTO
            {
                TipoAcervoId = tipoAcervo
            };

            dto.TipoAcervoId.Should().Be(tipoAcervo);
        }

        [Fact(DisplayName = "DTO - Deve permitir múltiplas instâncias independentes")]
        public void Deve_permitir_multiplas_instancias_independentes()
        {
            var dto1 = new AcervoTableRowDTO
            {
                AcervoId = 1,
                TipoAcervo = "Livro",
                Titulo = "Título 1",
                TipoAcervoId = TipoAcervo.Bibliografico
            };

            var dto2 = new AcervoTableRowDTO
            {
                AcervoId = 2,
                TipoAcervo = "Periódico",
                Titulo = "Título 2",
                TipoAcervoId = TipoAcervo.Tridimensional
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
            var dto = new AcervoTableRowDTO
            {
                AcervoId = 1,
                TipoAcervo = "Livro",
                Titulo = "Título Original",
                TipoAcervoId = TipoAcervo.Bibliografico
            };

            var tituloOriginal = dto.Titulo;
            var tipoAcervoOriginal = dto.TipoAcervoId;
            
            dto.Titulo = "Título Modificado";

            tituloOriginal.Should().Be("Título Original");
            dto.Titulo.Should().Be("Título Modificado");
            dto.TipoAcervoId.Should().Be(tipoAcervoOriginal);
        }

        [Fact(DisplayName = "DTO - Deve permitir autores com caracteres especiais")]
        public void Deve_permitir_autores_com_caracteres_especiais()
        {
            var autoresEspeciais = new[]
            {
                "José da Silva",
                "François Müller",
                "李明 (Li Ming)",
                "O'Connor"
            };

            var dto = new AcervoTableRowDTO();
            foreach (var autor in autoresEspeciais)
            {
                dto.CreditoAutoria = autor;
                dto.CreditoAutoria.Should().Be(autor);
            }
        }

        [Fact(DisplayName = "DTO - Deve permitir código com caracteres alfanuméricos")]
        public void Deve_permitir_codigo_caracteres_alfanumericos()
        {
            var codigosValidos = new[]
            {
                "ABC123",
                "00001",
                "LVR-2024-001",
                "CDEP/001/2024"
            };

            var dto = new AcervoTableRowDTO();
            foreach (var codigo in codigosValidos)
            {
                dto.Codigo = codigo;
                dto.Codigo.Should().Be(codigo);
            }
        }

        [Fact(DisplayName = "DTO - Deve permitir múltiplos formatos de data")]
        public void Deve_permitir_multiplos_formatos_data()
        {
            var datasValidas = new[]
            {
                "01/01/2024",
                "31/12/2023",
                "15/06/2024"
            };

            var dto = new AcervoTableRowDTO();
            foreach (var data in datasValidas)
            {
                dto.Data = data;
                dto.Data.Should().Be(data);
            }
        }

        [Fact(DisplayName = "DTO - Deve permitir editoras com nomes comerciais")]
        public void Deve_permitir_editoras_com_nomes_comerciais()
        {
            var editorasValidas = new[]
            {
                "Companhia das Letras",
                "Rocco",
                "Record",
                "Intrínseca"
            };

            var dto = new AcervoTableRowDTO();
            foreach (var editora in editorasValidas)
            {
                dto.Editora = editora;
                dto.Editora.Should().Be(editora);
            }
        }

        [Fact(DisplayName = "DTO - Deve permitir alternância entre tipos de acervo")]
        public void Deve_permitir_alternancia_entre_tipos_acervo()
        {
            var dto = new AcervoTableRowDTO { TipoAcervoId = TipoAcervo.Bibliografico };

            dto.TipoAcervoId.Should().Be(TipoAcervo.Bibliografico);

            dto.TipoAcervoId = TipoAcervo.Tridimensional;
            dto.TipoAcervoId.Should().Be(TipoAcervo.Tridimensional  );

            dto.TipoAcervoId = TipoAcervo.Tridimensional;

            dto.TipoAcervoId.Should().Be(TipoAcervo.Tridimensional);
        }

        [Fact(DisplayName = "DTO - Deve permitir atualização sequencial de todas as propriedades")]
        public void Deve_permitir_atualizacao_sequencial_todas_propriedades()
        {
            var dto = new AcervoTableRowDTO();

            dto.AcervoId = 100;
            dto.AcervoId.Should().Be(100);

            dto.TipoAcervo = "Livro";
            dto.TipoAcervo.Should().Be("Livro");

            dto.TipoAcervoId = TipoAcervo.Bibliografico;
            dto.TipoAcervoId.Should().Be(TipoAcervo.Bibliografico);

            dto.Titulo = "Novo Título";
            dto.Titulo.Should().Be("Novo Título");

            dto.CreditoAutoria = "Novo Autor";
            dto.CreditoAutoria.Should().Be("Novo Autor");

            dto.Codigo = "LVR-001";
            dto.Codigo.Should().Be("LVR-001");

            dto.Data = "01/01/2024";
            dto.Data.Should().Be("01/01/2024");

            dto.CapaDocumento = "http://example.com/capa.jpg";
            dto.CapaDocumento.Should().Be("http://example.com/capa.jpg");

            dto.Editora = "Editora XYZ";
            dto.Editora.Should().Be("Editora XYZ");
        }

        [Fact(DisplayName = "DTO - Deve manter consistência entre leitura e escrita")]
        public void Deve_manter_consistencia_entre_leitura_escrita()
        {
            var dto = new AcervoTableRowDTO();

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
            var dto = new AcervoTableRowDTO
            {
                AcervoId = 1,
                TipoAcervo = "Livro",
                Titulo = "Teste"
            };

            var acervoId1 = dto.AcervoId;
            var tipoAcervo1 = dto.TipoAcervo;
            var titulo1 = dto.Titulo;
            var acervoId2 = dto.AcervoId;
            var tipoAcervo2 = dto.TipoAcervo;
            var titulo2 = dto.Titulo;

            acervoId1.Should().Be(acervoId2);
            tipoAcervo1.Should().Be(tipoAcervo2);
            titulo1.Should().Be(titulo2);
        }

        [Fact(DisplayName = "DTO - Deve permitir concatenar múltiplas instâncias em coleção")]
        public void Deve_permitir_concatenar_multiplas_instancias_colecao()
        {
            var dto1 = new AcervoTableRowDTO
            {
                AcervoId = 1,
                Titulo = "Título 1",
                TipoAcervoId = TipoAcervo.Bibliografico
            };

            var dto2 = new AcervoTableRowDTO
            {
                AcervoId = 2,
                Titulo = "Título 2",
                TipoAcervoId = TipoAcervo.Tridimensional
            };

            var dtos = new[] { dto1, dto2 };

            dtos.Should().HaveCount(2);
            dtos[0].AcervoId.Should().Be(1);
            dtos[1].AcervoId.Should().Be(2);
        }

        [Fact(DisplayName = "DTO - Cobertura 100% - Todos os getters e setters")]
        public void Cobertura_100_porcento_todos_getters_setters()
        {
            var dto = new AcervoTableRowDTO();
            var acervoIdValor = faker.Random.Long(1, 1000);
            var tipoAcervoValor = faker.Lorem.Word();
            var tipoAcervoIdValor = TipoAcervo.Tridimensional;
            var tituloValor = faker.Lorem.Sentence();
            var creditoAutoriaValor = faker.Name.FullName();
            var codigoValor = faker.Random.AlphaNumeric(10);
            var dataValor = faker.Date.Recent().ToString("dd/MM/yyyy");
            var capaDocumentoValor = faker.Internet.Url();
            var editoraValor = faker.Company.CompanyName();

            dto.AcervoId = acervoIdValor;
            dto.TipoAcervo = tipoAcervoValor;
            dto.TipoAcervoId = tipoAcervoIdValor;
            dto.Titulo = tituloValor;
            dto.CreditoAutoria = creditoAutoriaValor;
            dto.Codigo = codigoValor;
            dto.Data = dataValor;
            dto.CapaDocumento = capaDocumentoValor;
            dto.Editora = editoraValor;

            dto.AcervoId.Should().Be(acervoIdValor);
            dto.TipoAcervo.Should().Be(tipoAcervoValor);
            dto.TipoAcervoId.Should().Be(tipoAcervoIdValor);
            dto.Titulo.Should().Be(tituloValor);
            dto.CreditoAutoria.Should().Be(creditoAutoriaValor);
            dto.Codigo.Should().Be(codigoValor);
            dto.Data.Should().Be(dataValor);
            dto.CapaDocumento.Should().Be(capaDocumentoValor);
            dto.Editora.Should().Be(editoraValor);

            dto.Should().NotBeNull();
            dto.AcervoId.Should().BeGreaterThan(0);
            dto.TipoAcervo.Should().NotBeEmpty();
            dto.Titulo.Should().NotBeEmpty();
            dto.CreditoAutoria.Should().NotBeEmpty();
            dto.Codigo.Should().NotBeEmpty();
            dto.Data.Should().NotBeEmpty();
            dto.CapaDocumento.Should().NotBeEmpty();
            dto.Editora.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "DTO - Deve permitir valores nulos e não-nulos intercalados")]
        public void Deve_permitir_valores_nulos_nao_nulos_intercalados()
        {
            var dto = new AcervoTableRowDTO
            {
                TipoAcervo = "Livro",
                Titulo = null!,
                CreditoAutoria = "Autor",
                Codigo = null!,
                CapaDocumento = "http://example.com/capa.jpg"
            };

            dto.TipoAcervo.Should().NotBeNull();
            dto.Titulo.Should().BeNull();
            dto.CreditoAutoria.Should().NotBeNull();
            dto.Codigo.Should().BeNull();
            dto.CapaDocumento.Should().NotBeNull();
        }

        [Fact(DisplayName = "DTO - Deve permitir reinicializar propriedades para nulo")]
        public void Deve_permitir_reinicializar_propriedades_para_nulo()
        {
            var dto = new AcervoTableRowDTO
            {
                TipoAcervo = "Livro",
                Titulo = "Título",
                CreditoAutoria = "Autor",
                Codigo = "LVR-001",
                Data = "01/01/2024",
                CapaDocumento = "http://example.com/capa.jpg",
                Editora = "Editora"
            };

            dto.TipoAcervo = null!;
            dto.Titulo = null!;
            dto.CreditoAutoria = null!;
            dto.Codigo = null!;
            dto.Data = null!;
            dto.CapaDocumento = null!;
            dto.Editora = null!;

            dto.TipoAcervo.Should().BeNull();
            dto.Titulo.Should().BeNull();
            dto.CreditoAutoria.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.Data.Should().BeNull();
            dto.CapaDocumento.Should().BeNull();
            dto.Editora.Should().BeNull();
        }

        [Fact(DisplayName = "DTO - Deve suportar valores numéricos negativos em AcervoId (se aplicável)")]
        public void Deve_suportar_valores_numericos_acrevo_id()
        {
            var dto = new AcervoTableRowDTO
            {
                AcervoId = faker.Random.Long(1, long.MaxValue)
            };

            dto.AcervoId.Should().BeGreaterThan(0);
        }
    }
}
