﻿using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum TipoAtendimento
{
    [Display(Description = "E-mail")]
    Email = 1,
    
    [Display(Description = "Presencial")]
    Presencial = 2
    
}

public static class TipoAtendimentoExtensions 
{
   
    public static bool EhAtendimentoPresencial(this TipoAtendimento? tipoAtendimento)
    {
        return tipoAtendimento == TipoAtendimento.Presencial;
    }
            
    public static bool EhAtendimentoViaEmail(this TipoAtendimento? tipoAtendimento)
    {
        return tipoAtendimento == TipoAtendimento.Email;
    }
    
    public static bool EhAtendimentoPresencial(this TipoAtendimento tipoAtendimento)
    {
        return tipoAtendimento == TipoAtendimento.Presencial;
    }
    
    public static bool EhAtendimentoViaEmail(this TipoAtendimento tipoAtendimento)
    {
        return tipoAtendimento == TipoAtendimento.Email;
    }
    
    public static bool EhInvalido(this TipoAtendimento tipoAtendimento)
    {
        var tiposValidos = new[] { TipoAtendimento.Presencial, TipoAtendimento.Email };
        
        return !tiposValidos.Contains(tipoAtendimento);
    }
    
    public static bool EhInvalido(this TipoAtendimento? tipoAtendimento)
    {
        if (!tipoAtendimento.HasValue)
            return false;
        
        var tiposValidos = new[] { TipoAtendimento.Presencial, TipoAtendimento.Email };
        
        return !tiposValidos.Contains(tipoAtendimento.Value);
    }
}