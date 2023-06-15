using SME.CDEP.Infra.Dominio.Enumerados;

namespace SME.CDEP.Dominio.Dominios
{
    public class Usuario : EntidadeBase
    {
        public DateTime? ExpiracaoRecuperacaoSenha { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public Guid? TokenRecuperacaoSenha { get; set; }
        public DateTime UltimoLogin { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public TipoPerfil Perfil { get; set; }
    }
}
