﻿namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoTridimensionalArquivo : EntidadeBaseSomenteId
    {
        public long AcervoTridimensionalId { get; set; }
        public long ArquivoId { get; set; }
        public long? ArquivoMiniaturaId { get; set; }
    }
}