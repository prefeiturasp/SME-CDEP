using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoSolicitacaoItemDetalheResumidoDtoTeste
    {
        #region Testes de Propriedade Id

        [Fact]
        public void DadoIdValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var id = 42L;
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Id = id;

            dto.Id.Should().Be(id);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoIdAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Id.Should().Be(0);
        }

        [Theory]
        [InlineData(1L)]
        [InlineData(100L)]
        [InlineData(999999L)]
        [InlineData(long.MaxValue)]
        public void DadoDiferentesIds_QuandoAssignar_EntaoRetornaValoresCorretos(long id)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Id = id;

            dto.Id.Should().Be(id);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoAtribuirValoresMultiplosAoId_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Id = 10;
            dto.Id = 20;
            dto.Id = 30;

            dto.Id.Should().Be(30);
        }

        #endregion

        #region Testes de Propriedade Codigo

        [Fact]
        public void DadoCodigoValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var codigo = "ACE-001";
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Codigo = codigo;

            dto.Codigo.Should().Be(codigo);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoCodigoAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Codigo.Should().BeNull();
        }

        [Fact]
        public void DadoCodigoNulo_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO { Codigo = "ACE-001" };

            dto.Codigo = null!;

            dto.Codigo.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("ACE-001")]
        [InlineData("LIV-002")]
        [InlineData("CODIGO-123456")]
        public void DadoDiferentesCodigos_QuandoAssignar_EntaoRetornaValoresCorretos(string codigo)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Codigo = codigo;

            dto.Codigo.Should().Be(codigo);
        }

        #endregion

        #region Testes de Propriedade TipoAcervo

        [Fact]
        public void DadoTipoAcervoValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var tipoAcervo = "Livro";
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.TipoAcervo = tipoAcervo;

            dto.TipoAcervo.Should().Be(tipoAcervo);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoTipoAcervoAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.TipoAcervo.Should().BeNull();
        }

        [Fact]
        public void DadoTipoAcervoNulo_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO { TipoAcervo = "Livro" };

            dto.TipoAcervo = null!;

            dto.TipoAcervo.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Livro")]
        [InlineData("Periódico")]
        [InlineData("Obra Rara")]
        [InlineData("Manuscrito")]
        public void DadoDiferentesTiposAcervo_QuandoAssignar_EntaoRetornaValoresCorretos(string tipoAcervo)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.TipoAcervo = tipoAcervo;

            dto.TipoAcervo.Should().Be(tipoAcervo);
        }

        #endregion

        #region Testes de Propriedade Titulo

        [Fact]
        public void DadoTituloValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var titulo = "O Cortiço";
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Titulo = titulo;

            dto.Titulo.Should().Be(titulo);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoTituloAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Titulo.Should().BeNull();
        }

        [Fact]
        public void DadoTituloNulo_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO { Titulo = "O Cortiço" };

            dto.Titulo = null!;

            dto.Titulo.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("O Cortiço")]
        [InlineData("Dom Casmurro")]
        [InlineData("Memórias Póstumas de Brás Cubas")]
        public void DadoDiferentesTitulos_QuandoAssignar_EntaoRetornaValoresCorretos(string titulo)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Titulo = titulo;

            dto.Titulo.Should().Be(titulo);
        }

        #endregion

        #region Testes de Propriedade Situacao

        [Fact]
        public void DadoSituacaoValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var situacao = "Aguardando";
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Situacao = situacao;

            dto.Situacao.Should().Be(situacao);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoSituacaoAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Situacao.Should().BeNull();
        }

        [Fact]
        public void DadoSituacaoNula_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO { Situacao = "Aguardando" };

            dto.Situacao = null!;

            dto.Situacao.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Aguardando Atendimento")]
        [InlineData("Aguardando Visita")]
        [InlineData("Finalizado")]
        public void DadoDiferentesSituacoes_QuandoAssignar_EntaoRetornaValoresCorretos(string situacao)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Situacao = situacao;

            dto.Situacao.Should().Be(situacao);
        }

        #endregion

        #region Testes de Propriedade SituacaoId

        [Fact]
        public void DadoSituacaoIdValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var situacaoId = SituacaoSolicitacaoItem.AGUARDANDO_VISITA;
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.SituacaoId = situacaoId;

            dto.SituacaoId.Should().Be(situacaoId);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoSituacaoIdAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.SituacaoId.Should().Be(default(SituacaoSolicitacaoItem));
        }

        [Theory]
        [InlineData(SituacaoSolicitacaoItem.AGUARDANDO_VISITA)]
        [InlineData(SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE)]
        [InlineData(SituacaoSolicitacaoItem.CANCELADO)]
        public void DadoDiferentesSituacaoIds_QuandoAssignar_EntaoRetornaValoresCorretos(SituacaoSolicitacaoItem situacaoId)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.SituacaoId = situacaoId;

            dto.SituacaoId.Should().Be(situacaoId);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoAtribuirValoresMultiplosAoSituacaoId_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.SituacaoId = SituacaoSolicitacaoItem.AGUARDANDO_VISITA;
            dto.SituacaoId = SituacaoSolicitacaoItem.FINALIZADO_MANUALMENTE;
            dto.SituacaoId = SituacaoSolicitacaoItem.CANCELADO;

            dto.SituacaoId.Should().Be(SituacaoSolicitacaoItem.CANCELADO);
        }

        #endregion

        #region Testes de Propriedade DataVisita

        [Fact]
        public void DadoDataVisitaValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var dataVisita = new DateTime(2024, 5, 15, 10, 30, 0);
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataVisita = dataVisita;

            dto.DataVisita.Should().Be(dataVisita);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoDataVisitaAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataVisita.Should().BeNull();
        }

        [Fact]
        public void DadoDataVisitaNula_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO { DataVisita = new DateTime(2024, 5, 15) };

            dto.DataVisita = null;

            dto.DataVisita.Should().BeNull();
        }

        [Theory]
        [InlineData("2024-01-01")]
        [InlineData("2024-06-15")]
        [InlineData("2024-12-31")]
        public void DadoDiferentesDataVisitas_QuandoAssignar_EntaoRetornaValoresCorretos(string dataString)
        {
            var data = DateTime.Parse(dataString);
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataVisita = data;

            dto.DataVisita.Should().Be(data);
        }

        #endregion

        #region Testes de Propriedade DataVisitaFormatada

        [Fact]
        public void DadoDataVisitaFormatadaValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var dataVisitaFormatada = "15/05/2024";
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataVisitaFormatada = dataVisitaFormatada;

            dto.DataVisitaFormatada.Should().Be(dataVisitaFormatada);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoDataVisitaFormatadaAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataVisitaFormatada.Should().BeNull();
        }

        [Fact]
        public void DadoDataVisitaFormatadaNula_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO { DataVisitaFormatada = "15/05/2024" };

            dto.DataVisitaFormatada = null!;

            dto.DataVisitaFormatada.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("01/01/2024")]
        [InlineData("31/12/2024")]
        public void DadoDiferentesDataVisitaFormatadas_QuandoAssignar_EntaoRetornaValoresCorretos(string dataFormatada)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataVisitaFormatada = dataFormatada;

            dto.DataVisitaFormatada.Should().Be(dataFormatada);
        }

        #endregion

        #region Testes de Propriedade TipoAtendimento

        [Fact]
        public void DadoTipoAtendimentoValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var tipoAtendimento = TipoAtendimento.Presencial;
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.TipoAtendimento = tipoAtendimento;

            dto.TipoAtendimento.Should().Be(tipoAtendimento);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoTipoAtendimentoAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.TipoAtendimento.Should().BeNull();
        }

        [Fact]
        public void DadoTipoAtendimentoNulo_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO { TipoAtendimento = TipoAtendimento.Presencial };

            dto.TipoAtendimento = null;

            dto.TipoAtendimento.Should().BeNull();
        }

        [Theory]
        [InlineData(TipoAtendimento.Presencial)]
        [InlineData(TipoAtendimento.Email)]
        public void DadoDiferentesTiposAtendimento_QuandoAssignar_EntaoRetornaValoresCorretos(TipoAtendimento? tipoAtendimento)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.TipoAtendimento = tipoAtendimento;

            dto.TipoAtendimento.Should().Be(tipoAtendimento);
        }

        #endregion

        #region Testes de Propriedade AcervoId

        [Fact]
        public void DadoAcervoIdValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var acervoId = 42L;
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.AcervoId = acervoId;

            dto.AcervoId.Should().Be(acervoId);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoAcervoIdAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.AcervoId.Should().Be(0);
        }

        [Theory]
        [InlineData(1L)]
        [InlineData(100L)]
        [InlineData(999999L)]
        [InlineData(long.MaxValue)]
        public void DadoDiferentesAcervoIds_QuandoAssignar_EntaoRetornaValoresCorretos(long acervoId)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.AcervoId = acervoId;

            dto.AcervoId.Should().Be(acervoId);
        }

        #endregion

        #region Testes de Propriedade Responsavel

        [Fact]
        public void DadoResponsavelValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var responsavel = "João Silva";
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Responsavel = responsavel;

            dto.Responsavel.Should().Be(responsavel);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoResponsavelAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Responsavel.Should().BeNull();
        }

        [Fact]
        public void DadoResponsavelNulo_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO { Responsavel = "João Silva" };

            dto.Responsavel = null!;

            dto.Responsavel.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("João Silva")]
        [InlineData("Maria Santos")]
        public void DadoDiferentesResponsaveis_QuandoAssignar_EntaoRetornaValoresCorretos(string responsavel)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Responsavel = responsavel;

            dto.Responsavel.Should().Be(responsavel);
        }

        #endregion

        #region Testes de Propriedade TipoAcervoId

        [Fact]
        public void DadoTipoAcervoIdValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var tipoAcervoId = TipoAcervo.Bibliografico;
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.TipoAcervoId = tipoAcervoId;

            dto.TipoAcervoId.Should().Be(tipoAcervoId);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoTipoAcervoIdAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.TipoAcervoId.Should().Be(default(TipoAcervo));
        }

        [Theory]
        [InlineData(TipoAcervo.Bibliografico)]
        [InlineData(TipoAcervo.Audiovisual)]
        [InlineData(TipoAcervo.ArtesGraficas)]
        public void DadoDiferentesTiposAcervoId_QuandoAssignar_EntaoRetornaValoresCorretos(TipoAcervo tipoAcervoId)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.TipoAcervoId = tipoAcervoId;

            dto.TipoAcervoId.Should().Be(tipoAcervoId);
        }

        #endregion

        #region Testes de Propriedade DataEmprestimo

        [Fact]
        public void DadoDataEmprestimoValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var dataEmprestimo = new DateTime(2024, 5, 15, 10, 30, 0);
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataEmprestimo = dataEmprestimo;

            dto.DataEmprestimo.Should().Be(dataEmprestimo);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoDataEmprestimoAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataEmprestimo.Should().BeNull();
        }

        [Fact]
        public void DadoDataEmprestimoNula_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO { DataEmprestimo = new DateTime(2024, 5, 15) };

            dto.DataEmprestimo = null;

            dto.DataEmprestimo.Should().BeNull();
        }

        [Theory]
        [InlineData("2024-01-01")]
        [InlineData("2024-06-15")]
        [InlineData("2024-12-31")]
        public void DadoDiferentesDataEmprestimos_QuandoAssignar_EntaoRetornaValoresCorretos(string dataString)
        {
            var data = DateTime.Parse(dataString);
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataEmprestimo = data;

            dto.DataEmprestimo.Should().Be(data);
        }

        #endregion

        #region Testes de Propriedade DataEmprestimoFormatada

        [Fact]
        public void DadoDataEmprestimoFormatadaValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var dataEmprestimoFormatada = "15/05/2024";
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataEmprestimoFormatada = dataEmprestimoFormatada;

            dto.DataEmprestimoFormatada.Should().Be(dataEmprestimoFormatada);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoDataEmprestimoFormatadaAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataEmprestimoFormatada.Should().BeNull();
        }

        [Fact]
        public void DadoDataEmprestimoFormatadaNula_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO { DataEmprestimoFormatada = "15/05/2024" };

            dto.DataEmprestimoFormatada = null!;

            dto.DataEmprestimoFormatada.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("01/01/2024")]
        [InlineData("31/12/2024")]
        public void DadoDiferentesDataEmprestimoFormatadas_QuandoAssignar_EntaoRetornaValoresCorretos(string dataFormatada)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataEmprestimoFormatada = dataFormatada;

            dto.DataEmprestimoFormatada.Should().Be(dataFormatada);
        }

        #endregion

        #region Testes de Propriedade DataDevolucao

        [Fact]
        public void DadoDataDevolucaoValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var dataDevolucao = new DateTime(2024, 5, 15, 10, 30, 0);
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataDevolucao = dataDevolucao;

            dto.DataDevolucao.Should().Be(dataDevolucao);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoDataDevolucaoAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataDevolucao.Should().BeNull();
        }

        [Fact]
        public void DadoDataDevolucaoNula_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO { DataDevolucao = new DateTime(2024, 5, 15) };

            dto.DataDevolucao = null;

            dto.DataDevolucao.Should().BeNull();
        }

        [Theory]
        [InlineData("2024-01-01")]
        [InlineData("2024-06-15")]
        [InlineData("2024-12-31")]
        public void DadoDiferentesDataDevolucoes_QuandoAssignar_EntaoRetornaValoresCorretos(string dataString)
        {
            var data = DateTime.Parse(dataString);
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataDevolucao = data;

            dto.DataDevolucao.Should().Be(data);
        }

        #endregion

        #region Testes de Propriedade DataDevolucaoFormatada

        [Fact]
        public void DadoDataDevolucaoFormatadaValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var dataDevolucaoFormatada = "15/05/2024";
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataDevolucaoFormatada = dataDevolucaoFormatada;

            dto.DataDevolucaoFormatada.Should().Be(dataDevolucaoFormatada);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoDataDevolucaoFormatadaAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataDevolucaoFormatada.Should().BeNull();
        }

        [Fact]
        public void DadoDataDevolucaoFormatadaNula_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO { DataDevolucaoFormatada = "15/05/2024" };

            dto.DataDevolucaoFormatada = null!;

            dto.DataDevolucaoFormatada.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("01/01/2024")]
        [InlineData("31/12/2024")]
        public void DadoDiferentesDataDevolucaoFormatadas_QuandoAssignar_EntaoRetornaValoresCorretos(string dataFormatada)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.DataDevolucaoFormatada = dataFormatada;

            dto.DataDevolucaoFormatada.Should().Be(dataFormatada);
        }

        #endregion

        #region Testes de Propriedade SituacaoEmprestimo

        [Fact]
        public void DadoSituacaoEmprestimoValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var situacaoEmprestimo = SituacaoEmprestimo.DEVOLUCAO_EM_ATRASO;
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.SituacaoEmprestimo = situacaoEmprestimo;

            dto.SituacaoEmprestimo.Should().Be(situacaoEmprestimo);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoSituacaoEmprestimoAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.SituacaoEmprestimo.Should().BeNull();
        }

        [Fact]
        public void DadoSituacaoEmprestimoNula_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO { SituacaoEmprestimo = SituacaoEmprestimo.DEVOLUCAO_EM_ATRASO };

            dto.SituacaoEmprestimo = null;

            dto.SituacaoEmprestimo.Should().BeNull();
        }

        [Theory]
        [InlineData(SituacaoEmprestimo.DEVOLVIDO)]
        [InlineData(SituacaoEmprestimo.DEVOLUCAO_EM_ATRASO)]
        public void DadoDiferentesSituacaoEmprestimos_QuandoAssignar_EntaoRetornaValoresCorretos(SituacaoEmprestimo? situacaoEmprestimo)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.SituacaoEmprestimo = situacaoEmprestimo;

            dto.SituacaoEmprestimo.Should().Be(situacaoEmprestimo);
        }

        #endregion

        #region Testes de Propriedade SituacaoDisponibilidade

        [Fact]
        public void DadoSituacaoDisponibilidadeValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var situacaoDisponibilidade = "Disponível";
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.SituacaoDisponibilidade = situacaoDisponibilidade;

            dto.SituacaoDisponibilidade.Should().Be(situacaoDisponibilidade);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoSituacaoDisponibilidadeAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.SituacaoDisponibilidade.Should().BeNull();
        }

        [Fact]
        public void DadoSituacaoDisponibilidadeNula_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO { SituacaoDisponibilidade = "Disponível" };

            dto.SituacaoDisponibilidade = null!;

            dto.SituacaoDisponibilidade.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Disponível")]
        [InlineData("Indisponível")]
        [InlineData("Sob Demanda")]
        public void DadoDiferentesSituacaoDisponibilidades_QuandoAssignar_EntaoRetornaValoresCorretos(string situacaoDisponibilidade)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.SituacaoDisponibilidade = situacaoDisponibilidade;

            dto.SituacaoDisponibilidade.Should().Be(situacaoDisponibilidade);
        }

        #endregion

        #region Testes de Propriedade EstaDisponivel

        [Fact]
        public void DadoEstaDisponivelComValorTrue_QuandoAssignar_EntaoRetornaTrue()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.EstaDisponivel = true;

            dto.EstaDisponivel.Should().BeTrue();
        }

        [Fact]
        public void DadoEstaDisponivelComValorFalse_QuandoAssignar_EntaoRetornaFalse()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.EstaDisponivel = false;

            dto.EstaDisponivel.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoEstaDisponivelAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.EstaDisponivel.Should().BeFalse();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DadoDiferentesEstaDisponivel_QuandoAssignar_EntaoRetornaValoresCorretos(bool estaDisponivel)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.EstaDisponivel = estaDisponivel;

            dto.EstaDisponivel.Should().Be(estaDisponivel);
        }

        #endregion

        #region Testes de Propriedade TemControleDisponibilidade

        [Fact]
        public void DadoTemControleDisponibilidadeComValorTrue_QuandoAssignar_EntaoRetornaTrue()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.TemControleDisponibilidade = true;

            dto.TemControleDisponibilidade.Should().BeTrue();
        }

        [Fact]
        public void DadoTemControleDisponibilidadeComValorFalse_QuandoAssignar_EntaoRetornaFalse()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.TemControleDisponibilidade = false;

            dto.TemControleDisponibilidade.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoTemControleDisponibilidadeAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.TemControleDisponibilidade.Should().BeFalse();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DadoDiferentesTemControleDisponibilidade_QuandoAssignar_EntaoRetornaValoresCorretos(bool temControleDisponibilidade)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.TemControleDisponibilidade = temControleDisponibilidade;

            dto.TemControleDisponibilidade.Should().Be(temControleDisponibilidade);
        }

        #endregion

        #region Testes de Propriedade PodeFinalizarItem

        [Fact]
        public void DadoPodeFinalizarItemComValorTrue_QuandoAssignar_EntaoRetornaTrue()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.PodeFinalizarItem = true;

            dto.PodeFinalizarItem.Should().BeTrue();
        }

        [Fact]
        public void DadoPodeFinalizarItemComValorFalse_QuandoAssignar_EntaoRetornaFalse()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.PodeFinalizarItem = false;

            dto.PodeFinalizarItem.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoPodeFinalizarItemAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.PodeFinalizarItem.Should().BeFalse();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DadoDiferentesPodeFinalizarItem_QuandoAssignar_EntaoRetornaValoresCorretos(bool podeFinalizarItem)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.PodeFinalizarItem = podeFinalizarItem;

            dto.PodeFinalizarItem.Should().Be(podeFinalizarItem);
        }

        #endregion

        #region Testes de Propriedade SituacaoSaldo

        [Fact]
        public void DadoSituacaoSaldoValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var situacaoSaldo = SituacaoSaldo.INDISPONIVEL_PARA_RESERVA_EMPRESTIMO ;
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.SituacaoSaldo = situacaoSaldo;

            dto.SituacaoSaldo.Should().Be(situacaoSaldo);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoSituacaoSaldoAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.SituacaoSaldo.Should().Be(default(SituacaoSaldo));
        }

        [Theory]
        [InlineData(SituacaoSaldo.RESERVADO)]
        [InlineData(SituacaoSaldo.DISPONIVEL)]
        [InlineData(SituacaoSaldo.INDISPONIVEL_PARA_RESERVA_EMPRESTIMO)]
        public void DadoDiferentesSituacaoSaldos_QuandoAssignar_EntaoRetornaValoresCorretos(SituacaoSaldo situacaoSaldo)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.SituacaoSaldo = situacaoSaldo;

            dto.SituacaoSaldo.Should().Be(situacaoSaldo);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoAtribuirValoresMultiplosAoSituacaoSaldo_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.SituacaoSaldo = SituacaoSaldo.RESERVADO;
            dto.SituacaoSaldo = SituacaoSaldo.DISPONIVEL;
            dto.SituacaoSaldo = SituacaoSaldo.INDISPONIVEL_PARA_RESERVA_EMPRESTIMO;

            dto.SituacaoSaldo.Should().Be(SituacaoSaldo.INDISPONIVEL_PARA_RESERVA_EMPRESTIMO);
        }

        #endregion

        #region Testes de Propriedade PodeEditar

        [Fact]
        public void DadoPodeEditarComValorTrue_QuandoAssignar_EntaoRetornaTrue()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.PodeEditar = true;

            dto.PodeEditar.Should().BeTrue();
        }

        [Fact]
        public void DadoPodeEditarComValorFalse_QuandoAssignar_EntaoRetornaFalse()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.PodeEditar = false;

            dto.PodeEditar.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoPodeEditarAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.PodeEditar.Should().BeFalse();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DadoDiferentesPodeEditar_QuandoAssignar_EntaoRetornaValoresCorretos(bool podeEditar)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.PodeEditar = podeEditar;

            dto.PodeEditar.Should().Be(podeEditar);
        }

        #endregion

        #region Testes de Combinações e Integração

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciar_EntaoTodosOsPropriedadesAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Id.Should().Be(0);
            dto.Codigo.Should().BeNull();
            dto.TipoAcervo.Should().BeNull();
            dto.Titulo.Should().BeNull();
            dto.Situacao.Should().BeNull();
            dto.SituacaoId.Should().Be(default(SituacaoSolicitacaoItem));
            dto.DataVisita.Should().BeNull();
            dto.DataVisitaFormatada.Should().BeNull();
            dto.TipoAtendimento.Should().BeNull();
            dto.AcervoId.Should().Be(0);
            dto.Responsavel.Should().BeNull();
            dto.TipoAcervoId.Should().Be(default(TipoAcervo));
            dto.DataEmprestimo.Should().BeNull();
            dto.DataEmprestimoFormatada.Should().BeNull();
            dto.DataDevolucao.Should().BeNull();
            dto.DataDevolucaoFormatada.Should().BeNull();
            dto.SituacaoEmprestimo.Should().BeNull();
            dto.SituacaoDisponibilidade.Should().BeNull();
            dto.EstaDisponivel.Should().BeFalse();
            dto.TemControleDisponibilidade.Should().BeFalse();
            dto.PodeFinalizarItem.Should().BeFalse();
            dto.SituacaoSaldo.Should().Be(default(SituacaoSaldo));
            dto.PodeEditar.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoAtribuirTodosOsPropriedades_EntaoRetornaTodosOsValoresAssignados()
        {
            var id = 15L;
            var codigo = "ACE-001";
            var tipoAcervo = "Livro";
            var titulo = "O Cortiço";
            var situacao = "Aguardando";
            var situacaoId = SituacaoSolicitacaoItem.CANCELADO;
            var dataVisita = new DateTime(2024, 5, 15);
            var dataVisitaFormatada = "15/05/2024";
            var tipoAtendimento = TipoAtendimento.Presencial;
            var acervoId = 100L;
            var responsavel = "João Silva";
            var tipoAcervoId = TipoAcervo.Bibliografico;
            var dataEmprestimo = new DateTime(2024, 5, 20);
            var dataEmprestimoFormatada = "20/05/2024";
            var dataDevolucao = new DateTime(2024, 6, 20);
            var dataDevolucaoFormatada = "20/06/2024";
            var situacaoEmprestimo = SituacaoEmprestimo.EMPRESTADO;
            var situacaoDisponibilidade = "Disponível";
            var estaDisponivel = true;
            var temControleDisponibilidade = true;
            var podeFinalizarItem = true;
            var situacaoSaldo = SituacaoSaldo.RESERVADO;
            var podeEditar = true;

            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO
            {
                Id = id,
                Codigo = codigo,
                TipoAcervo = tipoAcervo,
                Titulo = titulo,
                Situacao = situacao,
                SituacaoId = situacaoId,
                DataVisita = dataVisita,
                DataVisitaFormatada = dataVisitaFormatada,
                TipoAtendimento = tipoAtendimento,
                AcervoId = acervoId,
                Responsavel = responsavel,
                TipoAcervoId = tipoAcervoId,
                DataEmprestimo = dataEmprestimo,
                DataEmprestimoFormatada = dataEmprestimoFormatada,
                DataDevolucao = dataDevolucao,
                DataDevolucaoFormatada = dataDevolucaoFormatada,
                SituacaoEmprestimo = situacaoEmprestimo,
                SituacaoDisponibilidade = situacaoDisponibilidade,
                EstaDisponivel = estaDisponivel,
                TemControleDisponibilidade = temControleDisponibilidade,
                PodeFinalizarItem = podeFinalizarItem,
                SituacaoSaldo = situacaoSaldo,
                PodeEditar = podeEditar
            };

            dto.Id.Should().Be(id);
            dto.Codigo.Should().Be(codigo);
            dto.TipoAcervo.Should().Be(tipoAcervo);
            dto.Titulo.Should().Be(titulo);
            dto.Situacao.Should().Be(situacao);
            dto.SituacaoId.Should().Be(situacaoId);
            dto.DataVisita.Should().Be(dataVisita);
            dto.DataVisitaFormatada.Should().Be(dataVisitaFormatada);
            dto.TipoAtendimento.Should().Be(tipoAtendimento);
            dto.AcervoId.Should().Be(acervoId);
            dto.Responsavel.Should().Be(responsavel);
            dto.TipoAcervoId.Should().Be(tipoAcervoId);
            dto.DataEmprestimo.Should().Be(dataEmprestimo);
            dto.DataEmprestimoFormatada.Should().Be(dataEmprestimoFormatada);
            dto.DataDevolucao.Should().Be(dataDevolucao);
            dto.DataDevolucaoFormatada.Should().Be(dataDevolucaoFormatada);
            dto.SituacaoEmprestimo.Should().Be(situacaoEmprestimo);
            dto.SituacaoDisponibilidade.Should().Be(situacaoDisponibilidade);
            dto.EstaDisponivel.Should().Be(estaDisponivel);
            dto.TemControleDisponibilidade.Should().Be(temControleDisponibilidade);
            dto.PodeFinalizarItem.Should().Be(podeFinalizarItem);
            dto.SituacaoSaldo.Should().Be(situacaoSaldo);
            dto.PodeEditar.Should().Be(podeEditar);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoModificarPropriedadesSequencialmente_EntaoMantémCoerencia()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Id = 1;
            dto.AcervoId = 10;
            dto.Id.Should().Be(1);
            dto.AcervoId.Should().Be(10);

            dto.SituacaoId = SituacaoSolicitacaoItem.SEM_RESPOSTA_SOLICITANTE;
            dto.DataVisita = new DateTime(2024, 5, 15);
            dto.SituacaoId.Should().Be(SituacaoSolicitacaoItem.SEM_RESPOSTA_SOLICITANTE);
            dto.DataVisita.Should().Be(new DateTime(2024, 5, 15));

            dto.EstaDisponivel = true;
            dto.TemControleDisponibilidade = true;
            dto.PodeFinalizarItem = true;
            dto.EstaDisponivel.Should().BeTrue();
            dto.TemControleDisponibilidade.Should().BeTrue();
            dto.PodeFinalizarItem.Should().BeTrue();
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoCriarMultiplasInstancias_EntaoSãoIndependentes()
        {
            var dto1 = new AcervoSolicitacaoItemDetalheResumidoDTO
            {
                Id = 1,
                AcervoId = 10,
                Titulo = "Livro 1",
                SituacaoId = SituacaoSolicitacaoItem.PRESENCIAL_ABERTO,
                EstaDisponivel = true
            };

            var dto2 = new AcervoSolicitacaoItemDetalheResumidoDTO
            {
                Id = 2,
                AcervoId = 20,
                Titulo = "Livro 2",
                SituacaoId = SituacaoSolicitacaoItem.SEM_RESPOSTA_SOLICITANTE,
                EstaDisponivel = false
            };

            var dto3 = new AcervoSolicitacaoItemDetalheResumidoDTO
            {
                Id = 3,
                AcervoId = 30,
                Titulo = "Livro 3",
                SituacaoId = SituacaoSolicitacaoItem.CANCELADO,
                EstaDisponivel = true
            };

            dto1.Id.Should().Be(1);
            dto1.Titulo.Should().Be("Livro 1");
            dto1.EstaDisponivel.Should().BeTrue();

            dto2.Id.Should().Be(2);
            dto2.Titulo.Should().Be("Livro 2");
            dto2.EstaDisponivel.Should().BeFalse();

            dto3.Id.Should().Be(3);
            dto3.Titulo.Should().Be("Livro 3");
            dto3.EstaDisponivel.Should().BeTrue();

            dto1.Id = 100;
            dto2.Id.Should().Be(2);
            dto3.Id.Should().Be(3);
        }

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoInstanciarComConstrutorPadrao_EntaoTodosOsPropriedadesEstaoAcessiveis()
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<AcervoSolicitacaoItemDetalheResumidoDTO>();
        }

        #endregion

        #region Testes com dados fictícios (Bogus)

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTOComDadosFictícios_QuandoAssignar_EntaoArmazenaCorretamente()
        {
            var faker = new Faker();
            var id = faker.Random.Long(1, 10000);
            var acervoId = faker.Random.Long(1, 10000);
            var situacaoId = faker.PickRandom<SituacaoSolicitacaoItem>();
            var tipoAcervoId = faker.PickRandom<TipoAcervo>();
            var estaDisponivel = faker.Random.Bool();
            var temControleDisponibilidade = faker.Random.Bool();
            var podeFinalizarItem = faker.Random.Bool();
            var podeEditar = faker.Random.Bool();

            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO
            {
                Id = id,
                AcervoId = acervoId,
                Codigo = faker.Commerce.ProductName(),
                Titulo = faker.Lorem.Word(),
                Situacao = faker.Lorem.Word(),
                SituacaoId = situacaoId,
                DataVisita = faker.Date.Past(),
                DataVisitaFormatada = faker.Date.Past().ToString("dd/MM/yyyy"),
                TipoAtendimento = faker.PickRandom<TipoAtendimento>(),
                Responsavel = faker.Person.FirstName,
                TipoAcervoId = tipoAcervoId,
                DataEmprestimo = faker.Date.Past(),
                DataEmprestimoFormatada = faker.Date.Past().ToString("dd/MM/yyyy"),
                DataDevolucao = faker.Date.Future(),
                DataDevolucaoFormatada = faker.Date.Future().ToString("dd/MM/yyyy"),
                SituacaoDisponibilidade = faker.Lorem.Word(),
                EstaDisponivel = estaDisponivel,
                TemControleDisponibilidade = temControleDisponibilidade,
                PodeFinalizarItem = podeFinalizarItem,
                SituacaoSaldo = faker.PickRandom<SituacaoSaldo>(),
                PodeEditar = podeEditar
            };

            dto.Id.Should().Be(id);
            dto.AcervoId.Should().Be(acervoId);
            dto.Codigo.Should().NotBeNullOrEmpty();
            dto.Titulo.Should().NotBeNullOrEmpty();
            dto.EstaDisponivel.Should().Be(estaDisponivel);
            dto.TemControleDisponibilidade.Should().Be(temControleDisponibilidade);
            dto.PodeFinalizarItem.Should().Be(podeFinalizarItem);
            dto.PodeEditar.Should().Be(podeEditar);
        }

        [Theory]
        [InlineData(SituacaoSolicitacaoItem.CANCELADO, TipoAcervo.Bibliografico, true, true)]
        [InlineData(SituacaoSolicitacaoItem.SEM_RESPOSTA_SOLICITANTE, TipoAcervo.DocumentacaoTextual, true, false)]
        [InlineData(SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE, TipoAcervo.Fotografico, false, false)]
        public void DadoCombinacoesDeSituacaoEPermissoes_QuandoAssignar_EntaoRetornaValoresCorretos(
            SituacaoSolicitacaoItem situacao,
            TipoAcervo tipoAcervo,
            bool estaDisponivel,
            bool podeEditar)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO
            {
                SituacaoId = situacao,
                TipoAcervoId = tipoAcervo,
                EstaDisponivel = estaDisponivel,
                PodeEditar = podeEditar
            };

            dto.SituacaoId.Should().Be(situacao);
            dto.TipoAcervoId.Should().Be(tipoAcervo);
            dto.EstaDisponivel.Should().Be(estaDisponivel);
            dto.PodeEditar.Should().Be(podeEditar);
        }

        [Fact]
        public void DadoComDatasCheias_QuandoAssignar_EntaoMantémDados()
        {
            var dataVisita = new DateTime(2024, 5, 15, 14, 30, 45);
            var dataEmprestimo = new DateTime(2024, 5, 20, 10, 15, 30);
            var dataDevolucao = new DateTime(2024, 6, 20, 16, 45, 20);

            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO
            {
                DataVisita = dataVisita,
                DataEmprestimo = dataEmprestimo,
                DataDevolucao = dataDevolucao
            };

            var dtoRecuperado = dto;

            dtoRecuperado.DataVisita.Should().Be(dataVisita);
            dtoRecuperado.DataEmprestimo.Should().Be(dataEmprestimo);
            dtoRecuperado.DataDevolucao.Should().Be(dataDevolucao);
        }

        [Fact]
        public void DadoComCodigoEResponsavel_QuandoAssignar_EntaoMantémDados()
        {
            var codigo = "ACE-001";
            var responsavel = "João Silva";

            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO
            {
                Codigo = codigo,
                Responsavel = responsavel
            };

            var dtoRecuperado = dto;

            dtoRecuperado.Codigo.Should().Be(codigo);
            dtoRecuperado.Responsavel.Should().Be(responsavel);
        }

        [Theory]
        [InlineData(true, true, true, true)]
        [InlineData(true, true, false, false)]
        [InlineData(true, false, true, false)]
        [InlineData(false, false, false, false)]
        public void DadoComDiferentesCombinacoesDeBooleans_QuandoAssignar_EntaoArmazenaCorreto(
            bool estaDisponivel,
            bool temControleDisponibilidade,
            bool podeFinalizarItem,
            bool podeEditar)
        {
            var dto = new AcervoSolicitacaoItemDetalheResumidoDTO
            {
                EstaDisponivel = estaDisponivel,
                TemControleDisponibilidade = temControleDisponibilidade,
                PodeFinalizarItem = podeFinalizarItem,
                PodeEditar = podeEditar
            };

            dto.EstaDisponivel.Should().Be(estaDisponivel);
            dto.TemControleDisponibilidade.Should().Be(temControleDisponibilidade);
            dto.PodeFinalizarItem.Should().Be(podeFinalizarItem);
            dto.PodeEditar.Should().Be(podeEditar);
        }

        #endregion

        #region Testes de Serialização e Deserialização

        [Fact]
        public void DadoAcervoSolicitacaoItemDetalheResumidoDTO_QuandoUtilizarEmSerializacao_EntaoMantemPropriedades()
        {
            var id = 123L;
            var acervoId = 456L;
            var titulo = "Título Teste";

            var dto1 = new AcervoSolicitacaoItemDetalheResumidoDTO
            {
                Id = id,
                AcervoId = acervoId,
                Titulo = titulo
            };

            var dto2 = new AcervoSolicitacaoItemDetalheResumidoDTO
            {
                Id = dto1.Id,
                AcervoId = dto1.AcervoId,
                Titulo = dto1.Titulo
            };

            dto2.Id.Should().Be(dto1.Id);
            dto2.AcervoId.Should().Be(dto1.AcervoId);
            dto2.Titulo.Should().Be(dto1.Titulo);
        }

        #endregion
    }
}
