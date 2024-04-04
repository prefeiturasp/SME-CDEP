using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcervoEmprestimo : IServicoAcervoEmprestimo
    {
        private readonly IRepositorioAcervoEmprestimo repositorioAcervoEmprestimo;
        private readonly IServicoAcervoBibliografico servicoAcervoBibliografico;
        private readonly IRepositorioAcervoSolicitacaoItem repositorioAcervoSolicitacaoItem;
        private readonly IRepositorioAcervo repositorioAcervo;
        private readonly IRepositorioParametroSistema repositorioParametroSistema;
        private readonly IServicoMensageria servicoMensageria;
        
        public ServicoAcervoEmprestimo(IRepositorioAcervoEmprestimo repositorioAcervoEmprestimo,IServicoAcervoBibliografico servicoAcervoBibliografico,
            IRepositorioAcervoSolicitacaoItem repositorioAcervoSolicitacaoItem,IRepositorioAcervo repositorioAcervo,IRepositorioParametroSistema repositorioParametroSistema,
            IServicoMensageria servicoMensageria) 
        {
            this.repositorioAcervoEmprestimo = repositorioAcervoEmprestimo ?? throw new ArgumentNullException(nameof(repositorioAcervoEmprestimo));
            this.servicoAcervoBibliografico = servicoAcervoBibliografico ?? throw new ArgumentNullException(nameof(servicoAcervoBibliografico));
            this.repositorioAcervoSolicitacaoItem = repositorioAcervoSolicitacaoItem ?? throw new ArgumentNullException(nameof(repositorioAcervoSolicitacaoItem));
            this.repositorioAcervo = repositorioAcervo ?? throw new ArgumentNullException(nameof(repositorioAcervo));
            this.repositorioParametroSistema = repositorioParametroSistema ?? throw new ArgumentNullException(nameof(repositorioParametroSistema));
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
        }
       
        public async Task<bool> ProrrogarEmprestimo(AcervoEmprestimoProrrogacaoDTO acervoEmprestimoProrrogacaoDTO)
        {
            var acervoEmprestimoAtual = await repositorioAcervoEmprestimo.ObterUltimoEmprestimoPorAcervoSolicitacaoItemId(acervoEmprestimoProrrogacaoDTO.AcervoSolicitacaoItemId );

            if (acervoEmprestimoAtual.EhNulo())
                throw new NegocioException(MensagemNegocio.ACERVO_EMPRESTIMO_NAO_ENCONTRADO);

            if (acervoEmprestimoProrrogacaoDTO.DataDevolucao.EhMenorQue(acervoEmprestimoAtual.DataDevolucao))
                throw new NegocioException(MensagemNegocio.DATA_DA_DEVOLUCAO_MENOR_DATA_DA_DEVOLUCAO_ANTERIOR_OU_FUTURA);
            
            if (acervoEmprestimoProrrogacaoDTO.DataDevolucao.NaoEhDataFutura())
                throw new NegocioException(MensagemNegocio.DATA_DA_DEVOLUCAO_MENOR_DATA_DA_DEVOLUCAO_ANTERIOR_OU_FUTURA);
            
            var acervoEmprestimo = new AcervoEmprestimo()
            {
                AcervoSolicitacaoItemId = acervoEmprestimoProrrogacaoDTO.AcervoSolicitacaoItemId,
                DataEmprestimo = acervoEmprestimoAtual.DataEmprestimo,
                DataDevolucao = acervoEmprestimoProrrogacaoDTO.DataDevolucao,
                Situacao = SituacaoEmprestimo.EMPRESTADO_PRORROGACAO
            };
            await repositorioAcervoEmprestimo.Inserir(acervoEmprestimo);

            return true;
        }

        public async Task<bool> DevolverItemEmprestado(long acervoSolicitacaoItemId)
        {
            var acervoSolicitacaoItem = await repositorioAcervoSolicitacaoItem.ObterPorId(acervoSolicitacaoItemId);

            if (acervoSolicitacaoItem.EhNulo())
                throw new NegocioException(MensagemNegocio.SOLICITACAO_ATENDIMENTO_ITEM_NAO_ENCONTRADA);

            var acervoEmprestimoAtual =
                await repositorioAcervoEmprestimo.ObterUltimoEmprestimoPorAcervoSolicitacaoItemId(
                    acervoSolicitacaoItemId);

            if (acervoEmprestimoAtual.EhNulo())
                throw new NegocioException(MensagemNegocio.ACERVO_EMPRESTIMO_NAO_ENCONTRADO);

            var acervoEmprestimo = new AcervoEmprestimo()
            {
                AcervoSolicitacaoItemId = acervoEmprestimoAtual.AcervoSolicitacaoItemId,
                DataEmprestimo = acervoEmprestimoAtual.DataEmprestimo,
                DataDevolucao = DateTimeExtension.HorarioBrasilia(),
                Situacao = SituacaoEmprestimo.DEVOLVIDO
            };
            await repositorioAcervoEmprestimo.Inserir(acervoEmprestimo);

            var acervos = await repositorioAcervo.ObterAcervosPorIds(new[] { acervoSolicitacaoItem.AcervoId });

            if (acervos.Any(a => a.TipoAcervoId.EhAcervoBibliografico()))
                await servicoAcervoBibliografico.AlterarSituacaoSaldo(SituacaoSaldo.DISPONIVEL,
                    acervoSolicitacaoItem.AcervoId);

            return true;
        }

        public Task<IEnumerable<SituacaoItemDTO>> ObterSituacoesEmprestimo()
        {
            var lista = Enum.GetValues(typeof(SituacaoEmprestimo))
                .Cast<SituacaoEmprestimo>()
                .OrderBy(O=> O)
                .Select(v => new SituacaoItemDTO
                {
                    Id = (short)v,
                    Nome = v.Descricao()
                });

            return Task.FromResult(lista);
        }

        public async Task NotificarVencimentoEmprestimo()
        {
            var qtdeDiasNotificacoVencimentoEmprestimo = await repositorioParametroSistema.ObterParametroPorTipoEAno(
                TipoParametroSistema.NotificarQtdeDiasAntesDoVencimentoEmprestimo,
                DateTimeExtension.HorarioBrasilia().Year);

            var dataDevolucaoNotificada = DateTimeExtension.HorarioBrasilia().AddDays(int.Parse(qtdeDiasNotificacoVencimentoEmprestimo.Valor));
            
            var acervosNotificarAntesVencimentoEmprestimo = await repositorioAcervoEmprestimo.
                ObterDetalhamentoDosItensANotificarSobreVencimentoEmprestimoPorDataDevolucao(dataDevolucaoNotificada);

            foreach (var acervoEmprestimoAntesVencimentoDevolucao in acervosNotificarAntesVencimentoEmprestimo)
                await servicoMensageria.Publicar(RotasRabbit.NotificacaoVencimentoEmprestimoUsuario, acervoEmprestimoAntesVencimentoDevolucao, Guid.NewGuid());
        }

        public async Task NotificarDevolucaoEmprestimoAtrasado()
        {
            var qtdeDiasAtrasoDevolucaoEmprestimo = await repositorioParametroSistema.ObterParametroPorTipoEAno(
                TipoParametroSistema.NotificarQtdeDiasAtrasoDevolucaoEmprestimo,
                DateTimeExtension.HorarioBrasilia().Year);

            var dataDevolucaoNotificada = DateTimeExtension.HorarioBrasilia().AddDays(-int.Parse(qtdeDiasAtrasoDevolucaoEmprestimo.Valor));
            
            var acervosEmprestimoDevolucoes = await repositorioAcervoEmprestimo.
                ObterDetalhamentoDosItensANotificarSobreVencimentoEmprestimoPorDataDevolucao(dataDevolucaoNotificada);

            foreach (var acervoEmprestimoAntesVencimentoDevolucao in acervosEmprestimoDevolucoes)
                await servicoMensageria.Publicar(RotasRabbit.NotificacaoDevolucaoEmprestimoAtrasadoUsuario, acervoEmprestimoAntesVencimentoDevolucao, Guid.NewGuid());
        }
    }
}
