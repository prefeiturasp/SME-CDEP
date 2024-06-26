﻿using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioIdioma : IRepositorioBase<Idioma>
    {
        Task<long> ObterPorNome(string nome);
    }
}