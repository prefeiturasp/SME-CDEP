﻿using System.Collections;
using SME.CDEP.Dominio.Entidades;
using SME.CDEP.Dominio.Repositorios;

namespace SME.CDEP.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioAcervoEmprestimo : IRepositorioBaseAuditavel<AcervoEmprestimo>
    {
        Task<IEnumerable<AcervoEmprestimo>> ObterUltimoEmprestimoPorAcervoSolicitacaoItemIds(long[] acervoSolicitacaoItemIds);
        Task<AcervoEmprestimo> ObterUltimoEmprestimoPorAcervoSolicitacaoItemId(long acervoSolicitacaoItemId);
        Task<IEnumerable<AcervoEmprestimo>> ObterItensEmprestadosAtrasados();
        Task<IEnumerable<AcervoEmprestimoDevolucao>> ObterDetalhamentoDosItensANotificarSobreVencimentoEmprestimoPorDataDevolucao(DateTime dataDevolucaoNotificada);
    }
}