﻿using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class Idioma : EntidadeBase
    {
        public string Nome { get; set; }
        public bool Excluido { get; set; }
    }
}
