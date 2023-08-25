namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoFotografico : Acervo
    {
        public string Localizacao { get; set; }
        public string Procedencia { get; set; }
        public string DataAcervo { get; set; }
        public bool CopiaDigital { get; set; }
        public bool PermiteUsoImagem { get; set; }
        public long ConservacaoId { get; set; }
        public string Descricao { get; set; }
        public long Quantidade { get; set; }
        public float Largura { get; set; }
        public float Altura { get; set; }
        public long SuporteId { get; set; }
        public long Formato_id { get; set; }
        public long Cromia_id { get; set; }
        public string Resolucao { get; set; }
        public string TamanhoArquivo { get; set; }
    }
}
