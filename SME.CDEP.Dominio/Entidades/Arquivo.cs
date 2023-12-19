using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Entidades
{
    public class Arquivo : EntidadeBaseAuditavel
    {
        public string Nome { get; set; }
        public Guid Codigo { get; set; }
        public string TipoConteudo { get; set; }
        public TipoArquivo Tipo { get; set; }
        public string NomeArquivoFisico
        {
            get
            {
                var extensao = Path.GetExtension(Nome);
                return $"{Codigo.ToString()}{extensao}";
            }
        }
    }
}
