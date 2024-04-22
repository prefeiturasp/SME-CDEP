using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_obter_detalhes_acervo_solicitacao : TesteBase
    {
        public Ao_obter_detalhes_acervo_solicitacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo Solicitação - Obter todos por usuário logado")]
        public async Task Obter_todos_por_usuario_logado()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao(10);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var retorno = await servicoAcervoSolicitacao.ObterMinhasSolicitacoes();
            retorno.ShouldNotBeNull();
        }
        
       
        [Fact(DisplayName = "Acervo Solicitação - Obter Detalhes por Id de acrvo tridimensional")]
        public async Task Obter_detalhes_por_id()
        {
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);

            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var retorno = await servicoAcervoSolicitacao.ObterDetalhesParaAtendimentoSolicitadoesPorId(1);
            retorno.ShouldNotBeNull();
            retorno.DadosSolicitante.ShouldNotBeNull();
            retorno.DadosSolicitante.Nome.ShouldNotBeEmpty();
            retorno.DadosSolicitante.Tipo.ShouldNotBeEmpty();
            retorno.Id.ShouldBeGreaterThan(0);
            retorno.Situacao.ShouldNotBeEmpty();
            retorno.UsuarioId.ShouldBeGreaterThan(0);
            retorno.Itens.Any().ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Obter Detalhes por Id de acervo bibliográfico indisponível (reservado)")]
        public async Task Obter_detalhes_por_id_de_acervo_bibliografico_indisponivel_reservado()
        {
            var parametroQtdeCursistasSuportadosPorTurma = ParametroSistemaMock.Instance.GerarParametroSistema(TipoParametroSistema.LimiteDiasEmprestimoAcervo, "7");
            await InserirNaBase(parametroQtdeCursistasSuportadosPorTurma);
            
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);

            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos(SituacaoSaldo.RESERVADO);

            await InserirAcervoSolicitacao();

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var retorno = await servicoAcervoSolicitacao.ObterDetalhesParaAtendimentoSolicitadoesPorId(1);
            retorno.ShouldNotBeNull();
            retorno.DadosSolicitante.ShouldNotBeNull();
            retorno.DadosSolicitante.Nome.ShouldNotBeEmpty();
            retorno.DadosSolicitante.Tipo.ShouldNotBeEmpty();
            retorno.Id.ShouldBeGreaterThan(0);
            retorno.Situacao.ShouldNotBeEmpty();
            retorno.UsuarioId.ShouldBeGreaterThan(0);
            retorno.Itens.Any().ShouldBeTrue();
            retorno.Itens.Any(a=> a.EstaDisponivel).ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Obter Detalhes por Id de acervo bibliográfico indisponível (emprestado)")]
        public async Task Obter_detalhes_por_id_de_acervo_bibliografico_indisponivel_emprestado()
        {
            var parametroQtdeCursistasSuportadosPorTurma = ParametroSistemaMock.Instance.GerarParametroSistema(TipoParametroSistema.LimiteDiasEmprestimoAcervo, "7");
            await InserirNaBase(parametroQtdeCursistasSuportadosPorTurma);
            
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);

            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos(SituacaoSaldo.EMPRESTADO);

            await InserirAcervoSolicitacao();

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var retorno = await servicoAcervoSolicitacao.ObterDetalhesParaAtendimentoSolicitadoesPorId(1);
            retorno.ShouldNotBeNull();
            retorno.DadosSolicitante.ShouldNotBeNull();
            retorno.DadosSolicitante.Nome.ShouldNotBeEmpty();
            retorno.DadosSolicitante.Tipo.ShouldNotBeEmpty();
            retorno.Id.ShouldBeGreaterThan(0);
            retorno.Situacao.ShouldNotBeEmpty();
            retorno.UsuarioId.ShouldBeGreaterThan(0);
            retorno.Itens.Any().ShouldBeTrue();
            retorno.Itens.Any(a=> a.EstaDisponivel).ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Obter Detalhes por Id de acervo bibliográfico disponível")]
        public async Task Obter_detalhes_por_id_de_acervo_bibliografico_disponivel()
        {
            var parametroQtdeCursistasSuportadosPorTurma = ParametroSistemaMock.Instance.GerarParametroSistema(TipoParametroSistema.LimiteDiasEmprestimoAcervo, "7");
            await InserirNaBase(parametroQtdeCursistasSuportadosPorTurma);
            
            CriarClaimUsuario(Dominio.Constantes.Constantes.PERFIL_ADMIN_GERAL_GUID);

            await InserirDadosBasicosAleatorios();

            await InserirAcervosBibliograficos();

            await InserirAcervoSolicitacao();

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var retorno = await servicoAcervoSolicitacao.ObterDetalhesParaAtendimentoSolicitadoesPorId(1);
            retorno.ShouldNotBeNull();
            retorno.DadosSolicitante.ShouldNotBeNull();
            retorno.DadosSolicitante.Nome.ShouldNotBeEmpty();
            retorno.DadosSolicitante.Tipo.ShouldNotBeEmpty();
            retorno.Id.ShouldBeGreaterThan(0);
            retorno.Situacao.ShouldNotBeEmpty();
            retorno.UsuarioId.ShouldBeGreaterThan(0);
            retorno.Itens.Any().ShouldBeTrue();
            retorno.Itens.Any(a=> a.EstaDisponivel).ShouldBeTrue();
        }
    }
}