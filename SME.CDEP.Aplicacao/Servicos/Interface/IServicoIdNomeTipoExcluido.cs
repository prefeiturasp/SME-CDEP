﻿using SME.CDEP.Aplicacao.DTOS;

namespace SME.CDEP.Aplicacao.Servicos.Interface
{
    public interface IServicoIdNomeTipoExcluido :IServicoAplicacao
    {
        Task<long> Inserir(IdNomeTipoExcluidoDTO idNomeTipoExcluidoDto);
        Task<IEnumerable<IdNomeTipoExcluidoDTO>> ObterTodos();
        Task<IdNomeTipoExcluidoDTO> Alterar(IdNomeTipoExcluidoDTO idNomeTipoExcluidoDto);
        Task<IdNomeTipoExcluidoDTO> ObterPorId(long Id);
        Task<bool> Excluir(long Id);
    }
}
