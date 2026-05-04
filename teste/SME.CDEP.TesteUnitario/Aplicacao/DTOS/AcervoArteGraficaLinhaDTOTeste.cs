using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;
using Xunit;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoArteGraficaLinhaDTOTeste
    {
        #region Testes de Instanciação

        [Fact]
        public void DadoConstrutorPadrao_QuandoChamar_EntaoInstanciaComSucesso()
        {
            var dto = new AcervoArteGraficaLinhaDTO();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<AcervoArteGraficaLinhaDTO>();
        }

        [Fact]
        public void DadoConstrutorPadrao_QuandoChamar_EntaoHerdaDeAcervoLinhaDTO()
        {
            var dto = new AcervoArteGraficaLinhaDTO();

            dto.Should().BeAssignableTo<AcervoLinhaDTO>();
        }

        #endregion

        #region Testes de Propriedades - Valores Padrão

        [Fact]
        public void DadoPropriedades_QuandoInstanciar_EntaoValoresPadroSaoNulos()
        {
            var dto = new AcervoArteGraficaLinhaDTO();

            dto.Titulo.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.Credito.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.PermiteUsoImagem.Should().BeNull();
            dto.EstadoConservacao.Should().BeNull();
            dto.Cromia.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.Diametro.Should().BeNull();
            dto.Tecnica.Should().BeNull();
            dto.Suporte.Should().BeNull();
            dto.Quantidade.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoPropriedadesHerdadas_QuandoInstanciar_EntaoValoresPadroEstaoCorretos()
        {
            var dto = new AcervoArteGraficaLinhaDTO();

            dto.Status.Should().Be(default(ImportacaoStatus));
            dto.Mensagem.Should().BeNull();
            dto.NumeroLinha.Should().Be(0);
            dto.PossuiErros.Should().BeFalse();
        }

        #endregion

        #region Testes de Atribuição de Propriedades

        [Fact]
        public void DadoTituloAtribuido_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var titulo = CriarLinhaConteudoAjustar("Título Teste");
            var dto = new AcervoArteGraficaLinhaDTO { Titulo = titulo };

            dto.Titulo.Should().Be(titulo);
            dto.Titulo.Conteudo.Should().Be("Título Teste");
        }

        [Fact]
        public void DadoCodigoAtribuido_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var codigo = CriarLinhaConteudoAjustar("COD-001");
            var dto = new AcervoArteGraficaLinhaDTO { Codigo = codigo };

            dto.Codigo.Should().Be(codigo);
            dto.Codigo.Conteudo.Should().Be("COD-001");
        }

        [Fact]
        public void DadoCreditoAtribuido_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var credito = CriarLinhaConteudoAjustar("Crédito Teste");
            var dto = new AcervoArteGraficaLinhaDTO { Credito = credito };

            dto.Credito.Should().Be(credito);
            dto.Credito.Conteudo.Should().Be("Crédito Teste");
        }

        [Fact]
        public void DadoLocalizacaoAtribuida_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var localizacao = CriarLinhaConteudoAjustar("Sala 1, Prateleira 2");
            var dto = new AcervoArteGraficaLinhaDTO { Localizacao = localizacao };

            dto.Localizacao.Should().Be(localizacao);
            dto.Localizacao.Conteudo.Should().Be("Sala 1, Prateleira 2");
        }

        [Fact]
        public void DadoProcedenciaAtribuida_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var procedencia = CriarLinhaConteudoAjustar("Doação");
            var dto = new AcervoArteGraficaLinhaDTO { Procedencia = procedencia };

            dto.Procedencia.Should().Be(procedencia);
            dto.Procedencia.Conteudo.Should().Be("Doação");
        }

        [Fact]
        public void DadoCopiaDigitalAtribuida_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var copiaDigital = CriarLinhaConteudoAjustar("Sim");
            var dto = new AcervoArteGraficaLinhaDTO { CopiaDigital = copiaDigital };

            dto.CopiaDigital.Should().Be(copiaDigital);
            dto.CopiaDigital.Conteudo.Should().Be("Sim");
        }

        [Fact]
        public void DadoPermiteUsoImagemAtribuido_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var permiteUsoImagem = CriarLinhaConteudoAjustar("Não");
            var dto = new AcervoArteGraficaLinhaDTO { PermiteUsoImagem = permiteUsoImagem };

            dto.PermiteUsoImagem.Should().Be(permiteUsoImagem);
            dto.PermiteUsoImagem.Conteudo.Should().Be("Não");
        }

        [Fact]
        public void DadoEstadoConservacaoAtribuido_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var estadoConservacao = CriarLinhaConteudoAjustar("Bom");
            var dto = new AcervoArteGraficaLinhaDTO { EstadoConservacao = estadoConservacao };

            dto.EstadoConservacao.Should().Be(estadoConservacao);
            dto.EstadoConservacao.Conteudo.Should().Be("Bom");
        }

        [Fact]
        public void DadoCromiaAtribuida_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var cromia = CriarLinhaConteudoAjustar("Colorido");
            var dto = new AcervoArteGraficaLinhaDTO { Cromia = cromia };

            dto.Cromia.Should().Be(cromia);
            dto.Cromia.Conteudo.Should().Be("Colorido");
        }

        [Fact]
        public void DadoLarguraAtribuida_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var largura = CriarLinhaConteudoAjustar("50cm");
            var dto = new AcervoArteGraficaLinhaDTO { Largura = largura };

            dto.Largura.Should().Be(largura);
            dto.Largura.Conteudo.Should().Be("50cm");
        }

        [Fact]
        public void DadoAlturaAtribuida_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var altura = CriarLinhaConteudoAjustar("80cm");
            var dto = new AcervoArteGraficaLinhaDTO { Altura = altura };

            dto.Altura.Should().Be(altura);
            dto.Altura.Conteudo.Should().Be("80cm");
        }

        [Fact]
        public void DadoDiametroAtribuido_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var diametro = CriarLinhaConteudoAjustar("30cm");
            var dto = new AcervoArteGraficaLinhaDTO { Diametro = diametro };

            dto.Diametro.Should().Be(diametro);
            dto.Diametro.Conteudo.Should().Be("30cm");
        }

        [Fact]
        public void DadoTecnicaAtribuida_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var tecnica = CriarLinhaConteudoAjustar("Aquarela");
            var dto = new AcervoArteGraficaLinhaDTO { Tecnica = tecnica };

            dto.Tecnica.Should().Be(tecnica);
            dto.Tecnica.Conteudo.Should().Be("Aquarela");
        }

        [Fact]
        public void DadoSuporteAtribuido_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var suporte = CriarLinhaConteudoAjustar("Papel");
            var dto = new AcervoArteGraficaLinhaDTO { Suporte = suporte };

            dto.Suporte.Should().Be(suporte);
            dto.Suporte.Conteudo.Should().Be("Papel");
        }

        [Fact]
        public void DadoQuantidadeAtribuida_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var quantidade = CriarLinhaConteudoAjustar("5");
            var dto = new AcervoArteGraficaLinhaDTO { Quantidade = quantidade };

            dto.Quantidade.Should().Be(quantidade);
            dto.Quantidade.Conteudo.Should().Be("5");
        }

        [Fact]
        public void DadoDescricaoAtribuida_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var descricao = CriarLinhaConteudoAjustar("Descrição da obra");
            var dto = new AcervoArteGraficaLinhaDTO { Descricao = descricao };

            dto.Descricao.Should().Be(descricao);
            dto.Descricao.Conteudo.Should().Be("Descrição da obra");
        }

        [Fact]
        public void DadoAnoAtribuido_QuandoInstanciar_EntaoEhAtribuidoCorreto()
        {
            var ano = CriarLinhaConteudoAjustar("2024");
            var dto = new AcervoArteGraficaLinhaDTO { Ano = ano };

            dto.Ano.Should().Be(ano);
            dto.Ano.Conteudo.Should().Be("2024");
        }

        #endregion

        #region Testes de Múltiplas Propriedades

        [Fact]
        public void DadoMultiplasPropriedadesAtribuidas_QuandoInstanciar_EntaoTodasSaoAtribuidasCorreto()
        {
            var titulo = CriarLinhaConteudoAjustar("Obra Completa");
            var codigo = CriarLinhaConteudoAjustar("COD-123");
            var credito = CriarLinhaConteudoAjustar("Artista Famoso");
            var localizacao = CriarLinhaConteudoAjustar("Galeria Principal");
            var procedencia = CriarLinhaConteudoAjustar("Compra");
            var ano = CriarLinhaConteudoAjustar("2020");

            var dto = new AcervoArteGraficaLinhaDTO
            {
                Titulo = titulo,
                Codigo = codigo,
                Credito = credito,
                Localizacao = localizacao,
                Procedencia = procedencia,
                Ano = ano,
                NumeroLinha = 5,
                PossuiErros = false,
                Status = ImportacaoStatus.Pendente,
                Mensagem = "Dados iniciais"
            };

            dto.Titulo.Should().Be(titulo);
            dto.Codigo.Should().Be(codigo);
            dto.Credito.Should().Be(credito);
            dto.Localizacao.Should().Be(localizacao);
            dto.Procedencia.Should().Be(procedencia);
            dto.Ano.Should().Be(ano);
            dto.NumeroLinha.Should().Be(5);
            dto.PossuiErros.Should().BeFalse();
            dto.Status.Should().Be(ImportacaoStatus.Pendente);
            dto.Mensagem.Should().Be("Dados iniciais");
        }

        #endregion

        #region Testes do Método DefinirLinhaComoSucesso

        [Fact]
        public void DadoLinhaComErros_QuandoChamarDefinirLinhaComoSucesso_EntaoPossuiErrosEhFalso()
        {
            var dto = new AcervoArteGraficaLinhaDTO
            {
                PossuiErros = true,
                Mensagem = "Erro anterior",
                Status = ImportacaoStatus.Erros,
                Titulo = CriarLinhaConteudoAjustar(),
                Codigo = CriarLinhaConteudoAjustar(),
                Credito = CriarLinhaConteudoAjustar(),
                Localizacao = CriarLinhaConteudoAjustar(),
                Procedencia = CriarLinhaConteudoAjustar(),
                CopiaDigital = CriarLinhaConteudoAjustar(),
                PermiteUsoImagem = CriarLinhaConteudoAjustar(),
                EstadoConservacao = CriarLinhaConteudoAjustar(),
                Cromia = CriarLinhaConteudoAjustar(),
                Largura = CriarLinhaConteudoAjustar(),
                Altura = CriarLinhaConteudoAjustar(),
                Diametro = CriarLinhaConteudoAjustar(),
                Tecnica = CriarLinhaConteudoAjustar(),
                Suporte = CriarLinhaConteudoAjustar(),
                Quantidade = CriarLinhaConteudoAjustar(),
                Descricao = CriarLinhaConteudoAjustar(),
                Ano = CriarLinhaConteudoAjustar()
            };

            dto.DefinirLinhaComoSucesso();

            dto.PossuiErros.Should().BeFalse();
        }

        [Fact]
        public void DadoLinhaComErros_QuandoChamarDefinirLinhaComoSucesso_EntaoMensagemEhVazia()
        {
            var dto = new AcervoArteGraficaLinhaDTO
            {
                PossuiErros = true,
                Mensagem = "Erro anterior",
                Status = ImportacaoStatus.Erros,
                Titulo = CriarLinhaConteudoAjustar(),
                Codigo = CriarLinhaConteudoAjustar(),
                Credito = CriarLinhaConteudoAjustar(),
                Localizacao = CriarLinhaConteudoAjustar(),
                Procedencia = CriarLinhaConteudoAjustar(),
                CopiaDigital = CriarLinhaConteudoAjustar(),
                PermiteUsoImagem = CriarLinhaConteudoAjustar(),
                EstadoConservacao = CriarLinhaConteudoAjustar(),
                Cromia = CriarLinhaConteudoAjustar(),
                Largura = CriarLinhaConteudoAjustar(),
                Altura = CriarLinhaConteudoAjustar(),
                Diametro = CriarLinhaConteudoAjustar(),
                Tecnica = CriarLinhaConteudoAjustar(),
                Suporte = CriarLinhaConteudoAjustar(),
                Quantidade = CriarLinhaConteudoAjustar(),
                Descricao = CriarLinhaConteudoAjustar(),
                Ano = CriarLinhaConteudoAjustar()
            };

            dto.DefinirLinhaComoSucesso();

            dto.Mensagem.Should().BeEmpty();
        }

        [Fact]
        public void DadoLinhaComErros_QuandoChamarDefinirLinhaComoSucesso_EntaoStatusEhSucesso()
        {
            var dto = new AcervoArteGraficaLinhaDTO
            {
                PossuiErros = true,
                Status = ImportacaoStatus.Erros,
                Titulo = CriarLinhaConteudoAjustar(),
                Codigo = CriarLinhaConteudoAjustar(),
                Credito = CriarLinhaConteudoAjustar(),
                Localizacao = CriarLinhaConteudoAjustar(),
                Procedencia = CriarLinhaConteudoAjustar(),
                CopiaDigital = CriarLinhaConteudoAjustar(),
                PermiteUsoImagem = CriarLinhaConteudoAjustar(),
                EstadoConservacao = CriarLinhaConteudoAjustar(),
                Cromia = CriarLinhaConteudoAjustar(),
                Largura = CriarLinhaConteudoAjustar(),
                Altura = CriarLinhaConteudoAjustar(),
                Diametro = CriarLinhaConteudoAjustar(),
                Tecnica = CriarLinhaConteudoAjustar(),
                Suporte = CriarLinhaConteudoAjustar(),
                Quantidade = CriarLinhaConteudoAjustar(),
                Descricao = CriarLinhaConteudoAjustar(),
                Ano = CriarLinhaConteudoAjustar()
            };

            dto.DefinirLinhaComoSucesso();

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact]
        public void DadoLinhaSemTodasPropriedadesPreenchidas_QuandoChamarDefinirLinhaComoSucesso_EntaoTodasPropriedadesLinhaConteudoAreDefinidas()
        {
            var titulo = CriarLinhaConteudoAjustar("Título", possuiErro: true);
            var codigo = CriarLinhaConteudoAjustar("Código", possuiErro: true);
            var credito = CriarLinhaConteudoAjustar("Crédito", possuiErro: true);
            var localizacao = CriarLinhaConteudoAjustar("Localização", possuiErro: true);
            var procedencia = CriarLinhaConteudoAjustar("Procedência", possuiErro: true);
            var copiaDigital = CriarLinhaConteudoAjustar("Cópia Digital", possuiErro: true);
            var permiteUsoImagem = CriarLinhaConteudoAjustar("Permite Uso", possuiErro: true);
            var estadoConservacao = CriarLinhaConteudoAjustar("Estado", possuiErro: true);
            var cromia = CriarLinhaConteudoAjustar("Cromia", possuiErro: true);
            var largura = CriarLinhaConteudoAjustar("Largura", possuiErro: true);
            var altura = CriarLinhaConteudoAjustar("Altura", possuiErro: true);
            var diametro = CriarLinhaConteudoAjustar("Diâmetro", possuiErro: true);
            var tecnica = CriarLinhaConteudoAjustar("Técnica", possuiErro: true);
            var suporte = CriarLinhaConteudoAjustar("Suporte", possuiErro: true);
            var quantidade = CriarLinhaConteudoAjustar("Quantidade", possuiErro: true);
            var descricao = CriarLinhaConteudoAjustar("Descrição", possuiErro: true);

            var dto = new AcervoArteGraficaLinhaDTO
            {
                PossuiErros = true,
                Titulo = titulo,
                Codigo = codigo,
                Credito = credito,
                Localizacao = localizacao,
                Procedencia = procedencia,
                CopiaDigital = copiaDigital,
                PermiteUsoImagem = permiteUsoImagem,
                EstadoConservacao = estadoConservacao,
                Cromia = cromia,
                Largura = largura,
                Altura = altura,
                Diametro = diametro,
                Tecnica = tecnica,
                Suporte = suporte,
                Quantidade = quantidade,
                Descricao = descricao,
                Ano = CriarLinhaConteudoAjustar("Ano", possuiErro: true)
            };

            dto.DefinirLinhaComoSucesso();

            VerificarTodasPropriedadesComoSucesso(dto);
        }

        [Fact]
        public void DadoLinhaSemPropriedadesPreenchidas_QuandoChamarDefinirLinhaComoSucesso_EntaoLancaNullReferenceException()
        {
            var dto = new AcervoArteGraficaLinhaDTO();

            var acao = () => dto.DefinirLinhaComoSucesso();

            acao.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucessoComAlgumasPropriedadesNulas_QuandoChamar_EntaoLancaNullReferenceException()
        {
            var titulo = CriarLinhaConteudoAjustar("Título");
            var dto = new AcervoArteGraficaLinhaDTO
            {
                Titulo = titulo,
                Codigo = null
            };

            var acao = () => dto.DefinirLinhaComoSucesso();

            acao.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void DadoDefinirLinhaComoSucessoChamadoDuasVezes_QuandoChamar_EntaoAmbasChamadasExecutamComSucesso()
        {
            CriarDto(out var dto);

            dto.DefinirLinhaComoSucesso();
            var primeiraExecucao = dto.PossuiErros;

            dto.PossuiErros = true;
            dto.Mensagem = "Novo erro";
            dto.Titulo.PossuiErro = true;

            dto.DefinirLinhaComoSucesso();

            primeiraExecucao.Should().BeFalse();
            dto.PossuiErros.Should().BeFalse();
            dto.Mensagem.Should().BeEmpty();
            dto.Titulo.PossuiErro.Should().BeFalse();
        }

        #endregion

        #region Testes de Herança

        [Fact]
        public void DadoAcervoArteGraficaLinhaDTO_QuandoHerdarDeAcervoLinhaDTO_EntaoTemPropriedadesHerdadas()
        {
            var dto = new AcervoArteGraficaLinhaDTO();

            typeof(AcervoArteGraficaLinhaDTO).BaseType.Should().Be(typeof(AcervoLinhaDTO));
        }

        [Fact]
        public void DadoAcervoArteGraficaLinhaDTO_QuandoVerificarPropriedadesHerdadas_EntaoTemStatus()
        {
            var propriedade = typeof(AcervoArteGraficaLinhaDTO).GetProperty(nameof(AcervoLinhaDTO.Status));

            propriedade.Should().NotBeNull();
        }

        [Fact]
        public void DadoAcervoArteGraficaLinhaDTO_QuandoVerificarPropriedadesHerdadas_EntaoTemMensagem()
        {
            var propriedade = typeof(AcervoArteGraficaLinhaDTO).GetProperty(nameof(AcervoLinhaDTO.Mensagem));

            propriedade.Should().NotBeNull();
        }

        [Fact]
        public void DadoAcervoArteGraficaLinhaDTO_QuandoVerificarPropriedadesHerdadas_EntaoTemNumeroLinha()
        {
            var propriedade = typeof(AcervoArteGraficaLinhaDTO).GetProperty(nameof(AcervoLinhaDTO.NumeroLinha));

            propriedade.Should().NotBeNull();
        }

        [Fact]
        public void DadoAcervoArteGraficaLinhaDTO_QuandoVerificarPropriedadesHerdadas_EntaoTemPossuiErros()
        {
            var propriedade = typeof(AcervoArteGraficaLinhaDTO).GetProperty(nameof(AcervoLinhaDTO.PossuiErros));

            propriedade.Should().NotBeNull();
        }

        #endregion

        #region Métodos Auxiliares

        private LinhaConteudoAjustarDTO CriarLinhaConteudoAjustar(
            string conteudo = "",
            bool possuiErro = false,
            string mensagem = "")
        {
            return new LinhaConteudoAjustarDTO
            {
                Conteudo = conteudo,
                PossuiErro = possuiErro,
                Mensagem = mensagem
            };
        }

        private void CriarDto(out AcervoArteGraficaLinhaDTO dto)
        {
            dto = new AcervoArteGraficaLinhaDTO
            {
                PossuiErros = true,
                Titulo = CriarLinhaConteudoAjustar("Título", possuiErro: true),
                Codigo = CriarLinhaConteudoAjustar("Código", possuiErro: true),
                Credito = CriarLinhaConteudoAjustar("Crédito", possuiErro: true),
                Localizacao = CriarLinhaConteudoAjustar("Localização", possuiErro: true),
                Procedencia = CriarLinhaConteudoAjustar("Procedência", possuiErro: true),
                CopiaDigital = CriarLinhaConteudoAjustar("Cópia Digital", possuiErro: true),
                PermiteUsoImagem = CriarLinhaConteudoAjustar("Permite Uso", possuiErro: true),
                EstadoConservacao = CriarLinhaConteudoAjustar("Estado", possuiErro: true),
                Cromia = CriarLinhaConteudoAjustar("Cromia", possuiErro: true),
                Largura = CriarLinhaConteudoAjustar("Largura", possuiErro: true),
                Altura = CriarLinhaConteudoAjustar("Altura", possuiErro: true),
                Diametro = CriarLinhaConteudoAjustar("Diâmetro", possuiErro: true),
                Tecnica = CriarLinhaConteudoAjustar("Técnica", possuiErro: true),
                Suporte = CriarLinhaConteudoAjustar("Suporte", possuiErro: true),
                Quantidade = CriarLinhaConteudoAjustar("Quantidade", possuiErro: true),
                Descricao = CriarLinhaConteudoAjustar("Descrição", possuiErro: true),
                Ano = CriarLinhaConteudoAjustar("Ano", possuiErro: true)
            };
        }

        private void VerificarTodasPropriedadesComoSucesso(AcervoArteGraficaLinhaDTO dto)
        {
            dto.Titulo.PossuiErro.Should().BeFalse();
            dto.Codigo.PossuiErro.Should().BeFalse();
            dto.Credito.PossuiErro.Should().BeFalse();
            dto.Localizacao.PossuiErro.Should().BeFalse();
            dto.Procedencia.PossuiErro.Should().BeFalse();
            dto.CopiaDigital.PossuiErro.Should().BeFalse();
            dto.PermiteUsoImagem.PossuiErro.Should().BeFalse();
            dto.EstadoConservacao.PossuiErro.Should().BeFalse();
            dto.Cromia.PossuiErro.Should().BeFalse();
            dto.Largura.PossuiErro.Should().BeFalse();
            dto.Altura.PossuiErro.Should().BeFalse();
            dto.Diametro.PossuiErro.Should().BeFalse();
            dto.Tecnica.PossuiErro.Should().BeFalse();
            dto.Suporte.PossuiErro.Should().BeFalse();
            dto.Quantidade.PossuiErro.Should().BeFalse();
            dto.Descricao.PossuiErro.Should().BeFalse();

            dto.Titulo.Mensagem.Should().BeEmpty();
            dto.Codigo.Mensagem.Should().BeEmpty();
            dto.Credito.Mensagem.Should().BeEmpty();
            dto.Localizacao.Mensagem.Should().BeEmpty();
            dto.Procedencia.Mensagem.Should().BeEmpty();
            dto.CopiaDigital.Mensagem.Should().BeEmpty();
            dto.PermiteUsoImagem.Mensagem.Should().BeEmpty();
            dto.EstadoConservacao.Mensagem.Should().BeEmpty();
            dto.Cromia.Mensagem.Should().BeEmpty();
            dto.Largura.Mensagem.Should().BeEmpty();
            dto.Altura.Mensagem.Should().BeEmpty();
            dto.Diametro.Mensagem.Should().BeEmpty();
            dto.Tecnica.Mensagem.Should().BeEmpty();
            dto.Suporte.Mensagem.Should().BeEmpty();
            dto.Quantidade.Mensagem.Should().BeEmpty();
            dto.Descricao.Mensagem.Should().BeEmpty();
        }

        #endregion
    }
}
