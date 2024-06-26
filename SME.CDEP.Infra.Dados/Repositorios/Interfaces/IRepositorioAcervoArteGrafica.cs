﻿using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoArteGrafica : IRepositorioBase<AcervoArteGrafica>
    {
        Task<AcervoArteGraficaCompleto> ObterPorId(long id);
        Task<AcervoArteGraficaDetalhe> ObterDetalhamentoPorCodigo(string filtroCodigo);
    }
}