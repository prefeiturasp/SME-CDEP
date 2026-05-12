using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoArteGraficaDetalheDtoTeste
    {
        #region Testes de Instanciação

        [Fact]
        public void DadoConstrutorPadrao_QuandoChamar_EntaoInstanciaComSucesso()
        {
            var dto = new AcervoArteGraficaDetalheDTO();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<AcervoArteGraficaDetalheDTO>();
        }

        [Fact]
        public void DadoInstancia_QuandoVerificar_EntaoEhDerivaDeAcervoDetalheDTO()
        {
            var dto = new AcervoArteGraficaDetalheDTO();

            dto.Should().BeAssignableTo<AcervoDetalheDTO>();
        }

        #endregion

        #region Testes de Propriedades Específicas da Arte Gráfica

        [Fact]
        public void DadoPropriedadesArteGraficaPreenchidas_QuandoInstanciar_EntaoSaoAtribuidas()
        {
            var imagens = new ImagemDTO[]
            {
                new ImagemDTO { Original = "img1.jpg", Thumbnail = "thumb1.jpg" },
                new ImagemDTO { Original = "img2.jpg", Thumbnail = "thumb2.jpg" }
            };

            var dto = new AcervoArteGraficaDetalheDTO
            {
                Descricao = "Descrição da obra",
                CreditosAutores = "Autor 1, Autor 2",
                DataAcervo = "2024-01-15",
                Localizacao = "Sala de Exposição A",
                Procedencia = "Doação",
                CopiaDigital = "Sim",
                PermiteUsoImagem = "Não",
                Conservacao = "Excelente",
                Cromia = "Colorido",
                Tecnica = "Gravura",
                Suporte = "Papel",
                Quantidade = 5,
                Imagens = imagens,
                Dimensoes = "30x40 cm"
            };

            dto.Descricao.Should().Be("Descrição da obra");
            dto.CreditosAutores.Should().Be("Autor 1, Autor 2");
            dto.DataAcervo.Should().Be("2024-01-15");
            dto.Localizacao.Should().Be("Sala de Exposição A");
            dto.Procedencia.Should().Be("Doação");
            dto.CopiaDigital.Should().Be("Sim");
            dto.PermiteUsoImagem.Should().Be("Não");
            dto.Conservacao.Should().Be("Excelente");
            dto.Cromia.Should().Be("Colorido");
            dto.Tecnica.Should().Be("Gravura");
            dto.Suporte.Should().Be("Papel");
            dto.Quantidade.Should().Be(5);
            dto.Imagens.Should().HaveCount(2);
            dto.Dimensoes.Should().Be("30x40 cm");
        }

        [Fact]
        public void DadoPropriedadesArteGraficaNulas_QuandoInstanciar_EntaoValoresDefaultSaoNulos()
        {
            var dto = new AcervoArteGraficaDetalheDTO();

            dto.Descricao.Should().BeNull();
            dto.CreditosAutores.Should().BeNull();
            dto.DataAcervo.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.PermiteUsoImagem.Should().BeNull();
            dto.Conservacao.Should().BeNull();
            dto.Cromia.Should().BeNull();
            dto.Tecnica.Should().BeNull();
            dto.Suporte.Should().BeNull();
            dto.Imagens.Should().BeNull();
            dto.Dimensoes.Should().BeNull();
        }

        [Fact]
        public void DadoQuantidadeDefault_QuandoInstanciar_EntaoEhZero()
        {
            var dto = new AcervoArteGraficaDetalheDTO();

            dto.Quantidade.Should().Be(0);
        }

        #endregion

        #region Testes de Propriedades de Descrição e Créditos

        [Fact]
        public void DadoDescricao_QuandoAlterar_EntaoSobreEscreve()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Descricao = "Descrição Inicial" };

            dto.Descricao = "Descrição Alterada";

            dto.Descricao.Should().Be("Descrição Alterada");
        }

        [Fact]
        public void DadoCreditosAutores_QuandoAlterar_EntaoSobreEscreve()
        {
            var dto = new AcervoArteGraficaDetalheDTO { CreditosAutores = "Autor 1" };

            dto.CreditosAutores = "Autor 1, Autor 2, Autor 3";

            dto.CreditosAutores.Should().Be("Autor 1, Autor 2, Autor 3");
        }

        [Fact]
        public void DadoDescricaoVazia_QuandoAtribuir_EntaoPermiteString()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Descricao = string.Empty };

            dto.Descricao.Should().Be(string.Empty);
        }

        [Fact]
        public void DadoCreditosAutoresVazio_QuandoAtribuir_EntaoPermiteString()
        {
            var dto = new AcervoArteGraficaDetalheDTO { CreditosAutores = string.Empty };

            dto.CreditosAutores.Should().Be(string.Empty);
        }

        #endregion

        #region Testes de Propriedades de Data e Localização

        [Fact]
        public void DadoDataAcervo_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { DataAcervo = "2024-06-20" };

            dto.DataAcervo.Should().Be("2024-06-20");
        }

        [Fact]
        public void DadoLocalizacao_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Localizacao = "Depósito B, Prateleira 5" };

            dto.Localizacao.Should().Be("Depósito B, Prateleira 5");
        }

        [Fact]
        public void DadoProcedencia_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Procedencia = "Compra" };

            dto.Procedencia.Should().Be("Compra");
        }

        [Fact]
        public void DadoLocalizacaoVazia_QuandoAtribuir_EntaoPermiteString()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Localizacao = string.Empty };

            dto.Localizacao.Should().Be(string.Empty);
        }

        #endregion

        #region Testes de Propriedades de Cópia Digital e Uso de Imagem

        [Fact]
        public void DadoCopiaDigitalSim_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { CopiaDigital = "Sim" };

            dto.CopiaDigital.Should().Be("Sim");
        }

        [Fact]
        public void DadoCopiaDigitalNao_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { CopiaDigital = "Não" };

            dto.CopiaDigital.Should().Be("Não");
        }

        [Fact]
        public void DadoPermiteUsoImagemSim_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { PermiteUsoImagem = "Sim" };

            dto.PermiteUsoImagem.Should().Be("Sim");
        }

        [Fact]
        public void DadoPermiteUsoImagemNao_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { PermiteUsoImagem = "Não" };

            dto.PermiteUsoImagem.Should().Be("Não");
        }

        [Fact]
        public void DadoCopiaDigitalVazia_QuandoAtribuir_EntaoPermiteString()
        {
            var dto = new AcervoArteGraficaDetalheDTO { CopiaDigital = string.Empty };

            dto.CopiaDigital.Should().Be(string.Empty);
        }

        #endregion

        #region Testes de Propriedades de Conservação e Técnica

        [Fact]
        public void DadoConservacao_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Conservacao = "Bom" };

            dto.Conservacao.Should().Be("Bom");
        }

        [Fact]
        public void DadoCromia_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Cromia = "Preto e Branco" };

            dto.Cromia.Should().Be("Preto e Branco");
        }

        [Fact]
        public void DadoTecnica_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Tecnica = "Xilogravura" };

            dto.Tecnica.Should().Be("Xilogravura");
        }

        [Fact]
        public void DadoSuporte_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Suporte = "Tela" };

            dto.Suporte.Should().Be("Tela");
        }

        [Fact]
        public void DadoConservacaoVazia_QuandoAtribuir_EntaoPermiteString()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Conservacao = string.Empty };

            dto.Conservacao.Should().Be(string.Empty);
        }

        #endregion

        #region Testes de Propriedade Quantidade

        [Fact]
        public void DadoQuantidadePositiva_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Quantidade = 100 };

            dto.Quantidade.Should().Be(100);
        }

        [Fact]
        public void DadoQuantidadeUm_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Quantidade = 1 };

            dto.Quantidade.Should().Be(1);
        }

        [Fact]
        public void DadoQuantidadeMaximaLong_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Quantidade = long.MaxValue };

            dto.Quantidade.Should().Be(long.MaxValue);
        }

        [Fact]
        public void DadoQuantidadeZero_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Quantidade = 0 };

            dto.Quantidade.Should().Be(0);
        }

        #endregion

        #region Testes de Propriedade Imagens

        [Fact]
        public void DadoUmaImagem_QuandoAtribuir_EntaoArmazena()
        {
            var imagens = new ImagemDTO[]
            {
                new ImagemDTO { Original = "img1.jpg", Thumbnail = "thumb1.jpg" }
            };

            var dto = new AcervoArteGraficaDetalheDTO { Imagens = imagens };

            dto.Imagens.Should().HaveCount(1);
            dto.Imagens[0].Original.Should().Be("img1.jpg");
            dto.Imagens[0].Thumbnail.Should().Be("thumb1.jpg");
        }

        [Fact]
        public void DadoMultiplasImagens_QuandoAtribuir_EntaoArmazena()
        {
            var imagens = new ImagemDTO[]
            {
                new ImagemDTO { Original = "img1.jpg", Thumbnail = "thumb1.jpg" },
                new ImagemDTO { Original = "img2.jpg", Thumbnail = "thumb2.jpg" },
                new ImagemDTO { Original = "img3.jpg", Thumbnail = "thumb3.jpg" }
            };

            var dto = new AcervoArteGraficaDetalheDTO { Imagens = imagens };

            dto.Imagens.Should().HaveCount(3);
            dto.Imagens.Should().BeEquivalentTo(imagens);
        }

        [Fact]
        public void DadoImagemVazia_QuandoAtribuir_EntaoArmazena()
        {
            var imagens = new ImagemDTO[] { };

            var dto = new AcervoArteGraficaDetalheDTO { Imagens = imagens };

            dto.Imagens.Should().HaveCount(0);
        }

        [Fact]
        public void DadoImgensSemThumbnail_QuandoAtribuir_EntaoArmazena()
        {
            var imagens = new ImagemDTO[]
            {
                new ImagemDTO { Original = "img1.jpg", Thumbnail = null! }
            };

            var dto = new AcervoArteGraficaDetalheDTO { Imagens = imagens };

            dto.Imagens[0].Thumbnail.Should().BeNull();
        }

        #endregion

        #region Testes de Propriedade Dimensões

        [Fact]
        public void DadoDimensoes_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Dimensoes = "50x70 cm" };

            dto.Dimensoes.Should().Be("50x70 cm");
        }

        [Fact]
        public void DadoDimensoesComMultiplosValores_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Dimensoes = "Altura: 30cm, Largura: 40cm, Profundidade: 5cm" };

            dto.Dimensoes.Should().Be("Altura: 30cm, Largura: 40cm, Profundidade: 5cm");
        }

        [Fact]
        public void DadoDimensoesVazia_QuandoAtribuir_EntaoPermiteString()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Dimensoes = string.Empty };

            dto.Dimensoes.Should().Be(string.Empty);
        }

        #endregion

        #region Testes de Propriedades Herdadas de AcervoDetalheDTO

        [Fact]
        public void DadoPropriedadesHerdadas_QuandoInstanciar_EntaoValoresPadroSaoNulos()
        {
            var dto = new AcervoArteGraficaDetalheDTO();

            dto.Titulo.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.Ano.Should().BeNull();
            dto.EnderecoImagemPadrao.Should().BeNull();
            dto.SituacaoDisponibilidade.Should().BeNull();
        }

        [Fact]
        public void DadoPropriedadesHerdadasComValoresPadrao_QuandoInstanciar_EntaoValoresPadroSaoFalseZero()
        {
            var dto = new AcervoArteGraficaDetalheDTO();

            dto.AcervoId.Should().Be(0);
            dto.EstaDisponivel.Should().BeFalse();
            dto.TemControleDisponibilidade.Should().BeFalse();
            dto.TipoAcervoId.Should().Be(0);
        }

        [Fact]
        public void DadoPropriedadesHerdadasPreenchidas_QuandoInstanciar_EntaoSaoAtribuidas()
        {
            var dto = new AcervoArteGraficaDetalheDTO
            {
                Titulo = "Obra Histórica",
                Codigo = "AR001",
                Ano = "1980",
                AcervoId = 123,
                EnderecoImagemPadrao = "/imagens/obra1.jpg",
                SituacaoDisponibilidade = "Disponível",
                EstaDisponivel = true,
                TemControleDisponibilidade = true,
                TipoAcervoId = 5
            };

            dto.Titulo.Should().Be("Obra Histórica");
            dto.Codigo.Should().Be("AR001");
            dto.Ano.Should().Be("1980");
            dto.AcervoId.Should().Be(123);
            dto.EnderecoImagemPadrao.Should().Be("/imagens/obra1.jpg");
            dto.SituacaoDisponibilidade.Should().Be("Disponível");
            dto.EstaDisponivel.Should().BeTrue();
            dto.TemControleDisponibilidade.Should().BeTrue();
            dto.TipoAcervoId.Should().Be(5);
        }

        [Fact]
        public void DadoTitulo_QuandoAlterar_EntaoSobreEscreve()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Titulo = "Título Inicial" };

            dto.Titulo = "Título Alterado";

            dto.Titulo.Should().Be("Título Alterado");
        }

        [Fact]
        public void DadoCodigo_QuandoAlterar_EntaoSobreEscreve()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Codigo = "001" };

            dto.Codigo = "002";

            dto.Codigo.Should().Be("002");
        }

        #endregion

        #region Testes de Múltiplas Propriedades Combinadas

        [Fact]
        public void DadoTodosCamposPreenchidos_QuandoCriar_EntaoTodosArmazenam()
        {
            var imagens = new ImagemDTO[]
            {
                new ImagemDTO { Original = "img1.jpg", Thumbnail = "thumb1.jpg" }
            };

            var dto = new AcervoArteGraficaDetalheDTO
            {
                Titulo = "Obra Completa",
                Codigo = "OC001",
                Ano = "2020",
                AcervoId = 999,
                EnderecoImagemPadrao = "/imagens/padrao.jpg",
                SituacaoDisponibilidade = "Emprestado",
                EstaDisponivel = false,
                TemControleDisponibilidade = true,
                TipoAcervoId = 3,
                Descricao = "Descrição completa",
                CreditosAutores = "Autor Principal",
                DataAcervo = "2020-05-10",
                Localizacao = "Local Específico",
                Procedencia = "Herança",
                CopiaDigital = "Sim",
                PermiteUsoImagem = "Sim",
                Conservacao = "Ótimo",
                Cromia = "Colorido",
                Tecnica = "Fotografia",
                Suporte = "Papel Fotográfico",
                Quantidade = 1,
                Imagens = imagens,
                Dimensoes = "20x25 cm"
            };

            dto.Titulo.Should().Be("Obra Completa");
            dto.Codigo.Should().Be("OC001");
            dto.Ano.Should().Be("2020");
            dto.AcervoId.Should().Be(999);
            dto.EnderecoImagemPadrao.Should().Be("/imagens/padrao.jpg");
            dto.SituacaoDisponibilidade.Should().Be("Emprestado");
            dto.EstaDisponivel.Should().BeFalse();
            dto.TemControleDisponibilidade.Should().BeTrue();
            dto.TipoAcervoId.Should().Be(3);
            dto.Descricao.Should().Be("Descrição completa");
            dto.CreditosAutores.Should().Be("Autor Principal");
            dto.DataAcervo.Should().Be("2020-05-10");
            dto.Localizacao.Should().Be("Local Específico");
            dto.Procedencia.Should().Be("Herança");
            dto.CopiaDigital.Should().Be("Sim");
            dto.PermiteUsoImagem.Should().Be("Sim");
            dto.Conservacao.Should().Be("Ótimo");
            dto.Cromia.Should().Be("Colorido");
            dto.Tecnica.Should().Be("Fotografia");
            dto.Suporte.Should().Be("Papel Fotográfico");
            dto.Quantidade.Should().Be(1);
            dto.Imagens.Should().HaveCount(1);
            dto.Dimensoes.Should().Be("20x25 cm");
        }

        [Fact]
        public void DadoMinimoDeCamposPreenchidos_QuandoCriar_EntaoOutrosCamposPermanecemNulos()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Titulo = "Mínimo" };

            dto.Titulo.Should().Be("Mínimo");
            dto.Codigo.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.Quantidade.Should().Be(0);
        }

        #endregion

        #region Testes de Tipos de Propriedades

        [Fact]
        public void DadoPropriedadesString_QuandoVerificar_EntaoSaoTipoString()
        {
            var dto = new AcervoArteGraficaDetalheDTO();
            var tipo = typeof(AcervoArteGraficaDetalheDTO);

            var propriedadesString = new[]
            {
                "Descricao", "CreditosAutores", "DataAcervo", "Localizacao", "Procedencia",
                "CopiaDigital", "PermiteUsoImagem", "Conservacao", "Cromia", "Tecnica", "Suporte", "Dimensoes"
            };

            foreach (var propriedade in propriedadesString)
            {
                var prop = tipo.GetProperty(propriedade);
                prop.Should().NotBeNull();
                prop!.PropertyType.Should().Be(typeof(string));
            }
        }

        [Fact]
        public void DadoPropriedadeQuantidade_QuandoVerificar_EntaoEhTipoLong()
        {
            var tipo = typeof(AcervoArteGraficaDetalheDTO);
            var propriedade = tipo.GetProperty(nameof(AcervoArteGraficaDetalheDTO.Quantidade));

            propriedade.Should().NotBeNull();
            propriedade!.PropertyType.Should().Be(typeof(long));
        }

        [Fact]
        public void DadoPropriedadeImagens_QuandoVerificar_EntaoEhArrayDeImagemDTO()
        {
            var tipo = typeof(AcervoArteGraficaDetalheDTO);
            var propriedade = tipo.GetProperty(nameof(AcervoArteGraficaDetalheDTO.Imagens));

            propriedade.Should().NotBeNull();
            propriedade!.PropertyType.Should().Be(typeof(ImagemDTO[]));
        }

        #endregion

        #region Testes de Reflexão e Atributos

        [Fact]
        public void DadoClasse_QuandoVerificar_EntaoTemTodasAsPropriedadesEsperadas()
        {
            var tipo = typeof(AcervoArteGraficaDetalheDTO);
            var propriedades = tipo.GetProperties();

            var propriedadesEsperadas = new[]
            {
                nameof(AcervoArteGraficaDetalheDTO.Descricao),
                nameof(AcervoArteGraficaDetalheDTO.CreditosAutores),
                nameof(AcervoArteGraficaDetalheDTO.DataAcervo),
                nameof(AcervoArteGraficaDetalheDTO.Localizacao),
                nameof(AcervoArteGraficaDetalheDTO.Procedencia),
                nameof(AcervoArteGraficaDetalheDTO.CopiaDigital),
                nameof(AcervoArteGraficaDetalheDTO.PermiteUsoImagem),
                nameof(AcervoArteGraficaDetalheDTO.Conservacao),
                nameof(AcervoArteGraficaDetalheDTO.Cromia),
                nameof(AcervoArteGraficaDetalheDTO.Tecnica),
                nameof(AcervoArteGraficaDetalheDTO.Suporte),
                nameof(AcervoArteGraficaDetalheDTO.Quantidade),
                nameof(AcervoArteGraficaDetalheDTO.Imagens),
                nameof(AcervoArteGraficaDetalheDTO.Dimensoes)
            };

            foreach (var propEsperada in propriedadesEsperadas)
            {
                propriedades.Should().Contain(p => p.Name == propEsperada);
            }
        }

        [Fact]
        public void DadoPropriedades_QuandoVerificar_EntaoTodosSaoPublicos()
        {
            var tipo = typeof(AcervoArteGraficaDetalheDTO);
            var propriedades = tipo.GetProperties();

            foreach (var propriedade in propriedades)
            {
                var getter = propriedade.GetGetMethod();
                var setter = propriedade.GetSetMethod();

                getter.Should().NotBeNull();
                getter!.IsPublic.Should().BeTrue();
                setter.Should().NotBeNull();
                setter!.IsPublic.Should().BeTrue();
            }
        }

        [Fact]
        public void DadoClasse_QuandoVerificar_EntaoEhPublica()
        {
            var tipo = typeof(AcervoArteGraficaDetalheDTO);

            tipo.IsPublic.Should().BeTrue();
        }

        #endregion

        #region Testes de Valores Extremos e Edge Cases

        [Fact]
        public void DadoDescricaoMuitoLonga_QuandoAtribuir_EntaoArmazena()
        {
            var descricaoLonga = new string('A', 10000);
            var dto = new AcervoArteGraficaDetalheDTO { Descricao = descricaoLonga };

            dto.Descricao.Should().Be(descricaoLonga);
            dto.Descricao.Length.Should().Be(10000);
        }

        [Fact]
        public void DadoQuantidadeNegativa_QuandoAtribuir_EntaoArmazena()
        {
            var dto = new AcervoArteGraficaDetalheDTO { Quantidade = -1 };

            dto.Quantidade.Should().Be(-1);
        }

        [Fact]
        public void DadoMúltiplasAlteracoesConstrutivasEmSequencia_QuandoAlterar_EntaoMantémUltimoValor()
        {
            var dto = new AcervoArteGraficaDetalheDTO();

            dto.Descricao = "Descrição 1";
            dto.Descricao = "Descrição 2";
            dto.Descricao = "Descrição 3";

            dto.Descricao.Should().Be("Descrição 3");
        }

        [Fact]
        public void DadoImagensModificadasAposAtribuicao_QuandoVerificar_EntaoImagensCadastradasSaoAcessiveis()
        {
            var imagens = new ImagemDTO[]
            {
                new ImagemDTO { Original = "img1.jpg", Thumbnail = "thumb1.jpg" }
            };

            var dto = new AcervoArteGraficaDetalheDTO { Imagens = imagens };

            imagens[0].Original = "img_modificada.jpg";

            dto.Imagens[0].Original.Should().Be("img_modificada.jpg");
        }

        #endregion

        #region Testes de Inicialização com Object Initializer

        [Fact]
        public void DadoObjectInitializer_QuandoUsarMultiplosParametros_EntaoTodosArmazenam()
        {
            var dto = new AcervoArteGraficaDetalheDTO
            {
                Titulo = "Teste",
                Descricao = "Descrição",
                Cromia = "P&B",
                Quantidade = 5
            };

            dto.Should().NotBeNull();
            dto.Titulo.Should().Be("Teste");
            dto.Descricao.Should().Be("Descrição");
            dto.Cromia.Should().Be("P&B");
            dto.Quantidade.Should().Be(5);
        }

        [Fact]
        public void DadoObjectInitializerVazio_QuandoCriar_EntaoInstanciaComValoresPadrao()
        {
            var dto = new AcervoArteGraficaDetalheDTO { };

            dto.Should().NotBeNull();
            dto.Quantidade.Should().Be(0);
        }

        #endregion
    }
}
