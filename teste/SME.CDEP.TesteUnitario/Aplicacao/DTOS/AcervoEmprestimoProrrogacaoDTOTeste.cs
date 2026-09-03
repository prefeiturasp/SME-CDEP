using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoEmprestimoProrrogacaoDTOTeste
    {
        private static AcervoEmprestimoProrrogacaoDTO CriarAcervoEmprestimoProrrogacaoDTOCompleto()
        {
            return new AcervoEmprestimoProrrogacaoDTO
            {
                AcervoSolicitacaoItemId = 12345,
                DataDevolucao = new DateTime(2024, 12, 31)
            };
        }

        [Fact]
        public void DadoAcervoEmprestimoProrrogacaoDTO_QuandoInstanciar_EntaoTodosPropriedadesSaoInicializadas()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();

            dto.AcervoSolicitacaoItemId.Should().Be(0);
            dto.DataDevolucao.Should().Be(default(DateTime));
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemId_QuandoDefinirValor_EntaoAcervoSolicitacaoItemIdEhAtribuido()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            const long acervoSolicitacaoItemIdEsperado = 999;

            dto.AcervoSolicitacaoItemId = acervoSolicitacaoItemIdEsperado;

            dto.AcervoSolicitacaoItemId.Should().Be(acervoSolicitacaoItemIdEsperado);
        }

        [Fact]
        public void DadoDataDevolucao_QuandoDefinirValor_EntaoDataDevolucaoEhAtribuida()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            var dataDevolucaoEsperada = new DateTime(2025, 06, 15);

            dto.DataDevolucao = dataDevolucaoEsperada;

            dto.DataDevolucao.Should().Be(dataDevolucaoEsperada);
        }

        [Fact]
        public void DadoAcervoEmprestimoProrrogacaoDTO_QuandoDefiniTodosOsValores_EntaoTodosCamposSaoAcessiveis()
        {
            var dto = CriarAcervoEmprestimoProrrogacaoDTOCompleto();

            dto.AcervoSolicitacaoItemId.Should().Be(12345);
            dto.DataDevolucao.Should().Be(new DateTime(2024, 12, 31));
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemId_QuandoAtribuirValorMaximo_EntaoValorEhArmazenado()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            const long idMaximo = long.MaxValue;

            dto.AcervoSolicitacaoItemId = idMaximo;

            dto.AcervoSolicitacaoItemId.Should().Be(idMaximo);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemId_QuandoAtribuirZero_EntaoValorEhArmazenado()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            const long idZero = 0;

            dto.AcervoSolicitacaoItemId = idZero;

            dto.AcervoSolicitacaoItemId.Should().Be(idZero);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemId_QuandoAtribuirValorUm_EntaoValorEhArmazenado()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            const long idUm = 1;

            dto.AcervoSolicitacaoItemId = idUm;

            dto.AcervoSolicitacaoItemId.Should().Be(idUm);
        }

        [Fact]
        public void DadoDataDevolucao_QuandoAtribuirDataAtual_EntaoDataAtualEhArmazenada()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            var dataAtual = DateTime.Now;

            dto.DataDevolucao = dataAtual;

            dto.DataDevolucao.Should().Be(dataAtual);
        }

        [Fact]
        public void DadoDataDevolucao_QuandoAtribuirDataFutura_EntaoDataFuturaEhArmazenada()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            var dataFutura = DateTime.Now.AddDays(30);

            dto.DataDevolucao = dataFutura;

            dto.DataDevolucao.Should().Be(dataFutura);
        }

        [Fact]
        public void DadoDataDevolucao_QuandoAtribuirDataPassada_EntaoDataPassadaEhArmazenada()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            var dataPassada = DateTime.Now.AddDays(-30);

            dto.DataDevolucao = dataPassada;

            dto.DataDevolucao.Should().Be(dataPassada);
        }

        [Fact]
        public void DadoDataDevolucao_QuandoAtribuirDataMinima_EntaoDataMinimaEhArmazenada()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            var dataMinima = DateTime.MinValue;

            dto.DataDevolucao = dataMinima;

            dto.DataDevolucao.Should().Be(dataMinima);
        }

        [Fact]
        public void DadoDataDevolucao_QuandoAtribuirDataMaxima_EntaoDataMaximaEhArmazenada()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            var dataMaxima = DateTime.MaxValue;

            dto.DataDevolucao = dataMaxima;

            dto.DataDevolucao.Should().Be(dataMaxima);
        }

        [Fact]
        public void DadoDataDevolucao_QuandoAtribuirDataComHora_EntaoDataComHoraEhArmazenada()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            var dataComHora = new DateTime(2024, 12, 31, 23, 59, 59);

            dto.DataDevolucao = dataComHora;

            dto.DataDevolucao.Should().Be(dataComHora);
        }

        [Fact]
        public void DadoDataDevolucao_QuandoAtribuirDataComMilissegundos_EntaoDataEhArmazenada()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            var dataComMilissegundos = new DateTime(2024, 12, 31, 12, 30, 45, 123);

            dto.DataDevolucao = dataComMilissegundos;

            dto.DataDevolucao.Should().Be(dataComMilissegundos);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemId_QuandoAtribuirMultiplosValores_EntaoUltimoValorEhPreservado()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();

            dto.AcervoSolicitacaoItemId = 100;
            dto.AcervoSolicitacaoItemId = 200;
            dto.AcervoSolicitacaoItemId = 300;

            dto.AcervoSolicitacaoItemId.Should().Be(300);
        }

        [Fact]
        public void DadoDataDevolucao_QuandoAtribuirMultiplosDatas_EntaoUltimaDataEhPreservada()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            var primeiraData = new DateTime(2024, 12, 31);
            var segundaData = new DateTime(2025, 01, 15);
            var terceiraData = new DateTime(2025, 02, 28);

            dto.DataDevolucao = primeiraData;
            dto.DataDevolucao = segundaData;
            dto.DataDevolucao = terceiraData;

            dto.DataDevolucao.Should().Be(terceiraData);
        }

        [Fact]
        public void DadoAcervoEmprestimoProrrogacaoDTO_QuandoInicializarComConstrutorVazio_EntaoNaoLancaExcecao()
        {
            var exception = Record.Exception(() =>
            {
                var dto = new AcervoEmprestimoProrrogacaoDTO();
            });

            exception.Should().BeNull();
        }

        [Fact]
        public void DadoAcervoEmprestimoProrrogacaoDTO_QuandoAcessarPropriedadesComConstrutorVazio_EntaoPropriedadesNaoLancamExcecao()
        {
            var exception = Record.Exception(() =>
            {
                var dto = new AcervoEmprestimoProrrogacaoDTO();
                _ = dto.AcervoSolicitacaoItemId;
                _ = dto.DataDevolucao;
            });

            exception.Should().BeNull();
        }

        [Fact]
        public void DadoDuasInstancias_QuandoComMesmosValores_EntaoSaoInstanciasDistintas()
        {
            var dto1 = CriarAcervoEmprestimoProrrogacaoDTOCompleto();
            var dto2 = CriarAcervoEmprestimoProrrogacaoDTOCompleto();

            dto1.Should().NotBeSameAs(dto2);
            dto1.AcervoSolicitacaoItemId.Should().Be(dto2.AcervoSolicitacaoItemId);
            dto1.DataDevolucao.Should().Be(dto2.DataDevolucao);
        }

        [Fact]
        public void DadoAcervoEmprestimoProrrogacaoDTO_QuandoInicializarComValues_EntaoValoresEhArmazenados()
        {
            var dataEsperada = new DateTime(2024, 12, 25);
            const long acervoIdEsperado = 5678;

            var dto = new AcervoEmprestimoProrrogacaoDTO
            {
                AcervoSolicitacaoItemId = acervoIdEsperado,
                DataDevolucao = dataEsperada
            };

            dto.AcervoSolicitacaoItemId.Should().Be(acervoIdEsperado);
            dto.DataDevolucao.Should().Be(dataEsperada);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemId_QuandoAtribuirValoresSequenciais_EntaoValoresSequenciaisArmazenados()
        {
            var dto1 = new AcervoEmprestimoProrrogacaoDTO { AcervoSolicitacaoItemId = 1 };
            var dto2 = new AcervoEmprestimoProrrogacaoDTO { AcervoSolicitacaoItemId = 2 };
            var dto3 = new AcervoEmprestimoProrrogacaoDTO { AcervoSolicitacaoItemId = 3 };

            dto1.AcervoSolicitacaoItemId.Should().Be(1);
            dto2.AcervoSolicitacaoItemId.Should().Be(2);
            dto3.AcervoSolicitacaoItemId.Should().Be(3);
        }

        [Fact]
        public void DadoDataDevolucao_QuandoAtribuirDatasSequenciais_EntaoDatasSequenciaisArmazenadas()
        {
            var data1 = new DateTime(2024, 12, 01);
            var data2 = new DateTime(2024, 12, 15);
            var data3 = new DateTime(2024, 12, 31);

            var dto1 = new AcervoEmprestimoProrrogacaoDTO { DataDevolucao = data1 };
            var dto2 = new AcervoEmprestimoProrrogacaoDTO { DataDevolucao = data2 };
            var dto3 = new AcervoEmprestimoProrrogacaoDTO { DataDevolucao = data3 };

            dto1.DataDevolucao.Should().Be(data1);
            dto2.DataDevolucao.Should().Be(data2);
            dto3.DataDevolucao.Should().Be(data3);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemId_QuandoAtribuirValorNegativo_EntaoValorNegativoEhArmazenado()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            const long idNegativo = -1;

            dto.AcervoSolicitacaoItemId = idNegativo;

            dto.AcervoSolicitacaoItemId.Should().Be(idNegativo);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemId_QuandoAtribuirValoresGrandes_EntaoValoresGrandesArmazenados()
        {
            var valoresGrandes = new[] { 999999999L, 1000000000L, 9999999999L };

            foreach (var valor in valoresGrandes)
            {
                var dto = new AcervoEmprestimoProrrogacaoDTO { AcervoSolicitacaoItemId = valor };
                dto.AcervoSolicitacaoItemId.Should().Be(valor);
            }
        }

        [Fact]
        public void DadoDataDevolucao_QuandoAtribuirDatasDeAnosDistintos_EntaoDatasArmazenadas()
        {
            var ano2020 = new DateTime(2020, 01, 01);
            var ano2024 = new DateTime(2024, 06, 15);
            var ano2030 = new DateTime(2030, 12, 31);

            var dto1 = new AcervoEmprestimoProrrogacaoDTO { DataDevolucao = ano2020 };
            var dto2 = new AcervoEmprestimoProrrogacaoDTO { DataDevolucao = ano2024 };
            var dto3 = new AcervoEmprestimoProrrogacaoDTO { DataDevolucao = ano2030 };

            dto1.DataDevolucao.Should().Be(ano2020);
            dto2.DataDevolucao.Should().Be(ano2024);
            dto3.DataDevolucao.Should().Be(ano2030);
        }

        [Fact]
        public void DadoDataDevolucao_QuandoAtribuirDatasDeBysDiferentes_EntaoDatasArmazenadas()
        {
            var dataInicio = new DateTime(2024, 01, 01);
            var dataMeio = new DateTime(2024, 06, 15);
            var dataFim = new DateTime(2024, 12, 31);

            var dto1 = new AcervoEmprestimoProrrogacaoDTO { DataDevolucao = dataInicio };
            var dto2 = new AcervoEmprestimoProrrogacaoDTO { DataDevolucao = dataMeio };
            var dto3 = new AcervoEmprestimoProrrogacaoDTO { DataDevolucao = dataFim };

            dto1.DataDevolucao.Month.Should().Be(1);
            dto2.DataDevolucao.Month.Should().Be(6);
            dto3.DataDevolucao.Month.Should().Be(12);
        }

        [Fact]
        public void DadoAcervoEmprestimoProrrogacaoDTO_QuandoCompararPropriedadesAntesEDepois_EntaoPropriedadesAlteradas()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO
            {
                AcervoSolicitacaoItemId = 100,
                DataDevolucao = new DateTime(2024, 12, 31)
            };

            dto.AcervoSolicitacaoItemId = 200;
            dto.DataDevolucao = new DateTime(2025, 01, 15);

            dto.AcervoSolicitacaoItemId.Should().Be(200);
            dto.DataDevolucao.Should().Be(new DateTime(2025, 01, 15));
        }

        [Fact]
        public void DadoDataDevolucao_QuandoAtribuirPrimeiroDiaDoAno_EntaoDataArmazenada()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            var primeiroDia = new DateTime(2024, 01, 01);

            dto.DataDevolucao = primeiroDia;

            dto.DataDevolucao.Should().Be(primeiroDia);
            dto.DataDevolucao.Month.Should().Be(1);
            dto.DataDevolucao.Day.Should().Be(1);
        }

        [Fact]
        public void DadoDataDevolucao_QuandoAtribuirUltimoDiaDoAno_EntaoDataArmazenada()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            var ultimoDia = new DateTime(2024, 12, 31);

            dto.DataDevolucao = ultimoDia;

            dto.DataDevolucao.Should().Be(ultimoDia);
            dto.DataDevolucao.Month.Should().Be(12);
            dto.DataDevolucao.Day.Should().Be(31);
        }

        [Fact]
        public void DadoDataDevolucao_QuandoAtribuirDataDeAnoBissexto_EntaoDataArmazenada()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            var dataBissexta = new DateTime(2024, 02, 29);

            dto.DataDevolucao = dataBissexta;

            dto.DataDevolucao.Should().Be(dataBissexta);
            dto.DataDevolucao.Day.Should().Be(29);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemId_QuandoAtribuirApenasPropriedadeSpecific_EntaoOutrasPropriedadesMantemValorPadrao()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO
            {
                AcervoSolicitacaoItemId = 12345
            };

            dto.AcervoSolicitacaoItemId.Should().Be(12345);
            dto.DataDevolucao.Should().Be(default(DateTime));
        }

        [Fact]
        public void DadoDataDevolucao_QuandoAtribuirApenasPropriedadeSpecific_EntaoOutrasPropriedadesMantemValorPadrao()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO
            {
                DataDevolucao = new DateTime(2024, 12, 31)
            };

            dto.DataDevolucao.Should().Be(new DateTime(2024, 12, 31));
            dto.AcervoSolicitacaoItemId.Should().Be(0);
        }

        [Fact]
        public void DadoAcervoEmprestimoProrrogacaoDTO_QuandoAcessarPropriedadesEmSequencia_EntaoPropriedadesInicializadasCorretas()
        {
            var dto = CriarAcervoEmprestimoProrrogacaoDTOCompleto();

            var acervoId = dto.AcervoSolicitacaoItemId;
            var dataDevol = dto.DataDevolucao;

            acervoId.Should().Be(12345);
            dataDevol.Should().Be(new DateTime(2024, 12, 31));
        }

        [Fact]
        public void DadoAcervoEmprestimoProrrogacaoDTO_QuandoModificarPropriedadesVariasVezes_EntaoUltimoValorPreservado()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();

            for (int i = 0; i < 10; i++)
            {
                dto.AcervoSolicitacaoItemId = i;
                dto.DataDevolucao = new DateTime(2024, 01, 01).AddDays(i);
            }

            dto.AcervoSolicitacaoItemId.Should().Be(9);
            dto.DataDevolucao.Should().Be(new DateTime(2024, 01, 10));
        }

        [Fact]
        public void DadoDataDevolucao_QuandoUtilizarPropertyInfo_EntaoPropriedadeEhValidaParaReflection()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            var propriedadeDataDevolucao = typeof(AcervoEmprestimoProrrogacaoDTO).GetProperty("DataDevolucao");

            propriedadeDataDevolucao.Should().NotBeNull();
            propriedadeDataDevolucao!.PropertyType.Should().Be<DateTime>();
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemId_QuandoUtilizarPropertyInfo_EntaoPropriedadeEhValidaParaReflection()
        {
            var dto = new AcervoEmprestimoProrrogacaoDTO();
            var propriedadeAcervoId = typeof(AcervoEmprestimoProrrogacaoDTO).GetProperty("AcervoSolicitacaoItemId");

            propriedadeAcervoId.Should().NotBeNull();
            propriedadeAcervoId!.PropertyType.Should().Be<long>();
        }

        [Fact]
        public void DadoAcervoEmprestimoProrrogacaoDTO_QuandoInstanciarComTodasAsPropriedades_EntaoTodosCamposAcessiveis()
        {
            var dataEsperada = new DateTime(2025, 12, 31, 23, 59, 59, 999);
            const long acervoIdEsperado = 987654321;

            var dto = new AcervoEmprestimoProrrogacaoDTO
            {
                AcervoSolicitacaoItemId = acervoIdEsperado,
                DataDevolucao = dataEsperada
            };

            dto.Should().NotBeNull();
            dto.AcervoSolicitacaoItemId.Should().Be(acervoIdEsperado);
            dto.DataDevolucao.Should().Be(dataEsperada);
            dto.AcervoSolicitacaoItemId.Should().Be(987654321);
            dto.DataDevolucao.Year.Should().Be(2025);
            dto.DataDevolucao.Month.Should().Be(12);
            dto.DataDevolucao.Day.Should().Be(31);
        }
    }
}
