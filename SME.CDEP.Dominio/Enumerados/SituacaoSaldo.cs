using System.ComponentModel.DataAnnotations;
using SME.CDEP.Dominio.Extensions;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum SituacaoSaldo
{
    [Display(Description = "Disponível")]
    DISPONIVEL = 1,
    
    [Display(Description = "Indisponível para reserva/emprestimo")]
    INDISPONIVEL_PARA_RESERVA_EMPRESTIMO = 2,
    
    [Display(Description = "Reservado")]
    RESERVADO = 3,
        
    [Display(Description = "Emprestado")]
    EMPRESTADO = 4
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
    
    public static bool EstaIndisponivelParaReservaEmprestimo(this SituacaoSaldo situacaoSaldo)
    {
        return situacaoSaldo == SituacaoSaldo.INDISPONIVEL_PARA_RESERVA_EMPRESTIMO;
    }
    
    public static bool EstaIndisponivel(this SituacaoSaldo situacaoSaldo)
    {
        return situacaoSaldo.EstaReservado() || situacaoSaldo.EstaEmprestado() || situacaoSaldo.EstaIndisponivelParaReservaEmprestimo();
    }
}