﻿using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoSolicitacao : IServicoAplicacao
    {
        Task<long> Inserir(AcervoSolicitacaoItemCadastroDTO[] acervosSolicitacaoItensCadastroDTO);
        Task<AcervoSolicitacaoRetornoCadastroDTO> ObterPorId(long acervoSolicitacaoId);
        Task<bool> Remover(long acervoSolicitacaoId);
        Task<IEnumerable<AcervoTipoTituloAcervoIdCreditosAutoresDTO>> ObterItensAcervoPorAcervosIds(long[] acervosIds);
        Task<bool> Excluir(long acervoSolicitacaoId);
        Task<PaginacaoResultadoDTO<MinhaSolicitacaoDTO>> ObterMinhasSolicitacoes();
        Task<IEnumerable<SituacaoItemDTO>> ObterSituacoesAtendimentosItem();
        Task<PaginacaoResultadoDTO<SolicitacaoDTO>> ObterAtendimentoSolicitacoesPorFiltro(FiltroSolicitacaoDTO filtroSolicitacaoDto);
        Task<AcervoSolicitacaoDetalheDTO> ObterDetalhesParaAtendimentoSolicitadoesPorId(long acervoSolicitacaoId);
        IEnumerable<IdNomeDTO> ObterTiposDeAtendimentos();
        Task<bool> ConfirmarAtendimento(AcervoSolicitacaoConfirmarDTO acervoSolicitacaoConfirmar);
        Task<bool> FinalizarAtendimento(long acervoSolicitacaoId);
        Task<bool> CancelarAtendimento(long acervoSolicitacaoId);
        Task<bool> CancelarItemAtendimento(long acervoSolicitacaoItemId);
        Task<bool> AlterarDataVisitaDoItemAtendimento(AlterarDataVisitaAcervoSolicitacaoItemDTO alterarDataVisitaAcervoSolicitacaoItemDto);
        Task<long> Inserir(AcervoSolicitacaoManualDTO acervoSolicitacaoManualDto);
        Task<long> Alterar(AcervoSolicitacaoManualDTO acervoSolicitacaoManualDto);
        Task<bool> FinalizarAtendimentoItem(long acervoSolicitacaoItemId);
        Task<AcervoSolicitacaoRetornoCadastroDTO> ObterMinhaSolicitacaoPorId(long acervoSolicitacaoId);
        bool PodeFinalizar(Guid perfilLogado, AcervoSolicitacaoDetalheDTO acervoSolicitacao);
    }
}
