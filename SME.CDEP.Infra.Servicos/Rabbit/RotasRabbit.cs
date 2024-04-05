namespace SME.CDEP.Infra
{
    public static class RotasRabbit
    {
        public const string ExecutarCriacaoDeEventosTipoFeriadoAnoAtual = "cdep.eventos.feriados.ano.atual";
        public const string ExecutarCriacaoDeEventosTipoFeriadoAnoAtualPorData = "cdep.eventos.feriados.ano.atual.por.data";
        
        public const string NotificarViaEmailCancelamentoAtendimento = "cdep.enviar.email.cancelamento.atendimento";
        public const string NotificarViaEmailCancelamentoAtendimentoItem = "cdep.enviar.email.cancelamento.atendimento.item";
        public const string NotificarViaEmailConfirmacaoAtendimentoPresencial = "cdep.enviar.email.confirmacao.atendimento.presencial";
        
        public const string ExecutarAtualizacaoSituacaoParaEmprestimoComDevolucaoEmAtraso = "cdep.emprestimo.situacao.devolucao.atraso";
        
        public const string NotificacaoVencimentoEmprestimo = "cdep.emprestimo.situacao.vencimento.aviso";
        public const string NotificacaoVencimentoEmprestimoUsuario = "cdep.emprestimo.situacao.vencimento.aviso.usuario";
        
        public const string NotificacaoDevolucaoEmprestimoAtrasado = "cdep.emprestimo.situacao.atrasado.aviso";
        public const string NotificacaoDevolucaoEmprestimoAtrasadoUsuario = "cdep.emprestimo.situacao.atrasado.aviso.usuario";
    }
}