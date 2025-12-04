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
        
        public const string NotificacaoDevolucaoEmprestimoAtrasoProlongado = "cdep.emprestimo.situacao.atraso.prolongado.aviso";
        public const string NotificacaoDevolucaoEmprestimoAtrasoProlongadoUsuario = "cdep.emprestimo.situacao.atraso.prolongado.aviso.usuario";
        
        public const string ExecutarImportacaoArquivoAcervoBibliografico = "cdep.importacao.arquivo.acervo.bibliografico";
        public const string ExecutarImportacaoArquivoAcervoDocumental = "cdep.importacao.arquivo.acervo.documental";
        public const string ExecutarImportacaoArquivoAcervoArteGrafica = "cdep.importacao.arquivo.acervo.arte.grafica";
        public const string ExecutarImportacaoArquivoAcervoAudiovisual = "cdep.importacao.arquivo.acervo.audiovisual";
        public const string ExecutarImportacaoArquivoAcervoFotografico = "cdep.importacao.arquivo.acervo.fotografico";
        public const string ExecutarImportacaoArquivoAcervoTridimensional  = "cdep.importacao.arquivo.acervo.tridimensional";

        public const string ExecutarConsolidacaoDoHistoricoDeConsultasDeAcervo = "cdep.consolidacao.historico.consultas.acervo";
        public const string ExecutarConsolidacaoDasSolicitacoesDeAcervo = "cdep.consolidacao.solicitacoes.acervo";
    }
}