using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum SituacaoEmprestimo
{
    [Display(Description = "Emprestado")]
    EMPRESTADO = 1,
    
    [Display(Description = "Devolução em atraso")]
    DEVOLUCAO_EM_ATRASO = 2,
        
    [Display(Description = "Emprestado - Prorrogação")]
    EMPRESTADO_PRORROGACAO = 3,
    
    [Display(Description = "Devolvido")]
    DEVOLVIDO = 4
}

public static class SituacaoEmprestimoExtension
{
    public static bool EstaEmprestado(this SituacaoEmprestimo SituacaoEmprestimo)
    {
       return SituacaoEmprestimo == SituacaoEmprestimo.EMPRESTADO;
    }
    
    public static bool DevolucaoAtrasada(this SituacaoEmprestimo SituacaoEmprestimo)
    {
        return SituacaoEmprestimo == SituacaoEmprestimo.DEVOLUCAO_EM_ATRASO;
    }
    
    public static bool EmprestadoComProrrogacao(this SituacaoEmprestimo SituacaoEmprestimo)
    {
        return SituacaoEmprestimo == SituacaoEmprestimo.EMPRESTADO_PRORROGACAO;
    }
    
    public static bool EstaDevolvido(this SituacaoEmprestimo SituacaoEmprestimo)
    {
        return SituacaoEmprestimo == SituacaoEmprestimo.DEVOLVIDO;
    }
}