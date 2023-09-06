﻿using SME.CDEP.Dominio.Entidades;
namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioArquivo
    {
        Task<Arquivo> ObterPorCodigo(Guid codigo);
        Task<IEnumerable<Arquivo>> ObterPorCodigos(Guid[] codigos);
        Task<IEnumerable<Arquivo>> ObterPorIds(long[] ids);
        Task<bool> ExcluirArquivoPorCodigo(Guid codigoArquivo);
        Task<bool> ExcluirArquivoPorId(long id);
        Task<long> ObterIdPorCodigo(Guid arquivoCodigo);
        Task<bool> ExcluirArquivosPorIds(long[] ids);
        Task SalvarAsync(Arquivo arquivo);
    }
}