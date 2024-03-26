using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum SituacaoSaldo
{
    [Display(Description = "Disponível")]
    DISPONIVEL = 1,
    
    [Display(Description = "Reservado")]
    RESERVADO = 2,
        
    [Display(Description = "Emprestado")]
    EMPRESTADO = 3
}

public static class SituacaoSaldoExtension
{
    public static bool EstaDisponivel(this SituacaoSaldo situacaoSaldo)
    {
       return situacaoSaldo == SituacaoSaldo.DISPONIVEL;
    }
    
    public static bool EstaReservado(this SituacaoSaldo situacaoSaldo)
    {
        return situacaoSaldo == SituacaoSaldo.RESERVADO;
    }
    
    public static bool EstaEmprestado(this SituacaoSaldo situacaoSaldo)
    {
        return situacaoSaldo == SituacaoSaldo.EMPRESTADO;
    }
    
    public static bool EstaIndisponivel(this SituacaoSaldo situacaoSaldo)
    {
        return situacaoSaldo.EstaReservado() || situacaoSaldo.EstaEmprestado();
    }
}