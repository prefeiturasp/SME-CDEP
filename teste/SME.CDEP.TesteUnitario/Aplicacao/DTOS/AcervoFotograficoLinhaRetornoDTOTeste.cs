using Bogus;
using FluentAssertions;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.TesteUnitario.Aplicacao.DTOS
{
    public class AcervoFotograficoLinhaRetornoDtoTeste
    {
        #region Titulo

        [Fact]
        public void DadoTituloVazio_QuandoCriarDTO_EntaoTituloDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.Titulo.Should().BeNull();
        }

        [Fact]
        public void DadoTitulo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Título Teste" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Titulo = linhaConteudo };

            dto.Titulo.Should().NotBeNull();
            dto.Titulo.Conteudo.Should().Be("Título Teste");
        }

        [Fact]
        public void DadoTituloComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "Título",
                PossuiErro = true,
                Mensagem = "Campo obrigatório"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Titulo = linhaConteudo };

            dto.Titulo.PossuiErro.Should().BeTrue();
            dto.Titulo.Mensagem.Should().Be("Campo obrigatório");
        }

        #endregion

        #region Codigo

        [Fact]
        public void DadoCodigoVazio_QuandoCriarDTO_EntaoCodigoDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.Codigo.Should().BeNull();
        }

        [Fact]
        public void DadoCodigo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "COD001" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Codigo = linhaConteudo };

            dto.Codigo.Should().NotBeNull();
            dto.Codigo.Conteudo.Should().Be("COD001");
        }

        [Fact]
        public void DadoCodigoComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "COD",
                PossuiErro = true,
                Mensagem = "Código inválido"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Codigo = linhaConteudo };

            dto.Codigo.PossuiErro.Should().BeTrue();
            dto.Codigo.Mensagem.Should().Be("Código inválido");
        }

        #endregion

        #region CreditosAutoresIds

        [Fact]
        public void DadoCreditosAutoresIdsVazio_QuandoCriarDTO_EntaoCreditosAutoresIdsDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.CreditosAutoresIds.Should().BeNull();
        }

        [Fact]
        public void DadoCreditosAutoresIds_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1,2,3" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { CreditosAutoresIds = linhaConteudo };

            dto.CreditosAutoresIds.Should().NotBeNull();
            dto.CreditosAutoresIds.Conteudo.Should().Be("1,2,3");
        }

        [Fact]
        public void DadoCreditosAutoresIdsComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "1",
                PossuiErro = true,
                Mensagem = "Crédito inválido"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { CreditosAutoresIds = linhaConteudo };

            dto.CreditosAutoresIds.PossuiErro.Should().BeTrue();
        }

        #endregion

        #region Localizacao

        [Fact]
        public void DadoLocalizacaoVazia_QuandoCriarDTO_EntaoLocalizacaoDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.Localizacao.Should().BeNull();
        }

        [Fact]
        public void DadoLocalizacao_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Sala 1 - Prateleira 5" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Localizacao = linhaConteudo };

            dto.Localizacao.Should().NotBeNull();
            dto.Localizacao.Conteudo.Should().Be("Sala 1 - Prateleira 5");
        }

        [Fact]
        public void DadoLocalizacaoComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "Sala",
                PossuiErro = true,
                Mensagem = "Localização inválida"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Localizacao = linhaConteudo };

            dto.Localizacao.PossuiErro.Should().BeTrue();
        }

        #endregion

        #region Procedencia

        [Fact]
        public void DadoProcedenciaVazia_QuandoCriarDTO_EntaoProcedenciaDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.Procedencia.Should().BeNull();
        }

        [Fact]
        public void DadoProcedencia_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Doação" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Procedencia = linhaConteudo };

            dto.Procedencia.Should().NotBeNull();
            dto.Procedencia.Conteudo.Should().Be("Doação");
        }

        [Fact]
        public void DadoProcedenciaComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "Proc",
                PossuiErro = true,
                Mensagem = "Procedência inválida"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Procedencia = linhaConteudo };

            dto.Procedencia.PossuiErro.Should().BeTrue();
        }

        #endregion

        #region DataAcervo

        [Fact]
        public void DadoDataAcervoVazia_QuandoCriarDTO_EntaoDataAcervoDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.DataAcervo.Should().BeNull();
        }

        [Fact]
        public void DadoDataAcervo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var data = new Faker().Date.Past().ToString("dd/MM/yyyy");
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = data };
            var dto = new AcervoFotograficoLinhaRetornoDTO { DataAcervo = linhaConteudo };

            dto.DataAcervo.Should().NotBeNull();
            dto.DataAcervo.Conteudo.Should().Be(data);
        }

        [Fact]
        public void DadoDataAcervoComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "2020",
                PossuiErro = true,
                Mensagem = "Data inválida"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { DataAcervo = linhaConteudo };

            dto.DataAcervo.PossuiErro.Should().BeTrue();
        }

        #endregion

        #region CopiaDigital

        [Fact]
        public void DadoCopiaDigitalVazia_QuandoCriarDTO_EntaoCopiaDigitalDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.CopiaDigital.Should().BeNull();
        }

        [Fact]
        public void DadoCopiaDigital_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Sim" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { CopiaDigital = linhaConteudo };

            dto.CopiaDigital.Should().NotBeNull();
            dto.CopiaDigital.Conteudo.Should().Be("Sim");
        }

        [Fact]
        public void DadoCopiaDigitalComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "Não",
                PossuiErro = true,
                Mensagem = "Cópia digital inválida"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { CopiaDigital = linhaConteudo };

            dto.CopiaDigital.PossuiErro.Should().BeTrue();
        }

        #endregion

        #region PermiteUsoImagem

        [Fact]
        public void DadoPermiteUsoImagemVazia_QuandoCriarDTO_EntaoPermiteUsoImagemDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.PermiteUsoImagem.Should().BeNull();
        }

        [Fact]
        public void DadoPermiteUsoImagem_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Não" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { PermiteUsoImagem = linhaConteudo };

            dto.PermiteUsoImagem.Should().NotBeNull();
            dto.PermiteUsoImagem.Conteudo.Should().Be("Não");
        }

        [Fact]
        public void DadoPermiteUsoImagemComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "Sim",
                PossuiErro = true,
                Mensagem = "Permissão inválida"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { PermiteUsoImagem = linhaConteudo };

            dto.PermiteUsoImagem.PossuiErro.Should().BeTrue();
        }

        #endregion

        #region ConservacaoId

        [Fact]
        public void DadoConservacaoIdVazio_QuandoCriarDTO_EntaoConservacaoIdDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.ConservacaoId.Should().BeNull();
        }

        [Fact]
        public void DadoConservacaoId_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { ConservacaoId = linhaConteudo };

            dto.ConservacaoId.Should().NotBeNull();
            dto.ConservacaoId.Conteudo.Should().Be("1");
        }

        [Fact]
        public void DadoConservacaoIdComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "999",
                PossuiErro = true,
                Mensagem = "Conservação não encontrada"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { ConservacaoId = linhaConteudo };

            dto.ConservacaoId.PossuiErro.Should().BeTrue();
        }

        #endregion

        #region Descricao

        [Fact]
        public void DadoDescricaoVazia_QuandoCriarDTO_EntaoDescricaoDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.Descricao.Should().BeNull();
        }

        [Fact]
        public void DadoDescricao_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Descrição da foto" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Descricao = linhaConteudo };

            dto.Descricao.Should().NotBeNull();
            dto.Descricao.Conteudo.Should().Be("Descrição da foto");
        }

        [Fact]
        public void DadoDescricaoComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "Desc",
                PossuiErro = true,
                Mensagem = "Descrição incompleta"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Descricao = linhaConteudo };

            dto.Descricao.PossuiErro.Should().BeTrue();
        }

        #endregion

        #region Quantidade

        [Fact]
        public void DadoQuantidadeVazia_QuandoCriarDTO_EntaoQuantidadeDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.Quantidade.Should().BeNull();
        }

        [Fact]
        public void DadoQuantidade_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "10" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Quantidade = linhaConteudo };

            dto.Quantidade.Should().NotBeNull();
            dto.Quantidade.Conteudo.Should().Be("10");
        }

        [Fact]
        public void DadoQuantidadeComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "0",
                PossuiErro = true,
                Mensagem = "Quantidade inválida"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Quantidade = linhaConteudo };

            dto.Quantidade.PossuiErro.Should().BeTrue();
        }

        #endregion

        #region Largura

        [Fact]
        public void DadoLarguraVazia_QuandoCriarDTO_EntaoLarguraDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.Largura.Should().BeNull();
        }

        [Fact]
        public void DadoLargura_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "20cm" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Largura = linhaConteudo };

            dto.Largura.Should().NotBeNull();
            dto.Largura.Conteudo.Should().Be("20cm");
        }

        [Fact]
        public void DadoLarguraComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "20",
                PossuiErro = true,
                Mensagem = "Largura inválida"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Largura = linhaConteudo };

            dto.Largura.PossuiErro.Should().BeTrue();
        }

        #endregion

        #region Altura

        [Fact]
        public void DadoAlturaVazia_QuandoCriarDTO_EntaoAlturaDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.Altura.Should().BeNull();
        }

        [Fact]
        public void DadoAltura_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "30cm" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Altura = linhaConteudo };

            dto.Altura.Should().NotBeNull();
            dto.Altura.Conteudo.Should().Be("30cm");
        }

        [Fact]
        public void DadoAlturaComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "30",
                PossuiErro = true,
                Mensagem = "Altura inválida"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Altura = linhaConteudo };

            dto.Altura.PossuiErro.Should().BeTrue();
        }

        #endregion

        #region SuporteId

        [Fact]
        public void DadoSuporteIdVazio_QuandoCriarDTO_EntaoSuporteIdDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.SuporteId.Should().BeNull();
        }

        [Fact]
        public void DadoSuporteId_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "2" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { SuporteId = linhaConteudo };

            dto.SuporteId.Should().NotBeNull();
            dto.SuporteId.Conteudo.Should().Be("2");
        }

        [Fact]
        public void DadoSuporteIdComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "999",
                PossuiErro = true,
                Mensagem = "Suporte não encontrado"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { SuporteId = linhaConteudo };

            dto.SuporteId.PossuiErro.Should().BeTrue();
        }

        #endregion

        #region FormatoId

        [Fact]
        public void DadoFormatoIdVazio_QuandoCriarDTO_EntaoFormatoIdDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.FormatoId.Should().BeNull();
        }

        [Fact]
        public void DadoFormatoId_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "3" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { FormatoId = linhaConteudo };

            dto.FormatoId.Should().NotBeNull();
            dto.FormatoId.Conteudo.Should().Be("3");
        }

        [Fact]
        public void DadoFormatoIdComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "999",
                PossuiErro = true,
                Mensagem = "Formato não encontrado"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { FormatoId = linhaConteudo };

            dto.FormatoId.PossuiErro.Should().BeTrue();
        }

        #endregion

        #region TamanhoArquivo

        [Fact]
        public void DadoTamanhoArquivoVazio_QuandoCriarDTO_EntaoTamanhoArquivoDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.TamanhoArquivo.Should().BeNull();
        }

        [Fact]
        public void DadoTamanhoArquivo_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "2.5 MB" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { TamanhoArquivo = linhaConteudo };

            dto.TamanhoArquivo.Should().NotBeNull();
            dto.TamanhoArquivo.Conteudo.Should().Be("2.5 MB");
        }

        [Fact]
        public void DadoTamanhoArquivoComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "1000",
                PossuiErro = true,
                Mensagem = "Arquivo muito grande"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { TamanhoArquivo = linhaConteudo };

            dto.TamanhoArquivo.PossuiErro.Should().BeTrue();
        }

        #endregion

        #region CromiaId

        [Fact]
        public void DadoCromiaIdVazio_QuandoCriarDTO_EntaoCromiaIdDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.CromiaId.Should().BeNull();
        }

        [Fact]
        public void DadoCromiaId_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { CromiaId = linhaConteudo };

            dto.CromiaId.Should().NotBeNull();
            dto.CromiaId.Conteudo.Should().Be("1");
        }

        [Fact]
        public void DadoCromiaIdComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "999",
                PossuiErro = true,
                Mensagem = "Cromia não encontrada"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { CromiaId = linhaConteudo };

            dto.CromiaId.PossuiErro.Should().BeTrue();
        }

        #endregion

        #region Resolucao

        [Fact]
        public void DadoResolucaoVazia_QuandoCriarDTO_EntaoResolucaoDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.Resolucao.Should().BeNull();
        }

        [Fact]
        public void DadoResolucao_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "300 DPI" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Resolucao = linhaConteudo };

            dto.Resolucao.Should().NotBeNull();
            dto.Resolucao.Conteudo.Should().Be("300 DPI");
        }

        [Fact]
        public void DadoResolucaoComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "600",
                PossuiErro = true,
                Mensagem = "Resolução inválida"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Resolucao = linhaConteudo };

            dto.Resolucao.PossuiErro.Should().BeTrue();
        }

        #endregion

        #region Ano

        [Fact]
        public void DadoAnoVazio_QuandoCriarDTO_EntaoAnoDeveSerNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoAno_QuandoAtribuirValor_EntaoDeveArmazenarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "2020" };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Ano = linhaConteudo };

            dto.Ano.Should().NotBeNull();
            dto.Ano.Conteudo.Should().Be("2020");
        }

        [Fact]
        public void DadoAnoComErro_QuandoAtribuirComErro_EntaoDeveArmazenarComErro()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "1800",
                PossuiErro = true,
                Mensagem = "Ano inválido"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Ano = linhaConteudo };

            dto.Ano.PossuiErro.Should().BeTrue();
        }

        #endregion

        #region Testes de Integração - Herança

        [Fact]
        public void DadoDTOHerdando_QuandoCriarDTO_EntaoDeveConterPropriedadesHerdadas()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.Should().NotBeNull();
            dto.Should().BeAssignableTo<AcervoLinhaRetornoDTO>();
        }

        [Fact]
        public void DadoDTOComPropriedadesHerdadas_QuandoAtribuirValores_EntaoDeveArmazenarCorretamente()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO
            {
                Status = ImportacaoStatus.Sucesso,
                NumeroLinha = 1,
                Mensagem = "Processado com sucesso",
                ErrosCampos = new[] { "Campo1" }
            };

            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.NumeroLinha.Should().Be(1);
            dto.Mensagem.Should().Be("Processado com sucesso");
            dto.ErrosCampos.Should().NotBeNull();
            dto.ErrosCampos.Should().HaveCount(1);
        }

        [Fact]
        public void DadoDTOCompleto_QuandoInstanciarComTodosOsParametros_EntaoDeveArmazenarTodosCorretamente()
        {
            var faker = new Faker("pt_BR");
            var numeroLinha = faker.Random.Int(1, 100);
            var titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Lorem.Sentence() };
            var codigo = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Random.String(10) };
            var creditosAutoresIds = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1,2,3" };
            var localizacao = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Address.StreetName() };
            var procedencia = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Company.CompanyName() };
            var dataAcervo = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Date.Past().ToString("dd/MM/yyyy") };
            var copiaDigital = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Sim" };
            var permiteUsoImagem = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Não" };
            var conservacaoId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1" };
            var descricao = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Lorem.Paragraph() };
            var quantidade = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Random.Int(1, 100).ToString() };
            var largura = new LinhaConteudoAjustarRetornoDTO { Conteudo = "20cm" };
            var altura = new LinhaConteudoAjustarRetornoDTO { Conteudo = "30cm" };
            var suporteId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "2" };
            var formatoId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "3" };
            var tamanhoArquivo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "2.5 MB" };
            var cromiaId = new LinhaConteudoAjustarRetornoDTO { Conteudo = "1" };
            var resolucao = new LinhaConteudoAjustarRetornoDTO { Conteudo = "300 DPI" };
            var ano = new LinhaConteudoAjustarRetornoDTO { Conteudo = faker.Date.Past().Year.ToString() };

            var dto = new AcervoFotograficoLinhaRetornoDTO
            {
                NumeroLinha = numeroLinha,
                Status = ImportacaoStatus.Sucesso,
                Mensagem = "Processado",
                ErrosCampos = new string[] { },
                Titulo = titulo,
                Codigo = codigo,
                CreditosAutoresIds = creditosAutoresIds,
                Localizacao = localizacao,
                Procedencia = procedencia,
                DataAcervo = dataAcervo,
                CopiaDigital = copiaDigital,
                PermiteUsoImagem = permiteUsoImagem,
                ConservacaoId = conservacaoId,
                Descricao = descricao,
                Quantidade = quantidade,
                Largura = largura,
                Altura = altura,
                SuporteId = suporteId,
                FormatoId = formatoId,
                TamanhoArquivo = tamanhoArquivo,
                CromiaId = cromiaId,
                Resolucao = resolucao,
                Ano = ano
            };

            dto.NumeroLinha.Should().Be(numeroLinha);
            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
            dto.Mensagem.Should().Be("Processado");
            dto.ErrosCampos.Should().NotBeNull();
            dto.Titulo.Should().BeEquivalentTo(titulo);
            dto.Codigo.Should().BeEquivalentTo(codigo);
            dto.CreditosAutoresIds.Should().BeEquivalentTo(creditosAutoresIds);
            dto.Localizacao.Should().BeEquivalentTo(localizacao);
            dto.Procedencia.Should().BeEquivalentTo(procedencia);
            dto.DataAcervo.Should().BeEquivalentTo(dataAcervo);
            dto.CopiaDigital.Should().BeEquivalentTo(copiaDigital);
            dto.PermiteUsoImagem.Should().BeEquivalentTo(permiteUsoImagem);
            dto.ConservacaoId.Should().BeEquivalentTo(conservacaoId);
            dto.Descricao.Should().BeEquivalentTo(descricao);
            dto.Quantidade.Should().BeEquivalentTo(quantidade);
            dto.Largura.Should().BeEquivalentTo(largura);
            dto.Altura.Should().BeEquivalentTo(altura);
            dto.SuporteId.Should().BeEquivalentTo(suporteId);
            dto.FormatoId.Should().BeEquivalentTo(formatoId);
            dto.TamanhoArquivo.Should().BeEquivalentTo(tamanhoArquivo);
            dto.CromiaId.Should().BeEquivalentTo(cromiaId);
            dto.Resolucao.Should().BeEquivalentTo(resolucao);
            dto.Ano.Should().BeEquivalentTo(ano);
        }

        [Fact]
        public void DadoDTOVazio_QuandoInstanciarSemParametros_EntaoDeveSerValido()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.Should().NotBeNull();
            dto.Titulo.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.CreditosAutoresIds.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.DataAcervo.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.PermiteUsoImagem.Should().BeNull();
            dto.ConservacaoId.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.Quantidade.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.SuporteId.Should().BeNull();
            dto.FormatoId.Should().BeNull();
            dto.TamanhoArquivo.Should().BeNull();
            dto.CromiaId.Should().BeNull();
            dto.Resolucao.Should().BeNull();
            dto.Ano.Should().BeNull();
        }

        [Fact]
        public void DadoDTOComValoresNulos_QuandoAtribuirExplicitamente_EntaoDeveArmazenarNull()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO
            {
                Titulo = null!,
                Codigo = null!,
                CreditosAutoresIds = null!,
                Localizacao = null!,
                Procedencia = null!,
                DataAcervo = null!,
                CopiaDigital = null!,
                PermiteUsoImagem = null!,
                ConservacaoId = null!,
                Descricao = null!,
                Quantidade = null!,
                Largura = null!,
                Altura = null!,
                SuporteId = null!,
                FormatoId = null!,
                TamanhoArquivo = null!,
                CromiaId = null!,
                Resolucao = null!,
                Ano = null!,
                Mensagem = null!
            };

            dto.Titulo.Should().BeNull();
            dto.Codigo.Should().BeNull();
            dto.CreditosAutoresIds.Should().BeNull();
            dto.Localizacao.Should().BeNull();
            dto.Procedencia.Should().BeNull();
            dto.DataAcervo.Should().BeNull();
            dto.CopiaDigital.Should().BeNull();
            dto.PermiteUsoImagem.Should().BeNull();
            dto.ConservacaoId.Should().BeNull();
            dto.Descricao.Should().BeNull();
            dto.Quantidade.Should().BeNull();
            dto.Largura.Should().BeNull();
            dto.Altura.Should().BeNull();
            dto.SuporteId.Should().BeNull();
            dto.FormatoId.Should().BeNull();
            dto.TamanhoArquivo.Should().BeNull();
            dto.CromiaId.Should().BeNull();
            dto.Resolucao.Should().BeNull();
            dto.Ano.Should().BeNull();
            dto.Mensagem.Should().BeNull();
        }

        [Fact]
        public void DadoDuasInstancias_QuandoComMesmosValores_EntaoSaoInstanciasDistintas()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Teste" };
            var dto1 = new AcervoFotograficoLinhaRetornoDTO
            {
                NumeroLinha = 1,
                Titulo = linhaConteudo
            };

            var dto2 = new AcervoFotograficoLinhaRetornoDTO
            {
                NumeroLinha = 1,
                Titulo = linhaConteudo
            };

            dto1.Should().NotBeSameAs(dto2);
            dto1.NumeroLinha.Should().Be(dto2.NumeroLinha);
        }

        [Fact]
        public void DadoDTOComLinhasDeConteudo_QuandoAcessarMultiplasVezes_EntaoValoresPermanecem()
        {
            var titulo = new LinhaConteudoAjustarRetornoDTO { Conteudo = "Título Teste", PossuiErro = false };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Titulo = titulo };

            var titulo1 = dto.Titulo;
            var titulo2 = dto.Titulo;

            titulo1.Should().BeSameAs(titulo2);
            titulo1.Conteudo.Should().Be("Título Teste");
        }

        [Fact]
        public void DadoDTOComDiversosStatus_QuandoAlternarStatus_EntaoAlternaCorretamente()
        {
            var dto = new AcervoFotograficoLinhaRetornoDTO();

            dto.Status = ImportacaoStatus.Pendente;
            dto.Status.Should().Be(ImportacaoStatus.Pendente);

            dto.Status = ImportacaoStatus.Erros;
            dto.Status.Should().Be(ImportacaoStatus.Erros);

            dto.Status = ImportacaoStatus.Sucesso;
            dto.Status.Should().Be(ImportacaoStatus.Sucesso);
        }

        [Fact]
        public void DadoDTOComLinhaConteudoCompleto_QuandoVerificarErros_EntaoDeveRetornarCorretamente()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "Conteúdo",
                PossuiErro = true,
                Mensagem = "Erro de validação"
            };

            var dto = new AcervoFotograficoLinhaRetornoDTO { Titulo = linhaConteudo };

            dto.Titulo.PossuiErro.Should().BeTrue();
            dto.Titulo.Mensagem.Should().Be("Erro de validação");
        }

        [Fact]
        public void DadoDTOComMultiplosCamposComErro_QuandoVerificarCadaCampo_EntaoDeveTerErrosCorretos()
        {
            var titulo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "Título",
                PossuiErro = true,
                Mensagem = "Obrigatório"
            };
            var codigo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "COD",
                PossuiErro = true,
                Mensagem = "Inválido"
            };
            var descricao = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "Desc",
                PossuiErro = false,
                Mensagem = null!
            };

            var dto = new AcervoFotograficoLinhaRetornoDTO
            {
                Titulo = titulo,
                Codigo = codigo,
                Descricao = descricao
            };

            dto.Titulo.PossuiErro.Should().BeTrue();
            dto.Codigo.PossuiErro.Should().BeTrue();
            dto.Descricao.PossuiErro.Should().BeFalse();
        }

        [Fact]
        public void DadoDTOComConteudoEspecial_QuandoArmazenarCaracteresEspeciais_EntaoDeveArmazenarCompleto()
        {
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO
            {
                Conteudo = "Título com acentuação: ç, ã, ê, ö @#$%&*()"
            };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Titulo = linhaConteudo };

            dto.Titulo.Conteudo.Should().Be("Título com acentuação: ç, ã, ê, ö @#$%&*()");
        }

        [Fact]
        public void DadoDTOComConteudoMuitoGrande_QuandoArmazenarTextoLongo_EntaoDeveArmazenarCompleto()
        {
            var conteudoGrande = new string('A', 5000);
            var linhaConteudo = new LinhaConteudoAjustarRetornoDTO { Conteudo = conteudoGrande };
            var dto = new AcervoFotograficoLinhaRetornoDTO { Descricao = linhaConteudo };

            dto.Descricao.Conteudo.Should().Be(conteudoGrande);
            dto.Descricao.Conteudo.Length.Should().Be(5000);
        }

        #endregion
    }
}
