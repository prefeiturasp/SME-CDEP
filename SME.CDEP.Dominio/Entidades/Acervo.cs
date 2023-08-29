﻿namespace SME.CDEP.Dominio.Entidades
{
    public class Acervo : EntidadeBaseAuditavel
    {
        public string Titulo { get; set; }
        public long TipoAcervoId { get; set; }
        public string Codigo { get; set; }
        public long CreditoAutorId { get; set; }
        public CreditoAutor CreditoAutor { get; set; }
    }
}