﻿using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Aplicacao.DTOS;

public class FiltroTextoLivreTipoAcervoDTO
{
    public TipoAcervo? TipoAcervo { get; set; }
    public string? TextoLivre { get; set; }
}
