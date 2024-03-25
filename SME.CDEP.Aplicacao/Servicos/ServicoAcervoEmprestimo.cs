using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Aplicacao.Servicos.Interface;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dados.Repositorios.Interfaces;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.Servicos
{
    public class ServicoAcervoEmprestimo : IServicoAcervoEmprestimo
    {
       
        private readonly IRepositorioAcervoEmprestimo repositorioAcervoEmprestimo;
        
        public ServicoAcervoEmprestimo(IRepositorioAcervoEmprestimo repositorioAcervoEmprestimo) 
        {
            this.repositorioAcervoEmprestimo = repositorioAcervoEmprestimo ?? throw new ArgumentNullException(nameof(repositorioAcervoEmprestimo));
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
            var acervoEmprestimoAtual = await repositorioAcervoEmprestimo.ObterUltimoEmprestimoPorAcervoSolicitacaoItemId(acervoSolicitacaoItemId);

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
    }
}
