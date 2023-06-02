namespace SME.CDEP.Dominio.Dominios
{
    public class Usuario : EntidadeBase
    {
        public string CodigoRf { get; set; }

        public DateTime? ExpiracaoRecuperacaoSenha { get; set; }

        public string Login { get; set; }

        public string Nome { get; set; }

        public Guid PerfilAtual { get; set; }

        public Guid? TokenRecuperacaoSenha { get; set; }

        public DateTime UltimoLogin { get; set; }

        private string Email { get; set; }
    }
}
