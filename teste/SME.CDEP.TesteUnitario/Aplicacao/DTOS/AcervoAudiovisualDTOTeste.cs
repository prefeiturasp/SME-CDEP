using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoAudiovisualDTOTeste
    {
        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade Id")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeId()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("Id").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade AcervoId")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeAcervoId()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("AcervoId").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade Titulo")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeTitulo()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("Titulo").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade TipoAcervoId")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeTipoAcervoId()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("TipoAcervoId").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade Codigo")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeCodigo()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("Codigo").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade Localizacao")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeLocalizacao()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("Localizacao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade Procedencia")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeProcedencia()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("Procedencia").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade Copia")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeCopia()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("Copia").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade PermiteUsoImagem")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadePermiteUsoImagem()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("PermiteUsoImagem").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade ConservacaoId")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeConservacaoId()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("ConservacaoId").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade Descricao")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeDescricao()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("Descricao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade SuporteId")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeSuporteId()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("SuporteId").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade Duracao")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeDuracao()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("Duracao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade CromiaId")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeCromiaId()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("CromiaId").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade TamanhoArquivo")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeTamanhoArquivo()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("TamanhoArquivo").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade Acessibilidade")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeAcessibilidade()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("Acessibilidade").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade Disponibilizacao")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeDisponibilizacao()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("Disponibilizacao").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade Auditoria")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeAuditoria()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("Auditoria").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade CreditosAutoresIds")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeCreditosAutoresIds()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("CreditosAutoresIds").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade Ano")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeAno()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("Ano").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve conter propriedade SituacaoAcervo")]
        public void DadoDTOAcervoAudiovisual_QuandoCriar_EntaoContemPropriedadeSituacaoAcervo()
        {
            var dto = new AcervoAudiovisualDTO();

            typeof(AcervoAudiovisualDTO).GetProperty("SituacaoAcervo").Should().NotBeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve permitir atribuição de valores às propriedades")]
        public void DadoDTOAcervoAudiovisual_QuandoAtribuirValores_EntaoDeveFazerCorretamente()
        {
            var id = 1L;
            var acervoId = 100L;
            var titulo = "Título do Audiovisual";
            var tipoAcervoId = 5L;
            var codigo = "AUD001";
            var localizacao = "Prateleira A";
            var procedencia = "Doação";
            var copia = "Digital";
            var permiteUsoImagem = true;
            var conservacaoId = 2L;
            var descricao = "Descrição do audiovisual";
            var suporteId = 3L;
            var duracao = "02:30:00";
            var cromiaId = 1L;
            var tamanhoArquivo = "1.5GB";
            var acessibilidade = "Legendado";
            var disponibilizacao = "Público";
            var creditosAutoresIds = new long[] { 10, 20, 30 };
            var ano = "2024";
            var situacaoAcervo = SituacaoAcervo.Ativo;

            var auditoria = new AuditoriaDTO
            {
                CriadoEm = DateTime.Now,
                CriadoPor = "Usuário",
                CriadoLogin = "usuario@email.com"
            };

            var dto = new AcervoAudiovisualDTO
            {
                Id = id,
                AcervoId = acervoId,
                Titulo = titulo,
                TipoAcervoId = tipoAcervoId,
                Codigo = codigo,
                Localizacao = localizacao,
                Procedencia = procedencia,
                Copia = copia,
                PermiteUsoImagem = permiteUsoImagem,
                ConservacaoId = conservacaoId,
                Descricao = descricao,
                SuporteId = suporteId,
                Duracao = duracao,
                CromiaId = cromiaId,
                TamanhoArquivo = tamanhoArquivo,
                Acessibilidade = acessibilidade,
                Disponibilizacao = disponibilizacao,
                Auditoria = auditoria,
                CreditosAutoresIds = creditosAutoresIds,
                Ano = ano,
                SituacaoAcervo = situacaoAcervo
            };

            dto.Id.Should().Be(id);
            dto.AcervoId.Should().Be(acervoId);
            dto.Titulo.Should().Be(titulo);
            dto.TipoAcervoId.Should().Be(tipoAcervoId);
            dto.Codigo.Should().Be(codigo);
            dto.Localizacao.Should().Be(localizacao);
            dto.Procedencia.Should().Be(procedencia);
            dto.Copia.Should().Be(copia);
            dto.PermiteUsoImagem.Should().Be(permiteUsoImagem);
            dto.ConservacaoId.Should().Be(conservacaoId);
            dto.Descricao.Should().Be(descricao);
            dto.SuporteId.Should().Be(suporteId);
            dto.Duracao.Should().Be(duracao);
            dto.CromiaId.Should().Be(cromiaId);
            dto.TamanhoArquivo.Should().Be(tamanhoArquivo);
            dto.Acessibilidade.Should().Be(acessibilidade);
            dto.Disponibilizacao.Should().Be(disponibilizacao);
            dto.Auditoria.Should().NotBeNull();
            dto.CreditosAutoresIds.Should().Equal(creditosAutoresIds);
            dto.Ano.Should().Be(ano);
            dto.SituacaoAcervo.Should().Be(situacaoAcervo);
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve permitir valores nulos para propriedades nullable")]
        public void DadoDTOAcervoAudiovisualComValoresNulos_QuandoCriar_EntaoDeveAceitarNulos()
        {
            var dto = new AcervoAudiovisualDTO
            {
                Id = 1L,
                AcervoId = 100L,
                Titulo = "Título",
                TipoAcervoId = 5L,
                Codigo = "AUD001",
                Localizacao = null,
                Procedencia = null,
                Copia = null,
                PermiteUsoImagem = null,
                ConservacaoId = null,
                Descricao = "Descrição",
                SuporteId = null,
                Duracao = "02:30:00",
                CromiaId = null,
                TamanhoArquivo = null,
                Acessibilidade = null,
                Disponibilizacao = null,
                Ano = null
            };

            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.Copia.Should().BeNull();
            dto.PermiteUsoImagem.Should().BeNull();
            dto.ConservacaoId.Should().BeNull();
            dto.SuporteId.Should().BeNull();
            dto.CromiaId.Should().BeNull();
            dto.TamanhoArquivo.Should().BeNull();
            dto.Acessibilidade.Should().BeNull();
            dto.Disponibilizacao.Should().BeNull();
            dto.Ano.Should().BeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve permitir valores máximos para long")]
        public void DadoDTOAcervoAudiovisualComValoresMaximos_QuandoAtribuir_EntaoDeveAceitarSemErro()
        {
            var dto = new AcervoAudiovisualDTO
            {
                Id = long.MaxValue,
                AcervoId = long.MaxValue,
                TipoAcervoId = long.MaxValue,
                SuporteId = long.MaxValue,
                ConservacaoId = long.MaxValue,
                CromiaId = long.MaxValue
            };

            dto.Id.Should().Be(long.MaxValue);
            dto.AcervoId.Should().Be(long.MaxValue);
            dto.TipoAcervoId.Should().Be(long.MaxValue);
            dto.SuporteId.Should().Be(long.MaxValue);
            dto.ConservacaoId.Should().Be(long.MaxValue);
            dto.CromiaId.Should().Be(long.MaxValue);
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve permitir array vazio de créditos autores")]
        public void DadoDTOAcervoAudiovisualComArrayVazioDeCreditosAutores_QuandoCriar_EntaoDeveAceitarArrayVazio()
        {
            var creditosAutoresIds = Array.Empty<long>();

            var dto = new AcervoAudiovisualDTO
            {
                CreditosAutoresIds = creditosAutoresIds
            };

            dto.CreditosAutoresIds.Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve permitir array com múltiplos créditos autores")]
        public void DadoDTOAcervoAudiovisualComMultiplosCreditosAutores_QuandoCriar_EntaoDeveAceitarMultiplosIds()
        {
            var creditosAutoresIds = new long[] { 1L, 2L, 3L, 4L, 5L };

            var dto = new AcervoAudiovisualDTO
            {
                CreditosAutoresIds = creditosAutoresIds
            };

            dto.CreditosAutoresIds.Should().HaveCount(5);
            dto.CreditosAutoresIds.Should().Equal(creditosAutoresIds);
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - SituacaoAcervo deve aceitar valor Ativo")]
        public void DadoDTOAcervoAudiovisualComSituacaoAtivo_QuandoAtribuir_EntaoDeveAceitarAtivo()
        {
            var dto = new AcervoAudiovisualDTO
            {
                SituacaoAcervo = SituacaoAcervo.Ativo
            };

            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Ativo);
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - SituacaoAcervo deve aceitar valor Inativo")]
        public void DadoDTOAcervoAudiovisualComSituacaoInativo_QuandoAtribuir_EntaoDeveAceitarInativo()
        {
            var dto = new AcervoAudiovisualDTO
            {
                SituacaoAcervo = SituacaoAcervo.Inativo
            };

            dto.SituacaoAcervo.Should().Be(SituacaoAcervo.Inativo);
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve permitir boolean true para PermiteUsoImagem")]
        public void DadoDTOAcervoAudiovisualComPermiteUsoImagemTrue_QuandoAtribuir_EntaoDeveAceitarTrue()
        {
            var dto = new AcervoAudiovisualDTO
            {
                PermiteUsoImagem = true
            };

            dto.PermiteUsoImagem.Should().Be(true);
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve permitir boolean false para PermiteUsoImagem")]
        public void DadoDTOAcervoAudiovisualComPermiteUsoImagemFalse_QuandoAtribuir_EntaoDeveAceitarFalse()
        {
            var dto = new AcervoAudiovisualDTO
            {
                PermiteUsoImagem = false
            };

            dto.PermiteUsoImagem.Should().Be(false);
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve permitir strings vazias")]
        public void DadoDTOAcervoAudiovisualComStringsVazias_QuandoCriar_EntaoDeveAceitarStringsVazias()
        {
            var dto = new AcervoAudiovisualDTO
            {
                Titulo = string.Empty,
                Codigo = string.Empty,
                Descricao = string.Empty,
                Duracao = string.Empty,
                Localizacao = string.Empty,
                Procedencia = string.Empty,
                Copia = string.Empty,
                TamanhoArquivo = string.Empty,
                Acessibilidade = string.Empty,
                Disponibilizacao = string.Empty,
                Ano = string.Empty
            };

            dto.Titulo.Should().BeEmpty();
            dto.Codigo.Should().BeEmpty();
            dto.Descricao.Should().BeEmpty();
            dto.Duracao.Should().BeEmpty();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Propriedades devem ser públicas")]
        public void DadoDTOAcervoAudiovisual_QuandoVerificarVisibilidadePropriedades_EntaoDeveSerPublicas()
        {
            var tipo = typeof(AcervoAudiovisualDTO);

            var idProperty = tipo.GetProperty("Id");
            var acervoIdProperty = tipo.GetProperty("AcervoId");
            var tituloProperty = tipo.GetProperty("Titulo");
            var tipoAcervoIdProperty = tipo.GetProperty("TipoAcervoId");
            var codigoProperty = tipo.GetProperty("Codigo");
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
            var auditoriaProperty = tipo.GetProperty("Auditoria");
            var creditosAutoresIdsProperty = tipo.GetProperty("CreditosAutoresIds");
            var anoProperty = tipo.GetProperty("Ano");
            var situacaoAcervoProperty = tipo.GetProperty("SituacaoAcervo");

            idProperty?.CanRead.Should().BeTrue();
            idProperty?.CanWrite.Should().BeTrue();
            acervoIdProperty?.CanRead.Should().BeTrue();
            acervoIdProperty?.CanWrite.Should().BeTrue();
            tituloProperty?.CanRead.Should().BeTrue();
            tituloProperty?.CanWrite.Should().BeTrue();
            tipoAcervoIdProperty?.CanRead.Should().BeTrue();
            tipoAcervoIdProperty?.CanWrite.Should().BeTrue();
            codigoProperty?.CanRead.Should().BeTrue();
            codigoProperty?.CanWrite.Should().BeTrue();
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
            auditoriaProperty?.CanRead.Should().BeTrue();
            auditoriaProperty?.CanWrite.Should().BeTrue();
            creditosAutoresIdsProperty?.CanRead.Should().BeTrue();
            creditosAutoresIdsProperty?.CanWrite.Should().BeTrue();
            anoProperty?.CanRead.Should().BeTrue();
            anoProperty?.CanWrite.Should().BeTrue();
            situacaoAcervoProperty?.CanRead.Should().BeTrue();
            situacaoAcervoProperty?.CanWrite.Should().BeTrue();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Instâncias diferentes devem ser independentes")]
        public void DadoDuasInstanciasDTOAcervoAudiovisual_QuandoModificarUma_EntaoOutraNaoDeveSerAfetada()
        {
            var dto1 = new AcervoAudiovisualDTO
            {
                Id = 1L,
                Titulo = "Audiovisual 1",
                Descricao = "Descrição 1"
            };

            var dto2 = new AcervoAudiovisualDTO
            {
                Id = 2L,
                Titulo = "Audiovisual 2",
                Descricao = "Descrição 2"
            };

            dto1.Id.Should().NotBe(dto2.Id);
            dto1.Titulo.Should().NotBe(dto2.Titulo);
            dto1.Descricao.Should().NotBe(dto2.Descricao);
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve suportar inicialização com object initializer")]
        public void DadoDTOAcervoAudiovisualComObjectInitializer_QuandoCriar_EntaoDeveInicializarCorretamente()
        {
            var dto = new AcervoAudiovisualDTO
            {
                Id = 100L,
                AcervoId = 50L,
                Titulo = "Título",
                TipoAcervoId = 3L,
                Codigo = "AUD123",
                Duracao = "01:30:00",
                Descricao = "Descrição completa"
            };

            dto.Should().NotBeNull();
            dto.Id.Should().Be(100L);
            dto.AcervoId.Should().Be(50L);
            dto.Titulo.Should().Be("Título");
            dto.TipoAcervoId.Should().Be(3L);
            dto.Codigo.Should().Be("AUD123");
            dto.Duracao.Should().Be("01:30:00");
            dto.Descricao.Should().Be("Descrição completa");
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Validação completa com todos os campos preenchidos")]
        public void DadoDTOAcervoAudiovisualComTodosCamposPreenchidos_QuandoValidar_EntaoDeveSerValido()
        {
            var auditoria = new AuditoriaDTO
            {
                CriadoEm = DateTime.UtcNow,
                CriadoPor = "Usuário Teste",
                CriadoLogin = "teste@email.com",
                AlteradoEm = DateTime.UtcNow,
                AlteradoPor = "Usuário Alteração",
                AlteradoLogin = "alteracao@email.com"
            };

            var dto = new AcervoAudiovisualDTO
            {
                Id = 1L,
                AcervoId = 100L,
                Titulo = "Audiovisual de Teste",
                TipoAcervoId = 5L,
                Codigo = "AUD-001-2024",
                Localizacao = "Prateleira A-01",
                Procedencia = "Doação de particular",
                Copia = "Cópia digital de alta qualidade",
                PermiteUsoImagem = true,
                ConservacaoId = 2L,
                Descricao = "Descrição detalhada do audiovisual",
                SuporteId = 3L,
                Duracao = "02:45:30",
                CromiaId = 1L,
                TamanhoArquivo = "2.5GB",
                Acessibilidade = "Legendado em português",
                Disponibilizacao = "Disponível para pesquisa",
                Auditoria = auditoria,
                CreditosAutoresIds = new long[] { 10, 20, 30 },
                Ano = "2024",
                SituacaoAcervo = SituacaoAcervo.Ativo
            };

            dto.Should().NotBeNull();
            dto.Auditoria.Should().NotBeNull();
            dto.Auditoria.CriadoEm.Should().NotBe(default);
            dto.Auditoria.CriadoPor.Should().Be("Usuário Teste");
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Tipo deve ser classe e não interface")]
        public void DadoTipoDTOAcervoAudiovisual_QuandoVerificar_EntaoDeveSerClasse()
        {
            var tipo = typeof(AcervoAudiovisualDTO);

            tipo.IsClass.Should().BeTrue();
            tipo.IsInterface.Should().BeFalse();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve poder ser instanciado sem parâmetros")]
        public void DadoDTOAcervoAudiovisual_QuandoInstanciarSemParametros_EntaoDeveSerBemSucedido()
        {
            var dto = new AcervoAudiovisualDTO();

            dto.Should().NotBeNull();
            dto.Should().BeOfType<AcervoAudiovisualDTO>();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Propriedades herdadas de tipos de valor devem ter valores padrão")]
        public void DadoDTOAcervoAudiovisualNovaInstancia_QuandoVerificarValoresPadrao_EntaoIntSimuladoDeveSerZero()
        {
            var dto = new AcervoAudiovisualDTO();

            dto.Id.Should().Be(0L);
            dto.AcervoId.Should().Be(0L);
            dto.TipoAcervoId.Should().Be(0L);
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Atribuição de valores deve ser independente entre instâncias")]
        public void DadoDuasInstancisDTOAcervoAudiovisual_QuandoAlterarPropriedades_EntaoDeveSerIndependentes()
        {
            var dto1 = new AcervoAudiovisualDTO();
            var dto2 = new AcervoAudiovisualDTO();

            dto1.Titulo = "Audiovisual 1";
            dto2.Titulo = "Audiovisual 2";

            dto1.Titulo.Should().NotBe(dto2.Titulo);
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve permitir atribuição de auditoria com valor nulo")]
        public void DadoDTOAcervoAudiovisualComAuditoriaNula_QuandoAtribuir_EntaoDeveAceitarNulo()
        {
            var dto = new AcervoAudiovisualDTO
            {
                Auditoria = null
            };

            dto.Auditoria.Should().BeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve permitir atribuição de créditos autores como null")]
        public void DadoDTOAcervoAudiovisualComCreditosAutoresNulo_QuandoAtribuir_EntaoDeveAceitarNulo()
        {
            var dto = new AcervoAudiovisualDTO
            {
                CreditosAutoresIds = null
            };

            dto.CreditosAutoresIds.Should().BeNull();
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Propriedades de string devem ter capacidade de armazenar textos longos")]
        public void DadoDTOAcervoAudiovisualComTextoLongo_QuandoAtribuir_EntaoDeveArmazenarCorretamente()
        {
            var textolongo = new string('A', 1000);

            var dto = new AcervoAudiovisualDTO
            {
                Titulo = textolongo,
                Descricao = textolongo
            };

            dto.Titulo.Should().HaveLength(1000);
            dto.Descricao.Should().HaveLength(1000);
        }

        [Fact(DisplayName = "AcervoAudiovisualDTO - Deve permitir reconfiguração de propriedades após inicialização")]
        public void DadoDTOAcervoAudiovisualAposCriacao_QuandoReconfigurarPropriedades_EntaoDeveSobreescreverValoresAnteriores()
        {
            var dto = new AcervoAudiovisualDTO
            {
                Id = 1L,
                Titulo = "Título Inicial"
            };

            dto.Id = 2L;
            dto.Titulo = "Título Alterado";

            dto.Id.Should().Be(2L);
            dto.Titulo.Should().Be("Título Alterado");
        }
    }
}
