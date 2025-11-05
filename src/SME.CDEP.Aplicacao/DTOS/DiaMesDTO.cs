using System.ComponentModel.DataAnnotations;
using SME.CDEP.Dominio.Constantes;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Dominio.Extensions;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS
{
    public class DiaMesDTO
    {
        [Required(ErrorMessage = "É necessário informar o dia do evento")]
        public int Dia { get; set; }
        
        [Required(ErrorMessage = "É necessário informar o mês do evento")]
        public int Mes { get; set; }

        public int? Ano { get; set; } = DateTimeExtension.HorarioBrasilia().Year;
        public int Minuto { get; set; }
        public int Hora { get; set; }
        
        public DateTime Data
        {
            get
            {
                if (!Dia.EhDiaValido())
                    throw new NegocioException(MensagemNegocio.DIA_INVALIDO);
            
                if (!Mes.EhMesValido())
                    throw new NegocioException(MensagemNegocio.MES_INVALIDO);

                return new DateTime(Ano.Value, Mes, Dia, Hora, Minuto,0);
            }
        }
    }
}
