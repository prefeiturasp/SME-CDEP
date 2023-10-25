using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class PesquisaAcervo
    {
        public string Codigo { get; set; }
        public TipoAcervo Tipo { get; set; }
        public string Titulo { get; set; }
        public string CreditoAutoria { get; set; }
        public string Assunto { get; set; }
        public string Descricao { get; set; }

        public TipoAcervoTag TipoAcervoTag
        {
            get
            {
                switch (Tipo)
                {
                    case TipoAcervo.Bibliografico:
                        return TipoAcervoTag.Biblioteca;
                    
                    case TipoAcervo.DocumentacaoHistorica:
                        return TipoAcervoTag.MemoriaDocumental;
                    
                    default:
                        return TipoAcervoTag.MemoriaEducacaoMunicipal;
                }
            }           
        }
    }
}
