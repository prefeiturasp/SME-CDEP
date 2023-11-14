using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum ImportacaoStatus
{
    [Display(Name = "Pendente de importação")]
    Pendente = 1,
    
    [Display(Name = "Validado preenchimento, valor, formato e qtde caracteres")]
    ValidadoPreenchimentoValorFormatoQtdeCaracteres = 2,
    
    [Display(Name = "Validacao de domínios")]
    ValidacaoDominios = 3,
    
    [Display(Name = "Importado com Erros")]
    Erros = 4,
    
    [Display(Name = "Importado com Sucesso")]
    Sucesso = 5,
    
}