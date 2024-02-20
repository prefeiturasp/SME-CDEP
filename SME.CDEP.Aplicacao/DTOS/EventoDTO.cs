﻿using System.ComponentModel.DataAnnotations;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class EventoDTO
    {
        public long Id { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o dia do evento")]
        public int Dia { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o mês do evento")]
        public int Mes { get; set; }
        
        public TipoEvento Tipo { get; set; }
        
        [Required(ErrorMessage = "É necessário informar a descrição do evento")]
        public string Descricao { get; set; }
        public string Justificativa { get; set; }
        public long? AcervoSolicitacaoItemId { get; set; }

        public DateTime Data
        {
            get
            {
                if (Dia.EhDiaValido())
                    throw new NegocioException(MensagemNegocio.DIA_INVALIDO);
                
                if (Mes.EhMesValido())
                    throw new NegocioException(MensagemNegocio.MES_INVALIDO);

                return new DateTime(DateTimeExtension.HorarioBrasilia().Year, Mes, Dia);
            }
        }
    }
}
