﻿using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class AcervoSolicitacaoItemDetalheResumidoDTO
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public string TipoAcervo { get; set; }
        public string Titulo { get; set; }
        public string Situacao { get; set; }
        public SituacaoSolicitacaoItem SituacaoId { get; set; }
        public DateTime? DataVisita { get; set; }
        public string DataVisitaFormatada { get; set; }
        public TipoAtendimento? TipoAtendimento { get; set; }
        public long AcervoId { get; set; }
        public string Responsavel { get; set; }
        public TipoAcervo TipoAcervoId { get; set; }
        public DateTime? DataEmprestimo { get; set; }
        public string DataEmprestimoFormatada { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public string DataDevolucaoFormatada { get; set; }
        public SituacaoEmprestimo? SituacaoEmprestimo { get; set; }
        public string SituacaoDisponibilidade { get; set; }
        public bool EstaDisponivel { get; set; }
        public bool TemControleDisponibilidade { get; set; }
        public bool PodeFinalizarItem { get; set; }
        public SituacaoSaldo SituacaoSaldo { get; set; }
        public bool PodeEditar { get; set; }
    }
}
