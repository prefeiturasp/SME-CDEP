﻿using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioConservacao : IRepositorioBase<Conservacao>
    {
        Task<long> ObterPorNome(string nome);
    }
}