using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SME.CDEP.Aplicacao;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;

namespace SME.CDEP.TesteUnitario.Aplicacao
{
    public class NotificacaoEmailBaseUseCaseTeste
    {
        private readonly Mock<IRepositorioParametroSistema> _repositorioParametroSistemaMock;
        private readonly Mock<IServicoNotificacaoEmail> _servicoNotificacaoEmailMock;
        private readonly NotificacaoEmailBaseUseCase _sut;

        public NotificacaoEmailBaseUseCaseTeste()
        {
            var mocker = new AutoMocker();
            _repositorioParametroSistemaMock = mocker.GetMock<IRepositorioParametroSistema>();
            _servicoNotificacaoEmailMock = mocker.GetMock<IServicoNotificacaoEmail>();
            _sut = mocker.CreateInstance<NotificacaoEmailBaseUseCase>();
        }

        #region Testes do Construtor

        [Fact]
        public void DadoDependenciasValidas_QuandoInstanciarUseCase_EntaoRetornaInstanciaComSucesso()
        {
            Action acao = () => new NotificacaoEmailBaseUseCase(
                _repositorioParametroSistemaMock.Object,
                _servicoNotificacaoEmailMock.Object);

            acao.Should().NotThrow();
        }

        [Fact]
        public void DadoRepositorioNulo_QuandoInstanciarUseCase_EntaoLancaArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new NotificacaoEmailBaseUseCase(null!, _servicoNotificacaoEmailMock.Object));
        }

        [Fact]
        public void DadoRepositorioNulo_QuandoInstanciarUseCase_EntaoMensagemExcecaoContemNomeDaDependencia()
        {
            var excecao = Assert.Throws<ArgumentNullException>(() =>
                new NotificacaoEmailBaseUseCase(null!, _servicoNotificacaoEmailMock.Object));

            excecao.ParamName.Should().Be("repositorioParametroSistema");
        }

        [Fact]
        public void DadoServicoNotificacaoNulo_QuandoInstanciarUseCase_EntaoLancaArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new NotificacaoEmailBaseUseCase(_repositorioParametroSistemaMock.Object, null!));
        }

        [Fact]
        public void DadoServicoNotificacaoNulo_QuandoInstanciarUseCase_EntaoMensagemExcecaoContemNomeDaDependencia()
        {
            var excecao = Assert.Throws<ArgumentNullException>(() =>
                new NotificacaoEmailBaseUseCase(_repositorioParametroSistemaMock.Object, null!));

            excecao.ParamName.Should().Be("servicoNotificacaoEmail");
        }

        #endregion

        #region Testes do Método CarregarParametros

        [Fact]
        public async Task DadoRepositorioComParametrosValidos_QuandoCarregarParametros_EntaoCarregaTodosParametrosComSucesso()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            ConfigurarRepositorioComParametrosValidos(anoAtual);

            await _sut.CarregarParametros();

            _repositorioParametroSistemaMock.Verify(
                r => r.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita, anoAtual),
                Times.Once);

            _repositorioParametroSistemaMock.Verify(
                r => r.ObterParametroPorTipoEAno(TipoParametroSistema.EnderecoSedeCDEPVisita, anoAtual),
                Times.Once);

            _repositorioParametroSistemaMock.Verify(
                r => r.ObterParametroPorTipoEAno(TipoParametroSistema.HorarioFuncionamentoSedeCDEPVisita, anoAtual),
                Times.Once);
        }

        [Fact]
        public async Task DadoCarregarParametros_QuandoExecutado_EntaoUtilizaHorarioBrasiliaParaObterId()
        {
            var anoEsperado = DateTimeExtension.HorarioBrasilia().Year;
            ConfigurarRepositorioComParametrosValidos(anoEsperado);

            await _sut.CarregarParametros();

            _repositorioParametroSistemaMock.Verify(
                r => r.ObterParametroPorTipoEAno(
                    It.IsAny<TipoParametroSistema>(),
                    anoEsperado),
                Times.Exactly(3));
        }

        [Fact]
        public async Task DadoRepositorioRetornaEnderecoContatoCDEP_QuandoCarregarParametros_EntaoArmazenaValorCorreto()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var enderecoEsperado = "contato@cdep.com";
            
            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = enderecoEsperado });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "Rua Principal, 123" });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.HorarioFuncionamentoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "09:00 - 17:00" });

            await _sut.CarregarParametros();

            // Verifica se o método foi chamado com o tipo correto
            _repositorioParametroSistemaMock.Verify(
                r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita,
                    anoAtual),
                Times.Once);
        }

        [Fact]
        public async Task DadoRepositorioRetornaEnderecoSedeCDEPVisita_QuandoCarregarParametros_EntaoArmazenaValorCorreto()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var enderecoSede = "Av. Paulista, 1000 - São Paulo - SP";
            
            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "contato@cdep.com" });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = enderecoSede });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.HorarioFuncionamentoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "09:00 - 17:00" });

            await _sut.CarregarParametros();

            _repositorioParametroSistemaMock.Verify(
                r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoSedeCDEPVisita,
                    anoAtual),
                Times.Once);
        }

        [Fact]
        public async Task DadoRepositorioRetornaHorarioFuncionamento_QuandoCarregarParametros_EntaoArmazenaValorCorreto()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var horario = "08:00 - 18:00";
            
            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "contato@cdep.com" });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "Rua Principal, 123" });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.HorarioFuncionamentoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = horario });

            await _sut.CarregarParametros();

            _repositorioParametroSistemaMock.Verify(
                r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.HorarioFuncionamentoSedeCDEPVisita,
                    anoAtual),
                Times.Once);
        }

        [Fact]
        public async Task DadoCarregarParametrosComParametrosSemValor_QuandoMontarDadosTemplate_EntaoSubstituiPorVazio()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            
            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(It.IsAny<TipoParametroSistema>(), anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = string.Empty });

            await _sut.CarregarParametros();

            var resultado = await _sut.MontarDadosNoTemplateEmail(
                "João",
                "<table></table>",
                TipoParametroSistema.ModeloEmailAvisoAtrasoDevolucaoEmprestimo);

            resultado.Should().NotBeNull();
        }

        #endregion

        #region Testes do Método MontarDadosNoTemplateEmail

        [Fact]
        public async Task DadoTemplateValido_QuandoMontarDadosNoTemplateEmail_EntaoSubstituiTodosPlaceholders()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var nomeDestinatario = "João Silva";
            var conteudoInterno = "<table><tr><td>Dados</td></tr></table>";
            var enderecoContato = "contato@cdep.com.br";
            var enderecoSede = "Rua Principal, 123";
            var horario = "09:00 - 17:00";
            
            var templateEmail = "Olá #NOME, #CONTEUDO_TABELA Contato: #LINK_FORMULARIO_CDEP Endereço: #ENDERECO_SEDE_CDEP_VISITA Horário: #HORARIO_FUNCIONAMENTO_SEDE_CDEP";

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.ModeloEmailAvisoAtrasoDevolucaoEmprestimo,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = templateEmail });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = enderecoContato });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = enderecoSede });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.HorarioFuncionamentoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = horario });

            await _sut.CarregarParametros();

            var resultado = await _sut.MontarDadosNoTemplateEmail(
                nomeDestinatario,
                conteudoInterno,
                TipoParametroSistema.ModeloEmailAvisoAtrasoDevolucaoEmprestimo);

            resultado.Should().Contain(nomeDestinatario);
            resultado.Should().Contain(conteudoInterno);
            resultado.Should().Contain(enderecoContato);
            resultado.Should().Contain(enderecoSede);
            resultado.Should().Contain(horario);
        }

        [Fact]
        public async Task DadoNomeDestinatarioValido_QuandoMontarDadosNoTemplateEmail_EntaoSubstituiPlaceholderNome()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var templateEmail = "Prezado #NOME, este é seu email.";
            var nomeEsperado = "Maria Santos";

            ConfigurarRepositorioComTemplate(anoAtual, templateEmail);

            await _sut.CarregarParametros();

            var resultado = await _sut.MontarDadosNoTemplateEmail(
                nomeEsperado,
                "<table></table>",
                TipoParametroSistema.ModeloEmailAvisoAtrasoDevolucaoEmprestimo);

            resultado.Should().Contain(nomeEsperado);
            resultado.Should().NotContain("#NOME");
        }

        [Fact]
        public async Task DadoConteudoInternoValido_QuandoMontarDadosNoTemplateEmail_EntaoSubstituiPlaceholderConteudo()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var templateEmail = "Conteúdo: #CONTEUDO_TABELA Fim";
            var conteudo = "<table><tr><td>Item 1</td></tr></table>";

            ConfigurarRepositorioComTemplate(anoAtual, templateEmail);

            await _sut.CarregarParametros();

            var resultado = await _sut.MontarDadosNoTemplateEmail(
                "João",
                conteudo,
                TipoParametroSistema.ModeloEmailAvisoAtrasoDevolucaoEmprestimo);

            resultado.Should().Contain(conteudo);
            resultado.Should().NotContain("#CONTEUDO_TABELA");
        }

        [Fact]
        public async Task DadoPlaceholderEnderecoContato_QuandoMontarDadosNoTemplateEmail_EntaoSubstituiComValorArmazenado()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var templateEmail = "Formulário: #LINK_FORMULARIO_CDEP";
            var enderecoContato = "https://formulario.cdep.com.br";

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.ModeloEmailAvisoAtrasoDevolucaoEmprestimo,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = templateEmail });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = enderecoContato });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "Rua X" });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.HorarioFuncionamentoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "09:00" });

            await _sut.CarregarParametros();

            var resultado = await _sut.MontarDadosNoTemplateEmail(
                "João",
                "<table></table>",
                TipoParametroSistema.ModeloEmailAvisoAtrasoDevolucaoEmprestimo);

            resultado.Should().Contain(enderecoContato);
            resultado.Should().NotContain("#LINK_FORMULARIO_CDEP");
        }

        [Fact]
        public async Task DadoPlaceholderEnderecoSede_QuandoMontarDadosNoTemplateEmail_EntaoSubstituiComValorArmazenado()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var templateEmail = "Sede: #ENDERECO_SEDE_CDEP_VISITA";
            var enderecoSede = "Av. Paulista, 1000 - São Paulo";

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.ModeloEmailAvisoAtrasoDevolucaoEmprestimo,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = templateEmail });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "contato" });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = enderecoSede });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.HorarioFuncionamentoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "09:00" });

            await _sut.CarregarParametros();

            var resultado = await _sut.MontarDadosNoTemplateEmail(
                "João",
                "<table></table>",
                TipoParametroSistema.ModeloEmailAvisoAtrasoDevolucaoEmprestimo);

            resultado.Should().Contain(enderecoSede);
            resultado.Should().NotContain("#ENDERECO_SEDE_CDEP_VISITA");
        }

        [Fact]
        public async Task DadoPlaceholderHorarioFuncionamento_QuandoMontarDadosNoTemplateEmail_EntaoSubstituiComValorArmazenado()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var templateEmail = "Horário: #HORARIO_FUNCIONAMENTO_SEDE_CDEP";
            var horario = "08:00 - 18:00";

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.ModeloEmailAvisoAtrasoDevolucaoEmprestimo,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = templateEmail });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "contato" });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "Rua X" });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.HorarioFuncionamentoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = horario });

            await _sut.CarregarParametros();

            var resultado = await _sut.MontarDadosNoTemplateEmail(
                "João",
                "<table></table>",
                TipoParametroSistema.ModeloEmailAvisoAtrasoDevolucaoEmprestimo);

            resultado.Should().Contain(horario);
            resultado.Should().NotContain("#HORARIO_FUNCIONAMENTO_SEDE_CDEP");
        }

        [Fact]
        public async Task DadoTemplateComMultiplosPlaceholders_QuandoMontarDadosNoTemplateEmail_EntaoSubstituiTodos()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var templateEmail = "#NOME #CONTEUDO_TABELA #LINK_FORMULARIO_CDEP #ENDERECO_SEDE_CDEP_VISITA #HORARIO_FUNCIONAMENTO_SEDE_CDEP";

            ConfigurarRepositorioComTemplate(anoAtual, templateEmail);

            await _sut.CarregarParametros();

            var resultado = await _sut.MontarDadosNoTemplateEmail(
                "João",
                "<table></table>",
                TipoParametroSistema.ModeloEmailAvisoAtrasoDevolucaoEmprestimo);

            resultado.Should().NotContain("#");
        }

        [Fact]
        public async Task DadoNomeVazio_QuandoMontarDadosNoTemplateEmail_EntaoSubstituiPorVazio()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var templateEmail = "Olá #NOME!";

            ConfigurarRepositorioComTemplate(anoAtual, templateEmail);

            await _sut.CarregarParametros();

            var resultado = await _sut.MontarDadosNoTemplateEmail(
                string.Empty,
                "<table></table>",
                TipoParametroSistema.ModeloEmailAvisoAtrasoDevolucaoEmprestimo);

            resultado.Should().Be("Olá !");
        }

        [Fact]
        public async Task DadoConteudoVazio_QuandoMontarDadosNoTemplateEmail_EntaoSubstituiPorVazio()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var templateEmail = "Tabela: #CONTEUDO_TABELA Fim";

            ConfigurarRepositorioComTemplate(anoAtual, templateEmail);

            await _sut.CarregarParametros();

            var resultado = await _sut.MontarDadosNoTemplateEmail(
                "João",
                string.Empty,
                TipoParametroSistema.ModeloEmailAvisoAtrasoDevolucaoEmprestimo);

            resultado.Should().Be("Tabela:  Fim");
        }

        [Fact]
        public async Task DadoTipoParametroModelo_QuandoMontarDadosNoTemplateEmail_EntaoObtemTemplateCorreto()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var tipoParametroEsperado = TipoParametroSistema.ModeloEmailAvisoAtrasoDevolucaoEmprestimo;

            ConfigurarRepositorioComTemplate(anoAtual, "Template teste");

            await _sut.CarregarParametros();

            await _sut.MontarDadosNoTemplateEmail("João", "<table></table>", tipoParametroEsperado);

            _repositorioParametroSistemaMock.Verify(
                r => r.ObterParametroPorTipoEAno(tipoParametroEsperado, anoAtual),
                Times.Once);
        }

        #endregion

        #region Testes do Método EnviarEmail

        [Fact]
        public async Task DadoParametrosValidos_QuandoEnviarEmail_EntaoChamaServicoComSucesso()
        {
            var nomeDestinatario = "João Silva";
            var emailDestinatario = "joao@example.com";
            var assunto = "Notificação CDEP";
            var conteudoEmail = "<html><body>Conteúdo</body></html>";

            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(nomeDestinatario, emailDestinatario, assunto, conteudoEmail))
                .ReturnsAsync(true);

            await _sut.EnviarEmail(nomeDestinatario, emailDestinatario, assunto, conteudoEmail);

            _servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(nomeDestinatario, emailDestinatario, assunto, conteudoEmail),
                Times.Once);
        }

        [Fact]
        public async Task DadoNomeDestinatarioValido_QuandoEnviarEmail_EntaoPassaNomeParaServico()
        {
            var nomeDestinatario = "Maria Santos";
            var emailDestinatario = "maria@example.com";
            var assunto = "Aviso";
            var conteudoEmail = "Conteúdo";

            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            await _sut.EnviarEmail(nomeDestinatario, emailDestinatario, assunto, conteudoEmail);

            _servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(nomeDestinatario, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task DadoEmailDestinatarioValido_QuandoEnviarEmail_EntaoPassaEmailParaServico()
        {
            var nomeDestinatario = "João";
            var emailDestinatario = "joao.silva@domain.com";
            var assunto = "Assunto";
            var conteudoEmail = "Conteúdo";

            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            await _sut.EnviarEmail(nomeDestinatario, emailDestinatario, assunto, conteudoEmail);

            _servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(It.IsAny<string>(), emailDestinatario, It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task DadoAssuntoValido_QuandoEnviarEmail_EntaoPassaAssuntoParaServico()
        {
            var nomeDestinatario = "João";
            var emailDestinatario = "joao@example.com";
            var assuntoEsperado = "CDEP - Aviso de Devolução";
            var conteudoEmail = "Conteúdo";

            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            await _sut.EnviarEmail(nomeDestinatario, emailDestinatario, assuntoEsperado, conteudoEmail);

            _servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), assuntoEsperado, It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task DadoConteudoEmailValido_QuandoEnviarEmail_EntaoPassaConteudoParaServico()
        {
            var nomeDestinatario = "João";
            var emailDestinatario = "joao@example.com";
            var assunto = "Assunto";
            var conteudoEmailEsperado = "<html><body><h1>Título</h1><p>Parágrafo</p></body></html>";

            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            await _sut.EnviarEmail(nomeDestinatario, emailDestinatario, assunto, conteudoEmailEsperado);

            _servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), conteudoEmailEsperado),
                Times.Once);
        }

        [Fact]
        public async Task DadoNomeVazio_QuandoEnviarEmail_EntaoEnviaComNomeVazio()
        {
            var nomeDestinatario = string.Empty;
            var emailDestinatario = "joao@example.com";
            var assunto = "Assunto";
            var conteudoEmail = "Conteúdo";

            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            await _sut.EnviarEmail(nomeDestinatario, emailDestinatario, assunto, conteudoEmail);

            _servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(string.Empty, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task DadoEmailVazio_QuandoEnviarEmail_EntaoEnviaComEmailVazio()
        {
            var nomeDestinatario = "João";
            var emailDestinatario = string.Empty;
            var assunto = "Assunto";
            var conteudoEmail = "Conteúdo";

            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            await _sut.EnviarEmail(nomeDestinatario, emailDestinatario, assunto, conteudoEmail);

            _servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(It.IsAny<string>(), string.Empty, It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task DadoAssuntoVazio_QuandoEnviarEmail_EntaoEnviaComAssuntoVazio()
        {
            var nomeDestinatario = "João";
            var emailDestinatario = "joao@example.com";
            var assunto = string.Empty;
            var conteudoEmail = "Conteúdo";

            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            await _sut.EnviarEmail(nomeDestinatario, emailDestinatario, assunto, conteudoEmail);

            _servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), string.Empty, It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task DadoConteudoVazio_QuandoEnviarEmail_EntaoEnviaComConteudoVazio()
        {
            var nomeDestinatario = "João";
            var emailDestinatario = "joao@example.com";
            var assunto = "Assunto";
            var conteudoEmail = string.Empty;

            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            await _sut.EnviarEmail(nomeDestinatario, emailDestinatario, assunto, conteudoEmail);

            _servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task DadoServicoEnviaComSucesso_QuandoEnviarEmail_EntaoNaoLancaExcecao()
        {
            var nomeDestinatario = "João";
            var emailDestinatario = "joao@example.com";
            var assunto = "Assunto";
            var conteudoEmail = "Conteúdo";

            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            Func<Task> acao = async () => await _sut.EnviarEmail(nomeDestinatario, emailDestinatario, assunto, conteudoEmail);

            await acao.Should().NotThrowAsync();
        }

        [Fact]
        public async Task DadoServicoLancaExcecao_QuandoEnviarEmail_EntaoExcecaoEhPropagada()
        {
            var nomeDestinatario = "João";
            var emailDestinatario = "joao@example.com";
            var assunto = "Assunto";
            var conteudoEmail = "Conteúdo";
            var mensagemErro = "Erro ao enviar email";

            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception(mensagemErro));

            Func<Task> acao = async () => await _sut.EnviarEmail(nomeDestinatario, emailDestinatario, assunto, conteudoEmail);

            await acao.Should().ThrowAsync<Exception>().WithMessage(mensagemErro);
        }

        #endregion

        #region Testes de Fluxo Completo

        [Fact]
        public async Task DadoFluxoCompletoDomCarregamentoPriorAoEnvio_QuandoExecutar_EntaoTodosOsParametrosSaoUsados()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var nomeDestinatario = "João Silva";
            var emailDestinatario = "joao@example.com";
            var assunto = "CDEP - Notificação";
            var templateEmail = "Olá #NOME, #CONTEUDO_TABELA. Contato: #LINK_FORMULARIO_CDEP. Endereço: #ENDERECO_SEDE_CDEP_VISITA. Horário: #HORARIO_FUNCIONAMENTO_SEDE_CDEP";

            ConfigurarRepositorioComTemplate(anoAtual, templateEmail);

            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            await _sut.CarregarParametros();
            var conteudoMontado = await _sut.MontarDadosNoTemplateEmail(nomeDestinatario, "<table></table>", TipoParametroSistema.ModeloEmailAvisoAtrasoDevolucaoEmprestimo);
            await _sut.EnviarEmail(nomeDestinatario, emailDestinatario, assunto, conteudoMontado);

            _servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(nomeDestinatario, emailDestinatario, assunto, It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task DadoMultiplosEmails_QuandoEnviarVariosEmails_EntaoTodosEnviados()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            ConfigurarRepositorioComTemplate(anoAtual, "Template: #NOME");

            _servicoNotificacaoEmailMock
                .Setup(s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            await _sut.CarregarParametros();

            await _sut.EnviarEmail("João", "joao@example.com", "Assunto 1", "Conteúdo 1");
            await _sut.EnviarEmail("Maria", "maria@example.com", "Assunto 2", "Conteúdo 2");
            await _sut.EnviarEmail("Pedro", "pedro@example.com", "Assunto 3", "Conteúdo 3");

            _servicoNotificacaoEmailMock.Verify(
                s => s.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Exactly(3));
        }

        #endregion

        #region Testes de Métodos Auxiliares

        private void ConfigurarRepositorioComParametrosValidos(int anoAtual)
        {
            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "contato@cdep.com" });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "Rua Principal, 123" });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.HorarioFuncionamentoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "09:00 - 17:00" });
        }

        private void ConfigurarRepositorioComTemplate(int anoAtual, string templateEmail)
        {
            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.ModeloEmailAvisoAtrasoDevolucaoEmprestimo,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = templateEmail });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoContatoCDEPConfirmacaoCancelamentoVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "contato@cdep.com" });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.EnderecoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "Rua Principal, 123" });

            _repositorioParametroSistemaMock
                .Setup(r => r.ObterParametroPorTipoEAno(
                    TipoParametroSistema.HorarioFuncionamentoSedeCDEPVisita,
                    anoAtual))
                .ReturnsAsync(new ParametroSistema { Valor = "09:00 - 17:00" });
        }

        #endregion
    }
}
