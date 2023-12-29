using SME.CDEP.Dominio.Extensions;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoCodigoNomeResumido 
    {
        public long AcervoId { get; set; }
        public string Nome { get; set; }
        public Guid Codigo { get; set; }
        
        public string Thumbnail
        {
            get
            {
                return $"{Codigo}{Nome.ObterExtensao()}";
            }
        }
    }
}
