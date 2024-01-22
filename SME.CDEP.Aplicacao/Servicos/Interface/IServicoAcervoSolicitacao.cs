﻿using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoAcervoSolicitacao : IServicoAplicacao
    {
        Task<long> Inserir(AcervoSolicitacaoDTO acervoSolicitacao);
        Task<AcervoSolicitacaoDTO> ObterPorId(long acervoSolicitacaoId);
        Task<IEnumerable<AcervoSolicitacaoDTO>> ObterTodosPorUsuario(int usuarioId);
        Task<AcervoSolicitacaoDTO> Alterar(AcervoSolicitacaoDTO acervoSolicitacao);
        Task<bool> Remover(long acervoSolicitacaoId);
        Task<IEnumerable<AcervoSolicitacaoItemRetornoDTO>> ObterItensDoAcervoPorFiltros(AcervoSolicitacaoItemConsultaDTO[] acervosSolicitacaoItensConsultaDTO);
    }
}
