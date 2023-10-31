using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoCodigoNomeResumido 
    {
        public long AcervoId { get; set; }
        public Guid Codigo { get; set; }
        public string Nome { get; set; }
        public string TipoConteudo { get; set; }

        public string NomeArquivo
        {
            get
            {
                return $"{Codigo}.{Nome.Split('.').LastOrDefault()}";
            }
        }
    }
}
