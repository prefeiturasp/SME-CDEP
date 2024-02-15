﻿using System.ComponentModel.DataAnnotations;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class AcervoSolicitacaoItemManualCadastroDTO
    {
        [Required(ErrorMessage = "É necessário informar o identificador do acervo para realizar a solicitação manual de acervos")]
        public long AcervoId { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o tipo de atendimentopara realizar a solicitação manual de acervos")]
        public TipoAtendimento TipoAtendimento { get; set; }
        
        [Required(ErrorMessage = "É necessário informar a situação do item do acervo para realizar a solicitação manual de acervos")]
        public SituacaoSolicitacaoItem Situacao { get; set; }
        
        public DateTime? DataVisita { get; set; }
    }
}