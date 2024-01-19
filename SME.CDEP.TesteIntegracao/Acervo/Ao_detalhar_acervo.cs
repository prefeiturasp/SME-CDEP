﻿using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_detalhar_acervo : TesteBase
    {
        public Ao_detalhar_acervo(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        [Fact(DisplayName = "Acervo - Detalhar acervos com imagens")]
        public async Task Detalhar_acervo_exibindo_imagens_permite_uso_de_imagens()
        {
            await InserirDadosBasicosAleatorios();

            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();
            var servicoAcervo = GetServicoAcervo();

            var arquivos = ArquivoMock.Instance.GerarArquivo(TipoArquivo.AcervoArteGrafica).Generate(10);

            foreach (var arquivo in arquivos)
                await InserirNaBase(arquivo);
            
            var arquivosInseridos = ObterTodos<Arquivo>();

            var acervoArteGraficas = AcervoArteGraficaDTOMock.GerarAcervoArteGraficaCadastroDTO().Generate(25);

            var contador = 1;

            foreach (var arteGrafica in acervoArteGraficas)
            {
                arteGrafica.Codigo = $"{arteGrafica.Codigo}{contador}";
                arteGrafica.Arquivos = arquivosInseridos.Select(s => s.Id).ToArray();
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

            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();
            var servicoAcervo = GetServicoAcervo();

            var arquivos = ArquivoMock.Instance.GerarArquivo(TipoArquivo.AcervoArteGrafica).Generate(10);

            foreach (var arquivo in arquivos)
                await InserirNaBase(arquivo);
            
            var arquivosInseridos = ObterTodos<Arquivo>();

            var acervoArteGraficas = AcervoArteGraficaDTOMock.GerarAcervoArteGraficaCadastroDTO().Generate(25);

            var contador = 1;

            foreach (var arteGrafica in acervoArteGraficas)
            {
                arteGrafica.Codigo = $"{arteGrafica.Codigo}{contador}";
                arteGrafica.Arquivos = arquivosInseridos.Select(s => s.Id).ToArray();
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
    }
}