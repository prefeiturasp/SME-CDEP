namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoAudiovisual : EntidadeBase
    {
        public Acervo Acervo { get; set; }
        public long AcervoId { get; set; }
        public string Localizacao { get; set; }
        public string Procedencia { get; set; }
        public string DataAcervo { get; set; }
        public string Copia { get; set; }
        public bool PermiteUsoImagem { get; set; }
        public long? ConservacaoId { get; set; }
        public string Descricao { get; set; }
        public long SuporteId { get; set; }
        public string Duracao { get; set; }
        public long? CromiaId { get; set; }
        public string TamanhoArquivo { get; set; }
        public string Acessibilidade { get; set; }
        public string Disponibilizacao { get; set; }
    }
}
