﻿using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class Formato : EntidadeBase
    {
        public string Nome { get; set; }
        public bool Excluido { get; set; }
    }
}
