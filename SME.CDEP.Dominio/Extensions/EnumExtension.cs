
using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Extensions
{
    public static class EnumExtension
    {
        public static bool EhAcervoDocumental(this TipoAcervo tipoAcervo)
        {
            return tipoAcervo == TipoAcervo.DocumentacaoHistorica; 
        }
        
        public static bool NaoEhAcervoDocumental(this TipoAcervo tipoAcervo)
        {
            return !EhAcervoDocumental(tipoAcervo); 
        }
        
        public static bool EhAcervoBibliografico(this TipoAcervo tipoAcervo)
        {
            return tipoAcervo == TipoAcervo.Bibliografico; 
        }
        
        public static bool NaoEhAcervoBibliografico(this TipoAcervo tipoAcervo)
        {
            return !EhAcervoBibliografico(tipoAcervo); 
        } 
        
        public static string ObterPlanilhaModelo(this TipoAcervo tipoAcervo)
        {
            switch (tipoAcervo)
            {
                case TipoAcervo.Bibliografico:
                    return Constantes.Constantes.PLANILHA_ACERVO_BIBLIOGRAFICO;
                
                case TipoAcervo.DocumentacaoHistorica:
                    return Constantes.Constantes.PLANILHA_ACERVO_DOCUMENTAL;
                
                case TipoAcervo.ArtesGraficas:
                    return Constantes.Constantes.PLANILHA_ACERVO_ARTE_GRAFICA;
                
                case TipoAcervo.Audiovisual:
                    return Constantes.Constantes.PLANILHA_ACERVO_AUDIOVISUAL;
                
                case TipoAcervo.Fotografico:
                    return Constantes.Constantes.PLANILHA_ACERVO_FOTOGRAFICO;
                
                case TipoAcervo.Tridimensional:
                    return Constantes.Constantes.PLANILHA_ACERVO_TRIDIMENSIONAL;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(tipoAcervo), tipoAcervo, null);
            }
        }
    }
}
