﻿using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoSolicitacaoItem : EntidadeBaseAuditavel
    {
        public long AcervoSolicitacaoId { get; set; }
        public long AcervoId { get; set; }
        public SituacaoSolicitacaoItem Situacao { get; set; }
    }
}