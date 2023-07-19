﻿using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoIdNomeExcluido
    {
        Task<long> Inserir(IdNomeExcluidoDTO idNomeExcluidoDto);
        Task<IList<IdNomeExcluidoDTO>> ObterTodos();
        Task<IdNomeExcluidoDTO> Alterar(IdNomeExcluidoDTO idNomeExcluidoDto);
        Task<IdNomeExcluidoDTO> ObterPorId(long Id);
        Task<bool> Excluir(long Id);
    }
}