using System.ComponentModel.DataAnnotations;
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoSolicitacaoItemConsultaDTO 
{
    [Required(ErrorMessage = "É necessário informar o código/tombo do acervo a ser solicitado")]
    public string Codigo { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o tipo de acervo a ser solicitado")]
    public TipoAcervo Tipo { get; set; }
}