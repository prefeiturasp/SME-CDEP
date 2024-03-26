using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_detalhar_acervo : TesteBase
    {
        private readonly IServicoAcervoArteGrafica servicoAcervoArteGrafica;
        private readonly IServicoAcervo servicoAcervo;
        
        public Ao_detalhar_acervo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();
            servicoAcervo = GetServicoAcervo();
        }

        [Fact(DisplayName = "Acervo - Detalhar acervos com imagens")]
        public async Task Detalhar_acervo_exibindo_imagens_permite_uso_de_imagens()
        {
            await InserirDadosBasicosAleatorios();

            var arquivos = ArquivoMock.Instance.GerarArquivo(TipoArquivo.AcervoArteGrafica).Generate(10);
            GerarArquivosSistema(arquivos);
            
            foreach (var arquivo in arquivos)
                await InserirNaBase(arquivo);
            
            var arquivosInseridos = ObterTodos<Arquivo>();

            var acervoArteGraficas = AcervoArteGraficaDTOMock.GerarAcervoArteGraficaCadastroDTO().Generate(25);

            var contador = 1;

            foreach (var arteGrafica in acervoArteGraficas)
            {
                arteGrafica.Codigo = $"{arteGrafica.Codigo}{contador}";
                arteGrafica.Arquivos = arquivosInseridos.Where(w=> w.Tipo.NaoEhTipoArquivoSistema()).Select(s => s.Id).ToArray();
                await servicoAcervoArteGrafica.Inserir(arteGrafica);
                contador++;
            }

            var acervosInseridos = ObterTodos<Acervo>();
            
            var filtro = new FiltroDetalharAcervoDTO()
            {
                Codigo = acervosInseridos.FirstOrDefault().Codigo,
                Tipo = TipoAcervo.ArtesGraficas
            };
            
            var detalhamentoDoAcervo = await servicoAcervo.ObterDetalhamentoPorTipoAcervoECodigo(filtro);
            detalhamentoDoAcervo.ShouldNotBeNull();
            var detalhamentoDoAcervoArteGrafica = (AcervoArteGraficaDetalheDTO)detalhamentoDoAcervo;

            detalhamentoDoAcervoArteGrafica.Descricao.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.CreditosAutores.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.DataAcervo.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.Localizacao.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.Procedencia.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.CopiaDigital.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.PermiteUsoImagem.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.Conservacao.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.Cromia.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.Tecnica.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.Suporte.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.Dimensoes.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.Quantidade.ShouldBeGreaterThan(0);
            detalhamentoDoAcervoArteGrafica.Imagens.NaoEhNulo().ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo - Detalhar acervos sem imagens")]
        public async Task Detalhar_acervo_Sem_imagens_nao_permite_uso_de_imagens()
        {
            await InserirDadosBasicosAleatorios();

            var arquivos = ArquivoMock.Instance.GerarArquivo(TipoArquivo.AcervoArteGrafica).Generate(10);
            GerarArquivosSistema(arquivos);
            
            foreach (var arquivo in arquivos)
                await InserirNaBase(arquivo);
            
            var arquivosInseridos = ObterTodos<Arquivo>();

            var acervoArteGraficas = AcervoArteGraficaDTOMock.GerarAcervoArteGraficaCadastroDTO().Generate(25);

            var contador = 1;

            foreach (var arteGrafica in acervoArteGraficas)
            {
                arteGrafica.Codigo = $"{arteGrafica.Codigo}{contador}";
                arteGrafica.Arquivos = arquivosInseridos.Where(w=> w.Tipo.NaoEhTipoArquivoSistema()).Select(s => s.Id).ToArray();
                arteGrafica.PermiteUsoImagem = false;
                await servicoAcervoArteGrafica.Inserir(arteGrafica);
                contador++;
            }

            var acervosInseridos = ObterTodos<Acervo>();
            
            var filtro = new FiltroDetalharAcervoDTO()
            {
                Codigo = acervosInseridos.FirstOrDefault().Codigo,
                Tipo = TipoAcervo.ArtesGraficas
            };
            
            var detalhamentoDoAcervo = await servicoAcervo.ObterDetalhamentoPorTipoAcervoECodigo(filtro);
            detalhamentoDoAcervo.ShouldNotBeNull();
            var detalhamentoDoAcervoArteGrafica = (AcervoArteGraficaDetalheDTO)detalhamentoDoAcervo;

            detalhamentoDoAcervoArteGrafica.Descricao.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.CreditosAutores.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.DataAcervo.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.Localizacao.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.Procedencia.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.CopiaDigital.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.PermiteUsoImagem.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.Conservacao.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.Cromia.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.Tecnica.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.Suporte.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.Dimensoes.EstaPreenchido().ShouldBeTrue();
            detalhamentoDoAcervoArteGrafica.Quantidade.ShouldBeGreaterThan(0);
            detalhamentoDoAcervoArteGrafica.Imagens.Any().ShouldBeFalse();
        }
        
        [Theory(DisplayName = "Acervo - Detalhar acervos indisponíveis (emprestado e reservado)")]
        [InlineData(SituacaoSaldo.RESERVADO)]
        [InlineData(SituacaoSaldo.EMPRESTADO)]
        public async Task Detalhar_acervo_indisponiveis_emprestado_e_reservado(SituacaoSaldo situacaoSaldo)
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos(situacaoSaldo);

            await GerarArquivosSistema();

            var acervosInseridos = ObterTodos<Acervo>();
            
            var filtro = new FiltroDetalharAcervoDTO()
            {
                Codigo = acervosInseridos.FirstOrDefault().Codigo,
                Tipo = TipoAcervo.Bibliografico
            };
            
            var detalhamentoDoAcervo = await servicoAcervo.ObterDetalhamentoPorTipoAcervoECodigo(filtro);
            detalhamentoDoAcervo.ShouldNotBeNull();
            var detalhamentoDoAcervoArteGrafica = (AcervoBibliograficoDetalheDTO)detalhamentoDoAcervo;

            detalhamentoDoAcervoArteGrafica.Codigo.ShouldBe(filtro.Codigo);
            detalhamentoDoAcervoArteGrafica.EstaDisponivel.ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Acervo - Detalhar acervos disponíveis")]
        public async Task Detalhar_acervo_disponiveis()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await GerarArquivosSistema();

            var acervosInseridos = ObterTodos<Acervo>();
            
            var filtro = new FiltroDetalharAcervoDTO()
            {
                Codigo = acervosInseridos.FirstOrDefault().Codigo,
                Tipo = TipoAcervo.Bibliografico
            };
            
            var detalhamentoDoAcervo = await servicoAcervo.ObterDetalhamentoPorTipoAcervoECodigo(filtro);
            detalhamentoDoAcervo.ShouldNotBeNull();
            var detalhamentoDoAcervoArteGrafica = (AcervoBibliograficoDetalheDTO)detalhamentoDoAcervo;

            detalhamentoDoAcervoArteGrafica.Codigo.ShouldBe(filtro.Codigo);
            detalhamentoDoAcervoArteGrafica.EstaDisponivel.ShouldBeTrue();
        }
    }
}