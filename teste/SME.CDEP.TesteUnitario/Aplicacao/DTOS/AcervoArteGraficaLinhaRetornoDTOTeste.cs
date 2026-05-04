using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoArteGraficaLinhaRetornoDTOTeste
    {
        #region Testes de Instanciação

        [Fact]
        public void DadoConstrutorPadrao_QuandoChamar_EntaoInstanciaComSucesso()
        {
            var dto = new AcervoArteGraficaLinhaRetornoDTO();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<AcervoArteGraficaLinhaRetornoDTO>();
        }

        [Fact]
        public void DadoConstrutorPadrao_QuandoChamar_EntaoHerdaDeAcervoLinhaRetornoDTO()
        {
            var dto = new AcervoArteGraficaLinhaRetornoDTO();

            dto.Should().BeAssignableTo<AcervoLinhaRetornoDTO>();
        }

        #endregion

        #region Testes de Propriedades - Valores Padrão

        [Fact]
        public void DadoPropriedades_QuandoInstanciar_EntaoValoresPadroSaoNulos()
        {
            var dto = new AcervoArteGraficaLinhaRetornoDTO();

            dto.Titulo.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.CreditosAutoresIds.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.PermiteUsoImagem.Should().BeNull();
            dto.ConservacaoId.Should().BeNull();
            dto.CromiaId.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Diametro.Should().BeNull();
            dto.Tecnica.Should().BeNull();
            dto.SuporteId.Should().BeNull();
            dto.Quantidade.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoPropriedadesHerdadas_QuandoInstanciar_EntaoValoresPadroEstaoCorretos()
        {
            var dto = new AcervoArteGraficaLinhaRetornoDTO();

            dto.Status.Should().Be(default(ImportacaoStatus));
            dto.NumeroLinha.Should().Be(0);
            dto.Mensagem.Should().BeNull();
            dto.ErrosCampos.Should().BeNull();
        }

        #endregion

        #region Testes de Atribuição de Propriedade Titulo

        [Fact]
        public void DadoTituloAtribuido_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var titulo = CriarLinhaConteudoAjustarRetorno("Título Teste");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Titulo = titulo };

            dto.Titulo.Should().Be(titulo);
            dto.Titulo.Conteudo.Should().Be("Título Teste");
        }

        [Fact]
        public void DadoTituloAlterado_QuandoReatribuir_EntaoSobreEscreve()
        {
            var tituloAntigo = CriarLinhaConteudoAjustarRetorno("Título Antigo");
            var tituloNovo = CriarLinhaConteudoAjustarRetorno("Título Novo");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Titulo = tituloAntigo };

            dto.Titulo = tituloNovo;

            dto.Titulo.Should().Be(tituloNovo);
            dto.Titulo.Conteudo.Should().Be("Título Novo");
        }

        [Fact]
        public void DadoTituloVazio_QuandoAtribuir_EntaoArmazena()
        {
            var titulo = CriarLinhaConteudoAjustarRetorno(string.Empty);
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Titulo = titulo };

            dto.Titulo.Should().NotBeNull();
            dto.Titulo.Conteudo.Should().BeEmpty();
        }

        #endregion

        #region Testes de Atribuição de Propriedade Codigo

        [Fact]
        public void DadoCodigoAtribuido_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var codigo = CriarLinhaConteudoAjustarRetorno("COD-001");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Codigo = codigo };

            dto.Codigo.Should().Be(codigo);
            dto.Codigo.Conteudo.Should().Be("COD-001");
        }

        [Fact]
        public void DadoCodigoComCaracteresEspeciais_QuandoAtribuir_EntaoArmazena()
        {
            var codigo = CriarLinhaConteudoAjustarRetorno("COD-001-@#$");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Codigo = codigo };

            dto.Codigo.Conteudo.Should().Be("COD-001-@#$");
        }

        #endregion

        #region Testes de Atribuição de Propriedade CreditosAutoresIds

        [Fact]
        public void DadoCreditosAutoresIdsAtribuido_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var creditosAutores = CriarLinhaConteudoAjustarRetorno("1,2,3");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { CreditosAutoresIds = creditosAutores };

            dto.CreditosAutoresIds.Should().Be(creditosAutores);
            dto.CreditosAutoresIds.Conteudo.Should().Be("1,2,3");
        }

        [Fact]
        public void DadoCreditosAutoresIdsMultiplos_QuandoAtribuir_EntaoArmazena()
        {
            var creditosAutores = CriarLinhaConteudoAjustarRetorno("101,205,304");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { CreditosAutoresIds = creditosAutores };

            dto.CreditosAutoresIds.Conteudo.Should().Be("101,205,304");
        }

        #endregion

        #region Testes de Atribuição de Propriedade Localizacao

        [Fact]
        public void DadoLocalizacaoAtribuida_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var localizacao = CriarLinhaConteudoAjustarRetorno("Sala 1, Prateleira 2");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Localizacao = localizacao };

            dto.Localizacao.Should().Be(localizacao);
            dto.Localizacao.Conteudo.Should().Be("Sala 1, Prateleira 2");
        }

        [Fact]
        public void DadoLocalizacaoCompleta_QuandoAtribuir_EntaoArmazena()
        {
            var localizacao = CriarLinhaConteudoAjustarRetorno("Edifício A, Sala 10, Prateleira 5, Posição 3");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Localizacao = localizacao };

            dto.Localizacao.Conteudo.Should().Contain("Edifício A");
        }

        #endregion

        #region Testes de Atribuição de Propriedade Procedencia

        [Fact]
        public void DadoProcedenciaAtribuida_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var procedencia = CriarLinhaConteudoAjustarRetorno("Doação");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Procedencia = procedencia };

            dto.Procedencia.Should().Be(procedencia);
            dto.Procedencia.Conteudo.Should().Be("Doação");
        }

        [Fact]
        public void DadoProcedenciaCompra_QuandoAtribuir_EntaoArmazena()
        {
            var procedencia = CriarLinhaConteudoAjustarRetorno("Compra");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Procedencia = procedencia };

            dto.Procedencia.Conteudo.Should().Be("Compra");
        }

        [Fact]
        public void DadoProcedenciaHeranca_QuandoAtribuir_EntaoArmazena()
        {
            var procedencia = CriarLinhaConteudoAjustarRetorno("Herança");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Procedencia = procedencia };

            dto.Procedencia.Conteudo.Should().Be("Herança");
        }

        #endregion

        #region Testes de Atribuição de Propriedade CopiaDigital

        [Fact]
        public void DadoCopiaDigitalAtribuida_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var copiaDigital = CriarLinhaConteudoAjustarRetorno("Sim");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { CopiaDigital = copiaDigital };

            dto.CopiaDigital.Should().Be(copiaDigital);
            dto.CopiaDigital.Conteudo.Should().Be("Sim");
        }

        [Fact]
        public void DadoCopiaDigitalNao_QuandoAtribuir_EntaoArmazena()
        {
            var copiaDigital = CriarLinhaConteudoAjustarRetorno("Não");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { CopiaDigital = copiaDigital };

            dto.CopiaDigital.Conteudo.Should().Be("Não");
        }

        #endregion

        #region Testes de Atribuição de Propriedade PermiteUsoImagem

        [Fact]
        public void DadoPermiteUsoImagemAtribuido_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var permiteUsoImagem = CriarLinhaConteudoAjustarRetorno("Não");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { PermiteUsoImagem = permiteUsoImagem };

            dto.PermiteUsoImagem.Should().Be(permiteUsoImagem);
            dto.PermiteUsoImagem.Conteudo.Should().Be("Não");
        }

        [Fact]
        public void DadoPermiteUsoImagemSim_QuandoAtribuir_EntaoArmazena()
        {
            var permiteUsoImagem = CriarLinhaConteudoAjustarRetorno("Sim");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { PermiteUsoImagem = permiteUsoImagem };

            dto.PermiteUsoImagem.Conteudo.Should().Be("Sim");
        }

        #endregion

        #region Testes de Atribuição de Propriedade ConservacaoId

        [Fact]
        public void DadoConservacaoIdAtribuido_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var conservacaoId = CriarLinhaConteudoAjustarRetorno("1");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { ConservacaoId = conservacaoId };

            dto.ConservacaoId.Should().Be(conservacaoId);
            dto.ConservacaoId.Conteudo.Should().Be("1");
        }

        [Fact]
        public void DadoConservacaoIdMultiplos_QuandoAtribuir_EntaoArmazena()
        {
            var conservacaoId = CriarLinhaConteudoAjustarRetorno("5");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { ConservacaoId = conservacaoId };

            dto.ConservacaoId.Conteudo.Should().Be("5");
        }

        #endregion

        #region Testes de Atribuição de Propriedade CromiaId

        [Fact]
        public void DadoCromiaIdAtribuido_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var cromiaId = CriarLinhaConteudoAjustarRetorno("2");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { CromiaId = cromiaId };

            dto.CromiaId.Should().Be(cromiaId);
            dto.CromiaId.Conteudo.Should().Be("2");
        }

        [Fact]
        public void DadoCromiaIdDiferentes_QuandoAtribuir_EntaoArmazena()
        {
            var cromiaId = CriarLinhaConteudoAjustarRetorno("3");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { CromiaId = cromiaId };

            dto.CromiaId.Conteudo.Should().Be("3");
        }

        #endregion

        #region Testes de Atribuição de Propriedade Largura

        [Fact]
        public void DadoLarguraAtribuida_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var largura = CriarLinhaConteudoAjustarRetorno("50cm");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Largura = largura };

            dto.Largura.Should().Be(largura);
            dto.Largura.Conteudo.Should().Be("50cm");
        }

        [Fact]
        public void DadoLarguraEmMetros_QuandoAtribuir_EntaoArmazena()
        {
            var largura = CriarLinhaConteudoAjustarRetorno("1.5m");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Largura = largura };

            dto.Largura.Conteudo.Should().Be("1.5m");
        }

        [Fact]
        public void DadoLarguraSomenteNumeros_QuandoAtribuir_EntaoArmazena()
        {
            var largura = CriarLinhaConteudoAjustarRetorno("100");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Largura = largura };

            dto.Largura.Conteudo.Should().Be("100");
        }

        #endregion

        #region Testes de Atribuição de Propriedade Altura

        [Fact]
        public void DadoAlturaAtribuida_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var altura = CriarLinhaConteudoAjustarRetorno("80cm");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Altura = altura };

            dto.Altura.Should().Be(altura);
            dto.Altura.Conteudo.Should().Be("80cm");
        }

        [Fact]
        public void DadoAlturaEmMetros_QuandoAtribuir_EntaoArmazena()
        {
            var altura = CriarLinhaConteudoAjustarRetorno("2.5m");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Altura = altura };

            dto.Altura.Conteudo.Should().Be("2.5m");
        }

        #endregion

        #region Testes de Atribuição de Propriedade Diametro

        [Fact]
        public void DadoDiametroAtribuido_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var diametro = CriarLinhaConteudoAjustarRetorno("30cm");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Diametro = diametro };

            dto.Diametro.Should().Be(diametro);
            dto.Diametro.Conteudo.Should().Be("30cm");
        }

        [Fact]
        public void DadoDiametroEmMilimetros_QuandoAtribuir_EntaoArmazena()
        {
            var diametro = CriarLinhaConteudoAjustarRetorno("150mm");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Diametro = diametro };

            dto.Diametro.Conteudo.Should().Be("150mm");
        }

        #endregion

        #region Testes de Atribuição de Propriedade Tecnica

        [Fact]
        public void DadoTecnicaAtribuida_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var tecnica = CriarLinhaConteudoAjustarRetorno("Aquarela");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Tecnica = tecnica };

            dto.Tecnica.Should().Be(tecnica);
            dto.Tecnica.Conteudo.Should().Be("Aquarela");
        }

        [Fact]
        public void DadoTecnicaFotografia_QuandoAtribuir_EntaoArmazena()
        {
            var tecnica = CriarLinhaConteudoAjustarRetorno("Fotografia");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Tecnica = tecnica };

            dto.Tecnica.Conteudo.Should().Be("Fotografia");
        }

        [Fact]
        public void DadoTecnicaMista_QuandoAtribuir_EntaoArmazena()
        {
            var tecnica = CriarLinhaConteudoAjustarRetorno("Técnica Mista");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Tecnica = tecnica };

            dto.Tecnica.Conteudo.Should().Be("Técnica Mista");
        }

        #endregion

        #region Testes de Atribuição de Propriedade SuporteId

        [Fact]
        public void DadoSuporteIdAtribuido_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var suporteId = CriarLinhaConteudoAjustarRetorno("1");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { SuporteId = suporteId };

            dto.SuporteId.Should().Be(suporteId);
            dto.SuporteId.Conteudo.Should().Be("1");
        }

        [Fact]
        public void DadoSuporteIdDiferentes_QuandoAtribuir_EntaoArmazena()
        {
            var suporteId = CriarLinhaConteudoAjustarRetorno("5");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { SuporteId = suporteId };

            dto.SuporteId.Conteudo.Should().Be("5");
        }

        #endregion

        #region Testes de Atribuição de Propriedade Quantidade

        [Fact]
        public void DadoQuantidadeAtribuida_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var quantidade = CriarLinhaConteudoAjustarRetorno("5");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Quantidade = quantidade };

            dto.Quantidade.Should().Be(quantidade);
            dto.Quantidade.Conteudo.Should().Be("5");
        }

        [Fact]
        public void DadoQuantidadeUm_QuandoAtribuir_EntaoArmazena()
        {
            var quantidade = CriarLinhaConteudoAjustarRetorno("1");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Quantidade = quantidade };

            dto.Quantidade.Conteudo.Should().Be("1");
        }

        [Fact]
        public void DadoQuantidadeGrande_QuandoAtribuir_EntaoArmazena()
        {
            var quantidade = CriarLinhaConteudoAjustarRetorno("9999");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Quantidade = quantidade };

            dto.Quantidade.Conteudo.Should().Be("9999");
        }

        #endregion

        #region Testes de Atribuição de Propriedade Descricao

        [Fact]
        public void DadoDescricaoAtribuida_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var descricao = CriarLinhaConteudoAjustarRetorno("Descrição da obra");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Descricao = descricao };

            dto.Descricao.Should().Be(descricao);
            dto.Descricao.Conteudo.Should().Be("Descrição da obra");
        }

        [Fact]
        public void DadoDescricaoLonga_QuandoAtribuir_EntaoArmazena()
        {
            var descricaoLonga = "Esta é uma descrição muito longa que contém detalhes completos sobre a obra de arte gráfica incluindo seu contexto histórico e importância cultural.";
            var descricao = CriarLinhaConteudoAjustarRetorno(descricaoLonga);
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Descricao = descricao };

            dto.Descricao.Conteudo.Should().Be(descricaoLonga);
        }

        [Fact]
        public void DadoDescricaoVazia_QuandoAtribuir_EntaoArmazena()
        {
            var descricao = CriarLinhaConteudoAjustarRetorno(string.Empty);
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Descricao = descricao };

            dto.Descricao.Conteudo.Should().BeEmpty();
        }

        #endregion

        #region Testes de Atribuição de Propriedade Ano

        [Fact]
        public void DadoAnoAtribuido_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var ano = CriarLinhaConteudoAjustarRetorno("2024");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Ano = ano };

            dto.Ano.Should().Be(ano);
            dto.Ano.Conteudo.Should().Be("2024");
        }

        [Fact]
        public void DadoAnoAntigo_QuandoAtribuir_EntaoArmazena()
        {
            var ano = CriarLinhaConteudoAjustarRetorno("1850");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Ano = ano };

            dto.Ano.Conteudo.Should().Be("1850");
        }

        [Fact]
        public void DadoAnoIntervalo_QuandoAtribuir_EntaoArmazena()
        {
            var ano = CriarLinhaConteudoAjustarRetorno("1950-1955");
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Ano = ano };

            dto.Ano.Conteudo.Should().Be("1950-1955");
        }

        #endregion

        #region Testes de Múltiplas Propriedades

        [Fact]
        public void DadoMultiplasPropriedadesAtribuidas_QuandoInstanciar_EntaoTodasSaoAtribuidasCorreto()
        {
            var titulo = CriarLinhaConteudoAjustarRetorno("Obra Completa");
            var codigo = CriarLinhaConteudoAjustarRetorno("COD-123");
            var creditosAutores = CriarLinhaConteudoAjustarRetorno("1,2");
            var localizacao = CriarLinhaConteudoAjustarRetorno("Galeria Principal");
            var procedencia = CriarLinhaConteudoAjustarRetorno("Compra");
            var ano = CriarLinhaConteudoAjustarRetorno("2020");

            var dto = new AcervoArteGraficaLinhaRetornoDTO
            {
                Titulo = titulo,
                Codigo = codigo,
                CreditosAutoresIds = creditosAutores,
                Localizacao = localizacao,
                Procedencia = procedencia,
                Ano = ano,
                NumeroLinha = 5,
                Status = ImportacaoStatus.Pendente,
                Mensagem = "Dados iniciais"
            };

            dto.Titulo.Should().Be(titulo);
            dto.Codigo.Should().Be(codigo);
            dto.CreditosAutoresIds.Should().Be(creditosAutores);
            dto.Localizacao.Should().Be(localizacao);
            dto.Procedencia.Should().Be(procedencia);
            dto.Ano.Should().Be(ano);
            dto.NumeroLinha.Should().Be(5);
            dto.Status.Should().Be(ImportacaoStatus.Pendente);
            dto.Mensagem.Should().Be("Dados iniciais");
        }

        [Fact]
        public void DadoTodosOsCamposArteGraficaPreenchidos_QuandoInstanciar_EntaoTodosArmazenam()
        {
            var dto = new AcervoArteGraficaLinhaRetornoDTO
            {
                Titulo = CriarLinhaConteudoAjustarRetorno("Título"),
                Codigo = CriarLinhaConteudoAjustarRetorno("COD"),
                CreditosAutoresIds = CriarLinhaConteudoAjustarRetorno("1,2,3"),
                Localizacao = CriarLinhaConteudoAjustarRetorno("Local"),
                Procedencia = CriarLinhaConteudoAjustarRetorno("Doação"),
                CopiaDigital = CriarLinhaConteudoAjustarRetorno("Sim"),
                PermiteUsoImagem = CriarLinhaConteudoAjustarRetorno("Não"),
                ConservacaoId = CriarLinhaConteudoAjustarRetorno("1"),
                CromiaId = CriarLinhaConteudoAjustarRetorno("2"),
                Largura = CriarLinhaConteudoAjustarRetorno("50cm"),
                Altura = CriarLinhaConteudoAjustarRetorno("80cm"),
                Diametro = CriarLinhaConteudoAjustarRetorno("30cm"),
                Tecnica = CriarLinhaConteudoAjustarRetorno("Gravura"),
                SuporteId = CriarLinhaConteudoAjustarRetorno("1"),
                Quantidade = CriarLinhaConteudoAjustarRetorno("5"),
                Descricao = CriarLinhaConteudoAjustarRetorno("Descrição"),
                Ano = CriarLinhaConteudoAjustarRetorno("2020")
            };

            VerificarTodosOsCampos(dto);
        }

        #endregion

        #region Testes de Herança

        [Fact]
        public void DadoAcervoArteGraficaLinhaRetornoDTO_QuandoHerdarDeAcervoLinhaRetornoDTO_EntaoTemPropriedadesHerdadas()
        {
            var dto = new AcervoArteGraficaLinhaRetornoDTO();

            typeof(AcervoArteGraficaLinhaRetornoDTO).BaseType.Should().Be(typeof(AcervoLinhaRetornoDTO));
        }

        [Fact]
        public void DadoAcervoArteGraficaLinhaRetornoDTO_QuandoVerificarPropriedadesHerdadas_EntaoTemStatus()
        {
            var propriedade = typeof(AcervoArteGraficaLinhaRetornoDTO).GetProperty(nameof(AcervoLinhaRetornoDTO.Status));

            propriedade.Should().NotBeNull();
        }

        [Fact]
        public void DadoAcervoArteGraficaLinhaRetornoDTO_QuandoVerificarPropriedadesHerdadas_EntaoTemMensagem()
        {
            var propriedade = typeof(AcervoArteGraficaLinhaRetornoDTO).GetProperty(nameof(AcervoLinhaRetornoDTO.Mensagem));

            propriedade.Should().NotBeNull();
        }

        [Fact]
        public void DadoAcervoArteGraficaLinhaRetornoDTO_QuandoVerificarPropriedadesHerdadas_EntaoTemNumeroLinha()
        {
            var propriedade = typeof(AcervoArteGraficaLinhaRetornoDTO).GetProperty(nameof(AcervoLinhaRetornoDTO.NumeroLinha));

            propriedade.Should().NotBeNull();
        }

        [Fact]
        public void DadoAcervoArteGraficaLinhaRetornoDTO_QuandoVerificarPropriedadesHerdadas_EntaoTemErrosCampos()
        {
            var propriedade = typeof(AcervoArteGraficaLinhaRetornoDTO).GetProperty(nameof(AcervoLinhaRetornoDTO.ErrosCampos));

            propriedade.Should().NotBeNull();
        }

        [Fact]
        public void DadoClasse_QuandoVerificar_EntaoEhPublica()
        {
            var tipo = typeof(AcervoArteGraficaLinhaRetornoDTO);

            tipo.IsPublic.Should().BeTrue();
        }

        #endregion

        #region Testes de Propriedades da Classe

        [Fact]
        public void DadoClasse_QuandoVerificar_EntaoTemTodasAsPropriedadesEsperadas()
        {
            var tipo = typeof(AcervoArteGraficaLinhaRetornoDTO);
            var propriedades = tipo.GetProperties();

            var propriedadesEsperadas = new[]
            {
                nameof(AcervoArteGraficaLinhaRetornoDTO.Titulo),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Codigo),
                nameof(AcervoArteGraficaLinhaRetornoDTO.CreditosAutoresIds),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Localizacao),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Procedencia),
                nameof(AcervoArteGraficaLinhaRetornoDTO.CopiaDigital),
                nameof(AcervoArteGraficaLinhaRetornoDTO.PermiteUsoImagem),
                nameof(AcervoArteGraficaLinhaRetornoDTO.ConservacaoId),
                nameof(AcervoArteGraficaLinhaRetornoDTO.CromiaId),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Largura),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Altura),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Diametro),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Tecnica),
                nameof(AcervoArteGraficaLinhaRetornoDTO.SuporteId),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Quantidade),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Descricao),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Ano)
            };

            foreach (var propEsperada in propriedadesEsperadas)
            {
                propriedades.Should().Contain(p => p.Name == propEsperada);
            }
        }

        [Fact]
        public void DadoPropriedades_QuandoVerificar_EntaoTodosSaoDoTipoLinhaConteudoAjustarRetorno()
        {
            var tipo = typeof(AcervoArteGraficaLinhaRetornoDTO);
            var propriedadesSpecificas = new[]
            {
                nameof(AcervoArteGraficaLinhaRetornoDTO.Titulo),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Codigo),
                nameof(AcervoArteGraficaLinhaRetornoDTO.CreditosAutoresIds),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Localizacao),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Procedencia),
                nameof(AcervoArteGraficaLinhaRetornoDTO.CopiaDigital),
                nameof(AcervoArteGraficaLinhaRetornoDTO.PermiteUsoImagem),
                nameof(AcervoArteGraficaLinhaRetornoDTO.ConservacaoId),
                nameof(AcervoArteGraficaLinhaRetornoDTO.CromiaId),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Largura),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Altura),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Diametro),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Tecnica),
                nameof(AcervoArteGraficaLinhaRetornoDTO.SuporteId),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Quantidade),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Descricao),
                nameof(AcervoArteGraficaLinhaRetornoDTO.Ano)
            };

            foreach (var propName in propriedadesSpecificas)
            {
                var prop = tipo.GetProperty(propName);
                prop.Should().NotBeNull();
                prop!.PropertyType.Should().Be(typeof(LinhaConteudoAjustarRetornoDTO));
            }
        }

        [Fact]
        public void DadoPropriedades_QuandoVerificar_EntaoTodosSaoPublicos()
        {
            var tipo = typeof(AcervoArteGraficaLinhaRetornoDTO);
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

        #endregion

        #region Testes de Valores Extremos

        [Fact]
        public void DadoConteudoMuitoLongo_QuandoAtribuir_EntaoArmazena()
        {
            var conteudoLongo = new string('A', 5000);
            var descricao = CriarLinhaConteudoAjustarRetorno(conteudoLongo);
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Descricao = descricao };

            dto.Descricao.Conteudo.Length.Should().Be(5000);
        }

        [Fact]
        public void DadoConteudoComCaracteresEspeciais_QuandoAtribuir_EntaoArmazena()
        {
            var conteudoEspecial = "!@#$%^&*()_+-=[]{}|;':\",./<>?";
            var codigo = CriarLinhaConteudoAjustarRetorno(conteudoEspecial);
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Codigo = codigo };

            dto.Codigo.Conteudo.Should().Be(conteudoEspecial);
        }

        [Fact]
        public void DadoConteudoComUnicode_QuandoAtribuir_EntaoArmazena()
        {
            var conteudoUnicode = "Acrylique, Óleo, Café, 日本語";
            var tecnica = CriarLinhaConteudoAjustarRetorno(conteudoUnicode);
            var dto = new AcervoArteGraficaLinhaRetornoDTO { Tecnica = tecnica };

            dto.Tecnica.Conteudo.Should().Be(conteudoUnicode);
        }

        #endregion

        #region Testes de Múltiplas Alterações

        [Fact]
        public void DadoMúltiplasAlteracoesConstrutivasEmSequencia_QuandoAlterar_EntaoMantémUltimoValor()
        {
            var dto = new AcervoArteGraficaLinhaRetornoDTO();

            var titulo1 = CriarLinhaConteudoAjustarRetorno("Título 1");
            var titulo2 = CriarLinhaConteudoAjustarRetorno("Título 2");
            var titulo3 = CriarLinhaConteudoAjustarRetorno("Título 3");

            dto.Titulo = titulo1;
            dto.Titulo = titulo2;
            dto.Titulo = titulo3;

            dto.Titulo.Should().Be(titulo3);
            dto.Titulo.Conteudo.Should().Be("Título 3");
        }

        [Fact]
        public void DadoMúltiplasPropriedadesAlteradasEmSequencia_QuandoAlterar_EntaoTodasMantêmUltimosValores()
        {
            var dto = new AcervoArteGraficaLinhaRetornoDTO
            {
                Titulo = CriarLinhaConteudoAjustarRetorno("Título Original"),
                Codigo = CriarLinhaConteudoAjustarRetorno("Código Original"),
                Ano = CriarLinhaConteudoAjustarRetorno("2020")
            };

            dto.Titulo = CriarLinhaConteudoAjustarRetorno("Título Novo");
            dto.Codigo = CriarLinhaConteudoAjustarRetorno("Código Novo");
            dto.Ano = CriarLinhaConteudoAjustarRetorno("2024");

            dto.Titulo.Conteudo.Should().Be("Título Novo");
            dto.Codigo.Conteudo.Should().Be("Código Novo");
            dto.Ano.Conteudo.Should().Be("2024");
        }

        #endregion

        #region Testes de Inicialização com Object Initializer

        [Fact]
        public void DadoObjectInitializer_QuandoUsarMultiplosParametros_EntaoTodosArmazenam()
        {
            var dto = new AcervoArteGraficaLinhaRetornoDTO
            {
                Titulo = CriarLinhaConteudoAjustarRetorno("Teste"),
                Descricao = CriarLinhaConteudoAjustarRetorno("Descrição"),
                Ano = CriarLinhaConteudoAjustarRetorno("2024")
            };

            dto.Should().NotBeNull();
            dto.Titulo.Conteudo.Should().Be("Teste");
            dto.Descricao.Conteudo.Should().Be("Descrição");
            dto.Ano.Conteudo.Should().Be("2024");
        }

        [Fact]
        public void DadoObjectInitializerVazio_QuandoCriar_EntaoInstanciaComValoresPadrao()
        {
            var dto = new AcervoArteGraficaLinhaRetornoDTO { };

            dto.Should().NotBeNull();
            dto.Status.Should().Be(default(ImportacaoStatus));
            dto.NumeroLinha.Should().Be(0);
        }

        [Fact]
        public void DadoObjectInitializerComPropriedadesHerdadas_QuandoCriar_EntaoPropriedadesHerdadasArmazenam()
        {
            var dto = new AcervoArteGraficaLinhaRetornoDTO
            {
                Status = ImportacaoStatus.Sucesso,
                NumeroLinha = 10,
                Mensagem = "Teste de mensagem",
                ErrosCampos = new[] { "Campo1", "Campo2" }
            };

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.NumeroLinha.Should().Be(10);
            dto.Mensagem.Should().Be("Teste de mensagem");
            dto.ErrosCampos.Should().BeEquivalentTo(new[] { "Campo1", "Campo2" });
        }

        #endregion

        #region Métodos Auxiliares

        private LinhaConteudoAjustarRetornoDTO CriarLinhaConteudoAjustarRetorno(string conteudo = "")
        {
            return new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = conteudo
            };
        }

        private void VerificarTodosOsCampos(AcervoArteGraficaLinhaRetornoDTO dto)
        {
            dto.Titulo.Should().NotBeNull();
            dto.Codigo.Should().NotBeNull();
            dto.CreditosAutoresIds.Should().NotBeNull();
            dto.Localizacao.Should().NotBeNull();
            dto.Procedencia.Should().NotBeNull();
            dto.CopiaDigital.Should().NotBeNull();
            dto.PermiteUsoImagem.Should().NotBeNull();
            dto.ConservacaoId.Should().NotBeNull();
            dto.CromiaId.Should().NotBeNull();
            dto.Largura.Should().NotBeNull();
            dto.Altura.Should().NotBeNull();
            dto.Diametro.Should().NotBeNull();
            dto.Tecnica.Should().NotBeNull();
            dto.SuporteId.Should().NotBeNull();
            dto.Quantidade.Should().NotBeNull();
            dto.Descricao.Should().NotBeNull();
            dto.Ano.Should().NotBeNull();

            dto.Titulo.Conteudo.Should().Be("Título");
            dto.Codigo.Conteudo.Should().Be("COD");
            dto.CreditosAutoresIds.Conteudo.Should().Be("1,2,3");
            dto.Localizacao.Conteudo.Should().Be("Local");
            dto.Procedencia.Conteudo.Should().Be("Doação");
            dto.CopiaDigital.Conteudo.Should().Be("Sim");
            dto.PermiteUsoImagem.Conteudo.Should().Be("Não");
            dto.ConservacaoId.Conteudo.Should().Be("1");
            dto.CromiaId.Conteudo.Should().Be("2");
            dto.Largura.Conteudo.Should().Be("50cm");
            dto.Altura.Conteudo.Should().Be("80cm");
            dto.Diametro.Conteudo.Should().Be("30cm");
            dto.Tecnica.Conteudo.Should().Be("Gravura");
            dto.SuporteId.Conteudo.Should().Be("1");
            dto.Quantidade.Conteudo.Should().Be("5");
            dto.Descricao.Conteudo.Should().Be("Descrição");
            dto.Ano.Conteudo.Should().Be("2020");
        }

        #endregion
    }
}
