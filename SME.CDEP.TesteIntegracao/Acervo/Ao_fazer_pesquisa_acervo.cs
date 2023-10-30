using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_fazer_pesquisa_acervo : TesteBase
    {
        public Ao_fazer_pesquisa_acervo(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        [Fact(DisplayName = "Acervo - Pesquisar acervos por texto")]
        public async Task Pesquisar_acervo_por_texto()
        {
            await InserirDadosBasicos();

            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();
            var servicoAcervo = GetServicoAcervo();

            var arquivos = GerarArquivo(TipoArquivo.AcervoArteGrafica).Generate(10);

            foreach (var arquivo in arquivos)
                await InserirNaBase(arquivo);

            var acervoArteGraficas = GerarAcervoArteGraficaCadastroDTO().Generate(25);

            var contador = 1;

            foreach (var arteGrafica in acervoArteGraficas)
            {
                arteGrafica.Codigo = $"{arteGrafica.Codigo}{contador}";
                await servicoAcervoArteGrafica.Inserir(arteGrafica);
                contador++;
            }

            var filtro = new FiltroTextoLivreTipoAcervoDTO()
            {
                TextoLivre = "a"    
            };
            
            var pesquisa = await servicoAcervo.ObterPorTextoLivreETipoAcervo(filtro);
            pesquisa.ShouldNotBeNull();

            foreach (var pesquisaAcervoDto in pesquisa.Items)
            {
                pesquisaAcervoDto.Descricao.ShouldNotBeEmpty();   
                pesquisaAcervoDto.Codigo.ShouldNotBeEmpty();   
                pesquisaAcervoDto.Assunto.ShouldBeEmpty();   
                pesquisaAcervoDto.Tipo.ShouldBe(TipoAcervo.ArtesGraficas);   
                pesquisaAcervoDto.Titulo.ShouldNotBeEmpty();   
                pesquisaAcervoDto.CreditoAutoria.ShouldNotBeNull();   
                pesquisaAcervoDto.EnderecoImagem.ShouldNotBeEmpty();   
                pesquisaAcervoDto.TipoAcervoTag.ShouldBe(TipoAcervoTag.MemoriaEducacaoMunicipal);   
            }
        }
        
        [Fact(DisplayName = "Acervo - Pesquisar acervos por texto e tipo de acervo")]
        public async Task Pesquisar_acervo_por_texto_tipo_acervo()
        {
            await InserirDadosBasicos();

            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();
            var servicoAcervo = GetServicoAcervo();

            var arquivos = GerarArquivo(TipoArquivo.AcervoArteGrafica).Generate(10);

            foreach (var arquivo in arquivos)
                await InserirNaBase(arquivo);

            var acervoArteGraficas = GerarAcervoArteGraficaCadastroDTO().Generate(25);

            var contador = 1;

            foreach (var arteGrafica in acervoArteGraficas)
            {
                arteGrafica.Codigo = $"{arteGrafica.Codigo}{contador}";
                await servicoAcervoArteGrafica.Inserir(arteGrafica);
                contador++;
            }

            var filtro = new FiltroTextoLivreTipoAcervoDTO()
            {
                TextoLivre = "a",
                TipoAcervo = TipoAcervo.ArtesGraficas
            };
            
            var pesquisa = await servicoAcervo.ObterPorTextoLivreETipoAcervo(filtro);
            pesquisa.ShouldNotBeNull();

            foreach (var pesquisaAcervoDto in pesquisa.Items)
            {
                pesquisaAcervoDto.Descricao.ShouldNotBeEmpty();   
                pesquisaAcervoDto.Codigo.ShouldNotBeEmpty();   
                pesquisaAcervoDto.Assunto.ShouldBeEmpty();   
                pesquisaAcervoDto.Tipo.ShouldBe(TipoAcervo.ArtesGraficas);   
                pesquisaAcervoDto.Titulo.ShouldNotBeEmpty();   
                pesquisaAcervoDto.CreditoAutoria.ShouldNotBeNull();   
                pesquisaAcervoDto.EnderecoImagem.ShouldNotBeEmpty();   
                pesquisaAcervoDto.TipoAcervoTag.ShouldBe(TipoAcervoTag.MemoriaEducacaoMunicipal);   
            }
        }
        
        [Fact(DisplayName = "Acervo - Não deve retornar pesquisa de acervos")]
        public async Task Nao_deve_retornar_pesquisa_de_acervos()
        {
            await InserirDadosBasicos();

            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();
            var servicoAcervo = GetServicoAcervo();

            var arquivos = GerarArquivo(TipoArquivo.AcervoArteGrafica).Generate(10);

            foreach (var arquivo in arquivos)
                await InserirNaBase(arquivo);

            var acervoArteGraficas = GerarAcervoArteGraficaCadastroDTO().Generate(25);
            var contador = 1;

            foreach (var arteGrafica in acervoArteGraficas)
            {
                arteGrafica.Codigo = $"{arteGrafica.Codigo}{contador}";
                await servicoAcervoArteGrafica.Inserir(arteGrafica);
                contador++;
            }

            var filtro = new FiltroTextoLivreTipoAcervoDTO()
            {
                TextoLivre = "a",
                TipoAcervo = TipoAcervo.Fotografico
            };
            
            var pesquisa = await servicoAcervo.ObterPorTextoLivreETipoAcervo(filtro);
            pesquisa.ShouldBeNull();
        }
    }
}