using FluentAssertions;
using Moq;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoAudiovisualLinhaDTOTeste
    {
        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve conter propriedade Titulo")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoContemPropriedadeTitulo()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            typeof(AcervoAudiovisualLinhaDTO).GetProperty("Titulo").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve conter propriedade Codigo")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoContemPropriedadeCodigo()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            typeof(AcervoAudiovisualLinhaDTO).GetProperty("Codigo").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve conter propriedade Credito")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoContemPropriedadeCredito()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            typeof(AcervoAudiovisualLinhaDTO).GetProperty("Credito").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve conter propriedade Localizacao")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoContemPropriedadeLocalizacao()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            typeof(AcervoAudiovisualLinhaDTO).GetProperty("Localizacao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve conter propriedade Procedencia")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoContemPropriedadeProcedencia()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            typeof(AcervoAudiovisualLinhaDTO).GetProperty("Procedencia").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve conter propriedade Copia")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoContemPropriedadeCopia()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            typeof(AcervoAudiovisualLinhaDTO).GetProperty("Copia").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve conter propriedade PermiteUsoImagem")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoContemPropriedadePermiteUsoImagem()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            typeof(AcervoAudiovisualLinhaDTO).GetProperty("PermiteUsoImagem").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve conter propriedade EstadoConservacao")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoContemPropriedadeEstadoConservacao()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            typeof(AcervoAudiovisualLinhaDTO).GetProperty("EstadoConservacao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve conter propriedade Descricao")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoContemPropriedadeDescricao()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            typeof(AcervoAudiovisualLinhaDTO).GetProperty("Descricao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve conter propriedade Suporte")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoContemPropriedadeSuporte()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            typeof(AcervoAudiovisualLinhaDTO).GetProperty("Suporte").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve conter propriedade Duracao")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoContemPropriedadeDuracao()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            typeof(AcervoAudiovisualLinhaDTO).GetProperty("Duracao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve conter propriedade Cromia")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoContemPropriedadeCromia()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            typeof(AcervoAudiovisualLinhaDTO).GetProperty("Cromia").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve conter propriedade TamanhoArquivo")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoContemPropriedadeTamanhoArquivo()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            typeof(AcervoAudiovisualLinhaDTO).GetProperty("TamanhoArquivo").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve conter propriedade Acessibilidade")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoContemPropriedadeAcessibilidade()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            typeof(AcervoAudiovisualLinhaDTO).GetProperty("Acessibilidade").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve conter propriedade Disponibilizacao")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoContemPropriedadeDisponibilizacao()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            typeof(AcervoAudiovisualLinhaDTO).GetProperty("Disponibilizacao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve conter propriedade Ano")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoContemPropriedadeAno()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            typeof(AcervoAudiovisualLinhaDTO).GetProperty("Ano").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Herda de AcervoLinhaDTO")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoDeveHerdarDeAcervoLinhaDTO()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            dto.Should().BeAssignableTo<AcervoLinhaDTO>();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve conter propriedades herdadas de AcervoLinhaDTO")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoCriar_EntaoDeveConterPropriedadesHerdadas()
        {
            var tipo = typeof(AcervoAudiovisualLinhaDTO);

            tipo.GetProperty("Status").Should().NotBeNull();
            tipo.GetProperty("Mensagem").Should().NotBeNull();
            tipo.GetProperty("NumeroLinha").Should().NotBeNull();
            tipo.GetProperty("PossuiErros").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Propriedades devem ser públicas")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoVerificarVisibilidadePropriedades_EntaoDeveSerPublicas()
        {
            var tipo = typeof(AcervoAudiovisualLinhaDTO);

            var tituloProperty = tipo.GetProperty("Titulo");
            var codigoProperty = tipo.GetProperty("Codigo");
            var creditoProperty = tipo.GetProperty("Credito");
            var localizacaoProperty = tipo.GetProperty("Localizacao");
            var procedenciaProperty = tipo.GetProperty("Procedencia");
            var copiaProperty = tipo.GetProperty("Copia");
            var permiteUsoImagemProperty = tipo.GetProperty("PermiteUsoImagem");
            var estadoConservacaoProperty = tipo.GetProperty("EstadoConservacao");
            var descricaoProperty = tipo.GetProperty("Descricao");
            var suporteProperty = tipo.GetProperty("Suporte");
            var duracaoProperty = tipo.GetProperty("Duracao");
            var cromiaProperty = tipo.GetProperty("Cromia");
            var tamanhoArquivoProperty = tipo.GetProperty("TamanhoArquivo");
            var acessibilidadeProperty = tipo.GetProperty("Acessibilidade");
            var disponibilizacaoProperty = tipo.GetProperty("Disponibilizacao");
            var anoProperty = tipo.GetProperty("Ano");

            tituloProperty?.CanRead.Should().BeTrue();
            tituloProperty?.CanWrite.Should().BeTrue();
            codigoProperty?.CanRead.Should().BeTrue();
            codigoProperty?.CanWrite.Should().BeTrue();
            creditoProperty?.CanRead.Should().BeTrue();
            creditoProperty?.CanWrite.Should().BeTrue();
            localizacaoProperty?.CanRead.Should().BeTrue();
            localizacaoProperty?.CanWrite.Should().BeTrue();
            procedenciaProperty?.CanRead.Should().BeTrue();
            procedenciaProperty?.CanWrite.Should().BeTrue();
            copiaProperty?.CanRead.Should().BeTrue();
            copiaProperty?.CanWrite.Should().BeTrue();
            permiteUsoImagemProperty?.CanRead.Should().BeTrue();
            permiteUsoImagemProperty?.CanWrite.Should().BeTrue();
            estadoConservacaoProperty?.CanRead.Should().BeTrue();
            estadoConservacaoProperty?.CanWrite.Should().BeTrue();
            descricaoProperty?.CanRead.Should().BeTrue();
           descricaoProperty?.CanWrite.Should().BeTrue();
            suporteProperty?.CanRead.Should().BeTrue();
            suporteProperty?.CanWrite.Should().BeTrue();
            duracaoProperty?.CanRead.Should().BeTrue();
            duracaoProperty?.CanWrite.Should().BeTrue();
            cromiaProperty?.CanRead.Should().BeTrue();
            cromiaProperty?.CanWrite.Should().BeTrue();
            tamanhoArquivoProperty?.CanRead.Should().BeTrue();
            tamanhoArquivoProperty?.CanWrite.Should().BeTrue();
            acessibilidadeProperty?.CanRead.Should().BeTrue();
            acessibilidadeProperty?.CanWrite.Should().BeTrue();
            disponibilizacaoProperty?.CanRead.Should().BeTrue();
            disponibilizacaoProperty?.CanWrite.Should().BeTrue();
            anoProperty?.CanRead.Should().BeTrue();
            anoProperty?.CanWrite.Should().BeTrue();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve permitir atribuição de valores às propriedades")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoAtribuirValores_EntaoDeveFazerCorretamente()
        {
            var mockTitulo = new Mock<LinhaConteudoAjustarDTO>();
            var mockCodigo = new Mock<LinhaConteudoAjustarDTO>();
            var mockCredito = new Mock<LinhaConteudoAjustarDTO>();
            var mockLocalizacao = new Mock<LinhaConteudoAjustarDTO>();
            var mockProcedencia = new Mock<LinhaConteudoAjustarDTO>();
            var mockCopia = new Mock<LinhaConteudoAjustarDTO>();
            var mockPermiteUsoImagem = new Mock<LinhaConteudoAjustarDTO>();
            var mockEstadoConservacao = new Mock<LinhaConteudoAjustarDTO>();
            var mockDescricao = new Mock<LinhaConteudoAjustarDTO>();
            var mockSuporte = new Mock<LinhaConteudoAjustarDTO>();
            var mockDuracao = new Mock<LinhaConteudoAjustarDTO>();
            var mockCromia = new Mock<LinhaConteudoAjustarDTO>();
            var mockTamanhoArquivo = new Mock<LinhaConteudoAjustarDTO>();
            var mockAcessibilidade = new Mock<LinhaConteudoAjustarDTO>();
            var mockDisponibilizacao = new Mock<LinhaConteudoAjustarDTO>();
            var mockAno = new Mock<LinhaConteudoAjustarDTO>();

            var dto = new AcervoAudiovisualLinhaDTO
            {
                Status = ImportacaoStatus.Sucesso,
                Mensagem = "Sucesso na importação",
                NumeroLinha = 5,
                PossuiErros = false,
                Titulo = mockTitulo.Object,
                Codigo = mockCodigo.Object,
                Credito = mockCredito.Object,
                Localizacao = mockLocalizacao.Object,
                Procedencia = mockProcedencia.Object,
                Copia = mockCopia.Object,
                PermiteUsoImagem = mockPermiteUsoImagem.Object,
                EstadoConservacao = mockEstadoConservacao.Object,
                Descricao = mockDescricao.Object,
                Suporte = mockSuporte.Object,
                Duracao = mockDuracao.Object,
                Cromia = mockCromia.Object,
                TamanhoArquivo = mockTamanhoArquivo.Object,
                Acessibilidade = mockAcessibilidade.Object,
                Disponibilizacao = mockDisponibilizacao.Object,
                Ano = mockAno.Object
            };

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.Mensagem.Should().Be("Sucesso na importação");
            dto.NumeroLinha.Should().Be(5);
            dto.PossuiErros.Should().BeFalse();
            dto.Titulo.Should().Be(mockTitulo.Object);
            dto.Codigo.Should().Be(mockCodigo.Object);
            dto.Credito.Should().Be(mockCredito.Object);
            dto.Localizacao.Should().Be(mockLocalizacao.Object);
            dto.Procedencia.Should().Be(mockProcedencia.Object);
            dto.Copia.Should().Be(mockCopia.Object);
            dto.PermiteUsoImagem.Should().Be(mockPermiteUsoImagem.Object);
            dto.EstadoConservacao.Should().Be(mockEstadoConservacao.Object);
            dto.Descricao.Should().Be(mockDescricao.Object);
            dto.Suporte.Should().Be(mockSuporte.Object);
            dto.Duracao.Should().Be(mockDuracao.Object);
            dto.Cromia.Should().Be(mockCromia.Object);
            dto.TamanhoArquivo.Should().Be(mockTamanhoArquivo.Object);
            dto.Acessibilidade.Should().Be(mockAcessibilidade.Object);
            dto.Disponibilizacao.Should().Be(mockDisponibilizacao.Object);
            dto.Ano.Should().Be(mockAno.Object);
        }
       
        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Propriedades podem ser nulas")]
        public void DadoDTOAcervoAudiovisualLinhaComPropriedadesNulas_QuandoCriar_EntaoDeveAceitarNulos()
        {
            var dto = new AcervoAudiovisualLinhaDTO
            {
                Titulo = null,
                Codigo = null,
                Credito = null,
                Localizacao = null,
                Procedencia = null,
                Copia = null,
                PermiteUsoImagem = null,
                EstadoConservacao = null,
                Descricao = null,
                Suporte = null,
                Duracao = null,
                Cromia = null,
                TamanhoArquivo = null,
                Acessibilidade = null,
                Disponibilizacao = null,
                Ano = null
            };

            dto.Titulo.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.Credito.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.Copia.Should().BeNull();
            dto.PermiteUsoImagem.Should().BeNull();
            dto.EstadoConservacao.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.Suporte.Should().BeNull();
            dto.Duracao.Should().BeNull();
            dto.Cromia.Should().BeNull();
            dto.TamanhoArquivo.Should().BeNull();
            dto.Acessibilidade.Should().BeNull();
            dto.Disponibilizacao.Should().BeNull();
            dto.Ano.Should().BeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve aceitar status Pendente")]
        public void DadoDTOAcervoAudiovisualLinhaComStatusPendente_QuandoAtribuir_EntaoDeveAceitarPendente()
        {
            var dto = new AcervoAudiovisualLinhaDTO
            {
                Status = ImportacaoStatus.Pendente
            };

            dto.Status.Should().Be(ImportacaoStatus.Pendente);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve aceitar status Erros")]
        public void DadoDTOAcervoAudiovisualLinhaComStatusErros_QuandoAtribuir_EntaoDeveAceitarErros()
        {
            var dto = new AcervoAudiovisualLinhaDTO
            {
                Status = ImportacaoStatus.Erros
            };

            dto.Status.Should().Be(ImportacaoStatus.Erros);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve aceitar status Sucesso")]
        public void DadoDTOAcervoAudiovisualLinhaComStatusSucesso_QuandoAtribuir_EntaoDeveAceitarSucesso()
        {
            var dto = new AcervoAudiovisualLinhaDTO
            {
                Status = ImportacaoStatus.Sucesso
            };

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Mensagem pode ser nula")]
        public void DadoDTOAcervoAudiovisualLinhaComMensagemNula_QuandoAtribuir_EntaoDeveAceitarNulo()
        {
            var dto = new AcervoAudiovisualLinhaDTO
            {
                Mensagem = null
            };

            dto.Mensagem.Should().BeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Mensagem pode ser vazia")]
        public void DadoDTOAcervoAudiovisualLinhaComMensagemVazia_QuandoAtribuir_EntaoDeveAceitarVazio()
        {
            var dto = new AcervoAudiovisualLinhaDTO
            {
                Mensagem = string.Empty
            };

            dto.Mensagem.Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Mensagem com valor válido deve ser aceita")]
        public void DadoDTOAcervoAudiovisualLinhaComMensagemValida_QuandoAtribuir_EntaoDeveAceitarMensagem()
        {
            var mensagem = "Linha processada com sucesso";
            var dto = new AcervoAudiovisualLinhaDTO
            {
                Mensagem = mensagem
            };

            dto.Mensagem.Should().Be(mensagem);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - NumeroLinha com valor válido deve ser aceito")]
        public void DadoDTOAcervoAudiovisualLinhaComNumeroLinhaValido_QuandoAtribuir_EntaoDeveAceitarNumeroLinha()
        {
            var numeroLinha = 10;
            var dto = new AcervoAudiovisualLinhaDTO
            {
                NumeroLinha = numeroLinha
            };

            dto.NumeroLinha.Should().Be(numeroLinha);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - NumeroLinha pode ser zero")]
        public void DadoDTOAcervoAudiovisualLinhaComNumeroLinhaZero_QuandoAtribuir_EntaoDeveAceitarZero()
        {
            var dto = new AcervoAudiovisualLinhaDTO
            {
                NumeroLinha = 0
            };

            dto.NumeroLinha.Should().Be(0);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - NumeroLinha com valor máximo para int")]
        public void DadoDTOAcervoAudiovisualLinhaComNumeroLinhaMaximo_QuandoAtribuir_EntaoDeveAceitarMaximo()
        {
            var dto = new AcervoAudiovisualLinhaDTO
            {
                NumeroLinha = int.MaxValue
            };

            dto.NumeroLinha.Should().Be(int.MaxValue);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - PossuiErros com true deve ser aceito")]
        public void DadoDTOAcervoAudiovisualLinhaComPossuiErrosTrue_QuandoAtribuir_EntaoDeveAceitarTrue()
        {
            var dto = new AcervoAudiovisualLinhaDTO
            {
                PossuiErros = true
            };

            dto.PossuiErros.Should().BeTrue();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - PossuiErros com false deve ser aceito")]
        public void DadoDTOAcervoAudiovisualLinhaComPossuiErrosFalse_QuandoAtribuir_EntaoDeveAceitarFalse()
        {
            var dto = new AcervoAudiovisualLinhaDTO
            {
                PossuiErros = false
            };

            dto.PossuiErros.Should().BeFalse();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Tipo deve ser classe e não interface")]
        public void DadoTipoDTOAcervoAudiovisualLinha_QuandoVerificar_EntaoDeveSerClasse()
        {
            var tipo = typeof(AcervoAudiovisualLinhaDTO);

            tipo.IsClass.Should().BeTrue();
            tipo.IsInterface.Should().BeFalse();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve poder ser instanciado sem parâmetros")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoInstanciarSemParametros_EntaoDeveSerBemSucedido()
        {
            var dto = new AcervoAudiovisualLinhaDTO();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<AcervoAudiovisualLinhaDTO>();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Instâncias diferentes devem ser independentes")]
        public void DadoDuasInstanciasDTOAcervoAudiovisualLinha_QuandoModificarUma_EntaoOutraNaoDeveSerAfetada()
        {
            var dto1 = new AcervoAudiovisualLinhaDTO
            {
                NumeroLinha = 1,
                PossuiErros = true,
                Mensagem = "Erro 1"
            };

            var dto2 = new AcervoAudiovisualLinhaDTO
            {
                NumeroLinha = 2,
                PossuiErros = false,
                Mensagem = "Sucesso"
            };

            dto1.NumeroLinha.Should().NotBe(dto2.NumeroLinha);
            dto1.PossuiErros.Should().NotBe(dto2.PossuiErros);
            dto1.Mensagem.Should().NotBe(dto2.Mensagem);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve suportar inicialização com object initializer")]
        public void DadoDTOAcervoAudiovisualLinhaComObjectInitializer_QuandoCriar_EntaoDeveInicializarCorretamente()
        {
            var mockTitulo = new Mock<LinhaConteudoAjustarDTO>();
            var mockCodigo = new Mock<LinhaConteudoAjustarDTO>();

            var dto = new AcervoAudiovisualLinhaDTO
            {
                NumeroLinha = 5,
                PossuiErros = false,
                Mensagem = "Sucesso",
                Status = ImportacaoStatus.Sucesso,
                Titulo = mockTitulo.Object,
                Codigo = mockCodigo.Object
            };

            dto.Should().NotBeNull();
            dto.NumeroLinha.Should().Be(5);
            dto.PossuiErros.Should().BeFalse();
            dto.Mensagem.Should().Be("Sucesso");
            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.Titulo.Should().Be(mockTitulo.Object);
            dto.Codigo.Should().Be(mockCodigo.Object);
        }
       
        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Deve herdar corretamente de AcervoLinhaDTO")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoVerificarHeranca_EntaoDeveHerdarCorretamente()
        {
            var tipo = typeof(AcervoAudiovisualLinhaDTO);
            var baseType = tipo.BaseType;

            baseType.Should().Be(typeof(AcervoLinhaDTO));
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Propriedades podem ser reconfiguradas após inicialização")]
        public void DadoDTOAcervoAudiovisualLinhaAposCriacao_QuandoReconfigurarPropriedades_EntaoDeveSobreescreverValoresAnteriores()
        {
            var mockTitulo1 = new Mock<LinhaConteudoAjustarDTO>();
            var mockTitulo2 = new Mock<LinhaConteudoAjustarDTO>();

            var dto = new AcervoAudiovisualLinhaDTO
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

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Validação completa com todos os campos preenchidos")]
        public void DadoDTOAcervoAudiovisualLinhaComTodosCamposPreenchidos_QuandoValidar_EntaoDeveSerValido()
        {
            var mockTitulo = new Mock<LinhaConteudoAjustarDTO>();
            var mockCodigo = new Mock<LinhaConteudoAjustarDTO>();
            var mockCredito = new Mock<LinhaConteudoAjustarDTO>();
            var mockLocalizacao = new Mock<LinhaConteudoAjustarDTO>();
            var mockProcedencia = new Mock<LinhaConteudoAjustarDTO>();
            var mockCopia = new Mock<LinhaConteudoAjustarDTO>();
            var mockPermiteUsoImagem = new Mock<LinhaConteudoAjustarDTO>();
            var mockEstadoConservacao = new Mock<LinhaConteudoAjustarDTO>();
            var mockDescricao = new Mock<LinhaConteudoAjustarDTO>();
            var mockSuporte = new Mock<LinhaConteudoAjustarDTO>();
            var mockDuracao = new Mock<LinhaConteudoAjustarDTO>();
            var mockCromia = new Mock<LinhaConteudoAjustarDTO>();
            var mockTamanhoArquivo = new Mock<LinhaConteudoAjustarDTO>();
            var mockAcessibilidade = new Mock<LinhaConteudoAjustarDTO>();
            var mockDisponibilizacao = new Mock<LinhaConteudoAjustarDTO>();
            var mockAno = new Mock<LinhaConteudoAjustarDTO>();

            var dto = new AcervoAudiovisualLinhaDTO
            {
                Status = ImportacaoStatus.Sucesso,
                Mensagem = "Sucesso na importação",
                NumeroLinha = 10,
                PossuiErros = false,
                Titulo = mockTitulo.Object,
                Codigo = mockCodigo.Object,
                Credito = mockCredito.Object,
                Localizacao = mockLocalizacao.Object,
                Procedencia = mockProcedencia.Object,
                Copia = mockCopia.Object,
                PermiteUsoImagem = mockPermiteUsoImagem.Object,
                EstadoConservacao = mockEstadoConservacao.Object,
                Descricao = mockDescricao.Object,
                Suporte = mockSuporte.Object,
                Duracao = mockDuracao.Object,
                Cromia = mockCromia.Object,
                TamanhoArquivo = mockTamanhoArquivo.Object,
                Acessibilidade = mockAcessibilidade.Object,
                Disponibilizacao = mockDisponibilizacao.Object,
                Ano = mockAno.Object
            };

            dto.Should().NotBeNull();
            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.PossuiErros.Should().BeFalse();
            dto.NumeroLinha.Should().Be(10);
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Método DefinirLinhaComoSucesso é público")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoVerificarMetodo_EntaoDefinirLinhaComoSucessoDeveSerPublico()
        {
            var tipo = typeof(AcervoAudiovisualLinhaDTO);
            var metodo = tipo.GetMethod("DefinirLinhaComoSucesso");

            metodo.Should().NotBeNull();
            metodo?.IsPublic.Should().BeTrue();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Método DefinirLinhaComoSucesso não deve retornar valor")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoVerificarAssinaturaDeFuncao_EntaoDefinirLinhaComoSucessoDeveRetornarVoid()
        {
            var tipo = typeof(AcervoAudiovisualLinhaDTO);
            var metodo = tipo.GetMethod("DefinirLinhaComoSucesso");

            metodo?.ReturnType.Should().Be(typeof(void));
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Método DefinirLinhaComoSucesso não deve ter parâmetros")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoVerificarAssinaturaDeFuncao_EntaoDefinirLinhaComoSucessoNaoDeveConterParametros()
        {
            var tipo = typeof(AcervoAudiovisualLinhaDTO);
            var metodo = tipo.GetMethod("DefinirLinhaComoSucesso");

            metodo?.GetParameters().Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualLinhaDTO - Propriedades herdadas devem ter visibilidade pública")]
        public void DadoDTOAcervoAudiovisualLinha_QuandoVerificarVisibilidadePropriedadesHerdadas_EntaoDeveSerPublicas()
        {
            var tipo = typeof(AcervoAudiovisualLinhaDTO);

            var statusProperty = tipo.GetProperty("Status");
            var mensagemProperty = tipo.GetProperty("Mensagem");
            var numeroLinhaProperty = tipo.GetProperty("NumeroLinha");
            var possuiErrosProperty = tipo.GetProperty("PossuiErros");

            statusProperty?.CanRead.Should().BeTrue();
            statusProperty?.CanWrite.Should().BeTrue();
            mensagemProperty?.CanRead.Should().BeTrue();
            mensagemProperty?.CanWrite.Should().BeTrue();
            numeroLinhaProperty?.CanRead.Should().BeTrue();
            numeroLinhaProperty?.CanWrite.Should().BeTrue();
            possuiErrosProperty?.CanRead.Should().BeTrue();
            possuiErrosProperty?.CanWrite.Should().BeTrue();
        }
    }
}
