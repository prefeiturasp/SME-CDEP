﻿using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Infra.Dominio.Enumerados;

public enum TipoAcervo
{
    [Display(Description = "Bibliográfico")]
    Bibliografico = 1,
    
    [Display(Description = "Documentação histórica")]
    DocumentacaoHistorica = 2,
    
    [Display(Description = "Artes gráficas")]
    ArtesGraficas = 3,
    
    [Display(Description = "Audiovisual")]
    Audiovisual = 4,
    
    [Display(Description = "Fotográfico")]
    Fotografico = 5,
    
    [Display(Description = "Tridimensional")]
    Tridimensional = 6
}