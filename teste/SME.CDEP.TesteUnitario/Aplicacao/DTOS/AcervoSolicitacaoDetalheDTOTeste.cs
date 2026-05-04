using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoSolicitacaoDetalheDTOTeste
    {
        #region Testes de Propriedade DadosSolicitante

        [Fact]
        public void DadoDadosSolicitanteValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var dadosSolicitante = new DadosSolicitanteDto { Nome = "João Silva" };
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.DadosSolicitante = dadosSolicitante;

            dto.DadosSolicitante.Should().Be(dadosSolicitante);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoInstanciar_EntaoDadosSolicitanteAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoDetalheDTO { DadosSolicitante = new DadosSolicitanteDto() };

            dto.DadosSolicitante.Should().NotBeNull();
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoAtribuirNullAoDadosSolicitante_EntaoRetornaNulo()
        {
            var dto = new AcervoSolicitacaoDetalheDTO { DadosSolicitante = new DadosSolicitanteDto { Nome = "Teste" } };

            dto.DadosSolicitante = null;

            dto.DadosSolicitante.Should().BeNull();
        }

        #endregion

        #region Testes de Propriedade Id

        [Fact]
        public void DadoIdValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var id = 42L;
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.Id = id;

            dto.Id.Should().Be(id);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoInstanciar_EntaoIdAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.Id.Should().Be(0);
        }

        [Theory]
        [InlineData(1L)]
        [InlineData(100L)]
        [InlineData(999999L)]
        [InlineData(long.MaxValue)]
        public void DadoDiferentesIds_QuandoAssignar_EntaoRetornaValoresCorretos(long id)
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.Id = id;

            dto.Id.Should().Be(id);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoAtribuirValoresMultiplosAoId_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.Id = 10;
            dto.Id = 20;
            dto.Id = 30;

            dto.Id.Should().Be(30);
        }

        #endregion

        #region Testes de Propriedade UsuarioId

        [Fact]
        public void DadoUsuarioIdValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var usuarioId = 123L;
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.UsuarioId = usuarioId;

            dto.UsuarioId.Should().Be(usuarioId);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoInstanciar_EntaoUsuarioIdAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.UsuarioId.Should().Be(0);
        }

        [Theory]
        [InlineData(1L)]
        [InlineData(100L)]
        [InlineData(999999L)]
        [InlineData(long.MaxValue)]
        public void DadoDiferentesUsuarioIds_QuandoAssignar_EntaoRetornaValoresCorretos(long usuarioId)
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.UsuarioId = usuarioId;

            dto.UsuarioId.Should().Be(usuarioId);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoAtribuirValoresMultiplosAoUsuarioId_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.UsuarioId = 10;
            dto.UsuarioId = 20;
            dto.UsuarioId = 30;

            dto.UsuarioId.Should().Be(30);
        }

        #endregion

        #region Testes de Propriedade DataSolicitacao

        [Fact]
        public void DadoDataSolicitacaoValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var dataSolicitacao = new DateTime(2024, 5, 15, 10, 30, 0);
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.DataSolicitacao = dataSolicitacao;

            dto.DataSolicitacao.Should().Be(dataSolicitacao);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoInstanciar_EntaoDataSolicitacaoAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.DataSolicitacao.Should().Be(default(DateTime));
        }

        [Theory]
        [InlineData("2024-01-01")]
        [InlineData("2024-06-15")]
        [InlineData("2024-12-31")]
        public void DadoDiferentesDataSolicitacoes_QuandoAssignar_EntaoRetornaValoresCorretos(string dataString)
        {
            var data = DateTime.Parse(dataString);
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.DataSolicitacao = data;

            dto.DataSolicitacao.Should().Be(data);
        }

        #endregion

        #region Testes de Propriedade DataSolicitacaoFormatado

        [Fact]
        public void DadoDataSolicitacaoFormatadoValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var dataSolicitacaoFormatado = "15/05/2024";
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.DataSolicitacaoFormatado = dataSolicitacaoFormatado;

            dto.DataSolicitacaoFormatado.Should().Be(dataSolicitacaoFormatado);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoInstanciar_EntaoDataSolicitacaoFormatadoAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.DataSolicitacaoFormatado.Should().BeNull();
        }

        [Fact]
        public void DadoDataSolicitacaoFormatadoNula_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoDetalheDTO { DataSolicitacaoFormatado = "15/05/2024" };

            dto.DataSolicitacaoFormatado = null;

            dto.DataSolicitacaoFormatado.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("01/01/2024")]
        [InlineData("31/12/2024")]
        public void DadoDiferentesDataSolicitacaoFormatados_QuandoAssignar_EntaoRetornaValoresCorretos(string dataFormatado)
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.DataSolicitacaoFormatado = dataFormatado;

            dto.DataSolicitacaoFormatado.Should().Be(dataFormatado);
        }

        #endregion

        #region Testes de Propriedade Situacao

        [Fact]
        public void DadoSituacaoValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var situacao = "Aguardando";
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.Situacao = situacao;

            dto.Situacao.Should().Be(situacao);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoInstanciar_EntaoSituacaoAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.Situacao.Should().BeNull();
        }

        [Fact]
        public void DadoSituacaoNula_QuandoAssignar_EntaoArmazenaNull()
        {
            var dto = new AcervoSolicitacaoDetalheDTO { Situacao = "Aguardando" };

            dto.Situacao = null;

            dto.Situacao.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Aguardando Atendimento")]
        [InlineData("Aguardando Visita")]
        [InlineData("Finalizado")]
        public void DadoDiferentesSituacoes_QuandoAssignar_EntaoRetornaValoresCorretos(string situacao)
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.Situacao = situacao;

            dto.Situacao.Should().Be(situacao);
        }

        #endregion

        #region Testes de Propriedade SituacaoId

        [Fact]
        public void DadoSituacaoIdValida_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var situacaoId = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO;
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.SituacaoId = situacaoId;

            dto.SituacaoId.Should().Be(situacaoId);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoInstanciar_EntaoSituacaoIdAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.SituacaoId.Should().Be(default(SituacaoSolicitacao));
        }

        [Theory]
        [InlineData(SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO)]
        [InlineData(SituacaoSolicitacao.AGUARDANDO_VISITA)]
        [InlineData(SituacaoSolicitacao.ATENDIDO_PARCIALMENTE)]
        [InlineData(SituacaoSolicitacao.CANCELADO)]
        public void DadoDiferentesSituacaoIds_QuandoAssignar_EntaoRetornaValoresCorretos(SituacaoSolicitacao situacaoId)
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.SituacaoId = situacaoId;

            dto.SituacaoId.Should().Be(situacaoId);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoAtribuirValoresMultiplosAoSituacaoId_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.SituacaoId = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO;
            dto.SituacaoId = SituacaoSolicitacao.AGUARDANDO_VISITA;
            dto.SituacaoId = SituacaoSolicitacao.ATENDIDO_PARCIALMENTE;

            dto.SituacaoId.Should().Be(SituacaoSolicitacao.ATENDIDO_PARCIALMENTE);
        }

        #endregion

        #region Testes de Propriedade Itens

        [Fact]
        public void DadoItensValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var itens = new[]
            {
                new AcervoSolicitacaoItemDetalheResumidoDTO { Id = 1, Titulo = "Item 1" },
                new AcervoSolicitacaoItemDetalheResumidoDTO { Id = 2, Titulo = "Item 2" }
            };
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.Itens = itens;

            dto.Itens.Should().BeEquivalentTo(itens);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoInstanciar_EntaoItensAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.Itens.Should().NotBeNull();
            dto.Itens.Should().BeEmpty();
        }

        [Fact]
        public void DadoItensNula_QuandoAssignar_EntaoArmazenaEmpty()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.Itens = Enumerable.Empty<AcervoSolicitacaoItemDetalheResumidoDTO>();

            dto.Itens.Should().NotBeNull();
            dto.Itens.Should().BeEmpty();
        }

        [Fact]
        public void DadoItensComMultiplosItems_QuandoAssignar_EntaoRetornaTodosOsItens()
        {
            var itens = new[]
            {
                new AcervoSolicitacaoItemDetalheResumidoDTO { Id = 1, Titulo = "Item 1" },
                new AcervoSolicitacaoItemDetalheResumidoDTO { Id = 2, Titulo = "Item 2" },
                new AcervoSolicitacaoItemDetalheResumidoDTO { Id = 3, Titulo = "Item 3" }
            };
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.Itens = itens;

            dto.Itens.Count().Should().Be(3);
            dto.Itens.Should().BeEquivalentTo(itens);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoAtribuirValoresMultiplosAoItens_EntaoRetornaUltimoValorAssignado()
        {
            var itens1 = new[] { new AcervoSolicitacaoItemDetalheResumidoDTO { Id = 1 } };
            var itens2 = new[] { new AcervoSolicitacaoItemDetalheResumidoDTO { Id = 2 } };
            var itens3 = new[] { new AcervoSolicitacaoItemDetalheResumidoDTO { Id = 3 } };

            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.Itens = itens1;
            dto.Itens = itens2;
            dto.Itens = itens3;

            dto.Itens.First().Id.Should().Be(3);
        }

        #endregion

        #region Testes de Propriedade LimiteDiasEmprestimoAcervo

        [Fact]
        public void DadoLimiteDiasEmprestimoAcervoValido_QuandoAssignar_EntaoRetornaValorAssignado()
        {
            var limiteDiasEmprestimoAcervo = 14;
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.LimiteDiasEmprestimoAcervo = limiteDiasEmprestimoAcervo;

            dto.LimiteDiasEmprestimoAcervo.Should().Be(limiteDiasEmprestimoAcervo);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoInstanciar_EntaoLimiteDiasEmprestimoAcervoAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.LimiteDiasEmprestimoAcervo.Should().Be(0);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(14)]
        [InlineData(30)]
        [InlineData(365)]
        public void DadoDiferentesLimiteDiasEmprestimoAcervo_QuandoAssignar_EntaoRetornaValoresCorretos(int limiteDiasEmprestimoAcervo)
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.LimiteDiasEmprestimoAcervo = limiteDiasEmprestimoAcervo;

            dto.LimiteDiasEmprestimoAcervo.Should().Be(limiteDiasEmprestimoAcervo);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoAtribuirValoresMultiplosAoLimiteDiasEmprestimoAcervo_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.LimiteDiasEmprestimoAcervo = 7;
            dto.LimiteDiasEmprestimoAcervo = 14;
            dto.LimiteDiasEmprestimoAcervo = 30;

            dto.LimiteDiasEmprestimoAcervo.Should().Be(30);
        }

        #endregion

        #region Testes de Propriedade PodeFinalizar

        [Fact]
        public void DadoPodeFinalizarComValorTrue_QuandoAssignar_EntaoRetornaTrue()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.PodeFinalizar = true;

            dto.PodeFinalizar.Should().BeTrue();
        }

        [Fact]
        public void DadoPodeFinalizarComValorFalse_QuandoAssignar_EntaoRetornaFalse()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.PodeFinalizar = false;

            dto.PodeFinalizar.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoInstanciar_EntaoPodeFinalizarAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.PodeFinalizar.Should().BeFalse();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DadoDiferentesPodeFinalizar_QuandoAssignar_EntaoRetornaValoresCorretos(bool podeFinalizar)
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.PodeFinalizar = podeFinalizar;

            dto.PodeFinalizar.Should().Be(podeFinalizar);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoAtribuirValoresMultiplosAoPodeFinalizar_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.PodeFinalizar = true;
            dto.PodeFinalizar = false;
            dto.PodeFinalizar = true;

            dto.PodeFinalizar.Should().BeTrue();
        }

        #endregion

        #region Testes de Propriedade PodeCancelar

        [Fact]
        public void DadoPodeCancelarComValorTrue_QuandoAssignar_EntaoRetornaTrue()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.PodeCancelar = true;

            dto.PodeCancelar.Should().BeTrue();
        }

        [Fact]
        public void DadoPodeCancelarComValorFalse_QuandoAssignar_EntaoRetornaFalse()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.PodeCancelar = false;

            dto.PodeCancelar.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoInstanciar_EntaoPodeCancelarAssumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.PodeCancelar.Should().BeFalse();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DadoDiferentesPodeCancelar_QuandoAssignar_EntaoRetornaValoresCorretos(bool podeCancelar)
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.PodeCancelar = podeCancelar;

            dto.PodeCancelar.Should().Be(podeCancelar);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoAtribuirValoresMultiplosAoPodeCancelar_EntaoRetornaUltimoValorAssignado()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.PodeCancelar = true;
            dto.PodeCancelar = false;
            dto.PodeCancelar = true;

            dto.PodeCancelar.Should().BeTrue();
        }

        #endregion

        #region Testes de Combinações e Integração

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoInstanciar_EntaoTodosOsPropriedadesAsumeValorPadrao()
        {
            var dto = new AcervoSolicitacaoDetalheDTO { DadosSolicitante = new DadosSolicitanteDto() };

            dto.DadosSolicitante.Should().NotBeNull();
            dto.Id.Should().Be(0);
            dto.UsuarioId.Should().Be(0);
            dto.DataSolicitacao.Should().Be(default(DateTime));
            dto.DataSolicitacaoFormatado.Should().BeNull();
            dto.Situacao.Should().BeNull();
            dto.SituacaoId.Should().Be(default(SituacaoSolicitacao));
            dto.Itens.Should().NotBeNull();
            dto.Itens.Should().BeEmpty();
            dto.LimiteDiasEmprestimoAcervo.Should().Be(0);
            dto.PodeFinalizar.Should().BeFalse();
            dto.PodeCancelar.Should().BeFalse();
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoAtribuirTodosOsPropriedades_EntaoRetornaTodosOsValoresAssignados()
        {
            var dadosSolicitante = new DadosSolicitanteDto { Nome = "João Silva" };
            var id = 15L;
            var usuarioId = 25L;
            var dataSolicitacao = new DateTime(2024, 5, 15);
            var dataSolicitacaoFormatado = "15/05/2024";
            var situacao = "Aguardando Atendimento";
            var situacaoId = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO;
            var itens = new[] { new AcervoSolicitacaoItemDetalheResumidoDTO { Id = 1, Titulo = "Item 1" } };
            var limiteDiasEmprestimoAcervo = 14;
            var podeFinalizar = true;
            var podeCancelar = false;

            var dto = new AcervoSolicitacaoDetalheDTO
            {
                DadosSolicitante = dadosSolicitante,
                Id = id,
                UsuarioId = usuarioId,
                DataSolicitacao = dataSolicitacao,
                DataSolicitacaoFormatado = dataSolicitacaoFormatado,
                Situacao = situacao,
                SituacaoId = situacaoId,
                Itens = itens,
                LimiteDiasEmprestimoAcervo = limiteDiasEmprestimoAcervo,
                PodeFinalizar = podeFinalizar,
                PodeCancelar = podeCancelar
            };

            dto.DadosSolicitante.Should().Be(dadosSolicitante);
            dto.Id.Should().Be(id);
            dto.UsuarioId.Should().Be(usuarioId);
            dto.DataSolicitacao.Should().Be(dataSolicitacao);
            dto.DataSolicitacaoFormatado.Should().Be(dataSolicitacaoFormatado);
            dto.Situacao.Should().Be(situacao);
            dto.SituacaoId.Should().Be(situacaoId);
            dto.Itens.Should().BeEquivalentTo(itens);
            dto.LimiteDiasEmprestimoAcervo.Should().Be(limiteDiasEmprestimoAcervo);
            dto.PodeFinalizar.Should().Be(podeFinalizar);
            dto.PodeCancelar.Should().Be(podeCancelar);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoModificarPropriedadesSequencialmente_EntaoMantémCoerencia()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.Id = 1;
            dto.UsuarioId = 10;
            dto.Id.Should().Be(1);
            dto.UsuarioId.Should().Be(10);

            dto.SituacaoId = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO;
            dto.DataSolicitacao = new DateTime(2024, 5, 15);
            dto.SituacaoId.Should().Be(SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO);
            dto.DataSolicitacao.Should().Be(new DateTime(2024, 5, 15));

            dto.PodeFinalizar = true;
            dto.PodeCancelar = false;
            dto.LimiteDiasEmprestimoAcervo = 14;
            dto.PodeFinalizar.Should().BeTrue();
            dto.PodeCancelar.Should().BeFalse();
            dto.LimiteDiasEmprestimoAcervo.Should().Be(14);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoCriarMultiplasInstancias_EntaoSãoIndependentes()
        {
            var dto1 = new AcervoSolicitacaoDetalheDTO
            {
                Id = 1,
                UsuarioId = 10,
                SituacaoId = SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO,
                PodeFinalizar = true,
                PodeCancelar = false
            };

            var dto2 = new AcervoSolicitacaoDetalheDTO
            {
                Id = 2,
                UsuarioId = 20,
                SituacaoId = SituacaoSolicitacao.AGUARDANDO_VISITA,
                PodeFinalizar = false,
                PodeCancelar = true
            };

            var dto3 = new AcervoSolicitacaoDetalheDTO
            {
                Id = 3,
                UsuarioId = 30,
                SituacaoId = SituacaoSolicitacao.ATENDIDO_PARCIALMENTE,
                PodeFinalizar = false,
                PodeCancelar = false
            };

            dto1.Id.Should().Be(1);
            dto1.UsuarioId.Should().Be(10);
            dto1.SituacaoId.Should().Be(SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO);

            dto2.Id.Should().Be(2);
            dto2.UsuarioId.Should().Be(20);
            dto2.SituacaoId.Should().Be(SituacaoSolicitacao.AGUARDANDO_VISITA);

            dto3.Id.Should().Be(3);
            dto3.UsuarioId.Should().Be(30);
            dto3.SituacaoId.Should().Be(SituacaoSolicitacao.ATENDIDO_PARCIALMENTE);

            dto1.Id = 100;
            dto2.Id.Should().Be(2);
            dto3.Id.Should().Be(3);
        }

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTO_QuandoInstanciarComConstrutorPadrao_EntaoTodosOsPropriedadesEstaoAcessiveis()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<AcervoSolicitacaoDetalheDTO>();
        }

        [Fact]
        public void DadoComMultiplosItens_QuandoAssignarItens_EntaoArmazenaOsItensCorretamente()
        {
            var itens = new[]
            {
                new AcervoSolicitacaoItemDetalheResumidoDTO { Id = 1, Titulo = "Item 1", Situacao = "Aguardando" },
                new AcervoSolicitacaoItemDetalheResumidoDTO { Id = 2, Titulo = "Item 2", Situacao = "Finalizado" },
                new AcervoSolicitacaoItemDetalheResumidoDTO { Id = 3, Titulo = "Item 3", Situacao = "Cancelado" }
            };

            var dto = new AcervoSolicitacaoDetalheDTO { Itens = itens };

            dto.Itens.Should().HaveCount(3);
            dto.Itens.First().Id.Should().Be(1);
            dto.Itens.ElementAt(1).Id.Should().Be(2);
            dto.Itens.Last().Id.Should().Be(3);
        }

        #endregion

        #region Testes com dados fictícios (Bogus)

        [Fact]
        public void DadoAcervoSolicitacaoDetalheDTOComDadosFictícios_QuandoAssignar_EntaoArmazenaCorretamente()
        {
            var faker = new Faker();
            var id = faker.Random.Long(1, 10000);
            var usuarioId = faker.Random.Long(1, 10000);
            var situacaoId = faker.PickRandom<SituacaoSolicitacao>();
            var limiteDiasEmprestimoAcervo = faker.Random.Int(1, 365);
            var podeFinalizar = faker.Random.Bool();
            var podeCancelar = faker.Random.Bool();

            var dto = new AcervoSolicitacaoDetalheDTO
            {
                Id = id,
                UsuarioId = usuarioId,
                SituacaoId = situacaoId,
                DataSolicitacao = faker.Date.Past(),
                DataSolicitacaoFormatado = faker.Date.Past().ToString("dd/MM/yyyy"),
                Situacao = faker.Lorem.Word(),
                LimiteDiasEmprestimoAcervo = limiteDiasEmprestimoAcervo,
                PodeFinalizar = podeFinalizar,
                PodeCancelar = podeCancelar
            };

            dto.Id.Should().Be(id);
            dto.UsuarioId.Should().Be(usuarioId);
            dto.SituacaoId.Should().Be(situacaoId);
            dto.DataSolicitacao.Should().NotBe(default(DateTime));
            dto.DataSolicitacaoFormatado.Should().NotBeNullOrEmpty();
            dto.Situacao.Should().NotBeNullOrEmpty();
            dto.LimiteDiasEmprestimoAcervo.Should().Be(limiteDiasEmprestimoAcervo);
            dto.PodeFinalizar.Should().Be(podeFinalizar);
            dto.PodeCancelar.Should().Be(podeCancelar);
        }

        [Theory]
        [InlineData(SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO, true, false)]
        [InlineData(SituacaoSolicitacao.AGUARDANDO_VISITA, true, true)]
        [InlineData(SituacaoSolicitacao.ATENDIDO_PARCIALMENTE, false, false)]
        [InlineData(SituacaoSolicitacao.CANCELADO, false, false)]
        public void DadoCombinacoesDeSituacaoEPermissoes_QuandoAssignar_EntaoRetornaValoresCorretos(
            SituacaoSolicitacao situacao,
            bool podeFinalizar,
            bool podeCancelar)
        {
            var dto = new AcervoSolicitacaoDetalheDTO
            {
                SituacaoId = situacao,
                PodeFinalizar = podeFinalizar,
                PodeCancelar = podeCancelar
            };

            dto.SituacaoId.Should().Be(situacao);
            dto.PodeFinalizar.Should().Be(podeFinalizar);
            dto.PodeCancelar.Should().Be(podeCancelar);
        }

        [Fact]
        public void DadoComDadosSolicitantePreenchidos_QuandoAssignar_EntaoMantémDados()
        {
            var dadosSolicitante = new DadosSolicitanteDto { Nome = "Maria Silva", Email = "maria@example.com" };
            var dto = new AcervoSolicitacaoDetalheDTO { DadosSolicitante = dadosSolicitante };

            var dtoRecuperado = dto;

            dtoRecuperado.DadosSolicitante.Should().Be(dadosSolicitante);
            dtoRecuperado.DadosSolicitante.Nome.Should().Be("Maria Silva");
        }

        [Fact]
        public void DadoComLimiteDiasEmprestimoAcervoZero_QuandoAssignar_EntaoArmazenaZero()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.LimiteDiasEmprestimoAcervo = 0;

            dto.LimiteDiasEmprestimoAcervo.Should().Be(0);
        }

        [Fact]
        public void DadoComLimiteDiasEmprestimoAcervoNegativo_QuandoAssignar_EntaoArmazenaNegativo()
        {
            var dto = new AcervoSolicitacaoDetalheDTO();

            dto.LimiteDiasEmprestimoAcervo = -5;

            dto.LimiteDiasEmprestimoAcervo.Should().Be(-5);
        }

        #endregion
    }
}
