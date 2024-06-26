﻿using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoIdNomeExcluidoAuditavel : IServicoAplicacao
    {
        Task<long> Inserir(IdNomeExcluidoAuditavelDTO idNomeExcluidoAuditavelDTO);
        Task<IEnumerable<IdNomeExcluidoAuditavelDTO>> ObterTodos();
        Task<IdNomeExcluidoAuditavelDTO> Alterar(IdNomeExcluidoAuditavelDTO idNomeExcluidoAuditavelDTO);
        Task<IdNomeExcluidoAuditavelDTO> ObterPorId(long Id);
        Task<bool> Excluir(long Id);
        Task<PaginacaoResultadoDTO<IdNomeExcluidoAuditavelDTO>> ObterPaginado(string? nome = null);
    }
}
