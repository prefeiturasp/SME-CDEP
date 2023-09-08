﻿using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervo : IRepositorioBaseAuditavel<Acervo>
    {
        Task<IEnumerable<Acervo>> PesquisarPorFiltro(int? tipoAcervo, string titulo, long? creditoAutorId, string codigo);
        Task<bool> ExisteCodigo(string codigo, long id);
        Task<bool> ExisteTitulo(string titulo, long id);
    }
}