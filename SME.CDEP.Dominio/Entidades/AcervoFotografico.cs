namespace SME.CDEP.Dominio.Entidades
{
    public class AcervoFotografico : EntidadeBaseAuditavel
    {
        public string Titulo { get; set; }
        public long CreditoId { get; set; }
        public long Tombo { get; set; }
        public string Localizacao { get; set; }
        public string Procedencia { get; set; }
        public DateTime DataAcervo { get; set; }
        public bool CopiaDigital { get; set; }
        public bool PermiteUsoImagem { get; set; }
        public long ConservacaoId { get; set; }
        public string Descricao { get; set; }
        public long Quantidade { get; set; }
        public long Largura { get; set; }
        public long Altura { get; set; }
        public long SuporteId { get; set; }
        public long Formato_id { get; set; }
        public long Cromia_id { get; set; }
        public string Resolucao { get; set; }
        public string TamanhoArquivo { get; set; }
    }
}
