using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
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
                pesquisaAcervoDto.EnderecoImagem.EstaPreenchido().ShouldBeTrue();   
                pesquisaAcervoDto.TipoAcervoTag.ShouldBe(TipoAcervoTag.MemoriaEducacaoMunicipal);   
            }
        }
        
        [Fact(DisplayName = "Acervo - Pesquisar acervos por texto e tipo de acervo")]
        public async Task Pesquisar_acervo_por_texto_tipo_acervo()
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
                pesquisaAcervoDto.EnderecoImagem.EstaPreenchido().ShouldBeTrue();   
                pesquisaAcervoDto.TipoAcervoTag.ShouldBe(TipoAcervoTag.MemoriaEducacaoMunicipal);   
            }
        }
        
        [Fact(DisplayName = "Acervo - Pesquisar acervos por texto, tipo de acervo e ano início")]
        public async Task Pesquisar_acervo_por_texto_tipo_acervo_ano_inicio()
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
                arteGrafica.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy");
                arteGrafica.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
                arteGrafica.Arquivos = arquivosInseridos.Select(s => s.Id).ToArray();
                await servicoAcervoArteGrafica.Inserir(arteGrafica);
                contador++;
            }

            var filtro = new FiltroTextoLivreTipoAcervoDTO()
            {
                TextoLivre = "a",
                TipoAcervo = TipoAcervo.ArtesGraficas,
                AnoInicial = DateTimeExtension.HorarioBrasilia().Year,
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
                pesquisaAcervoDto.EnderecoImagem.EstaPreenchido().ShouldBeTrue();   
                pesquisaAcervoDto.TipoAcervoTag.ShouldBe(TipoAcervoTag.MemoriaEducacaoMunicipal);   
                pesquisaAcervoDto.DataAcervo.ShouldNotBeNull();
                pesquisaAcervoDto.DataAcervo.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"));
            }
        }
        
        [Fact(DisplayName = "Acervo - Pesquisar acervos por texto, tipo de acervo e ano final")]
        public async Task Pesquisar_acervo_por_texto_tipo_acervo_ano_final()
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
                arteGrafica.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy");
                arteGrafica.Ano = DateTimeExtension.HorarioBrasilia().Year.ToString();
                arteGrafica.Arquivos = arquivosInseridos.Select(s => s.Id).ToArray();
                await servicoAcervoArteGrafica.Inserir(arteGrafica);
                contador++;
            }

            var filtro = new FiltroTextoLivreTipoAcervoDTO()
            {
                TextoLivre = "a",
                TipoAcervo = TipoAcervo.ArtesGraficas,
                AnoFinal = DateTimeExtension.HorarioBrasilia().Year,
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
                pesquisaAcervoDto.EnderecoImagem.EstaPreenchido().ShouldBeTrue();   
                pesquisaAcervoDto.TipoAcervoTag.ShouldBe(TipoAcervoTag.MemoriaEducacaoMunicipal);   
                pesquisaAcervoDto.DataAcervo.ShouldNotBeNull();
                pesquisaAcervoDto.DataAcervo.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"));
            }
        }
        
        [Fact(DisplayName = "Acervo - Pesquisar acervos por texto, tipo de acervo, ano inicial e ano final")]
        public async Task Pesquisar_acervo_por_texto_tipo_acervo_ano_inicial_e_final()
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
                arteGrafica.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy");
                arteGrafica.PermiteUsoImagem = true;
                arteGrafica.Arquivos = arquivosInseridos.Select(s => s.Id).ToArray();
                arteGrafica.Ano = DateTimeExtension.HorarioBrasilia().AddYears(-3).Year.ToString();
                await servicoAcervoArteGrafica.Inserir(arteGrafica);
                contador++;
            }

            var filtro = new FiltroTextoLivreTipoAcervoDTO()
            {
                TextoLivre = "a",
                TipoAcervo = TipoAcervo.ArtesGraficas,
                AnoInicial = DateTimeExtension.HorarioBrasilia().AddYears(-5).Year,
                AnoFinal = DateTimeExtension.HorarioBrasilia().Year,
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
                pesquisaAcervoDto.EnderecoImagem.EstaPreenchido().ShouldBeTrue();     
                pesquisaAcervoDto.TipoAcervoTag.ShouldBe(TipoAcervoTag.MemoriaEducacaoMunicipal);   
                pesquisaAcervoDto.DataAcervo.ShouldNotBeNull();
                pesquisaAcervoDto.DataAcervo.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"));
            }
        }
        
        [Fact(DisplayName = "Acervo - Não deve retornar pesquisa de acervos")]
        public async Task Nao_deve_retornar_pesquisa_de_acervos()
        {
            await InserirDadosBasicosAleatorios();

            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();
            var servicoAcervo = GetServicoAcervo();

            var arquivos = ArquivoMock.Instance.GerarArquivo(TipoArquivo.AcervoArteGrafica).Generate(10);

            foreach (var arquivo in arquivos)
                await InserirNaBase(arquivo);

            var acervoArteGraficas =  AcervoArteGraficaDTOMock.GerarAcervoArteGraficaCadastroDTO().Generate(25);
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
        
        [Fact(DisplayName = "Acervo - Pesquisar acervos e não deve trazer imagens pois não permite uso de imagens")]
        public async Task Pesquisar_acervo_sem_imagens_pois_nao_permite_uso_de_imagem()
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
                arteGrafica.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy");
                arteGrafica.Ano = DateTimeExtension.HorarioBrasilia().AddYears(-3).Year.ToString();
                arteGrafica.PermiteUsoImagem = false;
                arteGrafica.Arquivos = arquivosInseridos.Select(s => s.Id).ToArray();
                await servicoAcervoArteGrafica.Inserir(arteGrafica);
                contador++;
            }

            var filtro = new FiltroTextoLivreTipoAcervoDTO()
            {
                TextoLivre = "a",
                TipoAcervo = TipoAcervo.ArtesGraficas,
                AnoInicial = DateTimeExtension.HorarioBrasilia().AddYears(-5).Year,
                AnoFinal = DateTimeExtension.HorarioBrasilia().Year,
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
                pesquisaAcervoDto.EnderecoImagem.NaoEstaPreenchido().ShouldBeTrue();   
                pesquisaAcervoDto.TipoAcervoTag.ShouldBe(TipoAcervoTag.MemoriaEducacaoMunicipal);   
                pesquisaAcervoDto.DataAcervo.ShouldNotBeNull();
                pesquisaAcervoDto.DataAcervo.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"));
            }
        }
        
        [Fact(DisplayName = "Acervo - Pesquisar acervos por ano exato (inicial e final) - retornar acervos com anos exatos")]
        public async Task Pesquisar_acervo_por_ano_exato_inicial_e_final()
        {
            await InserirDadosBasicosAleatorios();

            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();
            var servicoAcervo = GetServicoAcervo();

            var arquivos = ArquivoMock.Instance.GerarArquivo(TipoArquivo.AcervoArteGrafica).Generate(10);

            foreach (var arquivo in arquivos)
                await InserirNaBase(arquivo);

            var arquivosInseridos = ObterTodos<Arquivo>();

            var acervoArteGraficas = AcervoArteGraficaDTOMock.GerarAcervoArteGraficaCadastroDTO().Generate(60);

            var contador = 1;

            foreach (var arteGrafica in acervoArteGraficas)
            {
                arteGrafica.Codigo = $"{arteGrafica.Codigo}{contador}";
                arteGrafica.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy");
                arteGrafica.PermiteUsoImagem = true;
                arteGrafica.Arquivos = arquivosInseridos.Select(s => s.Id).ToArray();

                arteGrafica.Ano = contador switch
                {
                    <= 10 => "[19--]",
                    <= 20 => "[19--?]",
                    <= 30 => "[195-]",
                    <= 40 => "[195-?]",
                    <= 50 => "[1950]",
                    <= 60 => "1950",
                    _ => arteGrafica.Ano
                };

                await servicoAcervoArteGrafica.Inserir(arteGrafica);
                contador++;
            }

            var filtro = new FiltroTextoLivreTipoAcervoDTO()
            {
                AnoInicial = 1950,
                AnoFinal = 1950,
            };
            
            var pesquisa = await servicoAcervo.ObterPorTextoLivreETipoAcervo(filtro);
            pesquisa.ShouldNotBeNull();
            pesquisa.TotalRegistros.ShouldBe(20);
        }
        
        [Fact(DisplayName = "Acervo - Pesquisar acervos por década possível e exata (inicial e final) - retornar acervos com ano exato e décadas certas e possíveis")]
        public async Task Pesquisar_acervo_por_decada_possivel_exata_exato_inicial_e_final()
        {
            await InserirDadosBasicosAleatorios();

            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();
            var servicoAcervo = GetServicoAcervo();

            var arquivos = ArquivoMock.Instance.GerarArquivo(TipoArquivo.AcervoArteGrafica).Generate(10);

            foreach (var arquivo in arquivos)
                await InserirNaBase(arquivo);

            var arquivosInseridos = ObterTodos<Arquivo>();

            var acervoArteGraficas = AcervoArteGraficaDTOMock.GerarAcervoArteGraficaCadastroDTO().Generate(60);

            var contador = 1;

            foreach (var arteGrafica in acervoArteGraficas)
            {
                arteGrafica.Codigo = $"{arteGrafica.Codigo}{contador}";
                arteGrafica.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy");
                arteGrafica.PermiteUsoImagem = true;
                arteGrafica.Arquivos = arquivosInseridos.Select(s => s.Id).ToArray();

                arteGrafica.Ano = contador switch
                {
                    <= 10 => "[19--]",
                    <= 20 => "[19--?]",
                    <= 30 => "[195-]",
                    <= 40 => "[195-?]",
                    <= 50 => "[1950]",
                    <= 60 => "1950",
                    _ => arteGrafica.Ano
                };

                await servicoAcervoArteGrafica.Inserir(arteGrafica);
                contador++;
            }

            var filtro = new FiltroTextoLivreTipoAcervoDTO()
            {
                AnoInicial = 1940,
                AnoFinal = 1960,
            };
            
            var pesquisa = await servicoAcervo.ObterPorTextoLivreETipoAcervo(filtro);
            pesquisa.ShouldNotBeNull();
            pesquisa.TotalRegistros.ShouldBe(40);
        }
        
        [Fact(DisplayName = "Acervo - Pesquisar acervos por século possível e exato (inicial e final) - retornar acervos com anos exatos, décadas e séculos certos e possíveis (tudo)")]
        public async Task Pesquisar_acervo_por_seculo_possivel_exata_exato_inicial_e_final()
        {
            await InserirDadosBasicosAleatorios();

            var servicoAcervoArteGrafica = GetServicoAcervoArteGrafica();
            var servicoAcervo = GetServicoAcervo();

            var arquivos = ArquivoMock.Instance.GerarArquivo(TipoArquivo.AcervoArteGrafica).Generate(10);

            foreach (var arquivo in arquivos)
                await InserirNaBase(arquivo);

            var arquivosInseridos = ObterTodos<Arquivo>();

            var acervoArteGraficas = AcervoArteGraficaDTOMock.GerarAcervoArteGraficaCadastroDTO().Generate(60);

            var contador = 1;

            foreach (var arteGrafica in acervoArteGraficas)
            {
                arteGrafica.Codigo = $"{arteGrafica.Codigo}{contador}";
                arteGrafica.DataAcervo = DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy");
                arteGrafica.PermiteUsoImagem = true;
                arteGrafica.Arquivos = arquivosInseridos.Select(s => s.Id).ToArray();

                arteGrafica.Ano = contador switch
                {
                    <= 10 => "[19--]",
                    <= 20 => "[19--?]",
                    <= 30 => "[195-]",
                    <= 40 => "[195-?]",
                    <= 50 => "[1950]",
                    <= 60 => "1950",
                    _ => arteGrafica.Ano
                };

                await servicoAcervoArteGrafica.Inserir(arteGrafica);
                contador++;
            }

            var filtro = new FiltroTextoLivreTipoAcervoDTO()
            {
                AnoInicial = 1900,
                AnoFinal = 2000,
            };
            
            var pesquisa = await servicoAcervo.ObterPorTextoLivreETipoAcervo(filtro);
            pesquisa.ShouldNotBeNull();
            pesquisa.TotalRegistros.ShouldBe(60);
        }
    }
}