﻿namespace SME.CDEP.Dominio.Entidades
{
    public enum TipoParametroSistema
    {
        TermoCompromissoPesquisador = 1,
        EnderecoContatoCDEPConfirmacaoCancelamentoVisita = 2,
        EmailRemetente = 3,
        NomeRemetenteEmail = 4,
        EnderecoSMTP = 5,
        UsuarioRemetenteEmail = 6,
        SenhaRemetenteEmail = 7,
        UsarTLSEmail = 8,
        PortaEnvioEmail = 9,
        ModeloEmailCancelamentoSolicitacao = 10,
        ModeloEmailCancelamentoSolicitacaoItem = 11,
        ModeloEmailConfirmacaoSolicitacao = 12,
        EnderecoSedeCDEPVisita = 13,
        HorarioFuncionamentoSedeCDEPVisita = 14,
        LimiteDiasEmprestimoAcervo = 15,
        QtdeDiasAntesDoVencimentoEmprestimo = 16,
        ModeloEmailAvisoDevolucaoEmprestimo = 17,
        QtdeDiasAtrasoDevolucaoEmprestimo = 18,
        ModeloEmailAvisoAtrasoDevolucaoEmprestimo = 19,
        QtdeDiasAtrasoProlongadoDevolucaoEmprestimo = 20,
        ModeloEmailAvisoAtrasoProlongadoDevolucaoEmprestimo = 21,
        LimiteAcervosImportadosViaPanilha = 22,
    }
}