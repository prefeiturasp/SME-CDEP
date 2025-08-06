using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_pesquisar_acervo_solicitacao : TesteBase
    {
        private readonly IServicoAcervoSolicitacao _servicoAcervoSolicitacao;

        public Ao_pesquisar_acervo_solicitacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            _servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
        }
       
       
        [Fact(DisplayName = "Acervo Solicitação - Pesquisar por AcervoSolicitacaoId")]
        public async Task Obter_pesquisar_por_acervo_solicitacao_id()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var filtro = new FiltroSolicitacaoDTO() { AcervoSolicitacaoId = 1 };
            var retorno = await _servicoAcervoSolicitacao.ObterAtendimentoSolicitacoesPorFiltro(filtro);
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Pesquisar por Item da Situação ")]
        public async Task Obter_pesquisar_por_situacao_item()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var filtro = new FiltroSolicitacaoDTO() { SituacaoItem = SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE };
            var retorno = await _servicoAcervoSolicitacao.ObterAtendimentoSolicitacoesPorFiltro(filtro);
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Pesquisar por Tipo de Acervo")]
        public async Task Obter_pesquisar_por_tipo_de_acervo()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var filtro = new FiltroSolicitacaoDTO() { TipoAcervo = TipoAcervo.Tridimensional };
            var retorno = await _servicoAcervoSolicitacao.ObterAtendimentoSolicitacoesPorFiltro(filtro);
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Pesquisar por Data de solicitação")]
        public async Task Obter_pesquisar_por_data_solicitacao()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var filtro = new FiltroSolicitacaoDTO() { DataSolicitacaoInicio = DateTimeExtension.HorarioBrasilia() };
            var retorno = await _servicoAcervoSolicitacao.ObterAtendimentoSolicitacoesPorFiltro(filtro);
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Pesquisar por Data de visita válida")]
        public async Task Obter_pesquisar_por_data_de_visita_valida()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao(inserirEmAtendimento:true);

            var filtro = new FiltroSolicitacaoDTO()
            {
                DataVisitaInicio = DateTimeExtension.HorarioBrasilia(),
                DataVisitaFim = DateTimeExtension.HorarioBrasilia().AddDays(20)
            };
            var retorno = await _servicoAcervoSolicitacao.ObterAtendimentoSolicitacoesPorFiltro(filtro);
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Pesquisar por Data de visita inválida")]
        public async Task Obter_pesquisar_por_data_de_visita_invalida()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var filtro = new FiltroSolicitacaoDTO()
            {
                DataVisitaInicio = DateTimeExtension.HorarioBrasilia(),
                DataVisitaFim = DateTimeExtension.HorarioBrasilia()
            };
            var retorno = await _servicoAcervoSolicitacao.ObterAtendimentoSolicitacoesPorFiltro(filtro);
            retorno.Items.Count().ShouldBe(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Pesquisar por Responsavel")]
        public async Task Obter_pesquisar_por_responsavel()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var filtro = new FiltroSolicitacaoDTO()
            {
                Responsavel = "login_2"
            };
            var retorno = await _servicoAcervoSolicitacao.ObterAtendimentoSolicitacoesPorFiltro(filtro);
            retorno.Items.Count().ShouldBe(3);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Pesquisar por Solicitante")]
        public async Task Obter_pesquisar_por_solicitante()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var filtro = new FiltroSolicitacaoDTO()
            {
                SolicitanteRf = "login_1"
            };
            var retorno = await _servicoAcervoSolicitacao.ObterAtendimentoSolicitacoesPorFiltro(filtro);
            retorno.Items.Count().ShouldBe(3);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve retornar qualquer tipo de acervo com o perfil Adm Geral")]
        public async Task Deve_retornar_qualquer_tipo_de_acervo_com_perfil_admin_geral()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervosEAtendimentos();

            var retorno = await _servicoAcervoSolicitacao.ObterAtendimentoSolicitacoesPorFiltro(new FiltroSolicitacaoDTO());
            
            retorno.Items.Count().ShouldBe(6);
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.Tridimensional.Descricao()));
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.Bibliografico.Descricao()));
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.ArtesGraficas.Descricao()));
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.Fotografico.Descricao()));
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.DocumentacaoTextual.Descricao()));
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.Audiovisual.Descricao()));
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve retornar qualquer tipo de acervo com o perfil Básico")]
        public async Task Deve_retornar_qualquer_tipo_de_acervo_com_perfil_basico()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_BASICO_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervosEAtendimentos();

            var retorno = await _servicoAcervoSolicitacao.ObterAtendimentoSolicitacoesPorFiltro(new FiltroSolicitacaoDTO());
            
            retorno.Items.Count().ShouldBe(6);
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.Tridimensional.Descricao()));
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.Bibliografico.Descricao()));
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.ArtesGraficas.Descricao()));
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.Fotografico.Descricao()));
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.DocumentacaoTextual.Descricao()));
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.Audiovisual.Descricao()));
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Não deve retornar nenhum tipo de acervo com o perfil Externo")]
        public async Task Nao_deve_retornar_nenhum_tipo_de_acervo_com_perfil_externo()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_EXTERNO_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervosEAtendimentos();

            var retorno = await _servicoAcervoSolicitacao.ObterAtendimentoSolicitacoesPorFiltro(new FiltroSolicitacaoDTO());
            
            retorno.Items.Count().ShouldBe(0);
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve retornar todos os tipos de acervo exceto bibliográfico e documental com perfil Admin Memorial")]
        public async Task Deve_retornar_todos_os_tipos_de_acervo_exceto_bibliografico_e_documental_com_perfil_admin_memorial()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_MEMORIAL_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervosEAtendimentos();

            var retorno = await _servicoAcervoSolicitacao.ObterAtendimentoSolicitacoesPorFiltro(new FiltroSolicitacaoDTO());
            
            retorno.Items.Count().ShouldBe(4);
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.Tridimensional.Descricao()));
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.ArtesGraficas.Descricao()));
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.Fotografico.Descricao()));
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.Audiovisual.Descricao()));
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve retornar somente o tipo de acervo bibliográfico com perfil Admin Bibliográfico")]
        public async Task Deve_retornar_somente_o_tipo_de_acervo_bibliografico_com_perfil_admin_biblioteca()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_BIBLIOTECA_GUID);
            
            await InserirDadosBasicosAleatorios();

            await InserirAcervosEAtendimentos();

            var retorno = await _servicoAcervoSolicitacao.ObterAtendimentoSolicitacoesPorFiltro(new FiltroSolicitacaoDTO());
            
            retorno.Items.Count().ShouldBe(1);
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.Bibliografico.Descricao()));
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Deve retornar somente o tipo de acervo documental com perfil Admin Memoria")]
        public async Task Deve_retornar_somente_o_tipo_de_acervo_documental_com_perfil_admin_memoria()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_MEMORIA_GUID);
            
            await InserirDadosBasicosAleatorios();
            
            await InserirAcervosEAtendimentos();

            var retorno = await _servicoAcervoSolicitacao.ObterAtendimentoSolicitacoesPorFiltro(new FiltroSolicitacaoDTO());
            
            retorno.Items.Count().ShouldBe(1);
            retorno.Items.Any(w => w.TipoAcervo.Equals(TipoAcervo.DocumentacaoTextual.Descricao()));
        }

        private async Task InserirAcervosEAtendimentos()
        {
            var contadorAcervos = 1;
            var contadorSolicitacoes = 1;
            var quantidadePorTipo = 10;

            var inserirAcervosEAtendimentos = new List<Func<Task>>()
            {
                () => InserirAcervosBibliograficosEmMassa(contadorAcervos, quantidadePorTipo),
                () => InserirAcervosArteGraficasEmMassa(contadorAcervos + quantidadePorTipo, quantidadePorTipo),
                () => InserirAcervosTridimensionalEmMassa(contadorAcervos + 2 * quantidadePorTipo, quantidadePorTipo),
                () => InserirAcervosFotograficoEmMassa(contadorAcervos + 3 * quantidadePorTipo, quantidadePorTipo),
                () => InserirAcervosDocumentalEmMassa(contadorAcervos + 4 * quantidadePorTipo, quantidadePorTipo),
                () => InserirAcervosAudiovisualEmMassa(contadorAcervos + 5 * quantidadePorTipo, quantidadePorTipo),
                
                () => AcervoSolicitacaoEItem(contadorSolicitacoes++, 1),
                () => AcervoSolicitacaoEItem(contadorSolicitacoes++, 12),
                () => AcervoSolicitacaoEItem(contadorSolicitacoes++, 23),
                () => AcervoSolicitacaoEItem(contadorSolicitacoes++, 33),
                () => AcervoSolicitacaoEItem(contadorSolicitacoes++, 44),
                () => AcervoSolicitacaoEItem(contadorSolicitacoes++, 54),
            };

            foreach (var inserirAcervosEAtendimento in inserirAcervosEAtendimentos)
                await inserirAcervosEAtendimento();
        }
    }
}