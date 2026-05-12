using FluentAssertions;
using Moq;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoAudiovisualLinhaRetornoDTOTeste
    {
        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve conter propriedade Titulo")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoContemPropriedadeTitulo()
        {
            typeof(AcervoAudiovisualLinhaRetornoDTO).GetProperty("Titulo").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve conter propriedade Codigo")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoContemPropriedadeCodigo()
        {
            typeof(AcervoAudiovisualLinhaRetornoDTO).GetProperty("Codigo").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve conter propriedade CreditosAutoresIds")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoContemPropriedadeCreditosAutoresIds()
        {
            typeof(AcervoAudiovisualLinhaRetornoDTO).GetProperty("CreditosAutoresIds").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve conter propriedade Localizacao")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoContemPropriedadeLocalizacao()
        {
            typeof(AcervoAudiovisualLinhaRetornoDTO).GetProperty("Localizacao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve conter propriedade Procedencia")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoContemPropriedadeProcedencia()
        {
            typeof(AcervoAudiovisualLinhaRetornoDTO).GetProperty("Procedencia").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve conter propriedade Copia")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoContemPropriedadeCopia()
        {
            typeof(AcervoAudiovisualLinhaRetornoDTO).GetProperty("Copia").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve conter propriedade PermiteUsoImagem")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoContemPropriedadePermiteUsoImagem()
        {
            typeof(AcervoAudiovisualLinhaRetornoDTO).GetProperty("PermiteUsoImagem").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve conter propriedade ConservacaoId")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoContemPropriedadeConservacaoId()
        {
            typeof(AcervoAudiovisualLinhaRetornoDTO).GetProperty("ConservacaoId").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve conter propriedade Descricao")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoContemPropriedadeDescricao()
        {
            typeof(AcervoAudiovisualLinhaRetornoDTO).GetProperty("Descricao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve conter propriedade SuporteId")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoContemPropriedadeSuporteId()
        {
            typeof(AcervoAudiovisualLinhaRetornoDTO).GetProperty("SuporteId").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve conter propriedade Duracao")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoContemPropriedadeDuracao()
        {
            typeof(AcervoAudiovisualLinhaRetornoDTO).GetProperty("Duracao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve conter propriedade CromiaId")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoContemPropriedadeCromiaId()
        {
            typeof(AcervoAudiovisualLinhaRetornoDTO).GetProperty("CromiaId").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve conter propriedade TamanhoArquivo")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoContemPropriedadeTamanhoArquivo()
        {
            typeof(AcervoAudiovisualLinhaRetornoDTO).GetProperty("TamanhoArquivo").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve conter propriedade Acessibilidade")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoContemPropriedadeAcessibilidade()
        {
            typeof(AcervoAudiovisualLinhaRetornoDTO).GetProperty("Acessibilidade").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve conter propriedade Disponibilizacao")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoContemPropriedadeDisponibilizacao()
        {
            typeof(AcervoAudiovisualLinhaRetornoDTO).GetProperty("Disponibilizacao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve conter propriedade Ano")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoContemPropriedadeAno()
        {
            typeof(AcervoAudiovisualLinhaRetornoDTO).GetProperty("Ano").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Herda de AcervoLinhaRetornoDTO")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoDeveHerdarDeAcervoLinhaRetornoDTO()
        {
            var dto = new AcervoAudiovisualLinhaRetornoDTO();

            dto.Should().BeAssignableTo<AcervoLinhaRetornoDTO>();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve conter propriedades herdadas de AcervoLinhaRetornoDTO")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoCriar_EntaoDeveConterPropriedadesHerdadas()
        {
            var tipo = typeof(AcervoAudiovisualLinhaRetornoDTO);

            tipo.GetProperty("Status").Should().NotBeNull();
            tipo.GetProperty("NumeroLinha").Should().NotBeNull();
            tipo.GetProperty("Mensagem").Should().NotBeNull();
            tipo.GetProperty("ErrosCampos").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Propriedades devem ser públicas")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoVerificarVisibilidadePropriedades_EntaoDeveSerPublicas()
        {
            var tipo = typeof(AcervoAudiovisualLinhaRetornoDTO);

            var tituloProperty = tipo.GetProperty("Titulo");
            var codigoProperty = tipo.GetProperty("Codigo");
            var creditosAutoresIdsProperty = tipo.GetProperty("CreditosAutoresIds");
            var localizacaoProperty = tipo.GetProperty("Localizacao");
            var procedenciaProperty = tipo.GetProperty("Procedencia");
            var copiaProperty = tipo.GetProperty("Copia");
            var permiteUsoImagemProperty = tipo.GetProperty("PermiteUsoImagem");
            var conservacaoIdProperty = tipo.GetProperty("ConservacaoId");
            var descricaoProperty = tipo.GetProperty("Descricao");
            var suporteIdProperty = tipo.GetProperty("SuporteId");
            var duracaoProperty = tipo.GetProperty("Duracao");
            var cromiaIdProperty = tipo.GetProperty("CromiaId");
            var tamanhoArquivoProperty = tipo.GetProperty("TamanhoArquivo");
            var acessibilidadeProperty = tipo.GetProperty("Acessibilidade");
            var disponibilizacaoProperty = tipo.GetProperty("Disponibilizacao");
            var anoProperty = tipo.GetProperty("Ano");

            tituloProperty?.CanRead.Should().BeTrue();
            tituloProperty?.CanWrite.Should().BeTrue();
            codigoProperty?.CanRead.Should().BeTrue();
            codigoProperty?.CanWrite.Should().BeTrue();
            creditosAutoresIdsProperty?.CanRead.Should().BeTrue();
            creditosAutoresIdsProperty?.CanWrite.Should().BeTrue();
            localizacaoProperty?.CanRead.Should().BeTrue();
            localizacaoProperty?.CanWrite.Should().BeTrue();
            procedenciaProperty?.CanRead.Should().BeTrue();
            procedenciaProperty?.CanWrite.Should().BeTrue();
            copiaProperty?.CanRead.Should().BeTrue();
            copiaProperty?.CanWrite.Should().BeTrue();
            permiteUsoImagemProperty?.CanRead.Should().BeTrue();
            permiteUsoImagemProperty?.CanWrite.Should().BeTrue();
            conservacaoIdProperty?.CanRead.Should().BeTrue();
            conservacaoIdProperty?.CanWrite.Should().BeTrue();
            descricaoProperty?.CanRead.Should().BeTrue();
            descricaoProperty?.CanWrite.Should().BeTrue();
            suporteIdProperty?.CanRead.Should().BeTrue();
            suporteIdProperty?.CanWrite.Should().BeTrue();
            duracaoProperty?.CanRead.Should().BeTrue();
            duracaoProperty?.CanWrite.Should().BeTrue();
            cromiaIdProperty?.CanRead.Should().BeTrue();
            cromiaIdProperty?.CanWrite.Should().BeTrue();
            tamanhoArquivoProperty?.CanRead.Should().BeTrue();
            tamanhoArquivoProperty?.CanWrite.Should().BeTrue();
            acessibilidadeProperty?.CanRead.Should().BeTrue();
            acessibilidadeProperty?.CanWrite.Should().BeTrue();
            disponibilizacaoProperty?.CanRead.Should().BeTrue();
            disponibilizacaoProperty?.CanWrite.Should().BeTrue();
            anoProperty?.CanRead.Should().BeTrue();
            anoProperty?.CanWrite.Should().BeTrue();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve permitir atribuição de valores às propriedades")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoAtribuirValores_EntaoDeveFazerCorretamente()
        {
            var mockTitulo = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockCodigo = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockCreditosAutoresIds = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockLocalizacao = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockProcedencia = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockCopia = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockPermiteUsoImagem = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockConservacaoId = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockDescricao = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockSuporteId = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockDuracao = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockCromiaId = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockTamanhoArquivo = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockAcessibilidade = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockDisponibilizacao = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockAno = new Mock<LinhaConteudoAjustarRetornoDTO>();

            var dto = new AcervoAudiovisualLinhaRetornoDTO
            {
                Status = ImportacaoStatus.Sucesso,
                Mensagem = "Sucesso na importação",
                NumeroLinha = 5,
                ErrosCampos = new[] { "erro1", "erro2" },
                Titulo = mockTitulo.Object,
                Codigo = mockCodigo.Object,
                CreditosAutoresIds = mockCreditosAutoresIds.Object,
                Localizacao = mockLocalizacao.Object,
                Procedencia = mockProcedencia.Object,
                Copia = mockCopia.Object,
                PermiteUsoImagem = mockPermiteUsoImagem.Object,
                ConservacaoId = mockConservacaoId.Object,
                Descricao = mockDescricao.Object,
                SuporteId = mockSuporteId.Object,
                Duracao = mockDuracao.Object,
                CromiaId = mockCromiaId.Object,
                TamanhoArquivo = mockTamanhoArquivo.Object,
                Acessibilidade = mockAcessibilidade.Object,
                Disponibilizacao = mockDisponibilizacao.Object,
                Ano = mockAno.Object
            };

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.Mensagem.Should().Be("Sucesso na importação");
            dto.NumeroLinha.Should().Be(5);
            dto.ErrosCampos.Should().Equal("erro1", "erro2");
            dto.Titulo.Should().Be(mockTitulo.Object);
            dto.Codigo.Should().Be(mockCodigo.Object);
            dto.CreditosAutoresIds.Should().Be(mockCreditosAutoresIds.Object);
            dto.Localizacao.Should().Be(mockLocalizacao.Object);
            dto.Procedencia.Should().Be(mockProcedencia.Object);
            dto.Copia.Should().Be(mockCopia.Object);
            dto.PermiteUsoImagem.Should().Be(mockPermiteUsoImagem.Object);
            dto.ConservacaoId.Should().Be(mockConservacaoId.Object);
            dto.Descricao.Should().Be(mockDescricao.Object);
            dto.SuporteId.Should().Be(mockSuporteId.Object);
            dto.Duracao.Should().Be(mockDuracao.Object);
            dto.CromiaId.Should().Be(mockCromiaId.Object);
            dto.TamanhoArquivo.Should().Be(mockTamanhoArquivo.Object);
            dto.Acessibilidade.Should().Be(mockAcessibilidade.Object);
            dto.Disponibilizacao.Should().Be(mockDisponibilizacao.Object);
            dto.Ano.Should().Be(mockAno.Object);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve aceitar status Pendente")]
        public void DadoDTOAcervoAudiovisualLinhaRetornoComStatusPendente_QuandoAtribuir_EntaoDeveAceitarPendente()
        {
            var dto = new AcervoAudiovisualLinhaRetornoDTO
            {
                Status = ImportacaoStatus.Pendente
            };

            dto.Status.Should().Be(ImportacaoStatus.Pendente);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve aceitar status Erros")]
        public void DadoDTOAcervoAudiovisualLinhaRetornoComStatusErros_QuandoAtribuir_EntaoDeveAceitarErros()
        {
            var dto = new AcervoAudiovisualLinhaRetornoDTO
            {
                Status = ImportacaoStatus.Erros
            };

            dto.Status.Should().Be(ImportacaoStatus.Erros);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve aceitar status Sucesso")]
        public void DadoDTOAcervoAudiovisualLinhaRetornoComStatusSucesso_QuandoAtribuir_EntaoDeveAceitarSucesso()
        {
            var dto = new AcervoAudiovisualLinhaRetornoDTO
            {
                Status = ImportacaoStatus.Sucesso
            };

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Mensagem pode ser nula")]
        public void DadoDTOAcervoAudiovisualLinhaRetornoComMensagemNula_QuandoAtribuir_EntaoDeveAceitarNulo()
        {
            var dto = new AcervoAudiovisualLinhaRetornoDTO
            {
                Mensagem = null!
            };

            dto.Mensagem.Should().BeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Mensagem pode ser vazia")]
        public void DadoDTOAcervoAudiovisualLinhaRetornoComMensagemVazia_QuandoAtribuir_EntaoDeveAceitarVazio()
        {
            var dto = new AcervoAudiovisualLinhaRetornoDTO
            {
                Mensagem = string.Empty
            };

            dto.Mensagem.Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Mensagem com valor válido deve ser aceita")]
        public void DadoDTOAcervoAudiovisualLinhaRetornoComMensagemValida_QuandoAtribuir_EntaoDeveAceitarMensagem()
        {
            var mensagem = "Linha processada com sucesso";
            var dto = new AcervoAudiovisualLinhaRetornoDTO
            {
                Mensagem = mensagem
            };

            dto.Mensagem.Should().Be(mensagem);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - NumeroLinha com valor válido deve ser aceito")]
        public void DadoDTOAcervoAudiovisualLinhaRetornoComNumeroLinhaValido_QuandoAtribuir_EntaoDeveAceitarNumeroLinha()
        {
            var numeroLinha = 10;
            var dto = new AcervoAudiovisualLinhaRetornoDTO
            {
                NumeroLinha = numeroLinha
            };

            dto.NumeroLinha.Should().Be(numeroLinha);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - NumeroLinha pode ser zero")]
        public void DadoDTOAcervoAudiovisualLinhaRetornoComNumeroLinhaZero_QuandoAtribuir_EntaoDeveAceitarZero()
        {
            var dto = new AcervoAudiovisualLinhaRetornoDTO
            {
                NumeroLinha = 0
            };

            dto.NumeroLinha.Should().Be(0);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - NumeroLinha com valor máximo para int")]
        public void DadoDTOAcervoAudiovisualLinhaRetornoComNumeroLinhaMaximo_QuandoAtribuir_EntaoDeveAceitarMaximo()
        {
            var dto = new AcervoAudiovisualLinhaRetornoDTO
            {
                NumeroLinha = int.MaxValue
            };

            dto.NumeroLinha.Should().Be(int.MaxValue);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - ErrosCampos pode ser array vazio")]
        public void DadoDTOAcervoAudiovisualLinhaRetornoComErrosCamposVazio_QuandoAtribuir_EntaoDeveAceitarArrayVazio()
        {
            var dto = new AcervoAudiovisualLinhaRetornoDTO
            {
                ErrosCampos = Array.Empty<string>()
            };

            dto.ErrosCampos.Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - ErrosCampos pode conter múltiplos erros")]
        public void DadoDTOAcervoAudiovisualLinhaRetornoComMultiplosErrosCampos_QuandoAtribuir_EntaoDeveAceitarMultiplosErros()
        {
            var erros = new[] { "Titulo inválido", "Codigo duplicado", "Descricao vazia" };
            var dto = new AcervoAudiovisualLinhaRetornoDTO
            {
                ErrosCampos = erros
            };

            dto.ErrosCampos.Should().HaveCount(3);
            dto.ErrosCampos.Should().Equal(erros);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Tipo deve ser classe e não interface")]
        public void DadoTipoDTOAcervoAudiovisualLinhaRetorno_QuandoVerificar_EntaoDeveSerClasse()
        {
            var tipo = typeof(AcervoAudiovisualLinhaRetornoDTO);

            tipo.IsClass.Should().BeTrue();
            tipo.IsInterface.Should().BeFalse();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve poder ser instanciado sem parâmetros")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoInstanciarSemParametros_EntaoDeveSerBemSucedido()
        {
            var dto = new AcervoAudiovisualLinhaRetornoDTO();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<AcervoAudiovisualLinhaRetornoDTO>();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Instâncias diferentes devem ser independentes")]
        public void DadoDuasInstanciasDTOAcervoAudiovisualLinhaRetorno_QuandoModificarUma_EntaoOutraNaoDeveSerAfetada()
        {
            var dto1 = new AcervoAudiovisualLinhaRetornoDTO
            {
                NumeroLinha = 1,
                Mensagem = "Erro 1"
            };

            var dto2 = new AcervoAudiovisualLinhaRetornoDTO
            {
                NumeroLinha = 2,
                Mensagem = "Sucesso"
            };

            dto1.NumeroLinha.Should().NotBe(dto2.NumeroLinha);
            dto1.Mensagem.Should().NotBe(dto2.Mensagem);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve suportar inicialização com object initializer")]
        public void DadoDTOAcervoAudiovisualLinhaRetornoComObjectInitializer_QuandoCriar_EntaoDeveInicializarCorretamente()
        {
            var mockTitulo = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockCodigo = new Mock<LinhaConteudoAjustarRetornoDTO>();

            var dto = new AcervoAudiovisualLinhaRetornoDTO
            {
                NumeroLinha = 5,
                Mensagem = "Sucesso",
                Status = ImportacaoStatus.Sucesso,
                Titulo = mockTitulo.Object,
                Codigo = mockCodigo.Object
            };

            dto.Should().NotBeNull();
            dto.NumeroLinha.Should().Be(5);
            dto.Mensagem.Should().Be("Sucesso");
            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.Titulo.Should().Be(mockTitulo.Object);
            dto.Codigo.Should().Be(mockCodigo.Object);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve herdar corretamente de AcervoLinhaRetornoDTO")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoVerificarHeranca_EntaoDeveHerdarCorretamente()
        {
            var tipo = typeof(AcervoAudiovisualLinhaRetornoDTO);
            var baseType = tipo.BaseType;

            baseType.Should().Be(typeof(AcervoLinhaRetornoDTO));
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Propriedades podem ser reconfiguradas após inicialização")]
        public void DadoDTOAcervoAudiovisualLinhaRetornoAposCriacao_QuandoReconfigurarPropriedades_EntaoDeveSobrescreverValoresAnteriores()
        {
            var mockTitulo1 = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockTitulo2 = new Mock<LinhaConteudoAjustarRetornoDTO>();

            var dto = new AcervoAudiovisualLinhaRetornoDTO
            {
                NumeroLinha = 1,
                Titulo = mockTitulo1.Object
            };

            dto.NumeroLinha = 2;
            dto.Titulo = mockTitulo2.Object;

            dto.NumeroLinha.Should().Be(2);
            dto.Titulo.Should().Be(mockTitulo2.Object);
            dto.Titulo.Should().NotBe(mockTitulo1.Object);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Validação completa com todos os campos preenchidos")]
        public void DadoDTOAcervoAudiovisualLinhaRetornoComTodosCamposPreenchidos_QuandoValidar_EntaoDeveSerValido()
        {
            var mockTitulo = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockCodigo = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockCreditosAutoresIds = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockLocalizacao = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockProcedencia = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockCopia = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockPermiteUsoImagem = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockConservacaoId = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockDescricao = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockSuporteId = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockDuracao = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockCromiaId = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockTamanhoArquivo = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockAcessibilidade = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockDisponibilizacao = new Mock<LinhaConteudoAjustarRetornoDTO>();
            var mockAno = new Mock<LinhaConteudoAjustarRetornoDTO>();

            var dto = new AcervoAudiovisualLinhaRetornoDTO
            {
                Status = ImportacaoStatus.Sucesso,
                Mensagem = "Sucesso na importação",
                NumeroLinha = 10,
                ErrosCampos = Array.Empty<string>(),
                Titulo = mockTitulo.Object,
                Codigo = mockCodigo.Object,
                CreditosAutoresIds = mockCreditosAutoresIds.Object,
                Localizacao = mockLocalizacao.Object,
                Procedencia = mockProcedencia.Object,
                Copia = mockCopia.Object,
                PermiteUsoImagem = mockPermiteUsoImagem.Object,
                ConservacaoId = mockConservacaoId.Object,
                Descricao = mockDescricao.Object,
                SuporteId = mockSuporteId.Object,
                Duracao = mockDuracao.Object,
                CromiaId = mockCromiaId.Object,
                TamanhoArquivo = mockTamanhoArquivo.Object,
                Acessibilidade = mockAcessibilidade.Object,
                Disponibilizacao = mockDisponibilizacao.Object,
                Ano = mockAno.Object
            };

            dto.Should().NotBeNull();
            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.NumeroLinha.Should().Be(10);
            dto.Mensagem.Should().Be("Sucesso na importação");
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Propriedades herdadas devem ter visibilidade pública")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoVerificarVisibilidadePropriedadesHerdadas_EntaoDeveSerPublicas()
        {
            var tipo = typeof(AcervoAudiovisualLinhaRetornoDTO);

            var statusProperty = tipo.GetProperty("Status");
            var mensagemProperty = tipo.GetProperty("Mensagem");
            var numeroLinhaProperty = tipo.GetProperty("NumeroLinha");
            var errosCamposProperty = tipo.GetProperty("ErrosCampos");

            statusProperty?.CanRead.Should().BeTrue();
            statusProperty?.CanWrite.Should().BeTrue();
            mensagemProperty?.CanRead.Should().BeTrue();
            mensagemProperty?.CanWrite.Should().BeTrue();
            numeroLinhaProperty?.CanRead.Should().BeTrue();
            numeroLinhaProperty?.CanWrite.Should().BeTrue();
            errosCamposProperty?.CanRead.Should().BeTrue();
            errosCamposProperty?.CanWrite.Should().BeTrue();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Tipo deve ter 16 propriedades específicas além das herdadas")]
        public void DadoDTOAcervoAudiovisualLinhaRetorno_QuandoVerificarPropriedades_EntaoDeveTer16PropriedadesEspecificas()
        {
            var tipo = typeof(AcervoAudiovisualLinhaRetornoDTO);
            var propriedadesEspecificas = new[]
            {
                "Titulo", "Codigo", "CreditosAutoresIds", "Localizacao", "Procedencia",
                "Copia", "PermiteUsoImagem", "ConservacaoId", "Descricao", "SuporteId",
                "Duracao", "CromiaId", "TamanhoArquivo", "Acessibilidade", "Disponibilizacao", "Ano"
            };

            foreach (var nomePropriedade in propriedadesEspecificas)
            {
                tipo.GetProperty(nomePropriedade).Should().NotBeNull();
            }
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve permitir ErrosCampos como array com um elemento")]
        public void DadoDTOAcervoAudiovisualLinhaRetornoComErrosCamposUmElemento_QuandoAtribuir_EntaoDeveAceitarUmErro()
        {
            var erros = new[] { "Erro único" };
            var dto = new AcervoAudiovisualLinhaRetornoDTO
            {
                ErrosCampos = erros
            };

            dto.ErrosCampos.Should().HaveCount(1);
            dto.ErrosCampos[0].Should().Be("Erro único");
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaRetornoDTO - Deve manter integridade de dados ao combinar objetos mock")]
        public void DadoDTOAcervoAudiovisualLinhaRetornoComMocks_QuandoCombinarObjetos_EntaoDeveMantérIntegridade()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "Título teste",
                PossuiErro = false
            };

            var dto = new AcervoAudiovisualLinhaRetornoDTO
            {
                Titulo = linhaConteudo,
                NumeroLinha = 5,
                Status = ImportacaoStatus.Sucesso
            };

            dto.Titulo.Should().NotBeNull();
            dto.Titulo.Conteudo.Should().Be("Título teste");
            dto.Titulo.PossuiErro.Should().BeFalse();
        }
    }
}
