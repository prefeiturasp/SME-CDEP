using Shouldly;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;
using SME.CDEP.TesteIntegracao.Setup;
using Xunit;

namespace SME.CDEP.TesteIntegracao
{
    public class Ao_fazer_manutencao_acervo_solicitacao : TesteBase
    {
        public Ao_fazer_manutencao_acervo_solicitacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Acervo Solicitação - Obter por Id")]
        public async Task Obter_por_id()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao();

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();

            var retorno = await servicoAcervoSolicitacao.ObterPorId(1);
            retorno.ShouldNotBeNull();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Ao enviar a solicitação para análise - offline - sem arquivos")]
        public async Task Ao_enviar_solicitacao_para_analise_sem_arquivos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional(false);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var acervoSolicitacaoInserir = ObterItens();
            
            var retorno = await servicoAcervoSolicitacao.Inserir(acervoSolicitacaoInserir.ToArray());
            retorno.ShouldBeGreaterThan(0);

            var solicitacoes = ObterTodos<AcervoSolicitacao>();
            solicitacoes.FirstOrDefault().DataSolicitacao.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            solicitacoes.FirstOrDefault().Origem.ShouldBe(Origem.Portal);
            solicitacoes.FirstOrDefault().Situacao.ShouldBe(SituacaoSolicitacao.AGUARDANDO_ATENDIMENTO);
            solicitacoes.FirstOrDefault().ResponsavelId.ShouldBeNull();
            solicitacoes.FirstOrDefault().UsuarioId.ShouldBeGreaterThan(0);
            solicitacoes.FirstOrDefault().Excluido.ShouldBeFalse();
            
            var itens = ObterTodos<AcervoSolicitacaoItem>();
            itens.Count().ShouldBe(3);

            var primeiroItem = itens.FirstOrDefault(f => f.AcervoId == 1);
            primeiroItem.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO);
            primeiroItem.TipoAtendimento.ShouldBeNull();
            primeiroItem.DataVisita.ShouldBeNull();
            primeiroItem.AcervoSolicitacaoId.ShouldBeGreaterThan(0);
            primeiroItem.Excluido.ShouldBeFalse();
            
            var segundoItem = itens.FirstOrDefault(f => f.AcervoId == 2);
            segundoItem.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO);
            segundoItem.TipoAtendimento.ShouldBeNull();
            segundoItem.DataVisita.ShouldBeNull();
            segundoItem.AcervoSolicitacaoId.ShouldBeGreaterThan(0);
            segundoItem.Excluido.ShouldBeFalse();
            
            var terceiroItem = itens.FirstOrDefault(f => f.AcervoId == 3);
            terceiroItem.Situacao.ShouldBe(SituacaoSolicitacaoItem.AGUARDANDO_ATENDIMENTO);
            terceiroItem.TipoAtendimento.ShouldBeNull();
            terceiroItem.DataVisita.ShouldBeNull();
            terceiroItem.AcervoSolicitacaoId.ShouldBeGreaterThan(0);
            terceiroItem.Excluido.ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Ao enviar a solicitação para finalizado - online - com arquivos")]
        public async Task Ao_enviar_solicitacao_para_finalizado_com_arquivos()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var acervoSolicitacaoInserir = ObterItens();
            
            var retorno = await servicoAcervoSolicitacao.Inserir(acervoSolicitacaoInserir.ToArray());
            retorno.ShouldBeGreaterThan(0);
            
            var solicitacoes = ObterTodos<AcervoSolicitacao>();
            solicitacoes.FirstOrDefault().DataSolicitacao.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            solicitacoes.FirstOrDefault().Origem.ShouldBe(Origem.Portal);
            solicitacoes.FirstOrDefault().Situacao.ShouldBe(SituacaoSolicitacao.FINALIZADO_ATENDIMENTO);
            solicitacoes.FirstOrDefault().ResponsavelId.ShouldBeNull();
            solicitacoes.FirstOrDefault().UsuarioId.ShouldBeGreaterThan(0);
            solicitacoes.FirstOrDefault().Excluido.ShouldBeFalse();
            
            var itens = ObterTodos<AcervoSolicitacaoItem>();
            itens.Count().ShouldBe(3);

            var primeiroItem = itens.FirstOrDefault(f => f.AcervoId == 1);
            primeiroItem.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE);
            primeiroItem.TipoAtendimento.ShouldBeNull();
            primeiroItem.DataVisita.ShouldBeNull();
            primeiroItem.AcervoSolicitacaoId.ShouldBeGreaterThan(0);
            primeiroItem.Excluido.ShouldBeFalse();
            
            var segundoItem = itens.FirstOrDefault(f => f.AcervoId == 2);
            segundoItem.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE);
            segundoItem.TipoAtendimento.ShouldBeNull();
            segundoItem.DataVisita.ShouldBeNull();
            segundoItem.AcervoSolicitacaoId.ShouldBeGreaterThan(0);
            segundoItem.Excluido.ShouldBeFalse();
            
            var terceiroItem = itens.FirstOrDefault(f => f.AcervoId == 3);
            terceiroItem.Situacao.ShouldBe(SituacaoSolicitacaoItem.FINALIZADO_AUTOMATICAMENTE);
            terceiroItem.TipoAtendimento.ShouldBeNull();
            terceiroItem.DataVisita.ShouldBeNull();
            terceiroItem.AcervoSolicitacaoId.ShouldBeGreaterThan(0);
            terceiroItem.Excluido.ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Acervo Solicitação - Remover")]
        public async Task Ao_remover()
        {
            await InserirDadosBasicosAleatorios();

            await InserirAcervoTridimensional();

            await InserirAcervoSolicitacao(10);

            var servicoAcervoSolicitacao = GetServicoAcervoSolicitacao();
            
            var retornoAcervoSolicitacaoAlterado = await servicoAcervoSolicitacao.Remover(1);
            retornoAcervoSolicitacaoAlterado.ShouldBeTrue();
            
            var acervoSolicitacaoAlterado = await servicoAcervoSolicitacao.ObterPorId(1);
            acervoSolicitacaoAlterado.Itens.PossuiElementos().ShouldBeFalse();
        }
        
        private List<AcervoSolicitacaoItemCadastroDTO> ObterItens()
        {
            return new List<AcervoSolicitacaoItemCadastroDTO>()
            {
                new() { AcervoId = 1 },
                new() { AcervoId = 2 },
                new() { AcervoId = 3 },
            };
        }
    }
}