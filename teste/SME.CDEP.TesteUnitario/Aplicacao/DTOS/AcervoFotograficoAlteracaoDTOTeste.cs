using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoFotograficoAlteracaoDTOTeste
    {
        private AcervoFotograficoAlteracaoDTO CriarAcervoFotograficoAlteracaoDTOCompleto()
        {
            return new AcervoFotograficoAlteracaoDTO
            {
                Id = 12345,
                AcervoId = 67890,
                Localizacao = "Setor de Fotografia",
                Procedencia = "Arquivo Municipal",
                CopiaDigital = true,
                PermiteUsoImagem = true,
                ConservacaoId = 1,
                Quantidade = 100,
                Largura = "20",
                Altura = "30",
                SuporteId = 2,
                FormatoId = 3,
                TamanhoArquivo = "5MB",
                CromiaId = 4,
                Resolucao = "300DPI",
                Arquivos = new long[] { 1, 2, 3 }
            };
        }

        [Fact]
        public void DadoAcervoFotograficoAlteracaoDTO_QuandoInstanciar_EntaoTodosPropriedadesSaoInicializadas()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();

            dto.Id.Should().Be(0);
            dto.AcervoId.Should().Be(0);
            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.PermiteUsoImagem.Should().BeNull();
            dto.ConservacaoId.Should().Be(0);
            dto.Quantidade.Should().Be(0);
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.SuporteId.Should().Be(0);
            dto.FormatoId.Should().Be(0);
            dto.TamanhoArquivo.Should().BeNull();
            dto.CromiaId.Should().Be(0);
            dto.Resolucao.Should().BeNull();
            dto.Arquivos.Should().BeNull();
        }

        [Fact]
        public void DadoId_QuandoDefinirValor_EntaoIdEhAtribuido()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const long idEsperado = 999;

            dto.Id = idEsperado;

            dto.Id.Should().Be(idEsperado);
        }

        [Fact]
        public void DadoAcervoId_QuandoDefinirValor_EntaoAcervoIdEhAtribuido()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const long acervoIdEsperado = 555;

            dto.AcervoId = acervoIdEsperado;

            dto.AcervoId.Should().Be(acervoIdEsperado);
        }

        [Fact]
        public void DadoAcervoFotograficoAlteracaoDTO_QuandoDefiniTodosOsValores_EntaoTodosCamposSaoAcessiveis()
        {
            var dto = CriarAcervoFotograficoAlteracaoDTOCompleto();

            dto.Id.Should().Be(12345);
            dto.AcervoId.Should().Be(67890);
            dto.Localizacao.Should().Be("Setor de Fotografia");
            dto.Procedencia.Should().Be("Arquivo Municipal");
            dto.CopiaDigital.Should().BeTrue();
            dto.PermiteUsoImagem.Should().BeTrue();
            dto.ConservacaoId.Should().Be(1);
            dto.Quantidade.Should().Be(100);
            dto.Largura.Should().Be("20");
            dto.Altura.Should().Be("30");
            dto.SuporteId.Should().Be(2);
            dto.FormatoId.Should().Be(3);
            dto.TamanhoArquivo.Should().Be("5MB");
            dto.CromiaId.Should().Be(4);
            dto.Resolucao.Should().Be("300DPI");
            dto.Arquivos.Should().ContainInOrder(1, 2, 3);
        }

        [Fact]
        public void DadoId_QuandoAtribuirValorMaximo_EntaoValorEhArmazenado()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const long idMaximo = long.MaxValue;

            dto.Id = idMaximo;

            dto.Id.Should().Be(idMaximo);
        }

        [Fact]
        public void DadoId_QuandoAtribuirUm_EntaoValorEhArmazenado()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const long idUm = 1;

            dto.Id = idUm;

            dto.Id.Should().Be(idUm);
        }

        [Fact]
        public void DadoAcervoId_QuandoAtribuirValorMaximo_EntaoValorEhArmazenado()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const long acervoIdMaximo = long.MaxValue;

            dto.AcervoId = acervoIdMaximo;

            dto.AcervoId.Should().Be(acervoIdMaximo);
        }

        [Fact]
        public void DadoAcervoId_QuandoAtribuirUm_EntaoValorEhArmazenado()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const long acervoIdUm = 1;

            dto.AcervoId = acervoIdUm;

            dto.AcervoId.Should().Be(acervoIdUm);
        }

        [Fact]
        public void DadoAcervoFotograficoAlteracaoDTO_QuandoInicializarComConstrutorVazio_EntaoNaoLancaExcecao()
        {
            var exception = Record.Exception(() =>
            {
                var dto = new AcervoFotograficoAlteracaoDTO();
            });

            exception.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoFotograficoAlteracaoDTO_QuandoAcessarPropriedadesComConstrutorVazio_EntaoPropriedadesNaoLancamExcecao()
        {
            var exception = Record.Exception(() =>
            {
                var dto = new AcervoFotograficoAlteracaoDTO();
                _ = dto.Id;
                _ = dto.AcervoId;
                _ = dto.Localizacao;
                _ = dto.Procedencia;
                _ = dto.CopiaDigital;
                _ = dto.PermiteUsoImagem;
                _ = dto.ConservacaoId;
                _ = dto.Quantidade;
                _ = dto.Largura;
                _ = dto.Altura;
                _ = dto.SuporteId;
                _ = dto.FormatoId;
                _ = dto.TamanhoArquivo;
                _ = dto.CromiaId;
                _ = dto.Resolucao;
                _ = dto.Arquivos;
            });

            exception.Should().BeNull();
        }

        [Fact]
        public void DadoDuasInstancias_QuandoComMesmosValores_EntaoSaoInstanciasDistintas()
        {
            var dto1 = CriarAcervoFotograficoAlteracaoDTOCompleto();
            var dto2 = CriarAcervoFotograficoAlteracaoDTOCompleto();

            dto1.Should().NotBeSameAs(dto2);
            dto1.Id.Should().Be(dto2.Id);
            dto1.AcervoId.Should().Be(dto2.AcervoId);
        }

        [Fact]
        public void DadoAcervoFotograficoAlteracaoDTO_QuandoInicializarComValues_EntaoValoresEhArmazenados()
        {
            const long idEsperado = 5678;
            const long acervoIdEsperado = 9012;

            var dto = new AcervoFotograficoAlteracaoDTO
            {
                Id = idEsperado,
                AcervoId = acervoIdEsperado,
                Localizacao = "Test",
                Procedencia = "Procedência Teste"
            };

            dto.Id.Should().Be(idEsperado);
            dto.AcervoId.Should().Be(acervoIdEsperado);
        }

        [Fact]
        public void DadoId_QuandoAtribuirValoresSequenciais_EntaoValoresSequenciaisArmazenados()
        {
            var dto1 = new AcervoFotograficoAlteracaoDTO { Id = 1 };
            var dto2 = new AcervoFotograficoAlteracaoDTO { Id = 2 };
            var dto3 = new AcervoFotograficoAlteracaoDTO { Id = 3 };

            dto1.Id.Should().Be(1);
            dto2.Id.Should().Be(2);
            dto3.Id.Should().Be(3);
        }

        [Fact]
        public void DadoAcervoId_QuandoAtribuirValoresSequenciais_EntaoValoresSequenciaisArmazenados()
        {
            var dto1 = new AcervoFotograficoAlteracaoDTO { AcervoId = 1 };
            var dto2 = new AcervoFotograficoAlteracaoDTO { AcervoId = 2 };
            var dto3 = new AcervoFotograficoAlteracaoDTO { AcervoId = 3 };

            dto1.AcervoId.Should().Be(1);
            dto2.AcervoId.Should().Be(2);
            dto3.AcervoId.Should().Be(3);
        }

        [Fact]
        public void DadoId_QuandoAtribuirMultiplosValores_EntaoUltimoValorEhPreservado()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();

            dto.Id = 100;
            dto.Id = 200;
            dto.Id = 300;

            dto.Id.Should().Be(300);
        }

        [Fact]
        public void DadoAcervoId_QuandoAtribuirMultiplosValores_EntaoUltimoValorEhPreservado()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();

            dto.AcervoId = 100;
            dto.AcervoId = 200;
            dto.AcervoId = 300;

            dto.AcervoId.Should().Be(300);
        }

        [Fact]
        public void DadoLocalizacao_QuandoDefinirValor_EntaoLocalizacaoEhAtribuida()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const string localizacaoEsperada = "Sala de Arquivo";

            dto.Localizacao = localizacaoEsperada;

            dto.Localizacao.Should().Be(localizacaoEsperada);
        }

        [Fact]
        public void DadoLocalizacao_QuandoDefinirNull_EntaoLocalizacaoEhNull()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();

            dto.Localizacao = null;

            dto.Localizacao.Should().BeNull();
        }

        [Fact]
        public void DadoProcedencia_QuandoDefinirValor_EntaoProcedenciaEhAtribuida()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const string procedenciaEsperada = "Acervo Histórico";

            dto.Procedencia = procedenciaEsperada;

            dto.Procedencia.Should().Be(procedenciaEsperada);
        }

        [Fact]
        public void DadoCopiaDigital_QuandoDefinirTrue_EntaoCopiaDigitalEhTrue()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();

            dto.CopiaDigital = true;

            dto.CopiaDigital.Should().BeTrue();
        }

        [Fact]
        public void DadoCopiaDigital_QuandoDefinirFalse_EntaoCopiaDigitalEhFalse()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();

            dto.CopiaDigital = false;

            dto.CopiaDigital.Should().BeFalse();
        }

        [Fact]
        public void DadoPermiteUsoImagem_QuandoDefinirTrue_EntaoPermiteUsoImagemEhTrue()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();

            dto.PermiteUsoImagem = true;

            dto.PermiteUsoImagem.Should().BeTrue();
        }

        [Fact]
        public void DadoPermiteUsoImagem_QuandoDefinirFalse_EntaoPermiteUsoImagemEhFalse()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();

            dto.PermiteUsoImagem = false;

            dto.PermiteUsoImagem.Should().BeFalse();
        }

        [Fact]
        public void DadoConservacaoId_QuandoDefinirValor_EntaoConservacaoIdEhAtribuido()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const long conservacaoIdEsperado = 42;

            dto.ConservacaoId = conservacaoIdEsperado;

            dto.ConservacaoId.Should().Be(conservacaoIdEsperado);
        }

        [Fact]
        public void DadoQuantidade_QuandoDefinirValor_EntaoQuantidadeEhAtribuida()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const int quantidadeEsperada = 50;

            dto.Quantidade = quantidadeEsperada;

            dto.Quantidade.Should().Be(quantidadeEsperada);
        }

        [Fact]
        public void DadoLargura_QuandoDefinirValor_EntaoLarguraEhAtribuida()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const string larguraEsperada = "25cm";

            dto.Largura = larguraEsperada;

            dto.Largura.Should().Be(larguraEsperada);
        }

        [Fact]
        public void DadoAltura_QuandoDefinirValor_EntaoAlturaEhAtribuida()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const string alturaEsperada = "35cm";

            dto.Altura = alturaEsperada;

            dto.Altura.Should().Be(alturaEsperada);
        }

        [Fact]
        public void DadoSuporteId_QuandoDefinirValor_EntaoSuporteIdEhAtribuido()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const long suporteIdEsperado = 77;

            dto.SuporteId = suporteIdEsperado;

            dto.SuporteId.Should().Be(suporteIdEsperado);
        }

        [Fact]
        public void DadoFormatoId_QuandoDefinirValor_EntaoFormatoIdEhAtribuido()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const long formatoIdEsperado = 88;

            dto.FormatoId = formatoIdEsperado;

            dto.FormatoId.Should().Be(formatoIdEsperado);
        }

        [Fact]
        public void DadoTamanhoArquivo_QuandoDefinirValor_EntaoTamanhoArquivoEhAtribuido()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const string tamanhoEsperado = "10MB";

            dto.TamanhoArquivo = tamanhoEsperado;

            dto.TamanhoArquivo.Should().Be(tamanhoEsperado);
        }

        [Fact]
        public void DadoCromiaId_QuandoDefinirValor_EntaoCromiaIdEhAtribuido()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const long cromiaIdEsperado = 99;

            dto.CromiaId = cromiaIdEsperado;

            dto.CromiaId.Should().Be(cromiaIdEsperado);
        }

        [Fact]
        public void DadoResolucao_QuandoDefinirValor_EntaoResolucaoEhAtribuida()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const string resolucaoEsperada = "600DPI";

            dto.Resolucao = resolucaoEsperada;

            dto.Resolucao.Should().Be(resolucaoEsperada);
        }

        [Fact]
        public void DadoArquivos_QuandoDefinirArray_EntaoArquivosEhAtribuido()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            var arquivosEsperados = new long[] { 100, 200, 300 };

            dto.Arquivos = arquivosEsperados;

            dto.Arquivos.Should().ContainInOrder(100, 200, 300);
        }

        [Fact]
        public void DadoArquivos_QuandoDefinirNull_EntaoArquivosEhNull()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();

            dto.Arquivos = null;

            dto.Arquivos.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoFotograficoAlteracaoDTO_QuandoInicializarComTodasAsPropriedades_EntaoTodosCamposAcessiveis()
        {
            const long idEsperado = 987654321;
            const long acervoIdEsperado = 123456789;

            var dto = new AcervoFotograficoAlteracaoDTO
            {
                Id = idEsperado,
                AcervoId = acervoIdEsperado,
                Localizacao = "Arquivo Geral",
                Procedencia = "Doação Particular",
                CopiaDigital = true,
                PermiteUsoImagem = false,
                ConservacaoId = 1,
                Quantidade = 500,
                Largura = "50cm",
                Altura = "70cm",
                SuporteId = 5,
                FormatoId = 6,
                TamanhoArquivo = "20MB",
                CromiaId = 7,
                Resolucao = "1200DPI",
                Arquivos = new long[] { 10, 20, 30, 40, 50 }
            };

            dto.Should().NotBeNull();
            dto.Id.Should().Be(idEsperado);
            dto.AcervoId.Should().Be(acervoIdEsperado);
            dto.Localizacao.Should().Be("Arquivo Geral");
            dto.Procedencia.Should().Be("Doação Particular");
            dto.CopiaDigital.Should().BeTrue();
            dto.PermiteUsoImagem.Should().BeFalse();
            dto.ConservacaoId.Should().Be(1);
            dto.Quantidade.Should().Be(500);
            dto.Largura.Should().Be("50cm");
            dto.Altura.Should().Be("70cm");
            dto.SuporteId.Should().Be(5);
            dto.FormatoId.Should().Be(6);
            dto.TamanhoArquivo.Should().Be("20MB");
            dto.CromiaId.Should().Be(7);
            dto.Resolucao.Should().Be("1200DPI");
            dto.Arquivos.Should().ContainInOrder(10, 20, 30, 40, 50);
        }

        [Fact]
        public void DadoId_QuandoAtribuirApenasPropriedadeSpecific_EntaoOutrasPropriedadesMantemValorPadrao()
        {
            var dto = new AcervoFotograficoAlteracaoDTO
            {
                Id = 12345
            };

            dto.Id.Should().Be(12345);
            dto.AcervoId.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoId_QuandoAtribuirApenasPropriedadeSpecific_EntaoOutrasPropriedadesMantemValorPadrao()
        {
            var dto = new AcervoFotograficoAlteracaoDTO
            {
                AcervoId = 67890
            };

            dto.AcervoId.Should().Be(67890);
            dto.Id.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoFotograficoAlteracaoDTO_QuandoAcessarPropriedadesEmSequencia_EntaoPropriedadesInicializadasCorretas()
        {
            var dto = CriarAcervoFotograficoAlteracaoDTOCompleto();

            var id = dto.Id;
            var acervoId = dto.AcervoId;
            var localizacao = dto.Localizacao;
            var procedencia = dto.Procedencia;

            id.Should().Be(12345);
            acervoId.Should().Be(67890);
            localizacao.Should().Be("Setor de Fotografia");
            procedencia.Should().Be("Arquivo Municipal");
        }

        [Fact]
        public void DadoAcervoFotograficoAlteracaoDTO_QuandoModificarPropriedadesVariasVezes_EntaoUltimoValorPreservado()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();

            for (int i = 0; i < 10; i++)
            {
                dto.Id = i;
                dto.AcervoId = i * 2;
                dto.Quantidade = i * 10;
            }

            dto.Id.Should().Be(9);
            dto.AcervoId.Should().Be(18);
            dto.Quantidade.Should().Be(90);
        }

        [Fact]
        public void DadoId_QuandoUtilizarPropertyInfo_EntaoPropriedadeEhValidaParaReflection()
        {
            var propriedadeId = typeof(AcervoFotograficoAlteracaoDTO).GetProperty("Id");

            propriedadeId.Should().NotBeNull();
            propriedadeId!.PropertyType.Should().Be(typeof(long));
        }

        [Fact]
        public void DadoAcervoId_QuandoUtilizarPropertyInfo_EntaoPropriedadeEhValidaParaReflection()
        {
            var propriedadeAcervoId = typeof(AcervoFotograficoAlteracaoDTO).GetProperty("AcervoId");

            propriedadeAcervoId.Should().NotBeNull();
            propriedadeAcervoId!.PropertyType.Should().Be(typeof(long));
        }

        [Fact]
        public void DadoAcervoFotograficoAlteracaoDTO_QuandoCompararPropriedadesAntesEDepois_EntaoPropriedadesAlteradas()
        {
            var dto = new AcervoFotograficoAlteracaoDTO
            {
                Id = 100,
                AcervoId = 200,
                Quantidade = 10
            };

            dto.Id = 150;
            dto.AcervoId = 250;
            dto.Quantidade = 20;

            dto.Id.Should().Be(150);
            dto.AcervoId.Should().Be(250);
            dto.Quantidade.Should().Be(20);
        }

        [Fact]
        public void DadoAcervoFotograficoAlteracaoDTO_QuandoHerdarDeAcervoFotograficoCadastroDTO_EntaoTodosPropriedadesDisponiveis()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();

            // Verificar propriedades herdadas
            dto.Should().BeAssignableTo<AcervoFotograficoCadastroDTO>();

            // Verificar se pode acessar propriedades da classe base
            dto.Localizacao = "Teste Herança";
            dto.Procedencia = "Procedência Teste";
            dto.ConservacaoId = 5;
            dto.Quantidade = 100;

            dto.Localizacao.Should().Be("Teste Herança");
            dto.Procedencia.Should().Be("Procedência Teste");
            dto.ConservacaoId.Should().Be(5);
            dto.Quantidade.Should().Be(100);
        }

        [Fact]
        public void DadoAcervoFotograficoAlteracaoDTO_QuandoUsarPropertyInfoDasPropriedadesEspecificas_EntaoPropriedadesIdentificadas()
        {
            var tipo = typeof(AcervoFotograficoAlteracaoDTO);

            var propriedadeId = tipo.GetProperty("Id");
            var propriedadeAcervoId = tipo.GetProperty("AcervoId");

            propriedadeId.Should().NotBeNull();
            propriedadeAcervoId.Should().NotBeNull();

            propriedadeId!.PropertyType.Should().Be(typeof(long));
            propriedadeAcervoId!.PropertyType.Should().Be(typeof(long));
        }

        [Fact]
        public void DadoId_QuandoAtribuirValoresGrandes_EntaoValoresGrandesArmazenados()
        {
            var valoresGrandes = new[] { 999999999L, 1000000000L, 9999999999L };

            foreach (var valor in valoresGrandes)
            {
                var dto = new AcervoFotograficoAlteracaoDTO { Id = valor };
                dto.Id.Should().Be(valor);
            }
        }

        [Fact]
        public void DadoAcervoId_QuandoAtribuirValoresGrandes_EntaoValoresGrandesArmazenados()
        {
            var valoresGrandes = new[] { 999999999L, 1000000000L, 9999999999L };

            foreach (var valor in valoresGrandes)
            {
                var dto = new AcervoFotograficoAlteracaoDTO { AcervoId = valor };
                dto.AcervoId.Should().Be(valor);
            }
        }

        [Fact]
        public void DadoQuantidade_QuandoAtribuirValorMaximo_EntaoValorMaximoEhArmazenado()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const int quantidadeMaxima = int.MaxValue;

            dto.Quantidade = quantidadeMaxima;

            dto.Quantidade.Should().Be(quantidadeMaxima);
        }

        [Fact]
        public void DadoQuantidade_QuandoAtribuirZero_EntaoZeroEhArmazenado()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            const int quantidadeZero = 0;

            dto.Quantidade = quantidadeZero;

            dto.Quantidade.Should().Be(quantidadeZero);
        }

        [Fact]
        public void DadoArquivos_QuandoAtribuirArrayVazio_EntaoArrayVazioEhArmazenado()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            var arquivosVazios = Array.Empty<long>();

            dto.Arquivos = arquivosVazios;

            dto.Arquivos.Should().BeEmpty();
        }

        [Fact]
        public void DadoArquivos_QuandoAtribuirArrayComMultiplosValores_EntaoArrayEhArmazenado()
        {
            var dto = new AcervoFotograficoAlteracaoDTO();
            var arquivos = new long[] { 1, 2, 3, 4, 5, 100, 200 };

            dto.Arquivos = arquivos;

            dto.Arquivos.Should().HaveCount(7);
            dto.Arquivos.Should().ContainInOrder(1, 2, 3, 4, 5, 100, 200);
        }
    }
}
