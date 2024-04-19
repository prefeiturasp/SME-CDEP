namespace SME.CDEP.Infra.Servicos.ServicoArmazenamento
{
    public class ConfiguracaoArmazenamentoOptions
    {
        public const string Secao = "ConfiguracaoArmazenamento";
        public string BucketTemp { get; set; }
        public string BucketArquivos { get; set; }
        public string EndPoint { get; set; }
        public int Port { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string TipoRequisicao { get; set; }

        public string EnderecoCompletoBucketArquivos()
        {
            return $"{EnderecoCompletoPadrao()}/{BucketArquivos}/";
        }
        
        public string EnderecoCompletoBucketTemp()
        {
            return $"{EnderecoCompletoPadrao()}/{BucketTemp}/";
        }
        
        public string EnderecoCompletoPadrao()
        {
            return $"{TipoRequisicao}://{EndPoint}";
        }
    }
}